using Assets.Scripts.Characters.Titan;
using Assets.Scripts.Characters.Titan.Attacks;
using Assets.Scripts.Gamemode.Options;
using Assets.Scripts.Settings;
using Assets.Scripts.Settings.Gamemodes;
using Assets.Scripts.UI.Input;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using MonoBehaviour = Photon.MonoBehaviour;
using Random = UnityEngine.Random;

namespace Assets.Scripts.Gamemode
{
    public abstract class GamemodeBase : MonoBehaviour
    {
        public abstract GamemodeSettings Settings { get; set; }
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
                configuration.Attacks = new List<Attack>();
                return configuration;
            }

            configuration.Attacks = new List<Attack>
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
            FengGameManagerMKII.instance.gameEndCD = 0f;
            FengGameManagerMKII.instance.restartGame2();
        }

        public virtual void OnUpdate(float interval) { }

        public static GamemodeSettings ConvertToGamemode(string json, GamemodeType type)
        {
            switch (type)
            {
                case GamemodeType.Racing:
                    return JsonConvert.DeserializeObject<RacingSettings>(json);
                case GamemodeType.Capture:
                    return JsonConvert.DeserializeObject<CaptureGamemodeSettings>(json);
                case GamemodeType.Titans:
                    return JsonConvert.DeserializeObject<KillTitansSettings>(json);
                case GamemodeType.Endless:
                    return JsonConvert.DeserializeObject<EndlessSettings>(json);
                case GamemodeType.Wave:
                    return JsonConvert.DeserializeObject<WaveGamemodeSettings>(json);
                case GamemodeType.Trost:
                    return JsonConvert.DeserializeObject<TrostSettings>(json);
                case GamemodeType.TitanRush:
                    return JsonConvert.DeserializeObject<RushSettings>(json);
                case GamemodeType.PvpAhss:
                    return JsonConvert.DeserializeObject<PvPAhssSettings>(json);
                case GamemodeType.Infection:
                    return JsonConvert.DeserializeObject<InfectionGamemodeSettings>(json);
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }

        public virtual void OnPlayerSpawned(GameObject player)
        {
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
                if (FengGameManagerMKII.instance.getTitans().Count >= GameSettings.Titan.Limit) break;
                var randomSpawn = spawns[Random.Range(0, spawns.Length)];
                FengGameManagerMKII.instance.SpawnTitan(randomSpawn.transform.position, randomSpawn.transform.rotation, titanConfiguration.Invoke());
                yield return new WaitForEndOfFrame();
            }
        }

        public virtual void OnTitanSpawned(MindlessTitan titan)
        {
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
            return "Humanity Win!\nGame Restart in " + ((int)timeUntilRestart) + "s\n\n";
        }

        public virtual void OnTitanKilled(string titanName)
        {
            if (Settings.RestartOnTitansKilled && IsAllTitansDead())
            {
                OnAllTitansDead();
            }
        }

        public virtual string GetGamemodeStatusTop(int time = 0, int totalRoomTime = 0)
        {
            var content = "Titan Left: ";
            var length = GameObject.FindGameObjectsWithTag("titan").Length;
            content = content + length + "  Time : ";
            if (PhotonNetwork.offlineMode)
            {
                length = time;
                content += length.ToString();
            }
            else
            {
                length = totalRoomTime - (time);
                content += length.ToString();
            }

            return content;
        }

        public virtual string GetGamemodeStatusTopRight(int time = 0, int totalRoomTime = 0)
        {
            return string.Concat("Humanity ", Settings.HumanScore, " : Titan ", Settings.TitanScore, " ");

        }

        public virtual string GetRoundEndedMessage()
        {
            return $"Humanity {Settings.HumanScore} : Titan {Settings.TitanScore}";
        }

        public virtual void OnAllTitansDead() { }

        public virtual void OnLevelLoaded(Level level, bool isMasterClient = false)
        {
            if (!Settings.Supply)
            {
                UnityEngine.Object.Destroy(GameObject.Find("aot_supply"));
            }

            if (Settings.LavaMode)
            {
                UnityEngine.Object.Instantiate(Resources.Load("levelBottom"), new Vector3(0f, -29.5f, 0f), Quaternion.Euler(0f, 0f, 0f));
                var lavaSupplyStation = GameObject.Find("aot_supply_lava_position");
                var supplyStation = GameObject.Find("aot_supply");
                if (lavaSupplyStation == null || supplyStation == null) return;
                supplyStation.transform.position = lavaSupplyStation.transform.position;
                supplyStation.transform.rotation = lavaSupplyStation.transform.rotation;
            }
        }

        public virtual void OnGameWon()
        {
            Settings.HumanScore++;
            FengGameManagerMKII.instance.gameEndCD = FengGameManagerMKII.instance.gameEndTotalCDtime;
            var parameters = new object[] { Settings.HumanScore };
            FengGameManagerMKII.instance.photonView.RPC("netGameWin", PhotonTargets.Others, parameters);
            if (((int) FengGameManagerMKII.settings[0xf4]) == 1)
            {
                //this.chatRoom.addLINE("<color=#FFC000>(" + this.roundTime.ToString("F2") + ")</color> Round ended (game win).");
            }
        }

        public virtual void OnGameLost()
        {
            Settings.TitanScore++;
            var parameters = new object[] { Settings.TitanScore };
            FengGameManagerMKII.instance.photonView.RPC("netGameLose", PhotonTargets.Others, parameters);
            if ((int) FengGameManagerMKII.settings[0xf4] == 1)
            {
                //FengGameManagerMKII.instance.chatRoom.addLINE("<color=#FFC000>(" + this.roundTime.ToString("F2") + ")</color> Round ended (game lose).");
            }
        }
        
        public virtual void OnNetGameLost(int score)
        {
            Settings.TitanScore = score;
        }

        public virtual void OnNetGameWon(int score)
        {
            Settings.HumanScore = score;
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
                if ((obj2.GetComponent<MindlessTitan>() != null) && obj2.GetComponent<MindlessTitan>().TitanState != MindlessTitanState.Dead)
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
