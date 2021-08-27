using Assets.Scripts.Gamemode.Options;
using Assets.Scripts.Settings.Types;
using UnityEngine;

namespace Assets.Scripts.Settings.Game
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
            if (!(settings is PvPSettings pvp)) return;
            if (pvp.Cannon.HasValue) Cannon.Value = pvp.Cannon.Value;
            if (pvp.Mode.HasValue) Mode.Value = pvp.Mode.Value;
            if (pvp.PvPWinOnEnemiesDead.HasValue) PvPWinOnEnemiesDead.Value = pvp.PvPWinOnEnemiesDead.Value;
            if (pvp.AhssAirReload.HasValue) AhssAirReload.Value = pvp.AhssAirReload.Value;
        }
    }
}
