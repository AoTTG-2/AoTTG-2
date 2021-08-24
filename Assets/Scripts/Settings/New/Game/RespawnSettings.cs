using Assets.Scripts.Settings.New.Types;
using System;
using UnityEngine;

namespace Assets.Scripts.Settings.New.Game
{
    [CreateAssetMenu(fileName = "Custom", menuName = "Settings/Respawn", order = 2)]
    public class RespawnSettings : BaseSettings
    {
        public EnumSetting<RespawnMode> Mode;
        public IntSetting ReviveTime;

        public override void Override(BaseSettings settings)
        {
            throw new NotImplementedException();
        }
    }
}
