using Assets.Scripts.Gamemode.Options;

namespace Assets.Scripts.Gamemode.Settings
{
    public class EndlessSettings : GamemodeSettings
    {
        public EndlessSettings()
        {
            GamemodeType = GamemodeType.Endless;
            RespawnMode = RespawnMode.NEVER;
        }
    }
}
