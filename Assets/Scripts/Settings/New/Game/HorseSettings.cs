using Assets.Scripts.Settings.New.Types;
using System;
using UnityEngine;

namespace Assets.Scripts.Settings.New.Game
{
    [CreateAssetMenu(fileName = "Custom", menuName = "Settings/Horse", order = 1)]
    public class HorseSettings : BaseSettings
    {
        public BoolSetting Enabled;
        public FloatSetting BaseSpeed;

        public override void Override(BaseSettings settings)
        {
            throw new NotImplementedException();
        }
    }
}
