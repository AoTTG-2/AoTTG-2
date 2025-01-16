using Assets.Scripts.Settings.Types;
using UnityEngine;

namespace Assets.Scripts.Settings.Game.Titans
{
    [CreateAssetMenu(fileName = "Custom", menuName = "Settings/Titan/Female Titan", order = 2)]
    public class FemaleTitanSettings : BaseTitanSettings
    {
        public FloatSetting DespawnTimer;
        public BoolSetting SpawnTitansOnDefeat;
    }
}
