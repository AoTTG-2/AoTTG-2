using Assets.Scripts.Gamemode;

namespace Assets.Scripts.Settings.Gamemodes
{
    public class EndlessSettings : GamemodeSettings
    {
        public EndlessSettings()
        {
            GamemodeType = GamemodeType.Endless;
        }
        public EndlessSettings(Difficulty difficulty) : base(difficulty)
        {
            GamemodeType = GamemodeType.Endless;
        }
    }
}
