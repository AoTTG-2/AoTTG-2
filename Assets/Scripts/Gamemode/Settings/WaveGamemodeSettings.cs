using Assets.Scripts.UI.Elements;

namespace Assets.Scripts.Gamemode.Settings
{
    public class WaveGamemodeSettings : GamemodeSettings
    {
        public WaveGamemodeSettings()
        {
            GamemodeType = GamemodeType.Wave;
        }
        [UiElement("Start Wave", "What is the start wave?")]
        public int StartWave { get; set; } = 1;
        [UiElement("Max Wave", "What is the current wave?")]
        public int MaxWave { get; set; } = 20;
        [UiElement("Wave Increment", "How many titans will spawn per wave?")]
        public int WaveIncrement { get; set; } = 2;
    }
}
