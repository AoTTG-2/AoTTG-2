using Assets.Scripts.Gamemode.Racing;
using Assets.Scripts.Gamemode.Settings;
using Assets.Scripts.UI.Input;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System;
using UnityEngine;
using HUD = Assets.Scripts.UI.InGame.HUD;

namespace Assets.Scripts.Gamemode
{
    public class RacingGamemode : GamemodeBase
    {
        public const int StartTimerCountdown = 20;

        public bool startRacing;

        public bool endRacing;

        private List<RacingResult> racingResult;

        public Vector3 racingSpawnPoint;

        public bool racingSpawnPointSet;

        public string localRacingResult = string.Empty;
        public List<RacingObjective> Objectives = new List<RacingObjective>();

        public sealed override GamemodeSettings Settings { get; set; }
        private RacingSettings GamemodeSettings => Settings as RacingSettings;

        public void Awake()
        {
            //setting the refresh time twice the wanted resolution 
            this.HUDRefreshTime = .05f;

            this.racingResult = new List<RacingResult>();
        }

        public void OnEnable()
        {
            EventManager.OnMainObjectDeath += this.OnDeath;
        }

        public void OnDisable()
        {
            EventManager.OnMainObjectDeath -= this.OnDeath;
        }

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
            FengGameManagerMKII.RPC("netGameWin", PhotonTargets.Others, 0);
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

        [PunRPC]
        private void netRefreshRacingResult(string tmp)
        {
            this.localRacingResult = tmp;
        }

        private void RefreshRacingResult()
        {
            System.Text.StringBuilder tmpLocalRacingResult = new System.Text.StringBuilder("Result\n", 512);
            var newRacingResult = new List<RacingResult>(racingResult.OrderBy(rr => rr.time));
            this.racingResult = newRacingResult;
            int counter = 1;
            foreach(var rr in this.racingResult)
            {
                tmpLocalRacingResult.Append("Rank"+counter+" : ");
                tmpLocalRacingResult.Append(rr.ToString());

                if (counter++ == 10)
                    break;
            }
            for(;counter<=10;counter++)
                tmpLocalRacingResult.Append("Rank" + counter + " : \n");
            this.localRacingResult = tmpLocalRacingResult.ToString();
            base.photonView.RPC("netRefreshRacingResult", PhotonTargets.All, this.localRacingResult);
        }

        [PunRPC]
        private void GetRacingResult(string player, float time, PhotonMessageInfo info)
        {
            RacingResult result = new RacingResult(info.sender.ID, player, time);
            if (this.racingResult.Count(s => s.ID == result.ID) == 0)
            {
                this.racingResult.Add(result);
                this.RefreshRacingResult();
            }
        }

        private void GameWon()
        {
            EventManager.OnGameWon.Invoke();
            //for compatibility
            FengGameManagerMKII.instance.gameWin2();
        }

        public void RacingFinsihEvent()
        {
            localRacingResult = (FengGameManagerMKII.instance.timeTotalServer - RacingGamemode.StartTimerCountdown).ToString("f2");
            float time = FengGameManagerMKII.instance.roundTime - RacingGamemode.StartTimerCountdown;
            FengGameManagerMKII.RPC("GetRacingResult", PhotonTargets.MasterClient, LoginFengKAI.player.name, time);
            this.GameWon();
        }

        public override string GetVictoryMessage(float timeUntilRestart, float totalServerTime = 0f)
        {
            if (PhotonNetwork.offlineMode)
            {
                return $"{localRacingResult}s !!\n Press {InputManager.GetKey(InputUi.Restart)} to Restart.\n\n\n";
            }
            return $"{localRacingResult}\n\nGame Restart in {(int) timeUntilRestart}";
        }

        public override string GetGamemodeStatusTop()
        {
            float time = FengGameManagerMKII.instance.timeTotalServer - RacingGamemode.StartTimerCountdown;
            if (time > 0)
                return time.ToString("000.0");
            else
                return "Time: WAITING ";
        }

        private IEnumerator TimedRespawn()
        {
            yield return new WaitForSeconds(1.5f);
            var myInGameCamera = Camera.main.GetComponent<IN_GAME_MAIN_CAMERA>();

            if (this.needChooseSide || !myInGameCamera.GameOver || myInGameCamera.IsSpecmode)
                yield break;

            myInGameCamera.GameOver = false;
            if (FengGameManagerMKII.instance.checkpoint != null)
                FengGameManagerMKII.instance.StartCoroutine(FengGameManagerMKII.instance.WaitAndRespawn2(0.1f, FengGameManagerMKII.instance.checkpoint));
            else
                FengGameManagerMKII.instance.StartCoroutine(FengGameManagerMKII.instance.WaitAndRespawn1(0.1f, FengGameManagerMKII.instance.myLastRespawnTag));
            InGameHUD.ShowHUDInfo(HUD.LabelPosition.Center, string.Empty);
        }

        private void OnDeath()
        {
            var myInGameCamera = Camera.main.GetComponent<IN_GAME_MAIN_CAMERA>();
            if (!this.needChooseSide && myInGameCamera.GameOver && !myInGameCamera.IsSpecmode)
                StartCoroutine(this.TimedRespawn());
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

            InGameHUD.ShowHUDInfo(HUD.LabelPosition.TopCenter, GetGamemodeStatusTop() + 
                (Settings.TeamMode != Options.TeamMode.Disabled ? $"\n<color=#00ffff>Cyan: {FengGameManagerMKII.instance.cyanKills}</color><color=#ff00ff>       Magenta: {FengGameManagerMKII.instance.magentaKills}</color>" : ""));
            InGameHUD.ShowHUDInfo(HUD.LabelPosition.TopRight, GetGamemodeStatusTopRight());

            if (this.needChooseSide)
            {
                InGameHUD.ShowHUDInfo(HUD.LabelPosition.TopCenter, "\n\nPRESS 1 TO ENTER GAME", true);
            }
        }
    }
}