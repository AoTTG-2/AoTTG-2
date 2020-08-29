namespace Assets.Scripts.Settings.Gamemodes
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
