using Assets.Scripts.Settings.Types;
using System;
using UnityEngine;

namespace Assets.Scripts.Settings.Game
{
    [CreateAssetMenu(fileName = "Custom", menuName = "Settings/Respawn", order = 2)]
    public class RespawnSettings : BaseSettings
    {
        public EnumSetting<RespawnMode> Mode;
        public IntSetting ReviveTime;

        public override void Override(BaseSettings settings)
        {
            if (!(settings is RespawnSettings respawn)) return;
            if (respawn.Mode.HasValue) Mode.Value = respawn.Mode.Value;
            if (respawn.ReviveTime.HasValue) ReviveTime.Value = respawn.ReviveTime.Value;
        }
    }
}
