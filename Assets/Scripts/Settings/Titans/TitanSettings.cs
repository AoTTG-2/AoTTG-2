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

        [UiElement("Min Size", "Minimal titan size", SettingCategory.Titans)]
        public float? SizeMinimum { get; set; }

        [UiElement("Max size", "Maximum titan size", SettingCategory.Titans)]
        public float? SizeMaximum { get; set; }

        [UiElement("Titan Chase Distance", "", SettingCategory.Titans)]
        public float? ChaseDistance { get; set; }

        [UiElement("Titan Health Mode", "", SettingCategory.Titans)]
        public TitanHealthMode? HealthMode { get; set; }

        [UiElement("Titan Minimum Health", "", SettingCategory.Titans)]
        public int? HealthMinimum { get; set; }

        [UiElement("Titan Maximum Health", "", SettingCategory.Titans)]
        public int? HealthMaximum { get; set; }

        public int? HealthRegeneration { get; set; }

        [UiElement("Explode mode", "", SettingCategory.Titans)]
        public int? ExplodeMode { get; set; }

        [JsonIgnore]
        public float Size => Random.Range(SizeMinimum.Value, SizeMaximum.Value);

        [JsonIgnore]
        public int Health => Random.Range(HealthMinimum.Value, HealthMaximum.Value);

        public TitanSettings() { }

        public TitanSettings(Difficulty difficulty)
        {
            switch (difficulty)
            {
                case Difficulty.Easy:
                case Difficulty.Normal:
                case Difficulty.Hard:
                case Difficulty.Abnormal:
                case Difficulty.Realism:
                    SizeMinimum = 0.7f;
                    SizeMaximum = 3.0f;
                    ChaseDistance = 100f;
                    HealthMode = TitanHealthMode.Disabled;
                    HealthMinimum = 200;
                    HealthMaximum = 500;
                    HealthRegeneration = 0;
                    ExplodeMode = 0;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(difficulty), difficulty, null);
            }
        }

    }
}
