using Assets.Scripts.Settings.Types;
using UnityEngine;

namespace Assets.Scripts.Settings.Game
{
    [CreateAssetMenu(fileName = "Custom", menuName = "Settings/Horse", order = 1)]
    public class HorseSettings : BaseSettings
    {
        public BoolSetting Enabled;
        public FloatSetting BaseSpeed;

        public override void Override(BaseSettings settings)
        {
            if (!(settings is HorseSettings horse)) return;
            if (horse.Enabled.HasValue) Enabled.Value = horse.Enabled.Value;
            if (horse.BaseSpeed.HasValue) BaseSpeed.Value = horse.BaseSpeed.Value;
        }
    }
}
