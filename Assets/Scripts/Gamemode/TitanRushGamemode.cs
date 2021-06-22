using Assets.Scripts.Characters;
using Assets.Scripts.Characters.Titan;
using Assets.Scripts.Characters.Titan.Behavior;
using Assets.Scripts.Extensions;
using Assets.Scripts.Room;
using Assets.Scripts.Services;
using Assets.Scripts.Settings;
using Assets.Scripts.Settings.Gamemodes;
using Assets.Scripts.UI.InGame.HUD;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace Assets.Scripts.Gamemode
{
    //This is the colossal gamemode, where titans "rush" towards a specified endpoint
    public class TitanRushGamemode : GamemodeBase
    {
        public override GamemodeType GamemodeType { get; } = GamemodeType.TitanRush;

        private RushSettings Settings => GameSettings.Gamemode as RushSettings;
        private GameObject[] Routes { get; set; }
        private GameObject[] Spawns { get; set; }
        private List<RushBehavior> SubscribedEvents { get; } = new List<RushBehavior>();

        protected override void Level_OnLevelLoaded(int scene, Level level)
        {
            base.Level_OnLevelLoaded(scene, level);
            nextUpdate = default;
            SubscribedEvents.ForEach(x => x.OnCheckpointArrived -= OnCheckpointArrived);
            SubscribedEvents.Clear();

            GameObject.Find("playerRespawnTrost").SetActive(false);
            Object.Destroy(GameObject.Find("playerRespawnTrost"));
            Object.Destroy(GameObject.Find("rock"));
            if (!PhotonNetwork.isMasterClient) return;
            
            SpawnService.Spawn<ColossalTitan>(-Vector3.up * 10000f, Quaternion.Euler(0f, 180f, 0f), null);
            Routes = GameObject.FindGameObjectsWithTag("route");
            GameObject[] objArray = GameObject.FindGameObjectsWithTag("titanRespawn");
            var spawns = new List<GameObject>();
            foreach (GameObject obj2 in objArray)
            {
                if (obj2.transform.parent.name == "titanRespawnCT")
                {
                    spawns.Add(obj2);
                }
            }

            Spawns = spawns.ToArray();
        }

        protected override void SetStatusTop()
        {
            var content = $"{Localization.Common.GetLocalizedString("TIME")} : {TimeService.GetRoundDisplayTime()}" +
                          $"\n{Localization.Gamemode.Rush.GetLocalizedString("OBJECTIVE_HUMANITY")}";
            UiService.SetMessage(LabelPosition.Top, content);
        }

        protected override void OnEntityUnRegistered(Entity entity)
        {
            if (IsRoundOver || !PhotonNetwork.isMasterClient) return;
            if (entity is ColossalTitan)
            {
                HumanScore++;
                photonView.RPC(nameof(OnGameEndRpc), PhotonTargets.All, 
                    $"{Localization.Gamemode.Rush.GetLocalizedString("VICTORY_HUMANITY")}\n{Localization.Gamemode.Shared.GetLocalizedString("RESTART_COUNTDOWN")}",
                    HumanScore, TitanScore);
            }
        }

        private ArrayList GetRoute()
        {
            GameObject route = Routes[UnityEngine.Random.Range(0, Routes.Length)];
            while (route.name != "routeCT")
            {
                route = Routes[UnityEngine.Random.Range(0, Routes.Length)];
            }

            var checkPoints = new ArrayList();
            for (int i = 1; i <= 10; i++)
            {
                checkPoints.Add(route.transform.Find("r" + i).position);
            }
            checkPoints.Add("end");

            return checkPoints;
        }

        private int nextUpdate = 1;
        public void Update()
        {
            if (!PhotonNetwork.isMasterClient) return;
            if (Time.time < nextUpdate) return;
            nextUpdate = Mathf.FloorToInt(Time.time) + 1;

            if (nextUpdate % Settings.TitanInterval.Value == 0)
            {
                SpawnTitan();
            }

            if (Settings.TitanGroupInterval > 0 && Settings.TitanGroupSize > 0 && nextUpdate % Settings.TitanGroupInterval == 0)
            {
                for (var i = 0; i < Settings.TitanGroupSize; i++)
                {
                    SpawnTitan();
                }
            }
        }

        private void SpawnTitan()
        {
            if (EntityService.Count<MindlessTitan>() >= GameSettings.Titan.Limit.Value) return;
            var configuration = GetTitanConfiguration();
            var route = GetRoute();
            var behavior = new RushBehavior(route);
            behavior.OnCheckpointArrived += OnCheckpointArrived;
            SubscribedEvents.Add(behavior);
            configuration.Behaviors.Add(behavior);
            var spawn = Spawns[Random.Range(0, Spawns.Length)].transform;
            SpawnService.Spawn<MindlessTitan>(spawn.position, spawn.rotation, configuration);
        }

        private void OnCheckpointArrived(Vector3 checkpoint, Entity arriver)
        {
            photonView.RPC(nameof(OnArrivedAtLastCheckpointRpc), PhotonTargets.MasterClient, checkpoint, arriver.photonView.viewID);
        }

        [PunRPC]
        private void OnArrivedAtLastCheckpointRpc(Vector3 checkpoint, int viewId, PhotonMessageInfo info)
        {
            if (!PhotonNetwork.isMasterClient) return;
            if (IsRoundOver) return;
            TitanScore++;
            photonView.RPC(nameof(OnGameEndRpc), PhotonTargets.All,
                $"{Localization.Gamemode.Rush.GetLocalizedString("VICTORY_TITANITY")}\n{Localization.Gamemode.Shared.GetLocalizedString("RESTART_COUNTDOWN")}",
                HumanScore, TitanScore);
        }

    }
}
