namespace Assets.Scripts.Settings.Gamemodes
{
    public class KillTitansSettings : GamemodeSettings
    {
        public KillTitansSettings()
        {
            GamemodeType = GamemodeType.Titans;
            RestartOnTitansKilled = true;
            RespawnMode = RespawnMode.NEVER;
        }
    }
}
