using Assets.Scripts.Gamemode;
using Assets.Scripts.UI.Elements;

namespace Assets.Scripts.Settings.Gamemodes
{
    public class BombGamemodeSettings : GamemodeSettings
    {
        public BombGamemodeSettings()
        {
            GamemodeType = GamemodeType.Bomb;
        }
        public BombGamemodeSettings(Difficulty difficulty) : base(difficulty)
        {
            GamemodeType = GamemodeType.Bomb;
        }

    }
}
