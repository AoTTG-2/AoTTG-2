using Assets.Scripts;
using Assets.Scripts.Characters;
using Assets.Scripts.Characters.Humans;
using Assets.Scripts.Characters.Titan;
using Assets.Scripts.Services;
using Assets.Scripts.Services.Interface;
using Assets.Scripts.UI.Camera;
using Assets.Scripts.UI.InGame;
using Assets.Scripts.UI.InGame.HUD;
using Assets.Scripts.UI.Input;
using Assets.Scripts.Utility;
using System;
using UnityEngine;
using static Assets.Scripts.FengGameManagerMKII;
using Random = UnityEngine.Random;

public class IN_GAME_MAIN_CAMERA : MonoBehaviour
{
    private IEntityService EntityService => Service.Entity;

    /// <summary>
    /// The maximum distance to the closest titan to allow locking.
    /// <seealso cref="FindNearestLockableTitan"/>
    /// </summary>
    private readonly float lockableDistance = 150f;
    private int currentPeekPlayerIndex;
    [Obsolete("Replace with a Time Service")]
    public static DayLight dayLight = DayLight.Dawn;
    private float decay;
    private float distance = 10f;
    [SerializeField]
    private float distanceMulti;
    [SerializeField]
    private float distanceOffsetMulti;
    private float duration;
    private float flashDuration;
    private bool flip;
    public bool gameOver;
    [Obsolete("Refactor so that this static field is no longer required")]
    public static GAMETYPE gametype = GAMETYPE.Stop;
    private bool hasSnapShot;
    private Transform head;
    [SerializeField]
    private float heightMulti;
    [Obsolete("This is always false")]
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
    
    /// <summary>
    /// Whether the game window is in focus.
    /// <seealso cref="DoCameraMovement"/>
    /// </summary>
    private bool appInFocus = true;

    private void OnApplicationFocus(bool focus) => appInFocus = focus;

    public GameObject HUD;
    private void Awake()
    {
        EntityService.OnRegister += EntityService_OnRegistered;

        isTyping = false;
        isPausing = false;
        name = "MainCamera";
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
            SetMainObjectAsTitan(pt.gameObject);
            enabled = true;
            SpectatorMode.Disable();
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
        base.transform.position = (main_object.transform.position + (Vector3.up * heightMulti)) - ((Vector3.up * (0.6f - InputManager.Settings.CameraDistance)) * 2f);
        Transform transform = base.transform;
        transform.position -= ((base.transform.forward * distance) * distanceMulti) * num2;
        if (hero.CameraMultiplier < 0.65f)
        {
            Transform transform2 = base.transform;
            transform2.position += base.transform.right * Mathf.Max((0.6f - hero.CameraMultiplier) * 2f, 0.65f);
        }
        base.transform.rotation = Quaternion.Lerp(Camera.main.transform.rotation, hero.GetComponent<SmoothSyncMovement>().correctCameraRot, Time.deltaTime * 5f);
    }

    public void CreateSnapShotRT2()
    {
        if (snapshotRT != null)
        {
            snapshotRT.Release();
        }
        if (QualitySettings.GetQualityLevel() > 3)
        {
            snapshotRT = new RenderTexture((int) (Screen.width * 0.8f), (int) (Screen.height * 0.8f), 0x18);
            snapShotCamera.GetComponent<Camera>().targetTexture = snapshotRT;
        }
        else
        {
            snapshotRT = new RenderTexture((int) (Screen.width * 0.4f), (int) (Screen.height * 0.4f), 0x18);
            snapShotCamera.GetComponent<Camera>().targetTexture = snapshotRT;
        }
    }

    public void FlashBlind()
    {
        //GameObject.Find("flash").GetComponent<UISprite>().alpha = 1f;
        //this.flashDuration = 2f;
    }

    public GameObject SetMainObject(GameObject obj, bool resetRotation = true, bool lockAngle = false)
    {
        float num;
        main_object = obj;
        if (obj == null)
        {
            head = null;
            num = 1f;
            heightMulti = 1f;
            distanceMulti = num;
        }
        else if (main_object.transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/neck/head") != null)
        {
            head = main_object.transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/neck/head");
            distanceMulti = (head != null) ? (Vector3.Distance(head.transform.position, main_object.transform.position) * 0.2f) : 1f;
            heightMulti = (head != null) ? (Vector3.Distance(head.transform.position, main_object.transform.position) * 0.33f) : 1f;
            if (resetRotation)
            {
                transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            }
        }
        else if (main_object.transform.Find("Amarture/Controller_Body/hip/spine/chest/neck/head") != null)
        {
            head = main_object.transform.Find("Amarture/Controller_Body/hip/spine/chest/neck/head");
            num = 0.64f;
            heightMulti = 0.64f;
            distanceMulti = num;
            if (resetRotation)
            {
                transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            }
        }
        else
        {
            head = null;
            num = 1f;
            heightMulti = 1f;
            distanceMulti = num;
            if (resetRotation)
            {
                transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            }
        }
        this.lockAngle = lockAngle;
        return obj;
    }

    public GameObject SetMainObjectAsTitan(GameObject obj)
    {
        main_object = obj;
        if (main_object.transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/neck/head") != null)
        {
            head = main_object.transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/neck/head");
            distanceMulti = (head != null) ? (Vector3.Distance(head.transform.position, main_object.transform.position) * 0.4f) : 1f;
            heightMulti = (head != null) ? (Vector3.Distance(head.transform.position, main_object.transform.position) * 0.45f) : 1f;
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        }
        return obj;
    }

    public void SetSpectorMode(bool value)
    {
        spectatorMode = value;
        SpectatorMode.SetState(value);
        GameObject.Find("MainCamera").GetComponent<MouseLook>().disable = !value;
    }
    public void SnapShot2(int index)
    {
        Vector3 vector;
        RaycastHit hit;
        snapShotCamera.transform.position = (head == null) ? main_object.transform.position : head.transform.position;
        Transform transform = snapShotCamera.transform;
        transform.position += (Vector3.up * heightMulti);
        Transform transform2 = snapShotCamera.transform;
        transform2.position -= (Vector3.up * 1.1f);
        Vector3 worldPosition = vector = snapShotCamera.transform.position;
        Vector3 vector3 = ((worldPosition + snapShotTargetPosition) * 0.5f);
        snapShotCamera.transform.position = vector3;
        worldPosition = vector3;
        snapShotCamera.transform.LookAt(snapShotTargetPosition);
        float rotation = index == 3 ? Random.Range(-180f, 180f) : Random.Range(-20f, 20f);
        snapShotCamera.transform.RotateAround(base.transform.position, Vector3.up, rotation);
        snapShotCamera.transform.LookAt(worldPosition);
        snapShotCamera.transform.RotateAround(worldPosition, base.transform.right, Random.Range(-20f, 20f));
        float num = Vector3.Distance(snapShotTargetPosition, vector);
        if ((snapShotTarget != null) && (snapShotTarget.GetComponent<MindlessTitan>() != null))
        {
            num += ((index - 1) * snapShotTarget.transform.localScale.x) * 10f;
        }
        Transform transform3 = snapShotCamera.transform;
        transform3.position -= snapShotCamera.transform.forward * Random.Range(num + 3f, num + 10f);
        snapShotCamera.transform.LookAt(worldPosition);
        snapShotCamera.transform.RotateAround(worldPosition, base.transform.forward, Random.Range(-30f, 30f));
        Vector3 end = (head == null) ? main_object.transform.position : head.transform.position;
        Vector3 vector5 = ((head == null) ? main_object.transform.position : head.transform.position) - snapShotCamera.transform.position;
        end -= vector5;
        LayerMask mask = 1 << LayerMask.NameToLayer("Ground");
        LayerMask mask2 = 1 << LayerMask.NameToLayer("EnemyBox");
        LayerMask mask3 = mask | mask2;
        if (head != null)
        {
            if (Physics.Linecast(head.transform.position, end, out hit, mask))
            {
                snapShotCamera.transform.position = hit.point;
            }
            else if (Physics.Linecast(head.transform.position - ((vector5 * distanceMulti) * 3f), end, out hit, mask3))
            {
                snapShotCamera.transform.position = hit.point;
            }
        }
        else if (Physics.Linecast(main_object.transform.position + Vector3.up, end, out hit, mask3))
        {
            snapShotCamera.transform.position = hit.point;
        }
        switch (index)
        {
            case 1:
                snapshot1 = RTImage2(snapShotCamera.GetComponent<Camera>());
                break;

            case 2:
                snapshot2 = RTImage2(snapShotCamera.GetComponent<Camera>());
                break;

            case 3:
                snapshot3 = RTImage2(snapShotCamera.GetComponent<Camera>());
                break;
        }
        snapShotCount = index;
        hasSnapShot = true;
        snapShotCountDown = 2f;
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

    public void SnapShotUpdate()
    {
        if (startSnapShotFrameCount)
        {
            snapShotStartCountDownTime -= Time.deltaTime;
            if (snapShotStartCountDownTime <= 0f)
            {
                SnapShot2(1);
                startSnapShotFrameCount = false;
            }
        }
        if (hasSnapShot)
        {
            snapShotCountDown -= Time.deltaTime;
            if (snapShotCountDown <= 0f)
            {
                //GameObject.Find("UI_IN_GAME").GetComponent<UIReferArray>().panels[0].transform.Find("snapshot1").GetComponent<UITexture>().enabled = false;
                hasSnapShot = false;
                snapShotCountDown = 0f;
            }
            else if (snapShotCountDown < 1f)
            {
                //GameObject.Find("UI_IN_GAME").GetComponent<UIReferArray>().panels[0].transform.Find("snapshot1").GetComponent<UITexture>().mainTexture = this.snapshot3;
            }
            else if (snapShotCountDown < 1.5f)
            {
                //GameObject.Find("UI_IN_GAME").GetComponent<UIReferArray>().panels[0].transform.Find("snapshot1").GetComponent<UITexture>().mainTexture = this.snapshot2;
            }
            if (snapShotCount < 3)
            {
                snapShotInterval -= Time.deltaTime;
                if (snapShotInterval <= 0f)
                {
                    snapShotInterval = 0.05f;
                    snapShotCount++;
                    SnapShot2(snapShotCount);
                }
            }
        }
    }

    public void StartShake(float R, float duration, float decay = 0.95f)
    {
        if (this.duration < duration)
        {
            this.R = R;
            this.duration = duration;
            this.decay = decay;
        }
    }

    public void StartSnapShot2(Vector3 p, int dmg, GameObject target, float startTime)
    {
        int num;
        if (int.TryParse((string) FengGameManagerMKII.settings[0x5f], out num))
        {
            if (dmg >= num)
            {
                snapShotCount = 1;
                startSnapShotFrameCount = true;
                snapShotTargetPosition = p;
                snapShotTarget = target;
                snapShotStartCountDownTime = startTime;
                snapShotInterval = 0.05f + Random.Range(0f, 0.03f);
                snapShotDmg = dmg;
            }
        }
        else
        {
            snapShotCount = 1;
            startSnapShotFrameCount = true;
            snapShotTargetPosition = p;
            snapShotTarget = target;
            snapShotStartCountDownTime = startTime;
            snapShotInterval = 0.05f + Random.Range(0f, 0.03f);
            snapShotDmg = dmg;
        }
    }

    private bool isLocking() => triggerAutoLock && lockTarget != null;

    public void Update()
    {
        if (InputManager.KeyDown(InputUi.HideHUD))
        {
            ToggleHUD();
        }
        SnapShotUpdate();
        if (flashDuration > 0f)
        {
            flashDuration -= Time.deltaTime;
            if (flashDuration <= 0f)
            {
                flashDuration = 0f;
            }

            //GameObject.Find("flash").GetComponent<UISprite>().alpha = this.flashDuration * 0.5f;
        }
        if (gametype != GAMETYPE.Stop)
        {

            if (gameOver)
            {

                SetSpectorMode(true);
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
                if (spectatorMode)
                {
                    if (InputManager.KeyDown(InputHuman.Item2))
                    {
                        currentPeekPlayerIndex++;
                        int length = GameObject.FindGameObjectsWithTag("Player").Length;
                        if (currentPeekPlayerIndex >= length)
                        {
                            currentPeekPlayerIndex = 0;
                        }
                        if (length > 0)
                        {
                            SetMainObject(GameObject.FindGameObjectsWithTag("Player")[currentPeekPlayerIndex], true, false);
                            SetSpectorMode(false);
                            lockAngle = false;
                        }

                    }

                    if (InputManager.KeyDown(InputHuman.Item3))
                    {
                        currentPeekPlayerIndex--;
                        int num2 = GameObject.FindGameObjectsWithTag("Player").Length;
                        if (currentPeekPlayerIndex >= num2)
                        {
                            currentPeekPlayerIndex = 0;
                        }
                        if (currentPeekPlayerIndex < 0)
                        {
                            currentPeekPlayerIndex = num2;
                        }
                        if (num2 > 0)
                        {
                            SetMainObject(GameObject.FindGameObjectsWithTag("Player")[currentPeekPlayerIndex], true, false);
                            SetSpectorMode(false);
                            lockAngle = false;
                        }
                    }

                    if (spectatorMode)
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
            if (needSetHUD)
            {
                needSetHUD = false;
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
                needSetHUD = true;
            }
            if (isRestarting && Time.time - startingTime >= 0.5f && !InputManager.Key(InputUi.Restart))
                isRestarting = false;

            if (InputManager.KeyDown(InputUi.Restart) && PhotonNetwork.offlineMode && !isRestarting)
            {
                FengGameManagerMKII.instance.RestartRound();
            }
            if (main_object != null)
            {
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
                        lockTarget = FindNearestLockableTitan();
                        triggerAutoLock = lockTarget != null;
                    }
                }
                if (gameOver && (main_object != null))
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
                    Hero component = main_object.GetComponent<Hero>();
                    if ((((component != null) && (((int) FengGameManagerMKII.settings[0x107]) == 1)) && component.GetComponent<SmoothSyncMovement>().enabled) && component.isPhotonCamera)
                    {
                        CameraMovementLive(component);
                    }
                    else if (lockAngle)
                    {
                        transform.rotation = Quaternion.Lerp(transform.rotation, main_object.transform.rotation, 0.2f);
                        transform.position = Vector3.Lerp(transform.position, main_object.transform.position - (main_object.transform.forward * 5f), 0.2f);
                    }
                    else
                    {
                        DoCameraMovement();
                    }
                }
                else
                {
                    DoCameraMovement();
                }
                if (isLocking())
                {
                    float originalZAngle = transform.eulerAngles.z;
                    Transform neckTransform = lockTarget.transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/neck");
                    Vector3 toNeck = Vector3.Normalize(neckTransform.position - (head == null ? main_object.transform.position : head.transform.position));
                    lockCameraPosition = head == null ? main_object.transform.position : head.transform.position;
                    lockCameraPosition -= toNeck * distance * distanceMulti * distanceOffsetMulti;
                    lockCameraPosition += Vector3.up * 3f * heightMulti * distanceOffsetMulti;
                    transform.position = Vector3.Lerp(transform.position, lockCameraPosition, Time.deltaTime * 4f);
                    transform.LookAt(neckTransform.position);
                    transform.localEulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, originalZAngle);
                    // TODO: Plan reimplementation of lock-on feature.
                    //this.locker.transform.localPosition = new Vector3(vector3.x - (Screen.width * 0.5f), vector3.y - (Screen.height * 0.5f), 0f);
                    if ((lockTarget.GetComponent<MindlessTitan>() != null) && !lockTarget.GetComponent<MindlessTitan>().IsAlive)
                    {
                        lockTarget = null;
                    }
                }
                else
                {
                    //HACK
                    //this.locker.transform.localPosition = new Vector3(0f, (-Screen.height * 0.5f) - 50f, 0f);
                }
                float radius = Mathf.Tan(Mathf.Deg2Rad * 0.5f * Camera.VerticalToHorizontalFieldOfView(Camera.main.fieldOfView, Camera.main.aspect)) * Camera.main.nearClipPlane;
#if UNITY_EDITOR
                gizRadius = radius;
#endif
                LayerMask mask = 1 << LayerMask.NameToLayer("Ground");
                LayerMask mask2 = 1 << LayerMask.NameToLayer("EnemyBox");
                LayerMask mask3 = mask | mask2;

                Vector3 start = main_object.transform.position;
                Vector3 end = start + (Vector3.up * 1.7f);
                RaycastHit hit;
                if (Physics.Linecast(start, end, out hit, mask3))
                {
                    start = hit.point;
                }
                else
                {
                    start = end;
                }
                start -= Vector3.up * (radius + 0.5f);

                Vector3 direction = Vector3.Normalize(start - transform.position);
                if (main_object != null)
                {
                    if (Physics.SphereCast(start + (direction * radius), radius, -direction, out hit, distance * distanceMulti * distanceOffsetMulti, mask))
                    {
                        transform.position = start - (direction * (hit.distance - radius));
#if UNITY_EDITOR
                        gizSpherePos1 = hit.point;
                        gizSpherePos2 = start - (direction * (hit.distance - radius));
                        Debug.DrawLine(start, start - (direction * (hit.distance - radius)), Color.red);
#endif
                    }
                    else if (Physics.SphereCast(start - ((direction * distanceMulti) * 3f) + (direction * radius), radius, -direction, out hit, distance * distanceMulti, mask2))
                    {
                        transform.position = start - (direction * (hit.distance * distanceMulti + radius));
                    }
                    else
                    {
                        transform.position = start - (direction * (distance * distanceMulti * distanceOffsetMulti - radius));
#if UNITY_EDITOR
                        gizSpherePos2 = start - (direction * (distance * distanceMulti * distanceOffsetMulti - radius));
                        Debug.DrawLine(start + (direction * radius), start - (direction * distance * distanceMulti), Color.red);
#endif
                    }
                }
                else if (Physics.SphereCast(start + Vector3.up, radius, -direction, out hit, distance * distanceMulti, mask3))
                {
                    transform.position = start - (direction * (hit.distance + radius));
                }
                ShakeUpdate();
            }
        }
    }

#if UNITY_EDITOR
    float gizRadius;
    Vector3 gizSpherePos1;
    Vector3 gizSpherePos2;
    private void OnDrawGizmos()
    {
        // Green
        Gizmos.color = new Color(0.3f, 1f, 0.3f);
        Gizmos.DrawWireSphere(gizSpherePos1, gizRadius);
        // Blue
        Gizmos.color = new Color(0.3f, 0.9f, 0.9f);
        Gizmos.DrawWireSphere(gizSpherePos2, gizRadius);
    }
#endif

    public static void ToggleSpecMode()
    {
        SpectatorMode.Toggle();
        SpectatorMode.UpdateSpecMode();
        string message = SpectatorMode.IsEnable() ? "You have entered spectator mode." : "You have exited spectator mode.";
        instance.chatRoom.OutputSystemMessage(message);
    }

    public void ToggleHUD()
    {
        HUD.transform.gameObject.SetActive(!HUD.transform.gameObject.activeInHierarchy);
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

        if (!isLocking() && appInFocus)
        {
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
        }

        transform.position -= ((transform.forward * distance) * distanceMulti) * distanceOffsetMulti;

        if (InputManager.Settings.CameraDistance < 0.65f)
        {
            transform.position += transform.right * Mathf.Max((0.6f - InputManager.Settings.CameraDistance) * 2f, 0.65f);
        }
    }

    private void DoTPSMovement()
    {
        float num5 = (Input.GetAxis("Mouse X") * 10f) * GetSensitivityMulti();
        float num6 = ((-Input.GetAxis("Mouse Y") * 10f) * GetSensitivityMulti()) * GetReverse();
        transform.RotateAround(transform.position, Vector3.up, num5);
        float num7 = transform.rotation.eulerAngles.x % 360f;
        float num8 = num7 + num6;
        if (((num6 <= 0f) || (((num7 >= 260f) || (num8 <= 260f)) && ((num7 >= 80f) || (num8 <= 80f)))) && ((num6 >= 0f) || (((num7 <= 280f) || (num8 >= 280f)) && ((num7 <= 100f) || (num8 >= 100f)))))
        {
            transform.RotateAround(transform.position, transform.right, num6);
        }
    }

    private void DoOriginalMovement()
    {
        float num3 = 0f;
        if (Input.mousePosition.x < (Screen.width * 0.4f))
        {
            num3 = (-((((Screen.width * 0.4f) - Input.mousePosition.x) / Screen.width) * 0.4f) * GetSensitivityMultiWithDeltaTime()) * 150f;
            transform.RotateAround(transform.position, Vector3.up, num3);
        }
        else if (Input.mousePosition.x > (Screen.width * 0.6f))
        {
            num3 = ((((Input.mousePosition.x - (Screen.width * 0.6f)) / Screen.width) * 0.4f) * GetSensitivityMultiWithDeltaTime()) * 150f;
            transform.RotateAround(transform.position, Vector3.up, num3);
        }

        float x;

        if (!MenuManager.IsAnyMenuOpen)
        {
            x = ((140f * ((Screen.height * 0.6f) - Input.mousePosition.y)) / Screen.height) * 0.5f;
        }
        else
        {
            x = 0f;
        }
        transform.rotation = Quaternion.Euler(x, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
    }

    private void DoWOWMovement()
    {
        if (Input.GetKey(KeyCode.Mouse1))
        {
            float angle = (Input.GetAxis("Mouse X") * 10f) * GetSensitivityMulti();
            float num2 = ((-Input.GetAxis("Mouse Y") * 10f) * GetSensitivityMulti()) * GetReverse();
            transform.RotateAround(transform.position, Vector3.up, angle);
            transform.RotateAround(transform.position, transform.right, num2);
        }
    }

    /// <summary>
    /// Selects the closest titan within the <see cref="lockableDistance"/>.
    /// </summary>
    /// <returns>GameObject of the closest titan inside <see cref="lockableDistance"/>. <c>null</c> if none found.</returns>
    private GameObject FindNearestLockableTitan()
    {
        GameObject[] allTitans = GameObject.FindGameObjectsWithTag("titan");
        GameObject closestTitan = null;
        float positiveInfinity = float.PositiveInfinity;
        float closestDistance = positiveInfinity;
        Vector3 position = main_object.transform.position;

        foreach (GameObject currTitan in allTitans)
        {
            // TODO: Optimize accessing all titan positions.
            // Possibly another class that maintains a list of active titans,
            // removes them from the list when they die.
            Vector3 playerToCurrTitan = currTitan.transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/neck").position - position;
            float currDistance = playerToCurrTitan.magnitude;
            if (currDistance < lockableDistance && (currDistance < closestDistance) && (currTitan.GetComponent<MindlessTitan>() == null || currTitan.GetComponent<MindlessTitan>().IsAlive))
            {
                closestTitan = currTitan;
                closestDistance = currDistance;
            }
        }

        return closestTitan;
    }

    private int GetReverse()
    {
        return InputManager.Settings.MouseInvert
            ? -1
            : 1;
    }

    private float GetSensitivityMulti()
    {
        if (MenuManager.IsAnyMenuOpen)
        {
            return 0f; //Prevents camera from moving when on menu
        }
        else
        {
            return InputManager.Settings.MouseSensitivity;
        }
    }

    private float GetSensitivityMultiWithDeltaTime()
    {
        if (MenuManager.IsAnyMenuOpen)
        {
            return 0f; //Prevents camera from moving when on menu
        }
        else
        {
            return InputManager.Settings.MouseSensitivity * Time.deltaTime * 62f;
        }
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
            textured.ReadPixels(new Rect(num, num, cam.targetTexture.width - num, cam.targetTexture.height - num), destX, destX);
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

    private void ShakeUpdate()
    {
        if (duration > 0f)
        {
            duration -= Time.deltaTime;
            if (flip)
            {
                Transform transform = gameObject.transform;
                transform.position += Vector3.up * R;
            }
            else
            {
                Transform transform2 = gameObject.transform;
                transform2.position -= Vector3.up * R;
            }
            flip = !flip;
            R *= decay;
        }
    }

    private void Start()
    {
        GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>().addCamera(this);
        isPausing = false;
        HUD = Service.Ui.GetUiHandler().InGameUi.HUD.transform.gameObject;
        // This doesn't exist in the scene and causes a NullReferenceException.
        // TODO: Fix titan locking
        locker = GameObject.Find("locker");
        CreateSnapShotRT2();

        // Find the compass gameobject to set the camera's transform to this.
        CompassController compass = GameObject.Find("Compass").GetComponent<CompassController>();
        compass.cam = this.transform;
        compass.compassMode = true;

    }

    private void OnDestroy()
    {
        EntityService.OnRegister -= EntityService_OnRegistered;
        GameObject.Find("Compass").GetComponent<CompassController>().compassMode = false;
    }
}