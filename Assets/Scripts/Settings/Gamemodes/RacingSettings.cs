using Assets.Scripts.Gamemode;
using Assets.Scripts.Gamemode.Options;
using Assets.Scripts.UI.Elements;

namespace Assets.Scripts.Settings.Gamemodes
{
    public class RacingSettings : GamemodeSettings
    {
        public RacingSettings()
        {
            GamemodeType = GamemodeType.Racing;
        }

        public RacingSettings(Difficulty difficulty) : base(difficulty)
        {
            GamemodeType = GamemodeType.Racing;
            Titan.Start = 0;
            Pvp.Mode = PvpMode.Disabled;
            PlayerShifters = false;
            Supply = false;
            RestartOnFinish = true;
        }

        [UiElement("Restart on Finish", "Should the game restart in 10s upon someone finishing?")]
        public bool? RestartOnFinish { get; set; }
    }
}
