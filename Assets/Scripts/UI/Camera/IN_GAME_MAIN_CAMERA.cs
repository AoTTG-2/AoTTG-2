using System;
using System.Runtime.InteropServices;
using UnityEngine;

public class IN_GAME_MAIN_CAMERA : MonoBehaviour
{
    public RotationAxes axes;
    public AudioSource bgmusic;
    public static float cameraDistance = 0.6f;
    public static CAMERA_TYPE cameraMode;
    public static int cameraTilt = 1;
    public static int character = 1;
    private float closestDistance;
    private int currentPeekPlayerIndex;
    public static DayLight dayLight = DayLight.Dawn;
    private float decay;
    public static int difficulty;
    private float distance = 10f;
    private float distanceMulti;
    private float distanceOffsetMulti;
    private float duration;
    private float flashDuration;
    private bool flip;
    public bool gameOver;
    public static GAMETYPE gametype = GAMETYPE.STOP;
    private bool hasSnapShot;
    private Transform head;
    public float height = 5f;
    public float heightDamping = 2f;
    private float heightMulti;
    public FengCustomInputs inputManager;
    public static int invertY = 1;
    public static bool isCheating;
    public static bool isPausing;
    public static bool isTyping;
    public float justHit;
    public int lastScore;
    public static int level;
    private bool lockAngle;
    private Vector3 lockCameraPosition;
    private GameObject locker;
    private GameObject lockTarget;
    public GameObject main_object;
    public float maximumX = 360f;
    public float maximumY = 60f;
    public float minimumX = -360f;
    public float minimumY = -60f;
    private bool needSetHUD;
    private float R;
    public float rotationY;
    public int score;
    public static float sensitivityMulti = 0.5f;
    public static string singleCharacter;
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
    public Transform target;
    public Texture texture;
    public float timer;
    public static bool triggerAutoLock;
    public static bool usingTitan;
    private Vector3 verticalHeightOffset = Vector3.zero;
    public float verticalRotationOffset;
    public float xSpeed = -3f;
    public float ySpeed = -0.8f;

    private void Awake()
    {
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
    }

    private void camareMovement()
    {
        this.distanceOffsetMulti = (cameraDistance * (200f - base.GetComponent<Camera>().fieldOfView)) / 150f;
        base.transform.position = (this.head == null) ? this.main_object.transform.position : this.head.transform.position;
        Transform transform = base.transform;
        transform.position += (Vector3) (Vector3.up * this.heightMulti);
        Transform transform2 = base.transform;
        transform2.position -= (Vector3) ((Vector3.up * (0.6f - cameraDistance)) * 2f);
        if (cameraMode == CAMERA_TYPE.WOW)
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
        else if (cameraMode == CAMERA_TYPE.ORIGINAL)
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
        else if (cameraMode == CAMERA_TYPE.TPS)
        {
            if (!this.inputManager.menuOn)
            {
                Screen.lockCursor = true;
            }
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
        if (cameraDistance < 0.65f)
        {
            Transform transform6 = base.transform;
            transform6.position += (Vector3) (base.transform.right * Mathf.Max((float) ((0.6f - cameraDistance) * 2f), (float) 0.65f));
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
        base.transform.position = (Vector3) ((this.head.transform.position + (Vector3.up * this.heightMulti)) - ((Vector3.up * (0.6f - cameraDistance)) * 2f));
        Transform transform = base.transform;
        transform.position -= (Vector3) (((base.transform.forward * this.distance) * this.distanceMulti) * num2);
        if (hero.CameraMultiplier < 0.65f)
        {
            Transform transform2 = base.transform;
            transform2.position += (Vector3) (base.transform.right * Mathf.Max((float) ((0.6f - hero.CameraMultiplier) * 2f), (float) 0.65f));
        }
        base.transform.rotation = Quaternion.Lerp(Camera.main.transform.rotation, hero.GetComponent<SmoothSyncMovement>().correctCameraRot, Time.deltaTime * 5f);
    }

    private void CreateMinimap()
    {
        //LevelInfo info = LevelInfo.getInfo(FengGameManagerMKII.level);
        //if (info != null)
        //{
        //    Minimap minimap = base.gameObject.AddComponent<Minimap>();
        //    if (Minimap.instance.myCam == null)
        //    {
        //        Minimap.instance.myCam = new GameObject().AddComponent<Camera>();
        //        Minimap.instance.myCam.nearClipPlane = 0.3f;
        //        Minimap.instance.myCam.farClipPlane = 1000f;
        //        Minimap.instance.myCam.enabled = false;
        //    }
        //    //minimap.CreateMinimap(Minimap.instance.myCam, 0x200, 0.3f, info.minimapPreset);
        //    if ((((int)FengGameManagerMKII.settings[0xe7]) == 0) || (RCSettings.globalDisableMinimap == 1))
        //    {
        //        minimap.SetEnabled(false);
        //    }
        //}
    }

    public void createSnapShotRT()
    {
        if (this.snapShotCamera.GetComponent<Camera>().targetTexture != null)
        {
            this.snapShotCamera.GetComponent<Camera>().targetTexture.Release();
        }
        if (QualitySettings.GetQualityLevel() > 3)
        {
            this.snapShotCamera.GetComponent<Camera>().targetTexture = new RenderTexture((int) (Screen.width * 0.8f), (int) (Screen.height * 0.8f), 0x18);
        }
        else
        {
            this.snapShotCamera.GetComponent<Camera>().targetTexture = new RenderTexture((int) (Screen.width * 0.4f), (int) (Screen.height * 0.4f), 0x18);
        }
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
            Vector3 vector2 = obj3.transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/neck").position - position;
            float magnitude = vector2.magnitude;
            if ((magnitude < num2) && ((obj3.GetComponent<TITAN>() == null) || !obj3.GetComponent<TITAN>().hasDie))
            {
                obj2 = obj3;
                num2 = magnitude;
                this.closestDistance = num2;
            }
        }
        return obj2;
    }

    public void flashBlind()
    {
        //GameObject.Find("flash").GetComponent<UISprite>().alpha = 1f;
        //this.flashDuration = 2f;
    }

    private int getReverse()
    {
        return invertY;
    }

    private float getSensitivityMulti()
    {
        return sensitivityMulti;
    }

    private float getSensitivityMultiWithDeltaTime()
    {
        return ((sensitivityMulti * Time.deltaTime) * 62f);
    }

    private void reset()
    {
        if (gametype == GAMETYPE.SINGLE)
        {
            GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>().restartGameSingle2();
        }
    }

    private Texture2D RTImage(Camera cam)
    {
        RenderTexture active = RenderTexture.active;
        RenderTexture.active = cam.targetTexture;
        cam.Render();
        Texture2D textured = new Texture2D(cam.targetTexture.width, cam.targetTexture.height);
        int num = (int) (cam.targetTexture.width * 0.04f);
        int destX = (int) (cam.targetTexture.width * 0.02f);
        textured.ReadPixels(new Rect((float) num, (float) num, (float) (cam.targetTexture.width - num), (float) (cam.targetTexture.height - num)), destX, destX);
        textured.Apply();
        RenderTexture.active = active;
        return textured;
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

    public void setDayLight(DayLight val)
    {
        dayLight = val;
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
        if (usingTitan && (gametype != GAMETYPE.SINGLE))
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
            if (gametype == GAMETYPE.SINGLE)
            {
                this.main_object.GetComponent<Hero>().setSkillHUDPosition2();
            }
            else if ((this.main_object.GetPhotonView() != null) && this.main_object.GetPhotonView().isMine)
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
        //TODO MouseLook
        //GameObject.Find("MainCamera").GetComponent<MouseLook>().disable = !val;
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
        if (index == 3)
        {
            this.snapShotCamera.transform.RotateAround(base.transform.position, Vector3.up, UnityEngine.Random.Range((float) -180f, (float) 180f));
        }
        else
        {
            this.snapShotCamera.transform.RotateAround(base.transform.position, Vector3.up, UnityEngine.Random.Range((float) -20f, (float) 20f));
        }
        this.snapShotCamera.transform.LookAt(worldPosition);
        this.snapShotCamera.transform.RotateAround(worldPosition, base.transform.right, UnityEngine.Random.Range((float) -20f, (float) 20f));
        float num = Vector3.Distance(this.snapShotTargetPosition, vector);
        if ((this.snapShotTarget != null) && (this.snapShotTarget.GetComponent<TITAN>() != null))
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

    private void Start()
    {
        QualitySettings.vSyncCount = 0;
        GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>().addCamera(this);
        isPausing = false;
        if (PlayerPrefs.HasKey("MouseSensitivity"))
        { 
            sensitivityMulti = PlayerPrefs.GetFloat("MouseSensitivity"); 
        }
        if (PlayerPrefs.HasKey("invertMouseY"))
        {
            invertY = PlayerPrefs.GetInt("invertMouseY");
        }
        this.inputManager = GameObject.Find("InputManagerController").GetComponent<FengCustomInputs>();
        this.setDayLight(dayLight);
        this.locker = GameObject.Find("locker");
        if (PlayerPrefs.HasKey("cameraTilt"))
        {
            cameraTilt = PlayerPrefs.GetInt("cameraTilt");
        }
        else
        {
            cameraTilt = 1;
        }
        if (PlayerPrefs.HasKey("cameraDistance"))
        {
            cameraDistance = PlayerPrefs.GetFloat("cameraDistance") + 0.3f;
        }
        this.createSnapShotRT2();
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

    public void startSnapShot(Vector3 p, int dmg, GameObject target = null, float startTime = 0.02f)
    {
        this.snapShotCount = 1;
        this.startSnapShotFrameCount = true;
        this.snapShotTargetPosition = p;
        this.snapShotTarget = target;
        this.snapShotStartCountDownTime = startTime;
        this.snapShotInterval = 0.05f + UnityEngine.Random.Range((float) 0f, (float) 0.03f);
        this.snapShotDmg = dmg;
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

    public void update2()
    {
        if (this.flashDuration > 0f)
        {
            this.flashDuration -= Time.deltaTime;
            if (this.flashDuration <= 0f)
            {
                this.flashDuration = 0f;
            }
            //GameObject.Find("flash").GetComponent<UISprite>().alpha = this.flashDuration * 0.5f;
        }
        if (gametype == GAMETYPE.STOP)
        {
            Cursor.visible = true;
            Screen.lockCursor = false;
        }
        else
        {
            if ((gametype != GAMETYPE.SINGLE) && this.gameOver)
            {
                if (this.inputManager.isInputDown[InputCode.attack1])
                {
                    if (this.spectatorMode)
                    {
                        this.setSpectorMode(false);
                    }
                    else
                    {
                        this.setSpectorMode(true);
                    }
                }
                if (this.inputManager.isInputDown[InputCode.flare1])
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
                if (this.inputManager.isInputDown[InputCode.flare2])
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
            if (this.inputManager.isInputDown[InputCode.pause])
            {
                if (isPausing)
                {
                    if (this.main_object != null)
                    {
                        Vector3 position = base.transform.position;
                        position = (this.head == null) ? this.main_object.transform.position : this.head.transform.position;
                        position += (Vector3) (Vector3.up * this.heightMulti);
                        base.transform.position = Vector3.Lerp(base.transform.position, position - ((Vector3) (base.transform.forward * 5f)), 0.2f);
                    }
                    return;
                }
                isPausing = !isPausing;
                if (isPausing)
                {
                    if (gametype == GAMETYPE.SINGLE)
                    {
                        Time.timeScale = 0f;
                    }
                    GameObject.Find("InputManagerController").GetComponent<FengCustomInputs>().menuOn = true;
                    Cursor.visible = true;
                    Screen.lockCursor = false;
                }
            }
            if (this.needSetHUD)
            {
                this.needSetHUD = false;
                this.setHUDposition();
                Screen.lockCursor = !Screen.lockCursor;
                Screen.lockCursor = !Screen.lockCursor;
            }
            if (this.inputManager.isInputDown[InputCode.fullscreen])
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
                Minimap.OnScreenResolutionChanged();
            }
            if (this.inputManager.isInputDown[InputCode.restart])
            {
                this.reset();
            }
            if (this.main_object != null)
            {
                RaycastHit hit;
                if (this.inputManager.isInputDown[InputCode.camera])
                {
                    if (cameraMode == CAMERA_TYPE.ORIGINAL)
                    {
                        cameraMode = CAMERA_TYPE.WOW;
                        Screen.lockCursor = false;
                    }
                    else if (cameraMode == CAMERA_TYPE.WOW)
                    {
                        cameraMode = CAMERA_TYPE.TPS;
                        Screen.lockCursor = true;
                    }
                    else if (cameraMode == CAMERA_TYPE.TPS)
                    {
                        cameraMode = CAMERA_TYPE.ORIGINAL;
                        Screen.lockCursor = false;
                    }
                    this.verticalRotationOffset = 0f;
                    if ((((int) FengGameManagerMKII.settings[0xf5]) == 1) || (this.main_object.GetComponent<Hero>() == null))
                    {
                        Cursor.visible = false;
                    }
                }
                if (this.inputManager.isInputDown[InputCode.hideCursor])
                {
                    Cursor.visible = !Cursor.visible;
                }
                if (this.inputManager.isInputDown[InputCode.focus])
                {
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
                    if (FengGameManagerMKII.inputRC.isInputHumanDown(InputCodeRC.liveCam))
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
                        this.camareMovement();
                    }
                }
                else
                {
                    this.camareMovement();
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
                    this.locker.transform.localPosition = new Vector3(vector3.x - (Screen.width * 0.5f), vector3.y - (Screen.height * 0.5f), 0f);
                    if ((this.lockTarget.GetComponent<TITAN>() != null) && this.lockTarget.GetComponent<TITAN>().hasDie)
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

    public enum RotationAxes
    {
        MouseXAndY,
        MouseX,
        MouseY
    }
}

