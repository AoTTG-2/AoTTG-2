using Assets.Scripts.Gamemode.Racing;
using Assets.Scripts.Gamemode.Settings;
using Assets.Scripts.UI.Input;
using System.Collections.Generic;
using System.Linq;

namespace Assets.Scripts.Gamemode
{
    public class RacingGamemode : GamemodeBase
    {
        /// <summary>
        /// issue #277 will have to be removed once the conversion from fenggamemanager to single manager will be done
        /// </summary>
        public string localRacingResult = string.Empty;
        public List<RacingObjective> Objectives = new List<RacingObjective>();

        public sealed override GamemodeSettings Settings { get; set; }
        private RacingSettings GamemodeSettings => Settings as RacingSettings;

        ///Issue #277 added just to init the localLastRacingTime each time the race start again
        public override void OnRestart()
        {
            localRacingResult = string.Empty;
            base.OnRestart();
        }

        public override void OnGameWon()
        {
            FengGameManagerMKII.instance.gameEndCD = GamemodeSettings.RestartOnFinish
                ? 20f
                : 9999f;

            var parameters = new object[] { 0 };
            FengGameManagerMKII.instance.photonView.RPC("netGameWin", PhotonTargets.Others, parameters);
            
            /*
             * i have no idea what that's suppose to do as what's inside the if is commented anyway
             * 
            if (((int) FengGameManagerMKII.settings[0xf4]) == 1)
            {
                //this.chatRoom.addLINE("<color=#FFC000>(" + this.roundTime.ToString("F2") + ")</color> Round ended (game win).");
            }
            */
        }

        private void OnLevelWasLoaded()
        {
            if (Objectives.Count == 0) return;
            Objectives = Objectives.OrderBy(x => x.Order).ToList();
            for (int i = 0; i < Objectives.Count; i++)
            {
                if (i + 1 >= Objectives.Count) continue;
                Objectives[i].NextObjective = Objectives[i + 1];
            }
            Objectives[0].Current();
        }

        public override string GetVictoryMessage(float timeUntilRestart, float totalServerTime = 0f)
        {
            if (PhotonNetwork.offlineMode)
            {
                if (string.IsNullOrEmpty(localRacingResult))
                {
                    //also subtract 1 delta time cos this will be called the first time the update next to the game win 
                    totalServerTime -= (20f-UnityEngine.Time.deltaTime);
                    //the following one is the hh:mm:ss,sss format
                    //localRacingResult = ((int) totalServerTime/3600).ToString("00") + ":"+((int)totalServerTime/60%60).ToString("00") + ":" +(totalServerTime%60).ToString("00.000");
                    localRacingResult = totalServerTime.ToString("f2");

                }
                return $"{localRacingResult}s !!\n Press {InputManager.GetKey(InputUi.Restart)} to Restart.\n\n\n";
            }
            return $"{localRacingResult}\n\nGame Restart in {(int) timeUntilRestart}";
        }

        /// <summary>
        /// format the text that has to be printed on top of the screen for this specific gamemode
        /// Is just temporary as things will have to be changed and handled locally without needing to go throught FGMKII core2
        /// </summary>
        /// <param name="time">it require the roundtime in csec (0.1sec) so it require you to pass the time*10 so it will hold also info about the first dec</param>
        /// <param name="totalRoomTime"></param>
        /// <returns></returns>
        public override string GetGamemodeStatusTop(int time = 0, int totalRoomTime = 0)
        {
            time -= 200;
            if (time > 0)
                //if the starting time has passed it return 
                return (time / 10f).ToString("000.0");
            else
                //if the game has not started yet it tell waiting and the countdown 
                return "Time: WAITING ";// + (time / -10f).ToString("00.0");
        }

        public override void OnNetGameWon(int score)
        {
            FengGameManagerMKII.instance.gameEndCD = GamemodeSettings.RestartOnFinish
                ? 20f
                : 9999f;
        }
    }
}