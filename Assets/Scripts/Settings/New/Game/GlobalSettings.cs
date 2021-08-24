using Assets.Scripts.Settings.New.Types;
using UnityEngine;

namespace Assets.Scripts.Settings.New.Game
{
    [CreateAssetMenu(fileName = "Custom", menuName = "Settings/Global", order = 2)]
    public class GlobalSettings : BaseSettings
    {
        public FloatSetting Gravity;
        public override void Override(BaseSettings settings)
        {

        }
    }
}
