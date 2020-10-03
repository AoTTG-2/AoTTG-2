using Assets.Scripts.Gamemode;
using Assets.Scripts.UI.Elements;

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
            TitanInterval = 7;
        }

        [UiElement("Titan frequency", "1 titan will spawn per Interval", SettingCategory.Advanced)]
        public int? TitanInterval { get; set; }
    }
}
