using Assets.Scripts.Gamemode.Racing;
using Assets.Scripts.Gamemode.Settings;
using Assets.Scripts.UI.Input;
using System.Collections.Generic;
using System.Linq;

namespace Assets.Scripts.Gamemode
{
    public class RacingGamemode : GamemodeBase
    {
        public string localRacingResult = string.Empty;
        public List<RacingObjective> Objectives = new List<RacingObjective>();

        public sealed override GamemodeSettings Settings { get; set; }
        private RacingSettings GamemodeSettings => Settings as RacingSettings;

        public override void OnGameWon()
        {
            FengGameManagerMKII.instance.gameEndCD = GamemodeSettings.RestartOnFinish
                ? 20f
                : 9999f;

            var parameters = new object[] { 0 };
            FengGameManagerMKII.instance.photonView.RPC("netGameWin", PhotonTargets.Others, parameters);
            if (((int) FengGameManagerMKII.settings[0xf4]) == 1)
            {
                //this.chatRoom.addLINE("<color=#FFC000>(" + this.roundTime.ToString("F2") + ")</color> Round ended (game win).");
            }
        }

        private void OnLevelWasLoaded()
        {
            Objectives = Objectives.OrderBy(x => x.Order).ToList();
            for (int i = 0; i < Objectives.Count; i++)
            {
                if (i + 1 >= Objectives.Count) continue;
                Objectives[i].NextObjective = Objectives[i + 1];
            }
            Objectives[0].Next();
        }

        public override string GetVictoryMessage(float timeUntilRestart, float totalServerTime = 0f)
        {
            if (PhotonNetwork.offlineMode)
            {
                var num = (((int)(totalServerTime * 10f)) * 0.1f) - 5f;
                return $"{num}s !!\n Press {InputManager.GetKey(InputUi.Restart)} to Restart.\n\n\n";
            }
            return $"{localRacingResult}\n\nGame Restart in {(int) timeUntilRestart}";
        }

        public override void OnNetGameWon(int score)
        {
            FengGameManagerMKII.instance.gameEndCD = GamemodeSettings.RestartOnFinish
                ? 20f
                : 9999f;
        }
    }
}