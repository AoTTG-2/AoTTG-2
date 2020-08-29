using Assets.Scripts.Settings.Titans;

namespace Assets.Scripts.Gamemode.Settings
{
    public class RushSettings : GamemodeSettings
    {
        public RushSettings()
        {
            GamemodeType = GamemodeType.TitanRush;
            Titan = new SettingsTitan
            {
                Start = 2
            };
        }
    }
}
