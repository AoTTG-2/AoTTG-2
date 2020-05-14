using Assets.Scripts.Gamemode.Options;
using Assets.Scripts.UI.Elements;

namespace Assets.Scripts.Gamemode.Settings
{
    public class RacingSettings : GamemodeSettings
    {
        public RacingSettings()
        {
            GamemodeType = GamemodeType.Racing;
            PlayerTitanShifters = false;
            Pvp = PvpMode.Disabled;
            Supply = false;
            RespawnMode = RespawnMode.NEVER;
            Titans = 0;
            TitansEnabled = false;
        }
        [UiElement("Restart on Finish", "Should the game restart in 10s upon someone finishing?")]
        public bool RestartOnFinish { get; set; } = true;
    }
}
