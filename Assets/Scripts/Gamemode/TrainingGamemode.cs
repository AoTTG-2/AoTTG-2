using Assets.Scripts.Characters;
using Assets.Scripts.Characters.Titan;
using Assets.Scripts.Settings;
using Assets.Scripts.Settings.Gamemodes;
using Assets.Scripts.UI.InGame.HUD;
using UnityEngine;


namespace Assets.Scripts.Gamemode
{
    public class TrainingGamemode : GamemodeBase
    {
        public override GamemodeType GamemodeType { get; } = GamemodeType.Training;
        private TrainingSettings Settings => GameSettings.Gamemode as TrainingSettings;

        private int Score { get; set; }

        protected override void OnLevelWasLoaded()
        {
            Debug.Log("Level loaded");
            if (!PhotonNetwork.isMasterClient) return;
            //Spawn dummies
        }

        public override void OnRestart()
        {
            Score = 0;
            base.OnRestart();
        }

        protected override void OnEntityUnRegistered(Entity entity)
        {
            //Score when dummie killed
            // Score++;
        }

        protected override void SetStatusTop()
        {
            var content = $"Dummies Killed: {Score} Time : {TimeService.GetRoundTime()}";
            UiService.SetMessage(LabelPosition.Top, content);
        }
    }
}
