using Assets.Scripts.Gamemode.Racing;
using Assets.Scripts.Settings;
using Assets.Scripts.Settings.Gamemodes;
using Assets.Scripts.UI.InGame.HUD;
using Assets.Scripts.UI.Input;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace Assets.Scripts.Gamemode
{
    public class RacingGamemode : GamemodeBase
    {
        public override GamemodeType GamemodeType { get; } = GamemodeType.Racing;

        public string localRacingResult = string.Empty;
        public List<RacingObjective> Objectives = new List<RacingObjective>();
        public List<RacingStartBarrier> StartBarriers = new List<RacingStartBarrier>();

        private RacingSettings Settings => GameSettings.Gamemode as RacingSettings;
        private const float CountDownTimerLimit = 20f;

        private bool HasStarted { get; set; }

        private float TotalSpeed { get; set; }
        private int TotalFrames { get; set; }
        private float AverageSpeed => TotalSpeed / TotalFrames;
        
        private bool IsLoaded;

        protected override void OnLevelWasLoaded()
        {
            Debug.Log(IsLoaded);
            IsLoaded = true;
            base.OnLevelWasLoaded();
            StartBarriers = GameObject.FindObjectsOfType<RacingStartBarrier>().ToList();
            HasStarted = false;
            TotalSpeed = 0;
            TotalFrames = 0;

            if (Objectives.Count == 0) return;
            Objectives = Objectives.OrderBy(x => x.Order).ToList();
            for (int i = 0; i < Objectives.Count; i++)
            {
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

        private void Update()
        {
            if (HasStarted)
            {
                //TODO Refactor the average speed to be more performance friendly
                var currentSpeed = Camera.main.GetComponent<IN_GAME_MAIN_CAMERA>().main_object?.GetComponent<Rigidbody>().velocity.magnitude ?? 0f;
                TotalSpeed += currentSpeed;
                TotalFrames++;

                UiService.SetMessage(LabelPosition.Top, $"Time: {TimeService.GetRoundTime() - CountDownTimerLimit:F1} | " +
                                                        $"Average Speed: {AverageSpeed:F1}");
            }
            else
            {
                UiService.SetMessage(LabelPosition.Center, $"RACE START IN {CountDownTimerLimit - TimeService.GetRoundTime():F1}s");
                if (CountDownTimerLimit - TimeService.GetRoundTime() <= 0f)
                {
                    HasStarted = true;
                    if (PhotonNetwork.isMasterClient)
                    {
                        photonView.RPC(nameof(RacingStartRpc), PhotonTargets.AllBufferedViaServer);
                    }
                }
            }
        }

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
        }

        private IEnumerator FixStartBarriers()
        {
            if (IsLoaded)
            {
                yield return new WaitForSeconds(0.05f);
            }

            FindObjectsOfType<RacingStartBarrier>().ToList()
                .ForEach(x => x.gameObject.SetActive(false));
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
    }
}