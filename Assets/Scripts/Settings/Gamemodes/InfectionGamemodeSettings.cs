using Assets.Scripts.Gamemode;
using Assets.Scripts.UI.Elements;

namespace Assets.Scripts.Settings.Gamemodes
{
    public class InfectionGamemodeSettings : GamemodeSettings
    {
        public InfectionGamemodeSettings()
        {
            GamemodeType = GamemodeType.Infection;
        }
        public InfectionGamemodeSettings(Difficulty difficulty)
        {
            GamemodeType = GamemodeType.Infection;
            IsPlayerTitanEnabled = true;
            Infected = 1;
        }

        [UiElement("Start Infected", "The amount of players that start as an infected")]
        public int? Infected { get; set; }
    }
}
