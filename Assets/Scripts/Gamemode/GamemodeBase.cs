using Assets.Scripts.Characters.Titan;
using Assets.Scripts.Gamemode.Options;
using Assets.Scripts.Gamemode.Settings;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Scripts.Gamemode
{
    public abstract class GamemodeBase : MonoBehaviour
    {
        public virtual GamemodeSettings Settings { get; set; }
        public abstract void SetSettings(GamemodeSettings settings);
        private MindlessTitanType GetTitanType()
        {
            if (Settings.CustomTitanRatio)
            {
                var titanTypes = new Dictionary<MindlessTitanType, float>(Settings.TitanTypeRatio);
                foreach (var disabledTitanType in Settings.DisabledTitans)
                {
                    titanTypes.Remove(disabledTitanType);
                }

                var totalRatio = Settings.TitanTypeRatio.Values.Sum();
                var ratioList = new List<KeyValuePair<MindlessTitanType, float>>();
                var ratio = 0f;
                foreach (var titanTypeRatio in titanTypes)
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

            }

            var types = Enum.GetValues(typeof(MindlessTitanType));
            return (MindlessTitanType) types.GetValue(Random.Range(0, types.Length));
        }

        private int GetTitanHealth(float titanSize)
        {
            switch (Settings.TitanHealthMode)
            {
                case TitanHealthMode.Fixed:
                    return Random.Range(Settings.TitanHealthMinimum, Settings.TitanHealthMaximum + 1);
                case TitanHealthMode.Scaled:
                    return Mathf.Clamp(Mathf.RoundToInt(titanSize / 4f * Random.Range(Settings.TitanHealthMinimum, Settings.TitanHealthMaximum + 1)), Settings.TitanHealthMinimum, Settings.TitanHealthMaximum);
                case TitanHealthMode.Disabled:
                    return 0;
                default:
                    throw new ArgumentOutOfRangeException($"Invalid TitanHealthMode enum: {Settings.TitanHealthMode}");
            }
        }

        protected virtual TitanConfiguration GetTitanConfiguration()
        {
            return GetTitanConfiguration(GetTitanType());
        }

        protected virtual TitanConfiguration GetTitanConfiguration(MindlessTitanType type)
        {
            var size = Settings.TitanCustomSize ? Random.Range(Settings.TitanMinimumSize, Settings.TitanMaximumSize) : Random.Range(0.7f, 3f);
            var health = GetTitanHealth(size);
            return new TitanConfiguration(health, 10, 10, 10, size, type);
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
            if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE) return;
        }

        public virtual void OnSpawnTitan()
        {

        }

        public virtual void OnTitanSpawned(TITAN titan)
        {
            if ((IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.MULTIPLAYER) || titan.photonView.isMine)
            {
                if (!titan.hasSetLevel)
                {
                    titan.myLevel = UnityEngine.Random.Range((float)0.7f, (float)3f);
                    if (Settings.TitanCustomSize)
                    {
                        titan.myLevel = UnityEngine.Random.Range(Settings.TitanMinimumSize, Settings.TitanMaximumSize);
                    }
                    titan.hasSetLevel = true;
                }
            }
            if (titan.maxHealth == 0)
            {
                switch (Settings.TitanHealthMode)
                {
                    case TitanHealthMode.Fixed:
                        titan.maxHealth = titan.currentHealth = UnityEngine.Random.Range(Settings.TitanHealthMinimum, Settings.TitanHealthMaximum + 1);
                        break;
                    case TitanHealthMode.Scaled:
                        titan.maxHealth = titan.currentHealth = Mathf.Clamp(Mathf.RoundToInt((titan.myLevel / 4f) * UnityEngine.Random.Range(Settings.TitanHealthMinimum, Settings.TitanHealthMaximum + 1)), Settings.TitanHealthMinimum, Settings.TitanHealthMaximum);
                        break;
                }
            }
        }

        public virtual GameObject GetPlayerSpawnLocation(string tag = "playerRespawn")
        {
            var objArray = GameObject.FindGameObjectsWithTag(tag);
            return objArray[Random.Range(0, objArray.Length)];
        }

        public virtual string GetVictoryMessage(float timeUntilRestart, float totalServerTime = 0f)
        {
            if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
            {
                return "Humanity Win!\n Press " + FengGameManagerMKII.instance.inputManager.inputString[InputCode.restart] + " to Restart.\n\n\n";
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

        public virtual void OnSetTitanType(ref int titanType, bool flag) { }

        public virtual string GetGamemodeStatusTop(int time = 0, int totalRoomTime = 0)
        {
            var content = "Titan Left: ";
            var length = GameObject.FindGameObjectsWithTag("titan").Length;
            content = content + length + "  Time : ";
            if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
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
            if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER)
            {
                var parameters = new object[] { Settings.HumanScore };
                FengGameManagerMKII.instance.photonView.RPC("netGameWin", PhotonTargets.Others, parameters);
                if (((int)FengGameManagerMKII.settings[0xf4]) == 1)
                {
                    //this.chatRoom.addLINE("<color=#FFC000>(" + this.roundTime.ToString("F2") + ")</color> Round ended (game win).");
                }
            }
        }

        public virtual void OnGameLost()
        {
            Settings.TitanScore++;
            if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER)
            {
                var parameters = new object[] { Settings.TitanScore };
                FengGameManagerMKII.instance.photonView.RPC("netGameLose", PhotonTargets.Others, parameters);
                if ((int)FengGameManagerMKII.settings[0xf4] == 1)
                {
                    //FengGameManagerMKII.instance.chatRoom.addLINE("<color=#FFC000>(" + this.roundTime.ToString("F2") + ")</color> Round ended (game lose).");
                }
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

        public virtual GameObject SpawnNonAiTitan(Vector3 position, GameObject randomTitanRespawn)
        {
            return PhotonNetwork.Instantiate("TITAN_VER3.1", position, randomTitanRespawn.transform.rotation, 0);
        }

        internal bool IsAllPlayersDead()
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

        internal bool IsAllTitansDead()
        {
            foreach (GameObject obj2 in GameObject.FindGameObjectsWithTag("titan"))
            {
                if ((obj2.GetComponent<MindlessTitan>() != null) && obj2.GetComponent<MindlessTitan>().TitanState != MindlessTitanState.Dead)
                {
                    return false;
                }
                if (obj2.GetComponent<FEMALE_TITAN>() != null)
                {
                    return false;
                }
            }
            return true;
        }

        public virtual string GetDefeatMessage(float gameEndCd)
        {
            if (Settings.IsSinglePlayer)
            {
                return "Humanity Fail!\n Press " + FengGameManagerMKII.instance.inputManager.inputString[InputCode.restart] + " to Restart.\n\n\n";
            }
            return "Humanity Fail!\nAgain!\nGame Restart in " + ((int) gameEndCd) + "s\n\n";
        }
    }
}
