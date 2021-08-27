using Assets.Scripts.Characters.Humans;
using Assets.Scripts.Extensions;
using Assets.Scripts.Services;
using Assets.Scripts.UI.InGame.HUD;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Room;
using UnityEngine;

namespace Assets.Scripts.Gamemode.Catch
{
    public class CatchGamemode : GamemodeBase
    {
        public override GamemodeType GamemodeType { get; } = GamemodeType.Catch;
        public int TotalBalls = 5;
        public static float BallSpeed = 150f;
        public static float BallSize = 25;
        public int PointLimit = 5;

        private CatchSpawner[] Spawners = { };
        private HashSet<CatchBall> CatchBalls = new HashSet<CatchBall>();

        protected override void Level_OnLevelLoaded(int scene, Level level)
        {
            base.Level_OnLevelLoaded(scene, level);
            Spawners = GameObject.FindObjectsOfType<CatchSpawner>();

            for (int i = 0; i < TotalBalls; i++)
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
            CatchBalls.Remove(ball);
            ball.OnCaught -= Ball_OnCaught;
            Destroy(ball.gameObject);
            SpawnBall();
        }

        protected override void SetStatusTop()
        {
            if (PhotonNetwork.offlineMode)
                UiService.SetMessage(LabelPosition.Top, Localization.Gamemode.Catch.GetLocalizedString("OFFLINE_TOP"));
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