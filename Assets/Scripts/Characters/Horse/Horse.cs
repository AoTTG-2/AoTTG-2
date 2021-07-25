using Assets.Scripts.Characters.Humans;
using UnityEngine;

/// <summary>
/// The horse is a vehicle that can be controller by a <see cref="Human"/> once mounted. Current implementation is broken as its an incomplete version of #116
/// </summary>
public sealed class Horse : PhotonView
{
    private new Animation animation;

    private State currentState;

    [SerializeField]
    private ParticleSystem dustParticles;

    private FollowState followState;
    private MountState mountState;
    private IdleState idleState;

    [SerializeField]
    private float gravityFactor = -20f;

    private LayerMask groundMask;
    private Hero hero;
    private Transform heroTransform;
    private LayerMask isGroundedMask;
    private new Rigidbody rigidbody;

    private float speed = 45f;

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

    private void LateUpdate()
    {
        if (!hero && photonView.isMine)
        {
            PhotonNetwork.Destroy(gameObject);
            return;
        }

        currentState.Update();

        rigidbody.AddForce(new Vector3(0f, gravityFactor * rigidbody.mass, 0f));
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

        public override void Update()
        {
            if (!Horse.hero)
            {
                Horse.TransitionToState(Horse.idleState);
                return;
            }

            if (Horse.rigidbody.velocity.magnitude > 8f)
            {
                if (!Horse.animation.IsPlaying("horse_Run"))
                    Horse.CrossFade("horse_Run", 0.1f);

                Horse.EnableDust();
            }
            else
            {
                if (!Horse.animation.IsPlaying("horse_WALK"))
                    Horse.CrossFade("horse_WALK", 0.1f);

                Horse.DisableDust();
            }

            var horizontalVector = target - Horse.transform.position;
            var horizontalAngle = -Mathf.Atan2(horizontalVector.z, horizontalVector.x) * Mathf.Rad2Deg;
            var num = -Mathf.DeltaAngle(horizontalAngle, Horse.gameObject.transform.rotation.eulerAngles.y - 90f);
            Horse.gameObject.transform.rotation = Quaternion.Lerp(
                Horse.gameObject.transform.rotation,
                Quaternion.Euler(0f, Horse.gameObject.transform.rotation.eulerAngles.y + num, 0f),
                (200f * Time.deltaTime) / (Horse.rigidbody.velocity.magnitude + 20f));

            if (Vector3.Distance(target, Horse.transform.position) < 20f)
            {
                Horse.rigidbody.AddForce((Horse.transform.forward * Horse.speed) * 0.7f, ForceMode.Acceleration);
                if (Horse.rigidbody.velocity.magnitude >= Horse.speed)
                    Horse.rigidbody.AddForce((-Horse.speed * 0.7f) * Horse.rigidbody.velocity.normalized, ForceMode.Acceleration);
            }
            else
            {
                Horse.rigidbody.AddForce(Horse.transform.forward * Horse.speed, ForceMode.Acceleration);
                if (Horse.rigidbody.velocity.magnitude >= Horse.speed)
                    Horse.rigidbody.AddForce(-Horse.speed * Horse.rigidbody.velocity.normalized, ForceMode.Acceleration);
            }

            timeElapsed += Time.deltaTime;
            if (timeElapsed > 0.6f)
            {
                timeElapsed = 0f;
                if (Vector3.Distance(Horse.heroTransform.position, target) > 20f)
                    GetNewTarget();
            }

            if (Vector3.Distance(Horse.heroTransform.position, Horse.transform.position) < 5f)
                Horse.TransitionToState(Horse.idleState);
            else if (Vector3.Distance(target, Horse.transform.position) < 5f)
                Horse.TransitionToState(Horse.idleState);

            awayTimer += Time.deltaTime;
            if (awayTimer > 6f)
            {
                awayTimer = 0f;
                var start = Horse.transform.position + Vector3.up;
                var end = Horse.heroTransform.position + Vector3.up;
                if (Physics.Linecast(
                    start,
                    end,
                    Horse.groundMask))
                    Horse.transform.position = new Vector3(
                        Horse.heroTransform.position.x,
                        GetHeight(Horse.heroTransform.position + Vector3.up * 5f),
                        Horse.heroTransform.position.z);
            }
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

        public override void FixedUpdate()
        {
            base.FixedUpdate();
        }

        public override void Update()
        {
            Horse.ToIdleAnimation();
            var heroDistance = Vector3.Distance(Horse.heroTransform.position, Horse.transform.position);
            if (Horse.hero && heroDistance > 20f)
                Horse.TransitionToState(Horse.followState);
        }
    }

    private sealed class MountState : State
    {
        private readonly HorseController controller;

        public MountState(Horse horse, HorseController controller)
            : base(horse)
        {
            this.controller = controller;
        }

        public Animation HeroAnimation { get; set; }

        public Rigidbody HeroRigidbody { get; set; }

        private bool IsGrounded =>
            Physics.Raycast(
                Horse.gameObject.transform.position + Vector3.up * 0.1f,
                -Vector3.up,
                0.3f,
                Horse.isGroundedMask.value);

        public override void Enter()
        {
            controller.enabled = true;
        }

        public override void Exit()
        {
            controller.enabled = false;
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
        }

        public override void Update()
        {
            if (!Horse.hero)
            {
                Horse.TransitionToState(Horse.idleState);
                return;
            }

            var playerOffset = Vector3.up * 1.68f;
            Horse.heroTransform.position = Horse.transform.position + playerOffset;
            Horse.heroTransform.rotation = Horse.transform.rotation;
            HeroRigidbody.velocity = Horse.rigidbody.velocity;

            if (controller.TargetDirection != -874f)
            {
                Horse.gameObject.transform.rotation = Quaternion.Lerp(
                    Horse.gameObject.transform.rotation,
                    Quaternion.Euler(0f, controller.TargetDirection, 0f),
                    (100f * Time.deltaTime) / (Horse.rigidbody.velocity.magnitude + 20f));
                if (controller.ShouldWalk)
                {
                    Horse.rigidbody.AddForce((Horse.transform.forward * Horse.speed) * 0.6f, ForceMode.Acceleration);
                    if (Horse.rigidbody.velocity.magnitude >= (Horse.speed * 0.6f))
                        Horse.rigidbody.AddForce((-Horse.speed * 0.6f) * Horse.rigidbody.velocity.normalized, ForceMode.Acceleration);
                }
                else
                {
                    Horse.rigidbody.AddForce(Horse.transform.forward * Horse.speed, ForceMode.Acceleration);
                    if (Horse.rigidbody.velocity.magnitude >= Horse.speed)
                        Horse.rigidbody.AddForce(-Horse.speed * Horse.rigidbody.velocity.normalized, ForceMode.Acceleration);
                }

                if (Horse.rigidbody.velocity.magnitude > 8f)
                {
                    if (!Horse.animation.IsPlaying("horse_Run"))
                        Horse.CrossFade("horse_Run", 0.1f);

                    if (!HeroAnimation.IsPlaying("horse_Run"))
                        Horse.hero.CrossFade("horse_run", 0.1f);

                    Horse.EnableDust();
                }
                else
                {
                    if (!Horse.animation.IsPlaying("horse_WALK"))
                        Horse.CrossFade("horse_WALK", 0.1f);

                    if (!HeroAnimation.IsPlaying("horse_idle"))
                        Horse.hero.CrossFade("horse_idle", 0.1f);

                    Horse.DisableDust();
                }
            }
            else
            {
                Horse.ToIdleAnimation();
                if (Horse.rigidbody.velocity.magnitude > 15f)
                {
                    if (!HeroAnimation.IsPlaying("horse_Run"))
                        Horse.hero.CrossFade("horse_run", 0.1f);
                }
                else if (!HeroAnimation.IsPlaying("horse_idle"))
                    Horse.hero.CrossFade("horse_idle", 0.1f);
            }

            if (controller.ShouldJump && IsGrounded)
                Horse.rigidbody.AddForce(Vector3.up * 25f, ForceMode.VelocityChange);
        }
    }

    private abstract class State
    {
        protected readonly Horse Horse;

        public State(Horse horse)
        {
            Horse = horse;
        }

        public virtual void Enter()
        {
        }

        public virtual void Exit()
        {
        }

        public virtual void FixedUpdate()
        {
        }

        public virtual void Update()
        {
        }
    }
}