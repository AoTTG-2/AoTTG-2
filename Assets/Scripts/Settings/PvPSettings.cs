using Assets.Scripts.Gamemode;
using Assets.Scripts.Gamemode.Options;
using Assets.Scripts.UI.Elements;
using System;

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

        [UiElement("Ahss Air Reload", "Can AHSS reload in mid air?")]
        public bool? AhssAirReload { get; set; }

        public PvPSettings() { }

        public PvPSettings(Difficulty difficulty)
        {
            switch (difficulty)
            {
                case Difficulty.Easy:
                case Difficulty.Normal:
                case Difficulty.Hard:
                case Difficulty.Abnormal:
                case Difficulty.Realism:
                    Cannons = false;
                    Bomb = false;
                    Mode = PvpMode.Disabled;
                    PvPWinOnEnemiesDead = false;
                    AhssAirReload = true;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(difficulty), difficulty, null);
            }
        }
    }
}
