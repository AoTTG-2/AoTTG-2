using Assets.Scripts.Characters.Humans;
using Assets.Scripts.Extensions;
using Assets.Scripts.Services;
using Assets.Scripts.UI.InGame.HUD;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Room;
using Assets.Scripts.Settings;
using Assets.Scripts.Settings.Game.Gamemodes;
using Assets.Scripts.UI;
using UnityEngine;

namespace Assets.Scripts.Gamemode.Catch
{
    public class CatchGamemode : GamemodeBase
    {
        public override GamemodeType GamemodeType { get; } = GamemodeType.Catch;
        private CatchGamemodeSetting Settings => Setting.Gamemode as CatchGamemodeSetting;

        private CatchSpawner[] Spawners = { };
        private HashSet<CatchBall> CatchBalls = new HashSet<CatchBall>();
        private int BallsCaught = 0;

        protected override void Level_OnLevelLoaded(int scene, Level level)
        {
            IsValid = PhotonNetwork.offlineMode;
            base.Level_OnLevelLoaded(scene, level);
            if (!IsValid)
            {
                var message = Localization.Gamemode.Shared.GetLocalizedString("INVALID_GAMEMODE_MULTIPLAYER_NOTSUPPORTED",
                    GamemodeType.ToString());
                Service.Message.Local(message, DebugLevel.Critical);
                Service.Ui.SetMessage(LabelPosition.Top, message);
                return;
            }

            Spawners = GameObject.FindObjectsOfType<CatchSpawner>();
            BallsCaught = 0;

            for (var i = 0; i < Settings.TotalBalls; i++)
            {
                SpawnBall();
            }

            SetStatusTop();
        }

        private (Vector3 position, Quaternion rotation) GetCatchBallSpawnLocation()
        {
            if (Spawners.Length == 0)
            {
                return (new Vector3(0, 250, 0), new Quaternion());
            }

            var spawner = Spawners[Random.Range(0, Spawners.Length)];
            return (spawner.transform.position, spawner.transform.rotation);
        }

        private void SpawnBall()
        {
            var (pos, rot) = GetCatchBallSpawnLocation();
            var ball = PhotonNetwork.Instantiate("Gamemode/CatchBall", pos, rot, 0).GetComponent<CatchBall>();
            ball.OnCaught += Ball_OnCaught;
            CatchBalls.Add(ball);
        }

        private void Ball_OnCaught(CatchBall ball, Hero hero)
        {
            BallsCaught++;
            CatchBalls.Remove(ball);
            ball.OnCaught -= Ball_OnCaught;
            PhotonNetwork.Destroy(ball.gameObject);
            if (Settings.Endless == true)
            {
                SpawnBall();
            } else if (CatchBalls.Count == 0)
            {
                photonView.RPC(nameof(OnGameEndRpc), PhotonTargets.All, $"All balls are caught!\nRestarting in {{0}}s", HumanScore, TitanScore);
                Camera.main.GetComponent<IN_GAME_MAIN_CAMERA>().gameOver = true;
            }
        }

        protected override void SetStatusTop()
        {
            if (IsValid)
            {
                if (Settings.Endless == true)
                {
                    UiService.SetMessage(LabelPosition.Top, Localization.Gamemode.Catch.GetLocalizedString("OFFLINE_TOP_ENDLESS", BallsCaught));
                }
                else
                {
                    UiService.SetMessage(LabelPosition.Top, Localization.Gamemode.Catch.GetLocalizedString("OFFLINE_TOP", CatchBalls.Count));
                }
            }
            else
            {
                UiService.SetMessage(LabelPosition.Top, Localization.Gamemode.Shared.GetLocalizedString("INVALID_GAMEMODE_MULTIPLAYER_NOTSUPPORTED", GamemodeType.ToString()));
            }
        }

        protected override IEnumerator OnUpdateEverySecond()
        {
            while (true)
            {
                yield return new WaitForSeconds(1f);
                SetStatusTop();
            }
        }
    }
}