using Assets.Scripts.Gamemode;
using System;

namespace Assets.Scripts.Settings
{
    public class RespawnSettings
    {
        public RespawnMode Mode { get; set; }

        /// <summary>
        /// Time it takes for players to revive.
        /// </summary>
        public int? ReviveTime { get; set; }

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
                    Mode = RespawnMode.NewRound;
                    ReviveTime = 5;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(difficulty), difficulty, null);
            }
        }
    }
}
