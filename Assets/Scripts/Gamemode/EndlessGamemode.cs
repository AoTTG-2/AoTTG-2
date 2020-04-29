using Assets.Scripts.Gamemode.Options;

namespace Assets.Scripts.Gamemode
{
    public class EndlessGamemode : GamemodeBase
    {
        public EndlessGamemode()
        {
            GamemodeType = GamemodeType.Endless;
            RespawnMode = RespawnMode.NEVER;
            Pvp = PvpMode.Disabled;
            Titans = 10;
        }

        private int Score { get; set; }

        public override void OnTitanKilled(string titanName)
        {
            Score++;
            FengGameManagerMKII.instance.SpawnTitan(GetTitanConfiguration());
        }

        public override void OnRestart()
        {
            Score = 0;
            base.OnRestart();
        }

        public override string GetGamemodeStatusTop(int time = 0, int totalRoomTime = 0)
        {
            return $"Titans Killed: {Score} Time : {time}";
        }

        public override void OnLevelWasLoaded(Level level, bool isMasterClient = false)
        {
            if (!isMasterClient) return;
            for (int i = 0; i < Titans; i++)
            {
                FengGameManagerMKII.instance.SpawnTitan(GetTitanConfiguration());
            }
        }
    }
}
