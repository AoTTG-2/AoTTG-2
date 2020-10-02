using Assets.Scripts.Characters.Titan;
using Assets.Scripts.Characters.Titan.Behavior;
using Assets.Scripts.Settings;
using Assets.Scripts.Settings.Gamemodes;
using Assets.Scripts.UI.Elements;
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

        [UiElement("Titan frequency", "1 titan will spawn per Interval", SettingCategory.Advanced)]
        public int TitanInterval { get; set; } = 7;

        public override void OnLevelLoaded(Level level, bool isMasterClient = false)
        {
            base.OnLevelLoaded(level, isMasterClient);
            GameObject.Find("playerRespawnTrost").SetActive(false);
            Object.Destroy(GameObject.Find("playerRespawnTrost"));
            Object.Destroy(GameObject.Find("rock"));
            if (!isMasterClient) return;
            //if (IsAllPlayersDead()) return;
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

            if (nextUpdate % TitanInterval != 0) return;
            SpawnTitan();
        }

        private void SpawnTitan()
        {
            if (EntityService.Count<MindlessTitan>() >= GameSettings.Titan.Limit.Value) return;
            var configuration = GetTitanConfiguration();
            var route = GetRoute();
            configuration.Behaviors.Add(new RushBehavior(route));
            var spawn = Spawns[Random.Range(0, Spawns.Length)].transform;
            SpawnService.Spawn<MindlessTitan>(spawn.position, spawn.rotation, configuration);
        }

    }
}
