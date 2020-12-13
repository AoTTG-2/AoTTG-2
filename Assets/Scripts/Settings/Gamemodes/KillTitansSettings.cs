using Assets.Scripts.Gamemode;
using System;

namespace Assets.Scripts.Settings.Gamemodes
{
    public class KillTitansSettings : GamemodeSettings
    {
        public KillTitansSettings()
        {
            GamemodeType = GamemodeType.Titans;
        }

        public KillTitansSettings(Difficulty difficulty) : base(difficulty)
        {
            GamemodeType = GamemodeType.Titans;
            switch (difficulty)
            {
                case Difficulty.Easy:
                case Difficulty.Normal:
                case Difficulty.Hard:
                case Difficulty.Abnormal:
                case Difficulty.Realism:
                    Respawn.Mode = RespawnMode.NewRound;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(difficulty), difficulty, null);
            }
        }
    }
}
