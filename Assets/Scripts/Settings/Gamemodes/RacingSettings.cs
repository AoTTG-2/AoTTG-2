using Assets.Scripts.Settings.Titans;
using Assets.Scripts.UI.Elements;

namespace Assets.Scripts.Settings.Gamemodes
{
    public class RacingSettings : GamemodeSettings
    {
        public RacingSettings()
        {
            Titan = new SettingsTitan
            {
                Start = 0
            };
            GamemodeType = GamemodeType.Racing;
            PlayerShifters = false;
            Supply = false;
            RespawnMode = RespawnMode.NEVER;
            TitansEnabled = false;
        }
        [UiElement("Restart on Finish", "Should the game restart in 10s upon someone finishing?")]
        public bool RestartOnFinish { get; set; } = true;
    }
}
