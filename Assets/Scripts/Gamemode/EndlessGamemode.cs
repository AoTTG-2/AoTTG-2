using Assets.Scripts.Characters.Titan;
using Assets.Scripts.Settings;
using Assets.Scripts.Settings.Gamemodes;

namespace Assets.Scripts.Gamemode
{
    public class EndlessGamemode : GamemodeBase
    {
        private EndlessSettings Settings => GameSettings.Gamemode as EndlessSettings;

        private int Score { get; set; }

        public override void OnTitanKilled(string titanName)
        {
            Score++;
            SpawnService.Spawn<MindlessTitan>(GetTitanConfiguration());
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
            SpawnTitans(GameSettings.Titan.Start.Value);
        }
    }
}
