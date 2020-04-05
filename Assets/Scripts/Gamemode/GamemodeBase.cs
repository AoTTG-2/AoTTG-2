using Assets.Scripts.Settings;
using Assets.Scripts.UI.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Scripts.Gamemode
{
    public abstract class GamemodeBase
    {
        public GamemodeType GamemodeType;

        private string name;
        public string Name
        {
            get { return name ?? GamemodeType.ToString(); }
            set { name = value; }
        }

        public string Description;

        public List<TitanType> DisabledTitanTypes { get; set; } = new List<TitanType>();

        [UiElement("Start Titans", "The amount of titans that will spawn at the start", SettingCategory.Titans)]
        public int Titans { get; set; } = 25;

        [UiElement("Titan Limit", "The max amount of titans", SettingCategory.Titans)]
        public int TitanLimit { get; set; } = 30;

        [UiElement("Titan Chase Distance", "", SettingCategory.Titans)]
        public float TitanChaseDistance { get; set; } = 100f;

        [UiElement("Enable Titan Chase Distance", "", SettingCategory.Titans)]
        public bool TitanChaseDistanceEnabled { get; set; } = true;

        [UiElement("Spawn Titans on FT Defeat", "Should titans spawn when the Female Titan is killed?", SettingCategory.Advanced)]
        public bool SpawnTitansOnFemaleTitanDefeat { get; set; } = true;

        [UiElement("Female Titan Despawn Time", "How long (in seconds), will the FT be on the map after dying?", SettingCategory.Advanced)]
        public float FemaleTitanDespawnTimer { get; set; } = 5f;
        public float FemaleTitanHealthModifier = 1f;

        //LevelInfo attributes
        public bool Hint;

        [UiElement("Horses", "Enables/Disables horses in the game")]
        public bool Horse { get; set; }

        [UiElement("Lava mode", "The floor is lava! Touching the floor means that you will die...")]
        public bool LavaMode { get; set; }
        public bool Crawlers;
        public bool Punks = true;

        [UiElement("PvP", "Can players kill each other?")]
        public bool Pvp { get; set; } = true;

        [UiElement("Respawn Mode", "The Respawn mode", Category = SettingCategory.Respawn)]
        public RespawnMode RespawnMode { get; set; } = RespawnMode.DEATHMATCH;

        public bool Supply { get; set; } = true;
        public bool AllowPlayerTitans;
        public bool IsPlayerTitanEnabled;

        public int HumanScore = 0;
        public int TitanScore = 0;

        public float RespawnTime = 5f;
        public bool AhssAirReload = true;
        public bool PlayerTitanShifters = true;

        public bool RestartOnTitansKilled = true;

        public int Difficulty = 1;

        public bool IsSinglePlayer = IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE;
        public virtual void OnPlayerKilled(int id)
        {
            if (IsAllPlayersDead())
            {
                FengGameManagerMKII.instance.gameLose2();
            }
        }

        public static GamemodeBase ConvertToGamemode(string json, GamemodeType type)
        {
            GamemodeBase gamemode = null;
            switch (type)
            {
                case GamemodeType.Racing:
                    gamemode = JsonUtility.FromJson<RacingGamemode>(json);
                    break;
                case GamemodeType.Capture:
                    gamemode = JsonUtility.FromJson<CaptureGamemode>(json);
                    break;
                case GamemodeType.Titans:
                    gamemode = JsonUtility.FromJson<KillTitansGamemode>(json);
                    break;
                case GamemodeType.Endless:
                    gamemode = JsonUtility.FromJson<EndlessGamemode>(json);
                    break;
                case GamemodeType.Wave:
                    gamemode = JsonUtility.FromJson<WaveGamemode>(json);
                    break;
                case GamemodeType.Trost:
                    gamemode = JsonUtility.FromJson<TrostGamemode>(json);
                    break;
                case GamemodeType.TitanRush:
                    gamemode = JsonUtility.FromJson<TitanRushGamemode>(json);
                    break;
                case GamemodeType.PvpAhss:
                    gamemode = JsonUtility.FromJson<PvPAhssGamemode>(json);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
            return gamemode;
        }

        public virtual void OnPlayerSpawned(GameObject player)
        {
            if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE) return;

        }

        public bool IsEnabled(TitanType titanType)
        {
            return DisabledTitanTypes.All(x => x != titanType);
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

        public virtual void OnTitanKilled(string titanName, bool onPlayerLeave)
        {
            if (RestartOnTitansKilled && IsAllTitansDead())
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
            return string.Concat("Humanity ", HumanScore, " : Titan ", TitanScore, " ");

        }

        public virtual string GetRoundEndedMessage()
        {
            return $"Humanity {HumanScore} : Titan {TitanScore}";
        }

        public virtual void OnAllTitansDead() { }

        public virtual void OnLevelWasLoaded(Level level, bool isMasterClient = false)
        {
            if (!Supply)
            {
                UnityEngine.Object.Destroy(GameObject.Find("aot_supply"));
            }

            if (LavaMode)
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
            FengGameManagerMKII.instance.gameEndCD = FengGameManagerMKII.instance.gameEndTotalCDtime;
            if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER)
            {
                var parameters = new object[] { HumanScore };
                FengGameManagerMKII.instance.photonView.RPC("netGameWin", PhotonTargets.Others, parameters);
                if (((int)FengGameManagerMKII.settings[0xf4]) == 1)
                {
                    //this.chatRoom.addLINE("<color=#FFC000>(" + this.roundTime.ToString("F2") + ")</color> Round ended (game win).");
                }
            }
        }

        public virtual void OnNetGameWon(int score)
        {
            HumanScore = score;
            FengGameManagerMKII.instance.gameEndCD = FengGameManagerMKII.instance.gameEndTotalCDtime;
        }

        public virtual GameObject SpawnNonAiTitan(Vector3 position, GameObject randomTitanRespawn)
        {
            return PhotonNetwork.Instantiate("TITAN_VER3.1", position, randomTitanRespawn.transform.rotation, 0);
        }

        internal bool IsAllPlayersDead()
        {
            int num = 0;
            int num2 = 0;
            foreach (PhotonPlayer player in PhotonNetwork.playerList)
            {
                if (RCextensions.returnIntFromObject(player.CustomProperties[PhotonPlayerProperty.isTitan]) == 1)
                {
                    num++;
                    if (RCextensions.returnBoolFromObject(player.CustomProperties[PhotonPlayerProperty.dead]))
                    {
                        num2++;
                    }
                }
            }
            return (num == num2);
        }

        internal bool IsAllTitansDead()
        {
            foreach (GameObject obj2 in GameObject.FindGameObjectsWithTag("titan"))
            {
                if ((obj2.GetComponent<TITAN>() != null) && !obj2.GetComponent<TITAN>().hasDie)
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
            if (IsSinglePlayer)
            {
                return "Humanity Fail!\n Press " + FengGameManagerMKII.instance.inputManager.inputString[InputCode.restart] + " to Restart.\n\n\n";
            }
            return "Humanity Fail!\nAgain!\nGame Restart in " + ((int) gameEndCd) + "s\n\n";
        }
    }
}
