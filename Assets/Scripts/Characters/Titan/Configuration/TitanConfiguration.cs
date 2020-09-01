using Assets.Scripts.Characters.Titan.Attacks;
using Assets.Scripts.Characters.Titan.Behavior;
using Assets.Scripts.Gamemode;
using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Scripts.Characters.Titan
{
    public class TitanConfiguration
    {
        public int Health { get; set; } = 500;
        public int HealthRegeneration { get; set; } = 10;
        public float LimbHealth { get; set; } = 100f;
        public float LimbRegeneration { get; set; } = 10f;
        public float ViewDistance { get; set; } = 200f;
        public float Speed { get; set; } = 20f;
        public float RunSpeed { get; set; } = 25f;
        public float Size { get; set; } = 3f;
        public List<Attack> Attacks { get; set; } = new List<Attack> { new BiteAttack(), new KickAttack(), new StompAttack(), new SmashAttack(), new SlapFaceAttack(), new GrabAttack()};
        public float Stamina { get; set; } = 100f;
        public float StaminaRegeneration { get; set; } = 1f;
        public float Focus { get; set; } = 5f;
        public string AnimationWalk { get; set; } = "run_walk";
        public string AnimationRun { get; set; }
        public string AnimationDeath { get; set; } = "die_back";
        public string AnimationRecovery { get; set; } = "tired";
        public string AnimationTurnLeft { get; set; } = "turnaround2";
        public string AnimationTurnRight { get; set; } = "turnaround1";
        public MindlessTitanType Type { get; set; } = MindlessTitanType.Normal;
        public List<TitanBehavior> Behaviors { get; set; } = new List<TitanBehavior>();

        public TitanConfiguration() { }

        public TitanConfiguration(int health, int healthRegeneration, int limbHealth, float viewDistance, float size, MindlessTitanType type)
        {
            Health = health;
            HealthRegeneration = healthRegeneration;
            LimbHealth = limbHealth;
            Size = size;
            ViewDistance = viewDistance * size;
            Type = type;
            SetMindlessTitanType(type);
            Speed *= Mathf.Sqrt(Size);
            RunSpeed *= Mathf.Sqrt(Size);
            Stamina *= Mathf.Sqrt(Size);
            StaminaRegeneration *= Mathf.Sqrt(Size);
        }

        private void SetMindlessTitanType(MindlessTitanType type)
        {
            switch (type)
            {
                case MindlessTitanType.Normal:
                    AnimationWalk = "run_walk";
                    Attacks.Add(new ComboAttack());
                    Speed = 7f;
                    Focus = 10f;
                    break;
                case MindlessTitanType.Abberant:
                    AnimationWalk = "run_abnormal";
                    AnimationRun = "run_abnormal";
                    Speed = 16f;
                    RunSpeed = 20f;
                    Focus = 8f;
                    Attacks.Add(new BodySlamAttack());
                    break;
                case MindlessTitanType.Jumper:
                    AnimationWalk = "run_abnormal";
                    AnimationRun = "run_abnormal";
                    Speed = 16f;
                    RunSpeed = 20f;
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
                    Speed = 8f;
                    RunSpeed = 18f;
                    Focus = 1f;
                    break;
                case MindlessTitanType.Crawler:
                    AnimationWalk = AnimationRun = "crawler_run";
                    AnimationDeath = "crawler_die";
                    AnimationTurnLeft = "crawler_turnaround_L";
                    AnimationTurnRight = "crawler_turnaround_R";
                    Attacks = new List<Attack>
                    {
                        new JumpAttack(true)
                    };
                    Behaviors = new List<TitanBehavior> { new DeathOnFaceBehavior() };
                    Speed = 22f;
                    RunSpeed = 37f;
                    Focus = 2f;
                    break;
                case MindlessTitanType.Stalker:
                    Speed = 18f;
                    Focus = 200f;
                    break;
                case MindlessTitanType.Burster:
                    Speed = 18f;
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
            Speed = Random.Range(7f, 25f);
            RunSpeed = Random.Range(Speed, Speed + 10f);
            Focus = Random.Range(1f, 15f);
            Attacks = new List<Attack>();
            Behaviors = new List<TitanBehavior> { new RandomAttackBehavior() };
            var attacks = new List<Attack>
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
