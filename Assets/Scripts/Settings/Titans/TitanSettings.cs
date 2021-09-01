using Assets.Scripts.Gamemode;
using Assets.Scripts.Gamemode.Options;
using Assets.Scripts.Settings.Titans.Attacks;
using Assets.Scripts.UI.Elements;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using Random = UnityEngine.Random;

namespace Assets.Scripts.Settings.Titans
{
    public class TitanSettings
    {
        public bool? Enabled { get; set; } //TODO: New attribute which prevents titans from being spawned

        public List<AttackSetting> AttackSettings { get; set; }

        public T Attacks<T>() where T : AttackSetting
        {
            return AttackSettings.Single(x => x.GetType() == typeof(T)) as T;
        }

        [UiElement("Min Size", "Minimal titan size")]
        public float? SizeMinimum { get; set; }

        [UiElement("Max size", "Maximum titan size")]
        public float? SizeMaximum { get; set; }

        [UiElement("Titan Chase Distance", "")]
        public float? ChaseDistance { get; set; }

        [UiElement("Titan Health Mode", "")]
        public TitanHealthMode? HealthMode { get; set; }

        [UiElement("Titan Minimum Health", "")]
        public int? HealthMinimum { get; set; }

        [UiElement("Titan Maximum Health", "")]
        public int? HealthMaximum { get; set; }

        public int? HealthRegeneration { get; set; }

        [UiElement("Explode mode", "")]
        public int? ExplodeMode { get; set; }

        public float? Idle { get; set; }

        public float? Speed { get; set; }

        public float? RunSpeed { get; set; }

        [JsonIgnore]
        public float? Size => SizeMinimum.HasValue && SizeMaximum.HasValue
                    ? Random.Range(SizeMinimum.Value, SizeMaximum.Value)
                    : (float?)null;

        [JsonIgnore]
        public int Health => Random.Range(HealthMinimum.Value, HealthMaximum.Value);

        public TitanSettings() { }

        public TitanSettings(Difficulty difficulty)
        {
            SizeMinimum = 0.7f;
            SizeMaximum = 3.0f;
            ChaseDistance = 100f;
            HealthMinimum = 200;
            HealthMaximum = 500;
            HealthRegeneration = 0;
            ExplodeMode = 0;
            Speed = RunSpeed = 15f;

            switch (difficulty)
            {
                case Difficulty.Easy:
                    HealthMode = TitanHealthMode.Disabled;
                    Idle = 1.8f;
                    break;
                case Difficulty.Normal:
                    HealthMode = TitanHealthMode.Scaled;
                    HealthMinimum = 100;
                    HealthMaximum = 500;
                    Idle = 1.0f;
                    break;
                case Difficulty.Hard:
                    HealthMode = TitanHealthMode.Scaled;
                    HealthMinimum = 250;
                    HealthMaximum = 1000;
                    HealthRegeneration = 10;
                    SizeMinimum = 0.7f;
                    SizeMaximum = 3.5f;
                    Idle = 0.8f;
                    break;
                case Difficulty.Abnormal:
                    HealthMode = TitanHealthMode.Scaled;
                    HealthMinimum = 500;
                    HealthMaximum = 2000;
                    HealthRegeneration = 25;
                    SizeMinimum = 0.7f;
                    SizeMaximum = 3.5f;
                    Idle = 0.5f;
                    break;
                case Difficulty.Realism:
                    HealthMode = TitanHealthMode.Scaled;
                    HealthMinimum = 500;
                    HealthMaximum = 1500;
                    HealthRegeneration = 50;
                    SizeMinimum = 0.7f;
                    SizeMaximum = 3.5f;
                    Idle = 0.3f;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(difficulty), difficulty, null);
            }
        }

    }
}
