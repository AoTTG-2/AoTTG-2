using Assets.Scripts.UI.Elements;

namespace Assets.Scripts.Gamemode.Settings
{
    public class InfectionGamemodeSettings : GamemodeSettings
    {
        public InfectionGamemodeSettings()
        {
            GamemodeType = GamemodeType.Infection;
        }
        [UiElement("Start Infected", "The amount of players that start as an infected")]
        public int Infected { get; set; } = 1;
    }
}
