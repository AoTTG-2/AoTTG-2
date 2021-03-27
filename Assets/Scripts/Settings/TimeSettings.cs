
using Assets.Scripts.Gamemode;
using System;

namespace Assets.Scripts.Settings
{
    public class TimeSettings
    {
       
        public float? CurrentTime { get; set; }
        public float? DayLength { get; set; }
        public bool? Pause { get; set; }
        public DateTime? LastModified { get; set; }

        public TimeSettings() 
        {
            CurrentTime = 12;
            DayLength = 300;
            Pause = true;
            LastModified = DateTime.UtcNow;
        }
        public TimeSettings(Difficulty difficulty)
        {
            switch (difficulty)
            {
                case Difficulty.Easy:
                case Difficulty.Normal:
                case Difficulty.Hard:
                case Difficulty.Abnormal:
                case Difficulty.Realism:
                    DayLength = 1000;
                    LastModified = DateTime.UtcNow;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(difficulty), difficulty, null);
            }
        }

    }
}
