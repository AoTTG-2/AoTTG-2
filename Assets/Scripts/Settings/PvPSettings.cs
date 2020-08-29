using System;
using Assets.Scripts.Gamemode;
using Assets.Scripts.Gamemode.Options;
using Assets.Scripts.UI.Elements;

namespace Assets.Scripts.Settings
{
    public class PvPSettings
    {

        [UiElement("PvP Cannons", "Can cannons kill humans?")]
        public bool? Cannons { get; set; }

        [UiElement("PvP Bomb", "Is Bomb PvP enabled?")]
        public bool? Bomb { get; set; }

        [UiElement("PvP Mode", "Can players kill each other")] 
        public PvpMode? Mode { get; set; }

        [UiElement("PvP win on enemies killed", "Does the round end if all PvP enemies are dead?")]
        public bool? PvPWinOnEnemiesDead { get; set; }


        public PvPSettings()
        {
            Cannons = false;
            Bomb = false;
            Mode = PvpMode.Disabled;
            PvPWinOnEnemiesDead = false;
        }

        public PvPSettings(Difficulty difficulty)
        {
            switch (difficulty)
            {
                case Difficulty.Easy:
                    break;
                case Difficulty.Normal:
                    break;
                case Difficulty.Hard:
                    break;
                case Difficulty.Abnormal:
                    break;
                case Difficulty.Realism:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(difficulty), difficulty, null);
            }
        }
    }
}
