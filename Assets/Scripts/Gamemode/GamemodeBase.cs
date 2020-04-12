using Assets.Scripts.Gamemode.Options;
using Assets.Scripts.Settings;
using Assets.Scripts.UI.Elements;
using Newtonsoft.Json;
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

        [UiElement("MOTD", "Message of the Day")]
        public string Motd { get; set; } = string.Empty;

        public List<TitanType> DisabledTitanTypes { get; set; } = new List<TitanType>();

        [UiElement("Start Titans", "The amount of titans that will spawn at the start", SettingCategory.Titans)]
        public int Titans { get; set; } = 25;

        [UiElement("Titan Limit", "The max amount of titans", SettingCategory.Titans)]
        public int TitanLimit { get; set; } = 30;

        [UiElement("Min Size", "Minimal titan size", SettingCategory.Titans)]
        public float TitanMinimumSize { get; set; } = 0.7f;

        [UiElement("Max size", "Maximun titan size", SettingCategory.Titans)]
        public float TitanMaximumSize { get; set; } = 3f;

        [UiElement("Custom Size", "Enable custom titan sizes", SettingCategory.Titans)]
        public bool TitanCustomSize { get; set; } = true;

        [UiElement("Titan Chase Distance", "", SettingCategory.Titans)]
        public float TitanChaseDistance { get; set; } = 100f;

        [UiElement("Enable Titan Chase Distance", "", SettingCategory.Titans)]
        public bool TitanChaseDistanceEnabled { get; set; } = true;

        [UiElement("Titan Health Mode", "", SettingCategory.Titans)]
        public TitanHealthMode TitanHealthMode { get; set; } = TitanHealthMode.Disabled;

        [UiElement("Titan Minimum Health", "", SettingCategory.Titans)]
        public int TitanHealthMinimum { get; set; } = 200;

        [UiElement("Titan Maximum Health", "", SettingCategory.Titans)]
        public int TitanHealthMaximum { get; set; } = 500;

        [UiElement("Punk rock throwing", "", SettingCategory.Titans)]
        public bool PunkRockThrow { get; set; } = true;

        [UiElement("Custom Titans", "Should custom titan rates be used?", SettingCategory.Titans)]
        public bool CustomTitanRatio { get; set; } = false;

        [UiElement("Normal Ratio", "", SettingCategory.Titans)]
        public float TitanNormalRatio { get; set; } = 20f;

        [UiElement("Abberant Ratio", "", SettingCategory.Titans)]
        public float TitanAbberantRatio { get; set; } = 20f;

        [UiElement("Jumper Ratio", "", SettingCategory.Titans)]
        public float TitanJumperRatio { get; set; } = 20f;

        [UiElement("Crawler Ratio", "", SettingCategory.Titans)]
        public float TitanCrawlerRatio { get; set; } = 20f;

        [UiElement("Punk Ratio", "", SettingCategory.Titans)]
        public float TitanPunkRatio { get; set; } = 20f;

        [UiElement("Damage Mode", "Minimum damage you need to do", SettingCategory.Titans)]
        public int DamageMode { get; set; }

        //If the explode mode <= 0, then it's disabled, 0 > then it's enabled.
        [UiElement("Explode mode", "", SettingCategory.Titans)]
        public int TitanExplodeMode { get; set; } = 0;

        [UiElement("Allow Titan Shifters", "")]
        public bool TitanShifters { get; set; } = true;

        public bool TitansEnabled { get; set; } = true;

        [UiElement("Spawn Titans on FT Defeat", "Should titans spawn when the Female Titan is killed?", SettingCategory.Advanced)]
        public bool SpawnTitansOnFemaleTitanDefeat { get; set; } = true;

        [UiElement("Female Titan Despawn Time", "How long (in seconds), will the FT be on the map after dying?", SettingCategory.Advanced)]
        public float FemaleTitanDespawnTimer { get; set; } = 5f;

        [UiElement("PvP Cannons", "Can cannons kill humans?", SettingCategory.Pvp)]
        public bool PvpCannons { get; set; }

        public float FemaleTitanHealthModifier = 1f;

        //LevelInfo attributes
        public bool Hint;

        [UiElement("Horses", "Enables/Disables horses in the game")]
        public bool Horse { get; set; }

        [UiElement("Lava mode", "The floor is lava! Touching the floor means that you will die...")]
        public bool LavaMode { get; set; }
        public bool Crawlers;
        public bool Punks = true;

        [UiElement("PvP", "Can players kill each other?", SettingCategory.Pvp)]
        public PvpMode Pvp { get; set; } = PvpMode.Disabled;

        [UiElement("PvP win on enemies killed", "Does the round end if all PvP enemies are dead?", SettingCategory.Pvp)]
        public bool PvPWinOnEnemiesDead { get; set; } = false;

        [UiElement("Bomb PvP", "", SettingCategory.Pvp)]
        public bool PvPBomb { get; set; }

        [UiElement("Team mode", "Enable teams", SettingCategory.Pvp)]
        public TeamMode TeamMode { get; set; }

        [UiElement("Save KDR on DC", "When a player disconnects, should their KDR be saved?")]
        public bool SaveKDROnDisconnect { get; set; } = true;

        [UiElement("Endless Revive", "")]
        public int EndlessRevive { get; set; }

        [UiElement("Point mode", "", SettingCategory.Advanced)]
        public int PointMode { get; set; }

        public bool Supply { get; set; } = true;
        public bool IsPlayerTitanEnabled { get; set; }
        public RespawnMode RespawnMode { get; set; } = RespawnMode.DEATHMATCH;

        public int HumanScore = 0;
        public int TitanScore = 0;

        public float RespawnTime = 5f;
        [UiElement("Ahss Air Reload", "Can AHSS reload in mid air?", SettingCategory.Pvp)]
        public bool AhssAirReload { get; set; } = true;
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

        public virtual void OnRestart()
        {
            if (PointMode > 0)
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

        public static GamemodeBase ConvertToGamemode(string json, GamemodeType type)
        {
            GamemodeBase gamemode = null;
            switch (type)
            {
                case GamemodeType.Racing:
                    gamemode = JsonConvert.DeserializeObject<RacingGamemode>(json);
                    break;
                case GamemodeType.Capture:
                    gamemode = JsonConvert.DeserializeObject<CaptureGamemode>(json);
                    break;
                case GamemodeType.Titans:
                    gamemode = JsonConvert.DeserializeObject<KillTitansGamemode>(json);
                    break;
                case GamemodeType.Endless:
                    gamemode = JsonConvert.DeserializeObject<EndlessGamemode>(json);
                    break;
                case GamemodeType.Wave:
                    gamemode = JsonConvert.DeserializeObject<WaveGamemode>(json);
                    break;
                case GamemodeType.Trost:
                    gamemode = JsonConvert.DeserializeObject<TrostGamemode>(json);
                    break;
                case GamemodeType.TitanRush:
                    gamemode = JsonConvert.DeserializeObject<TitanRushGamemode>(json);
                    break;
                case GamemodeType.PvpAhss:
                    gamemode = JsonConvert.DeserializeObject<PvPAhssGamemode>(json);
                    break;
                case GamemodeType.Infection:
                    gamemode = JsonConvert.DeserializeObject<InfectionGamemode>(json);
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

        public virtual void OnTitanSpawned(TITAN titan)
        {
            if ((IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.MULTIPLAYER) || titan.photonView.isMine)
            {
                if (!titan.hasSetLevel)
                {
                    titan.myLevel = UnityEngine.Random.Range((float)0.7f, (float)3f);
                    if (TitanCustomSize)
                    {
                        titan.myLevel = UnityEngine.Random.Range(TitanMinimumSize, TitanMaximumSize);
                    }
                    titan.hasSetLevel = true;
                }
            }
            if (titan.maxHealth == 0)
            {
                switch (TitanHealthMode)
                {
                    case TitanHealthMode.Fixed:
                        titan.maxHealth = titan.currentHealth = UnityEngine.Random.Range(TitanHealthMinimum, TitanHealthMaximum + 1);
                        break;
                    case TitanHealthMode.Scaled:
                        titan.maxHealth = titan.currentHealth = Mathf.Clamp(Mathf.RoundToInt((titan.myLevel / 4f) * UnityEngine.Random.Range(TitanHealthMinimum, TitanHealthMaximum + 1)), TitanHealthMinimum, TitanHealthMaximum);
                        break;
                }
            }
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

        public virtual void OnTitanKilled(string titanName)
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
            HumanScore++;
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

        public virtual void OnGameLost()
        {
            TitanScore++;
            if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER)
            {
                var parameters = new object[] { TitanScore };
                FengGameManagerMKII.instance.photonView.RPC("netGameLose", PhotonTargets.Others, parameters);
                if ((int)FengGameManagerMKII.settings[0xf4] == 1)
                {
                    //FengGameManagerMKII.instance.chatRoom.addLINE("<color=#FFC000>(" + this.roundTime.ToString("F2") + ")</color> Round ended (game lose).");
                }
            }
        }
        
        public virtual void OnNetGameLost(int score)
        {
            TitanScore = score;
        }

        public virtual void OnNetGameWon(int score)
        {
            HumanScore = score;
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
