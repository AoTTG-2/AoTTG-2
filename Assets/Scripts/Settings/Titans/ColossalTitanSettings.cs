using Assets.Scripts.Gamemode;
using Assets.Scripts.Gamemode.Options;
using System;

namespace Assets.Scripts.Settings.Titans
{
    public class ColossalTitanSettings : TitanSettings
    {
        public ColossalTitanSettings() { }

        public ColossalTitanSettings(Difficulty difficulty) : base(difficulty)
        {
            HealthMode = TitanHealthMode.Scaled;
            switch (difficulty)
            {
                case Difficulty.Easy:
                    HealthMinimum = HealthMaximum = 2000;
                    break;
                case Difficulty.Normal:
                    HealthMinimum = HealthMaximum = 3000;
                    break;
                case Difficulty.Hard:
                    HealthMinimum = HealthMaximum = 5000;
                    break;
                case Difficulty.Abnormal:
                    HealthMinimum = HealthMaximum = 7500;
                    break;
                case Difficulty.Realism:
                    HealthMinimum = HealthMaximum = 10000;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(difficulty), difficulty, null);
            }
        }
    }
}
