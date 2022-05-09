using Assets.Scripts.Gamemode;
using System;

namespace Assets.Scripts.Settings.Gamemodes
{
    public class CatchGamemodeSettings : GamemodeSettings
    {
        public CatchGamemodeSettings()
        {
            GamemodeType = GamemodeType.Catch;
        }

        public CatchGamemodeSettings(Difficulty difficulty) : base(difficulty)
        {
            GamemodeType = GamemodeType.Catch;
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
