using Assets.Scripts;
using Assets.Scripts.Audio;
using Assets.Scripts.Characters;
using Assets.Scripts.Characters.Base;
using Assets.Scripts.Characters.Titan;
using Assets.Scripts.Services;
using Assets.Scripts.Settings;
using Assets.Scripts.UI.InGame.HUD;
using Assets.Scripts.UI.Input;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public partial class Hero : Human
{
    #region Fields

    public Equipment Equipment { get; set; }
    public EquipmentType EquipmentType;

    public List<HeroSkill> Skills;

    private const float HookRaycastDistance = 1000f;

    public HERO_STATE _state;
    private bool almostSingleHook;
    private string attackAnimation;
    private int attackLoop;
    private bool attackMove;
    private bool attackReleased;

    private GameObject badGuy;
    public new Animation animation;
    public Rigidbody rigidBody;

    public bool bigLean;
    private float buffTime;

    public BombSettings bombSettings;

    [Header("Bullet / Hook")]
    public Bullet bulletLeft;
    public Bullet bulletRight;
    private int bulletMAX = 7;

    private bool buttonAttackRelease;
    public Dictionary<string, Image> cachedSprites;
    public float CameraMultiplier;
    public bool canJump = true;
    [SerializeField] TriggerColliderWeapon checkLeftTrigger;
    [SerializeField] TriggerColliderWeapon checkRightTrigger;

    public string currentAnimation;
    [Obsolete]
    public int currentBladeNum = 5;
    public float currentBladeSta = 100f;
    private BUFF currentBuff;
    public Camera currentCamera;
    private float currentGas = 100f;
    public float currentSpeed;
    public Vector3 currentV;
    private bool dashD;
    private Vector3 dashDirection;
    private bool dashL;
    private bool dashR;
    private float dashTime;
    private bool dashU;
    private Vector3 dashV;
    public bool detonate;
    private float dTapTime = -1f;
    private bool eHold;
    
    private int escapeTimes = 1;
    private float facingDirection;
    private float flare1CD;
    private float flare2CD;
    private float flare3CD;
    private float flareTotalCD = 30f;
    private Transform forearmL;
    private Transform forearmR;
    private float gravity = 20f;
    public bool grounded;
    private GameObject gunDummy;
    private Vector3 gunTarget;
    private Transform handL;
    private Transform handR;
    private bool hasDied;
    public bool hasspawn;
    private bool hookBySomeOne = true;
    public GameObject hookRefL1;
    public GameObject hookRefL2;
    public GameObject hookRefR1;
    public GameObject hookRefR2;
    private bool hookSomeOne;
    private GameObject hookTarget;
    private float invincible = 3f;
    public bool isCannon;
    private bool isLaunchLeft;
    private bool isLaunchRight;
    private bool isLeftHandHooked;
    private bool isMounted;
    public bool isPhotonCamera;
    private bool isRightHandHooked;
    public float jumpHeight = 2f;
    private bool justGrounded;

    public Transform lastHook;
    private float launchElapsedTimeL;
    private float launchElapsedTimeR;
    private Vector3 launchForce;
    private Vector3 launchPointLeft;
    private Vector3 launchPointRight;
    private bool leanLeft;
    private bool leftArmAim;

    [Obsolete]
    public int leftBulletLeft = 7;
    public bool leftGunHasBullet = true;
    private float lTapTime = -1f;

    public float maxVelocityChange = 10f;

    public Bomb myBomb;
    public GameObject myCannon;
    public Transform myCannonBase;
    public Transform myCannonPlayer;
    public CannonPropRegion myCannonRegion;
    private Horse myHorse;
    public GameObject myNetWorkName;
    public float myScale = 1f;
    public int myTeam = 1;
    public List<MindlessTitan> myTitans;
    private bool needLean;
    private Quaternion oldHeadRotation;
    private float originVM;
    private bool qHold;
    public string reloadAnimation = string.Empty;
    private bool rightArmAim;

    [Obsolete]
    public int rightBulletLeft = 7;
    public bool rightGunHasBullet = true;

    private float rTapTime = -1f;
    public HERO_SETUP setup;
    private GameObject skillCD;
    public float skillCDDuration;
    public float skillCDLast;
    public float skillCDLastCannon;
    private string skillId;
    public string skillIDHUD;

    private ParticleSystem smoke_3dmg;
    private ParticleSystem.EmissionModule smoke_3dmgEmission;
    private ParticleSystem sparks;
    private ParticleSystem.EmissionModule sparksEmission;
    public float speed = 10f;
    public GameObject speedFX;
    public GameObject speedFX1;
    private ParticleSystem speedFXPS;
    public bool spinning;
    private Quaternion targetHeadRotation;
    private Quaternion targetRotation;
    public Vector3 targetV;
    public bool throwedBlades;
    public bool titanForm;
    private GameObject titanWhoGrabMe;
    private int titanWhoGrabMeID;
    private int totalBladeNum = 5;
    public float totalBladeSta = 100f;
    public float totalGas = 100f;
    private Transform upperarmL;
    private Transform upperarmR;
    private float useGasSpeed = 0.2f;
    public bool useGun;
    private float uTapTime = -1f;
    private bool wallJump;
    private float wallRunTime;

    public GameObject InGameUI;
    public TextMeshPro PlayerName;

    string standAnimation = "stand";

    private GameObject erenTitanGameObject;
    private ErenTitan erenTitan;

    public HeroAudio audioSystem;
    private HookUI hookUI = new HookUI();
    private SmoothSyncMovement smoothSyncMovement;

    [Space]
    [SerializeField] Body body;

    private LayerMask maskGround;
    private LayerMask maskEnemy;
    private LayerMask maskPlayerAttackBox;
    private LayerMask maskGroundEnemy;
    private LayerMask maskGroundEnemyPlayer;

    #endregion

    #region Unity Methods
    protected override void Awake()
    {
        base.Awake();
        InGameUI = GameObject.Find("InGameUi");
        Cache();
        setup = GetComponent<HERO_SETUP>();
        rigidBody.freezeRotation = true;
        rigidBody.useGravity = false;
        smoothSyncMovement = GetComponent<SmoothSyncMovement>();

        maskGround = 1 << LayerMask.NameToLayer("Ground");
        maskEnemy = 1 << LayerMask.NameToLayer("EnemyBox");
        maskPlayerAttackBox = 1 << LayerMask.NameToLayer("PlayerAttackBox");
        maskGroundEnemy = maskGround | maskEnemy;
        maskGroundEnemyPlayer = maskGroundEnemy | maskPlayerAttackBox;

        Equipment = gameObject.AddComponent<Equipment>();
        Faction = Service.Faction.GetHumanity();
        Service.Entity.Register(this);
    }

    private void Start()
    {
        gameObject.AddComponent<PlayerInteractable>();
        SetHorse();
        sparks = transform.Find("slideSparks").GetComponent<ParticleSystem>();
        sparksEmission = sparks.emission;
        smoke_3dmg = transform.Find("3dmg_smoke").GetComponent<ParticleSystem>();
        smoke_3dmgEmission = smoke_3dmg.emission;
        transform.localScale = new Vector3(myScale, myScale, myScale);
        facingDirection = transform.rotation.eulerAngles.y;
        targetRotation = Quaternion.Euler(0f, facingDirection, 0f);
        sparksEmission.enabled = false;
        sparksEmission.enabled = false;
        //HACK
        //speedFXPS = speedFX1.GetComponent<ParticleSystem>();
        //speedFXPS.enableEmission = false;
        if (PhotonNetwork.isMasterClient)
        {
            int iD = photonView.owner.ID;
            if (FengGameManagerMKII.heroHash.ContainsKey(iD))
            {
                FengGameManagerMKII.heroHash[iD] = this;
            }
            else
            {
                FengGameManagerMKII.heroHash.Add(iD, this);
            }
        }

        PlayerName.text = FengGameManagerMKII.instance.name;
        ////HACK
        ////GameObject obj2 = GameObject.Find("UI_IN_GAME");
        //myNetWorkName = (GameObject) UnityEngine.Object.Instantiate(Resources.Load("UI/LabelNameOverHead"));
        //myNetWorkName.name = "LabelNameOverHead";
        ////HACK
        ////myNetWorkName.transform.parent = obj2.GetComponent<UIReferArray>().panels[0].transform;
        //myNetWorkName.transform.localScale = new Vector3(14f, 14f, 14f);
        //myNetWorkName.GetComponent<UILabel>().text = string.Empty;
        if (photonView.isMine)
        {
            GetComponent<SmoothSyncMovement>().PhotonCamera = true;
            photonView.RPC(nameof(SetMyPhotonCamera), PhotonTargets.OthersBuffered,
                new object[] { PlayerPrefs.GetFloat("cameraDistance") + 0.3f });
        }

        if (!photonView.isMine)
        {
            gameObject.layer = LayerMask.NameToLayer("NetworkObject");
            if (IN_GAME_MAIN_CAMERA.dayLight == DayLight.Night)
            {
                GameObject obj3 = (GameObject) Instantiate(Resources.Load("flashlight"));
                obj3.transform.parent = transform;
                obj3.transform.position = transform.position + Vector3.up;
                obj3.transform.rotation = Quaternion.Euler(353f, 0f, 0f);
            }
            setup.init();
            setup.myCostume = new HeroCostume();
            setup.myCostume = CostumeConeveter.PhotonDataToHeroCostume2(photonView.owner);
            setup.setCharacterComponent();
            Destroy(checkLeftTrigger.gameObject);
            Destroy(checkRightTrigger.gameObject);
            hasspawn = true;
        }
        else
        {
            currentCamera = GameObject.Find(NAME_MAIN_CAMERA).GetComponent<Camera>();
            LoadSkin();
            hasspawn = true;
            StartCoroutine(ReloadSky());
            bombSettings.bombImmune = false;
            if (GameSettings.PvP.Bomb.Value)
            {
                bombSettings.bombImmune = true;
                StartCoroutine(StopImmunity());
            }
        }
    }

    public void Update()
    {
        if (IN_GAME_MAIN_CAMERA.isPausing) // If paused, do nothing. Better than a huge if { ... } block
            return;

        if (invincible > 0f)
        {
            invincible -= Time.deltaTime;
        }
        if (!hasDied)
        {
            if (titanForm && (erenTitan != null))
            {
                transform.position = erenTitan.Body.Neck.position;
                smoothSyncMovement.disabled = true;
            }
            else if (isCannon && (myCannon != null))
            {
                UpdateCannon();
                smoothSyncMovement.disabled = true;
            }
            if (photonView.isMine)
            {
                if (myCannonRegion != null)
                {
                    Service.Ui.SetMessage(LabelPosition.Center, "Press 'Cannon Mount' key to use Cannon.");
                    if (InputManager.KeyDown(InputCannon.Mount))
                    {
                        myCannonRegion.photonView.RPC(nameof(CannonPropRegion.RequestControlRPC), PhotonTargets.MasterClient, new object[] { photonView.viewID });
                    }
                }
                if ((State == HERO_STATE.Grab) && !useGun)
                {
                    switch (skillId)
                    {
                        case SKILL_JEAN:
                            if (((State != HERO_STATE.Attack) &&
                              (InputManager.KeyDown(InputHuman.Attack) ||
                               InputManager.KeyDown(InputHuman.AttackSpecial))) &&
                                ((escapeTimes > 0) && !animation.IsPlaying(ANIM_GRABBED_JEAN)))
                            {
                                PlayAnimation(ANIM_GRABBED_JEAN);
                                animation[ANIM_GRABBED_JEAN].time = 0f;
                                escapeTimes--;
                            }
                            if ((animation.IsPlaying(ANIM_GRABBED_JEAN) && (animation[ANIM_GRABBED_JEAN].normalizedTime > 0.64f)) && (titanWhoGrabMe.GetComponent<MindlessTitan>() != null))
                            {
                                Ungrabbed();
                                rigidBody.velocity = (Vector3.up * 30f);
                                photonView.RPC(nameof(NetSetIsGrabbedFalse), PhotonTargets.All, new object[0]);
                                if (PhotonNetwork.isMasterClient)
                                {
                                    titanWhoGrabMe.GetComponent<MindlessTitan>().GrabEscapeRpc();
                                }
                                else
                                {
                                    PhotonView.Find(titanWhoGrabMeID).RPC(nameof(MindlessTitan.GrabEscapeRpc), PhotonTargets.MasterClient, new object[0]);
                                }
                            }
                            break;
                        case SKILL_EREN:
                            if (!IN_GAME_MAIN_CAMERA.isPausing)
                            {
                                CalcSkillCD();
                                CalcFlareCD();
                            }
                            if (InputManager.KeyDown(InputHuman.AttackSpecial))
                            {
                                bool flag2 = false;
                                if ((skillCDDuration > 0f) || flag2)
                                {
                                    flag2 = true;
                                }
                                else
                                {
                                    skillCDDuration = skillCDLast;
                                    if ((skillId == SKILL_EREN) && (titanWhoGrabMe.GetComponent<MindlessTitan>() != null))
                                    {
                                        Ungrabbed();
                                        photonView.RPC(nameof(NetSetIsGrabbedFalse), PhotonTargets.All, new object[0]);
                                        if (PhotonNetwork.isMasterClient)
                                        {
                                            titanWhoGrabMe.GetComponent<MindlessTitan>().GrabEscapeRpc();
                                        }
                                        else
                                        {
                                            PhotonView.Find(titanWhoGrabMeID).photonView.RPC(nameof(MindlessTitan.GrabEscapeRpc), PhotonTargets.MasterClient, new object[0]);
                                        }
                                        ErenTransform();
                                    }
                                }
                            }
                            break;
                    }
                }
                else if (!titanForm && !isCannon)
                {
                    bool hookBoth;
                    bool hookRight;
                    bool hookLeft;
                    BufferUpdate();
                    UpdateExt();
                    if (!grounded && (State != HERO_STATE.AirDodge))
                    {
                        if (InputManager.Settings.GasBurstDoubleTap)
                        {
                            CheckDashDoubleTap();
                        }
                        else
                        {
                            CheckDashRebind();
                        }
                        if (dashD)
                        {
                            dashD = false;
                            Dash(0f, -1f);
                            return;
                        }
                        if (dashU)
                        {
                            dashU = false;
                            Dash(0f, 1f);
                            return;
                        }
                        if (dashL)
                        {
                            dashL = false;
                            Dash(-1f, 0f);
                            return;
                        }
                        if (dashR)
                        {
                            dashR = false;
                            Dash(1f, 0f);
                            return;
                        }
                    }
                    if (grounded && ((State == HERO_STATE.Idle) || (State == HERO_STATE.Slide)))
                    {
                        if (!((!InputManager.KeyDown(InputHuman.Jump) || animation.IsPlaying(ANIM_JUMP)) || animation.IsPlaying(ANIM_HORSE_GETON)))
                        {
                            Idle();
                            CrossFade(ANIM_JUMP, 0.1f);
                            sparksEmission.enabled = false;
                        }
                        if (!((!InputManager.KeyDown(InputHorse.Mount) || animation.IsPlaying(ANIM_JUMP)) || animation.IsPlaying(ANIM_HORSE_GETON)) && (((myHorse != null) && !isMounted) && (Vector3.Distance(myHorse.transform.position, transform.position) < 15f)))
                        {
                            GetOnHorse();
                        }
                        if (!((!InputManager.KeyDown(InputHuman.Dodge) || animation.IsPlaying(ANIM_JUMP)) || animation.IsPlaying(ANIM_HORSE_GETON)))
                        {
                            Dodge(false);
                            return;
                        }
                    }

                    Update_HeroState();

                    if (InputManager.Key(InputHuman.HookLeft))
                    {
                        hookLeft = true;
                    }
                    else
                    {
                        hookLeft = false;
                    }
                    if (!(hookLeft ? (((animation.IsPlaying(ANIM_ATTACK_3_1) || animation.IsPlaying(ANIM_ATTACK_5)) || (animation.IsPlaying(ANIM_SPECIAL_PETRA) || (State == HERO_STATE.Grab))) ? (State != HERO_STATE.Idle) : false) : true))
                    {
                        if (bulletLeft != null)
                        {
                            qHold = true;
                        }
                        else
                        {
                            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                            if (Physics.Raycast(ray, out var hit, HookRaycastDistance, maskGroundEnemy))
                            {
                                LaunchLeftRope(hit.distance, hit.point, true);
                            }
                            else
                            {
                                LaunchLeftRope(HookRaycastDistance, ray.GetPoint(HookRaycastDistance), true);
                            }
                            audioSystem.PlayOneShot(audioSystem.clipRope);

                        }
                    }
                    else
                    {
                        qHold = false;
                    }
                    if (InputManager.Key(InputHuman.HookRight))
                    {
                        hookRight = true;
                    }
                    else
                    {
                        hookRight = false;
                    }
                    if (!(hookRight ? (((animation.IsPlaying(ANIM_ATTACK_3_1) || animation.IsPlaying(ANIM_ATTACK_5)) || (animation.IsPlaying(ANIM_SPECIAL_PETRA) || (State == HERO_STATE.Grab))) ? (State != HERO_STATE.Idle) : false) : true))
                    {
                        if (bulletRight != null)
                        {
                            eHold = true;
                        }
                        else
                        {
                            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                            if (Physics.Raycast(ray, out var hit, HookRaycastDistance, maskGroundEnemy))
                            {
                                LaunchRightRope(hit.distance, hit.point, true);
                            }
                            else
                            {
                                LaunchRightRope(HookRaycastDistance, ray.GetPoint(HookRaycastDistance), true);
                            }
                            audioSystem.PlayOneShot(audioSystem.clipRope);

                        }
                    }
                    else
                    {
                        eHold = false;
                    }
                    if (InputManager.Key(InputHuman.HookBoth))
                    {
                        hookBoth = true;
                    }
                    else
                    {
                        hookBoth = false;
                    }
                    if (!(hookBoth ? (((animation.IsPlaying(ANIM_ATTACK_3_1) || animation.IsPlaying(ANIM_ATTACK_5)) || (animation.IsPlaying(ANIM_SPECIAL_PETRA) || (State == HERO_STATE.Grab))) ? (State != HERO_STATE.Idle) : false) : true))
                    {
                        qHold = true;
                        eHold = true;
                        if ((bulletLeft == null) && (bulletRight == null))
                        {
                            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                            if (Physics.Raycast(ray, out var hit, HookRaycastDistance, maskGroundEnemy))
                            {
                                LaunchLeftRope(hit.distance, hit.point, false);
                                LaunchRightRope(hit.distance, hit.point, false);
                            }
                            else
                            {
                                LaunchLeftRope(HookRaycastDistance, ray.GetPoint(HookRaycastDistance), false);
                                LaunchRightRope(HookRaycastDistance, ray.GetPoint(HookRaycastDistance), false);
                            }
                            audioSystem.PlayOneShot(audioSystem.clipRope);

                        }
                    }
                    if (!IN_GAME_MAIN_CAMERA.isPausing)
                    {
                        CalcSkillCD();
                        CalcFlareCD();

                        ShowGas2();
                        ShowAimUI2();
                    }
                }
            }
        }
    }

    private void FixedUpdate()
    {
        if ((!titanForm && !isCannon) && (!IN_GAME_MAIN_CAMERA.isPausing))
        {
            currentSpeed = rigidBody.velocity.magnitude;
            if (photonView.isMine)
            {
                if (!((animation.IsPlaying(ANIM_ATTACK_3_2) ||
                    animation.IsPlaying(ANIM_ATTACK_5)) ||
                    animation.IsPlaying(ANIM_SPECIAL_PETRA)))
                {
                    rigidBody.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 6f);
                }

                switch (State)
                {
                    case HERO_STATE.Grab:
                        rigidBody.AddForce(-rigidBody.velocity, ForceMode.VelocityChange);
                        break;
                    default:
                        if (IsGrounded())
                        {
                            if (!grounded)
                            {
                                justGrounded = true;
                            }
                            grounded = true;
                        }
                        else
                        {
                            grounded = false;
                        }
                        if (hookSomeOne)
                        {
                            if (hookTarget != null)
                            {
                                Vector3 vector2 = hookTarget.transform.position - transform.position;
                                float magnitude = vector2.magnitude;
                                if (magnitude > 2f)
                                {
                                    rigidBody.AddForce((((vector2.normalized * Mathf.Pow(magnitude, 0.15f)) * 30f) - (rigidBody.velocity * 0.95f)), ForceMode.VelocityChange);
                                }
                            }
                            else
                            {
                                hookSomeOne = false;
                            }
                        }
                        else if (hookBySomeOne && (badGuy != null))
                        {
                            if (badGuy != null)
                            {
                                Vector3 vector3 = badGuy.transform.position - transform.position;
                                float f = vector3.magnitude;
                                if (f > 5f)
                                {
                                    rigidBody.AddForce(((vector3.normalized * Mathf.Pow(f, 0.15f)) * 0.2f), ForceMode.Impulse);
                                }
                            }
                            else
                            {
                                hookBySomeOne = false;
                            }
                        }
                        float x = 0f;
                        float z = 0f;
                        if (!IN_GAME_MAIN_CAMERA.isTyping)
                        {
                            if (InputManager.Key(InputHuman.Forward))
                            {
                                z = 1f;
                            }
                            else if (InputManager.Key(InputHuman.Backward))
                            {
                                z = -1f;
                            }
                            else
                            {
                                z = 0f;
                            }
                            if (InputManager.Key(InputHuman.Left))
                            {
                                x = -1f;
                            }
                            else if (InputManager.Key(InputHuman.Right))
                            {
                                x = 1f;
                            }
                            else
                            {
                                x = 0f;
                            }
                        }
                        bool flag2 = false;
                        bool flag3 = false;
                        bool flag4 = false;
                        isLeftHandHooked = false;
                        isRightHandHooked = false;
                        if (isLaunchLeft)
                        {
                            if ((bulletLeft != null) && bulletLeft.isHooked())
                            {
                                isLeftHandHooked = true;
                                Vector3 to = bulletLeft.transform.position - transform.position;
                                to.Normalize();
                                to = (to * 10f);
                                if (!isLaunchRight)
                                {
                                    to = (to * 2f);
                                }
                                if ((Vector3.Angle(rigidBody.velocity, to) > 90f) && InputManager.Key(InputHuman.Jump))
                                {
                                    flag3 = true;
                                    flag2 = true;
                                }
                                if (!flag3)
                                {
                                    rigidBody.AddForce(to);
                                    if (Vector3.Angle(rigidBody.velocity, to) > 90f)
                                    {
                                        rigidBody.AddForce((-rigidBody.velocity * 2f), ForceMode.Acceleration);
                                    }
                                }
                            }
                            launchElapsedTimeL += Time.deltaTime;
                            if (qHold && (currentGas > 0f))
                            {
                                UseGas(useGasSpeed * Time.deltaTime);
                            }
                            else if (launchElapsedTimeL > 0.3f)
                            {
                                isLaunchLeft = false;
                                if (bulletLeft != null)
                                {
                                    bulletLeft.disable();
                                    ReleaseIfIHookSb();
                                    bulletLeft = null;
                                    flag3 = false;
                                }
                            }
                        }
                        if (isLaunchRight)
                        {
                            if ((bulletRight != null) && bulletRight.isHooked())
                            {
                                isRightHandHooked = true;
                                Vector3 vector5 = bulletRight.transform.position - transform.position;
                                vector5.Normalize();
                                vector5 = (vector5 * 10f);
                                if (!isLaunchLeft)
                                {
                                    vector5 = (vector5 * 2f);
                                }
                                if ((Vector3.Angle(rigidBody.velocity, vector5) > 90f) && InputManager.Key(InputHuman.Jump))
                                {
                                    flag4 = true;
                                    flag2 = true;
                                }
                                if (!flag4)
                                {
                                    rigidBody.AddForce(vector5);
                                    if (Vector3.Angle(rigidBody.velocity, vector5) > 90f)
                                    {
                                        rigidBody.AddForce((-rigidBody.velocity * 2f), ForceMode.Acceleration);
                                    }
                                }
                            }
                            launchElapsedTimeR += Time.deltaTime;
                            if (eHold && (currentGas > 0f))
                            {
                                UseGas(useGasSpeed * Time.deltaTime);
                            }
                            else if (launchElapsedTimeR > 0.3f)
                            {
                                isLaunchRight = false;
                                if (bulletRight != null)
                                {
                                    bulletRight.disable();
                                    ReleaseIfIHookSb();
                                    bulletRight = null;
                                    flag4 = false;
                                }
                            }
                        }
                        if (grounded)
                        {
                            Vector3 vector7;
                            Vector3 zero = Vector3.zero;
                            if (State == HERO_STATE.Attack)
                            {
                                if (attackAnimation == ANIM_ATTACK_5)
                                {
                                    if ((animation[attackAnimation].normalizedTime > 0.4f) && (animation[attackAnimation].normalizedTime < 0.61f))
                                    {
                                        rigidBody.AddForce((transform.forward * 200f));
                                    }
                                }
                                else if (attackAnimation == ANIM_SPECIAL_PETRA)
                                {
                                    if ((animation[attackAnimation].normalizedTime > 0.35f) && (animation[attackAnimation].normalizedTime < 0.48f))
                                    {
                                        rigidBody.AddForce((transform.forward * 200f));
                                    }
                                }
                                else if (animation.IsPlaying(ANIM_ATTACK_3_2))
                                {
                                    zero = Vector3.zero;
                                }
                                else if (animation.IsPlaying(ANIM_ATTACK_1) || animation.IsPlaying(ANIM_ATTACK_2))
                                {
                                    rigidBody.AddForce((transform.forward * 200f));
                                }
                                if (animation.IsPlaying(ANIM_ATTACK_3_2))
                                {
                                    zero = Vector3.zero;
                                }
                            }
                            if (justGrounded)
                            {
                                if ((State != HERO_STATE.Attack) || (((attackAnimation != ANIM_ATTACK_3_1) && (attackAnimation != ANIM_ATTACK_5)) && (attackAnimation != ANIM_SPECIAL_PETRA)))
                                {
                                    if ((((State != HERO_STATE.Attack) && (x == 0f)) && ((z == 0f) && (bulletLeft == null))) && ((bulletRight == null) && (State != HERO_STATE.FillGas)))
                                    {
                                        State = HERO_STATE.Land;
                                        CrossFade(ANIM_DASH_LAND, 0.01f);
                                    }
                                    else
                                    {
                                        buttonAttackRelease = true;
                                        if (((State != HERO_STATE.Attack) && (((rigidBody.velocity.x * rigidBody.velocity.x) + (rigidBody.velocity.z * rigidBody.velocity.z)) > ((speed * speed) * 1.5f))) && (State != HERO_STATE.FillGas))
                                        {
                                            State = HERO_STATE.Slide;
                                            CrossFade(ANIM_SLIDE, 0.05f);
                                            facingDirection = Mathf.Atan2(rigidBody.velocity.x, rigidBody.velocity.z) * Mathf.Rad2Deg;
                                            targetRotation = Quaternion.Euler(0f, facingDirection, 0f);
                                            sparksEmission.enabled = true;
                                        }
                                    }
                                }
                                justGrounded = false;
                                zero = rigidBody.velocity;
                            }
                            if (((State == HERO_STATE.Attack) && (attackAnimation == ANIM_ATTACK_3_1)) && (animation[attackAnimation].normalizedTime >= 1f))
                            {
                                PlayAnimation(ANIM_ATTACK_3_2);
                                ResetAnimationSpeed();
                                vector7 = Vector3.zero;
                                rigidBody.velocity = vector7;
                                zero = vector7;
                                currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().StartShake(0.2f, 0.3f, 0.95f);
                            }
                            if (State == HERO_STATE.GroundDodge)
                            {
                                if ((animation[ANIM_DODGE].normalizedTime >= 0.2f) && (animation[ANIM_DODGE].normalizedTime < 0.8f))
                                {
                                    zero = ((-transform.forward * 2.4f) * speed);
                                }
                                if (animation[ANIM_DODGE].normalizedTime > 0.8f)
                                {
                                    zero = (rigidBody.velocity * 0.9f);
                                }
                            }
                            else if (State == HERO_STATE.Idle)
                            {
                                Vector3 vector8 = new Vector3(x, 0f, z);
                                float resultAngle = GetGlobalFacingDirection(x, z);
                                zero = GetGlobalFacingVector3(resultAngle);
                                float num6 = (vector8.magnitude <= 0.95f) ? ((vector8.magnitude >= 0.25f) ? vector8.magnitude : 0f) : 1f;
                                zero = (zero * num6);
                                zero = (zero * speed);
                                if ((buffTime > 0f) && (currentBuff == BUFF.SpeedUp))
                                {
                                    zero = (zero * 4f);
                                }
                                if ((x != 0f) || (z != 0f))
                                {
                                    if (((!animation.IsPlaying(ANIM_RUN_1) && !animation.IsPlaying(ANIM_JUMP)) && !animation.IsPlaying(ANIM_RUN_SASHA)) && (!animation.IsPlaying(ANIM_HORSE_GETON) || (animation[ANIM_HORSE_GETON].normalizedTime >= 0.5f)))
                                    {
                                        if ((buffTime > 0f) && (currentBuff == BUFF.SpeedUp))
                                        {
                                            CrossFade(ANIM_RUN_SASHA, 0.1f);
                                        }
                                        else
                                        {
                                            CrossFade(ANIM_RUN_1, 0.1f);
                                        }
                                    }
                                }
                                else
                                {
                                    if (!(((animation.IsPlaying(standAnimation) || (State == HERO_STATE.Land)) || (animation.IsPlaying(ANIM_JUMP) || animation.IsPlaying(ANIM_HORSE_GETON))) || animation.IsPlaying("grabbed")))
                                    {
                                        CrossFade(standAnimation, 0.1f);
                                        zero = (zero * 0f);
                                    }
                                    resultAngle = -874f;
                                }
                                if (resultAngle != -874f)
                                {
                                    facingDirection = resultAngle;
                                    targetRotation = Quaternion.Euler(0f, facingDirection, 0f);
                                }
                            }
                            else if (State == HERO_STATE.Land)
                            {
                                zero = (rigidBody.velocity * 0.96f);
                            }
                            else if (State == HERO_STATE.Slide)
                            {
                                zero = (rigidBody.velocity * 0.99f);
                                if (currentSpeed < (speed * 1.2f))
                                {
                                    Idle();
                                    sparksEmission.enabled = false;
                                }
                            }
                            Vector3 velocity = rigidBody.velocity;
                            Vector3 force = zero - velocity;
                            force.x = Mathf.Clamp(force.x, -maxVelocityChange, maxVelocityChange);
                            force.z = Mathf.Clamp(force.z, -maxVelocityChange, maxVelocityChange);
                            force.y = 0f;
                            if (animation.IsPlaying(ANIM_JUMP) && (animation[ANIM_JUMP].normalizedTime > 0.18f))
                            {
                                force.y += 8f;
                            }
                            if ((animation.IsPlaying(ANIM_HORSE_GETON) && (animation[ANIM_HORSE_GETON].normalizedTime > 0.18f)) && (animation[ANIM_HORSE_GETON].normalizedTime < 1f))
                            {
                                float num7 = 6f;
                                force = -rigidBody.velocity;
                                force.y = num7;
                                float num8 = Vector3.Distance(myHorse.transform.position, transform.position);
                                float num9 = ((0.6f * gravity) * num8) / 12f;
                                vector7 = myHorse.transform.position - transform.position;
                                force += (num9 * vector7.normalized);
                            }
                            if (!((State == HERO_STATE.Attack) && useGun))
                            {
                                rigidBody.AddForce(force, ForceMode.VelocityChange);
                                rigidBody.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0f, facingDirection, 0f), Time.deltaTime * 10f);
                            }
                        }
                        else
                        {
                            if (sparksEmission.enabled)
                            {
                                sparksEmission.enabled = false;
                            }
                            if ((myHorse && (animation.IsPlaying(ANIM_HORSE_GETON) || animation.IsPlaying("air_fall"))) && ((rigidBody.velocity.y < 0f) && (Vector3.Distance(myHorse.transform.position + Vector3.up * 1.65f, transform.position) < 0.5f)))
                            {
                                transform.position = myHorse.transform.position + Vector3.up * 1.65f;
                                transform.rotation = myHorse.transform.rotation;
                                isMounted = true;
                                CrossFade("horse_idle", 0.1f);
                                myHorse.Mount();
                            }
                            if (!((((((State != HERO_STATE.Idle) || animation.IsPlaying(ANIM_DASH)) || (animation.IsPlaying("wallrun") || animation.IsPlaying("toRoof"))) || ((animation.IsPlaying(ANIM_HORSE_GETON) || animation.IsPlaying(ANIM_HORSE_GETOFF)) || (animation.IsPlaying(ANIM_AIR_RELEASE) || isMounted))) || ((animation.IsPlaying(ANIM_AIR_HOOK_L_JUST) && (animation[ANIM_AIR_HOOK_L_JUST].normalizedTime < 1f)) || (animation.IsPlaying("air_hook_r_just") && (animation["air_hook_r_just"].normalizedTime < 1f)))) ? (animation[ANIM_DASH].normalizedTime < 0.99f) : false))
                            {
                                if (((!isLeftHandHooked && !isRightHandHooked) && ((animation.IsPlaying(ANIM_AIR_HOOK_L) || animation.IsPlaying(ANIM_AIR_HOOK_R)) || animation.IsPlaying(ANIM_AIR_HOOK))) && (rigidBody.velocity.y > 20f))
                                {
                                    animation.CrossFade(ANIM_AIR_RELEASE);
                                }
                                else
                                {
                                    bool flag5 = (Mathf.Abs(rigidBody.velocity.x) + Mathf.Abs(rigidBody.velocity.z)) > 25f;
                                    bool flag6 = rigidBody.velocity.y < 0f;
                                    if (!flag5)
                                    {
                                        if (flag6)
                                        {
                                            if (!animation.IsPlaying("air_fall"))
                                            {
                                                CrossFade("air_fall", 0.2f);
                                            }
                                        }
                                        else if (!animation.IsPlaying("air_rise"))
                                        {
                                            CrossFade("air_rise", 0.2f);
                                        }
                                    }
                                    else if (!isLeftHandHooked && !isRightHandHooked)
                                    {
                                        float current = -Mathf.Atan2(rigidBody.velocity.z, rigidBody.velocity.x) * Mathf.Rad2Deg;
                                        float num11 = -Mathf.DeltaAngle(current, transform.rotation.eulerAngles.y - 90f);
                                        if (Mathf.Abs(num11) < 45f)
                                        {
                                            if (!animation.IsPlaying("air2"))
                                            {
                                                CrossFade("air2", 0.2f);
                                            }
                                        }
                                        else if ((num11 < 135f) && (num11 > 0f))
                                        {
                                            if (!animation.IsPlaying("air2_right"))
                                            {
                                                CrossFade("air2_right", 0.2f);
                                            }
                                        }
                                        else if ((num11 > -135f) && (num11 < 0f))
                                        {
                                            if (!animation.IsPlaying("air2_left"))
                                            {
                                                CrossFade("air2_left", 0.2f);
                                            }
                                        }
                                        else if (!animation.IsPlaying("air2_backward"))
                                        {
                                            CrossFade("air2_backward", 0.2f);
                                        }
                                    }

                                    else if (!isRightHandHooked)
                                    {
                                        TryCrossFade(Equipment.Weapon.HookForwardLeft, 0.1f);
                                    }
                                    else if (!isLeftHandHooked)
                                    {
                                        TryCrossFade(Equipment.Weapon.HookForwardRight, 0.1f);
                                    }
                                    else if (!animation.IsPlaying(Equipment.Weapon.HookForward))
                                    {
                                        TryCrossFade(Equipment.Weapon.HookForward, 0.1f);
                                    }
                                }
                            }
                            if (((State == HERO_STATE.Idle) && animation.IsPlaying(ANIM_AIR_RELEASE)) && (animation[ANIM_AIR_RELEASE].normalizedTime >= 1f))
                            {
                                CrossFade("air_rise", 0.2f);
                            }
                            if (animation.IsPlaying(ANIM_HORSE_GETOFF) && (animation[ANIM_HORSE_GETOFF].normalizedTime >= 1f))
                            {
                                CrossFade("air_rise", 0.2f);
                            }
                            if (animation.IsPlaying("toRoof"))
                            {
                                if (animation["toRoof"].normalizedTime < 0.22f)
                                {
                                    rigidBody.velocity = Vector3.zero;
                                    rigidBody.AddForce(new Vector3(0f, gravity * rigidBody.mass, 0f));
                                }
                                else
                                {
                                    if (!wallJump)
                                    {
                                        wallJump = true;
                                        rigidBody.AddForce((Vector3.up * 8f), ForceMode.Impulse);
                                    }
                                    rigidBody.AddForce((transform.forward * 0.05f), ForceMode.Impulse);
                                }
                                if (animation["toRoof"].normalizedTime >= 1f)
                                {
                                    PlayAnimation("air_rise");
                                }
                            }
                            else if (!(((((State != HERO_STATE.Idle) || !IsPressDirectionTowardsHero(x, z)) ||
                                            (InputManager.Key(InputHuman.Jump) ||
                                            InputManager.Key(InputHuman.HookLeft))) ||
                                        ((InputManager.Key(InputHuman.HookRight) ||
                                            InputManager.Key(InputHuman.HookBoth)) ||
                                            (!IsFrontGrounded() || animation.IsPlaying("wallrun")))) ||
                                        animation.IsPlaying(ANIM_DODGE)))
                            {
                                CrossFade("wallrun", 0.1f);
                                wallRunTime = 0f;
                            }
                            else if (animation.IsPlaying("wallrun"))
                            {
                                rigidBody.AddForce(((Vector3.up * speed)) - rigidBody.velocity, ForceMode.VelocityChange);
                                wallRunTime += Time.deltaTime;
                                if ((wallRunTime > 1f) || ((z == 0f) && (x == 0f)))
                                {
                                    rigidBody.AddForce(((-transform.forward * speed) * 0.75f), ForceMode.Impulse);
                                    Dodge(true);
                                }
                                else if (!IsUpFrontGrounded())
                                {
                                    wallJump = false;
                                    CrossFade("toRoof", 0.1f);
                                }
                                else if (!IsFrontGrounded())
                                {
                                    CrossFade("air_fall", 0.1f);
                                }
                            }
                            else if ((!animation.IsPlaying(ANIM_ATTACK_5) && !animation.IsPlaying(ANIM_SPECIAL_PETRA)) && (!animation.IsPlaying(ANIM_DASH) && !animation.IsPlaying(ANIM_JUMP)))
                            {
                                Vector3 vector11 = new Vector3(x, 0f, z);
                                float num12 = GetGlobalFacingDirection(x, z);
                                Vector3 vector12 = GetGlobalFacingVector3(num12);
                                float num13 = (vector11.magnitude <= 0.95f) ? ((vector11.magnitude >= 0.25f) ? vector11.magnitude : 0f) : 1f;
                                vector12 = (vector12 * num13);
                                vector12 = (vector12 * ((((float) setup.myCostume.stat.ACL) / 10f) * 2f));
                                if ((x == 0f) && (z == 0f))
                                {
                                    if (State == HERO_STATE.Attack)
                                    {
                                        vector12 = (vector12 * 0f);
                                    }
                                    num12 = -874f;
                                }
                                if (num12 != -874f)
                                {
                                    facingDirection = num12;
                                    targetRotation = Quaternion.Euler(0f, facingDirection, 0f);
                                }
                                if (((!flag3 && !flag4) && (!isMounted && InputManager.Key(InputHuman.Jump))) && (currentGas > 0f))
                                {
                                    if ((x != 0f) || (z != 0f))
                                    {
                                        rigidBody.AddForce(vector12, ForceMode.Acceleration);
                                    }
                                    else
                                    {
                                        rigidBody.AddForce((transform.forward * vector12.magnitude), ForceMode.Acceleration);
                                    }
                                    flag2 = true;
                                }
                            }
                            if ((animation.IsPlaying("air_fall") && (currentSpeed < 0.2f)) && IsFrontGrounded())
                            {
                                CrossFade("onWall", 0.3f);
                            }
                        }
                        spinning = false;
                        if (flag3 && flag4)
                        {
                            float num14 = currentSpeed + 0.1f;
                            AddRightForce();
                            Vector3 vector13 = (((bulletRight.transform.position + bulletLeft.transform.position) * 0.5f)) - transform.position;
                            float num15 = 0f;
                            if (InputManager.Key(InputHuman.ReelIn))
                            {
                                num15 = -1f;
                            }
                            else if (InputManager.Key(InputHuman.ReelOut))
                            {
                                num15 = 1f;
                            }
                            else
                            {
                                num15 = Input.GetAxis(INPUT_MOUSE_SCROLLWHEEL) * 5555f;
                            }
                            num15 = Mathf.Clamp(num15, -0.8f, 0.8f);
                            float num16 = 1f + num15;
                            Vector3 vector14 = Vector3.RotateTowards(vector13, rigidBody.velocity, 1.53938f * num16, 1.53938f * num16);
                            vector14.Normalize();
                            spinning = true;
                            rigidBody.velocity = (vector14 * num14);
                        }
                        else if (flag3)
                        {
                            float num17 = currentSpeed + 0.1f;
                            AddRightForce();
                            Vector3 vector15 = bulletLeft.transform.position - transform.position;
                            float num18 = 0f;
                            if (InputManager.Key(InputHuman.ReelIn))
                            {
                                num18 = -1f;
                            }
                            else if (InputManager.Key(InputHuman.ReelOut))
                            {
                                num18 = 1f;
                            }
                            else
                            {
                                num18 = Input.GetAxis(INPUT_MOUSE_SCROLLWHEEL) * 5555f;
                            }
                            num18 = Mathf.Clamp(num18, -0.8f, 0.8f);
                            float num19 = 1f + num18;
                            Vector3 vector16 = Vector3.RotateTowards(vector15, rigidBody.velocity, 1.53938f * num19, 1.53938f * num19);
                            vector16.Normalize();
                            spinning = true;
                            rigidBody.velocity = (vector16 * num17);
                        }
                        else if (flag4)
                        {
                            float num20 = currentSpeed + 0.1f;
                            AddRightForce();
                            Vector3 vector17 = bulletRight.transform.position - transform.position;
                            float num21 = 0f;
                            if (InputManager.Key(InputHuman.ReelIn))
                            {
                                num21 = -1f;
                            }
                            else if (InputManager.Key(InputHuman.ReelOut))
                            {
                                num21 = 1f;
                            }
                            else
                            {
                                num21 = Input.GetAxis(INPUT_MOUSE_SCROLLWHEEL) * 5555f;
                            }
                            num21 = Mathf.Clamp(num21, -0.8f, 0.8f);
                            float num22 = 1f + num21;
                            Vector3 vector18 = Vector3.RotateTowards(vector17, rigidBody.velocity, 1.53938f * num22, 1.53938f * num22);
                            vector18.Normalize();
                            spinning = true;
                            rigidBody.velocity = (vector18 * num20);
                        }
                        if (((State == HERO_STATE.Attack) && ((attackAnimation == ANIM_ATTACK_5) || (attackAnimation == ANIM_SPECIAL_PETRA))) && ((animation[attackAnimation].normalizedTime > 0.4f) && !attackMove))
                        {
                            attackMove = true;
                            if (launchPointRight.magnitude > 0f)
                            {
                                Vector3 vector19 = launchPointRight - transform.position;
                                vector19.Normalize();
                                vector19 = (vector19 * 13f);
                                rigidBody.AddForce(vector19, ForceMode.Impulse);
                            }
                            if ((attackAnimation == ANIM_SPECIAL_PETRA) && (launchPointLeft.magnitude > 0f))
                            {
                                Vector3 vector20 = launchPointLeft - transform.position;
                                vector20.Normalize();
                                vector20 = (vector20 * 13f);
                                rigidBody.AddForce(vector20, ForceMode.Impulse);
                                if (bulletRight != null)
                                {
                                    bulletRight.disable();
                                    ReleaseIfIHookSb();
                                }
                                if (bulletLeft != null)
                                {
                                    bulletLeft.disable();
                                    ReleaseIfIHookSb();
                                }
                            }
                            rigidBody.AddForce((Vector3.up * 2f), ForceMode.Impulse);
                        }
                        bool flag7 = false;
                        if ((bulletLeft != null) || (bulletRight != null))
                        {
                            if (((bulletLeft != null) && (bulletLeft.transform.position.y > transform.position.y)) && (isLaunchLeft && bulletLeft.isHooked()))
                            {
                                flag7 = true;
                            }
                            if (((bulletRight != null) && (bulletRight.transform.position.y > transform.position.y)) && (isLaunchRight && bulletRight.isHooked()))
                            {
                                flag7 = true;
                            }
                        }
                        if (flag7)
                        {
                            rigidBody.AddForce(new Vector3(0f, -10f * rigidBody.mass, 0f));
                        }
                        else
                        {
                            rigidBody.AddForce(new Vector3(0f, -gravity * rigidBody.mass, 0f));
                        }
                        if (currentSpeed > 10f)
                        {
                            currentCamera.fieldOfView = Mathf.Lerp(currentCamera.fieldOfView, Mathf.Min((float) 100f, (float) (currentSpeed + 40f)), 0.1f);
                        }
                        else
                        {
                            currentCamera.fieldOfView = Mathf.Lerp(currentCamera.fieldOfView, 50f, 0.1f);
                        }
                        if (flag2)
                        {
                            UseGas(useGasSpeed * Time.deltaTime);
                            if (!smoke_3dmgEmission.enabled && photonView.isMine)
                            {
                                object[] parameters = new object[] { true };
                                photonView.RPC(nameof(Net3DMGSMOKE), PhotonTargets.Others, parameters);
                            }
                            smoke_3dmgEmission.enabled = true;
                        }
                        else
                        {
                            if (smoke_3dmgEmission.enabled && photonView.isMine)
                            {
                                object[] objArray3 = new object[] { false };
                                photonView.RPC(nameof(Net3DMGSMOKE), PhotonTargets.Others, objArray3);
                            }
                            smoke_3dmgEmission.enabled = false;
                        }

                        break;

                }
            }
        }
    }


    protected override void OnDestroy()
    {
        base.OnDestroy();
        if (myNetWorkName != null)
        {
            Destroy(myNetWorkName);
        }
        if (gunDummy != null)
        {
            Destroy(gunDummy);
        }
        ReleaseIfIHookSb();

        if (setup.part_cape != null)
        {
            ClothFactory.DisposeObject(setup.part_cape);
        }
        if (setup.part_hair_1 != null)
        {
            ClothFactory.DisposeObject(setup.part_hair_1);
        }
        if (setup.part_hair_2 != null)
        {
            ClothFactory.DisposeObject(setup.part_hair_2);
        }
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "titan") return;
        var force = collision.impulse.magnitude / Time.fixedDeltaTime;
        if (GameSettings.Gamemode.ImpactForce > 0 && force >= GameSettings.Gamemode.ImpactForce)
        {
            Die(new Vector3(), false);
        }
    }

    #endregion


    #region Checks and Properties
    /// <summary>
    /// Is <see cref="State"/> == <see cref="HERO_STATE.Grab"/>
    /// </summary>
    public bool IsGrabbed => (State == HERO_STATE.Grab);
    /// <summary>
    /// Is Hero invincible? <code>invincible > 0f</code>
    /// </summary>
    public bool IsInvincible => (invincible > 0f);

    private HERO_STATE State
    {
        get
        {
            return _state;
        }
        set
        {
            if ((_state == HERO_STATE.AirDodge) || (_state == HERO_STATE.GroundDodge))
            {
                dashTime = 0f;
            }
            _state = value;
        }
    }

    /// <summary>
    /// Has Hero died or is Hero invincible?
    /// </summary>
    /// <returns>If Hero is dead or invincible</returns>
    public bool HasDied()
    {
        return (hasDied || IsInvincible);
    }

    private bool IsFrontGrounded()
    {
        return Physics.Raycast(transform.position + ((transform.up * 1f)), transform.forward, (float) 1f, maskGroundEnemy);
    }
    private bool IsUpFrontGrounded()
    {
        return Physics.Raycast(transform.position + ((transform.up * 3f)), transform.forward, (float) 1.2f, maskGroundEnemy);
    }

    public bool IsGrounded()
    {
        return Physics.Raycast(transform.position + ((Vector3.up * 0.1f)), -Vector3.up, (float) 0.3f, maskGroundEnemy);
    }


    private bool IsPressDirectionTowardsHero(float h, float v)
    {
        if ((h == 0f) && (v == 0f))
        {
            return false;
        }
        return (Mathf.Abs(Mathf.DeltaAngle(GetGlobalFacingDirection(h, v), transform.rotation.eulerAngles.y)) < 45f);
    }


    #endregion


    #region Update() switch( HeroState )

    /// <summary>
    /// Called by <see cref="Update"/>. Evaluate <see cref="HERO_STATE"/> and call appropriate Methods, e.g. <see cref="Update_HeroState_Idle"/>
    /// </summary>
    void Update_HeroState()
    {
        switch (State)
        {
            case HERO_STATE.Idle:
                Update_HeroState_Idle();
                break;
            case HERO_STATE.Attack:
                Update_HeroState_Attack();
                break;
            case HERO_STATE.ChangeBlade:
                Update_HeroState_ChangeBlade();
                break;
            case HERO_STATE.Salute:
                Update_HeroState_Salute();
                break;
            case HERO_STATE.GroundDodge:
                Update_HeroState_GroundDodge();
                break;
            case HERO_STATE.Land:
                Update_HeroState_Land();
                break;
            case HERO_STATE.FillGas:
                Update_HeroState_FillGas();
                break;
            case HERO_STATE.Slide:
                Update_HeroState_Slide();
                break;
            case HERO_STATE.AirDodge:
                Update_HeroState_AirDodge();
                break;
        }
    }

    /// <summary>
    /// Called by <see cref="Update_HeroState"/>
    /// </summary>
    void Update_HeroState_Idle()
    {
        if (!MenuManager.IsAnyMenuOpen)
        {
            if (InputManager.KeyDown(InputHuman.Item1))
            {
                ShootFlare(1);
            }
            if (InputManager.KeyDown(InputHuman.Item2))
            {
                ShootFlare(2);
            }
            if (InputManager.KeyDown(InputHuman.Item3))
            {
                ShootFlare(3);
            }
        }
        if (InputManager.KeyDown(InputUi.Restart))
        {
            if (!PhotonNetwork.offlineMode)
            {
                Suicide();
            }
        }
        if (((myHorse != null) && isMounted) && InputManager.KeyDown(InputHorse.Mount))
        {
            GetOffHorse();
        }
        if (((animation.IsPlaying(standAnimation) || !grounded) && InputManager.KeyDown(InputHuman.Reload)) && ((!useGun || (GameSettings.PvP.AhssAirReload.Value)) || grounded))
        {
            ChangeBlade();
            return;
        }
        if (animation.IsPlaying(standAnimation) && InputManager.KeyDown(InputHuman.Salute))
        {
            Salute();
            return;
        }
        if ((!isMounted && (InputManager.KeyDown(InputHuman.Attack) || InputManager.KeyDown(InputHuman.AttackSpecial))) && !useGun)
        {
            bool specialUnused = false;
            if (InputManager.KeyDown(InputHuman.AttackSpecial))
            {
                if ((skillCDDuration > 0f))
                {
                    specialUnused = true;
                }
                else
                {
                    skillCDDuration = skillCDLast;
                    switch (skillId)
                    {
                        case SKILL_EREN:
                            ErenTransform();
                            return;
                        case "marco":
                            if (IsGrounded())
                            {
                                attackAnimation = (UnityEngine.Random.Range(0, 2) != 0) ? "special_marco_1" : "special_marco_0";
                                PlayAnimation(attackAnimation);
                            }
                            else
                            {
                                specialUnused = true;
                                skillCDDuration = 0f;
                            }
                            break;
                        case "armin":
                            if (IsGrounded())
                            {
                                attackAnimation = "special_armin";
                                PlayAnimation("special_armin");
                            }
                            else
                            {
                                specialUnused = true;
                                skillCDDuration = 0f;
                            }
                            break;
                        case "sasha":
                            if (IsGrounded())
                            {
                                attackAnimation = "special_sasha";
                                PlayAnimation("special_sasha");
                                currentBuff = BUFF.SpeedUp;
                                buffTime = 10f;
                            }
                            else
                            {
                                specialUnused = true;
                                skillCDDuration = 0f;
                            }
                            break;
                        case "mikasa":
                            attackAnimation = ANIM_ATTACK_3_1;
                            PlayAnimation(ANIM_ATTACK_3_1);
                            rigidBody.velocity = (Vector3.up * 10f);
                            break;
                        case "levi":
                            {
                                RaycastHit hit;
                                attackAnimation = ANIM_ATTACK_5;
                                PlayAnimation(ANIM_ATTACK_5);
                                rigidBody.velocity += (Vector3.up * 5f);
                                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                                if (Physics.Raycast(ray, out hit, float.MaxValue, maskGroundEnemy))
                                {
                                    if (bulletRight != null)
                                    {
                                        bulletRight.disable();
                                        ReleaseIfIHookSb();
                                    }
                                    dashDirection = hit.point - transform.position;
                                    LaunchRightRope(hit.distance, hit.point, true);
                                    audioSystem.PlayOneShot(audioSystem.clipRope);
                                }
                                facingDirection = Mathf.Atan2(dashDirection.x, dashDirection.z) * Mathf.Rad2Deg;
                                targetRotation = Quaternion.Euler(0f, facingDirection, 0f);
                                attackLoop = 3;
                                break;
                            }

                        case "petra":
                            {
                                attackAnimation = ANIM_SPECIAL_PETRA;
                                PlayAnimation(ANIM_SPECIAL_PETRA);
                                rigidBody.velocity += (Vector3.up * 5f);
                                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                                if (Physics.Raycast(ray, out var hit, float.MaxValue, maskGroundEnemy))
                                {
                                    if (bulletRight != null)
                                    {
                                        bulletRight.disable();
                                        ReleaseIfIHookSb();
                                    }
                                    if (bulletLeft != null)
                                    {
                                        bulletLeft.disable();
                                        ReleaseIfIHookSb();
                                    }
                                    dashDirection = hit.point - transform.position;
                                    LaunchLeftRope(hit.distance, hit.point, true);
                                    LaunchRightRope(hit.distance, hit.point, true);
                                    audioSystem.PlayOneShot(audioSystem.clipRope);

                                }
                                facingDirection = Mathf.Atan2(dashDirection.x, dashDirection.z) * Mathf.Rad2Deg;
                                targetRotation = Quaternion.Euler(0f, facingDirection, 0f);
                                attackLoop = 3;
                                break;
                            }

                        default:
                            if (needLean)
                            {
                                if (leanLeft)
                                {
                                    attackAnimation = (Random.Range(0, 100) >= 50) ? "attack1_hook_l1" : "attack1_hook_l2";
                                }
                                else
                                {
                                    attackAnimation = (Random.Range(0, 100) >= 50) ? "attack1_hook_r1" : "attack1_hook_r2";
                                }
                            }
                            else
                            {
                                attackAnimation = ANIM_ATTACK_1;
                            }
                            PlayAnimation(attackAnimation);
                            break;
                    }
                }
            }
            else if (InputManager.KeyDown(InputHuman.Attack))
            {
                if (needLean)
                {
                    if (InputManager.Key(InputHuman.Left))
                    {
                        attackAnimation = (Random.Range(0, 100) >= 50) ? "attack1_hook_l1" : "attack1_hook_l2";
                    }
                    else if (InputManager.Key(InputHuman.Right))
                    {
                        attackAnimation = (Random.Range(0, 100) >= 50) ? "attack1_hook_r1" : "attack1_hook_r2";
                    }
                    else if (leanLeft)
                    {
                        attackAnimation = (Random.Range(0, 100) >= 50) ? "attack1_hook_l1" : "attack1_hook_l2";
                    }
                    else
                    {
                        attackAnimation = (Random.Range(0, 100) >= 50) ? "attack1_hook_r1" : "attack1_hook_r2";
                    }
                }
                else if (InputManager.Key(InputHuman.Left))
                {
                    attackAnimation = ANIM_ATTACK_2;
                }
                else if (InputManager.Key(InputHuman.Right))
                {
                    attackAnimation = ANIM_ATTACK_1;
                }
                else if (lastHook != null)
                {
                    if (lastHook.Find("Amarture/Core/Controller_Body/hip/spine/chest/neck") != null)
                    {
                        AttackAccordingToTarget(lastHook.Find("Amarture/Core/Controller_Body/hip/spine/chest/neck"));
                    }
                    else
                    {
                        specialUnused = true;
                    }
                }
                else if ((bulletLeft != null) && (bulletLeft.transform.parent != null))
                {
                    Transform a = bulletLeft.transform.parent.transform.root.Find("Amarture/Core/Controller_Body/hip/spine/chest/neck");
                    if (a != null)
                    {
                        AttackAccordingToTarget(a);
                    }
                    else
                    {
                        AttackAccordingToMouse();
                    }
                }
                else if ((bulletRight != null) && (bulletRight.transform.parent != null))
                {
                    Transform transform2 = bulletRight.transform.parent.transform.root.Find("Amarture/Core/Controller_Body/hip/spine/chest/neck");
                    if (transform2 != null)
                    {
                        AttackAccordingToTarget(transform2);
                    }
                    else
                    {
                        AttackAccordingToMouse();
                    }
                }
                else
                {
                    GameObject obj2 = FindNearestTitan();
                    if (obj2 != null)
                    {
                        Transform transform3 = obj2.transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/neck");
                        if (transform3 != null)
                        {
                            AttackAccordingToTarget(transform3);
                        }
                        else
                        {
                            AttackAccordingToMouse();
                        }
                    }
                    else
                    {
                        AttackAccordingToMouse();
                    }
                }
            }
            if (!specialUnused)
            {
                WeaponColliderClearHits();
                if (grounded)
                {
                    rigidBody.AddForce((transform.forward * 200f));
                }
                PlayAnimation(attackAnimation);
                animation[attackAnimation].time = 0f;
                buttonAttackRelease = false;
                State = HERO_STATE.Attack;
                if ((grounded || (attackAnimation == ANIM_ATTACK_3_1)) || ((attackAnimation == ANIM_ATTACK_5) || (attackAnimation == ANIM_SPECIAL_PETRA)))
                {
                    attackReleased = true;
                    buttonAttackRelease = true;
                }
                else
                {
                    attackReleased = false;
                }
                sparksEmission.enabled = false;
            }
        }
        if (useGun)
        {
            if (InputManager.Key(InputHuman.AttackSpecial))
            {
                leftArmAim = true;
                rightArmAim = true;
            }
            else if (InputManager.Key(InputHuman.Attack))
            {
                if (leftGunHasBullet)
                {
                    leftArmAim = true;
                    rightArmAim = false;
                }
                else
                {
                    leftArmAim = false;
                    if (rightGunHasBullet)
                    {
                        rightArmAim = true;
                    }
                    else
                    {
                        rightArmAim = false;
                    }
                }
            }
            else
            {
                leftArmAim = false;
                rightArmAim = false;
            }
            if (leftArmAim || rightArmAim)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out var hit, float.MaxValue, maskGroundEnemy))
                {
                    gunTarget = hit.point;
                }
            }
            bool flag4 = false;
            bool flag5 = false;
            bool flag6 = false;
            if (InputManager.KeyUp(InputHuman.AttackSpecial) && (skillId != BOMB_STRING))
            {
                if (leftGunHasBullet && rightGunHasBullet)
                {
                    attackAnimation = grounded ? "AHSS_shoot_both" : "AHSS_shoot_both_air";
                    flag4 = true;
                }
                else if (!(leftGunHasBullet || rightGunHasBullet))
                {
                    flag5 = true;
                }
                else
                {
                    flag6 = true;
                }
            }
            if (flag6 || InputManager.KeyUp(InputHuman.Attack))
            {
                if (grounded)
                {
                    if (leftGunHasBullet && rightGunHasBullet)
                    {
                        attackAnimation = isLeftHandHooked ? "AHSS_shoot_r" : "AHSS_shoot_l";
                    }
                    else if (leftGunHasBullet)
                    {
                        attackAnimation = "AHSS_shoot_l";
                    }
                    else if (rightGunHasBullet)
                    {
                        attackAnimation = "AHSS_shoot_r";
                    }
                }
                else if (leftGunHasBullet && rightGunHasBullet)
                {
                    attackAnimation = isLeftHandHooked ? "AHSS_shoot_r_air" : "AHSS_shoot_l_air";
                }
                else if (leftGunHasBullet)
                {
                    attackAnimation = "AHSS_shoot_l_air";
                }
                else if (rightGunHasBullet)
                {
                    attackAnimation = "AHSS_shoot_r_air";
                }
                if (leftGunHasBullet || rightGunHasBullet)
                {
                    flag4 = true;
                }
                else
                {
                    flag5 = true;
                }
            }
            if (flag4)
            {
                State = HERO_STATE.Attack;
                CrossFade(attackAnimation, 0.05f);
                gunDummy.transform.position = transform.position;
                gunDummy.transform.rotation = transform.rotation;
                gunDummy.transform.LookAt(gunTarget);
                attackReleased = false;
                facingDirection = gunDummy.transform.rotation.eulerAngles.y;
                targetRotation = Quaternion.Euler(0f, facingDirection, 0f);
            }
            else if (flag5 && (grounded || (GameSettings.PvP.AhssAirReload.Value)))
            {
                ChangeBlade();
            }
        }
    }

    /// <summary>
    /// Called by <see cref="Update_HeroState"/>
    /// </summary>
    void Update_HeroState_Attack()
    {
        if (!useGun)
        {
            if (!InputManager.Key(InputHuman.Attack))
            {
                buttonAttackRelease = true;
            }
            if (!attackReleased)
            {
                if (buttonAttackRelease)
                {
                    ContinueAnimation();
                    attackReleased = true;
                }
                else if (animation[attackAnimation].normalizedTime >= 0.32f)
                {
                    PauseAnimation();
                }
            }
            if ((attackAnimation == ANIM_ATTACK_3_1) && (currentBladeSta > 0f))
            {
                if (animation[attackAnimation].normalizedTime >= 0.8f)
                {
                    if (!checkLeftTrigger.IsActive)
                    {
                        ActivateWeaponCollider(true, null);
                        rigidBody.velocity = (-Vector3.up * 30f);
                    }
                    if (!checkRightTrigger.IsActive)
                    {
                        ActivateWeaponCollider(null, true);

                        audioSystem.PlayOneShot(audioSystem.clipSlash);

                    }
                }
                else if (checkLeftTrigger.IsActive)
                {
                    ActivateWeaponCollider(false);
                    WeaponColliderClearHits();
                }
            }
            else
            {
                float num;
                float num2;
                if (currentBladeSta == 0f)
                {
                    num = -1f;
                    num2 = -1f;
                }
                else
                {
                    switch (attackAnimation)
                    {
                        case ANIM_ATTACK_5:
                            num2 = 0.35f;
                            num = 0.5f;
                            break;
                        case ANIM_SPECIAL_PETRA:
                            num2 = 0.35f;
                            num = 0.48f;
                            break;
                        case "special_armin":
                            num2 = 0.25f;
                            num = 0.35f;
                            break;
                        case "attack4":
                            num2 = 0.6f;
                            num = 0.9f;
                            break;
                        case "special_sasha":
                            num = -1f;
                            num2 = -1f;
                            break;
                        default:
                            num2 = 0.5f;
                            num = 0.85f;
                            break;
                    }
                }

                if ((animation[attackAnimation].normalizedTime > num2) && (animation[attackAnimation].normalizedTime < num))
                {
                    if (!checkLeftTrigger.IsActive)
                    {
                        ActivateWeaponCollider(true, null);
                        audioSystem.PlayOneShot(audioSystem.clipSlash);

                    }
                    if (!checkRightTrigger.IsActive)
                    {
                        ActivateWeaponCollider(null, true);

                    }
                }
                else if (checkLeftTrigger.IsActive)
                {
                    ActivateWeaponCollider(false, false);

                    WeaponColliderClearHits();

                }
                if ((attackLoop > 0) && (animation[attackAnimation].normalizedTime > num))
                {
                    attackLoop--;
                    PlayAnimationAt(attackAnimation, num2);
                }
            }
            if (animation[attackAnimation].normalizedTime >= 1f)
            {
                switch (attackAnimation)
                {
                    case "special_marco_0":
                    case "special_marco_1":
                        object[] parameters = new object[] { 5f, 100f };
                        if (!PhotonNetwork.isMasterClient)
                        {
                            photonView.RPC(nameof(NetTauntAttack), PhotonTargets.MasterClient, parameters);
                        }
                        else
                        {
                            NetTauntAttack(5f, 100f);
                        }
                        FalseAttack();
                        Idle();
                        break;
                    case "special_armin":
                        if (!PhotonNetwork.isMasterClient)
                        {
                            photonView.RPC(nameof(NetLaughAttack), PhotonTargets.MasterClient, new object[0]);
                        }
                        else
                        {
                            NetLaughAttack();
                        }
                        FalseAttack();
                        Idle();
                        break;
                    case ANIM_ATTACK_3_1:
                        rigidBody.velocity -= ((Vector3.up * Time.deltaTime) * 30f);
                        break;
                    default:
                        FalseAttack();
                        Idle();
                        break;
                }
            }
            if (animation.IsPlaying(ANIM_ATTACK_3_2) && (animation[ANIM_ATTACK_3_2].normalizedTime >= 1f))
            {
                FalseAttack();
                Idle();
            }
        }
        else
        {
            ActivateWeaponCollider(false);
            transform.rotation = Quaternion.Lerp(transform.rotation, gunDummy.transform.rotation, Time.deltaTime * 30f);
            if (!attackReleased && (animation[attackAnimation].normalizedTime > 0.167f))
            {
                GameObject obj4;
                attackReleased = true;
                bool flag7 = false;
                if ((attackAnimation == "AHSS_shoot_both") || (attackAnimation == "AHSS_shoot_both_air"))
                {
                    //Should use AHSSShotgunCollider instead of TriggerColliderWeapon.  
                    //Apply that change when abstracting weapons from this class.
                    //Note, when doing the abstraction, the relationship between the weapon collider and the abstracted weapon class should be carefully considered.
                    ActivateWeaponCollider(true);

                    flag7 = true;
                    leftGunHasBullet = false;
                    rightGunHasBullet = false;
                    rigidBody.AddForce((-transform.forward * 1000f), ForceMode.Acceleration);
                }
                else
                {
                    if ((attackAnimation == "AHSS_shoot_l") || (attackAnimation == "AHSS_shoot_l_air"))
                    {
                        ActivateWeaponCollider(true, null);
                        leftGunHasBullet = false;
                    }
                    else
                    {
                        ActivateWeaponCollider(null, true);

                        rightGunHasBullet = false;
                    }
                    rigidBody.AddForce((-transform.forward * 600f), ForceMode.Acceleration);
                }
                rigidBody.AddForce((Vector3.up * 200f), ForceMode.Acceleration);
                string prefabName = "FX/shotGun";
                if (flag7)
                {
                    prefabName = "FX/shotGun 1";
                }
                if (photonView.isMine)
                {
                    obj4 = PhotonNetwork.Instantiate(prefabName, ((transform.position + (transform.up * 0.8f)) - (transform.right * 0.1f)), transform.rotation, 0);
                    if (obj4.GetComponent<EnemyfxIDcontainer>() != null)
                    {
                        obj4.GetComponent<EnemyfxIDcontainer>().myOwnerViewID = photonView.viewID;
                    }
                }
                else
                {
                    obj4 = (GameObject) Instantiate(Resources.Load(prefabName), ((transform.position + (transform.up * 0.8f)) - (transform.right * 0.1f)), transform.rotation);
                }
            }
            if (animation[attackAnimation].normalizedTime >= 1f)
            {
                FalseAttack();
                Idle();
                ActivateWeaponCollider(false);
            }
            if (!animation.IsPlaying(attackAnimation))
            {
                FalseAttack();
                Idle();
                ActivateWeaponCollider(false);
            }
        }

    }

    /// <summary>
    /// Called by <see cref="Update_HeroState"/>
    /// </summary>
    void Update_HeroState_ChangeBlade()
    {
        Equipment.Weapon.Reload();
        if (animation[reloadAnimation].normalizedTime >= 1f)
        {
            Idle();
        }
    }

    /// <summary>
    /// Called by <see cref="Update_HeroState"/>
    /// </summary>
    void Update_HeroState_Salute()
    {
        if (animation["salute"].normalizedTime >= 1f)
        {
            Idle();
        }
    }

    /// <summary>
    /// Called by <see cref="Update_HeroState"/>
    /// </summary>
    void Update_HeroState_GroundDodge()
    {
        if (animation.IsPlaying(ANIM_DODGE))
        {
            if (!(grounded || (animation[ANIM_DODGE].normalizedTime <= 0.6f)))
            {
                Idle();
            }
            if (animation[ANIM_DODGE].normalizedTime >= 1f)
            {
                Idle();
            }
        }
    }

    /// <summary>
    /// Called by <see cref="Update_HeroState"/>
    /// </summary>
    void Update_HeroState_Land()
    {
        if (animation.IsPlaying(ANIM_DASH_LAND) && (animation[ANIM_DASH_LAND].normalizedTime >= 1f))
        {
            Idle();
        }
    }

    /// <summary>
    /// Called by <see cref="Update_HeroState"/>
    /// </summary>
    void Update_HeroState_FillGas()
    {
        if (animation.IsPlaying("supply") && (animation["supply"].normalizedTime >= 1f))
        {
            currentBladeSta = totalBladeSta;
            currentBladeNum = totalBladeNum;
            Equipment.Weapon.AmountLeft = Equipment.Weapon.AmountRight = totalBladeNum;
            currentGas = totalGas;
            if (!useGun)
            {
                setup.part_blade_l.SetActive(true);
                setup.part_blade_r.SetActive(true);
            }
            else
            {
                leftBulletLeft = rightBulletLeft = bulletMAX;
                rightGunHasBullet = true;
                leftGunHasBullet = true;
                setup.part_blade_l.SetActive(true);
                setup.part_blade_r.SetActive(true);
                UpdateRightMagUI();
                UpdateLeftMagUI();
            }
            Idle();
        }
    }

    /// <summary>
    /// Called by <see cref="Update_HeroState"/>
    /// </summary>
    void Update_HeroState_Slide()
    {
        if (!grounded)
        {
            Idle();
        }
    }

    /// <summary>
    /// Called by <see cref="Update_HeroState"/>
    /// </summary>
    void Update_HeroState_AirDodge()
    {
        if (dashTime > 0f)
        {
            dashTime -= Time.deltaTime;
            if (currentSpeed > originVM)
            {
                rigidBody.AddForce(((-rigidBody.velocity * Time.deltaTime) * 1.7f), ForceMode.VelocityChange);
            }
        }
        else
        {
            dashTime = 0f;
            Idle();
        }
    }

    #endregion

    #region Weapon Collider
    public void WeaponColliderClearHits()
    {
        checkLeftTrigger.ClearHits();
        checkRightTrigger.ClearHits();
    }
    public void ActivateWeaponCollider(bool active)
    {
        checkLeftTrigger.IsActive = active;
        checkRightTrigger.IsActive = active;
    }
    public void ActivateWeaponCollider(bool? left, bool? right)
    {
        if (left.HasValue)
            checkLeftTrigger.IsActive = left.Value;
        if (right.HasValue)
            checkRightTrigger.IsActive = right.Value;
    }
    public void ActivateWeaponCollider(bool left, bool right)
    {
        checkLeftTrigger.IsActive = left;
        checkRightTrigger.IsActive = right;
    }
    #endregion



    public override void OnHit(Entity attacker, int damage)
    {
        //TODO: 160 HERO OnHit logic
        //if (!isInvincible() && _state != HERO_STATE.Grab)
        //    markDie();
    }

    private void ApplyForceToBody(GameObject GO, Vector3 v)
    {
        if (GO.TryGetComponent<Rigidbody>(out var r))
        {
            r.AddForce(v);
            r.AddTorque(
                Random.Range(-10f, 10f),
                Random.Range(-10f, 10f),
                Random.Range(-10f, 10f));
        }
    }

    public void AttackAccordingToMouse()
    {
        attackAnimation = Input.mousePosition.x < (Screen.width * 0.5) ? ANIM_ATTACK_2 : ANIM_ATTACK_1;
    }

    public void AttackAccordingToTarget(Transform a)
    {
        var vector = a.position - transform.position;
        float current = -Mathf.Atan2(vector.z, vector.x) * Mathf.Rad2Deg;
        float f = -Mathf.DeltaAngle(current, transform.rotation.eulerAngles.y - 90f);
        if (((Mathf.Abs(f) < 90f) && (vector.magnitude < 6f)) && ((a.position.y <= (transform.position.y + 2f)) && (a.position.y >= (transform.position.y - 5f))))
        {
            attackAnimation = "attack4";
        }
        else
        {
            attackAnimation = f > 0f ? ANIM_ATTACK_1 : ANIM_ATTACK_2;
        }
    }



    public void BackToHuman()
    {
        smoothSyncMovement.disabled = false;
        rigidBody.velocity = Vector3.zero;
        titanForm = false;
        Ungrabbed();
        FalseAttack();
        skillCDDuration = skillCDLast;
        GameObject.Find(NAME_MAIN_CAMERA).GetComponent<IN_GAME_MAIN_CAMERA>().SetMainObject(gameObject, true, false);
        photonView.RPC(nameof(BackToHumanRPC), PhotonTargets.Others, new object[0]);
    }

    public void BombInit()
    {
        skillIDHUD = skillId;
        skillCDDuration = skillCDLast;
        if (GameSettings.PvP.Bomb == true)
        {
            int num = (int) FengGameManagerMKII.settings[250];
            int num2 = (int) FengGameManagerMKII.settings[251];
            int num3 = (int) FengGameManagerMKII.settings[252];
            int num4 = (int) FengGameManagerMKII.settings[253];
            if ((num < 0) || (num > 10))
            {
                num = 5;
                FengGameManagerMKII.settings[250] = 5;
            }
            if ((num2 < 0) || (num2 > 10))
            {
                num2 = 5;
                FengGameManagerMKII.settings[251] = 5;
            }
            if ((num3 < 0) || (num3 > 10))
            {
                num3 = 5;
                FengGameManagerMKII.settings[252] = 5;
            }
            if ((num4 < 0) || (num4 > 10))
            {
                num4 = 5;
                FengGameManagerMKII.settings[253] = 5;
            }
            if ((((num + num2) + num3) + num4) > 20)
            {
                num = 5;
                num2 = 5;
                num3 = 5;
                num4 = 5;
                FengGameManagerMKII.settings[250] = 5;
                FengGameManagerMKII.settings[251] = 5;
                FengGameManagerMKII.settings[252] = 5;
                FengGameManagerMKII.settings[253] = 5;
            }
            bombSettings.bombTimeMax = ((num2 * 60f) + 200f) / ((num3 * 60f) + 200f);
            bombSettings.bombRadius = (num * 4f) + 20f;
            bombSettings.bombCD = (num4 * -0.4f) + 5f;
            bombSettings.bombSpeed = (num3 * 60f) + 200f;
            ExitGames.Client.Photon.Hashtable propertiesToSet = new ExitGames.Client.Photon.Hashtable();
            propertiesToSet.Add(PhotonPlayerProperty.RCBombR, (float) FengGameManagerMKII.settings[246]);
            propertiesToSet.Add(PhotonPlayerProperty.RCBombG, (float) FengGameManagerMKII.settings[247]);
            propertiesToSet.Add(PhotonPlayerProperty.RCBombB, (float) FengGameManagerMKII.settings[248]);
            propertiesToSet.Add(PhotonPlayerProperty.RCBombA, (float) FengGameManagerMKII.settings[249]);
            propertiesToSet.Add(PhotonPlayerProperty.RCBombRadius, bombSettings.bombRadius);
            PhotonNetwork.player.SetCustomProperties(propertiesToSet);
            skillId = BOMB_STRING;
            skillIDHUD = "armin";
            skillCDLast = bombSettings.bombCD;
            skillCDDuration = 10f;
            if (Service.Time.GetRoundTime() > 10f)
            {
                skillCDDuration = 5f;
            }
        }
    }

    private void BreakApart(Vector3 v, bool isBite)
    {
        GameObject obj6;
        GameObject obj7;
        GameObject obj8;
        GameObject obj9;
        GameObject obj10;
        var obj2 = (GameObject) Instantiate(Resources.Load("Character_parts/AOTTG_HERO_body"), transform.position, transform.rotation);
        var objHeroSetup = obj2.GetComponent<HERO_SETUP>();
        objHeroSetup.myCostume = setup.myCostume;
        objHeroSetup.isDeadBody = true;
        obj2.GetComponent<HERO_DEAD_BODY_SETUP>().init(currentAnimation, animation[currentAnimation].normalizedTime, BODY_PARTS.ARM_R);
        if (!isBite)
        {
            var gO = (GameObject) Instantiate(Resources.Load("Character_parts/AOTTG_HERO_body"), transform.position, transform.rotation);
            var obj4 = (GameObject) Instantiate(Resources.Load("Character_parts/AOTTG_HERO_body"), transform.position, transform.rotation);
            var obj5 = (GameObject) Instantiate(Resources.Load("Character_parts/AOTTG_HERO_body"), transform.position, transform.rotation);
            gO.gameObject.GetComponent<HERO_SETUP>().myCostume = setup.myCostume;
            obj4.gameObject.GetComponent<HERO_SETUP>().myCostume = setup.myCostume;
            obj5.gameObject.GetComponent<HERO_SETUP>().myCostume = setup.myCostume;
            gO.GetComponent<HERO_SETUP>().isDeadBody = true;
            obj4.GetComponent<HERO_SETUP>().isDeadBody = true;
            obj5.GetComponent<HERO_SETUP>().isDeadBody = true;
            gO.GetComponent<HERO_DEAD_BODY_SETUP>().init(currentAnimation, animation[currentAnimation].normalizedTime, BODY_PARTS.UPPER);
            obj4.GetComponent<HERO_DEAD_BODY_SETUP>().init(currentAnimation, animation[currentAnimation].normalizedTime, BODY_PARTS.LOWER);
            obj5.GetComponent<HERO_DEAD_BODY_SETUP>().init(currentAnimation, animation[currentAnimation].normalizedTime, BODY_PARTS.ARM_L);
            ApplyForceToBody(gO, v);
            ApplyForceToBody(obj4, v);
            ApplyForceToBody(obj5, v);
            if (photonView.isMine)
            {
                currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().SetMainObject(gO, false, false);
            }
        }
        else if (photonView.isMine)
        {
            currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().SetMainObject(obj2, false, false);
        }
        ApplyForceToBody(obj2, v);
        var handL = body.HandLeft; //transform.Find("Amarture/Controller_Body/hip/spine/chest/shoulder_L/upper_arm_L/forearm_L/hand_L");
        var handR = body.HandRight; //transform.Find("Amarture/Controller_Body/hip/spine/chest/shoulder_R/upper_arm_R/forearm_R/hand_R");
        if (useGun)
        {
            obj6 = (GameObject) Instantiate(Resources.Load("Character_parts/character_gun_l"), handL.position, handL.rotation);
            obj7 = (GameObject) Instantiate(Resources.Load("Character_parts/character_gun_r"), handR.position, handR.rotation);
            obj8 = (GameObject) Instantiate(Resources.Load("Character_parts/character_3dmg_2"), handL.position, handL.rotation);
            obj9 = (GameObject) Instantiate(Resources.Load("Character_parts/character_gun_mag_l"), handL.position, handL.rotation);
            obj10 = (GameObject) Instantiate(Resources.Load("Character_parts/character_gun_mag_r"), handL.position, handL.rotation);
        }
        else
        {
            obj6 = (GameObject) Instantiate(Resources.Load("Character_parts/character_blade_l"), handL.position, handL.rotation);
            obj7 = (GameObject) Instantiate(Resources.Load("Character_parts/character_blade_r"), handR.position, handR.rotation);
            obj8 = (GameObject) Instantiate(Resources.Load("Character_parts/character_3dmg"), handL.position, handL.rotation);
            obj9 = (GameObject) Instantiate(Resources.Load("Character_parts/character_3dmg_gas_l"), handL.position, handL.rotation);
            obj10 = (GameObject) Instantiate(Resources.Load("Character_parts/character_3dmg_gas_r"), handL.position, handL.rotation);
        }
        obj6.GetComponent<Renderer>().material = CharacterMaterials.materials[setup.myCostume._3dmg_texture];
        obj7.GetComponent<Renderer>().material = CharacterMaterials.materials[setup.myCostume._3dmg_texture];
        obj8.GetComponent<Renderer>().material = CharacterMaterials.materials[setup.myCostume._3dmg_texture];
        obj9.GetComponent<Renderer>().material = CharacterMaterials.materials[setup.myCostume._3dmg_texture];
        obj10.GetComponent<Renderer>().material = CharacterMaterials.materials[setup.myCostume._3dmg_texture];
        ApplyForceToBody(obj6, v);
        ApplyForceToBody(obj7, v);
        ApplyForceToBody(obj8, v);
        ApplyForceToBody(obj9, v);
        ApplyForceToBody(obj10, v);
    }

    private void BufferUpdate()
    {
        if (buffTime > 0f)
        {
            buffTime -= Time.deltaTime;
            if (buffTime <= 0f)
            {
                buffTime = 0f;
                if ((currentBuff == BUFF.SpeedUp) && animation.IsPlaying(ANIM_RUN_SASHA))
                {
                    CrossFade(ANIM_RUN_1, 0.1f);
                }
                currentBuff = BUFF.NoBuff;
            }
        }
    }

    public void Cache()
    {
        rigidBody = GetComponent<Rigidbody>();
        if (photonView.isMine)
        {
            animation = GetComponent<Animation>();
            cachedSprites = new Dictionary<string, Image>();

            hookUI.Find();

            foreach (Image image in InGameUI.GetComponentsInChildren(typeof(Image), true))
            {
                if (image == null) continue;
                if (image.gameObject.name.Contains("Gas"))
                {
                    cachedSprites.Add(image.gameObject.name, image);
                }
            }
        }
    }

    private void CalcFlareCD()
    {
        if (flare1CD > 0f)
        {
            flare1CD -= Time.deltaTime;
            if (flare1CD < 0f)
            {
                flare1CD = 0f;
            }
        }
        if (flare2CD > 0f)
        {
            flare2CD -= Time.deltaTime;
            if (flare2CD < 0f)
            {
                flare2CD = 0f;
            }
        }
        if (flare3CD > 0f)
        {
            flare3CD -= Time.deltaTime;
            if (flare3CD < 0f)
            {
                flare3CD = 0f;
            }
        }
    }

    private void CalcSkillCD()
    {
        if (skillCDDuration > 0f)
        {
            skillCDDuration -= Time.deltaTime;
            if (skillCDDuration < 0f)
            {
                skillCDDuration = 0f;
            }
        }
    }



    private void CheckDashDoubleTap()
    {
        if (uTapTime >= 0f)
        {
            uTapTime += Time.deltaTime;
            if (uTapTime > 0.2f)
            {
                uTapTime = -1f;
            }
        }
        if (dTapTime >= 0f)
        {
            dTapTime += Time.deltaTime;
            if (dTapTime > 0.2f)
            {
                dTapTime = -1f;
            }
        }
        if (lTapTime >= 0f)
        {
            lTapTime += Time.deltaTime;
            if (lTapTime > 0.2f)
            {
                lTapTime = -1f;
            }
        }
        if (rTapTime >= 0f)
        {
            rTapTime += Time.deltaTime;
            if (rTapTime > 0.2f)
            {
                rTapTime = -1f;
            }
        }
        if (InputManager.KeyDown(InputHuman.Forward))
        {
            if (uTapTime == -1f)
            {
                uTapTime = 0f;
            }
            if (uTapTime != 0f)
            {
                dashU = true;
            }
        }
        if (InputManager.KeyDown(InputHuman.Backward))
        {
            if (dTapTime == -1f)
            {
                dTapTime = 0f;
            }
            if (dTapTime != 0f)
            {
                dashD = true;
            }
        }
        if (InputManager.KeyDown(InputHuman.Left))
        {
            if (lTapTime == -1f)
            {
                lTapTime = 0f;
            }
            if (lTapTime != 0f)
            {
                dashL = true;
            }
        }
        if (InputManager.KeyDown(InputHuman.Right))
        {
            if (rTapTime == -1f)
            {
                rTapTime = 0f;
            }
            if (rTapTime != 0f)
            {
                dashR = true;
            }
        }
    }

    private void CheckDashRebind()
    {
        if (InputManager.Key(InputHuman.GasBurst))
        {
            if (InputManager.Key(InputHuman.Forward))
            {
                dashU = true;
            }
            else if (InputManager.Key(InputHuman.Backward))
            {
                dashD = true;
            }
            else if (InputManager.Key(InputHuman.Left))
            {
                dashL = true;
            }
            else if (InputManager.Key(InputHuman.Right))
            {
                dashR = true;
            }
        }
    }

    public void CheckTitan()
    {
        int count;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit[] hitArray = Physics.RaycastAll(ray, 180f, maskGroundEnemyPlayer);
        List<RaycastHit> list = new List<RaycastHit>();
        List<MindlessTitan> list2 = new List<MindlessTitan>();
        for (count = 0; count < hitArray.Length; count++)
        {
            RaycastHit item = hitArray[count];
            list.Add(item);
        }
        list.Sort((x, y) => x.distance.CompareTo(y.distance));
        float num2 = 180f;
        for (count = 0; count < list.Count; count++)
        {
            RaycastHit hit2 = list[count];
            GameObject gameObject = hit2.collider.gameObject;
            if (gameObject.layer == 16)
            {
                if (gameObject.name.Contains("PlayerCollisionDetection") && ((hit2 = list[count]).distance < num2))
                {
                    num2 -= 60f;
                    if (num2 <= 60f)
                    {
                        count = list.Count;
                    }
                    MindlessTitan component = transform.root.gameObject.GetComponent<MindlessTitan>();
                    if (component != null)
                    {
                        list2.Add(component);
                    }
                }
            }
            else
            {
                count = list.Count;
            }
        }
        for (count = 0; count < myTitans.Count; count++)
        {
            MindlessTitan titan2 = myTitans[count];
            if (!list2.Contains(titan2))
            {
                titan2.IsLooked = false;
            }
        }
        for (count = 0; count < list2.Count; count++)
        {
            MindlessTitan titan3 = list2[count];
            titan3.IsLooked = true;
        }
        myTitans = list2;
    }


    public void ContinueAnimation()
    {
        IEnumerator enumerator = animation.GetEnumerator();
        try
        {
            while (enumerator.MoveNext())
            {
                AnimationState current = (AnimationState) enumerator.Current;
                if (current != null && current.speed == 1f)
                {
                    return;
                }
                current.speed = 1f;
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
        CustomAnimationSpeed();
        PlayAnimation(CurrentPlayingClipName());
        if (photonView.isMine)
        {
            photonView.RPC(nameof(NetContinueAnimation), PhotonTargets.Others, new object[0]);
        }
    }

    public void CrossFade(string aniName, float time)
    {
        currentAnimation = aniName;
        animation.CrossFade(aniName, time);
        if (PhotonNetwork.connected && photonView.isMine)
        {
            object[] parameters = new object[] { aniName, time };
            photonView.RPC(nameof(NetCrossFade), PhotonTargets.Others, parameters);
        }
    }

    public void TryCrossFade(string animationName, float time)
    {
        if (!animation.IsPlaying(animationName))
        {
            CrossFade(animationName, time);
        }
    }

    public string CurrentPlayingClipName()
    {
        IEnumerator enumerator = animation.GetEnumerator();
        try
        {
            while (enumerator.MoveNext())
            {
                AnimationState current = (AnimationState) enumerator.Current;
                if (current != null && animation.IsPlaying(current.name))
                {
                    return current.name;
                }
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
        return string.Empty;
    }

    private void CustomAnimationSpeed()
    {
        animation[ANIM_ATTACK_5].speed = 1.85f;
        animation[ANIM_CHANGE_BLADE].speed = 1.2f;
        animation[ANIM_AIR_RELEASE].speed = 0.6f;
        animation[ANIM_CHANGE_BLADE_AIR].speed = 0.8f;
        animation[ANIM_AHSS_RELOAD_BOTH].speed = 0.38f;
        animation[ANIM_AHSS_RELOAD_BOTH_AIR].speed = 0.5f;
        animation[ANIM_AHSS_RELOAD_L].speed = 0.4f;
        animation[ANIM_AHSS_RELOAD_L_AIR].speed = 0.5f;
        animation[ANIM_AHSS_RELOAD_R].speed = 0.4f;
        animation[ANIM_AHSS_RELOAD_R_AIR].speed = 0.5f;
    }

    private void Dash(float horizontal, float vertical)
    {
        if (((dashTime <= 0f) && (currentGas > 0f)) && !isMounted)
        {
            UseGas(totalGas * 0.04f);
            facingDirection = GetGlobalFacingDirection(horizontal, vertical);
            dashV = GetGlobalFacingVector3(facingDirection);
            originVM = currentSpeed;
            Quaternion quaternion = Quaternion.Euler(0f, facingDirection, 0f);
            rigidBody.rotation = quaternion;
            targetRotation = quaternion;
            PhotonNetwork.Instantiate("FX/boost_smoke", transform.position, transform.rotation, 0);
            dashTime = 0.5f;
            CrossFade(ANIM_DASH, 0.1f);
            animation[ANIM_DASH].time = 0.1f;
            State = HERO_STATE.AirDodge;
            FalseAttack();
            rigidBody.AddForce((dashV * 40f), ForceMode.VelocityChange);
        }
    }

    public void Die(Vector3 v, bool isBite)
    {
        if (invincible <= 0f)
        {
            if (titanForm && (erenTitan != null))
            {
                erenTitan.lifeTime = 0.1f;
            }
            if (bulletLeft != null)
            {
                bulletLeft.removeMe();
            }
            if (bulletRight != null)
            {
                bulletRight.removeMe();
            }

            BreakApart(v, isBite);
            currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().gameOver = true;
            FalseAttack();
            hasDied = true;
            audioSystem
                .PlayOneShot(audioSystem.clipDie)
                .Disconnect(audioSystem.clipDie);
            var propertiesToSet = new ExitGames.Client.Photon.Hashtable();
            propertiesToSet.Add(PhotonPlayerProperty.deaths, (int) PhotonNetwork.player.CustomProperties[PhotonPlayerProperty.deaths] + 1);
            photonView.owner.SetCustomProperties(propertiesToSet);
            if (PlayerPrefs.HasKey("EnableSS") && (PlayerPrefs.GetInt("EnableSS") == 1))
            {
                GameObject.Find(NAME_MAIN_CAMERA).GetComponent<IN_GAME_MAIN_CAMERA>().StartSnapShot2(audioSystem.Position, 0, null, 0.02f);
            }
            Destroy(gameObject);
        }
    }

    private void Dodge(bool offTheWall = false)
    {
        if (((!InputManager.Key(InputHorse.Mount) || !myHorse) || isMounted) || (Vector3.Distance(myHorse.transform.position, transform.position) >= 15f))
        {
            State = HERO_STATE.GroundDodge;
            if (!offTheWall)
            {
                float num;
                float num2;
                if (InputManager.Key(InputHuman.Forward))
                {
                    num = 1f;
                }
                else if (InputManager.Key(InputHuman.Backward))
                {
                    num = -1f;
                }
                else
                {
                    num = 0f;
                }
                if (InputManager.Key(InputHuman.Left))
                {
                    num2 = -1f;
                }
                else if (InputManager.Key(InputHuman.Right))
                {
                    num2 = 1f;
                }
                else
                {
                    num2 = 0f;
                }
                float num3 = GetGlobalFacingDirection(num2, num);
                if ((num2 != 0f) || (num != 0f))
                {
                    facingDirection = num3 + 180f;
                    targetRotation = Quaternion.Euler(0f, facingDirection, 0f);
                }
                CrossFade(ANIM_DODGE, 0.1f);
            }
            else
            {
                PlayAnimation(ANIM_DODGE);
                PlayAnimationAt(ANIM_DODGE, 0.2f);
            }
            sparksEmission.enabled = false;
        }
    }

    private void ErenTransform()
    {
        skillCDDuration = skillCDLast;
        if (bulletLeft != null)
        {
            bulletLeft.removeMe();
        }
        if (bulletRight != null)
        {
            bulletRight.removeMe();
        }
        erenTitanGameObject = PhotonNetwork.Instantiate("ErenTitan", transform.position, transform.rotation, 0);
        erenTitan = erenTitanGameObject.GetComponent<ErenTitan>();
        erenTitan.realBody = gameObject;
        var cam = GameObject.Find(NAME_MAIN_CAMERA).GetComponent<IN_GAME_MAIN_CAMERA>();
        cam.FlashBlind();
        cam.SetMainObject(erenTitanGameObject, true, false);
        erenTitan.born();
        erenTitanGameObject.GetComponent<Rigidbody>().velocity = rigidBody.velocity;
        rigidBody.velocity = Vector3.zero;
        transform.position = erenTitan.Body.Neck.position;
        titanForm = true;
        object[] parameters = new object[] { erenTitanGameObject.GetPhotonView().viewID };
        photonView.RPC(nameof(WhoIsMyErenTitan), PhotonTargets.Others, parameters);
        if ((smoke_3dmgEmission.enabled && photonView.isMine))
        {
            object[] objArray2 = new object[] { false };
            photonView.RPC(nameof(Net3DMGSMOKE), PhotonTargets.Others, objArray2);
        }
        smoke_3dmgEmission.enabled = false;
    }

    public void FalseAttack()
    {
        attackMove = false;
        if (useGun)
        {
            if (!attackReleased)
            {
                ContinueAnimation();
                attackReleased = true;
            }
        }
        else
        {
            if (photonView.isMine)
            {
                ActivateWeaponCollider(false);
                WeaponColliderClearHits();
            }
            attackLoop = 0;
            if (!attackReleased)
            {
                ContinueAnimation();
                attackReleased = true;
            }
        }
    }


    private GameObject FindNearestTitan()
    {
        GameObject[] objArray = GameObject.FindGameObjectsWithTag("titan");
        GameObject obj2 = null;
        float positiveInfinity = float.PositiveInfinity;
        var position = transform.position;
        foreach (GameObject obj3 in objArray)
        {
            Vector3 vector2 = obj3.transform.position - position;
            float sqrMagnitude = vector2.sqrMagnitude;
            if (sqrMagnitude < positiveInfinity)
            {
                obj2 = obj3;
                positiveInfinity = sqrMagnitude;
            }
        }
        return obj2;
    }

    //Hotfix for Issue 97.
    private void AddRightForce()
    {
        //Whereas this may not be completely accurate to AoTTG, it is very close. Further balancing required in the future.
        rigidBody.AddForce(rigidBody.velocity * 0.00f, ForceMode.Acceleration);
    }

    private Vector3 GetGlobalFacingVector3(float resultAngle)
    {
        float num = -resultAngle + 90f;
        float x = Mathf.Cos(num * 0.01745329f);
        return new Vector3(x, 0f, Mathf.Sin(num * 0.01745329f));
    }

    private float GetGlobalFacingDirection(float horizontal, float vertical)
    {
        if ((vertical == 0f) && (horizontal == 0f))
        {
            return transform.rotation.eulerAngles.y;
        }
        float y = currentCamera.transform.rotation.eulerAngles.y;
        float num2 = Mathf.Atan2(vertical, horizontal) * Mathf.Rad2Deg;
        num2 = -num2 + 90f;
        return (y + num2);
    }

    private void GetOffHorse()
    {
        PlayAnimation(ANIM_HORSE_GETOFF);
        rigidBody.AddForce((((Vector3.up * 10f) - (transform.forward * 2f)) - (transform.right * 1f)), ForceMode.VelocityChange);
        Unmounted();
    }

    private void GetOnHorse()
    {
        PlayAnimation(ANIM_HORSE_GETON);
        facingDirection = myHorse.transform.rotation.eulerAngles.y;
        targetRotation = Quaternion.Euler(0f, facingDirection, 0f);
    }


    public void Grabbed(GameObject titan, bool leftHand)
    {
        if (isMounted)
        {
            Unmounted();
        }
        State = HERO_STATE.Grab;
        GetComponent<CapsuleCollider>().isTrigger = true;
        FalseAttack();
        titanWhoGrabMe = titan;
        if (titanForm && (erenTitan != null))
        {
            erenTitan.lifeTime = 0.1f;
        }

        smoke_3dmgEmission.enabled = false;
        sparksEmission.enabled = false;
    }

    public void HookedByHuman(int hooker, Vector3 hookPosition)
    {
        object[] parameters = new object[] { hooker, hookPosition };
        photonView.RPC(nameof(RPCHookedByHuman), photonView.owner, parameters);
    }


    public void HookToHuman(GameObject target, Vector3 hookPosition)
    {
        ReleaseIfIHookSb();
        hookTarget = target;
        hookSomeOne = true;
        if (target.TryGetComponent<Hero>(out var h))
        {
            h.HookedByHuman(photonView.viewID, hookPosition);
        }
        launchForce = hookPosition - transform.position;
        float num = Mathf.Pow(launchForce.magnitude, 0.1f);
        if (grounded)
        {
            rigidBody.AddForce((Vector3.up * Mathf.Min((float) (launchForce.magnitude * 0.2f), (float) 10f)), ForceMode.Impulse);
        }
        rigidBody.AddForce(((launchForce * num) * 0.1f), ForceMode.Impulse);
    }

    private void Idle()
    {
        if (State == HERO_STATE.Attack)
        {
            FalseAttack();
        }
        State = HERO_STATE.Idle;
        CrossFade(standAnimation, 0.1f);
    }



    public void MarkDie()
    {
        hasDied = true;
        State = HERO_STATE.Die;
    }


    public void NetDieLocal(Vector3 v, bool isBite, int viewID = -1, string titanName = "", bool killByTitan = true)
    {
        if (photonView.isMine)
        {
            Vector3 vector = (Vector3.up * 5000f);
            if (titanForm && (erenTitan != null))
            {
                erenTitan.lifeTime = 0.1f;
            }
            if (myBomb != null)
            {
                myBomb.destroyMe();
            }
            if (myCannon != null)
            {
                PhotonNetwork.Destroy(myCannon);
            }
            if (skillCD != null)
            {
                skillCD.transform.localPosition = vector;
            }
        }
        if (bulletLeft != null)
        {
            bulletLeft.removeMe();
        }
        if (bulletRight != null)
        {
            bulletRight.removeMe();
        }


        FalseAttack();
        BreakApart(v, isBite);
        if (photonView.isMine)
        {
            var cam = currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>();
            cam.SetSpectorMode(false);
            cam.gameOver = true;
            FengGameManagerMKII.instance.myRespawnTime = 0f;
        }
        hasDied = true;
        audioSystem
            .PlayOneShot(audioSystem.clipDie)
            .Disconnect(audioSystem.clipDie);
        smoothSyncMovement.disabled = true;
        if (photonView.isMine)
        {
            PhotonNetwork.RemoveRPCs(photonView);
            ExitGames.Client.Photon.Hashtable propertiesToSet = new ExitGames.Client.Photon.Hashtable();
            propertiesToSet.Add(PhotonPlayerProperty.dead, true);
            PhotonNetwork.player.SetCustomProperties(propertiesToSet);
            propertiesToSet = new ExitGames.Client.Photon.Hashtable();
            propertiesToSet.Add(PhotonPlayerProperty.deaths, RCextensions.returnIntFromObject(PhotonNetwork.player.CustomProperties[PhotonPlayerProperty.deaths]) + 1);
            PhotonNetwork.player.SetCustomProperties(propertiesToSet);
            if (viewID != -1)
            {
                PhotonView view = PhotonView.Find(viewID);
                if (view != null)
                {
                    FengGameManagerMKII.instance.sendKillInfo(killByTitan, RCextensions.returnStringFromObject(view.owner.CustomProperties[PhotonPlayerProperty.name]), false, RCextensions.returnStringFromObject(PhotonNetwork.player.CustomProperties[PhotonPlayerProperty.name]), 0);
                    propertiesToSet = new ExitGames.Client.Photon.Hashtable();
                    propertiesToSet.Add(PhotonPlayerProperty.kills, RCextensions.returnIntFromObject(view.owner.CustomProperties[PhotonPlayerProperty.kills]) + 1);
                    view.owner.SetCustomProperties(propertiesToSet);
                }
            }
            else
            {
                FengGameManagerMKII.instance.sendKillInfo(!(titanName == string.Empty), titanName, false, RCextensions.returnStringFromObject(PhotonNetwork.player.CustomProperties[PhotonPlayerProperty.name]), 0);
            }
        }
        if (photonView.isMine)
        {
            PhotonNetwork.Destroy(photonView);
        }
        if (PhotonNetwork.isMasterClient)
        {
            int iD = photonView.owner.ID;
            if (FengGameManagerMKII.heroHash.ContainsKey(iD))
            {
                FengGameManagerMKII.heroHash.Remove(iD);
            }
        }
    }



    public void PauseAnimation()
    {
        IEnumerator enumerator = animation.GetEnumerator();
        try
        {
            while (enumerator.MoveNext())
            {
                AnimationState current = (AnimationState) enumerator.Current;
                if (current != null)
                    current.speed = 0f;
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
        if (photonView.isMine)
        {
            photonView.RPC(nameof(NetPauseAnimation), PhotonTargets.Others, new object[0]);
        }
    }

    public void PlayAnimation(string aniName)
    {
        currentAnimation = aniName;
        animation.Play(aniName);
        if (PhotonNetwork.connected && photonView.isMine)
        {
            object[] parameters = new object[] { aniName };
            photonView.RPC(nameof(NetPlayAnimation), PhotonTargets.Others, parameters);
        }
    }

    private void PlayAnimationAt(string aniName, float normalizedTime)
    {
        currentAnimation = aniName;
        animation.Play(aniName);
        animation[aniName].normalizedTime = normalizedTime;
        if (PhotonNetwork.connected && photonView.isMine)
        {
            object[] parameters = new object[] { aniName, normalizedTime };
            photonView.RPC(nameof(NetPlayAnimationAt), PhotonTargets.Others, parameters);
        }
    }

    private void ReleaseIfIHookSb()
    {
        if (hookSomeOne && (hookTarget != null))
        {
            hookTarget.GetPhotonView().RPC(nameof(BadGuyReleaseMe), hookTarget.GetPhotonView().owner, new object[0]);
            hookTarget = null;
            hookSomeOne = false;
        }
    }

    public IEnumerator ReloadSky()
    {
        yield return new WaitForSeconds(0.5f);
        if ((FengGameManagerMKII.skyMaterial != null) && (Camera.main.GetComponent<Skybox>().material != FengGameManagerMKII.skyMaterial))
        {
            Camera.main.GetComponent<Skybox>().material = FengGameManagerMKII.skyMaterial;
        }
    }

    public void ResetAnimationSpeed()
    {
        IEnumerator enumerator = animation.GetEnumerator();
        try
        {
            while (enumerator.MoveNext())
            {
                AnimationState current = (AnimationState) enumerator.Current;
                if (current != null)
                    current.speed = 1f;
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
        CustomAnimationSpeed();
    }

    private void Salute()
    {
        State = HERO_STATE.Salute;
        CrossFade("salute", 0.1f);
    }

    [Obsolete]
    public void SetSkillHUDPosition2()
    {

    }

    public void SetStat2()
    {
        skillCDLast = 1.5f;
        skillId = setup.myCostume.stat.skillId;
        CustomAnimationSpeed();
        switch (skillId)
        {
            case "levi":
                skillCDLast = 3.5f;
                break;
            case "armin":
                skillCDLast = 5f;
                break;
            case "marco":
                skillCDLast = 10f;
                break;
            case SKILL_JEAN:
                skillCDLast = 0.001f;
                break;
            case SKILL_EREN:
                skillCDLast = 120f;
                if (!PhotonNetwork.offlineMode)
                {
                    if (!GameSettings.Gamemode.PlayerShifters.Value)
                    {
                        skillId = "petra";
                        skillCDLast = 1f;
                    }
                    else
                    {
                        var num = 0;
                        foreach (var player in PhotonNetwork.playerList)
                        {
                            if (RCextensions.returnIntFromObject(player.CustomProperties[PhotonPlayerProperty.isTitan]) == 1 &&
                                RCextensions.returnStringFromObject(player.CustomProperties[PhotonPlayerProperty.character]).ToUpper() == "EREN")
                            {
                                num++;
                            }
                        }
                        if (num > 1)
                        {
                            skillId = "petra";
                            skillCDLast = 1f;
                        }
                    }
                }

                break;
            case "sasha":
                skillCDLast = 20f;
                break;
            case "petra":
                skillCDLast = 3.5f;
                break;
        }
        BombInit();
        speed = ((float) setup.myCostume.stat.SPD) / 10f;
        totalGas = currentGas = setup.myCostume.stat.GAS;
        totalBladeSta = currentBladeSta = setup.myCostume.stat.BLA;
        rigidBody.mass = 0.5f - ((setup.myCostume.stat.ACL - 100) * 0.001f);

        if (EquipmentType == EquipmentType.Ahss)
        {
            standAnimation = "AHSS_stand_gun";
            useGun = true;
            gunDummy = new GameObject();
            gunDummy.name = "gunDummy";
            gunDummy.transform.position = transform.position;
            gunDummy.transform.rotation = transform.rotation;
            SetTeam(2);
        }
        else if (setup.myCostume.sex == SEX.FEMALE)
        {
            standAnimation = "stand";
            SetTeam(1);
        }
        else
        {
            standAnimation = "stand_levi";
            SetTeam(1);
        }
    }

    public void SetTeam(int team)
    {
        if (photonView.isMine)
        {
            object[] parameters = new object[] { team };
            photonView.RPC(nameof(SetMyTeam), PhotonTargets.AllBuffered, parameters);
            ExitGames.Client.Photon.Hashtable propertiesToSet = new ExitGames.Client.Photon.Hashtable();
            propertiesToSet.Add(PhotonPlayerProperty.team, team);
            PhotonNetwork.player.SetCustomProperties(propertiesToSet);
        }
        else
        {
            SetMyTeam(team);
        }
    }

    public void ShootFlare(int type)
    {
        bool flag = false;
        if ((type == 1) && (flare1CD == 0f))
        {
            flare1CD = flareTotalCD;
            flag = true;
        }
        if ((type == 2) && (flare2CD == 0f))
        {
            flare2CD = flareTotalCD;
            flag = true;
        }
        if ((type == 3) && (flare3CD == 0f))
        {
            flare3CD = flareTotalCD;
            flag = true;
        }
        if (flag)
        {
            PhotonNetwork.Instantiate("FX/flareBullet" + type, transform.position, transform.rotation, 0).GetComponent<FlareMovement>().dontShowHint();
        }
    }

    private void ShowAimUI2()
    {
        if (MenuManager.IsAnyMenuOpen)
        {
            hookUI.Disable();
        }
        else
        {
            hookUI.Enable();

            CheckTitan();
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            var distance = "???";
            var magnitude = HookRaycastDistance;
            var hitDistance = HookRaycastDistance;
            var hitPoint = ray.GetPoint(hitDistance);

            var mousePos = Input.mousePosition;
            hookUI.cross.position = mousePos;

            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 1000f, maskGroundEnemy))
            {
                magnitude = (hit.point - transform.position).magnitude;
                distance = ((int) magnitude).ToString();
                hitDistance = hit.distance;
                hitPoint = hit.point;
            }

            hookUI.crossImage.color = magnitude > 120f ? Color.red : Color.white;
            hookUI.distanceLabel.transform.localPosition = hookUI.cross.localPosition;

            if (((int) FengGameManagerMKII.settings[189]) == 1)
            {
                distance += "\n" + currentSpeed.ToString("F1") + " u/s";
            }
            else if (((int) FengGameManagerMKII.settings[189]) == 2)
            {
                distance += "\n" + ((currentSpeed / 100f)).ToString("F1") + "K";
            }
            hookUI.distanceLabel.text = distance;

            var offset = transform.right * 0.3f;
            var up = Vector3.up * 0.4f;
            Vector3 upLeft = up - offset;
            Vector3 upRight = up + offset;

            float hitDistMultiplier = (hitDistance <= 50f) ? (hitDistance * 0.05f) : (hitDistance * 0.3f);
            Vector3 leftHookVector = (hitPoint - ((transform.right * hitDistMultiplier))) - (transform.position + upLeft);
            Vector3 rightHookVector = (hitPoint + ((transform.right * hitDistMultiplier))) - (transform.position + upRight);
            leftHookVector.Normalize();
            rightHookVector.Normalize();
            leftHookVector *= HookRaycastDistance;
            rightHookVector *= HookRaycastDistance;
            hitPoint = (transform.position + upLeft) + leftHookVector;
            hitDistance = HookRaycastDistance;
            if (Physics.Linecast(transform.position + upLeft, (transform.position + upLeft) + leftHookVector, out hit, maskGroundEnemy))
            {
                hitPoint = hit.point;
                hitDistance = hit.distance;
            }

            hookUI.crossL.transform.position = currentCamera.WorldToScreenPoint(hitPoint);
            hookUI.crossL.transform.localRotation = Quaternion.Euler(0f, 0f, (Mathf.Atan2(hookUI.crossL.transform.position.y - mousePos.y, hookUI.crossL.transform.position.x - mousePos.x) * Mathf.Rad2Deg) + 180f);
            hookUI.crossImageL.color = hitDistance > 120f ? Color.red : Color.white;

            hitPoint = (transform.position + upRight) + rightHookVector;
            hitDistance = HookRaycastDistance;
            if (Physics.Linecast(transform.position + upRight, (transform.position + upRight) + rightHookVector, out hit, maskGroundEnemy))
            {
                hitPoint = hit.point;
                hitDistance = hit.distance;
            }

            hookUI.crossR.transform.position = currentCamera.WorldToScreenPoint(hitPoint);
            hookUI.crossR.transform.localRotation = Quaternion.Euler(0f, 0f, Mathf.Atan2(hookUI.crossR.transform.position.y - mousePos.y, hookUI.crossR.transform.position.x - mousePos.x) * Mathf.Rad2Deg);
            hookUI.crossImageR.color = hitDistance > 120f ? Color.red : Color.white;
        }
    }

    private void ShowGas2()
    {
        float num = currentGas / totalGas;
        float num2 = currentBladeSta / totalBladeSta;
        cachedSprites[STR_GAS_L].fillAmount = cachedSprites[STR_GAS_R].fillAmount = currentGas / totalGas;
        if (num <= 0.25f)
        {
            cachedSprites[STR_GAS_L].color = cachedSprites[STR_GAS_R].color = Color.red;
        }
        else if (num < 0.5f)
        {
            cachedSprites[STR_GAS_L].color = cachedSprites[STR_GAS_R].color = Color.yellow;
        }
        else
        {
            cachedSprites[STR_GAS_L].color = cachedSprites[STR_GAS_R].color = Color.white;
        }
        Equipment.Weapon.UpdateSupplyUi(InGameUI);
    }


    public void SetHorse()
    {
        if (!photonView.isMine) return;
        if (GameSettings.Horse.Enabled.Value && myHorse == null)
        {
            var position = transform.position + Vector3.up * 5f;
            var rotation = transform.rotation;
            myHorse = Horse.Create(this, position, rotation);
        }

        if (!GameSettings.Horse.Enabled.Value && myHorse != null)
        {
            PhotonNetwork.Destroy(myHorse);
        }
    }


    public IEnumerator StopImmunity(float delay = 5f)
    {
        yield return new WaitForSeconds(delay);
        bombSettings.bombImmune = false;
    }

    private void Suicide()
    {
        NetDieLocal((rigidBody.velocity * 50f), false, -1, string.Empty, true);
        FengGameManagerMKII.instance.needChooseSide = true;
    }

    public void Ungrabbed()
    {
        facingDirection = 0f;
        targetRotation = Quaternion.Euler(0f, 0f, 0f);
        transform.parent = null;
        GetComponent<CapsuleCollider>().isTrigger = false;
        State = HERO_STATE.Idle;
    }

    private void Unmounted()
    {
        myHorse.GetComponent<Horse>().Unmount();
        isMounted = false;
    }


    public void UpdateCannon()
    {
        transform.position = myCannonPlayer.position;
        transform.rotation = myCannonBase.rotation;
    }

    public void UpdateExt()
    {
        if (skillId == BOMB_STRING)
        {
            if (InputManager.KeyDown(InputHuman.AttackSpecial) && (skillCDDuration <= 0f))
            {
                if (!((myBomb == null) || myBomb.disabled))
                {
                    myBomb.Explode(bombSettings.bombRadius);
                }
                detonate = false;
                skillCDDuration = bombSettings.bombCD;

                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                currentV = transform.position;
                targetV = currentV + ((Vector3.forward * 200f));
                if (Physics.Raycast(ray, out var hitInfo, float.MaxValue, maskGroundEnemy))
                {
                    targetV = hitInfo.point;
                }
                Vector3 vector = Vector3.Normalize(targetV - currentV);
                GameObject obj2 = PhotonNetwork.Instantiate("RCAsset/BombMain", currentV + ((vector * 4f)), new Quaternion(0f, 0f, 0f, 1f), 0);
                obj2.GetComponent<Rigidbody>().velocity = (vector * bombSettings.bombSpeed);
                myBomb = obj2.GetComponent<Bomb>();
                bombSettings.bombTime = 0f;
            }
            else if ((myBomb != null) && !myBomb.disabled)
            {
                bombSettings.bombTime += Time.deltaTime;
                bool flag2 = false;
                if (InputManager.KeyUp(InputHuman.AttackSpecial))
                {
                    detonate = true;
                }
                else if (InputManager.KeyDown(InputHuman.AttackSpecial) && detonate)
                {
                    detonate = false;
                    flag2 = true;
                }
                if (bombSettings.bombTime >= bombSettings.bombTimeMax)
                {
                    flag2 = true;
                }
                if (flag2)
                {
                    myBomb.Explode(bombSettings.bombRadius);
                    detonate = false;
                }
            }
        }
    }


    private void UpdateLeftMagUI()
    {
        throw new NotImplementedException($"The Method {nameof(UpdateLeftMagUI)} is not implemented");
    }
    private void UpdateRightMagUI()
    {
        throw new NotImplementedException($"The Method {nameof(UpdateRightMagUI)} is not implemented");
    }
}