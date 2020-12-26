
using Assets.Scripts.Gamemode;
using System;

namespace Assets.Scripts.Settings
{
    public class TimeSettings
    {
       
        public float? currentTime { get; set; }
        public float? dayLength { get; set; }
        public bool? pause { get; set; }

        public TimeSettings() { }
        public TimeSettings(Difficulty difficulty)
        {
            switch (difficulty)
            {
                case Difficulty.Easy:
                case Difficulty.Normal:
                case Difficulty.Hard:
                case Difficulty.Abnormal:
                case Difficulty.Realism:
                    dayLength = 1000;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(difficulty), difficulty, null);
            }
        }
        //add difficulty cases?

    }
}
