using Assets.Scripts.Gamemode.Options;
using Assets.Scripts.Settings.New.Types;
using UnityEngine;

namespace Assets.Scripts.Settings.New.Game
{
    [CreateAssetMenu(fileName = "Custom", menuName = "Settings/PvP", order = 2)]
    public class PvPSettings : BaseSettings
    {
        public BoolSetting Cannon;
        public EnumSetting<PvpMode> Mode;
        public BoolSetting PvPWinOnEnemiesDead;
        public BoolSetting AhssAirReload;

        public override void Override(BaseSettings settings)
        {
            
        }
    }
}
