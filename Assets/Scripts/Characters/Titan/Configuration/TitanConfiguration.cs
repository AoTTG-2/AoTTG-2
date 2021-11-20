using Assets.Scripts.Characters.Titan.Attacks;
using Assets.Scripts.Characters.Titan.Behavior;
using Assets.Scripts.Gamemode.Options;
using Assets.Scripts.Settings;
using Assets.Scripts.Settings.Titans;
using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Scripts.Characters.Titan.Configuration
{
    /// <summary>
    /// Contains the Configuration of the Titan. All these settings will determine how the titan will behave and act
    /// </summary>
    public class TitanConfiguration : EntityConfiguration
    {
        private MindlessTitanSettings Settings => GameSettings.Titan.Mindless;
        private TitanSettings TypeSettings { get; set; }

        public int Health { get; set; } = 5000;
        public int HealthRegeneration { get; set; } = 10;
        public float LimbHealth { get; set; } = 100f;
        public float LimbRegeneration { get; set; } = 10f;
        public float ViewDistance { get; set; } = 200f;
        public float Speed { get; set; } = 20f;
        public float RunSpeed { get; set; } = 25f;
        public float Idle { get; set; } = 1f;
        public float Size { get; set; } = 3f;
        public List<Attack<MindlessTitan>> Attacks { get; set; } = new List<Attack<MindlessTitan>> { new BiteAttack(), new KickAttack(), new StompAttack(), new SmashAttack(), new SlapFaceAttack(), new GrabAttack() };
        public float Stamina { get; set; } = 100f;
        public float StaminaRegeneration { get; set; } = 1f;
        public float Focus { get; set; } = 5f;
        public string AnimationWalk { get; set; } = "run_walk";
        public string AnimationRun { get; set; }
        public string AnimationIdle { get; set; } = "idle_2";
        public string AnimationDeath { get; set; } = "die_back";
        public string AnimationRecovery { get; set; } = "tired";
        public string AnimationTurnLeft { get; set; } = "turnaround2";
        public string AnimationTurnRight { get; set; } = "turnaround1";
        public MindlessTitanType Type { get; set; } = MindlessTitanType.Normal;
        public List<TitanBehavior> Behaviors { get; set; } = new List<TitanBehavior>();

        public TitanConfiguration() { }


        public TitanConfiguration(int healthRegeneration, int limbHealth, float viewDistance, MindlessTitanType type)
        {
            Type = type;
            Settings.TypeSettings.TryGetValue(type, out var typeSettings);
            TypeSettings = typeSettings;
            Size = TypeSettings?.Size ?? Settings.Size.Value;
            Health = SetHealth();
            HealthRegeneration = healthRegeneration;
            LimbHealth = limbHealth;
            ViewDistance = viewDistance * Size;
            SetMindlessTitanType(type);
            Speed *= Mathf.Sqrt(Size);
            RunSpeed *= Mathf.Sqrt(Size);
            Stamina *= Mathf.Sqrt(Size);
            StaminaRegeneration *= Mathf.Sqrt(Size);
        }

        private int SetHealth()
        {
            var healthMode = TypeSettings?.HealthMode ?? Settings.HealthMode;
            switch (healthMode)
            {
                case TitanHealthMode.Fixed:
                    return GameSettings.Titan.Mindless.Health;
                case TitanHealthMode.Hit:
                case TitanHealthMode.Scaled:
                    return Mathf.Clamp(Mathf.RoundToInt(Size / 4f * GameSettings.Titan.Mindless.Health), GameSettings.Titan.Mindless.HealthMinimum.Value, GameSettings.Titan.Mindless.HealthMaximum.Value);
                case TitanHealthMode.Disabled:
                    return 0;
                default:
                    throw new ArgumentOutOfRangeException($"Invalid TitanHealthMode enum: {GameSettings.Titan.Mindless.HealthMode}");
            }
        }

        private void SetMindlessTitanType(MindlessTitanType type)
        {
            Idle = TypeSettings?.Idle ?? Settings.Idle.Value;
            Speed = TypeSettings?.Speed ?? Settings.Speed.Value;
            RunSpeed = TypeSettings?.RunSpeed ?? Settings.RunSpeed ?? Speed;

            switch (type)
            {
                case MindlessTitanType.Normal:
                    AnimationWalk = "run_walk";
                    AnimationDeath = "die_front";
                    Attacks.Add(new ComboAttack());
                    Focus = 10f;
                    break;
                case MindlessTitanType.Abberant:
                    AnimationWalk = "run_abnormal";
                    AnimationRun = "run_abnormal";
                    Focus = 8f;
                    Attacks.Add(new BodySlamAttack());
                    break;
                case MindlessTitanType.Jumper:
                    AnimationWalk = "run_abnormal";
                    AnimationRun = "run_abnormal";
                    Focus = 4f;
                    Attacks.Add(new BodySlamAttack());
                    Attacks.Add(new JumpAttack());
                    break;
                case MindlessTitanType.Punk:
                    AnimationWalk = "run_walk";
                    AnimationRun = "run_abnormal_1";
                    Attacks.Add(new ComboAttack(true));
                    Attacks.Add(new RockThrowAttack());
                    Attacks.Add(new SlapAttack());
                    Attacks.Add(new BodySlamAttack());
                    Focus = 1f;
                    break;
                case MindlessTitanType.Crawler:
                    AnimationWalk = AnimationRun = "crawler_run";
                    AnimationDeath = "crawler_die";
                    AnimationTurnLeft = "crawler_turnaround_L";
                    AnimationTurnRight = "crawler_turnaround_R";
                    AnimationIdle = "crawler_idle";
                    Attacks = new List<Attack<MindlessTitan>>
                    {
                        new JumpAttack(true)
                    };
                    Behaviors = new List<TitanBehavior> { new DeathOnFaceBehavior() };
                    Focus = 2f;
                    break;
                case MindlessTitanType.Stalker:
                    Focus = 200f;
                    break;
                case MindlessTitanType.Burster:
                    break;
                case MindlessTitanType.Abnormal:
                    SetAbnormal();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }

        private void SetAbnormal()
        {
            var walkingAnimations = new[] {"run_walk", "run_abnormal", "run_abnormal_1"};
            var runningAnimations = new[] { "run_abnormal", "run_abnormal_1" };
            AnimationWalk = walkingAnimations[Random.Range(0, walkingAnimations.Length)];
            AnimationRun = runningAnimations[Random.Range(0, runningAnimations.Length)];
            Focus = Random.Range(1f, 15f);
            Attacks = new List<Attack<MindlessTitan>>();
            Behaviors = new List<TitanBehavior> { new RandomAttackBehavior() };
            var attacks = new List<Attack<MindlessTitan>>
            {
                new BiteAttack(), new BodySlamAttack(), new GrabAttack(), new KickAttack(), new JumpAttack(),
                new RockThrowAttack(), new SlapFaceAttack(), new SlapAttack(), new StompAttack(), new SmashAttack(),
            };

            for (var i = 0; i < 4; i++)
            {
                var randomAttack = attacks[Random.Range(0, attacks.Count)];
                Attacks.Add(randomAttack);
                attacks.Remove(randomAttack);
            }
        }
    }
}
