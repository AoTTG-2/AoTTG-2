using System;
using Assets.Scripts.Gamemode;
using Assets.Scripts.UI.Elements;

namespace Assets.Scripts.Settings.Titans
{
    public class FemaleTitanSettings : TitanSettings
    {
        [UiElement("Female Titan Despawn Time", "How long (in seconds), will the FT be on the map after dying?")]
        public float? DespawnTimer { get; set; }

        [UiElement("Spawn Titans on FT Defeat", "Should titans spawn when the Female Titan is killed?")]
        public bool? SpawnTitansOnDefeat { get; set; }

        public FemaleTitanSettings() { }

        public FemaleTitanSettings(Difficulty difficulty) : base(difficulty)
        {
            SpawnTitansOnDefeat = true;
            DespawnTimer = 20f;
            switch (difficulty)
            {
                case Difficulty.Easy:
                    HealthMinimum = HealthMaximum = 1000;
                    break;
                case Difficulty.Normal:
                    HealthMinimum = HealthMaximum = 2500;
                    break;
                case Difficulty.Hard:
                    HealthMinimum = HealthMaximum = 4000;
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
