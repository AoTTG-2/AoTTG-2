using Assets.Scripts;
using Assets.Scripts.Characters;
using Assets.Scripts.Characters.Titan;
using Assets.Scripts.Services;
using Assets.Scripts.Services.Interface;
using Assets.Scripts.UI.Camera;
using Assets.Scripts.UI.Input;
using System;
using UnityEngine;
using static Assets.Scripts.FengGameManagerMKII;
using Random = UnityEngine.Random;

public class IN_GAME_MAIN_CAMERA : MonoBehaviour
{
    private IEntityService EntityService => Service.Entity;

    private float closestDistance;
    private int currentPeekPlayerIndex;
    [Obsolete("Replace with a Time Service")]
    public static DayLight dayLight = DayLight.Dawn;
    private float decay;
    private float distance = 10f;
    private float distanceMulti;
    private float distanceOffsetMulti;
    private float duration;
    private float flashDuration;
    private bool flip;
    public bool gameOver;
    [Obsolete("Refactor so that this static field is no longer required")]
    public static GAMETYPE gametype = GAMETYPE.Stop;
    private bool hasSnapShot;
    private Transform head;
    private float heightMulti;
    public static bool isPausing;
    public static bool isTyping;
    private bool lockAngle;
    private Vector3 lockCameraPosition;
    private GameObject locker;
    private GameObject lockTarget;
    public GameObject main_object;
    private bool needSetHUD;
    private float R;
    public Material skyBoxDAWN;
    public Material skyBoxDAY;
    public Material skyBoxNIGHT;
    private Texture2D snapshot1;
    private Texture2D snapshot2;
    private Texture2D snapshot3;
    public GameObject snapShotCamera;
    private int snapShotCount;
    private float snapShotCountDown;
    private int snapShotDmg;
    private float snapShotInterval = 0.02f;
    public RenderTexture snapshotRT;
    private float snapShotStartCountDownTime;
    private GameObject snapShotTarget;
    private Vector3 snapShotTargetPosition;
    public bool spectatorMode;
    private bool startSnapShotFrameCount;
    public static STEREO_3D_TYPE stereoType;
    public static bool triggerAutoLock;
    public static bool usingTitan;
    private bool isRestarting = true;
    private float startingTime;
    public bool IsSpecmode => (int) settings[0xf5] == 1;

    private void Awake()
    {
        EntityService.OnRegister += EntityService_OnRegistered;

        isTyping = false;
        isPausing = false;
        base.name = "MainCamera";
        if (PlayerPrefs.HasKey("GameQuality"))
        {
            //TODO TiltShift
            if (PlayerPrefs.GetFloat("GameQuality") >= 0.9f)
            {
                //base.GetComponent<TiltShift>().enabled = true;
            }
            else
            {
                //base.GetComponent<TiltShift>().enabled = false;
            }
        }
        else
        {
            //base.GetComponent<TiltShift>().enabled = true;
        }

        startingTime = Time.time;
    }

    private void EntityService_OnRegistered(Entity entity)
    {
        if (entity is PlayerTitan pt)
        {
            setMainObjectASTITAN(pt.gameObject);
            enabled = true;
            GetComponent<SpectatorMovement>().disable = true;
            GetComponent<MouseLook>().disable = true;
            gameOver = false;
        }
    }

    public void CameraMovementLive(Hero hero)
    {
        float magnitude = hero.GetComponent<Rigidbody>().velocity.magnitude;
        if (magnitude > 10f)
        {
            Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, Mathf.Min((float) 100f, (float) (magnitude + 40f)), 0.1f);
        }
        else
        {
            Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, 50f, 0.1f);
        }
        float num2 = (hero.CameraMultiplier * (200f - Camera.main.fieldOfView)) / 150f;
        base.transform.position = (Vector3) ((this.head.transform.position + (Vector3.up * this.heightMulti)) - ((Vector3.up * (0.6f - InputManager.Settings.CameraDistance)) * 2f));
        Transform transform = base.transform;
        transform.position -= (Vector3) (((base.transform.forward * this.distance) * this.distanceMulti) * num2);
        if (hero.CameraMultiplier < 0.65f)
        {
            Transform transform2 = base.transform;
            transform2.position += (Vector3) (base.transform.right * Mathf.Max((float) ((0.6f - hero.CameraMultiplier) * 2f), (float) 0.65f));
        }
        base.transform.rotation = Quaternion.Lerp(Camera.main.transform.rotation, hero.GetComponent<SmoothSyncMovement>().correctCameraRot, Time.deltaTime * 5f);
    }

    public void createSnapShotRT2()
    {
        if (this.snapshotRT != null)
        {
            this.snapshotRT.Release();
        }
        if (QualitySettings.GetQualityLevel() > 3)
        {
            this.snapshotRT = new RenderTexture((int) (Screen.width * 0.8f), (int) (Screen.height * 0.8f), 0x18);
            this.snapShotCamera.GetComponent<Camera>().targetTexture = this.snapshotRT;
        }
        else
        {
            this.snapshotRT = new RenderTexture((int) (Screen.width * 0.4f), (int) (Screen.height * 0.4f), 0x18);
            this.snapShotCamera.GetComponent<Camera>().targetTexture = this.snapshotRT;
        }
    }

    public void flashBlind()
    {
        //GameObject.Find("flash").GetComponent<UISprite>().alpha = 1f;
        //this.flashDuration = 2f;
    }

    public void setDayLight(DayLight val)
    {
        return;
        dayLight = val;
        dayLight = DayLight.Day;
        if (dayLight == DayLight.Night)
        {
            GameObject obj2 = (GameObject) UnityEngine.Object.Instantiate(Resources.Load("flashlight"));
            obj2.transform.parent = base.transform;
            obj2.transform.position = base.transform.position;
            obj2.transform.rotation = Quaternion.Euler(353f, 0f, 0f);
            RenderSettings.ambientLight = FengColor.nightAmbientLight;
            GameObject.Find("mainLight").GetComponent<Light>().color = FengColor.nightLight;
            base.gameObject.GetComponent<Skybox>().material = this.skyBoxNIGHT;
        }
        if (dayLight == DayLight.Day)
        {
            RenderSettings.ambientLight = FengColor.dayAmbientLight;
            GameObject.Find("mainLight").GetComponent<Light>().color = FengColor.dayLight;
            base.gameObject.GetComponent<Skybox>().material = this.skyBoxDAY;
        }
        if (dayLight == DayLight.Dawn)
        {
            RenderSettings.ambientLight = FengColor.dawnAmbientLight;
            GameObject.Find("mainLight").GetComponent<Light>().color = FengColor.dawnAmbientLight;
            base.gameObject.GetComponent<Skybox>().material = this.skyBoxDAWN;
        }

        //HACK Fix this
        //this.snapShotCamera.gameObject.GetComponent<Skybox>().material = base.gameObject.GetComponent<Skybox>().material;
    }

    public void setHUDposition()
    {
        return;
        GameObject.Find("Flare").transform.localPosition = new Vector3((float) (((int) (-Screen.width * 0.5f)) + 14), (float) ((int) (-Screen.height * 0.5f)), 0f);
        GameObject obj2 = GameObject.Find("LabelInfoBottomRight");
        obj2.transform.localPosition = new Vector3((float) ((int) (Screen.width * 0.5f)), (float) ((int) (-Screen.height * 0.5f)), 0f);

        //obj2.GetComponent<UILabel>().text = "Pause : " + GameObject.Find("InputManagerController").GetComponent<FengCustomInputs>().inputString[InputCode.pause] + " ";
        GameObject.Find("LabelInfoTopCenter").transform.localPosition = new Vector3(0f, (float) ((int) (Screen.height * 0.5f)), 0f);
        GameObject.Find("LabelInfoTopRight").transform.localPosition = new Vector3((float) ((int) (Screen.width * 0.5f)), (float) ((int) (Screen.height * 0.5f)), 0f);
        GameObject.Find("LabelNetworkStatus").transform.localPosition = new Vector3((float) ((int) (-Screen.width * 0.5f)), (float) ((int) (Screen.height * 0.5f)), 0f);
        GameObject.Find("LabelInfoTopLeft").transform.localPosition = new Vector3((float) ((int) (-Screen.width * 0.5f)), (float) ((int) ((Screen.height * 0.5f) - 20f)), 0f);
        GameObject.Find("Chatroom").transform.localPosition = new Vector3((float) ((int) (-Screen.width * 0.5f)), (float) ((int) (-Screen.height * 0.5f)), 0f);
        if (usingTitan)
        {
            Vector3 vector = new Vector3(0f, 9999f, 0f);
            GameObject.Find("skill_cd_bottom").transform.localPosition = vector;
            GameObject.Find("skill_cd_armin").transform.localPosition = vector;
            GameObject.Find("skill_cd_eren").transform.localPosition = vector;
            GameObject.Find("skill_cd_jean").transform.localPosition = vector;
            GameObject.Find("skill_cd_levi").transform.localPosition = vector;
            GameObject.Find("skill_cd_marco").transform.localPosition = vector;
            GameObject.Find("skill_cd_mikasa").transform.localPosition = vector;
            GameObject.Find("skill_cd_petra").transform.localPosition = vector;
            GameObject.Find("skill_cd_sasha").transform.localPosition = vector;
            GameObject.Find("GasUI").transform.localPosition = vector;
            GameObject.Find("stamina_titan").transform.localPosition = new Vector3(-160f, (float) ((int) ((-Screen.height * 0.5f) + 15f)), 0f);
            GameObject.Find("stamina_titan_bottom").transform.localPosition = new Vector3(-160f, (float) ((int) ((-Screen.height * 0.5f) + 15f)), 0f);
        }
        else
        {
            GameObject.Find("skill_cd_bottom").transform.localPosition = new Vector3(0f, (float) ((int) ((-Screen.height * 0.5f) + 5f)), 0f);
            GameObject.Find("GasUI").transform.localPosition = GameObject.Find("skill_cd_bottom").transform.localPosition;
            GameObject.Find("stamina_titan").transform.localPosition = new Vector3(0f, 9999f, 0f);
            GameObject.Find("stamina_titan_bottom").transform.localPosition = new Vector3(0f, 9999f, 0f);
        }
        if ((this.main_object != null) && (this.main_object.GetComponent<Hero>() != null))
        {
            if ((this.main_object.GetPhotonView() != null) && this.main_object.GetPhotonView().isMine)
            {
                this.main_object.GetComponent<Hero>().setSkillHUDPosition2();
            }
        }
        if (stereoType == STEREO_3D_TYPE.SIDE_BY_SIDE)
        {
            base.gameObject.GetComponent<Camera>().aspect = Screen.width / Screen.height;
        }
        this.createSnapShotRT2();
    }

    public GameObject setMainObject(GameObject obj, bool resetRotation = true, bool lockAngle = false)
    {
        float num;
        this.main_object = obj;
        if (obj == null)
        {
            this.head = null;
            num = 1f;
            this.heightMulti = 1f;
            this.distanceMulti = num;
        }
        else if (this.main_object.transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/neck/head") != null)
        {
            this.head = this.main_object.transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/neck/head");
            this.distanceMulti = (this.head != null) ? (Vector3.Distance(this.head.transform.position, this.main_object.transform.position) * 0.2f) : 1f;
            this.heightMulti = (this.head != null) ? (Vector3.Distance(this.head.transform.position, this.main_object.transform.position) * 0.33f) : 1f;
            if (resetRotation)
            {
                base.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            }
        }
        else if (this.main_object.transform.Find("Amarture/Controller_Body/hip/spine/chest/neck/head") != null)
        {
            this.head = this.main_object.transform.Find("Amarture/Controller_Body/hip/spine/chest/neck/head");
            num = 0.64f;
            this.heightMulti = 0.64f;
            this.distanceMulti = num;
            if (resetRotation)
            {
                base.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            }
        }
        else
        {
            this.head = null;
            num = 1f;
            this.heightMulti = 1f;
            this.distanceMulti = num;
            if (resetRotation)
            {
                base.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            }
        }
        this.lockAngle = lockAngle;
        return obj;
    }

    public GameObject setMainObjectASTITAN(GameObject obj)
    {
        this.main_object = obj;
        if (this.main_object.transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/neck/head") != null)
        {
            this.head = this.main_object.transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/neck/head");
            this.distanceMulti = (this.head != null) ? (Vector3.Distance(this.head.transform.position, this.main_object.transform.position) * 0.4f) : 1f;
            this.heightMulti = (this.head != null) ? (Vector3.Distance(this.head.transform.position, this.main_object.transform.position) * 0.45f) : 1f;
            base.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        }
        return obj;
    }

    public void setSpectorMode(bool val)
    {
        this.spectatorMode = val;
        GameObject.Find("MainCamera").GetComponent<SpectatorMovement>().disable = !val;
        GameObject.Find("MainCamera").GetComponent<MouseLook>().disable = !val;
    }
    public void snapShot2(int index)
    {
        Vector3 vector;
        RaycastHit hit;
        this.snapShotCamera.transform.position = (this.head == null) ? this.main_object.transform.position : this.head.transform.position;
        Transform transform = this.snapShotCamera.transform;
        transform.position += (Vector3) (Vector3.up * this.heightMulti);
        Transform transform2 = this.snapShotCamera.transform;
        transform2.position -= (Vector3) (Vector3.up * 1.1f);
        Vector3 worldPosition = vector = this.snapShotCamera.transform.position;
        Vector3 vector3 = (Vector3) ((worldPosition + this.snapShotTargetPosition) * 0.5f);
        this.snapShotCamera.transform.position = vector3;
        worldPosition = vector3;
        this.snapShotCamera.transform.LookAt(this.snapShotTargetPosition);
        float rotation = index == 3 ? Random.Range(-180f, 180f) : Random.Range(-20f, 20f);
        this.snapShotCamera.transform.RotateAround(base.transform.position, Vector3.up, rotation);
        this.snapShotCamera.transform.LookAt(worldPosition);
        this.snapShotCamera.transform.RotateAround(worldPosition, base.transform.right, UnityEngine.Random.Range((float) -20f, (float) 20f));
        float num = Vector3.Distance(this.snapShotTargetPosition, vector);
        if ((this.snapShotTarget != null) && (this.snapShotTarget.GetComponent<MindlessTitan>() != null))
        {
            num += ((index - 1) * this.snapShotTarget.transform.localScale.x) * 10f;
        }
        Transform transform3 = this.snapShotCamera.transform;
        transform3.position -= (Vector3) (this.snapShotCamera.transform.forward * UnityEngine.Random.Range((float) (num + 3f), (float) (num + 10f)));
        this.snapShotCamera.transform.LookAt(worldPosition);
        this.snapShotCamera.transform.RotateAround(worldPosition, base.transform.forward, UnityEngine.Random.Range((float) -30f, (float) 30f));
        Vector3 end = (this.head == null) ? this.main_object.transform.position : this.head.transform.position;
        Vector3 vector5 = ((this.head == null) ? this.main_object.transform.position : this.head.transform.position) - this.snapShotCamera.transform.position;
        end -= vector5;
        LayerMask mask = ((int) 1) << LayerMask.NameToLayer("Ground");
        LayerMask mask2 = ((int) 1) << LayerMask.NameToLayer("EnemyBox");
        LayerMask mask3 = mask | mask2;
        if (this.head != null)
        {
            if (Physics.Linecast(this.head.transform.position, end, out hit, (int) mask))
            {
                this.snapShotCamera.transform.position = hit.point;
            }
            else if (Physics.Linecast(this.head.transform.position - ((Vector3) ((vector5 * this.distanceMulti) * 3f)), end, out hit, (int) mask3))
            {
                this.snapShotCamera.transform.position = hit.point;
            }
        }
        else if (Physics.Linecast(this.main_object.transform.position + Vector3.up, end, out hit, (int) mask3))
        {
            this.snapShotCamera.transform.position = hit.point;
        }
        switch (index)
        {
            case 1:
                this.snapshot1 = this.RTImage2(this.snapShotCamera.GetComponent<Camera>());
                SnapShotSaves.addIMG(this.snapshot1, this.snapShotDmg);
                break;

            case 2:
                this.snapshot2 = this.RTImage2(this.snapShotCamera.GetComponent<Camera>());
                SnapShotSaves.addIMG(this.snapshot2, this.snapShotDmg);
                break;

            case 3:
                this.snapshot3 = this.RTImage2(this.snapShotCamera.GetComponent<Camera>());
                SnapShotSaves.addIMG(this.snapshot3, this.snapShotDmg);
                break;
        }
        this.snapShotCount = index;
        this.hasSnapShot = true;
        this.snapShotCountDown = 2f;
        if (index == 1)
        {
            //GameObject.Find("UI_IN_GAME").GetComponent<UIReferArray>().panels[0].transform.Find("snapshot1").GetComponent<UITexture>().mainTexture = this.snapshot1;
            //GameObject.Find("UI_IN_GAME").GetComponent<UIReferArray>().panels[0].transform.Find("snapshot1").GetComponent<UITexture>().transform.localScale = new Vector3(Screen.width * 0.4f, Screen.height * 0.4f, 1f);
            //GameObject.Find("UI_IN_GAME").GetComponent<UIReferArray>().panels[0].transform.Find("snapshot1").GetComponent<UITexture>().transform.localPosition = new Vector3(-Screen.width * 0.225f, Screen.height * 0.225f, 0f);
            //GameObject.Find("UI_IN_GAME").GetComponent<UIReferArray>().panels[0].transform.Find("snapshot1").GetComponent<UITexture>().transform.rotation = Quaternion.Euler(0f, 0f, 10f);
            //if (PlayerPrefs.HasKey("showSSInGame") && (PlayerPrefs.GetInt("showSSInGame") == 1))
            //{
            //    GameObject.Find("UI_IN_GAME").GetComponent<UIReferArray>().panels[0].transform.Find("snapshot1").GetComponent<UITexture>().enabled = true;
            //}
            //else
            //{
            //    GameObject.Find("UI_IN_GAME").GetComponent<UIReferArray>().panels[0].transform.Find("snapshot1").GetComponent<UITexture>().enabled = false;
            //}
        }
    }

    public void snapShotUpdate()
    {
        if (this.startSnapShotFrameCount)
        {
            this.snapShotStartCountDownTime -= Time.deltaTime;
            if (this.snapShotStartCountDownTime <= 0f)
            {
                this.snapShot2(1);
                this.startSnapShotFrameCount = false;
            }
        }
        if (this.hasSnapShot)
        {
            this.snapShotCountDown -= Time.deltaTime;
            if (this.snapShotCountDown <= 0f)
            {
                //GameObject.Find("UI_IN_GAME").GetComponent<UIReferArray>().panels[0].transform.Find("snapshot1").GetComponent<UITexture>().enabled = false;
                this.hasSnapShot = false;
                this.snapShotCountDown = 0f;
            }
            else if (this.snapShotCountDown < 1f)
            {
                //GameObject.Find("UI_IN_GAME").GetComponent<UIReferArray>().panels[0].transform.Find("snapshot1").GetComponent<UITexture>().mainTexture = this.snapshot3;
            }
            else if (this.snapShotCountDown < 1.5f)
            {
                //GameObject.Find("UI_IN_GAME").GetComponent<UIReferArray>().panels[0].transform.Find("snapshot1").GetComponent<UITexture>().mainTexture = this.snapshot2;
            }
            if (this.snapShotCount < 3)
            {
                this.snapShotInterval -= Time.deltaTime;
                if (this.snapShotInterval <= 0f)
                {
                    this.snapShotInterval = 0.05f;
                    this.snapShotCount++;
                    this.snapShot2(this.snapShotCount);
                }
            }
        }
    }

    public void startShake(float R, float duration, float decay = 0.95f)
    {
        if (this.duration < duration)
        {
            this.R = R;
            this.duration = duration;
            this.decay = decay;
        }
    }

    public void startSnapShot2(Vector3 p, int dmg, GameObject target, float startTime)
    {
        int num;
        if (int.TryParse((string) FengGameManagerMKII.settings[0x5f], out num))
        {
            if (dmg >= num)
            {
                this.snapShotCount = 1;
                this.startSnapShotFrameCount = true;
                this.snapShotTargetPosition = p;
                this.snapShotTarget = target;
                this.snapShotStartCountDownTime = startTime;
                this.snapShotInterval = 0.05f + UnityEngine.Random.Range((float) 0f, (float) 0.03f);
                this.snapShotDmg = dmg;
            }
        }
        else
        {
            this.snapShotCount = 1;
            this.startSnapShotFrameCount = true;
            this.snapShotTargetPosition = p;
            this.snapShotTarget = target;
            this.snapShotStartCountDownTime = startTime;
            this.snapShotInterval = 0.05f + UnityEngine.Random.Range((float) 0f, (float) 0.03f);
            this.snapShotDmg = dmg;
        }
    }

    public void Update()
    {
        snapShotUpdate();
        if (this.flashDuration > 0f)
        {
            this.flashDuration -= Time.deltaTime;
            if (this.flashDuration <= 0f)
            {
                this.flashDuration = 0f;
            }

            //GameObject.Find("flash").GetComponent<UISprite>().alpha = this.flashDuration * 0.5f;
        }
        if (gametype != GAMETYPE.Stop)
        {

            if (this.gameOver)
            {

                this.setSpectorMode(true);
                //TODO #160
                //FengGameManagerMKII.instance.ShowHUDInfoCenter(
                //$"Press <color=#f7d358>{InputManager.GetKey(InputHuman.Item1)}</color> to toggle the spawn menu.\n" +
                //$"Press <color=#f7d358>{InputManager.GetKey(InputHuman.Item2)}</color> to spectate the next player.\n" +
                //$"Press <color=#f7d358>{InputManager.GetKey(InputHuman.Item3)}</color> to spectate the previous player.\n");
                if (InputManager.KeyDown(InputHuman.Item1))
                {
                    ToggleSpecMode();
                    ToggleSpawnMenu();
                }
                if (this.spectatorMode)
                {
                    if (InputManager.KeyDown(InputHuman.Item2))
                    {
                        this.currentPeekPlayerIndex++;
                        int length = GameObject.FindGameObjectsWithTag("Player").Length;
                        if (this.currentPeekPlayerIndex >= length)
                        {
                            this.currentPeekPlayerIndex = 0;
                        }
                        if (length > 0)
                        {
                            this.setMainObject(GameObject.FindGameObjectsWithTag("Player")[this.currentPeekPlayerIndex], true, false);
                            this.setSpectorMode(false);
                            this.lockAngle = false;
                        }

                    }

                    if (InputManager.KeyDown(InputHuman.Item3))
                    {
                        this.currentPeekPlayerIndex--;
                        int num2 = GameObject.FindGameObjectsWithTag("Player").Length;
                        if (this.currentPeekPlayerIndex >= num2)
                        {
                            this.currentPeekPlayerIndex = 0;
                        }
                        if (this.currentPeekPlayerIndex < 0)
                        {
                            this.currentPeekPlayerIndex = num2;
                        }
                        if (num2 > 0)
                        {
                            this.setMainObject(GameObject.FindGameObjectsWithTag("Player")[this.currentPeekPlayerIndex], true, false);
                            this.setSpectorMode(false);
                            this.lockAngle = false;
                        }
                    }

                    if (this.spectatorMode)
                    {
                        return;
                    }
                }
            }
            //TODO #204 - Pause Menu
            //if (InputManager.KeyDown(InputUi.Pause))
            //{
            //    if (isPausing)
            //    {
            //        if (this.main_object != null)
            //        {
            //            Vector3 position = base.transform.position;
            //            position = (this.head == null) ? this.main_object.transform.position : this.head.transform.position;
            //            position += (Vector3)(Vector3.up * this.heightMulti);
            //            base.transform.position = Vector3.Lerp(base.transform.position, position - ((Vector3)(base.transform.forward * 5f)), 0.2f);
            //        }
            //        return;
            //    }
            //    isPausing = !isPausing;
            //    if (isPausing)
            //    {
            //        if (gametype == GAMETYPE.SINGLE)
            //        {
            //            Time.timeScale = 0f;
            //        }
            //        //TODO: Pausing menu disable input
            //        //GameObject.Find("InputManagerController").GetComponent<FengCustomInputs>().menuOn = true;
            //    }
            //}
            if (this.needSetHUD)
            {
                this.needSetHUD = false;
                this.setHUDposition();
            }
            if (InputManager.KeyDown(InputUi.Fullscreen))
            {
                Screen.fullScreen = !Screen.fullScreen;
                if (Screen.fullScreen)
                {
                    Screen.SetResolution(960, 600, false);
                }
                else
                {
                    Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, true);
                }
                this.needSetHUD = true;
            }
            if (isRestarting && Time.time - startingTime >= 0.5f && !InputManager.Key(InputUi.Restart))
                isRestarting = false;

            if (InputManager.KeyDown(InputUi.Restart) && PhotonNetwork.offlineMode && !isRestarting)
            {
                FengGameManagerMKII.instance.restartRC();
            }
            if (this.main_object != null)
            {
                RaycastHit hit;
                if (InputManager.KeyDown(InputUi.Camera))
                    GameCursor.Cycle();

                if (InputManager.KeyDown(InputUi.ToggleCursor))
                    GameCursor.ForceFreeCursor = !GameCursor.ForceFreeCursor;

                if (InputManager.KeyDown(InputHuman.Focus))
                {
                    if (Service.Player.Self is TitanBase) return;
                    triggerAutoLock = !triggerAutoLock;
                    if (triggerAutoLock)
                    {
                        this.lockTarget = this.findNearestTitan();
                        if (this.closestDistance >= 150f)
                        {
                            this.lockTarget = null;
                            triggerAutoLock = false;
                        }
                    }
                }
                if (this.gameOver && (this.main_object != null))
                {
                    if (InputManager.KeyDown(InputUi.LiveCamera))
                    {
                        if (((int) FengGameManagerMKII.settings[0x107]) == 0)
                        {
                            FengGameManagerMKII.settings[0x107] = 1;
                        }
                        else
                        {
                            FengGameManagerMKII.settings[0x107] = 0;
                        }
                    }
                    Hero component = this.main_object.GetComponent<Hero>();
                    if ((((component != null) && (((int) FengGameManagerMKII.settings[0x107]) == 1)) && component.GetComponent<SmoothSyncMovement>().enabled) && component.isPhotonCamera)
                    {
                        this.CameraMovementLive(component);
                    }
                    else if (this.lockAngle)
                    {
                        base.transform.rotation = Quaternion.Lerp(base.transform.rotation, this.main_object.transform.rotation, 0.2f);
                        base.transform.position = Vector3.Lerp(base.transform.position, this.main_object.transform.position - ((Vector3) (this.main_object.transform.forward * 5f)), 0.2f);
                    }
                    else
                    {
                        this.DoCameraMovement();
                    }
                }
                else
                {
                    this.DoCameraMovement();
                }
                if (triggerAutoLock && (this.lockTarget != null))
                {
                    float z = base.transform.eulerAngles.z;
                    Transform transform = this.lockTarget.transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/neck");
                    Vector3 vector2 = transform.position - ((this.head == null) ? this.main_object.transform.position : this.head.transform.position);
                    vector2.Normalize();
                    this.lockCameraPosition = (this.head == null) ? this.main_object.transform.position : this.head.transform.position;
                    this.lockCameraPosition -= (Vector3) (((vector2 * this.distance) * this.distanceMulti) * this.distanceOffsetMulti);
                    this.lockCameraPosition += (Vector3) (((Vector3.up * 3f) * this.heightMulti) * this.distanceOffsetMulti);
                    base.transform.position = Vector3.Lerp(base.transform.position, this.lockCameraPosition, Time.deltaTime * 4f);
                    if (this.head != null)
                    {
                        base.transform.LookAt((Vector3) ((this.head.transform.position * 0.8f) + (transform.position * 0.2f)));
                    }
                    else
                    {
                        base.transform.LookAt((Vector3) ((this.main_object.transform.position * 0.8f) + (transform.position * 0.2f)));
                    }
                    base.transform.localEulerAngles = new Vector3(base.transform.eulerAngles.x, base.transform.eulerAngles.y, z);
                    Vector2 vector3 = base.GetComponent<Camera>().WorldToScreenPoint(transform.position - ((Vector3) (transform.forward * this.lockTarget.transform.localScale.x)));
                    // TODO: Plan reimplementation of lock-on feature.
                    //this.locker.transform.localPosition = new Vector3(vector3.x - (Screen.width * 0.5f), vector3.y - (Screen.height * 0.5f), 0f);
                    if ((this.lockTarget.GetComponent<MindlessTitan>() != null) && !lockTarget.GetComponent<MindlessTitan>().IsAlive)
                    {
                        this.lockTarget = null;
                    }
                }
                else
                {
                    //HACK
                    //this.locker.transform.localPosition = new Vector3(0f, (-Screen.height * 0.5f) - 50f, 0f);
                }
                Vector3 end = (this.head == null) ? this.main_object.transform.position : this.head.transform.position;
                Vector3 vector5 = ((this.head == null) ? this.main_object.transform.position : this.head.transform.position) - base.transform.position;
                Vector3 normalized = vector5.normalized;
                end -= (Vector3) ((this.distance * normalized) * this.distanceMulti);
                LayerMask mask = ((int) 1) << LayerMask.NameToLayer("Ground");
                LayerMask mask2 = ((int) 1) << LayerMask.NameToLayer("EnemyBox");
                LayerMask mask3 = mask | mask2;
                if (this.head != null)
                {
                    if (Physics.Linecast(this.head.transform.position, end, out hit, (int) mask))
                    {
                        base.transform.position = hit.point;
                    }
                    else if (Physics.Linecast(this.head.transform.position - ((Vector3) ((normalized * this.distanceMulti) * 3f)), end, out hit, (int) mask2))
                    {
                        base.transform.position = hit.point;
                    }
                    Debug.DrawLine(this.head.transform.position - ((Vector3) ((normalized * this.distanceMulti) * 3f)), end, Color.red);
                }
                else if (Physics.Linecast(this.main_object.transform.position + Vector3.up, end, out hit, (int) mask3))
                {
                    base.transform.position = hit.point;
                }
                this.shakeUpdate();
            }
        }
    }

    public static void ToggleSpecMode()
    {
        settings[0xf5] = (int) settings[0xf5] == 1 ? 0 : 1;
        bool specMode = (int) settings[0xf5] == 1;
        instance.EnterSpecMode(specMode);
        string message = specMode ? "You have entered spectator mode." : "You have exited spectator mode.";
        instance.chatRoom.OutputSystemMessage(message);
    }

    public static void ToggleSpawnMenu()
    {
        var spawnMenu = FengGameManagerMKII.instance.InGameUI.SpawnMenu.gameObject;
        spawnMenu.SetActive(!spawnMenu.activeSelf);
    }

    private void DoCameraMovement()
    {
        distanceOffsetMulti = InputManager.Settings.CameraDistance * (200f - GetComponent<Camera>().fieldOfView) / 150f;
        transform.position = (head == null) ? main_object.transform.position : head.transform.position;
        transform.position += Vector3.up * heightMulti;
        transform.position -= Vector3.up * (0.6f - InputManager.Settings.CameraDistance) * 2f;

        switch (GameCursor.CameraMode)
        {
            case CameraMode.Original:
                DoOriginalMovement();
                break;

            case CameraMode.TPS:
                DoTPSMovement();
                break;

            case CameraMode.WOW:
                DoWOWMovement();
                break;

            default:
                Debug.LogError($"{GameCursor.CameraMode} is an unhandled camera mode - Original, TPS, or WOW was expected");
                break;
        }

        if (InputManager.Settings.CameraDistance < 0.65f)
        {
            Transform transform6 = base.transform;
            transform6.position += (Vector3) (base.transform.right * Mathf.Max((float) ((0.6f - InputManager.Settings.CameraDistance) * 2f), (float) 0.65f));
        }
    }

    private void DoTPSMovement()
    {
        float num5 = (Input.GetAxis("Mouse X") * 10f) * this.getSensitivityMulti();
        float num6 = ((-Input.GetAxis("Mouse Y") * 10f) * this.getSensitivityMulti()) * this.getReverse();
        base.transform.RotateAround(base.transform.position, Vector3.up, num5);
        float num7 = base.transform.rotation.eulerAngles.x % 360f;
        float num8 = num7 + num6;
        if (((num6 <= 0f) || (((num7 >= 260f) || (num8 <= 260f)) && ((num7 >= 80f) || (num8 <= 80f)))) && ((num6 >= 0f) || (((num7 <= 280f) || (num8 >= 280f)) && ((num7 <= 100f) || (num8 >= 100f)))))
        {
            base.transform.RotateAround(base.transform.position, base.transform.right, num6);
        }
        Transform transform5 = base.transform;
        transform5.position -= (Vector3) (((base.transform.forward * this.distance) * this.distanceMulti) * this.distanceOffsetMulti);
    }

    private void DoOriginalMovement()
    {
        float num3 = 0f;
        if (Input.mousePosition.x < (Screen.width * 0.4f))
        {
            num3 = (-((((Screen.width * 0.4f) - Input.mousePosition.x) / ((float) Screen.width)) * 0.4f) * this.getSensitivityMultiWithDeltaTime()) * 150f;
            base.transform.RotateAround(base.transform.position, Vector3.up, num3);
        }
        else if (Input.mousePosition.x > (Screen.width * 0.6f))
        {
            num3 = ((((Input.mousePosition.x - (Screen.width * 0.6f)) / ((float) Screen.width)) * 0.4f) * this.getSensitivityMultiWithDeltaTime()) * 150f;
            base.transform.RotateAround(base.transform.position, Vector3.up, num3);
        }
        float x = ((140f * ((Screen.height * 0.6f) - Input.mousePosition.y)) / ((float) Screen.height)) * 0.5f;
        base.transform.rotation = Quaternion.Euler(x, base.transform.rotation.eulerAngles.y, base.transform.rotation.eulerAngles.z);
        Transform transform4 = base.transform;
        transform4.position -= (Vector3) (((base.transform.forward * this.distance) * this.distanceMulti) * this.distanceOffsetMulti);
    }

    private void DoWOWMovement()
    {
        if (Input.GetKey(KeyCode.Mouse1))
        {
            float angle = (Input.GetAxis("Mouse X") * 10f) * this.getSensitivityMulti();
            float num2 = ((-Input.GetAxis("Mouse Y") * 10f) * this.getSensitivityMulti()) * this.getReverse();
            base.transform.RotateAround(base.transform.position, Vector3.up, angle);
            base.transform.RotateAround(base.transform.position, base.transform.right, num2);
        }
        Transform transform3 = base.transform;
        transform3.position -= (Vector3) (((base.transform.forward * this.distance) * this.distanceMulti) * this.distanceOffsetMulti);
    }

    private GameObject findNearestTitan()
    {
        GameObject[] objArray = GameObject.FindGameObjectsWithTag("titan");
        GameObject obj2 = null;
        float positiveInfinity = float.PositiveInfinity;
        this.closestDistance = float.PositiveInfinity;
        float num2 = positiveInfinity;
        Vector3 position = this.main_object.transform.position;
        foreach (GameObject obj3 in objArray)
        {
            // TODO: Optimize accessing all titan positions.
            // Possibly another class that maintains a list of active titans,
            // removes them from the list when they die.
            Vector3 vector2 = obj3.transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/neck").position - position;
            float magnitude = vector2.magnitude;
            if ((magnitude < num2) && ((obj3.GetComponent<MindlessTitan>() == null) || obj3.GetComponent<MindlessTitan>().IsAlive))
            {
                obj2 = obj3;
                num2 = magnitude;
                this.closestDistance = num2;
            }
        }
        return obj2;
    }

    private int getReverse()
    {
        return InputManager.Settings.MouseInvert
            ? -1
            : 1;
    }

    private float getSensitivityMulti()
    {
        return InputManager.Settings.MouseSensitivity;
    }

    private float getSensitivityMultiWithDeltaTime()
    {
        return InputManager.Settings.MouseSensitivity * Time.deltaTime * 62f;
    }
    
    private Texture2D RTImage2(Camera cam)
    {
        RenderTexture active = RenderTexture.active;
        RenderTexture.active = cam.targetTexture;
        cam.Render();
        Texture2D textured = new Texture2D(cam.targetTexture.width, cam.targetTexture.height);
        int num = (int) (cam.targetTexture.width * 0.04f);
        int destX = (int) (cam.targetTexture.width * 0.02f);
        try
        {
            textured.SetPixel(0, 0, Color.white);
            textured.ReadPixels(new Rect((float) num, (float) num, (float) (cam.targetTexture.width - num), (float) (cam.targetTexture.height - num)), destX, destX);
            textured.Apply();
            RenderTexture.active = active;
        }
        catch
        {
            textured = new Texture2D(1, 1);
            textured.SetPixel(0, 0, Color.white);
            return textured;
        }
        return textured;
    }

    private void shakeUpdate()
    {
        if (this.duration > 0f)
        {
            this.duration -= Time.deltaTime;
            if (this.flip)
            {
                Transform transform = base.gameObject.transform;
                transform.position += (Vector3) (Vector3.up * this.R);
            }
            else
            {
                Transform transform2 = base.gameObject.transform;
                transform2.position -= (Vector3) (Vector3.up * this.R);
            }
            this.flip = !this.flip;
            this.R *= this.decay;
        }
    }

    private void Start()
    {
        GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>().addCamera(this);
        isPausing = false;
        this.setDayLight(dayLight);

        // This doesn't exist in the scene and causes a NullReferenceException.
        // TODO: Fix titan locking
        this.locker = GameObject.Find("locker");
        this.createSnapShotRT2();
    }

    private void OnDestroy()
    {
        EntityService.OnRegister -= EntityService_OnRegistered;
    }
}