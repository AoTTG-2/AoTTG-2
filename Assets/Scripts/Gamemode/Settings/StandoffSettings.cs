using Assets.Scripts.Gamemode.Options;
using Assets.Scripts.UI.Elements;

namespace Assets.Scripts.Gamemode.Settings
{
    public class StandoffSettings : GamemodeSettings
    {
        public StandoffSettings()
        {
            GamemodeType = GamemodeType.Standoff;
            PlayerShifters = false;
            Titans = 10;
            TitanLimit = 40;
            TitanChaseDistance = 60f;
            TeamMode = TeamMode.NoSort;
            RespawnMode = RespawnMode.NEWROUND;
        }
        [UiElement("Alpha team points", "Once this reaches 20, the team wins")]
        public int AlphaScore { get; set; } = 10;
        [UiElement("Beta team points", "Once this reaches 20, the team wins")]
        public int BetaScore { get; set; } = 10;
    }
}
