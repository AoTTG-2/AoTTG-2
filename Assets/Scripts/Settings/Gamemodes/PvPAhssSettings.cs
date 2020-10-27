using Assets.Scripts.Gamemode;
using Assets.Scripts.Gamemode.Options;

namespace Assets.Scripts.Settings.Gamemodes
{
    public class PvPAhssSettings : GamemodeSettings
    {
        public PvPAhssSettings()
        {
            GamemodeType = GamemodeType.PvpAhss;
        }

        public PvPAhssSettings(Difficulty difficulty) : base(difficulty)
        {
            GamemodeType = GamemodeType.PvpAhss;
            Pvp.Cannons = false;
            Pvp.Mode = PvpMode.AhssVsBlades;
            Titan.Start = -1;
            PlayerShifters = false;
        }
    }
}
