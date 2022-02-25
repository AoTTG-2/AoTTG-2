using Assets.Scripts.Audio;
using Assets.Scripts.Characters.Humans.Constants;
using Assets.Scripts.Characters.Humans.Customization;
using Assets.Scripts.Characters.Humans.Equipment;
using Assets.Scripts.Characters.Humans.Skills;
using Assets.Scripts.Characters.Titan;
using Assets.Scripts.Constants;
using Assets.Scripts.Events.Args;
using Assets.Scripts.Extensions;
using Assets.Scripts.Gamemode.Options;
using Assets.Scripts.Serialization;
using Assets.Scripts.Services;
using Assets.Scripts.Settings;
using Assets.Scripts.Settings.New;
using Assets.Scripts.UI.InGame.HUD;
using Assets.Scripts.UI.Input;
using Assets.Scripts.Utility;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Toorah.ScriptableVariables;
using UnityEngine;
using UnityEngine.UI;
using PhotonHash = ExitGames.Client.Photon.Hashtable;

namespace Assets.Scripts.Characters.Humans
{
    /// <summary>
    /// The GOD class for ODMG & Player controllers humans. Very inefficient and poorly written, and requires a lot of refactoring
    /// </summary>
    public class Hero : Human
    {
        public CharacterPrefabs Prefabs;
        public EquipmentType EquipmentType;


        public CombatTimer CombatTimer;
        private SpeedTimer speedTimer;
        private const float HookRaycastDistance = 1000f;



        #region Properties
        public Equipment.Equipment Equipment { get; set; }
        public bool weaponDisabledOnReloading;        //Has the gameObject been disabled for reloading?(Usually reeanbled quickly after this, but can get stuck)
        public Skill Skill { get; set; }
        public HumanState State { get; protected set; } = HumanState.Idle;
        public HumanState _state { get; set; }
        private HumanState state
        {
            get
            {
                return _state;
            }
            set
            {
                if ((_state == HumanState.AirDodge) || (_state == HumanState.GroundDodge))
                {
                    dashTime = 0f;
                }
                _state = value;
            }
        }

        public bool UseWeaponTrail = true; //TODO Add a check in the graphic menu to enable and disable weapon trail//
        private float acl;
        private bool almostSingleHook { get; set; }
        public string attackAnimation { get; set; }
        public int attackLoop { get; set; }
        private bool attackReleased { get; set; }
        private GameObject badGuy { get; set; }
        public float bombCD;
        public bool bombImmune;
        public float bombRadius;
        public float bombSpeed;
        public float bombTime;
        public float bombTimeMax;
        private float buffTime { get; set; }
        private int bulletMAX { get; set; } = 7;
        public Bullet hookLeft { get; private set; }
        public Bullet hookRight { get; private set; }
        private bool buttonAttackRelease { get; set; }
        public Dictionary<string, Image> cachedSprites;
        public float CameraMultiplier;
        public TriggerColliderWeapon checkBoxLeft;
        public TriggerColliderWeapon checkBoxRight;
        public string CurrentAnimation;
        public float currentBladeSta = 100f;
        private BUFF currentBuff { get; set; }
        public Camera currentCamera;
        public IN_GAME_MAIN_CAMERA currentInGameCamera;
        public float currentGas { get; set; } = 100f;
        public float currentSpeed;
        public Vector3 currentV;
        private bool dashD { get; set; }
        public Vector3 dashDirection { get; set; }
        private bool dashL { get; set; }
        private bool dashR { get; set; }
        private float dashTime { get; set; }
        private bool dashU { get; set; }
        private Vector3 dashV { get; set; }
        public bool detonate;
        private float dTapTime { get; set; } = -1f;
        private bool EHold { get; set; }
        private ErenTitan eren_titan { get; set; }
        public float facingDirection { get; set; }
        private Transform forearmL { get; set; }
        private Transform forearmR { get; set; }
        private float Gravity => 20f * gravityModifier;
        private float gravityModifier = GameSettings.Global?.Gravity ?? 1;
        public bool grounded;
        public bool regrounded; //Was falling down or not grounded for at least one frame
        private GameObject gunDummy { get; set; }
        private Vector3 gunTarget { get; set; }
        private Transform handL { get; set; }
        private Transform handR { get; set; }
        private bool hasDied { get; set; }
        public bool hasspawn;
        private bool hookBySomeOne { get; set; } = true;
        public GameObject hookRefL1;
        public GameObject hookRefL2;
        public GameObject hookRefR1;
        public GameObject hookRefR2;
        private bool hookSomeOne { get; set; }
        private GameObject hookTarget { get; set; }
        private float invincible { get; set; } = 3f; // Time when you cannot be harmed after spawning
        public bool isCannon;
        private bool isLaunchLeft { get; set; }
        private bool isLaunchRight { get; set; }
        private bool isLeftHandHooked { get; set; }
        private bool isMounted { get; set; }
        public bool isPhotonCamera;
        private bool isRightHandHooked { get; set; }
        public float jumpHeight = 2f;
        private bool justGrounded { get; set; }
        public Transform lastHook;
        private float launchElapsedTimeL { get; set; }
        private float launchElapsedTimeR { get; set; }
        private Vector3 launchForce { get; set; }
        public Vector3 launchPointLeft { get; private set; }
        public Vector3 launchPointRight { get; private set; }
        private bool leanLeft { get; set; }
        private bool leftArmAim { get; set; }

        public MeleeWeaponTrail leftweapontrail;
        public MeleeWeaponTrail rightweapontrail;

        [Obsolete("Should be within AHSS.cs")]
        public int leftBulletLeft = 7;
        public bool leftGunHasBullet = true;
        private float lTapTime { get; set; } = -1f;
        public GameObject maincamera;
        public float maxVelocityChange = 10f;
        public AudioSource meatDie;
        public Bomb myBomb;
        public GameObject myCannon;
        public Transform myCannonBase;
        public Transform myCannonPlayer;
        public CannonPropRegion myCannonRegion;
        private Horse myHorse { get; set; }
        [Obsolete("Old method of using player names")]
        public GameObject myNetWorkName { get; set; }
        public float myScale = 1f;
        public int myTeam = 1;
        public List<MindlessTitan> myTitans;
        private bool needLean { get; set; }
        private Quaternion oldHeadRotation { get; set; }
        private float originVM { get; set; }
        private bool QHold { get; set; }
        public string reloadAnimation = string.Empty;
        private bool rightArmAim { get; set; }

        [Obsolete("Should be within AHSS.cs")]
        public int rightBulletLeft = 7;
        public bool rightGunHasBullet = true;
        public AudioSource rope;
        public AudioSource ropeNoGas;
        private float rTapTime { get; set; } = -1f;
        private GameObject skillCD { get; set; }
        public float skillCDDuration;
        public float skillCDLast;
        public float skillCDLastCannon;
        public string skillIDHUD;
        public AudioSource slash;
        public AudioSource slashHit;

        [Header("Particles")]
        [SerializeField] private ParticleSystem particle_Smoke_3dmg;
        private ParticleSystem.EmissionModule smoke_3dmg_em;
        [SerializeField] private ParticleSystem particle_Sparks;
        private ParticleSystem.EmissionModule sparks_em;

        public float speed = 10f;
        public GameObject speedFX;
        public GameObject speedFX1;
        public bool spinning;

        private float reelForce;
        private float scrollWheelWait = 0f;
        public float scrollWheelWaitValue = 0.05f;      // Set this value higher if you want to smooth out scrolling more

        private string standAnimation { get; set; } = HeroAnim.STAND;
        private Quaternion targetHeadRotation { get; set; }
        public Quaternion targetRotation { get; set; }
        public Vector3 targetV;
        public bool throwedBlades;
        public bool titanForm;
        private GameObject titanWhoGrabMe { get; set; }
        private int titanWhoGrabMeID { get; set; }
        public float totalBladeSta = 100f;
        public float totalGas = 100f;
        private Transform upperarmL { get; set; }
        private Transform upperarmR { get; set; }
        private float useGasSpeed { get; set; } = 0.2f;
        public bool useGun;
        private float uTapTime { get; set; } = -1f;
        private bool wallJump { get; set; }
        private float wallRunTime { get; set; }

        private readonly System.Diagnostics.Stopwatch burstCD = new System.Diagnostics.Stopwatch();
        private const int BurstCDmin = 1;
        private const int BurstCDmax = 300;

        public bool IsGrabbed => state == HumanState.Grab;
        public bool IsInvincible => (invincible > 0f);


        private readonly HookUI hookUI = new HookUI();
        private Coroutine CombatStateStart;
        #endregion


        public GameObject InGameUI;
        public TextMesh PlayerName;

        // Hero 2.0
        public Animation Animation { get; protected set; }
        public Rigidbody Rigidbody { get; protected set; }
        public SmoothSyncMovement SmoothSync { get; protected set; }

        [SerializeField] StringVariable bombMainPath;

        #region Unity Methods

        protected override void Awake()
        {
            base.Awake();
            Animation = GetComponent<Animation>();
            Rigidbody = GetComponent<Rigidbody>();
            SmoothSync = GetComponent<SmoothSyncMovement>();
            CombatTimer = gameObject.AddComponent<CombatTimer>();
            speedTimer = gameObject.AddComponent<SpeedTimer>();

            InGameUI = GameObject.Find("InGameUi");
            Cache();
            Rigidbody.freezeRotation = true;
            Rigidbody.useGravity = false;
            handL = Body.hand_L;
            handR = Body.hand_R;
            forearmL = Body.forearm_L;
            forearmR = Body.forearm_R;
            upperarmL = Body.upper_arm_L;
            upperarmR = Body.upper_arm_R;
            Equipment = gameObject.AddComponent<Equipment.Equipment>();
            Faction = Service.Faction.GetHumanity();
            Service.Settings.OnGlobalSettingsChanged += OnGlobalSettingsChanged;
            Service.Entity.Register(this);

            CustomAnimationSpeed();
            Setting.Debug.NoClip.OnValueChanged += NoClip_OnValueChanged;
            if (Setting.Debug.NoClip == true)
                NoClip_OnValueChanged(true);
        }

        private void NoClip_OnValueChanged(bool value)
        {
            if (photonView.isMine && PhotonNetwork.isMasterClient)
            {
                gameObject.GetComponent<CapsuleCollider>().enabled = !value; // Inverted as NoClip enabled = no collider
            }
        }

        public void OnGlobalSettingsChanged(GlobalSettings settings)
        {
            if (settings.Gravity.HasValue)
            {
                gravityModifier = settings.Gravity.Value;
            }
        }

        private void Start()
        {
            Service.Music.SetMusicState(new MusicStateChangedEvent(MusicState.Ambient));
            gameObject.AddComponent<PlayerInteractable>();
            SetHorse();

            sparks_em = particle_Sparks.emission;
            smoke_3dmg_em = particle_Smoke_3dmg.emission;

            transform.localScale = new Vector3(myScale, myScale, myScale);
            facingDirection = transform.rotation.eulerAngles.y;
            targetRotation = Quaternion.Euler(0f, facingDirection, 0f);
            smoke_3dmg_em.enabled = false;
            sparks_em.enabled = false;

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
            if (photonView.isMine)
            {
                SmoothSync.PhotonCamera = true;
                photonView.RPC(nameof(SetMyPhotonCamera), PhotonTargets.OthersBuffered,
                    new object[] { PlayerPrefs.GetFloat("cameraDistance") + 0.3f });
            }

            if (!photonView.isMine)
            {
                gameObject.layer = (int) Layers.NetworkObject;
                if (IN_GAME_MAIN_CAMERA.dayLight == DayLight.Night)
                {
                    GameObject obj3 = Instantiate(Resources.Load<GameObject>("flashlight"));
                    obj3.transform.parent = transform;
                    obj3.transform.position = transform.position + Vector3.up;
                    obj3.transform.rotation = Quaternion.Euler(353f, 0f, 0f);
                }
                Destroy(checkBoxLeft);
                Destroy(checkBoxRight);

                hasspawn = true;
            }
            else
            {
                currentCamera = GameObject.Find("MainCamera").GetComponent<Camera>();
                currentInGameCamera = currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>();

                hasspawn = true;
                StartCoroutine(ReloadSky());
                bombImmune = false;
                if (GameSettings.PvP.Bomb.Value)
                {
                    bombImmune = true;
                    StartCoroutine(StopImmunity());
                }
            }
        }
        public void Update()
        {
            // Upon spawning, we cannot be damaged for 3s
            if (invincible > 0f)
            {
                invincible -= Time.deltaTime;
            }

            if (hasDied) return;
            if (titanForm && (eren_titan != null))
            {
                transform.position = eren_titan.Body.Neck.position;
                SmoothSync.disabled = true;
            }
            else if (isCannon && (myCannon != null))
            {
                UpdateCannon();
                SmoothSync.disabled = true;
            }

            if (!photonView.isMine) return;
            if (myCannonRegion != null)
            {
                Service.Ui.SetMessage(LabelPosition.Center, "Press 'Cannon Mount' key to use Cannon.");
                if (InputManager.KeyDown(InputCannon.Mount))
                {
                    myCannonRegion.photonView.RPC(nameof(CannonPropRegion.RequestControlRPC), PhotonTargets.MasterClient, new object[] { photonView.viewID });
                }
            }

            CheckForScrollingInput();

            if (Skill != null)
            {
                if (Skill.IsActive)
                {
                    Skill.OnUpdate();
                }
                else if (InputManager.KeyDown(InputHuman.AttackSpecial) && !isMounted)
                {
                    if (!Skill.Use() && _state == HumanState.Idle)
                    {
                        if (needLean)
                        {
                            if (leanLeft)
                            {
                                attackAnimation = (UnityEngine.Random.Range(0, 100) >= 50) ? HeroAnim.ATTACK1_HOOK_L1 : HeroAnim.ATTACK1_HOOK_L2;
                            }
                            else
                            {
                                attackAnimation = (UnityEngine.Random.Range(0, 100) >= 50) ? HeroAnim.ATTACK1_HOOK_R1 : HeroAnim.ATTACK1_HOOK_R2;
                            }
                        }
                        else
                        {
                            attackAnimation = HeroAnim.ATTACK1;
                        }
                        PlayAnimation(attackAnimation);
                    }
                }
            }

            if ((state == HumanState.Grab) && !useGun)
            {
                if (Skill is ErenSkill)
                {
                    ShowSkillCD();
                    if (!IN_GAME_MAIN_CAMERA.isPausing)
                    {
                        CalcSkillCD();
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
                            if (titanWhoGrabMe.GetComponent<MindlessTitan>() != null)
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
                }
            }
            else if (!titanForm && !isCannon)
            {
                bool isBothHooksPressed;
                bool isRightHookPressed;
                bool isLeftHookPressed;
                BufferUpdate();
                UpdateExt();
                if (state != HumanState.ChangeBlade && weaponDisabledOnReloading)
                {
                    //If the reload animation is cancelled before the weapon has a chance to be reenabled, call this function to do that
                    Equipment.Weapon.EnableWeapons();
                }

                if (!grounded && (state != HumanState.AirDodge))
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
                if (grounded && ((state == HumanState.Idle) || (state == HumanState.Slide)))
                {
                    if (!((!InputManager.KeyDown(InputHuman.Jump) || Animation.IsPlaying(HeroAnim.JUMP)) || Animation.IsPlaying(HeroAnim.HORSE_GET_ON)))
                    {
                        Idle();
                        CrossFade(HeroAnim.JUMP, 0.1f);
                        sparks_em.enabled = false;
                    }
                    if (!((!InputManager.KeyDown(InputHorse.Mount) || Animation.IsPlaying(HeroAnim.JUMP)) || Animation.IsPlaying(HeroAnim.HORSE_GET_ON)) && (((myHorse != null) && !isMounted) && (Vector3.Distance(myHorse.transform.position, transform.position) < 15f)))
                    {
                        GetOnHorse();
                    }
                    if (!((!InputManager.KeyDown(InputHuman.Dodge) || Animation.IsPlaying(HeroAnim.JUMP)) || Animation.IsPlaying(HeroAnim.HORSE_GET_ON)))
                    {
                        Dodge(false);
                        return;
                    }
                }
                if (state == HumanState.Idle)
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
                    if (((Animation.IsPlaying(standAnimation) || !grounded) && InputManager.KeyDown(InputHuman.Reload)) && ((!useGun || (GameSettings.PvP.AhssAirReload.Value)) || grounded))
                    {
                        ChangeBlade();
                        return;
                    }
                    if (Animation.IsPlaying(standAnimation) && InputManager.KeyDown(InputHuman.Salute))
                    {
                        Salute();
                        return;
                    }
                    if ((!isMounted && (InputManager.KeyDown(InputHuman.Attack) || InputManager.KeyDown(InputHuman.AttackSpecial))) && !useGun)
                    {
                        bool flag3 = false;
                        if (InputManager.KeyDown(InputHuman.AttackSpecial))
                        {
                            if ((skillCDDuration > 0f) || flag3)
                            {
                                flag3 = true;
                            }
                            else
                            {
                                skillCDDuration = skillCDLast;
                                //TODO: Eren Skill
                                if (Skill is ErenSkill)
                                {
                                    ErenTransform();
                                    return;
                                }
                                //TODO: Marco Skill
                                if (Skill is MarcoSkill)
                                {
                                    if (IsGrounded())
                                    {
                                        attackAnimation = (UnityEngine.Random.Range(0, 2) != 0) ? HeroAnim.SPECIAL_MARCO_1 : HeroAnim.SPECIAL_MARCO_0;
                                        PlayAnimation(attackAnimation);
                                    }
                                    else
                                    {
                                        flag3 = true;
                                        skillCDDuration = 0f;
                                    }
                                }
                                //TODO: Armin Skill
                                else if (Skill is ArminSkill)
                                {
                                    if (IsGrounded())
                                    {
                                        attackAnimation = HeroAnim.SPECIAL_ARMIN;
                                        PlayAnimation(HeroAnim.SPECIAL_ARMIN);
                                    }
                                    else
                                    {
                                        flag3 = true;
                                        skillCDDuration = 0f;
                                    }
                                }
                                //TODO: Sasha Skill
                                else if (Skill is SashaSkill)
                                {
                                    if (IsGrounded())
                                    {
                                        attackAnimation = HeroAnim.SPECIAL_SASHA;
                                        PlayAnimation(HeroAnim.SPECIAL_SASHA);
                                        currentBuff = BUFF.SpeedUp;
                                        buffTime = 10f;
                                    }
                                    else
                                    {
                                        flag3 = true;
                                        skillCDDuration = 0f;
                                    }
                                }
                            }
                        }
                        else if (InputManager.KeyDown(InputHuman.Attack))
                        {
                            if (needLean)
                            {
                                if (InputManager.Key(InputHuman.Left))
                                {
                                    attackAnimation = (UnityEngine.Random.Range(0, 100) >= 50) ? HeroAnim.ATTACK1_HOOK_L1 : HeroAnim.ATTACK1_HOOK_L2;
                                }
                                else if (InputManager.Key(InputHuman.Right))
                                {
                                    attackAnimation = (UnityEngine.Random.Range(0, 100) >= 50) ? HeroAnim.ATTACK1_HOOK_R1 : HeroAnim.ATTACK1_HOOK_R2;
                                }
                                else if (leanLeft)
                                {
                                    attackAnimation = (UnityEngine.Random.Range(0, 100) >= 50) ? HeroAnim.ATTACK1_HOOK_L1 : HeroAnim.ATTACK1_HOOK_L2;
                                }
                                else
                                {
                                    attackAnimation = (UnityEngine.Random.Range(0, 100) >= 50) ? HeroAnim.ATTACK1_HOOK_R1 : HeroAnim.ATTACK1_HOOK_R2;
                                }
                            }
                            else if (InputManager.Key(InputHuman.Left))
                            {
                                attackAnimation = HeroAnim.ATTACK2;
                            }
                            else if (InputManager.Key(InputHuman.Right))
                            {
                                attackAnimation = HeroAnim.ATTACK1;
                            }
                            else if (lastHook != null && lastHook.TryGetComponent<TitanBase>(out var titan))
                            {
                                if (titan.Body.Neck != null)
                                {
                                    AttackAccordingToTarget(titan.Body.Neck);
                                }
                                else
                                {
                                    flag3 = true;
                                }
                            }
                            else if ((hookLeft != null) && (hookLeft.transform.parent != null))
                            {
                                Transform a = hookLeft.transform.parent.transform.root.Find("Amarture/Core/Controller_Body/hip/spine/chest/neck");
                                if (a != null)
                                {
                                    AttackAccordingToTarget(a);
                                }
                                else
                                {
                                    AttackAccordingToMouse();
                                }
                            }
                            else if ((hookRight != null) && (hookRight.transform.parent != null))
                            {
                                Transform transform2 = hookRight.transform.parent.transform.root.Find("Amarture/Core/Controller_Body/hip/spine/chest/neck");
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
                        if (!flag3)
                        {
                            checkBoxLeft.ClearHits();
                            checkBoxRight.ClearHits();
                            if (grounded)
                            {
                                Rigidbody.AddForce((gameObject.transform.forward * 200f));
                            }
                            PlayAnimation(attackAnimation);
                            Animation[attackAnimation].time = 0f;
                            buttonAttackRelease = false;
                            state = HumanState.Attack;
                            if ((grounded || (attackAnimation == HeroAnim.ATTACK3_1)) || ((attackAnimation == HeroAnim.ATTACK5) || (attackAnimation == HeroAnim.SPECIAL_PETRA)))
                            {
                                attackReleased = true;
                                
                                buttonAttackRelease = true;
                            }
                            else
                            {
                                attackReleased = false;
                            }
                            sparks_em.enabled = false;
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
                            RaycastHit hit3;
                            Ray ray3 = Camera.main.ScreenPointToRay(Input.mousePosition);
                            LayerMask mask = Layers.Ground.ToLayer() | Layers.EnemyBox.ToLayer();
                            if (Physics.Raycast(ray3, out hit3, 1E+07f, mask.value))
                            {
                                gunTarget = hit3.point;
                            }
                        }
                        bool flag4 = false;
                        bool flag5 = false;
                        bool flag6 = false;
                        //TODO: AHSS skill dual shot
                        if (InputManager.KeyUp(InputHuman.AttackSpecial) && (!(Skill is BombPvpSkill)))
                        {
                            if (leftGunHasBullet && rightGunHasBullet)
                            {
                                if (grounded)
                                {
                                    attackAnimation = HeroAnim.AHSS_SHOOT_BOTH;
                                }
                                else
                                {
                                    attackAnimation = HeroAnim.AHSS_SHOOT_BOTH_AIR;
                                }
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
                                    if (isLeftHandHooked)
                                    {
                                        attackAnimation = HeroAnim.AHSS_SHOOT_R;
                                    }
                                    else
                                    {
                                        attackAnimation = HeroAnim.AHSS_SHOOT_L;
                                    }
                                }
                                else if (leftGunHasBullet)
                                {
                                    attackAnimation = HeroAnim.AHSS_SHOOT_L;
                                }
                                else if (rightGunHasBullet)
                                {
                                    attackAnimation = HeroAnim.AHSS_SHOOT_R;
                                }
                            }
                            else if (leftGunHasBullet && rightGunHasBullet)
                            {
                                if (isLeftHandHooked)
                                {
                                    attackAnimation = HeroAnim.AHSS_SHOOT_R_AIR;
                                }
                                else
                                {
                                    attackAnimation = HeroAnim.AHSS_SHOOT_L_AIR;
                                }
                            }
                            else if (leftGunHasBullet)
                            {
                                attackAnimation = HeroAnim.AHSS_SHOOT_L_AIR;
                            }
                            else if (rightGunHasBullet)
                            {
                                attackAnimation = HeroAnim.AHSS_SHOOT_R_AIR;
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
                            state = HumanState.Attack;
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
                else if (state == HumanState.Attack)
                {
                    if (!useGun)
                    {
                        if (!InputManager.Key(InputHuman.Attack))
                        {
                            buttonAttackRelease = true;
                        }
                        if (!attackReleased)
                        {
                            //TODO: Pause the Animation if the player is holding a button
                            if (buttonAttackRelease)
                            {
                                SetAnimationSpeed(CurrentAnimation);
                                attackReleased = true;
                            }
                            else if (Animation[attackAnimation].normalizedTime >= 0.32f && Animation[attackAnimation].speed > 0f)
                            {
                                SetAnimationSpeed(attackAnimation, 0f);
                            }
                        }
                        if ((attackAnimation == HeroAnim.ATTACK3_1) && (currentBladeSta > 0f))
                        {
                            if (Animation[attackAnimation].normalizedTime >= 0.8f)
                            {
                                if (!checkBoxLeft.IsActive)
                                {
                                    checkBoxLeft.IsActive = true;

                                    if (UseWeaponTrail)
                                    {
                                        rightweapontrail.enabled = true;
                                        leftweapontrail.enabled = true;
                                    }

                                    Rigidbody.velocity = (-Vector3.up * 30f);
                                }
                                if (!checkBoxRight.IsActive)
                                {
                                    checkBoxRight.IsActive = true;
                                    slash.Play();
                                }
                            }
                            else if (checkBoxLeft.IsActive)
                            {
                                this.activeBoxes(false);
                                checkBoxLeft.ClearHits();
                                checkBoxRight.ClearHits();
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
                            else if (attackAnimation == HeroAnim.ATTACK5)
                            {
                                num2 = 0.35f;
                                num = 0.5f;
                            }
                            else if (attackAnimation == HeroAnim.SPECIAL_PETRA)
                            {
                                num2 = 0.35f;
                                num = 0.48f;
                            }
                            else if (attackAnimation == HeroAnim.SPECIAL_ARMIN)
                            {
                                num2 = 0.25f;
                                num = 0.35f;
                            }
                            else if (attackAnimation == HeroAnim.ATTACK4)
                            {
                                num2 = 0.6f;
                                num = 0.9f;
                            }
                            else if (attackAnimation == HeroAnim.SPECIAL_SASHA)
                            {
                                num = -1f;
                                num2 = -1f;
                            }
                            else
                            {
                                num2 = 0.5f;
                                num = 0.85f;
                            }
                            if (Animation[attackAnimation].normalizedTime.Between(num2, num))
                            {
                                if (!checkBoxLeft.IsActive)
                                {
                                    checkBoxLeft.IsActive = true;
                                    slash.Play();

                                    if (UseWeaponTrail)
                                    {
                                        rightweapontrail.enabled = true;
                                        leftweapontrail.enabled = true;
                                    }
                                }
                                if (!checkBoxRight.IsActive)
                                {
                                    checkBoxRight.IsActive = true;
                                }
                            }
                            else if (checkBoxLeft.IsActive)
                            {
                                this.activeBoxes(false);
                                checkBoxLeft.ClearHits();
                                checkBoxRight.ClearHits();
                            }
                            if ((attackLoop > 0) && (Animation[attackAnimation].normalizedTime > num))
                            {
                                attackLoop--;
                                PlayAnimationAt(attackAnimation, num2);
                            }
                        }
                        if (Animation[attackAnimation].normalizedTime >= 1f)
                        {
                            if ((attackAnimation == HeroAnim.SPECIAL_MARCO_0) || (attackAnimation == HeroAnim.SPECIAL_MARCO_1))
                            {
                                if (!PhotonNetwork.isMasterClient)
                                {
                                    object[] parameters = new object[] { 5f, 100f };
                                    photonView.RPC(nameof(NetTauntAttack), PhotonTargets.MasterClient, parameters);
                                }
                                else
                                {
                                    NetTauntAttack(5f, 100f);
                                }
                                FalseAttack();
                                Idle();
                            }
                            else if (attackAnimation == HeroAnim.SPECIAL_ARMIN)
                            {
                                if (!PhotonNetwork.isMasterClient)
                                {
                                    photonView.RPC(nameof(NetlaughAttack), PhotonTargets.MasterClient, new object[0]);
                                }
                                else
                                {
                                    NetlaughAttack();
                                }
                                FalseAttack();
                                Idle();
                            }
                            else if (attackAnimation == HeroAnim.ATTACK3_1)
                            {
                                Rigidbody.velocity -= ((Vector3.up * Time.deltaTime) * 30f);
                            }
                            else
                            {
                                FalseAttack();
                                Idle();
                            }
                        }
                        if (Animation.IsPlaying(HeroAnim.ATTACK3_2) && (Animation[HeroAnim.ATTACK3_2].normalizedTime >= 1f))
                        {
                            FalseAttack();
                            Idle();
                        }
                    }
                    else
                    {
                        checkBoxLeft.IsActive = false;
                        checkBoxRight.IsActive = false;
                        transform.rotation = Quaternion.Lerp(transform.rotation, gunDummy.transform.rotation, Time.deltaTime * 30f);
                        if (!attackReleased && (Animation[attackAnimation].normalizedTime > 0.167f))
                        {
                            GameObject obj4;
                            attackReleased = true;
                            bool flag7 = false;
                            if ((attackAnimation == HeroAnim.AHSS_SHOOT_BOTH) || (attackAnimation == HeroAnim.AHSS_SHOOT_BOTH_AIR))
                            {
                                //Should use AHSSShotgunCollider instead of TriggerColliderWeapon.  
                                //Apply that change when abstracting weapons from this class.
                                //Note, when doing the abstraction, the relationship between the weapon collider and the abstracted weapon class should be carefully considered.
                                checkBoxLeft.IsActive = true;
                                checkBoxRight.IsActive = true;
                                flag7 = true;
                                leftGunHasBullet = false;
                                rightGunHasBullet = false;
                                Rigidbody.AddForce((-transform.forward * 1000f), ForceMode.Acceleration);
                            }
                            else
                            {
                                if ((attackAnimation == HeroAnim.AHSS_SHOOT_L) || (attackAnimation == HeroAnim.AHSS_SHOOT_L_AIR))
                                {
                                    checkBoxLeft.IsActive = true;
                                    leftGunHasBullet = false;
                                }
                                else
                                {
                                    checkBoxRight.IsActive = true;
                                    rightGunHasBullet = false;
                                }
                                Rigidbody.AddForce((-transform.forward * 600f), ForceMode.Acceleration);
                            }
                            Rigidbody.AddForce((Vector3.up * 200f), ForceMode.Acceleration);
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
                                obj4 = Instantiate(Resources.Load<GameObject>(prefabName), ((transform.position + (transform.up * 0.8f)) - (transform.right * 0.1f)), transform.rotation);
                            }
                        }
                        if (Animation[attackAnimation].normalizedTime >= 1f ||
                            !Animation.IsPlaying(attackAnimation))
                        {
                            FalseAttack();
                            Idle();
                        }
                    }
                }
                else if (state == HumanState.ChangeBlade)
                {
                    Equipment.Weapon.Reload();

                    if (Animation[reloadAnimation].normalizedTime >= 1f)
                    {
                        Idle();
                    }
                }
                else if (state == HumanState.Salute)
                {
                    if (Animation[HeroAnim.SALUTE].normalizedTime >= 1f)
                    {
                        Idle();
                    }
                }
                else if (state == HumanState.GroundDodge)
                {
                    if (Animation.IsPlaying(HeroAnim.DODGE))
                    {
                        if (!(grounded || (Animation[HeroAnim.DODGE].normalizedTime <= 0.6f)))
                        {
                            Idle();
                        }
                        if (Animation[HeroAnim.DODGE].normalizedTime >= 1f)
                        {
                            Idle();
                        }
                    }
                }
                else if (state == HumanState.Land)
                {
                    if (Animation.IsPlaying(HeroAnim.DASH_LAND) && (Animation[HeroAnim.DASH_LAND].normalizedTime >= 1f))
                    {
                        Idle();
                    }
                }
                else if (state == HumanState.FillGas)
                {
                    if (Animation.IsPlaying(HeroAnim.SUPPLY) && Animation[HeroAnim.SUPPLY].normalizedTime >= 1f)
                    {
                        Equipment.Weapon.Resupply();
                        currentBladeSta = totalBladeSta;
                        currentGas = totalGas;
                        if (useGun)
                        {
                            leftBulletLeft = rightBulletLeft = bulletMAX;
                            rightGunHasBullet = true;
                            leftGunHasBullet = true;
                        }
                        Idle();
                    }
                }
                else if (state == HumanState.Slide)
                {
                    if (!grounded)
                    {
                        state = HumanState.Idle;
                    }
                }
                else if (state == HumanState.AirDodge)
                {
                    if (dashTime > 0f)
                    {
                        dashTime -= Time.deltaTime;
                        if (currentSpeed > originVM)
                        {
                            Rigidbody.AddForce(((-Rigidbody.velocity * Time.deltaTime) * 1.7f), ForceMode.VelocityChange);
                        }
                    }
                    else
                    {
                        dashTime = 0f;
                        // State must be set directly, as Idle() will cause the HERO to briefly enter the stand animation mid-air
                        state = HumanState.Idle;
                    }
                }
                if (InputManager.Key(InputHuman.HookLeft))
                {
                    isLeftHookPressed = true;
                }
                else
                {
                    isLeftHookPressed = false;
                }

                //TODO: Properly refactor these if statements

                // Attack 3_1 = Mikasa part 1
                // Attack 3_2 = Mikasa part 2
                // Attack 5 = Levi spin
                // special_petra = Petra skill

                // If leftHookPressed
                // (Using HeroAnim.ATTACK3_1 OR Attack5 OR Petra OR Grabbed) AND NOT IDLE
                // 

                if (!(isLeftHookPressed ? (((Animation.IsPlaying(HeroAnim.ATTACK3_1) || Animation.IsPlaying(HeroAnim.ATTACK5)) || (Animation.IsPlaying(HeroAnim.SPECIAL_PETRA) || (state == HumanState.Grab))) ? (state != HumanState.Idle) : false) : true))

                {
                    if (hookLeft != null)
                    {
                        QHold = true;
                    }
                    else
                    {
                        RaycastHit hit4;
                        Ray ray4 = Camera.main.ScreenPointToRay(Input.mousePosition);
                        LayerMask mask = Layers.Ground.ToLayer() | Layers.EnemyBox.ToLayer();

                        if (Physics.Raycast(ray4, out hit4, HookRaycastDistance, mask.value))
                        {
                            LaunchLeftRope(hit4.distance, hit4.point, true);
                        }
                        else
                        {
                            LaunchLeftRope(HookRaycastDistance, ray4.GetPoint(HookRaycastDistance), true);
                        }
                        if (currentGas > 0) rope.Play();
                        else if (InputManager.KeyDown(InputHuman.HookLeft)) ropeNoGas.Play();
                    }
                }
                else
                {
                    QHold = false;
                }
                if (InputManager.Key(InputHuman.HookRight))
                {
                    isRightHookPressed = true;
                }
                else
                {
                    isRightHookPressed = false;
                }
                if (!(isRightHookPressed ? (((Animation.IsPlaying(HeroAnim.ATTACK3_1) || Animation.IsPlaying(HeroAnim.ATTACK5)) || (Animation.IsPlaying(HeroAnim.SPECIAL_PETRA) || (state == HumanState.Grab))) ? (state != HumanState.Idle) : false) : true))
                {
                    if (hookRight != null)
                    {
                        EHold = true;
                    }
                    else
                    {
                        RaycastHit hit5;
                        Ray ray5 = Camera.main.ScreenPointToRay(Input.mousePosition);
                        LayerMask mask = Layers.Ground.ToLayer() | Layers.EnemyBox.ToLayer();

                        if (Physics.Raycast(ray5, out hit5, HookRaycastDistance, mask.value))
                        {
                            LaunchRightRope(hit5.distance, hit5.point, true);
                        }
                        else
                        {
                            LaunchRightRope(HookRaycastDistance, ray5.GetPoint(HookRaycastDistance), true);
                        }
                        if (currentGas > 0) rope.Play();
                        else if (InputManager.KeyDown(InputHuman.HookRight)) ropeNoGas.Play();
                    }
                }
                else
                {
                    EHold = false;
                }
                if (InputManager.Key(InputHuman.HookBoth))
                {
                    isBothHooksPressed = true;
                }
                else
                {
                    isBothHooksPressed = false;
                }
                if (!(isBothHooksPressed ? (((Animation.IsPlaying(HeroAnim.ATTACK3_1) || Animation.IsPlaying(HeroAnim.ATTACK5)) || (Animation.IsPlaying(HeroAnim.SPECIAL_PETRA) || (state == HumanState.Grab))) ? (state != HumanState.Idle) : false) : true))
                {
                    QHold = true;
                    EHold = true;
                    if ((hookLeft == null) && (hookRight == null))
                    {
                        RaycastHit hit6;
                        Ray ray6 = Camera.main.ScreenPointToRay(Input.mousePosition);
                        LayerMask mask = Layers.Ground.ToLayer() | Layers.EnemyBox.ToLayer();

                        if (Physics.Raycast(ray6, out hit6, HookRaycastDistance, mask.value))
                        {
                            LaunchLeftRope(hit6.distance, hit6.point, false);
                            LaunchRightRope(hit6.distance, hit6.point, false);
                        }
                        else
                        {
                            LaunchLeftRope(HookRaycastDistance, ray6.GetPoint(HookRaycastDistance), false);
                            LaunchRightRope(HookRaycastDistance, ray6.GetPoint(HookRaycastDistance), false);
                        }
                        if (currentGas > 0) rope.Play();
                        else if (InputManager.KeyDown(InputHuman.HookBoth)) ropeNoGas.Play();
                    }
                }
                if (!IN_GAME_MAIN_CAMERA.isPausing)
                {
                    CalcSkillCD();
                    ShowGas();
                    ShowAimUI();
                }
            }
        }

        private void CheckForScrollingInput()
        {
            if (InputManager.Key(InputHuman.ReelIn))
            {
                reelForce = -1f;
            }
            else if (InputManager.Key(InputHuman.ReelOut))
            {
                reelForce = 1f;
            }
            else if (Input.GetAxis("Mouse ScrollWheel") != 0)
            {
                reelForce = Input.GetAxis("Mouse ScrollWheel") * 5555f;
                scrollWheelWait = scrollWheelWaitValue;
            }
            else
            {
                if (scrollWheelWait > 0)
                {
                    scrollWheelWait -= Time.deltaTime;
                }
                else
                {
                    reelForce = 0f;
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
            Service.Settings.OnGlobalSettingsChanged -= OnGlobalSettingsChanged;
            Setting.Debug.NoClip.OnValueChanged -= NoClip_OnValueChanged;
        }

        public void LateUpdate()
        {
            if ((myNetWorkName != null))
            {
                if (titanForm && (eren_titan != null))
                {
                    myNetWorkName.transform.localPosition = ((Vector3.up * Screen.height) * 2f);
                }
                Vector3 start = new Vector3(transform.position.x, transform.position.y + 2f, transform.position.z);

                LayerMask mask = Layers.Ground.ToLayer() | Layers.EnemyBox.ToLayer();

                if ((Vector3.Angle(maincamera.transform.forward, start - maincamera.transform.position) > 90f) || Physics.Linecast(start, maincamera.transform.position, mask))
                {
                    myNetWorkName.transform.localPosition = ((Vector3.up * Screen.height) * 2f);
                }
                else
                {
                    Vector2 vector2 = maincamera.GetComponent<Camera>().WorldToScreenPoint(start);
                    myNetWorkName.transform.localPosition = new Vector3((float) ((int) (vector2.x - (Screen.width * 0.5f))), (float) ((int) (vector2.y - (Screen.height * 0.5f))), 0f);
                }
            }
            if (!titanForm && !isCannon)
            {
                if (InputManager.Settings.CameraTilt && (photonView.isMine))
                {
                    Quaternion quaternion2;
                    Vector3 zero = Vector3.zero;
                    Vector3 position = Vector3.zero;
                    if ((isLaunchLeft && (hookLeft != null)) && hookLeft.isHooked())
                    {
                        zero = hookLeft.transform.position;
                    }
                    if ((isLaunchRight && (hookRight != null)) && hookRight.isHooked())
                    {
                        position = hookRight.transform.position;
                    }
                    Vector3 vector5 = Vector3.zero;
                    if ((zero.magnitude != 0f) && (position.magnitude == 0f))
                    {
                        vector5 = zero;
                    }
                    else if ((zero.magnitude == 0f) && (position.magnitude != 0f))
                    {
                        vector5 = position;
                    }
                    else if ((zero.magnitude != 0f) && (position.magnitude != 0f))
                    {
                        vector5 = ((zero + position) * 0.5f);
                    }
                    Vector3 from = Vector3.Project(vector5 - transform.position, maincamera.transform.up);
                    Vector3 vector7 = Vector3.Project(vector5 - transform.position, maincamera.transform.right);
                    if (vector5.magnitude > 0f)
                    {
                        Vector3 to = from + vector7;
                        float num = Vector3.Angle(vector5 - transform.position, Rigidbody.velocity) * 0.005f;
                        Vector3 vector9 = maincamera.transform.right + vector7.normalized;
                        quaternion2 = Quaternion.Euler(maincamera.transform.rotation.eulerAngles.x, maincamera.transform.rotation.eulerAngles.y, (vector9.magnitude >= 1f) ? (-Vector3.Angle(from, to) * num) : (Vector3.Angle(from, to) * num));
                    }
                    else
                    {
                        quaternion2 = Quaternion.Euler(maincamera.transform.rotation.eulerAngles.x, maincamera.transform.rotation.eulerAngles.y, 0f);
                    }
                    maincamera.transform.rotation = Quaternion.Lerp(maincamera.transform.rotation, quaternion2, Time.deltaTime * 2f);
                }
                if ((state == HumanState.Grab) && (titanWhoGrabMe != null))
                {
                    if (titanWhoGrabMe.TryGetComponent<MindlessTitan>(out var mindlessTitan))
                    {
                        transform.position = mindlessTitan.grabTF.transform.position;
                        transform.rotation = mindlessTitan.grabTF.transform.rotation;
                    }
                    else if (titanWhoGrabMe.TryGetComponent<FemaleTitan>(out var femaleTitan))
                    {
                        transform.position = femaleTitan.grabTF.transform.position;
                        transform.rotation = femaleTitan.grabTF.transform.rotation;
                    }
                }
                if (useGun)
                {
                    if (leftArmAim || rightArmAim)
                    {
                        Vector3 vector10 = gunTarget - transform.position;
                        float current = -Mathf.Atan2(vector10.z, vector10.x) * Mathf.Rad2Deg;
                        float num3 = -Mathf.DeltaAngle(current, transform.rotation.eulerAngles.y - 90f);
                        HeadMovement();
                        if ((!isLeftHandHooked && leftArmAim) && ((num3 < 40f) && (num3 > -90f)))
                        {
                            LeftArmAimTo(gunTarget);
                        }
                        if ((!isRightHandHooked && rightArmAim) && ((num3 > -40f) && (num3 < 90f)))
                        {
                            RightArmAimTo(gunTarget);
                        }
                    }
                    else if (!grounded)
                    {
                        handL.localRotation = Quaternion.Euler(90f, 0f, 0f);
                        handR.localRotation = Quaternion.Euler(-90f, 0f, 0f);
                    }
                    if (isLeftHandHooked && (hookLeft != null))
                    {
                        LeftArmAimTo(hookLeft.transform.position);
                    }
                    if (isRightHandHooked && (hookRight != null))
                    {
                        RightArmAimTo(hookRight.transform.position);
                    }
                }
                SetHookedPplDirection();
                BodyLean();
            }
        }

        private void FixedUpdate()
        {
            if (!photonView.isMine) return;
            if ((!titanForm && !isCannon) && (!IN_GAME_MAIN_CAMERA.isPausing))
            {
                currentSpeed = Rigidbody.velocity.magnitude;

                if (currentSpeed > 150)
                {
                    speedTimer.AddTime(2);
                }

                if (!((Animation.IsPlaying(HeroAnim.ATTACK3_2) || Animation.IsPlaying(HeroAnim.ATTACK5)) || Animation.IsPlaying(HeroAnim.SPECIAL_PETRA)))
                {
                    Rigidbody.rotation = Quaternion.Lerp(gameObject.transform.rotation, targetRotation, Time.deltaTime * 6f);
                }
                if (state == HumanState.Grab)
                {
                    Rigidbody.AddForce(-Rigidbody.velocity, ForceMode.VelocityChange);
                }
                else
                {
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
                        regrounded = false;
                    }

                    if (Skill.IsActive)
                    {
                        Skill.OnFixedUpdate();
                    }

                    if (hookSomeOne)
                    {
                        if (hookTarget != null)
                        {
                            Vector3 vector2 = hookTarget.transform.position - transform.position;
                            float magnitude = vector2.magnitude;
                            if (magnitude > 2f)
                            {
                                Rigidbody.AddForce((((vector2.normalized * Mathf.Pow(magnitude, 0.15f)) * 30f) - (Rigidbody.velocity * 0.95f)), ForceMode.VelocityChange);
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
                                Rigidbody.AddForce(((vector3.normalized * Mathf.Pow(f, 0.15f)) * 0.2f), ForceMode.Impulse);
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
                    
                    bool canUseGas = false;
                    bool canReelOffLeftHook = false;
                    bool canReelOffRightHook = false;
                    isLeftHandHooked = false;
                    isRightHandHooked = false;

                    if (isLaunchLeft)
                    {
                        if ((hookLeft != null) && hookLeft.isHooked())
                        {
                            isLeftHandHooked = true;
                            Vector3 dirToLeftHook = hookLeft.transform.position - transform.position;
                            dirToLeftHook.Normalize();
                            dirToLeftHook = (dirToLeftHook * 10f);
                            if (!isLaunchRight)
                            {
                                dirToLeftHook = (dirToLeftHook * 2f);
                            }

                            if ((Vector3.Angle(Rigidbody.velocity, dirToLeftHook) > 90f) && InputManager.Key(InputHuman.Jump))
                            {
                                canReelOffLeftHook = true;
                                canUseGas = true;
                            }

                            if (!canReelOffLeftHook)
                            {
                                Rigidbody.AddForce(dirToLeftHook);
                                if (Vector3.Angle(Rigidbody.velocity, dirToLeftHook) > 90f)
                                {
                                    Rigidbody.AddForce((-Rigidbody.velocity * 2f), ForceMode.Acceleration);
                                }
                            }
                        }
                        launchElapsedTimeL += Time.deltaTime;
                        if (QHold && (currentGas > 0f))
                        {
                            UseGas(useGasSpeed * Time.deltaTime);
                        }
                        else if (launchElapsedTimeL > 0.3f)
                        {
                            isLaunchLeft = false;
                            if (hookLeft != null)
                            {
                                hookLeft.disable();
                                ReleaseIfIHookSb();
                                hookLeft = null;
                                canReelOffLeftHook = false;
                            }
                        }
                    }

                    if (isLaunchRight)
                    {
                        if ((hookRight != null) && hookRight.isHooked())
                        {
                            isRightHandHooked = true;
                            Vector3 dirToRightHook = hookRight.transform.position - transform.position;
                            dirToRightHook.Normalize();
                            dirToRightHook = (dirToRightHook * 10f);
                            if (!isLaunchLeft)
                            {
                                dirToRightHook = (dirToRightHook * 2f);
                            }

                            if ((Vector3.Angle(Rigidbody.velocity, dirToRightHook) > 90f) && InputManager.Key(InputHuman.Jump))
                            {
                                canReelOffRightHook = true;
                                canUseGas = true;
                            }

                            if (!canReelOffRightHook)
                            {
                                Rigidbody.AddForce(dirToRightHook);
                                if (Vector3.Angle(Rigidbody.velocity, dirToRightHook) > 90f)
                                {
                                    Rigidbody.AddForce((-Rigidbody.velocity * 2f), ForceMode.Acceleration);
                                }
                            }
                        }
                        launchElapsedTimeR += Time.deltaTime;
                        if (EHold && (currentGas > 0f))
                        {
                            UseGas(useGasSpeed * Time.deltaTime);
                        }
                        else if (launchElapsedTimeR > 0.3f)
                        {
                            isLaunchRight = false;
                            if (hookRight != null)
                            {
                                hookRight.disable();
                                ReleaseIfIHookSb();
                                hookRight = null;
                                canReelOffRightHook = false;
                            }
                        }
                    }
                    if (grounded)
                    {
                        Vector3 vector7;
                        Vector3 zero = Vector3.zero;
                        if (state == HumanState.Attack)
                        {
                            if (attackAnimation == HeroAnim.ATTACK5)
                            {
                                if ((Animation[attackAnimation].normalizedTime > 0.4f) && (Animation[attackAnimation].normalizedTime < 0.61f))
                                {
                                    Rigidbody.AddForce((gameObject.transform.forward * 200f));
                                }
                            }
                            else if (Animation.IsPlaying(HeroAnim.ATTACK3_2))
                            {
                                zero = Vector3.zero;
                            }
                            else if (Animation.IsPlaying(HeroAnim.ATTACK1) || Animation.IsPlaying(HeroAnim.ATTACK2))
                            {
                                Rigidbody.AddForce((gameObject.transform.forward * 200f));
                            }
                            if (Animation.IsPlaying(HeroAnim.ATTACK3_2))
                            {
                                zero = Vector3.zero;
                            }
                        }
                        if (justGrounded)
                        {
                            //TODO: attackAnimation conditions appear to be useless
                            if ((state != HumanState.Attack) || (((attackAnimation != HeroAnim.ATTACK3_1) && (attackAnimation != HeroAnim.ATTACK5)) && (attackAnimation != HeroAnim.SPECIAL_PETRA)))
                            {
                                if ((((state != HumanState.Attack) && (x == 0f)) && ((z == 0f) && (hookLeft == null))) && ((hookRight == null) && (state != HumanState.FillGas)))
                                {
                                    state = HumanState.Land;
                                    CrossFade(HeroAnim.DASH_LAND, 0.01f);
                                }
                                else
                                {
                                    buttonAttackRelease = true;
                                    if (((state != HumanState.Attack) && (((Rigidbody.velocity.x * Rigidbody.velocity.x) + (Rigidbody.velocity.z * Rigidbody.velocity.z)) > ((speed * speed) * 1.5f))) && (state != HumanState.FillGas))
                                    {
                                        state = HumanState.Slide;
                                        CrossFade(HeroAnim.SLIDE, 0.05f);
                                        facingDirection = Mathf.Atan2(Rigidbody.velocity.x, Rigidbody.velocity.z) * Mathf.Rad2Deg;
                                        targetRotation = Quaternion.Euler(0f, facingDirection, 0f);
                                        sparks_em.enabled = true;
                                    }
                                }
                            }
                            justGrounded = false;
                            zero = Rigidbody.velocity;
                        }
                        if (state == HumanState.GroundDodge)
                        {
                            if ((Animation[HeroAnim.DODGE].normalizedTime >= 0.2f) && (Animation[HeroAnim.DODGE].normalizedTime < 0.8f))
                            {
                                zero = ((-transform.forward * 2.4f) * speed);
                            }
                            if (Animation[HeroAnim.DODGE].normalizedTime > 0.8f)
                            {
                                zero = (Rigidbody.velocity * 0.9f);
                            }
                        }
                        else if (state == HumanState.Idle)
                        {
                            Vector3 vector8 = new Vector3(x, 0f, z);
                            float resultAngle = GetGlobalFacingDirection(x, z);
                            zero = GetGlobaleFacingVector3(resultAngle);
                            float num6 = (vector8.magnitude <= 0.95f) ? ((vector8.magnitude >= 0.25f) ? vector8.magnitude : 0f) : 1f;
                            zero = (zero * num6);
                            zero = (zero * speed);
                            if ((buffTime > 0f) && (currentBuff == BUFF.SpeedUp))
                            {
                                zero = (zero * 4f);
                            }
                            if ((x != 0f) || (z != 0f))
                            {
                                if (((!Animation.IsPlaying(HeroAnim.RUN_1) && !Animation.IsPlaying(HeroAnim.JUMP)) && !Animation.IsPlaying(HeroAnim.RUN_SASHA)) && (!Animation.IsPlaying(HeroAnim.HORSE_GET_ON) || (Animation[HeroAnim.HORSE_GET_ON].normalizedTime >= 0.5f)))
                                {
                                    if ((buffTime > 0f) && (currentBuff == BUFF.SpeedUp))
                                    {
                                        CrossFade(HeroAnim.RUN_SASHA, 0.1f);
                                    }
                                    else
                                    {
                                        CrossFade(HeroAnim.RUN_1, 0.1f);
                                    }
                                }
                            }
                            else
                            {
                                if (!(((Animation.IsPlaying(standAnimation) || (state == HumanState.Land)) || (Animation.IsPlaying(HeroAnim.JUMP) || Animation.IsPlaying(HeroAnim.HORSE_GET_ON))) || Animation.IsPlaying(HeroAnim.GRABBED)))
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
                        else if (state == HumanState.Land)
                        {
                            zero = (Rigidbody.velocity * 0.96f);
                        }
                        else if (state == HumanState.Slide)
                        {
                            zero = (Rigidbody.velocity * 0.99f);
                            if (currentSpeed < (speed * 1.2f))
                            {
                                Idle();
                                sparks_em.enabled = false;
                            }
                        }
                        Vector3 velocity = Rigidbody.velocity;
                        Vector3 force = zero - velocity;
                        force.x = Mathf.Clamp(force.x, -maxVelocityChange, maxVelocityChange);
                        force.z = Mathf.Clamp(force.z, -maxVelocityChange, maxVelocityChange);
                        force.y = 0f;

                        if (velocity.y <= 0f) regrounded = false;
                        if (Animation.IsPlaying(HeroAnim.JUMP) && (Animation[HeroAnim.JUMP].normalizedTime > 0.18f) && !regrounded)
                        {
                            regrounded = true;
                            force.y += 16f;
                        }
                        if ((Animation.IsPlaying(HeroAnim.HORSE_GET_ON) && (Animation[HeroAnim.HORSE_GET_ON].normalizedTime > 0.18f)) && (Animation[HeroAnim.HORSE_GET_ON].normalizedTime < 1f))
                        {
                            float num7 = 6f;
                            force = -Rigidbody.velocity;
                            force.y = num7;
                            float num8 = Vector3.Distance(myHorse.transform.position, transform.position);
                            float num9 = ((0.6f * Gravity) * num8) / 12f;
                            vector7 = myHorse.transform.position - transform.position;
                            force += (num9 * vector7.normalized);
                        }
                        if (!(state == HumanState.Attack && useGun))
                        {
                            Rigidbody.AddForce(force, ForceMode.VelocityChange);
                            Rigidbody.rotation = Quaternion.Lerp(gameObject.transform.rotation, Quaternion.Euler(0f, facingDirection, 0f), Time.deltaTime * 10f);
                        }
                    }
                    else
                    {
                        if (sparks_em.enabled)
                        {
                            sparks_em.enabled = false;
                        }
                        if ((myHorse && (Animation.IsPlaying(HeroAnim.HORSE_GET_ON) || Animation.IsPlaying(HeroAnim.AIR_FALL))) && ((Rigidbody.velocity.y < 0f) && (Vector3.Distance(myHorse.transform.position + Vector3.up * 1.65f, transform.position) < 0.5f)))
                        {
                            transform.position = myHorse.transform.position + Vector3.up * 1.65f;
                            transform.rotation = myHorse.transform.rotation;
                            isMounted = true;
                            CrossFade(HeroAnim.HORSE_IDLE, 0.1f);
                            myHorse.Mount();
                        }
                        if (!((((((state != HumanState.Idle) || Animation.IsPlaying(HeroAnim.DASH)) ||
                            (Animation.IsPlaying(HeroAnim.WALL_RUN) || Animation.IsPlaying(HeroAnim.TO_ROOF))) ||
                            ((Animation.IsPlaying(HeroAnim.HORSE_GET_ON) || Animation.IsPlaying(HeroAnim.HORSE_GET_OFF)) || (Animation.IsPlaying(HeroAnim.AIR_RELEASE) || isMounted))) ||
                            ((Animation.IsPlaying(HeroAnim.AIR_HOOK_L_JUST) && (Animation[HeroAnim.AIR_HOOK_L_JUST].normalizedTime < 1f)) ||
                            (Animation.IsPlaying(HeroAnim.AIR_HOOK_R_JUST) && (Animation[HeroAnim.AIR_HOOK_R_JUST].normalizedTime < 1f)))) ? (Animation[HeroAnim.DASH].normalizedTime < 0.99f) : false))
                        {
                            if (((!isLeftHandHooked && !isRightHandHooked) && ((Animation.IsPlaying(HeroAnim.AIR_HOOK_L) || Animation.IsPlaying(HeroAnim.AIR_HOOK_R)) || Animation.IsPlaying(HeroAnim.AIR_HOOK))) && (Rigidbody.velocity.y > 20f))
                            {
                                Animation.CrossFade(HeroAnim.AIR_RELEASE);
                            }
                            else
                            {
                                bool flag5 = (Mathf.Abs(Rigidbody.velocity.x) + Mathf.Abs(Rigidbody.velocity.z)) > 25f;
                                bool flag6 = Rigidbody.velocity.y < 0f;
                                if (!flag5)
                                {
                                    if (flag6)
                                    {
                                        if (!Animation.IsPlaying(HeroAnim.AIR_FALL))
                                        {
                                            CrossFade(HeroAnim.AIR_FALL, 0.2f);
                                        }
                                    }
                                    else if (!Animation.IsPlaying(HeroAnim.AIR_RISE))
                                    {
                                        CrossFade(HeroAnim.AIR_RISE, 0.2f);
                                    }
                                }
                                else if (!isLeftHandHooked && !isRightHandHooked)
                                {
                                    float current = -Mathf.Atan2(Rigidbody.velocity.z, Rigidbody.velocity.x) * Mathf.Rad2Deg;
                                    float num11 = -Mathf.DeltaAngle(current, transform.rotation.eulerAngles.y - 90f);
                                    if (Mathf.Abs(num11) < 45f)
                                    {
                                        if (!Animation.IsPlaying(HeroAnim.AIR2))
                                        {
                                            CrossFade(HeroAnim.AIR2, 0.2f);
                                        }
                                    }
                                    else if ((num11 < 135f) && (num11 > 0f))
                                    {
                                        if (!Animation.IsPlaying(HeroAnim.AIR2_RIGHT))
                                        {
                                            CrossFade(HeroAnim.AIR2_RIGHT, 0.2f);
                                        }
                                    }
                                    else if ((num11 > -135f) && (num11 < 0f))
                                    {
                                        if (!Animation.IsPlaying(HeroAnim.AIR2_LEFT))
                                        {
                                            CrossFade(HeroAnim.AIR2_LEFT, 0.2f);
                                        }
                                    }
                                    else if (!Animation.IsPlaying(HeroAnim.AIR2_BACKWARD))
                                    {
                                        CrossFade(HeroAnim.AIR2_BACKWARD, 0.2f);
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
                                else if (!Animation.IsPlaying(Equipment.Weapon.HookForward))
                                {
                                    TryCrossFade(Equipment.Weapon.HookForward, 0.1f);
                                }
                            }
                        }
                        if (((state == HumanState.Idle) && Animation.IsPlaying(HeroAnim.AIR_RELEASE)) && (Animation[HeroAnim.AIR_RELEASE].normalizedTime >= 1f))
                        {
                            CrossFade(HeroAnim.AIR_RISE, 0.2f);
                        }
                        if (Animation.IsPlaying(HeroAnim.HORSE_GET_OFF) && (Animation[HeroAnim.HORSE_GET_OFF].normalizedTime >= 1f))
                        {
                            CrossFade(HeroAnim.AIR_RISE, 0.2f);
                        }
                        if (Animation.IsPlaying(HeroAnim.TO_ROOF))
                        {
                            if (Animation[HeroAnim.TO_ROOF].normalizedTime < 0.22f)
                            {
                                Rigidbody.velocity = Vector3.zero;
                                Rigidbody.AddForce(new Vector3(0f, Gravity * Rigidbody.mass, 0f));
                            }
                            else
                            {
                                if (!wallJump)
                                {
                                    wallJump = true;
                                    Rigidbody.AddForce((Vector3.up * 8f), ForceMode.Impulse);
                                }
                                Rigidbody.AddForce((transform.forward * 0.05f), ForceMode.Impulse);
                            }
                            if (Animation[HeroAnim.TO_ROOF].normalizedTime >= 1f)
                            {
                                PlayAnimation(HeroAnim.AIR_RISE);
                            }
                        }
                        else if (!(((((state != HumanState.Idle) || !IsPressDirectionTowardsHero(x, z)) ||
                                     (InputManager.Key(InputHuman.Jump) ||
                                      InputManager.Key(InputHuman.HookLeft))) ||
                                    ((InputManager.Key(InputHuman.HookRight) ||
                                      InputManager.Key(InputHuman.HookBoth)) ||
                                     (!IsFrontGrounded() || Animation.IsPlaying(HeroAnim.WALL_RUN)))) ||
                                   Animation.IsPlaying(HeroAnim.DODGE)))
                        {
                            CrossFade(HeroAnim.WALL_RUN, 0.1f);
                            wallRunTime = 0f;
                        }
                        else if (Animation.IsPlaying(HeroAnim.WALL_RUN))
                        {
                            Rigidbody.AddForce(((Vector3.up * speed)) - Rigidbody.velocity, ForceMode.VelocityChange);
                            wallRunTime += Time.deltaTime;
                            if ((wallRunTime > 1f) || ((z == 0f) && (x == 0f)))
                            {
                                Rigidbody.AddForce(((-transform.forward * speed) * 0.75f), ForceMode.Impulse);
                                Dodge(true);
                            }
                            else if (!IsUpFrontGrounded())
                            {
                                wallJump = false;
                                CrossFade(HeroAnim.TO_ROOF, 0.1f);
                            }
                            else if (!IsFrontGrounded())
                            {
                                CrossFade(HeroAnim.AIR_FALL, 0.1f);
                            }
                        }
                        // If we are using these skills, then we cannot use gas force
                        else if ((!Animation.IsPlaying(HeroAnim.ATTACK5) && !Animation.IsPlaying(HeroAnim.SPECIAL_PETRA)) && (!Animation.IsPlaying(HeroAnim.DASH) && !Animation.IsPlaying(HeroAnim.JUMP)))
                        {
                            Vector3 vector11 = new Vector3(x, 0f, z);
                            float num12 = GetGlobalFacingDirection(x, z);
                            Vector3 vector12 = GetGlobaleFacingVector3(num12);
                            float num13 = (vector11.magnitude <= 0.95f) ? ((vector11.magnitude >= 0.25f) ? vector11.magnitude : 0f) : 1f;
                            vector12 = (vector12 * num13);
                            vector12 = (vector12 * (( acl / 10f) * 2f));
                            if ((x == 0f) && (z == 0f))
                            {
                                if (state == HumanState.Attack)
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

                            if (((!canReelOffLeftHook && !canReelOffRightHook) && (!isMounted && InputManager.Key(InputHuman.Jump))) && (currentGas > 0f))
                            {
                                if ((x != 0f) || (z != 0f))
                                {
                                    Rigidbody.AddForce(vector12, ForceMode.Acceleration);
                                }
                                else
                                {
                                    Rigidbody.AddForce((transform.forward * vector12.magnitude), ForceMode.Acceleration);
                                }
                                canUseGas = true;

                            }
                        }
                        if ((Animation.IsPlaying(HeroAnim.AIR_FALL) && (currentSpeed < 0.2f)) && IsFrontGrounded())
                        {
                            CrossFade(HeroAnim.ON_WALL, 0.3f);
                        }
                    }
                    spinning = false;
                    CheckForScrollingInput();

                    if (canReelOffLeftHook && canReelOffRightHook)
                    {
                        float num14 = currentSpeed + 0.1f;
                        AddRightForce();
                        Vector3 vector13 = (((hookRight.transform.position + hookLeft.transform.position) * 0.5f)) - transform.position;
                        reelForce = Mathf.Clamp(reelForce, -0.8f, 0.8f);

                        float num16 = 1f + reelForce;
                        Vector3 vector14 = Vector3.RotateTowards(vector13, Rigidbody.velocity, 1.53938f * num16, 1.53938f * num16);
                        vector14.Normalize();
                        spinning = true;
                        Rigidbody.velocity = (vector14 * num14);
                    }
                    else if (canReelOffLeftHook)
                    {
                        float num17 = currentSpeed + 0.1f;
                        AddRightForce();
                        Vector3 vector15 = hookLeft.transform.position - transform.position;
                        reelForce = Mathf.Clamp(reelForce, -0.8f, 0.8f);

                        float num19 = 1f + reelForce;
                        Vector3 vector16 = Vector3.RotateTowards(vector15, Rigidbody.velocity, 1.53938f * num19, 1.53938f * num19);
                        vector16.Normalize();
                        spinning = true;
                        Rigidbody.velocity = (vector16 * num17);
                    }
                    else if (canReelOffRightHook)
                    {
                        float num20 = currentSpeed + 0.1f;
                        AddRightForce();
                        Vector3 vector17 = hookRight.transform.position - transform.position;
                        reelForce = Mathf.Clamp(reelForce, -0.8f, 0.8f);

                        float num22 = 1f + reelForce;
                        Vector3 vector18 = Vector3.RotateTowards(vector17, Rigidbody.velocity, 1.53938f * num22, 1.53938f * num22);
                        vector18.Normalize();
                        spinning = true;
                        Rigidbody.velocity = (vector18 * num20);

                    }
                    bool flag7 = false;
                    if ((hookLeft != null) || (hookRight != null))
                    {
                        if (((hookLeft != null) && (hookLeft.transform.position.y > gameObject.transform.position.y)) && (isLaunchLeft && hookLeft.isHooked()))
                        {
                            flag7 = true;
                        }
                        if (((hookRight != null) && (hookRight.transform.position.y > gameObject.transform.position.y)) && (isLaunchRight && hookRight.isHooked()))
                        {
                            flag7 = true;
                        }
                    }
                    if (flag7)
                    {
                        Rigidbody.AddForce(new Vector3(0f, -10f * Rigidbody.mass, 0f));
                    }
                    else
                    {
                        Rigidbody.AddForce(new Vector3(0f, -Gravity * Rigidbody.mass, 0f));
                    }

                    if (currentSpeed > 10f)
                    {
                        currentCamera.fieldOfView = Mathf.Lerp(currentCamera.fieldOfView, Mathf.Min((float) 100f, (float) (currentSpeed + 40f)), 0.1f);
                    }
                    else
                    {
                        currentCamera.fieldOfView = Mathf.Lerp(currentCamera.fieldOfView, 50f, 0.1f);
                    }
                    if (canUseGas)
                    {
                        UseGas(useGasSpeed * Time.deltaTime);
                        if (!smoke_3dmg_em.enabled && photonView.isMine)
                        {
                            object[] parameters = new object[] { true };
                            photonView.RPC(nameof(Net3DMGSMOKE), PhotonTargets.Others, parameters);
                        }
                        smoke_3dmg_em.enabled = true;
                    }
                    else
                    {
                        if (smoke_3dmg_em.enabled && photonView.isMine)
                        {
                            object[] objArray3 = new object[] { false };
                            photonView.RPC(nameof(Net3DMGSMOKE), PhotonTargets.Others, objArray3);
                        }
                        smoke_3dmg_em.enabled = false;
                    }
                }
            }
        }


        #endregion

        public void Initialize(CharacterPreset preset)
        {
            //TODO: Remove hack
            var manager = GetComponent<CustomizationManager>();
            if (preset == null)
            {
                preset = manager.Presets.First();
            }

            preset.Apply(this, manager.Prefabs);
            Skill = Skill.Create(preset.CurrentBuild.Skill, this);

            EquipmentType = preset.CurrentBuild.Equipment;
            Equipment.Initialize();

            if (EquipmentType == EquipmentType.Ahss)
            {
                standAnimation = HeroAnim.AHSS_STAND_GUN;
                useGun = true;
                gunDummy = new GameObject();
                gunDummy.name = "gunDummy";
                gunDummy.transform.position = transform.position;
                gunDummy.transform.rotation = transform.rotation;
            }

            if (photonView.isMine)
            {
                //TODO: If this is a default preset, find a more efficient way
                var config = JsonConvert.SerializeObject(CustomizationNetworkObject.Convert(Prefabs, preset), Formatting.Indented, new ColorJsonConverter());
                photonView.RPC(nameof(InitializeRpc), PhotonTargets.OthersBuffered, config);
            }

            /*int index = EquipmentType == EquipmentType.Ahss ? 1 : 0;              
            acl = preset.CharacterBuild[index].Stats.Acceleration;*/                //<-once correct character presets are implemented, uncomment this value assignation
            acl = 150f;                                                             //<-and delete this one, but leave the formula below intact
            Rigidbody.mass = 0.5f - (acl - 100f) * 0.001f;      
            /*I was asked by antigasp to use 0.45 (corresponding to ACL 150) as a placeholder because most testers are used to playing as Levi and it'd be
            easier for them to spot if something is wrong. Obviously this is going to have to be reworked once character-speficic stats are implemented,
            but for now it would probably make life easier for the testers.*/

            EntityService.Register(this);
        }

        [PunRPC]
        public void InitializeRpc(string characterPreset, PhotonMessageInfo info)
        {
            if (photonView.isMine)
            {
                //TODO: Handle Abusive RPC
                return;
            }

            if (info.sender.ID == photonView.ownerId)
            {
                var config = JsonConvert.DeserializeObject<CustomizationNetworkObject>(characterPreset, new ColorJsonConverter());
                Initialize(config.ToPreset(Prefabs));
            }
        }

        public override void OnHit(Entity attacker, int damage)
        {
            //TODO: 160 HERO OnHit logic
            //if (!isInvincible() && _state != HERO_STATE.Grab)
            //    markDie();
        }

        #region Animation

        private void SetAnimationSpeed(string animationName, float animationSpeed = 1f)
        {
            Animation[animationName].speed = animationSpeed;
            if (!photonView.isMine) return;

            photonView.RPC(nameof(SetAnimationSpeedRpc), PhotonTargets.Others, animationName, animationSpeed);
        }

        [PunRPC]
        private void SetAnimationSpeedRpc(string animationName, float animationSpeed, PhotonMessageInfo info)
        {
            if (info.sender.ID == photonView.owner.ID)
            {
                Animation[animationName].speed = animationSpeed;
            }
        }

        public void CrossFade(string newAnimation, float fadeLength = 0.1f)
        {
            if (string.IsNullOrWhiteSpace(newAnimation)) return;
            if (Animation.IsPlaying(newAnimation)) return;
            if (!photonView.isMine) return;

            CurrentAnimation = newAnimation;
            Animation.CrossFade(newAnimation, fadeLength);
            photonView.RPC(nameof(CrossFadeRpc), PhotonTargets.Others, newAnimation, fadeLength);
        }

        [PunRPC]
        protected void CrossFadeRpc(string newAnimation, float fadeLength, PhotonMessageInfo info)
        {
            if (info.sender.ID == photonView.owner.ID)
            {
                CurrentAnimation = newAnimation;
                Animation.CrossFade(newAnimation, fadeLength);
            }
        }

        public void TryCrossFade(string animationName, float time)
        {
            if (!Animation.IsPlaying(animationName))
            {
                CrossFade(animationName, time);
            }
        }

        private void CustomAnimationSpeed()
        {
            Animation[HeroAnim.ATTACK5].speed = 1.85f;
            Animation[HeroAnim.CHANGE_BLADE].speed = 1.2f;
            Animation[HeroAnim.AIR_RELEASE].speed = 0.6f;
            Animation[HeroAnim.CHANGE_BLADE_AIR].speed = 0.8f;
            Animation[HeroAnim.AHSS_GUN_RELOAD_BOTH].speed = 0.38f;
            Animation[HeroAnim.AHSS_GUN_RELOAD_BOTH_AIR].speed = 0.5f;
            Animation[HeroAnim.AHSS_GUN_RELOAD_L].speed = 0.4f;
            Animation[HeroAnim.AHSS_GUN_RELOAD_L_AIR].speed = 0.5f;
            Animation[HeroAnim.AHSS_GUN_RELOAD_R].speed = 0.4f;
            Animation[HeroAnim.AHSS_GUN_RELOAD_R_AIR].speed = 0.5f;
        }

        [PunRPC]
        public void NetPlayAnimation(string aniName)
        {
            CurrentAnimation = aniName;
            if (Animation != null)
            {
                Animation.Play(aniName);
            }
        }

        [PunRPC]
        private void NetPlayAnimationAt(string aniName, float normalizedTime)
        {
            CurrentAnimation = aniName;
            if (Animation != null)
            {
                Animation.Play(aniName);
                Animation[aniName].normalizedTime = normalizedTime;
            }
        }

        public void PlayAnimation(string aniName)
        {
            CurrentAnimation = aniName;
            Animation.Play(aniName);
            if (PhotonNetwork.connected && photonView.isMine)
            {
                object[] parameters = new object[] { aniName };
                photonView.RPC(nameof(NetPlayAnimation), PhotonTargets.Others, parameters);
            }
        }

        private void PlayAnimationAt(string aniName, float normalizedTime)
        {
            CurrentAnimation = aniName;
            Animation.Play(aniName);
            Animation[aniName].normalizedTime = normalizedTime;
            if (PhotonNetwork.connected && photonView.isMine)
            {
                object[] parameters = new object[] { aniName, normalizedTime };
                photonView.RPC(nameof(NetPlayAnimationAt), PhotonTargets.Others, parameters);
            }
        }

        #endregion

        public void AttackAccordingToMouse()
        {
            if (Input.mousePosition.x < (Screen.width * 0.5))
            {
                attackAnimation = HeroAnim.ATTACK2;
            }
            else
            {
                attackAnimation = HeroAnim.ATTACK1;
            }
        }

        public void AttackAccordingToTarget(Transform a)
        {
            Vector3 vector = a.position - transform.position;
            float current = -Mathf.Atan2(vector.z, vector.x) * Mathf.Rad2Deg;
            float f = -Mathf.DeltaAngle(current, transform.rotation.eulerAngles.y - 90f);
            if (((Mathf.Abs(f) < 90f) && (vector.magnitude < 6f)) && ((a.position.y <= (transform.position.y + 2f)) && (a.position.y >= (transform.position.y - 5f))))
            {
                attackAnimation = HeroAnim.ATTACK4;
            }
            else if (f > 0f)
            {
                attackAnimation = HeroAnim.ATTACK1;
            }
            else
            {
                attackAnimation = HeroAnim.ATTACK2;
            }
        }

        public void BackToHuman()
        {
            SmoothSync.disabled = false;
            Rigidbody.velocity = Vector3.zero;
            titanForm = false;
            Ungrabbed();
            FalseAttack();
            skillCDDuration = skillCDLast;
            currentInGameCamera.SetMainObject(gameObject, true, false);
            photonView.RPC(nameof(BackToHumanRPC), PhotonTargets.Others, new object[0]);
        }

        [PunRPC]
        private void BackToHumanRPC()
        {
            titanForm = false;
            eren_titan = null;
            SmoothSync.disabled = false;
        }

        [PunRPC]
        public void BadGuyReleaseMe()
        {
            hookBySomeOne = false;
            badGuy = null;
        }

        [PunRPC]
        public void BlowAway(Vector3 force)
        {
            if (photonView.isMine)
            {
                Rigidbody.AddForce(force, ForceMode.Impulse);
                transform.LookAt(transform.position);
            }
        }

        private void BodyLean()
        {
            if (photonView.isMine)
            {
                float z = 0f;
                needLean = false;
                if ((!useGun && (state == HumanState.Attack)) && ((attackAnimation != HeroAnim.ATTACK3_1) && (attackAnimation != HeroAnim.ATTACK3_2)))
                {
                    float y = Rigidbody.velocity.y;
                    float x = Rigidbody.velocity.x;
                    float num4 = Rigidbody.velocity.z;
                    float num5 = Mathf.Sqrt((x * x) + (num4 * num4));
                    float num6 = Mathf.Atan2(y, num5) * Mathf.Rad2Deg;
                    targetRotation = Quaternion.Euler(-num6 * (1f - (Vector3.Angle(Rigidbody.velocity, transform.forward) / 90f)), facingDirection, 0f);
                    if ((isLeftHandHooked && (hookLeft != null)) || (isRightHandHooked && (hookRight != null)))
                    {
                        transform.rotation = targetRotation;
                    }
                }
                else
                {
                    if ((isLeftHandHooked && (hookLeft != null)) && (isRightHandHooked && (hookRight != null)))
                    {
                        if (almostSingleHook)
                        {
                            needLean = true;
                            z = GetLeanAngle(hookRight.transform.position, true);
                        }
                    }
                    else if (isLeftHandHooked && (hookLeft != null))
                    {
                        needLean = true;
                        z = GetLeanAngle(hookLeft.transform.position, true);
                    }
                    else if (isRightHandHooked && (hookRight != null))
                    {
                        needLean = true;
                        z = GetLeanAngle(hookRight.transform.position, false);
                    }
                    if (needLean)
                    {
                        float a = 0f;
                        if (!useGun && (state != HumanState.Attack))
                        {
                            a = currentSpeed * 0.1f;
                            a = Mathf.Min(a, 20f);
                        }
                        targetRotation = Quaternion.Euler(-a, facingDirection, z);
                    }
                    else if (state != HumanState.Attack)
                    {
                        targetRotation = Quaternion.Euler(0f, facingDirection, 0f);
                    }
                }
            }
        }

        public void BombInit()
        {
            //skillIDHUD = skillId.ToString();
            //skillCDDuration = skillCDLast;
            //if (GameSettings.PvP.Bomb == true)
            //{
            //    int num = (int) FengGameManagerMKII.settings[250];
            //    int num2 = (int) FengGameManagerMKII.settings[251];
            //    int num3 = (int) FengGameManagerMKII.settings[252];
            //    int num4 = (int) FengGameManagerMKII.settings[253];
            //    if ((num < 0) || (num > 10))
            //    {
            //        num = 5;
            //        FengGameManagerMKII.settings[250] = 5;
            //    }
            //    if ((num2 < 0) || (num2 > 10))
            //    {
            //        num2 = 5;
            //        FengGameManagerMKII.settings[0xfb] = 5;
            //    }
            //    if ((num3 < 0) || (num3 > 10))
            //    {
            //        num3 = 5;
            //        FengGameManagerMKII.settings[0xfc] = 5;
            //    }
            //    if ((num4 < 0) || (num4 > 10))
            //    {
            //        num4 = 5;
            //        FengGameManagerMKII.settings[0xfd] = 5;
            //    }
            //    if ((((num + num2) + num3) + num4) > 20)
            //    {
            //        num = 5;
            //        num2 = 5;
            //        num3 = 5;
            //        num4 = 5;
            //        FengGameManagerMKII.settings[250] = 5;
            //        FengGameManagerMKII.settings[0xfb] = 5;
            //        FengGameManagerMKII.settings[0xfc] = 5;
            //        FengGameManagerMKII.settings[0xfd] = 5;
            //    }
            //    bombTimeMax = ((num2 * 60f) + 200f) / ((num3 * 60f) + 200f);
            //    bombRadius = (num * 4f) + 20f;
            //    bombCD = (num4 * -0.4f) + 5f;
            //    bombSpeed = (num3 * 60f) + 200f;
            //    PhotonHash propertiesToSet = new PhotonHash();
            //    propertiesToSet.Add(PhotonPlayerProperty.RCBombR, (float) FengGameManagerMKII.settings[0xf6]);
            //    propertiesToSet.Add(PhotonPlayerProperty.RCBombG, (float) FengGameManagerMKII.settings[0xf7]);
            //    propertiesToSet.Add(PhotonPlayerProperty.RCBombB, (float) FengGameManagerMKII.settings[0xf8]);
            //    propertiesToSet.Add(PhotonPlayerProperty.RCBombA, (float) FengGameManagerMKII.settings[0xf9]);
            //    propertiesToSet.Add(PhotonPlayerProperty.RCBombRadius, bombRadius);
            //    PhotonNetwork.player.SetCustomProperties(propertiesToSet);
            //    skillId = SkillId.bomb;
            //    skillIDHUD = SkillId.armin.ToString();
            //    skillCDLast = bombCD;
            //    skillCDDuration = 10f;
            //    if (Service.Time.GetRoundTime() > 10f)
            //    {
            //        skillCDDuration = 5f;
            //    }
            //}
        }

        private void BreakApart(Vector3 v, bool isBite)
        {
            //TODO: Implement Character Break Apart with the characters materials
            return;
        }

        private void BufferUpdate()
        {
            if (buffTime > 0f)
            {
                buffTime -= Time.deltaTime;
                if (buffTime <= 0f)
                {
                    buffTime = 0f;
                    if ((currentBuff == BUFF.SpeedUp) && Animation.IsPlaying(HeroAnim.RUN_SASHA))
                    {
                        CrossFade(HeroAnim.RUN_1, 0.1f);
                    }
                    currentBuff = BUFF.NoBuff;
                }
            }
        }

        public void Cache()
        {
            maincamera = GameObject.Find("MainCamera");
            if (photonView.isMine)
            {
                hookUI.Find();
                cachedSprites = new Dictionary<string, Image>();
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

        public void ChangeBlade()
        {
            if ((!useGun || grounded) || GameSettings.PvP.AhssAirReload.Value)
            {
                state = HumanState.ChangeBlade;
                throwedBlades = false;
                Equipment.Weapon.PlayReloadAnimation();
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

            LayerMask mask = Layers.Ground.ToLayer() | Layers.EnemyBox.ToLayer() | Layers.PlayerAttackBox.ToLayer();

            RaycastHit[] hitArray = Physics.RaycastAll(ray, 180f, mask.value);
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
                if (gameObject.layer == 0x10)
                {
                    if (gameObject.name.Contains("PlayerCollisionDetection") && ((hit2 = list[count]).distance < num2))
                    {
                        num2 -= 60f;
                        if (num2 <= 60f)
                        {
                            count = list.Count;
                        }
                        MindlessTitan component = gameObject.GetComponentInParent<MindlessTitan>();
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

        private void Dash(float horizontal, float vertical)
        {
            if (((dashTime <= 0f) && (currentGas > 0f)) && !isMounted && (burstCD.ElapsedMilliseconds <= BurstCDmin || burstCD.ElapsedMilliseconds >= BurstCDmax))
            {
                burstCD.Reset();
                UseGas(totalGas * 0.04f);
                facingDirection = GetGlobalFacingDirection(horizontal, vertical);
                dashV = GetGlobaleFacingVector3(facingDirection);
                originVM = currentSpeed;
                Quaternion quaternion = Quaternion.Euler(0f, facingDirection, 0f);
                Rigidbody.rotation = quaternion;
                targetRotation = quaternion;
                PhotonNetwork.Instantiate("FX/boost_smoke", transform.position, transform.rotation, 0);
                dashTime = 0.5f;
                CrossFade(HeroAnim.DASH, 0.1f);
                Animation[HeroAnim.DASH].time = 0.1f;
                state = HumanState.AirDodge;
                FalseAttack();
                Rigidbody.AddForce((dashV * 40f), ForceMode.VelocityChange);
                burstCD.Start();
            }
        }

        public void Die(Vector3 v, bool isBite)
        {
            if (invincible <= 0f)
            {
                Service.Music.SetMusicState(new MusicStateChangedEvent(MusicState.HumanPlayerDead));
                if (titanForm && (eren_titan != null))
                {
                    eren_titan.lifeTime = 0.1f;
                }
                if (hookLeft != null)
                {
                    hookLeft.removeMe();
                }
                if (hookRight != null)
                {
                    hookRight.removeMe();
                }
                meatDie.Play();
                if ((photonView.isMine) && !useGun)
                {
                    rightweapontrail.enabled = false;
                    leftweapontrail.enabled = false;

                }
                BreakApart(v, isBite);
                currentInGameCamera.gameOver = true;
                FalseAttack();
                hasDied = true;
                Transform audioDie = transform.Find("audio_die");
                audioDie.parent = null;
                audioDie.GetComponent<AudioSource>().Play();

                var propertiesToSet = new PhotonHash();
                propertiesToSet.Add(PhotonPlayerProperty.deaths, (int) PhotonNetwork.player.CustomProperties[PhotonPlayerProperty.deaths] + 1);
                photonView.owner.SetCustomProperties(propertiesToSet);

                if (PlayerPrefs.HasKey("EnableSS") && (PlayerPrefs.GetInt("EnableSS") == 1))
                {
                    currentInGameCamera.StartSnapShot2(audioDie.position, 0, null, 0.02f);
                }
                UnityEngine.Object.Destroy(gameObject);
            }
        }

        private void Dodge(bool offTheWall = false)
        {
            if (((!InputManager.Key(InputHorse.Mount) || !myHorse) || isMounted) || (Vector3.Distance(myHorse.transform.position, transform.position) >= 15f))
            {
                state = HumanState.GroundDodge;
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
                    CrossFade(HeroAnim.DODGE, 0.1f);
                }
                else
                {
                    PlayAnimation(HeroAnim.DODGE);
                    PlayAnimationAt(HeroAnim.DODGE, 0.2f);
                }
                sparks_em.enabled = false;
            }
        }

        private void ErenTransform()
        {
            skillCDDuration = skillCDLast;
            if (hookLeft != null)
            {
                hookLeft.removeMe();
            }
            if (hookRight != null)
            {
                hookRight.removeMe();
            }
            eren_titan = PhotonNetwork.Instantiate("ErenTitan", transform.position, transform.rotation, 0).GetComponent<ErenTitan>();
            eren_titan.realBody = gameObject;

            currentInGameCamera.FlashBlind();
            currentInGameCamera.SetMainObject(eren_titan.gameObject, true, false);
            eren_titan.born();
            eren_titan.Rigidbody.velocity = Rigidbody.velocity;
            Rigidbody.velocity = Vector3.zero;
            transform.position = eren_titan.Body.Neck.position;
            titanForm = true;
            object[] parameters = new object[] { eren_titan.gameObject.GetPhotonView().viewID };
            photonView.RPC(nameof(WhoIsMyErenTitan), PhotonTargets.Others, parameters);
            if ((smoke_3dmg_em.enabled && photonView.isMine))
            {
                object[] objArray2 = new object[] { false };
                photonView.RPC(nameof(Net3DMGSMOKE), PhotonTargets.Others, objArray2);
            }
            smoke_3dmg_em.enabled = false;
        }

        private void activeBoxes(bool val)
        {
            checkBoxLeft.IsActive = val;
            checkBoxRight.IsActive = val;
            val &= UseWeaponTrail;
            rightweapontrail.enabled = val;
            leftweapontrail.enabled = val;
        }

        public void FalseAttack()
        {
            if (useGun)
            {
                if (!attackReleased)
                {
                    SetAnimationSpeed(CurrentAnimation);
                    attackReleased = true;
                }
            }
            else
            {
                if (photonView.isMine)
                {
                    this.activeBoxes(false);
                    checkBoxLeft.ClearHits();
                    checkBoxRight.ClearHits();
                }
                attackLoop = 0;
                if (!attackReleased)
                {
                    SetAnimationSpeed(CurrentAnimation);
                    attackReleased = true;
                }
            }
        }

        public void FillGas()
        {
            currentGas = totalGas;
        }

        private GameObject FindNearestTitan()
        {
            GameObject[] objArray = GameObject.FindGameObjectsWithTag("titan");
            GameObject obj2 = null;
            float positiveInfinity = float.PositiveInfinity;
            Vector3 position = transform.position;
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

        [Obsolete("Does this do something?")]
        //Hotfix for Issue 97.
        private void AddRightForce()
        {
            //Whereas this may not be completely accurate to AoTTG, it is very close. Further balancing required in the future.
            Rigidbody.AddForce(Rigidbody.velocity * 0.00f, ForceMode.Acceleration);
        }


        private Vector3 GetGlobaleFacingVector3(float resultAngle)
        {
            float num = -resultAngle + 90f;
            float x = Mathf.Cos(num * Mathf.Deg2Rad);
            return new Vector3(x, 0f, Mathf.Sin(num * Mathf.Deg2Rad));
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

        private float GetLeanAngle(Vector3 p, bool left)
        {
            if (!useGun && (state == HumanState.Attack))
            {
                return 0f;
            }
            float num = p.y - transform.position.y;
            float num2 = Vector3.Distance(p, transform.position);
            float a = Mathf.Acos(num / num2) * Mathf.Rad2Deg;
            a *= 0.1f;
            a *= 1f + Mathf.Pow(Rigidbody.velocity.magnitude, 0.2f);
            Vector3 vector3 = p - transform.position;
            float current = Mathf.Atan2(vector3.x, vector3.z) * Mathf.Rad2Deg;
            float target = Mathf.Atan2(Rigidbody.velocity.x, Rigidbody.velocity.z) * Mathf.Rad2Deg;
            float num6 = Mathf.DeltaAngle(current, target);
            a += Mathf.Abs((float) (num6 * 0.5f));
            if (state != HumanState.Attack)
            {
                a = Mathf.Min(a, 80f);
            }
            if (num6 > 0f)
            {
                leanLeft = true;
            }
            else
            {
                leanLeft = false;
            }
            if (useGun)
            {
                return (a * ((num6 >= 0f) ? ((float) 1) : ((float) (-1))));
            }
            float num7 = 0f;
            if ((left && (num6 < 0f)) || (!left && (num6 > 0f)))
            {
                num7 = 0.1f;
            }
            else
            {
                num7 = 0.5f;
            }
            return (a * ((num6 >= 0f) ? num7 : -num7));
        }

        private void GetOffHorse()
        {
            PlayAnimation(HeroAnim.HORSE_GET_OFF);
            Rigidbody.AddForce((((Vector3.up * 10f) - (transform.forward * 2f)) - (transform.right * 1f)), ForceMode.VelocityChange);
            Unmounted();
        }

        private void GetOnHorse()
        {
            PlayAnimation(HeroAnim.HORSE_GET_ON);
            facingDirection = myHorse.transform.rotation.eulerAngles.y;
            targetRotation = Quaternion.Euler(0f, facingDirection, 0f);
        }

        public void GetSupply()
        {
            if ((Animation.IsPlaying(standAnimation)
                 || Animation.IsPlaying(HeroAnim.RUN_1)
                 || Animation.IsPlaying(HeroAnim.RUN_SASHA))
                && (currentBladeSta != totalBladeSta || currentGas != totalGas || Equipment.Weapon.CanReload))
            {
                state = HumanState.FillGas;
                CrossFade(HeroAnim.SUPPLY, 0.1f);
            }
        }

        public void Grabbed(GameObject titan, bool leftHand)
        {
            if (isMounted)
            {
                Unmounted();
            }
            state = HumanState.Grab;
            GetComponent<CapsuleCollider>().isTrigger = true;
            FalseAttack();
            titanWhoGrabMe = titan;
            if (titanForm && (eren_titan != null))
            {
                eren_titan.lifeTime = 0.1f;
            }
            smoke_3dmg_em.enabled = false;
            sparks_em.enabled = false;
        }

        public bool HasDied()
        {
            return (hasDied || IsInvincible);
        }

        private void HeadMovement()
        {
            Transform neck = Body.neck;
            Transform head = Body.head;
            float x = Mathf.Sqrt(((gunTarget.x - head.position.x) * (gunTarget.x - head.position.x)) + ((gunTarget.z - head.position.z) * (gunTarget.z - head.position.z)));
            targetHeadRotation = head.rotation;
            Vector3 vector5 = gunTarget - head.position;
            float current = -Mathf.Atan2(vector5.z, vector5.x) * Mathf.Rad2Deg;
            float num3 = -Mathf.DeltaAngle(current, head.rotation.eulerAngles.y - 90f);
            num3 = Mathf.Clamp(num3, -40f, 40f);
            float y = neck.position.y - gunTarget.y;
            float num5 = Mathf.Atan2(y, x) * Mathf.Rad2Deg;
            num5 = Mathf.Clamp(num5, -40f, 30f);
            targetHeadRotation = Quaternion.Euler(head.rotation.eulerAngles.x + num5, head.rotation.eulerAngles.y + num3, head.rotation.eulerAngles.z);
            oldHeadRotation = Quaternion.Lerp(oldHeadRotation, targetHeadRotation, Time.deltaTime * 60f);
            head.rotation = oldHeadRotation;
        }

        public void HookedByHuman(int hooker, Vector3 hookPosition)
        {
            object[] parameters = new object[] { hooker, hookPosition };
            photonView.RPC(nameof(RPCHookedByHuman), photonView.owner, parameters);
        }

        [PunRPC]
        public void HookFail()
        {
            hookTarget = null;
            hookSomeOne = false;
        }

        public void HookToHuman(GameObject target, Vector3 hookPosition)
        {
            ReleaseIfIHookSb();
            hookTarget = target;
            hookSomeOne = true;
            if (target.GetComponent<Hero>() != null)
            {
                target.GetComponent<Hero>().HookedByHuman(photonView.viewID, hookPosition);
            }
            launchForce = hookPosition - transform.position;
            float num = Mathf.Pow(launchForce.magnitude, 0.1f);
            if (grounded)
            {
                Rigidbody.AddForce((Vector3.up * Mathf.Min((float) (launchForce.magnitude * 0.2f), (float) 10f)), ForceMode.Impulse);
            }
            Rigidbody.AddForce(((launchForce * num) * 0.1f), ForceMode.Impulse);
        }

        private void Idle()
        {
            if (state == HumanState.Attack)
            {
                FalseAttack();
            }
            state = HumanState.Idle;
            CrossFade(standAnimation, 0.1f);
        }

        private bool IsFrontGrounded()
        {
            LayerMask mask = Layers.Ground.ToLayer() | Layers.EnemyBox.ToLayer();

            return Physics.Raycast(gameObject.transform.position + ((gameObject.transform.up * 1f)), gameObject.transform.forward, (float) 1f, mask.value);
        }

        public bool IsGrounded()
        {
            LayerMask mask = Layers.Ground.ToLayer() | Layers.EnemyBox.ToLayer();
            RaycastHit hit; //DONT DELETE THE OUT HIT FROM RAYCAST. IT BREAKS UTGARD CASTLE AND OTHER CONCAVE MESH COLLIDERS
            bool didHit = Physics.Raycast(gameObject.transform.position + (Vector3.up * 0.1f), -Vector3.up, out hit, 0.3f, mask.value);
            return didHit;
        }


        private bool IsPressDirectionTowardsHero(float h, float v)
        {
            if ((h == 0f) && (v == 0f))
            {
                return false;
            }
            return (Mathf.Abs(Mathf.DeltaAngle(GetGlobalFacingDirection(h, v), transform.rotation.eulerAngles.y)) < 45f);
        }

        private bool IsUpFrontGrounded()
        {
            LayerMask mask = Layers.Ground.ToLayer() | Layers.EnemyBox.ToLayer();

            return Physics.Raycast(gameObject.transform.position + ((gameObject.transform.up * 3f)), gameObject.transform.forward, (float) 1.2f, mask.value);
        }

        public void Launch(Vector3 des, bool left = true, bool leviMode = false)
        {
            if (isMounted)
            {
                Unmounted();
            }
            if (state != HumanState.Attack)
            {
                Idle();
            }
            Vector3 vector = des - transform.position;
            if (left)
            {
                launchPointLeft = des;
            }
            else
            {
                launchPointRight = des;
            }
            vector.Normalize();
            vector = (vector * 20f);
            if (((hookLeft != null) && (hookRight != null)) && (hookLeft.isHooked() && hookRight.isHooked()))
            {
                vector = (vector * 0.8f);
            }
            if (!Animation.IsPlaying(HeroAnim.ATTACK5) && !Animation.IsPlaying(HeroAnim.SPECIAL_PETRA))
            {
                leviMode = false;
            }
            else
            {
                leviMode = true;
            }
            if (!leviMode)
            {
                FalseAttack();
                Idle();
                if (useGun)
                {
                    CrossFade(HeroAnim.AHSS_HOOK_FORWARD_BOTH, 0.1f);
                }
                else if (left && !isRightHandHooked)
                {
                    CrossFade(HeroAnim.AIR_HOOK_L_JUST, 0.1f);
                }
                else if (!left && !isLeftHandHooked)
                {
                    CrossFade(HeroAnim.AIR_HOOK_R_JUST, 0.1f);
                }
                else
                {
                    CrossFade(HeroAnim.DASH, 0.1f);
                    Animation[HeroAnim.DASH].time = 0f;
                }
            }
            if (left)
            {
                isLaunchLeft = true;
            }
            if (!left)
            {
                isLaunchRight = true;
            }
            launchForce = vector;
            if (!leviMode)
            {
                if (vector.y < 30f)
                {
                    launchForce += (Vector3.up * (30f - vector.y));
                }
                if (des.y >= transform.position.y)
                {
                    launchForce += ((Vector3.up * (des.y - transform.position.y)) * 10f);
                }
                Rigidbody.AddForce(launchForce);
            }
            facingDirection = Mathf.Atan2(launchForce.x, launchForce.z) * Mathf.Rad2Deg;
            Quaternion quaternion = Quaternion.Euler(0f, facingDirection, 0f);
            gameObject.transform.rotation = quaternion;
            Rigidbody.rotation = quaternion;
            targetRotation = quaternion;
            if (left)
            {
                launchElapsedTimeL = 0f;
            }
            else
            {
                launchElapsedTimeR = 0f;
            }
            if (leviMode)
            {
                launchElapsedTimeR = -100f;
            }
            if (Animation.IsPlaying(HeroAnim.SPECIAL_PETRA))
            {
                launchElapsedTimeR = -100f;
                launchElapsedTimeL = -100f;
                if (hookRight != null)
                {
                    hookRight.disable();
                    ReleaseIfIHookSb();
                }
                if (hookLeft != null)
                {
                    hookLeft.disable();
                    ReleaseIfIHookSb();
                }
            }
            sparks_em.enabled = false;
        }

        public void LaunchLeftRope(float distance, Vector3 point, bool single, int mode = 0)
        {
            if (currentGas != 0f)
            {
                UseGas(0f);
                hookLeft = PhotonNetwork.Instantiate("hook", transform.position, transform.rotation, 0).GetComponent<Bullet>();              

                GameObject obj2 = !useGun ? hookRefL1 : hookRefL2;
                string str = !useGun ? "hookRefL1" : "hookRefL2";
                hookLeft.transform.position = obj2.transform.position;
                float num = !single ? ((distance <= 50f) ? (distance * 0.05f) : (distance * 0.3f)) : 0f;
                Vector3 vector = (point - ((transform.right * num))) - hookLeft.transform.position;
                vector.Normalize();
                if (mode == 1)
                {
                    hookLeft.launch((vector * 3f), Rigidbody.velocity, str, true, gameObject, true);
                }
                else
                {
                    hookLeft.launch((vector * 3f), Rigidbody.velocity, str, true, gameObject, false);
                }
                launchPointLeft = Vector3.zero;
            }
        }

        public void LaunchRightRope(float distance, Vector3 point, bool single, int mode = 0)
        {
            if (currentGas != 0f)
            {
                UseGas(0f);
                hookRight = PhotonNetwork.Instantiate("hook", transform.position, transform.rotation, 0).GetComponent<Bullet>();

                GameObject obj2 = !useGun ? hookRefR1 : hookRefR2;
                string str = !useGun ? "hookRefR1" : "hookRefR2";
                hookRight.transform.position = obj2.transform.position;
                float num = !single ? ((distance <= 50f) ? (distance * 0.05f) : (distance * 0.3f)) : 0f;
                Vector3 vector = (point + ((transform.right * num))) - hookRight.transform.position;
                vector.Normalize();
                if (mode == 1)
                {
                    hookRight.launch((vector * 5f), Rigidbody.velocity, str, false, gameObject, true);
                }
                else
                {
                    hookRight.launch((vector * 3f), Rigidbody.velocity, str, false, gameObject, false);
                }
                launchPointRight = Vector3.zero;
            }
        }

        private void LeftArmAimTo(Vector3 target)
        {
            float y = target.x - upperarmL.transform.position.x;
            float num2 = target.y - upperarmL.transform.position.y;
            float x = target.z - upperarmL.transform.position.z;
            float num4 = Mathf.Sqrt((y * y) + (x * x));
            handL.localRotation = Quaternion.Euler(90f, 0f, 0f);
            forearmL.localRotation = Quaternion.Euler(-90f, 0f, 0f);
            upperarmL.rotation = Quaternion.Euler(0f, 90f + (Mathf.Atan2(y, x) * Mathf.Rad2Deg), -Mathf.Atan2(num2, num4) * Mathf.Rad2Deg);
        }

        public void MarkDie()
        {
            hasDied = true;
            state = HumanState.Die;
            Service.Music.SetMusicState(new MusicStateChangedEvent(MusicState.HumanPlayerDead));
        }

        [PunRPC]
        private void Net3DMGSMOKE(bool ifON)
        {
            if (particle_Smoke_3dmg != null)
            {
                smoke_3dmg_em.enabled = ifON;
            }
        }

        [PunRPC]
        public void NetDie(Vector3 v, bool isBite, int viewID = -1, string titanName = "", bool killByTitan = true, PhotonMessageInfo info = new PhotonMessageInfo())
        {
            if ((photonView.isMine && (GameSettings.Gamemode.GamemodeType != GamemodeType.TitanRush)))
            {
                if (FengGameManagerMKII.ignoreList.Contains(info.sender.ID))
                {
                    photonView.RPC(nameof(BackToHumanRPC), PhotonTargets.Others, new object[0]);
                    return;
                }
                if (!info.sender.isLocal && !info.sender.isMasterClient)
                {
                    if ((info.sender.CustomProperties[PhotonPlayerProperty.name] == null) || (info.sender.CustomProperties[PhotonPlayerProperty.isTitan] == null))
                    {
                        FengGameManagerMKII.instance.chatRoom.AddMessage("<color=#FFCC00>Unusual Kill from ID " + info.sender.ID.ToString() + "</color>");
                    }
                    else if (viewID < 0)
                    {
                        if (titanName == "")
                        {
                            FengGameManagerMKII.instance.chatRoom.AddMessage("<color=#FFCC00>Unusual Kill from ID " + info.sender.ID.ToString() + " (possibly valid).</color>");
                        }
                        else
                        {
                            FengGameManagerMKII.instance.chatRoom.AddMessage("<color=#FFCC00>Unusual Kill from ID " + info.sender.ID.ToString() + "</color>");
                        }
                    }
                    else if (PhotonView.Find(viewID) == null)
                    {
                        FengGameManagerMKII.instance.chatRoom.AddMessage("<color=#FFCC00>Unusual Kill from ID " + info.sender.ID.ToString() + "</color>");
                    }
                    else if (PhotonView.Find(viewID).owner.ID != info.sender.ID)
                    {
                        FengGameManagerMKII.instance.chatRoom.AddMessage("<color=#FFCC00>Unusual Kill from ID " + info.sender.ID.ToString() + "</color>");
                    }
                }
            }
            if (PhotonNetwork.isMasterClient)
            {
                int iD = photonView.owner.ID;
                if (FengGameManagerMKII.heroHash.ContainsKey(iD))
                {
                    FengGameManagerMKII.heroHash.Remove(iD);
                }
            }
            if (photonView.isMine)
            {
                Vector3 vector = (Vector3.up * 5000f);
                if (myBomb != null)
                {
                    myBomb.destroyMe();
                }
                if (myCannon != null)
                {
                    PhotonNetwork.Destroy(myCannon);
                }
                if (titanForm && (eren_titan != null))
                {
                    eren_titan.lifeTime = 0.1f;
                }
                if (skillCD != null)
                {
                    skillCD.transform.localPosition = vector;
                }
            }
            if (hookLeft != null)
            {
                hookLeft.removeMe();
            }
            if (hookRight != null)
            {
                hookRight.removeMe();
            }
            meatDie.Play();
            FalseAttack();
            BreakApart(v, isBite);
            if (photonView.isMine)
            {
                currentInGameCamera.SetSpectorMode(false);
                currentInGameCamera.gameOver = true;
                FengGameManagerMKII.instance.myRespawnTime = 0f;
            }
            hasDied = true;
            Transform audioDie = transform.Find("audio_die");
            if (audioDie != null)
            {
                audioDie.parent = null;
                audioDie.GetComponent<AudioSource>().Play();
            }
            SmoothSync.disabled = true;
            if (photonView.isMine)
            {
                PhotonNetwork.RemoveRPCs(photonView);
                PhotonHash propertiesToSet = new PhotonHash();
                propertiesToSet.Add(PhotonPlayerProperty.dead, true);
                PhotonNetwork.player.SetCustomProperties(propertiesToSet);
                propertiesToSet = new PhotonHash();
                propertiesToSet.Add(PhotonPlayerProperty.deaths, RCextensions.returnIntFromObject(PhotonNetwork.player.CustomProperties[PhotonPlayerProperty.deaths]) + 1);
                PhotonNetwork.player.SetCustomProperties(propertiesToSet);
                if (viewID != -1)
                {
                    PhotonView view2 = PhotonView.Find(viewID);
                    if (view2 != null)
                    {
                        FengGameManagerMKII.instance.sendKillInfo(killByTitan, $"[{info.sender.ID.ToString().Color("ffc000")}] {RCextensions.returnStringFromObject(view2.owner.CustomProperties[PhotonPlayerProperty.name])}", false, RCextensions.returnStringFromObject(PhotonNetwork.player.CustomProperties[PhotonPlayerProperty.name]), 0);
                        propertiesToSet = new PhotonHash();
                        propertiesToSet.Add(PhotonPlayerProperty.kills, RCextensions.returnIntFromObject(view2.owner.CustomProperties[PhotonPlayerProperty.kills]) + 1);
                        view2.owner.SetCustomProperties(propertiesToSet);
                    }
                }
                else
                {
                    FengGameManagerMKII.instance.sendKillInfo(!(titanName == string.Empty), $"[{info.sender.ID.ToString().Color("ffc000")}] {titanName}", false, RCextensions.returnStringFromObject(PhotonNetwork.player.CustomProperties[PhotonPlayerProperty.name]), 0);
                }
            }
            if (photonView.isMine)
            {
                PhotonNetwork.Destroy(photonView);
            }
        }

        [PunRPC]
        public void NetDie2(int viewID = -1, string titanName = "", PhotonMessageInfo info = new PhotonMessageInfo())
        {
            GameObject obj2;
            if ((photonView.isMine) && (GameSettings.Gamemode.GamemodeType != GamemodeType.TitanRush))
            {
                if (FengGameManagerMKII.ignoreList.Contains(info.sender.ID))
                {
                    photonView.RPC(nameof(BackToHumanRPC), PhotonTargets.Others, new object[0]);
                    return;
                }
                if (!info.sender.IsLocal && !info.sender.IsMasterClient)
                {
                    if ((info.sender.CustomProperties[PhotonPlayerProperty.name] == null) || (info.sender.CustomProperties[PhotonPlayerProperty.isTitan] == null))
                    {
                        FengGameManagerMKII.instance.chatRoom.AddMessage($"Unusual Kill from ID {info.sender.ID}".Color("FFCC00"));
                    }
                    else if (viewID < 0)
                    {
                        if (titanName == "")
                        {
                            FengGameManagerMKII.instance.chatRoom.AddMessage($"Unusual Kill from ID {info.sender.ID} (possibly valid).".Color("FFCC00"));
                        }
                        else if (GameSettings.PvP.Bomb.Value && (!GameSettings.PvP.Cannons.Value))
                        {
                            FengGameManagerMKII.instance.chatRoom.AddMessage($"Unusual Kill from ID {info.sender.ID}".Color("FFCC00"));
                        }
                    }
                    else if (PhotonView.Find(viewID) == null)
                    {
                        FengGameManagerMKII.instance.chatRoom.AddMessage($"Unusual Kill from ID {info.sender.ID}".Color("FFCC00"));
                    }
                    else if (PhotonView.Find(viewID).owner.ID != info.sender.ID)
                    {
                        FengGameManagerMKII.instance.chatRoom.AddMessage($"Unusual Kill from ID {info.sender.ID}".Color("FFCC00"));
                    }
                }
            }
            if (photonView.isMine)
            {
                Vector3 vector = (Vector3.up * 5000f);
                if (myBomb != null)
                {
                    myBomb.destroyMe();
                }
                if (myCannon != null)
                {
                    PhotonNetwork.Destroy(myCannon);
                }
                PhotonNetwork.RemoveRPCs(photonView);
                if (titanForm && (eren_titan != null))
                {
                    eren_titan.lifeTime = 0.1f;
                }
                if (skillCD != null)
                {
                    skillCD.transform.localPosition = vector;
                }
            }
            meatDie.Play();
            if (hookLeft != null)
            {
                hookLeft.removeMe();
            }
            if (hookRight != null)
            {
                hookRight.removeMe();
            }
            Transform audioDie = transform.Find("audio_die");
            audioDie.parent = null;
            audioDie.GetComponent<AudioSource>().Play();
            if (photonView.isMine)
            {
                currentInGameCamera.SetMainObject(null, true, false);
                currentInGameCamera.SetSpectorMode(true);
                currentInGameCamera.gameOver = true;
                FengGameManagerMKII.instance.myRespawnTime = 0f;
            }
            FalseAttack();
            hasDied = true;
            SmoothSync.disabled = true;
            if (photonView.isMine)
            {
                PhotonNetwork.RemoveRPCs(photonView);
                PhotonHash propertiesToSet = new PhotonHash();
                propertiesToSet.Add(PhotonPlayerProperty.dead, true);
                PhotonNetwork.player.SetCustomProperties(propertiesToSet);
                propertiesToSet = new PhotonHash();
                propertiesToSet.Add(PhotonPlayerProperty.deaths, ((int) PhotonNetwork.player.CustomProperties[PhotonPlayerProperty.deaths]) + 1);
                PhotonNetwork.player.SetCustomProperties(propertiesToSet);
                if (viewID != -1)
                {
                    PhotonView view2 = PhotonView.Find(viewID);
                    if (view2 != null)
                    {
                        FengGameManagerMKII.instance.sendKillInfo(true, $"{info.sender.ID.ToString().Color("ffc000")} {RCextensions.returnStringFromObject(view2.owner.CustomProperties[PhotonPlayerProperty.name])}", false, RCextensions.returnStringFromObject(PhotonNetwork.player.CustomProperties[PhotonPlayerProperty.name]), 0);
                        propertiesToSet = new PhotonHash();
                        propertiesToSet.Add(PhotonPlayerProperty.kills, RCextensions.returnIntFromObject(view2.owner.CustomProperties[PhotonPlayerProperty.kills]) + 1);
                        view2.owner.SetCustomProperties(propertiesToSet);
                    }
                }
                else
                {
                    FengGameManagerMKII.instance.sendKillInfo(true, $"{info.sender.ID.ToString().Color("ffc000")} {titanName}", false, RCextensions.returnStringFromObject(PhotonNetwork.player.CustomProperties[PhotonPlayerProperty.name]), 0);
                }
            }
            if (photonView.isMine)
            {
                obj2 = PhotonNetwork.Instantiate("hitMeat2", audioDie.position, Quaternion.Euler(270f, 0f, 0f), 0);
            }
            else
            {
                obj2 = Instantiate(Resources.Load<GameObject>("hitMeat2"));
            }
            obj2.transform.position = audioDie.position;
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

        public void NetDieLocal(Vector3 v, bool isBite, int viewID = -1, string titanName = "", bool killByTitan = true)
        {
            if (photonView.isMine)
            {
                Vector3 vector = (Vector3.up * 5000f);
                if (titanForm && (eren_titan != null))
                {
                    eren_titan.lifeTime = 0.1f;
                }
                if (myBomb)
                {
                    myBomb.destroyMe();
                }
                if (myCannon)
                {
                    PhotonNetwork.Destroy(myCannon);
                }
                if (skillCD)
                {
                    skillCD.transform.localPosition = vector;
                }
            }
            if (hookLeft)
            {
                hookLeft.removeMe();
            }
            if (hookRight)
            {
                hookRight.removeMe();
            }
            meatDie.Play();
            if (!(useGun || (!photonView.isMine)))
            {
                rightweapontrail.enabled = false;
                leftweapontrail.enabled = false;
            }
            FalseAttack();
            BreakApart(v, isBite);
            if (photonView.isMine)
            {
                currentInGameCamera.SetSpectorMode(false);
                currentInGameCamera.gameOver = true;
                FengGameManagerMKII.instance.myRespawnTime = 0f;
            }
            hasDied = true;
            Transform audioDie = transform.Find("audio_die");
            audioDie.parent = null;
            audioDie.GetComponent<AudioSource>().Play();
            SmoothSync.disabled = true;
            if (photonView.isMine)
            {
                PhotonNetwork.RemoveRPCs(photonView);
                PhotonHash propertiesToSet = new PhotonHash()
                {
                    { PhotonPlayerProperty.dead, true},
                    { PhotonPlayerProperty.deaths, PhotonNetwork.player.CustomProperties.SafeGet(PhotonPlayerProperty.deaths,0) + 1}
                };
                PhotonNetwork.player.SetCustomProperties(propertiesToSet);
                if (viewID != -1)
                {
                    PhotonView view = PhotonView.Find(viewID);
                    if (view != null)
                    {
                        FengGameManagerMKII.instance.sendKillInfo(killByTitan, RCextensions.returnStringFromObject(view.owner.CustomProperties[PhotonPlayerProperty.name]), false, RCextensions.returnStringFromObject(PhotonNetwork.player.CustomProperties[PhotonPlayerProperty.name]), 0);
                        view.owner.SetCustomProperties(new PhotonHash()
                        {
                            {PhotonPlayerProperty.kills,
                                view.owner.CustomProperties.SafeGet(PhotonPlayerProperty.kills,0)+1 }
                        });
                    }
                }
                else
                {
                    FengGameManagerMKII.instance.sendKillInfo(!(titanName == string.Empty), titanName, false, RCextensions.returnStringFromObject(PhotonNetwork.player.CustomProperties[PhotonPlayerProperty.name]), 0);
                }

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

        [PunRPC]
        public void NetGrabbed(int id, bool leftHand)
        {
            titanWhoGrabMeID = id;
            NetPlayAnimation("grabbed");
            Grabbed(PhotonView.Find(id).gameObject, leftHand);
        }

        [PunRPC]
        private void NetlaughAttack()
        {
            throw new NotImplementedException("Titan laugh attack is not implemented yet");
            //foreach (GameObject obj2 in GameObject.FindGameObjectsWithTag("titan"))
            //{
            //    if (((Vector3.Distance(obj2.transform.position, transform.position) < 50f) && (Vector3.Angle(obj2.transform.forward, transform.position - obj2.transform.position) < 90f)) && (obj2.GetComponent<TITAN>() != null))
            //    {
            //        obj2.GetComponent<TITAN>().beLaughAttacked();
            //    }
            //}
        }

        [PunRPC]
        private void NetSetIsGrabbedFalse()
        {
            state = HumanState.Idle;
        }

        [PunRPC]
        private void NetTauntAttack(float tauntTime, float distance = 100f)
        {
            throw new NotImplementedException("Titan taunt behavior is not yet implemented");
        }

        [PunRPC]
        public void NetUngrabbed()
        {
            Ungrabbed();
            NetPlayAnimation(standAnimation);
            FalseAttack();
        }

        public void ReleaseIfIHookSb()
        {
            if (hookSomeOne && (hookTarget != null))
            {
                hookTarget.GetPhotonView().RPC(nameof(BadGuyReleaseMe), hookTarget.GetPhotonView().owner, new object[0]);
                hookTarget = null;
                hookSomeOne = false;
            }
        }

        //Change with 113
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
            IEnumerator enumerator = Animation.GetEnumerator();
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

        [PunRPC]
        public void ReturnFromCannon(PhotonMessageInfo info)
        {
            if (info.sender == photonView.owner)
            {
                isCannon = false;
                SmoothSync.disabled = false;
            }
        }

        private void RightArmAimTo(Vector3 target)
        {
            float y = target.x - upperarmR.transform.position.x;
            float num2 = target.y - upperarmR.transform.position.y;
            float x = target.z - upperarmR.transform.position.z;
            float num4 = Mathf.Sqrt((y * y) + (x * x));
            handR.localRotation = Quaternion.Euler(-90f, 0f, 0f);
            forearmR.localRotation = Quaternion.Euler(90f, 0f, 0f);
            upperarmR.rotation = Quaternion.Euler(180f, 90f + (Mathf.Atan2(y, x) * Mathf.Rad2Deg), Mathf.Atan2(num2, num4) * Mathf.Rad2Deg);
        }

        [PunRPC]
        private void RPCHookedByHuman(int hooker, Vector3 hookPosition)
        {
            hookBySomeOne = true;
            badGuy = PhotonView.Find(hooker).gameObject;
            if (Vector3.Distance(hookPosition, transform.position) < 15f)
            {
                launchForce = PhotonView.Find(hooker).gameObject.transform.position - transform.position;
                Rigidbody.AddForce((-Rigidbody.velocity * 0.9f), ForceMode.VelocityChange);
                float num = Mathf.Pow(launchForce.magnitude, 0.1f);
                if (grounded)
                {
                    Rigidbody.AddForce((Vector3.up * Mathf.Min(launchForce.magnitude * 0.2f, 10f)), ForceMode.Impulse);
                }
                Rigidbody.AddForce(((launchForce * num) * 0.1f), ForceMode.Impulse);
                if (state != HumanState.Grab)
                {
                    dashTime = 1f;
                    CrossFade(HeroAnim.DASH, 0.05f);
                    Animation[HeroAnim.DASH].time = 0.1f;
                    state = HumanState.AirDodge;
                    FalseAttack();
                    facingDirection = Mathf.Atan2(launchForce.x, launchForce.z) * Mathf.Rad2Deg;
                    Quaternion quaternion = Quaternion.Euler(0f, facingDirection, 0f);
                    gameObject.transform.rotation = quaternion;
                    Rigidbody.rotation = quaternion;
                    targetRotation = quaternion;
                }
            }
            else
            {
                hookBySomeOne = false;
                badGuy = null;
                PhotonView.Find(hooker).RPC(nameof(HookFail), PhotonView.Find(hooker).owner, new object[0]);
            }
        }

        private void Salute()
        {
            state = HumanState.Salute;
            CrossFade(HeroAnim.SALUTE, 0.1f);
        }

        private void SetHookedPplDirection()
        {
            almostSingleHook = false;
            if (isRightHandHooked && isLeftHandHooked)
            {
                if ((hookLeft != null) && (hookRight != null))
                {
                    Vector3 normal = hookLeft.transform.position - hookRight.transform.position;
                    if (normal.sqrMagnitude < 4f)
                    {
                        Vector3 vector2 = (((hookLeft.transform.position + hookRight.transform.position) * 0.5f)) - transform.position;
                        facingDirection = Mathf.Atan2(vector2.x, vector2.z) * Mathf.Rad2Deg;
                        if (useGun && (state != HumanState.Attack))
                        {
                            float current = -Mathf.Atan2(Rigidbody.velocity.z, Rigidbody.velocity.x) * Mathf.Rad2Deg;
                            float target = -Mathf.Atan2(vector2.z, vector2.x) * Mathf.Rad2Deg;
                            float num3 = -Mathf.DeltaAngle(current, target);
                            facingDirection += num3;
                        }
                        almostSingleHook = true;
                    }
                    else
                    {
                        Vector3 to = transform.position - hookLeft.transform.position;
                        Vector3 vector6 = transform.position - hookRight.transform.position;
                        Vector3 vector7 = ((hookLeft.transform.position + hookRight.transform.position) * 0.5f);
                        Vector3 from = transform.position - vector7;
                        if ((Vector3.Angle(from, to) < 30f) && (Vector3.Angle(from, vector6) < 30f))
                        {
                            almostSingleHook = true;
                            Vector3 vector9 = vector7 - transform.position;
                            facingDirection = Mathf.Atan2(vector9.x, vector9.z) * Mathf.Rad2Deg;
                        }
                        else
                        {
                            almostSingleHook = false;
                            Vector3 forward = transform.forward;
                            Vector3.OrthoNormalize(ref normal, ref forward);
                            facingDirection = Mathf.Atan2(forward.x, forward.z) * Mathf.Rad2Deg;
                            float num4 = Mathf.Atan2(to.x, to.z) * Mathf.Rad2Deg;
                            if (Mathf.DeltaAngle(num4, facingDirection) > 0f)
                            {
                                facingDirection += 180f;
                            }
                        }
                    }
                }
            }
            else
            {
                almostSingleHook = true;
                Vector3 zero = Vector3.zero;
                if (isRightHandHooked && (hookRight != null))
                {
                    zero = hookRight.transform.position - transform.position;
                }
                else
                {
                    if (!isLeftHandHooked || (hookLeft == null))
                    {
                        return;
                    }
                    zero = hookLeft.transform.position - transform.position;
                }
                facingDirection = Mathf.Atan2(zero.x, zero.z) * Mathf.Rad2Deg;
                if (state != HumanState.Attack)
                {
                    float num6 = -Mathf.Atan2(Rigidbody.velocity.z, Rigidbody.velocity.x) * Mathf.Rad2Deg;
                    float num7 = -Mathf.Atan2(zero.z, zero.x) * Mathf.Rad2Deg;
                    float num8 = -Mathf.DeltaAngle(num6, num7);
                    if (useGun)
                    {
                        facingDirection += num8;
                    }
                    else
                    {
                        float num9 = 0f;
                        if ((isLeftHandHooked && (num8 < 0f)) || (isRightHandHooked && (num8 > 0f)))
                        {
                            num9 = -0.1f;
                        }
                        else
                        {
                            num9 = 0.1f;
                        }
                        facingDirection += num8 * num9;
                    }
                }
            }
        }

        [PunRPC]
        public void SetMyCannon(int viewID, PhotonMessageInfo info)
        {
            if (info.sender == photonView.owner)
            {
                PhotonView view = PhotonView.Find(viewID);
                if (view != null)
                {
                    myCannon = view.gameObject;
                    if (myCannon != null)
                    {
                        myCannonBase = myCannon.transform;
                        myCannonPlayer = myCannonBase.Find("PlayerPoint");
                        isCannon = true;
                    }
                }
            }
        }

        [PunRPC]
        public void SetMyPhotonCamera(float offset, PhotonMessageInfo info)
        {
            if (photonView.owner == info.sender)
            {
                CameraMultiplier = offset;
                SmoothSync.PhotonCamera = true;
                isPhotonCamera = true;
            }
        }

        [PunRPC]
        private void SetMyTeam(int val)
        {
            myTeam = val;
            checkBoxLeft.myTeam = val;
            checkBoxRight.myTeam = val;
            if (PhotonNetwork.isMasterClient)
            {
                object[] objArray;
                //TODO: Sync these upon gamemode syncSettings
                if (GameSettings.PvP.Mode == PvpMode.AhssVsBlades)
                {
                    int num = 0;
                    if (photonView.owner.CustomProperties[PhotonPlayerProperty.RCteam] != null)
                    {
                        num = RCextensions.returnIntFromObject(photonView.owner.CustomProperties[PhotonPlayerProperty.RCteam]);
                    }
                    if (val != num)
                    {
                        objArray = new object[] { num };
                        photonView.RPC(nameof(SetMyTeam), PhotonTargets.AllBuffered, objArray);
                    }
                }
                else if (GameSettings.PvP.Mode == PvpMode.FreeForAll && (val != photonView.owner.ID))
                {
                    objArray = new object[] { photonView.owner.ID };
                    photonView.RPC(nameof(SetMyTeam), PhotonTargets.AllBuffered, objArray);
                }
            }
        }

        public void SetTeam(int team)
        {
            if (photonView.isMine)
            {
                object[] parameters = new object[] { team };
                photonView.RPC(nameof(SetMyTeam), PhotonTargets.AllBuffered, parameters);
                PhotonHash propertiesToSet = new PhotonHash();
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
            var flare = Service.Inventory.GetItems<Items.Flare>()[type - 1];
            flare.Use(this);
        }

        private void ShowAimUI()
        {
            Vector3 vector;
            if (MenuManager.IsAnyMenuOpen)
            {
                hookUI.Disable();
            }
            else
            {
                hookUI.Enable();

                CheckTitan();
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                LayerMask mask = Layers.Ground.ToLayer() | Layers.EnemyBox.ToLayer();

                var distance = "???";
                var magnitude = HookRaycastDistance;
                var hitDistance = HookRaycastDistance;
                var hitPoint = ray.GetPoint(hitDistance);

                var mousePos = Input.mousePosition;
                hookUI.cross.position = mousePos;

                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, 1000f, mask.value))
                {
                    magnitude = (hit.point - transform.position).magnitude;
                    distance = ((int) magnitude).ToString();
                    hitDistance = hit.distance;
                    hitPoint = hit.point;
                }

                hookUI.crossImage.color = magnitude > 120f ? Color.red : Color.white;
                hookUI.distanceLabel.transform.localPosition = hookUI.cross.localPosition;
                hookUI.speedLabel.transform.localPosition = hookUI.cross.localPosition;

                if (((int) FengGameManagerMKII.settings[0xbd]) == 1)
                {
                    distance += "\n" + currentSpeed.ToString("F1") + " u/s";
                }
                else if (((int) FengGameManagerMKII.settings[0xbd]) == 2)
                {
                    distance += "\n" + ((currentSpeed / 100f)).ToString("F1") + "K";
                }
                hookUI.distanceLabel.text = distance;
                hookUI.speedLabel.text = ((currentSpeed / 100f)).ToString("F1") + "K";

                Vector3 vector2 = new Vector3(0f, 0.4f, 0f);
                vector2 -= (transform.right * 0.3f);
                Vector3 vector3 = new Vector3(0f, 0.4f, 0f);
                vector3 += (transform.right * 0.3f);
                float num4 = (hitDistance <= 50f) ? (hitDistance * 0.05f) : (hitDistance * 0.3f);
                Vector3 vector4 = (hitPoint - ((transform.right * num4))) - (transform.position + vector2);
                Vector3 vector5 = (hitPoint + ((transform.right * num4))) - (transform.position + vector3);
                vector4.Normalize();
                vector5.Normalize();
                vector4 = (vector4 * HookRaycastDistance);
                vector5 = (vector5 * HookRaycastDistance);
                RaycastHit hit2;
                hitPoint = (transform.position + vector2) + vector4;
                hitDistance = HookRaycastDistance;
                if (Physics.Linecast(transform.position + vector2, (transform.position + vector2) + vector4, out hit2, mask.value))
                {
                    hitPoint = hit2.point;
                    hitDistance = hit2.distance;
                }

                hookUI.crossL.transform.position = currentCamera.WorldToScreenPoint(hitPoint);
                hookUI.crossL.transform.localRotation = Quaternion.Euler(0f, 0f, (Mathf.Atan2(hookUI.crossL.transform.position.y - mousePos.y, hookUI.crossL.transform.position.x - mousePos.x) * Mathf.Rad2Deg) + 180f);
                hookUI.crossImageL.color = hitDistance > 120f ? Color.red : Color.white;

                hitPoint = (transform.position + vector3) + vector5;
                hitDistance = HookRaycastDistance;
                if (Physics.Linecast(transform.position + vector3, (transform.position + vector3) + vector5, out hit2, mask.value))
                {
                    hitPoint = hit2.point;
                    hitDistance = hit2.distance;
                }

                hookUI.crossR.transform.position = currentCamera.WorldToScreenPoint(hitPoint);
                hookUI.crossR.transform.localRotation = Quaternion.Euler(0f, 0f, Mathf.Atan2(hookUI.crossR.transform.position.y - mousePos.y, hookUI.crossR.transform.position.x - mousePos.x) * Mathf.Rad2Deg);
                hookUI.crossImageR.color = hitDistance > 120f ? Color.red : Color.white;
            }
        }

        private void ShowGas()
        {
            float num2 = currentBladeSta / totalBladeSta;
            cachedSprites["GasLeft"].fillAmount = cachedSprites["GasRight"].fillAmount = 1 - (currentGas / totalGas);
            Equipment.Weapon.UpdateSupplyUi(InGameUI);
        }

        private void ShowSkillCD()
        {
            if (skillCD != null)
            {
                //skillCD.GetComponent<UISprite>().fillAmount = (skillCDLast - skillCDDuration) / skillCDLast;
            }
        }

        [PunRPC]
        public void SpawnCannonRPC(string settings, PhotonMessageInfo info)
        {
            if (info.sender.isMasterClient && photonView.isMine && !myCannon)
            {
                if (myHorse && isMounted)
                    GetOffHorse();

                Idle();

                if (hookLeft)
                    hookLeft.removeMe();

                if (hookRight)
                    hookRight.removeMe();

                if (smoke_3dmg_em.enabled && photonView.isMine)
                {
                    object[] parameters = new object[] { false };
                    photonView.RPC(nameof(Net3DMGSMOKE), PhotonTargets.Others, parameters);
                }
                smoke_3dmg_em.enabled = false;
                Rigidbody.velocity = Vector3.zero;
                string[] strArray = settings.Split(new char[] { ',' });
                if (strArray.Length > 15)
                {
                    myCannon = PhotonNetwork.Instantiate("RCAsset/" + strArray[1], new Vector3(Convert.ToSingle(strArray[12]), Convert.ToSingle(strArray[13]), Convert.ToSingle(strArray[14])), new Quaternion(Convert.ToSingle(strArray[15]), Convert.ToSingle(strArray[0x10]), Convert.ToSingle(strArray[0x11]), Convert.ToSingle(strArray[0x12])), 0);
                }
                else
                {
                    myCannon = PhotonNetwork.Instantiate("RCAsset/" + strArray[1], new Vector3(Convert.ToSingle(strArray[2]), Convert.ToSingle(strArray[3]), Convert.ToSingle(strArray[4])), new Quaternion(Convert.ToSingle(strArray[5]), Convert.ToSingle(strArray[6]), Convert.ToSingle(strArray[7]), Convert.ToSingle(strArray[8])), 0);
                }
                myCannonBase = myCannon.transform;
                myCannonPlayer = myCannon.transform.Find("PlayerPoint");
                isCannon = true;
                myCannon.GetComponent<Cannon>().myHero = this;
                myCannonRegion = null;
                currentInGameCamera.SetMainObject(myCannon.transform.Find("Barrel").Find("FiringPoint").gameObject, true, false);
                currentCamera.fieldOfView = 55f;
                photonView.RPC(nameof(SetMyCannon), PhotonTargets.OthersBuffered, new object[] { myCannon.GetPhotonView().viewID });
                skillCDLastCannon = skillCDLast;
                skillCDLast = 3.5f;
                skillCDDuration = 3.5f;
            }
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

        public IEnumerator StopImmunity()
        {
            yield return new WaitForSeconds(5f);
            bombImmune = false;
        }

        private void Suicide()
        {
            NetDieLocal((Rigidbody.velocity * 50f), false, -1, string.Empty, true);
            FengGameManagerMKII.instance.needChooseSide = true;
        }

        public void Ungrabbed()
        {
            facingDirection = 0f;
            targetRotation = Quaternion.Euler(0f, 0f, 0f);
            transform.parent = null;
            GetComponent<CapsuleCollider>().isTrigger = false;
            state = HumanState.Idle;
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
            if (Skill is BombPvpSkill)
            {
                if (InputManager.KeyDown(InputHuman.AttackSpecial) && (skillCDDuration <= 0f))
                {
                    if (!((myBomb == null) || myBomb.disabled))
                    {
                        myBomb.Explode(bombRadius);
                    }
                    detonate = false;
                    skillCDDuration = bombCD;
                    RaycastHit hitInfo = new RaycastHit();
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    LayerMask mask = Layers.Ground.ToLayer() | Layers.EnemyBox.ToLayer();

                    currentV = transform.position;
                    targetV = currentV + ((Vector3.forward * 200f));
                    if (Physics.Raycast(ray, out hitInfo, 1000000f, mask.value))
                    {
                        targetV = hitInfo.point;
                    }
                    Vector3 vector = Vector3.Normalize(targetV - currentV);
                    GameObject obj2 = PhotonNetwork.Instantiate(bombMainPath, currentV + ((vector * 4f)), new Quaternion(0f, 0f, 0f, 1f), 0);
                    obj2.GetComponent<Rigidbody>().velocity = (vector * bombSpeed);
                    myBomb = obj2.GetComponent<Bomb>();
                    bombTime = 0f;
                }
                else if ((myBomb != null) && !myBomb.disabled)
                {
                    bombTime += Time.deltaTime;
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
                    if (bombTime >= bombTimeMax)
                    {
                        flag2 = true;
                    }
                    if (flag2)
                    {
                        myBomb.Explode(bombRadius);
                        detonate = false;
                    }
                }
            }
        }

        private void UseGas(float amount = 0)
        {
            if (amount == 0f)
            {
                amount = useGasSpeed;
            }
            if (currentGas > 0f)
            {
                currentGas -= amount;
                if (currentGas < 0f)
                {
                    currentGas = 0f;
                }
            }
        }

        [PunRPC]
        private void WhoIsMyErenTitan(int id)
        {
            eren_titan = PhotonView.Find(id).gameObject.GetComponent<ErenTitan>();
            titanForm = true;
        }

        private void OnTriggerEnter(Collider collision)
        {
            AddTimeToCombatTimer(collision);
        }

        private void OnTriggerStay(Collider collision)
        {
            AddTimeToCombatTimer(collision);
        }

        private void AddTimeToCombatTimer(Collider collider)
        {
            if (collider.CompareTag("SoundTrigger") && collider.transform.root.GetComponent<MindlessTitan>().State != TitanState.Dead)
            {
                CombatTimer.AddTime();
            }
        }
    }
}