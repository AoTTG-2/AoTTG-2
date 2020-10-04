using Assets.Scripts.Characters;
using Assets.Scripts.Characters.Titan;
using Assets.Scripts.Characters.Titan.Behavior;
using Assets.Scripts.Settings;
using Assets.Scripts.Settings.Gamemodes;
using Assets.Scripts.UI.InGame.HUD;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Gamemode
{
    //This is the colossal gamemode, where titans "rush" towards a specified endpoint
    public class TitanRushGamemode : GamemodeBase
    {
        private RushSettings Settings => GameSettings.Gamemode as RushSettings;
        private GameObject[] Routes { get; set; }
        private GameObject[] Spawns { get; set; }
        private List<RushBehavior> SubscribedEvents { get; } = new List<RushBehavior>();

        protected override void OnLevelWasLoaded()
        {
            base.OnLevelWasLoaded();
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
            var content = $"Time : {TimeService.GetRoundDisplayTime()}" +
                          $"\nDefeat the Colossal Titan.\nPrevent abnormal titan from running to the north gate";
            UiService.SetMessage(LabelPosition.Top, content);
        }

        protected override void OnEntityUnRegistered(Entity entity)
        {
            if (IsRoundOver || !PhotonNetwork.isMasterClient) return;
            if (entity is ColossalTitan)
            {
                HumanScore++;
                photonView.RPC(nameof(OnGameEndRpc), PhotonTargets.All, $"The colossal titan has been defeated!\nRestarting in {{0}}s", HumanScore, TitanScore);
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
            if (Time.time < nextUpdate) return;
            nextUpdate = Mathf.FloorToInt(Time.time) + 1;

            if (nextUpdate % Settings.TitanInterval.Value != 0) return;
            SpawnTitan();
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
            photonView.RPC(nameof(OnGameEndRpc), PhotonTargets.All, $"The civilians have died!\nRestarting in {{0}}s", HumanScore, TitanScore);
        }

    }
}
