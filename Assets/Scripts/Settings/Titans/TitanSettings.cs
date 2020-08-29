using System;
using Assets.Scripts.Gamemode;
using Assets.Scripts.Gamemode.Options;
using Assets.Scripts.UI.Elements;
using Random = UnityEngine.Random;

namespace Assets.Scripts.Settings.Titans
{
    public class TitanSettings
    {
        [UiElement("Min Size", "Minimal titan size", SettingCategory.Titans)]
        public float? SizeMinimum { get; set; }

        [UiElement("Max size", "Maximun titan size", SettingCategory.Titans)]
        public float? SizeMaximum { get; set; }

        [UiElement("Titan Chase Distance", "", SettingCategory.Titans)]
        public float? ChaseDistance { get; set; }

        [UiElement("Titan Health Mode", "", SettingCategory.Titans)]
        public TitanHealthMode HealthMode { get; set; }

        [UiElement("Titan Minimum Health", "", SettingCategory.Titans)]
        public int? HealthMinimum { get; set; }

        [UiElement("Titan Maximum Health", "", SettingCategory.Titans)]
        public int? HealthMaximum { get; set; }

        public int? HealthRegeneration { get; set; }

        [UiElement("Explode mode", "", SettingCategory.Titans)]
        public int? ExplodeMode { get; set; }

        public float Size => Random.Range(SizeMinimum.Value, SizeMaximum.Value);

        public TitanSettings() : this(Difficulty.Normal) { }

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
