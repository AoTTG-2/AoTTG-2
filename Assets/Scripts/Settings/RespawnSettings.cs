using Assets.Scripts.Gamemode;
using System;

namespace Assets.Scripts.Settings
{
    public class RespawnSettings
    {
        public RespawnMode Mode { get; set; }

        public int? EndlessRevive { get; set; }

        public RespawnSettings() { }

        public RespawnSettings(Difficulty difficulty)
        {
            switch (difficulty)
            {
                case Difficulty.Easy:
                case Difficulty.Normal:
                case Difficulty.Hard:
                case Difficulty.Abnormal:
                case Difficulty.Realism:
                    Mode = RespawnMode.NEWROUND;
                    EndlessRevive = 5;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(difficulty), difficulty, null);
            }
        }
    }
}
