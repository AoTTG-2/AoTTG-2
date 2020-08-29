using System;
using Assets.Scripts.Gamemode;
using Assets.Scripts.UI.Elements;

namespace Assets.Scripts.Settings.Titans
{
    public class FemaleTitanSettings : TitanSettings
    {
        [UiElement("Female Titan Despawn Time", "How long (in seconds), will the FT be on the map after dying?", SettingCategory.Advanced)]
        public float? DespawnTimer { get; set; }

        [UiElement("Spawn Titans on FT Defeat", "Should titans spawn when the Female Titan is killed?", SettingCategory.Advanced)]
        public bool? SpawnTitansOnDefeat { get; set; }

        public FemaleTitanSettings() : this(Difficulty.Normal) { }

        public FemaleTitanSettings(Difficulty difficulty) : base(difficulty)
        {
            switch (difficulty)
            {
                case Difficulty.Easy:
                case Difficulty.Normal:
                case Difficulty.Hard:
                case Difficulty.Abnormal:
                case Difficulty.Realism:
                    HealthMinimum = HealthMaximum = 2000;
                    SpawnTitansOnDefeat = true;
                    DespawnTimer = 5f;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(difficulty), difficulty, null);
            }
        }
    }
}
