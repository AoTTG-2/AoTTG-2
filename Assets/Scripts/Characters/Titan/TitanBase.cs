using Assets.Scripts.Characters.Titan.Behavior;
using Assets.Scripts.Characters.Titan.Body;
using Assets.Scripts.Gamemode;
using UnityEngine;
using MonoBehaviour = Photon.MonoBehaviour;

namespace Assets.Scripts.Characters.Titan
{
    public abstract class TitanBase : MonoBehaviour
    {
        protected TitanBase()
        {
            //Faction = FactionManager.SetTitanity(gameObject);
        }

        public Animation Animation { get; protected set; }
        public TitanBody Body { get; protected set; }
        public Rigidbody Rigidbody { get; protected set; }

        public TitanState State { get; protected set; } = TitanState.Wandering;
        public TitanType Type { get; set; }
        public Difficulty Difficulty { get; set; } = Difficulty.Normal;

        protected string CurrentAnimation { get; set; } = "idle";

        protected TitanBehavior[] Behaviors { get; set; }

        /// <summary cref="Size">
        /// The distance a titan can reach with its attacks. Value influenced by Size
        /// </summary>
        public float AttackDistance { get; protected set; }

        public Faction Faction { get; protected set; }

        /// <summary>
        /// Time in seconds on how long a titan will remain focused on one individual player. Value should not be lower than 1 due to performance reasons
        /// </summary>
        public float Focus { get; protected set; }

        /// <summary>
        /// Time in seconds on how long a titan will remain within idle
        /// </summary>
        public float Idle { get; protected set; }

        /// <summary>
        /// Once health reaches 0 or lower, the titan is dead
        /// </summary>
        public float Health { get; protected set; }

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
        public GameObject Target { get; protected set; }

        /// <summary>
        /// The distance between the target and the titan
        /// </summary>
        public float TargetDistance { get; protected set; }

        /// <summary>
        /// The distance in units of how far a titan is able to detect players
        /// </summary>
        public float ViewDistance { get; protected set; }

        private float FocusTimer { get; set; }
        private float HealthLimit { get; set; }
        private float HealthRegenerationTimer { get; set; }
        private float IdleTimer { get; set; }
        private float StaminaLimit { get; set; }
        private float StaminaRegenerationTimer { get; set; }

        public bool IsAlive => State != TitanState.Dead;
        public bool IsHealthEnabled => HealthLimit > 0;
        public bool IsTarget => Target != null;
        public bool IsTargetHero => Target.GetComponent<Hero>() != null;
        public bool IsTargetTitan => Target.GetComponent<TitanBase>() != null;
        public Hero GetTargetAsHero()
        {
            return Target.GetComponent<Hero>();
        }

        public TitanBase GetTargetAsTitan()
        {
            return Target.GetComponent<TitanBase>();
        }

        protected virtual void SetStamina()
        {
            StaminaRegenerationTimer -= Time.deltaTime;
            if (Stamina >= StaminaLimit) return;
            Stamina += StaminaRecovery * Time.deltaTime;
            if (Stamina > StaminaLimit)
            {
                Stamina = StaminaLimit;
            }
        }

        protected virtual void SetTargetDistance()
        {
            TargetDistance = Target == null
                ? float.MaxValue
                : Mathf.Sqrt((Target.transform.position.x - transform.position.x) * (Target.transform.position.x - transform.position.x) + ((Target.transform.position.z - transform.position.z) * (Target.transform.position.z - transform.position.z)));
        }


        protected virtual void Awake()
        {
            TitanManager.Add(this);
            Animation = GetComponent<Animation>();
            Rigidbody = GetComponent<Rigidbody>();
            Body = GetComponent<MindlessTitanBody>();
        }

        protected virtual void Update()
        {
            if (!photonView.isMine) return;

        }

        protected virtual void OnDestroy()
        {
            TitanManager.Remove(this);
        }
    }
}
