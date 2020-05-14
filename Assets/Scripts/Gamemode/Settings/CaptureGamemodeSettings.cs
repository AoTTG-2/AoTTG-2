using Assets.Scripts.UI.Elements;

namespace Assets.Scripts.Gamemode.Settings
{
    public class CaptureGamemodeSettings : GamemodeSettings
    {
        public CaptureGamemodeSettings()
        {
            GamemodeType = GamemodeType.Capture;
            RespawnTime = 20f;
            PlayerTitanShifters = false;
            Titans = 0;
            TitanLimit = 25;
            TitanChaseDistance = 120f;
            SpawnTitansOnFemaleTitanDefeat = false;
            FemaleTitanDespawnTimer = 20f;
            FemaleTitanHealthModifier = 0.8f;
        }

        [UiElement("Human Point Limit", "Once this reaches 0, the titans win")]
        public int PvpTitanScoreLimit { get; set; } = 200;

        [UiElement("Titan Point Limit", "Once this reaches 0, the humans win")]
        public int PvpHumanScoreLimit { get; set; } = 200;

        [UiElement("Supply Station on Capture", "Should Supply stations spawn when a point is captured by humans?")]
        public bool SpawnSupplyStationOnHumanCapture { get; set; }
    }
}
