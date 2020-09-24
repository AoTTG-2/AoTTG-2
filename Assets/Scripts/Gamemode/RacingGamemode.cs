using Assets.Scripts.Gamemode.Racing;
using Assets.Scripts.Gamemode.Settings;
using Assets.Scripts.UI.Input;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;
using HUD = Assets.Scripts.UI.InGame.HUD;

namespace Assets.Scripts.Gamemode
{
    public class RacingGamemode : GamemodeBase
    {
        public void Awake()
        {
            //setting the refresh time twice the wanted resolution 
            this.HUDRefreshTime = .05f;
        }

        public const int StartTimerCountdown = 20;

        public bool startRacing;

        public bool endRacing;

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

        private float CalculateGameEndCD()
        {
            if (GamemodeSettings.RestartOnFinish)
                return 20f;
            return 9999f;
        }

        public override void OnGameWon()
        {
            FengGameManagerMKII.instance.gameEndCD = CalculateGameEndCD();
            FengGameManagerMKII.instance.photonView.RPC("netGameWin", PhotonTargets.Others, 0);
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
                    totalServerTime -= (RacingGamemode.StartTimerCountdown - UnityEngine.Time.deltaTime);
                    //the following one is to write the result in the hh:mm:ss,sss format  ((int) totalServerTime/3600).ToString("00") + ":"+((int)totalServerTime/60%60).ToString("00") + ":" +(totalServerTime%60).ToString("00.000");
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
        /// <param name="time">it require the roundtime in csec (0.1sec) so it require you to pass the time multiplied by RacingGamemode.startTimerCountdown so it will hold also info about the first dec</param>
        /// <param name="totalRoomTime"></param>
        /// <returns></returns>
        public override string GetGamemodeStatusTop()
        {
            float time = FengGameManagerMKII.instance.timeTotalServer - RacingGamemode.StartTimerCountdown;
            if (time > 0)
                return time.ToString("000.0");
            else
                return "Time: WAITING ";
        }

        public override void OnPlayerKilled(int id)
        {
            base.OnPlayerKilled(id);
        }

        public override void OnNetGameWon(int score)
        {
            FengGameManagerMKII.instance.gameEndCD = CalculateGameEndCD();
        }

        public override void CoreUpdate()
        {
            RefreshCountdown -= Time.deltaTime;
            if (RefreshCountdown < 0)
                RefreshCountdown = this.HUDRefreshTime;
            else
                return;

            this.InGameHUD.ShowHUDInfo(HUD.LabelPosition.TopCenter, GetGamemodeStatusTop());

            if (FengGameManagerMKII.instance.roundTime < 20f)
            {
                this.InGameHUD.ShowHUDInfo(HUD.LabelPosition.Center, "RACE START IN " + ((int) (RacingGamemode.StartTimerCountdown - FengGameManagerMKII.instance.roundTime)) +
                                       (!(this.localRacingResult == string.Empty)
                                           ? ("\nLast Round\n" + this.localRacingResult)
                                           : "\n\n"));
            }
            else if (!this.startRacing)
            {
                this.InGameHUD.ShowHUDInfo(HUD.LabelPosition.Center, string.Empty);
                this.startRacing = true;
                this.endRacing = false;
                GameObject.Find("door")?.SetActive(false);
                foreach (var racingDoor in GameObject.FindObjectsOfType<RacingStartBarrier>())
                    racingDoor.gameObject.SetActive(false);
            }

            var myInGameCamera = Camera.main.GetComponent<IN_GAME_MAIN_CAMERA>();


            InGameHUD.ShowHUDInfo(HUD.LabelPosition.TopCenter, GetGamemodeStatusTop() + (Settings.TeamMode != Assets.Scripts.Gamemode.Options.TeamMode.Disabled ? $"\n<color=#00ffff>Cyan: {FengGameManagerMKII.instance.cyanKills}</color><color=#ff00ff>       Magenta: {FengGameManagerMKII.instance.magentaKills}</color>" : ""));
            InGameHUD.ShowHUDInfo(HUD.LabelPosition.TopRight, GetGamemodeStatusTopRight());

            if (this.needChooseSide)
            {
                InGameHUD.ShowHUDInfo(HUD.LabelPosition.TopCenter, "\n\nPRESS 1 TO ENTER GAME", true);
            }
            else if (myInGameCamera.GameOver && !myInGameCamera.IsSpecmode)
            {
                FengGameManagerMKII.instance.myRespawnTime += Time.deltaTime;
                if (FengGameManagerMKII.instance.myRespawnTime > 1.5f)
                {
                    FengGameManagerMKII.instance.myRespawnTime = 0f;
                    myInGameCamera.GameOver = false;
                    if (FengGameManagerMKII.instance.checkpoint != null)
                    {
                        FengGameManagerMKII.instance.StartCoroutine(FengGameManagerMKII.instance.WaitAndRespawn2(0.1f, FengGameManagerMKII.instance.checkpoint));
                    }
                    else
                    {
                        FengGameManagerMKII.instance.StartCoroutine(FengGameManagerMKII.instance.WaitAndRespawn1(0.1f, FengGameManagerMKII.instance.myLastRespawnTag));
                    }
                    InGameHUD.ShowHUDInfo(HUD.LabelPosition.Center, string.Empty);
                }
            }
        }
    }
}