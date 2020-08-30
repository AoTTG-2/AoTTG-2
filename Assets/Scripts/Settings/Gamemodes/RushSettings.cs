using Assets.Scripts.Gamemode;

namespace Assets.Scripts.Settings.Gamemodes
{
    public class RushSettings : GamemodeSettings
    {
        public RushSettings()
        {
            GamemodeType = GamemodeType.TitanRush;
        }

        public RushSettings(Difficulty difficulty) : base(difficulty)
        {
            GamemodeType = GamemodeType.TitanRush;
            Titan.Start = 2;
        }
    }
}
