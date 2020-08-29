using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Gamemode;
using Assets.Scripts.UI.Elements;

namespace Assets.Scripts.Settings.Titans
{
    public class SettingsTitan
    {
        [UiElement("Start Titans", "The amount of titans that will spawn at the start", SettingCategory.Titans)]
        public int? Start { get; set; }

        [UiElement("Titan Limit", "The max amount of titans", SettingCategory.Titans)]
        public int? Limit { get; set; }

        public MindlessTitanSettings Mindless { get; set; }
        public FemaleTitanSettings Female { get; set; }
        public TitanSettings Colossal { get; set; }
        public TitanSettings Eren { get; set; }

        public SettingsTitan() : this (Difficulty.Normal) { }

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
                    Mindless = new MindlessTitanSettings();
                    Female = new FemaleTitanSettings();
                    Colossal = new TitanSettings();
                    Eren = new TitanSettings();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(difficulty), difficulty, null);
            }
        }
    }
}
