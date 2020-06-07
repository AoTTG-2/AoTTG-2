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
    public static string Version = "Alpha-Issue183";
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
        this.mainCamera = c;
    }

    public void addCT(COLOSSAL_TITAN titan)
    {
        this.cT.Add(titan);
    }

    public void addET(TITAN_EREN hero)
    {
        this.eT.Add(hero);
    }

    public void addFT(FEMALE_TITAN titan)
    {
        this.fT.Add(titan);
    }

    public void addHero(Hero hero)
    {
        this.heroes.Add(hero);
    }

    public void addHook(Bullet h)
    {
        this.hooks.Add(h);
    }

    public void addTime(float time)
    {
        this.timeTotalServer -= time;
    }

    public void addTitan(MindlessTitan titan)
    {
        this.titans.Add(titan);
    }

    private void cache()
    {
        ClothFactory.ClearClothCache();
        this.inputManager = GameObject.Find("InputManagerController").GetComponent<FengCustomInputs>();
        //HACK
        //this.chatRoom = GameObject.Find("Chatroom").GetComponent<InRoomChat>();
        this.playersRPC.Clear();
        this.titanSpawners.Clear();
        this.groundList.Clear();
        this.PreservedPlayerKDR = new Dictionary<string, int[]>();
        noRestart = false;
        skyMaterial = null;
        this.isSpawning = false;
        this.retryTime = 0f;
        logicLoaded = false;
        customLevelLoaded = true;
        this.isUnloading = false;
        this.isRecompiling = false;
        Time.timeScale = 1f;
        Camera.main.farClipPlane = 1500f;
        this.pauseWaitTime = 0f;
        this.spectateSprites = new List<GameObject>();
        this.isRestarting = false;
        if (PhotonNetwork.isMasterClient)
        {
            base.StartCoroutine(this.WaitAndResetRestarts());
        }
        if (IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.SINGLE)
        {
            this.roundTime = 0f;
            if (Level.Name.StartsWith("Custom"))
            {
                customLevelLoaded = false;
            }
            if (PhotonNetwork.isMasterClient)
            {
                if (this.isFirstLoad)
                {
                    this.setGameSettings(this.checkGameGUI());
                }
            }
            if (((int)settings[0xf4]) == 1)
            {
                this.chatRoom.addLINE("<color=#FFC000>(" + this.roundTime.ToString("F2") + ")</color> Round Start.");
            }
        }
        this.isFirstLoad = false;
        this.RecompilePlayerList(0.5f);
    }

    [PunRPC]
    private void Chat(string content, string sender, PhotonMessageInfo info)
    {
        if (sender != string.Empty)
        {
            content = sender + ":" + content;
        }
        content = "<color=#FFC000>[" + Convert.ToString(info.sender.ID) + "]</color> " + content;
        this.chatRoom.addLINE(content);
    }

    [PunRPC]
    private void ChatPM(string sender, string content, PhotonMessageInfo info)
    {
        content = sender + ":" + content;
        content = "<color=#FFC000>FROM [" + Convert.ToString(info.sender.ID) + "]</color> " + content;
        this.chatRoom.addLINE(content);
    }

    private ExitGames.Client.Photon.Hashtable checkGameGUI()
    {
        int num;
        int num2;
        PhotonPlayer player;
        int num4;
        float num8;
        float num9;
        ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
        return hashtable;
    }

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

    private IEnumerator clearlevelE(string[] skybox)
    {
        string key = skybox[6];
        bool mipmap = true;
        bool iteratorVariable2 = false;
        if (((int)settings[0x3f]) == 1)
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
                Camera.main.GetComponent<Skybox>().material = (Material)linkHash[1][iteratorVariable3];
                skyMaterial = (Material)linkHash[1][iteratorVariable3];
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
                                iteratorVariable24.material = (Material)linkHash[0][key];
                            }
                            else
                            {
                                iteratorVariable24.material = (Material)linkHash[0][key];
                            }
                        }
                        else
                        {
                            iteratorVariable24.material = (Material)linkHash[0][key];
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
        if (((int)settings[0x40]) >= 100)
        {
            this.coreeditor();
        }
        else
        {
            if ((IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.SINGLE) && this.needChooseSide)
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
                    this.coreadd();
                    this.ShowHUDInfoTopLeft(this.playerList);
                    if ((((Camera.main != null) && (Gamemode.Settings.GamemodeType != GamemodeType.Racing)) && (Camera.main.GetComponent<IN_GAME_MAIN_CAMERA>().gameOver && !this.needChooseSide)) && (((int)settings[0xf5]) == 0))
                    {
                        this.ShowHUDInfoCenter("Press [F7D358]" + this.inputManager.inputString[InputCode.flare1] + "[-] to spectate the next player. \nPress [F7D358]" + this.inputManager.inputString[InputCode.flare2] + "[-] to spectate the previous player.\nPress [F7D358]" + this.inputManager.inputString[InputCode.attack1] + "[-] to enter the spectator mode.\n\n\n\n");
                        if (((Gamemode.Settings.RespawnMode == RespawnMode.DEATHMATCH) || (Gamemode.Settings.EndlessRevive > 0)) || !(((Gamemode.Settings.PvPBomb) || (Gamemode.Settings.Pvp != PvpMode.Disabled)) ? (Gamemode.Settings.PointMode <= 0) : true))
                        {
                            this.myRespawnTime += Time.deltaTime;
                            int endlessMode = 5;
                            if (RCextensions.returnIntFromObject(PhotonNetwork.player.CustomProperties[PhotonPlayerProperty.isTitan]) == 2)
                            {
                                endlessMode = 10;
                            }
                            if (Gamemode.Settings.EndlessRevive > 0)
                            {
                                endlessMode = Gamemode.Settings.EndlessRevive;
                            }
                            length = endlessMode - ((int)this.myRespawnTime);
                            this.ShowHUDInfoCenterADD("Respawn in " + length.ToString() + "s.");
                            if (this.myRespawnTime > endlessMode)
                            {
                                this.myRespawnTime = 0f;
                                Camera.main.GetComponent<IN_GAME_MAIN_CAMERA>().gameOver = false;
                                if (RCextensions.returnIntFromObject(PhotonNetwork.player.CustomProperties[PhotonPlayerProperty.isTitan]) == 2)
                                {
                                    SpawnPlayerTitan();
                                }
                                else
                                {
                                    base.StartCoroutine(this.WaitAndRespawn1(0.1f, this.myLastRespawnTag));
                                }
                                Camera.main.GetComponent<IN_GAME_MAIN_CAMERA>().gameOver = false;
                                this.ShowHUDInfoCenter(string.Empty);
                            }
                        }
                    }
                }
                else if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
                {
                    if (Gamemode.Settings.GamemodeType == GamemodeType.Racing)
                    {
                        if (!this.isLosing)
                        {
                            this.currentSpeed = Camera.main.GetComponent<IN_GAME_MAIN_CAMERA>().main_object.GetComponent<Rigidbody>().velocity.magnitude;
                            this.maxSpeed = Mathf.Max(this.maxSpeed, this.currentSpeed);
                            this.ShowHUDInfoTopLeft(string.Concat(new object[] { "Current Speed : ", (int)this.currentSpeed, "\nMax Speed:", this.maxSpeed }));
                        }
                    }
                    else
                    {
                        this.ShowHUDInfoTopLeft(string.Concat(new object[] { "Kills:", this.single_kills, "\nMax Damage:", this.single_maxDamage, "\nTotal Damage:", this.single_totalDamage }));
                    }
                }
                if (this.isLosing && (Gamemode.Settings.GamemodeType != GamemodeType.Racing))
                {
                    ShowHUDInfoCenter(Gamemode.GetDefeatMessage(gameEndCD));
                    if (IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.SINGLE)
                    {
                        if (this.gameEndCD <= 0f)
                        {
                            this.gameEndCD = 0f;
                            if (PhotonNetwork.isMasterClient)
                            {
                                this.restartRC();
                            }

                            this.ShowHUDInfoCenter(string.Empty);
                        }
                        else
                        {
                            this.gameEndCD -= Time.deltaTime;
                        }
                    }
                }
                if (this.isWinning)
                {
                    ShowHUDInfoCenter(Gamemode.GetVictoryMessage(gameEndCD, timeTotalServer));
                    if (this.gameEndCD <= 0f)
                    {
                        this.gameEndCD = 0f;
                        if (PhotonNetwork.isMasterClient)
                        {
                            this.restartRC();
                        }
                        this.ShowHUDInfoCenter(string.Empty);
                    }
                    else
                    {
                        this.gameEndCD -= Time.deltaTime;
                    }

                }
                this.timeElapse += Time.deltaTime;
                this.roundTime += Time.deltaTime;
                if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
                {
                    //TODO Investigate the purpose of this
                    if (Gamemode.Settings.GamemodeType == GamemodeType.Racing)
                    {
                        if (!this.isWinning)
                        {
                            this.timeTotalServer += Time.deltaTime;
                        }
                    }
                    else if (!(this.isLosing || this.isWinning))
                    {
                        this.timeTotalServer += Time.deltaTime;
                    }
                }
                else
                {
                    this.timeTotalServer += Time.deltaTime;
                }
                if (Gamemode.Settings.GamemodeType == GamemodeType.Racing)
                {
                    if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
                    {
                        if (!this.isWinning)
                        {
                            this.ShowHUDInfoTopCenter("Time : " + ((((int)(this.timeTotalServer * 10f)) * 0.1f) - 5f));
                        }
                        if (this.timeTotalServer < 5f)
                        {
                            this.ShowHUDInfoCenter("RACE START IN " + ((int)(5f - this.timeTotalServer)));
                        }
                        else if (!this.startRacing)
                        {
                            this.ShowHUDInfoCenter(string.Empty);
                            this.startRacing = true;
                            this.endRacing = false;
                            GameObject.Find("door").SetActive(false);
                        }
                    }
                    else
                    {
                        this.ShowHUDInfoTopCenter("Time : " + ((this.roundTime >= 20f) ? (num3 = (((int)(this.roundTime * 10f)) * 0.1f) - 20f).ToString() : "WAITING"));
                        if (this.roundTime < 20f)
                        {
                            this.ShowHUDInfoCenter("RACE START IN " + ((int)(20f - this.roundTime)) + (!(this.localRacingResult == string.Empty) ? ("\nLast Round\n" + this.localRacingResult) : "\n\n"));
                        }
                        else if (!this.startRacing)
                        {
                            this.ShowHUDInfoCenter(string.Empty);
                            this.startRacing = true;
                            this.endRacing = false;
                            GameObject obj2 = GameObject.Find("door");
                            if (obj2 != null)
                            {
                                obj2.SetActive(false);
                            }
                            if ((this.racingDoors != null) && customLevelLoaded)
                            {
                                foreach (GameObject obj3 in this.racingDoors)
                                {
                                    obj3.SetActive(false);
                                }
                                this.racingDoors = null;
                            }
                        }
                        else if ((this.racingDoors != null) && customLevelLoaded)
                        {
                            foreach (GameObject obj3 in this.racingDoors)
                            {
                                obj3.SetActive(false);
                            }
                            this.racingDoors = null;
                        }
                    }
                    if ((Camera.main.GetComponent<IN_GAME_MAIN_CAMERA>().gameOver && !this.needChooseSide) && customLevelLoaded)
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
                            this.ShowHUDInfoCenter(string.Empty);
                        }
                    }
                }
                if (this.timeElapse > 1f)
                {
                    this.timeElapse--;
                    var content = Gamemode.GetGamemodeStatusTop((int)timeTotalServer, time);
                    if (Gamemode.Settings.TeamMode != TeamMode.Disabled)
                    {
                        content += $"\n<color=#00ffff>Cyan: {cyanKills}</color><color=#ff00ff>       Magenta: {magentaKills}</color>";
                    }
                    this.ShowHUDInfoTopCenter(content);
                    content = Gamemode.GetGamemodeStatusTopRight((int)timeTotalServer, time);
                    this.ShowHUDInfoTopRight(content);
                    string str4 = (IN_GAME_MAIN_CAMERA.difficulty >= 0) ? ((IN_GAME_MAIN_CAMERA.difficulty != 0) ? ((IN_GAME_MAIN_CAMERA.difficulty != 1) ? "Abnormal" : "Hard") : "Normal") : "Trainning";
                    this.ShowHUDInfoTopRightMAPNAME("\n" + Level.Name + " : " + str4);
                    if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER)
                    {
                        char[] separator = new char[] { "`"[0] };
                        string str5 = PhotonNetwork.room.name.Split(separator)[0];
                        if (str5.Length > 20)
                        {
                            str5 = str5.Remove(0x13) + "...";
                        }
                        this.ShowHUDInfoTopRightMAPNAME("\n" + str5 + " [FFC000](" + Convert.ToString(PhotonNetwork.room.playerCount) + "/" + Convert.ToString(PhotonNetwork.room.maxPlayers) + ")");
                        if (this.needChooseSide)
                        {
                            this.ShowHUDInfoTopCenterADD("\n\nPRESS 1 TO ENTER GAME");
                        }
                    }
                }
                if (((IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER) && (this.killInfoGO.Count > 0)) && (this.killInfoGO[0] == null))
                {
                    this.killInfoGO.RemoveAt(0);
                }
                if (((IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.SINGLE) && PhotonNetwork.isMasterClient) && (this.timeTotalServer > this.time))
                {
                    string str11;
                    IN_GAME_MAIN_CAMERA.gametype = GAMETYPE.STOP;
                    this.gameStart = false;
                    Screen.lockCursor = false;
                    Cursor.visible = true;
                    string str6 = string.Empty;
                    string str7 = string.Empty;
                    string str8 = string.Empty;
                    string str9 = string.Empty;
                    string str10 = string.Empty;
                    foreach (PhotonPlayer player in PhotonNetwork.playerList)
                    {
                        if (player != null)
                        {
                            str6 = str6 + player.CustomProperties[PhotonPlayerProperty.name] + "\n";
                            str7 = str7 + player.CustomProperties[PhotonPlayerProperty.kills] + "\n";
                            str8 = str8 + player.CustomProperties[PhotonPlayerProperty.deaths] + "\n";
                            str9 = str9 + player.CustomProperties[PhotonPlayerProperty.max_dmg] + "\n";
                            str10 = str10 + player.CustomProperties[PhotonPlayerProperty.total_dmg] + "\n";
                        }
                    }

                    str11 = Gamemode.GetRoundEndedMessage();
                    object[] parameters = new object[] { str6, str7, str8, str9, str10, str11 };
                    base.photonView.RPC("showResult", PhotonTargets.AllBuffered, parameters);
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
                for (int i = 0; i < this.titanSpawners.Count; i++)
                {
                    TitanSpawner item = this.titanSpawners[i];
                    item.time -= Time.deltaTime;
                    if ((item.time <= 0f) && ((this.titans.Count + this.fT.Count) < Gamemode.Settings.TitanLimit))
                    {
                        string name = item.name;
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
            if (this.pauseWaitTime <= 3f)
            {
                this.pauseWaitTime -= Time.deltaTime * 1000000f;
                if (this.pauseWaitTime <= 1f)
                {
                    Camera.main.farClipPlane = 1500f;
                }
                if (this.pauseWaitTime <= 0f)
                {
                    this.pauseWaitTime = 0f;
                    Time.timeScale = 1f;
                }
            }
            this.ReloadPlayerlist();
        }
    }

    private void coreeditor()
    {
        if (Input.GetKey(KeyCode.Tab))
        {
            GUI.FocusControl(null);
        }
        if (this.selectedObj != null)
        {
            float num = 0.2f;
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
                Transform transform = this.selectedObj.transform;
                transform.position += (Vector3)(num * new Vector3(Camera.main.transform.forward.x, 0f, Camera.main.transform.forward.z));
            }
            else if (inputRC.isInputLevel(InputCodeRC.levelBack))
            {
                Transform transform9 = this.selectedObj.transform;
                transform9.position -= (Vector3)(num * new Vector3(Camera.main.transform.forward.x, 0f, Camera.main.transform.forward.z));
            }
            if (inputRC.isInputLevel(InputCodeRC.levelLeft))
            {
                Transform transform10 = this.selectedObj.transform;
                transform10.position -= (Vector3)(num * new Vector3(Camera.main.transform.right.x, 0f, Camera.main.transform.right.z));
            }
            else if (inputRC.isInputLevel(InputCodeRC.levelRight))
            {
                Transform transform11 = this.selectedObj.transform;
                transform11.position += (Vector3)(num * new Vector3(Camera.main.transform.right.x, 0f, Camera.main.transform.right.z));
            }
            if (inputRC.isInputLevel(InputCodeRC.levelDown))
            {
                Transform transform12 = this.selectedObj.transform;
                transform12.position -= (Vector3)(Vector3.up * num);
            }
            else if (inputRC.isInputLevel(InputCodeRC.levelUp))
            {
                Transform transform13 = this.selectedObj.transform;
                transform13.position += (Vector3)(Vector3.up * num);
            }
            if (!this.selectedObj.name.StartsWith("misc,region"))
            {
                if (inputRC.isInputLevel(InputCodeRC.levelRRight))
                {
                    this.selectedObj.transform.Rotate((Vector3)(Vector3.up * num));
                }
                else if (inputRC.isInputLevel(InputCodeRC.levelRLeft))
                {
                    this.selectedObj.transform.Rotate((Vector3)(Vector3.down * num));
                }
                if (inputRC.isInputLevel(InputCodeRC.levelRCCW))
                {
                    this.selectedObj.transform.Rotate((Vector3)(Vector3.forward * num));
                }
                else if (inputRC.isInputLevel(InputCodeRC.levelRCW))
                {
                    this.selectedObj.transform.Rotate((Vector3)(Vector3.back * num));
                }
                if (inputRC.isInputLevel(InputCodeRC.levelRBack))
                {
                    this.selectedObj.transform.Rotate((Vector3)(Vector3.left * num));
                }
                else if (inputRC.isInputLevel(InputCodeRC.levelRForward))
                {
                    this.selectedObj.transform.Rotate((Vector3)(Vector3.right * num));
                }
            }
            if (inputRC.isInputLevel(InputCodeRC.levelPlace))
            {
                linkHash[3].Add(this.selectedObj.GetInstanceID(), this.selectedObj.name + "," + Convert.ToString(this.selectedObj.transform.position.x) + "," + Convert.ToString(this.selectedObj.transform.position.y) + "," + Convert.ToString(this.selectedObj.transform.position.z) + "," + Convert.ToString(this.selectedObj.transform.rotation.x) + "," + Convert.ToString(this.selectedObj.transform.rotation.y) + "," + Convert.ToString(this.selectedObj.transform.rotation.z) + "," + Convert.ToString(this.selectedObj.transform.rotation.w));
                this.selectedObj = null;
                //TODO Mouselook
                //Camera.main.GetComponent<MouseLook>().enabled = true;
                Screen.lockCursor = true;
            }
            if (inputRC.isInputLevel(InputCodeRC.levelDelete))
            {
                UnityEngine.Object.Destroy(this.selectedObj);
                this.selectedObj = null;
                //TODO Mouselook
                //Camera.main.GetComponent<MouseLook>().enabled = true;
                Screen.lockCursor = true;
                linkHash[3].Remove(this.selectedObj.GetInstanceID());
            }
        }
        else
        {
            if (Screen.lockCursor)
            {
                float num2 = 100f;
                if (inputRC.isInputLevel(InputCodeRC.levelSlow))
                {
                    num2 = 20f;
                }
                else if (inputRC.isInputLevel(InputCodeRC.levelFast))
                {
                    num2 = 400f;
                }
                Transform transform7 = Camera.main.transform;
                if (inputRC.isInputLevel(InputCodeRC.levelForward))
                {
                    transform7.position += (Vector3)((transform7.forward * num2) * Time.deltaTime);
                }
                else if (inputRC.isInputLevel(InputCodeRC.levelBack))
                {
                    transform7.position -= (Vector3)((transform7.forward * num2) * Time.deltaTime);
                }
                if (inputRC.isInputLevel(InputCodeRC.levelLeft))
                {
                    transform7.position -= (Vector3)((transform7.right * num2) * Time.deltaTime);
                }
                else if (inputRC.isInputLevel(InputCodeRC.levelRight))
                {
                    transform7.position += (Vector3)((transform7.right * num2) * Time.deltaTime);
                }
                if (inputRC.isInputLevel(InputCodeRC.levelUp))
                {
                    transform7.position += (Vector3)((transform7.up * num2) * Time.deltaTime);
                }
                else if (inputRC.isInputLevel(InputCodeRC.levelDown))
                {
                    transform7.position -= (Vector3)((transform7.up * num2) * Time.deltaTime);
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
                RaycastHit hitInfo = new RaycastHit();
                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo))
                {
                    Transform transform8 = hitInfo.transform;
                    if ((((transform8.gameObject.name.StartsWith("custom") || transform8.gameObject.name.StartsWith("base")) || (transform8.gameObject.name.StartsWith("racing") || transform8.gameObject.name.StartsWith("photon"))) || transform8.gameObject.name.StartsWith("spawnpoint")) || transform8.gameObject.name.StartsWith("misc"))
                    {
                        this.selectedObj = transform8.gameObject;
                        //TODO Mouselook
                        //Camera.main.GetComponent<MouseLook>().enabled = false;
                        Screen.lockCursor = true;
                        linkHash[3].Remove(this.selectedObj.GetInstanceID());
                    }
                    else if (((transform8.parent.gameObject.name.StartsWith("custom") || transform8.parent.gameObject.name.StartsWith("base")) || transform8.parent.gameObject.name.StartsWith("racing")) || transform8.parent.gameObject.name.StartsWith("photon"))
                    {
                        this.selectedObj = transform8.parent.gameObject;
                        //TODO Mouselook
                        //Camera.main.GetComponent<MouseLook>().enabled = false;
                        Screen.lockCursor = true;
                        linkHash[3].Remove(this.selectedObj.GetInstanceID());
                    }
                }
            }
        }
    }

    private IEnumerator customlevelcache()
    {
        int iteratorVariable0 = 0;
        while (true)
        {
            if (iteratorVariable0 >= this.levelCache.Count)
            {
                yield break;
            }
            this.customlevelclientE(this.levelCache[iteratorVariable0], false);
            yield return new WaitForEndOfFrame();
            iteratorVariable0++;
        }
    }

    private void customlevelclientE(string[] content, bool renewHash)
    {
        int num;
        string[] strArray;
        bool flag = false;
        bool flag2 = false;
        if (content[content.Length - 1].StartsWith("a"))
        {
            flag = true;
        }
        else if (content[content.Length - 1].StartsWith("z"))
        {
            flag2 = true;
            customLevelLoaded = true;
            this.spawnPlayerCustomMap();
            this.unloadAssets();
            //TODO TiltShift
            //Camera.main.GetComponent<TiltShift>().enabled = false;
        }
        if (renewHash)
        {
            if (flag)
            {
                currentLevel = string.Empty;
                this.levelCache.Clear();
                this.titanSpawns.Clear();
                this.playerSpawnsC.Clear();
                this.playerSpawnsM.Clear();
                for (num = 0; num < content.Length; num++)
                {
                    strArray = content[num].Split(new char[] { ',' });
                    if (strArray[0] == "titan")
                    {
                        this.titanSpawns.Add(new Vector3(Convert.ToSingle(strArray[1]), Convert.ToSingle(strArray[2]), Convert.ToSingle(strArray[3])));
                    }
                    else if (strArray[0] == "playerC")
                    {
                        this.playerSpawnsC.Add(new Vector3(Convert.ToSingle(strArray[1]), Convert.ToSingle(strArray[2]), Convert.ToSingle(strArray[3])));
                    }
                    else if (strArray[0] == "playerM")
                    {
                        this.playerSpawnsM.Add(new Vector3(Convert.ToSingle(strArray[1]), Convert.ToSingle(strArray[2]), Convert.ToSingle(strArray[3])));
                    }
                }
                this.spawnPlayerCustomMap();
            }
            currentLevel = currentLevel + content[content.Length - 1];
            this.levelCache.Add(content);
            ExitGames.Client.Photon.Hashtable propertiesToSet = new ExitGames.Client.Photon.Hashtable();
            propertiesToSet.Add(PhotonPlayerProperty.currentLevel, currentLevel);
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
                    obj2 = (GameObject)UnityEngine.Object.Instantiate((GameObject)RCassets.LoadAsset(strArray[1]), new Vector3(Convert.ToSingle(strArray[12]), Convert.ToSingle(strArray[13]), Convert.ToSingle(strArray[14])), new Quaternion(Convert.ToSingle(strArray[15]), Convert.ToSingle(strArray[0x10]), Convert.ToSingle(strArray[0x11]), Convert.ToSingle(strArray[0x12])));
                    if (strArray[2] != "default")
                    {
                        if (strArray[2].StartsWith("transparent"))
                        {
                            if (float.TryParse(strArray[2].Substring(11), out num3))
                            {
                                num2 = num3;
                            }
                            foreach (Renderer renderer in obj2.GetComponentsInChildren<Renderer>())
                            {
                                renderer.material = (Material)RCassets.LoadAsset("transparent");
                                if ((Convert.ToSingle(strArray[10]) != 1f) || (Convert.ToSingle(strArray[11]) != 1f))
                                {
                                    renderer.material.mainTextureScale = new Vector2(renderer.material.mainTextureScale.x * Convert.ToSingle(strArray[10]), renderer.material.mainTextureScale.y * Convert.ToSingle(strArray[11]));
                                }
                            }
                        }
                        else
                        {
                            foreach (Renderer renderer in obj2.GetComponentsInChildren<Renderer>())
                            {
                                renderer.material = (Material)RCassets.LoadAsset(strArray[2]);
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
                        foreach (MeshFilter filter in obj2.GetComponentsInChildren<MeshFilter>())
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
                        UnityEngine.Object.Instantiate(Resources.Load(strArray[1]), new Vector3(Convert.ToSingle(strArray[2]), Convert.ToSingle(strArray[3]), Convert.ToSingle(strArray[4])), new Quaternion(Convert.ToSingle(strArray[5]), Convert.ToSingle(strArray[6]), Convert.ToSingle(strArray[7]), Convert.ToSingle(strArray[8])));
                    }
                    else
                    {
                        num2 = 1f;
                        obj2 = null;
                        obj2 = (GameObject)UnityEngine.Object.Instantiate((GameObject)Resources.Load(strArray[1]), new Vector3(Convert.ToSingle(strArray[12]), Convert.ToSingle(strArray[13]), Convert.ToSingle(strArray[14])), new Quaternion(Convert.ToSingle(strArray[15]), Convert.ToSingle(strArray[0x10]), Convert.ToSingle(strArray[0x11]), Convert.ToSingle(strArray[0x12])));
                        if (strArray[2] != "default")
                        {
                            if (strArray[2].StartsWith("transparent"))
                            {
                                if (float.TryParse(strArray[2].Substring(11), out num3))
                                {
                                    num2 = num3;
                                }
                                foreach (Renderer renderer in obj2.GetComponentsInChildren<Renderer>())
                                {
                                    renderer.material = (Material)RCassets.LoadAsset("transparent");
                                    if ((Convert.ToSingle(strArray[10]) != 1f) || (Convert.ToSingle(strArray[11]) != 1f))
                                    {
                                        renderer.material.mainTextureScale = new Vector2(renderer.material.mainTextureScale.x * Convert.ToSingle(strArray[10]), renderer.material.mainTextureScale.y * Convert.ToSingle(strArray[11]));
                                    }
                                }
                            }
                            else
                            {
                                foreach (Renderer renderer in obj2.GetComponentsInChildren<Renderer>())
                                {
                                    if (!(renderer.name.Contains("Particle System") && obj2.name.Contains("aot_supply")))
                                    {
                                        renderer.material = (Material)RCassets.LoadAsset(strArray[2]);
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
                            foreach (MeshFilter filter in obj2.GetComponentsInChildren<MeshFilter>())
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
                        obj2 = (GameObject)UnityEngine.Object.Instantiate((GameObject)RCassets.LoadAsset(strArray[1]), new Vector3(Convert.ToSingle(strArray[5]), Convert.ToSingle(strArray[6]), Convert.ToSingle(strArray[7])), new Quaternion(Convert.ToSingle(strArray[8]), Convert.ToSingle(strArray[9]), Convert.ToSingle(strArray[10]), Convert.ToSingle(strArray[11])));
                        num5 = obj2.transform.localScale.x * Convert.ToSingle(strArray[2]);
                        num5 -= 0.001f;
                        num6 = obj2.transform.localScale.y * Convert.ToSingle(strArray[3]);
                        num7 = obj2.transform.localScale.z * Convert.ToSingle(strArray[4]);
                        obj2.transform.localScale = new Vector3(num5, num6, num7);
                    }
                    else if (strArray[1].StartsWith("racingStart"))
                    {
                        obj2 = null;
                        obj2 = (GameObject)UnityEngine.Object.Instantiate((GameObject)RCassets.LoadAsset(strArray[1]), new Vector3(Convert.ToSingle(strArray[5]), Convert.ToSingle(strArray[6]), Convert.ToSingle(strArray[7])), new Quaternion(Convert.ToSingle(strArray[8]), Convert.ToSingle(strArray[9]), Convert.ToSingle(strArray[10]), Convert.ToSingle(strArray[11])));
                        num5 = obj2.transform.localScale.x * Convert.ToSingle(strArray[2]);
                        num5 -= 0.001f;
                        num6 = obj2.transform.localScale.y * Convert.ToSingle(strArray[3]);
                        num7 = obj2.transform.localScale.z * Convert.ToSingle(strArray[4]);
                        obj2.transform.localScale = new Vector3(num5, num6, num7);
                        if (this.racingDoors != null)
                        {
                            this.racingDoors.Add(obj2);
                        }
                    }
                    else if (strArray[1].StartsWith("racingEnd"))
                    {
                        obj2 = null;
                        obj2 = (GameObject)UnityEngine.Object.Instantiate((GameObject)RCassets.LoadAsset(strArray[1]), new Vector3(Convert.ToSingle(strArray[5]), Convert.ToSingle(strArray[6]), Convert.ToSingle(strArray[7])), new Quaternion(Convert.ToSingle(strArray[8]), Convert.ToSingle(strArray[9]), Convert.ToSingle(strArray[10]), Convert.ToSingle(strArray[11])));
                        num5 = obj2.transform.localScale.x * Convert.ToSingle(strArray[2]);
                        num5 -= 0.001f;
                        num6 = obj2.transform.localScale.y * Convert.ToSingle(strArray[3]);
                        num7 = obj2.transform.localScale.z * Convert.ToSingle(strArray[4]);
                        obj2.transform.localScale = new Vector3(num5, num6, num7);
                        obj2.AddComponent<LevelTriggerRacingEnd>();
                    }
                    else if (strArray[1].StartsWith("region") && PhotonNetwork.isMasterClient)
                    {
                        Vector3 loc = new Vector3(Convert.ToSingle(strArray[6]), Convert.ToSingle(strArray[7]), Convert.ToSingle(strArray[8]));
                        RCRegion region = new RCRegion(loc, Convert.ToSingle(strArray[3]), Convert.ToSingle(strArray[4]), Convert.ToSingle(strArray[5]));
                        string key = strArray[2];
                        if (RCRegionTriggers.ContainsKey(key))
                        {
                            GameObject obj3 = (GameObject)UnityEngine.Object.Instantiate((GameObject)RCassets.LoadAsset("region"));
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
                        obj2 = (GameObject)UnityEngine.Object.Instantiate((GameObject)RCassets.LoadAsset(strArray[1]), new Vector3(Convert.ToSingle(strArray[5]), Convert.ToSingle(strArray[6]), Convert.ToSingle(strArray[7])), new Quaternion(Convert.ToSingle(strArray[8]), Convert.ToSingle(strArray[9]), Convert.ToSingle(strArray[10]), Convert.ToSingle(strArray[11])));
                        num5 = obj2.transform.localScale.x * Convert.ToSingle(strArray[2]);
                        num5 -= 0.001f;
                        num6 = obj2.transform.localScale.y * Convert.ToSingle(strArray[3]);
                        num7 = obj2.transform.localScale.z * Convert.ToSingle(strArray[4]);
                        obj2.transform.localScale = new Vector3(num5, num6, num7);
                        if (this.racingDoors != null)
                        {
                            this.racingDoors.Add(obj2);
                        }
                    }
                    else if (strArray[1].StartsWith("end"))
                    {
                        obj2 = null;
                        obj2 = (GameObject)UnityEngine.Object.Instantiate((GameObject)RCassets.LoadAsset(strArray[1]), new Vector3(Convert.ToSingle(strArray[5]), Convert.ToSingle(strArray[6]), Convert.ToSingle(strArray[7])), new Quaternion(Convert.ToSingle(strArray[8]), Convert.ToSingle(strArray[9]), Convert.ToSingle(strArray[10]), Convert.ToSingle(strArray[11])));
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
                        obj2 = (GameObject)UnityEngine.Object.Instantiate((GameObject)RCassets.LoadAsset(strArray[1]), new Vector3(Convert.ToSingle(strArray[5]), Convert.ToSingle(strArray[6]), Convert.ToSingle(strArray[7])), new Quaternion(Convert.ToSingle(strArray[8]), Convert.ToSingle(strArray[9]), Convert.ToSingle(strArray[10]), Convert.ToSingle(strArray[11])));
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
                        obj2 = (GameObject)UnityEngine.Object.Instantiate((GameObject)RCassets.LoadAsset(strArray[1]), new Vector3(Convert.ToSingle(strArray[5]), Convert.ToSingle(strArray[6]), Convert.ToSingle(strArray[7])), new Quaternion(Convert.ToSingle(strArray[8]), Convert.ToSingle(strArray[9]), Convert.ToSingle(strArray[10]), Convert.ToSingle(strArray[11])));
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
                        UnityEngine.Object.Destroy(GameObject.Find("gameobjectOutSide"));
                        UnityEngine.Object.Instantiate(RCassets.LoadAsset("outside"));
                    }
                }
                else if (PhotonNetwork.isMasterClient && strArray[0].StartsWith("photon"))
                {
                    if (strArray[1].StartsWith("Cannon"))
                    {
                        if (strArray.Length > 15)
                        {
                            GameObject go = PhotonNetwork.Instantiate("RCAsset/" + strArray[1] + "Prop", new Vector3(Convert.ToSingle(strArray[12]), Convert.ToSingle(strArray[13]), Convert.ToSingle(strArray[14])), new Quaternion(Convert.ToSingle(strArray[15]), Convert.ToSingle(strArray[0x10]), Convert.ToSingle(strArray[0x11]), Convert.ToSingle(strArray[0x12])), 0);
                            go.GetComponent<CannonPropRegion>().settings = content[num];
                            go.GetPhotonView().RPC("SetSize", PhotonTargets.AllBuffered, new object[] { content[num] });
                        }
                        else
                        {
                            PhotonNetwork.Instantiate("RCAsset/" + strArray[1] + "Prop", new Vector3(Convert.ToSingle(strArray[2]), Convert.ToSingle(strArray[3]), Convert.ToSingle(strArray[4])), new Quaternion(Convert.ToSingle(strArray[5]), Convert.ToSingle(strArray[6]), Convert.ToSingle(strArray[7]), Convert.ToSingle(strArray[8])), 0).GetComponent<CannonPropRegion>().settings = content[num];
                        }
                    }
                    else
                    {
                        TitanSpawner item = new TitanSpawner();
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
                        this.titanSpawners.Add(item);
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
            for (int i = 0; i < this.levelCache.Count; i++)
            {
                foreach (PhotonPlayer player in players)
                {
                    if (((player.CustomProperties[PhotonPlayerProperty.currentLevel] != null) && (currentLevel != string.Empty)) && (RCextensions.returnStringFromObject(player.CustomProperties[PhotonPlayerProperty.currentLevel]) == currentLevel))
                    {
                        if (i == 0)
                        {
                            strArray = new string[] { "loadcached" };
                            this.photonView.RPC("customlevelRPC", player, new object[] { strArray });
                        }
                    }
                    else
                    {
                        this.photonView.RPC("customlevelRPC", player, new object[] { this.levelCache[i] });
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
            foreach (PhotonPlayer player in players)
            {
                this.photonView.RPC("customlevelRPC", player, new object[] { strArray });
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
                base.StartCoroutine(this.customlevelcache());
            }
            else if ((content.Length == 1) && (content[0] == "loadempty"))
            {
                currentLevel = string.Empty;
                this.levelCache.Clear();
                this.titanSpawns.Clear();
                this.playerSpawnsC.Clear();
                this.playerSpawnsM.Clear();
                ExitGames.Client.Photon.Hashtable propertiesToSet = new ExitGames.Client.Photon.Hashtable();
                propertiesToSet.Add(PhotonPlayerProperty.currentLevel, currentLevel);
                PhotonNetwork.player.SetCustomProperties(propertiesToSet);
                customLevelLoaded = true;
                this.spawnPlayerCustomMap();
            }
            else
            {
                this.customlevelclientE(content, true);
            }
        }
    }

    public void debugChat(string str)
    {
        this.chatRoom.addLINE(str);
    }

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

    private void endGameRC()
    {
    }

    public void EnterSpecMode(bool enter)
    {
        if (enter)
        {
            this.spectateSprites = new List<GameObject>();
            foreach (GameObject obj2 in UnityEngine.Object.FindObjectsOfType(typeof(GameObject)))
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
            string[] strArray2 = new string[] { "Flare", "LabelInfoBottomRight" };
            foreach (string str2 in strArray2)
            {
                GameObject item = GameObject.Find(str2);
                if (item != null)
                {
                    if (!this.spectateSprites.Contains(item))
                    {
                        this.spectateSprites.Add(item);
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
            GameObject obj4 = GameObject.FindGameObjectWithTag("Player");
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
            base.StartCoroutine(this.reloadSky());
        }
        else
        {
            if (GameObject.Find("cross1") != null)
            {
                GameObject.Find("cross1").transform.localPosition = (Vector3)(Vector3.up * 5000f);
            }
            if (this.spectateSprites != null)
            {
                foreach (GameObject obj2 in this.spectateSprites)
                {
                    if (obj2 != null)
                    {
                        obj2.SetActive(true);
                    }
                }
            }
            this.spectateSprites = new List<GameObject>();
            instance.needChooseSide = true;
            Camera.main.GetComponent<IN_GAME_MAIN_CAMERA>().setMainObject(null, true, false);
            Camera.main.GetComponent<IN_GAME_MAIN_CAMERA>().setSpectorMode(true);
            Camera.main.GetComponent<IN_GAME_MAIN_CAMERA>().gameOver = true;
        }
    }

    public void gameLose2()
    {
        if (!(this.isWinning || this.isLosing))
        {
            EventManager.OnGameLost.Invoke();
            this.isLosing = true;
            this.gameEndCD = this.gameEndTotalCDtime;
        }
    }

    public void gameWin2()
    {
        if (!this.isLosing && !this.isWinning)
        {
            EventManager.OnGameWon.Invoke();
            this.isWinning = true;
        }
    }

    public ArrayList getPlayers()
    {
        return this.heroes;
    }

    [PunRPC]
    private void getRacingResult(string player, float time)
    {
        RacingResult result = new RacingResult
        {
            name = player,
            time = time
        };
        this.racingResult.Add(result);
        this.refreshRacingResult2();
    }

    public ArrayList getTitans()
    {
        return this.titans;
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

    public static GameObject InstantiateCustomAsset(string key)
    {
        key = key.Substring(8);
        return (GameObject)RCassets.LoadAsset(key);
    }

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
            base.photonView.RPC("ignorePlayer", PhotonTargets.Others, new object[] { player.ID });
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
                this.chatRoom.addLINE("Player " + player.ID.ToString() + " was autobanned. Reason:" + reason);
            }
            this.RecompilePlayerList(0.1f);
        }
    }

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

    private void LateUpdate()
    {
        if (this.gameStart)
        {
            IEnumerator enumerator = this.heroes.GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    var current = (Hero)enumerator.Current;
                    if (current != null)
                        current.lateUpdate2();
                }
            }
            finally
            {
                IDisposable disposable = enumerator as IDisposable;
                if (disposable != null)
                {
                    disposable.Dispose();
                }
            }
            IEnumerator enumerator2 = this.eT.GetEnumerator();
            try
            {
                while (enumerator2.MoveNext())
                {
                    var titanEren = (TITAN_EREN)enumerator2.Current;
                    if (titanEren != null)
                        titanEren.lateUpdate();
                }
            }
            finally
            {
                IDisposable disposable2 = enumerator2 as IDisposable;
                if (disposable2 != null)
                {
                    disposable2.Dispose();
                }
            }
            IEnumerator enumerator4 = this.fT.GetEnumerator();
            try
            {
                while (enumerator4.MoveNext())
                {
                    var femaleTitan = (FEMALE_TITAN)enumerator4.Current;
                    if (femaleTitan != null)
                        femaleTitan.lateUpdate2();
                }
            }
            finally
            {
                IDisposable disposable4 = enumerator4 as IDisposable;
                if (disposable4 != null)
                {
                    disposable4.Dispose();
                }
            }
            this.core2();
        }
    }

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
        objArray[0xb5] = PlayerPrefs.GetInt("dashenable", 0);
        objArray[0xb6] = PlayerPrefs.GetString("dashkey", "RightControl");
        objArray[0xb7] = PlayerPrefs.GetInt("vsync", 0);
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
        inputRC.setInputHuman(InputCodeRC.reelin, (string)objArray[0x62]);
        inputRC.setInputHuman(InputCodeRC.reelout, (string)objArray[0x63]);
        inputRC.setInputHuman(InputCodeRC.dash, (string)objArray[0xb6]);
        inputRC.setInputHuman(InputCodeRC.mapMaximize, (string)objArray[0xe8]);
        inputRC.setInputHuman(InputCodeRC.mapToggle, (string)objArray[0xe9]);
        inputRC.setInputHuman(InputCodeRC.mapReset, (string)objArray[0xea]);
        inputRC.setInputHuman(InputCodeRC.chat, (string)objArray[0xec]);
        inputRC.setInputHuman(InputCodeRC.liveCam, (string)objArray[0x106]);
        if (!Enum.IsDefined(typeof(KeyCode), (string)objArray[0xe8]))
        {
            objArray[0xe8] = "None";
        }
        if (!Enum.IsDefined(typeof(KeyCode), (string)objArray[0xe9]))
        {
            objArray[0xe9] = "None";
        }
        if (!Enum.IsDefined(typeof(KeyCode), (string)objArray[0xea]))
        {
            objArray[0xea] = "None";
        }
        for (num = 0; num < 15; num++)
        {
            inputRC.setInputTitan(num, (string)objArray[0x65 + num]);
        }
        for (num = 0; num < 0x10; num++)
        {
            inputRC.setInputLevel(num, (string)objArray[0x75 + num]);
        }
        for (num = 0; num < 7; num++)
        {
            inputRC.setInputHorse(num, (string)objArray[0xed + num]);
        }
        for (num = 0; num < 7; num++)
        {
            inputRC.setInputCannon(num, (string)objArray[0xfe + num]);
        }
        inputRC.setInputLevel(InputCodeRC.levelFast, (string)objArray[0xa1]);
        Application.targetFrameRate = -1;
        if (int.TryParse((string)objArray[0xb8], out num2) && (num2 > 0))
        {
            Application.targetFrameRate = num2;
        }
        QualitySettings.vSyncCount = 0;
        if (((int)objArray[0xb7]) == 1)
        {
            QualitySettings.vSyncCount = 1;
        }
        AudioListener.volume = PlayerPrefs.GetFloat("vol", 1f);
        QualitySettings.masterTextureLimit = PlayerPrefs.GetInt("skinQ", 0);
        linkHash = new ExitGames.Client.Photon.Hashtable[] { new ExitGames.Client.Photon.Hashtable(), new ExitGames.Client.Photon.Hashtable(), new ExitGames.Client.Photon.Hashtable(), new ExitGames.Client.Photon.Hashtable(), new ExitGames.Client.Photon.Hashtable() };
        settings = objArray;
        this.scroll = Vector2.zero;
        this.scroll2 = Vector2.zero;
        this.distanceSlider = PlayerPrefs.GetFloat("cameraDistance", 1f);
        this.mouseSlider = PlayerPrefs.GetFloat("MouseSensitivity", 0.5f);
        this.qualitySlider = PlayerPrefs.GetFloat("GameQuality", 0f);
        this.transparencySlider = 1f;
    }

    private void loadskin()
    {
        GameObject[] objArray;
        int num;
        GameObject obj2;
        if (((int)settings[0x40]) >= 100)
        {
            string[] strArray2 = new string[] { "Flare", "LabelInfoBottomRight", "LabelNetworkStatus", "skill_cd_bottom", "GasUI" };
            objArray = (GameObject[])UnityEngine.Object.FindObjectsOfType(typeof(GameObject));
            for (num = 0; num < objArray.Length; num++)
            {
                obj2 = objArray[num];
                if ((obj2.name.Contains("TREE") || obj2.name.Contains("aot_supply")) || obj2.name.Contains("gameobjectOutSide"))
                {
                    UnityEngine.Object.Destroy(obj2);
                }
            }
            GameObject.Find("Cube_001").GetComponent<Renderer>().material.mainTexture = ((Material)RCassets.LoadAsset("grass")).mainTexture;
            UnityEngine.Object.Instantiate(RCassets.LoadAsset("spawnPlayer"), new Vector3(-10f, 1f, -10f), new Quaternion(0f, 0f, 0f, 1f));
            for (num = 0; num < strArray2.Length; num++)
            {
                string name = strArray2[num];
                GameObject obj3 = GameObject.Find(name);
                if (obj3 != null)
                {
                    UnityEngine.Object.Destroy(obj3);
                }
            }
            Camera.main.GetComponent<SpectatorMovement>().disable = true;
        }
        else
        {
            GameObject obj4;
            string[] strArray3;
            int num2;
            InstantiateTracker.instance.Dispose();
            if ((IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER) && PhotonNetwork.isMasterClient)
            {
                this.updateTime = 1f;
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
                base.photonView.RPC("setMasterRC", PhotonTargets.All, new object[0]);
            }
            logicLoaded = true;
            this.racingSpawnPoint = new Vector3(0f, 0f, 0f);
            this.racingSpawnPointSet = false;
            this.racingDoors = new List<GameObject>();
            this.allowedToCannon = new Dictionary<int, CannonValues>();
            if ((!Level.Name.StartsWith("Custom") && (((int)settings[2]) == 1)) && ((IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE) || PhotonNetwork.isMasterClient))
            {
                string url = string.Empty;
                string str3 = string.Empty;
                string n = string.Empty;
                strArray3 = new string[] { string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty };
                if (Level.SceneName.Contains("City"))
                {
                    for (num = 0x33; num < 0x3b; num++)
                    {
                        url = url + ((string)settings[num]) + ",";
                    }
                    url.TrimEnd(new char[] { ',' });
                    num2 = 0;
                    while (num2 < 250)
                    {
                        n = n + Convert.ToString((int)UnityEngine.Random.Range((float)0f, (float)8f));
                        num2++;
                    }
                    str3 = ((string)settings[0x3b]) + "," + ((string)settings[60]) + "," + ((string)settings[0x3d]);
                    for (num = 0; num < 6; num++)
                    {
                        strArray3[num] = (string)settings[num + 0xa9];
                    }
                }
                else if (Level.SceneName.Contains("Forest"))
                {
                    for (int i = 0x21; i < 0x29; i++)
                    {
                        url = url + ((string)settings[i]) + ",";
                    }
                    url.TrimEnd(new char[] { ',' });
                    for (int j = 0x29; j < 0x31; j++)
                    {
                        str3 = str3 + ((string)settings[j]) + ",";
                    }
                    str3 = str3 + ((string)settings[0x31]);
                    for (int k = 0; k < 150; k++)
                    {
                        string str5 = Convert.ToString((int)UnityEngine.Random.Range((float)0f, (float)8f));
                        n = n + str5;
                        if (((int)settings[50]) == 0)
                        {
                            n = n + str5;
                        }
                        else
                        {
                            n = n + Convert.ToString((int)UnityEngine.Random.Range((float)0f, (float)8f));
                        }
                    }
                    for (num = 0; num < 6; num++)
                    {
                        strArray3[num] = (string)settings[num + 0xa3];
                    }
                }
                if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
                {
                    base.StartCoroutine(this.loadskinE(n, url, str3, strArray3));
                }
                else if (PhotonNetwork.isMasterClient)
                {
                    base.photonView.RPC("loadskinRPC", PhotonTargets.AllBuffered, new object[] { n, url, str3, strArray3 });
                }
            }
            else if (Level.Name.StartsWith("Custom") && (IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.SINGLE))
            {
                GameObject[] objArray3 = GameObject.FindGameObjectsWithTag("playerRespawn");
                for (num = 0; num < objArray3.Length; num++)
                {
                    obj4 = objArray3[num];
                    obj4.transform.position = new Vector3(UnityEngine.Random.Range((float)-5f, (float)5f), 0f, UnityEngine.Random.Range((float)-5f, (float)5f));
                }
                objArray = (GameObject[])UnityEngine.Object.FindObjectsOfType(typeof(GameObject));
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
                        obj2.GetComponent<Renderer>().material.mainTexture = ((Material)RCassets.LoadAsset("grass")).mainTexture;
                    }
                }
                if (PhotonNetwork.isMasterClient)
                {
                    int num6;
                    strArray3 = new string[] { string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty };
                    for (num = 0; num < 6; num++)
                    {
                        strArray3[num] = (string)settings[num + 0xaf];
                    }
                    strArray3[6] = (string)settings[0xa2];
                    base.photonView.RPC("clearlevel", PhotonTargets.AllBuffered, new object[] { strArray3 });
                    RCRegions.Clear();
                    if (oldScript != currentScript)
                    {
                        ExitGames.Client.Photon.Hashtable hashtable;
                        this.levelCache.Clear();
                        this.titanSpawns.Clear();
                        this.playerSpawnsC.Clear();
                        this.playerSpawnsM.Clear();
                        this.titanSpawners.Clear();
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
                            for (num = 0; num < (Mathf.FloorToInt((float)((strArray4.Length - 1) / 100)) + 1); num++)
                            {
                                string[] strArray5;
                                int num7;
                                string[] strArray6;
                                string str6;
                                if (num < Mathf.FloorToInt((float)(strArray4.Length / 100)))
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
                                                this.titanSpawns.Add(new Vector3(Convert.ToSingle(strArray6[2]), Convert.ToSingle(strArray6[3]), Convert.ToSingle(strArray6[4])));
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
                                                this.titanSpawns.Add(new Vector3(Convert.ToSingle(strArray6[2]), Convert.ToSingle(strArray6[3]), Convert.ToSingle(strArray6[4])));
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
                            foreach (Vector3 vector in this.titanSpawns)
                            {
                                list.Add("titan," + vector.x.ToString() + "," + vector.y.ToString() + "," + vector.z.ToString());
                            }
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
                    base.StartCoroutine(this.customlevelE(this.playersRPC));
                    base.StartCoroutine(this.customlevelcache());
                }
            }
        }
    }

    private IEnumerator loadskinE(string n, string url, string url2, string[] skybox)
    {
        bool mipmap = true;
        bool iteratorVariable1 = false;
        if (((int)settings[0x3f]) == 1)
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
                Camera.main.GetComponent<Skybox>().material = (Material)linkHash[1][key];
                skyMaterial = (Material)linkHash[1][key];
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
                                                iteratorVariable33.material = (Material)linkHash[2][iteratorVariable31];
                                            }
                                            else
                                            {
                                                iteratorVariable33.material = (Material)linkHash[2][iteratorVariable31];
                                            }
                                        }
                                        else
                                        {
                                            iteratorVariable33.material = (Material)linkHash[2][iteratorVariable31];
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
                                                iteratorVariable33.material = (Material)linkHash[0][iteratorVariable32];
                                            }
                                            else
                                            {
                                                iteratorVariable33.material = (Material)linkHash[0][iteratorVariable32];
                                            }
                                        }
                                        else
                                        {
                                            iteratorVariable33.material = (Material)linkHash[0][iteratorVariable32];
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
                                        iteratorVariable39.material = (Material)linkHash[0][iteratorVariable38];
                                    }
                                    else
                                    {
                                        iteratorVariable39.material = (Material)linkHash[0][iteratorVariable38];
                                    }
                                }
                                else
                                {
                                    iteratorVariable39.material = (Material)linkHash[0][iteratorVariable38];
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
                                            iteratorVariable49.material = (Material)linkHash[0][iteratorVariable48];
                                        }
                                        else
                                        {
                                            iteratorVariable49.material = (Material)linkHash[0][iteratorVariable48];
                                        }
                                    }
                                    else
                                    {
                                        iteratorVariable49.material = (Material)linkHash[0][iteratorVariable48];
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
                                            iteratorVariable53.material = (Material)linkHash[0][iteratorVariable52];
                                        }
                                        else
                                        {
                                            iteratorVariable53.material = (Material)linkHash[0][iteratorVariable52];
                                        }
                                    }
                                    else
                                    {
                                        iteratorVariable53.material = (Material)linkHash[0][iteratorVariable52];
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
                                            iteratorVariable59.material = (Material)linkHash[2][iteratorVariable58];
                                        }
                                        else
                                        {
                                            iteratorVariable59.material = (Material)linkHash[2][iteratorVariable58];
                                        }
                                    }
                                    else
                                    {
                                        iteratorVariable59.material = (Material)linkHash[2][iteratorVariable58];
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
                                        iteratorVariable63.material = (Material)linkHash[2][iteratorVariable62];
                                    }
                                    else
                                    {
                                        iteratorVariable63.material = (Material)linkHash[2][iteratorVariable62];
                                    }
                                }
                                else
                                {
                                    iteratorVariable63.material = (Material)linkHash[2][iteratorVariable62];
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
    private void loadskinRPC(string n, string url, string url2, string[] skybox, PhotonMessageInfo info)
    {
        if ((((int)settings[2]) == 1) && info.sender.isMasterClient)
        {
            base.StartCoroutine(this.loadskinE(n, url, url2, skybox));
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

    public void multiplayerRacingFinsih()
    {
        float time = this.roundTime - 20f;
        if (PhotonNetwork.isMasterClient)
        {
            this.getRacingResult(LoginFengKAI.player.name, time);
        }
        else
        {
            object[] parameters = new object[] { LoginFengKAI.player.name, time };
            base.photonView.RPC("getRacingResult", PhotonTargets.MasterClient, parameters);
        }
        this.gameWin2();
    }

    [PunRPC]
    private void netGameLose(int score, PhotonMessageInfo info)
    {
        this.isLosing = true;
        Gamemode.OnNetGameLost(score);
        this.gameEndCD = this.gameEndTotalCDtime;
        if (((int)settings[0xf4]) == 1)
        {
            this.chatRoom.addLINE("<color=#FFC000>(" + this.roundTime.ToString("F2") + ")</color> Round ended (game lose).");
        }
        if (!((info.sender == PhotonNetwork.masterClient) || info.sender.isLocal) && PhotonNetwork.isMasterClient)
        {
            this.chatRoom.addLINE("<color=#FFC000>Round end sent from Player " + info.sender.ID.ToString() + "</color>");
        }
    }

    [PunRPC]
    private void netGameWin(int score, PhotonMessageInfo info)
    {
        this.isWinning = true;
        Gamemode.OnNetGameWon(score);
        this.gameEndCD = this.gameEndTotalCDtime;
        if (((int)settings[0xf4]) == 1)
        {
            this.chatRoom.addLINE("<color=#FFC000>(" + this.roundTime.ToString("F2") + ")</color> Round ended (game win).");
        }
        if (!((info.sender == PhotonNetwork.masterClient) || info.sender.isLocal))
        {
            this.chatRoom.addLINE("<color=#FFC000>Round end sent from Player " + info.sender.ID.ToString() + "</color>");
        }
    }

    [PunRPC]
    private void netRefreshRacingResult(string tmp)
    {
        this.localRacingResult = tmp;
    }

    [PunRPC]
    public void netShowDamage(int damage)
    {
        InGameUI.HUD.SetDamage(damage);
    }
    
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
        if (IN_GAME_MAIN_CAMERA.cameraMode == CAMERA_TYPE.TPS)
        {
            Screen.lockCursor = true;
        }
        else
        {
            Screen.lockCursor = false;
        }
        Cursor.visible = false;
        this.ShowHUDInfoCenter("the game has started for 60 seconds.\n please wait for next round.\n Click Right Mouse Key to Enter or Exit the Spectator Mode.");
        GameObject.Find("MainCamera").GetComponent<IN_GAME_MAIN_CAMERA>().enabled = true;
        GameObject.Find("MainCamera").GetComponent<IN_GAME_MAIN_CAMERA>().setMainObject(null, true, false);
        GameObject.Find("MainCamera").GetComponent<IN_GAME_MAIN_CAMERA>().setSpectorMode(true);
        GameObject.Find("MainCamera").GetComponent<IN_GAME_MAIN_CAMERA>().gameOver = true;
    }

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
        UnityEngine.MonoBehaviour.print("OnConnectedToMaster");
    }

    public void OnConnectedToPhoton()
    {
        UnityEngine.MonoBehaviour.print("OnConnectedToPhoton");
    }

    public void OnConnectionFail(DisconnectCause cause)
    {
        UnityEngine.MonoBehaviour.print("OnConnectionFail : " + cause.ToString());
        Screen.lockCursor = false;
        Cursor.visible = true;
        IN_GAME_MAIN_CAMERA.gametype = GAMETYPE.STOP;
        this.gameStart = false;
    }

    public void OnCreatedRoom()
    {
        this.racingResult = new ArrayList();
        UnityEngine.MonoBehaviour.print("OnCreatedRoom");
    }

    public void OnCustomAuthenticationFailed()
    {
        UnityEngine.MonoBehaviour.print("OnCustomAuthenticationFailed");
    }

    public void OnDisconnectedFromPhoton()
    {
        UnityEngine.MonoBehaviour.print("OnDisconnectedFromPhoton");
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
        UnityEngine.MonoBehaviour.print("OnFailedToConnectToPhoton");
    }

    public void OnGUI()
    {
        if(GUILayout.Button("Photon Spawn Test!"))
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
        this.maxPlayers = PhotonNetwork.room.MaxPlayers;
        this.playerList = string.Empty;
        char[] separator = new char[] { "`"[0] };
        //UnityEngine.MonoBehaviour.print("OnJoinedRoom " + PhotonNetwork.room.name + "    >>>>   " + LevelInfo.getInfo(PhotonNetwork.room.name.Split(separator)[1]).mapName);
        this.gameTimesUp = false;
        char[] chArray3 = new char[] { "`"[0] };
        string[] strArray = PhotonNetwork.room.name.Split(chArray3);
        this.difficulty = 0;
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
        IN_GAME_MAIN_CAMERA.difficulty = this.difficulty;
        this.time = 5000;//int.Parse(strArray[3]);
        this.time *= 60;
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
        ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
        hashtable.Add(PhotonPlayerProperty.name, LoginFengKAI.player.name);
        hashtable.Add(PhotonPlayerProperty.guildName, LoginFengKAI.player.guildname);
        hashtable.Add(PhotonPlayerProperty.kills, 0);
        hashtable.Add(PhotonPlayerProperty.max_dmg, 0);
        hashtable.Add(PhotonPlayerProperty.total_dmg, 0);
        hashtable.Add(PhotonPlayerProperty.deaths, 0);
        hashtable.Add(PhotonPlayerProperty.dead, true);
        hashtable.Add(PhotonPlayerProperty.isTitan, 0);
        hashtable.Add(PhotonPlayerProperty.RCteam, 0);
        hashtable.Add(PhotonPlayerProperty.currentLevel, string.Empty);
        ExitGames.Client.Photon.Hashtable propertiesToSet = hashtable;
        PhotonNetwork.player.SetCustomProperties(propertiesToSet);
        this.needChooseSide = true;
        this.chatContent = new ArrayList();
        this.killInfoGO = new ArrayList();
        //InRoomChat.messages = new List<string>();
        if (!PhotonNetwork.isMasterClient)
        {
            base.photonView.RPC("RequireStatus", PhotonTargets.MasterClient, new object[0]);
            base.photonView.RPC("RequestSettings", PhotonTargets.MasterClient);
        }
        this.assetCacheTextures = new Dictionary<string, Texture2D>();
        this.isFirstLoad = true;
        this.name = LoginFengKAI.player.name;
        if (loginstate != 3)
        {
            this.name = nameField;
            if ((!this.name.StartsWith("[") || (this.name.Length < 8)) || (this.name.Substring(7, 1) != "]"))
            {
                this.name = $"<color=#9999ff>{this.name}</color>";
            }
            this.name = this.name.Replace("[-]", "");
            LoginFengKAI.player.name = this.name;
        }
        ExitGames.Client.Photon.Hashtable hashtable3 = new ExitGames.Client.Photon.Hashtable();
        hashtable3.Add(PhotonPlayerProperty.name, this.name);
        PhotonNetwork.player.SetCustomProperties(hashtable3);
        if (OnPrivateServer)
        {
            ServerRequestAuthentication(PrivateServerAuthPass);
        }
    }

    public void OnLeftLobby()
    {
        UnityEngine.MonoBehaviour.print("OnLeftLobby");
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
            this.resetSettings(true);
            this.loadconfig();
            IN_GAME_MAIN_CAMERA.gametype = GAMETYPE.STOP;
            this.gameStart = false;
            Screen.lockCursor = false;
            Cursor.visible = true;
            this.inputManager.menuOn = false;
            this.DestroyAllExistingCloths();
            UnityEngine.Object.Destroy(GameObject.Find("MultiplayerManager"));
            Application.LoadLevel(0);
        }
    }

    private void OnLevelWasLoaded(int level)
    {
        if ((level != 0) && ((Application.loadedLevelName != "characterCreation") && (Application.loadedLevelName != "SnapShot")))
        {
            ChangeQuality.setCurrentQuality();
            foreach (GameObject obj2 in GameObject.FindGameObjectsWithTag("titan"))
            {
                if (!((obj2.GetPhotonView() != null) && obj2.GetPhotonView().owner.isMasterClient))
                {
                    UnityEngine.Object.Destroy(obj2);
                }
            }
            this.isWinning = false;
            this.gameStart = true;
            this.ShowHUDInfoCenter(string.Empty);
            GameObject obj3 = (GameObject)UnityEngine.Object.Instantiate(Resources.Load("MainCamera_mono"), GameObject.Find("cameraDefaultPosition").transform.position, GameObject.Find("cameraDefaultPosition").transform.rotation);
            UnityEngine.Object.Destroy(GameObject.Find("cameraDefaultPosition"));
            obj3.name = "MainCamera";
            Screen.lockCursor = true;
            Cursor.visible = true;
            this.cache();
            this.loadskin();
            Camera.main.GetComponent<IN_GAME_MAIN_CAMERA>().setHUDposition();
            Camera.main.GetComponent<IN_GAME_MAIN_CAMERA>().setDayLight(IN_GAME_MAIN_CAMERA.dayLight);
            //TODO: How should a gamemode and level be loaded in singlePlayer?
            if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
            {
                this.single_kills = 0;
                this.single_maxDamage = 0;
                this.single_totalDamage = 0;
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
                int abnormal = 90;
                if (this.difficulty == 1)
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
                if (this.needChooseSide)
                {
                    this.ShowHUDInfoTopCenterADD("\n\nPRESS 1 TO ENTER GAME");
                }
                else if (((int)settings[0xf5]) == 0)
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
                        this.SpawnPlayer(this.myLastHero, this.myLastRespawnTag);
                    }
                }

                if (!PhotonNetwork.isMasterClient)
                {
                    base.photonView.RPC("RequireStatus", PhotonTargets.MasterClient, new object[0]);
                }
                Gamemode.OnLevelLoaded(Level, PhotonNetwork.isMasterClient);
                if (((int)settings[0xf5]) == 1)
                {
                    this.EnterSpecMode(true);
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
                this.restartingMC = true;
            }
            this.resetSettings(false);
            if (!Gamemode.Settings.IsPlayerTitanEnabled)
            {
                ExitGames.Client.Photon.Hashtable propertiesToSet = new ExitGames.Client.Photon.Hashtable();
                propertiesToSet.Add(PhotonPlayerProperty.isTitan, 1);
                PhotonNetwork.player.SetCustomProperties(propertiesToSet);
            }
            if (!(this.gameTimesUp || !PhotonNetwork.isMasterClient))
            {
                this.restartGame2(true);
                base.photonView.RPC("setMasterRC", PhotonTargets.All, new object[0]);
            }
        }
        noRestart = false;
    }

    public void OnPhotonCreateRoomFailed()
    {
        UnityEngine.MonoBehaviour.print("OnPhotonCreateRoomFailed");
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
            if (PhotonNetwork.room.maxPlayers != this.maxPlayers)
            {
                PhotonNetwork.room.maxPlayers = this.maxPlayers;
            }
        }
        else
        {
            this.maxPlayers = PhotonNetwork.room.maxPlayers;
        }
    }

    public void OnPhotonInstantiate()
    {
        UnityEngine.MonoBehaviour.print("OnPhotonInstantiate");
    }

    public void OnPhotonJoinRoomFailed()
    {
        UnityEngine.MonoBehaviour.print("OnPhotonJoinRoomFailed");
    }

    public void OnPhotonMaxCccuReached()
    {
        UnityEngine.MonoBehaviour.print("OnPhotonMaxCccuReached");
    }

    public void OnPhotonPlayerConnected(PhotonPlayer player)
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
                if (Gamemode.Settings.SaveKDROnDisconnect)
                {
                    base.StartCoroutine(this.WaitAndReloadKDR(player));
                }
                if (Level.Name.StartsWith("Custom"))
                {
                    base.StartCoroutine(this.customlevelE(new List<PhotonPlayer> { player }));
                }
                ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
                if ((ignoreList != null) && (ignoreList.Count > 0))
                {
                    photonView.RPC("ignorePlayerArray", player, new object[] { ignoreList.ToArray() });
                }
                //photonView.RPC("settingRPC", player, new object[] { hashtable });
                photonView.RPC("setMasterRC", player, new object[0]);
                if ((Time.timeScale <= 0.1f) && (this.pauseWaitTime > 3f))
                {
                    photonView.RPC("pauseRPC", player, new object[] { true });
                    object[] parameters = new object[] { "<color=#FFCC00>MasterClient has paused the game.</color>", "" };
                    photonView.RPC("Chat", player, parameters);
                }
            }
        }
        this.RecompilePlayerList(0.1f);
    }

    public void OnPhotonPlayerDisconnected(PhotonPlayer player)
    {
        if (!this.gameTimesUp)
        {
            this.oneTitanDown(string.Empty);
            this.someOneIsDead(0);
        }
        if (ignoreList.Contains(player.ID))
        {
            ignoreList.Remove(player.ID);
        }
        InstantiateTracker.instance.TryRemovePlayer(player.ID);
        if (PhotonNetwork.isMasterClient)
        {
            base.photonView.RPC("verifyPlayerHasLeft", PhotonTargets.All, new object[] { player.ID });
        }
        if (Gamemode.Settings.SaveKDROnDisconnect)
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

    public void OnPhotonPlayerPropertiesChanged(object[] playerAndUpdatedProps)
    {
        this.RecompilePlayerList(0.1f);
        if (((playerAndUpdatedProps != null) && (playerAndUpdatedProps.Length >= 2)) && (((PhotonPlayer)playerAndUpdatedProps[0]) == PhotonNetwork.player))
        {
            ExitGames.Client.Photon.Hashtable hashtable2;
            ExitGames.Client.Photon.Hashtable hashtable = (ExitGames.Client.Photon.Hashtable)playerAndUpdatedProps[1];
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

    public void OnPhotonRandomJoinFailed()
    {
        UnityEngine.MonoBehaviour.print("OnPhotonRandomJoinFailed");
    }

    public void OnPhotonSerializeView()
    {
        UnityEngine.MonoBehaviour.print("OnPhotonSerializeView");
    }

    public void OnReceivedRoomListUpdate()
    {
    }

    public void OnUpdatedFriendList()
    {
        UnityEngine.MonoBehaviour.print("OnUpdatedFriendList");
    }
    
    [PunRPC]
    public void pauseRPC(bool pause, PhotonMessageInfo info)
    {
        if (info.sender.isMasterClient)
        {
            if (pause)
            {
                this.pauseWaitTime = 100000f;
                Time.timeScale = 1E-06f;
            }
            else
            {
                this.pauseWaitTime = 3f;
            }
        }
    }

    public void playerKillInfoSingleUpdate(int dmg)
    {
        this.single_kills++;
        this.single_maxDamage = Mathf.Max(dmg, this.single_maxDamage);
        this.single_totalDamage += dmg;
    }

    public void playerKillInfoUpdate(PhotonPlayer player, int dmg)
    {
        ExitGames.Client.Photon.Hashtable propertiesToSet = new ExitGames.Client.Photon.Hashtable();
        propertiesToSet.Add(PhotonPlayerProperty.kills, ((int)player.CustomProperties[PhotonPlayerProperty.kills]) + 1);
        player.SetCustomProperties(propertiesToSet);
        propertiesToSet = new ExitGames.Client.Photon.Hashtable();
        propertiesToSet.Add(PhotonPlayerProperty.max_dmg, Mathf.Max(dmg, (int)player.CustomProperties[PhotonPlayerProperty.max_dmg]));
        player.SetCustomProperties(propertiesToSet);
        propertiesToSet = new ExitGames.Client.Photon.Hashtable();
        propertiesToSet.Add(PhotonPlayerProperty.total_dmg, ((int)player.CustomProperties[PhotonPlayerProperty.total_dmg]) + dmg);
        player.SetCustomProperties(propertiesToSet);
    }
    
    public void RecompilePlayerList(float time)
    {
        if (!this.isRecompiling)
        {
            this.isRecompiling = true;
            base.StartCoroutine(this.WaitAndRecompilePlayerList(time));
        }
    }

    private void refreshRacingResult2()
    {
        this.localRacingResult = "Result\n";
        IComparer comparer = new IComparerRacingResult();
        this.racingResult.Sort(comparer);
        int num = Mathf.Min(this.racingResult.Count, 10);
        for (int i = 0; i < num; i++)
        {
            string localRacingResult = this.localRacingResult;
            object[] objArray2 = new object[] { localRacingResult, "Rank ", i + 1, " : " };
            this.localRacingResult = string.Concat(objArray2);
            this.localRacingResult = this.localRacingResult + (this.racingResult[i] as RacingResult).name;
            this.localRacingResult = this.localRacingResult + "   " + ((((int)((this.racingResult[i] as RacingResult).time * 100f)) * 0.01f)).ToString() + "s";
            this.localRacingResult = this.localRacingResult + "\n";
        }
        object[] parameters = new object[] { this.localRacingResult };
        base.photonView.RPC("netRefreshRacingResult", PhotonTargets.All, parameters);
    }

    [PunRPC]
    private void refreshStatus(float time1, float time2, bool startRacin, bool endRacin)
    {
        this.roundTime = time1;
        this.timeTotalServer = time2;
        this.startRacing = startRacin;
        this.endRacing = endRacin;
        if (this.startRacing && (GameObject.Find("door") != null))
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
        this.cT.Remove(titan);
    }

    public void removeET(TITAN_EREN hero)
    {
        this.eT.Remove(hero);
    }

    public void removeFT(FEMALE_TITAN titan)
    {
        this.fT.Remove(titan);
    }

    public void removeHero(Hero hero)
    {
        this.heroes.Remove(hero);
    }

    public void removeHook(Bullet h)
    {
        this.hooks.Remove(h);
    }

    public void removeTitan(MindlessTitan titan)
    {
        this.titans.Remove(titan);
    }

    [PunRPC]
    public void RequireStatus()
    {
        object[] parameters = new object[] { this.roundTime, this.timeTotalServer, this.startRacing, this.endRacing };
        base.photonView.RPC("refreshStatus", PhotonTargets.Others, parameters);
    }

    private void resetGameSettings()
    {
    }

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
            this.titanSpawns.Clear();
            this.playerSpawnsC.Clear();
            this.playerSpawnsM.Clear();
            this.titanSpawners.Clear();
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
            this.restartingMC = false;
        }
        PhotonNetwork.player.SetCustomProperties(propertiesToSet);
        this.resetGameSettings();
        banHash = new ExitGames.Client.Photon.Hashtable();
        imatitan = new ExitGames.Client.Photon.Hashtable();
        oldScript = string.Empty;
        ignoreList = new List<int>();
        this.restartCount = new List<float>();
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
            if (!this.isLosing && !this.isWinning)
            {
                for (int j = 0; j < PhotonNetwork.playerList.Length; j++)
                {
                    PhotonPlayer targetPlayer = PhotonNetwork.playerList[j];
                    if (((targetPlayer.CustomProperties[PhotonPlayerProperty.RCteam] == null) && RCextensions.returnBoolFromObject(targetPlayer.CustomProperties[PhotonPlayerProperty.dead])) && (RCextensions.returnIntFromObject(targetPlayer.CustomProperties[PhotonPlayerProperty.isTitan]) != 2))
                    {
                        this.photonView.RPC("respawnHeroInNewRound", targetPlayer, new object[0]);
                    }
                }
            }
        }
    }

    [PunRPC]
    private void respawnHeroInNewRound()
    {
        if (!this.needChooseSide && GameObject.Find("MainCamera").GetComponent<IN_GAME_MAIN_CAMERA>().gameOver)
        {
            this.SpawnPlayer(this.myLastHero, this.myLastRespawnTag);
            GameObject.Find("MainCamera").GetComponent<IN_GAME_MAIN_CAMERA>().gameOver = false;
            this.ShowHUDInfoCenter(string.Empty);
        }
    }

    public IEnumerator restartE(float time)
    {
        yield return new WaitForSeconds(time);
        this.restartGame2(false);
    }

    public void restartGame2(bool masterclientSwitched = false)
    {
        if (!this.gameTimesUp)
        {
            this.startRacing = false;
            this.endRacing = false;
            this.checkpoint = null;
            this.timeElapse = 0f;
            this.roundTime = 0f;
            this.isWinning = false;
            this.isLosing = false;
            this.isPlayer1Winning = false;
            this.isPlayer2Winning = false;
            this.myRespawnTime = 0f;
            this.killInfoGO = new ArrayList();
            this.racingResult = new ArrayList();
            this.ShowHUDInfoCenter(string.Empty);
            this.isRestarting = true;
            this.DestroyAllExistingCloths();
            PhotonNetwork.DestroyAll();
            ExitGames.Client.Photon.Hashtable hash = this.checkGameGUI();
            base.photonView.RPC("settingRPC", PhotonTargets.Others, new object[] { hash });
            base.photonView.RPC("RPCLoadLevel", PhotonTargets.All, new object[0]);
            this.setGameSettings(hash);
            if (masterclientSwitched)
            {
                this.sendChatContentInfo("<color=#A8FF24>MasterClient has switched to </color>" + ((string)PhotonNetwork.player.CustomProperties[PhotonPlayerProperty.name]).hexColor());
            }
        }
    }

    public void restartGameSingle2()
    {
        this.startRacing = false;
        this.endRacing = false;
        this.checkpoint = null;
        this.single_kills = 0;
        this.single_maxDamage = 0;
        this.single_totalDamage = 0;
        this.timeElapse = 0f;
        this.roundTime = 0f;
        this.timeTotalServer = 0f;
        this.isWinning = false;
        this.isLosing = false;
        this.isPlayer1Winning = false;
        this.isPlayer2Winning = false;
        this.myRespawnTime = 0f;
        this.ShowHUDInfoCenter(string.Empty);
        this.DestroyAllExistingCloths();
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
            photonView.RPC("SyncSettings", PhotonTargets.Others, json, Gamemode.Settings.GamemodeType);
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
            photonView.RPC("SyncSettings", PhotonTargets.Others, json, Gamemode.Settings.GamemodeType);
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
            this.DestroyAllExistingCloths();
            PhotonNetwork.LoadLevel(Level.SceneName);
        }
        else if (PhotonNetwork.isMasterClient)
        {
            this.kickPlayerRC(info.sender, true, "false restart.");
        }
        else if (!masterRC)
        {
            this.restartCount.Add(Time.time);
            foreach (float num in this.restartCount)
            {
                if ((Time.time - num) > 60f)
                {
                    this.restartCount.Remove(num);
                }
            }
            if (this.restartCount.Count < 6)
            {
                this.DestroyAllExistingCloths();
                PhotonNetwork.LoadLevel(Level.SceneName);
            }
        }
    }

    public void sendChatContentInfo(string content)
    {
        object[] parameters = new object[] { content, string.Empty };
        base.photonView.RPC("Chat", PhotonTargets.All, parameters);
    }

    public void sendKillInfo(bool t1, string killer, bool t2, string victim, int dmg = 0)
    {
        object[] parameters = new object[] { t1, killer, t2, victim, dmg };
        base.photonView.RPC("updateKillInfo", PhotonTargets.All, parameters);
    }

    public static void ServerCloseConnection(PhotonPlayer targetPlayer, bool requestIpBan, string inGameName = null)
    {
        RaiseEventOptions options = new RaiseEventOptions
        {
            TargetActors = new int[] { targetPlayer.ID }
        };
        if (requestIpBan)
        {
            ExitGames.Client.Photon.Hashtable eventContent = new ExitGames.Client.Photon.Hashtable();
            eventContent[(byte)0] = true;
            if ((inGameName != null) && (inGameName.Length > 0))
            {
                eventContent[(byte)1] = inGameName;
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
            ExitGames.Client.Photon.Hashtable eventContent = new ExitGames.Client.Photon.Hashtable();
            eventContent[(byte)0] = authPassword;
            PhotonNetwork.RaiseEvent(0xc6, eventContent, true, new RaiseEventOptions());
        }
    }

    public static void ServerRequestUnban(string bannedAddress)
    {
        if (!string.IsNullOrEmpty(bannedAddress))
        {
            ExitGames.Client.Photon.Hashtable eventContent = new ExitGames.Client.Photon.Hashtable();
            eventContent[(byte)0] = bannedAddress;
            PhotonNetwork.RaiseEvent(0xc7, eventContent, true, new RaiseEventOptions());
        }
    }

    public void setBackground()
    {
        if (isAssetLoaded)
        {
            UnityEngine.Object.Instantiate(RCassets.LoadAsset("backgroundCamera"));
        }
    }

    private void setGameSettings(ExitGames.Client.Photon.Hashtable hash)
    {
    }

    [PunRPC]
    private void setMasterRC(PhotonMessageInfo info)
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
            string name = LoginFengKAI.player.name;
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
            string str2 = LoginFengKAI.player.name;
            if (!str2.StartsWith("<color=#ff00ff>"))
            {
                str2 = $"<color=#ff00ff>{str2}</color>";
            }
            this.name = str2;
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
                    base.photonView.RPC("labelRPC", PhotonTargets.All, new object[] { obj2.GetPhotonView().viewID });
                }
            }
        }
    }

    [PunRPC]
    private void setTeamRPC(int setting, PhotonMessageInfo info)
    {
        if (info.sender.isMasterClient || info.sender.isLocal)
        {
            this.setTeam(setting);
        }
    }

    [PunRPC]
    private void settingRPC(ExitGames.Client.Photon.Hashtable hash, PhotonMessageInfo info)
    {
        if (info.sender.isMasterClient)
        {
            this.setGameSettings(hash);
        }
    }

    [PunRPC]
    private void showChatContent(string content)
    {
        this.chatContent.Add(content);
        if (this.chatContent.Count > 10)
        {
            this.chatContent.RemoveAt(0);
        }
        //GameObject.Find("LabelChatContent").GetComponent<UILabel>().text = string.Empty;
        //for (int i = 0; i < this.chatContent.Count; i++)
        //{
        //    UILabel component = GameObject.Find("LabelChatContent").GetComponent<UILabel>();
        //    component.text = component.text + this.chatContent[i];
        //}
    }

    [PunRPC]
    private void SyncSettings(string gamemodeRaw, GamemodeType type, PhotonMessageInfo info)
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
                    this.setTeam(3);
                }
            }
            else
            {
                this.setTeam(0);
            }
                

            if (Gamemode.Settings.GamemodeType == GamemodeType.Infection)
            {
                var gamemodeInfection = (InfectionGamemodeSettings) settings;
                if (gamemodeInfection.Infected > 0)
                {
                    var hashtable = new ExitGames.Client.Photon.Hashtable();
                    hashtable.Add(PhotonPlayerProperty.RCteam, 0);
                    PhotonNetwork.player.SetCustomProperties(hashtable);
                    this.chatRoom.addLINE($"<color=#FFCC00>Infection mode ({gamemodeInfection.Infected}) enabled. Make sure your first character is human.</color>");
                }
                else
                {
                    var hashtable = new ExitGames.Client.Photon.Hashtable();
                    hashtable.Add(PhotonPlayerProperty.isTitan, 1);
                    PhotonNetwork.player.SetCustomProperties(hashtable);
                    this.chatRoom.addLINE("<color=#FFCC00>Infection Mode disabled.</color>");
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
        if (!(this.gameTimesUp || !t.sender.isMasterClient))
        {
            this.gameTimesUp = true;
            GameObject obj2 = GameObject.Find("UI_IN_GAME");
            Screen.lockCursor = false;
            Cursor.visible = true;
            IN_GAME_MAIN_CAMERA.gametype = GAMETYPE.STOP;
            this.gameStart = false;
        }
        else if (!(t.sender.isMasterClient || !PhotonNetwork.player.isMasterClient))
        {
            this.kickPlayerRC(t.sender, true, "false game end.");
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
            this.NOTSpawnPlayerRC(id);
        }
        else
        {
            Vector3 position = pos.transform.position;
            if (this.racingSpawnPointSet)
            {
                position = this.racingSpawnPoint;
            }
            else if (Level.Name.StartsWith("Custom"))
            {
                if (RCextensions.returnIntFromObject(PhotonNetwork.player.CustomProperties[PhotonPlayerProperty.RCteam]) == 0)
                {
                    List<Vector3> list = new List<Vector3>();
                    foreach (Vector3 vector2 in this.playerSpawnsC)
                    {
                        list.Add(vector2);
                    }
                    foreach (Vector3 vector2 in this.playerSpawnsM)
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
                    if (this.playerSpawnsC.Count > 0)
                    {
                        position = this.playerSpawnsC[UnityEngine.Random.Range(0, this.playerSpawnsC.Count)];
                    }
                }
                else if ((RCextensions.returnIntFromObject(PhotonNetwork.player.CustomProperties[PhotonPlayerProperty.RCteam]) == 2) && (this.playerSpawnsM.Count > 0))
                {
                    position = this.playerSpawnsM[UnityEngine.Random.Range(0, this.playerSpawnsM.Count)];
                }
            }
            IN_GAME_MAIN_CAMERA component = GameObject.Find("MainCamera").GetComponent<IN_GAME_MAIN_CAMERA>();
            this.myLastHero = id.ToUpper();
            if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
            {
                if (IN_GAME_MAIN_CAMERA.singleCharacter == "TITAN_EREN")
                {
                    component.setMainObject((GameObject)UnityEngine.Object.Instantiate(Resources.Load("TITAN_EREN"), pos.transform.position, pos.transform.rotation), true, false);
                }
                else
                {
                    component.setMainObject((GameObject)UnityEngine.Object.Instantiate(Resources.Load("AOTTG_HERO 1"), pos.transform.position, pos.transform.rotation), true, false);
                    if (((IN_GAME_MAIN_CAMERA.singleCharacter == "SET 1") || (IN_GAME_MAIN_CAMERA.singleCharacter == "SET 2")) || (IN_GAME_MAIN_CAMERA.singleCharacter == "SET 3"))
                    {
                        HeroCostume costume = CostumeConeveter.LocalDataToHeroCostume(IN_GAME_MAIN_CAMERA.singleCharacter);
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
                        for (int i = 0; i < HeroCostume.costume.Length; i++)
                        {
                            if (HeroCostume.costume[i].name.ToUpper() == IN_GAME_MAIN_CAMERA.singleCharacter.ToUpper())
                            {
                                int index = (HeroCostume.costume[i].id + CheckBoxCostume.costumeSet) - 1;
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
                    HeroCostume costume2 = CostumeConeveter.LocalDataToHeroCostume(id);
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
                    for (int j = 0; j < HeroCostume.costume.Length; j++)
                    {
                        if (HeroCostume.costume[j].name.ToUpper() == id.ToUpper())
                        {
                            int num4 = HeroCostume.costume[j].id;
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
                ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
                hashtable.Add("dead", false);
                ExitGames.Client.Photon.Hashtable propertiesToSet = hashtable;
                PhotonNetwork.player.SetCustomProperties(propertiesToSet);
                hashtable = new ExitGames.Client.Photon.Hashtable();
                hashtable.Add(PhotonPlayerProperty.isTitan, 1);
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
            this.isLosing = false;
            this.ShowHUDInfoCenter(string.Empty);
        }
    }


    [PunRPC]
    public void spawnPlayerAtRPC(float posX, float posY, float posZ, PhotonMessageInfo info)
    {
        if (((info.sender.isMasterClient && logicLoaded) && (customLevelLoaded && !this.needChooseSide)) && Camera.main.GetComponent<IN_GAME_MAIN_CAMERA>().gameOver)
        {
            Vector3 position = new Vector3(posX, posY, posZ);
            IN_GAME_MAIN_CAMERA component = Camera.main.GetComponent<IN_GAME_MAIN_CAMERA>();
            component.setMainObject(PhotonNetwork.Instantiate("AOTTG_HERO 1", position, new Quaternion(0f, 0f, 0f, 1f), 0), true, false);
            string slot = this.myLastHero.ToUpper();
            switch (slot)
            {
                case "SET 1":
                case "SET 2":
                case "SET 3":
                    {
                        HeroCostume costume = CostumeConeveter.LocalDataToHeroCostume(slot);
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
                    for (int i = 0; i < HeroCostume.costume.Length; i++)
                    {
                        if (HeroCostume.costume[i].name.ToUpper() == slot.ToUpper())
                        {
                            int id = HeroCostume.costume[i].id;
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
            ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
            hashtable.Add("dead", false);
            ExitGames.Client.Photon.Hashtable propertiesToSet = hashtable;
            PhotonNetwork.player.SetCustomProperties(propertiesToSet);
            hashtable = new ExitGames.Client.Photon.Hashtable();
            hashtable.Add(PhotonPlayerProperty.isTitan, 1);
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
            this.isLosing = false;
            this.ShowHUDInfoCenter(string.Empty);
        }
    }

    private void spawnPlayerCustomMap()
    {
        if (!this.needChooseSide && GameObject.Find("MainCamera").GetComponent<IN_GAME_MAIN_CAMERA>().gameOver)
        {
            Camera.main.GetComponent<IN_GAME_MAIN_CAMERA>().gameOver = false;
            if (RCextensions.returnIntFromObject(PhotonNetwork.player.CustomProperties[PhotonPlayerProperty.isTitan]) == 2)
            {
                SpawnPlayerTitan();
            }
            else
            {
                this.SpawnPlayer(this.myLastHero, this.myLastRespawnTag);
            }
            this.ShowHUDInfoCenter(string.Empty);
        }
    }

    public GameObject SpawnTitan(TitanConfiguration configuration)
    {
        Vector3 position = new Vector3();
        Quaternion rotation = new Quaternion();
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
        Vector3 position = location.transform.position;
        if (Level.Name.StartsWith("Custom") && (this.titanSpawns.Count > 0))
        {
            position = this.titanSpawns[UnityEngine.Random.Range(0, this.titanSpawns.Count)];
        }
        this.myLastHero = id.ToUpper();
        var playerTitan = PhotonNetwork.Instantiate("PlayerTitan", position, new Quaternion(), 0).GetComponent<PlayerTitan>();
        playerTitan.Initialize(Gamemode.GetPlayerTitanConfiguration());
        GameObject.Find("MainCamera").GetComponent<IN_GAME_MAIN_CAMERA>().setMainObjectASTITAN(playerTitan.gameObject);
        GameObject.Find("MainCamera").GetComponent<IN_GAME_MAIN_CAMERA>().enabled = true;
        GameObject.Find("MainCamera").GetComponent<SpectatorMovement>().disable = true;
        //TODO MouseLook
        //GameObject.Find("MainCamera").GetComponent<MouseLook>().disable = true;
        GameObject.Find("MainCamera").GetComponent<IN_GAME_MAIN_CAMERA>().gameOver = false;
        ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
        hashtable.Add("dead", false);
        ExitGames.Client.Photon.Hashtable propertiesToSet = hashtable;
        PhotonNetwork.player.SetCustomProperties(propertiesToSet);
        hashtable = new ExitGames.Client.Photon.Hashtable();
        hashtable.Add(PhotonPlayerProperty.isTitan, 2);
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
        this.ShowHUDInfoCenter(string.Empty);
    }

    private void Start()
    {
        Debug.Log($"Version: {Version}");
        instance = this;
        base.gameObject.name = "MultiplayerManager";
        CostumeHair.init();
        CharacterMaterials.init();
        HeroCostume.init2();
        UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
        this.heroes = new ArrayList();
        this.eT = new ArrayList();
        this.titans = new ArrayList();
        this.fT = new ArrayList();
        this.cT = new ArrayList();
        this.hooks = new ArrayList();
        this.name = string.Empty;
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
        this.resetGameSettings();
        banHash = new ExitGames.Client.Photon.Hashtable();
        imatitan = new ExitGames.Client.Photon.Hashtable();
        oldScript = string.Empty;
        currentLevel = string.Empty;
        if (currentScript == null)
        {
            currentScript = string.Empty;
        }
        this.titanSpawns = new List<Vector3>();
        this.playerSpawnsC = new List<Vector3>();
        this.playerSpawnsM = new List<Vector3>();
        this.playersRPC = new List<PhotonPlayer>();
        this.levelCache = new List<string[]>();
        this.titanSpawners = new List<TitanSpawner>();
        this.restartCount = new List<float>();
        ignoreList = new List<int>();
        this.groundList = new List<GameObject>();
        noRestart = false;
        masterRC = false;
        this.isSpawning = false;
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
        this.retryTime = 0f;
        this.playerList = string.Empty;
        this.updateTime = 0f;
        if (this.textureBackgroundBlack == null)
        {
            this.textureBackgroundBlack = new Texture2D(1, 1, TextureFormat.ARGB32, false);
            this.textureBackgroundBlack.SetPixel(0, 0, new Color(0f, 0f, 0f, 1f));
            this.textureBackgroundBlack.Apply();
        }
        if (this.textureBackgroundBlue == null)
        {
            this.textureBackgroundBlue = new Texture2D(1, 1, TextureFormat.ARGB32, false);
            this.textureBackgroundBlue.SetPixel(0, 0, new Color(0.08f, 0.3f, 0.4f, 1f));
            this.textureBackgroundBlue.Apply();
        }
        this.loadconfig();
        this.setBackground();
        ChangeQuality.setCurrentQuality();
    }

    [PunRPC]
    public void titanGetKill(PhotonPlayer player, int Damage, string name)
    {
        Damage = Mathf.Max(10, Damage);
        object[] parameters = new object[] { Damage };
        base.photonView.RPC("netShowDamage", player, parameters);
        base.photonView.RPC("oneTitanDown", PhotonTargets.MasterClient, name);
        this.sendKillInfo(false, (string)player.CustomProperties[PhotonPlayerProperty.name], true, name, Damage);
        this.playerKillInfoUpdate(player, Damage);
    }

    public void titanGetKillbyServer(int Damage, string name)
    {
        Damage = Mathf.Max(10, Damage);
        this.sendKillInfo(false, LoginFengKAI.player.name, true, name, Damage);
        this.netShowDamage(Damage);
        this.oneTitanDown(name);
        this.playerKillInfoUpdate(PhotonNetwork.player, Damage);
    }

    public void unloadAssets()
    {
        if (!this.isUnloading)
        {
            this.isUnloading = true;
            base.StartCoroutine(this.unloadAssetsE(10f));
        }
    }

    public IEnumerator unloadAssetsE(float time)
    {
        yield return new WaitForSeconds(time);
        Resources.UnloadUnusedAssets();
        this.isUnloading = false;
    }

    public void unloadAssetsEditor()
    {
        if (!this.isUnloading)
        {
            this.isUnloading = true;
            base.StartCoroutine(this.unloadAssetsE(30f));
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
        if (this.gameStart)
        {
            IEnumerator enumerator = this.heroes.GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    var current = (Hero)enumerator.Current;
                    if (current != null)
                        current.update2();
                }
            }
            finally
            {
                IDisposable disposable = enumerator as IDisposable;
                if (disposable != null)
                {
                    disposable.Dispose();
                }
            }
            IEnumerator enumerator2 = this.hooks.GetEnumerator();
            try
            {
                while (enumerator2.MoveNext())
                {
                    var current = (Bullet)enumerator2.Current;
                    if (current != null)
                        current.update();
                }
            }
            finally
            {
                IDisposable disposable2 = enumerator2 as IDisposable;
                if (disposable2 != null)
                {
                    disposable2.Dispose();
                }
            }
            if (this.mainCamera != null)
            {
                this.mainCamera.snapShotUpdate();
            }
            IEnumerator enumerator3 = this.eT.GetEnumerator();
            try
            {
                while (enumerator3.MoveNext())
                {
                    var titanEren = (TITAN_EREN)enumerator3.Current;
                    if (titanEren != null)
                        titanEren.update();
                }
            }
            finally
            {
                IDisposable disposable3 = enumerator3 as IDisposable;
                if (disposable3 != null)
                {
                    disposable3.Dispose();
                }
            }
            IEnumerator enumerator5 = this.fT.GetEnumerator();
            try
            {
                while (enumerator5.MoveNext())
                {
                    var femaleTitan = (FEMALE_TITAN)enumerator5.Current;
                    if (femaleTitan != null)
                        femaleTitan.update();
                }
            }
            finally
            {
                IDisposable disposable5 = enumerator5 as IDisposable;
                if (disposable5 != null)
                {
                    disposable5.Dispose();
                }
            }
            IEnumerator enumerator6 = this.cT.GetEnumerator();
            try
            {
                while (enumerator6.MoveNext())
                {
                    var colossalTitan = (COLOSSAL_TITAN)enumerator6.Current;
                    if (colossalTitan != null)
                        colossalTitan.update2();
                }
            }
            finally
            {
                IDisposable disposable6 = enumerator6 as IDisposable;
                if (disposable6 != null)
                {
                    disposable6.Dispose();
                }
            }
            if (this.mainCamera != null)
            {
                this.mainCamera.update2();
            }
        }
    }

    [PunRPC]
    private void updateKillInfo(bool t1, string killer, bool t2, string victim, int dmg)
    {
        GameObject obj4;
        GameObject obj2 = GameObject.Find("KillFeed");
        GameObject obj3 = (GameObject)UnityEngine.Object.Instantiate(Resources.Load("UI/KillInfo"));
        for (int i = 0; i < this.killInfoGO.Count; i++)
        {
            obj4 = (GameObject)this.killInfoGO[i];
            if (obj4 != null)
            {
                obj4.GetComponent<KillInfo>().MoveOn();
            }
        }
        if (this.killInfoGO.Count > 4)
        {
            obj4 = (GameObject)this.killInfoGO[0];
            if (obj4 != null)
            {
                obj4.GetComponent<KillInfo>().Destroy();
            }
            this.killInfoGO.RemoveAt(0);
        }

        obj3.transform.parent = obj2.transform;
        obj3.transform.position = new Vector3();
        obj3.GetComponent<KillInfo>().Show(t1, killer, t2, victim, dmg);
        this.killInfoGO.Add(obj3);
        if (((int)settings[0xf4]) == 1)
        {
            string str2 = ("<color=#FFC000>(" + this.roundTime.ToString("F2") + ")</color> ") + killer.hexColor() + " killed ";
            string newLine = str2 + victim.hexColor() + " for " + dmg.ToString() + " damage.";
            this.chatRoom.addLINE(newLine);
        }
    }

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
        string iteratorVariable1 = string.Empty;
        if (Gamemode.Settings.TeamMode == TeamMode.Disabled)
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
                if (Gamemode.Settings.TeamMode == TeamMode.LockBySize)
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
                            this.photonView.RPC("setTeamRPC", player2, new object[] { num12 });
                        }
                    }
                }
                else if (Gamemode.Settings.TeamMode == TeamMode.LockBySkill)
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
                                this.photonView.RPC("setTeamRPC", player3, new object[] { num13 });
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
        if (PhotonNetwork.isMasterClient && ((!this.isWinning && !this.isLosing) && (this.roundTime >= 5f)))
        {
            int num22;
            if (Gamemode.Settings.PointMode > 0)
            {
                if (Gamemode.Settings.TeamMode != TeamMode.Disabled)
                {
                    if (this.cyanKills >= Gamemode.Settings.PointMode)
                    {
                        object[] parameters = new object[] { "<color=#00FFFF>Team Cyan wins! </color>", string.Empty };
                        this.photonView.RPC("Chat", PhotonTargets.All, parameters);
                        this.gameWin2();
                    }
                    else if (this.magentaKills >= Gamemode.Settings.PointMode)
                    {
                        objArray2 = new object[] { "<color=#FF00FF>Team Magenta wins! </color>", string.Empty };
                        this.photonView.RPC("Chat", PhotonTargets.All, objArray2);
                        this.gameWin2();
                    }
                }
                else if (Gamemode.Settings.TeamMode == TeamMode.Disabled)
                {
                    for (num22 = 0; num22 < PhotonNetwork.playerList.Length; num22++)
                    {
                        PhotonPlayer player9 = PhotonNetwork.playerList[num22];
                        if (RCextensions.returnIntFromObject(player9.CustomProperties[PhotonPlayerProperty.kills]) >= Gamemode.Settings.PointMode)
                        {
                            object[] objArray4 = new object[] { "<color=#FFCC00>" + RCextensions.returnStringFromObject(player9.CustomProperties[PhotonPlayerProperty.name]).hexColor() + " wins!</color>", string.Empty };
                            this.photonView.RPC("Chat", PhotonTargets.All, objArray4);
                            this.gameWin2();
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
                                this.photonView.RPC("Chat", PhotonTargets.All, objArray5);
                                this.gameWin2();
                            }
                            else if (num25 == 0)
                            {
                                object[] objArray6 = new object[] { "<color=#00FFFF>Team Cyan wins! </color>", string.Empty };
                                this.photonView.RPC("Chat", PhotonTargets.All, objArray6);
                                this.gameWin2();
                            }
                        }
                    }
                    else if ((Gamemode.Settings.TeamMode == TeamMode.Disabled) && (PhotonNetwork.playerList.Length > 1))
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
                            this.photonView.RPC("Chat", PhotonTargets.All, objArray7);
                            this.gameWin2();
                        }
                    }
                }
            }
        }
        this.isRecompiling = false;
    }

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

    public IEnumerator WaitAndResetRestarts()
    {
        yield return new WaitForSeconds(10f);
        this.restartingMC = false;
    }

    public IEnumerator WaitAndRespawn1(float time, string str)
    {
        yield return new WaitForSeconds(time);
        this.SpawnPlayer(this.myLastHero, str);
    }

    public IEnumerator WaitAndRespawn2(float time, GameObject pos)
    {
        yield return new WaitForSeconds(time);
        this.SpawnPlayerAt2(this.myLastHero, pos);
    }
}
