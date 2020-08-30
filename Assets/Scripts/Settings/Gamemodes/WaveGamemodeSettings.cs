using Assets.Scripts.Characters.Titan;
using Assets.Scripts.Gamemode;
using Assets.Scripts.UI.Elements;

namespace Assets.Scripts.Settings.Gamemodes
{
    public class WaveGamemodeSettings : GamemodeSettings
    {
        public WaveGamemodeSettings()
        {
            GamemodeType = GamemodeType.Wave;
        }

        public WaveGamemodeSettings(Difficulty difficulty) : base(difficulty)
        {
            GamemodeType = GamemodeType.Wave;
            Titan.Start = 3;
            Respawn.Mode = RespawnMode.NEWROUND;

            StartWave = 1;
            MaxWave = 20;
            WaveIncrement = 2;
            BossWave = 5;
            BossType = MindlessTitanType.Punk;
        }

        [UiElement("Start Wave", "What is the start wave?")]
        public int? StartWave { get; set; }
        [UiElement("Max Wave", "What is the final wave?")]
        public int? MaxWave { get; set; }
        [UiElement("Wave Increment", "How many titans will spawn per wave?")]
        public int? WaveIncrement { get; set; }
        public int? BossWave { get; set; }
        public MindlessTitanType? BossType { get; set; }
    }
}
