using Assets.Scripts.Settings;
using Assets.Scripts.Settings.Gamemodes;
using Assets.Scripts.UI.InGame.HUD;
using Assets.Scripts.Room;


namespace Assets.Scripts.Gamemode
{
    public class TrainingGamemode : GamemodeBase
    {
        public override GamemodeType GamemodeType { get; } = GamemodeType.Training;
        private TrainingSettings Settings => GameSettings.Gamemode as TrainingSettings;

        private int Score { get; set; }

        protected override void Level_OnLevelLoaded(int scene, Level level)
        {
            if (!PhotonNetwork.isMasterClient) return;
        }

        public override void OnRestart()
        {
            Score = 0;
            base.OnRestart();
        }

        protected override void SetStatusTop()
        {
            var content = $"Dummies Killed: {Score}";
            UiService.SetMessage(LabelPosition.Top, content);
        }

        //TODO
        //Score when dummie killed
        //Respawn dummies when they have all been killed
        
    }
}
