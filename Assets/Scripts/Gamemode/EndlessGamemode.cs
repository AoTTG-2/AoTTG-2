using Assets.Scripts.Characters;
using Assets.Scripts.Characters.Titan;
using Assets.Scripts.Room;
using Assets.Scripts.Settings;
using Assets.Scripts.Settings.Gamemodes;
using Assets.Scripts.UI.InGame.HUD;

namespace Assets.Scripts.Gamemode
{
    public class EndlessGamemode : GamemodeBase
    {
        public override GamemodeType GamemodeType { get; } = GamemodeType.Endless;
        private EndlessSettings Settings => GameSettings.Gamemode as EndlessSettings;

        private int Score { get; set; }

        protected override void Level_OnLevelLoaded(int scene, Level level)
        {
            if (!PhotonNetwork.isMasterClient) return;
            SpawnTitans(GameSettings.Titan.Start.Value);
        }

        public override void OnRestart()
        {
            Score = 0;
            base.OnRestart();
        }

        protected override void OnEntityUnRegistered(Entity entity)
        {
            Score++;
            if (entity is MindlessTitan)
            {
                SpawnService.Spawn<MindlessTitan>(GetTitanConfiguration());
            }
        }

        protected override void SetStatusTop()
        {
            var content = $"Titans Killed: {Score} Time : {TimeService.GetRoundTime()}";
            UiService.SetMessage(LabelPosition.Top, content);
        }
    }
}
