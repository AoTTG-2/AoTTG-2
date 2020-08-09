using Assets.Scripts.UI.Elements;

namespace Assets.Scripts.Gamemode.Settings
{
    public class StandoffGamemodeSettings : GamemodeSettings
    {
        public StandoffGamemodeSettings()
        {
            GamemodeType = GamemodeType.Standoff;
            PlayerShifters = false;
            Titans = 0;
            TitanLimit = 50;
            TitanChaseDistance = 60f;
            RespawnMode = RespawnMode.NEWROUND;

        }

        [UiElement("Human Point Limit", "Once this reaches 0, the titans win")]
        public int PvpTitanScoreLimit { get; set; } = 200;

        [UiElement("Titan Point Limit", "Once this reaches 0, the humans win")]
        public int PvpHumanScoreLimit { get; set; } = 200;

        [UiElement("Supply Station on Capture", "Should Supply stations spawn when a point is captured by humans?")]
        public bool SpawnSupplyStationOnHumanCapture { get; set; }
    }
}
