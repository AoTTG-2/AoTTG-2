using Assets.Scripts.Characters.Titan.Attacks;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Characters.Titan
{
    public class TitanConfiguration
    {
        public int Health { get; set; } = 500;
        public int HealthRegeneration { get; set; } = 10;
        public float LimbHealth { get; set; } = 100;
        public float ViewDistance { get; set; } = 200f;
        public float Speed { get; set; } = 20f;
        public float Size { get; set; } = 3f;
        public List<Attack> Attacks { get; set; } = new List<Attack> {new GrabAttack(), new BiteAttack(), new StompAttack(), new SmashAttack()};
        public float Stamina { get; set; } = 100f;
        public float StaminaRegeneration { get; set; } = 5f;
        public string AnimationWalk { get; set; } = "run_walk";
        public string AnimationRun { get; set; }
        public string AnimationDeath { get; set; } = "die_back";
        public string AnimationRecovery { get; set; } = "tired";
        public string AnimationTurnLeft { get; set; } = "turnaround2";
        public string AnimationTurnRight { get; set; } = "turnaround1";
        public MindlessTitanType Type { get; set; } = MindlessTitanType.Normal;

        public TitanConfiguration() { }

        public TitanConfiguration(int health, int healthRegeneration, int limbHealth, int viewDistance, float speed, float size, MindlessTitanType type)
        {
            Health = health;
            HealthRegeneration = healthRegeneration;
            LimbHealth = limbHealth;
            ViewDistance = viewDistance;
            Speed = speed;
            Size = size;
            Type = type;
            SetMindlessTitanType(type);
        }

        public TitanConfiguration(int health, int healthRegeneration, int limbHealth, int viewDistance, float size, MindlessTitanType type)
        {
            Health = health;
            HealthRegeneration = healthRegeneration;
            LimbHealth = limbHealth;
            ViewDistance = viewDistance;
            Size = size;
            Type = type;
            SetSpeed(type);
            SetMindlessTitanType(type);
            if (Size > 1f)
            {
                Speed *= Mathf.Sqrt(Size);
            }
        }

        private void SetSpeed(MindlessTitanType type)
        {
            switch (type)
            {
                case MindlessTitanType.Normal:
                    Speed = 7f;
                    break;
                case MindlessTitanType.Abnormal:
                case MindlessTitanType.Jumper:
                case MindlessTitanType.Stalker:
                case MindlessTitanType.Burster:
                case MindlessTitanType.Punk:
                    Speed = 18f;
                    break;
                case MindlessTitanType.Crawler:
                    Speed = 25f;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }

        private void SetMindlessTitanType(MindlessTitanType type)
        {
            switch (type)
            {
                case MindlessTitanType.Normal:
                    AnimationWalk = AnimationRun = "run_walk";
                    break;
                case MindlessTitanType.Abnormal:
                    AnimationWalk = "run_walk";
                    AnimationRun = "run_abnormal"; 
                    break;
                case MindlessTitanType.Jumper:
                    AnimationWalk = "run_walk";
                    AnimationRun = "run_abnormal";
                    break;
                case MindlessTitanType.Punk:
                    AnimationWalk = "run_walk";
                    AnimationRun = "run_abnormal_1";
                    break;
                case MindlessTitanType.Crawler:
                    AnimationWalk = AnimationRun = "crawler_run";
                    AnimationTurnLeft = "crawler_turnaround_L";
                    AnimationTurnRight = "crawler_turnaround_R";
                    break;
                case MindlessTitanType.Stalker:
                    break;
                case MindlessTitanType.Burster:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }
    }
}
