using Assets.Scripts.Gamemode;
using Assets.Scripts.UI.Elements;
using System;

namespace Assets.Scripts.Settings
{
    public class HorseSettings
    {
        [UiElement("Enabled", "Enables/Disables horses in the game")]
        public bool? Enabled { get; set; }
        public float? BaseSpeed { get; set; }

        public HorseSettings() { }
        public HorseSettings(Difficulty difficulty)
        {
            switch (difficulty)
            {
                case Difficulty.Easy:
                case Difficulty.Normal:
                case Difficulty.Hard:
                case Difficulty.Abnormal:
                case Difficulty.Realism:
                    Enabled = false;
                    BaseSpeed = 45f;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(difficulty), difficulty, null);
            }
        }
    }
}
