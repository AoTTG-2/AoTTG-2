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

        public override string GetGamemodeStatusTop()
        {
            return $"Titans Killed: {Score} Time : {FengGameManagerMKII.instance.timeTotalServer}";
        }

        public override void OnLevelLoaded(Level level, bool isMasterClient = false)
        {
            if (!isMasterClient) return;
            SpawnTitans(Settings.Titans);
        }
    }
}
