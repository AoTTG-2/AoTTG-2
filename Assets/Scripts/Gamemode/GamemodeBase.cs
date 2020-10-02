using Assets.Scripts.Characters;
using Assets.Scripts.Characters.Titan;
using Assets.Scripts.Characters.Titan.Attacks;
using Assets.Scripts.Characters.Titan.Configuration;
using Assets.Scripts.Gamemode.Options;
using Assets.Scripts.Services;
using Assets.Scripts.Services.Interface;
using Assets.Scripts.Settings;
using Assets.Scripts.Settings.Gamemodes;
using Assets.Scripts.UI.InGame.HUD;
using Assets.Scripts.UI.Input;
using Photon;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Scripts.Gamemode
{
    public abstract class GamemodeBase : PunBehaviour
    {
        private GamemodeSettings Settings => GameSettings.Gamemode;

        protected IEntityService EntityService => Service.Entity;
        protected IFactionService FactionService => Service.Faction;
        protected IPlayerService PlayerService => Service.Player;
        protected ISpawnService SpawnService => Service.Spawn;
        protected ITimeService TimeService => Service.Time;
        protected IUiService UiService => Service.Ui;

        public int HumanScore { get; set; }
        public int TitanScore { get; set; }

        /// <summary>
        /// A list of Coroutines that will be cleared upon restarting
        /// </summary>
        protected List<Coroutine> Coroutines { get; set; } = new List<Coroutine>();

        protected virtual void OnLevelWasLoaded()
        {
            UiService.ResetMessagesAll();
            Coroutines.ForEach(StopCoroutine);
        }

        private void Start()
        {
            EntityService.OnRegister += OnEntityRegistered;
            EntityService.OnUnRegister += OnEntityUnRegistered;
            FactionService.OnFactionDefeated += OnFactionDefeated;
            StartCoroutine(OnUpdateEverySecond());
            StartCoroutine(OnUpdateEveryTenthSecond());
        }

        private void OnDestroy()
        {
            EntityService.OnRegister -= OnEntityRegistered;
            EntityService.OnUnRegister -= OnEntityUnRegistered;            
            FactionService.OnFactionDefeated -= OnFactionDefeated;
        }

        #region Events and Coroutines
        protected virtual IEnumerator OnUpdateEverySecond()
        {
            while (true)
            {
                yield return new WaitForSeconds(1f);
                SetStatusTop();
                SetStatusTopLeft();
                SetStatusTopRight();
            }
        }

        protected virtual IEnumerator OnUpdateEveryTenthSecond()
        {
            while (true)
            {
                yield return new WaitForSeconds(0.1f);
            }
        }

        protected virtual void OnFactionDefeated(Faction faction) { }
        protected virtual void OnEntityRegistered(Entity entity) { }
        protected virtual void OnEntityUnRegistered(Entity entity) { }
        #endregion

        private MindlessTitanType GetTitanType()
        {
            return GetDefaultTitanType();
        }

        private MindlessTitanType GetTitanTypeFromDictionary(Dictionary<MindlessTitanType, float> titanRatio)
        {
            foreach (var disabledTitanType in GameSettings.Titan.Mindless.Disabled)
            {
                titanRatio.Remove(disabledTitanType);
            }

            var totalRatio = titanRatio.Values.Sum();
            var ratioList = new List<KeyValuePair<MindlessTitanType, float>>();
            var ratio = 0f;
            foreach (var titanTypeRatio in titanRatio)
            {
                ratio += titanTypeRatio.Value / totalRatio;
                ratioList.Add(new KeyValuePair<MindlessTitanType, float>(titanTypeRatio.Key, ratio));
            }

            var randomNumber = Random.Range(0f, 1f);
            foreach (var titanTypeRatio in ratioList)
            {
                if (randomNumber < titanTypeRatio.Value)
                {
                    return titanTypeRatio.Key;
                }
            }

            throw new ArgumentOutOfRangeException("Titan Ratio dictionary is out of range");
        }

        protected MindlessTitanType GetDefaultTitanType()
        {
            return GetTitanTypeFromDictionary(GameSettings.Titan.Mindless.TypeRatio);
        }

        private int GetTitanHealth(float titanSize)
        {
            switch (GameSettings.Titan.Mindless.HealthMode)
            {
                case TitanHealthMode.Fixed:
                    return GameSettings.Titan.Mindless.Health;
                case TitanHealthMode.Hit:
                case TitanHealthMode.Scaled:
                    return Mathf.Clamp(Mathf.RoundToInt(titanSize / 4f * GameSettings.Titan.Mindless.Health), GameSettings.Titan.Mindless.HealthMinimum.Value, GameSettings.Titan.Mindless.HealthMaximum.Value);
                case TitanHealthMode.Disabled:
                    return 0;
                default:
                    throw new ArgumentOutOfRangeException($"Invalid TitanHealthMode enum: {GameSettings.Titan.Mindless.HealthMode}");
            }
        }

        public virtual TitanConfiguration GetPlayerTitanConfiguration()
        {
            var configuration = GetTitanConfiguration();
            if (configuration.Type == MindlessTitanType.Crawler)
            {
                configuration.Attacks = new List<Attack<MindlessTitan>>();
                return configuration;
            }

            configuration.Attacks = new List<Attack<MindlessTitan>>
            {
                new KickAttack(), new SlapAttack(), new SlapFaceAttack(),
                new BiteAttack(), new BodySlamAttack(), new GrabAttack()
            };
            return configuration;
        }

        public virtual TitanConfiguration GetTitanConfiguration()
        {
            return GetTitanConfiguration(GetTitanType());
        }

        public virtual TitanConfiguration GetTitanConfiguration(MindlessTitanType type)
        {
            var size = GameSettings.Titan.Mindless.Size;
            var health = GetTitanHealth(size);
            return new TitanConfiguration(health, 10, 100, 150f, size, type);
        }

        public virtual void OnPlayerKilled(int id)
        {
            if (IsAllPlayersDead())
            {
                FengGameManagerMKII.instance.gameLose2();
            }
        }

        public virtual void OnRestart()
        {
            if (Settings.PointMode > 0)
            {
                for (int i = 0; i < PhotonNetwork.playerList.Length; i++)
                {
                    PhotonPlayer player = PhotonNetwork.playerList[i];
                    ExitGames.Client.Photon.Hashtable propertiesToSet = new ExitGames.Client.Photon.Hashtable();
                    propertiesToSet.Add(PhotonPlayerProperty.kills, 0);
                    propertiesToSet.Add(PhotonPlayerProperty.deaths, 0);
                    propertiesToSet.Add(PhotonPlayerProperty.max_dmg, 0);
                    propertiesToSet.Add(PhotonPlayerProperty.total_dmg, 0);
                    player.SetCustomProperties(propertiesToSet);
                }
            }
            FengGameManagerMKII.instance.restartGame2();
        }
        
        protected void SpawnTitans(int amount)
        {
            SpawnTitans(amount, GetTitanConfiguration);
        }

        protected void SpawnTitans(int amount, Func<TitanConfiguration> titanConfiguration)
        {
            StartCoroutine(SpawnTitan(amount, titanConfiguration));
        }

        private IEnumerator SpawnTitan(int amount, Func<TitanConfiguration> titanConfiguration)
        {
            var spawns = GameObject.FindGameObjectsWithTag("titanRespawn");
            for (var i = 0; i < amount; i++)
            {
                if (EntityService.Count<MindlessTitan>() >= GameSettings.Titan.Limit) break;
                var randomSpawn = spawns[Random.Range(0, spawns.Length)].transform;
                SpawnService.Spawn<MindlessTitan>(randomSpawn.position, randomSpawn.rotation, titanConfiguration.Invoke());
                yield return new WaitForEndOfFrame();
            }
        }
        
        public virtual GameObject GetPlayerSpawnLocation(string tag = "playerRespawn")
        {
            var objArray = GameObject.FindGameObjectsWithTag(tag);
            return objArray[Random.Range(0, objArray.Length)];
        }

        public virtual string GetVictoryMessage(float timeUntilRestart, float totalServerTime = 0f)
        {
            if (PhotonNetwork.offlineMode)
            {
                return $"Humanity Win!\n Press {InputManager.GetKey(InputUi.Restart)} to Restart.\n\n\n";
            }
            return "Humanity Win!\nGame Restart in " + ((int) timeUntilRestart) + "s\n\n";
        }
        
        protected virtual void SetStatusTop()
        {
            var content = $"Enemy left: {FactionService.CountHostile(Service.Player.Self)} | " +
                          $"Friendly left: { FactionService.CountFriendly(Service.Player.Self)} | " +
                          $"Time: {TimeService.GetRoundDisplayTime()}";
            UiService.SetMessage(LabelPosition.Top, content);
        }

        protected virtual void SetStatusTopLeft()
        {
            if (PhotonNetwork.offlineMode)
            {
                var player = PhotonNetwork.player;
                var kills = RCextensions.returnIntFromObject(player.CustomProperties[PhotonPlayerProperty.kills]);
                var deaths = RCextensions.returnIntFromObject(player.CustomProperties[PhotonPlayerProperty.deaths]);
                var maxDamage = RCextensions.returnIntFromObject(player.CustomProperties[PhotonPlayerProperty.max_dmg]);
                var totalDamage = RCextensions.returnIntFromObject(player.CustomProperties[PhotonPlayerProperty.total_dmg]);

                var content = $"Kills: {kills}\n" +
                              $"Deaths: {deaths}\n" +
                              $"Max Damage: {maxDamage}\n" +
                              $"Total Damage: {totalDamage}";
                UiService.SetMessage(LabelPosition.TopLeft, content);
            }
        }

        protected virtual void SetStatusTopRight()
        {
            var context = string.Concat("Humanity ", HumanScore, " : Titan ", TitanScore, " ");
            UiService.SetMessage(LabelPosition.TopRight, context);
        }
        
        public virtual void OnLevelLoaded(Level level, bool isMasterClient = false)
        {
            if (!GameSettings.Gamemode.Supply.Value)
            {
                UnityEngine.Object.Destroy(GameObject.Find("aot_supply"));
            }

            if (GameSettings.Gamemode.LavaMode.Value)
            {
                UnityEngine.Object.Instantiate(Resources.Load("levelBottom"), new Vector3(0f, -29.5f, 0f), Quaternion.Euler(0f, 0f, 0f));
                var lavaSupplyStation = GameObject.Find("aot_supply_lava_position");
                var supplyStation = GameObject.Find("aot_supply");
                if (lavaSupplyStation == null || supplyStation == null) return;
                supplyStation.transform.position = lavaSupplyStation.transform.position;
                supplyStation.transform.rotation = lavaSupplyStation.transform.rotation;
            }
        }

        [PunRPC]
        public virtual void OnGameEndRpc(string raw, int humanScore, int titanScore, PhotonMessageInfo info)
        {
            if (!info.sender.IsMasterClient) return;
            HumanScore = humanScore;
            TitanScore = titanScore;
            Coroutines.Add(StartCoroutine(GameEndingCountdown(raw)));
        }

        private IEnumerator GameEndingCountdown(string raw)
        {
            var totalTime = 10f;
            UiService.SetMessage(LabelPosition.Center, string.Format(raw, totalTime));
            while (totalTime >= 0)
            {
                yield return new WaitForSeconds(1f);
                totalTime--;
                UiService.SetMessage(LabelPosition.Center, string.Format(raw, totalTime));
            }

            if (PhotonNetwork.isMasterClient)
            {
                FengGameManagerMKII.instance.restartRC();
            }
        }
        
        protected bool IsAllPlayersDead()
        {
            var num = 0;
            var num2 = 0;
            foreach (var player in PhotonNetwork.playerList)
            {
                if (RCextensions.returnIntFromObject(player.CustomProperties[PhotonPlayerProperty.isTitan]) != 1) continue;
                num++;
                if (RCextensions.returnBoolFromObject(player.CustomProperties[PhotonPlayerProperty.dead]))
                {
                    num2++;
                }
            }
            return (num == num2);
        }


        protected bool IsAllTitansDead()
        {
            foreach (GameObject obj2 in GameObject.FindGameObjectsWithTag("titan"))
            {
                if ((obj2.GetComponent<MindlessTitan>() != null) && obj2.GetComponent<MindlessTitan>().State != TitanState.Dead)
                {
                    return false;
                }
                if (obj2.GetComponent<FemaleTitan>() != null)
                {
                    return false;
                }
            }
            return true;
        }

        public virtual string GetDefeatMessage(float gameEndCd)
        {
            if (PhotonNetwork.offlineMode)
            {
                return $"Humanity Fail!\n Press {InputManager.GetKey(InputUi.Restart)} to Restart.\n\n\n";
            }
            return "Humanity Fail!\nAgain!\nGame Restart in " + ((int) gameEndCd) + "s\n\n";
        }
    }
}
