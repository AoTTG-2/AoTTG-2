using Assets.Scripts.Gamemode.Settings;

namespace Assets.Scripts.Gamemode
{
    public class EndlessGamemode : GamemodeBase
    {
        public sealed override GamemodeSettings Settings { get; set; }
        private EndlessSettings GamemodeSettings => Settings as EndlessSettings;

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

        public override void OnLevelLoaded(Level level, bool isMasterClient = false)
        {
            if (!isMasterClient) return;
            for (int i = 0; i < Settings.Titans; i++)
            {
                FengGameManagerMKII.instance.SpawnTitan(GetTitanConfiguration());
            }
        }
    }
}
