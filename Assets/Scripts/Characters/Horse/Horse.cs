using Assets.Scripts.Characters.Humans;
using UnityEngine;
using Random = UnityEngine.Random;

/// <summary>
/// The horse is a vehicle that can be controller by a <see cref="Human"/> once mounted. Current implementation is broken as its an incomplete version of #116
/// </summary>
public sealed class Horse : PhotonView
{
    [SerializeField] private float gravityFactor = -200f;
    [SerializeField] private float walkSpeedModifier = 27f;
    [SerializeField] private float runSpeedModifier = 45f;
    
    [SerializeField] private float slowRadius = 20f;
    [SerializeField] private float stopRadius = 5f;
    [SerializeField] private float slowModifier = 0.7f;
    [SerializeField] private float coneAngle = 20f;
    [SerializeField] private float predictionMultiplier = 2f;
    
    [SerializeField] private float teleportCountdown = 6f;

    [SerializeField]
    private ParticleSystem dustParticles;

    private new Animation animation;

    private State currentState;

    private FollowState followState;
    private MountState mountState;
    private IdleState idleState;

    private LayerMask groundMask;
    private Hero hero;
    private Transform heroTransform;
    private LayerMask isGroundedMask;
    private new Rigidbody rigidbody;

    public static Horse Create(Hero hero, Vector3 position, Quaternion rotation)
    {
        var horse = PhotonNetwork.Instantiate("horse", position, rotation, 0).GetComponent<Horse>();

        horse.RPC(nameof(InitializeRPC), PhotonTargets.AllBuffered, hero.photonView.viewID);

        return horse;
    }

    [PunRPC]
    private void InitializeRPC(int heroViewID)
    {
        animation = GetComponent<Animation>();
        rigidbody = GetComponent<Rigidbody>();

        LayerMask enemyBoxMask = 1 << LayerMask.NameToLayer("EnemyBox");
        groundMask = 1 << LayerMask.NameToLayer("Ground");
        isGroundedMask = enemyBoxMask | groundMask;

        hero = Find(heroViewID).GetComponent<Hero>();

        followState = new FollowState(this);
        idleState = new IdleState(this);
        mountState = new MountState(this, GetComponent<HorseController>());

        heroTransform = hero.transform;
        mountState.HeroAnimation = hero.GetComponent<Animation>();
        mountState.HeroRigidbody = hero.GetComponent<Rigidbody>();

        TransitionToState(idleState);
    }

    public void Mount()
    {
        TransitionToState(mountState);
    }

    public void Unmount()
    {
        TransitionToState(idleState);
    }

    private void CrossFade(string aniName, float time)
    {
        animation.CrossFade(aniName, time);
        if (PhotonNetwork.connected && photonView.isMine)
            photonView.RPC(nameof(netCrossFade), PhotonTargets.Others, aniName, time);
    }

    private void DisableDust()
    {
        if (dustParticles.enableEmission)
        {
            dustParticles.enableEmission = false;
            photonView.RPC(nameof(setDust), PhotonTargets.Others, false);
        }
    }

    private void EnableDust()
    {
        if (!dustParticles.enableEmission)
        {
            dustParticles.enableEmission = true;
            photonView.RPC(nameof(setDust), PhotonTargets.Others, true);
        }
    }

    private void Update()
    {
        if (!hero && photonView.isMine)
        {
            PhotonNetwork.Destroy(gameObject);
            return;
        }

        currentState.Update();
    }

    private void FixedUpdate()
    {
        currentState.FixedUpdate();

        rigidbody.AddForce(new Vector3(0f, gravityFactor, 0f));
    }

    [PunRPC]
    private void netCrossFade(string aniName, float time)
    {
        animation.CrossFade(aniName, time);
    }

    [PunRPC]
    private void netPlayAnimation(string aniName)
    {
        animation.Play(aniName);
    }

    [PunRPC]
    private void netPlayAnimationAt(string aniName, float normalizedTime)
    {
        animation.Play(aniName);
        animation[aniName].normalizedTime = normalizedTime;
    }

    private void Reset()
    {
        dustParticles = GetComponentInChildren<ParticleSystem>();
    }

    [PunRPC]
    private void setDust(bool enable)
    {
        if (dustParticles.enableEmission)
            dustParticles.enableEmission = enable;
    }

    private void ToIdleAnimation()
    {
        if (rigidbody.velocity.magnitude > 0.1f)
        {
            if (rigidbody.velocity.magnitude > 15f)
            {
                if (!animation.IsPlaying("horse_Run"))
                    CrossFade("horse_Run", 0.1f);

                EnableDust();
            }
            else
            {
                if (!animation.IsPlaying("horse_WALK"))
                    CrossFade("horse_WALK", 0.1f);

                DisableDust();
            }
        }
        else
        {
            if (animation.IsPlaying("horse_idle1") && (animation["horse_idle1"].normalizedTime >= 1f))
                CrossFade("horse_idle0", 0.1f);

            if (animation.IsPlaying("horse_idle2") && (animation["horse_idle2"].normalizedTime >= 1f))
                CrossFade("horse_idle0", 0.1f);

            if (animation.IsPlaying("horse_idle3") && (animation["horse_idle3"].normalizedTime >= 1f))
                CrossFade("horse_idle0", 0.1f);

            if ((!animation.IsPlaying("horse_idle0") && !animation.IsPlaying("horse_idle1")) && (!animation.IsPlaying("horse_idle2") && !animation.IsPlaying("horse_idle3")))
                CrossFade("horse_idle0", 0.1f);

            if (animation.IsPlaying("horse_idle0"))
            {
                switch (Random.Range(0, 1000))
                {
                    case 0:
                        CrossFade("horse_idle1", 0.1f);
                        break;

                    case 1:
                        CrossFade("horse_idle2", 0.1f);
                        break;

                    case 2:
                        CrossFade("horse_idle3", 0.1f);
                        break;

                    default:
                        break;
                }
            }

            DisableDust();

            rigidbody.AddForce(-rigidbody.velocity, ForceMode.VelocityChange);
        }
    }

    private void TransitionToState(State nextState)
    {
        currentState?.Exit();
        currentState = nextState;
        currentState?.Enter();
    }

    private sealed class FollowState : State
    {
        private Vector3 horsePosition;
        private Quaternion horseRotation;
        private Vector3 horseVelocity;
     
        private float awayTimer;
        private Vector3 target;
        private float timeElapsed;

        public FollowState(Horse horse)
            : base(horse)
        {
        }

        public override void Enter()
        {
            GetNewTarget();
        }

        private void GetNewTarget()
        {
            var randomOffset = new Vector3(
                Random.Range(-6, 6),
                5f,
                Random.Range(-6, 6));
            var point = Horse.heroTransform.position + randomOffset;
            point.y = GetHeight(point);
            target = point;
            awayTimer = 0f;
        }

        public override void FixedUpdate()
        {
            if (!Horse.hero)
            {
                Horse.TransitionToState(Horse.idleState);
                return;
            }
            
            horsePosition = Horse.rigidbody.position;
            horseRotation = Horse.rigidbody.rotation;
            horseVelocity = Horse.rigidbody.velocity;

            var heroRigidbody = Horse.hero.Rigidbody;
            var heroPosition = heroRigidbody.position;

            if (horseVelocity.magnitude > 8f) PlayRunAnimation();
            else PlayWalkAnimation();

            var targetDelta = target - horsePosition;
            var horizontalAngle = 90f - Mathf.Atan2(targetDelta.z, targetDelta.x) * Mathf.Rad2Deg;
            var currentY = horseRotation.eulerAngles.y;
            var deltaY = -Mathf.DeltaAngle(horizontalAngle, currentY);
            var headingY = Mathf.Lerp(currentY, currentY + deltaY, 4f / (horseVelocity.magnitude + 20f));
            horseRotation = Quaternion.Euler(0f, headingY, 0f);

            var shouldSlow = targetDelta.magnitude < Horse.slowRadius;
            var speedModifier = shouldSlow ? Horse.runSpeedModifier * Horse.slowModifier : Horse.runSpeedModifier;
            var velocityY = headingY + Mathf.Clamp(deltaY, -Horse.coneAngle, Horse.coneAngle) * Horse.predictionMultiplier;
            var horseHeading = Quaternion.Euler(0f, velocityY, 0f) * Vector3.forward;

            horseVelocity += horseHeading * speedModifier * Time.deltaTime;
            if (horseVelocity.magnitude >= speedModifier)
                horseVelocity -= speedModifier * horseVelocity.normalized * Time.deltaTime;
            
            Debug.DrawRay(horsePosition, Quaternion.Euler(0f, currentY - Horse.coneAngle, 0f) * Vector3.forward, Color.red, 0.02f);
            Debug.DrawRay(horsePosition, horseVelocity, Color.green, 0.02f);
            Debug.DrawRay(horsePosition, horseHeading * speedModifier * Time.deltaTime, Color.blue, 0.02f);
            Debug.DrawRay(horsePosition, Quaternion.Euler(0f, currentY + Horse.coneAngle, 0f) * Vector3.forward, Color.red, 0.02f);

            timeElapsed += Time.deltaTime;
            if (timeElapsed > 0.6f)
            {
                timeElapsed = 0f;
                if (Vector3.Distance(heroPosition, target) > Horse.slowRadius)
                    GetNewTarget();
            }

            if (Vector3.Distance(heroPosition, horsePosition) < Horse.stopRadius)
                Horse.TransitionToState(Horse.idleState);
            else if (Vector3.Distance(target, horsePosition) < Horse.stopRadius)
                Horse.TransitionToState(Horse.idleState);

            awayTimer += Time.deltaTime;
            if (awayTimer > Horse.teleportCountdown)
            {
                awayTimer = 0f;
                var start = horsePosition + Vector3.up;
                var end = heroPosition + Vector3.up;
                if (Physics.Linecast(start, end, Horse.groundMask))
                    Horse.rigidbody.position = new Vector3(
                        heroPosition.x,
                        GetHeight(heroPosition + Vector3.up * 5f),
                        heroPosition.z);
            }
            else
            {
                Horse.rigidbody.MoveRotation(horseRotation);
                Horse.rigidbody.velocity = horseVelocity;
            }
        }
        private void PlayWalkAnimation()
        {

            if (!Horse.animation.IsPlaying("horse_WALK"))
                Horse.CrossFade("horse_WALK", 0.1f);

            Horse.DisableDust();
        }
        private void PlayRunAnimation()
        {

            if (!Horse.animation.IsPlaying("horse_Run"))
                Horse.CrossFade("horse_Run", 0.1f);

            Horse.EnableDust();
        }

        private float GetHeight(Vector3 pt)
        {
            RaycastHit hit;
            if (Physics.Raycast(pt, -Vector3.up, out hit, 1000f, Horse.groundMask.value))
                return hit.point.y;

            return 0f;
        }
    }

    private sealed class IdleState : State
    {
        public IdleState(Horse horse)
            : base(horse)
        {
        }

        public override void Update()
        {
            Horse.ToIdleAnimation();
            var heroDistance = Vector3.Distance(Horse.heroTransform.position, Horse.transform.position);
            if (Horse.hero && heroDistance > Horse.slowRadius)
                Horse.TransitionToState(Horse.followState);
        }
    }

    private sealed class MountState : State
    {
        private readonly HorseController controller;

        private Vector3 horsePosition;
        private Quaternion horseRotation;
        private Vector3 horseVelocity;
        
        public MountState(Horse horse, HorseController controller)
            : base(horse) =>
            this.controller = controller;

        public Animation HeroAnimation { get; set; }

        public Rigidbody HeroRigidbody { get; set; }

        // ReSharper disable once CompareOfFloatsByEqualityOperator
        // -874f is used by the HorseController to indicate no directional input
        private bool HasMovementInput => controller.TargetDirection != -874f;

        private bool IsGrounded =>
            Physics.Raycast(
                Horse.rigidbody.position + Vector3.up * 0.1f,
                Vector3.down,
                0.3f,
                Horse.isGroundedMask.value);

        public override void Enter() => controller.enabled = true;

        public override void FixedUpdate()
        {
            if (!Horse.hero)
            {
                Horse.TransitionToState(Horse.idleState);
                return;
            }

            horsePosition = Horse.rigidbody.position;
            horseRotation = Horse.rigidbody.rotation;
            horseVelocity = Horse.rigidbody.velocity;

            if (HasMovementInput) Accelerate();
            else Decelerate();

            if (controller.ShouldJump && IsGrounded)
                horseVelocity += Vector3.up * 25f;

            Horse.rigidbody.MoveRotation(horseRotation);
            Horse.rigidbody.velocity = horseVelocity;

            var playerOffset = Vector3.up * 1.68f;
            HeroRigidbody.MovePosition(horsePosition + playerOffset);
            HeroRigidbody.MoveRotation(horseRotation);
            HeroRigidbody.velocity = horseVelocity;
        }
                
        public override void Exit() => controller.enabled = false;
        
        private void Accelerate()
        {
            var targetRotation = Quaternion.Euler(0f, controller.TargetDirection, 0f);
            var lerpStep = 2f / (horseVelocity.magnitude + 20f);
            horseRotation = Quaternion.Lerp(horseRotation, targetRotation, lerpStep);
            var horseHeading = horseRotation * Vector3.forward;

            var speedModifier = controller.ShouldWalk ? Horse.walkSpeedModifier : Horse.runSpeedModifier;
            horseVelocity += horseHeading * speedModifier * Time.deltaTime;
            if (horseVelocity.magnitude >= speedModifier)
                horseVelocity += -speedModifier * horseVelocity.normalized * Time.deltaTime;

            if (horseVelocity.magnitude > 8f)
            {
                PlayRunAnimation();
                PlayHeroRun();
                Horse.EnableDust();
            }
            else
            {
                PlayWalkAnimation();
                PlayHeroIdle();
                Horse.DisableDust();
            }
        }

        private void Decelerate()
        {
            Horse.ToIdleAnimation();
            if (horseVelocity.magnitude > 15f) PlayHeroRun();
            else PlayHeroIdle();
        }
        private void PlayHeroRun()
        {
            if (!HeroAnimation.IsPlaying("horse_run"))
                Horse.hero.CrossFade("horse_run", 0.1f);
        }

        private void PlayWalkAnimation()
        {
            if (!Horse.animation.IsPlaying("horse_WALK"))
                Horse.CrossFade("horse_WALK", 0.1f);
        }
        private void PlayHeroIdle()
        {

            if (!HeroAnimation.IsPlaying("horse_idle"))
                Horse.hero.CrossFade("horse_idle", 0.1f);
        }

        private void PlayRunAnimation()
        {
            if (!Horse.animation.IsPlaying("horse_Run"))
                Horse.CrossFade("horse_Run", 0.1f);

            Horse.EnableDust();
        }
    }

    private abstract class State
    {
        protected readonly Horse Horse;

        protected State(Horse horse) => Horse = horse;

        public virtual void Enter()
        {
        }

        public virtual void Update()
        {
        }
        
        public virtual void FixedUpdate()
        {
        }

        public virtual void Exit()
        {
        }
    }
}
