using Assets.Scripts.Settings.New.Types;
using UnityEngine;

namespace Assets.Scripts.Settings.New.Game.Titans
{
    [CreateAssetMenu(fileName = "Custom", menuName = "Settings/Titan/Female Titan", order = 2)]
    public class FemaleTitanSettings : BaseTitanSettings
    {
        public FloatSetting DespawnTimer;
        public BoolSetting SpawnTitansOnDefeat;
    }
}
