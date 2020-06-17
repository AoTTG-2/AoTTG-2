using Assets.Scripts;
using Assets.Scripts.Characters.Titan;
using Assets.Scripts.Gamemode;
using Assets.Scripts.Gamemode.Options;
using Assets.Scripts.Gamemode.Settings;
using Assets.Scripts.Room;
using Assets.Scripts.UI.InGame;
using Assets.Scripts.UI.InGame.HUD;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

//[Obsolete]
public class FengGameManagerMKII : Photon.MonoBehaviour
{
    [SerializeField]
    private VersionManager versionManager;

    public static bool showHackMenu = true;

    public Dictionary<int, CannonValues> allowedToCannon;
    public Dictionary<string, Texture2D> assetCacheTextures;
    public static ExitGames.Client.Photon.Hashtable banHash;
    public static ExitGames.Client.Photon.Hashtable boolVariables;
    public static Dictionary<string, GameObject> CachedPrefabs;
    private ArrayList chatContent;
    public InRoomChat chatRoom;
    public GameObject checkpoint;
    private ArrayList cT;
    public static string currentLevel;
    public static string currentScript;
    public static string currentScriptLogic;
    private float currentSpeed;
    public static bool customLevelLoaded;
    public int cyanKills;
    [Obsolete("Please use Gamemode.Difficulty")]
    public int difficulty;
    public float distanceSlider;
    private bool endRacing;
    private ArrayList eT;
    public static ExitGames.Client.Photon.Hashtable floatVariables;
    private ArrayList fT;
    public float gameEndCD;
    public float gameEndTotalCDtime = 9f;
    public bool gameStart;
    private bool gameTimesUp;
    public static ExitGames.Client.Photon.Hashtable globalVariables;
    public List<GameObject> groundList;
    public static bool hasLogged;
    private ArrayList heroes;
    public static ExitGames.Client.Photon.Hashtable heroHash;
    private ArrayList hooks;
    public static List<int> ignoreList;
    public static ExitGames.Client.Photon.Hashtable imatitan;
    public FengCustomInputs inputManager;
    public static InputManagerRC inputRC;
    public static FengGameManagerMKII instance;
    public static ExitGames.Client.Photon.Hashtable intVariables;
    public static bool isAssetLoaded;
    public bool isFirstLoad;
    private bool isLosing;
    public bool isPlayer1Winning;
    public bool isPlayer2Winning;
    public bool isRecompiling;
    public bool isRestarting;
    public bool isSpawning;
    public bool isUnloading;
    private bool isWinning;
    public bool justSuicide;
    private ArrayList killInfoGO = new ArrayList();
    public static bool LAN;
    public List<string[]> levelCache;
    public static ExitGames.Client.Photon.Hashtable[] linkHash;
    [Obsolete("Use RacingGamemode.localRacingResult")]
    private string localRacingResult;
    public static bool logicLoaded;
    public static int loginstate;
    public int magentaKills;
    private IN_GAME_MAIN_CAMERA mainCamera;
    public static bool masterRC;
    public int maxPlayers;
    private float maxSpeed;
    public float mouseSlider;
    private string myLastHero;
    private string myLastRespawnTag = "playerRespawn";
    public float myRespawnTime;
    public static string nameField;
    public bool needChooseSide;
    public static bool noRestart;
    public static string oldScript;
    public static string oldScriptLogic;
    public static bool OnPrivateServer;
    public static string passwordField;
    public float pauseWaitTime;
    public string playerList;
    public List<Vector3> playerSpawnsC;
    public List<Vector3> playerSpawnsM;
    public List<PhotonPlayer> playersRPC;
    public static ExitGames.Client.Photon.Hashtable playerVariables;
    public Dictionary<string, int[]> PreservedPlayerKDR;
    public static string PrivateServerAuthPass;
    public static string privateServerField;
    public float qualitySlider;
    public List<GameObject> racingDoors;
    private ArrayList racingResult;
    public Vector3 racingSpawnPoint;
    public bool racingSpawnPointSet;
    public static AssetBundle RCassets;
    public static ExitGames.Client.Photon.Hashtable RCEvents;
    public static ExitGames.Client.Photon.Hashtable RCRegions;
    public static ExitGames.Client.Photon.Hashtable RCRegionTriggers;
    public static ExitGames.Client.Photon.Hashtable RCVariableNames;
    public List<float> restartCount;
    public bool restartingBomb;
    public bool restartingMC;
    public float retryTime;
    public float roundTime;
    public static string[] s = "verified343,hair,character_eye,glass,character_face,character_head,character_hand,character_body,character_arm,character_leg,character_chest,character_cape,character_brand,character_3dmg,r,character_blade_l,character_3dmg_gas_r,character_blade_r,3dmg_smoke,HORSE,hair,body_001,Cube,Plane_031,mikasa_asset,character_cap_,character_gun".Split(new char[] { ',' });
    public Vector2 scroll;
    public Vector2 scroll2;
    public GameObject selectedObj;
    public static object[] settings;
    private int single_kills;
    private int single_maxDamage;
    private int single_totalDamage;
    public static Material skyMaterial;
    public List<GameObject> spectateSprites;
    private bool startRacing;
    public static ExitGames.Client.Photon.Hashtable stringVariables;
    public Texture2D textureBackgroundBlack;
    public Texture2D textureBackgroundBlue;
    public int time = 600;
    private float timeElapse;
    private float timeTotalServer;
    private ArrayList titans;
    public List<TitanSpawner> titanSpawners;
    public List<Vector3> titanSpawns;
    public static ExitGames.Client.Photon.Hashtable titanVariables;
    public float transparencySlider;
    public float updateTime;
    public static string usernameField;

    public InGameUi InGameUI;

    public new string name { get; set; }
    public static GamemodeBase Gamemode { get; private set; }
    public static Level Level { get; set; }

    public static Level NewRoundLevel { get; set; }
    public static GamemodeSettings NewRoundGamemode { get; set; }

    public void addCamera(IN_GAME_MAIN_CAMERA c)
    {
        mainCamera = c;
    }

    public void addCT(COLOSSAL_TITAN titan)
    {
        cT.Add(titan);
    }

    public void addET(TITAN_EREN hero)
    {
        eT.Add(hero);
    }

    public void addFT(FEMALE_TITAN titan)
    {
        fT.Add(titan);
    }

    public void addHero(Hero hero)
    {
        heroes.Add(hero);
    }

    public void addHook(Bullet h)
    {
        hooks.Add(h);
    }

    public void addTime(float time)
    {
        timeTotalServer -= time;
    }

    public void addTitan(MindlessTitan titan)
    {
        titans.Add(titan);
    }

    private void cache()
    {
        ClothFactory.ClearClothCache();
        inputManager = GameObject.Find("InputManagerController").GetComponent<FengCustomInputs>();
        //HACK
        //this.chatRoom = GameObject.Find("Chatroom").GetComponent<InRoomChat>();
        playersRPC.Clear();
        titanSpawners.Clear();
        groundList.Clear();
        PreservedPlayerKDR = new Dictionary<string, int[]>();
        noRestart = false;
        skyMaterial = null;
        isSpawning = false;
        retryTime = 0f;
        logicLoaded = false;
        customLevelLoaded = true;
        isUnloading = false;
        isRecompiling = false;
        Time.timeScale = 1f;
        Camera.main.farClipPlane = 1500f;
        pauseWaitTime = 0f;
        spectateSprites = new List<GameObject>();
        isRestarting = false;
        if (PhotonNetwork.isMasterClient)
        {
            StartCoroutine(WaitAndResetRestarts());
        }
        if (IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.SINGLE)
        {
            roundTime = 0f;
            if (Level.Name.StartsWith("Custom"))
            {
                customLevelLoaded = false;
            }
            if (PhotonNetwork.isMasterClient)
            {
                if (isFirstLoad)
                {
                    setGameSettings(checkGameGUI());
                }
            }
            if (((int) settings[0xf4]) == 1)
            {
                chatRoom.addLINE("<color=#FFC000>(" + roundTime.ToString("F2") + ")</color> Round Start.");
            }
        }
        isFirstLoad = false;
        RecompilePlayerList(0.5f);
    }

    [PunRPC]
    public void Chat(string content, string sender, PhotonMessageInfo info)
    {
        if (sender != string.Empty)
        {
            content = sender + ":" + content;
        }
        content = "<color=#FFC000>[" + Convert.ToString(info.sender.ID) + "]</color> " + content;
        chatRoom.addLINE(content);
    }

    [PunRPC]
    public void ChatPM(string sender, string content, PhotonMessageInfo info)
    {
        content = sender + ":" + content;
        content = "<color=#FFC000>FROM [" + Convert.ToString(info.sender.ID) + "]</color> " + content;
        chatRoom.addLINE(content);
    }

    private ExitGames.Client.Photon.Hashtable checkGameGUI()
    {
        int num;
        int num2;
        PhotonPlayer player;
        int num4;
        float num8;
        float num9;
        var hashtable = new ExitGames.Client.Photon.Hashtable();
        return hashtable;
    }

    [PunRPC]
    private void clearlevel(string[] link, PhotonMessageInfo info)
    {
        if (info.sender.isMasterClient)
        {
            if (info.sender.isMasterClient && (link.Length > 6))
            {
                StartCoroutine(clearlevelE(link));
            }
        }
    }

    private IEnumerator clearlevelE(string[] skybox)
    {
        var key = skybox[6];
        var mipmap = true;
        var iteratorVariable2 = false;
        if (((int) settings[0x3f]) == 1)
        {
            mipmap = false;
        }
        if (((((skybox[0] != string.Empty) || (skybox[1] != string.Empty)) || ((skybox[2] != string.Empty) || (skybox[3] != string.Empty))) || (skybox[4] != string.Empty)) || (skybox[5] != string.Empty))
        {
            var iteratorVariable3 = string.Join(",", skybox);
            if (!linkHash[1].ContainsKey(iteratorVariable3))
            {
                iteratorVariable2 = true;
                var material = Camera.main.GetComponent<Skybox>().material;
                var url = skybox[0];
                var iteratorVariable6 = skybox[1];
                var iteratorVariable7 = skybox[2];
                var iteratorVariable8 = skybox[3];
                var iteratorVariable9 = skybox[4];
                var iteratorVariable10 = skybox[5];
                if ((url.EndsWith(".jpg") || url.EndsWith(".png")) || url.EndsWith(".jpeg"))
                {
                    var link = new WWW(url);
                    yield return link;
                    var texture = RCextensions.loadimage(link, mipmap, 0x7a120);
                    link.Dispose();
                    material.SetTexture("_FrontTex", texture);
                }
                if ((iteratorVariable6.EndsWith(".jpg") || iteratorVariable6.EndsWith(".png")) || iteratorVariable6.EndsWith(".jpeg"))
                {
                    var iteratorVariable13 = new WWW(iteratorVariable6);
                    yield return iteratorVariable13;
                    var iteratorVariable14 = RCextensions.loadimage(iteratorVariable13, mipmap, 0x7a120);
                    iteratorVariable13.Dispose();
                    material.SetTexture("_BackTex", iteratorVariable14);
                }
                if ((iteratorVariable7.EndsWith(".jpg") || iteratorVariable7.EndsWith(".png")) || iteratorVariable7.EndsWith(".jpeg"))
                {
                    var iteratorVariable15 = new WWW(iteratorVariable7);
                    yield return iteratorVariable15;
                    var iteratorVariable16 = RCextensions.loadimage(iteratorVariable15, mipmap, 0x7a120);
                    iteratorVariable15.Dispose();
                    material.SetTexture("_LeftTex", iteratorVariable16);
                }
                if ((iteratorVariable8.EndsWith(".jpg") || iteratorVariable8.EndsWith(".png")) || iteratorVariable8.EndsWith(".jpeg"))
                {
                    var iteratorVariable17 = new WWW(iteratorVariable8);
                    yield return iteratorVariable17;
                    var iteratorVariable18 = RCextensions.loadimage(iteratorVariable17, mipmap, 0x7a120);
                    iteratorVariable17.Dispose();
                    material.SetTexture("_RightTex", iteratorVariable18);
                }
                if ((iteratorVariable9.EndsWith(".jpg") || iteratorVariable9.EndsWith(".png")) || iteratorVariable9.EndsWith(".jpeg"))
                {
                    var iteratorVariable19 = new WWW(iteratorVariable9);
                    yield return iteratorVariable19;
                    var iteratorVariable20 = RCextensions.loadimage(iteratorVariable19, mipmap, 0x7a120);
                    iteratorVariable19.Dispose();
                    material.SetTexture("_UpTex", iteratorVariable20);
                }
                if ((iteratorVariable10.EndsWith(".jpg") || iteratorVariable10.EndsWith(".png")) || iteratorVariable10.EndsWith(".jpeg"))
                {
                    var iteratorVariable21 = new WWW(iteratorVariable10);
                    yield return iteratorVariable21;
                    var iteratorVariable22 = RCextensions.loadimage(iteratorVariable21, mipmap, 0x7a120);
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
            foreach (var iteratorVariable23 in groundList)
            {
                if ((iteratorVariable23 != null) && (iteratorVariable23.GetComponent<Renderer>() != null))
                {
                    foreach (var iteratorVariable24 in iteratorVariable23.GetComponentsInChildren<Renderer>())
                    {
                        if (!linkHash[0].ContainsKey(key))
                        {
                            var iteratorVariable25 = new WWW(key);
                            yield return iteratorVariable25;
                            var iteratorVariable26 = RCextensions.loadimage(iteratorVariable25, mipmap, 0x30d40);
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
            foreach (var obj2 in groundList)
            {
                if ((obj2 != null) && (obj2.GetComponent<Renderer>() != null))
                {
                    foreach (var renderer in obj2.GetComponentsInChildren<Renderer>())
                    {
                        renderer.enabled = false;
                    }
                }
            }
        }
        if (iteratorVariable2)
        {
            unloadAssets();
        }
    }

    public int conditionType(string str)
    {
        if (!str.StartsWith("Int"))
        {
            if (str.StartsWith("Bool"))
            {
                return 1;
            }
            if (str.StartsWith("String"))
            {
                return 2;
            }
            if (str.StartsWith("Float"))
            {
                return 3;
            }
            if (str.StartsWith("Titan"))
            {
                return 5;
            }
            if (str.StartsWith("Player"))
            {
                return 4;
            }
        }
        return 0;
    }

    private void core2()
    {
        if (((int) settings[0x40]) >= 100)
        {
            coreeditor();
        }
        else
        {
            if ((IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.SINGLE) && needChooseSide)
            {
                InGameUI.SpawnMenu.gameObject.SetActive(true);
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            if ((IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE) || (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER))
            {
                int length;
                float num3;
                if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER)
                {
                    coreadd();
                    ShowHUDInfoTopLeft(playerList);
                    if ((((Camera.main != null) && (Gamemode.Settings.GamemodeType != GamemodeType.Racing)) && (Camera.main.GetComponent<IN_GAME_MAIN_CAMERA>().gameOver && !needChooseSide)) && (((int) settings[0xf5]) == 0))
                    {
                        ShowHUDInfoCenter("Press [F7D358]" + inputManager.inputString[InputCode.flare1] + "[-] to spectate the next player. \nPress [F7D358]" + inputManager.inputString[InputCode.flare2] + "[-] to spectate the previous player.\nPress [F7D358]" + inputManager.inputString[InputCode.attack1] + "[-] to enter the spectator mode.\n\n\n\n");
                        if (((Gamemode.Settings.RespawnMode == RespawnMode.DEATHMATCH) || (Gamemode.Settings.EndlessRevive > 0)) || !(((Gamemode.Settings.PvPBomb) || (Gamemode.Settings.Pvp != PvpMode.Disabled)) ? (Gamemode.Settings.PointMode <= 0) : true))
                        {
                            myRespawnTime += Time.deltaTime;
                            var endlessMode = 5;
                            if (RCextensions.returnIntFromObject(PhotonNetwork.player.CustomProperties[PhotonPlayerProperty.isTitan]) == 2)
                            {
                                endlessMode = 10;
                            }
                            if (Gamemode.Settings.EndlessRevive > 0)
                            {
                                endlessMode = Gamemode.Settings.EndlessRevive;
                            }
                            length = endlessMode - ((int) myRespawnTime);
                            ShowHUDInfoCenterADD("Respawn in " + length.ToString() + "s.");
                            if (myRespawnTime > endlessMode)
                            {
                                myRespawnTime = 0f;
                                Camera.main.GetComponent<IN_GAME_MAIN_CAMERA>().gameOver = false;
                                if (RCextensions.returnIntFromObject(PhotonNetwork.player.CustomProperties[PhotonPlayerProperty.isTitan]) == 2)
                                {
                                    SpawnPlayerTitan();
                                }
                                else
                                {
                                    StartCoroutine(WaitAndRespawn1(0.1f, myLastRespawnTag));
                                }
                                Camera.main.GetComponent<IN_GAME_MAIN_CAMERA>().gameOver = false;
                                ShowHUDInfoCenter(string.Empty);
                            }
                        }
                    }
                }
                else if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
                {
                    if (Gamemode.Settings.GamemodeType == GamemodeType.Racing)
                    {
                        if (!isLosing)
                        {
                            currentSpeed = Camera.main.GetComponent<IN_GAME_MAIN_CAMERA>().main_object.GetComponent<Rigidbody>().velocity.magnitude;
                            maxSpeed = Mathf.Max(maxSpeed, currentSpeed);
                            ShowHUDInfoTopLeft(string.Concat(new object[] { "Current Speed : ", (int) currentSpeed, "\nMax Speed:", maxSpeed }));
                        }
                    }
                    else
                    {
                        ShowHUDInfoTopLeft(string.Concat(new object[] { "Kills:", single_kills, "\nMax Damage:", single_maxDamage, "\nTotal Damage:", single_totalDamage }));
                    }
                }
                if (isLosing && (Gamemode.Settings.GamemodeType != GamemodeType.Racing))
                {
                    ShowHUDInfoCenter(Gamemode.GetDefeatMessage(gameEndCD));
                    if (IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.SINGLE)
                    {
                        if (gameEndCD <= 0f)
                        {
                            gameEndCD = 0f;
                            if (PhotonNetwork.isMasterClient)
                            {
                                restartRC();
                            }

                            ShowHUDInfoCenter(string.Empty);
                        }
                        else
                        {
                            gameEndCD -= Time.deltaTime;
                        }
                    }
                }
                if (isWinning)
                {
                    ShowHUDInfoCenter(Gamemode.GetVictoryMessage(gameEndCD, timeTotalServer));
                    if (gameEndCD <= 0f)
                    {
                        gameEndCD = 0f;
                        if (PhotonNetwork.isMasterClient)
                        {
                            restartRC();
                        }
                        ShowHUDInfoCenter(string.Empty);
                    }
                    else
                    {
                        gameEndCD -= Time.deltaTime;
                    }

                }
                timeElapse += Time.deltaTime;
                roundTime += Time.deltaTime;
                if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
                {
                    //TODO Investigate the purpose of this
                    if (Gamemode.Settings.GamemodeType == GamemodeType.Racing)
                    {
                        if (!isWinning)
                        {
                            timeTotalServer += Time.deltaTime;
                        }
                    }
                    else if (!(isLosing || isWinning))
                    {
                        timeTotalServer += Time.deltaTime;
                    }
                }
                else
                {
                    timeTotalServer += Time.deltaTime;
                }
                if (Gamemode.Settings.GamemodeType == GamemodeType.Racing)
                {
                    if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
                    {
                        if (!isWinning)
                        {
                            ShowHUDInfoTopCenter("Time : " + ((((int) (timeTotalServer * 10f)) * 0.1f) - 5f));
                        }
                        if (timeTotalServer < 5f)
                        {
                            ShowHUDInfoCenter("RACE START IN " + ((int) (5f - timeTotalServer)));
                        }
                        else if (!startRacing)
                        {
                            ShowHUDInfoCenter(string.Empty);
                            startRacing = true;
                            endRacing = false;
                            GameObject.Find("door").SetActive(false);
                        }
                    }
                    else
                    {
                        ShowHUDInfoTopCenter("Time : " + ((roundTime >= 20f) ? (num3 = (((int) (roundTime * 10f)) * 0.1f) - 20f).ToString() : "WAITING"));
                        if (roundTime < 20f)
                        {
                            ShowHUDInfoCenter("RACE START IN " + ((int) (20f - roundTime)) + (!(localRacingResult == string.Empty) ? ("\nLast Round\n" + localRacingResult) : "\n\n"));
                        }
                        else if (!startRacing)
                        {
                            ShowHUDInfoCenter(string.Empty);
                            startRacing = true;
                            endRacing = false;
                            var obj2 = GameObject.Find("door");
                            if (obj2 != null)
                            {
                                obj2.SetActive(false);
                            }
                            if ((racingDoors != null) && customLevelLoaded)
                            {
                                foreach (var obj3 in racingDoors)
                                {
                                    obj3.SetActive(false);
                                }
                                racingDoors = null;
                            }
                        }
                        else if ((racingDoors != null) && customLevelLoaded)
                        {
                            foreach (var obj3 in racingDoors)
                            {
                                obj3.SetActive(false);
                            }
                            racingDoors = null;
                        }
                    }
                    if ((Camera.main.GetComponent<IN_GAME_MAIN_CAMERA>().gameOver && !needChooseSide) && customLevelLoaded)
                    {
                        myRespawnTime += Time.deltaTime;
                        if (myRespawnTime > 1.5f)
                        {
                            myRespawnTime = 0f;
                            Camera.main.GetComponent<IN_GAME_MAIN_CAMERA>().gameOver = false;
                            if (checkpoint != null)
                            {
                                StartCoroutine(WaitAndRespawn2(0.1f, checkpoint));
                            }
                            else
                            {
                                StartCoroutine(WaitAndRespawn1(0.1f, myLastRespawnTag));
                            }
                            Camera.main.GetComponent<IN_GAME_MAIN_CAMERA>().gameOver = false;
                            ShowHUDInfoCenter(string.Empty);
                        }
                    }
                }
                if (timeElapse > 1f)
                {
                    timeElapse--;
                    var content = Gamemode.GetGamemodeStatusTop((int) timeTotalServer, time);
                    if (Gamemode.Settings.TeamMode != TeamMode.Disabled)
                    {
                        content += $"\n<color=#00ffff>Cyan: {cyanKills}</color><color=#ff00ff>       Magenta: {magentaKills}</color>";
                    }
                    ShowHUDInfoTopCenter(content);
                    content = Gamemode.GetGamemodeStatusTopRight((int) timeTotalServer, time);
                    ShowHUDInfoTopRight(content);
                    var str4 = (IN_GAME_MAIN_CAMERA.difficulty >= 0) ? ((IN_GAME_MAIN_CAMERA.difficulty != 0) ? ((IN_GAME_MAIN_CAMERA.difficulty != 1) ? "Abnormal" : "Hard") : "Normal") : "Trainning";
                    ShowHUDInfoTopRightMAPNAME("\n" + Level.Name + " : " + str4);
                    if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER)
                    {
                        var separator = new char[] { "`"[0] };
                        var str5 = PhotonNetwork.room.name.Split(separator)[0];
                        if (str5.Length > 20)
                        {
                            str5 = str5.Remove(0x13) + "...";
                        }
                        ShowHUDInfoTopRightMAPNAME("\n" + str5 + " [FFC000](" + Convert.ToString(PhotonNetwork.room.playerCount) + "/" + Convert.ToString(PhotonNetwork.room.maxPlayers) + ")");
                        if (needChooseSide)
                        {
                            ShowHUDInfoTopCenterADD("\n\nPRESS 1 TO ENTER GAME");
                        }
                    }
                }
                if (((IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER) && (killInfoGO.Count > 0)) && (killInfoGO[0] == null))
                {
                    killInfoGO.RemoveAt(0);
                }
                if (((IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.SINGLE) && PhotonNetwork.isMasterClient) && (timeTotalServer > time))
                {
                    IN_GAME_MAIN_CAMERA.gametype = GAMETYPE.STOP;
                    gameStart = false;
                    Screen.lockCursor = false;
                    Cursor.visible = true;
                    var names = string.Empty;
                    var kills = string.Empty;
                    var deaths = string.Empty;
                    var maxDamage = string.Empty;
                    var totalDamage = string.Empty;
                    foreach (var player in PhotonNetwork.playerList)
                    {
                        if (player != null)
                        {
                            names = names + player.CustomProperties[PhotonPlayerProperty.name] + "\n";
                            kills = kills + player.CustomProperties[PhotonPlayerProperty.kills] + "\n";
                            deaths = deaths + player.CustomProperties[PhotonPlayerProperty.deaths] + "\n";
                            maxDamage = maxDamage + player.CustomProperties[PhotonPlayerProperty.max_dmg] + "\n";
                            totalDamage = totalDamage + player.CustomProperties[PhotonPlayerProperty.total_dmg] + "\n";
                        }
                    }

                    var roundEndMessage = Gamemode.GetRoundEndedMessage();
                    photonView.RPC<string, string, string, string, string, string, PhotonMessageInfo>(
                        showResult,
                        PhotonTargets.AllBuffered,
                        names,
                        kills,
                        deaths,
                        maxDamage,
                        totalDamage,
                        roundEndMessage);
                }
            }
        }
    }

    private void coreadd()
    {
        if (PhotonNetwork.isMasterClient)
        {
            if (customLevelLoaded)
            {
                for (var i = 0; i < titanSpawners.Count; i++)
                {
                    var item = titanSpawners[i];
                    item.time -= Time.deltaTime;
                    if ((item.time <= 0f) && ((titans.Count + fT.Count) < Gamemode.Settings.TitanLimit))
                    {
                        var name = item.name;
                        throw new NotImplementedException("Spawning titans on custom maps is not supported for mindless titans");
                        //if (name == "spawnAnnie")
                        //{
                        //    PhotonNetwork.Instantiate("FEMALE_TITAN", item.location, new Quaternion(0f, 0f, 0f, 1f), 0);
                        //}
                        //else
                        //{
                        //    GameObject obj2 = PhotonNetwork.Instantiate("TITAN_VER3.1", item.location, new Quaternion(0f, 0f, 0f, 1f), 0);
                        //    if (name == "spawnAbnormal")
                        //    {
                        //        obj2.GetComponent<TITAN>().setAbnormalType2(TitanType.TYPE_I, false);
                        //    }
                        //    else if (name == "spawnJumper")
                        //    {
                        //        obj2.GetComponent<TITAN>().setAbnormalType2(TitanType.TYPE_JUMPER, false);
                        //    }
                        //    else if (name == "spawnCrawler")
                        //    {
                        //        obj2.GetComponent<TITAN>().setAbnormalType2(TitanType.TYPE_CRAWLER, true);
                        //    }
                        //    else if (name == "spawnPunk")
                        //    {
                        //        obj2.GetComponent<TITAN>().setAbnormalType2(TitanType.TYPE_PUNK, false);
                        //    }
                        //}
                        //if (item.endless)
                        //{
                        //    item.time = item.delay;
                        //}
                        //else
                        //{
                        //    this.titanSpawners.Remove(item);
                        //}
                    }
                }
            }
        }
        if (Time.timeScale <= 0.1f)
        {
            if (pauseWaitTime <= 3f)
            {
                pauseWaitTime -= Time.deltaTime * 1000000f;
                if (pauseWaitTime <= 1f)
                {
                    Camera.main.farClipPlane = 1500f;
                }
                if (pauseWaitTime <= 0f)
                {
                    pauseWaitTime = 0f;
                    Time.timeScale = 1f;
                }
            }
            ReloadPlayerlist();
        }
    }

    private void coreeditor()
    {
        if (Input.GetKey(KeyCode.Tab))
        {
            GUI.FocusControl(null);
        }
        if (selectedObj != null)
        {
            var num = 0.2f;
            if (inputRC.isInputLevel(InputCodeRC.levelSlow))
            {
                num = 0.04f;
            }
            else if (inputRC.isInputLevel(InputCodeRC.levelFast))
            {
                num = 0.6f;
            }
            if (inputRC.isInputLevel(InputCodeRC.levelForward))
            {
                var transform = selectedObj.transform;
                transform.position += (Vector3) (num * new Vector3(Camera.main.transform.forward.x, 0f, Camera.main.transform.forward.z));
            }
            else if (inputRC.isInputLevel(InputCodeRC.levelBack))
            {
                var transform9 = selectedObj.transform;
                transform9.position -= (Vector3) (num * new Vector3(Camera.main.transform.forward.x, 0f, Camera.main.transform.forward.z));
            }
            if (inputRC.isInputLevel(InputCodeRC.levelLeft))
            {
                var transform10 = selectedObj.transform;
                transform10.position -= (Vector3) (num * new Vector3(Camera.main.transform.right.x, 0f, Camera.main.transform.right.z));
            }
            else if (inputRC.isInputLevel(InputCodeRC.levelRight))
            {
                var transform11 = selectedObj.transform;
                transform11.position += (Vector3) (num * new Vector3(Camera.main.transform.right.x, 0f, Camera.main.transform.right.z));
            }
            if (inputRC.isInputLevel(InputCodeRC.levelDown))
            {
                var transform12 = selectedObj.transform;
                transform12.position -= (Vector3) (Vector3.up * num);
            }
            else if (inputRC.isInputLevel(InputCodeRC.levelUp))
            {
                var transform13 = selectedObj.transform;
                transform13.position += (Vector3) (Vector3.up * num);
            }
            if (!selectedObj.name.StartsWith("misc,region"))
            {
                if (inputRC.isInputLevel(InputCodeRC.levelRRight))
                {
                    selectedObj.transform.Rotate((Vector3) (Vector3.up * num));
                }
                else if (inputRC.isInputLevel(InputCodeRC.levelRLeft))
                {
                    selectedObj.transform.Rotate((Vector3) (Vector3.down * num));
                }
                if (inputRC.isInputLevel(InputCodeRC.levelRCCW))
                {
                    selectedObj.transform.Rotate((Vector3) (Vector3.forward * num));
                }
                else if (inputRC.isInputLevel(InputCodeRC.levelRCW))
                {
                    selectedObj.transform.Rotate((Vector3) (Vector3.back * num));
                }
                if (inputRC.isInputLevel(InputCodeRC.levelRBack))
                {
                    selectedObj.transform.Rotate((Vector3) (Vector3.left * num));
                }
                else if (inputRC.isInputLevel(InputCodeRC.levelRForward))
                {
                    selectedObj.transform.Rotate((Vector3) (Vector3.right * num));
                }
            }
            if (inputRC.isInputLevel(InputCodeRC.levelPlace))
            {
                linkHash[3].Add(selectedObj.GetInstanceID(), selectedObj.name + "," + Convert.ToString(selectedObj.transform.position.x) + "," + Convert.ToString(selectedObj.transform.position.y) + "," + Convert.ToString(selectedObj.transform.position.z) + "," + Convert.ToString(selectedObj.transform.rotation.x) + "," + Convert.ToString(selectedObj.transform.rotation.y) + "," + Convert.ToString(selectedObj.transform.rotation.z) + "," + Convert.ToString(selectedObj.transform.rotation.w));
                selectedObj = null;
                //TODO Mouselook
                //Camera.main.GetComponent<MouseLook>().enabled = true;
                Screen.lockCursor = true;
            }
            if (inputRC.isInputLevel(InputCodeRC.levelDelete))
            {
                Destroy(selectedObj);
                selectedObj = null;
                //TODO Mouselook
                //Camera.main.GetComponent<MouseLook>().enabled = true;
                Screen.lockCursor = true;
                linkHash[3].Remove(selectedObj.GetInstanceID());
            }
        }
        else
        {
            if (Screen.lockCursor)
            {
                var num2 = 100f;
                if (inputRC.isInputLevel(InputCodeRC.levelSlow))
                {
                    num2 = 20f;
                }
                else if (inputRC.isInputLevel(InputCodeRC.levelFast))
                {
                    num2 = 400f;
                }
                var transform7 = Camera.main.transform;
                if (inputRC.isInputLevel(InputCodeRC.levelForward))
                {
                    transform7.position += (Vector3) ((transform7.forward * num2) * Time.deltaTime);
                }
                else if (inputRC.isInputLevel(InputCodeRC.levelBack))
                {
                    transform7.position -= (Vector3) ((transform7.forward * num2) * Time.deltaTime);
                }
                if (inputRC.isInputLevel(InputCodeRC.levelLeft))
                {
                    transform7.position -= (Vector3) ((transform7.right * num2) * Time.deltaTime);
                }
                else if (inputRC.isInputLevel(InputCodeRC.levelRight))
                {
                    transform7.position += (Vector3) ((transform7.right * num2) * Time.deltaTime);
                }
                if (inputRC.isInputLevel(InputCodeRC.levelUp))
                {
                    transform7.position += (Vector3) ((transform7.up * num2) * Time.deltaTime);
                }
                else if (inputRC.isInputLevel(InputCodeRC.levelDown))
                {
                    transform7.position -= (Vector3) ((transform7.up * num2) * Time.deltaTime);
                }
            }
            if (inputRC.isInputLevelDown(InputCodeRC.levelCursor))
            {
                if (Screen.lockCursor)
                {
                    //TODO Mouselook
                    //Camera.main.GetComponent<MouseLook>().enabled = false;
                    Screen.lockCursor = false;
                }
                else
                {
                    //TODO Mouselook
                    //Camera.main.GetComponent<MouseLook>().enabled = true;
                    Screen.lockCursor = true;
                }
            }
            if (((Input.GetKeyDown(KeyCode.Mouse0) && !Screen.lockCursor) && (GUIUtility.hotControl == 0)) && !(((Input.mousePosition.x <= 300f) || (Input.mousePosition.x >= (Screen.width - 300f))) ? ((Screen.height - Input.mousePosition.y) <= 600f) : false))
            {
                var hitInfo = new RaycastHit();
                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo))
                {
                    var transform8 = hitInfo.transform;
                    if ((((transform8.gameObject.name.StartsWith("custom") || transform8.gameObject.name.StartsWith("base")) || (transform8.gameObject.name.StartsWith("racing") || transform8.gameObject.name.StartsWith("photon"))) || transform8.gameObject.name.StartsWith("spawnpoint")) || transform8.gameObject.name.StartsWith("misc"))
                    {
                        selectedObj = transform8.gameObject;
                        //TODO Mouselook
                        //Camera.main.GetComponent<MouseLook>().enabled = false;
                        Screen.lockCursor = true;
                        linkHash[3].Remove(selectedObj.GetInstanceID());
                    }
                    else if (((transform8.parent.gameObject.name.StartsWith("custom") || transform8.parent.gameObject.name.StartsWith("base")) || transform8.parent.gameObject.name.StartsWith("racing")) || transform8.parent.gameObject.name.StartsWith("photon"))
                    {
                        selectedObj = transform8.parent.gameObject;
                        //TODO Mouselook
                        //Camera.main.GetComponent<MouseLook>().enabled = false;
                        Screen.lockCursor = true;
                        linkHash[3].Remove(selectedObj.GetInstanceID());
                    }
                }
            }
        }
    }

    private IEnumerator customlevelcache()
    {
        var iteratorVariable0 = 0;
        while (true)
        {
            if (iteratorVariable0 >= levelCache.Count)
            {
                yield break;
            }
            customlevelclientE(levelCache[iteratorVariable0], false);
            yield return new WaitForEndOfFrame();
            iteratorVariable0++;
        }
    }

    private void customlevelclientE(string[] content, bool renewHash)
    {
        int num;
        string[] strArray;
        var flag = false;
        var flag2 = false;
        if (content[content.Length - 1].StartsWith("a"))
        {
            flag = true;
        }
        else if (content[content.Length - 1].StartsWith("z"))
        {
            flag2 = true;
            customLevelLoaded = true;
            spawnPlayerCustomMap();
            unloadAssets();
            //TODO TiltShift
            //Camera.main.GetComponent<TiltShift>().enabled = false;
        }
        if (renewHash)
        {
            if (flag)
            {
                currentLevel = string.Empty;
                levelCache.Clear();
                titanSpawns.Clear();
                playerSpawnsC.Clear();
                playerSpawnsM.Clear();
                for (num = 0; num < content.Length; num++)
                {
                    strArray = content[num].Split(new char[] { ',' });
                    if (strArray[0] == "titan")
                    {
                        titanSpawns.Add(new Vector3(Convert.ToSingle(strArray[1]), Convert.ToSingle(strArray[2]), Convert.ToSingle(strArray[3])));
                    }
                    else if (strArray[0] == "playerC")
                    {
                        playerSpawnsC.Add(new Vector3(Convert.ToSingle(strArray[1]), Convert.ToSingle(strArray[2]), Convert.ToSingle(strArray[3])));
                    }
                    else if (strArray[0] == "playerM")
                    {
                        playerSpawnsM.Add(new Vector3(Convert.ToSingle(strArray[1]), Convert.ToSingle(strArray[2]), Convert.ToSingle(strArray[3])));
                    }
                }
                spawnPlayerCustomMap();
            }
            currentLevel = currentLevel + content[content.Length - 1];
            levelCache.Add(content);
            var propertiesToSet = new ExitGames.Client.Photon.Hashtable
            {
                { PhotonPlayerProperty.currentLevel, currentLevel }
            };
            PhotonNetwork.player.SetCustomProperties(propertiesToSet);
        }
        if (!flag && !flag2)
        {
            for (num = 0; num < content.Length; num++)
            {
                float num2;
                GameObject obj2;
                float num3;
                float num5;
                float num6;
                float num7;
                Color color;
                Mesh mesh;
                Color[] colorArray;
                int num8;
                strArray = content[num].Split(new char[] { ',' });
                if (strArray[0].StartsWith("custom"))
                {
                    num2 = 1f;
                    obj2 = null;
                    obj2 = (GameObject) Instantiate((GameObject) RCassets.LoadAsset(strArray[1]), new Vector3(Convert.ToSingle(strArray[12]), Convert.ToSingle(strArray[13]), Convert.ToSingle(strArray[14])), new Quaternion(Convert.ToSingle(strArray[15]), Convert.ToSingle(strArray[0x10]), Convert.ToSingle(strArray[0x11]), Convert.ToSingle(strArray[0x12])));
                    if (strArray[2] != "default")
                    {
                        if (strArray[2].StartsWith("transparent"))
                        {
                            if (float.TryParse(strArray[2].Substring(11), out num3))
                            {
                                num2 = num3;
                            }
                            foreach (var renderer in obj2.GetComponentsInChildren<Renderer>())
                            {
                                renderer.material = (Material) RCassets.LoadAsset("transparent");
                                if ((Convert.ToSingle(strArray[10]) != 1f) || (Convert.ToSingle(strArray[11]) != 1f))
                                {
                                    renderer.material.mainTextureScale = new Vector2(renderer.material.mainTextureScale.x * Convert.ToSingle(strArray[10]), renderer.material.mainTextureScale.y * Convert.ToSingle(strArray[11]));
                                }
                            }
                        }
                        else
                        {
                            foreach (var renderer in obj2.GetComponentsInChildren<Renderer>())
                            {
                                renderer.material = (Material) RCassets.LoadAsset(strArray[2]);
                                if ((Convert.ToSingle(strArray[10]) != 1f) || (Convert.ToSingle(strArray[11]) != 1f))
                                {
                                    renderer.material.mainTextureScale = new Vector2(renderer.material.mainTextureScale.x * Convert.ToSingle(strArray[10]), renderer.material.mainTextureScale.y * Convert.ToSingle(strArray[11]));
                                }
                            }
                        }
                    }
                    num5 = obj2.transform.localScale.x * Convert.ToSingle(strArray[3]);
                    num5 -= 0.001f;
                    num6 = obj2.transform.localScale.y * Convert.ToSingle(strArray[4]);
                    num7 = obj2.transform.localScale.z * Convert.ToSingle(strArray[5]);
                    obj2.transform.localScale = new Vector3(num5, num6, num7);
                    if (strArray[6] != "0")
                    {
                        color = new Color(Convert.ToSingle(strArray[7]), Convert.ToSingle(strArray[8]), Convert.ToSingle(strArray[9]), num2);
                        foreach (var filter in obj2.GetComponentsInChildren<MeshFilter>())
                        {
                            mesh = filter.mesh;
                            colorArray = new Color[mesh.vertexCount];
                            num8 = 0;
                            while (num8 < mesh.vertexCount)
                            {
                                colorArray[num8] = color;
                                num8++;
                            }
                            mesh.colors = colorArray;
                        }
                    }
                }
                else if (strArray[0].StartsWith("base"))
                {
                    if (strArray.Length < 15)
                    {
                        Instantiate(Resources.Load(strArray[1]), new Vector3(Convert.ToSingle(strArray[2]), Convert.ToSingle(strArray[3]), Convert.ToSingle(strArray[4])), new Quaternion(Convert.ToSingle(strArray[5]), Convert.ToSingle(strArray[6]), Convert.ToSingle(strArray[7]), Convert.ToSingle(strArray[8])));
                    }
                    else
                    {
                        num2 = 1f;
                        obj2 = null;
                        obj2 = (GameObject) Instantiate((GameObject) Resources.Load(strArray[1]), new Vector3(Convert.ToSingle(strArray[12]), Convert.ToSingle(strArray[13]), Convert.ToSingle(strArray[14])), new Quaternion(Convert.ToSingle(strArray[15]), Convert.ToSingle(strArray[0x10]), Convert.ToSingle(strArray[0x11]), Convert.ToSingle(strArray[0x12])));
                        if (strArray[2] != "default")
                        {
                            if (strArray[2].StartsWith("transparent"))
                            {
                                if (float.TryParse(strArray[2].Substring(11), out num3))
                                {
                                    num2 = num3;
                                }
                                foreach (var renderer in obj2.GetComponentsInChildren<Renderer>())
                                {
                                    renderer.material = (Material) RCassets.LoadAsset("transparent");
                                    if ((Convert.ToSingle(strArray[10]) != 1f) || (Convert.ToSingle(strArray[11]) != 1f))
                                    {
                                        renderer.material.mainTextureScale = new Vector2(renderer.material.mainTextureScale.x * Convert.ToSingle(strArray[10]), renderer.material.mainTextureScale.y * Convert.ToSingle(strArray[11]));
                                    }
                                }
                            }
                            else
                            {
                                foreach (var renderer in obj2.GetComponentsInChildren<Renderer>())
                                {
                                    if (!(renderer.name.Contains("Particle System") && obj2.name.Contains("aot_supply")))
                                    {
                                        renderer.material = (Material) RCassets.LoadAsset(strArray[2]);
                                        if ((Convert.ToSingle(strArray[10]) != 1f) || (Convert.ToSingle(strArray[11]) != 1f))
                                        {
                                            renderer.material.mainTextureScale = new Vector2(renderer.material.mainTextureScale.x * Convert.ToSingle(strArray[10]), renderer.material.mainTextureScale.y * Convert.ToSingle(strArray[11]));
                                        }
                                    }
                                }
                            }
                        }
                        num5 = obj2.transform.localScale.x * Convert.ToSingle(strArray[3]);
                        num5 -= 0.001f;
                        num6 = obj2.transform.localScale.y * Convert.ToSingle(strArray[4]);
                        num7 = obj2.transform.localScale.z * Convert.ToSingle(strArray[5]);
                        obj2.transform.localScale = new Vector3(num5, num6, num7);
                        if (strArray[6] != "0")
                        {
                            color = new Color(Convert.ToSingle(strArray[7]), Convert.ToSingle(strArray[8]), Convert.ToSingle(strArray[9]), num2);
                            foreach (var filter in obj2.GetComponentsInChildren<MeshFilter>())
                            {
                                mesh = filter.mesh;
                                colorArray = new Color[mesh.vertexCount];
                                for (num8 = 0; num8 < mesh.vertexCount; num8++)
                                {
                                    colorArray[num8] = color;
                                }
                                mesh.colors = colorArray;
                            }
                        }
                    }
                }
                else if (strArray[0].StartsWith("misc"))
                {
                    if (strArray[1].StartsWith("barrier"))
                    {
                        obj2 = null;
                        obj2 = (GameObject) Instantiate((GameObject) RCassets.LoadAsset(strArray[1]), new Vector3(Convert.ToSingle(strArray[5]), Convert.ToSingle(strArray[6]), Convert.ToSingle(strArray[7])), new Quaternion(Convert.ToSingle(strArray[8]), Convert.ToSingle(strArray[9]), Convert.ToSingle(strArray[10]), Convert.ToSingle(strArray[11])));
                        num5 = obj2.transform.localScale.x * Convert.ToSingle(strArray[2]);
                        num5 -= 0.001f;
                        num6 = obj2.transform.localScale.y * Convert.ToSingle(strArray[3]);
                        num7 = obj2.transform.localScale.z * Convert.ToSingle(strArray[4]);
                        obj2.transform.localScale = new Vector3(num5, num6, num7);
                    }
                    else if (strArray[1].StartsWith("racingStart"))
                    {
                        obj2 = null;
                        obj2 = (GameObject) Instantiate((GameObject) RCassets.LoadAsset(strArray[1]), new Vector3(Convert.ToSingle(strArray[5]), Convert.ToSingle(strArray[6]), Convert.ToSingle(strArray[7])), new Quaternion(Convert.ToSingle(strArray[8]), Convert.ToSingle(strArray[9]), Convert.ToSingle(strArray[10]), Convert.ToSingle(strArray[11])));
                        num5 = obj2.transform.localScale.x * Convert.ToSingle(strArray[2]);
                        num5 -= 0.001f;
                        num6 = obj2.transform.localScale.y * Convert.ToSingle(strArray[3]);
                        num7 = obj2.transform.localScale.z * Convert.ToSingle(strArray[4]);
                        obj2.transform.localScale = new Vector3(num5, num6, num7);
                        if (racingDoors != null)
                        {
                            racingDoors.Add(obj2);
                        }
                    }
                    else if (strArray[1].StartsWith("racingEnd"))
                    {
                        obj2 = null;
                        obj2 = (GameObject) Instantiate((GameObject) RCassets.LoadAsset(strArray[1]), new Vector3(Convert.ToSingle(strArray[5]), Convert.ToSingle(strArray[6]), Convert.ToSingle(strArray[7])), new Quaternion(Convert.ToSingle(strArray[8]), Convert.ToSingle(strArray[9]), Convert.ToSingle(strArray[10]), Convert.ToSingle(strArray[11])));
                        num5 = obj2.transform.localScale.x * Convert.ToSingle(strArray[2]);
                        num5 -= 0.001f;
                        num6 = obj2.transform.localScale.y * Convert.ToSingle(strArray[3]);
                        num7 = obj2.transform.localScale.z * Convert.ToSingle(strArray[4]);
                        obj2.transform.localScale = new Vector3(num5, num6, num7);
                        obj2.AddComponent<LevelTriggerRacingEnd>();
                    }
                    else if (strArray[1].StartsWith("region") && PhotonNetwork.isMasterClient)
                    {
                        var loc = new Vector3(Convert.ToSingle(strArray[6]), Convert.ToSingle(strArray[7]), Convert.ToSingle(strArray[8]));
                        var region = new RCRegion(loc, Convert.ToSingle(strArray[3]), Convert.ToSingle(strArray[4]), Convert.ToSingle(strArray[5]));
                        var key = strArray[2];
                        if (RCRegionTriggers.ContainsKey(key))
                        {
                            var obj3 = (GameObject) Instantiate((GameObject) RCassets.LoadAsset("region"));
                            obj3.transform.position = loc;
                            //obj3.AddComponent<RegionTrigger>();
                            //obj3.GetComponent<RegionTrigger>().CopyTrigger((RegionTrigger)RCRegionTriggers[key]);
                            num5 = obj3.transform.localScale.x * Convert.ToSingle(strArray[3]);
                            num5 -= 0.001f;
                            num6 = obj3.transform.localScale.y * Convert.ToSingle(strArray[4]);
                            num7 = obj3.transform.localScale.z * Convert.ToSingle(strArray[5]);
                            obj3.transform.localScale = new Vector3(num5, num6, num7);
                            region.myBox = obj3;
                        }
                        RCRegions.Add(key, region);
                    }
                }
                else if (strArray[0].StartsWith("racing"))
                {
                    if (strArray[1].StartsWith("start"))
                    {
                        obj2 = null;
                        obj2 = (GameObject) Instantiate((GameObject) RCassets.LoadAsset(strArray[1]), new Vector3(Convert.ToSingle(strArray[5]), Convert.ToSingle(strArray[6]), Convert.ToSingle(strArray[7])), new Quaternion(Convert.ToSingle(strArray[8]), Convert.ToSingle(strArray[9]), Convert.ToSingle(strArray[10]), Convert.ToSingle(strArray[11])));
                        num5 = obj2.transform.localScale.x * Convert.ToSingle(strArray[2]);
                        num5 -= 0.001f;
                        num6 = obj2.transform.localScale.y * Convert.ToSingle(strArray[3]);
                        num7 = obj2.transform.localScale.z * Convert.ToSingle(strArray[4]);
                        obj2.transform.localScale = new Vector3(num5, num6, num7);
                        if (racingDoors != null)
                        {
                            racingDoors.Add(obj2);
                        }
                    }
                    else if (strArray[1].StartsWith("end"))
                    {
                        obj2 = null;
                        obj2 = (GameObject) Instantiate((GameObject) RCassets.LoadAsset(strArray[1]), new Vector3(Convert.ToSingle(strArray[5]), Convert.ToSingle(strArray[6]), Convert.ToSingle(strArray[7])), new Quaternion(Convert.ToSingle(strArray[8]), Convert.ToSingle(strArray[9]), Convert.ToSingle(strArray[10]), Convert.ToSingle(strArray[11])));
                        num5 = obj2.transform.localScale.x * Convert.ToSingle(strArray[2]);
                        num5 -= 0.001f;
                        num6 = obj2.transform.localScale.y * Convert.ToSingle(strArray[3]);
                        num7 = obj2.transform.localScale.z * Convert.ToSingle(strArray[4]);
                        obj2.transform.localScale = new Vector3(num5, num6, num7);
                        obj2.GetComponentInChildren<Collider>().gameObject.AddComponent<LevelTriggerRacingEnd>();
                    }
                    else if (strArray[1].StartsWith("kill"))
                    {
                        obj2 = null;
                        obj2 = (GameObject) Instantiate((GameObject) RCassets.LoadAsset(strArray[1]), new Vector3(Convert.ToSingle(strArray[5]), Convert.ToSingle(strArray[6]), Convert.ToSingle(strArray[7])), new Quaternion(Convert.ToSingle(strArray[8]), Convert.ToSingle(strArray[9]), Convert.ToSingle(strArray[10]), Convert.ToSingle(strArray[11])));
                        num5 = obj2.transform.localScale.x * Convert.ToSingle(strArray[2]);
                        num5 -= 0.001f;
                        num6 = obj2.transform.localScale.y * Convert.ToSingle(strArray[3]);
                        num7 = obj2.transform.localScale.z * Convert.ToSingle(strArray[4]);
                        obj2.transform.localScale = new Vector3(num5, num6, num7);
                        obj2.GetComponentInChildren<Collider>().gameObject.AddComponent<RacingKillTrigger>();
                    }
                    else if (strArray[1].StartsWith("checkpoint"))
                    {
                        obj2 = null;
                        obj2 = (GameObject) Instantiate((GameObject) RCassets.LoadAsset(strArray[1]), new Vector3(Convert.ToSingle(strArray[5]), Convert.ToSingle(strArray[6]), Convert.ToSingle(strArray[7])), new Quaternion(Convert.ToSingle(strArray[8]), Convert.ToSingle(strArray[9]), Convert.ToSingle(strArray[10]), Convert.ToSingle(strArray[11])));
                        num5 = obj2.transform.localScale.x * Convert.ToSingle(strArray[2]);
                        num5 -= 0.001f;
                        num6 = obj2.transform.localScale.y * Convert.ToSingle(strArray[3]);
                        num7 = obj2.transform.localScale.z * Convert.ToSingle(strArray[4]);
                        obj2.transform.localScale = new Vector3(num5, num6, num7);
                        obj2.GetComponentInChildren<Collider>().gameObject.AddComponent<RacingCheckpointTrigger>();
                    }
                }
                else if (strArray[0].StartsWith("map"))
                {
                    if (strArray[1].StartsWith("disablebounds"))
                    {
                        Destroy(GameObject.Find("gameobjectOutSide"));
                        Instantiate(RCassets.LoadAsset("outside"));
                    }
                }
                else if (PhotonNetwork.isMasterClient && strArray[0].StartsWith("photon"))
                {
                    if (strArray[1].StartsWith("Cannon"))
                    {
                        if (strArray.Length > 15)
                        {
                            var cannon = PhotonNetwork.Instantiate("RC Resources/RC Prefabs/" + "Unmanned" + strArray[1], new Vector3(Convert.ToSingle(strArray[12]), Convert.ToSingle(strArray[13]), Convert.ToSingle(strArray[14])), new Quaternion(Convert.ToSingle(strArray[15]), Convert.ToSingle(strArray[0x10]), Convert.ToSingle(strArray[0x11]), Convert.ToSingle(strArray[0x12])), 0).GetComponent<UnmannedCannon>();
                            cannon.photonView.RPC<string, PhotonMessageInfo>(cannon.SetSize, PhotonTargets.AllBuffered, cannon.settings = content[num]);
                        }
                        else
                        {
                            PhotonNetwork.Instantiate("RC Resources/RC Prefabs/" + "Unmanned" + strArray[1], new Vector3(Convert.ToSingle(strArray[2]), Convert.ToSingle(strArray[3]), Convert.ToSingle(strArray[4])), new Quaternion(Convert.ToSingle(strArray[5]), Convert.ToSingle(strArray[6]), Convert.ToSingle(strArray[7]), Convert.ToSingle(strArray[8])), 0).GetComponent<UnmannedCannon>().settings = content[num];
                        }
                    }
                    else
                    {
                        var item = new TitanSpawner();
                        num5 = 30f;
                        if (float.TryParse(strArray[2], out num3))
                        {
                            num5 = Mathf.Max(Convert.ToSingle(strArray[2]), 1f);
                        }
                        item.time = num5;
                        item.delay = num5;
                        item.name = strArray[1];
                        if (strArray[3] == "1")
                        {
                            item.endless = true;
                        }
                        else
                        {
                            item.endless = false;
                        }
                        item.location = new Vector3(Convert.ToSingle(strArray[4]), Convert.ToSingle(strArray[5]), Convert.ToSingle(strArray[6]));
                        titanSpawners.Add(item);
                    }
                }
            }
        }
    }

    private IEnumerator customlevelE(List<PhotonPlayer> players)
    {
        string[] strArray;
        if (!(currentLevel == string.Empty))
        {
            for (var i = 0; i < levelCache.Count; i++)
            {
                foreach (var player in players)
                {
                    if (((player.CustomProperties[PhotonPlayerProperty.currentLevel] != null) && (currentLevel != string.Empty)) && (RCextensions.returnStringFromObject(player.CustomProperties[PhotonPlayerProperty.currentLevel]) == currentLevel))
                    {
                        if (i == 0)
                        {
                            strArray = new string[] { "loadcached" };
                            photonView.RPC("customlevelRPC", player, new object[] { strArray });
                        }
                    }
                    else
                    {
                        photonView.RPC("customlevelRPC", player, new object[] { levelCache[i] });
                    }
                }
                if (i > 0)
                {
                    yield return new WaitForSeconds(0.75f);
                }
                else
                {
                    yield return new WaitForSeconds(0.25f);
                }
            }
        }
        else
        {
            strArray = new string[] { "loadempty" };
            foreach (var player in players)
            {
                photonView.RPC("customlevelRPC", player, new object[] { strArray });
            }
            customLevelLoaded = true;
        }
    }

    [PunRPC]
    private void customlevelRPC(string[] content, PhotonMessageInfo info)
    {
        if (info.sender.isMasterClient)
        {
            if ((content.Length == 1) && (content[0] == "loadcached"))
            {
                StartCoroutine(customlevelcache());
            }
            else if ((content.Length == 1) && (content[0] == "loadempty"))
            {
                currentLevel = string.Empty;
                levelCache.Clear();
                titanSpawns.Clear();
                playerSpawnsC.Clear();
                playerSpawnsM.Clear();
                var propertiesToSet = new ExitGames.Client.Photon.Hashtable
                {
                    { PhotonPlayerProperty.currentLevel, currentLevel }
                };
                PhotonNetwork.player.SetCustomProperties(propertiesToSet);
                customLevelLoaded = true;
                spawnPlayerCustomMap();
            }
            else
            {
                customlevelclientE(content, true);
            }
        }
    }

    public void debugChat(string str)
    {
        chatRoom.addLINE(str);
    }

    public void DestroyAllExistingCloths()
    {
        var clothArray = FindObjectsOfType<Cloth>();
        if (clothArray.Length > 0)
        {
            for (var i = 0; i < clothArray.Length; i++)
            {
                ClothFactory.DisposeObject(clothArray[i].gameObject);
            }
        }
    }

    private void endGameRC()
    {
    }

    public void EnterSpecMode(bool enter)
    {
        if (enter)
        {
            spectateSprites = new List<GameObject>();
            foreach (GameObject obj2 in FindObjectsOfType(typeof(GameObject)))
            {
                //if ((obj2.GetComponent<UISprite>() != null) && obj2.activeInHierarchy)
                //{
                //    string name = obj2.name;
                //    if (((name.Contains("blade") || name.Contains("bullet")) || (name.Contains("gas") || name.Contains("flare"))) || name.Contains("skill_cd"))
                //    {
                //        if (!this.spectateSprites.Contains(obj2))
                //        {
                //            this.spectateSprites.Add(obj2);
                //        }
                //        obj2.SetActive(false);
                //    }
                //}
            }
            var strArray2 = new string[] { "Flare", "LabelInfoBottomRight" };
            foreach (var str2 in strArray2)
            {
                var item = GameObject.Find(str2);
                if (item != null)
                {
                    if (!spectateSprites.Contains(item))
                    {
                        spectateSprites.Add(item);
                    }
                    item.SetActive(false);
                }
            }
            foreach (Hero hero in instance.getPlayers())
            {
                if (hero.photonView.isMine)
                {
                    PhotonNetwork.Destroy(hero.photonView);
                }
            }
            if ((RCextensions.returnIntFromObject(PhotonNetwork.player.CustomProperties[PhotonPlayerProperty.isTitan]) == 2) && !RCextensions.returnBoolFromObject(PhotonNetwork.player.CustomProperties[PhotonPlayerProperty.dead]))
            {
                foreach (MindlessTitan titan in instance.getTitans())
                {
                    if (titan.photonView.isMine)
                    {
                        PhotonNetwork.Destroy(titan.photonView);
                    }
                }
            }
            instance.needChooseSide = false;
            Camera.main.GetComponent<IN_GAME_MAIN_CAMERA>().enabled = true;
            if (IN_GAME_MAIN_CAMERA.cameraMode == CAMERA_TYPE.ORIGINAL)
            {
                Screen.lockCursor = false;
                Cursor.visible = false;
            }
            var obj4 = GameObject.FindGameObjectWithTag("Player");
            if ((obj4 != null) && (obj4.GetComponent<Hero>() != null))
            {
                Camera.main.GetComponent<IN_GAME_MAIN_CAMERA>().setMainObject(obj4, true, false);
            }
            else
            {
                Camera.main.GetComponent<IN_GAME_MAIN_CAMERA>().setMainObject(null, true, false);
            }
            Camera.main.GetComponent<IN_GAME_MAIN_CAMERA>().setSpectorMode(false);
            Camera.main.GetComponent<IN_GAME_MAIN_CAMERA>().gameOver = true;
            StartCoroutine(reloadSky());
        }
        else
        {
            if (GameObject.Find("cross1") != null)
            {
                GameObject.Find("cross1").transform.localPosition = (Vector3) (Vector3.up * 5000f);
            }
            if (spectateSprites != null)
            {
                foreach (var obj2 in spectateSprites)
                {
                    if (obj2 != null)
                    {
                        obj2.SetActive(true);
                    }
                }
            }
            spectateSprites = new List<GameObject>();
            instance.needChooseSide = true;
            Camera.main.GetComponent<IN_GAME_MAIN_CAMERA>().setMainObject(null, true, false);
            Camera.main.GetComponent<IN_GAME_MAIN_CAMERA>().setSpectorMode(true);
            Camera.main.GetComponent<IN_GAME_MAIN_CAMERA>().gameOver = true;
        }
    }

    public void gameLose2()
    {
        if (!(isWinning || isLosing))
        {
            EventManager.OnGameLost.Invoke();
            isLosing = true;
            gameEndCD = gameEndTotalCDtime;
        }
    }

    public void gameWin2()
    {
        if (!isLosing && !isWinning)
        {
            EventManager.OnGameWon.Invoke();
            isWinning = true;
        }
    }

    public ArrayList getPlayers()
    {
        return heroes;
    }

    [PunRPC]
    private void getRacingResult(string player, float time)
    {
        var result = new RacingResult
        {
            name = player,
            time = time
        };
        racingResult.Add(result);
        refreshRacingResult2();
    }

    public ArrayList getTitans()
    {
        return titans;
    }

    private string hairtype(int lol)
    {
        if (lol < 0)
        {
            return "Random";
        }
        return ("Male " + lol);
    }

    [PunRPC]
    private void ignorePlayer(int ID, PhotonMessageInfo info)
    {
        if (info.sender.isMasterClient)
        {
            var player = PhotonPlayer.Find(ID);
            if ((player != null) && !ignoreList.Contains(ID))
            {
                for (var i = 0; i < PhotonNetwork.playerList.Length; i++)
                {
                    if (PhotonNetwork.playerList[i] == player)
                    {
                        ignoreList.Add(ID);
                        var options = new RaiseEventOptions
                        {
                            TargetActors = new int[] { ID }
                        };
                        PhotonNetwork.RaiseEvent(0xfe, null, true, options);
                    }
                }
            }
        }
        RecompilePlayerList(0.1f);
    }

    [PunRPC]
    private void ignorePlayerArray(int[] IDS, PhotonMessageInfo info)
    {
        if (info.sender.isMasterClient)
        {
            for (var i = 0; i < IDS.Length; i++)
            {
                var iD = IDS[i];
                var player = PhotonPlayer.Find(iD);
                if ((player != null) && !ignoreList.Contains(iD))
                {
                    for (var j = 0; j < PhotonNetwork.playerList.Length; j++)
                    {
                        if (PhotonNetwork.playerList[j] == player)
                        {
                            ignoreList.Add(iD);
                            var options = new RaiseEventOptions
                            {
                                TargetActors = new int[] { iD }
                            };
                            PhotonNetwork.RaiseEvent(0xfe, null, true, options);
                        }
                    }
                }
            }
        }
        RecompilePlayerList(0.1f);
    }

    public static GameObject InstantiateCustomAsset(string key)
    {
        key = key.Substring(8);
        return (GameObject) RCassets.LoadAsset(key);
    }

    private void ReloadPlayerlist()
    {
        var playerList = "";
        foreach (var player in PhotonNetwork.playerList)
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
            photonView.RPC<int, PhotonMessageInfo>(ignorePlayer, PhotonTargets.Others, player.ID);
            if (!ignoreList.Contains(player.ID))
            {
                ignoreList.Add(player.ID);
                var options = new RaiseEventOptions
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
                chatRoom.addLINE("Player " + player.ID.ToString() + " was autobanned. Reason:" + reason);
            }
            RecompilePlayerList(0.1f);
        }
    }

    [PunRPC]
    private void labelRPC(int setting, PhotonMessageInfo info)
    {
        if (PhotonView.Find(setting) != null)
        {
            var owner = PhotonView.Find(setting).owner;
            if (owner == info.sender)
            {
                var str = RCextensions.returnStringFromObject(owner.CustomProperties[PhotonPlayerProperty.guildName]);
                var str2 = RCextensions.returnStringFromObject(owner.CustomProperties[PhotonPlayerProperty.name]);
                var gameObject = PhotonView.Find(setting).gameObject;
                if (gameObject != null)
                {
                    var component = gameObject.GetComponent<Hero>();
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

    private void LateUpdate()
    {
        if (gameStart)
        {
            var enumerator = heroes.GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    var current = (Hero) enumerator.Current;
                    if (current != null)
                        current.lateUpdate2();
                }
            }
            finally
            {
                var disposable = enumerator as IDisposable;
                if (disposable != null)
                {
                    disposable.Dispose();
                }
            }
            var enumerator2 = eT.GetEnumerator();
            try
            {
                while (enumerator2.MoveNext())
                {
                    var titanEren = (TITAN_EREN) enumerator2.Current;
                    if (titanEren != null)
                        titanEren.lateUpdate();
                }
            }
            finally
            {
                var disposable2 = enumerator2 as IDisposable;
                if (disposable2 != null)
                {
                    disposable2.Dispose();
                }
            }
            var enumerator4 = fT.GetEnumerator();
            try
            {
                while (enumerator4.MoveNext())
                {
                    var femaleTitan = (FEMALE_TITAN) enumerator4.Current;
                    if (femaleTitan != null)
                        femaleTitan.lateUpdate2();
                }
            }
            finally
            {
                var disposable4 = enumerator4 as IDisposable;
                if (disposable4 != null)
                {
                    disposable4.Dispose();
                }
            }
            core2();
        }
    }

    private void loadconfig()
    {
        int num;
        int num2;
        var objArray = new object[270];
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
        objArray[0xb5] = PlayerPrefs.GetInt("dashenable", 0);
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
        objArray[0xf5] = 0;
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
        inputRC = new InputManagerRC();
        inputRC.setInputHuman(InputCodeRC.reelin, (string) objArray[0x62]);
        inputRC.setInputHuman(InputCodeRC.reelout, (string) objArray[0x63]);
        inputRC.setInputHuman(InputCodeRC.dash, (string) objArray[0xb6]);
        inputRC.setInputHuman(InputCodeRC.mapMaximize, (string) objArray[0xe8]);
        inputRC.setInputHuman(InputCodeRC.mapToggle, (string) objArray[0xe9]);
        inputRC.setInputHuman(InputCodeRC.mapReset, (string) objArray[0xea]);
        inputRC.setInputHuman(InputCodeRC.chat, (string) objArray[0xec]);
        inputRC.setInputHuman(InputCodeRC.liveCam, (string) objArray[0x106]);
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
        for (num = 0; num < 15; num++)
        {
            inputRC.setInputTitan(num, (string) objArray[0x65 + num]);
        }
        for (num = 0; num < 0x10; num++)
        {
            inputRC.setInputLevel(num, (string) objArray[0x75 + num]);
        }
        for (num = 0; num < 7; num++)
        {
            inputRC.setInputHorse(num, (string) objArray[0xed + num]);
        }
        for (num = 0; num < 7; num++)
        {
            inputRC.setInputCannon(num, (string) objArray[0xfe + num]);
        }
        inputRC.setInputLevel(InputCodeRC.levelFast, (string) objArray[0xa1]);
        Application.targetFrameRate = -1;
        if (int.TryParse((string) objArray[0xb8], out num2) && (num2 > 0))
        {
            Application.targetFrameRate = num2;
        }
        AudioListener.volume = PlayerPrefs.GetFloat("vol", 1f);
        QualitySettings.masterTextureLimit = PlayerPrefs.GetInt("skinQ", 0);
        linkHash = new ExitGames.Client.Photon.Hashtable[] { new ExitGames.Client.Photon.Hashtable(), new ExitGames.Client.Photon.Hashtable(), new ExitGames.Client.Photon.Hashtable(), new ExitGames.Client.Photon.Hashtable(), new ExitGames.Client.Photon.Hashtable() };
        settings = objArray;
        scroll = Vector2.zero;
        scroll2 = Vector2.zero;
        distanceSlider = PlayerPrefs.GetFloat("cameraDistance", 1f);
        mouseSlider = PlayerPrefs.GetFloat("MouseSensitivity", 0.5f);
        qualitySlider = PlayerPrefs.GetFloat("GameQuality", 0f);
        transparencySlider = 1f;
    }

    private void loadskin()
    {
        GameObject[] objArray;
        int num;
        GameObject obj2;
        if (((int) settings[0x40]) >= 100)
        {
            var strArray2 = new string[] { "Flare", "LabelInfoBottomRight", "LabelNetworkStatus", "skill_cd_bottom", "GasUI" };
            objArray = (GameObject[]) FindObjectsOfType(typeof(GameObject));
            for (num = 0; num < objArray.Length; num++)
            {
                obj2 = objArray[num];
                if ((obj2.name.Contains("TREE") || obj2.name.Contains("aot_supply")) || obj2.name.Contains("gameobjectOutSide"))
                {
                    Destroy(obj2);
                }
            }
            GameObject.Find("Cube_001").GetComponent<Renderer>().material.mainTexture = ((Material) RCassets.LoadAsset("grass")).mainTexture;
            Instantiate(RCassets.LoadAsset("spawnPlayer"), new Vector3(-10f, 1f, -10f), new Quaternion(0f, 0f, 0f, 1f));
            for (num = 0; num < strArray2.Length; num++)
            {
                var name = strArray2[num];
                var obj3 = GameObject.Find(name);
                if (obj3 != null)
                {
                    Destroy(obj3);
                }
            }
            Camera.main.GetComponent<SpectatorMovement>().disable = true;
        }
        else
        {
            GameObject obj4;
            string[] customLevelUrls;
            int num2;
            InstantiateTracker.instance.Dispose();
            if ((IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER) && PhotonNetwork.isMasterClient)
            {
                updateTime = 1f;
                if (oldScriptLogic != currentScriptLogic)
                {
                    intVariables.Clear();
                    boolVariables.Clear();
                    stringVariables.Clear();
                    floatVariables.Clear();
                    globalVariables.Clear();
                    RCEvents.Clear();
                    RCVariableNames.Clear();
                    playerVariables.Clear();
                    titanVariables.Clear();
                    RCRegionTriggers.Clear();
                    oldScriptLogic = currentScriptLogic;
                }

                photonView.RPC<PhotonMessageInfo>(setMasterRC, PhotonTargets.All);
            }
            logicLoaded = true;
            racingSpawnPoint = new Vector3(0f, 0f, 0f);
            racingSpawnPointSet = false;
            racingDoors = new List<GameObject>();
            allowedToCannon = new Dictionary<int, CannonValues>();
            if ((!Level.Name.StartsWith("Custom") && (((int) settings[2]) == 1)) && ((IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE) || PhotonNetwork.isMasterClient))
            {
                var url = string.Empty;
                var str3 = string.Empty;
                var n = string.Empty;
                customLevelUrls = new string[] { string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty };
                if (Level.SceneName.Contains("City"))
                {
                    for (num = 0x33; num < 0x3b; num++)
                    {
                        url = url + ((string) settings[num]) + ",";
                    }
                    url.TrimEnd(new char[] { ',' });
                    num2 = 0;
                    while (num2 < 250)
                    {
                        n = n + Convert.ToString((int) UnityEngine.Random.Range((float) 0f, (float) 8f));
                        num2++;
                    }
                    str3 = ((string) settings[0x3b]) + "," + ((string) settings[60]) + "," + ((string) settings[0x3d]);
                    for (num = 0; num < 6; num++)
                    {
                        customLevelUrls[num] = (string) settings[num + 0xa9];
                    }
                }
                else if (Level.SceneName.Contains("Forest"))
                {
                    for (var i = 0x21; i < 0x29; i++)
                    {
                        url = url + ((string) settings[i]) + ",";
                    }
                    url.TrimEnd(new char[] { ',' });
                    for (var j = 0x29; j < 0x31; j++)
                    {
                        str3 = str3 + ((string) settings[j]) + ",";
                    }
                    str3 = str3 + ((string) settings[0x31]);
                    for (var k = 0; k < 150; k++)
                    {
                        var str5 = Convert.ToString((int) UnityEngine.Random.Range((float) 0f, (float) 8f));
                        n = n + str5;
                        if (((int) settings[50]) == 0)
                        {
                            n = n + str5;
                        }
                        else
                        {
                            n = n + Convert.ToString((int) UnityEngine.Random.Range((float) 0f, (float) 8f));
                        }
                    }
                    for (num = 0; num < 6; num++)
                    {
                        customLevelUrls[num] = (string) settings[num + 0xa3];
                    }
                }
                if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
                {
                    StartCoroutine(loadskinE(n, url, str3, customLevelUrls));
                }
                else if (PhotonNetwork.isMasterClient)
                {
                    photonView.RPC<string, string, string, string[], PhotonMessageInfo>(loadskinRPC, PhotonTargets.AllBuffered, new object[] { n, url, str3, customLevelUrls });
                }
            }
            else if (Level.Name.StartsWith("Custom") && (IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.SINGLE))
            {
                var objArray3 = GameObject.FindGameObjectsWithTag("playerRespawn");
                for (num = 0; num < objArray3.Length; num++)
                {
                    obj4 = objArray3[num];
                    obj4.transform.position = new Vector3(UnityEngine.Random.Range((float) -5f, (float) 5f), 0f, UnityEngine.Random.Range((float) -5f, (float) 5f));
                }
                objArray = (GameObject[]) FindObjectsOfType(typeof(GameObject));
                for (num = 0; num < objArray.Length; num++)
                {
                    obj2 = objArray[num];
                    if (obj2.name.Contains("TREE") || obj2.name.Contains("aot_supply"))
                    {
                        Destroy(obj2);
                    }
                    else if (((obj2.name == "Cube_001") && (obj2.transform.parent.gameObject.tag != "player")) && (obj2.GetComponent<Renderer>() != null))
                    {
                        groundList.Add(obj2);
                        obj2.GetComponent<Renderer>().material.mainTexture = ((Material) RCassets.LoadAsset("grass")).mainTexture;
                    }
                }
                if (PhotonNetwork.isMasterClient)
                {
                    customLevelUrls = new string[7];
                    // Custom sky textures.
                    for (num = 0; num < customLevelUrls.Length - 1; num++)
                        customLevelUrls[num] = (string) settings[num + 0xaf];

                    // Custom ground texture.
                    customLevelUrls[6] = (string) settings[0xa2];
                    photonView.RPC<string[], PhotonMessageInfo>(clearlevel, PhotonTargets.AllBuffered, customLevelUrls);
                    RCRegions.Clear();
                    if (oldScript != currentScript)
                    {
                        ExitGames.Client.Photon.Hashtable hashtable;
                        levelCache.Clear();
                        titanSpawns.Clear();
                        playerSpawnsC.Clear();
                        playerSpawnsM.Clear();
                        titanSpawners.Clear();
                        currentLevel = string.Empty;
                        if (currentScript == string.Empty)
                        {
                            hashtable = new ExitGames.Client.Photon.Hashtable
                            {
                                { PhotonPlayerProperty.currentLevel, currentLevel }
                            };
                            PhotonNetwork.player.SetCustomProperties(hashtable);
                            oldScript = currentScript;
                        }
                        else
                        {
                            var strArray4 = Regex.Replace(currentScript, @"\s+", "").Replace("\r\n", "").Replace("\n", "").Replace("\r", "").Split(new char[] { ';' });
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
                                                titanSpawns.Add(new Vector3(Convert.ToSingle(strArray6[2]), Convert.ToSingle(strArray6[3]), Convert.ToSingle(strArray6[4])));
                                            }
                                            else if (strArray6[1] == "playerC")
                                            {
                                                playerSpawnsC.Add(new Vector3(Convert.ToSingle(strArray6[2]), Convert.ToSingle(strArray6[3]), Convert.ToSingle(strArray6[4])));
                                            }
                                            else if (strArray6[1] == "playerM")
                                            {
                                                playerSpawnsM.Add(new Vector3(Convert.ToSingle(strArray6[2]), Convert.ToSingle(strArray6[3]), Convert.ToSingle(strArray6[4])));
                                            }
                                        }
                                        strArray5[num7] = strArray4[num2];
                                        num7++;
                                        num2++;
                                    }
                                    str6 = UnityEngine.Random.Range(0x2710, 0x1869f).ToString();
                                    strArray5[100] = str6;
                                    currentLevel = currentLevel + str6;
                                    levelCache.Add(strArray5);
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
                                                titanSpawns.Add(new Vector3(Convert.ToSingle(strArray6[2]), Convert.ToSingle(strArray6[3]), Convert.ToSingle(strArray6[4])));
                                            }
                                            else if (strArray6[1] == "playerC")
                                            {
                                                playerSpawnsC.Add(new Vector3(Convert.ToSingle(strArray6[2]), Convert.ToSingle(strArray6[3]), Convert.ToSingle(strArray6[4])));
                                            }
                                            else if (strArray6[1] == "playerM")
                                            {
                                                playerSpawnsM.Add(new Vector3(Convert.ToSingle(strArray6[2]), Convert.ToSingle(strArray6[3]), Convert.ToSingle(strArray6[4])));
                                            }
                                        }
                                        strArray5[num7] = strArray4[num2];
                                        num7++;
                                    }
                                    str6 = UnityEngine.Random.Range(0x2710, 0x1869f).ToString();
                                    strArray5[strArray4.Length % 100] = str6;
                                    currentLevel = currentLevel + str6;
                                    levelCache.Add(strArray5);
                                }
                            }
                            var list = new List<string>();
                            foreach (var vector in titanSpawns)
                            {
                                list.Add("titan," + vector.x.ToString() + "," + vector.y.ToString() + "," + vector.z.ToString());
                            }
                            foreach (var vector in playerSpawnsC)
                            {
                                list.Add("playerC," + vector.x.ToString() + "," + vector.y.ToString() + "," + vector.z.ToString());
                            }
                            foreach (var vector in playerSpawnsM)
                            {
                                list.Add("playerM," + vector.x.ToString() + "," + vector.y.ToString() + "," + vector.z.ToString());
                            }
                            var item = "a" + UnityEngine.Random.Range(0x2710, 0x1869f).ToString();
                            list.Add(item);
                            currentLevel = item + currentLevel;
                            levelCache.Insert(0, list.ToArray());
                            var str8 = "z" + UnityEngine.Random.Range(0x2710, 0x1869f).ToString();
                            levelCache.Add(new string[] { str8 });
                            currentLevel = currentLevel + str8;
                            hashtable = new ExitGames.Client.Photon.Hashtable
                            {
                                { PhotonPlayerProperty.currentLevel, currentLevel }
                            };
                            PhotonNetwork.player.SetCustomProperties(hashtable);
                            oldScript = currentScript;
                        }
                    }
                    for (num = 0; num < PhotonNetwork.playerList.Length; num++)
                    {
                        var player = PhotonNetwork.playerList[num];
                        if (!player.isMasterClient)
                        {
                            playersRPC.Add(player);
                        }
                    }
                    StartCoroutine(customlevelE(playersRPC));
                    StartCoroutine(customlevelcache());
                }
            }
        }
    }

    private IEnumerator loadskinE(string n, string url, string url2, string[] skybox)
    {
        var mipmap = true;
        var iteratorVariable1 = false;
        if (((int) settings[0x3f]) == 1)
        {
            mipmap = false;
        }
        if ((skybox.Length > 5) && (((((skybox[0] != string.Empty) || (skybox[1] != string.Empty)) || ((skybox[2] != string.Empty) || (skybox[3] != string.Empty))) || (skybox[4] != string.Empty)) || (skybox[5] != string.Empty)))
        {
            var key = string.Join(",", skybox);
            if (!linkHash[1].ContainsKey(key))
            {
                iteratorVariable1 = true;
                var material = Camera.main.GetComponent<Skybox>().material;
                var iteratorVariable4 = skybox[0];
                var iteratorVariable5 = skybox[1];
                var iteratorVariable6 = skybox[2];
                var iteratorVariable7 = skybox[3];
                var iteratorVariable8 = skybox[4];
                var iteratorVariable9 = skybox[5];
                if ((iteratorVariable4.EndsWith(".jpg") || iteratorVariable4.EndsWith(".png")) || iteratorVariable4.EndsWith(".jpeg"))
                {
                    var link = new WWW(iteratorVariable4);
                    yield return link;
                    var texture = RCextensions.loadimage(link, mipmap, 0x7a120);
                    link.Dispose();
                    texture.wrapMode = TextureWrapMode.Clamp;
                    material.SetTexture("_FrontTex", texture);
                }
                if ((iteratorVariable5.EndsWith(".jpg") || iteratorVariable5.EndsWith(".png")) || iteratorVariable5.EndsWith(".jpeg"))
                {
                    var iteratorVariable12 = new WWW(iteratorVariable5);
                    yield return iteratorVariable12;
                    var iteratorVariable13 = RCextensions.loadimage(iteratorVariable12, mipmap, 0x7a120);
                    iteratorVariable12.Dispose();
                    iteratorVariable13.wrapMode = TextureWrapMode.Clamp;
                    material.SetTexture("_BackTex", iteratorVariable13);
                }
                if ((iteratorVariable6.EndsWith(".jpg") || iteratorVariable6.EndsWith(".png")) || iteratorVariable6.EndsWith(".jpeg"))
                {
                    var iteratorVariable14 = new WWW(iteratorVariable6);
                    yield return iteratorVariable14;
                    var iteratorVariable15 = RCextensions.loadimage(iteratorVariable14, mipmap, 0x7a120);
                    iteratorVariable14.Dispose();
                    iteratorVariable15.wrapMode = TextureWrapMode.Clamp;
                    material.SetTexture("_LeftTex", iteratorVariable15);
                }
                if ((iteratorVariable7.EndsWith(".jpg") || iteratorVariable7.EndsWith(".png")) || iteratorVariable7.EndsWith(".jpeg"))
                {
                    var iteratorVariable16 = new WWW(iteratorVariable7);
                    yield return iteratorVariable16;
                    var iteratorVariable17 = RCextensions.loadimage(iteratorVariable16, mipmap, 0x7a120);
                    iteratorVariable16.Dispose();
                    iteratorVariable17.wrapMode = TextureWrapMode.Clamp;
                    material.SetTexture("_RightTex", iteratorVariable17);
                }
                if ((iteratorVariable8.EndsWith(".jpg") || iteratorVariable8.EndsWith(".png")) || iteratorVariable8.EndsWith(".jpeg"))
                {
                    var iteratorVariable18 = new WWW(iteratorVariable8);
                    yield return iteratorVariable18;
                    var iteratorVariable19 = RCextensions.loadimage(iteratorVariable18, mipmap, 0x7a120);
                    iteratorVariable18.Dispose();
                    iteratorVariable19.wrapMode = TextureWrapMode.Clamp;
                    material.SetTexture("_UpTex", iteratorVariable19);
                }
                if ((iteratorVariable9.EndsWith(".jpg") || iteratorVariable9.EndsWith(".png")) || iteratorVariable9.EndsWith(".jpeg"))
                {
                    var iteratorVariable20 = new WWW(iteratorVariable9);
                    yield return iteratorVariable20;
                    var iteratorVariable21 = RCextensions.loadimage(iteratorVariable20, mipmap, 0x7a120);
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
            var iteratorVariable22 = url.Split(new char[] { ',' });
            var iteratorVariable23 = url2.Split(new char[] { ',' });
            var startIndex = 0;
            object[] iteratorVariable25 = FindObjectsOfType(typeof(GameObject));
            foreach (GameObject iteratorVariable26 in iteratorVariable25)
            {
                if (iteratorVariable26 != null)
                {
                    if (iteratorVariable26.name.Contains("TREE") && (n.Length > (startIndex + 1)))
                    {
                        int iteratorVariable28;
                        int iteratorVariable27;
                        var s = n.Substring(startIndex, 1);
                        var iteratorVariable30 = n.Substring(startIndex + 1, 1);
                        if ((((int.TryParse(s, out iteratorVariable27) && int.TryParse(iteratorVariable30, out iteratorVariable28)) && ((iteratorVariable27 >= 0) && (iteratorVariable27 < 8))) && (((iteratorVariable28 >= 0) && (iteratorVariable28 < 8)) && ((iteratorVariable22.Length >= 8) && (iteratorVariable23.Length >= 8)))) && ((iteratorVariable22[iteratorVariable27] != null) && (iteratorVariable23[iteratorVariable28] != null)))
                        {
                            var iteratorVariable31 = iteratorVariable22[iteratorVariable27];
                            var iteratorVariable32 = iteratorVariable23[iteratorVariable28];
                            foreach (var iteratorVariable33 in iteratorVariable26.GetComponentsInChildren<Renderer>())
                            {
                                if (iteratorVariable33.name.Contains(FengGameManagerMKII.s[0x16]))
                                {
                                    if ((iteratorVariable31.EndsWith(".jpg") || iteratorVariable31.EndsWith(".png")) || iteratorVariable31.EndsWith(".jpeg"))
                                    {
                                        if (!linkHash[2].ContainsKey(iteratorVariable31))
                                        {
                                            var iteratorVariable34 = new WWW(iteratorVariable31);
                                            yield return iteratorVariable34;
                                            var iteratorVariable35 = RCextensions.loadimage(iteratorVariable34, mipmap, 0xf4240);
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
                                            var iteratorVariable36 = new WWW(iteratorVariable32);
                                            yield return iteratorVariable36;
                                            var iteratorVariable37 = RCextensions.loadimage(iteratorVariable36, mipmap, 0x30d40);
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
                        var iteratorVariable38 = iteratorVariable23[8];
                        if ((iteratorVariable38.EndsWith(".jpg") || iteratorVariable38.EndsWith(".png")) || iteratorVariable38.EndsWith(".jpeg"))
                        {
                            foreach (var iteratorVariable39 in iteratorVariable26.GetComponentsInChildren<Renderer>())
                            {
                                if (!linkHash[0].ContainsKey(iteratorVariable38))
                                {
                                    var iteratorVariable40 = new WWW(iteratorVariable38);
                                    yield return iteratorVariable40;
                                    var iteratorVariable41 = RCextensions.loadimage(iteratorVariable40, mipmap, 0x30d40);
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
                            foreach (var renderer in iteratorVariable26.GetComponentsInChildren<Renderer>())
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
            var iteratorVariable42 = url.Split(new char[] { ',' });
            var iteratorVariable43 = url2.Split(new char[] { ',' });
            var iteratorVariable44 = iteratorVariable43[2];
            var iteratorVariable45 = 0;
            object[] iteratorVariable46 = FindObjectsOfType(typeof(GameObject));
            foreach (GameObject iteratorVariable47 in iteratorVariable46)
            {
                if ((iteratorVariable47 != null) && (iteratorVariable47.name.Contains("Cube_") && (iteratorVariable47.transform.parent.gameObject.tag != "Player")))
                {
                    if (iteratorVariable47.name.EndsWith("001"))
                    {
                        if ((iteratorVariable43.Length > 0) && (iteratorVariable43[0] != null))
                        {
                            var iteratorVariable48 = iteratorVariable43[0];
                            if ((iteratorVariable48.EndsWith(".jpg") || iteratorVariable48.EndsWith(".png")) || iteratorVariable48.EndsWith(".jpeg"))
                            {
                                foreach (var iteratorVariable49 in iteratorVariable47.GetComponentsInChildren<Renderer>())
                                {
                                    if (!linkHash[0].ContainsKey(iteratorVariable48))
                                    {
                                        var iteratorVariable50 = new WWW(iteratorVariable48);
                                        yield return iteratorVariable50;
                                        var iteratorVariable51 = RCextensions.loadimage(iteratorVariable50, mipmap, 0x30d40);
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
                                foreach (var renderer in iteratorVariable47.GetComponentsInChildren<Renderer>())
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
                            var iteratorVariable52 = iteratorVariable43[1];
                            if ((iteratorVariable52.EndsWith(".jpg") || iteratorVariable52.EndsWith(".png")) || iteratorVariable52.EndsWith(".jpeg"))
                            {
                                foreach (var iteratorVariable53 in iteratorVariable47.GetComponentsInChildren<Renderer>())
                                {
                                    if (!linkHash[0].ContainsKey(iteratorVariable52))
                                    {
                                        var iteratorVariable54 = new WWW(iteratorVariable52);
                                        yield return iteratorVariable54;
                                        var iteratorVariable55 = RCextensions.loadimage(iteratorVariable54, mipmap, 0x30d40);
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
                        var iteratorVariable57 = n.Substring(iteratorVariable45, 1);
                        if (((int.TryParse(iteratorVariable57, out iteratorVariable56) && (iteratorVariable56 >= 0)) && ((iteratorVariable56 < 8) && (iteratorVariable42.Length >= 8))) && (iteratorVariable42[iteratorVariable56] != null))
                        {
                            var iteratorVariable58 = iteratorVariable42[iteratorVariable56];
                            if ((iteratorVariable58.EndsWith(".jpg") || iteratorVariable58.EndsWith(".png")) || iteratorVariable58.EndsWith(".jpeg"))
                            {
                                foreach (var iteratorVariable59 in iteratorVariable47.GetComponentsInChildren<Renderer>())
                                {
                                    if (!linkHash[2].ContainsKey(iteratorVariable58))
                                    {
                                        var iteratorVariable60 = new WWW(iteratorVariable58);
                                        yield return iteratorVariable60;
                                        var iteratorVariable61 = RCextensions.loadimage(iteratorVariable60, mipmap, 0xf4240);
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
                        var iteratorVariable62 = iteratorVariable43[2];
                        if ((iteratorVariable62.EndsWith(".jpg") || iteratorVariable62.EndsWith(".png")) || iteratorVariable62.EndsWith(".jpeg"))
                        {
                            foreach (var iteratorVariable63 in iteratorVariable47.GetComponentsInChildren<Renderer>())
                            {
                                if (!linkHash[2].ContainsKey(iteratorVariable62))
                                {
                                    var iteratorVariable64 = new WWW(iteratorVariable62);
                                    yield return iteratorVariable64;
                                    var iteratorVariable65 = RCextensions.loadimage(iteratorVariable64, mipmap, 0xf4240);
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
            unloadAssets();
        }
    }

    [PunRPC]
    public void loadskinRPC(string n, string url, string url2, string[] skybox, PhotonMessageInfo info)
    {
        if ((((int) settings[2]) == 1) && info.sender.isMasterClient)
        {
            StartCoroutine(loadskinE(n, url, url2, skybox));
        }
    }

    private string mastertexturetype(int lol)
    {
        if (lol == 0)
        {
            return "High";
        }
        if (lol == 1)
        {
            return "Med";
        }
        return "Low";
    }

    public void MultiplayerRacingFinish()
    {
        var time = roundTime - 20f;
        if (PhotonNetwork.isMasterClient)
            getRacingResult(LoginFengKAI.player.name, time);
        else
            photonView.RPC<string, float>(getRacingResult, PhotonTargets.MasterClient, LoginFengKAI.player.name, time);

        gameWin2();
    }

    [PunRPC]
    public void netGameLose(int score, PhotonMessageInfo info)
    {
        isLosing = true;
        Gamemode.OnNetGameLost(score);
        gameEndCD = gameEndTotalCDtime;
        if (((int) settings[0xf4]) == 1)
        {
            chatRoom.addLINE("<color=#FFC000>(" + roundTime.ToString("F2") + ")</color> Round ended (game lose).");
        }
        if (!((info.sender == PhotonNetwork.masterClient) || info.sender.isLocal) && PhotonNetwork.isMasterClient)
        {
            chatRoom.addLINE("<color=#FFC000>Round end sent from Player " + info.sender.ID.ToString() + "</color>");
        }
    }

    [PunRPC]
    public void netGameWin(int score, PhotonMessageInfo info)
    {
        isWinning = true;
        Gamemode.OnNetGameWon(score);
        gameEndCD = gameEndTotalCDtime;
        if (((int) settings[0xf4]) == 1)
        {
            chatRoom.addLINE("<color=#FFC000>(" + roundTime.ToString("F2") + ")</color> Round ended (game win).");
        }
        if (!((info.sender == PhotonNetwork.masterClient) || info.sender.isLocal))
        {
            chatRoom.addLINE("<color=#FFC000>Round end sent from Player " + info.sender.ID.ToString() + "</color>");
        }
    }

    [PunRPC]
    public void netRefreshRacingResult(string tmp)
    {
        localRacingResult = tmp;
    }

    [PunRPC]
    public void netShowDamage(int damage)
    {
        InGameUI.HUD.SetDamage(damage);
    }

    public void NOTSpawnPlayer(string id = "2")
    {
        myLastHero = id.ToUpper();
        var hashtable = new ExitGames.Client.Photon.Hashtable
        {
            { "dead", true }
        };
        var propertiesToSet = hashtable;
        PhotonNetwork.player.SetCustomProperties(propertiesToSet);
        hashtable = new ExitGames.Client.Photon.Hashtable
        {
            { PhotonPlayerProperty.isTitan, 1 }
        };
        propertiesToSet = hashtable;
        PhotonNetwork.player.SetCustomProperties(propertiesToSet);
        if (IN_GAME_MAIN_CAMERA.cameraMode == CAMERA_TYPE.TPS)
        {
            Screen.lockCursor = true;
        }
        else
        {
            Screen.lockCursor = false;
        }
        Cursor.visible = false;
        ShowHUDInfoCenter("the game has started for 60 seconds.\n please wait for next round.\n Click Right Mouse Key to Enter or Exit the Spectator Mode.");
        GameObject.Find("MainCamera").GetComponent<IN_GAME_MAIN_CAMERA>().enabled = true;
        GameObject.Find("MainCamera").GetComponent<IN_GAME_MAIN_CAMERA>().setMainObject(null, true, false);
        GameObject.Find("MainCamera").GetComponent<IN_GAME_MAIN_CAMERA>().setSpectorMode(true);
        GameObject.Find("MainCamera").GetComponent<IN_GAME_MAIN_CAMERA>().gameOver = true;
    }

    public void NOTSpawnPlayerRC(string id)
    {
        myLastHero = id.ToUpper();
        var hashtable = new ExitGames.Client.Photon.Hashtable
        {
            { "dead", true }
        };
        var propertiesToSet = hashtable;
        PhotonNetwork.player.SetCustomProperties(propertiesToSet);
        hashtable = new ExitGames.Client.Photon.Hashtable
        {
            { PhotonPlayerProperty.isTitan, 1 }
        };
        propertiesToSet = hashtable;
        PhotonNetwork.player.SetCustomProperties(propertiesToSet);
        if (IN_GAME_MAIN_CAMERA.cameraMode == CAMERA_TYPE.TPS)
        {
            Screen.lockCursor = true;
        }
        else
        {
            Screen.lockCursor = false;
        }
        Cursor.visible = false;
        GameObject.Find("MainCamera").GetComponent<IN_GAME_MAIN_CAMERA>().enabled = true;
        GameObject.Find("MainCamera").GetComponent<IN_GAME_MAIN_CAMERA>().setMainObject(null, true, false);
        GameObject.Find("MainCamera").GetComponent<IN_GAME_MAIN_CAMERA>().setSpectorMode(true);
        GameObject.Find("MainCamera").GetComponent<IN_GAME_MAIN_CAMERA>().gameOver = true;
    }

    public void OnConnectedToMaster()
    {
        print("OnConnectedToMaster");
    }

    public void OnConnectedToPhoton()
    {
        print("OnConnectedToPhoton");
    }

    public void OnConnectionFail(DisconnectCause cause)
    {
        print("OnConnectionFail : " + cause.ToString());
        Screen.lockCursor = false;
        Cursor.visible = true;
        IN_GAME_MAIN_CAMERA.gametype = GAMETYPE.STOP;
        gameStart = false;
    }

    public void OnCreatedRoom()
    {
        racingResult = new ArrayList();
        print("OnCreatedRoom");
    }

    public void OnCustomAuthenticationFailed()
    {
        print("OnCustomAuthenticationFailed");
    }

    public void OnDisconnectedFromPhoton()
    {
        print("OnDisconnectedFromPhoton");
        Screen.lockCursor = false;
        Cursor.visible = true;
    }

    [PunRPC]
    public void oneTitanDown(string titanName)
    {
        if ((IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE) || PhotonNetwork.isMasterClient)
        {
            EventManager.OnTitanKilled.Invoke(titanName);
        }
    }

    public void OnFailedToConnectToPhoton()
    {
        print("OnFailedToConnectToPhoton");
    }

    public void OnGUI()
    {
        if (GUILayout.Button("Photon Spawn Test!"))
        {
            PhotonNetwork.Instantiate("DummyTitanPrefab", GameObject.FindGameObjectWithTag("Player").transform.position, Quaternion.identity, 0);
        }
    }

    public void OnJoinedLobby()
    {
    }

    private void SetGamemode(GamemodeSettings settings)
    {
        if (Gamemode == null)
        {
            var gamemodeObject = new GameObject("Gamemode");
            gamemodeObject.AddComponent(settings.GetGamemodeFromSettings());
            gamemodeObject.transform.parent = gameObject.transform;
            Gamemode = gamemodeObject.GetComponent<GamemodeBase>();
            Gamemode.Settings = settings;
        }
        else
        {
            var gamemodeComponent = GetComponentInChildren<GamemodeBase>();
            Destroy(gamemodeComponent.gameObject);
            Gamemode = null;
            SetGamemode(settings);
        }
    }

    public void OnJoinedRoom()
    {
        Level = PhotonNetwork.room.GetLevel();
        SetGamemode(PhotonNetwork.room.GetGamemodeSetting(Level));
        maxPlayers = PhotonNetwork.room.MaxPlayers;
        playerList = string.Empty;
        var separator = new char[] { "`"[0] };
        //UnityEngine.MonoBehaviour.print("OnJoinedRoom " + PhotonNetwork.room.name + "    >>>>   " + LevelInfo.getInfo(PhotonNetwork.room.name.Split(separator)[1]).mapName);
        gameTimesUp = false;
        var chArray3 = new char[] { "`"[0] };
        var strArray = PhotonNetwork.room.name.Split(chArray3);
        difficulty = 0;
        //if (strArray[2] == "normal")
        //{
        //    this.difficulty = 0;
        //}
        //else if (strArray[2] == "hard")
        //{
        //    this.difficulty = 1;
        //}
        //else if (strArray[2] == "abnormal")
        //{
        //    this.difficulty = 2;
        //}
        IN_GAME_MAIN_CAMERA.difficulty = difficulty;
        time = 5000;//int.Parse(strArray[3]);
        time *= 60;
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
        PhotonNetwork.LoadLevel(Level.SceneName);
        var hashtable = new ExitGames.Client.Photon.Hashtable
        {
            { PhotonPlayerProperty.name, LoginFengKAI.player.name },
            { PhotonPlayerProperty.guildName, LoginFengKAI.player.guildname },
            { PhotonPlayerProperty.kills, 0 },
            { PhotonPlayerProperty.max_dmg, 0 },
            { PhotonPlayerProperty.total_dmg, 0 },
            { PhotonPlayerProperty.deaths, 0 },
            { PhotonPlayerProperty.dead, true },
            { PhotonPlayerProperty.isTitan, 0 },
            { PhotonPlayerProperty.RCteam, 0 },
            { PhotonPlayerProperty.currentLevel, string.Empty }
        };
        var propertiesToSet = hashtable;
        PhotonNetwork.player.SetCustomProperties(propertiesToSet);
        needChooseSide = true;
        chatContent = new ArrayList();
        killInfoGO = new ArrayList();
        //InRoomChat.messages = new List<string>();
        if (!PhotonNetwork.isMasterClient)
        {
            photonView.RPC(RequireStatus, PhotonTargets.MasterClient);
            photonView.RPC<PhotonMessageInfo>(RequestSettings, PhotonTargets.MasterClient);
        }
        assetCacheTextures = new Dictionary<string, Texture2D>();
        isFirstLoad = true;
        name = LoginFengKAI.player.name;
        if (loginstate != 3)
        {
            name = nameField;
            if ((!name.StartsWith("[") || (name.Length < 8)) || (name.Substring(7, 1) != "]"))
            {
                name = $"<color=#9999ff>{name}</color>";
            }
            name = name.Replace("[-]", "");
            LoginFengKAI.player.name = name;
        }
        var hashtable3 = new ExitGames.Client.Photon.Hashtable
        {
            { PhotonPlayerProperty.name, name }
        };
        PhotonNetwork.player.SetCustomProperties(hashtable3);
        if (OnPrivateServer)
        {
            ServerRequestAuthentication(PrivateServerAuthPass);
        }
    }

    public void OnLeftLobby()
    {
        print("OnLeftLobby");
    }

    public void OnLeftRoom()
    {
        if (Application.loadedLevel != 0)
        {
            Time.timeScale = 1f;
            if (PhotonNetwork.connected)
            {
                PhotonNetwork.Disconnect();
            }
            resetSettings(true);
            loadconfig();
            IN_GAME_MAIN_CAMERA.gametype = GAMETYPE.STOP;
            gameStart = false;
            Screen.lockCursor = false;
            Cursor.visible = true;
            inputManager.menuOn = false;
            DestroyAllExistingCloths();
            Destroy(GameObject.Find("MultiplayerManager"));
            Application.LoadLevel(0);
        }
    }

    private void OnLevelWasLoaded(int level)
    {
        if ((level != 0) && ((Application.loadedLevelName != "characterCreation") && (Application.loadedLevelName != "SnapShot")))
        {
            ChangeQuality.setCurrentQuality();
            foreach (var obj2 in GameObject.FindGameObjectsWithTag("titan"))
            {
                if (!((obj2.GetPhotonView() != null) && obj2.GetPhotonView().owner.isMasterClient))
                {
                    Destroy(obj2);
                }
            }
            isWinning = false;
            gameStart = true;
            ShowHUDInfoCenter(string.Empty);
            var obj3 = (GameObject) Instantiate(Resources.Load("MainCamera_mono"), GameObject.Find("cameraDefaultPosition").transform.position, GameObject.Find("cameraDefaultPosition").transform.rotation);
            Destroy(GameObject.Find("cameraDefaultPosition"));
            obj3.name = "MainCamera";
            Screen.lockCursor = true;
            Cursor.visible = true;
            cache();
            loadskin();
            Camera.main.GetComponent<IN_GAME_MAIN_CAMERA>().setHUDposition();
            Camera.main.GetComponent<IN_GAME_MAIN_CAMERA>().setDayLight(IN_GAME_MAIN_CAMERA.dayLight);
            //TODO: How should a gamemode and level be loaded in singlePlayer?
            if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
            {
                single_kills = 0;
                single_maxDamage = 0;
                single_totalDamage = 0;
                Camera.main.GetComponent<IN_GAME_MAIN_CAMERA>().enabled = true;
                Camera.main.GetComponent<SpectatorMovement>().disable = true;
                //TODO MouseLook
                //Camera.main.GetComponent<MouseLook>().disable = true;
                //this.SpawnPlayer(IN_GAME_MAIN_CAMERA.singleCharacter.ToUpper(), "playerRespawn");
                SpawnPlayer(null);
                if (IN_GAME_MAIN_CAMERA.cameraMode == CAMERA_TYPE.TPS)
                {
                    Screen.lockCursor = true;
                }
                else
                {
                    Screen.lockCursor = false;
                }
                Cursor.visible = false;
                var abnormal = 90;
                if (difficulty == 1)
                {
                    abnormal = 70;
                }
                throw new NotImplementedException("Titan spawners for singleplayer don't exist yet");
                //this.spawnTitanCustom("titanRespawn", abnormal, Gamemode.Titans, false);
            }
            else
            {
                PVPcheckPoint.chkPts = new ArrayList();
                Camera.main.GetComponent<IN_GAME_MAIN_CAMERA>().enabled = false;
                Camera.main.GetComponent<CameraShake>().enabled = false;
                IN_GAME_MAIN_CAMERA.gametype = GAMETYPE.MULTIPLAYER;
                if (needChooseSide)
                {
                    ShowHUDInfoTopCenterADD("\n\nPRESS 1 TO ENTER GAME");
                }
                else if (((int) settings[0xf5]) == 0)
                {
                    if (IN_GAME_MAIN_CAMERA.cameraMode == CAMERA_TYPE.TPS)
                    {
                        Screen.lockCursor = true;
                    }
                    else
                    {
                        Screen.lockCursor = false;
                    }
                    if (RCextensions.returnIntFromObject(PhotonNetwork.player.CustomProperties[PhotonPlayerProperty.isTitan]) == 2)
                    {
                        SpawnPlayerTitan();
                    }
                    else
                    {
                        SpawnPlayer(myLastHero, myLastRespawnTag);
                    }
                }

                if (!PhotonNetwork.isMasterClient)
                    photonView.RPC(RequireStatus, PhotonTargets.MasterClient);

                Gamemode.OnLevelLoaded(Level, PhotonNetwork.isMasterClient);
                if (((int) settings[0xf5]) == 1)
                {
                    EnterSpecMode(true);
                }
            }
        }
    }

    public void OnMasterClientSwitched(PhotonPlayer newMasterClient)
    {
        if (!noRestart)
        {
            if (PhotonNetwork.isMasterClient)
            {
                restartingMC = true;
            }
            resetSettings(false);
            if (!Gamemode.Settings.IsPlayerTitanEnabled)
            {
                var propertiesToSet = new ExitGames.Client.Photon.Hashtable
                {
                    { PhotonPlayerProperty.isTitan, 1 }
                };
                PhotonNetwork.player.SetCustomProperties(propertiesToSet);
            }
            if (!(gameTimesUp || !PhotonNetwork.isMasterClient))
            {
                restartGame2(true);
                photonView.RPC<PhotonMessageInfo>(setMasterRC, PhotonTargets.All);
            }
        }
        noRestart = false;
    }

    public void OnPhotonCreateRoomFailed()
    {
        print("OnPhotonCreateRoomFailed");
    }

    public void OnPhotonCustomRoomPropertiesChanged()
    {
        if (PhotonNetwork.isMasterClient)
        {
            if (!PhotonNetwork.room.open)
            {
                PhotonNetwork.room.open = true;
            }
            if (!PhotonNetwork.room.visible)
            {
                PhotonNetwork.room.visible = true;
            }
            if (PhotonNetwork.room.maxPlayers != maxPlayers)
            {
                PhotonNetwork.room.maxPlayers = maxPlayers;
            }
        }
        else
        {
            maxPlayers = PhotonNetwork.room.maxPlayers;
        }
    }

    public void OnPhotonInstantiate()
    {
        print("OnPhotonInstantiate");
    }

    public void OnPhotonJoinRoomFailed()
    {
        print("OnPhotonJoinRoomFailed");
    }

    public void OnPhotonMaxCccuReached()
    {
        print("OnPhotonMaxCccuReached");
    }

    public void OnPhotonPlayerConnected(PhotonPlayer player)
    {
        if (PhotonNetwork.isMasterClient)
        {
            var photonView = base.photonView;
            if (banHash.ContainsValue(RCextensions.returnStringFromObject(player.CustomProperties[PhotonPlayerProperty.name])))
            {
                kickPlayerRC(player, false, "banned.");
            }
            else
            {
                var num = RCextensions.returnIntFromObject(player.CustomProperties[PhotonPlayerProperty.statACL]);
                var num2 = RCextensions.returnIntFromObject(player.CustomProperties[PhotonPlayerProperty.statBLA]);
                var num3 = RCextensions.returnIntFromObject(player.CustomProperties[PhotonPlayerProperty.statGAS]);
                var num4 = RCextensions.returnIntFromObject(player.CustomProperties[PhotonPlayerProperty.statSPD]);
                if ((((num > 150) || (num2 > 125)) || (num3 > 150)) || (num4 > 140))
                {
                    kickPlayerRC(player, true, "excessive stats.");
                    return;
                }
                if (Gamemode.Settings.SaveKDROnDisconnect)
                {
                    StartCoroutine(WaitAndReloadKDR(player));
                }
                if (Level.Name.StartsWith("Custom"))
                {
                    StartCoroutine(customlevelE(new List<PhotonPlayer> { player }));
                }
                var hashtable = new ExitGames.Client.Photon.Hashtable();
                if ((ignoreList != null) && (ignoreList.Count > 0))
                {
                    photonView.RPC("ignorePlayerArray", player, new object[] { ignoreList.ToArray() });
                }
                //photonView.RPC("settingRPC", player, new object[] { hashtable });
                photonView.RPC("setMasterRC", player, new object[0]);
                if ((Time.timeScale <= 0.1f) && (pauseWaitTime > 3f))
                {
                    photonView.RPC("pauseRPC", player, new object[] { true });
                    var parameters = new object[] { "<color=#FFCC00>MasterClient has paused the game.</color>", "" };
                    photonView.RPC("Chat", player, parameters);
                }
            }
        }
        RecompilePlayerList(0.1f);
    }

    public void OnPhotonPlayerDisconnected(PhotonPlayer player)
    {
        if (!gameTimesUp)
        {
            oneTitanDown(string.Empty);
            someOneIsDead(0);
        }

        if (ignoreList.Contains(player.ID))
        {
            ignoreList.Remove(player.ID);
        }

        InstantiateTracker.instance.TryRemovePlayer(player.ID);
        if (PhotonNetwork.isMasterClient)
            photonView.RPC<int, PhotonMessageInfo>(verifyPlayerHasLeft, PhotonTargets.All, player.ID);

        if (Gamemode.Settings.SaveKDROnDisconnect)
        {
            var key = RCextensions.returnStringFromObject(player.CustomProperties[PhotonPlayerProperty.name]);
            if (PreservedPlayerKDR.ContainsKey(key))
            {
                PreservedPlayerKDR.Remove(key);
            }
            var numArray2 = new int[] { RCextensions.returnIntFromObject(player.CustomProperties[PhotonPlayerProperty.kills]), RCextensions.returnIntFromObject(player.CustomProperties[PhotonPlayerProperty.deaths]), RCextensions.returnIntFromObject(player.CustomProperties[PhotonPlayerProperty.max_dmg]), RCextensions.returnIntFromObject(player.CustomProperties[PhotonPlayerProperty.total_dmg]) };
            PreservedPlayerKDR.Add(key, numArray2);
        }
        RecompilePlayerList(0.1f);
    }

    public void OnPhotonPlayerPropertiesChanged(object[] playerAndUpdatedProps)
    {
        RecompilePlayerList(0.1f);
        if (((playerAndUpdatedProps != null) && (playerAndUpdatedProps.Length >= 2)) && (((PhotonPlayer) playerAndUpdatedProps[0]) == PhotonNetwork.player))
        {
            ExitGames.Client.Photon.Hashtable hashtable2;
            var hashtable = (ExitGames.Client.Photon.Hashtable) playerAndUpdatedProps[1];
            if (hashtable.ContainsKey("name") && (RCextensions.returnStringFromObject(PhotonNetwork.player.CustomProperties[PhotonPlayerProperty.name]) != name))
            {
                hashtable2 = new ExitGames.Client.Photon.Hashtable
                {
                    { PhotonPlayerProperty.name, name }
                };
                PhotonNetwork.player.SetCustomProperties(hashtable2);
            }
            if (((hashtable.ContainsKey("statACL") || hashtable.ContainsKey("statBLA")) || hashtable.ContainsKey("statGAS")) || hashtable.ContainsKey("statSPD"))
            {
                var player = PhotonNetwork.player;
                var num = RCextensions.returnIntFromObject(player.CustomProperties[PhotonPlayerProperty.statACL]);
                var num2 = RCextensions.returnIntFromObject(player.CustomProperties[PhotonPlayerProperty.statBLA]);
                var num3 = RCextensions.returnIntFromObject(player.CustomProperties[PhotonPlayerProperty.statGAS]);
                var num4 = RCextensions.returnIntFromObject(player.CustomProperties[PhotonPlayerProperty.statSPD]);
                if (num > 150)
                {
                    hashtable2 = new ExitGames.Client.Photon.Hashtable
                    {
                        { PhotonPlayerProperty.statACL, 100 }
                    };
                    PhotonNetwork.player.SetCustomProperties(hashtable2);
                    num = 100;
                }
                if (num2 > 0x7d)
                {
                    hashtable2 = new ExitGames.Client.Photon.Hashtable
                    {
                        { PhotonPlayerProperty.statBLA, 100 }
                    };
                    PhotonNetwork.player.SetCustomProperties(hashtable2);
                    num2 = 100;
                }
                if (num3 > 150)
                {
                    hashtable2 = new ExitGames.Client.Photon.Hashtable
                    {
                        { PhotonPlayerProperty.statGAS, 100 }
                    };
                    PhotonNetwork.player.SetCustomProperties(hashtable2);
                    num3 = 100;
                }
                if (num4 > 140)
                {
                    hashtable2 = new ExitGames.Client.Photon.Hashtable
                    {
                        { PhotonPlayerProperty.statSPD, 100 }
                    };
                    PhotonNetwork.player.SetCustomProperties(hashtable2);
                    num4 = 100;
                }
            }
        }
    }

    public void OnPhotonRandomJoinFailed()
    {
        print("OnPhotonRandomJoinFailed");
    }

    public void OnPhotonSerializeView()
    {
        print("OnPhotonSerializeView");
    }

    public void OnReceivedRoomListUpdate()
    {
    }

    public void OnUpdatedFriendList()
    {
        print("OnUpdatedFriendList");
    }

    [PunRPC]
    public void pauseRPC(bool pause, PhotonMessageInfo info)
    {
        if (info.sender.isMasterClient)
        {
            if (pause)
            {
                pauseWaitTime = 100000f;
                Time.timeScale = 1E-06f;
            }
            else
            {
                pauseWaitTime = 3f;
            }
        }
    }

    public void playerKillInfoSingleUpdate(int dmg)
    {
        single_kills++;
        single_maxDamage = Mathf.Max(dmg, single_maxDamage);
        single_totalDamage += dmg;
    }

    public void playerKillInfoUpdate(PhotonPlayer player, int dmg)
    {
        var propertiesToSet = new ExitGames.Client.Photon.Hashtable
        {
            { PhotonPlayerProperty.kills, ((int) player.CustomProperties[PhotonPlayerProperty.kills]) + 1 }
        };
        player.SetCustomProperties(propertiesToSet);
        propertiesToSet = new ExitGames.Client.Photon.Hashtable
        {
            { PhotonPlayerProperty.max_dmg, Mathf.Max(dmg, (int) player.CustomProperties[PhotonPlayerProperty.max_dmg]) }
        };
        player.SetCustomProperties(propertiesToSet);
        propertiesToSet = new ExitGames.Client.Photon.Hashtable
        {
            { PhotonPlayerProperty.total_dmg, ((int) player.CustomProperties[PhotonPlayerProperty.total_dmg]) + dmg }
        };
        player.SetCustomProperties(propertiesToSet);
    }

    public void RecompilePlayerList(float time)
    {
        if (!isRecompiling)
        {
            isRecompiling = true;
            StartCoroutine(WaitAndRecompilePlayerList(time));
        }
    }

    private void refreshRacingResult2()
    {
        localRacingResult = "Result\n";
        IComparer comparer = new IComparerRacingResult();
        racingResult.Sort(comparer);
        var num = Mathf.Min(racingResult.Count, 10);
        for (var i = 0; i < num; i++)
        {
            var localRacingResult = this.localRacingResult;
            var objArray2 = new object[] { localRacingResult, "Rank ", i + 1, " : " };
            this.localRacingResult = string.Concat(objArray2);
            this.localRacingResult = this.localRacingResult + (racingResult[i] as RacingResult).name;
            this.localRacingResult = this.localRacingResult + "   " + ((((int) ((racingResult[i] as RacingResult).time * 100f)) * 0.01f)).ToString() + "s";
            this.localRacingResult = this.localRacingResult + "\n";
        }

        photonView.RPC<string>(netRefreshRacingResult, PhotonTargets.All, localRacingResult);
    }

    [PunRPC]
    private void refreshStatus(float time1, float time2, bool startRacin, bool endRacin)
    {
        roundTime = time1;
        timeTotalServer = time2;
        startRacing = startRacin;
        endRacing = endRacin;
        if (startRacing && (GameObject.Find("door") != null))
        {
            GameObject.Find("door").SetActive(false);
        }
    }

    public IEnumerator reloadSky()
    {
        yield return new WaitForSeconds(0.5f);
        if ((skyMaterial != null) && (Camera.main.GetComponent<Skybox>().material != skyMaterial))
        {
            Camera.main.GetComponent<Skybox>().material = skyMaterial;
        }
        Screen.lockCursor = !Screen.lockCursor;
        Screen.lockCursor = !Screen.lockCursor;
    }

    public void removeCT(COLOSSAL_TITAN titan)
    {
        cT.Remove(titan);
    }

    public void removeET(TITAN_EREN hero)
    {
        eT.Remove(hero);
    }

    public void removeFT(FEMALE_TITAN titan)
    {
        fT.Remove(titan);
    }

    public void removeHero(Hero hero)
    {
        heroes.Remove(hero);
    }

    public void removeHook(Bullet h)
    {
        hooks.Remove(h);
    }

    public void removeTitan(MindlessTitan titan)
    {
        titans.Remove(titan);
    }

    [PunRPC]
    public void RequireStatus()
    {
        photonView.RPC<float, float, bool, bool>(refreshStatus, PhotonTargets.Others, roundTime, timeTotalServer, startRacing, endRacing);
    }

    private void resetGameSettings()
    {
    }

    private void resetSettings(bool isLeave)
    {
        name = LoginFengKAI.player.name;
        masterRC = false;
        var propertiesToSet = new ExitGames.Client.Photon.Hashtable
        {
            { PhotonPlayerProperty.RCteam, 0 }
        };
        if (isLeave)
        {
            currentLevel = string.Empty;
            propertiesToSet.Add(PhotonPlayerProperty.currentLevel, string.Empty);
            levelCache = new List<string[]>();
            titanSpawns.Clear();
            playerSpawnsC.Clear();
            playerSpawnsM.Clear();
            titanSpawners.Clear();
            intVariables.Clear();
            boolVariables.Clear();
            stringVariables.Clear();
            floatVariables.Clear();
            globalVariables.Clear();
            RCRegions.Clear();
            RCEvents.Clear();
            RCVariableNames.Clear();
            playerVariables.Clear();
            titanVariables.Clear();
            RCRegionTriggers.Clear();
            currentScriptLogic = string.Empty;
            propertiesToSet.Add(PhotonPlayerProperty.statACL, 100);
            propertiesToSet.Add(PhotonPlayerProperty.statBLA, 100);
            propertiesToSet.Add(PhotonPlayerProperty.statGAS, 100);
            propertiesToSet.Add(PhotonPlayerProperty.statSPD, 100);
            restartingMC = false;
        }
        PhotonNetwork.player.SetCustomProperties(propertiesToSet);
        resetGameSettings();
        banHash = new ExitGames.Client.Photon.Hashtable();
        imatitan = new ExitGames.Client.Photon.Hashtable();
        oldScript = string.Empty;
        ignoreList = new List<int>();
        restartCount = new List<float>();
        heroHash = new ExitGames.Client.Photon.Hashtable();
    }

    [PunRPC]
    public void RespawnRpc(PhotonMessageInfo info)
    {
        if (!info.sender.IsMasterClient) return;
        Respawn(PhotonNetwork.player);
    }

    private void Respawn(PhotonPlayer player)
    {
        if (player.CustomProperties[PhotonPlayerProperty.dead] == null
            || !RCextensions.returnBoolFromObject(player.CustomProperties[PhotonPlayerProperty.dead])) return;

        chatRoom.AddLine("<color=#FFCC00>You have been revived by the master client.</color>");
        var isPlayerTitan = RCextensions.returnIntFromObject(player.CustomProperties[PhotonPlayerProperty.isTitan]) == 2;
        if (isPlayerTitan)
        {
            SpawnPlayerTitan();
        }
        else
        {
            respawnHeroInNewRound();
        }
    }

    private IEnumerator respawnE(float seconds)
    {
        while (true)
        {
            yield return new WaitForSeconds(seconds);
            if (!isLosing && !isWinning)
            {
                for (var j = 0; j < PhotonNetwork.playerList.Length; j++)
                {
                    var targetPlayer = PhotonNetwork.playerList[j];
                    if (((targetPlayer.CustomProperties[PhotonPlayerProperty.RCteam] == null) && RCextensions.returnBoolFromObject(targetPlayer.CustomProperties[PhotonPlayerProperty.dead])) && (RCextensions.returnIntFromObject(targetPlayer.CustomProperties[PhotonPlayerProperty.isTitan]) != 2))
                    {
                        photonView.RPC("respawnHeroInNewRound", targetPlayer, new object[0]);
                    }
                }
            }
        }
    }

    [PunRPC]
    public void respawnHeroInNewRound()
    {
        if (!needChooseSide && GameObject.Find("MainCamera").GetComponent<IN_GAME_MAIN_CAMERA>().gameOver)
        {
            SpawnPlayer(myLastHero, myLastRespawnTag);
            GameObject.Find("MainCamera").GetComponent<IN_GAME_MAIN_CAMERA>().gameOver = false;
            ShowHUDInfoCenter(string.Empty);
        }
    }

    public IEnumerator restartE(float time)
    {
        yield return new WaitForSeconds(time);
        restartGame2(false);
    }

    public void restartGame2(bool masterclientSwitched = false)
    {
        if (!gameTimesUp)
        {
            startRacing = false;
            endRacing = false;
            checkpoint = null;
            timeElapse = 0f;
            roundTime = 0f;
            isWinning = false;
            isLosing = false;
            isPlayer1Winning = false;
            isPlayer2Winning = false;
            myRespawnTime = 0f;
            killInfoGO = new ArrayList();
            racingResult = new ArrayList();
            ShowHUDInfoCenter(string.Empty);
            isRestarting = true;
            DestroyAllExistingCloths();
            PhotonNetwork.DestroyAll();
            var hash = checkGameGUI();
            photonView.RPC<ExitGames.Client.Photon.Hashtable, PhotonMessageInfo>(settingRPC, PhotonTargets.Others, hash);
            photonView.RPC<PhotonMessageInfo>(RPCLoadLevel, PhotonTargets.All);
            setGameSettings(hash);
            if (masterclientSwitched)
            {
                sendChatContentInfo("<color=#A8FF24>MasterClient has switched to </color>" + ((string) PhotonNetwork.player.CustomProperties[PhotonPlayerProperty.name]).hexColor());
            }
        }
    }

    public void restartGameSingle2()
    {
        startRacing = false;
        endRacing = false;
        checkpoint = null;
        single_kills = 0;
        single_maxDamage = 0;
        single_totalDamage = 0;
        timeElapse = 0f;
        roundTime = 0f;
        timeTotalServer = 0f;
        isWinning = false;
        isLosing = false;
        isPlayer1Winning = false;
        isPlayer2Winning = false;
        myRespawnTime = 0f;
        ShowHUDInfoCenter(string.Empty);
        DestroyAllExistingCloths();
        Application.LoadLevel(Application.loadedLevel);
    }

    public void restartRC()
    {
        if (NewRoundLevel != null && Level.Name != NewRoundLevel.Name && PhotonNetwork.isMasterClient)
        {
            Level = NewRoundLevel;
            SetGamemode(NewRoundGamemode);
            var hash = new ExitGames.Client.Photon.Hashtable
            {
                {"level", Level.Name},
                {"gamemode", Gamemode.Settings.GamemodeType.ToString()}
            };
            PhotonNetwork.room.SetCustomProperties(hash);
            var json = JsonConvert.SerializeObject(Gamemode.Settings);
            photonView.RPC<string, GamemodeType, PhotonMessageInfo>(
                SyncSettings,
                PhotonTargets.Others,
                json,
                Gamemode.Settings.GamemodeType);
            PhotonNetwork.LoadLevel(Level.SceneName);
        }
        else if (NewRoundGamemode != null && Gamemode.Settings.GamemodeType != NewRoundGamemode.GamemodeType && PhotonNetwork.isMasterClient)
        {
            SetGamemode(NewRoundGamemode);
            var hash = new ExitGames.Client.Photon.Hashtable
            {
                {"level", Level.Name},
                {"gamemode", Gamemode.Settings.GamemodeType.ToString()}
            };
            PhotonNetwork.room.SetCustomProperties(hash);
            var json = JsonConvert.SerializeObject(Gamemode.Settings);
            photonView.RPC<string, GamemodeType, PhotonMessageInfo>(
                SyncSettings,
                PhotonTargets.Others,
                json,
                Gamemode.Settings.GamemodeType);
        }

        intVariables.Clear();
        boolVariables.Clear();
        stringVariables.Clear();
        floatVariables.Clear();
        playerVariables.Clear();
        titanVariables.Clear();
        EventManager.OnRestart.Invoke();
    }

    [PunRPC]
    private void RPCLoadLevel(PhotonMessageInfo info)
    {
        if (info.sender.isMasterClient)
        {
            DestroyAllExistingCloths();
            PhotonNetwork.LoadLevel(Level.SceneName);
        }
        else if (PhotonNetwork.isMasterClient)
        {
            kickPlayerRC(info.sender, true, "false restart.");
        }
        else if (!masterRC)
        {
            restartCount.Add(Time.time);
            foreach (var num in restartCount)
            {
                if ((Time.time - num) > 60f)
                {
                    restartCount.Remove(num);
                }
            }
            if (restartCount.Count < 6)
            {
                DestroyAllExistingCloths();
                PhotonNetwork.LoadLevel(Level.SceneName);
            }
        }
    }

    public void sendChatContentInfo(string content)
    {
        photonView.RPC<string, string, PhotonMessageInfo>(
            Chat,
            PhotonTargets.All,
            content,
            string.Empty);
    }

    public void sendKillInfo(bool t1, string killer, bool t2, string victim, int dmg = 0)
    {
        photonView.RPC<bool, string, bool, string, int>(updateKillInfo, PhotonTargets.All, t1, killer, t2, victim, dmg);
    }

    public static void ServerCloseConnection(PhotonPlayer targetPlayer, bool requestIpBan, string inGameName = null)
    {
        var options = new RaiseEventOptions
        {
            TargetActors = new int[] { targetPlayer.ID }
        };
        if (requestIpBan)
        {
            var eventContent = new ExitGames.Client.Photon.Hashtable
            {
                [(byte) 0] = true
            };
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

    public static void ServerRequestAuthentication(string authPassword)
    {
        if (!string.IsNullOrEmpty(authPassword))
        {
            var eventContent = new ExitGames.Client.Photon.Hashtable
            {
                [(byte) 0] = authPassword
            };
            PhotonNetwork.RaiseEvent(0xc6, eventContent, true, new RaiseEventOptions());
        }
    }

    public static void ServerRequestUnban(string bannedAddress)
    {
        if (!string.IsNullOrEmpty(bannedAddress))
        {
            var eventContent = new ExitGames.Client.Photon.Hashtable
            {
                [(byte) 0] = bannedAddress
            };
            PhotonNetwork.RaiseEvent(0xc7, eventContent, true, new RaiseEventOptions());
        }
    }

    public void setBackground()
    {
        if (isAssetLoaded)
        {
            Instantiate(RCassets.LoadAsset("backgroundCamera"));
        }
    }

    private void setGameSettings(ExitGames.Client.Photon.Hashtable hash)
    {
    }

    [PunRPC]
    public void setMasterRC(PhotonMessageInfo info)
    {
        if (info.sender.isMasterClient)
        {
            masterRC = true;
        }
    }

    private void setTeam(int setting)
    {
        if (setting == 0)
        {
            name = LoginFengKAI.player.name;
            var propertiesToSet = new ExitGames.Client.Photon.Hashtable
            {
                { PhotonPlayerProperty.RCteam, 0 },
                { PhotonPlayerProperty.name, name }
            };
            PhotonNetwork.player.SetCustomProperties(propertiesToSet);
        }
        else if (setting == 1)
        {
            var hashtable2 = new ExitGames.Client.Photon.Hashtable
            {
                { PhotonPlayerProperty.RCteam, 1 }
            };
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
            var hashtable3 = new ExitGames.Client.Photon.Hashtable
            {
                { PhotonPlayerProperty.RCteam, 2 }
            };
            var str2 = LoginFengKAI.player.name;
            if (!str2.StartsWith("<color=#ff00ff>"))
            {
                str2 = $"<color=#ff00ff>{str2}</color>";
            }
            name = str2;
            hashtable3.Add(PhotonPlayerProperty.name, name);
            PhotonNetwork.player.SetCustomProperties(hashtable3);
        }
        else if (setting == 3)
        {
            var num3 = 0;
            var num4 = 0;
            var num5 = 1;
            foreach (var player in PhotonNetwork.playerList)
            {
                var num7 = RCextensions.returnIntFromObject(player.CustomProperties[PhotonPlayerProperty.RCteam]);
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
            setTeam(num5);
        }
        if (((setting == 0) || (setting == 1)) || (setting == 2))
        {
            foreach (var obj2 in GameObject.FindGameObjectsWithTag("Player"))
            {
                if (obj2.GetPhotonView().isMine)
                {
                    photonView.RPC<int, PhotonMessageInfo>(labelRPC, PhotonTargets.All, obj2.GetPhotonView().viewID);
                }
            }
        }
    }

    [PunRPC]
    public void setTeamRPC(int setting, PhotonMessageInfo info)
    {
        if (info.sender.isMasterClient || info.sender.isLocal)
        {
            setTeam(setting);
        }
    }

    [PunRPC]
    public void settingRPC(ExitGames.Client.Photon.Hashtable hash, PhotonMessageInfo info)
    {
        if (info.sender.isMasterClient)
        {
            setGameSettings(hash);
        }
    }

    [PunRPC]
    public void showChatContent(string content)
    {
        chatContent.Add(content);
        if (chatContent.Count > 10)
        {
            chatContent.RemoveAt(0);
        }
        //GameObject.Find("LabelChatContent").GetComponent<UILabel>().text = string.Empty;
        //for (int i = 0; i < this.chatContent.Count; i++)
        //{
        //    UILabel component = GameObject.Find("LabelChatContent").GetComponent<UILabel>();
        //    component.text = component.text + this.chatContent[i];
        //}
    }

    [PunRPC]
    public void SyncSettings(string gamemodeRaw, GamemodeType type, PhotonMessageInfo info)
    {
        var settings = GamemodeBase.ConvertToGamemode(gamemodeRaw, type);
        if (info.sender.IsMasterClient)
        {
            Gamemode.Settings = settings;
            if (mainCamera.main_object != null)
            {
                mainCamera.main_object.GetComponent<Hero>()?.SetHorse();
            }
            if (Gamemode.Settings.EndlessRevive > 0)
            {
                StopCoroutine(respawnE(Gamemode.Settings.EndlessRevive));
                StartCoroutine(respawnE(Gamemode.Settings.EndlessRevive));
            }
            else
            {
                StopCoroutine(respawnE(Gamemode.Settings.EndlessRevive));
            }

            if (Gamemode.Settings.TeamMode != TeamMode.Disabled)
            {
                if (RCextensions.returnIntFromObject(PhotonNetwork.player.CustomProperties[PhotonPlayerProperty.RCteam]) == 0)
                {
                    setTeam(3);
                }
            }
            else
            {
                setTeam(0);
            }


            if (Gamemode.Settings.GamemodeType == GamemodeType.Infection)
            {
                var gamemodeInfection = (InfectionGamemodeSettings) settings;
                if (gamemodeInfection.Infected > 0)
                {
                    var hashtable = new ExitGames.Client.Photon.Hashtable
                    {
                        { PhotonPlayerProperty.RCteam, 0 }
                    };
                    PhotonNetwork.player.SetCustomProperties(hashtable);
                    chatRoom.addLINE($"<color=#FFCC00>Infection mode ({gamemodeInfection.Infected}) enabled. Make sure your first character is human.</color>");
                }
                else
                {
                    var hashtable = new ExitGames.Client.Photon.Hashtable
                    {
                        { PhotonPlayerProperty.isTitan, 1 }
                    };
                    PhotonNetwork.player.SetCustomProperties(hashtable);
                    chatRoom.addLINE("<color=#FFCC00>Infection Mode disabled.</color>");
                }
            }
        }
    }

    [PunRPC]
    private void RequestSettings(PhotonMessageInfo info)
    {
        var json = JsonConvert.SerializeObject(Gamemode.Settings);
        photonView.RPC("SyncSettings", info.sender, json, Gamemode.Settings.GamemodeType);
    }

    public void ShowHUDInfoCenter(string content)
    {
        InGameUI.HUD.Labels.Center.text = content;

    }

    public void ShowHUDInfoCenterADD(string content)
    {
    }

    private void ShowHUDInfoTopCenter(string content)
    {
        InGameUI.HUD.Labels.Top.text = content;
    }

    private void ShowHUDInfoTopCenterADD(string content)
    {

    }

    private void ShowHUDInfoTopLeft(string content)
    {
        InGameUI.HUD.Labels.TopLeft.text = content;
    }

    private void ShowHUDInfoTopRight(string content)
    {
        InGameUI.HUD.Labels.TopRight.text = content;
    }

    private void ShowHUDInfoTopRightMAPNAME(string content)
    {

    }

    [PunRPC]
    private void showResult(string text0, string text1, string text2, string text3, string text4, string text6, PhotonMessageInfo t)
    {
        if (!(gameTimesUp || !t.sender.isMasterClient))
        {
            gameTimesUp = true;
            var obj2 = GameObject.Find("UI_IN_GAME");
            Screen.lockCursor = false;
            Cursor.visible = true;
            IN_GAME_MAIN_CAMERA.gametype = GAMETYPE.STOP;
            gameStart = false;
        }
        else if (!(t.sender.isMasterClient || !PhotonNetwork.player.isMasterClient))
        {
            kickPlayerRC(t.sender, true, "false game end.");
        }
    }

    [PunRPC]
    public void someOneIsDead(int id = -1)
    {
        EventManager.OnPlayerKilled.Invoke(id);
    }

    public void SpawnPlayer(string id, string tag = "playerRespawn")
    {
        if (id == null)
        {
            id = "1";
        }
        myLastRespawnTag = tag;
        var location = Gamemode.GetPlayerSpawnLocation(tag);
        SpawnPlayerAt2(id, location);
    }

    public void SpawnPlayerAt2(string id, GameObject pos)
    {
        // HACK
        if (false)
        //if (!logicLoaded || !customLevelLoaded)
        {
            NOTSpawnPlayerRC(id);
        }
        else
        {
            var position = pos.transform.position;
            if (racingSpawnPointSet)
            {
                position = racingSpawnPoint;
            }
            else if (Level.Name.StartsWith("Custom"))
            {
                if (RCextensions.returnIntFromObject(PhotonNetwork.player.CustomProperties[PhotonPlayerProperty.RCteam]) == 0)
                {
                    var list = new List<Vector3>();
                    foreach (var vector2 in playerSpawnsC)
                    {
                        list.Add(vector2);
                    }
                    foreach (var vector2 in playerSpawnsM)
                    {
                        list.Add(vector2);
                    }
                    if (list.Count > 0)
                    {
                        position = list[UnityEngine.Random.Range(0, list.Count)];
                    }
                }
                else if (RCextensions.returnIntFromObject(PhotonNetwork.player.CustomProperties[PhotonPlayerProperty.RCteam]) == 1)
                {
                    if (playerSpawnsC.Count > 0)
                    {
                        position = playerSpawnsC[UnityEngine.Random.Range(0, playerSpawnsC.Count)];
                    }
                }
                else if ((RCextensions.returnIntFromObject(PhotonNetwork.player.CustomProperties[PhotonPlayerProperty.RCteam]) == 2) && (playerSpawnsM.Count > 0))
                {
                    position = playerSpawnsM[UnityEngine.Random.Range(0, playerSpawnsM.Count)];
                }
            }
            var component = GameObject.Find("MainCamera").GetComponent<IN_GAME_MAIN_CAMERA>();
            myLastHero = id.ToUpper();
            if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
            {
                if (IN_GAME_MAIN_CAMERA.singleCharacter == "TITAN_EREN")
                {
                    component.setMainObject((GameObject) Instantiate(Resources.Load("TITAN_EREN"), pos.transform.position, pos.transform.rotation), true, false);
                }
                else
                {
                    component.setMainObject((GameObject) Instantiate(Resources.Load("AOTTG_HERO 1"), pos.transform.position, pos.transform.rotation), true, false);
                    if (((IN_GAME_MAIN_CAMERA.singleCharacter == "SET 1") || (IN_GAME_MAIN_CAMERA.singleCharacter == "SET 2")) || (IN_GAME_MAIN_CAMERA.singleCharacter == "SET 3"))
                    {
                        var costume = CostumeConeveter.LocalDataToHeroCostume(IN_GAME_MAIN_CAMERA.singleCharacter);
                        costume.checkstat();
                        CostumeConeveter.HeroCostumeToLocalData(costume, IN_GAME_MAIN_CAMERA.singleCharacter);
                        component.main_object.GetComponent<Hero>().GetComponent<HERO_SETUP>().init();
                        if (costume != null)
                        {
                            component.main_object.GetComponent<Hero>().GetComponent<HERO_SETUP>().myCostume = costume;
                            component.main_object.GetComponent<Hero>().GetComponent<HERO_SETUP>().myCostume.stat = costume.stat;
                        }
                        else
                        {
                            costume = HeroCostume.costumeOption[3];
                            component.main_object.GetComponent<Hero>().GetComponent<HERO_SETUP>().myCostume = costume;
                            component.main_object.GetComponent<Hero>().GetComponent<HERO_SETUP>().myCostume.stat = HeroStat.getInfo(costume.name.ToUpper());
                        }
                        component.main_object.GetComponent<Hero>().GetComponent<HERO_SETUP>().setCharacterComponent();
                        component.main_object.GetComponent<Hero>().setStat2();
                        component.main_object.GetComponent<Hero>().setSkillHUDPosition2();
                    }
                    else
                    {
                        for (var i = 0; i < HeroCostume.costume.Length; i++)
                        {
                            if (HeroCostume.costume[i].name.ToUpper() == IN_GAME_MAIN_CAMERA.singleCharacter.ToUpper())
                            {
                                var index = (HeroCostume.costume[i].id + CheckBoxCostume.costumeSet) - 1;
                                if (HeroCostume.costume[index].name != HeroCostume.costume[i].name)
                                {
                                    index = HeroCostume.costume[i].id + 1;
                                }
                                component.main_object.GetComponent<Hero>().GetComponent<HERO_SETUP>().init();
                                component.main_object.GetComponent<Hero>().GetComponent<HERO_SETUP>().myCostume = HeroCostume.costume[index];
                                component.main_object.GetComponent<Hero>().GetComponent<HERO_SETUP>().myCostume.stat = HeroStat.getInfo(HeroCostume.costume[index].name.ToUpper());
                                component.main_object.GetComponent<Hero>().GetComponent<HERO_SETUP>().setCharacterComponent();
                                component.main_object.GetComponent<Hero>().setStat2();
                                component.main_object.GetComponent<Hero>().setSkillHUDPosition2();
                                break;
                            }
                        }
                    }
                }
            }
            else
            {
                component.setMainObject(PhotonNetwork.Instantiate("AOTTG_HERO 1", position, pos.transform.rotation, 0), true, false);
                id = id.ToUpper();
                if (((id == "SET 1") || (id == "SET 2")) || (id == "SET 3") || true) //HACK
                {
                    var costume2 = CostumeConeveter.LocalDataToHeroCostume(id);
                    costume2.checkstat();
                    CostumeConeveter.HeroCostumeToLocalData(costume2, id);
                    component.main_object.GetComponent<Hero>().GetComponent<HERO_SETUP>().init();
                    if (costume2 != null)
                    {
                        component.main_object.GetComponent<Hero>().GetComponent<HERO_SETUP>().myCostume = costume2;
                        component.main_object.GetComponent<Hero>().GetComponent<HERO_SETUP>().myCostume.stat = costume2.stat;
                    }
                    else
                    {
                        costume2 = HeroCostume.costumeOption[3];
                        component.main_object.GetComponent<Hero>().GetComponent<HERO_SETUP>().myCostume = costume2;
                        component.main_object.GetComponent<Hero>().GetComponent<HERO_SETUP>().myCostume.stat = HeroStat.getInfo(costume2.name.ToUpper());
                    }
                    component.main_object.GetComponent<Hero>().GetComponent<HERO_SETUP>().setCharacterComponent();
                    component.main_object.GetComponent<Hero>().setStat2();
                    component.main_object.GetComponent<Hero>().setSkillHUDPosition2();
                }
                else
                {
                    for (var j = 0; j < HeroCostume.costume.Length; j++)
                    {
                        if (HeroCostume.costume[j].name.ToUpper() == id.ToUpper())
                        {
                            var num4 = HeroCostume.costume[j].id;
                            if (id.ToUpper() != "AHSS")
                            {
                                num4 += CheckBoxCostume.costumeSet - 1;
                            }
                            if (HeroCostume.costume[num4].name != HeroCostume.costume[j].name)
                            {
                                num4 = HeroCostume.costume[j].id + 1;
                            }
                            component.main_object.GetComponent<Hero>().GetComponent<HERO_SETUP>().init();
                            component.main_object.GetComponent<Hero>().GetComponent<HERO_SETUP>().myCostume = HeroCostume.costume[num4];
                            component.main_object.GetComponent<Hero>().GetComponent<HERO_SETUP>().myCostume.stat = HeroStat.getInfo(HeroCostume.costume[num4].name.ToUpper());
                            component.main_object.GetComponent<Hero>().GetComponent<HERO_SETUP>().setCharacterComponent();
                            component.main_object.GetComponent<Hero>().setStat2();
                            component.main_object.GetComponent<Hero>().setSkillHUDPosition2();
                            break;
                        }
                    }
                }
                CostumeConeveter.HeroCostumeToPhotonData2(component.main_object.GetComponent<Hero>().GetComponent<HERO_SETUP>().myCostume, PhotonNetwork.player);
                var hashtable = new ExitGames.Client.Photon.Hashtable
                {
                    { "dead", false }
                };
                var propertiesToSet = hashtable;
                PhotonNetwork.player.SetCustomProperties(propertiesToSet);
                hashtable = new ExitGames.Client.Photon.Hashtable
                {
                    { PhotonPlayerProperty.isTitan, 1 }
                };
                propertiesToSet = hashtable;
                PhotonNetwork.player.SetCustomProperties(propertiesToSet);
            }
            Gamemode.OnPlayerSpawned(component.main_object);
            component.enabled = true;
            GameObject.Find("MainCamera").GetComponent<IN_GAME_MAIN_CAMERA>().setHUDposition();
            GameObject.Find("MainCamera").GetComponent<SpectatorMovement>().disable = true;
            //TODO MouseLook
            //GameObject.Find("MainCamera").GetComponent<MouseLook>().disable = true;
            component.gameOver = false;
            if (IN_GAME_MAIN_CAMERA.cameraMode == CAMERA_TYPE.TPS)
            {
                Screen.lockCursor = true;
            }
            else
            {
                Screen.lockCursor = false;
            }
            Cursor.visible = false;
            isLosing = false;
            ShowHUDInfoCenter(string.Empty);
        }
    }


    [PunRPC]
    public void spawnPlayerAtRPC(float posX, float posY, float posZ, PhotonMessageInfo info)
    {
        if (((info.sender.isMasterClient && logicLoaded) && (customLevelLoaded && !needChooseSide)) && Camera.main.GetComponent<IN_GAME_MAIN_CAMERA>().gameOver)
        {
            var position = new Vector3(posX, posY, posZ);
            var component = Camera.main.GetComponent<IN_GAME_MAIN_CAMERA>();
            component.setMainObject(PhotonNetwork.Instantiate("AOTTG_HERO 1", position, new Quaternion(0f, 0f, 0f, 1f), 0), true, false);
            var slot = myLastHero.ToUpper();
            switch (slot)
            {
                case "SET 1":
                case "SET 2":
                case "SET 3":
                    {
                        var costume = CostumeConeveter.LocalDataToHeroCostume(slot);
                        costume.checkstat();
                        CostumeConeveter.HeroCostumeToLocalData(costume, slot);
                        component.main_object.GetComponent<Hero>().GetComponent<HERO_SETUP>().init();
                        if (costume != null)
                        {
                            component.main_object.GetComponent<Hero>().GetComponent<HERO_SETUP>().myCostume = costume;
                            component.main_object.GetComponent<Hero>().GetComponent<HERO_SETUP>().myCostume.stat = costume.stat;
                        }
                        else
                        {
                            costume = HeroCostume.costumeOption[3];
                            component.main_object.GetComponent<Hero>().GetComponent<HERO_SETUP>().myCostume = costume;
                            component.main_object.GetComponent<Hero>().GetComponent<HERO_SETUP>().myCostume.stat = HeroStat.getInfo(costume.name.ToUpper());
                        }
                        component.main_object.GetComponent<Hero>().GetComponent<HERO_SETUP>().setCharacterComponent();
                        component.main_object.GetComponent<Hero>().setStat2();
                        component.main_object.GetComponent<Hero>().setSkillHUDPosition2();
                        break;
                    }
                default:
                    for (var i = 0; i < HeroCostume.costume.Length; i++)
                    {
                        if (HeroCostume.costume[i].name.ToUpper() == slot.ToUpper())
                        {
                            var id = HeroCostume.costume[i].id;
                            if (slot.ToUpper() != "AHSS")
                            {
                                id += CheckBoxCostume.costumeSet - 1;
                            }
                            if (HeroCostume.costume[id].name != HeroCostume.costume[i].name)
                            {
                                id = HeroCostume.costume[i].id + 1;
                            }
                            component.main_object.GetComponent<Hero>().GetComponent<HERO_SETUP>().init();
                            component.main_object.GetComponent<Hero>().GetComponent<HERO_SETUP>().myCostume = HeroCostume.costume[id];
                            component.main_object.GetComponent<Hero>().GetComponent<HERO_SETUP>().myCostume.stat = HeroStat.getInfo(HeroCostume.costume[id].name.ToUpper());
                            component.main_object.GetComponent<Hero>().GetComponent<HERO_SETUP>().setCharacterComponent();
                            component.main_object.GetComponent<Hero>().setStat2();
                            component.main_object.GetComponent<Hero>().setSkillHUDPosition2();
                            break;
                        }
                    }
                    break;
            }
            CostumeConeveter.HeroCostumeToPhotonData2(component.main_object.GetComponent<Hero>().GetComponent<HERO_SETUP>().myCostume, PhotonNetwork.player);
            Gamemode.OnPlayerSpawned(component.main_object);
            var hashtable = new ExitGames.Client.Photon.Hashtable
            {
                { "dead", false }
            };
            var propertiesToSet = hashtable;
            PhotonNetwork.player.SetCustomProperties(propertiesToSet);
            hashtable = new ExitGames.Client.Photon.Hashtable
            {
                { PhotonPlayerProperty.isTitan, 1 }
            };
            propertiesToSet = hashtable;
            PhotonNetwork.player.SetCustomProperties(propertiesToSet);
            component.enabled = true;
            GameObject.Find("MainCamera").GetComponent<IN_GAME_MAIN_CAMERA>().setHUDposition();
            GameObject.Find("MainCamera").GetComponent<SpectatorMovement>().disable = true;
            //TODO MouseLook
            //GameObject.Find("MainCamera").GetComponent<MouseLook>().disable = true;
            component.gameOver = false;
            if (IN_GAME_MAIN_CAMERA.cameraMode == CAMERA_TYPE.TPS)
            {
                Screen.lockCursor = true;
            }
            else
            {
                Screen.lockCursor = false;
            }
            Cursor.visible = false;
            isLosing = false;
            ShowHUDInfoCenter(string.Empty);
        }
    }

    private void spawnPlayerCustomMap()
    {
        if (!needChooseSide && GameObject.Find("MainCamera").GetComponent<IN_GAME_MAIN_CAMERA>().gameOver)
        {
            Camera.main.GetComponent<IN_GAME_MAIN_CAMERA>().gameOver = false;
            if (RCextensions.returnIntFromObject(PhotonNetwork.player.CustomProperties[PhotonPlayerProperty.isTitan]) == 2)
            {
                SpawnPlayerTitan();
            }
            else
            {
                SpawnPlayer(myLastHero, myLastRespawnTag);
            }
            ShowHUDInfoCenter(string.Empty);
        }
    }

    public GameObject SpawnTitan(TitanConfiguration configuration)
    {
        var position = new Vector3();
        var rotation = new Quaternion();
        if (titanSpawns.Count > 0) // RC Custom Map Spawns
        {
            position = titanSpawns[UnityEngine.Random.Range(0, titanSpawns.Count)];
        }
        else
        {
            var randomSpawns = GameObject.FindGameObjectsWithTag("titanRespawn");
            if (randomSpawns.Length > 0)
            {
                var random = UnityEngine.Random.Range(0, randomSpawns.Length);
                var randomSpawn = randomSpawns[random];
                while (randomSpawn == null)
                {
                    random = UnityEngine.Random.Range(0, randomSpawns.Length);
                    randomSpawn = randomSpawns[random];
                }
                randomSpawns[random] = null;
                position = randomSpawn.transform.position;
                rotation = randomSpawn.transform.rotation;
            }
        }

        return SpawnTitan(position, rotation, configuration);
    }

    public GameObject SpawnTitan(Vector3 position, Quaternion rotation)
    {
        return SpawnTitan(position, rotation, new TitanConfiguration());
    }

    public GameObject SpawnTitan(Vector3 position, Quaternion rotation, TitanConfiguration configuration)
    {
        var titan = PhotonNetwork.Instantiate("MindlessTitan", position, rotation, 0);
        titan.GetComponent<MindlessTitan>().Initialize(configuration);
        return titan;
    }

    public void SpawnPlayerTitan()
    {
        var id = "TITAN";
        var tag = "titanRespawn";
        var location = Gamemode.GetPlayerSpawnLocation(tag);
        var position = location.transform.position;
        if (Level.Name.StartsWith("Custom") && (titanSpawns.Count > 0))
        {
            position = titanSpawns[UnityEngine.Random.Range(0, titanSpawns.Count)];
        }
        myLastHero = id.ToUpper();
        var playerTitan = PhotonNetwork.Instantiate("PlayerTitan", position, new Quaternion(), 0).GetComponent<PlayerTitan>();
        playerTitan.Initialize(Gamemode.GetPlayerTitanConfiguration());
        GameObject.Find("MainCamera").GetComponent<IN_GAME_MAIN_CAMERA>().setMainObjectASTITAN(playerTitan.gameObject);
        GameObject.Find("MainCamera").GetComponent<IN_GAME_MAIN_CAMERA>().enabled = true;
        GameObject.Find("MainCamera").GetComponent<SpectatorMovement>().disable = true;
        //TODO MouseLook
        //GameObject.Find("MainCamera").GetComponent<MouseLook>().disable = true;
        GameObject.Find("MainCamera").GetComponent<IN_GAME_MAIN_CAMERA>().gameOver = false;
        var hashtable = new ExitGames.Client.Photon.Hashtable
        {
            { "dead", false }
        };
        var propertiesToSet = hashtable;
        PhotonNetwork.player.SetCustomProperties(propertiesToSet);
        hashtable = new ExitGames.Client.Photon.Hashtable
        {
            { PhotonPlayerProperty.isTitan, 2 }
        };
        propertiesToSet = hashtable;
        PhotonNetwork.player.SetCustomProperties(propertiesToSet);
        if (IN_GAME_MAIN_CAMERA.cameraMode == CAMERA_TYPE.TPS)
        {
            Screen.lockCursor = true;
        }
        else
        {
            Screen.lockCursor = false;
        }
        Cursor.visible = true;
        ShowHUDInfoCenter(string.Empty);
    }

    private void Start()
    {
        Debug.Log($"Version: {versionManager.Version}");
        instance = this;
        gameObject.name = "MultiplayerManager";
        CostumeHair.init();
        CharacterMaterials.init();
        HeroCostume.init2();
        DontDestroyOnLoad(gameObject);
        heroes = new ArrayList();
        eT = new ArrayList();
        titans = new ArrayList();
        fT = new ArrayList();
        cT = new ArrayList();
        hooks = new ArrayList();
        name = string.Empty;
        if (nameField == null)
        {
            nameField = "GUEST" + UnityEngine.Random.Range(0, 0x186a0);
        }
        if (privateServerField == null)
        {
            privateServerField = string.Empty;
        }
        usernameField = string.Empty;
        passwordField = string.Empty;
        resetGameSettings();
        banHash = new ExitGames.Client.Photon.Hashtable();
        imatitan = new ExitGames.Client.Photon.Hashtable();
        oldScript = string.Empty;
        currentLevel = string.Empty;
        if (currentScript == null)
        {
            currentScript = string.Empty;
        }
        titanSpawns = new List<Vector3>();
        playerSpawnsC = new List<Vector3>();
        playerSpawnsM = new List<Vector3>();
        playersRPC = new List<PhotonPlayer>();
        levelCache = new List<string[]>();
        titanSpawners = new List<TitanSpawner>();
        restartCount = new List<float>();
        ignoreList = new List<int>();
        groundList = new List<GameObject>();
        noRestart = false;
        masterRC = false;
        isSpawning = false;
        intVariables = new ExitGames.Client.Photon.Hashtable();
        heroHash = new ExitGames.Client.Photon.Hashtable();
        boolVariables = new ExitGames.Client.Photon.Hashtable();
        stringVariables = new ExitGames.Client.Photon.Hashtable();
        floatVariables = new ExitGames.Client.Photon.Hashtable();
        globalVariables = new ExitGames.Client.Photon.Hashtable();
        RCRegions = new ExitGames.Client.Photon.Hashtable();
        RCEvents = new ExitGames.Client.Photon.Hashtable();
        RCVariableNames = new ExitGames.Client.Photon.Hashtable();
        RCRegionTriggers = new ExitGames.Client.Photon.Hashtable();
        playerVariables = new ExitGames.Client.Photon.Hashtable();
        titanVariables = new ExitGames.Client.Photon.Hashtable();
        logicLoaded = false;
        customLevelLoaded = false;
        oldScriptLogic = string.Empty;
        currentScriptLogic = string.Empty;
        retryTime = 0f;
        playerList = string.Empty;
        updateTime = 0f;
        if (textureBackgroundBlack == null)
        {
            textureBackgroundBlack = new Texture2D(1, 1, TextureFormat.ARGB32, false);
            textureBackgroundBlack.SetPixel(0, 0, new Color(0f, 0f, 0f, 1f));
            textureBackgroundBlack.Apply();
        }
        if (textureBackgroundBlue == null)
        {
            textureBackgroundBlue = new Texture2D(1, 1, TextureFormat.ARGB32, false);
            textureBackgroundBlue.SetPixel(0, 0, new Color(0.08f, 0.3f, 0.4f, 1f));
            textureBackgroundBlue.Apply();
        }
        loadconfig();
        setBackground();
        ChangeQuality.setCurrentQuality();
    }

    [PunRPC]
    public void titanGetKill(PhotonPlayer player, int Damage, string name)
    {
        Damage = Mathf.Max(10, Damage);
        photonView.RPC<int>(netShowDamage, player, Damage);
        photonView.RPC<string>(oneTitanDown, PhotonTargets.MasterClient, name);
        sendKillInfo(false, (string) player.CustomProperties[PhotonPlayerProperty.name], true, name, Damage);
        playerKillInfoUpdate(player, Damage);
    }

    public void titanGetKillbyServer(int Damage, string name)
    {
        Damage = Mathf.Max(10, Damage);
        sendKillInfo(false, LoginFengKAI.player.name, true, name, Damage);
        netShowDamage(Damage);
        oneTitanDown(name);
        playerKillInfoUpdate(PhotonNetwork.player, Damage);
    }

    public void unloadAssets()
    {
        if (!isUnloading)
        {
            isUnloading = true;
            StartCoroutine(unloadAssetsE(10f));
        }
    }

    public IEnumerator unloadAssetsE(float time)
    {
        yield return new WaitForSeconds(time);
        Resources.UnloadUnusedAssets();
        isUnloading = false;
    }

    public void unloadAssetsEditor()
    {
        if (!isUnloading)
        {
            isUnloading = true;
            StartCoroutine(unloadAssetsE(30f));
        }
    }

    //TODO: This is called every frame... wtf???
    //Major performance increase can be achieved by moving some of this into fixed update.
    private void Update()
    {
        if ((IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.SINGLE) && (GameObject.Find("LabelNetworkStatus") != null))
        {
            //GameObject.Find("LabelNetworkStatus").GetComponent<UILabel>().text = PhotonNetwork.connectionState.ToString();
            //if (PhotonNetwork.connected)
            //{
            //    UILabel component = GameObject.Find("LabelNetworkStatus").GetComponent<UILabel>();
            //    component.text = component.text + " ping:" + PhotonNetwork.GetPing();
            //}
        }
        if (gameStart)
        {
            var enumerator = heroes.GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    var current = (Hero) enumerator.Current;
                    if (current != null)
                        current.update2();
                }
            }
            finally
            {
                var disposable = enumerator as IDisposable;
                if (disposable != null)
                {
                    disposable.Dispose();
                }
            }
            var enumerator2 = hooks.GetEnumerator();
            try
            {
                while (enumerator2.MoveNext())
                {
                    var current = (Bullet) enumerator2.Current;
                    if (current != null)
                        current.update();
                }
            }
            finally
            {
                var disposable2 = enumerator2 as IDisposable;
                if (disposable2 != null)
                {
                    disposable2.Dispose();
                }
            }
            if (mainCamera != null)
            {
                mainCamera.snapShotUpdate();
            }
            var enumerator3 = eT.GetEnumerator();
            try
            {
                while (enumerator3.MoveNext())
                {
                    var titanEren = (TITAN_EREN) enumerator3.Current;
                    if (titanEren != null)
                        titanEren.update();
                }
            }
            finally
            {
                var disposable3 = enumerator3 as IDisposable;
                if (disposable3 != null)
                {
                    disposable3.Dispose();
                }
            }
            var enumerator5 = fT.GetEnumerator();
            try
            {
                while (enumerator5.MoveNext())
                {
                    var femaleTitan = (FEMALE_TITAN) enumerator5.Current;
                    if (femaleTitan != null)
                        femaleTitan.update();
                }
            }
            finally
            {
                var disposable5 = enumerator5 as IDisposable;
                if (disposable5 != null)
                {
                    disposable5.Dispose();
                }
            }
            var enumerator6 = cT.GetEnumerator();
            try
            {
                while (enumerator6.MoveNext())
                {
                    var colossalTitan = (COLOSSAL_TITAN) enumerator6.Current;
                    if (colossalTitan != null)
                        colossalTitan.update2();
                }
            }
            finally
            {
                var disposable6 = enumerator6 as IDisposable;
                if (disposable6 != null)
                {
                    disposable6.Dispose();
                }
            }
            if (mainCamera != null)
            {
                mainCamera.update2();
            }
        }
    }

    [PunRPC]
    private void updateKillInfo(bool t1, string killer, bool t2, string victim, int dmg)
    {
        GameObject obj4;
        var obj2 = GameObject.Find("KillFeed");
        var obj3 = (GameObject) Instantiate(Resources.Load("UI/KillInfo"));
        for (var i = 0; i < killInfoGO.Count; i++)
        {
            obj4 = (GameObject) killInfoGO[i];
            if (obj4 != null)
            {
                obj4.GetComponent<KillInfo>().MoveOn();
            }
        }
        if (killInfoGO.Count > 4)
        {
            obj4 = (GameObject) killInfoGO[0];
            if (obj4 != null)
            {
                obj4.GetComponent<KillInfo>().Destroy();
            }
            killInfoGO.RemoveAt(0);
        }

        obj3.transform.parent = obj2.transform;
        obj3.transform.position = new Vector3();
        obj3.GetComponent<KillInfo>().Show(t1, killer, t2, victim, dmg);
        killInfoGO.Add(obj3);
        if (((int) settings[0xf4]) == 1)
        {
            var str2 = ("<color=#FFC000>(" + roundTime.ToString("F2") + ")</color> ") + killer.hexColor() + " killed ";
            var newLine = str2 + victim.hexColor() + " for " + dmg.ToString() + " damage.";
            chatRoom.addLINE(newLine);
        }
    }

    [PunRPC]
    public void verifyPlayerHasLeft(int ID, PhotonMessageInfo info)
    {
        if (info.sender.isMasterClient && (PhotonPlayer.Find(ID) != null))
        {
            var player = PhotonPlayer.Find(ID);
            var str = string.Empty;
            str = RCextensions.returnStringFromObject(player.CustomProperties[PhotonPlayerProperty.name]);
            banHash.Add(ID, str);
        }
    }

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
        EventManager.OnUpdate.Invoke(time);
        var iteratorVariable1 = string.Empty;
        if (Gamemode.Settings.TeamMode == TeamMode.Disabled)
        {
            foreach (var player7 in PhotonNetwork.playerList)
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
                    var iteratorVariable0 = iteratorVariable1;
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
            var num2 = 0;
            var num3 = 0;
            var num4 = 0;
            var num5 = 0;
            var num6 = 0;
            var num7 = 0;
            var num8 = 0;
            var num9 = 0;
            var dictionary = new Dictionary<int, PhotonPlayer>();
            var dictionary2 = new Dictionary<int, PhotonPlayer>();
            var dictionary3 = new Dictionary<int, PhotonPlayer>();
            var playerList = PhotonNetwork.playerList;
            for (var j = 0; j < playerList.Length; j++)
            {
                var player = playerList[j];
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
            cyanKills = num2;
            magentaKills = num3;
            if (PhotonNetwork.isMasterClient)
            {
                if (Gamemode.Settings.TeamMode == TeamMode.LockBySize)
                {
                    foreach (var player2 in PhotonNetwork.playerList)
                    {
                        var num12 = 0;
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
                            photonView.RPC("setTeamRPC", player2, new object[] { num12 });
                        }
                    }
                }
                else if (Gamemode.Settings.TeamMode == TeamMode.LockBySkill)
                {
                    foreach (var player3 in PhotonNetwork.playerList)
                    {
                        var num13 = 0;
                        num11 = RCextensions.returnIntFromObject(player3.CustomProperties[PhotonPlayerProperty.RCteam]);
                        if (num11 > 0)
                        {
                            if (num11 == 1)
                            {
                                var num14 = 0;
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
                                var num15 = 0;
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
                                photonView.RPC("setTeamRPC", player3, new object[] { num13 });
                            }
                        }
                    }
                }
            }
            iteratorVariable1 = string.Concat(new object[] { iteratorVariable1, "[00FFFF]TEAM CYAN", "[ffffff]:", cyanKills, "/", num4, "/", num6, "/", num8, "\n" });
            foreach (var player4 in dictionary.Values)
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
                    str2 = string.Empty;
                    str2 = RCextensions.returnStringFromObject(player4.CustomProperties[PhotonPlayerProperty.name]);
                    num17 = 0;
                    num17 = RCextensions.returnIntFromObject(player4.CustomProperties[PhotonPlayerProperty.kills]);
                    num18 = 0;
                    num18 = RCextensions.returnIntFromObject(player4.CustomProperties[PhotonPlayerProperty.deaths]);
                    num19 = 0;
                    num19 = RCextensions.returnIntFromObject(player4.CustomProperties[PhotonPlayerProperty.max_dmg]);
                    num20 = 0;
                    num20 = RCextensions.returnIntFromObject(player4.CustomProperties[PhotonPlayerProperty.total_dmg]);
                    iteratorVariable1 = string.Concat(new object[] { str, string.Empty, str2, "[ffffff]:", num17, "/", num18, "/", num19, "/", num20 });
                    if (RCextensions.returnBoolFromObject(player4.CustomProperties[PhotonPlayerProperty.dead]))
                    {
                        iteratorVariable1 = iteratorVariable1 + "[-]";
                    }
                    iteratorVariable1 = iteratorVariable1 + "\n";
                }
            }
            iteratorVariable1 = string.Concat(new object[] { iteratorVariable1, " \n", "[FF00FF]TEAM MAGENTA", "[ffffff]:", magentaKills, "/", num5, "/", num7, "/", num9, "\n" });
            foreach (var player5 in dictionary2.Values)
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
            foreach (var player6 in dictionary3.Values)
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
        if (PhotonNetwork.isMasterClient && ((!isWinning && !isLosing) && (roundTime >= 5f)))
        {
            int num22;
            if (Gamemode.Settings.PointMode > 0)
            {
                if (Gamemode.Settings.TeamMode != TeamMode.Disabled)
                {
                    if (cyanKills >= Gamemode.Settings.PointMode)
                    {
                        photonView.RPC<string, string, PhotonMessageInfo>(
                            Chat,
                            PhotonTargets.All,
                            "<color=#00FFFF>Team Cyan wins! </color>",
                            string.Empty);
                        gameWin2();
                    }
                    else if (magentaKills >= Gamemode.Settings.PointMode)
                    {
                        photonView.RPC<string, string, PhotonMessageInfo>(
                            Chat,
                            PhotonTargets.All,
                            "<color=#FF00FF>Team Magenta wins! </color>",
                            string.Empty);
                        gameWin2();
                    }
                }
                else if (Gamemode.Settings.TeamMode == TeamMode.Disabled)
                {
                    for (num22 = 0; num22 < PhotonNetwork.playerList.Length; num22++)
                    {
                        var player9 = PhotonNetwork.playerList[num22];
                        if (RCextensions.returnIntFromObject(player9.CustomProperties[PhotonPlayerProperty.kills]) >= Gamemode.Settings.PointMode)
                        {
                            var winner = RCextensions.returnStringFromObject(player9.CustomProperties[PhotonPlayerProperty.name]).hexColor();
                            photonView.RPC(
                                (Action<string, string, PhotonMessageInfo>) Chat,
                                PhotonTargets.All,
                                "<color=#FFCC00>" + winner + " wins!</color>",
                                string.Empty);
                            gameWin2();
                        }
                    }
                }
            }
            else if ((Gamemode.Settings.PointMode <= 0) && ((Gamemode.Settings.PvPBomb) || (Gamemode.Settings.Pvp != PvpMode.Disabled)))
            {
                if (Gamemode.Settings.PvPWinOnEnemiesDead)
                {
                    if ((Gamemode.Settings.TeamMode != TeamMode.Disabled) && (PhotonNetwork.playerList.Length > 1))
                    {
                        var num24 = 0;
                        var num25 = 0;
                        var num26 = 0;
                        var num27 = 0;
                        for (num22 = 0; num22 < PhotonNetwork.playerList.Length; num22++)
                        {
                            var player10 = PhotonNetwork.playerList[num22];
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
                                photonView.RPC<string, string, PhotonMessageInfo>(
                                    Chat,
                                    PhotonTargets.All,
                                    "<color=#FF00FF>Team Magenta wins! </color>",
                                    string.Empty);
                                gameWin2();
                            }
                            else if (num25 == 0)
                            {
                                photonView.RPC<string, string, PhotonMessageInfo>(
                                    Chat,
                                    PhotonTargets.All,
                                    "<color=#00FFFF>Team Cyan wins! </color>",
                                    string.Empty);
                                gameWin2();
                            }
                        }
                    }
                    else if ((Gamemode.Settings.TeamMode == TeamMode.Disabled) && (PhotonNetwork.playerList.Length > 1))
                    {
                        var num28 = 0;
                        var text = "Nobody";
                        var player11 = PhotonNetwork.playerList[0];
                        for (num22 = 0; num22 < PhotonNetwork.playerList.Length; num22++)
                        {
                            var player12 = PhotonNetwork.playerList[num22];
                            if (!((player12.CustomProperties[PhotonPlayerProperty.dead] == null) || RCextensions.returnBoolFromObject(player12.CustomProperties[PhotonPlayerProperty.dead])))
                            {
                                text = RCextensions.returnStringFromObject(player12.CustomProperties[PhotonPlayerProperty.name]).hexColor();
                                player11 = player12;
                                num28++;
                            }
                        }
                        if (num28 <= 1)
                        {
                            var scoreText = " 5 points added.";
                            if (text == "Nobody")
                            {
                                scoreText = string.Empty;
                            }
                            else
                            {
                                for (num22 = 0; num22 < 5; num22++)
                                {
                                    playerKillInfoUpdate(player11, 0);
                                }
                            }

                            photonView.RPC<string, string, PhotonMessageInfo>(
                                Chat,
                                PhotonTargets.All,
                                "<color=#FFCC00>" + text.hexColor() + " wins." + scoreText + "</color>",
                                string.Empty);
                            gameWin2();
                        }
                    }
                }
            }
        }
        isRecompiling = false;
    }

    public IEnumerator WaitAndReloadKDR(PhotonPlayer player)
    {
        yield return new WaitForSeconds(5f);
        var key = RCextensions.returnStringFromObject(player.CustomProperties[PhotonPlayerProperty.name]);
        if (PreservedPlayerKDR.ContainsKey(key))
        {
            var numArray = PreservedPlayerKDR[key];
            PreservedPlayerKDR.Remove(key);
            var propertiesToSet = new ExitGames.Client.Photon.Hashtable
            {
                { PhotonPlayerProperty.kills, numArray[0] },
                { PhotonPlayerProperty.deaths, numArray[1] },
                { PhotonPlayerProperty.max_dmg, numArray[2] },
                { PhotonPlayerProperty.total_dmg, numArray[3] }
            };
            player.SetCustomProperties(propertiesToSet);
        }
    }

    public IEnumerator WaitAndResetRestarts()
    {
        yield return new WaitForSeconds(10f);
        restartingMC = false;
    }

    public IEnumerator WaitAndRespawn1(float time, string str)
    {
        yield return new WaitForSeconds(time);
        SpawnPlayer(myLastHero, str);
    }

    public IEnumerator WaitAndRespawn2(float time, GameObject pos)
    {
        yield return new WaitForSeconds(time);
        SpawnPlayerAt2(myLastHero, pos);
    }
}
