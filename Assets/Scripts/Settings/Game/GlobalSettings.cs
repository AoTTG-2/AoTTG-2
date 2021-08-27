using Assets.Scripts.Settings.Types;
using UnityEngine;

namespace Assets.Scripts.Settings.Game
{
    [CreateAssetMenu(fileName = "Custom", menuName = "Settings/Global", order = 2)]
    public class GlobalSettings : BaseSettings
    {
        public FloatSetting Gravity;
        public override void Override(BaseSettings settings)
        {
            if (!(settings is GlobalSettings globalSettings)) return;
            if (globalSettings.Gravity.HasValue) Gravity.Value = globalSettings.Gravity.Value;
        }
    }
}
