using Assets.Scripts.Characters;
using Assets.Scripts.Characters.Humans;
using Assets.Scripts.Characters.Humans.Customization;
using Assets.Scripts.Characters.Titan;
using Assets.Scripts.Gamemode;
using Assets.Scripts.Gamemode.Options;
using Assets.Scripts.Legacy.CustomMap;
using Assets.Scripts.Room;
using Assets.Scripts.Room.Chat;
using Assets.Scripts.Services;
using Assets.Scripts.Services.Interface;
using Assets.Scripts.Settings;
using Assets.Scripts.Settings.Gamemodes;
using Assets.Scripts.UI;
using Assets.Scripts.UI.Camera;
using Assets.Scripts.UI.InGame;
using Assets.Scripts.UI.InGame.HUD;
using Assets.Scripts.Utility;
using Photon;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using Hashtable = ExitGames.Client.Photon.Hashtable;

namespace Assets.Scripts
{
    /// <summary>
    /// The Original AoTTG1 god class of 17k+ lines of code! This class used to do literally everything, and still does too much.
    /// Do not add new code to this class, and rather refactor the logic to a service or a different component.
    /// This class should eventually only have 2 responsibilities:
    /// 1. Initializing the Settings
    /// 2. Loading a level and the respective gamemode
    /// </summary>
    public class FengGameManagerMKII : PunBehaviour
    {
        protected ISpawnService SpawnService => Service.Spawn;

        #region Obsolete Properties
        [Obsolete("Dirty way of getting some materials. Should be deleted once its dependencies in FengGameManager are resolved")]
        public RCLegacy RcLegacy;

        [Obsolete("Cannon specific logic. Should be moved into a dedicated Cannon manager.")]
        public Dictionary<int, CannonValues> allowedToCannon;

        [Obsolete("Used in AoTTG to handle bans, however this needs to be refactored and possibly partially moved to the photon server")]
        public static ExitGames.Client.Photon.Hashtable banHash;

        [Obsolete("FengGameManager should not have a public InRoomChat variable. This must be made private. Use the ChatService instead to get a reference to InRoomChat.")]
        public InRoomChat chatRoom;

        [Obsolete("Only a Respawn Service or Gamemode should contain knowledge over this")]
        public GameObject checkpoint;

        [Obsolete("Legacy RC scripts are no longer supported in AoTTG2")]
        public static string currentLevel;
        [Obsolete("Legacy RC scripts are no longer supported in AoTTG2")]
        public static string currentScript;
        [Obsolete("Legacy RC custom logic is no longer supported in AoTTG2")]
        public static string currentScriptLogic;
        [Obsolete("Migrate this to a dedicated TeamService")]
        public int cyanKills;
        [Obsolete("AoTTG used to have a limit on for how long a room could be hosted, this has been removed in AoTTG2 so this bool serves no purpose.")]
        private bool gameTimesUp;
        [Obsolete("This list is only used to replace the CUBE_001 TEXTURE when CUSTOM MAP is loaded. For AoTTG2 we no longer require this method")]
        public List<GameObject> groundList;
        [Obsolete("This is a hashtable which keeps track of every HERO.cs instance. Appears to do the same as 'FengGameManager.heroes' yet no logic happens to this Hashtable other than adding and deleting items.")]
        public static ExitGames.Client.Photon.Hashtable heroHash;
        public static List<int> ignoreList;
        [Obsolete("Hashtable only used for InfectionGamemode. Migrate code to InfectionGamemode")]
        public static ExitGames.Client.Photon.Hashtable imatitan;
        [Obsolete("A static reference to the god class is something we don't want. Avoid introducing new code which makes use of this, and refactor and introduce a new services instead.")]
        public static FengGameManagerMKII instance;
        [Obsolete("Used to automatically recompile a player list, yet again, this logic shouldn't be here...")]
        public bool isRecompiling;
        [Obsolete("Only used for Cannons. Remove in Issue #75")]
        public bool isRestarting;
        [Obsolete("Used to schedule as Resources.UnloadUnusedAssets (https://docs.unity3d.com/ScriptReference/Resources.UnloadUnusedAssets.html) after 10s. Do we even need to do this manually? Either way, logic should be moved to classes which required this. ")]
        public bool isUnloading;
        [Obsolete("Used to keep track over how many KillInfo objects exist. UI related logic to prevent more than 5 kill info objects being loaded at once, should be moved to some UI class. ")]
        private readonly List<GameObject> killInfoGO = new List<GameObject>();
        [Obsolete("Legacy method of keeping track of custom level scripts, which we no longer support")]
        public List<string[]> levelCache;
        public static ExitGames.Client.Photon.Hashtable[] linkHash;
        [Obsolete("Use RacingGamemode.localRacingResult")]
        private string localRacingResult;
        public static bool logicLoaded;
        [Obsolete("Migrate this to a dedicated TeamService")]
        public int magentaKills;
        private IN_GAME_MAIN_CAMERA mainCamera;
        [Obsolete("Legacy method which appears to have been used to determine if a client is 'master' RC or not. This would have given special permissions, but the feature is only used within 2 locations and is obviously prone to cheating.")]
        public static bool masterRC;
        [Obsolete("Migrate this to HERO.cs, as FengGameManager does not need to know how fast a player is going. Hero.cs can then have a method named 'Speed' which returns the current speed")]
        private float maxSpeed;
        [Obsolete("Seems to be used to determine whether a player is a human or titan.")]
        private string myLastHero;
        [Obsolete("Value is always playerRespawn")]
        private string myLastRespawnTag = "playerRespawn";
        [Obsolete("Only a Respawn Service or Gamemode should contain knowledge over this")]
        public float myRespawnTime;
        [Obsolete("A gamemode should decide whether or not a player has to choose between humanity, AHSS or titanity")]
        public bool needChooseSide;
        [Obsolete("A bool used to prevent restarting when true, yet this is never true. Refactor this in the future so it does have a purpose")]
        public static bool noRestart;
        [Obsolete("Legacy RC custom logic is no longer supported in AoTTG2")]
        public static string oldScript;
        [Obsolete("Legacy RC custom logic is no longer supported in AoTTG2")]
        public static string oldScriptLogic;
        [Obsolete("This value is always false")]
        public static bool OnPrivateServer;
        public string playerList;
        [Obsolete("Use the class PlayerSpawns instead")]
        public List<Vector3> playerSpawnsC;
        [Obsolete("Use the class PlayerSpawns instead")]
        public List<Vector3> playerSpawnsM;
        public List<PhotonPlayer> playersRPC;
        public Dictionary<string, int[]> PreservedPlayerKDR;
        [Obsolete("A value is never assigned")]
        public static string PrivateServerAuthPass;
        [Obsolete("Use RacingGamemode instead")]
        public Vector3 racingSpawnPoint;
        [Obsolete("Use RacingGamemode instead")]
        public bool racingSpawnPointSet;
        [Obsolete("This is only used for the obsolete MasterRC field")]
        public List<float> restartCount;
        public bool restartingMC;
        [Obsolete("Hardcoded string. Avoid using this")]
        public static string[] s = "verified343,hair,character_eye,glass,character_face,character_head,character_hand,character_body,character_arm,character_leg,character_chest,character_cape,character_brand,character_3dmg,r,character_blade_l,character_3dmg_gas_r,character_blade_r,3dmg_smoke,HORSE,hair,body_001,Cube,Plane_031,mikasa_asset,character_cap_,character_gun".Split(new char[] { ',' });
        [Obsolete("A god class array for settings. Move these settings to the classes where they belong")]
        public static object[] settings;
        public static Material skyMaterial;
        [Obsolete("This is used to assign a name to the HERO, but it shouldn't be within FengGameManager")]
        public new string name { get; set; }
        [Obsolete("A dirty way to get a reference to the InGameUI. Should use UiService instead of UI references.")]
        public InGameUi InGameUI;
        #endregion

        [SerializeField]
        private VersionManager versionManager;

        /// <summary>
        /// A static accessor to the current Gamemode
        /// </summary>

        public bool IsReconnecting = false;
        public static GamemodeBase Gamemode { get; set; }
        /// <summary>
        /// A static accessor to the current loaded Level
        /// </summary>
        public static Level Level { get; set; }

        /// <summary>
        /// A static accessor to the Level that should be loaded on a new round
        /// </summary>
        public static Level NewRoundLevel { get; set; }
        /// <summary>
        /// A static accessor to the Gamemode settings that should be loaded on a new round
        /// </summary>
        public static GamemodeSettings NewRoundGamemode { get; set; }

        /// <summary>
        /// We store this in a variable to make sure the Coroutine is killed if the game 
        /// is restarted, making it so player can't be duplicated.
        /// 
        /// <para>This should be moved if respawn is moved to Spawn/Player Service.</para>
        /// </summary>
        private Coroutine respawnCoroutine;

        #region PUN Events
        public override void OnConnectedToMaster()
        {
            Debug.Log("OnConnectedToMaster");
        }

        public override void OnConnectedToPhoton()
        {
            Debug.Log("OnConnectedToPhoton");
        }

        public override void OnConnectionFail(DisconnectCause cause)
        {
            Debug.Log("OnConnectionFail : " + cause.ToString());
            IN_GAME_MAIN_CAMERA.gametype = GAMETYPE.Stop;

            if (cause is DisconnectCause.DisconnectByClientTimeout)
            {
                IsReconnecting = true;
            }

        }

        public override void OnCreatedRoom()
        {
            Debug.Log("OnCreatedRoom");
        }

        public override void OnDisconnectedFromPhoton()
        {
            Debug.Log("OnDisconnectedFromPhoton");
            if (Application.loadedLevel != 0)
            {
                Time.timeScale = 1f;
                this.resetSettings(true);
                this.loadconfig();
                IN_GAME_MAIN_CAMERA.gametype = GAMETYPE.Stop;
                this.DestroyAllExistingCloths();
                Application.LoadLevel(0);
                if (IsReconnecting)
                {
                    IsReconnecting = false;
                    PhotonNetwork.ReconnectAndRejoin();
                }

            }
        }

        //TODO: CustomMapService.OnLevelWasLoaded is called before OnJoinedRoom
        public override void OnJoinedRoom()
        {
            Service.Settings.SetRoomPropertySettings();
            SetLevelAndGamemode();

            this.playerList = string.Empty;
            char[] separator = new char[] { "`"[0] };
            //UnityEngine.MonoBehaviour.print("OnJoinedRoom " + PhotonNetwork.room.name + "    >>>>   " + LevelInfo.getInfo(PhotonNetwork.room.name.Split(separator)[1]).mapName);
            this.gameTimesUp = false;
            char[] chArray3 = new char[] { "`"[0] };
            string[] strArray = PhotonNetwork.room.name.Split(chArray3);
            //if (strArray[4] == "day")
            //{
            //    IN_GAME_MAIN_CAMERA.dayLight = DayLight.Day;
            //}
            //else if (strArray[4] == "dawn")
            //{
            //    IN_GAME_MAIN_CAMERA.dayLight = DayLight.Dawn;
            //}
            //else if (strArray[4] == "night")
            //{
            //    IN_GAME_MAIN_CAMERA.dayLight = DayLight.Night;
            //}
            if (PhotonNetwork.isMasterClient)
            {
                Level.LoadLevel();
            }
            GameCursor.CursorMode = CursorMode.Loading;
            var hashtable = new Hashtable
            {
                {PhotonPlayerProperty.name, LoginFengKAI.player.name},
                {PhotonPlayerProperty.guildName, LoginFengKAI.player.guildname},
                {PhotonPlayerProperty.kills, 0},
                {PhotonPlayerProperty.max_dmg, 0},
                {PhotonPlayerProperty.total_dmg, 0},
                {PhotonPlayerProperty.deaths, 0},
                {PhotonPlayerProperty.dead, true},
                {PhotonPlayerProperty.isTitan, 0},
                {PhotonPlayerProperty.RCteam, 0},
                {PhotonPlayerProperty.currentLevel, string.Empty}
            };
            var propertiesToSet = hashtable;
            PhotonNetwork.player.SetCustomProperties(propertiesToSet);
            this.needChooseSide = true;
            this.ClearKillInfo();
            this.name = LoginFengKAI.player.name;
            var hashtable3 = new ExitGames.Client.Photon.Hashtable
            {
                {PhotonPlayerProperty.name, this.name}
            };
            PhotonNetwork.player.SetCustomProperties(hashtable3);
            if (OnPrivateServer)
            {
                ServerRequestAuthentication(PrivateServerAuthPass);
            }

            Service.Discord.UpdateDiscordActivity(PhotonNetwork.room);
        }

        public override void OnLeftLobby()
        {
            Debug.Log("OnLeftLobby");
        }

        public override void OnLeftRoom()
        {
            Debug.Log("OnLeftRoom");
        }

        public override void OnMasterClientSwitched(PhotonPlayer newMasterClient)
        {
            if (!noRestart)
            {
                if (PhotonNetwork.isMasterClient)
                {
                    this.restartingMC = true;
                }
                this.resetSettings(false);
                if (!GameSettings.Gamemode.IsPlayerTitanEnabled.Value)
                {
                    ExitGames.Client.Photon.Hashtable propertiesToSet = new ExitGames.Client.Photon.Hashtable();
                    propertiesToSet.Add(PhotonPlayerProperty.isTitan, 1);
                    PhotonNetwork.player.SetCustomProperties(propertiesToSet);
                }
                if (!(this.gameTimesUp || !PhotonNetwork.isMasterClient))
                {
                    this.restartGame2(true);
                    base.photonView.RPC(nameof(setMasterRC), PhotonTargets.All, new object[0]);
                }
            }
            noRestart = false;
        }

        public override void OnPhotonMaxCccuReached()
        {
            Debug.Log("OnPhotonMaxCccuReached");
        }

        public override void OnPhotonPlayerConnected(PhotonPlayer player)
        {
            if (PhotonNetwork.isMasterClient)
            {
                PhotonView photonView = base.photonView;
                if (banHash.ContainsValue(RCextensions.returnStringFromObject(player.CustomProperties[PhotonPlayerProperty.name])))
                {
                    this.kickPlayerRC(player, false, "banned.");
                }
                else
                {
                    int num = RCextensions.returnIntFromObject(player.CustomProperties[PhotonPlayerProperty.statACL]);
                    int num2 = RCextensions.returnIntFromObject(player.CustomProperties[PhotonPlayerProperty.statBLA]);
                    int num3 = RCextensions.returnIntFromObject(player.CustomProperties[PhotonPlayerProperty.statGAS]);
                    int num4 = RCextensions.returnIntFromObject(player.CustomProperties[PhotonPlayerProperty.statSPD]);
                    if ((((num > 150) || (num2 > 125)) || (num3 > 150)) || (num4 > 140))
                    {
                        this.kickPlayerRC(player, true, "excessive stats.");
                        return;
                    }
                    if (GameSettings.Gamemode.SaveKDROnDisconnect.Value)
                    {
                        base.StartCoroutine(this.WaitAndReloadKDR(player));
                    }
                    ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
                    if ((ignoreList != null) && (ignoreList.Count > 0))
                    {
                        photonView.RPC(nameof(ignorePlayerArray), player, new object[] { ignoreList.ToArray() });
                    }
                    photonView.RPC(nameof(setMasterRC), player, new object[0]);
                }
            }
            this.RecompilePlayerList(0.1f);
        }

        public override void OnPhotonPlayerDisconnected(PhotonPlayer player)
        {
            if (ignoreList.Contains(player.ID))
            {
                ignoreList.Remove(player.ID);
            }
            InstantiateTracker.instance.TryRemovePlayer(player.ID);
            if (PhotonNetwork.isMasterClient)
            {
                base.photonView.RPC(nameof(verifyPlayerHasLeft), PhotonTargets.All, new object[] { player.ID });
            }
            if (GameSettings.Gamemode.SaveKDROnDisconnect.Value)
            {
                string key = RCextensions.returnStringFromObject(player.CustomProperties[PhotonPlayerProperty.name]);
                if (this.PreservedPlayerKDR.ContainsKey(key))
                {
                    this.PreservedPlayerKDR.Remove(key);
                }
                int[] numArray2 = new int[] { RCextensions.returnIntFromObject(player.CustomProperties[PhotonPlayerProperty.kills]), RCextensions.returnIntFromObject(player.CustomProperties[PhotonPlayerProperty.deaths]), RCextensions.returnIntFromObject(player.CustomProperties[PhotonPlayerProperty.max_dmg]), RCextensions.returnIntFromObject(player.CustomProperties[PhotonPlayerProperty.total_dmg]) };
                this.PreservedPlayerKDR.Add(key, numArray2);
            }
            this.RecompilePlayerList(0.1f);
        }

        public override void OnPhotonPlayerPropertiesChanged(object[] playerAndUpdatedProps)
        {
            this.RecompilePlayerList(0.1f);
            if (((playerAndUpdatedProps != null) && (playerAndUpdatedProps.Length >= 2)) && (((PhotonPlayer) playerAndUpdatedProps[0]) == PhotonNetwork.player))
            {
                ExitGames.Client.Photon.Hashtable hashtable2;
                ExitGames.Client.Photon.Hashtable hashtable = (ExitGames.Client.Photon.Hashtable) playerAndUpdatedProps[1];
                if (hashtable.ContainsKey("name") && (RCextensions.returnStringFromObject(PhotonNetwork.player.CustomProperties[PhotonPlayerProperty.name]) != this.name))
                {
                    hashtable2 = new ExitGames.Client.Photon.Hashtable();
                    hashtable2.Add(PhotonPlayerProperty.name, this.name);
                    PhotonNetwork.player.SetCustomProperties(hashtable2);
                }
                if (((hashtable.ContainsKey("statACL") || hashtable.ContainsKey("statBLA")) || hashtable.ContainsKey("statGAS")) || hashtable.ContainsKey("statSPD"))
                {
                    PhotonPlayer player = PhotonNetwork.player;
                    int num = RCextensions.returnIntFromObject(player.CustomProperties[PhotonPlayerProperty.statACL]);
                    int num2 = RCextensions.returnIntFromObject(player.CustomProperties[PhotonPlayerProperty.statBLA]);
                    int num3 = RCextensions.returnIntFromObject(player.CustomProperties[PhotonPlayerProperty.statGAS]);
                    int num4 = RCextensions.returnIntFromObject(player.CustomProperties[PhotonPlayerProperty.statSPD]);
                    if (num > 150)
                    {
                        hashtable2 = new ExitGames.Client.Photon.Hashtable();
                        hashtable2.Add(PhotonPlayerProperty.statACL, 100);
                        PhotonNetwork.player.SetCustomProperties(hashtable2);
                        num = 100;
                    }
                    if (num2 > 0x7d)
                    {
                        hashtable2 = new ExitGames.Client.Photon.Hashtable();
                        hashtable2.Add(PhotonPlayerProperty.statBLA, 100);
                        PhotonNetwork.player.SetCustomProperties(hashtable2);
                        num2 = 100;
                    }
                    if (num3 > 150)
                    {
                        hashtable2 = new ExitGames.Client.Photon.Hashtable();
                        hashtable2.Add(PhotonPlayerProperty.statGAS, 100);
                        PhotonNetwork.player.SetCustomProperties(hashtable2);
                        num3 = 100;
                    }
                    if (num4 > 140)
                    {
                        hashtable2 = new ExitGames.Client.Photon.Hashtable();
                        hashtable2.Add(PhotonPlayerProperty.statSPD, 100);
                        PhotonNetwork.player.SetCustomProperties(hashtable2);
                        num4 = 100;
                    }
                }
            }
        }

        public override void OnReceivedRoomListUpdate() { }

        public override void OnCustomAuthenticationResponse(Dictionary<string, object> data)
        {
            Debug.LogError(data);
        }

        #endregion

        private void Start()
        {
            Service.Level.OnLevelLoaded += Level_OnLevelLoaded;
            PhotonNetwork.automaticallySyncScene = true;
            Debug.Log($"Version: {versionManager.Version}");
            instance = this;
            base.gameObject.name = "MultiplayerManager";
            CostumeHair.init();
            CharacterMaterials.init();
            UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
            this.name = string.Empty;
            banHash = new ExitGames.Client.Photon.Hashtable();
            imatitan = new ExitGames.Client.Photon.Hashtable();
            oldScript = string.Empty;
            currentLevel = string.Empty;
            if (currentScript == null)
            {
                currentScript = string.Empty;
            }
            this.playerSpawnsC = new List<Vector3>();
            this.playerSpawnsM = new List<Vector3>();
            this.playersRPC = new List<PhotonPlayer>();
            this.levelCache = new List<string[]>();
            this.restartCount = new List<float>();
            ignoreList = new List<int>();
            this.groundList = new List<GameObject>();
            noRestart = false;
            masterRC = false;
            heroHash = new ExitGames.Client.Photon.Hashtable();
            logicLoaded = false;
            oldScriptLogic = string.Empty;
            currentScriptLogic = string.Empty;
            this.playerList = string.Empty;
            this.loadconfig();
            ChangeQuality.setCurrentQuality();
        }

        /// <summary>
        /// Retrieves and sets the current Level and Gamemode from the <see cref="PhotonNetwork.room"/>. Used by new players who just joined the room
        /// </summary>
        public void SetLevelAndGamemode()
        {
            Level = PhotonNetwork.room.GetLevel();
            var gamemodeSettings = PhotonNetwork.room.GetGamemodeSetting(Level);
            SetGamemode(gamemodeSettings);
        }

        /// <summary>
        /// Removes an existing <see cref="GamemodeBase"/> component from the GameObject, and adds a new <see cref="GamemodeBase"/> based on <paramref name="settings"/>
        /// </summary>
        /// <param name="settings">The settings on which a new <see cref="GamemodeBase"/> will be initialized</param>
        private void SetGamemode(GamemodeSettings settings)
        {
            if (Gamemode == null)
            {
                Service.Settings.Get().ChangeSettings(settings);
                var gamemodeObject = GameObject.Find("Gamemode");
                Gamemode = (GamemodeBase) gamemodeObject.AddComponent(settings.GetGamemodeFromSettings());
            }
            else
            {
                foreach (var comp in Gamemode.gameObject.GetComponents<Component>())
                {
                    if (!(comp is Transform || comp is PhotonView))
                    {
                        Destroy(comp);
                    }
                }
                Gamemode = null;
                SetGamemode(settings);
            }
        }

        /// <summary>
        /// This method handles the "OnLevelLoaded" event from the <see cref="ILevelService"/>. The regular OnLevelWasLoaded or SceneManager.sceneLoaded cannot be used, as this event also takes the loading of custom maps in consideration.
        /// </summary>
        /// <param name="scene">Index of the scene that was loaded</param>
        /// <param name="level">The level that was loaded</param>
        private void Level_OnLevelLoaded(int scene, Level level)
        {
            // Scene 0 = Menu Scene
            if (scene == 0) return;
            var ui = GameObject.Find("Canvas").GetComponent<UiHandler>();
            ui.ShowInGameUi();
            ChangeQuality.setCurrentQuality();
            foreach (GameObject obj2 in GameObject.FindGameObjectsWithTag("titan"))
            {
                if (!((obj2.GetPhotonView() != null) && obj2.GetPhotonView().owner.isMasterClient))
                {
                    UnityEngine.Object.Destroy(obj2);
                }
            }
            GameObject obj3 = (GameObject) UnityEngine.Object.Instantiate(Resources.Load("MainCamera_mono"), GameObject.Find("cameraDefaultPosition").transform.position, GameObject.Find("cameraDefaultPosition").transform.rotation);
            UnityEngine.Object.Destroy(GameObject.Find("cameraDefaultPosition"));
            obj3.name = "MainCamera";
            this.cache();
            this.loadskin();
            IN_GAME_MAIN_CAMERA.gametype = GAMETYPE.Playing;
            PVPcheckPoint.chkPts = new ArrayList();
            Camera.main.GetComponent<IN_GAME_MAIN_CAMERA>().enabled = false;
            Camera.main.GetComponent<CameraShake>().enabled = false;
            if (this.needChooseSide)
            {
                //TODO: Show ChooseSide Message
                //this.ShowHUDInfoTopCenterADD("\n\nPRESS 1 TO ENTER GAME");
            }
            else if (SpectatorMode.IsDisable())
            {
                if (RCextensions.returnIntFromObject(PhotonNetwork.player.CustomProperties[PhotonPlayerProperty.isTitan]) == 2)
                {
                    SpawnService.Spawn<PlayerTitan>();
                }
                else
                {
                    this.SpawnPlayer(this.myLastHero, this.myLastRespawnTag);
                }
            }

            if (SpectatorMode.IsEnable())
            {
                SpectatorMode.UpdateSpecMode();
            }
        }

        /// <summary>
        /// Restarts the room to start a new round If a new level or gamemode was setup by the MC, additional steps will be taken to replace these settings and load the new level.
        /// </summary>
        public void RestartRound()
        {
            if (respawnCoroutine != null)
                StopCoroutine(respawnCoroutine);

            if (NewRoundLevel != null && Level.Name != NewRoundLevel.Name && PhotonNetwork.isMasterClient)
            {
                Level = NewRoundLevel;
                SetGamemode(NewRoundGamemode);
                var hash = new ExitGames.Client.Photon.Hashtable
                {
                    {"level", Level.Name},
                    {"gamemode", GameSettings.Gamemode.GamemodeType.ToString()}
                };
                PhotonNetwork.room.SetCustomProperties(hash);
            }
            else if (NewRoundGamemode != null && GameSettings.Gamemode.GamemodeType != NewRoundGamemode.GamemodeType && PhotonNetwork.isMasterClient)
            {
                SetGamemode(NewRoundGamemode);
                var hash = new ExitGames.Client.Photon.Hashtable
                {
                    {"level", Level.Name},
                    {"gamemode", GameSettings.Gamemode.GamemodeType.ToString()}
                };
                PhotonNetwork.room.SetCustomProperties(hash);
            }

            Service.Entity.OnRestart();
            EventManager.OnRestart.Invoke();
        }

        /// <summary>
        /// Mostly contains obsolete logic, yet this is currently used to setup the right Level and Gamemode Settings for a non-MC.
        /// </summary>
        /// <param name="info"></param>
        [PunRPC]
        private void RPCLoadLevel(PhotonMessageInfo info)
        {
            if (info.sender.isMasterClient)
            {
                this.DestroyAllExistingCloths();
                SetLevelAndGamemode();
                if (PhotonNetwork.isMasterClient) Level.LoadLevel();
            }
            else if (PhotonNetwork.isMasterClient)
            {
                this.kickPlayerRC(info.sender, true, "false restart.");
            }
        }

        /// <summary>
        /// Activates Endless Respawn when the settings where changed
        /// </summary>
        [Obsolete("This handled the 'OnRoomSettingsChanged' event, however, Gamemode specific logic should this be moved to the gamemode classes")]
        public void OnRoomSettingsInitialized()
        {
            if (mainCamera?.main_object != null)
            {
                mainCamera.main_object.GetComponent<Hero>()?.SetHorse();
            }
            if (GameSettings.Respawn.Mode == RespawnMode.Endless)
            {
                StopCoroutine(respawnE(GameSettings.Respawn.ReviveTime.Value));
                StartCoroutine(respawnE(GameSettings.Respawn.ReviveTime.Value));
            }
            else
            {
                StopCoroutine(respawnE(GameSettings.Respawn.ReviveTime.Value));
            }

            if (GameSettings.Gamemode.TeamMode != TeamMode.Disabled)
            {
                if (RCextensions.returnIntFromObject(PhotonNetwork.player.CustomProperties[PhotonPlayerProperty.RCteam]) == 0)
                {
                    this.setTeam(3);
                }
            }
            else
            {
                this.setTeam(0);
            }


            if (GameSettings.Gamemode.GamemodeType == GamemodeType.Infection)
            {
                var gamemodeInfection = GameSettings.DerivedGamemode<InfectionGamemodeSettings>();
                if (gamemodeInfection.Infected > 0)
                {
                    var hashtable = new ExitGames.Client.Photon.Hashtable();
                    hashtable.Add(PhotonPlayerProperty.RCteam, 0);
                    PhotonNetwork.player.SetCustomProperties(hashtable);
                    this.chatRoom.AddMessage($"<color=#FFCC00>Infection mode ({gamemodeInfection.Infected}) enabled. Make sure your first character is human.</color>");
                }
                else
                {
                    var hashtable = new ExitGames.Client.Photon.Hashtable();
                    hashtable.Add(PhotonPlayerProperty.isTitan, 1);
                    PhotonNetwork.player.SetCustomProperties(hashtable);
                    this.chatRoom.AddMessage("<color=#FFCC00>Infection Mode disabled.</color>");
                }
            }
        }

        #region Obsolete Methods
        [Obsolete("FengGameManager doesn't require the usage of IN_GAME_MAIN_CAMERA.")]
        public void addCamera(IN_GAME_MAIN_CAMERA c)
        {
            this.mainCamera = c;
        }
        [Obsolete("GameManager will 'cache' various properties, yet all of these are obsolete, and so is this method.")]
        private void cache()
        {
            ClothFactory.ClearClothCache();
            this.playersRPC.Clear();
            this.groundList.Clear();
            this.PreservedPlayerKDR = new Dictionary<string, int[]>();
            noRestart = false;
            skyMaterial = null;
            logicLoaded = false;
            this.isUnloading = false;
            this.isRecompiling = false;
            Time.timeScale = 1f;
            Camera.main.farClipPlane = 1500f; //TODO Make camera view distance a configurable setting
            this.isRestarting = false;
            if (PhotonNetwork.isMasterClient)
            {
                base.StartCoroutine(this.WaitAndResetRestarts());
            }
            this.RecompilePlayerList(0.5f);
        }

        [Obsolete("This is a responsibility for the InRoomChat.")]
        [PunRPC]
        public void Chat(string content, string sender, PhotonMessageInfo info)
        {
            if (sender != string.Empty)
            {
                content = sender + ":" + content;
            }
            content = "<color=#FFC000>[" + Convert.ToString(info.sender.ID) + "]</color> " + content;
            this.chatRoom.AddMessage(content);
        }

        [Obsolete("This is a responsibility for the InRoomChat.")]
        [PunRPC]
        public void ChatPM(string sender, string content, PhotonMessageInfo info)
        {
            content = sender + ":" + content;
            content = "<color=#FFC000>FROM [" + Convert.ToString(info.sender.ID) + "]</color> " + content;
            this.chatRoom.AddMessage(content);
        }

        [Obsolete("This is a responsibility for the InRoomChat.")]
        [PunRPC]
        private void ClearChat()
        {
            chatRoom.ClearMessages();
        }

        [Obsolete("The AoTTG way of disposing dynamically downloaded textures.")]
        [PunRPC]
        private void clearlevel(string[] link, PhotonMessageInfo info)
        {
            if (info.sender.isMasterClient)
            {
                if (info.sender.isMasterClient && (link.Length > 6))
                {
                    base.StartCoroutine(this.clearlevelE(link));
                }
            }
        }

        [Obsolete("This IEnumerator of clearLevel(string[] link, PhotonMessageInfo info)")]
        private IEnumerator clearlevelE(string[] skybox)
        {
            string key = skybox[6];
            bool mipmap = true;
            bool iteratorVariable2 = false;
            if (((int) settings[0x3f]) == 1)
            {
                mipmap = false;
            }
            if (((((skybox[0] != string.Empty) || (skybox[1] != string.Empty)) || ((skybox[2] != string.Empty) || (skybox[3] != string.Empty))) || (skybox[4] != string.Empty)) || (skybox[5] != string.Empty))
            {
                string iteratorVariable3 = string.Join(",", skybox);
                if (!linkHash[1].ContainsKey(iteratorVariable3))
                {
                    iteratorVariable2 = true;
                    Material material = Camera.main.GetComponent<Skybox>().material;
                    string url = skybox[0];
                    string iteratorVariable6 = skybox[1];
                    string iteratorVariable7 = skybox[2];
                    string iteratorVariable8 = skybox[3];
                    string iteratorVariable9 = skybox[4];
                    string iteratorVariable10 = skybox[5];
                    if ((url.EndsWith(".jpg") || url.EndsWith(".png")) || url.EndsWith(".jpeg"))
                    {
                        WWW link = new WWW(url);
                        yield return link;
                        Texture2D texture = RCextensions.loadimage(link, mipmap, 0x7a120);
                        link.Dispose();
                        material.SetTexture("_FrontTex", texture);
                    }
                    if ((iteratorVariable6.EndsWith(".jpg") || iteratorVariable6.EndsWith(".png")) || iteratorVariable6.EndsWith(".jpeg"))
                    {
                        WWW iteratorVariable13 = new WWW(iteratorVariable6);
                        yield return iteratorVariable13;
                        Texture2D iteratorVariable14 = RCextensions.loadimage(iteratorVariable13, mipmap, 0x7a120);
                        iteratorVariable13.Dispose();
                        material.SetTexture("_BackTex", iteratorVariable14);
                    }
                    if ((iteratorVariable7.EndsWith(".jpg") || iteratorVariable7.EndsWith(".png")) || iteratorVariable7.EndsWith(".jpeg"))
                    {
                        WWW iteratorVariable15 = new WWW(iteratorVariable7);
                        yield return iteratorVariable15;
                        Texture2D iteratorVariable16 = RCextensions.loadimage(iteratorVariable15, mipmap, 0x7a120);
                        iteratorVariable15.Dispose();
                        material.SetTexture("_LeftTex", iteratorVariable16);
                    }
                    if ((iteratorVariable8.EndsWith(".jpg") || iteratorVariable8.EndsWith(".png")) || iteratorVariable8.EndsWith(".jpeg"))
                    {
                        WWW iteratorVariable17 = new WWW(iteratorVariable8);
                        yield return iteratorVariable17;
                        Texture2D iteratorVariable18 = RCextensions.loadimage(iteratorVariable17, mipmap, 0x7a120);
                        iteratorVariable17.Dispose();
                        material.SetTexture("_RightTex", iteratorVariable18);
                    }
                    if ((iteratorVariable9.EndsWith(".jpg") || iteratorVariable9.EndsWith(".png")) || iteratorVariable9.EndsWith(".jpeg"))
                    {
                        WWW iteratorVariable19 = new WWW(iteratorVariable9);
                        yield return iteratorVariable19;
                        Texture2D iteratorVariable20 = RCextensions.loadimage(iteratorVariable19, mipmap, 0x7a120);
                        iteratorVariable19.Dispose();
                        material.SetTexture("_UpTex", iteratorVariable20);
                    }
                    if ((iteratorVariable10.EndsWith(".jpg") || iteratorVariable10.EndsWith(".png")) || iteratorVariable10.EndsWith(".jpeg"))
                    {
                        WWW iteratorVariable21 = new WWW(iteratorVariable10);
                        yield return iteratorVariable21;
                        Texture2D iteratorVariable22 = RCextensions.loadimage(iteratorVariable21, mipmap, 0x7a120);
                        iteratorVariable21.Dispose();
                        material.SetTexture("_DownTex", iteratorVariable22);
                    }
                    Camera.main.GetComponent<Skybox>().material = material;
                    linkHash[1].Add(iteratorVariable3, material);
                    skyMaterial = material;
                }
                else
                {
                    Camera.main.GetComponent<Skybox>().material = (Material) linkHash[1][iteratorVariable3];
                    skyMaterial = (Material) linkHash[1][iteratorVariable3];
                }
            }
            if ((key.EndsWith(".jpg") || key.EndsWith(".png")) || key.EndsWith(".jpeg"))
            {
                foreach (GameObject iteratorVariable23 in this.groundList)
                {
                    if ((iteratorVariable23 != null) && (iteratorVariable23.GetComponent<Renderer>() != null))
                    {
                        foreach (Renderer iteratorVariable24 in iteratorVariable23.GetComponentsInChildren<Renderer>())
                        {
                            if (!linkHash[0].ContainsKey(key))
                            {
                                WWW iteratorVariable25 = new WWW(key);
                                yield return iteratorVariable25;
                                Texture2D iteratorVariable26 = RCextensions.loadimage(iteratorVariable25, mipmap, 0x30d40);
                                iteratorVariable25.Dispose();
                                if (!linkHash[0].ContainsKey(key))
                                {
                                    iteratorVariable2 = true;
                                    iteratorVariable24.material.mainTexture = iteratorVariable26;
                                    linkHash[0].Add(key, iteratorVariable24.material);
                                    iteratorVariable24.material = (Material) linkHash[0][key];
                                }
                                else
                                {
                                    iteratorVariable24.material = (Material) linkHash[0][key];
                                }
                            }
                            else
                            {
                                iteratorVariable24.material = (Material) linkHash[0][key];
                            }
                        }
                    }
                }
            }
            else if (key.ToLower() == "transparent")
            {
                foreach (GameObject obj2 in this.groundList)
                {
                    if ((obj2 != null) && (obj2.GetComponent<Renderer>() != null))
                    {
                        foreach (Renderer renderer in obj2.GetComponentsInChildren<Renderer>())
                        {
                            renderer.enabled = false;
                        }
                    }
                }
            }
            if (iteratorVariable2)
            {
                this.unloadAssets();
            }
        }

        /// <summary>
        /// This is used to ensure ping is only retrieved once a second. 
        /// </summary>
        [Obsolete("UI logic should be within the UI classes")]
        private float pingTimeLast = 0;

        [Obsolete("Cycolmatic complexity too high. Move into different classes and private methods")]
        private void LateUpdate()
        {
            if (((int) settings[0x40]) >= 100)
            {
                throw new NotImplementedException("Level editor is not implemented");
            }
            else
            {
                if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.Stop) return;
                if (this.needChooseSide)
                {
                    InGameUI.SpawnMenu.gameObject.SetActive(true);
                }

                // Update the players ping once every second. 
                float timeSince = Time.time * 1000;
                if (timeSince - pingTimeLast > 1000)
                {
                    pingTimeLast = timeSince;
                    var hashtable = new Hashtable
                    {
                        {PhotonPlayerProperty.ping, PhotonNetwork.GetPing().ToString()},
                    };
                    var propertiesToSet = hashtable;
                    PhotonNetwork.player.SetCustomProperties(propertiesToSet);
                }

                int length;
                if (!PhotonNetwork.offlineMode)
                {
                    this.coreadd();
                    Service.Ui.SetMessage(LabelPosition.TopLeft, playerList);
                    if ((((Camera.main != null) && (GameSettings.Gamemode.GamemodeType != GamemodeType.Racing)) &&
                         (Camera.main.GetComponent<IN_GAME_MAIN_CAMERA>().gameOver && !this.needChooseSide)) &&
                        SpectatorMode.IsDisable())
                    {
                        if (GameSettings.Respawn.Mode == RespawnMode.Endless ||
                            !(((GameSettings.PvP.Bomb.Value) || (GameSettings.PvP.Mode != PvpMode.Disabled))
                                ? (GameSettings.Gamemode.PointMode <= 0)
                                : true))
                        {
                            this.myRespawnTime += Time.deltaTime;
                            int endlessMode = 5;
                            if (RCextensions.returnIntFromObject(
                                    PhotonNetwork.player.CustomProperties[PhotonPlayerProperty.isTitan]) == 2)
                            {
                                endlessMode = 10;
                            }

                            if (GameSettings.Respawn.Mode == RespawnMode.Endless)
                            {
                                endlessMode = GameSettings.Respawn.ReviveTime.Value;
                            }

                            //TODO
                            //length = endlessMode - ((int) this.myRespawnTime);
                            //this.ShowHUDInfoCenterADD("Respawn in " + length.ToString() + "s.");
                            if (this.myRespawnTime > endlessMode)
                            {
                                this.myRespawnTime = 0f;
                                Camera.main.GetComponent<IN_GAME_MAIN_CAMERA>().gameOver = false;
                                if (RCextensions.returnIntFromObject(
                                        PhotonNetwork.player.CustomProperties[PhotonPlayerProperty.isTitan]) == 2)
                                {
                                    SpawnService.Spawn<PlayerTitan>();
                                }
                                else
                                {
                                    respawnCoroutine = StartCoroutine(WaitAndRespawn1(0.1f, myLastRespawnTag));
                                }
                                Camera.main.GetComponent<IN_GAME_MAIN_CAMERA>().gameOver = false;
                            }
                        }
                    }
                }

                if (GameSettings.Gamemode.GamemodeType == GamemodeType.Racing)
                {
                    if ((Camera.main.GetComponent<IN_GAME_MAIN_CAMERA>().gameOver && !this.needChooseSide) &&
                        SpectatorMode.IsDisable())
                    {
                        this.myRespawnTime += Time.deltaTime;
                        if (this.myRespawnTime > 1.5f)
                        {
                            this.myRespawnTime = 0f;
                            Camera.main.GetComponent<IN_GAME_MAIN_CAMERA>().gameOver = false;
                            if (this.checkpoint != null)
                            {
                                base.StartCoroutine(this.WaitAndRespawn2(0.1f, this.checkpoint));
                            }
                            else
                            {
                                base.StartCoroutine(this.WaitAndRespawn1(0.1f, this.myLastRespawnTag));
                            }

                            Camera.main.GetComponent<IN_GAME_MAIN_CAMERA>().gameOver = false;
                        }
                    }
                }

                //TODO: Move into Gamemode
                //if (this.timeElapse > 1f)
                //{
                //    this.timeElapse--;
                //    var content = Gamemode.GetGamemodeStatusTop((int) timeTotalServer, time);
                //    if (GameSettings.Gamemode.TeamMode != TeamMode.Disabled)
                //    {
                //        content +=
                //            $"\n<color=#00ffff>Cyan: {cyanKills}</color><color=#ff00ff>       Magenta: {magentaKills}</color>";
                //    }

                //    this.ShowHUDInfoTopCenter(content);
                //    content = Gamemode.GetGamemodeStatusTopRight((int) timeTotalServer, time);
                //    this.ShowHUDInfoTopRight(content);
                //    //TODO: Display difficulty
                //    //string str4 = Gamemode.Settings.Difficulty.ToString();
                //    //this.ShowHUDInfoTopRightMAPNAME("\n" + Level.Name + " : " + str4);
                //    char[] separator = new char[] { "`"[0] };
                //    string str5 = PhotonNetwork.room.name.Split(separator)[0];
                //    if (str5.Length > 20)
                //    {
                //        str5 = str5.Remove(0x13) + "...";
                //    }

                //    this.ShowHUDInfoTopRightMAPNAME("\n" + str5 + " [FFC000](" +
                //                                    Convert.ToString(PhotonNetwork.room.playerCount) + "/" +
                //                                    Convert.ToString(PhotonNetwork.room.maxPlayers) + ")");
                //    if (this.needChooseSide)
                //    {
                //        this.ShowHUDInfoTopCenterADD("\n\nPRESS 1 TO ENTER GAME");
                //    }
                //}

                if (this.killInfoGO.Count > 0 && this.killInfoGO[0] == null)
                {
                    this.killInfoGO.RemoveAt(0);
                }

                //TODO: Display the results when MC closes the room
                //if (PhotonNetwork.isMasterClient &&
                //    (this.timeTotalServer > this.time))
                //{
                //    string str11;
                //    IN_GAME_MAIN_CAMERA.gametype = GAMETYPE.Stop;
                //    string str6 = string.Empty;
                //    string str7 = string.Empty;
                //    string str8 = string.Empty;
                //    string str9 = string.Empty;
                //    string str10 = string.Empty;
                //    foreach (PhotonPlayer player in PhotonNetwork.playerList)
                //    {
                //        if (player != null)
                //        {
                //            str6 = str6 + player.CustomProperties[PhotonPlayerProperty.name] + "\n";
                //            str7 = str7 + player.CustomProperties[PhotonPlayerProperty.kills] + "\n";
                //            str8 = str8 + player.CustomProperties[PhotonPlayerProperty.deaths] + "\n";
                //            str9 = str9 + player.CustomProperties[PhotonPlayerProperty.max_dmg] + "\n";
                //            str10 = str10 + player.CustomProperties[PhotonPlayerProperty.total_dmg] + "\n";
                //        }
                //    }

                //    str11 = Gamemode.GetRoundEndedMessage();
                //    object[] parameters = new object[] {str6, str7, str8, str9, str10, str11};
                //    base.photonView.RPC(nameof(showResult), PhotonTargets.AllBuffered, parameters);
                //}
            }
        }

        [Obsolete("I think this was used to Reload the playerlist of the game was paused? This method is called on LateUpdate whereas the ReloadPlayerList is quite complex.")]
        private void coreadd()
        {
            if (Time.timeScale <= 0.1f)
            {
                this.ReloadPlayerlist();
            }
        }

        /// <summary>
        /// Will look for all Cloth objects and try to dispose them via the <see cref="ClothFactory.DisposeObject"/>
        /// </summary>
        public void DestroyAllExistingCloths()
        {
            Cloth[] clothArray = UnityEngine.Object.FindObjectsOfType<Cloth>();
            if (clothArray.Length > 0)
            {
                for (int i = 0; i < clothArray.Length; i++)
                {
                    ClothFactory.DisposeObject(clothArray[i].gameObject);
                }
            }
        }

        /// <summary>
        /// The master client will send a RPC to another players, telling them to ignore a specific player. Do we even need this in AoTTG2? Think this was needed in AoTTG as there wasn't a reliable way to kick a player
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="info"></param>
        [PunRPC]
        private void ignorePlayer(int ID, PhotonMessageInfo info)
        {
            if (info.sender.isMasterClient)
            {
                PhotonPlayer player = PhotonPlayer.Find(ID);
                if ((player != null) && !ignoreList.Contains(ID))
                {
                    for (int i = 0; i < PhotonNetwork.playerList.Length; i++)
                    {
                        if (PhotonNetwork.playerList[i] == player)
                        {
                            ignoreList.Add(ID);
                            RaiseEventOptions options = new RaiseEventOptions
                            {
                                TargetActors = new int[] { ID }
                            };
                            PhotonNetwork.RaiseEvent(0xfe, null, true, options);
                        }
                    }
                }
            }
            this.RecompilePlayerList(0.1f);
        }

        /// <summary>
        /// Same as <see cref="ignorePlayer"/> but then <paramref name="IDS"/> is an array.
        /// </summary>
        /// <param name="IDS"></param>
        /// <param name="info"></param>
        [PunRPC]
        private void ignorePlayerArray(int[] IDS, PhotonMessageInfo info)
        {
            if (info.sender.isMasterClient)
            {
                for (int i = 0; i < IDS.Length; i++)
                {
                    int iD = IDS[i];
                    PhotonPlayer player = PhotonPlayer.Find(iD);
                    if ((player != null) && !ignoreList.Contains(iD))
                    {
                        for (int j = 0; j < PhotonNetwork.playerList.Length; j++)
                        {
                            if (PhotonNetwork.playerList[j] == player)
                            {
                                ignoreList.Add(iD);
                                RaiseEventOptions options = new RaiseEventOptions
                                {
                                    TargetActors = new int[] { iD }
                                };
                                PhotonNetwork.RaiseEvent(0xfe, null, true, options);
                            }
                        }
                    }
                }
            }
            this.RecompilePlayerList(0.1f);
        }

        /// <summary>
        /// AoTTG1's way of reloading a player list which often is called every frame. Far from efficient. 
        /// </summary>
        [Obsolete("Highly inefficient and expensive method to create a player list. Refactor by using StringBuilder")]
        private void ReloadPlayerlist()
        {
            var playerList = "";
            foreach (PhotonPlayer player in PhotonNetwork.playerList)
            {
                if (ignoreList.Contains(player.ID))
                {
                    playerList += "<color=red>[X]</color> ";
                }
                playerList += "[" + Convert.ToString(player.ID) + "] ";
                if (player.IsMasterClient)
                {
                    playerList += "[M] ";
                }
                if (RCextensions.returnBoolFromObject(player.CustomProperties[PhotonPlayerProperty.dead]))
                {
                    playerList += "<color=red>*dead*</color> ";
                }
                if (RCextensions.returnIntFromObject(player.CustomProperties[PhotonPlayerProperty.isTitan]) < 2)
                {
                    var team = RCextensions.returnIntFromObject(player.CustomProperties[PhotonPlayerProperty.team]);
                    if (team < 2)
                    {
                        playerList += $"<color=#{ColorSet.color_human}> <H> </color> ";
                    }
                    else if (team == 2)
                    {
                        playerList += $"<color=#{ColorSet.color_human_1}> <A> </color> ";
                    }
                }
                else if (RCextensions.returnIntFromObject(player.CustomProperties[PhotonPlayerProperty.isTitan]) == 2)
                {
                    playerList += $"<color=#{ColorSet.color_titan_player}> <T> </color> ";
                }
                var name = RCextensions.returnStringFromObject(player.CustomProperties[PhotonPlayerProperty.name]);
                var kills = RCextensions.returnIntFromObject(player.CustomProperties[PhotonPlayerProperty.kills]);
                var deaths = RCextensions.returnIntFromObject(player.CustomProperties[PhotonPlayerProperty.deaths]);
                var maxDamage = RCextensions.returnIntFromObject(player.CustomProperties[PhotonPlayerProperty.max_dmg]);
                var totalDamage = RCextensions.returnIntFromObject(player.CustomProperties[PhotonPlayerProperty.total_dmg]);
                playerList += $"{name} {kills}/{deaths}/{maxDamage}/{totalDamage}";
                if (RCextensions.returnBoolFromObject(player.CustomProperties[PhotonPlayerProperty.dead]))
                {
                    playerList += "[-]";
                }
                playerList += "\n";
            }
            this.playerList = playerList;
        }

        /// <summary>
        /// A way too complicated method on how to kick a player, yet this was neccesary in AoTTG1. In AoTTG2, a kick will be enforced via the Photon Server.
        /// </summary>
        /// <param name="player"></param>
        /// <param name="ban"></param>
        /// <param name="reason"></param>
        [Obsolete("Kicking a player is simplified with the Photon Server")]
        public void kickPlayerRC(PhotonPlayer player, bool ban, string reason)
        {
            string str;
            if (OnPrivateServer)
            {
                str = string.Empty;
                str = RCextensions.returnStringFromObject(player.CustomProperties[PhotonPlayerProperty.name]);
                ServerCloseConnection(player, ban, str);
            }
            else
            {
                PhotonNetwork.DestroyPlayerObjects(player);
                PhotonNetwork.CloseConnection(player);
                base.photonView.RPC(nameof(ignorePlayer), PhotonTargets.Others, new object[] { player.ID });
                if (!ignoreList.Contains(player.ID))
                {
                    ignoreList.Add(player.ID);
                    RaiseEventOptions options = new RaiseEventOptions
                    {
                        TargetActors = new int[] { player.ID }
                    };
                    PhotonNetwork.RaiseEvent(0xfe, null, true, options);
                }
                if (!(!ban || banHash.ContainsKey(player.ID)))
                {
                    str = string.Empty;
                    str = RCextensions.returnStringFromObject(player.CustomProperties[PhotonPlayerProperty.name]);
                    banHash.Add(player.ID, str);
                }
                if (reason != string.Empty)
                {
                    this.chatRoom.AddMessage("Player " + player.ID.ToString() + " was autobanned. Reason:" + reason);
                }
                this.RecompilePlayerList(0.1f);
            }
        }

        [Obsolete("This is used to set the label of the HERO to a different team color. Move to HERO.cs and TeamService")]
        [PunRPC]
        private void labelRPC(int setting, PhotonMessageInfo info)
        {
            if (PhotonView.Find(setting) != null)
            {
                PhotonPlayer owner = PhotonView.Find(setting).owner;
                if (owner == info.sender)
                {
                    string str = RCextensions.returnStringFromObject(owner.CustomProperties[PhotonPlayerProperty.guildName]);
                    string str2 = RCextensions.returnStringFromObject(owner.CustomProperties[PhotonPlayerProperty.name]);
                    GameObject gameObject = PhotonView.Find(setting).gameObject;
                    if (gameObject != null)
                    {
                        Hero component = gameObject.GetComponent<Hero>();
                        if (component != null)
                        {
                            if (str != string.Empty)
                            {
                                //component.myNetWorkName.GetComponent<UILabel>().text = "[FFFF00]" + str + "\n[FFFFFF]" + str2;
                            }
                            else
                            {
                                //component.myNetWorkName.GetComponent<UILabel>().text = str2;
                            }
                        }
                    }
                }
            }
        }

        [Obsolete("Replaced by the GameSettings classes.")]
        private void loadconfig()
        {
            int num;
            int num2;
            object[] objArray = new object[270];
            objArray[0] = PlayerPrefs.GetInt("human", 1);
            objArray[1] = PlayerPrefs.GetInt("titan", 1);
            objArray[2] = PlayerPrefs.GetInt("level", 1);
            objArray[3] = PlayerPrefs.GetString("horse", string.Empty);
            objArray[4] = PlayerPrefs.GetString("hair", string.Empty);
            objArray[5] = PlayerPrefs.GetString("eye", string.Empty);
            objArray[6] = PlayerPrefs.GetString("glass", string.Empty);
            objArray[7] = PlayerPrefs.GetString("face", string.Empty);
            objArray[8] = PlayerPrefs.GetString("skin", string.Empty);
            objArray[9] = PlayerPrefs.GetString("costume", string.Empty);
            objArray[10] = PlayerPrefs.GetString("logo", string.Empty);
            objArray[11] = PlayerPrefs.GetString("bladel", string.Empty);
            objArray[12] = PlayerPrefs.GetString("blader", string.Empty);
            objArray[13] = PlayerPrefs.GetString("gas", string.Empty);
            objArray[14] = PlayerPrefs.GetString("hoodie", string.Empty);
            objArray[15] = PlayerPrefs.GetInt("gasenable", 0);
            objArray[0x10] = PlayerPrefs.GetInt("titantype1", -1);
            objArray[0x11] = PlayerPrefs.GetInt("titantype2", -1);
            objArray[0x12] = PlayerPrefs.GetInt("titantype3", -1);
            objArray[0x13] = PlayerPrefs.GetInt("titantype4", -1);
            objArray[20] = PlayerPrefs.GetInt("titantype5", -1);
            objArray[0x15] = PlayerPrefs.GetString("titanhair1", string.Empty);
            objArray[0x16] = PlayerPrefs.GetString("titanhair2", string.Empty);
            objArray[0x17] = PlayerPrefs.GetString("titanhair3", string.Empty);
            objArray[0x18] = PlayerPrefs.GetString("titanhair4", string.Empty);
            objArray[0x19] = PlayerPrefs.GetString("titanhair5", string.Empty);
            objArray[0x1a] = PlayerPrefs.GetString("titaneye1", string.Empty);
            objArray[0x1b] = PlayerPrefs.GetString("titaneye2", string.Empty);
            objArray[0x1c] = PlayerPrefs.GetString("titaneye3", string.Empty);
            objArray[0x1d] = PlayerPrefs.GetString("titaneye4", string.Empty);
            objArray[30] = PlayerPrefs.GetString("titaneye5", string.Empty);
            objArray[0x1f] = 0;
            objArray[0x20] = PlayerPrefs.GetInt("titanR", 0);
            objArray[0x21] = PlayerPrefs.GetString("tree1", "http://i.imgur.com/QhvQaOY.png");
            objArray[0x22] = PlayerPrefs.GetString("tree2", "http://i.imgur.com/QhvQaOY.png");
            objArray[0x23] = PlayerPrefs.GetString("tree3", "http://i.imgur.com/k08IX81.png");
            objArray[0x24] = PlayerPrefs.GetString("tree4", "http://i.imgur.com/k08IX81.png");
            objArray[0x25] = PlayerPrefs.GetString("tree5", "http://i.imgur.com/JQPNchU.png");
            objArray[0x26] = PlayerPrefs.GetString("tree6", "http://i.imgur.com/JQPNchU.png");
            objArray[0x27] = PlayerPrefs.GetString("tree7", "http://i.imgur.com/IZdYWv4.png");
            objArray[40] = PlayerPrefs.GetString("tree8", "http://i.imgur.com/IZdYWv4.png");
            objArray[0x29] = PlayerPrefs.GetString("leaf1", "http://i.imgur.com/oFGV5oL.png");
            objArray[0x2a] = PlayerPrefs.GetString("leaf2", "http://i.imgur.com/oFGV5oL.png");
            objArray[0x2b] = PlayerPrefs.GetString("leaf3", "http://i.imgur.com/mKzawrQ.png");
            objArray[0x2c] = PlayerPrefs.GetString("leaf4", "http://i.imgur.com/mKzawrQ.png");
            objArray[0x2d] = PlayerPrefs.GetString("leaf5", "http://i.imgur.com/Ymzavsi.png");
            objArray[0x2e] = PlayerPrefs.GetString("leaf6", "http://i.imgur.com/Ymzavsi.png");
            objArray[0x2f] = PlayerPrefs.GetString("leaf7", "http://i.imgur.com/oQfD1So.png");
            objArray[0x30] = PlayerPrefs.GetString("leaf8", "http://i.imgur.com/oQfD1So.png");
            objArray[0x31] = PlayerPrefs.GetString("forestG", "http://i.imgur.com/IsDTn7x.png");
            objArray[50] = PlayerPrefs.GetInt("forestR", 0);
            objArray[0x33] = PlayerPrefs.GetString("house1", "http://i.imgur.com/wuy77R8.png");
            objArray[0x34] = PlayerPrefs.GetString("house2", "http://i.imgur.com/wuy77R8.png");
            objArray[0x35] = PlayerPrefs.GetString("house3", "http://i.imgur.com/wuy77R8.png");
            objArray[0x36] = PlayerPrefs.GetString("house4", "http://i.imgur.com/wuy77R8.png");
            objArray[0x37] = PlayerPrefs.GetString("house5", "http://i.imgur.com/wuy77R8.png");
            objArray[0x38] = PlayerPrefs.GetString("house6", "http://i.imgur.com/wuy77R8.png");
            objArray[0x39] = PlayerPrefs.GetString("house7", "http://i.imgur.com/wuy77R8.png");
            objArray[0x3a] = PlayerPrefs.GetString("house8", "http://i.imgur.com/wuy77R8.png");
            objArray[0x3b] = PlayerPrefs.GetString("cityG", "http://i.imgur.com/Mr9ZXip.png");
            objArray[60] = PlayerPrefs.GetString("cityW", "http://i.imgur.com/Tm7XfQP.png");
            objArray[0x3d] = PlayerPrefs.GetString("cityH", "http://i.imgur.com/Q3YXkNM.png");
            objArray[0x3e] = PlayerPrefs.GetInt("skinQ", 0);
            objArray[0x3f] = PlayerPrefs.GetInt("skinQL", 0);
            objArray[0x40] = 0;
            objArray[0x41] = PlayerPrefs.GetString("eren", string.Empty);
            objArray[0x42] = PlayerPrefs.GetString("annie", string.Empty);
            objArray[0x43] = PlayerPrefs.GetString("colossal", string.Empty);
            objArray[0x44] = 100;
            objArray[0x45] = "default";
            objArray[70] = "1";
            objArray[0x47] = "1";
            objArray[0x48] = "1";
            objArray[0x49] = 1f;
            objArray[0x4a] = 1f;
            objArray[0x4b] = 1f;
            objArray[0x4c] = 0;
            objArray[0x4d] = string.Empty;
            objArray[0x4e] = 0;
            objArray[0x4f] = "1.0";
            objArray[80] = "1.0";
            objArray[0x51] = 0;
            objArray[0x52] = PlayerPrefs.GetString("cnumber", "1");
            objArray[0x53] = "30";
            objArray[0x54] = 0;
            objArray[0x55] = PlayerPrefs.GetString("cmax", "20");
            objArray[0x56] = PlayerPrefs.GetString("titanbody1", string.Empty);
            objArray[0x57] = PlayerPrefs.GetString("titanbody2", string.Empty);
            objArray[0x58] = PlayerPrefs.GetString("titanbody3", string.Empty);
            objArray[0x59] = PlayerPrefs.GetString("titanbody4", string.Empty);
            objArray[90] = PlayerPrefs.GetString("titanbody5", string.Empty);
            objArray[0x5b] = 0;
            objArray[0x5c] = PlayerPrefs.GetInt("traildisable", 0);
            objArray[0x5d] = PlayerPrefs.GetInt("wind", 0);
            objArray[0x5e] = PlayerPrefs.GetString("trailskin", string.Empty);
            objArray[0x5f] = PlayerPrefs.GetString("snapshot", "0");
            objArray[0x60] = PlayerPrefs.GetString("trailskin2", string.Empty);
            objArray[0x61] = PlayerPrefs.GetInt("reel", 0);
            objArray[0x62] = PlayerPrefs.GetString("reelin", "LeftControl");
            objArray[0x63] = PlayerPrefs.GetString("reelout", "LeftAlt");
            objArray[100] = 0;
            objArray[0x65] = PlayerPrefs.GetString("tforward", "W");
            objArray[0x66] = PlayerPrefs.GetString("tback", "S");
            objArray[0x67] = PlayerPrefs.GetString("tleft", "A");
            objArray[0x68] = PlayerPrefs.GetString("tright", "D");
            objArray[0x69] = PlayerPrefs.GetString("twalk", "LeftShift");
            objArray[0x6a] = PlayerPrefs.GetString("tjump", "Space");
            objArray[0x6b] = PlayerPrefs.GetString("tpunch", "Q");
            objArray[0x6c] = PlayerPrefs.GetString("tslam", "E");
            objArray[0x6d] = PlayerPrefs.GetString("tgrabfront", "Alpha1");
            objArray[110] = PlayerPrefs.GetString("tgrabback", "Alpha3");
            objArray[0x6f] = PlayerPrefs.GetString("tgrabnape", "Mouse1");
            objArray[0x70] = PlayerPrefs.GetString("tantiae", "Mouse0");
            objArray[0x71] = PlayerPrefs.GetString("tbite", "Alpha2");
            objArray[0x72] = PlayerPrefs.GetString("tcover", "Z");
            objArray[0x73] = PlayerPrefs.GetString("tsit", "X");
            objArray[0x74] = PlayerPrefs.GetInt("reel2", 0);
            objArray[0x75] = PlayerPrefs.GetString("lforward", "W");
            objArray[0x76] = PlayerPrefs.GetString("lback", "S");
            objArray[0x77] = PlayerPrefs.GetString("lleft", "A");
            objArray[120] = PlayerPrefs.GetString("lright", "D");
            objArray[0x79] = PlayerPrefs.GetString("lup", "Mouse1");
            objArray[0x7a] = PlayerPrefs.GetString("ldown", "Mouse0");
            objArray[0x7b] = PlayerPrefs.GetString("lcursor", "X");
            objArray[0x7c] = PlayerPrefs.GetString("lplace", "Space");
            objArray[0x7d] = PlayerPrefs.GetString("ldel", "Backspace");
            objArray[0x7e] = PlayerPrefs.GetString("lslow", "LeftShift");
            objArray[0x7f] = PlayerPrefs.GetString("lrforward", "R");
            objArray[0x80] = PlayerPrefs.GetString("lrback", "F");
            objArray[0x81] = PlayerPrefs.GetString("lrleft", "Q");
            objArray[130] = PlayerPrefs.GetString("lrright", "E");
            objArray[0x83] = PlayerPrefs.GetString("lrccw", "Z");
            objArray[0x84] = PlayerPrefs.GetString("lrcw", "C");
            objArray[0x85] = PlayerPrefs.GetInt("humangui", 0);
            objArray[0x86] = PlayerPrefs.GetString("horse2", string.Empty);
            objArray[0x87] = PlayerPrefs.GetString("hair2", string.Empty);
            objArray[0x88] = PlayerPrefs.GetString("eye2", string.Empty);
            objArray[0x89] = PlayerPrefs.GetString("glass2", string.Empty);
            objArray[0x8a] = PlayerPrefs.GetString("face2", string.Empty);
            objArray[0x8b] = PlayerPrefs.GetString("skin2", string.Empty);
            objArray[140] = PlayerPrefs.GetString("costume2", string.Empty);
            objArray[0x8d] = PlayerPrefs.GetString("logo2", string.Empty);
            objArray[0x8e] = PlayerPrefs.GetString("bladel2", string.Empty);
            objArray[0x8f] = PlayerPrefs.GetString("blader2", string.Empty);
            objArray[0x90] = PlayerPrefs.GetString("gas2", string.Empty);
            objArray[0x91] = PlayerPrefs.GetString("hoodie2", string.Empty);
            objArray[0x92] = PlayerPrefs.GetString("trail2", string.Empty);
            objArray[0x93] = PlayerPrefs.GetString("horse3", string.Empty);
            objArray[0x94] = PlayerPrefs.GetString("hair3", string.Empty);
            objArray[0x95] = PlayerPrefs.GetString("eye3", string.Empty);
            objArray[150] = PlayerPrefs.GetString("glass3", string.Empty);
            objArray[0x97] = PlayerPrefs.GetString("face3", string.Empty);
            objArray[0x98] = PlayerPrefs.GetString("skin3", string.Empty);
            objArray[0x99] = PlayerPrefs.GetString("costume3", string.Empty);
            objArray[0x9a] = PlayerPrefs.GetString("logo3", string.Empty);
            objArray[0x9b] = PlayerPrefs.GetString("bladel3", string.Empty);
            objArray[0x9c] = PlayerPrefs.GetString("blader3", string.Empty);
            objArray[0x9d] = PlayerPrefs.GetString("gas3", string.Empty);
            objArray[0x9e] = PlayerPrefs.GetString("hoodie3", string.Empty);
            objArray[0x9f] = PlayerPrefs.GetString("trail3", string.Empty);
            objArray[0xa1] = PlayerPrefs.GetString("lfast", "LeftControl");
            objArray[0xa2] = PlayerPrefs.GetString("customGround", string.Empty);
            objArray[0xa3] = PlayerPrefs.GetString("forestskyfront", string.Empty);
            objArray[0xa4] = PlayerPrefs.GetString("forestskyback", string.Empty);
            objArray[0xa5] = PlayerPrefs.GetString("forestskyleft", string.Empty);
            objArray[0xa6] = PlayerPrefs.GetString("forestskyright", string.Empty);
            objArray[0xa7] = PlayerPrefs.GetString("forestskyup", string.Empty);
            objArray[0xa8] = PlayerPrefs.GetString("forestskydown", string.Empty);
            objArray[0xa9] = PlayerPrefs.GetString("cityskyfront", string.Empty);
            objArray[170] = PlayerPrefs.GetString("cityskyback", string.Empty);
            objArray[0xab] = PlayerPrefs.GetString("cityskyleft", string.Empty);
            objArray[0xac] = PlayerPrefs.GetString("cityskyright", string.Empty);
            objArray[0xad] = PlayerPrefs.GetString("cityskyup", string.Empty);
            objArray[0xae] = PlayerPrefs.GetString("cityskydown", string.Empty);
            objArray[0xaf] = PlayerPrefs.GetString("customskyfront", string.Empty);
            objArray[0xb0] = PlayerPrefs.GetString("customskyback", string.Empty);
            objArray[0xb1] = PlayerPrefs.GetString("customskyleft", string.Empty);
            objArray[0xb2] = PlayerPrefs.GetString("customskyright", string.Empty);
            objArray[0xb3] = PlayerPrefs.GetString("customskyup", string.Empty);
            objArray[180] = PlayerPrefs.GetString("customskydown", string.Empty);
            objArray[0xb6] = PlayerPrefs.GetString("dashkey", "RightControl");
            objArray[0xb8] = PlayerPrefs.GetString("fpscap", "0");
            objArray[0xb9] = 0;
            objArray[0xba] = 0;
            objArray[0xbb] = 0;
            objArray[0xbc] = 0;
            objArray[0xbd] = PlayerPrefs.GetInt("speedometer", 0);
            objArray[190] = 0;
            objArray[0xbf] = string.Empty;
            objArray[0xc0] = PlayerPrefs.GetInt("bombMode", 0);
            objArray[0xc1] = PlayerPrefs.GetInt("teamMode", 0);
            objArray[0xc2] = PlayerPrefs.GetInt("rockThrow", 0);
            objArray[0xc3] = PlayerPrefs.GetInt("explodeModeOn", 0);
            objArray[0xc4] = PlayerPrefs.GetString("explodeModeNum", "30");
            objArray[0xc5] = PlayerPrefs.GetInt("healthMode", 0);
            objArray[0xc6] = PlayerPrefs.GetString("healthLower", "100");
            objArray[0xc7] = PlayerPrefs.GetString("healthUpper", "200");
            objArray[200] = PlayerPrefs.GetInt("infectionModeOn", 0);
            objArray[0xc9] = PlayerPrefs.GetString("infectionModeNum", "1");
            objArray[0xca] = PlayerPrefs.GetInt("banEren", 0);
            objArray[0xcb] = PlayerPrefs.GetInt("moreTitanOn", 0);
            objArray[0xcc] = PlayerPrefs.GetString("moreTitanNum", "1");
            objArray[0xcd] = PlayerPrefs.GetInt("damageModeOn", 0);
            objArray[0xce] = PlayerPrefs.GetString("damageModeNum", "1000");
            objArray[0xcf] = PlayerPrefs.GetInt("sizeMode", 0);
            objArray[0xd0] = PlayerPrefs.GetString("sizeLower", "1.0");
            objArray[0xd1] = PlayerPrefs.GetString("sizeUpper", "3.0");
            objArray[210] = PlayerPrefs.GetInt("spawnModeOn", 0);
            objArray[0xd3] = PlayerPrefs.GetString("nRate", "20.0");
            objArray[0xd4] = PlayerPrefs.GetString("aRate", "20.0");
            objArray[0xd5] = PlayerPrefs.GetString("jRate", "20.0");
            objArray[0xd6] = PlayerPrefs.GetString("cRate", "20.0");
            objArray[0xd7] = PlayerPrefs.GetString("pRate", "20.0");
            objArray[0xd8] = PlayerPrefs.GetInt("horseMode", 0);
            objArray[0xd9] = PlayerPrefs.GetInt("waveModeOn", 0);
            objArray[0xda] = PlayerPrefs.GetString("waveModeNum", "1");
            objArray[0xdb] = PlayerPrefs.GetInt("friendlyMode", 0);
            objArray[220] = PlayerPrefs.GetInt("pvpMode", 0);
            objArray[0xdd] = PlayerPrefs.GetInt("maxWaveOn", 0);
            objArray[0xde] = PlayerPrefs.GetString("maxWaveNum", "20");
            objArray[0xdf] = PlayerPrefs.GetInt("endlessModeOn", 0);
            objArray[0xe0] = PlayerPrefs.GetString("endlessModeNum", "10");
            objArray[0xe1] = PlayerPrefs.GetString("motd", string.Empty);
            objArray[0xe2] = PlayerPrefs.GetInt("pointModeOn", 0);
            objArray[0xe3] = PlayerPrefs.GetString("pointModeNum", "50");
            objArray[0xe4] = PlayerPrefs.GetInt("ahssReload", 0);
            objArray[0xe5] = PlayerPrefs.GetInt("punkWaves", 0);
            objArray[230] = 0;
            objArray[0xe7] = PlayerPrefs.GetInt("mapOn", 0);
            objArray[0xe8] = PlayerPrefs.GetString("mapMaximize", "Tab");
            objArray[0xe9] = PlayerPrefs.GetString("mapToggle", "M");
            objArray[0xea] = PlayerPrefs.GetString("mapReset", "K");
            objArray[0xeb] = PlayerPrefs.GetInt("globalDisableMinimap", 0);
            objArray[0xec] = PlayerPrefs.GetString("chatRebind", "None");
            objArray[0xed] = PlayerPrefs.GetString("hforward", "W");
            objArray[0xee] = PlayerPrefs.GetString("hback", "S");
            objArray[0xef] = PlayerPrefs.GetString("hleft", "A");
            objArray[240] = PlayerPrefs.GetString("hright", "D");
            objArray[0xf1] = PlayerPrefs.GetString("hwalk", "LeftShift");
            objArray[0xf2] = PlayerPrefs.GetString("hjump", "Q");
            objArray[0xf3] = PlayerPrefs.GetString("hmount", "LeftControl");
            objArray[0xf4] = PlayerPrefs.GetInt("chatfeed", 0);
            SpectatorMode.Initialize();
            objArray[0xf6] = PlayerPrefs.GetFloat("bombR", 1f);
            objArray[0xf7] = PlayerPrefs.GetFloat("bombG", 1f);
            objArray[0xf8] = PlayerPrefs.GetFloat("bombB", 1f);
            objArray[0xf9] = PlayerPrefs.GetFloat("bombA", 1f);
            objArray[250] = PlayerPrefs.GetInt("bombRadius", 5);
            objArray[0xfb] = PlayerPrefs.GetInt("bombRange", 5);
            objArray[0xfc] = PlayerPrefs.GetInt("bombSpeed", 5);
            objArray[0xfd] = PlayerPrefs.GetInt("bombCD", 5);
            objArray[0xfe] = PlayerPrefs.GetString("cannonUp", "W");
            objArray[0xff] = PlayerPrefs.GetString("cannonDown", "S");
            objArray[0x100] = PlayerPrefs.GetString("cannonLeft", "A");
            objArray[0x101] = PlayerPrefs.GetString("cannonRight", "D");
            objArray[0x102] = PlayerPrefs.GetString("cannonFire", "Q");
            objArray[0x103] = PlayerPrefs.GetString("cannonMount", "G");
            objArray[260] = PlayerPrefs.GetString("cannonSlow", "LeftShift");
            objArray[0x105] = PlayerPrefs.GetInt("deadlyCannon", 0);
            objArray[0x106] = PlayerPrefs.GetString("liveCam", "Y");
            objArray[0x107] = 0;
            if (!Enum.IsDefined(typeof(KeyCode), (string) objArray[0xe8]))
            {
                objArray[0xe8] = "None";
            }
            if (!Enum.IsDefined(typeof(KeyCode), (string) objArray[0xe9]))
            {
                objArray[0xe9] = "None";
            }
            if (!Enum.IsDefined(typeof(KeyCode), (string) objArray[0xea]))
            {
                objArray[0xea] = "None";
            }
            AudioListener.volume = PlayerPrefs.GetFloat("vol", 1f);
            linkHash = new ExitGames.Client.Photon.Hashtable[] { new ExitGames.Client.Photon.Hashtable(), new ExitGames.Client.Photon.Hashtable(), new ExitGames.Client.Photon.Hashtable(), new ExitGames.Client.Photon.Hashtable(), new ExitGames.Client.Photon.Hashtable() };
            settings = objArray;
        }

        [Obsolete("Too high complexity and obsolete UI Gameobjects. Refactor")]
        private void loadskin()
        {
            GameObject[] objArray;
            int num;
            GameObject obj2;
            if (((int) settings[0x40]) >= 100)
            {
                string[] strArray2 = new string[] { "Flare", "LabelInfoBottomRight", "LabelNetworkStatus", "skill_cd_bottom", "GasUI" };
                objArray = (GameObject[]) UnityEngine.Object.FindObjectsOfType(typeof(GameObject));
                for (num = 0; num < objArray.Length; num++)
                {
                    obj2 = objArray[num];
                    if ((obj2.name.Contains("TREE") || obj2.name.Contains("aot_supply")) || obj2.name.Contains("gameobjectOutSide"))
                    {
                        UnityEngine.Object.Destroy(obj2);
                    }
                }
                GameObject.Find("Cube_001").GetComponent<Renderer>().material.mainTexture = (RcLegacy.GetMaterial("grass")).mainTexture;
                UnityEngine.Object.Instantiate(RcLegacy.GetPrefab("spawnPlayer"), new Vector3(-10f, 1f, -10f), new Quaternion(0f, 0f, 0f, 1f));
                for (num = 0; num < strArray2.Length; num++)
                {
                    string name = strArray2[num];
                    GameObject obj3 = GameObject.Find(name);
                    if (obj3 != null)
                    {
                        UnityEngine.Object.Destroy(obj3);
                    }
                }
                SpectatorMode.Disable();
                Camera.main.GetComponent<Assets.Scripts.UI.Camera.MouseLook>().disable = true;
            }
            else
            {
                GameObject obj4;
                string[] strArray3;
                int num2;
                InstantiateTracker.instance.Dispose();
                if (PhotonNetwork.isMasterClient)
                {
                    oldScriptLogic = currentScriptLogic;
                    base.photonView.RPC(nameof(setMasterRC), PhotonTargets.All, new object[0]);
                }
                logicLoaded = true;
                this.racingSpawnPoint = new Vector3(0f, 0f, 0f);
                this.racingSpawnPointSet = false;
                this.allowedToCannon = new Dictionary<int, CannonValues>();
                //if ((!Level.Name.StartsWith("Custom") && (((int)settings[2]) == 1)) && ((IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE) || PhotonNetwork.isMasterClient))
                //{
                //    string url = string.Empty;
                //    string str3 = string.Empty;
                //    string n = string.Empty;
                //    strArray3 = new string[] { string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty };
                //    if (Level.SceneName.Contains("City"))
                //    {
                //        for (num = 0x33; num < 0x3b; num++)
                //        {
                //            url = url + ((string)settings[num]) + ",";
                //        }
                //        url.TrimEnd(new char[] { ',' });
                //        num2 = 0;
                //        while (num2 < 250)
                //        {
                //            n = n + Convert.ToString((int)UnityEngine.Random.Range((float)0f, (float)8f));
                //            num2++;
                //        }
                //        str3 = ((string)settings[0x3b]) + "," + ((string)settings[60]) + "," + ((string)settings[0x3d]);
                //        for (num = 0; num < 6; num++)
                //        {
                //            strArray3[num] = (string)settings[num + 0xa9];
                //        }
                //    }
                //    else if (Level.SceneName.Contains("Forest"))
                //    {
                //        for (int i = 0x21; i < 0x29; i++)
                //        {
                //            url = url + ((string)settings[i]) + ",";
                //        }
                //        url.TrimEnd(new char[] { ',' });
                //        for (int j = 0x29; j < 0x31; j++)
                //        {
                //            str3 = str3 + ((string)settings[j]) + ",";
                //        }
                //        str3 = str3 + ((string)settings[0x31]);
                //        for (int k = 0; k < 150; k++)
                //        {
                //            string str5 = Convert.ToString((int)UnityEngine.Random.Range((float)0f, (float)8f));
                //            n = n + str5;
                //            if (((int)settings[50]) == 0)
                //            {
                //                n = n + str5;
                //            }
                //            else
                //            {
                //                n = n + Convert.ToString((int)UnityEngine.Random.Range((float)0f, (float)8f));
                //            }
                //        }
                //        for (num = 0; num < 6; num++)
                //        {
                //            strArray3[num] = (string)settings[num + 0xa3];
                //        }
                //    }
                //    if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
                //    {
                //        base.StartCoroutine(this.loadskinE(n, url, str3, strArray3));
                //    }
                //    else if (PhotonNetwork.isMasterClient)
                //    {
                //        base.photonView.RPC(nameof(loadskinRPC), PhotonTargets.AllBuffered, new object[] { n, url, str3, strArray3 });
                //    }
                //}
                if (Level.Name.StartsWith("Custom"))
                {
                    GameObject[] objArray3 = GameObject.FindGameObjectsWithTag("playerRespawn");
                    for (num = 0; num < objArray3.Length; num++)
                    {
                        obj4 = objArray3[num];
                        obj4.transform.position = new Vector3(UnityEngine.Random.Range((float) -5f, (float) 5f), 0f, UnityEngine.Random.Range((float) -5f, (float) 5f));
                    }
                    objArray = (GameObject[]) UnityEngine.Object.FindObjectsOfType(typeof(GameObject));
                    for (num = 0; num < objArray.Length; num++)
                    {
                        obj2 = objArray[num];
                        if (obj2.name.Contains("TREE") || obj2.name.Contains("aot_supply"))
                        {
                            UnityEngine.Object.Destroy(obj2);
                        }
                        else if (((obj2.name == "Cube_001") && (obj2.transform.parent.gameObject.tag != "player")) && (obj2.GetComponent<Renderer>() != null))
                        {
                            this.groundList.Add(obj2);
                            obj2.GetComponent<Renderer>().material.mainTexture = RcLegacy.GetMaterial("grass").mainTexture;
                        }
                    }
                    if (PhotonNetwork.isMasterClient)
                    {
                        int num6;
                        strArray3 = new string[] { string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty };
                        for (num = 0; num < 6; num++)
                        {
                            strArray3[num] = (string) settings[num + 0xaf];
                        }
                        strArray3[6] = (string) settings[0xa2];
                        base.photonView.RPC(nameof(clearlevel), PhotonTargets.AllBuffered, new object[] { strArray3 });
                        if (oldScript != currentScript)
                        {
                            ExitGames.Client.Photon.Hashtable hashtable;
                            this.levelCache.Clear();
                            this.playerSpawnsC.Clear();
                            this.playerSpawnsM.Clear();
                            currentLevel = string.Empty;
                            if (currentScript == string.Empty)
                            {
                                hashtable = new ExitGames.Client.Photon.Hashtable();
                                hashtable.Add(PhotonPlayerProperty.currentLevel, currentLevel);
                                PhotonNetwork.player.SetCustomProperties(hashtable);
                                oldScript = currentScript;
                            }
                            else
                            {
                                string[] strArray4 = Regex.Replace(currentScript, @"\s+", "").Replace("\r\n", "").Replace("\n", "").Replace("\r", "").Split(new char[] { ';' });
                                for (num = 0; num < (Mathf.FloorToInt((float) ((strArray4.Length - 1) / 100)) + 1); num++)
                                {
                                    string[] strArray5;
                                    int num7;
                                    string[] strArray6;
                                    string str6;
                                    if (num < Mathf.FloorToInt((float) (strArray4.Length / 100)))
                                    {
                                        strArray5 = new string[0x65];
                                        num7 = 0;
                                        num2 = 100 * num;
                                        while (num2 < ((100 * num) + 100))
                                        {
                                            if (strArray4[num2].StartsWith("spawnpoint"))
                                            {
                                                strArray6 = strArray4[num2].Split(new char[] { ',' });
                                                if (strArray6[1] == "titan")
                                                {
                                                    Instantiate(RcLegacy.GetPrefab("titanRespawn"), new Vector3(Convert.ToSingle(strArray6[2]), Convert.ToSingle(strArray6[3]), Convert.ToSingle(strArray6[4])), new Quaternion());
                                                }
                                                else if (strArray6[1] == "playerC")
                                                {
                                                    this.playerSpawnsC.Add(new Vector3(Convert.ToSingle(strArray6[2]), Convert.ToSingle(strArray6[3]), Convert.ToSingle(strArray6[4])));
                                                }
                                                else if (strArray6[1] == "playerM")
                                                {
                                                    this.playerSpawnsM.Add(new Vector3(Convert.ToSingle(strArray6[2]), Convert.ToSingle(strArray6[3]), Convert.ToSingle(strArray6[4])));
                                                }
                                            }
                                            strArray5[num7] = strArray4[num2];
                                            num7++;
                                            num2++;
                                        }
                                        str6 = UnityEngine.Random.Range(0x2710, 0x1869f).ToString();
                                        strArray5[100] = str6;
                                        currentLevel = currentLevel + str6;
                                        this.levelCache.Add(strArray5);
                                    }
                                    else
                                    {
                                        strArray5 = new string[(strArray4.Length % 100) + 1];
                                        num7 = 0;
                                        for (num2 = 100 * num; num2 < ((100 * num) + (strArray4.Length % 100)); num2++)
                                        {
                                            if (strArray4[num2].StartsWith("spawnpoint"))
                                            {
                                                strArray6 = strArray4[num2].Split(new char[] { ',' });
                                                if (strArray6[1] == "titan")
                                                {
                                                    Instantiate(RcLegacy.GetPrefab("titanRespawn"), new Vector3(Convert.ToSingle(strArray6[2]), Convert.ToSingle(strArray6[3]), Convert.ToSingle(strArray6[4])), new Quaternion());
                                                }
                                                else if (strArray6[1] == "playerC")
                                                {
                                                    this.playerSpawnsC.Add(new Vector3(Convert.ToSingle(strArray6[2]), Convert.ToSingle(strArray6[3]), Convert.ToSingle(strArray6[4])));
                                                }
                                                else if (strArray6[1] == "playerM")
                                                {
                                                    this.playerSpawnsM.Add(new Vector3(Convert.ToSingle(strArray6[2]), Convert.ToSingle(strArray6[3]), Convert.ToSingle(strArray6[4])));
                                                }
                                            }
                                            strArray5[num7] = strArray4[num2];
                                            num7++;
                                        }
                                        str6 = UnityEngine.Random.Range(0x2710, 0x1869f).ToString();
                                        strArray5[strArray4.Length % 100] = str6;
                                        currentLevel = currentLevel + str6;
                                        this.levelCache.Add(strArray5);
                                    }
                                }
                                List<string> list = new List<string>();
                                foreach (Vector3 vector in this.playerSpawnsC)
                                {
                                    list.Add("playerC," + vector.x.ToString() + "," + vector.y.ToString() + "," + vector.z.ToString());
                                }
                                foreach (Vector3 vector in this.playerSpawnsM)
                                {
                                    list.Add("playerM," + vector.x.ToString() + "," + vector.y.ToString() + "," + vector.z.ToString());
                                }
                                string item = "a" + UnityEngine.Random.Range(0x2710, 0x1869f).ToString();
                                list.Add(item);
                                currentLevel = item + currentLevel;
                                this.levelCache.Insert(0, list.ToArray());
                                string str8 = "z" + UnityEngine.Random.Range(0x2710, 0x1869f).ToString();
                                this.levelCache.Add(new string[] { str8 });
                                currentLevel = currentLevel + str8;
                                hashtable = new ExitGames.Client.Photon.Hashtable();
                                hashtable.Add(PhotonPlayerProperty.currentLevel, currentLevel);
                                PhotonNetwork.player.SetCustomProperties(hashtable);
                                oldScript = currentScript;
                            }
                        }
                        for (num = 0; num < PhotonNetwork.playerList.Length; num++)
                        {
                            PhotonPlayer player = PhotonNetwork.playerList[num];
                            if (!player.isMasterClient)
                            {
                                this.playersRPC.Add(player);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Legacy IEnumerator for <see cref="loadskin"/>. It was used in AoTTG to replace the skybox, and certain textures from the City and Forest maps. No longer used thus obsolete.
        /// </summary>
        /// <param name="n"></param>
        /// <param name="url"></param>
        /// <param name="url2"></param>
        /// <param name="skybox"></param>
        /// <returns></returns>
        [Obsolete("Legacy method to replace certain level skins.")]
        private IEnumerator loadskinE(string n, string url, string url2, string[] skybox)
        {
            bool mipmap = true;
            bool iteratorVariable1 = false;
            if (((int) settings[0x3f]) == 1)
            {
                mipmap = false;
            }
            if ((skybox.Length > 5) && (((((skybox[0] != string.Empty) || (skybox[1] != string.Empty)) || ((skybox[2] != string.Empty) || (skybox[3] != string.Empty))) || (skybox[4] != string.Empty)) || (skybox[5] != string.Empty)))
            {
                string key = string.Join(",", skybox);
                if (!linkHash[1].ContainsKey(key))
                {
                    iteratorVariable1 = true;
                    Material material = Camera.main.GetComponent<Skybox>().material;
                    string iteratorVariable4 = skybox[0];
                    string iteratorVariable5 = skybox[1];
                    string iteratorVariable6 = skybox[2];
                    string iteratorVariable7 = skybox[3];
                    string iteratorVariable8 = skybox[4];
                    string iteratorVariable9 = skybox[5];
                    if ((iteratorVariable4.EndsWith(".jpg") || iteratorVariable4.EndsWith(".png")) || iteratorVariable4.EndsWith(".jpeg"))
                    {
                        WWW link = new WWW(iteratorVariable4);
                        yield return link;
                        Texture2D texture = RCextensions.loadimage(link, mipmap, 0x7a120);
                        link.Dispose();
                        texture.wrapMode = TextureWrapMode.Clamp;
                        material.SetTexture("_FrontTex", texture);
                    }
                    if ((iteratorVariable5.EndsWith(".jpg") || iteratorVariable5.EndsWith(".png")) || iteratorVariable5.EndsWith(".jpeg"))
                    {
                        WWW iteratorVariable12 = new WWW(iteratorVariable5);
                        yield return iteratorVariable12;
                        Texture2D iteratorVariable13 = RCextensions.loadimage(iteratorVariable12, mipmap, 0x7a120);
                        iteratorVariable12.Dispose();
                        iteratorVariable13.wrapMode = TextureWrapMode.Clamp;
                        material.SetTexture("_BackTex", iteratorVariable13);
                    }
                    if ((iteratorVariable6.EndsWith(".jpg") || iteratorVariable6.EndsWith(".png")) || iteratorVariable6.EndsWith(".jpeg"))
                    {
                        WWW iteratorVariable14 = new WWW(iteratorVariable6);
                        yield return iteratorVariable14;
                        Texture2D iteratorVariable15 = RCextensions.loadimage(iteratorVariable14, mipmap, 0x7a120);
                        iteratorVariable14.Dispose();
                        iteratorVariable15.wrapMode = TextureWrapMode.Clamp;
                        material.SetTexture("_LeftTex", iteratorVariable15);
                    }
                    if ((iteratorVariable7.EndsWith(".jpg") || iteratorVariable7.EndsWith(".png")) || iteratorVariable7.EndsWith(".jpeg"))
                    {
                        WWW iteratorVariable16 = new WWW(iteratorVariable7);
                        yield return iteratorVariable16;
                        Texture2D iteratorVariable17 = RCextensions.loadimage(iteratorVariable16, mipmap, 0x7a120);
                        iteratorVariable16.Dispose();
                        iteratorVariable17.wrapMode = TextureWrapMode.Clamp;
                        material.SetTexture("_RightTex", iteratorVariable17);
                    }
                    if ((iteratorVariable8.EndsWith(".jpg") || iteratorVariable8.EndsWith(".png")) || iteratorVariable8.EndsWith(".jpeg"))
                    {
                        WWW iteratorVariable18 = new WWW(iteratorVariable8);
                        yield return iteratorVariable18;
                        Texture2D iteratorVariable19 = RCextensions.loadimage(iteratorVariable18, mipmap, 0x7a120);
                        iteratorVariable18.Dispose();
                        iteratorVariable19.wrapMode = TextureWrapMode.Clamp;
                        material.SetTexture("_UpTex", iteratorVariable19);
                    }
                    if ((iteratorVariable9.EndsWith(".jpg") || iteratorVariable9.EndsWith(".png")) || iteratorVariable9.EndsWith(".jpeg"))
                    {
                        WWW iteratorVariable20 = new WWW(iteratorVariable9);
                        yield return iteratorVariable20;
                        Texture2D iteratorVariable21 = RCextensions.loadimage(iteratorVariable20, mipmap, 0x7a120);
                        iteratorVariable20.Dispose();
                        iteratorVariable21.wrapMode = TextureWrapMode.Clamp;
                        material.SetTexture("_DownTex", iteratorVariable21);
                    }
                    Camera.main.GetComponent<Skybox>().material = material;
                    skyMaterial = material;
                    linkHash[1].Add(key, material);
                }
                else
                {
                    Camera.main.GetComponent<Skybox>().material = (Material) linkHash[1][key];
                    skyMaterial = (Material) linkHash[1][key];
                }
            }
            if (Level.SceneName.Contains("Forest"))
            {
                string[] iteratorVariable22 = url.Split(new char[] { ',' });
                string[] iteratorVariable23 = url2.Split(new char[] { ',' });
                int startIndex = 0;
                object[] iteratorVariable25 = UnityEngine.Object.FindObjectsOfType(typeof(GameObject));
                foreach (GameObject iteratorVariable26 in iteratorVariable25)
                {
                    if (iteratorVariable26 != null)
                    {
                        if (iteratorVariable26.name.Contains("TREE") && (n.Length > (startIndex + 1)))
                        {
                            int iteratorVariable28;
                            int iteratorVariable27;
                            string s = n.Substring(startIndex, 1);
                            string iteratorVariable30 = n.Substring(startIndex + 1, 1);
                            if ((((int.TryParse(s, out iteratorVariable27) && int.TryParse(iteratorVariable30, out iteratorVariable28)) && ((iteratorVariable27 >= 0) && (iteratorVariable27 < 8))) && (((iteratorVariable28 >= 0) && (iteratorVariable28 < 8)) && ((iteratorVariable22.Length >= 8) && (iteratorVariable23.Length >= 8)))) && ((iteratorVariable22[iteratorVariable27] != null) && (iteratorVariable23[iteratorVariable28] != null)))
                            {
                                string iteratorVariable31 = iteratorVariable22[iteratorVariable27];
                                string iteratorVariable32 = iteratorVariable23[iteratorVariable28];
                                foreach (Renderer iteratorVariable33 in iteratorVariable26.GetComponentsInChildren<Renderer>())
                                {
                                    if (iteratorVariable33.name.Contains(FengGameManagerMKII.s[0x16]))
                                    {
                                        if ((iteratorVariable31.EndsWith(".jpg") || iteratorVariable31.EndsWith(".png")) || iteratorVariable31.EndsWith(".jpeg"))
                                        {
                                            if (!linkHash[2].ContainsKey(iteratorVariable31))
                                            {
                                                WWW iteratorVariable34 = new WWW(iteratorVariable31);
                                                yield return iteratorVariable34;
                                                Texture2D iteratorVariable35 = RCextensions.loadimage(iteratorVariable34, mipmap, 0xf4240);
                                                iteratorVariable34.Dispose();
                                                if (!linkHash[2].ContainsKey(iteratorVariable31))
                                                {
                                                    iteratorVariable1 = true;
                                                    iteratorVariable33.material.mainTexture = iteratorVariable35;
                                                    linkHash[2].Add(iteratorVariable31, iteratorVariable33.material);
                                                    iteratorVariable33.material = (Material) linkHash[2][iteratorVariable31];
                                                }
                                                else
                                                {
                                                    iteratorVariable33.material = (Material) linkHash[2][iteratorVariable31];
                                                }
                                            }
                                            else
                                            {
                                                iteratorVariable33.material = (Material) linkHash[2][iteratorVariable31];
                                            }
                                        }
                                    }
                                    else if (iteratorVariable33.name.Contains(FengGameManagerMKII.s[0x17]))
                                    {
                                        if ((iteratorVariable32.EndsWith(".jpg") || iteratorVariable32.EndsWith(".png")) || iteratorVariable32.EndsWith(".jpeg"))
                                        {
                                            if (!linkHash[0].ContainsKey(iteratorVariable32))
                                            {
                                                WWW iteratorVariable36 = new WWW(iteratorVariable32);
                                                yield return iteratorVariable36;
                                                Texture2D iteratorVariable37 = RCextensions.loadimage(iteratorVariable36, mipmap, 0x30d40);
                                                iteratorVariable36.Dispose();
                                                if (!linkHash[0].ContainsKey(iteratorVariable32))
                                                {
                                                    iteratorVariable1 = true;
                                                    iteratorVariable33.material.mainTexture = iteratorVariable37;
                                                    linkHash[0].Add(iteratorVariable32, iteratorVariable33.material);
                                                    iteratorVariable33.material = (Material) linkHash[0][iteratorVariable32];
                                                }
                                                else
                                                {
                                                    iteratorVariable33.material = (Material) linkHash[0][iteratorVariable32];
                                                }
                                            }
                                            else
                                            {
                                                iteratorVariable33.material = (Material) linkHash[0][iteratorVariable32];
                                            }
                                        }
                                        else if (iteratorVariable32.ToLower() == "transparent")
                                        {
                                            iteratorVariable33.enabled = false;
                                        }
                                    }
                                }
                            }
                            startIndex += 2;
                        }
                        else if ((iteratorVariable26.name.Contains("Cube_001") && (iteratorVariable26.transform.parent.gameObject.tag != "Player")) && ((iteratorVariable23.Length > 8) && (iteratorVariable23[8] != null)))
                        {
                            string iteratorVariable38 = iteratorVariable23[8];
                            if ((iteratorVariable38.EndsWith(".jpg") || iteratorVariable38.EndsWith(".png")) || iteratorVariable38.EndsWith(".jpeg"))
                            {
                                foreach (Renderer iteratorVariable39 in iteratorVariable26.GetComponentsInChildren<Renderer>())
                                {
                                    if (!linkHash[0].ContainsKey(iteratorVariable38))
                                    {
                                        WWW iteratorVariable40 = new WWW(iteratorVariable38);
                                        yield return iteratorVariable40;
                                        Texture2D iteratorVariable41 = RCextensions.loadimage(iteratorVariable40, mipmap, 0x30d40);
                                        iteratorVariable40.Dispose();
                                        if (!linkHash[0].ContainsKey(iteratorVariable38))
                                        {
                                            iteratorVariable1 = true;
                                            iteratorVariable39.material.mainTexture = iteratorVariable41;
                                            linkHash[0].Add(iteratorVariable38, iteratorVariable39.material);
                                            iteratorVariable39.material = (Material) linkHash[0][iteratorVariable38];
                                        }
                                        else
                                        {
                                            iteratorVariable39.material = (Material) linkHash[0][iteratorVariable38];
                                        }
                                    }
                                    else
                                    {
                                        iteratorVariable39.material = (Material) linkHash[0][iteratorVariable38];
                                    }
                                }
                            }
                            else if (iteratorVariable38.ToLower() == "transparent")
                            {
                                foreach (Renderer renderer in iteratorVariable26.GetComponentsInChildren<Renderer>())
                                {
                                    renderer.enabled = false;
                                }
                            }
                        }
                    }
                }
            }
            else if (Level.SceneName.Contains("City"))
            {
                string[] iteratorVariable42 = url.Split(new char[] { ',' });
                string[] iteratorVariable43 = url2.Split(new char[] { ',' });
                string iteratorVariable44 = iteratorVariable43[2];
                int iteratorVariable45 = 0;
                object[] iteratorVariable46 = UnityEngine.Object.FindObjectsOfType(typeof(GameObject));
                foreach (GameObject iteratorVariable47 in iteratorVariable46)
                {
                    if ((iteratorVariable47 != null) && (iteratorVariable47.name.Contains("Cube_") && (iteratorVariable47.transform.parent.gameObject.tag != "Player")))
                    {
                        if (iteratorVariable47.name.EndsWith("001"))
                        {
                            if ((iteratorVariable43.Length > 0) && (iteratorVariable43[0] != null))
                            {
                                string iteratorVariable48 = iteratorVariable43[0];
                                if ((iteratorVariable48.EndsWith(".jpg") || iteratorVariable48.EndsWith(".png")) || iteratorVariable48.EndsWith(".jpeg"))
                                {
                                    foreach (Renderer iteratorVariable49 in iteratorVariable47.GetComponentsInChildren<Renderer>())
                                    {
                                        if (!linkHash[0].ContainsKey(iteratorVariable48))
                                        {
                                            WWW iteratorVariable50 = new WWW(iteratorVariable48);
                                            yield return iteratorVariable50;
                                            Texture2D iteratorVariable51 = RCextensions.loadimage(iteratorVariable50, mipmap, 0x30d40);
                                            iteratorVariable50.Dispose();
                                            if (!linkHash[0].ContainsKey(iteratorVariable48))
                                            {
                                                iteratorVariable1 = true;
                                                iteratorVariable49.material.mainTexture = iteratorVariable51;
                                                linkHash[0].Add(iteratorVariable48, iteratorVariable49.material);
                                                iteratorVariable49.material = (Material) linkHash[0][iteratorVariable48];
                                            }
                                            else
                                            {
                                                iteratorVariable49.material = (Material) linkHash[0][iteratorVariable48];
                                            }
                                        }
                                        else
                                        {
                                            iteratorVariable49.material = (Material) linkHash[0][iteratorVariable48];
                                        }
                                    }
                                }
                                else if (iteratorVariable48.ToLower() == "transparent")
                                {
                                    foreach (Renderer renderer in iteratorVariable47.GetComponentsInChildren<Renderer>())
                                    {
                                        renderer.enabled = false;
                                    }
                                }
                            }
                        }
                        else if (((iteratorVariable47.name.EndsWith("006") || iteratorVariable47.name.EndsWith("007")) || (iteratorVariable47.name.EndsWith("015") || iteratorVariable47.name.EndsWith("000"))) || ((iteratorVariable47.name.EndsWith("002") && (iteratorVariable47.transform.position.x == 0f)) && ((iteratorVariable47.transform.position.y == 0f) && (iteratorVariable47.transform.position.z == 0f))))
                        {
                            if ((iteratorVariable43.Length > 0) && (iteratorVariable43[1] != null))
                            {
                                string iteratorVariable52 = iteratorVariable43[1];
                                if ((iteratorVariable52.EndsWith(".jpg") || iteratorVariable52.EndsWith(".png")) || iteratorVariable52.EndsWith(".jpeg"))
                                {
                                    foreach (Renderer iteratorVariable53 in iteratorVariable47.GetComponentsInChildren<Renderer>())
                                    {
                                        if (!linkHash[0].ContainsKey(iteratorVariable52))
                                        {
                                            WWW iteratorVariable54 = new WWW(iteratorVariable52);
                                            yield return iteratorVariable54;
                                            Texture2D iteratorVariable55 = RCextensions.loadimage(iteratorVariable54, mipmap, 0x30d40);
                                            iteratorVariable54.Dispose();
                                            if (!linkHash[0].ContainsKey(iteratorVariable52))
                                            {
                                                iteratorVariable1 = true;
                                                iteratorVariable53.material.mainTexture = iteratorVariable55;
                                                linkHash[0].Add(iteratorVariable52, iteratorVariable53.material);
                                                iteratorVariable53.material = (Material) linkHash[0][iteratorVariable52];
                                            }
                                            else
                                            {
                                                iteratorVariable53.material = (Material) linkHash[0][iteratorVariable52];
                                            }
                                        }
                                        else
                                        {
                                            iteratorVariable53.material = (Material) linkHash[0][iteratorVariable52];
                                        }
                                    }
                                }
                            }
                        }
                        else if ((iteratorVariable47.name.EndsWith("005") || iteratorVariable47.name.EndsWith("003")) || ((iteratorVariable47.name.EndsWith("002") && (((iteratorVariable47.transform.position.x != 0f) || (iteratorVariable47.transform.position.y != 0f)) || (iteratorVariable47.transform.position.z != 0f))) && (n.Length > iteratorVariable45)))
                        {
                            int iteratorVariable56;
                            string iteratorVariable57 = n.Substring(iteratorVariable45, 1);
                            if (((int.TryParse(iteratorVariable57, out iteratorVariable56) && (iteratorVariable56 >= 0)) && ((iteratorVariable56 < 8) && (iteratorVariable42.Length >= 8))) && (iteratorVariable42[iteratorVariable56] != null))
                            {
                                string iteratorVariable58 = iteratorVariable42[iteratorVariable56];
                                if ((iteratorVariable58.EndsWith(".jpg") || iteratorVariable58.EndsWith(".png")) || iteratorVariable58.EndsWith(".jpeg"))
                                {
                                    foreach (Renderer iteratorVariable59 in iteratorVariable47.GetComponentsInChildren<Renderer>())
                                    {
                                        if (!linkHash[2].ContainsKey(iteratorVariable58))
                                        {
                                            WWW iteratorVariable60 = new WWW(iteratorVariable58);
                                            yield return iteratorVariable60;
                                            Texture2D iteratorVariable61 = RCextensions.loadimage(iteratorVariable60, mipmap, 0xf4240);
                                            iteratorVariable60.Dispose();
                                            if (!linkHash[2].ContainsKey(iteratorVariable58))
                                            {
                                                iteratorVariable1 = true;
                                                iteratorVariable59.material.mainTexture = iteratorVariable61;
                                                linkHash[2].Add(iteratorVariable58, iteratorVariable59.material);
                                                iteratorVariable59.material = (Material) linkHash[2][iteratorVariable58];
                                            }
                                            else
                                            {
                                                iteratorVariable59.material = (Material) linkHash[2][iteratorVariable58];
                                            }
                                        }
                                        else
                                        {
                                            iteratorVariable59.material = (Material) linkHash[2][iteratorVariable58];
                                        }
                                    }
                                }
                            }
                            iteratorVariable45++;
                        }
                        else if ((iteratorVariable47.name.EndsWith("019") || iteratorVariable47.name.EndsWith("020")) && ((iteratorVariable43.Length > 2) && (iteratorVariable43[2] != null)))
                        {
                            string iteratorVariable62 = iteratorVariable43[2];
                            if ((iteratorVariable62.EndsWith(".jpg") || iteratorVariable62.EndsWith(".png")) || iteratorVariable62.EndsWith(".jpeg"))
                            {
                                foreach (Renderer iteratorVariable63 in iteratorVariable47.GetComponentsInChildren<Renderer>())
                                {
                                    if (!linkHash[2].ContainsKey(iteratorVariable62))
                                    {
                                        WWW iteratorVariable64 = new WWW(iteratorVariable62);
                                        yield return iteratorVariable64;
                                        Texture2D iteratorVariable65 = RCextensions.loadimage(iteratorVariable64, mipmap, 0xf4240);
                                        iteratorVariable64.Dispose();
                                        if (!linkHash[2].ContainsKey(iteratorVariable62))
                                        {
                                            iteratorVariable1 = true;
                                            iteratorVariable63.material.mainTexture = iteratorVariable65;
                                            linkHash[2].Add(iteratorVariable62, iteratorVariable63.material);
                                            iteratorVariable63.material = (Material) linkHash[2][iteratorVariable62];
                                        }
                                        else
                                        {
                                            iteratorVariable63.material = (Material) linkHash[2][iteratorVariable62];
                                        }
                                    }
                                    else
                                    {
                                        iteratorVariable63.material = (Material) linkHash[2][iteratorVariable62];
                                    }
                                }
                            }
                        }
                    }
                }
            }
            if (iteratorVariable1)
            {
                this.unloadAssets();
            }
        }

        [PunRPC]
        [Obsolete("The RPC for the obsolete loadskin and loadSkinE methods")]
        private void loadskinRPC(string n, string url, string url2, string[] skybox, PhotonMessageInfo info)
        {
            if ((((int) settings[2]) == 1) && info.sender.isMasterClient)
            {
                base.StartCoroutine(this.loadskinE(n, url, url2, skybox));
            }
        }

        [PunRPC]
        [Obsolete("A RPC which is used to display the damage that was done to an enemy, yet UI related logic shouldn't be in this class. Move to UI classes.")]
        public void netShowDamage(int damage)
        {
            InGameUI.HUD.SetDamage(damage);
        }

        [Obsolete("Use RespawnService instead")]
        public void NOTSpawnPlayer(string id = "2")
        {
            this.myLastHero = id.ToUpper();
            ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
            hashtable.Add("dead", true);
            ExitGames.Client.Photon.Hashtable propertiesToSet = hashtable;
            PhotonNetwork.player.SetCustomProperties(propertiesToSet);
            hashtable = new ExitGames.Client.Photon.Hashtable();
            hashtable.Add(PhotonPlayerProperty.isTitan, 1);
            propertiesToSet = hashtable;
            PhotonNetwork.player.SetCustomProperties(propertiesToSet);
            GameObject.Find("MainCamera").GetComponent<IN_GAME_MAIN_CAMERA>().enabled = true;
            GameObject.Find("MainCamera").GetComponent<IN_GAME_MAIN_CAMERA>().SetMainObject(null, true, false);
            GameObject.Find("MainCamera").GetComponent<IN_GAME_MAIN_CAMERA>().SetSpectorMode(true);
            GameObject.Find("MainCamera").GetComponent<IN_GAME_MAIN_CAMERA>().gameOver = true;
        }

        [Obsolete("Use RespawnService instead")]
        public void NOTSpawnPlayerRC(string id)
        {
            this.myLastHero = id.ToUpper();
            ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
            hashtable.Add("dead", true);
            ExitGames.Client.Photon.Hashtable propertiesToSet = hashtable;
            PhotonNetwork.player.SetCustomProperties(propertiesToSet);
            hashtable = new ExitGames.Client.Photon.Hashtable();
            hashtable.Add(PhotonPlayerProperty.isTitan, 1);
            propertiesToSet = hashtable;
            PhotonNetwork.player.SetCustomProperties(propertiesToSet);
            GameObject.Find("MainCamera").GetComponent<IN_GAME_MAIN_CAMERA>().enabled = true;
            GameObject.Find("MainCamera").GetComponent<IN_GAME_MAIN_CAMERA>().SetMainObject(null, true, false);
            GameObject.Find("MainCamera").GetComponent<IN_GAME_MAIN_CAMERA>().SetSpectorMode(true);
            GameObject.Find("MainCamera").GetComponent<IN_GAME_MAIN_CAMERA>().gameOver = true;
        }

        [Obsolete("AoTTG1's way of handling player stats. Whereas the concept itself is fine, this needs to be part of the HERO classes, not the GameManager.")]
        public void playerKillInfoUpdate(PhotonPlayer player, int dmg)
        {
            ExitGames.Client.Photon.Hashtable propertiesToSet = new ExitGames.Client.Photon.Hashtable();
            propertiesToSet.Add(PhotonPlayerProperty.kills, ((int) player.CustomProperties[PhotonPlayerProperty.kills]) + 1);
            player.SetCustomProperties(propertiesToSet);
            propertiesToSet = new ExitGames.Client.Photon.Hashtable();
            propertiesToSet.Add(PhotonPlayerProperty.max_dmg, Mathf.Max(dmg, (int) player.CustomProperties[PhotonPlayerProperty.max_dmg]));
            player.SetCustomProperties(propertiesToSet);
            propertiesToSet = new ExitGames.Client.Photon.Hashtable();
            propertiesToSet.Add(PhotonPlayerProperty.total_dmg, ((int) player.CustomProperties[PhotonPlayerProperty.total_dmg]) + dmg);
            player.SetCustomProperties(propertiesToSet);
        }

        [Obsolete("UI logic should be within UI classes.")]
        public void RecompilePlayerList(float time)
        {
            if (!this.isRecompiling)
            {
                this.isRecompiling = true;
                base.StartCoroutine(this.WaitAndRecompilePlayerList(time));
            }
        }

        [Obsolete("Was used to replace the skybox, but we can do something better.")]
        public IEnumerator reloadSky()
        {
            yield return new WaitForSeconds(0.5f);
            if ((skyMaterial != null) && (Camera.main.GetComponent<Skybox>().material != skyMaterial))
                Camera.main.GetComponent<Skybox>().material = skyMaterial;
        }

        [Obsolete("Legacy way of handling AoTTG1 settings. Most settings inside this method are no longer used.")]
        private void resetSettings(bool isLeave)
        {
            this.name = LoginFengKAI.player.name;
            masterRC = false;
            ExitGames.Client.Photon.Hashtable propertiesToSet = new ExitGames.Client.Photon.Hashtable();
            propertiesToSet.Add(PhotonPlayerProperty.RCteam, 0);
            if (isLeave)
            {
                currentLevel = string.Empty;
                propertiesToSet.Add(PhotonPlayerProperty.currentLevel, string.Empty);
                this.levelCache = new List<string[]>();
                this.playerSpawnsC.Clear();
                this.playerSpawnsM.Clear();
                currentScriptLogic = string.Empty;
                propertiesToSet.Add(PhotonPlayerProperty.statACL, 100);
                propertiesToSet.Add(PhotonPlayerProperty.statBLA, 100);
                propertiesToSet.Add(PhotonPlayerProperty.statGAS, 100);
                propertiesToSet.Add(PhotonPlayerProperty.statSPD, 100);
                this.restartingMC = false;
            }
            PhotonNetwork.player.SetCustomProperties(propertiesToSet);
            banHash = new ExitGames.Client.Photon.Hashtable();
            imatitan = new ExitGames.Client.Photon.Hashtable();
            oldScript = string.Empty;
            ignoreList = new List<int>();
            this.restartCount = new List<float>();
            heroHash = new ExitGames.Client.Photon.Hashtable();
            needChooseSide = true;
        }

        [Obsolete("Move into a RespawnService")]
        [PunRPC]
        public void RespawnRpc(PhotonMessageInfo info)
        {
            if (!info.sender.IsMasterClient) return;
            Respawn(PhotonNetwork.player);
        }

        [Obsolete("Move into a RespawnService")]
        private void Respawn(PhotonPlayer player)
        {
            if (player.CustomProperties[PhotonPlayerProperty.dead] == null
                || !RCextensions.returnBoolFromObject(player.CustomProperties[PhotonPlayerProperty.dead])) return;

            chatRoom.AddMessage("<color=#FFCC00>You have been revived by the master client.</color>");
            var isPlayerTitan = RCextensions.returnIntFromObject(player.CustomProperties[PhotonPlayerProperty.isTitan]) == 2;
            if (isPlayerTitan)
            {
                SpawnService.Spawn<PlayerTitan>();
            }
            else
            {
                respawnHeroInNewRound();
            }
        }

        [Obsolete("Move into a RespawnService")]
        private IEnumerator respawnE(float seconds)
        {
            while (true)
            {
                yield return new WaitForSeconds(seconds);
                for (int j = 0; j < PhotonNetwork.playerList.Length; j++)
                {
                    PhotonPlayer targetPlayer = PhotonNetwork.playerList[j];
                    if (((targetPlayer.CustomProperties[PhotonPlayerProperty.RCteam] == null) && RCextensions.returnBoolFromObject(targetPlayer.CustomProperties[PhotonPlayerProperty.dead])) && (RCextensions.returnIntFromObject(targetPlayer.CustomProperties[PhotonPlayerProperty.isTitan]) != 2))
                    {
                        this.photonView.RPC(nameof(respawnHeroInNewRound), targetPlayer, new object[0]);
                    }
                }
            }
        }

        [Obsolete("Move into a RespawnService")]
        [PunRPC]
        public void respawnHeroInNewRound()
        {
            if (!this.needChooseSide && GameObject.Find("MainCamera").GetComponent<IN_GAME_MAIN_CAMERA>().gameOver)
            {
                this.SpawnPlayer(this.myLastHero, this.myLastRespawnTag);
                GameObject.Find("MainCamera").GetComponent<IN_GAME_MAIN_CAMERA>().gameOver = false;
            }
        }

        [Obsolete("AoTTG's way of handling a game restart, not used in AoTTG2 anymore.")]
        public void restartGame2(bool masterclientSwitched = false)
        {
            if (!this.gameTimesUp)
            {
                this.checkpoint = null;
                this.myRespawnTime = 0f;
                this.ClearKillInfo();
                this.isRestarting = true;
                this.DestroyAllExistingCloths();
                PhotonNetwork.DestroyAll();
                base.photonView.RPC(nameof(RPCLoadLevel), PhotonTargets.All, new object[0]);
                if (masterclientSwitched)
                {
                    this.sendChatContentInfo("<color=#A8FF24>MasterClient has switched to </color>" + ((string) PhotonNetwork.player.CustomProperties[PhotonPlayerProperty.name]).hexColor());
                }
            }
        }

        [Obsolete("Make use directly of the InRoomChat RPCs.")]
        public void sendChatContentInfo(string content)
        {
            object[] parameters = new object[] { content, string.Empty };
            base.photonView.RPC(nameof(Chat), PhotonTargets.All, parameters);
        }

        [Obsolete("UI logic needs to be within UI classes")]
        public void sendKillInfo(bool t1, string killer, bool t2, string victim, int dmg = 0)
        {
            object[] parameters = new object[] { t1, killer, t2, victim, dmg };
            base.photonView.RPC(nameof(updateKillInfo), PhotonTargets.All, parameters);
        }

        [Obsolete("AoTTG required several work around in order to actually kick users, with the authorative Photon Server this will no longer be needed.")]
        public static void ServerCloseConnection(PhotonPlayer targetPlayer, bool requestIpBan, string inGameName = null)
        {
            RaiseEventOptions options = new RaiseEventOptions
            {
                TargetActors = new int[] { targetPlayer.ID }
            };
            if (requestIpBan)
            {
                ExitGames.Client.Photon.Hashtable eventContent = new ExitGames.Client.Photon.Hashtable();
                eventContent[(byte) 0] = true;
                if ((inGameName != null) && (inGameName.Length > 0))
                {
                    eventContent[(byte) 1] = inGameName;
                }
                PhotonNetwork.RaiseEvent(0xcb, eventContent, true, options);
            }
            else
            {
                PhotonNetwork.RaiseEvent(0xcb, null, true, options);
            }
        }

        [Obsolete("A custom Photon Event to pass a password to a Photon Server. I don't believe that the Photon Server currently supports this?")]
        public static void ServerRequestAuthentication(string authPassword)
        {
            if (!string.IsNullOrEmpty(authPassword))
            {
                ExitGames.Client.Photon.Hashtable eventContent = new ExitGames.Client.Photon.Hashtable();
                eventContent[(byte) 0] = authPassword;
                PhotonNetwork.RaiseEvent(0xc6, eventContent, true, new RaiseEventOptions());
            }
        }

        [Obsolete("AoTTG required several work around in order to actually kick users, with the authorative Photon Server this will no longer be needed.")]
        public static void ServerRequestUnban(string bannedAddress)
        {
            if (!string.IsNullOrEmpty(bannedAddress))
            {
                ExitGames.Client.Photon.Hashtable eventContent = new ExitGames.Client.Photon.Hashtable();
                eventContent[(byte) 0] = bannedAddress;
                PhotonNetwork.RaiseEvent(0xc7, eventContent, true, new RaiseEventOptions());
            }
        }

        [Obsolete("A RPC for the 'MasterRC' concept, its basically the same as the 'sub MC' system that was proposed, where a NonMC can be given some extra privileges. Mainly something that needs to be handled on the Photon Server and this MasterRC was never implemented.")]
        [PunRPC]
        private void setMasterRC(PhotonMessageInfo info)
        {
            if (info.sender.isMasterClient)
            {
                masterRC = true;
            }
        }

        [Obsolete("Migrate to TeamService")]
        private void setTeam(int setting)
        {
            if (setting == 0)
            {
                this.name = LoginFengKAI.player.name;
                ExitGames.Client.Photon.Hashtable propertiesToSet = new ExitGames.Client.Photon.Hashtable();
                propertiesToSet.Add(PhotonPlayerProperty.RCteam, 0);
                propertiesToSet.Add(PhotonPlayerProperty.name, this.name);
                PhotonNetwork.player.SetCustomProperties(propertiesToSet);
            }
            else if (setting == 1)
            {
                ExitGames.Client.Photon.Hashtable hashtable2 = new ExitGames.Client.Photon.Hashtable();
                hashtable2.Add(PhotonPlayerProperty.RCteam, 1);
                var name = LoginFengKAI.player.name;
                if (!name.StartsWith("<color=#00ffff>"))
                {
                    name = $"<color=#00ffff>{name}</color>";
                }
                this.name = name;
                hashtable2.Add(PhotonPlayerProperty.name, this.name);
                PhotonNetwork.player.SetCustomProperties(hashtable2);
            }
            else if (setting == 2)
            {
                ExitGames.Client.Photon.Hashtable hashtable3 = new ExitGames.Client.Photon.Hashtable();
                hashtable3.Add(PhotonPlayerProperty.RCteam, 2);
                var name = LoginFengKAI.player.name;
                if (!name.StartsWith("<color=#ff00ff>"))
                {
                    name = $"<color=#ff00ff>{name}</color>";
                }
                this.name = name;
                hashtable3.Add(PhotonPlayerProperty.name, this.name);
                PhotonNetwork.player.SetCustomProperties(hashtable3);
            }
            else if (setting == 3)
            {
                int num3 = 0;
                int num4 = 0;
                int num5 = 1;
                foreach (PhotonPlayer player in PhotonNetwork.playerList)
                {
                    int num7 = RCextensions.returnIntFromObject(player.CustomProperties[PhotonPlayerProperty.RCteam]);
                    if (num7 > 0)
                    {
                        if (num7 == 1)
                        {
                            num3++;
                        }
                        else if (num7 == 2)
                        {
                            num4++;
                        }
                    }
                }
                if (num3 > num4)
                {
                    num5 = 2;
                }
                this.setTeam(num5);
            }
            if (((setting == 0) || (setting == 1)) || (setting == 2))
            {
                foreach (GameObject obj2 in GameObject.FindGameObjectsWithTag("Player"))
                {
                    if (obj2.GetPhotonView().isMine)
                    {
                        base.photonView.RPC(nameof(labelRPC), PhotonTargets.All, new object[] { obj2.GetPhotonView().viewID });
                    }
                }
            }
        }

        [Obsolete("Migrate to TeamService")]
        [PunRPC]
        public void setTeamRPC(int setting, PhotonMessageInfo info)
        {
            if (info.sender.isMasterClient || info.sender.isLocal)
            {
                this.setTeam(setting);
            }
        }

        [Obsolete("Legacy code which was used to display the final results when a room ended, however, this functionality no longer exists in AoTTG2.")]
        [PunRPC]
        private void showResult(string text0, string text1, string text2, string text3, string text4, string text6, PhotonMessageInfo t)
        {
            if (!(this.gameTimesUp || !t.sender.isMasterClient))
            {
                this.gameTimesUp = true;
                GameObject obj2 = GameObject.Find("UI_IN_GAME");
                IN_GAME_MAIN_CAMERA.gametype = GAMETYPE.Stop;
            }
            else if (!(t.sender.isMasterClient || !PhotonNetwork.player.isMasterClient))
            {
                this.kickPlayerRC(t.sender, true, "false game end.");
            }
        }

        //TODO: 184 - This gets called upon MapLoaded
        [Obsolete("Migrate into a SpawnService")]
        public void SpawnPlayer(string id, string tag = "playerRespawn", CharacterPreset preset = null)
        {
            if (id == null)
            {
                id = "1";
            }
            myLastRespawnTag = tag;
            var location = Gamemode.GetPlayerSpawnLocation(tag);
            SpawnPlayerAt2(id, location, preset);
        }

        [Obsolete("Migrate into a SpawnService")]
        public void SpawnPlayerAt2(string id, GameObject pos, CharacterPreset preset = null)
        {
            // HACK
            if (false)
            //if (!logicLoaded || !customLevelLoaded)
            {
                this.NOTSpawnPlayerRC(id);
            }
            else
            {
                Vector3 position = pos?.transform.position ?? new Vector3(0f, 5f, 0f);
                if (this.racingSpawnPointSet)
                {
                    position = this.racingSpawnPoint;
                }
                else
                {
                    if (Level.IsCustom)
                    {
                        if (RCextensions.returnIntFromObject(PhotonNetwork.player.CustomProperties[PhotonPlayerProperty.RCteam]) == 0)
                        {
                            position = SpawnService.GetRandom<HumanSpawner>()?.gameObject.transform.position ?? new Vector3();
                        }
                        else if (RCextensions.returnIntFromObject(PhotonNetwork.player.CustomProperties[PhotonPlayerProperty.RCteam]) == 1)
                        {
                            var cyanSpawners = SpawnService.GetByType(PlayerSpawnType.Cyan);
                            if (cyanSpawners.Count > 0)
                            {
                                position = cyanSpawners[UnityEngine.Random.Range(0, cyanSpawners.Count)].gameObject.transform
                                    .position;
                            }
                        }
                        else if ((RCextensions.returnIntFromObject(PhotonNetwork.player.CustomProperties[PhotonPlayerProperty.RCteam]) == 2))
                        {
                            var magentaSpawners = SpawnService.GetByType(PlayerSpawnType.Magenta);
                            if (magentaSpawners.Count > 0)
                            {
                                position = magentaSpawners[UnityEngine.Random.Range(0, magentaSpawners.Count)].gameObject.transform
                                    .position;
                            }
                        }
                    }
                }
                IN_GAME_MAIN_CAMERA component = GameObject.Find("MainCamera").GetComponent<IN_GAME_MAIN_CAMERA>();
                this.myLastHero = id.ToUpper();
                if (myLastHero == "ErenTitan")
                {
                    component.SetMainObject(PhotonNetwork.Instantiate("ErenTitan", position, pos?.transform.rotation ?? new Quaternion(), 0),
                        true, false);
                }
                else
                {
                    var hero = SpawnService.Spawn<Hero>(position + new Vector3(0, 5f, 0), pos?.transform.rotation ?? new Quaternion(), preset);
                    component.SetMainObject(hero.transform.gameObject, true, false);
                    ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
                    hashtable.Add("dead", false);
                    ExitGames.Client.Photon.Hashtable propertiesToSet = hashtable;
                    PhotonNetwork.player.SetCustomProperties(propertiesToSet);
                    hashtable = new ExitGames.Client.Photon.Hashtable();
                    hashtable.Add(PhotonPlayerProperty.isTitan, 1);
                    propertiesToSet = hashtable;
                    PhotonNetwork.player.SetCustomProperties(propertiesToSet);
                }

                component.enabled = true;
                SpectatorMode.Disable();
                GameObject.Find("MainCamera").GetComponent<MouseLook>().disable = true;
                component.gameOver = false;
                Service.Player.Self = component.main_object.GetComponent<Entity>();
            }
        }


        [Obsolete("UI logic should be placed in UI classes")]
        [PunRPC]
        public void titanGetKill(PhotonPlayer player, int Damage, string name)
        {
            Damage = Mathf.Max(10, Damage);
            object[] parameters = new object[] { Damage };
            base.photonView.RPC(nameof(netShowDamage), player, parameters);

            this.sendKillInfo(false, (string) player.CustomProperties[PhotonPlayerProperty.name], true, name, Damage);

            this.playerKillInfoUpdate(player, Damage);
        }

        [Obsolete("Calls Resources.UnloadedUnusedAssets after 10 seconds, probably this was required due to how inefficient AoTTG is, not sure if this is still needed.")]
        public void unloadAssets()
        {
            if (!this.isUnloading)
            {
                this.isUnloading = true;
                base.StartCoroutine(this.unloadAssetsE(10f));
            }
        }

        [Obsolete("The enumerator for unloadAssets, but the same concept.")]
        public IEnumerator unloadAssetsE(float time)
        {
            yield return new WaitForSeconds(time);
            Resources.UnloadUnusedAssets();
            this.isUnloading = false;
        }

        [Obsolete("UI logic should be within UI classes.")]
        [PunRPC]
        private void updateKillInfo(bool killerIsTitan, string killer, bool victimIsTitan, string victim, int dmg)
        {
            var killFeed = GameObject.Find("KillFeed");
            var newKillInfo = (GameObject) UnityEngine.Object.Instantiate(Resources.Load("UI/KillInfo"));
            foreach (var killInfo in killInfoGO)
            {
                if (killInfo != null)
                {
                    killInfo.GetComponent<KillInfo>().MoveOn();
                }
            }

            if (killInfoGO.Count > 4)
            {
                var lastKillInfo = killInfoGO[0];
                if (lastKillInfo != null)
                {
                    lastKillInfo.GetComponent<KillInfo>().Destroy();
                }
                killInfoGO.RemoveAt(0);
            }

            newKillInfo.transform.parent = killFeed.transform;
            newKillInfo.transform.position = new Vector3();
            newKillInfo.GetComponent<KillInfo>().Show(killerIsTitan, killer, victimIsTitan, victim, dmg);
            killInfoGO.Add(newKillInfo);
        }

        [Obsolete("UI logic should be within UI classes.")]
        private void ClearKillInfo()
        {
            foreach (var killInfo in killInfoGO)
            {
                Destroy(killInfo);
            }
            killInfoGO.Clear();
        }

        [Obsolete("It adds a player ID to a banHash, yet this is no longer required as for AoTTG2 we have an authorative Photon Server which can deal with bans.")]
        [PunRPC]
        public void verifyPlayerHasLeft(int ID, PhotonMessageInfo info)
        {
            if (info.sender.isMasterClient && (PhotonPlayer.Find(ID) != null))
            {
                PhotonPlayer player = PhotonPlayer.Find(ID);
                string str = string.Empty;
                str = RCextensions.returnStringFromObject(player.CustomProperties[PhotonPlayerProperty.name]);
                banHash.Add(ID, str);
            }
        }

        [Obsolete("A way too complicated, and extremely inefficient way to generate a player list. Should be moved to the respective UI class and refactored (Using StringBuilder for massive performance improvements)")]
        public IEnumerator WaitAndRecompilePlayerList(float time)
        {
            int num16;
            string str2;
            int num17;
            int num18;
            int num19;
            int num20;
            object[] objArray2;
            yield return new WaitForSeconds(time);
            string iteratorVariable1 = string.Empty;
            if (GameSettings.Gamemode.TeamMode == TeamMode.Disabled)
            {
                foreach (PhotonPlayer player7 in PhotonNetwork.playerList)
                {
                    if (player7.CustomProperties[PhotonPlayerProperty.dead] != null)
                    {
                        if (ignoreList.Contains(player7.ID))
                        {
                            iteratorVariable1 = iteratorVariable1 + "[FF0000][X] ";
                        }
                        if (player7.isLocal)
                        {
                            iteratorVariable1 = iteratorVariable1 + "[00CC00]";
                        }
                        else
                        {
                            iteratorVariable1 = iteratorVariable1 + "[FFCC00]";
                        }
                        iteratorVariable1 = iteratorVariable1 + "[" + Convert.ToString(player7.ID) + "] ";
                        if (player7.isMasterClient)
                        {
                            iteratorVariable1 = iteratorVariable1 + "[ffffff][M] ";
                        }
                        if (RCextensions.returnBoolFromObject(player7.CustomProperties[PhotonPlayerProperty.dead]))
                        {
                            iteratorVariable1 = iteratorVariable1 + "[" + ColorSet.color_red + "] *dead* ";
                        }
                        if (RCextensions.returnIntFromObject(player7.CustomProperties[PhotonPlayerProperty.isTitan]) < 2)
                        {
                            num16 = RCextensions.returnIntFromObject(player7.CustomProperties[PhotonPlayerProperty.team]);
                            if (num16 < 2)
                            {
                                iteratorVariable1 = iteratorVariable1 + "[" + ColorSet.color_human + "] <H> ";
                            }
                            else if (num16 == 2)
                            {
                                iteratorVariable1 = iteratorVariable1 + "[" + ColorSet.color_human_1 + "] <A> ";
                            }
                        }
                        else if (RCextensions.returnIntFromObject(player7.CustomProperties[PhotonPlayerProperty.isTitan]) == 2)
                        {
                            iteratorVariable1 = iteratorVariable1 + "[" + ColorSet.color_titan_player + "] <T> ";
                        }
                        string iteratorVariable0 = iteratorVariable1;
                        str2 = string.Empty;
                        str2 = RCextensions.returnStringFromObject(player7.CustomProperties[PhotonPlayerProperty.name]);
                        num17 = 0;
                        num17 = RCextensions.returnIntFromObject(player7.CustomProperties[PhotonPlayerProperty.kills]);
                        num18 = 0;
                        num18 = RCextensions.returnIntFromObject(player7.CustomProperties[PhotonPlayerProperty.deaths]);
                        num19 = 0;
                        num19 = RCextensions.returnIntFromObject(player7.CustomProperties[PhotonPlayerProperty.max_dmg]);
                        num20 = 0;
                        num20 = RCextensions.returnIntFromObject(player7.CustomProperties[PhotonPlayerProperty.total_dmg]);
                        objArray2 = new object[] { iteratorVariable0, string.Empty, str2, "[ffffff]:", num17, "/", num18, "/", num19, "/", num20 };
                        iteratorVariable1 = string.Concat(objArray2);
                        if (RCextensions.returnBoolFromObject(player7.CustomProperties[PhotonPlayerProperty.dead]))
                        {
                            iteratorVariable1 = iteratorVariable1 + "[-]";
                        }
                        iteratorVariable1 = iteratorVariable1 + "\n";
                    }
                }
            }
            else
            {
                int num11;
                string str;
                int num2 = 0;
                int num3 = 0;
                int num4 = 0;
                int num5 = 0;
                int num6 = 0;
                int num7 = 0;
                int num8 = 0;
                int num9 = 0;
                Dictionary<int, PhotonPlayer> dictionary = new Dictionary<int, PhotonPlayer>();
                Dictionary<int, PhotonPlayer> dictionary2 = new Dictionary<int, PhotonPlayer>();
                Dictionary<int, PhotonPlayer> dictionary3 = new Dictionary<int, PhotonPlayer>();
                PhotonPlayer[] playerList = PhotonNetwork.playerList;
                for (int j = 0; j < playerList.Length; j++)
                {
                    PhotonPlayer player = playerList[j];
                    if ((player.CustomProperties[PhotonPlayerProperty.dead] != null) && !ignoreList.Contains(player.ID))
                    {
                        num11 = RCextensions.returnIntFromObject(player.CustomProperties[PhotonPlayerProperty.RCteam]);
                        switch (num11)
                        {
                            case 0:
                                dictionary3.Add(player.ID, player);
                                break;

                            case 1:
                                dictionary.Add(player.ID, player);
                                num2 += RCextensions.returnIntFromObject(player.CustomProperties[PhotonPlayerProperty.kills]);
                                num4 += RCextensions.returnIntFromObject(player.CustomProperties[PhotonPlayerProperty.deaths]);
                                num6 += RCextensions.returnIntFromObject(player.CustomProperties[PhotonPlayerProperty.max_dmg]);
                                num8 += RCextensions.returnIntFromObject(player.CustomProperties[PhotonPlayerProperty.total_dmg]);
                                break;

                            case 2:
                                dictionary2.Add(player.ID, player);
                                num3 += RCextensions.returnIntFromObject(player.CustomProperties[PhotonPlayerProperty.kills]);
                                num5 += RCextensions.returnIntFromObject(player.CustomProperties[PhotonPlayerProperty.deaths]);
                                num7 += RCextensions.returnIntFromObject(player.CustomProperties[PhotonPlayerProperty.max_dmg]);
                                num9 += RCextensions.returnIntFromObject(player.CustomProperties[PhotonPlayerProperty.total_dmg]);
                                break;
                        }
                    }
                }
                this.cyanKills = num2;
                this.magentaKills = num3;
                if (PhotonNetwork.isMasterClient)
                {
                    if (GameSettings.Gamemode.TeamMode == TeamMode.LockBySize)
                    {
                        foreach (PhotonPlayer player2 in PhotonNetwork.playerList)
                        {
                            int num12 = 0;
                            if (dictionary.Count > (dictionary2.Count + 1))
                            {
                                num12 = 2;
                                if (dictionary.ContainsKey(player2.ID))
                                {
                                    dictionary.Remove(player2.ID);
                                }
                                if (!dictionary2.ContainsKey(player2.ID))
                                {
                                    dictionary2.Add(player2.ID, player2);
                                }
                            }
                            else if (dictionary2.Count > (dictionary.Count + 1))
                            {
                                num12 = 1;
                                if (!dictionary.ContainsKey(player2.ID))
                                {
                                    dictionary.Add(player2.ID, player2);
                                }
                                if (dictionary2.ContainsKey(player2.ID))
                                {
                                    dictionary2.Remove(player2.ID);
                                }
                            }
                            if (num12 > 0)
                            {
                                this.photonView.RPC(nameof(setTeamRPC), player2, new object[] { num12 });
                            }
                        }
                    }
                    else if (GameSettings.Gamemode.TeamMode == TeamMode.LockBySkill)
                    {
                        foreach (PhotonPlayer player3 in PhotonNetwork.playerList)
                        {
                            int num13 = 0;
                            num11 = RCextensions.returnIntFromObject(player3.CustomProperties[PhotonPlayerProperty.RCteam]);
                            if (num11 > 0)
                            {
                                if (num11 == 1)
                                {
                                    int num14 = 0;
                                    num14 = RCextensions.returnIntFromObject(player3.CustomProperties[PhotonPlayerProperty.kills]);
                                    if (((num3 + num14) + 7) < (num2 - num14))
                                    {
                                        num13 = 2;
                                        num3 += num14;
                                        num2 -= num14;
                                    }
                                }
                                else if (num11 == 2)
                                {
                                    int num15 = 0;
                                    num15 = RCextensions.returnIntFromObject(player3.CustomProperties[PhotonPlayerProperty.kills]);
                                    if (((num2 + num15) + 7) < (num3 - num15))
                                    {
                                        num13 = 1;
                                        num2 += num15;
                                        num3 -= num15;
                                    }
                                }
                                if (num13 > 0)
                                {
                                    this.photonView.RPC(nameof(setTeamRPC), player3, new object[] { num13 });
                                }
                            }
                        }
                    }
                }
                iteratorVariable1 = string.Concat(new object[] { iteratorVariable1, "[00FFFF]TEAM CYAN", "[ffffff]:", this.cyanKills, "/", num4, "/", num6, "/", num8, "\n" });
                foreach (PhotonPlayer player4 in dictionary.Values)
                {
                    num11 = RCextensions.returnIntFromObject(player4.CustomProperties[PhotonPlayerProperty.RCteam]);
                    if ((player4.CustomProperties[PhotonPlayerProperty.dead] != null) && (num11 == 1))
                    {
                        if (ignoreList.Contains(player4.ID))
                        {
                            iteratorVariable1 = iteratorVariable1 + "[FF0000][X] ";
                        }
                        if (player4.isLocal)
                        {
                            iteratorVariable1 = iteratorVariable1 + "[00CC00]";
                        }
                        else
                        {
                            iteratorVariable1 = iteratorVariable1 + "[FFCC00]";
                        }
                        iteratorVariable1 = iteratorVariable1 + "[" + Convert.ToString(player4.ID) + "] ";
                        if (player4.isMasterClient)
                        {
                            iteratorVariable1 = iteratorVariable1 + "[ffffff][M] ";
                        }
                        if (RCextensions.returnBoolFromObject(player4.CustomProperties[PhotonPlayerProperty.dead]))
                        {
                            iteratorVariable1 = iteratorVariable1 + "[" + ColorSet.color_red + "] *dead* ";
                        }
                        if (RCextensions.returnIntFromObject(player4.CustomProperties[PhotonPlayerProperty.isTitan]) < 2)
                        {
                            num16 = RCextensions.returnIntFromObject(player4.CustomProperties[PhotonPlayerProperty.team]);
                            if (num16 < 2)
                            {
                                iteratorVariable1 = iteratorVariable1 + "[" + ColorSet.color_human + "] <H> ";
                            }
                            else if (num16 == 2)
                            {
                                iteratorVariable1 = iteratorVariable1 + "[" + ColorSet.color_human_1 + "] <A> ";
                            }
                        }
                        else if (RCextensions.returnIntFromObject(player4.CustomProperties[PhotonPlayerProperty.isTitan]) == 2)
                        {
                            iteratorVariable1 = iteratorVariable1 + "[" + ColorSet.color_titan_player + "] <T> ";
                        }
                        str = iteratorVariable1;
                        str2 = RCextensions.returnStringFromObject(player4.CustomProperties[PhotonPlayerProperty.name]);
                        num17 = RCextensions.returnIntFromObject(player4.CustomProperties[PhotonPlayerProperty.kills]);
                        num18 = RCextensions.returnIntFromObject(player4.CustomProperties[PhotonPlayerProperty.deaths]);
                        num19 = RCextensions.returnIntFromObject(player4.CustomProperties[PhotonPlayerProperty.max_dmg]);
                        num20 = RCextensions.returnIntFromObject(player4.CustomProperties[PhotonPlayerProperty.total_dmg]);
                        iteratorVariable1 = string.Concat(new object[] { str, string.Empty, str2, "[ffffff]:", num17, "/", num18, "/", num19, "/", num20 });
                        if (RCextensions.returnBoolFromObject(player4.CustomProperties[PhotonPlayerProperty.dead]))
                        {
                            iteratorVariable1 = iteratorVariable1 + "[-]";
                        }
                        iteratorVariable1 = iteratorVariable1 + "\n";
                    }
                }
                iteratorVariable1 = string.Concat(new object[] { iteratorVariable1, " \n", "[FF00FF]TEAM MAGENTA", "[ffffff]:", this.magentaKills, "/", num5, "/", num7, "/", num9, "\n" });
                foreach (PhotonPlayer player5 in dictionary2.Values)
                {
                    num11 = RCextensions.returnIntFromObject(player5.CustomProperties[PhotonPlayerProperty.RCteam]);
                    if ((player5.CustomProperties[PhotonPlayerProperty.dead] != null) && (num11 == 2))
                    {
                        if (ignoreList.Contains(player5.ID))
                        {
                            iteratorVariable1 = iteratorVariable1 + "[FF0000][X] ";
                        }
                        if (player5.isLocal)
                        {
                            iteratorVariable1 = iteratorVariable1 + "[00CC00]";
                        }
                        else
                        {
                            iteratorVariable1 = iteratorVariable1 + "[FFCC00]";
                        }
                        iteratorVariable1 = iteratorVariable1 + "[" + Convert.ToString(player5.ID) + "] ";
                        if (player5.isMasterClient)
                        {
                            iteratorVariable1 = iteratorVariable1 + "[ffffff][M] ";
                        }
                        if (RCextensions.returnBoolFromObject(player5.CustomProperties[PhotonPlayerProperty.dead]))
                        {
                            iteratorVariable1 = iteratorVariable1 + "[" + ColorSet.color_red + "] *dead* ";
                        }
                        if (RCextensions.returnIntFromObject(player5.CustomProperties[PhotonPlayerProperty.isTitan]) < 2)
                        {
                            num16 = RCextensions.returnIntFromObject(player5.CustomProperties[PhotonPlayerProperty.team]);
                            if (num16 < 2)
                            {
                                iteratorVariable1 = iteratorVariable1 + "[" + ColorSet.color_human + "] <H> ";
                            }
                            else if (num16 == 2)
                            {
                                iteratorVariable1 = iteratorVariable1 + "[" + ColorSet.color_human_1 + "] <A> ";
                            }
                        }
                        else if (RCextensions.returnIntFromObject(player5.CustomProperties[PhotonPlayerProperty.isTitan]) == 2)
                        {
                            iteratorVariable1 = iteratorVariable1 + "[" + ColorSet.color_titan_player + "] <T> ";
                        }
                        str = iteratorVariable1;
                        str2 = string.Empty;
                        str2 = RCextensions.returnStringFromObject(player5.CustomProperties[PhotonPlayerProperty.name]);
                        num17 = 0;
                        num17 = RCextensions.returnIntFromObject(player5.CustomProperties[PhotonPlayerProperty.kills]);
                        num18 = 0;
                        num18 = RCextensions.returnIntFromObject(player5.CustomProperties[PhotonPlayerProperty.deaths]);
                        num19 = 0;
                        num19 = RCextensions.returnIntFromObject(player5.CustomProperties[PhotonPlayerProperty.max_dmg]);
                        num20 = 0;
                        num20 = RCextensions.returnIntFromObject(player5.CustomProperties[PhotonPlayerProperty.total_dmg]);
                        iteratorVariable1 = string.Concat(new object[] { str, string.Empty, str2, "[ffffff]:", num17, "/", num18, "/", num19, "/", num20 });
                        if (RCextensions.returnBoolFromObject(player5.CustomProperties[PhotonPlayerProperty.dead]))
                        {
                            iteratorVariable1 = iteratorVariable1 + "[-]";
                        }
                        iteratorVariable1 = iteratorVariable1 + "\n";
                    }
                }
                iteratorVariable1 = string.Concat(new object[] { iteratorVariable1, " \n", "[00FF00]INDIVIDUAL\n" });
                foreach (PhotonPlayer player6 in dictionary3.Values)
                {
                    num11 = RCextensions.returnIntFromObject(player6.CustomProperties[PhotonPlayerProperty.RCteam]);
                    if ((player6.CustomProperties[PhotonPlayerProperty.dead] != null) && (num11 == 0))
                    {
                        if (ignoreList.Contains(player6.ID))
                        {
                            iteratorVariable1 = iteratorVariable1 + "[FF0000][X] ";
                        }
                        if (player6.isLocal)
                        {
                            iteratorVariable1 = iteratorVariable1 + "[00CC00]";
                        }
                        else
                        {
                            iteratorVariable1 = iteratorVariable1 + "[FFCC00]";
                        }
                        iteratorVariable1 = iteratorVariable1 + "[" + Convert.ToString(player6.ID) + "] ";
                        if (player6.isMasterClient)
                        {
                            iteratorVariable1 = iteratorVariable1 + "[ffffff][M] ";
                        }
                        if (RCextensions.returnBoolFromObject(player6.CustomProperties[PhotonPlayerProperty.dead]))
                        {
                            iteratorVariable1 = iteratorVariable1 + "[" + ColorSet.color_red + "] *dead* ";
                        }
                        if (RCextensions.returnIntFromObject(player6.CustomProperties[PhotonPlayerProperty.isTitan]) < 2)
                        {
                            num16 = RCextensions.returnIntFromObject(player6.CustomProperties[PhotonPlayerProperty.team]);
                            if (num16 < 2)
                            {
                                iteratorVariable1 = iteratorVariable1 + "[" + ColorSet.color_human + "] <H> ";
                            }
                            else if (num16 == 2)
                            {
                                iteratorVariable1 = iteratorVariable1 + "[" + ColorSet.color_human_1 + "] <A> ";
                            }
                        }
                        else if (RCextensions.returnIntFromObject(player6.CustomProperties[PhotonPlayerProperty.isTitan]) == 2)
                        {
                            iteratorVariable1 = iteratorVariable1 + "[" + ColorSet.color_titan_player + "] <T> ";
                        }
                        str = iteratorVariable1;
                        str2 = string.Empty;
                        str2 = RCextensions.returnStringFromObject(player6.CustomProperties[PhotonPlayerProperty.name]);
                        num17 = 0;
                        num17 = RCextensions.returnIntFromObject(player6.CustomProperties[PhotonPlayerProperty.kills]);
                        num18 = 0;
                        num18 = RCextensions.returnIntFromObject(player6.CustomProperties[PhotonPlayerProperty.deaths]);
                        num19 = 0;
                        num19 = RCextensions.returnIntFromObject(player6.CustomProperties[PhotonPlayerProperty.max_dmg]);
                        num20 = 0;
                        num20 = RCextensions.returnIntFromObject(player6.CustomProperties[PhotonPlayerProperty.total_dmg]);
                        iteratorVariable1 = string.Concat(new object[] { str, string.Empty, str2, "[ffffff]:", num17, "/", num18, "/", num19, "/", num20 });
                        if (RCextensions.returnBoolFromObject(player6.CustomProperties[PhotonPlayerProperty.dead]))
                        {
                            iteratorVariable1 = iteratorVariable1 + "[-]";
                        }
                        iteratorVariable1 = iteratorVariable1 + "\n";
                    }
                }
            }
            ReloadPlayerlist();
            if (PhotonNetwork.isMasterClient && (/*(!this.isWinning && !this.isLosing) &&*/ Service.Time.GetRoundTime() >= 5f))
            {
                int num22;
                if (GameSettings.Gamemode.PointMode > 0)
                {
                    if (GameSettings.Gamemode.TeamMode != TeamMode.Disabled)
                    {
                        if (this.cyanKills >= GameSettings.Gamemode.PointMode)
                        {
                            object[] parameters = new object[] { "<color=#00FFFF>Team Cyan wins! </color>", string.Empty };
                            this.photonView.RPC(nameof(Chat), PhotonTargets.All, parameters);
                            //TODO: 160, game won
                            //this.gameWin2();
                        }
                        else if (this.magentaKills >= GameSettings.Gamemode.PointMode)
                        {
                            objArray2 = new object[] { "<color=#FF00FF>Team Magenta wins! </color>", string.Empty };
                            this.photonView.RPC(nameof(Chat), PhotonTargets.All, objArray2);
                            //TODO: 160, game won
                            //this.gameWin2();
                        }
                    }
                    else if (GameSettings.Gamemode.TeamMode == TeamMode.Disabled)
                    {
                        for (num22 = 0; num22 < PhotonNetwork.playerList.Length; num22++)
                        {
                            PhotonPlayer player9 = PhotonNetwork.playerList[num22];
                            if (RCextensions.returnIntFromObject(player9.CustomProperties[PhotonPlayerProperty.kills]) >= GameSettings.Gamemode.PointMode)
                            {
                                object[] objArray4 = new object[] { "<color=#FFCC00>" + RCextensions.returnStringFromObject(player9.CustomProperties[PhotonPlayerProperty.name]).hexColor() + " wins!</color>", string.Empty };
                                this.photonView.RPC(nameof(Chat), PhotonTargets.All, objArray4);
                                //TODO: 160, game won
                                //this.gameWin2();
                            }
                        }
                    }
                }
                else if ((GameSettings.Gamemode.PointMode <= 0) && ((GameSettings.PvP.Bomb.Value) || (GameSettings.PvP.Mode != PvpMode.Disabled)))
                {
                    if (GameSettings.PvP.PvPWinOnEnemiesDead.Value)
                    {
                        if ((GameSettings.Gamemode.TeamMode != TeamMode.Disabled) && (PhotonNetwork.playerList.Length > 1))
                        {
                            int num24 = 0;
                            int num25 = 0;
                            int num26 = 0;
                            int num27 = 0;
                            for (num22 = 0; num22 < PhotonNetwork.playerList.Length; num22++)
                            {
                                PhotonPlayer player10 = PhotonNetwork.playerList[num22];
                                if ((!ignoreList.Contains(player10.ID) && (player10.CustomProperties[PhotonPlayerProperty.RCteam] != null)) && (player10.CustomProperties[PhotonPlayerProperty.dead] != null))
                                {
                                    if (RCextensions.returnIntFromObject(player10.CustomProperties[PhotonPlayerProperty.RCteam]) == 1)
                                    {
                                        num26++;
                                        if (!RCextensions.returnBoolFromObject(player10.CustomProperties[PhotonPlayerProperty.dead]))
                                        {
                                            num24++;
                                        }
                                    }
                                    else if (RCextensions.returnIntFromObject(player10.CustomProperties[PhotonPlayerProperty.RCteam]) == 2)
                                    {
                                        num27++;
                                        if (!RCextensions.returnBoolFromObject(player10.CustomProperties[PhotonPlayerProperty.dead]))
                                        {
                                            num25++;
                                        }
                                    }
                                }
                            }
                            if ((num26 > 0) && (num27 > 0))
                            {
                                if (num24 == 0)
                                {
                                    object[] objArray5 = new object[] { "<color=#FF00FF>Team Magenta wins! </color>", string.Empty };
                                    this.photonView.RPC(nameof(Chat), PhotonTargets.All, objArray5);
                                    //TODO: 160, game won
                                    //this.gameWin2();
                                }
                                else if (num25 == 0)
                                {
                                    object[] objArray6 = new object[] { "<color=#00FFFF>Team Cyan wins! </color>", string.Empty };
                                    this.photonView.RPC(nameof(Chat), PhotonTargets.All, objArray6);
                                    //TODO: 160, game won
                                    //this.gameWin2();
                                }
                            }
                        }
                        else if ((GameSettings.Gamemode.TeamMode == TeamMode.Disabled) && (PhotonNetwork.playerList.Length > 1))
                        {
                            int num28 = 0;
                            string text = "Nobody";
                            PhotonPlayer player11 = PhotonNetwork.playerList[0];
                            for (num22 = 0; num22 < PhotonNetwork.playerList.Length; num22++)
                            {
                                PhotonPlayer player12 = PhotonNetwork.playerList[num22];
                                if (!((player12.CustomProperties[PhotonPlayerProperty.dead] == null) || RCextensions.returnBoolFromObject(player12.CustomProperties[PhotonPlayerProperty.dead])))
                                {
                                    text = RCextensions.returnStringFromObject(player12.CustomProperties[PhotonPlayerProperty.name]).hexColor();
                                    player11 = player12;
                                    num28++;
                                }
                            }
                            if (num28 <= 1)
                            {
                                string str4 = " 5 points added.";
                                if (text == "Nobody")
                                {
                                    str4 = string.Empty;
                                }
                                else
                                {
                                    for (num22 = 0; num22 < 5; num22++)
                                    {
                                        this.playerKillInfoUpdate(player11, 0);
                                    }
                                }
                                object[] objArray7 = new object[] { "<color=#FFCC00>" + text.hexColor() + " wins." + str4 + "</color>", string.Empty };
                                this.photonView.RPC(nameof(Chat), PhotonTargets.All, objArray7);
                                //TODO: 160, game won
                                //this.gameWin2();
                            }
                        }
                    }
                }
            }
            this.isRecompiling = false;
        }

        [Obsolete("AoTTG code on how to re-add player properties after they disconnected.")]
        public IEnumerator WaitAndReloadKDR(PhotonPlayer player)
        {
            yield return new WaitForSeconds(5f);
            string key = RCextensions.returnStringFromObject(player.CustomProperties[PhotonPlayerProperty.name]);
            if (this.PreservedPlayerKDR.ContainsKey(key))
            {
                int[] numArray = this.PreservedPlayerKDR[key];
                this.PreservedPlayerKDR.Remove(key);
                ExitGames.Client.Photon.Hashtable propertiesToSet = new ExitGames.Client.Photon.Hashtable();
                propertiesToSet.Add(PhotonPlayerProperty.kills, numArray[0]);
                propertiesToSet.Add(PhotonPlayerProperty.deaths, numArray[1]);
                propertiesToSet.Add(PhotonPlayerProperty.max_dmg, numArray[2]);
                propertiesToSet.Add(PhotonPlayerProperty.total_dmg, numArray[3]);
                player.SetCustomProperties(propertiesToSet);
            }
        }

        [Obsolete("Legacy method for a restart")]
        public IEnumerator WaitAndResetRestarts()
        {
            yield return new WaitForSeconds(10f);
            this.restartingMC = false;
        }

        [Obsolete("Migrate into a SpawnService")]
        public IEnumerator WaitAndRespawn1(float time, string str)
        {
            yield return new WaitForSeconds(time);
            this.SpawnPlayer(this.myLastHero, str);
        }

        [Obsolete("Migrate into a SpawnService")]
        public IEnumerator WaitAndRespawn2(float time, GameObject pos)
        {
            yield return new WaitForSeconds(time);
            this.SpawnPlayerAt2(this.myLastHero, pos);
        }
        #endregion

    }
}
