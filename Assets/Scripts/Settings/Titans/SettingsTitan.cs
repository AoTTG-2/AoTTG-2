using Assets.Scripts.Gamemode;
using Assets.Scripts.UI.Elements;
using System;

namespace Assets.Scripts.Settings.Titans
{
    public class SettingsTitan
    {
        [UiElement("Start Titans", "The amount of titans that will spawn at the start")]
        public int? Start { get; set; }

        [UiElement("Titan Limit", "The max amount of titans")]
        public int? Limit { get; set; }

        [UiElement("Minimum Damage Mode", "Minimum damage you need to do")]
        public int? MinimumDamage { get; set; }

        [UiElement("Maximum Damage Mode", "Maximum damage that can be dealt")]
        public int? MaximumDamage { get; set; }

        public MindlessTitanSettings Mindless { get; set; }
        public FemaleTitanSettings Female { get; set; }
        public TitanSettings Colossal { get; set; }
        public TitanSettings Eren { get; set; }

        public SettingsTitan() { }

        public SettingsTitan(Difficulty difficulty)
        {
            switch (difficulty)
            {
                case Difficulty.Easy:
                case Difficulty.Normal:
                case Difficulty.Hard:
                case Difficulty.Abnormal:
                case Difficulty.Realism:
                    Start = 10;
                    Limit = 30;
                    MinimumDamage = 0;
                    MaximumDamage = int.MaxValue;
                    Mindless = new MindlessTitanSettings();
                    Female = new FemaleTitanSettings();
                    Colossal = new ColossalTitanSettings();
                    Eren = new TitanSettings();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(difficulty), difficulty, null);
            }
        }
    }
}
