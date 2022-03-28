using Assets.Scripts.Characters.Titan;
using Assets.Scripts.Settings.Types;
using UnityEngine;

namespace Assets.Scripts.Settings.Game.Titans
{
    [CreateAssetMenu(fileName = "Custom", menuName = "Settings/Titan/Mindless Titan", order = 2)]
    public class MindlessTitanSettings : BaseTitanSettings
    {
        public DictionarySetting<MindlessTitanType, float> TypeRatio;
        public MultipleEnumSetting<MindlessTitanType> Disabled;
        public DictionarySetting<MindlessTitanType, BaseTitanSettings> TypeSettings;
    }
}
