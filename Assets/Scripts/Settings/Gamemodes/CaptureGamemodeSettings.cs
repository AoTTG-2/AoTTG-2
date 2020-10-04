using Assets.Scripts.Gamemode;
using Assets.Scripts.UI.Elements;

namespace Assets.Scripts.Settings.Gamemodes
{
    public class CaptureGamemodeSettings : GamemodeSettings
    {
        public CaptureGamemodeSettings()
        {
            GamemodeType = GamemodeType.Capture;
        }

        public CaptureGamemodeSettings(Difficulty difficulty) : base(difficulty)
        {
            GamemodeType = GamemodeType.Capture;
            Titan.Limit = 25;
            Titan.Start = 0;
            PlayerShifters = false;
            PvpTitanScoreLimit = 300;
            PvpHumanScoreLimit = 300;
            SpawnSupplyStationOnHumanCapture = true;
        }

        [UiElement("Human Point Limit", "Once this reaches 0, the titans win")]
        public int? PvpTitanScoreLimit { get; set; }

        [UiElement("Titan Point Limit", "Once this reaches 0, the humans win")]
        public int? PvpHumanScoreLimit { get; set; }

        [UiElement("Supply Station on Capture", "Should Supply stations spawn when a point is captured by humans?")]
        public bool? SpawnSupplyStationOnHumanCapture { get; set; }
    }
}
