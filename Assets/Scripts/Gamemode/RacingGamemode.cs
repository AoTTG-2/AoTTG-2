using Assets.Scripts.Extensions;
using Assets.Scripts.Gamemode.Racing;
using Assets.Scripts.Room;
using Assets.Scripts.Services;
using Assets.Scripts.Settings;
using Assets.Scripts.Settings.Gamemodes;
using Assets.Scripts.UI;
using Assets.Scripts.UI.InGame.HUD;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Gamemode
{
    public class RacingGamemode : GamemodeBase
    {
        public override GamemodeType GamemodeType { get; } = GamemodeType.Racing;

        public List<RacingObjective> Objectives = new List<RacingObjective>();
        public List<RacingStartBarrier> StartBarriers = new List<RacingStartBarrier>();

        private RacingSettings Settings => GameSettings.Gamemode as RacingSettings;
        private const float CountDownTimerLimit = 5f;

        private bool HasStarted { get; set; }

        private float TotalSpeed { get; set; }
        private int TotalFrames { get; set; }
        private float AverageSpeed => TotalSpeed / TotalFrames;

        private bool IsLoaded;
        private bool isValid;

        private List<RacingResult> RacingResults = new List<RacingResult>();
        private static List<RacingResult> OfflineResults = new List<RacingResult>();

        #region Unity Events
        private void Update()
        {
            if (!isValid) return;
            if (HasStarted)
            {
                //TODO Refactor the average speed to be more performance friendly
                var currentSpeed = Camera.main.GetComponent<IN_GAME_MAIN_CAMERA>().main_object?.GetComponent<Rigidbody>().velocity.magnitude ?? 0f;
                TotalSpeed += currentSpeed;
                TotalFrames++;

                UiService.SetMessage(LabelPosition.Top,
                    $"{Localization.Common.GetLocalizedString("TIME")} : {TimeService.GetRoundTime() - CountDownTimerLimit:F1} | " +
                    $"{Localization.Gamemode.Racing.GetLocalizedString("AVERAGE_SPEED")} : {AverageSpeed:F1}");
            }
            else
            {
                UiService.SetMessage(LabelPosition.Center, Localization.Gamemode.Racing.GetLocalizedString("START", $"{CountDownTimerLimit - TimeService.GetRoundTime():F1}"));
                if (CountDownTimerLimit - TimeService.GetRoundTime() <= 0f)
                {
                    UiService.ResetMessage(LabelPosition.Center);
                    if (!PhotonNetwork.offlineMode) RacingResults.Clear();
                    HasStarted = true;
                    if (PhotonNetwork.isMasterClient)
                    {
                        photonView.RPC(nameof(RacingStartRpc), PhotonTargets.AllBufferedViaServer);
                    }
                }
            }
        }

        #endregion

        #region Overrides

        protected override void Level_OnLevelLoaded(int scene, Level level)
        {
            isValid = false;
            IsLoaded = true;
            base.Level_OnLevelLoaded(scene, level);
            StartBarriers = GameObject.FindObjectsOfType<RacingStartBarrier>().ToList();
            HasStarted = false;
            TotalSpeed = 0;
            TotalFrames = 0;

            if (!PhotonNetwork.isMasterClient)
                photonView.RPC(nameof(RequestStatus), PhotonTargets.MasterClient);

            Objectives = FindObjectsOfType<RacingObjective>().OrderBy(x => x.Order).ToList();
            var finish = FindObjectsOfType<LevelTriggerRacingEnd>();
            if (!finish.Any() && !Objectives.Any())
            {
                var message = Localization.Gamemode.Shared.GetLocalizedString("INVALID_GAMEMODE",
                    GamemodeType.ToString());
                Service.Message.Local(message, DebugLevel.Critical);
                Service.Ui.SetMessage(LabelPosition.Top, message);
                isValid = false;
                return;
            }

            isValid = true;

            if (PhotonNetwork.offlineMode) UiService.SetMessage(LabelPosition.TopLeft, GetSinglePlayerStats());
            if (Objectives.Count == 0) return;

            for (int i = 0; i < Objectives.Count; i++)
            {
                Objectives[i].Queue();
                if (i + 1 >= Objectives.Count) continue;
                Objectives[i].NextObjective = Objectives[i + 1];
            }
            Objectives[0].Current();
        }

        protected override void SetStatusTop()
        {
            // Ignored
        }

        protected override void SetStatusTopLeft()
        {
            // Ignore
        }

        protected override void SetStatusTopRight()
        {
            // Ignore
        }

        protected override IEnumerator OnUpdateEverySecond()
        {
            yield break;
        }

        protected override IEnumerator OnUpdateEveryTenthSecond()
        {
            while (true)
            {
                yield return new WaitForSeconds(0.1f);
                SetStatusTopLeft();
            }
        }
        #endregion

        #region Network

        [PunRPC]
        private void RequestStatus(PhotonMessageInfo info)
        {
            if (!PhotonNetwork.isMasterClient) return;
            photonView.RPC(nameof(RacingStartRpc), info.sender);
        }

        [PunRPC]
        private void RacingStartRpc(PhotonMessageInfo info)
        {
            if (!info.sender.IsMasterClient) return;
            StartCoroutine(FixStartBarriers());

            IEnumerator FixStartBarriers()
            {
                if (IsLoaded)
                {
                    yield return new WaitForSeconds(0.05f);
                }

                FindObjectsOfType<RacingStartBarrier>().ToList()
                    .ForEach(x => x.gameObject.SetActive(false));
            }
        }

        [PunRPC]
        public void RacingFinishRpc(string playerName, float time, float averageSpeed, int deaths, PhotonMessageInfo info)
        {
            if (!PhotonNetwork.isMasterClient) return;
            var result = new RacingResult
            {
                Name = playerName,
                Time = time,
                AverageSpeed = averageSpeed,
                Deaths = deaths
            };

            string racingResult;
            if (PhotonNetwork.offlineMode)
            {
                OfflineResults.Add(result);
                racingResult =
                    $"Finished!\n {result.Name}   {result.Time}s ({result.AverageSpeed}us / {result.Deaths})";
            }
            else
            {
                RacingResults.Add(result);
                racingResult = "Result\n";
                int num = Mathf.Min(RacingResults.Count, 10);
                for (int i = 0; i < num; i++)
                {
                    object[] objArray2 = { racingResult, "Rank ", i + 1, " : " };
                    racingResult = string.Concat(objArray2);
                    racingResult += RacingResults[i].Name;
                    racingResult += "   " + ((int) (RacingResults[i].Time * 100f) * 0.01f).ToString(CultureInfo.InvariantCulture) + "s";
                    racingResult += "\n";
                }
            }

            photonView.RPC(nameof(SetRacingResultsRpc), PhotonTargets.All, racingResult);
        }

        [PunRPC]
        public void SetRacingResultsRpc(string data, PhotonMessageInfo info)
        {
            if (!info.sender.IsMasterClient) return;
            UiService.SetMessage(LabelPosition.Center, data);
        }
        #endregion

        public void OnRacingFinished()
        {
            var finishTime = TimeService.GetRoundTime() - CountDownTimerLimit;
            photonView.RPC(nameof(RacingFinishRpc), PhotonTargets.MasterClient, LoginFengKAI.player.name, finishTime, AverageSpeed, 0);
        }

        private string GetSinglePlayerStats()
        {
            if (!OfflineResults.Any()) return string.Empty;
            if (OfflineResults.Count > 10) OfflineResults.RemoveAt(0);

            var stats = new StringBuilder();
            stats.AppendLine("Previous rounds:");
            foreach (var result in OfflineResults)
            {
                stats.AppendLine($"> {result.Time:F2}s  /  {result.AverageSpeed:F2}us  /  {result.Deaths}");
            }
            return stats.ToString();
        }
    }
}