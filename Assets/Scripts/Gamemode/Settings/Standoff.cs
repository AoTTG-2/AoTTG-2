using Assets.Scripts.UI.Elements;
using Assets.Scripts.Gamemode.Options;

namespace Assets.Scripts.Gamemode.Settings
{
    public class StandoffGamemodeSettings : GamemodeSettings
    {
        public StandoffGamemodeSettings()
        {
            GamemodeType = GamemodeType.Standoff;
            PlayerShifters = false;
            Titans = 0;
            TitanLimit = 40;
            TitanChaseDistance = 60f;
            RespawnMode = RespawnMode.NEWROUND;

        }

        [UiElement("Alpha team points", "Once this reaches 0, the team wins")]
        public int AlphaScore { get; set; } = 0;

        [UiElement("Beta team points", "Once this reaches 0, the team wins")]
        public int BetaScore { get; set; } = 0;


    }
}
