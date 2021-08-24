using Assets.Scripts.Characters.Titan;
using Assets.Scripts.Settings.New.Types;
using UnityEngine;

namespace Assets.Scripts.Settings.New.Game.Titans
{
    [CreateAssetMenu(fileName = "Custom", menuName = "Settings/Titan/Mindless Titan", order = 2)]
    public class MindlessTitanSettings : BaseTitanSettings
    {
        //public Dictionary<MindlessTitanType, float> TypeRatio { get; set; }
        public MultipleEnumSetting<MindlessTitanType> Disabled;
        //public Dictionary<MindlessTitanType, TitanSettings> TypeSettings { get; set; }
    }
}
