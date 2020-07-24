using Assets.Scripts.Gamemode.Options;

namespace Assets.Scripts.Gamemode.Settings
{
    public class PvPAhssSettings : GamemodeSettings
    {
        public PvPAhssSettings()
        {
            GamemodeType = GamemodeType.PvpAhss;
            AhssAirReload = false;
            Titans = -1;
            Pvp = PvpMode.AhssVsBlades;
            PlayerShifters = false;
            Horse = false;
            TitansEnabled = false;
        }
    }
}
