using Assets.Scripts;
using Assets.Scripts.Characters;
using Assets.Scripts.Characters.Titan;
using Assets.Scripts.Gamemode.Options;
using Assets.Scripts.Services;
using Assets.Scripts.Settings;
using Assets.Scripts.UI.InGame.HUD;
using Assets.Scripts.UI.Input;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hero : Human
{
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
    public Animation baseAnimation;
    public Rigidbody baseRigidBody;
    public Transform baseTransform;
    public bool bigLean;
    public float bombCD;
    public bool bombImmune;
    public float bombRadius;
    public float bombSpeed;
    public float bombTime;
    public float bombTimeMax;
    private float buffTime;
    public GameObject bulletLeft;
    Bullet bulletL;
    private int bulletMAX = 7;
    public GameObject bulletRight;
    Bullet bulletR;
    private bool buttonAttackRelease;
    public Dictionary<string, Image> cachedSprites;
    public float CameraMultiplier;
    public bool canJump = true;
    public GameObject checkBoxLeft;
    public GameObject checkBoxRight;


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
    private GameObject eren_titan;
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
    /*
    public XWeaponTrail leftbladetrail;
    public XWeaponTrail leftbladetrail2;
    */
    [Obsolete]
    public int leftBulletLeft = 7;
    public bool leftGunHasBullet = true;
    private float lTapTime = -1f;
    public GameObject maincamera;
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
    /*
    public XWeaponTrail rightbladetrail;
    public XWeaponTrail rightbladetrail2;
    */
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
    private string standAnimation = "stand";
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
    public TextMesh PlayerName;


    public HeroAudio audioSystem;
    private HookUI hookUI = new HookUI();

    /// <summary>
    /// Class to manage all <see cref="Hero"/> related Audio
    /// </summary>
    [Serializable]
    public class HeroAudio
    {
        /// <summary>
        /// Reference to the <see cref="AudioSource"/>
        /// </summary>
        public AudioSource source;

        public AudioClip clipDie;
        public AudioClip clipHit;
        public AudioClip clipRope;
        public AudioClip clipSlash;

        /// <summary>
        /// World Position of <see cref="source"/>
        /// </summary>
        public Vector3 Position => source.transform.position;

        /// <summary>
        /// Play an <see cref="AudioClip"/> once. Internally calls <see cref="AudioSource.PlayOneShot(AudioClip)"/>
        /// </summary>
        /// <param name="clip"><see cref="AudioClip"/> to play</param>
        /// <returns><see cref="HeroAudio"/> to allow chaining of Methods: <code>heroAudio.PlayOneShot(clip).PlayOneShot(clip2);</code></returns>
        public HeroAudio PlayOneShot(AudioClip clip)
        {
            source.PlayOneShot(clip);
            return this;
        }
        /// <summary>
        /// Play an <see cref="AudioClip"/> once. Internally calls <see cref="AudioSource.PlayOneShot(AudioClip, float)"/>
        /// </summary>
        /// <param name="clip"><see cref="AudioClip"/> to play</param>
        /// <param name="volume">Volume to play the <see cref="AudioClip"/> at</param>
        /// <returns><see cref="HeroAudio"/> to allow chaining of Methods: <code>heroAudio.PlayOneShot(clip).PlayOneShot(clip2);</code></returns>
        public HeroAudio PlayOneShot(AudioClip clip, float volume)
        {
            source.PlayOneShot(clip, volume);
            return this;
        }

        /// <summary>
        /// Set the <see cref="Transform.parent"/> to NULL, then Destroy the GameObject after <paramref name="destroyTime"/>
        /// </summary>
        /// <param name="destroyTime">Time to destroy <see cref="source"/> in seconds</param>
        public void Disconnect(float destroyTime)
        {
            source.transform.SetParent(null, true);
            Destroy(source, destroyTime);
        }
        /// <summary>
        /// Set the <see cref="Transform.parent"/> to NULL, then Destroy the GameObject after the duration of <paramref name="clip"/>
        /// </summary>
        /// <param name="clip">Destroy <see cref="source"/> after <see cref="AudioClip.length"/>+1 seconds. Internally calls <see cref="Disconnect(float)"/></param>
        public void Disconnect(AudioClip clip)
        {
            Disconnect(clip.length + 1);
        }
    }

    /// <summary>
    /// Class to Contain, Manage and Disable/Enable all Hook UI GameObjects
    /// </summary>
    [Serializable]
    public class HookUI
    {
        public Transform cross;
        public Transform crossL;
        public Transform crossR;

        public Image crossImage;
        public Image crossImageL;
        public Image crossImageR;

        public Text distanceLabel;

        public bool enabled = false;

        /// <summary>
        /// Find and Enable all Hook UI Elements required
        /// </summary>
        public void Find()
        {
            // Todo: Implement system that does not use GameObject.Find()

            cross = GameObject.Find("cross1").transform;
            crossImage = cross.GetComponentInChildren<Image>();
            crossL = GameObject.Find("crossL1").transform;
            crossImageL = crossL.GetComponentInChildren<Image>();
            crossR = GameObject.Find("crossR1").transform;
            crossImageR = crossR.GetComponentInChildren<Image>();

            distanceLabel = GameObject.Find("Distance").GetComponent<Text>();

            Enable();
        }

        /// <summary>
        /// Disable all Hook UI GameObjects
        /// </summary>
        public void Disable()
        {
            if (enabled)
            {
                cross.gameObject.SetActive(false);
                crossImage.gameObject.SetActive(false);
                crossL.gameObject.SetActive(false);
                crossImageL.gameObject.SetActive(false);
                crossR.gameObject.SetActive(false);
                crossImageR.gameObject.SetActive(false);

                distanceLabel.gameObject.SetActive(false);

                enabled = false;
            }
        }

        /// <summary>
        /// Enable all Hook UI GameObjects
        /// </summary>
        public void Enable()
        {
            if (!enabled)
            {
                cross.gameObject.SetActive(true);
                crossImage.gameObject.SetActive(true);
                crossL.gameObject.SetActive(true);
                crossImageL.gameObject.SetActive(true);
                crossR.gameObject.SetActive(true);
                crossImageR.gameObject.SetActive(true);

                distanceLabel.gameObject.SetActive(true);

                enabled = true;
            }
        }
    }

    public override void OnHit(Entity attacker, int damage)
    {
        //TODO: 160 HERO OnHit logic
        //if (!isInvincible() && _state != HERO_STATE.Grab)
        //    markDie();
    }

    private void ApplyForceToBody(GameObject GO, Vector3 v)
    {
        if(GO.TryGetComponent<Rigidbody>(out var r))
        {
            r.AddForce(v);
            r.AddTorque(
                UnityEngine.Random.Range((float)-10f, (float)10f), 
                UnityEngine.Random.Range((float)-10f, (float)10f), 
                UnityEngine.Random.Range((float)-10f, (float)10f));
        }


    }

    public void AttackAccordingToMouse()
    {
        if (Input.mousePosition.x < (Screen.width * 0.5))
        {
            attackAnimation = "attack2";
        }
        else
        {
            attackAnimation = "attack1";
        }
    }

    public void AttackAccordingToTarget(Transform a)
    {
        Vector3 vector = a.position - transform.position;
        float current = -Mathf.Atan2(vector.z, vector.x) * Mathf.Rad2Deg;
        float f = -Mathf.DeltaAngle(current, transform.rotation.eulerAngles.y - 90f);
        if (((Mathf.Abs(f) < 90f) && (vector.magnitude < 6f)) && ((a.position.y <= (transform.position.y + 2f)) && (a.position.y >= (transform.position.y - 5f))))
        {
            attackAnimation = "attack4";
        }
        else if (f > 0f)
        {
            attackAnimation = "attack1";
        }
        else
        {
            attackAnimation = "attack2";
        }
    }

    protected override void Awake()
    {
        base.Awake();
        InGameUI = GameObject.Find("InGameUi");
        Cache();
        setup = gameObject.GetComponent<HERO_SETUP>();
        baseRigidBody.freezeRotation = true;
        baseRigidBody.useGravity = false;
        handL = baseTransform.Find("Amarture/Controller_Body/hip/spine/chest/shoulder_L/upper_arm_L/forearm_L/hand_L");
        handR = baseTransform.Find("Amarture/Controller_Body/hip/spine/chest/shoulder_R/upper_arm_R/forearm_R/hand_R");
        forearmL = baseTransform.Find("Amarture/Controller_Body/hip/spine/chest/shoulder_L/upper_arm_L/forearm_L");
        forearmR = baseTransform.Find("Amarture/Controller_Body/hip/spine/chest/shoulder_R/upper_arm_R/forearm_R");
        upperarmL = baseTransform.Find("Amarture/Controller_Body/hip/spine/chest/shoulder_L/upper_arm_L");
        upperarmR = baseTransform.Find("Amarture/Controller_Body/hip/spine/chest/shoulder_R/upper_arm_R");
        Equipment = gameObject.AddComponent<Equipment>();
        Faction = Service.Faction.GetHumanity();
        Service.Entity.Register(this);
    }

    public void BackToHuman()
    {
        gameObject.GetComponent<SmoothSyncMovement>().disabled = false;
        baseRigidBody.velocity = Vector3.zero;
        titanForm = false;
        Ungrabbed();
        FalseAttack();
        skillCDDuration = skillCDLast;
        GameObject.Find("MainCamera").GetComponent<IN_GAME_MAIN_CAMERA>().SetMainObject(gameObject, true, false);
        photonView.RPC(nameof(BackToHumanRPC), PhotonTargets.Others, new object[0]);
    }

    [PunRPC]
    private void BackToHumanRPC()
    {
        titanForm = false;
        eren_titan = null;
        gameObject.GetComponent<SmoothSyncMovement>().disabled = false;
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
            baseRigidBody.AddForce(force, ForceMode.Impulse);
            transform.LookAt(transform.position);
        }
    }

    private void BodyLean()
    {
        if (photonView.isMine)
        {
            float z = 0f;
            needLean = false;
            if ((!useGun && (State == HERO_STATE.Attack)) && ((attackAnimation != "attack3_1") && (attackAnimation != "attack3_2")))
            {
                float y = baseRigidBody.velocity.y;
                float x = baseRigidBody.velocity.x;
                float num4 = baseRigidBody.velocity.z;
                float num5 = Mathf.Sqrt((x * x) + (num4 * num4));
                float num6 = Mathf.Atan2(y, num5) * Mathf.Rad2Deg;
                targetRotation = Quaternion.Euler(-num6 * (1f - (Vector3.Angle(baseRigidBody.velocity, transform.forward) / 90f)), facingDirection, 0f);
                if ((isLeftHandHooked && (bulletLeft != null)) || (isRightHandHooked && (bulletRight != null)))
                {
                    transform.rotation = targetRotation;
                }
            }
            else
            {
                if ((isLeftHandHooked && (bulletLeft != null)) && (isRightHandHooked && (bulletRight != null)))
                {
                    if (almostSingleHook)
                    {
                        needLean = true;
                        z = GetLeanAngle(bulletRight.transform.position, true);
                    }
                }
                else if (isLeftHandHooked && (bulletLeft != null))
                {
                    needLean = true;
                    z = GetLeanAngle(bulletLeft.transform.position, true);
                }
                else if (isRightHandHooked && (bulletRight != null))
                {
                    needLean = true;
                    z = GetLeanAngle(bulletRight.transform.position, false);
                }
                if (needLean)
                {
                    float a = 0f;
                    if (!useGun && (State != HERO_STATE.Attack))
                    {
                        a = currentSpeed * 0.1f;
                        a = Mathf.Min(a, 20f);
                    }
                    targetRotation = Quaternion.Euler(-a, facingDirection, z);
                }
                else if (State != HERO_STATE.Attack)
                {
                    targetRotation = Quaternion.Euler(0f, facingDirection, 0f);
                }
            }
        }
    }

    public void BombInit()
    {
        skillIDHUD = skillId;
        skillCDDuration = skillCDLast;
        if (GameSettings.PvP.Bomb == true)
        {
            int num = (int)FengGameManagerMKII.settings[250];
            int num2 = (int)FengGameManagerMKII.settings[0xfb];
            int num3 = (int)FengGameManagerMKII.settings[0xfc];
            int num4 = (int)FengGameManagerMKII.settings[0xfd];
            if ((num < 0) || (num > 10))
            {
                num = 5;
                FengGameManagerMKII.settings[250] = 5;
            }
            if ((num2 < 0) || (num2 > 10))
            {
                num2 = 5;
                FengGameManagerMKII.settings[0xfb] = 5;
            }
            if ((num3 < 0) || (num3 > 10))
            {
                num3 = 5;
                FengGameManagerMKII.settings[0xfc] = 5;
            }
            if ((num4 < 0) || (num4 > 10))
            {
                num4 = 5;
                FengGameManagerMKII.settings[0xfd] = 5;
            }
            if ((((num + num2) + num3) + num4) > 20)
            {
                num = 5;
                num2 = 5;
                num3 = 5;
                num4 = 5;
                FengGameManagerMKII.settings[250] = 5;
                FengGameManagerMKII.settings[0xfb] = 5;
                FengGameManagerMKII.settings[0xfc] = 5;
                FengGameManagerMKII.settings[0xfd] = 5;
            }
            bombTimeMax = ((num2 * 60f) + 200f) / ((num3 * 60f) + 200f);
            bombRadius = (num * 4f) + 20f;
            bombCD = (num4 * -0.4f) + 5f;
            bombSpeed = (num3 * 60f) + 200f;
            ExitGames.Client.Photon.Hashtable propertiesToSet = new ExitGames.Client.Photon.Hashtable();
            propertiesToSet.Add(PhotonPlayerProperty.RCBombR, (float)FengGameManagerMKII.settings[0xf6]);
            propertiesToSet.Add(PhotonPlayerProperty.RCBombG, (float)FengGameManagerMKII.settings[0xf7]);
            propertiesToSet.Add(PhotonPlayerProperty.RCBombB, (float)FengGameManagerMKII.settings[0xf8]);
            propertiesToSet.Add(PhotonPlayerProperty.RCBombA, (float)FengGameManagerMKII.settings[0xf9]);
            propertiesToSet.Add(PhotonPlayerProperty.RCBombRadius, bombRadius);
            PhotonNetwork.player.SetCustomProperties(propertiesToSet);
            skillId = "bomb";
            skillIDHUD = "armin";
            skillCDLast = bombCD;
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
        GameObject obj2 = (GameObject)Instantiate(Resources.Load("Character_parts/AOTTG_HERO_body"), transform.position, transform.rotation);
        obj2.gameObject.GetComponent<HERO_SETUP>().myCostume = setup.myCostume;
        obj2.GetComponent<HERO_SETUP>().isDeadBody = true;
        obj2.GetComponent<HERO_DEAD_BODY_SETUP>().init(currentAnimation, baseAnimation[currentAnimation].normalizedTime, BODY_PARTS.ARM_R);
        if (!isBite)
        {
            GameObject gO = (GameObject)Instantiate(Resources.Load("Character_parts/AOTTG_HERO_body"), transform.position, transform.rotation);
            GameObject obj4 = (GameObject)Instantiate(Resources.Load("Character_parts/AOTTG_HERO_body"), transform.position, transform.rotation);
            GameObject obj5 = (GameObject)Instantiate(Resources.Load("Character_parts/AOTTG_HERO_body"), transform.position, transform.rotation);
            gO.gameObject.GetComponent<HERO_SETUP>().myCostume = setup.myCostume;
            obj4.gameObject.GetComponent<HERO_SETUP>().myCostume = setup.myCostume;
            obj5.gameObject.GetComponent<HERO_SETUP>().myCostume = setup.myCostume;
            gO.GetComponent<HERO_SETUP>().isDeadBody = true;
            obj4.GetComponent<HERO_SETUP>().isDeadBody = true;
            obj5.GetComponent<HERO_SETUP>().isDeadBody = true;
            gO.GetComponent<HERO_DEAD_BODY_SETUP>().init(currentAnimation, baseAnimation[currentAnimation].normalizedTime, BODY_PARTS.UPPER);
            obj4.GetComponent<HERO_DEAD_BODY_SETUP>().init(currentAnimation, baseAnimation[currentAnimation].normalizedTime, BODY_PARTS.LOWER);
            obj5.GetComponent<HERO_DEAD_BODY_SETUP>().init(currentAnimation, baseAnimation[currentAnimation].normalizedTime, BODY_PARTS.ARM_L);
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
        Transform handL = transform.Find("Amarture/Controller_Body/hip/spine/chest/shoulder_L/upper_arm_L/forearm_L/hand_L");
        Transform handR = transform.Find("Amarture/Controller_Body/hip/spine/chest/shoulder_R/upper_arm_R/forearm_R/hand_R");
        if (useGun)
        {
            obj6  = (GameObject)Instantiate(Resources.Load("Character_parts/character_gun_l"), handL.position, handL.rotation);
            obj7  = (GameObject)Instantiate(Resources.Load("Character_parts/character_gun_r"), handR.position, handR.rotation);
            obj8  = (GameObject)Instantiate(Resources.Load("Character_parts/character_3dmg_2"), handL.position, handL.rotation);
            obj9  = (GameObject)Instantiate(Resources.Load("Character_parts/character_gun_mag_l"), handL.position, handL.rotation);
            obj10 = (GameObject)Instantiate(Resources.Load("Character_parts/character_gun_mag_r"), handL.position, handL.rotation);
        }
        else
        {
            obj6  = (GameObject)Instantiate(Resources.Load("Character_parts/character_blade_l"), handL.position, handL.rotation);
            obj7  = (GameObject)Instantiate(Resources.Load("Character_parts/character_blade_r"), handR.position, handR.rotation);
            obj8  = (GameObject)Instantiate(Resources.Load("Character_parts/character_3dmg"), handL.position, handL.rotation);
            obj9  = (GameObject)Instantiate(Resources.Load("Character_parts/character_3dmg_gas_l"), handL.position, handL.rotation);
            obj10 = (GameObject)Instantiate(Resources.Load("Character_parts/character_3dmg_gas_r"), handL.position, handL.rotation);
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
                if ((currentBuff == BUFF.SpeedUp) && baseAnimation.IsPlaying("run_sasha"))
                {
                    CrossFade("run_1", 0.1f);
                }
                currentBuff = BUFF.NoBuff;
            }
        }
    }

    public void Cache()
    {
        baseTransform = transform;
        baseRigidBody = GetComponent<Rigidbody>();
        maincamera = GameObject.Find("MainCamera");
        if (photonView.isMine)
        {
            baseAnimation = GetComponent<Animation>();
            cachedSprites = new Dictionary<string, Image>();

            hookUI.Find();

            //foreach (GameObject obj2 in UnityEngine.Object.FindObjectsOfType(typeof(GameObject)))
            //{
            //    if ((obj2.GetComponent<UISprite>() != null) && obj2.activeInHierarchy)
            //    {
            //        string name = obj2.name;
            //        if (!((((name.Contains("blade") || name.Contains("bullet")) || (name.Contains("gas") || name.Contains("flare"))) || name.Contains("skill_cd")) ? cachedSprites.ContainsKey(name) : true))
            //        {
            //            cachedSprites.Add(name, obj2.GetComponent<UISprite>());
            //        }
            //    }
            //}
            //foreach (var obj in Resources.FindObjectsOfTypeAll<GameObject>() )
            //{
            //    var image = obj.GetComponent<Image>();
            //    if (image == null || !obj.activeInHierarchy) continue;
            //    if (obj.name.Contains("Gas"))
            //    {
            //        cachedSprites.Add(obj.name, image);
            //    }
            //}
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

    private float CalculateJumpVerticalSpeed()
    {
        return Mathf.Sqrt((2f * jumpHeight) * gravity);
    }

    private void ChangeBlade()
    {
        if ((!useGun || grounded) || GameSettings.PvP.AhssAirReload.Value)
        {
            State = HERO_STATE.ChangeBlade;
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
        LayerMask mask = ((int)1) << LayerMask.NameToLayer("PlayerAttackBox");
        LayerMask mask2 = ((int)1) << LayerMask.NameToLayer("Ground");
        LayerMask mask3 = ((int)1) << LayerMask.NameToLayer("EnemyBox");
        LayerMask mask4 = (mask | mask2) | mask3;
        RaycastHit[] hitArray = Physics.RaycastAll(ray, 180f, mask4.value);
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
                    MindlessTitan component = gameObject.transform.root.gameObject.GetComponent<MindlessTitan>();
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

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "titan") return;
        var force = collision.impulse.magnitude / Time.fixedDeltaTime;
        if (GameSettings.Gamemode.ImpactForce > 0 && force >= GameSettings.Gamemode.ImpactForce)
        {
            Die(new Vector3(), false); 
        }
    }

    public void ContinueAnimation()
    {
        IEnumerator enumerator = baseAnimation.GetEnumerator();
        try
        {
            while (enumerator.MoveNext())
            {
                AnimationState current = (AnimationState)enumerator.Current;
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
        baseAnimation.CrossFade(aniName, time);
        if (PhotonNetwork.connected && photonView.isMine)
        {
            object[] parameters = new object[] { aniName, time };
            photonView.RPC(nameof(NetCrossFade), PhotonTargets.Others, parameters);
        }
    }

    public void TryCrossFade(string animationName, float time)
    {
        if (!baseAnimation.IsPlaying(animationName))
        {
            CrossFade(animationName, time);
        }
    }

    public string CurrentPlayingClipName()
    {
        IEnumerator enumerator = baseAnimation.GetEnumerator();
        try
        {
            while (enumerator.MoveNext())
            {
                AnimationState current = (AnimationState)enumerator.Current;
                if (current != null && baseAnimation.IsPlaying(current.name))
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
        baseAnimation["attack5"].speed = 1.85f;
        baseAnimation["changeBlade"].speed = 1.2f;
        baseAnimation["air_release"].speed = 0.6f;
        baseAnimation["changeBlade_air"].speed = 0.8f;
        baseAnimation["AHSS_gun_reload_both"].speed = 0.38f;
        baseAnimation["AHSS_gun_reload_both_air"].speed = 0.5f;
        baseAnimation["AHSS_gun_reload_l"].speed = 0.4f;
        baseAnimation["AHSS_gun_reload_l_air"].speed = 0.5f;
        baseAnimation["AHSS_gun_reload_r"].speed = 0.4f;
        baseAnimation["AHSS_gun_reload_r_air"].speed = 0.5f;
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
            baseRigidBody.rotation = quaternion;
            targetRotation = quaternion;
            PhotonNetwork.Instantiate("FX/boost_smoke", transform.position, transform.rotation, 0);
            dashTime = 0.5f;
            CrossFade("dash", 0.1f);
            baseAnimation["dash"].time = 0.1f;
            State = HERO_STATE.AirDodge;
            FalseAttack();
            baseRigidBody.AddForce((dashV * 40f), ForceMode.VelocityChange);
        }
    }

    public void Die(Vector3 v, bool isBite)
    {
        if (invincible <= 0f)
        {
            if (titanForm && (eren_titan != null))
            {
                eren_titan.GetComponent<ErenTitan>().lifeTime = 0.1f;
            }
            if (bulletLeft != null)
            {
                bulletLeft.GetComponent<Bullet>().removeMe();
            }
            if (bulletRight != null)
            {
                bulletRight.GetComponent<Bullet>().removeMe();
            }

            if ((photonView.isMine) && !useGun)
            {
                /*
                leftbladetrail.Deactivate();
                rightbladetrail.Deactivate();
                leftbladetrail2.Deactivate();
                rightbladetrail2.Deactivate();
                */
            }
            BreakApart(v, isBite);
            currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().gameOver = true;
            FalseAttack();
            hasDied = true;
            audioSystem
                .PlayOneShot(audioSystem.clipDie)
                .Disconnect(audioSystem.clipDie);
            var propertiesToSet = new ExitGames.Client.Photon.Hashtable();
            propertiesToSet.Add(PhotonPlayerProperty.deaths, (int)PhotonNetwork.player.CustomProperties[PhotonPlayerProperty.deaths] + 1);
            photonView.owner.SetCustomProperties(propertiesToSet);
            if (PlayerPrefs.HasKey("EnableSS") && (PlayerPrefs.GetInt("EnableSS") == 1))
            {
                GameObject.Find("MainCamera").GetComponent<IN_GAME_MAIN_CAMERA>().StartSnapShot2(audioSystem.Position, 0, null, 0.02f);
            }
            Destroy(gameObject);
        }
    }

    [Obsolete("Unused Method")]
    public void Die(Transform tf)
    {
        if (invincible <= 0f)
        {
            if (titanForm && (eren_titan != null))
            {
                eren_titan.GetComponent<ErenTitan>().lifeTime = 0.1f;
            }
            if (bulletLeft != null)
            {
                bulletLeft.GetComponent<Bullet>().removeMe();
            }
            if (bulletRight != null)
            {
                bulletRight.GetComponent<Bullet>().removeMe();
            }
            audioSystem
                .PlayOneShot(audioSystem.clipDie)
                .Disconnect(audioSystem.clipDie);

            currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().SetMainObject(null, true, false);
            currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().gameOver = true;
            FalseAttack();
            hasDied = true;
            GameObject obj2 = (GameObject)Instantiate(Resources.Load("hitMeat2"));
            obj2.transform.position = audioSystem.Position;
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
                CrossFade("dodge", 0.1f);
            }
            else
            {
                PlayAnimation("dodge");
                playAnimationAt("dodge", 0.2f);
            }
            sparksEmission.enabled = false;
        }
    }

    private void ErenTransform()
    {
        skillCDDuration = skillCDLast;
        if (bulletLeft != null)
        {
            bulletLeft.GetComponent<Bullet>().removeMe();
        }
        if (bulletRight != null)
        {
            bulletRight.GetComponent<Bullet>().removeMe();
        }
        eren_titan = PhotonNetwork.Instantiate("ErenTitan", transform.position, transform.rotation, 0);
        eren_titan.GetComponent<ErenTitan>().realBody = gameObject;
        GameObject.Find("MainCamera").GetComponent<IN_GAME_MAIN_CAMERA>().FlashBlind();
        GameObject.Find("MainCamera").GetComponent<IN_GAME_MAIN_CAMERA>().SetMainObject(eren_titan, true, false);
        eren_titan.GetComponent<ErenTitan>().born();
        eren_titan.GetComponent<Rigidbody>().velocity = baseRigidBody.velocity;
        baseRigidBody.velocity = Vector3.zero;
        transform.position = eren_titan.transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/neck").position;
        titanForm = true;
        object[] parameters = new object[] { eren_titan.GetPhotonView().viewID };
        photonView.RPC(nameof(WhoIsMyErenTitan), PhotonTargets.Others, parameters);
        if ((smoke_3dmgEmission.enabled && photonView.isMine))
        {
            object[] objArray2 = new object[] { false };
            photonView.RPC(nameof(Net3DMGSMOKE), PhotonTargets.Others, objArray2);
        }
        smoke_3dmgEmission.enabled = false;
    }

    private void EscapeFromGrab()
    {
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
                checkBoxLeft.GetComponent<TriggerColliderWeapon>().IsActive = false;
                checkBoxRight.GetComponent<TriggerColliderWeapon>().IsActive = false;
                checkBoxLeft.GetComponent<TriggerColliderWeapon>().ClearHits();
                checkBoxRight.GetComponent<TriggerColliderWeapon>().ClearHits();
                //leftbladetrail.StopSmoothly(0.2f);
                //rightbladetrail.StopSmoothly(0.2f);
                //leftbladetrail2.StopSmoothly(0.2f);
                //rightbladetrail2.StopSmoothly(0.2f);
            }
            attackLoop = 0;
            if (!attackReleased)
            {
                ContinueAnimation();
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

    //Hotfix for Issue 97.
    private void AddRightForce()
    {
        //Whereas this may not be completely accurate to AoTTG, it is very close. Further balancing required in the future.
        baseRigidBody.AddForce(baseRigidBody.velocity * 0.00f, ForceMode.Acceleration);
    }

    private void FixedUpdate()
    {
        if ((!titanForm && !isCannon) && (!IN_GAME_MAIN_CAMERA.isPausing))
        {
            currentSpeed = baseRigidBody.velocity.magnitude;
            if (photonView.isMine)
            {
                if (!((baseAnimation.IsPlaying("attack3_2") || baseAnimation.IsPlaying("attack5")) || baseAnimation.IsPlaying("special_petra")))
                {
                    baseRigidBody.rotation = Quaternion.Lerp(gameObject.transform.rotation, targetRotation, Time.deltaTime * 6f);
                }
                if (State == HERO_STATE.Grab)
                {
                    baseRigidBody.AddForce(-baseRigidBody.velocity, ForceMode.VelocityChange);
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
                    }
                    if (hookSomeOne)
                    {
                        if (hookTarget != null)
                        {
                            Vector3 vector2 = hookTarget.transform.position - baseTransform.position;
                            float magnitude = vector2.magnitude;
                            if (magnitude > 2f)
                            {
                                baseRigidBody.AddForce((((vector2.normalized * Mathf.Pow(magnitude, 0.15f)) * 30f) - (baseRigidBody.velocity * 0.95f)), ForceMode.VelocityChange);
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
                            Vector3 vector3 = badGuy.transform.position - baseTransform.position;
                            float f = vector3.magnitude;
                            if (f > 5f)
                            {
                                baseRigidBody.AddForce(((vector3.normalized * Mathf.Pow(f, 0.15f)) * 0.2f), ForceMode.Impulse);
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
                        if ((bulletLeft != null) && bulletLeft.GetComponent<Bullet>().isHooked())
                        {
                            isLeftHandHooked = true;
                            Vector3 to = bulletLeft.transform.position - baseTransform.position;
                            to.Normalize();
                            to = (to * 10f);
                            if (!isLaunchRight)
                            {
                                to = (to * 2f);
                            }
                            if ((Vector3.Angle(baseRigidBody.velocity, to) > 90f) && InputManager.Key(InputHuman.Jump))
                            {
                                flag3 = true;
                                flag2 = true;
                            }
                            if (!flag3)
                            {
                                baseRigidBody.AddForce(to);
                                if (Vector3.Angle(baseRigidBody.velocity, to) > 90f)
                                {
                                    baseRigidBody.AddForce((-baseRigidBody.velocity * 2f), ForceMode.Acceleration);
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
                                bulletLeft.GetComponent<Bullet>().disable();
                                releaseIfIHookSb();
                                bulletLeft = null;
                                flag3 = false;
                            }
                        }
                    }
                    if (isLaunchRight)
                    {
                        if ((bulletRight != null) && bulletRight.GetComponent<Bullet>().isHooked())
                        {
                            isRightHandHooked = true;
                            Vector3 vector5 = bulletRight.transform.position - baseTransform.position;
                            vector5.Normalize();
                            vector5 = (vector5 * 10f);
                            if (!isLaunchLeft)
                            {
                                vector5 = (vector5 * 2f);
                            }
                            if ((Vector3.Angle(baseRigidBody.velocity, vector5) > 90f) && InputManager.Key(InputHuman.Jump))
                            {
                                flag4 = true;
                                flag2 = true;
                            }
                            if (!flag4)
                            {
                                baseRigidBody.AddForce(vector5);
                                if (Vector3.Angle(baseRigidBody.velocity, vector5) > 90f)
                                {
                                    baseRigidBody.AddForce((-baseRigidBody.velocity * 2f), ForceMode.Acceleration);
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
                                bulletRight.GetComponent<Bullet>().disable();
                                releaseIfIHookSb();
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
                            if (attackAnimation == "attack5")
                            {
                                if ((baseAnimation[attackAnimation].normalizedTime > 0.4f) && (baseAnimation[attackAnimation].normalizedTime < 0.61f))
                                {
                                    baseRigidBody.AddForce((gameObject.transform.forward * 200f));
                                }
                            }
                            else if (attackAnimation == "special_petra")
                            {
                                if ((baseAnimation[attackAnimation].normalizedTime > 0.35f) && (baseAnimation[attackAnimation].normalizedTime < 0.48f))
                                {
                                    baseRigidBody.AddForce((gameObject.transform.forward * 200f));
                                }
                            }
                            else if (baseAnimation.IsPlaying("attack3_2"))
                            {
                                zero = Vector3.zero;
                            }
                            else if (baseAnimation.IsPlaying("attack1") || baseAnimation.IsPlaying("attack2"))
                            {
                                baseRigidBody.AddForce((gameObject.transform.forward * 200f));
                            }
                            if (baseAnimation.IsPlaying("attack3_2"))
                            {
                                zero = Vector3.zero;
                            }
                        }
                        if (justGrounded)
                        {
                            if ((State != HERO_STATE.Attack) || (((attackAnimation != "attack3_1") && (attackAnimation != "attack5")) && (attackAnimation != "special_petra")))
                            {
                                if ((((State != HERO_STATE.Attack) && (x == 0f)) && ((z == 0f) && (bulletLeft == null))) && ((bulletRight == null) && (State != HERO_STATE.FillGas)))
                                {
                                    State = HERO_STATE.Land;
                                    CrossFade("dash_land", 0.01f);
                                }
                                else
                                {
                                    buttonAttackRelease = true;
                                    if (((State != HERO_STATE.Attack) && (((baseRigidBody.velocity.x * baseRigidBody.velocity.x) + (baseRigidBody.velocity.z * baseRigidBody.velocity.z)) > ((speed * speed) * 1.5f))) && (State != HERO_STATE.FillGas))
                                    {
                                        State = HERO_STATE.Slide;
                                        CrossFade("slide", 0.05f);
                                        facingDirection = Mathf.Atan2(baseRigidBody.velocity.x, baseRigidBody.velocity.z) * Mathf.Rad2Deg;
                                        targetRotation = Quaternion.Euler(0f, facingDirection, 0f);
                                        sparksEmission.enabled = true;
                                    }
                                }
                            }
                            justGrounded = false;
                            zero = baseRigidBody.velocity;
                        }
                        if (((State == HERO_STATE.Attack) && (attackAnimation == "attack3_1")) && (baseAnimation[attackAnimation].normalizedTime >= 1f))
                        {
                            PlayAnimation("attack3_2");
                            resetAnimationSpeed();
                            vector7 = Vector3.zero;
                            baseRigidBody.velocity = vector7;
                            zero = vector7;
                            currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().StartShake(0.2f, 0.3f, 0.95f);
                        }
                        if (State == HERO_STATE.GroundDodge)
                        {
                            if ((baseAnimation["dodge"].normalizedTime >= 0.2f) && (baseAnimation["dodge"].normalizedTime < 0.8f))
                            {
                                zero = ((-baseTransform.forward * 2.4f) * speed);
                            }
                            if (baseAnimation["dodge"].normalizedTime > 0.8f)
                            {
                                zero = (baseRigidBody.velocity * 0.9f);
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
                                if (((!baseAnimation.IsPlaying("run_1") && !baseAnimation.IsPlaying("jump")) && !baseAnimation.IsPlaying("run_sasha")) && (!baseAnimation.IsPlaying("horse_geton") || (baseAnimation["horse_geton"].normalizedTime >= 0.5f)))
                                {
                                    if ((buffTime > 0f) && (currentBuff == BUFF.SpeedUp))
                                    {
                                        CrossFade("run_sasha", 0.1f);
                                    }
                                    else
                                    {
                                        CrossFade("run_1", 0.1f);
                                    }
                                }
                            }
                            else
                            {
                                if (!(((baseAnimation.IsPlaying(standAnimation) || (State == HERO_STATE.Land)) || (baseAnimation.IsPlaying("jump") || baseAnimation.IsPlaying("horse_geton"))) || baseAnimation.IsPlaying("grabbed")))
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
                            zero = (baseRigidBody.velocity * 0.96f);
                        }
                        else if (State == HERO_STATE.Slide)
                        {
                            zero = (baseRigidBody.velocity * 0.99f);
                            if (currentSpeed < (speed * 1.2f))
                            {
                                Idle();
                                sparksEmission.enabled = false;
                            }
                        }
                        Vector3 velocity = baseRigidBody.velocity;
                        Vector3 force = zero - velocity;
                        force.x = Mathf.Clamp(force.x, -maxVelocityChange, maxVelocityChange);
                        force.z = Mathf.Clamp(force.z, -maxVelocityChange, maxVelocityChange);
                        force.y = 0f;
                        if (baseAnimation.IsPlaying("jump") && (baseAnimation["jump"].normalizedTime > 0.18f))
                        {
                            force.y += 8f;
                        }
                        if ((baseAnimation.IsPlaying("horse_geton") && (baseAnimation["horse_geton"].normalizedTime > 0.18f)) && (baseAnimation["horse_geton"].normalizedTime < 1f))
                        {
                            float num7 = 6f;
                            force = -baseRigidBody.velocity;
                            force.y = num7;
                            float num8 = Vector3.Distance(myHorse.transform.position, baseTransform.position);
                            float num9 = ((0.6f * gravity) * num8) / 12f;
                            vector7 = myHorse.transform.position - baseTransform.position;
                            force += (num9 * vector7.normalized);
                        }
                        if (!((State == HERO_STATE.Attack) && useGun))
                        {
                            baseRigidBody.AddForce(force, ForceMode.VelocityChange);
                            baseRigidBody.rotation = Quaternion.Lerp(gameObject.transform.rotation, Quaternion.Euler(0f, facingDirection, 0f), Time.deltaTime * 10f);
                        }
                    }
                    else
                    {
                        if (sparksEmission.enabled)
                        {
                            sparksEmission.enabled = false;
                        }
                        if ((myHorse && (baseAnimation.IsPlaying("horse_geton") || baseAnimation.IsPlaying("air_fall"))) && ((baseRigidBody.velocity.y < 0f) && (Vector3.Distance(myHorse.transform.position + Vector3.up * 1.65f, baseTransform.position) < 0.5f)))
                        {
                            baseTransform.position = myHorse.transform.position + Vector3.up * 1.65f;
                            baseTransform.rotation = myHorse.transform.rotation;
                            isMounted = true;
                            CrossFade("horse_idle", 0.1f);
                            myHorse.Mount();
                        }
                        if (!((((((State != HERO_STATE.Idle) || baseAnimation.IsPlaying("dash")) || (baseAnimation.IsPlaying("wallrun") || baseAnimation.IsPlaying("toRoof"))) || ((baseAnimation.IsPlaying("horse_geton") || baseAnimation.IsPlaying("horse_getoff")) || (baseAnimation.IsPlaying("air_release") || isMounted))) || ((baseAnimation.IsPlaying("air_hook_l_just") && (baseAnimation["air_hook_l_just"].normalizedTime < 1f)) || (baseAnimation.IsPlaying("air_hook_r_just") && (baseAnimation["air_hook_r_just"].normalizedTime < 1f)))) ? (baseAnimation["dash"].normalizedTime < 0.99f) : false))
                        {
                            if (((!isLeftHandHooked && !isRightHandHooked) && ((baseAnimation.IsPlaying("air_hook_l") || baseAnimation.IsPlaying("air_hook_r")) || baseAnimation.IsPlaying("air_hook"))) && (baseRigidBody.velocity.y > 20f))
                            {
                                baseAnimation.CrossFade("air_release");
                            }
                            else
                            {
                                bool flag5 = (Mathf.Abs(baseRigidBody.velocity.x) + Mathf.Abs(baseRigidBody.velocity.z)) > 25f;
                                bool flag6 = baseRigidBody.velocity.y < 0f;
                                if (!flag5)
                                {
                                    if (flag6)
                                    {
                                        if (!baseAnimation.IsPlaying("air_fall"))
                                        {
                                            CrossFade("air_fall", 0.2f);
                                        }
                                    }
                                    else if (!baseAnimation.IsPlaying("air_rise"))
                                    {
                                        CrossFade("air_rise", 0.2f);
                                    }
                                }
                                else if (!isLeftHandHooked && !isRightHandHooked)
                                {
                                    float current = -Mathf.Atan2(baseRigidBody.velocity.z, baseRigidBody.velocity.x) * Mathf.Rad2Deg;
                                    float num11 = -Mathf.DeltaAngle(current, baseTransform.rotation.eulerAngles.y - 90f);
                                    if (Mathf.Abs(num11) < 45f)
                                    {
                                        if (!baseAnimation.IsPlaying("air2"))
                                        {
                                            CrossFade("air2", 0.2f);
                                        }
                                    }
                                    else if ((num11 < 135f) && (num11 > 0f))
                                    {
                                        if (!baseAnimation.IsPlaying("air2_right"))
                                        {
                                            CrossFade("air2_right", 0.2f);
                                        }
                                    }
                                    else if ((num11 > -135f) && (num11 < 0f))
                                    {
                                        if (!baseAnimation.IsPlaying("air2_left"))
                                        {
                                            CrossFade("air2_left", 0.2f);
                                        }
                                    }
                                    else if (!baseAnimation.IsPlaying("air2_backward"))
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
                                else if (!baseAnimation.IsPlaying(Equipment.Weapon.HookForward))
                                {
                                    TryCrossFade(Equipment.Weapon.HookForward, 0.1f);
                                }
                            }
                        }
                        if (((State == HERO_STATE.Idle) && baseAnimation.IsPlaying("air_release")) && (baseAnimation["air_release"].normalizedTime >= 1f))
                        {
                            CrossFade("air_rise", 0.2f);
                        }
                        if (baseAnimation.IsPlaying("horse_getoff") && (baseAnimation["horse_getoff"].normalizedTime >= 1f))
                        {
                            CrossFade("air_rise", 0.2f);
                        }
                        if (baseAnimation.IsPlaying("toRoof"))
                        {
                            if (baseAnimation["toRoof"].normalizedTime < 0.22f)
                            {
                                baseRigidBody.velocity = Vector3.zero;
                                baseRigidBody.AddForce(new Vector3(0f, gravity * baseRigidBody.mass, 0f));
                            }
                            else
                            {
                                if (!wallJump)
                                {
                                    wallJump = true;
                                    baseRigidBody.AddForce((Vector3.up * 8f), ForceMode.Impulse);
                                }
                                baseRigidBody.AddForce((baseTransform.forward * 0.05f), ForceMode.Impulse);
                            }
                            if (baseAnimation["toRoof"].normalizedTime >= 1f)
                            {
                                PlayAnimation("air_rise");
                            }
                        }
                        else if (!(((((State != HERO_STATE.Idle) || !IsPressDirectionTowardsHero(x, z)) ||
                                     (InputManager.Key(InputHuman.Jump) ||
                                      InputManager.Key(InputHuman.HookLeft))) ||
                                    ((InputManager.Key(InputHuman.HookRight) ||
                                      InputManager.Key(InputHuman.HookBoth)) ||
                                     (!IsFrontGrounded() || baseAnimation.IsPlaying("wallrun")))) ||
                                   baseAnimation.IsPlaying("dodge")))
                        {
                            CrossFade("wallrun", 0.1f);
                            wallRunTime = 0f;
                        }
                        else if (baseAnimation.IsPlaying("wallrun"))
                        {
                            baseRigidBody.AddForce(((Vector3.up * speed)) - baseRigidBody.velocity, ForceMode.VelocityChange);
                            wallRunTime += Time.deltaTime;
                            if ((wallRunTime > 1f) || ((z == 0f) && (x == 0f)))
                            {
                                baseRigidBody.AddForce(((-baseTransform.forward * speed) * 0.75f), ForceMode.Impulse);
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
                        else if ((!baseAnimation.IsPlaying("attack5") && !baseAnimation.IsPlaying("special_petra")) && (!baseAnimation.IsPlaying("dash") && !baseAnimation.IsPlaying("jump")))
                        {
                            Vector3 vector11 = new Vector3(x, 0f, z);
                            float num12 = GetGlobalFacingDirection(x, z);
                            Vector3 vector12 = GetGlobalFacingVector3(num12);
                            float num13 = (vector11.magnitude <= 0.95f) ? ((vector11.magnitude >= 0.25f) ? vector11.magnitude : 0f) : 1f;
                            vector12 = (vector12 * num13);
                            vector12 = (vector12 * ((((float)setup.myCostume.stat.ACL) / 10f) * 2f));
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
                                    baseRigidBody.AddForce(vector12, ForceMode.Acceleration);
                                }
                                else
                                {
                                    baseRigidBody.AddForce((baseTransform.forward * vector12.magnitude), ForceMode.Acceleration);
                                }
                                flag2 = true;
                            }
                        }
                        if ((baseAnimation.IsPlaying("air_fall") && (currentSpeed < 0.2f)) && IsFrontGrounded())
                        {
                            CrossFade("onWall", 0.3f);
                        }
                    }
                    spinning = false;
                    if (flag3 && flag4)
                    {
                        float num14 = currentSpeed + 0.1f;
                        AddRightForce();
                        Vector3 vector13 = (((bulletRight.transform.position + bulletLeft.transform.position) * 0.5f)) - baseTransform.position;
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
                            num15 = Input.GetAxis("Mouse ScrollWheel") * 5555f;
                        }
                        num15 = Mathf.Clamp(num15, -0.8f, 0.8f);
                        float num16 = 1f + num15;
                        Vector3 vector14 = Vector3.RotateTowards(vector13, baseRigidBody.velocity, 1.53938f * num16, 1.53938f * num16);
                        vector14.Normalize();
                        spinning = true;
                        baseRigidBody.velocity = (vector14 * num14);
                    }
                    else if (flag3)
                    {
                        float num17 = currentSpeed + 0.1f;
                        AddRightForce();
                        Vector3 vector15 = bulletLeft.transform.position - baseTransform.position;
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
                            num18 = Input.GetAxis("Mouse ScrollWheel") * 5555f;
                        }
                        num18 = Mathf.Clamp(num18, -0.8f, 0.8f);
                        float num19 = 1f + num18;
                        Vector3 vector16 = Vector3.RotateTowards(vector15, baseRigidBody.velocity, 1.53938f * num19, 1.53938f * num19);
                        vector16.Normalize();
                        spinning = true;
                        baseRigidBody.velocity = (vector16 * num17);
                    }
                    else if (flag4)
                    {
                        float num20 = currentSpeed + 0.1f;
                        AddRightForce();
                        Vector3 vector17 = bulletRight.transform.position - baseTransform.position;
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
                            num21 = Input.GetAxis("Mouse ScrollWheel") * 5555f;
                        }
                        num21 = Mathf.Clamp(num21, -0.8f, 0.8f);
                        float num22 = 1f + num21;
                        Vector3 vector18 = Vector3.RotateTowards(vector17, baseRigidBody.velocity, 1.53938f * num22, 1.53938f * num22);
                        vector18.Normalize();
                        spinning = true;
                        baseRigidBody.velocity = (vector18 * num20);
                    }
                    if (((State == HERO_STATE.Attack) && ((attackAnimation == "attack5") || (attackAnimation == "special_petra"))) && ((baseAnimation[attackAnimation].normalizedTime > 0.4f) && !attackMove))
                    {
                        attackMove = true;
                        if (launchPointRight.magnitude > 0f)
                        {
                            Vector3 vector19 = launchPointRight - baseTransform.position;
                            vector19.Normalize();
                            vector19 = (vector19 * 13f);
                            baseRigidBody.AddForce(vector19, ForceMode.Impulse);
                        }
                        if ((attackAnimation == "special_petra") && (launchPointLeft.magnitude > 0f))
                        {
                            Vector3 vector20 = launchPointLeft - baseTransform.position;
                            vector20.Normalize();
                            vector20 = (vector20 * 13f);
                            baseRigidBody.AddForce(vector20, ForceMode.Impulse);
                            if (bulletRight != null)
                            {
                                bulletRight.GetComponent<Bullet>().disable();
                                releaseIfIHookSb();
                            }
                            if (bulletLeft != null)
                            {
                                bulletLeft.GetComponent<Bullet>().disable();
                                releaseIfIHookSb();
                            }
                        }
                        baseRigidBody.AddForce((Vector3.up * 2f), ForceMode.Impulse);
                    }
                    bool flag7 = false;
                    if ((bulletLeft != null) || (bulletRight != null))
                    {
                        if (((bulletLeft != null) && (bulletLeft.transform.position.y > gameObject.transform.position.y)) && (isLaunchLeft && bulletLeft.GetComponent<Bullet>().isHooked()))
                        {
                            flag7 = true;
                        }
                        if (((bulletRight != null) && (bulletRight.transform.position.y > gameObject.transform.position.y)) && (isLaunchRight && bulletRight.GetComponent<Bullet>().isHooked()))
                        {
                            flag7 = true;
                        }
                    }
                    if (flag7)
                    {
                        baseRigidBody.AddForce(new Vector3(0f, -10f * baseRigidBody.mass, 0f));
                    }
                    else
                    {
                        baseRigidBody.AddForce(new Vector3(0f, -gravity * baseRigidBody.mass, 0f));
                    }
                    if (currentSpeed > 10f)
                    {
                        currentCamera.GetComponent<Camera>().fieldOfView = Mathf.Lerp(currentCamera.GetComponent<Camera>().fieldOfView, Mathf.Min((float)100f, (float)(currentSpeed + 40f)), 0.1f);
                    }
                    else
                    {
                        currentCamera.GetComponent<Camera>().fieldOfView = Mathf.Lerp(currentCamera.GetComponent<Camera>().fieldOfView, 50f, 0.1f);
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
                    if (currentSpeed > 80f)
                    {
                        //if (!speedFXPS.enableEmission)
                        //{
                        //    speedFXPS.enableEmission = true;
                        //}
                        //speedFXPS.startSpeed = currentSpeed;
                        //speedFX.transform.LookAt(baseTransform.position + baseRigidBody.velocity);
                    }
                    //else if (speedFXPS.enableEmission)
                    //{
                    //    speedFXPS.enableEmission = false;
                    //}
                }
            }
        }
    }

    public string GetDebugInfo()
    {
        string str = "\n";
        str = "Left:" + isLeftHandHooked + " ";
        if (isLeftHandHooked && (bulletLeft != null))
        {
            Vector3 vector = bulletLeft.transform.position - transform.position;
            str = str + ((int)(Mathf.Atan2(vector.x, vector.z) * Mathf.Rad2Deg));
        }
        string str2 = str;
        object[] objArray1 = new object[] { str2, "\nRight:", isRightHandHooked, " " };
        str = string.Concat(objArray1);
        if (isRightHandHooked && (bulletRight != null))
        {
            Vector3 vector2 = bulletRight.transform.position - transform.position;
            str = str + ((int)(Mathf.Atan2(vector2.x, vector2.z) * Mathf.Rad2Deg));
        }
        str = (((str + "\nfacingDirection:" + ((int)facingDirection)) + "\nActual facingDirection:" + ((int)transform.rotation.eulerAngles.y)) + "\nState:" + State.ToString()) + "\n\n\n\n\n";
        if (State == HERO_STATE.Attack)
        {
            targetRotation = Quaternion.Euler(0f, facingDirection, 0f);
        }
        return str;
    }

    private Vector3 GetGlobalFacingVector3(float resultAngle)
    {
        float num = -resultAngle + 90f;
        float x = Mathf.Cos(num * 0.01745329f);
        return new Vector3(x, 0f, Mathf.Sin(num * 0.01745329f));
    }

    private Vector3 GetGlobalFacingVector3(float horizontal, float vertical)
    {
        float num = -GetGlobalFacingDirection(horizontal, vertical) + 90f;
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

    private float GetLeanAngle(Vector3 p, bool left)
    {
        if (!useGun && (State == HERO_STATE.Attack))
        {
            return 0f;
        }
        float num = p.y - transform.position.y;
        float num2 = Vector3.Distance(p, transform.position);
        float a = Mathf.Acos(num / num2) * Mathf.Rad2Deg;
        a *= 0.1f;
        a *= 1f + Mathf.Pow(baseRigidBody.velocity.magnitude, 0.2f);
        Vector3 vector3 = p - transform.position;
        float current = Mathf.Atan2(vector3.x, vector3.z) * Mathf.Rad2Deg;
        float target = Mathf.Atan2(baseRigidBody.velocity.x, baseRigidBody.velocity.z) * Mathf.Rad2Deg;
        float num6 = Mathf.DeltaAngle(current, target);
        a += Mathf.Abs((float)(num6 * 0.5f));
        if (State != HERO_STATE.Attack)
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
            return (a * ((num6 >= 0f) ? ((float)1) : ((float)(-1))));
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
        PlayAnimation("horse_getoff");
        baseRigidBody.AddForce((((Vector3.up * 10f) - (transform.forward * 2f)) - (transform.right * 1f)), ForceMode.VelocityChange);
        Unmounted();
    }

    private void GetOnHorse()
    {
        PlayAnimation("horse_geton");
        facingDirection = myHorse.transform.rotation.eulerAngles.y;
        targetRotation = Quaternion.Euler(0f, facingDirection, 0f);
    }

    public void GetSupply()
    {
        if (((baseAnimation.IsPlaying(standAnimation) || baseAnimation.IsPlaying("run_1")) || baseAnimation.IsPlaying("run_sasha")) && (((currentBladeSta != totalBladeSta) || (currentBladeNum != totalBladeNum)) || (((currentGas != totalGas) || (leftBulletLeft != bulletMAX)) || (rightBulletLeft != bulletMAX))))
        {
            State = HERO_STATE.FillGas;
            CrossFade("supply", 0.1f);
        }
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
        if (titanForm && (eren_titan != null))
        {
            eren_titan.GetComponent<ErenTitan>().lifeTime = 0.1f;
        }
        if (!useGun && photonView.isMine)
        {
            //leftbladetrail.Deactivate();
            //rightbladetrail.Deactivate();
            //leftbladetrail2.Deactivate();
            //rightbladetrail2.Deactivate();
        }
        smoke_3dmgEmission.enabled = false;
        sparksEmission.enabled = false;
    }

    public bool HasDied()
    {
        return (hasDied || IsInvincible());
    }

    private void HeadMovement()
    {
        Transform head = transform.Find("Amarture/Controller_Body/hip/spine/chest/neck/head");
        Transform neck = transform.Find("Amarture/Controller_Body/hip/spine/chest/neck");
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
        releaseIfIHookSb();
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
            baseRigidBody.AddForce((Vector3.up * Mathf.Min((float)(launchForce.magnitude * 0.2f), (float)10f)), ForceMode.Impulse);
        }
        baseRigidBody.AddForce(((launchForce * num) * 0.1f), ForceMode.Impulse);
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

    private bool IsFrontGrounded()
    {
        LayerMask mask = ((int)1) << LayerMask.NameToLayer("Ground");
        LayerMask mask2 = ((int)1) << LayerMask.NameToLayer("EnemyBox");
        LayerMask mask3 = mask2 | mask;
        return Physics.Raycast(gameObject.transform.position + ((gameObject.transform.up * 1f)), gameObject.transform.forward, (float)1f, mask3.value);
    }

    public bool IsGrounded()
    {
        LayerMask mask = ((int)1) << LayerMask.NameToLayer("Ground");
        LayerMask mask2 = ((int)1) << LayerMask.NameToLayer("EnemyBox");
        LayerMask mask3 = mask2 | mask;
        return Physics.Raycast(gameObject.transform.position + ((Vector3.up * 0.1f)), -Vector3.up, (float)0.3f, mask3.value);
    }

    public bool IsInvincible()
    {
        return (invincible > 0f);
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
        LayerMask mask = ((int)1) << LayerMask.NameToLayer("Ground");
        LayerMask mask2 = ((int)1) << LayerMask.NameToLayer("EnemyBox");
        LayerMask mask3 = mask2 | mask;
        return Physics.Raycast(gameObject.transform.position + ((gameObject.transform.up * 3f)), gameObject.transform.forward, (float)1.2f, mask3.value);
    }

    [PunRPC]
    private void KillObject()
    {
        Destroy(gameObject);
    }

    public void LateUpdate()
    {
        if ((myNetWorkName != null))
        {
            if (titanForm && (eren_titan != null))
            {
                myNetWorkName.transform.localPosition = ((Vector3.up * Screen.height) * 2f);
            }
            Vector3 start = new Vector3(baseTransform.position.x, baseTransform.position.y + 2f, baseTransform.position.z);
            GameObject mainCam = maincamera;
            LayerMask mask = ((int)1) << LayerMask.NameToLayer("Ground");
            LayerMask mask2 = ((int)1) << LayerMask.NameToLayer("EnemyBox");
            LayerMask mask3 = mask2 | mask;
            if ((Vector3.Angle(mainCam.transform.forward, start - mainCam.transform.position) > 90f) || Physics.Linecast(start, mainCam.transform.position, (int)mask3))
            {
                myNetWorkName.transform.localPosition = ((Vector3.up * Screen.height) * 2f);
            }
            else
            {
                Vector2 vector2 = mainCam.GetComponent<Camera>().WorldToScreenPoint(start);
                myNetWorkName.transform.localPosition = new Vector3((float)((int)(vector2.x - (Screen.width * 0.5f))), (float)((int)(vector2.y - (Screen.height * 0.5f))), 0f);
            }
        }
        if (!titanForm && !isCannon)
        {
            if (InputManager.Settings.CameraTilt && (photonView.isMine))
            {
                Quaternion quaternion2;
                Vector3 zero = Vector3.zero;
                Vector3 position = Vector3.zero;
                if ((isLaunchLeft && (bulletLeft != null)) && bulletLeft.GetComponent<Bullet>().isHooked())
                {
                    zero = bulletLeft.transform.position;
                }
                if ((isLaunchRight && (bulletRight != null)) && bulletRight.GetComponent<Bullet>().isHooked())
                {
                    position = bulletRight.transform.position;
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
                Vector3 from = Vector3.Project(vector5 - baseTransform.position, maincamera.transform.up);
                Vector3 vector7 = Vector3.Project(vector5 - baseTransform.position, maincamera.transform.right);
                if (vector5.magnitude > 0f)
                {
                    Vector3 to = from + vector7;
                    float num = Vector3.Angle(vector5 - baseTransform.position, baseRigidBody.velocity) * 0.005f;
                    Vector3 vector9 = maincamera.transform.right + vector7.normalized;
                    quaternion2 = Quaternion.Euler(maincamera.transform.rotation.eulerAngles.x, maincamera.transform.rotation.eulerAngles.y, (vector9.magnitude >= 1f) ? (-Vector3.Angle(from, to) * num) : (Vector3.Angle(from, to) * num));
                }
                else
                {
                    quaternion2 = Quaternion.Euler(maincamera.transform.rotation.eulerAngles.x, maincamera.transform.rotation.eulerAngles.y, 0f);
                }
                maincamera.transform.rotation = Quaternion.Lerp(maincamera.transform.rotation, quaternion2, Time.deltaTime * 2f);
            }
            if ((State == HERO_STATE.Grab) && (titanWhoGrabMe != null))
            {
                if (titanWhoGrabMe.GetComponent<MindlessTitan>() != null)
                {
                    baseTransform.position = titanWhoGrabMe.GetComponent<MindlessTitan>().grabTF.transform.position;
                    baseTransform.rotation = titanWhoGrabMe.GetComponent<MindlessTitan>().grabTF.transform.rotation;
                }
                else if (titanWhoGrabMe.GetComponent<FemaleTitan>() != null)
                {
                    baseTransform.position = titanWhoGrabMe.GetComponent<FemaleTitan>().grabTF.transform.position;
                    baseTransform.rotation = titanWhoGrabMe.GetComponent<FemaleTitan>().grabTF.transform.rotation;
                }
            }
            if (useGun)
            {
                if (leftArmAim || rightArmAim)
                {
                    Vector3 vector10 = gunTarget - baseTransform.position;
                    float current = -Mathf.Atan2(vector10.z, vector10.x) * Mathf.Rad2Deg;
                    float num3 = -Mathf.DeltaAngle(current, baseTransform.rotation.eulerAngles.y - 90f);
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
                if (isLeftHandHooked && (bulletLeft != null))
                {
                    LeftArmAimTo(bulletLeft.transform.position);
                }
                if (isRightHandHooked && (bulletRight != null))
                {
                    RightArmAimTo(bulletRight.transform.position);
                }
            }
            SetHookedPplDirection();
            BodyLean();
        }
    }

    public void Launch(Vector3 des, bool left = true, bool leviMode = false)
    {
        if (isMounted)
        {
            Unmounted();
        }
        if (State != HERO_STATE.Attack)
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
        if (((bulletLeft != null) && (bulletRight != null)) && (bulletLeft.GetComponent<Bullet>().isHooked() && bulletRight.GetComponent<Bullet>().isHooked()))
        {
            vector = (vector * 0.8f);
        }
        if (!baseAnimation.IsPlaying("attack5") && !baseAnimation.IsPlaying("special_petra"))
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
                CrossFade("AHSS_hook_forward_both", 0.1f);
            }
            else if (left && !isRightHandHooked)
            {
                CrossFade("air_hook_l_just", 0.1f);
            }
            else if (!left && !isLeftHandHooked)
            {
                CrossFade("air_hook_r_just", 0.1f);
            }
            else
            {
                CrossFade("dash", 0.1f);
                baseAnimation["dash"].time = 0f;
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
            baseRigidBody.AddForce(launchForce);
        }
        facingDirection = Mathf.Atan2(launchForce.x, launchForce.z) * Mathf.Rad2Deg;
        Quaternion quaternion = Quaternion.Euler(0f, facingDirection, 0f);
        gameObject.transform.rotation = quaternion;
        baseRigidBody.rotation = quaternion;
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
        if (baseAnimation.IsPlaying("special_petra"))
        {
            launchElapsedTimeR = -100f;
            launchElapsedTimeL = -100f;
            if (bulletRight != null)
            {
                bulletRight.GetComponent<Bullet>().disable();
                releaseIfIHookSb();
            }
            if (bulletLeft != null)
            {
                bulletLeft.GetComponent<Bullet>().disable();
                releaseIfIHookSb();
            }
        }
        sparksEmission.enabled = false;
    }

    private void LaunchLeftRope(float distance, Vector3 point, bool single, int mode = 0)
    {
        if (currentGas != 0f)
        {
            UseGas(0f);
            bulletLeft = PhotonNetwork.Instantiate("hook", transform.position, transform.rotation, 0);
            GameObject obj2 = !useGun ? hookRefL1 : hookRefL2;
            string str = !useGun ? "hookRefL1" : "hookRefL2";
            bulletLeft.transform.position = obj2.transform.position;
            Bullet component = bulletLeft.GetComponent<Bullet>();
            float num = !single ? ((distance <= 50f) ? (distance * 0.05f) : (distance * 0.3f)) : 0f;
            Vector3 vector = (point - ( (transform.right * num))) - bulletLeft.transform.position;
            vector.Normalize();
            if (mode == 1)
            {
                component.launch( (vector * 3f), baseRigidBody.velocity, str, true, gameObject, true);
            }
            else
            {
                component.launch( (vector * 3f), baseRigidBody.velocity, str, true, gameObject, false);
            }
            launchPointLeft = Vector3.zero;
        }
    }

    private void LaunchRightRope(float distance, Vector3 point, bool single, int mode = 0)
    {
        if (currentGas != 0f)
        {
            UseGas(0f);
            bulletRight = PhotonNetwork.Instantiate("hook", transform.position, transform.rotation, 0);
            GameObject obj2 = !useGun ? hookRefR1 : hookRefR2;
            string str = !useGun ? "hookRefR1" : "hookRefR2";
            bulletRight.transform.position = obj2.transform.position;
            Bullet component = bulletRight.GetComponent<Bullet>();
            float num = !single ? ((distance <= 50f) ? (distance * 0.05f) : (distance * 0.3f)) : 0f;
            Vector3 vector = (point + ((transform.right * num))) - bulletRight.transform.position;
            vector.Normalize();
            if (mode == 1)
            {
                component.launch((vector * 5f), baseRigidBody.velocity, str, false, gameObject, true);
            }
            else
            {
                component.launch((vector * 3f), baseRigidBody.velocity, str, false, gameObject, false);
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

    public void LoadSkin()
    {
        if (photonView.isMine)
        {
            if (((int)FengGameManagerMKII.settings[0x5d]) == 1)
            {
                foreach (Renderer renderer in GetComponentsInChildren<Renderer>())
                {
                    if (renderer.name.Contains("speed"))
                    {
                        renderer.enabled = false;
                    }
                }
            }
            if (((int)FengGameManagerMKII.settings[0]) == 1)
            {
                int index = 14;
                int num3 = 4;
                int num4 = 5;
                int num5 = 6;
                int num6 = 7;
                int num7 = 8;
                int num8 = 9;
                int num9 = 10;
                int num10 = 11;
                int num11 = 12;
                int num12 = 13;
                int num13 = 3;
                int num14 = 0x5e;
                if (((int)FengGameManagerMKII.settings[0x85]) == 1)
                {
                    num13 = 0x86;
                    num3 = 0x87;
                    num4 = 0x88;
                    num5 = 0x89;
                    num6 = 0x8a;
                    num7 = 0x8b;
                    num8 = 140;
                    num9 = 0x8d;
                    num10 = 0x8e;
                    num11 = 0x8f;
                    num12 = 0x90;
                    index = 0x91;
                    num14 = 0x92;
                }
                else if (((int)FengGameManagerMKII.settings[0x85]) == 2)
                {
                    num13 = 0x93;
                    num3 = 0x94;
                    num4 = 0x95;
                    num5 = 150;
                    num6 = 0x97;
                    num7 = 0x98;
                    num8 = 0x99;
                    num9 = 0x9a;
                    num10 = 0x9b;
                    num11 = 0x9c;
                    num12 = 0x9d;
                    index = 0x9e;
                    num14 = 0x9f;
                }
                string str = (string)FengGameManagerMKII.settings[index];
                string str2 = (string)FengGameManagerMKII.settings[num3];
                string str3 = (string)FengGameManagerMKII.settings[num4];
                string str4 = (string)FengGameManagerMKII.settings[num5];
                string str5 = (string)FengGameManagerMKII.settings[num6];
                string str6 = (string)FengGameManagerMKII.settings[num7];
                string str7 = (string)FengGameManagerMKII.settings[num8];
                string str8 = (string)FengGameManagerMKII.settings[num9];
                string str9 = (string)FengGameManagerMKII.settings[num10];
                string str10 = (string)FengGameManagerMKII.settings[num11];
                string str11 = (string)FengGameManagerMKII.settings[num12];
                string str12 = (string)FengGameManagerMKII.settings[num13];
                string str13 = (string)FengGameManagerMKII.settings[num14];
                string url = str12 + "," + str2 + "," + str3 + "," + str4 + "," + str5 + "," + str6 + "," + str7 + "," + str8 + "," + str9 + "," + str10 + "," + str11 + "," + str + "," + str13;
                int viewID = -1;
                if (myHorse)
                {
                    viewID = myHorse.photonView.viewID;
                }
                photonView.RPC(nameof(LoadSkinRPC), PhotonTargets.AllBuffered, new object[] { viewID, url });
            }
        }
    }

    public IEnumerator LoadSkinE(int horse, string url)
    {
        while (!hasspawn)
        {
            yield return null;
        }
        bool mipmap = true;
        bool iteratorVariable1 = false;
        if (((int)FengGameManagerMKII.settings[0x3f]) == 1)
        {
            mipmap = false;
        }
        string[] iteratorVariable2 = url.Split(new char[] { ',' });
        bool iteratorVariable3 = false;
        if (((int)FengGameManagerMKII.settings[15]) == 0)
        {
            iteratorVariable3 = true;
        }
        bool iteratorVariable4 = false;
        if (GameSettings.Horse.Enabled.Value)
        {
            iteratorVariable4 = true;
        }
        bool iteratorVariable5 = false;
        if (photonView.isMine)
        {
            iteratorVariable5 = true;
        }
        if (setup.part_hair_1 != null)
        {
            Renderer renderer = setup.part_hair_1.GetComponent<Renderer>();
            if ((iteratorVariable2[1].EndsWith(".jpg") || iteratorVariable2[1].EndsWith(".png")) || iteratorVariable2[1].EndsWith(".jpeg"))
            {
                if (!FengGameManagerMKII.linkHash[0].ContainsKey(iteratorVariable2[1]))
                {
                    WWW link = new WWW(iteratorVariable2[1]);
                    yield return link;
                    Texture2D iteratorVariable8 = RCextensions.loadimage(link, mipmap, 0x30d40);
                    link.Dispose();
                    if (!FengGameManagerMKII.linkHash[0].ContainsKey(iteratorVariable2[1]))
                    {
                        iteratorVariable1 = true;
                        if (setup.myCostume.hairInfo.id >= 0)
                        {
                            renderer.material = CharacterMaterials.materials[setup.myCostume.hairInfo.texture];
                        }
                        renderer.material.mainTexture = iteratorVariable8;
                        FengGameManagerMKII.linkHash[0].Add(iteratorVariable2[1], renderer.material);
                        renderer.material = (Material)FengGameManagerMKII.linkHash[0][iteratorVariable2[1]];
                    }
                    else
                    {
                        renderer.material = (Material)FengGameManagerMKII.linkHash[0][iteratorVariable2[1]];
                    }
                }
                else
                {
                    renderer.material = (Material)FengGameManagerMKII.linkHash[0][iteratorVariable2[1]];
                }
            }
            else if (iteratorVariable2[1].ToLower() == "transparent")
            {
                renderer.enabled = false;
            }
        }
        if (setup.part_cape != null)
        {
            Renderer iteratorVariable9 = setup.part_cape.GetComponent<Renderer>();
            if ((iteratorVariable2[7].EndsWith(".jpg") || iteratorVariable2[7].EndsWith(".png")) || iteratorVariable2[7].EndsWith(".jpeg"))
            {
                if (!FengGameManagerMKII.linkHash[0].ContainsKey(iteratorVariable2[7]))
                {
                    WWW iteratorVariable10 = new WWW(iteratorVariable2[7]);
                    yield return iteratorVariable10;
                    Texture2D iteratorVariable11 = RCextensions.loadimage(iteratorVariable10, mipmap, 0x30d40);
                    iteratorVariable10.Dispose();
                    if (!FengGameManagerMKII.linkHash[0].ContainsKey(iteratorVariable2[7]))
                    {
                        iteratorVariable1 = true;
                        iteratorVariable9.material.mainTexture = iteratorVariable11;
                        FengGameManagerMKII.linkHash[0].Add(iteratorVariable2[7], iteratorVariable9.material);
                        iteratorVariable9.material = (Material)FengGameManagerMKII.linkHash[0][iteratorVariable2[7]];
                    }
                    else
                    {
                        iteratorVariable9.material = (Material)FengGameManagerMKII.linkHash[0][iteratorVariable2[7]];
                    }
                }
                else
                {
                    iteratorVariable9.material = (Material)FengGameManagerMKII.linkHash[0][iteratorVariable2[7]];
                }
            }
            else if (iteratorVariable2[7].ToLower() == "transparent")
            {
                iteratorVariable9.enabled = false;
            }
        }
        if (setup.part_chest_3 != null)
        {
            Renderer iteratorVariable12 = setup.part_chest_3.GetComponent<Renderer>();
            if ((iteratorVariable2[6].EndsWith(".jpg") || iteratorVariable2[6].EndsWith(".png")) || iteratorVariable2[6].EndsWith(".jpeg"))
            {
                if (!FengGameManagerMKII.linkHash[1].ContainsKey(iteratorVariable2[6]))
                {
                    WWW iteratorVariable13 = new WWW(iteratorVariable2[6]);
                    yield return iteratorVariable13;
                    Texture2D iteratorVariable14 = RCextensions.loadimage(iteratorVariable13, mipmap, 0x7a120);
                    iteratorVariable13.Dispose();
                    if (!FengGameManagerMKII.linkHash[1].ContainsKey(iteratorVariable2[6]))
                    {
                        iteratorVariable1 = true;
                        iteratorVariable12.material.mainTexture = iteratorVariable14;
                        FengGameManagerMKII.linkHash[1].Add(iteratorVariable2[6], iteratorVariable12.material);
                        iteratorVariable12.material = (Material)FengGameManagerMKII.linkHash[1][iteratorVariable2[6]];
                    }
                    else
                    {
                        iteratorVariable12.material = (Material)FengGameManagerMKII.linkHash[1][iteratorVariable2[6]];
                    }
                }
                else
                {
                    iteratorVariable12.material = (Material)FengGameManagerMKII.linkHash[1][iteratorVariable2[6]];
                }
            }
            else if (iteratorVariable2[6].ToLower() == "transparent")
            {
                iteratorVariable12.enabled = false;
            }
        }
        foreach (Renderer iteratorVariable15 in GetComponentsInChildren<Renderer>())
        {
            if (iteratorVariable15.name.Contains(FengGameManagerMKII.s[1]))
            {
                if ((iteratorVariable2[1].EndsWith(".jpg") || iteratorVariable2[1].EndsWith(".png")) || iteratorVariable2[1].EndsWith(".jpeg"))
                {
                    if (!FengGameManagerMKII.linkHash[0].ContainsKey(iteratorVariable2[1]))
                    {
                        WWW iteratorVariable16 = new WWW(iteratorVariable2[1]);
                        yield return iteratorVariable16;
                        Texture2D iteratorVariable17 = RCextensions.loadimage(iteratorVariable16, mipmap, 0x30d40);
                        iteratorVariable16.Dispose();
                        if (!FengGameManagerMKII.linkHash[0].ContainsKey(iteratorVariable2[1]))
                        {
                            iteratorVariable1 = true;
                            if (setup.myCostume.hairInfo.id >= 0)
                            {
                                iteratorVariable15.material = CharacterMaterials.materials[setup.myCostume.hairInfo.texture];
                            }
                            iteratorVariable15.material.mainTexture = iteratorVariable17;
                            FengGameManagerMKII.linkHash[0].Add(iteratorVariable2[1], iteratorVariable15.material);
                            iteratorVariable15.material = (Material)FengGameManagerMKII.linkHash[0][iteratorVariable2[1]];
                        }
                        else
                        {
                            iteratorVariable15.material = (Material)FengGameManagerMKII.linkHash[0][iteratorVariable2[1]];
                        }
                    }
                    else
                    {
                        iteratorVariable15.material = (Material)FengGameManagerMKII.linkHash[0][iteratorVariable2[1]];
                    }
                }
                else if (iteratorVariable2[1].ToLower() == "transparent")
                {
                    iteratorVariable15.enabled = false;
                }
            }
            else if (iteratorVariable15.name.Contains(FengGameManagerMKII.s[2]))
            {
                if ((iteratorVariable2[2].EndsWith(".jpg") || iteratorVariable2[2].EndsWith(".png")) || iteratorVariable2[2].EndsWith(".jpeg"))
                {
                    if (!FengGameManagerMKII.linkHash[0].ContainsKey(iteratorVariable2[2]))
                    {
                        WWW iteratorVariable18 = new WWW(iteratorVariable2[2]);
                        yield return iteratorVariable18;
                        Texture2D iteratorVariable19 = RCextensions.loadimage(iteratorVariable18, mipmap, 0x30d40);
                        iteratorVariable18.Dispose();
                        if (!FengGameManagerMKII.linkHash[0].ContainsKey(iteratorVariable2[2]))
                        {
                            iteratorVariable1 = true;
                            iteratorVariable15.material.mainTextureScale = (Vector2)(iteratorVariable15.material.mainTextureScale * 8f);
                            iteratorVariable15.material.mainTextureOffset = new Vector2(0f, 0f);
                            iteratorVariable15.material.mainTexture = iteratorVariable19;
                            FengGameManagerMKII.linkHash[0].Add(iteratorVariable2[2], iteratorVariable15.material);
                            iteratorVariable15.material = (Material)FengGameManagerMKII.linkHash[0][iteratorVariable2[2]];
                        }
                        else
                        {
                            iteratorVariable15.material = (Material)FengGameManagerMKII.linkHash[0][iteratorVariable2[2]];
                        }
                    }
                    else
                    {
                        iteratorVariable15.material = (Material)FengGameManagerMKII.linkHash[0][iteratorVariable2[2]];
                    }
                }
                else if (iteratorVariable2[2].ToLower() == "transparent")
                {
                    iteratorVariable15.enabled = false;
                }
            }
            else if (iteratorVariable15.name.Contains(FengGameManagerMKII.s[3]))
            {
                if ((iteratorVariable2[3].EndsWith(".jpg") || iteratorVariable2[3].EndsWith(".png")) || iteratorVariable2[3].EndsWith(".jpeg"))
                {
                    if (!FengGameManagerMKII.linkHash[0].ContainsKey(iteratorVariable2[3]))
                    {
                        WWW iteratorVariable20 = new WWW(iteratorVariable2[3]);
                        yield return iteratorVariable20;
                        Texture2D iteratorVariable21 = RCextensions.loadimage(iteratorVariable20, mipmap, 0x30d40);
                        iteratorVariable20.Dispose();
                        if (!FengGameManagerMKII.linkHash[0].ContainsKey(iteratorVariable2[3]))
                        {
                            iteratorVariable1 = true;
                            iteratorVariable15.material.mainTextureScale = (Vector2)(iteratorVariable15.material.mainTextureScale * 8f);
                            iteratorVariable15.material.mainTextureOffset = new Vector2(0f, 0f);
                            iteratorVariable15.material.mainTexture = iteratorVariable21;
                            FengGameManagerMKII.linkHash[0].Add(iteratorVariable2[3], iteratorVariable15.material);
                            iteratorVariable15.material = (Material)FengGameManagerMKII.linkHash[0][iteratorVariable2[3]];
                        }
                        else
                        {
                            iteratorVariable15.material = (Material)FengGameManagerMKII.linkHash[0][iteratorVariable2[3]];
                        }
                    }
                    else
                    {
                        iteratorVariable15.material = (Material)FengGameManagerMKII.linkHash[0][iteratorVariable2[3]];
                    }
                }
                else if (iteratorVariable2[3].ToLower() == "transparent")
                {
                    iteratorVariable15.enabled = false;
                }
            }
            else if (iteratorVariable15.name.Contains(FengGameManagerMKII.s[4]))
            {
                if ((iteratorVariable2[4].EndsWith(".jpg") || iteratorVariable2[4].EndsWith(".png")) || iteratorVariable2[4].EndsWith(".jpeg"))
                {
                    if (!FengGameManagerMKII.linkHash[0].ContainsKey(iteratorVariable2[4]))
                    {
                        WWW iteratorVariable22 = new WWW(iteratorVariable2[4]);
                        yield return iteratorVariable22;
                        Texture2D iteratorVariable23 = RCextensions.loadimage(iteratorVariable22, mipmap, 0x30d40);
                        iteratorVariable22.Dispose();
                        if (!FengGameManagerMKII.linkHash[0].ContainsKey(iteratorVariable2[4]))
                        {
                            iteratorVariable1 = true;
                            iteratorVariable15.material.mainTextureScale = (Vector2)(iteratorVariable15.material.mainTextureScale * 8f);
                            iteratorVariable15.material.mainTextureOffset = new Vector2(0f, 0f);
                            iteratorVariable15.material.mainTexture = iteratorVariable23;
                            FengGameManagerMKII.linkHash[0].Add(iteratorVariable2[4], iteratorVariable15.material);
                            iteratorVariable15.material = (Material)FengGameManagerMKII.linkHash[0][iteratorVariable2[4]];
                        }
                        else
                        {
                            iteratorVariable15.material = (Material)FengGameManagerMKII.linkHash[0][iteratorVariable2[4]];
                        }
                    }
                    else
                    {
                        iteratorVariable15.material = (Material)FengGameManagerMKII.linkHash[0][iteratorVariable2[4]];
                    }
                }
                else if (iteratorVariable2[4].ToLower() == "transparent")
                {
                    iteratorVariable15.enabled = false;
                }
            }
            else if ((iteratorVariable15.name.Contains(FengGameManagerMKII.s[5]) || iteratorVariable15.name.Contains(FengGameManagerMKII.s[6])) || iteratorVariable15.name.Contains(FengGameManagerMKII.s[10]))
            {
                if ((iteratorVariable2[5].EndsWith(".jpg") || iteratorVariable2[5].EndsWith(".png")) || iteratorVariable2[5].EndsWith(".jpeg"))
                {
                    if (!FengGameManagerMKII.linkHash[0].ContainsKey(iteratorVariable2[5]))
                    {
                        WWW iteratorVariable24 = new WWW(iteratorVariable2[5]);
                        yield return iteratorVariable24;
                        Texture2D iteratorVariable25 = RCextensions.loadimage(iteratorVariable24, mipmap, 0x30d40);
                        iteratorVariable24.Dispose();
                        if (!FengGameManagerMKII.linkHash[0].ContainsKey(iteratorVariable2[5]))
                        {
                            iteratorVariable1 = true;
                            iteratorVariable15.material.mainTexture = iteratorVariable25;
                            FengGameManagerMKII.linkHash[0].Add(iteratorVariable2[5], iteratorVariable15.material);
                            iteratorVariable15.material = (Material)FengGameManagerMKII.linkHash[0][iteratorVariable2[5]];
                        }
                        else
                        {
                            iteratorVariable15.material = (Material)FengGameManagerMKII.linkHash[0][iteratorVariable2[5]];
                        }
                    }
                    else
                    {
                        iteratorVariable15.material = (Material)FengGameManagerMKII.linkHash[0][iteratorVariable2[5]];
                    }
                }
                else if (iteratorVariable2[5].ToLower() == "transparent")
                {
                    iteratorVariable15.enabled = false;
                }
            }
            else if (((iteratorVariable15.name.Contains(FengGameManagerMKII.s[7]) || iteratorVariable15.name.Contains(FengGameManagerMKII.s[8])) || iteratorVariable15.name.Contains(FengGameManagerMKII.s[9])) || iteratorVariable15.name.Contains(FengGameManagerMKII.s[0x18]))
            {
                if ((iteratorVariable2[6].EndsWith(".jpg") || iteratorVariable2[6].EndsWith(".png")) || iteratorVariable2[6].EndsWith(".jpeg"))
                {
                    if (!FengGameManagerMKII.linkHash[1].ContainsKey(iteratorVariable2[6]))
                    {
                        WWW iteratorVariable26 = new WWW(iteratorVariable2[6]);
                        yield return iteratorVariable26;
                        Texture2D iteratorVariable27 = RCextensions.loadimage(iteratorVariable26, mipmap, 0x7a120);
                        iteratorVariable26.Dispose();
                        if (!FengGameManagerMKII.linkHash[1].ContainsKey(iteratorVariable2[6]))
                        {
                            iteratorVariable1 = true;
                            iteratorVariable15.material.mainTexture = iteratorVariable27;
                            FengGameManagerMKII.linkHash[1].Add(iteratorVariable2[6], iteratorVariable15.material);
                            iteratorVariable15.material = (Material)FengGameManagerMKII.linkHash[1][iteratorVariable2[6]];
                        }
                        else
                        {
                            iteratorVariable15.material = (Material)FengGameManagerMKII.linkHash[1][iteratorVariable2[6]];
                        }
                    }
                    else
                    {
                        iteratorVariable15.material = (Material)FengGameManagerMKII.linkHash[1][iteratorVariable2[6]];
                    }
                }
                else if (iteratorVariable2[6].ToLower() == "transparent")
                {
                    iteratorVariable15.enabled = false;
                }
            }
            else if (iteratorVariable15.name.Contains(FengGameManagerMKII.s[11]) || iteratorVariable15.name.Contains(FengGameManagerMKII.s[12]))
            {
                if ((iteratorVariable2[7].EndsWith(".jpg") || iteratorVariable2[7].EndsWith(".png")) || iteratorVariable2[7].EndsWith(".jpeg"))
                {
                    if (!FengGameManagerMKII.linkHash[0].ContainsKey(iteratorVariable2[7]))
                    {
                        WWW iteratorVariable28 = new WWW(iteratorVariable2[7]);
                        yield return iteratorVariable28;
                        Texture2D iteratorVariable29 = RCextensions.loadimage(iteratorVariable28, mipmap, 0x30d40);
                        iteratorVariable28.Dispose();
                        if (!FengGameManagerMKII.linkHash[0].ContainsKey(iteratorVariable2[7]))
                        {
                            iteratorVariable1 = true;
                            iteratorVariable15.material.mainTexture = iteratorVariable29;
                            FengGameManagerMKII.linkHash[0].Add(iteratorVariable2[7], iteratorVariable15.material);
                            iteratorVariable15.material = (Material)FengGameManagerMKII.linkHash[0][iteratorVariable2[7]];
                        }
                        else
                        {
                            iteratorVariable15.material = (Material)FengGameManagerMKII.linkHash[0][iteratorVariable2[7]];
                        }
                    }
                    else
                    {
                        iteratorVariable15.material = (Material)FengGameManagerMKII.linkHash[0][iteratorVariable2[7]];
                    }
                }
                else if (iteratorVariable2[7].ToLower() == "transparent")
                {
                    iteratorVariable15.enabled = false;
                }
            }
            else if (iteratorVariable15.name.Contains(FengGameManagerMKII.s[15]) || ((iteratorVariable15.name.Contains(FengGameManagerMKII.s[13]) || iteratorVariable15.name.Contains(FengGameManagerMKII.s[0x1a])) && !iteratorVariable15.name.Contains("_r")))
            {
                if ((iteratorVariable2[8].EndsWith(".jpg") || iteratorVariable2[8].EndsWith(".png")) || iteratorVariable2[8].EndsWith(".jpeg"))
                {
                    if (!FengGameManagerMKII.linkHash[1].ContainsKey(iteratorVariable2[8]))
                    {
                        WWW iteratorVariable30 = new WWW(iteratorVariable2[8]);
                        yield return iteratorVariable30;
                        Texture2D iteratorVariable31 = RCextensions.loadimage(iteratorVariable30, mipmap, 0x7a120);
                        iteratorVariable30.Dispose();
                        if (!FengGameManagerMKII.linkHash[1].ContainsKey(iteratorVariable2[8]))
                        {
                            iteratorVariable1 = true;
                            iteratorVariable15.material.mainTexture = iteratorVariable31;
                            FengGameManagerMKII.linkHash[1].Add(iteratorVariable2[8], iteratorVariable15.material);
                            iteratorVariable15.material = (Material)FengGameManagerMKII.linkHash[1][iteratorVariable2[8]];
                        }
                        else
                        {
                            iteratorVariable15.material = (Material)FengGameManagerMKII.linkHash[1][iteratorVariable2[8]];
                        }
                    }
                    else
                    {
                        iteratorVariable15.material = (Material)FengGameManagerMKII.linkHash[1][iteratorVariable2[8]];
                    }
                }
                else if (iteratorVariable2[8].ToLower() == "transparent")
                {
                    iteratorVariable15.enabled = false;
                }
            }
            else if ((iteratorVariable15.name.Contains(FengGameManagerMKII.s[0x11]) || iteratorVariable15.name.Contains(FengGameManagerMKII.s[0x10])) || (iteratorVariable15.name.Contains(FengGameManagerMKII.s[0x1a]) && iteratorVariable15.name.Contains("_r")))
            {
                if ((iteratorVariable2[9].EndsWith(".jpg") || iteratorVariable2[9].EndsWith(".png")) || iteratorVariable2[9].EndsWith(".jpeg"))
                {
                    if (!FengGameManagerMKII.linkHash[1].ContainsKey(iteratorVariable2[9]))
                    {
                        WWW iteratorVariable32 = new WWW(iteratorVariable2[9]);
                        yield return iteratorVariable32;
                        Texture2D iteratorVariable33 = RCextensions.loadimage(iteratorVariable32, mipmap, 0x7a120);
                        iteratorVariable32.Dispose();
                        if (!FengGameManagerMKII.linkHash[1].ContainsKey(iteratorVariable2[9]))
                        {
                            iteratorVariable1 = true;
                            iteratorVariable15.material.mainTexture = iteratorVariable33;
                            FengGameManagerMKII.linkHash[1].Add(iteratorVariable2[9], iteratorVariable15.material);
                            iteratorVariable15.material = (Material)FengGameManagerMKII.linkHash[1][iteratorVariable2[9]];
                        }
                        else
                        {
                            iteratorVariable15.material = (Material)FengGameManagerMKII.linkHash[1][iteratorVariable2[9]];
                        }
                    }
                    else
                    {
                        iteratorVariable15.material = (Material)FengGameManagerMKII.linkHash[1][iteratorVariable2[9]];
                    }
                }
                else if (iteratorVariable2[9].ToLower() == "transparent")
                {
                    iteratorVariable15.enabled = false;
                }
            }
            else if ((iteratorVariable15.name == FengGameManagerMKII.s[0x12]) && iteratorVariable3)
            {
                if ((iteratorVariable2[10].EndsWith(".jpg") || iteratorVariable2[10].EndsWith(".png")) || iteratorVariable2[10].EndsWith(".jpeg"))
                {
                    if (!FengGameManagerMKII.linkHash[0].ContainsKey(iteratorVariable2[10]))
                    {
                        WWW iteratorVariable34 = new WWW(iteratorVariable2[10]);
                        yield return iteratorVariable34;
                        Texture2D iteratorVariable35 = RCextensions.loadimage(iteratorVariable34, mipmap, 0x30d40);
                        iteratorVariable34.Dispose();
                        if (!FengGameManagerMKII.linkHash[0].ContainsKey(iteratorVariable2[10]))
                        {
                            iteratorVariable1 = true;
                            iteratorVariable15.material.mainTexture = iteratorVariable35;
                            FengGameManagerMKII.linkHash[0].Add(iteratorVariable2[10], iteratorVariable15.material);
                            iteratorVariable15.material = (Material)FengGameManagerMKII.linkHash[0][iteratorVariable2[10]];
                        }
                        else
                        {
                            iteratorVariable15.material = (Material)FengGameManagerMKII.linkHash[0][iteratorVariable2[10]];
                        }
                    }
                    else
                    {
                        iteratorVariable15.material = (Material)FengGameManagerMKII.linkHash[0][iteratorVariable2[10]];
                    }
                }
                else if (iteratorVariable2[10].ToLower() == "transparent")
                {
                    iteratorVariable15.enabled = false;
                }
            }
            else if (iteratorVariable15.name.Contains(FengGameManagerMKII.s[0x19]))
            {
                if ((iteratorVariable2[11].EndsWith(".jpg") || iteratorVariable2[11].EndsWith(".png")) || iteratorVariable2[11].EndsWith(".jpeg"))
                {
                    if (!FengGameManagerMKII.linkHash[0].ContainsKey(iteratorVariable2[11]))
                    {
                        WWW iteratorVariable36 = new WWW(iteratorVariable2[11]);
                        yield return iteratorVariable36;
                        Texture2D iteratorVariable37 = RCextensions.loadimage(iteratorVariable36, mipmap, 0x30d40);
                        iteratorVariable36.Dispose();
                        if (!FengGameManagerMKII.linkHash[0].ContainsKey(iteratorVariable2[11]))
                        {
                            iteratorVariable1 = true;
                            iteratorVariable15.material.mainTexture = iteratorVariable37;
                            FengGameManagerMKII.linkHash[0].Add(iteratorVariable2[11], iteratorVariable15.material);
                            iteratorVariable15.material = (Material)FengGameManagerMKII.linkHash[0][iteratorVariable2[11]];
                        }
                        else
                        {
                            iteratorVariable15.material = (Material)FengGameManagerMKII.linkHash[0][iteratorVariable2[11]];
                        }
                    }
                    else
                    {
                        iteratorVariable15.material = (Material)FengGameManagerMKII.linkHash[0][iteratorVariable2[11]];
                    }
                }
                else if (iteratorVariable2[11].ToLower() == "transparent")
                {
                    iteratorVariable15.enabled = false;
                }
            }
        }
        if (iteratorVariable4 && (horse >= 0))
        {
            GameObject gameObject = PhotonView.Find(horse).gameObject;
            if (gameObject != null)
            {
                foreach (Renderer iteratorVariable39 in gameObject.GetComponentsInChildren<Renderer>())
                {
                    if (iteratorVariable39.name.Contains(FengGameManagerMKII.s[0x13]))
                    {
                        if ((iteratorVariable2[0].EndsWith(".jpg") || iteratorVariable2[0].EndsWith(".png")) || iteratorVariable2[0].EndsWith(".jpeg"))
                        {
                            if (!FengGameManagerMKII.linkHash[1].ContainsKey(iteratorVariable2[0]))
                            {
                                WWW iteratorVariable40 = new WWW(iteratorVariable2[0]);
                                yield return iteratorVariable40;
                                Texture2D iteratorVariable41 = RCextensions.loadimage(iteratorVariable40, mipmap, 0x7a120);
                                iteratorVariable40.Dispose();
                                if (!FengGameManagerMKII.linkHash[1].ContainsKey(iteratorVariable2[0]))
                                {
                                    iteratorVariable1 = true;
                                    iteratorVariable39.material.mainTexture = iteratorVariable41;
                                    FengGameManagerMKII.linkHash[1].Add(iteratorVariable2[0], iteratorVariable39.material);
                                    iteratorVariable39.material = (Material)FengGameManagerMKII.linkHash[1][iteratorVariable2[0]];
                                }
                                else
                                {
                                    iteratorVariable39.material = (Material)FengGameManagerMKII.linkHash[1][iteratorVariable2[0]];
                                }
                            }
                            else
                            {
                                iteratorVariable39.material = (Material)FengGameManagerMKII.linkHash[1][iteratorVariable2[0]];
                            }
                        }
                        else if (iteratorVariable2[0].ToLower() == "transparent")
                        {
                            iteratorVariable39.enabled = false;
                        }
                    }
                }
            }
        }
        if (iteratorVariable5 && ((iteratorVariable2[12].EndsWith(".jpg") || iteratorVariable2[12].EndsWith(".png")) || iteratorVariable2[12].EndsWith(".jpeg")))
        {
            if (!FengGameManagerMKII.linkHash[0].ContainsKey(iteratorVariable2[12]))
            {
                WWW iteratorVariable42 = new WWW(iteratorVariable2[12]);
                yield return iteratorVariable42;
                Texture2D iteratorVariable43 = RCextensions.loadimage(iteratorVariable42, mipmap, 0x30d40);
                iteratorVariable42.Dispose();
                if (!FengGameManagerMKII.linkHash[0].ContainsKey(iteratorVariable2[12]))
                {
                    iteratorVariable1 = true;
                    /*
                    leftbladetrail.MyMaterial.mainTexture = iteratorVariable43;
                    rightbladetrail.MyMaterial.mainTexture = iteratorVariable43;
                    FengGameManagerMKII.linkHash[0].Add(iteratorVariable2[12], leftbladetrail.MyMaterial);
                    leftbladetrail.MyMaterial = (Material)FengGameManagerMKII.linkHash[0][iteratorVariable2[12]];
                    rightbladetrail.MyMaterial = (Material)FengGameManagerMKII.linkHash[0][iteratorVariable2[12]];
                    leftbladetrail2.MyMaterial = leftbladetrail.MyMaterial;
                    rightbladetrail2.MyMaterial = leftbladetrail.MyMaterial;
                    */
                }
                else
                {
                    /*
                    leftbladetrail2.MyMaterial = (Material)FengGameManagerMKII.linkHash[0][iteratorVariable2[12]];
                    rightbladetrail2.MyMaterial = (Material)FengGameManagerMKII.linkHash[0][iteratorVariable2[12]];
                    leftbladetrail.MyMaterial = (Material)FengGameManagerMKII.linkHash[0][iteratorVariable2[12]];
                    rightbladetrail.MyMaterial = (Material)FengGameManagerMKII.linkHash[0][iteratorVariable2[12]];
                    */
                }
            }
            else
            {
                /*
                leftbladetrail2.MyMaterial = (Material)FengGameManagerMKII.linkHash[0][iteratorVariable2[12]];
                rightbladetrail2.MyMaterial = (Material)FengGameManagerMKII.linkHash[0][iteratorVariable2[12]];
                leftbladetrail.MyMaterial = (Material)FengGameManagerMKII.linkHash[0][iteratorVariable2[12]];
                rightbladetrail.MyMaterial = (Material)FengGameManagerMKII.linkHash[0][iteratorVariable2[12]];
                */  
            }
        }
        if (iteratorVariable1)
        {
            FengGameManagerMKII.instance.unloadAssets();
        }
    }

    [PunRPC]
    public void LoadSkinRPC(int horse, string url)
    {
        if (((int)FengGameManagerMKII.settings[0]) == 1)
        {
            StartCoroutine(LoadSkinE(horse, url));
        }
    }

    public void MarkDie()
    {
        hasDied = true;
        State = HERO_STATE.Die;
    }

    [PunRPC]
    public void MoveToRPC(float posX, float posY, float posZ, PhotonMessageInfo info)
    {
        if (info.sender.isMasterClient)
        {
            transform.position = new Vector3(posX, posY, posZ);
        }
    }

    [PunRPC]
    private void Net3DMGSMOKE(bool ifON)
    {
        if (smoke_3dmg != null)
        {
            smoke_3dmgEmission.enabled = ifON;
        }
    }

    [PunRPC]
    private void NetContinueAnimation()
    {
        IEnumerator enumerator = baseAnimation.GetEnumerator();
        try
        {
            while (enumerator.MoveNext())
            {
                AnimationState current = (AnimationState)enumerator.Current;
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
        PlayAnimation(CurrentPlayingClipName());
    }

    [PunRPC]
    private void NetCrossFade(string aniName, float time)
    {
        currentAnimation = aniName;
        if (baseAnimation != null)
        {
            baseAnimation.CrossFade(aniName, time);
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
                eren_titan.GetComponent<ErenTitan>().lifeTime = 0.1f;
            }
            if (skillCD != null)
            {
                skillCD.transform.localPosition = vector;
            }
        }
        if (bulletLeft != null)
        {
            bulletLeft.GetComponent<Bullet>().removeMe();
        }
        if (bulletRight != null)
        {
            bulletRight.GetComponent<Bullet>().removeMe();
        }

        if (!(useGun || (!photonView.isMine)))
        {
            //TODO: Re-enable these again
            //leftbladetrail.Deactivate();
            //rightbladetrail.Deactivate();
            //leftbladetrail2.Deactivate();
            //rightbladetrail2.Deactivate();
        }
        FalseAttack();
        BreakApart(v, isBite);
        if (photonView.isMine)
        {
            currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().SetSpectorMode(false);
            currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().gameOver = true;
            FengGameManagerMKII.instance.myRespawnTime = 0f;
        }
        hasDied = true;
        audioSystem
            .PlayOneShot(audioSystem.clipDie)
            .Disconnect(audioSystem.clipDie);

        gameObject.GetComponent<SmoothSyncMovement>().disabled = true;
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
                PhotonView view2 = PhotonView.Find(viewID);
                if (view2 != null)
                {
                    FengGameManagerMKII.instance.sendKillInfo(killByTitan, $"<color=#ffc000>[{info.sender.ID}]</color> " + RCextensions.returnStringFromObject(view2.owner.CustomProperties[PhotonPlayerProperty.name]), false, RCextensions.returnStringFromObject(PhotonNetwork.player.CustomProperties[PhotonPlayerProperty.name]), 0);
                    propertiesToSet = new ExitGames.Client.Photon.Hashtable();
                    propertiesToSet.Add(PhotonPlayerProperty.kills, RCextensions.returnIntFromObject(view2.owner.CustomProperties[PhotonPlayerProperty.kills]) + 1);
                    view2.owner.SetCustomProperties(propertiesToSet);
                }
            }
            else
            {
                FengGameManagerMKII.instance.sendKillInfo(!(titanName == string.Empty), $"<color=#ffc000>[{info.sender.ID}]</color> " + titanName, false, RCextensions.returnStringFromObject(PhotonNetwork.player.CustomProperties[PhotonPlayerProperty.name]), 0);
            }
        }
        if (photonView.isMine)
        {
            PhotonNetwork.Destroy(photonView);
        }
    }

    [PunRPC]
    private void NetDie2(int viewID = -1, string titanName = "", PhotonMessageInfo info = new PhotonMessageInfo())
    {
        GameObject obj2;
        if ((photonView.isMine) && (GameSettings.Gamemode.GamemodeType != GamemodeType.TitanRush))
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
                    else if (GameSettings.PvP.Bomb.Value && (!GameSettings.PvP.Cannons.Value))
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
                eren_titan.GetComponent<ErenTitan>().lifeTime = 0.1f;
            }
            if (skillCD != null)
            {
                skillCD.transform.localPosition = vector;
            }
        }

        if (bulletLeft != null)
        {
            bulletLeft.GetComponent<Bullet>().removeMe();
        }
        if (bulletRight != null)
        {
            bulletRight.GetComponent<Bullet>().removeMe();
        }
        audioSystem
            .PlayOneShot(audioSystem.clipDie)
            .Disconnect(audioSystem.clipDie);

        if (photonView.isMine)
        {
            currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().SetMainObject(null, true, false);
            currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().SetSpectorMode(true);
            currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().gameOver = true;
            FengGameManagerMKII.instance.myRespawnTime = 0f;
        }
        FalseAttack();
        hasDied = true;
        gameObject.GetComponent<SmoothSyncMovement>().disabled = true;
        if (photonView.isMine)
        {
            PhotonNetwork.RemoveRPCs(photonView);
            ExitGames.Client.Photon.Hashtable propertiesToSet = new ExitGames.Client.Photon.Hashtable();
            propertiesToSet.Add(PhotonPlayerProperty.dead, true);
            PhotonNetwork.player.SetCustomProperties(propertiesToSet);
            propertiesToSet = new ExitGames.Client.Photon.Hashtable();
            propertiesToSet.Add(PhotonPlayerProperty.deaths, ((int)PhotonNetwork.player.CustomProperties[PhotonPlayerProperty.deaths]) + 1);
            PhotonNetwork.player.SetCustomProperties(propertiesToSet);
            if (viewID != -1)
            {
                PhotonView view2 = PhotonView.Find(viewID);
                if (view2 != null)
                {
                    FengGameManagerMKII.instance.sendKillInfo(true, $"<color=#ffc000>[{info.sender.ID}]</color> " + RCextensions.returnStringFromObject(view2.owner.CustomProperties[PhotonPlayerProperty.name]), false, RCextensions.returnStringFromObject(PhotonNetwork.player.CustomProperties[PhotonPlayerProperty.name]), 0);
                    propertiesToSet = new ExitGames.Client.Photon.Hashtable();
                    propertiesToSet.Add(PhotonPlayerProperty.kills, RCextensions.returnIntFromObject(view2.owner.CustomProperties[PhotonPlayerProperty.kills]) + 1);
                    view2.owner.SetCustomProperties(propertiesToSet);
                }
            }
            else
            {
                FengGameManagerMKII.instance.sendKillInfo(true, $"<color=#ffc000>[{info.sender.ID}]</color> " + titanName, false, RCextensions.returnStringFromObject(PhotonNetwork.player.CustomProperties[PhotonPlayerProperty.name]), 0);
            }
        }
        if (photonView.isMine)
        {
            obj2 = PhotonNetwork.Instantiate("hitMeat2", audioSystem.Position, Quaternion.Euler(270f, 0f, 0f), 0);
        }
        else
        {
            obj2 = (GameObject)Instantiate(Resources.Load("hitMeat2"));
        }
        obj2.transform.position = audioSystem.Position;
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
                eren_titan.GetComponent<ErenTitan>().lifeTime = 0.1f;
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
            bulletLeft.GetComponent<Bullet>().removeMe();
        }
        if (bulletRight != null)
        {
            bulletRight.GetComponent<Bullet>().removeMe();
        }

        if (!(useGun || (!photonView.isMine)))
        {
            /*
            leftbladetrail.Deactivate();
            rightbladetrail.Deactivate();
            leftbladetrail2.Deactivate();
            rightbladetrail2.Deactivate();
            */
        }
        FalseAttack();
        BreakApart(v, isBite);
        if (photonView.isMine)
        {
            currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().SetSpectorMode(false);
            currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().gameOver = true;
            FengGameManagerMKII.instance.myRespawnTime = 0f;
        }
        hasDied = true;
        audioSystem
            .PlayOneShot(audioSystem.clipDie)
            .Disconnect(audioSystem.clipDie);
        gameObject.GetComponent<SmoothSyncMovement>().disabled = true;
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

    [PunRPC]
    private void NetGrabbed(int id, bool leftHand)
    {
        titanWhoGrabMeID = id;
        Grabbed(PhotonView.Find(id).gameObject, leftHand);
    }

    [PunRPC]
    private void NetLaughAttack()
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
    private void NetPauseAnimation()
    {
        IEnumerator enumerator = baseAnimation.GetEnumerator();
        try
        {
            while (enumerator.MoveNext())
            {
                AnimationState current = (AnimationState)enumerator.Current;
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
    }

    [PunRPC]
    private void NetPlayAnimation(string aniName)
    {
        currentAnimation = aniName;
        if (baseAnimation != null)
        {
            baseAnimation.Play(aniName);
        }
    }

    [PunRPC]
    private void NetPlayAnimationAt(string aniName, float normalizedTime)
    {
        currentAnimation = aniName;
        if (baseAnimation != null)
        {
            baseAnimation.Play(aniName);
            baseAnimation[aniName].normalizedTime = normalizedTime;
        }
    }

    [PunRPC]
    private void NetSetIsGrabbedFalse()
    {
        State = HERO_STATE.Idle;
    }

    [PunRPC]
    private void NetTauntAttack(float tauntTime, float distance = 100f)
    {
        throw new NotImplementedException("Titan taunt behavior is not yet implemented");
        //foreach (GameObject obj2 in GameObject.FindGameObjectsWithTag("titan"))
        //{
        //    if ((Vector3.Distance(obj2.transform.position, transform.position) < distance) && (obj2.GetComponent<TITAN>() != null))
        //    {
        //        obj2.GetComponent<TITAN>().beTauntedBy(gameObject, tauntTime);
        //    }
        //}
    }

    [PunRPC]
    private void NetUngrabbed()
    {
        Ungrabbed();
        NetPlayAnimation(standAnimation);
        FalseAttack();
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
        releaseIfIHookSb();
        //if ((IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER) && photonView.isMine)
        //{
        //    Vector3 vector = (Vector3) (Vector3.up * 5000f);
        //    cross1.transform.localPosition = vector;
        //    cross2.transform.localPosition = vector;
        //    crossL1.transform.localPosition = vector;
        //    crossL2.transform.localPosition = vector;
        //    crossR1.transform.localPosition = vector;
        //    crossR2.transform.localPosition = vector;
        //    LabelDistance.transform.localPosition = vector;
        //}
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

    public void PauseAnimation()
    {
        IEnumerator enumerator = baseAnimation.GetEnumerator();
        try
        {
            while (enumerator.MoveNext())
            {
                AnimationState current = (AnimationState)enumerator.Current;
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
        baseAnimation.Play(aniName);
        if (PhotonNetwork.connected && photonView.isMine)
        {
            object[] parameters = new object[] { aniName };
            photonView.RPC(nameof(NetPlayAnimation), PhotonTargets.Others, parameters);
        }
    }

    private void playAnimationAt(string aniName, float normalizedTime)
    {
        currentAnimation = aniName;
        baseAnimation.Play(aniName);
        baseAnimation[aniName].normalizedTime = normalizedTime;
        if (PhotonNetwork.connected && photonView.isMine)
        {
            object[] parameters = new object[] { aniName, normalizedTime };
            photonView.RPC(nameof(NetPlayAnimationAt), PhotonTargets.Others, parameters);
        }
    }

    private void releaseIfIHookSb()
    {
        if (hookSomeOne && (hookTarget != null))
        {
            hookTarget.GetPhotonView().RPC(nameof(BadGuyReleaseMe), hookTarget.GetPhotonView().owner, new object[0]);
            hookTarget = null;
            hookSomeOne = false;
        }
    }

    public IEnumerator reloadSky()
    {
        yield return new WaitForSeconds(0.5f);
        if ((FengGameManagerMKII.skyMaterial != null) && (Camera.main.GetComponent<Skybox>().material != FengGameManagerMKII.skyMaterial))
        {
            Camera.main.GetComponent<Skybox>().material = FengGameManagerMKII.skyMaterial;
        }
    }

    public void resetAnimationSpeed()
    {
        IEnumerator enumerator = baseAnimation.GetEnumerator();
        try
        {
            while (enumerator.MoveNext())
            {
                AnimationState current = (AnimationState)enumerator.Current;
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
            gameObject.GetComponent<SmoothSyncMovement>().disabled = false;
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
            baseRigidBody.AddForce((-baseRigidBody.velocity * 0.9f), ForceMode.VelocityChange);
            float num = Mathf.Pow(launchForce.magnitude, 0.1f);
            if (grounded)
            {
                baseRigidBody.AddForce((Vector3.up * Mathf.Min((float)(launchForce.magnitude * 0.2f), (float)10f)), ForceMode.Impulse);
            }
            baseRigidBody.AddForce(((launchForce * num) * 0.1f), ForceMode.Impulse);
            if (State != HERO_STATE.Grab)
            {
                dashTime = 1f;
                CrossFade("dash", 0.05f);
                baseAnimation["dash"].time = 0.1f;
                State = HERO_STATE.AirDodge;
                FalseAttack();
                facingDirection = Mathf.Atan2(launchForce.x, launchForce.z) * Mathf.Rad2Deg;
                Quaternion quaternion = Quaternion.Euler(0f, facingDirection, 0f);
                gameObject.transform.rotation = quaternion;
                baseRigidBody.rotation = quaternion;
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
        State = HERO_STATE.Salute;
        CrossFade("salute", 0.1f);
    }

    private void SetHookedPplDirection()
    {
        almostSingleHook = false;
        if (isRightHandHooked && isLeftHandHooked)
        {
            if ((bulletLeft != null) && (bulletRight != null))
            {
                Vector3 normal = bulletLeft.transform.position - bulletRight.transform.position;
                if (normal.sqrMagnitude < 4f)
                {
                    Vector3 vector2 = (((bulletLeft.transform.position + bulletRight.transform.position) * 0.5f)) - transform.position;
                    facingDirection = Mathf.Atan2(vector2.x, vector2.z) * Mathf.Rad2Deg;
                    if (useGun && (State != HERO_STATE.Attack))
                    {
                        float current = -Mathf.Atan2(baseRigidBody.velocity.z, baseRigidBody.velocity.x) * Mathf.Rad2Deg;
                        float target = -Mathf.Atan2(vector2.z, vector2.x) * Mathf.Rad2Deg;
                        float num3 = -Mathf.DeltaAngle(current, target);
                        facingDirection += num3;
                    }
                    almostSingleHook = true;
                }
                else
                {
                    Vector3 to = transform.position - bulletLeft.transform.position;
                    Vector3 vector6 = transform.position - bulletRight.transform.position;
                    Vector3 vector7 = ((bulletLeft.transform.position + bulletRight.transform.position) * 0.5f);
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
            if (isRightHandHooked && (bulletRight != null))
            {
                zero = bulletRight.transform.position - transform.position;
            }
            else
            {
                if (!isLeftHandHooked || (bulletLeft == null))
                {
                    return;
                }
                zero = bulletLeft.transform.position - transform.position;
            }
            facingDirection = Mathf.Atan2(zero.x, zero.z) * Mathf.Rad2Deg;
            if (State != HERO_STATE.Attack)
            {
                float num6 = -Mathf.Atan2(baseRigidBody.velocity.z, baseRigidBody.velocity.x) * Mathf.Rad2Deg;
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
            GetComponent<SmoothSyncMovement>().PhotonCamera = true;
            isPhotonCamera = true;
        }
    }

    [PunRPC]
    private void SetMyTeam(int val)
    {
        myTeam = val;
        checkBoxLeft.GetComponent<TriggerColliderWeapon>().myTeam = val;
        checkBoxRight.GetComponent<TriggerColliderWeapon>().myTeam = val;
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

    public void SetSkillHUDPosition()
    {
        skillCD = GameObject.Find("skill_cd_" + skillId);
        if (skillCD != null)
        {
            skillCD.transform.localPosition = GameObject.Find("skill_cd_bottom").transform.localPosition;
        }
        if (useGun)
        {
            skillCD.transform.localPosition = (Vector3.up * 5000f);
        }
    }

    public void SetSkillHUDPosition2()
    {
        return;
        //skillCD = GameObject.Find("skill_cd_" + skillIDHUD);
        //if (skillCD != null)
        //{
        //    skillCD.transform.localPosition = GameObject.Find("skill_cd_bottom").transform.localPosition;
        //}
        //if (useGun && (FengGameManagerMKII.Gamemode.Settings.PvPBomb))
        //{
        //    skillCD.transform.localPosition = (Vector3)(Vector3.up * 5000f);
        //}
    }

    public void SetStat2()
    {
        skillCDLast = 1.5f;
        skillId = setup.myCostume.stat.skillId;
        if (skillId == "levi")
        {
            skillCDLast = 3.5f;
        }
        CustomAnimationSpeed();
        if (skillId == "armin")
        {
            skillCDLast = 5f;
        }
        if (skillId == "marco")
        {
            skillCDLast = 10f;
        }
        if (skillId == "jean")
        {
            skillCDLast = 0.001f;
        }
        if (skillId == "eren")
        {
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
                    int num = 0;
                    foreach (PhotonPlayer player in PhotonNetwork.playerList)
                    {
                        if ((RCextensions.returnIntFromObject(player.CustomProperties[PhotonPlayerProperty.isTitan]) == 1) && (RCextensions.returnStringFromObject(player.CustomProperties[PhotonPlayerProperty.character]).ToUpper() == "EREN"))
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
        }
        if (skillId == "sasha")
        {
            skillCDLast = 20f;
        }
        if (skillId == "petra")
        {
            skillCDLast = 3.5f;
        }
        BombInit();
        speed = ((float)setup.myCostume.stat.SPD) / 10f;
        totalGas = currentGas = setup.myCostume.stat.GAS;
        totalBladeSta = currentBladeSta = setup.myCostume.stat.BLA;
        baseRigidBody.mass = 0.5f - ((setup.myCostume.stat.ACL - 100) * 0.001f);
        //GameObject.Find("skill_cd_bottom").transform.localPosition = new Vector3(0f, (-Screen.height * 0.5f) + 5f, 0f);
        //skillCD = GameObject.Find("skill_cd_" + skillIDHUD);
        //skillCD.transform.localPosition = GameObject.Find("skill_cd_bottom").transform.localPosition;
        //GameObject.Find("GasUI").transform.localPosition = GameObject.Find("skill_cd_bottom").transform.localPosition;
        if (photonView.isMine)
        {
            //GameObject.Find("bulletL").GetComponent<UISprite>().enabled = false;
            //GameObject.Find("bulletR").GetComponent<UISprite>().enabled = false;
            //GameObject.Find("bulletL1").GetComponent<UISprite>().enabled = false;
            //GameObject.Find("bulletR1").GetComponent<UISprite>().enabled = false;
            //GameObject.Find("bulletL2").GetComponent<UISprite>().enabled = false;
            //GameObject.Find("bulletR2").GetComponent<UISprite>().enabled = false;
            //GameObject.Find("bulletL3").GetComponent<UISprite>().enabled = false;
            //GameObject.Find("bulletR3").GetComponent<UISprite>().enabled = false;
            //GameObject.Find("bulletL4").GetComponent<UISprite>().enabled = false;
            //GameObject.Find("bulletR4").GetComponent<UISprite>().enabled = false;
            //GameObject.Find("bulletL5").GetComponent<UISprite>().enabled = false;
            //GameObject.Find("bulletR5").GetComponent<UISprite>().enabled = false;
            //GameObject.Find("bulletL6").GetComponent<UISprite>().enabled = false;
            //GameObject.Find("bulletR6").GetComponent<UISprite>().enabled = false;
            //GameObject.Find("bulletL7").GetComponent<UISprite>().enabled = false;
            //GameObject.Find("bulletR7").GetComponent<UISprite>().enabled = false;
        }
        if (EquipmentType == EquipmentType.Ahss)
        {
            standAnimation = "AHSS_stand_gun";
            useGun = true;
            gunDummy = new GameObject();
            gunDummy.name = "gunDummy";
            gunDummy.transform.position = baseTransform.position;
            gunDummy.transform.rotation = baseTransform.rotation;
            SetTeam2(2);
            if (photonView.isMine)
            {
                //GameObject.Find("bladeCL").GetComponent<UISprite>().enabled = false;
                //GameObject.Find("bladeCR").GetComponent<UISprite>().enabled = false;
                //GameObject.Find("bladel1").GetComponent<UISprite>().enabled = false;
                //GameObject.Find("blader1").GetComponent<UISprite>().enabled = false;
                //GameObject.Find("bladel2").GetComponent<UISprite>().enabled = false;
                //GameObject.Find("blader2").GetComponent<UISprite>().enabled = false;
                //GameObject.Find("bladel3").GetComponent<UISprite>().enabled = false;
                //GameObject.Find("blader3").GetComponent<UISprite>().enabled = false;
                //GameObject.Find("bladel4").GetComponent<UISprite>().enabled = false;
                //GameObject.Find("blader4").GetComponent<UISprite>().enabled = false;
                //GameObject.Find("bladel5").GetComponent<UISprite>().enabled = false;
                //GameObject.Find("blader5").GetComponent<UISprite>().enabled = false;
                //GameObject.Find("bulletL").GetComponent<UISprite>().enabled = true;
                //GameObject.Find("bulletR").GetComponent<UISprite>().enabled = true;
                //GameObject.Find("bulletL1").GetComponent<UISprite>().enabled = true;
                //GameObject.Find("bulletR1").GetComponent<UISprite>().enabled = true;
                //GameObject.Find("bulletL2").GetComponent<UISprite>().enabled = true;
                //GameObject.Find("bulletR2").GetComponent<UISprite>().enabled = true;
                //GameObject.Find("bulletL3").GetComponent<UISprite>().enabled = true;
                //GameObject.Find("bulletR3").GetComponent<UISprite>().enabled = true;
                //GameObject.Find("bulletL4").GetComponent<UISprite>().enabled = true;
                //GameObject.Find("bulletR4").GetComponent<UISprite>().enabled = true;
                //GameObject.Find("bulletL5").GetComponent<UISprite>().enabled = true;
                //GameObject.Find("bulletR5").GetComponent<UISprite>().enabled = true;
                //GameObject.Find("bulletL6").GetComponent<UISprite>().enabled = true;
                //GameObject.Find("bulletR6").GetComponent<UISprite>().enabled = true;
                //GameObject.Find("bulletL7").GetComponent<UISprite>().enabled = true;
                //GameObject.Find("bulletR7").GetComponent<UISprite>().enabled = true;
                //if (skillId != "bomb")
                //{
                //    skillCD.transform.localPosition = (Vector3) (Vector3.up * 5000f);
                //}
            }
        }
        else if (setup.myCostume.sex == SEX.FEMALE)
        {
            standAnimation = "stand";
            SetTeam2(1);
        }
        else
        {
            standAnimation = "stand_levi";
            SetTeam2(1);
        }
    }

    public void SetTeam(int team)
    {
        SetMyTeam(team);
        if (photonView.isMine)
        {
            object[] parameters = new object[] { team };
            photonView.RPC(nameof(SetMyTeam), PhotonTargets.OthersBuffered, parameters);
            ExitGames.Client.Photon.Hashtable propertiesToSet = new ExitGames.Client.Photon.Hashtable();
            propertiesToSet.Add(PhotonPlayerProperty.team, team);
            PhotonNetwork.player.SetCustomProperties(propertiesToSet);
        }
    }

    public void SetTeam2(int team)
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
            LayerMask mask = ((int)1) << LayerMask.NameToLayer("Ground");
            LayerMask mask2 = ((int)1) << LayerMask.NameToLayer("EnemyBox");
            LayerMask mask3 = mask2 | mask;
            var distance = "???";
            var magnitude = HookRaycastDistance;
            var hitDistance = HookRaycastDistance;
            var hitPoint = ray.GetPoint(hitDistance);

            var mousePos = Input.mousePosition;
            hookUI.cross.position = mousePos;

            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 1000f, mask3.value))
            {
                magnitude = (hit.point - baseTransform.position).magnitude;
                distance = ((int) magnitude).ToString();
                hitDistance = hit.distance;
                hitPoint = hit.point;
            }

            hookUI.crossImage.color = magnitude > 120f ? Color.red : Color.white;
            hookUI.distanceLabel.transform.localPosition = hookUI.cross.localPosition;

            if (((int) FengGameManagerMKII.settings[0xbd]) == 1)
            {
                distance += "\n" + currentSpeed.ToString("F1") + " u/s";
            }
            else if (((int) FengGameManagerMKII.settings[0xbd]) == 2)
            {
                distance += "\n" + ((currentSpeed / 100f)).ToString("F1") + "K";
            }
            hookUI.distanceLabel.text = distance;

            Vector3 vector2 = new Vector3(0f, 0.4f, 0f);
            vector2 -=  (baseTransform.right * 0.3f);
            Vector3 vector3 = new Vector3(0f, 0.4f, 0f);
            vector3 +=  (baseTransform.right * 0.3f);
            float num4 = (hitDistance <= 50f) ? (hitDistance * 0.05f) : (hitDistance * 0.3f);
            Vector3 vector4 = (hitPoint - ( (baseTransform.right * num4))) - (baseTransform.position + vector2);
            Vector3 vector5 = (hitPoint + ( (baseTransform.right * num4))) - (baseTransform.position + vector3);
            vector4.Normalize();
            vector5.Normalize();
            vector4 =  (vector4 * HookRaycastDistance);
            vector5 =  (vector5 * HookRaycastDistance);
            RaycastHit hit2;
            hitPoint = (baseTransform.position + vector2) + vector4;
            hitDistance = HookRaycastDistance;
            if (Physics.Linecast(baseTransform.position + vector2, (baseTransform.position + vector2) + vector4, out hit2, mask3.value))
            {
                hitPoint = hit2.point;
                hitDistance = hit2.distance;
            }

            hookUI.crossL.transform.position = currentCamera.WorldToScreenPoint(hitPoint);
            hookUI.crossL.transform.localRotation = Quaternion.Euler(0f, 0f, (Mathf.Atan2(hookUI.crossL.transform.position.y - mousePos.y, hookUI.crossL.transform.position.x - mousePos.x) * Mathf.Rad2Deg) + 180f);
            hookUI.crossImageL.color = hitDistance > 120f ? Color.red : Color.white;

            hitPoint = (baseTransform.position + vector3) + vector5;
            hitDistance = HookRaycastDistance;
            if (Physics.Linecast(baseTransform.position + vector3, (baseTransform.position + vector3) + vector5, out hit2, mask3.value))
            {
                hitPoint = hit2.point;
                hitDistance = hit2.distance;
            }

            hookUI.crossR.transform.position = currentCamera.WorldToScreenPoint(hitPoint);
            hookUI.crossR.transform.localRotation = Quaternion.Euler(0f, 0f, Mathf.Atan2(hookUI.crossR.transform.position.y - mousePos.y, hookUI.crossR.transform.position.x - mousePos.x) * Mathf.Rad2Deg);
            hookUI.crossImageR.color = hitDistance > 120f ? Color.red : Color.white;
        }
    }

    private void ShowFlareCD()
    {
        if (GameObject.Find("UIflare1") != null)
        {
            //GameObject.Find("UIflare1").GetComponent<UISprite>().fillAmount = (flareTotalCD - flare1CD) / flareTotalCD;
            //GameObject.Find("UIflare2").GetComponent<UISprite>().fillAmount = (flareTotalCD - flare2CD) / flareTotalCD;
            //GameObject.Find("UIflare3").GetComponent<UISprite>().fillAmount = (flareTotalCD - flare3CD) / flareTotalCD;
        }
    }

    private void ShowFlareCD2()
    {
        if (cachedSprites["UIflare1"] != null)
        {
            cachedSprites["UIflare1"].fillAmount = (flareTotalCD - flare1CD) / flareTotalCD;
            cachedSprites["UIflare2"].fillAmount = (flareTotalCD - flare2CD) / flareTotalCD;
            cachedSprites["UIflare3"].fillAmount = (flareTotalCD - flare3CD) / flareTotalCD;
        }
    }

    private void ShowGas2()
    {
        float num = currentGas / totalGas;
        float num2 = currentBladeSta / totalBladeSta;
        cachedSprites["GasLeft"].fillAmount = cachedSprites["GasRight"].fillAmount = currentGas / totalGas;
        if (num <= 0.25f)
        {
            cachedSprites["GasLeft"].color = cachedSprites["GasRight"].color = Color.red;
        }
        else if (num < 0.5f)
        {
            cachedSprites["GasLeft"].color = cachedSprites["GasRight"].color = Color.yellow;
        }
        else
        {
            cachedSprites["GasLeft"].color = cachedSprites["GasRight"].color = Color.white;
        }
        Equipment.Weapon.UpdateSupplyUi(InGameUI);
        //if (!useGun)
        //{
        //    cachedSprites["bladeCL"].fillAmount = currentBladeSta / totalBladeSta;
        //    cachedSprites["bladeCR"].fillAmount = currentBladeSta / totalBladeSta;
        //    if (num <= 0f)
        //    {
        //        cachedSprites["gasL"].color = Color.red;
        //        cachedSprites["gasR"].color = Color.red;
        //    }
        //    else if (num < 0.3f)
        //    {
        //        cachedSprites["gasL"].color = Color.yellow;
        //        cachedSprites["gasR"].color = Color.yellow;
        //    }
        //    else
        //    {
        //        cachedSprites["gasL"].color = Color.white;
        //        cachedSprites["gasR"].color = Color.white;
        //    }
        //    if (num2 <= 0f)
        //    {
        //        cachedSprites["bladel1"].color = Color.red;
        //        cachedSprites["blader1"].color = Color.red;
        //    }
        //    else if (num2 < 0.3f)
        //    {
        //        cachedSprites["bladel1"].color = Color.yellow;
        //        cachedSprites["blader1"].color = Color.yellow;
        //    }
        //    else
        //    {
        //        cachedSprites["bladel1"].color = Color.white;
        //        cachedSprites["blader1"].color = Color.white;
        //    }
        //    if (currentBladeNum <= 4)
        //    {
        //        cachedSprites["bladel5"].enabled = false;
        //        cachedSprites["blader5"].enabled = false;
        //    }
        //    else
        //    {
        //        cachedSprites["bladel5"].enabled = true;
        //        cachedSprites["blader5"].enabled = true;
        //    }
        //    if (currentBladeNum <= 3)
        //    {
        //        cachedSprites["bladel4"].enabled = false;
        //        cachedSprites["blader4"].enabled = false;
        //    }
        //    else
        //    {
        //        cachedSprites["bladel4"].enabled = true;
        //        cachedSprites["blader4"].enabled = true;
        //    }
        //    if (currentBladeNum <= 2)
        //    {
        //        cachedSprites["bladel3"].enabled = false;
        //        cachedSprites["blader3"].enabled = false;
        //    }
        //    else
        //    {
        //        cachedSprites["bladel3"].enabled = true;
        //        cachedSprites["blader3"].enabled = true;
        //    }
        //    if (currentBladeNum <= 1)
        //    {
        //        cachedSprites["bladel2"].enabled = false;
        //        cachedSprites["blader2"].enabled = false;
        //    }
        //    else
        //    {
        //        cachedSprites["bladel2"].enabled = true;
        //        cachedSprites["blader2"].enabled = true;
        //    }
        //    if (currentBladeNum <= 0)
        //    {
        //        cachedSprites["bladel1"].enabled = false;
        //        cachedSprites["blader1"].enabled = false;
        //    }
        //    else
        //    {
        //        cachedSprites["bladel1"].enabled = true;
        //        cachedSprites["blader1"].enabled = true;
        //    }
        //}
        //else
        //{
        //    if (leftGunHasBullet)
        //    {
        //        cachedSprites["bulletL"].enabled = true;
        //    }
        //    else
        //    {
        //        cachedSprites["bulletL"].enabled = false;
        //    }
        //    if (rightGunHasBullet)
        //    {
        //        cachedSprites["bulletR"].enabled = true;
        //    }
        //    else
        //    {
        //        cachedSprites["bulletR"].enabled = false;
        //    }
        //}
    }

    [PunRPC]
    private void ShowHitDamage()
    {
        GameObject target = GameObject.Find("LabelScore");
        if (target != null)
        {
            speed = Mathf.Max(10f, speed);
            //target.GetComponent<UILabel>().text = speed.ToString();
            target.transform.localScale = Vector3.zero;
            speed = (int)(speed * 0.1f);
            speed = Mathf.Clamp(speed, 40f, 150f);
            iTween.Stop(target);
            object[] args = new object[] { "x", speed, "y", speed, "z", speed, "easetype", iTween.EaseType.easeOutElastic, "time", 1f };
            iTween.ScaleTo(target, iTween.Hash(args));
            object[] objArray2 = new object[] { "x", 0, "y", 0, "z", 0, "easetype", iTween.EaseType.easeInBounce, "time", 0.5f, "delay", 2f };
            iTween.ScaleTo(target, iTween.Hash(objArray2));
        }
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
            
            if (bulletLeft)
                bulletLeft.GetComponent<Bullet>().removeMe();

            if (bulletRight)
                bulletRight.GetComponent<Bullet>().removeMe();

            if ((smoke_3dmgEmission.enabled) && photonView.isMine)
            {
                object[] parameters = new object[] { false };
                photonView.RPC(nameof(Net3DMGSMOKE), PhotonTargets.Others, parameters);
            }
            smoke_3dmgEmission.enabled = false;
            baseRigidBody.velocity = Vector3.zero;
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
            Camera.main.GetComponent<IN_GAME_MAIN_CAMERA>().SetMainObject(myCannon.transform.Find("Barrel").Find("FiringPoint").gameObject, true, false);
            Camera.main.fieldOfView = 55f;
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
            var position = baseTransform.position + Vector3.up * 5f;
            var rotation = baseTransform.rotation;
            myHorse = Horse.Create(this, position, rotation);
        }

        if (!GameSettings.Horse.Enabled.Value && myHorse != null)
        {
            PhotonNetwork.Destroy(myHorse);
        }
    }

    private void Start()
    {
        gameObject.AddComponent<PlayerInteractable>();
        SetHorse();
        sparks = baseTransform.Find("slideSparks").GetComponent<ParticleSystem>();
        sparksEmission = sparks.emission;
        smoke_3dmg = baseTransform.Find("3dmg_smoke").GetComponent<ParticleSystem>();
        smoke_3dmgEmission = smoke_3dmg.emission;
        baseTransform.localScale = new Vector3(myScale, myScale, myScale);
        facingDirection = baseTransform.rotation.eulerAngles.y;
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

        PlayerName.GetComponent<TextMesh>().text = FengGameManagerMKII.instance.name;
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
                new object[] {PlayerPrefs.GetFloat("cameraDistance") + 0.3f});
        }
        else
        {
            bool flag2 = false;
            //HACK
            //if (photonView.owner.CustomProperties[PhotonPlayerProperty.RCteam] != null)
            //{
            //    switch (RCextensions.returnIntFromObject(photonView.owner.CustomProperties[PhotonPlayerProperty.RCteam]))
            //    {
            //        case 1:
            //            flag2 = true;
            //            if (Minimap.instance != null)
            //            {
            //                Minimap.instance.TrackGameObjectOnMinimap(gameObject, Color.cyan, false, true, Minimap.IconStyle.CIRCLE);
            //            }
            //            break;

            //        case 2:
            //            flag2 = true;
            //            if (Minimap.instance != null)
            //            {
            //                Minimap.instance.TrackGameObjectOnMinimap(gameObject, Color.magenta, false, true, Minimap.IconStyle.CIRCLE);
            //            }
            //            break;
            //    }
            //}
            //if (RCextensions.returnIntFromObject(photonView.owner.CustomProperties[PhotonPlayerProperty.team]) == 2)
            //{
            //    myNetWorkName.GetComponent<UILabel>().text = "[FF0000]AHSS\n[FFFFFF]";
            //    if (!flag2 && (Minimap.instance != null))
            //    {
            //        Minimap.instance.TrackGameObjectOnMinimap(gameObject, Color.red, false, true, Minimap.IconStyle.CIRCLE);
            //    }
            //}
            //else if (!flag2 && (Minimap.instance != null))
            //{
            //    Minimap.instance.TrackGameObjectOnMinimap(gameObject, Color.blue, false, true, Minimap.IconStyle.CIRCLE);
            //}
        }

        //string str = RCextensions.returnStringFromObject(photonView.owner.CustomProperties[PhotonPlayerProperty.guildName]);
        //if (str != string.Empty)
        //{
        //    UILabel component = myNetWorkName.GetComponent<UILabel>();
        //    string text = component.text;
        //    string[] strArray2 = new string[] { text, "[FFFF00]", str, "\n[FFFFFF]", RCextensions.returnStringFromObject(photonView.owner.CustomProperties[PhotonPlayerProperty.name]) };
        //    component.text = string.Concat(strArray2);
        //}
        //else
        //{
        //    UILabel label2 = myNetWorkName.GetComponent<UILabel>();
        //    label2.text = label2.text + RCextensions.returnStringFromObject(photonView.owner.CustomProperties[PhotonPlayerProperty.name]);
        //}
        if (!photonView.isMine)
        {
            gameObject.layer = LayerMask.NameToLayer("NetworkObject");
            if (IN_GAME_MAIN_CAMERA.dayLight == DayLight.Night)
            {
                GameObject obj3 = (GameObject)Instantiate(Resources.Load("flashlight"));
                obj3.transform.parent = baseTransform;
                obj3.transform.position = baseTransform.position + Vector3.up;
                obj3.transform.rotation = Quaternion.Euler(353f, 0f, 0f);
            }   
            setup.init();
            setup.myCostume = new HeroCostume();
            setup.myCostume = CostumeConeveter.PhotonDataToHeroCostume2(photonView.owner);
            setup.setCharacterComponent();
            Destroy(checkBoxLeft);
            Destroy(checkBoxRight);
            /*
            UnityEngine.Object.Destroy(leftbladetrail);
            UnityEngine.Object.Destroy(rightbladetrail);
            UnityEngine.Object.Destroy(leftbladetrail2);
            UnityEngine.Object.Destroy(rightbladetrail2);
            */
            hasspawn = true;
        }
        else
        {
            currentCamera = GameObject.Find("MainCamera").GetComponent<Camera>();
            LoadSkin();
            hasspawn = true;
            StartCoroutine(reloadSky());
            bombImmune = false;
            if (GameSettings.PvP.Bomb.Value)
            {
                bombImmune = true;
                StartCoroutine(StopImmunity());
            }
        }
    }

    public IEnumerator StopImmunity()
    {
        yield return new WaitForSeconds(5f);
        bombImmune = false;
    }

    private void Suicide()
    {
        NetDieLocal((baseRigidBody.velocity * 50f), false, -1, string.Empty, true);
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

    public void Update()
    {
        if (!IN_GAME_MAIN_CAMERA.isPausing)
        {
            if (invincible > 0f)
            {
                invincible -= Time.deltaTime;
            }
            if (!hasDied)
            {
                if (titanForm && (eren_titan != null))
                {
                    baseTransform.position = eren_titan.transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/neck").position;
                    gameObject.GetComponent<SmoothSyncMovement>().disabled = true;
                }
                else if (isCannon && (myCannon != null))
                {
                    UpdateCannon();
                    gameObject.GetComponent<SmoothSyncMovement>().disabled = true;
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
                        if (skillId == "jean")
                        {
                            if (((State != HERO_STATE.Attack) &&
                                 (InputManager.KeyDown(InputHuman.Attack) ||
                                  InputManager.KeyDown(InputHuman.AttackSpecial))) &&
                                ((escapeTimes > 0) && !baseAnimation.IsPlaying("grabbed_jean")))
                            {
                                PlayAnimation("grabbed_jean");
                                baseAnimation["grabbed_jean"].time = 0f;
                                escapeTimes--;
                            }
                            if ((baseAnimation.IsPlaying("grabbed_jean") && (baseAnimation["grabbed_jean"].normalizedTime > 0.64f)) && (titanWhoGrabMe.GetComponent<MindlessTitan>() != null))
                            {
                                Ungrabbed();
                                baseRigidBody.velocity = (Vector3.up * 30f);
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
                        }
                        else if (skillId == "eren")
                        {
                            ShowSkillCD();
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
                                    if ((skillId == "eren") && (titanWhoGrabMe.GetComponent<MindlessTitan>() != null))
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
                        bool ReflectorVariable2;
                        bool ReflectorVariable1;
                        bool ReflectorVariable0;
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
                            if (!((!InputManager.KeyDown(InputHuman.Jump) || baseAnimation.IsPlaying("jump")) || baseAnimation.IsPlaying("horse_geton")))
                            {
                                Idle();
                                CrossFade("jump", 0.1f);
                                sparksEmission.enabled = false;
                            }
                            if (!((!InputManager.KeyDown(InputHorse.Mount) || baseAnimation.IsPlaying("jump")) || baseAnimation.IsPlaying("horse_geton")) && (((myHorse != null) && !isMounted) && (Vector3.Distance(myHorse.transform.position, transform.position) < 15f)))
                            {
                                GetOnHorse();
                            }
                            if (!((!InputManager.KeyDown(InputHuman.Dodge) || baseAnimation.IsPlaying("jump")) || baseAnimation.IsPlaying("horse_geton")))
                            {
                                Dodge(false);
                                return;
                            }
                        }
                        if (State == HERO_STATE.Idle)
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
                            if (((baseAnimation.IsPlaying(standAnimation) || !grounded) && InputManager.KeyDown(InputHuman.Reload)) && ((!useGun || (GameSettings.PvP.AhssAirReload.Value)) || grounded))
                            {
                                ChangeBlade();
                                return;
                            }
                            if (baseAnimation.IsPlaying(standAnimation) && InputManager.KeyDown(InputHuman.Salute))
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
                                        if (skillId == "eren")
                                        {
                                            ErenTransform();
                                            return;
                                        }
                                        if (skillId == "marco")
                                        {
                                            if (IsGrounded())
                                            {
                                                attackAnimation = (UnityEngine.Random.Range(0, 2) != 0) ? "special_marco_1" : "special_marco_0";
                                                PlayAnimation(attackAnimation);
                                            }
                                            else
                                            {
                                                flag3 = true;
                                                skillCDDuration = 0f;
                                            }
                                        }
                                        else if (skillId == "armin")
                                        {
                                            if (IsGrounded())
                                            {
                                                attackAnimation = "special_armin";
                                                PlayAnimation("special_armin");
                                            }
                                            else
                                            {
                                                flag3 = true;
                                                skillCDDuration = 0f;
                                            }
                                        }
                                        else if (skillId == "sasha")
                                        {
                                            if (IsGrounded())
                                            {
                                                attackAnimation = "special_sasha";
                                                PlayAnimation("special_sasha");
                                                currentBuff = BUFF.SpeedUp;
                                                buffTime = 10f;
                                            }
                                            else
                                            {
                                                flag3 = true;
                                                skillCDDuration = 0f;
                                            }
                                        }
                                        else if (skillId == "mikasa")
                                        {
                                            attackAnimation = "attack3_1";
                                            PlayAnimation("attack3_1");
                                            baseRigidBody.velocity = (Vector3.up * 10f);
                                        }
                                        else if (skillId == "levi")
                                        {
                                            RaycastHit hit;
                                            attackAnimation = "attack5";
                                            PlayAnimation("attack5");
                                            baseRigidBody.velocity += (Vector3.up * 5f);
                                            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                                            LayerMask mask = ((int)1) << LayerMask.NameToLayer("Ground");
                                            LayerMask mask2 = ((int)1) << LayerMask.NameToLayer("EnemyBox");
                                            LayerMask mask3 = mask2 | mask;
                                            if (Physics.Raycast(ray, out hit, 1E+07f, mask3.value))
                                            {
                                                if (bulletRight != null)
                                                {
                                                    bulletRight.GetComponent<Bullet>().disable();
                                                    releaseIfIHookSb();
                                                }
                                                dashDirection = hit.point - baseTransform.position;
                                                LaunchRightRope(hit.distance, hit.point, true);
                                                audioSystem.PlayOneShot(audioSystem.clipRope);
                                            }
                                            facingDirection = Mathf.Atan2(dashDirection.x, dashDirection.z) * Mathf.Rad2Deg;
                                            targetRotation = Quaternion.Euler(0f, facingDirection, 0f);
                                            attackLoop = 3;
                                        }
                                        else if (skillId == "petra")
                                        {
                                            RaycastHit hit2;
                                            attackAnimation = "special_petra";
                                            PlayAnimation("special_petra");
                                            baseRigidBody.velocity += (Vector3.up * 5f);
                                            Ray ray2 = Camera.main.ScreenPointToRay(Input.mousePosition);
                                            LayerMask mask4 = ((int)1) << LayerMask.NameToLayer("Ground");
                                            LayerMask mask5 = ((int)1) << LayerMask.NameToLayer("EnemyBox");
                                            LayerMask mask6 = mask5 | mask4;
                                            if (Physics.Raycast(ray2, out hit2, 1E+07f, mask6.value))
                                            {
                                                if (bulletRight != null)
                                                {
                                                    bulletRight.GetComponent<Bullet>().disable();
                                                    releaseIfIHookSb();
                                                }
                                                if (bulletLeft != null)
                                                {
                                                    bulletLeft.GetComponent<Bullet>().disable();
                                                    releaseIfIHookSb();
                                                }
                                                dashDirection = hit2.point - baseTransform.position;
                                                LaunchLeftRope(hit2.distance, hit2.point, true);
                                                LaunchRightRope(hit2.distance, hit2.point, true);
                                                audioSystem.PlayOneShot(audioSystem.clipRope);

                                            }
                                            facingDirection = Mathf.Atan2(dashDirection.x, dashDirection.z) * Mathf.Rad2Deg;
                                            targetRotation = Quaternion.Euler(0f, facingDirection, 0f);
                                            attackLoop = 3;
                                        }
                                        else
                                        {
                                            if (needLean)
                                            {
                                                if (leanLeft)
                                                {
                                                    attackAnimation = (UnityEngine.Random.Range(0, 100) >= 50) ? "attack1_hook_l1" : "attack1_hook_l2";
                                                }
                                                else
                                                {
                                                    attackAnimation = (UnityEngine.Random.Range(0, 100) >= 50) ? "attack1_hook_r1" : "attack1_hook_r2";
                                                }
                                            }
                                            else
                                            {
                                                attackAnimation = "attack1";
                                            }
                                            PlayAnimation(attackAnimation);
                                        }
                                    }
                                }
                                else if (InputManager.KeyDown(InputHuman.Attack))
                                {
                                    if (needLean)
                                    {
                                        if (InputManager.Key(InputHuman.Left))
                                        {
                                            attackAnimation = (UnityEngine.Random.Range(0, 100) >= 50) ? "attack1_hook_l1" : "attack1_hook_l2";
                                        }
                                        else if (InputManager.Key(InputHuman.Right))
                                        {
                                            attackAnimation = (UnityEngine.Random.Range(0, 100) >= 50) ? "attack1_hook_r1" : "attack1_hook_r2";
                                        }
                                        else if (leanLeft)
                                        {
                                            attackAnimation = (UnityEngine.Random.Range(0, 100) >= 50) ? "attack1_hook_l1" : "attack1_hook_l2";
                                        }
                                        else
                                        {
                                            attackAnimation = (UnityEngine.Random.Range(0, 100) >= 50) ? "attack1_hook_r1" : "attack1_hook_r2";
                                        }
                                    }
                                    else if (InputManager.Key(InputHuman.Left))
                                    {
                                        attackAnimation = "attack2";
                                    }
                                    else if (InputManager.Key(InputHuman.Right))
                                    {
                                        attackAnimation = "attack1";
                                    }
                                    else if (lastHook != null)
                                    {
                                        if (lastHook.Find("Amarture/Core/Controller_Body/hip/spine/chest/neck") != null)
                                        {
                                            AttackAccordingToTarget(lastHook.Find("Amarture/Core/Controller_Body/hip/spine/chest/neck"));
                                        }
                                        else
                                        {
                                            flag3 = true;
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
                                if (!flag3)
                                {
                                    checkBoxLeft.GetComponent<TriggerColliderWeapon>().ClearHits();
                                    checkBoxRight.GetComponent<TriggerColliderWeapon>().ClearHits();
                                    if (grounded)
                                    {
                                        baseRigidBody.AddForce((gameObject.transform.forward * 200f));
                                    }
                                    PlayAnimation(attackAnimation);
                                    baseAnimation[attackAnimation].time = 0f;
                                    buttonAttackRelease = false;
                                    State = HERO_STATE.Attack;
                                    if ((grounded || (attackAnimation == "attack3_1")) || ((attackAnimation == "attack5") || (attackAnimation == "special_petra")))
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
                                    RaycastHit hit3;
                                    Ray ray3 = Camera.main.ScreenPointToRay(Input.mousePosition);
                                    LayerMask mask7 = ((int)1) << LayerMask.NameToLayer("Ground");
                                    LayerMask mask8 = ((int)1) << LayerMask.NameToLayer("EnemyBox");
                                    LayerMask mask9 = mask8 | mask7;
                                    if (Physics.Raycast(ray3, out hit3, 1E+07f, mask9.value))
                                    {
                                        gunTarget = hit3.point;
                                    }
                                }
                                bool flag4 = false;
                                bool flag5 = false;
                                bool flag6 = false;
                                if (InputManager.KeyUp(InputHuman.AttackSpecial) && (skillId != "bomb"))
                                {
                                    if (leftGunHasBullet && rightGunHasBullet)
                                    {
                                        if (grounded)
                                        {
                                            attackAnimation = "AHSS_shoot_both";
                                        }
                                        else
                                        {
                                            attackAnimation = "AHSS_shoot_both_air";
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
                                                attackAnimation = "AHSS_shoot_r";
                                            }
                                            else
                                            {
                                                attackAnimation = "AHSS_shoot_l";
                                            }
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
                                        if (isLeftHandHooked)
                                        {
                                            attackAnimation = "AHSS_shoot_r_air";
                                        }
                                        else
                                        {
                                            attackAnimation = "AHSS_shoot_l_air";
                                        }
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
                                    gunDummy.transform.position = baseTransform.position;
                                    gunDummy.transform.rotation = baseTransform.rotation;
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
                        else if (State == HERO_STATE.Attack)
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
                                    else if (baseAnimation[attackAnimation].normalizedTime >= 0.32f)
                                    {
                                        PauseAnimation();
                                    }
                                }
                                if ((attackAnimation == "attack3_1") && (currentBladeSta > 0f))
                                {
                                    if (baseAnimation[attackAnimation].normalizedTime >= 0.8f)
                                    {
                                        if (!checkBoxLeft.GetComponent<TriggerColliderWeapon>().IsActive)
                                        {
                                            checkBoxLeft.GetComponent<TriggerColliderWeapon>().IsActive = true;
                                            if (((int)FengGameManagerMKII.settings[0x5c]) == 0)
                                            {
                                                /*
                                                leftbladetrail2.Activate();
                                                rightbladetrail2.Activate();
                                                leftbladetrail.Activate();
                                                rightbladetrail.Activate();
                                                */
                                            }
                                            baseRigidBody.velocity = (-Vector3.up * 30f);
                                        }
                                        if (!checkBoxRight.GetComponent<TriggerColliderWeapon>().IsActive)
                                        {
                                            checkBoxRight.GetComponent<TriggerColliderWeapon>().IsActive = true;
                                            audioSystem.PlayOneShot(audioSystem.clipSlash);
                                            
                                        }
                                    }
                                    else if (checkBoxLeft.GetComponent<TriggerColliderWeapon>().IsActive)
                                    {
                                        checkBoxLeft.GetComponent<TriggerColliderWeapon>().IsActive = false;
                                        checkBoxRight.GetComponent<TriggerColliderWeapon>().IsActive = false;
                                        checkBoxLeft.GetComponent<TriggerColliderWeapon>().ClearHits();
                                        checkBoxRight.GetComponent<TriggerColliderWeapon>().ClearHits();
                                        /*
                                        leftbladetrail.StopSmoothly(0.1f);
                                        rightbladetrail.StopSmoothly(0.1f);
                                        leftbladetrail2.StopSmoothly(0.1f);
                                        rightbladetrail2.StopSmoothly(0.1f);
                                        */
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
                                    else if (attackAnimation == "attack5")
                                    {
                                        num2 = 0.35f;
                                        num = 0.5f;
                                    }
                                    else if (attackAnimation == "special_petra")
                                    {
                                        num2 = 0.35f;
                                        num = 0.48f;
                                    }
                                    else if (attackAnimation == "special_armin")
                                    {
                                        num2 = 0.25f;
                                        num = 0.35f;
                                    }
                                    else if (attackAnimation == "attack4")
                                    {
                                        num2 = 0.6f;
                                        num = 0.9f;
                                    }
                                    else if (attackAnimation == "special_sasha")
                                    {
                                        num = -1f;
                                        num2 = -1f;
                                    }
                                    else
                                    {
                                        num2 = 0.5f;
                                        num = 0.85f;
                                    }
                                    if ((baseAnimation[attackAnimation].normalizedTime > num2) && (baseAnimation[attackAnimation].normalizedTime < num))
                                    {
                                        if (!checkBoxLeft.GetComponent<TriggerColliderWeapon>().IsActive)
                                        {
                                            checkBoxLeft.GetComponent<TriggerColliderWeapon>().IsActive = true;
                                            audioSystem.PlayOneShot(audioSystem.clipSlash);

                                            if (((int)FengGameManagerMKII.settings[0x5c]) == 0)
                                            {
                                                //leftbladetrail2.Activate();
                                                //rightbladetrail2.Activate();
                                                //leftbladetrail.Activate();
                                                //rightbladetrail.Activate();
                                            }
                                        }
                                        if (!checkBoxRight.GetComponent<TriggerColliderWeapon>().IsActive)
                                        {
                                            checkBoxRight.GetComponent<TriggerColliderWeapon>().IsActive = true;
                                        }
                                    }
                                    else if (checkBoxLeft.GetComponent<TriggerColliderWeapon>().IsActive)
                                    {
                                        checkBoxLeft.GetComponent<TriggerColliderWeapon>().IsActive = false;
                                        checkBoxRight.GetComponent<TriggerColliderWeapon>().IsActive = false;
                                        checkBoxLeft.GetComponent<TriggerColliderWeapon>().ClearHits();
                                        checkBoxRight.GetComponent<TriggerColliderWeapon>().ClearHits();
                                        //leftbladetrail2.StopSmoothly(0.1f);
                                        //rightbladetrail2.StopSmoothly(0.1f);
                                        //leftbladetrail.StopSmoothly(0.1f);
                                        //rightbladetrail.StopSmoothly(0.1f);
                                    }
                                    if ((attackLoop > 0) && (baseAnimation[attackAnimation].normalizedTime > num))
                                    {
                                        attackLoop--;
                                        playAnimationAt(attackAnimation, num2);
                                    }
                                }
                                if (baseAnimation[attackAnimation].normalizedTime >= 1f)
                                {
                                    if ((attackAnimation == "special_marco_0") || (attackAnimation == "special_marco_1"))
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
                                    else if (attackAnimation == "special_armin")
                                    {
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
                                    }
                                    else if (attackAnimation == "attack3_1")
                                    {
                                        baseRigidBody.velocity -= ((Vector3.up * Time.deltaTime) * 30f);
                                    }
                                    else
                                    {
                                        FalseAttack();
                                        Idle();
                                    }
                                }
                                if (baseAnimation.IsPlaying("attack3_2") && (baseAnimation["attack3_2"].normalizedTime >= 1f))
                                {
                                    FalseAttack();
                                    Idle();
                                }
                            }
                            else
                            {
                                checkBoxLeft.GetComponent<TriggerColliderWeapon>().IsActive = false;
                                checkBoxRight.GetComponent<TriggerColliderWeapon>().IsActive = false;
                                baseTransform.rotation = Quaternion.Lerp(baseTransform.rotation, gunDummy.transform.rotation, Time.deltaTime * 30f);
                                if (!attackReleased && (baseAnimation[attackAnimation].normalizedTime > 0.167f))
                                {
                                    GameObject obj4;
                                    attackReleased = true;
                                    bool flag7 = false;
                                    if ((attackAnimation == "AHSS_shoot_both") || (attackAnimation == "AHSS_shoot_both_air"))
                                    {
                                        //Should use AHSSShotgunCollider instead of TriggerColliderWeapon.  
                                        //Apply that change when abstracting weapons from this class.
                                        //Note, when doing the abstraction, the relationship between the weapon collider and the abstracted weapon class should be carefully considered.
                                        checkBoxLeft.GetComponent<TriggerColliderWeapon>().IsActive = true;
                                        checkBoxRight.GetComponent<TriggerColliderWeapon>().IsActive = true;
                                        flag7 = true;
                                        leftGunHasBullet = false;
                                        rightGunHasBullet = false;
                                        baseRigidBody.AddForce((-baseTransform.forward * 1000f), ForceMode.Acceleration);
                                    }
                                    else
                                    {
                                        if ((attackAnimation == "AHSS_shoot_l") || (attackAnimation == "AHSS_shoot_l_air"))
                                        {
                                            checkBoxLeft.GetComponent<TriggerColliderWeapon>().IsActive = true;
                                            leftGunHasBullet = false;
                                        }
                                        else
                                        {
                                            checkBoxRight.GetComponent<TriggerColliderWeapon>().IsActive = true;
                                            rightGunHasBullet = false;
                                        }
                                        baseRigidBody.AddForce((-baseTransform.forward * 600f), ForceMode.Acceleration);
                                    }
                                    baseRigidBody.AddForce((Vector3.up * 200f), ForceMode.Acceleration);
                                    string prefabName = "FX/shotGun";
                                    if (flag7)
                                    {
                                        prefabName = "FX/shotGun 1";
                                    }
                                    if (photonView.isMine)
                                    {
                                        obj4 = PhotonNetwork.Instantiate(prefabName, ((baseTransform.position + (baseTransform.up * 0.8f)) - (baseTransform.right * 0.1f)), baseTransform.rotation, 0);
                                        if (obj4.GetComponent<EnemyfxIDcontainer>() != null)
                                        {
                                            obj4.GetComponent<EnemyfxIDcontainer>().myOwnerViewID = photonView.viewID;
                                        }
                                    }
                                    else
                                    {
                                        obj4 = (GameObject)Instantiate(Resources.Load(prefabName), ((baseTransform.position + (baseTransform.up * 0.8f)) - (baseTransform.right * 0.1f)), baseTransform.rotation);
                                    }
                                }
                                if (baseAnimation[attackAnimation].normalizedTime >= 1f)
                                {
                                    FalseAttack();
                                    Idle();
                                    checkBoxLeft.GetComponent<TriggerColliderWeapon>().IsActive = false;
                                    checkBoxRight.GetComponent<TriggerColliderWeapon>().IsActive = false;
                                }
                                if (!baseAnimation.IsPlaying(attackAnimation))
                                {
                                    FalseAttack();
                                    Idle();
                                    checkBoxLeft.GetComponent<TriggerColliderWeapon>().IsActive = false;
                                    checkBoxRight.GetComponent<TriggerColliderWeapon>().IsActive = false;
                                }
                            }
                        }
                        else if (State == HERO_STATE.ChangeBlade)
                        {
                            Equipment.Weapon.Reload();
                            if (baseAnimation[reloadAnimation].normalizedTime >= 1f)
                            {
                                Idle();
                            }
                        }
                        else if (State == HERO_STATE.Salute)
                        {
                            if (baseAnimation["salute"].normalizedTime >= 1f)
                            {
                                Idle();
                            }
                        }
                        else if (State == HERO_STATE.GroundDodge)
                        {
                            if (baseAnimation.IsPlaying("dodge"))
                            {
                                if (!(grounded || (baseAnimation["dodge"].normalizedTime <= 0.6f)))
                                {
                                    Idle();
                                }
                                if (baseAnimation["dodge"].normalizedTime >= 1f)
                                {
                                    Idle();
                                }
                            }
                        }
                        else if (State == HERO_STATE.Land)
                        {
                            if (baseAnimation.IsPlaying("dash_land") && (baseAnimation["dash_land"].normalizedTime >= 1f))
                            {
                                Idle();
                            }
                        }
                        else if (State == HERO_STATE.FillGas)
                        {
                            if (baseAnimation.IsPlaying("supply") && (baseAnimation["supply"].normalizedTime >= 1f))
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
                        else if (State == HERO_STATE.Slide)
                        {
                            if (!grounded)
                            {
                                Idle();
                            }
                        }
                        else if (State == HERO_STATE.AirDodge)
                        {
                            if (dashTime > 0f)
                            {
                                dashTime -= Time.deltaTime;
                                if (currentSpeed > originVM)
                                {
                                    baseRigidBody.AddForce(((-baseRigidBody.velocity * Time.deltaTime) * 1.7f), ForceMode.VelocityChange);
                                }
                            }
                            else
                            {
                                dashTime = 0f;
                                Idle();
                            }
                        }
                        if (InputManager.Key(InputHuman.HookLeft))
                        {
                            ReflectorVariable0 = true;
                        }
                        else
                        {
                            ReflectorVariable0 = false;
                        }
                        if (!(ReflectorVariable0 ? (((baseAnimation.IsPlaying("attack3_1") || baseAnimation.IsPlaying("attack5")) || (baseAnimation.IsPlaying("special_petra") || (State == HERO_STATE.Grab))) ? (State != HERO_STATE.Idle) : false) : true))
                        {
                            if (bulletLeft != null)
                            {
                                qHold = true;
                            }
                            else
                            {
                                RaycastHit hit4;
                                Ray ray4 = Camera.main.ScreenPointToRay(Input.mousePosition);
                                LayerMask mask10 = ((int)1) << LayerMask.NameToLayer("Ground");
                                LayerMask mask11 = ((int)1) << LayerMask.NameToLayer("EnemyBox");
                                LayerMask mask12 = mask11 | mask10;
                                if (Physics.Raycast(ray4, out hit4, HookRaycastDistance, mask12.value))
                                {
                                    LaunchLeftRope(hit4.distance, hit4.point, true);
                                }
                                else
                                {
                                    LaunchLeftRope(HookRaycastDistance, ray4.GetPoint(HookRaycastDistance), true);
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
                            ReflectorVariable1 = true;
                        }
                        else
                        {
                            ReflectorVariable1 = false;
                        }
                        if (!(ReflectorVariable1 ? (((baseAnimation.IsPlaying("attack3_1") || baseAnimation.IsPlaying("attack5")) || (baseAnimation.IsPlaying("special_petra") || (State == HERO_STATE.Grab))) ? (State != HERO_STATE.Idle) : false) : true))
                        {
                            if (bulletRight != null)
                            {
                                eHold = true;
                            }
                            else
                            {
                                RaycastHit hit5;
                                Ray ray5 = Camera.main.ScreenPointToRay(Input.mousePosition);
                                LayerMask mask13 = ((int)1) << LayerMask.NameToLayer("Ground");
                                LayerMask mask14 = ((int)1) << LayerMask.NameToLayer("EnemyBox");
                                LayerMask mask15 = mask14 | mask13;
                                if (Physics.Raycast(ray5, out hit5, HookRaycastDistance, mask15.value))
                                {
                                    LaunchRightRope(hit5.distance, hit5.point, true);
                                }
                                else
                                {
                                    LaunchRightRope(HookRaycastDistance, ray5.GetPoint(HookRaycastDistance), true);
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
                            ReflectorVariable2 = true;
                        }
                        else
                        {
                            ReflectorVariable2 = false;
                        }
                        if (!(ReflectorVariable2 ? (((baseAnimation.IsPlaying("attack3_1") || baseAnimation.IsPlaying("attack5")) || (baseAnimation.IsPlaying("special_petra") || (State == HERO_STATE.Grab))) ? (State != HERO_STATE.Idle) : false) : true))
                        {
                            qHold = true;
                            eHold = true;
                            if ((bulletLeft == null) && (bulletRight == null))
                            {
                                RaycastHit hit6;
                                Ray ray6 = Camera.main.ScreenPointToRay(Input.mousePosition);
                                LayerMask mask16 = ((int)1) << LayerMask.NameToLayer("Ground");
                                LayerMask mask17 = ((int)1) << LayerMask.NameToLayer("EnemyBox");
                                LayerMask mask18 = mask17 | mask16;
                                if (Physics.Raycast(ray6, out hit6, HookRaycastDistance, mask18.value))
                                {
                                    LaunchLeftRope(hit6.distance, hit6.point, false);
                                    LaunchRightRope(hit6.distance, hit6.point, false);
                                }
                                else
                                {
                                    LaunchLeftRope(HookRaycastDistance, ray6.GetPoint(HookRaycastDistance), false);
                                    LaunchRightRope(HookRaycastDistance, ray6.GetPoint(HookRaycastDistance), false);
                                }
                                audioSystem.PlayOneShot(audioSystem.clipRope);

                            }
                        }
                        if (!IN_GAME_MAIN_CAMERA.isPausing)
                        {
                            CalcSkillCD();
                            CalcFlareCD();
                        }
                        //if (!useGun)
                        //{
                        //    if (leftbladetrail.gameObject.GetActive())
                        //    {
                        //        leftbladetrail.update();
                        //        rightbladetrail.update();
                        //    }
                        //    if (leftbladetrail2.gameObject.GetActive())
                        //    {
                        //        leftbladetrail2.update();
                        //        rightbladetrail2.update();
                        //    }
                        //    if (leftbladetrail.gameObject.GetActive())
                        //    {
                        //        leftbladetrail.lateUpdate();
                        //        rightbladetrail.lateUpdate();
                        //    }
                        //    if (leftbladetrail2.gameObject.GetActive())
                        //    {
                        //        leftbladetrail2.lateUpdate();
                        //        rightbladetrail2.lateUpdate();
                        //    }
                        //}
                        if (!IN_GAME_MAIN_CAMERA.isPausing)
                        {
                            //showSkillCD();
                            //showFlareCD2();
                            ShowGas2();
                            ShowAimUI2();
                        }
                    }
                    else if (isCannon && !IN_GAME_MAIN_CAMERA.isPausing)
                    {
                        //showAimUI2();
                        //calcSkillCD();
                        //showSkillCD();
                    }
                }
            }
        }
    }

    public void UpdateCannon()
    {
        baseTransform.position = myCannonPlayer.position;
        baseTransform.rotation = myCannonBase.rotation;
    }

    public void UpdateExt()
    {
        if (skillId == "bomb")
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
                LayerMask mask = ((int)1) << LayerMask.NameToLayer("Ground");
                LayerMask mask2 = ((int)1) << LayerMask.NameToLayer("EnemyBox");
                LayerMask mask3 = mask2 | mask;
                currentV = baseTransform.position;
                targetV = currentV + ((Vector3.forward * 200f));
                if (Physics.Raycast(ray, out hitInfo, 1000000f, mask3.value))
                {
                    targetV = hitInfo.point;
                }
                Vector3 vector = Vector3.Normalize(targetV - currentV);
                GameObject obj2 = PhotonNetwork.Instantiate("RCAsset/BombMain", currentV + ((vector * 4f)), new Quaternion(0f, 0f, 0f, 1f), 0);
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


    private void UpdateLeftMagUI()
    {
        throw new NotImplementedException($"The Method {nameof(UpdateLeftMagUI)} is not implemented");

        for (int i = 1; i <= bulletMAX; i++)
        {
            //GameObject.Find("bulletL" + i).GetComponent<UISprite>().enabled = false;
        }
        for (int j = 1; j <= leftBulletLeft; j++)
        {
            //GameObject.Find("bulletL" + j).GetComponent<UISprite>().enabled = true;
        }
    }


    private void UpdateRightMagUI()
    {
        throw new NotImplementedException($"The Method {nameof(UpdateRightMagUI)} is not implemented");
        for (int i = 1; i <= bulletMAX; i++)
        {
            //GameObject.Find("bulletR" + i).GetComponent<UISprite>().enabled = false;
        }
        for (int j = 1; j <= rightBulletLeft; j++)
        {
            //GameObject.Find("bulletR" + j).GetComponent<UISprite>().enabled = true;
        }
    }

    [Obsolete("Using a weapon should be moved within Weapon class...")]


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
        eren_titan = PhotonView.Find(id).gameObject;
        titanForm = true;
    }

    public bool IsGrabbed
    {
        get
        {
            return (State == HERO_STATE.Grab);
        }
    }

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
}