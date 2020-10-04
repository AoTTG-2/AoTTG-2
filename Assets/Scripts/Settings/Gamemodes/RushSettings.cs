using Assets.Scripts.Gamemode;
using Assets.Scripts.UI.Elements;
using System;

namespace Assets.Scripts.Settings.Gamemodes
{
    public class RushSettings : GamemodeSettings
    {
        public RushSettings()
        {
            GamemodeType = GamemodeType.TitanRush;
        }

        public RushSettings(Difficulty difficulty) : base(difficulty)
        {
            GamemodeType = GamemodeType.TitanRush;
            Titan.Start = 2;
            TitanInterval = 7;
            switch (difficulty)
            {
                case Difficulty.Easy:
                    TitanInterval = 10;
                    break;
                case Difficulty.Normal:
                    TitanInterval = 7;
                    break;
                case Difficulty.Hard:
                    TitanInterval = 6;
                    break;
                case Difficulty.Abnormal:
                case Difficulty.Realism:
                    TitanInterval = 5;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(difficulty), difficulty, null);
            }
        }

        [UiElement("Titan frequency", "1 titan will spawn per Interval", SettingCategory.Advanced)]
        public int? TitanInterval { get; set; }
    }
}
