using Assets.Scripts.Characters;
using Assets.Scripts.Characters.Titan;
using Assets.Scripts.Characters.Titan.Attacks;
using Assets.Scripts.Characters.Titan.Configuration;
using Assets.Scripts.Room;
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
using Assets.Scripts.Extensions;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Scripts.Gamemode
{
    public abstract class GamemodeBase : PunBehaviour
    {
        /// <summary>
        /// An ENUM for the current gamemode
        /// </summary>
        public abstract GamemodeType GamemodeType { get; }

        private GamemodeSettings Settings => GameSettings.Gamemode;

        protected IEntityService EntityService => Service.Entity;
        protected IFactionService FactionService => Service.Faction;
        protected IPlayerService PlayerService => Service.Player;
        protected ISpawnService SpawnService => Service.Spawn;
        protected ITimeService TimeService => Service.Time;
        protected IUiService UiService => Service.Ui;

        /// <summary>
        /// The current score of the Humanity faction
        /// </summary>
        public int HumanScore { get; set; }
        /// <summary>
        /// The current score of the Titanity faction
        /// </summary>
        public int TitanScore { get; set; }

        /// <summary>
        /// A list of Coroutines that will be cleared upon restarting
        /// </summary>
        protected List<Coroutine> Coroutines { get; set; } = new List<Coroutine>();

        /// <summary>
        /// True if the round has ended
        /// </summary>
        protected bool IsRoundOver { get; private set; }

        protected virtual void Level_OnLevelLoaded(int scene, Level level)
        {
            IsRoundOver = false;
            UiService.ResetMessagesAll();
            Coroutines.ForEach(StopCoroutine);

            if (!GameSettings.Gamemode.Supply.Value)
            {
                Destroy(GameObject.Find("aot_supply"));
            }

            if (GameSettings.Gamemode.LavaMode.Value)
            {
                Instantiate(Resources.Load("levelBottom"), new Vector3(0f, -29.5f, 0f), Quaternion.Euler(0f, 0f, 0f));
                var lavaSupplyStation = GameObject.Find("aot_supply_lava_position");
                var supplyStation = GameObject.Find("aot_supply");
                if (lavaSupplyStation == null || supplyStation == null) return;
                supplyStation.transform.position = lavaSupplyStation.transform.position;
                supplyStation.transform.rotation = lavaSupplyStation.transform.rotation;
            }
        }

        private void Awake()
        {
            Service.Level.OnLevelLoaded += Level_OnLevelLoaded;
            EntityService.OnRegister += OnEntityRegistered;
            EntityService.OnUnRegister += OnEntityUnRegistered;
            FactionService.OnFactionDefeated += OnFactionDefeated;
            StartCoroutine(OnUpdateEverySecond());
            StartCoroutine(OnUpdateEveryTenthSecond());
        }

        //private void Start()
        //{
        //    EntityService.OnRegister += OnEntityRegistered;
        //    EntityService.OnUnRegister += OnEntityUnRegistered;
        //    FactionService.OnFactionDefeated += OnFactionDefeated;
        //    StartCoroutine(OnUpdateEverySecond());
        //    StartCoroutine(OnUpdateEveryTenthSecond());
        //}

        private void OnDestroy()
        {
            EntityService.OnRegister -= OnEntityRegistered;
            EntityService.OnUnRegister -= OnEntityUnRegistered;
            FactionService.OnFactionDefeated -= OnFactionDefeated;
            Service.Level.OnLevelLoaded -= Level_OnLevelLoaded;
            StopAllCoroutines();
        }

        public override void OnDisconnectedFromPhoton()
        {
            Destroy(this);
        }

        #region Events and Coroutines
        /// <summary>
        /// An IEnumerator which runs every second
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// An IEnumerator which runs every tenth of a second (0.1s)
        /// </summary>
        /// <returns></returns>
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
                new BiteAttack(), new BodySlamAttack(), new GrabAttack(),
                new ComboAttack()
            };
            return configuration;
        }

        public virtual TitanConfiguration GetTitanConfiguration()
        {
            return GetTitanConfiguration(GetTitanType());
        }

        public virtual TitanConfiguration GetTitanConfiguration(MindlessTitanType type)
        {
            return new TitanConfiguration(10, 100, 150f, type);
        }

        public virtual void OnRestart()
        {
            if (PhotonNetwork.isMasterClient)
                PhotonNetwork.RemoveRPCs(photonView);
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
            Coroutines.Add(StartCoroutine(SpawnTitan(amount, titanConfiguration)));
        }

        protected virtual (Vector3 position, Quaternion rotation) GetSpawnLocation()
        {
            //TODO: Remove this once classic maps no longer rely on this.
            var spawns = GameObject.FindGameObjectsWithTag("titanRespawn").Select(x => (x.transform.position, x.transform.rotation)).ToList();
            if (!spawns.Any())
            {
                spawns = Service.Spawn.GetAll<TitanSpawner>().Select(x => (x.transform.position, x.transform.rotation))
                    .ToList();
            }

            return spawns.Any() ? spawns[Random.Range(0, spawns.Count)] : Service.Spawn.GetRandomSpawnPosition();
        }

        private IEnumerator SpawnTitan(int amount, Func<TitanConfiguration> titanConfiguration)
        {
            var spawns = GameObject.FindGameObjectsWithTag("titanRespawn").Select(x => (x.transform.position, x.transform.rotation)).ToList();
            if (!spawns.Any())
            {
                spawns = Service.Spawn.GetAll<TitanSpawner>().Select(x => (x.transform.position, x.transform.rotation))
                    .ToList();
            }

            if (spawns.Any())
            {
                for (var i = 0; i < amount; i++)
                {
                    if (EntityService.Count<MindlessTitan>() >= GameSettings.Titan.Limit) break;
                    var randomSpawn = spawns[Random.Range(0, spawns.Count)];
                    SpawnService.Spawn<MindlessTitan>(randomSpawn.position, randomSpawn.rotation, titanConfiguration.Invoke());
                    yield return new WaitForEndOfFrame();
                }
            }
            else
            {
                for (var i = 0; i < amount; i++)
                {
                    if (EntityService.Count<MindlessTitan>() >= GameSettings.Titan.Limit) break;
                    var randomSpawn = Service.Spawn.GetRandomSpawnPosition();
                    SpawnService.Spawn<MindlessTitan>(randomSpawn.position, randomSpawn.rotation, titanConfiguration.Invoke());
                    yield return new WaitForEndOfFrame();
                }
            }
        }

        public virtual GameObject GetPlayerSpawnLocation(string tag = "playerRespawn")
        {
            var objArray = GameObject.FindGameObjectsWithTag(tag);
            if (objArray.Length == 0) return null;
            return objArray[Random.Range(0, objArray.Length)];
        }

        [Obsolete]
        public virtual string GetVictoryMessage(float timeUntilRestart, float totalServerTime = 0f)
        {
            var winLocalizated = $"{Localization.Gamemode.Shared.GetLocalizedString("VICTORY", Localization.Common.GetLocalizedString("HUMANITY"))}";
            if (PhotonNetwork.offlineMode)
            {
                return $"{winLocalizated}\n {Localization.Gamemode.Shared.GetLocalizedString("RESTART_OFFLINE", InputManager.GetKey(InputUi.Restart).ToString())}\n\n\n";
            }
            return "Humanity Win!\nGame Restart in " + ((int) timeUntilRestart) + "s\n\n";
        }

        protected virtual void SetStatusTop()
        {
            var content = $"{Localization.Gamemode.Shared.GetLocalizedString("ENEMY_LEFT", FactionService.CountHostile(Service.Player.Self))} | " +
                          $"{Localization.Gamemode.Shared.GetLocalizedString("FRIENDLY_LEFT", FactionService.CountFriendly(Service.Player.Self))} | " +
                          $"{Localization.Common.GetLocalizedString("TIME")}: {TimeService.GetRoundDisplayTime()}";
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

                var content =
                    Localization.Gamemode.Shared.GetLocalizedString("OFFLINE_STATS", kills, deaths, maxDamage, totalDamage);
                UiService.SetMessage(LabelPosition.TopLeft, content);
            }
        }

        protected virtual void SetStatusTopRight()
        {
            var context = string.Concat($"{Localization.Common.GetLocalizedString("HUMANITY")} ", HumanScore, $" : {Localization.Common.GetLocalizedString("TITANITY")} ", TitanScore, " ");
            UiService.SetMessage(LabelPosition.TopRight, context);
        }

        [PunRPC]
        public virtual void OnGameEndRpc(string raw, int humanScore, int titanScore, PhotonMessageInfo info)
        {
            if (!info.sender.IsMasterClient || IsRoundOver) return;
            IsRoundOver = true;
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
                FengGameManagerMKII.instance.RestartRound();
            }
        }
    }
}
