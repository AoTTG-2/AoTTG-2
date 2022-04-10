using Assets.Scripts.Characters.Humans;
using System;
using System.Linq;
using Assets.Scripts.Characters.Titan.Attacks;
using Assets.Scripts.Characters.Titan.Behavior;
using Assets.Scripts.Characters.Titan.Configuration;
using Assets.Scripts.Gamemode;
using Assets.Scripts.Gamemode.Options;
using Assets.Scripts.Services;
using Assets.Scripts.Services.Interface;
using Assets.Scripts.Settings;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace Assets.Scripts.Characters.Titan
{
    /// <summary>
    /// TitanBase is the abstract Titan class. It provides a basic implementation for titans, that can be further extended or overriden.
    /// </summary>
    public abstract class TitanBase : Entity
    {
        protected readonly IFactionService FactionService = Service.Faction;
        protected readonly IEntityService EntityService = Service.Entity;

        public GameObject HealthLabel;

        /// <summary>
        /// A reference to the TitanBody component, which contains references to all Titan body parts. Always use this, and NEVER use GameObject.Find()
        /// </summary>
        public TitanBody Body { get; protected set; }
        /// <summary>
        /// A reference to the Rigidbody component
        /// </summary>
        public Rigidbody Rigidbody { get; protected set; }

        public TitanState State { get; protected set; } = TitanState.Wandering;
        public TitanState PreviousState { get; protected set; }
        public TitanState NextState { get; protected set; }

        /// <summary>
        /// The ENUM value of the Titan. This should match with the implemetation type.
        /// </summary>
        public TitanType Type { get; set; }
        [Obsolete("Rather use Settings instead of the hardcoded difficulty. Should change for 608")]
        public Difficulty Difficulty { get; set; } = Difficulty.Normal;

        #region Animations
        /// <summary>
        /// Reference to the Animation component
        /// </summary>
        public Animation Animation { get; protected set; }

        protected string AnimationTurnLeft { get; set; }
        protected string AnimationTurnRight { get; set; }
        public string AnimationWalk { get; protected set; }
        protected string AnimationRun { get; set; }
        protected string AnimationRecovery { get; set; } = "tired";
        protected string AnimationDeath { get; set; } = "die_back";
        protected string AnimationIdle { get; set; } = "idle_2";
        protected string AnimationCover { get; set; } = "idle_recovery";
        protected string AnimationEyes { get; set; } = "hit_eye";

        protected string CurrentAnimation { get; set; } = "idle";

        /// <summary>
        /// Used to CrossFade the titans animation, while also doing various sanity checks. If the player owns the titan object, it will also send the <see cref="CrossFadeRpc"/>
        /// </summary>
        /// <param name="newAnimation"></param>
        /// <param name="fadeLength"></param>
        public virtual void CrossFade(string newAnimation, float fadeLength = 0.1f)
        {
            if (string.IsNullOrWhiteSpace(newAnimation)) return;
            if (Animation.IsPlaying(newAnimation)) return;
            if (!photonView.isMine) return;

            CurrentAnimation = newAnimation;
            Animation.CrossFade(newAnimation, fadeLength);
            photonView.RPC(nameof(CrossFadeRpc), PhotonTargets.Others, newAnimation, fadeLength);
        }

        /// <summary>
        /// RPC for <see cref="CrossFade"/>
        /// </summary>
        /// <param name="newAnimation"></param>
        /// <param name="fadeLength"></param>
        /// <param name="info"></param>
        [PunRPC]
        protected void CrossFadeRpc(string newAnimation, float fadeLength, PhotonMessageInfo info)
        {
            if (info.sender.ID == photonView.owner.ID)
            {
                CurrentAnimation = newAnimation;
                Animation.CrossFade(newAnimation, fadeLength);
            }
        }

        #endregion

        /// <summary>
        /// A list of attacks which the titan can use
        /// </summary>
        public Attack<TitanBase>[] Attacks { get; protected set; }
        /// <summary>
        /// The current attack the titan is using
        /// </summary>
        public Attack<TitanBase> CurrentAttack { get; set; }
        /// <summary>
        /// A list of behaviors which the titan uses
        /// </summary>
        protected TitanBehavior[] Behaviors { get; set; }
        //TODO: 608: Add a setting which can disable the NavMeshAgent even on maps that do support it.
        /// <summary>
        /// The NavMesh agent should be used on maps that support NavMeshes.
        /// </summary>
        public NavMeshAgent NavMeshAgent;

        #region Properties
        /// <summary cref="Size">
        /// The distance a titan can reach with its attacks. Value influenced by Size
        /// </summary>
        public float AttackDistance { get; protected set; }

        /// <summary>
        /// Time in seconds on how long a titan will remain focused on one individual player. Value should not be lower than 1 due to performance reasons
        /// </summary>
        public float Focus { get; protected set; } = 5f;

        /// <summary>
        /// Time in seconds on how long a titan will remain within idle
        /// </summary>
        public float Idle { get; protected set; } = 1f;

        /// <summary>
        /// Once health reaches 0 or lower, the titan is dead
        /// </summary>
        public int Health { get; protected set; }

        /// <summary>
        /// The health the titan spawned with
        /// </summary>
        protected int MaxHealth { get; set; }

        /// <summary>
        /// The amount of health a titan regenerates per second. This value does not exceed the max health that a titan spawned with
        /// </summary>
        public float HealthRegeneration { get; protected set; }

        /// <summary>
        /// Multiplier to prefab scale
        /// </summary>
        public float Size { get; protected set; }

        /// <summary>
        /// Units per second a titan can move
        /// </summary>
        public float Speed { get; protected set; }

        /// <summary>
        /// Modifier to the titan speed when the titan is running
        /// </summary>
        public float SpeedRun { get; protected set; }

        /// <summary>
        /// The amount of stamina a titan has. Once it reaches 0, the titan will enter the recovery state
        /// </summary>
        public float Stamina { get; protected set; }

        /// <summary>
        /// The amount of stamina a titan recovers per second
        /// </summary>
        public float StaminaRecovery { get; protected set; }

        /// <summary>
        /// An objective or enemy that the titan is targeting
        /// </summary>
        public Entity Target { get; protected set; }

        /// <summary>
        /// The distance between the target and the titan
        /// </summary>
        public float TargetDistance { get; protected set; }

        /// <summary>
        /// The distance in units of how far a titan is able to detect players
        /// </summary>
        public float ViewDistance { get; protected set; }
        #endregion

        private float FocusTimer { get; set; }
        private float HealthLimit { get; set; }
        private float HealthRegenerationTimer { get; set; }
        private float IdleTimer { get; set; }
        private float StaminaLimit { get; set; }
        private float StaminaRegenerationTimer { get; set; }

        public bool IsAlive => State != TitanState.Dead;
        public bool IsHealthEnabled => HealthLimit > 0;
        /// <summary>
        /// Returns true if the titan has a Target.
        /// </summary>
        public bool IsTarget => Target != null;

        /// <summary>
        /// Executes the formula which is used to determine what the next target should be. This should not be used OnUpdate or OnFixedUpdate
        /// </summary>
        protected virtual void OnTargetRefresh()
        {
            var hostiles = FactionService.GetAllHostile(this);

            Entity closest = null;
            var distance = float.MaxValue;
            foreach (var hostile in hostiles)
            {
                var hostileDistance = GetTargetDistance(hostile);
                if (hostileDistance < distance)
                {
                    distance = hostileDistance;
                    closest = hostile;
                }
            }

            Target = closest;
            TargetDistance = distance;
            FocusTimer = 0f;
        }

        /// <summary>
        /// Returns the distance between the Titan and its current <see cref="Target"/>
        /// </summary>
        /// <returns></returns>
        protected virtual float GetTargetDistance()
        {
            return GetTargetDistance(Target);
        }

        /// <summary>
        /// Returns the distance between the Titan and an entity
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        protected virtual float GetTargetDistance(Entity entity)
        {
            return entity == null
                ? float.MaxValue
                : Mathf.Sqrt(
                    (entity.transform.position.x - transform.position.x) *
                    (entity.transform.position.x - transform.position.x) +
                    (entity.transform.position.z - transform.position.z) *
                    (entity.transform.position.z - transform.position.z));
        }

        protected virtual void UpdateStamina()
        {
            StaminaRegenerationTimer -= Time.deltaTime;
            if (Stamina >= StaminaLimit) return;
            Stamina += StaminaRecovery * Time.deltaTime;
            if (Stamina > StaminaLimit)
            {
                Stamina = StaminaLimit;
            }
        }
        
        protected override void Awake()
        {
            base.Awake();
            Faction = FactionService.GetTitanity();
            Animation = GetComponent<Animation>();
            Rigidbody = GetComponent<Rigidbody>();
            Body = GetComponent<TitanBody>();
        }

        /// <summary>
        /// Initializes the titan with its configuration
        /// </summary>
        /// <param name="configuration"></param>
        public abstract void Initialize(TitanConfiguration configuration);

        protected virtual void Update()
        {
            if (!photonView.isMine) return;

            if (!IsAlive)
            {
                OnDead();
            }

            TargetDistance = GetTargetDistance();

            switch (State)
            {
                //case TitanState.Disabled:
                //    OnDisabled();
                //    break;
                //case TitanState.Idle:
                //    OnIdle();
                //    break;
                case TitanState.Wandering:
                    OnWandering();
                    break;
                case TitanState.Idle:
                    OnIdle();
                    break;
                //case TitanState.Turning:
                //    OnTurning();
                //    break;
                case TitanState.Chase:
                    OnChasing();
                    break;
                case TitanState.Attacking:
                    OnAttacking();
                    break;
                //case TitanState.Recovering:
                //    OnRecovering();
                //    break;
                //case TitanState.Eat:
                //    OnGrabbing();
                //    break;
            }

        }

        private bool Between(float value, float min = -1f, float max = 1f)
        {
            return value > min && value < max;
        }

        public bool IsStuck()
        {
            var velocity = Rigidbody.velocity;
            return Between(velocity.z, -Speed / 4, Speed / 4)
                   && Between(velocity.x, -Speed / 4, Speed / 4)
                   && Animation[CurrentAnimation].normalizedTime > 2f;
        }

        protected virtual void FixedUpdate()
        {
            if (!photonView.isMine) return;
            Rigidbody.AddForce(new Vector3(0f, -120f * Rigidbody.mass, 0f));
            //if (Behaviors != null && Behaviors.Any(x => x.OnFixedUpdate()))
            //{
            //    return;
            //}

            //if (State == TitanState.Wandering)
            //{
            //    var runModifier = 1f;
            //    var vector12 = transform.forward * Speed * runModifier;
            //    var vector14 = vector12 - Rigidbody.velocity;
            //    vector14.x = Mathf.Clamp(vector14.x, -10f, 10f);
            //    vector14.z = Mathf.Clamp(vector14.z, -10f, 10f);
            //    vector14.y = 0f;
            //    Rigidbody.AddForce(vector14, ForceMode.VelocityChange);
            //    transform.Rotate(0, RotationModifier * Time.fixedDeltaTime, 0);
            //}
        }

        private float RotationModifier { get; set; }
        private void Pathfinding()
        {
            Vector3 forwardDirection = Body.Hip.transform.TransformDirection(new Vector3(-0.3f, 0, 1f));
            RaycastHit objectHit;
            var mask = ~LayerMask.NameToLayer("Ground");
            if (Physics.Raycast(Body.Hip.transform.position, forwardDirection, out objectHit, 50, mask))
            {
                Vector3 leftDirection = Body.Hip.transform.TransformDirection(new Vector3(-0.3f, -1f, 1f));
                Vector3 rightDirection = Body.Hip.transform.TransformDirection(new Vector3(-0.3f, 1f, 1f));
                RaycastHit leftHit;
                RaycastHit rightHit;
                Physics.Raycast(Body.Hip.transform.position, leftDirection, out leftHit, 250, mask);
                Physics.Raycast(Body.Hip.transform.position, rightDirection, out rightHit, 250, mask);

                if (leftHit.distance < rightHit.distance)
                {
                    RotationModifier += Random.Range(10f, 30f);
                }
                else
                {
                    RotationModifier += Random.Range(-30f, -10f); ;
                }

                return;
            }

            RotationModifier = 0f;
        }

        protected virtual void UpdateEverySecond()
        {
            if (State == TitanState.Wandering || State == TitanState.Chase)
            {
                Pathfinding();
            }
        }

        protected virtual void OnDeath()
        {
            base.OnDestroy();
        }

        public override void OnHit(Entity attacker, int damage)
        {
            var direction = (transform.position - attacker.transform.position).normalized;
            Rigidbody.AddForce(direction * 50f, ForceMode.VelocityChange);
            if (this is MindlessTitan t)
            {
                t.photonView.RPC(nameof(OnNapeHitRpc), PhotonTargets.All, attacker.photonView.viewID, damage);
            }
            else
            {
                photonView.RPC(nameof(OnNapeHitRpc), PhotonTargets.All, attacker.photonView.viewID, damage);
            }
            if (Target is Hero hero) hero.CombatTimer?.AddTime();
        }

        [Obsolete("Blocking all damage for 0.2s isn't viable. Instead block this per view ID instead of all")]
        private float DamageTimer { get; set; }

        [PunRPC]
        public virtual void OnNapeHitRpc(int viewId, int damage, PhotonMessageInfo info = new PhotonMessageInfo())
        {
            if (!IsAlive) return;
            var view = PhotonView.Find(viewId);
            if (view == null || !IsAlive || Time.time - DamageTimer < 0.2f) return;
            if (damage < GameSettings.Titan.MinimumDamage.Value) return;
            if (damage > GameSettings.Titan.MaximumDamage.Value)
            {
                damage = GameSettings.Titan.MaximumDamage.Value;
            }

            if (GameSettings.Titan.Mindless.HealthMode.Value == TitanHealthMode.Hit)
            {
                Health--;
            }
            else
            {
                Health -= damage;
            }

            DamageTimer = Time.time;

            if (MaxHealth > 0)
            {
                photonView.RPC(nameof(UpdateHealthLabelRpc), PhotonTargets.All, Health, MaxHealth);
            }

            if (Health <= 0)
            {
                Health = 0;
            }
            else
            {
                return;
            }

            OnDeath();
            SetState(TitanState.Dead);
            FengGameManagerMKII.instance.titanGetKill(view.owner, damage, name);
        }

        [PunRPC]
        protected void UpdateHealthLabelRpc(int currentHealth, int maxHealth)
        {
            if (currentHealth < 0)
            {
                if (HealthLabel != null)
                {
                    Destroy(HealthLabel);
                }
            }
            else
            {
                var color = "7FFF00";
                var num2 = ((float) currentHealth) / ((float) maxHealth);
                if ((num2 < 0.75f) && (num2 >= 0.5f))
                {
                    color = "f2b50f";
                }
                else if ((num2 < 0.5f) && (num2 >= 0.25f))
                {
                    color = "ff8100";
                }
                else if (num2 < 0.25f)
                {
                    color = "ff3333";
                }
                HealthLabel.GetComponent<TextMesh>().text = $"<color=#{color}>{currentHealth}</color>";
            }
        }

        #region Titan State Logic

        public virtual void SetState(TitanState state)
        {
            if (!IsAlive) return;
            if (state == State) return;

            if ((State == TitanState.Attacking)
                && state != TitanState.Dead
                && PreviousState != TitanState.Idle)
            {
                PreviousState = State;
                NextState = state;
                State = TitanState.Idle;
                IdleTimer = Idle;
                SetStateAnimation(TitanState.Idle);
                return;
            }

            if (State == TitanState.Idle) { }

            PreviousState = State == TitanState.Idle
                ? TitanState.Chase
                : State;
            State = state;
            SetStateAnimation(State);
        }

        protected virtual void SetStateAnimation(TitanState state)
        {
            switch (state)
            {
                case TitanState.Wandering:
                case TitanState.Chase:
                    CrossFade(AnimationWalk, 0.5f);
                    break;
                case TitanState.Dead:
                    CrossFade(AnimationDeath);
                    break;
                case TitanState.Idle:
                    CrossFade("idle", 0.2f);
                    break;
                case TitanState.Attacking:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(state), state, null);
            }
        }

        protected virtual void OnAttacking()
        {
            if (CurrentAttack.IsFinished)
            {
                CurrentAttack.IsFinished = false;
                Stamina -= CurrentAttack.Stamina;
                SetState(TitanState.Chase);
                return;
            }
            CurrentAttack.Execute();
        }
        
        protected virtual void OnChasing()
        {
            if (Target == null/* || ViewDistance < TargetDistance*/)
            {
                SetState(TitanState.Wandering);
                return;
            }


            FocusTimer += Time.deltaTime;
            if (FocusTimer > Focus)
            {
                OnTargetRefresh();
                return;
            }

            var pos = Target.transform.position;
            pos.y = 0;
            NavMeshAgent.SetDestination(pos);

            var availableAttacks = Attacks.Where(x => x.CanAttack()).ToArray();
            if (availableAttacks.Length > 0)
            {
                CurrentAttack = availableAttacks[Random.Range(0, availableAttacks.Length)];
                SetState(TitanState.Attacking);
            }
        }

        private float DespawnTimer { get; } = 5f;
        private bool HasDieSteam { get; set; }
        protected virtual void OnDead()
        {
            var deathTime = Animation[AnimationDeath].normalizedTime;
            if (deathTime > 2f && !HasDieSteam && photonView.isMine)
            {
                HasDieSteam = true;
                PhotonNetwork.Instantiate("FX/FXtitanDie1", Body.Hip.position, Quaternion.Euler(-90f, 0f, 0f), 0).transform.localScale = transform.localScale;
            }
            if (deathTime > DespawnTimer)
            {
                if (base.photonView.isMine)
                {
                    PhotonNetwork.Instantiate("FX/FXtitanDie", Body.Hip.position, Quaternion.Euler(-90f, 0f, 0f), 0).transform.localScale = transform.localScale;
                    PhotonNetwork.Destroy(gameObject);
                }
            }
        }

        protected void OnIdle()
        {
            IdleTimer -= Time.deltaTime;
            if (IdleTimer <= 0)
            {
                SetState(NextState);
            }
        }

        protected virtual void OnWandering()
        {
            if (!Animation.IsPlaying(AnimationWalk))
            {
                SetStateAnimation(TitanState.Wandering);
            }

            FocusTimer += Time.deltaTime;
            if (FocusTimer > Focus)
            {
                OnTargetRefresh();
                SetState(TitanState.Chase);
                return;
            }
        }

        #endregion

    }
}
