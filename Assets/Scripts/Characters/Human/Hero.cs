using Assets.Scripts.Gamemode.Options;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Xft;

public class Hero : Human
{
    public Equipment Equipment { get; set; }
    public EquipmentType EquipmentType;

    public List<HeroSkill> Skills;

    public HERO_STATE _state;
    private bool almostSingleHook;
    private string attackAnimation;
    private int attackLoop;
    private bool attackMove;
    private bool attackReleased;
    public AudioSource audio_ally;
    public AudioSource audio_hitwall;
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
    private int bulletMAX = 7;
    public GameObject bulletRight;
    private bool buttonAttackRelease;
    public Dictionary<string, Image> cachedSprites;
    public float CameraMultiplier;
    public bool canJump = true;
    public GameObject checkBoxLeft;
    public GameObject checkBoxRight;
    public GameObject cross1;
    public GameObject cross2;
    public GameObject crossL1;
    public GameObject crossL2;
    public GameObject crossR1;
    public GameObject crossR2;
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
    private bool EHold;
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
    public FengCustomInputs inputManager;
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
    public Text LabelDistance;
    public Transform lastHook;
    private float launchElapsedTimeL;
    private float launchElapsedTimeR;
    private Vector3 launchForce;
    private Vector3 launchPointLeft;
    private Vector3 launchPointRight;
    private bool leanLeft;
    private bool leftArmAim;
    public XWeaponTrail leftbladetrail;
    public XWeaponTrail leftbladetrail2;
    [Obsolete]
    public int leftBulletLeft = 7;
    public bool leftGunHasBullet = true;
    private float lTapTime = -1f;
    public GameObject maincamera;
    public float maxVelocityChange = 10f;
    public AudioSource meatDie;
    public Bomb myBomb;
    public GameObject myCannon;
    public Transform myCannonBase;
    public Transform myCannonPlayer;
    public CannonPropRegion myCannonRegion;
    public GROUP myGroup;
    private GameObject myHorse;
    public GameObject myNetWorkName;
    public float myScale = 1f;
    public int myTeam = 1;
    public List<TITAN> myTitans;
    private bool needLean;
    private Quaternion oldHeadRotation;
    private float originVM;
    private bool QHold;
    public string reloadAnimation = string.Empty;
    private bool rightArmAim;
    public XWeaponTrail rightbladetrail;
    public XWeaponTrail rightbladetrail2;
    [Obsolete]
    public int rightBulletLeft = 7;
    public bool rightGunHasBullet = true;
    public AudioSource rope;
    private float rTapTime = -1f;
    public HERO_SETUP setup;
    private GameObject skillCD;
    public float skillCDDuration;
    public float skillCDLast;
    public float skillCDLastCannon;
    private string skillId;
    public string skillIDHUD;
    public AudioSource slash;
    public AudioSource slashHit;
    private ParticleSystem smoke_3dmg;
    private ParticleSystem sparks;
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

    private void applyForceToBody(GameObject GO, Vector3 v)
    {
        GO.GetComponent<Rigidbody>().AddForce(v);
        GO.GetComponent<Rigidbody>().AddTorque(UnityEngine.Random.Range((float)-10f, (float)10f), UnityEngine.Random.Range((float)-10f, (float)10f), UnityEngine.Random.Range((float)-10f, (float)10f));
    }

    public void attackAccordingToMouse()
    {
        if (Input.mousePosition.x < (Screen.width * 0.5))
        {
            this.attackAnimation = "attack2";
        }
        else
        {
            this.attackAnimation = "attack1";
        }
    }

    public void attackAccordingToTarget(Transform a)
    {
        Vector3 vector = a.position - base.transform.position;
        float current = -Mathf.Atan2(vector.z, vector.x) * 57.29578f;
        float f = -Mathf.DeltaAngle(current, base.transform.rotation.eulerAngles.y - 90f);
        if (((Mathf.Abs(f) < 90f) && (vector.magnitude < 6f)) && ((a.position.y <= (base.transform.position.y + 2f)) && (a.position.y >= (base.transform.position.y - 5f))))
        {
            this.attackAnimation = "attack4";
        }
        else if (f > 0f)
        {
            this.attackAnimation = "attack1";
        }
        else
        {
            this.attackAnimation = "attack2";
        }
    }

    private void Awake()
    {
        InGameUI = GameObject.Find("InGameUi");
        this.cache();
        this.setup = base.gameObject.GetComponent<HERO_SETUP>();
        this.baseRigidBody.freezeRotation = true;
        this.baseRigidBody.useGravity = false;
        this.handL = this.baseTransform.Find("Amarture/Controller_Body/hip/spine/chest/shoulder_L/upper_arm_L/forearm_L/hand_L");
        this.handR = this.baseTransform.Find("Amarture/Controller_Body/hip/spine/chest/shoulder_R/upper_arm_R/forearm_R/hand_R");
        this.forearmL = this.baseTransform.Find("Amarture/Controller_Body/hip/spine/chest/shoulder_L/upper_arm_L/forearm_L");
        this.forearmR = this.baseTransform.Find("Amarture/Controller_Body/hip/spine/chest/shoulder_R/upper_arm_R/forearm_R");
        this.upperarmL = this.baseTransform.Find("Amarture/Controller_Body/hip/spine/chest/shoulder_L/upper_arm_L");
        this.upperarmR = this.baseTransform.Find("Amarture/Controller_Body/hip/spine/chest/shoulder_R/upper_arm_R");
        Equipment = gameObject.AddComponent<Equipment>();
    }

    public void backToHuman()
    {
        base.gameObject.GetComponent<SmoothSyncMovement>().disabled = false;
        base.GetComponent<Rigidbody>().velocity = Vector3.zero;
        this.titanForm = false;
        this.ungrabbed();
        this.falseAttack();
        this.skillCDDuration = this.skillCDLast;
        GameObject.Find("MainCamera").GetComponent<IN_GAME_MAIN_CAMERA>().setMainObject(base.gameObject, true, false);
        if (IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.SINGLE)
        {
            base.photonView.RPC("backToHumanRPC", PhotonTargets.Others, new object[0]);
        }
    }

    [PunRPC]
    private void backToHumanRPC()
    {
        this.titanForm = false;
        this.eren_titan = null;
        base.gameObject.GetComponent<SmoothSyncMovement>().disabled = false;
    }

    [PunRPC]
    public void badGuyReleaseMe()
    {
        this.hookBySomeOne = false;
        this.badGuy = null;
    }

    [PunRPC]
    public void blowAway(Vector3 force)
    {
        if ((IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE) || base.photonView.isMine)
        {
            base.GetComponent<Rigidbody>().AddForce(force, ForceMode.Impulse);
            base.transform.LookAt(base.transform.position);
        }
    }

    private void bodyLean()
    {
        if ((IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE) || base.photonView.isMine)
        {
            float z = 0f;
            this.needLean = false;
            if ((!this.useGun && (this.state == HERO_STATE.Attack)) && ((this.attackAnimation != "attack3_1") && (this.attackAnimation != "attack3_2")))
            {
                float y = base.GetComponent<Rigidbody>().velocity.y;
                float x = base.GetComponent<Rigidbody>().velocity.x;
                float num4 = base.GetComponent<Rigidbody>().velocity.z;
                float num5 = Mathf.Sqrt((x * x) + (num4 * num4));
                float num6 = Mathf.Atan2(y, num5) * 57.29578f;
                this.targetRotation = Quaternion.Euler(-num6 * (1f - (Vector3.Angle(base.GetComponent<Rigidbody>().velocity, base.transform.forward) / 90f)), this.facingDirection, 0f);
                if ((this.isLeftHandHooked && (this.bulletLeft != null)) || (this.isRightHandHooked && (this.bulletRight != null)))
                {
                    base.transform.rotation = this.targetRotation;
                }
            }
            else
            {
                if ((this.isLeftHandHooked && (this.bulletLeft != null)) && (this.isRightHandHooked && (this.bulletRight != null)))
                {
                    if (this.almostSingleHook)
                    {
                        this.needLean = true;
                        z = this.getLeanAngle(this.bulletRight.transform.position, true);
                    }
                }
                else if (this.isLeftHandHooked && (this.bulletLeft != null))
                {
                    this.needLean = true;
                    z = this.getLeanAngle(this.bulletLeft.transform.position, true);
                }
                else if (this.isRightHandHooked && (this.bulletRight != null))
                {
                    this.needLean = true;
                    z = this.getLeanAngle(this.bulletRight.transform.position, false);
                }
                if (this.needLean)
                {
                    float a = 0f;
                    if (!this.useGun && (this.state != HERO_STATE.Attack))
                    {
                        a = this.currentSpeed * 0.1f;
                        a = Mathf.Min(a, 20f);
                    }
                    this.targetRotation = Quaternion.Euler(-a, this.facingDirection, z);
                }
                else if (this.state != HERO_STATE.Attack)
                {
                    this.targetRotation = Quaternion.Euler(0f, this.facingDirection, 0f);
                }
            }
        }
    }

    public void bombInit()
    {
        this.skillIDHUD = this.skillId;
        this.skillCDDuration = this.skillCDLast;
        if (FengGameManagerMKII.Gamemode.PvPBomb)
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
            this.bombTimeMax = ((num2 * 60f) + 200f) / ((num3 * 60f) + 200f);
            this.bombRadius = (num * 4f) + 20f;
            this.bombCD = (num4 * -0.4f) + 5f;
            this.bombSpeed = (num3 * 60f) + 200f;
            ExitGames.Client.Photon.Hashtable propertiesToSet = new ExitGames.Client.Photon.Hashtable();
            propertiesToSet.Add(PhotonPlayerProperty.RCBombR, (float)FengGameManagerMKII.settings[0xf6]);
            propertiesToSet.Add(PhotonPlayerProperty.RCBombG, (float)FengGameManagerMKII.settings[0xf7]);
            propertiesToSet.Add(PhotonPlayerProperty.RCBombB, (float)FengGameManagerMKII.settings[0xf8]);
            propertiesToSet.Add(PhotonPlayerProperty.RCBombA, (float)FengGameManagerMKII.settings[0xf9]);
            propertiesToSet.Add(PhotonPlayerProperty.RCBombRadius, this.bombRadius);
            PhotonNetwork.player.SetCustomProperties(propertiesToSet);
            this.skillId = "bomb";
            this.skillIDHUD = "armin";
            this.skillCDLast = this.bombCD;
            this.skillCDDuration = 10f;
            if (FengGameManagerMKII.instance.roundTime > 10f)
            {
                this.skillCDDuration = 5f;
            }
        }
    }

    private void breakApart2(Vector3 v, bool isBite)
    {
        GameObject obj6;
        GameObject obj7;
        GameObject obj8;
        GameObject obj9;
        GameObject obj10;
        GameObject obj2 = (GameObject)UnityEngine.Object.Instantiate(Resources.Load("Character_parts/AOTTG_HERO_body"), base.transform.position, base.transform.rotation);
        obj2.gameObject.GetComponent<HERO_SETUP>().myCostume = this.setup.myCostume;
        obj2.GetComponent<HERO_SETUP>().isDeadBody = true;
        obj2.GetComponent<HERO_DEAD_BODY_SETUP>().init(this.currentAnimation, base.GetComponent<Animation>()[this.currentAnimation].normalizedTime, BODY_PARTS.ARM_R);
        if (!isBite)
        {
            GameObject gO = (GameObject)UnityEngine.Object.Instantiate(Resources.Load("Character_parts/AOTTG_HERO_body"), base.transform.position, base.transform.rotation);
            GameObject obj4 = (GameObject)UnityEngine.Object.Instantiate(Resources.Load("Character_parts/AOTTG_HERO_body"), base.transform.position, base.transform.rotation);
            GameObject obj5 = (GameObject)UnityEngine.Object.Instantiate(Resources.Load("Character_parts/AOTTG_HERO_body"), base.transform.position, base.transform.rotation);
            gO.gameObject.GetComponent<HERO_SETUP>().myCostume = this.setup.myCostume;
            obj4.gameObject.GetComponent<HERO_SETUP>().myCostume = this.setup.myCostume;
            obj5.gameObject.GetComponent<HERO_SETUP>().myCostume = this.setup.myCostume;
            gO.GetComponent<HERO_SETUP>().isDeadBody = true;
            obj4.GetComponent<HERO_SETUP>().isDeadBody = true;
            obj5.GetComponent<HERO_SETUP>().isDeadBody = true;
            gO.GetComponent<HERO_DEAD_BODY_SETUP>().init(this.currentAnimation, base.GetComponent<Animation>()[this.currentAnimation].normalizedTime, BODY_PARTS.UPPER);
            obj4.GetComponent<HERO_DEAD_BODY_SETUP>().init(this.currentAnimation, base.GetComponent<Animation>()[this.currentAnimation].normalizedTime, BODY_PARTS.LOWER);
            obj5.GetComponent<HERO_DEAD_BODY_SETUP>().init(this.currentAnimation, base.GetComponent<Animation>()[this.currentAnimation].normalizedTime, BODY_PARTS.ARM_L);
            this.applyForceToBody(gO, v);
            this.applyForceToBody(obj4, v);
            this.applyForceToBody(obj5, v);
            if ((IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE) || base.photonView.isMine)
            {
                this.currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().setMainObject(gO, false, false);
            }
        }
        else if ((IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE) || base.photonView.isMine)
        {
            this.currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().setMainObject(obj2, false, false);
        }
        this.applyForceToBody(obj2, v);
        Transform transform = base.transform.Find("Amarture/Controller_Body/hip/spine/chest/shoulder_L/upper_arm_L/forearm_L/hand_L").transform;
        Transform transform2 = base.transform.Find("Amarture/Controller_Body/hip/spine/chest/shoulder_R/upper_arm_R/forearm_R/hand_R").transform;
        if (this.useGun)
        {
            obj6 = (GameObject)UnityEngine.Object.Instantiate(Resources.Load("Character_parts/character_gun_l"), transform.position, transform.rotation);
            obj7 = (GameObject)UnityEngine.Object.Instantiate(Resources.Load("Character_parts/character_gun_r"), transform2.position, transform2.rotation);
            obj8 = (GameObject)UnityEngine.Object.Instantiate(Resources.Load("Character_parts/character_3dmg_2"), base.transform.position, base.transform.rotation);
            obj9 = (GameObject)UnityEngine.Object.Instantiate(Resources.Load("Character_parts/character_gun_mag_l"), base.transform.position, base.transform.rotation);
            obj10 = (GameObject)UnityEngine.Object.Instantiate(Resources.Load("Character_parts/character_gun_mag_r"), base.transform.position, base.transform.rotation);
        }
        else
        {
            obj6 = (GameObject)UnityEngine.Object.Instantiate(Resources.Load("Character_parts/character_blade_l"), transform.position, transform.rotation);
            obj7 = (GameObject)UnityEngine.Object.Instantiate(Resources.Load("Character_parts/character_blade_r"), transform2.position, transform2.rotation);
            obj8 = (GameObject)UnityEngine.Object.Instantiate(Resources.Load("Character_parts/character_3dmg"), base.transform.position, base.transform.rotation);
            obj9 = (GameObject)UnityEngine.Object.Instantiate(Resources.Load("Character_parts/character_3dmg_gas_l"), base.transform.position, base.transform.rotation);
            obj10 = (GameObject)UnityEngine.Object.Instantiate(Resources.Load("Character_parts/character_3dmg_gas_r"), base.transform.position, base.transform.rotation);
        }
        obj6.GetComponent<Renderer>().material = CharacterMaterials.materials[this.setup.myCostume._3dmg_texture];
        obj7.GetComponent<Renderer>().material = CharacterMaterials.materials[this.setup.myCostume._3dmg_texture];
        obj8.GetComponent<Renderer>().material = CharacterMaterials.materials[this.setup.myCostume._3dmg_texture];
        obj9.GetComponent<Renderer>().material = CharacterMaterials.materials[this.setup.myCostume._3dmg_texture];
        obj10.GetComponent<Renderer>().material = CharacterMaterials.materials[this.setup.myCostume._3dmg_texture];
        this.applyForceToBody(obj6, v);
        this.applyForceToBody(obj7, v);
        this.applyForceToBody(obj8, v);
        this.applyForceToBody(obj9, v);
        this.applyForceToBody(obj10, v);
    }

    private void bufferUpdate()
    {
        if (this.buffTime > 0f)
        {
            this.buffTime -= Time.deltaTime;
            if (this.buffTime <= 0f)
            {
                this.buffTime = 0f;
                if ((this.currentBuff == BUFF.SpeedUp) && base.GetComponent<Animation>().IsPlaying("run_sasha"))
                {
                    this.crossFade("run_1", 0.1f);
                }
                this.currentBuff = BUFF.NoBuff;
            }
        }
    }

    public void cache()
    {
        this.baseTransform = base.transform;
        this.baseRigidBody = base.GetComponent<Rigidbody>();
        this.maincamera = GameObject.Find("MainCamera");
        if ((IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE) || base.photonView.isMine)
        {
            this.baseAnimation = base.GetComponent<Animation>();
            this.cross1 = GameObject.Find("cross1");
            this.cross2 = GameObject.Find("cross2");
            this.crossL1 = GameObject.Find("crossL1");
            this.crossL2 = GameObject.Find("crossL2");
            this.crossR1 = GameObject.Find("crossR1");
            this.crossR2 = GameObject.Find("crossR2");
            this.LabelDistance = GameObject.Find("Distance").GetComponent<Text>();
            this.cachedSprites = new Dictionary<string, Image>();
            //foreach (GameObject obj2 in UnityEngine.Object.FindObjectsOfType(typeof(GameObject)))
            //{
            //    if ((obj2.GetComponent<UISprite>() != null) && obj2.activeInHierarchy)
            //    {
            //        string name = obj2.name;
            //        if (!((((name.Contains("blade") || name.Contains("bullet")) || (name.Contains("gas") || name.Contains("flare"))) || name.Contains("skill_cd")) ? this.cachedSprites.ContainsKey(name) : true))
            //        {
            //            this.cachedSprites.Add(name, obj2.GetComponent<UISprite>());
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

    private void calcFlareCD()
    {
        if (this.flare1CD > 0f)
        {
            this.flare1CD -= Time.deltaTime;
            if (this.flare1CD < 0f)
            {
                this.flare1CD = 0f;
            }
        }
        if (this.flare2CD > 0f)
        {
            this.flare2CD -= Time.deltaTime;
            if (this.flare2CD < 0f)
            {
                this.flare2CD = 0f;
            }
        }
        if (this.flare3CD > 0f)
        {
            this.flare3CD -= Time.deltaTime;
            if (this.flare3CD < 0f)
            {
                this.flare3CD = 0f;
            }
        }
    }

    private void calcSkillCD()
    {
        if (this.skillCDDuration > 0f)
        {
            this.skillCDDuration -= Time.deltaTime;
            if (this.skillCDDuration < 0f)
            {
                this.skillCDDuration = 0f;
            }
        }
    }

    private float CalculateJumpVerticalSpeed()
    {
        return Mathf.Sqrt((2f * this.jumpHeight) * this.gravity);
    }

    private void changeBlade()
    {
        if ((!this.useGun || this.grounded) || FengGameManagerMKII.Gamemode.AhssAirReload)
        {
            this.state = HERO_STATE.ChangeBlade;
            this.throwedBlades = false;
            Equipment.Weapon.PlayReloadAnimation();
        }
    }

    private void checkDashDoubleTap()
    {
        if (this.uTapTime >= 0f)
        {
            this.uTapTime += Time.deltaTime;
            if (this.uTapTime > 0.2f)
            {
                this.uTapTime = -1f;
            }
        }
        if (this.dTapTime >= 0f)
        {
            this.dTapTime += Time.deltaTime;
            if (this.dTapTime > 0.2f)
            {
                this.dTapTime = -1f;
            }
        }
        if (this.lTapTime >= 0f)
        {
            this.lTapTime += Time.deltaTime;
            if (this.lTapTime > 0.2f)
            {
                this.lTapTime = -1f;
            }
        }
        if (this.rTapTime >= 0f)
        {
            this.rTapTime += Time.deltaTime;
            if (this.rTapTime > 0.2f)
            {
                this.rTapTime = -1f;
            }
        }
        if (this.inputManager.isInputDown[InputCode.up])
        {
            if (this.uTapTime == -1f)
            {
                this.uTapTime = 0f;
            }
            if (this.uTapTime != 0f)
            {
                this.dashU = true;
            }
        }
        if (this.inputManager.isInputDown[InputCode.down])
        {
            if (this.dTapTime == -1f)
            {
                this.dTapTime = 0f;
            }
            if (this.dTapTime != 0f)
            {
                this.dashD = true;
            }
        }
        if (this.inputManager.isInputDown[InputCode.left])
        {
            if (this.lTapTime == -1f)
            {
                this.lTapTime = 0f;
            }
            if (this.lTapTime != 0f)
            {
                this.dashL = true;
            }
        }
        if (this.inputManager.isInputDown[InputCode.right])
        {
            if (this.rTapTime == -1f)
            {
                this.rTapTime = 0f;
            }
            if (this.rTapTime != 0f)
            {
                this.dashR = true;
            }
        }
    }

    private void checkDashRebind()
    {
        if (FengGameManagerMKII.inputRC.isInputHuman(InputCodeRC.dash))
        {
            if (this.inputManager.isInput[InputCode.up])
            {
                this.dashU = true;
            }
            else if (this.inputManager.isInput[InputCode.down])
            {
                this.dashD = true;
            }
            else if (this.inputManager.isInput[InputCode.left])
            {
                this.dashL = true;
            }
            else if (this.inputManager.isInput[InputCode.right])
            {
                this.dashR = true;
            }
        }
    }

    public void checkTitan()
    {
        int count;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        LayerMask mask = ((int)1) << LayerMask.NameToLayer("PlayerAttackBox");
        LayerMask mask2 = ((int)1) << LayerMask.NameToLayer("Ground");
        LayerMask mask3 = ((int)1) << LayerMask.NameToLayer("EnemyBox");
        LayerMask mask4 = (mask | mask2) | mask3;
        RaycastHit[] hitArray = Physics.RaycastAll(ray, 180f, mask4.value);
        List<RaycastHit> list = new List<RaycastHit>();
        List<TITAN> list2 = new List<TITAN>();
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
                if (gameObject.name.Contains("PlayerDetectorRC") && ((hit2 = list[count]).distance < num2))
                {
                    num2 -= 60f;
                    if (num2 <= 60f)
                    {
                        count = list.Count;
                    }
                    TITAN component = gameObject.transform.root.gameObject.GetComponent<TITAN>();
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
        for (count = 0; count < this.myTitans.Count; count++)
        {
            TITAN titan2 = this.myTitans[count];
            if (!list2.Contains(titan2))
            {
                titan2.isLook = false;
            }
        }
        for (count = 0; count < list2.Count; count++)
        {
            TITAN titan3 = list2[count];
            titan3.isLook = true;
        }
        this.myTitans = list2;
    }

    public void ClearPopup()
    {
        FengGameManagerMKII.instance.ShowHUDInfoCenter(string.Empty);
    }

    public void continueAnimation()
    {
        IEnumerator enumerator = base.GetComponent<Animation>().GetEnumerator();
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
        this.customAnimationSpeed();
        this.playAnimation(this.currentPlayingClipName());
        if ((IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.SINGLE) && base.photonView.isMine)
        {
            base.photonView.RPC("netContinueAnimation", PhotonTargets.Others, new object[0]);
        }
    }

    public void crossFade(string aniName, float time)
    {
        this.currentAnimation = aniName;
        base.GetComponent<Animation>().CrossFade(aniName, time);
        if (PhotonNetwork.connected && base.photonView.isMine)
        {
            object[] parameters = new object[] { aniName, time };
            base.photonView.RPC("netCrossFade", PhotonTargets.Others, parameters);
        }
    }

    public void TryCrossFade(string animationName, float time)
    {
        if (!this.baseAnimation.IsPlaying(animationName))
        {
            this.crossFade(animationName, time);
        }
    }

    public string currentPlayingClipName()
    {
        IEnumerator enumerator = base.GetComponent<Animation>().GetEnumerator();
        try
        {
            while (enumerator.MoveNext())
            {
                AnimationState current = (AnimationState)enumerator.Current;
                if (current != null && base.GetComponent<Animation>().IsPlaying(current.name))
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

    private void customAnimationSpeed()
    {
        base.GetComponent<Animation>()["attack5"].speed = 1.85f;
        base.GetComponent<Animation>()["changeBlade"].speed = 1.2f;
        base.GetComponent<Animation>()["air_release"].speed = 0.6f;
        base.GetComponent<Animation>()["changeBlade_air"].speed = 0.8f;
        base.GetComponent<Animation>()["AHSS_gun_reload_both"].speed = 0.38f;
        base.GetComponent<Animation>()["AHSS_gun_reload_both_air"].speed = 0.5f;
        base.GetComponent<Animation>()["AHSS_gun_reload_l"].speed = 0.4f;
        base.GetComponent<Animation>()["AHSS_gun_reload_l_air"].speed = 0.5f;
        base.GetComponent<Animation>()["AHSS_gun_reload_r"].speed = 0.4f;
        base.GetComponent<Animation>()["AHSS_gun_reload_r_air"].speed = 0.5f;
    }

    private void dash(float horizontal, float vertical)
    {
        UnityEngine.MonoBehaviour.print(this.dashTime + " " + this.currentGas);
        if (((this.dashTime <= 0f) && (this.currentGas > 0f)) && !this.isMounted)
        {
            this.useGas(this.totalGas * 0.04f);
            this.facingDirection = this.getGlobalFacingDirection(horizontal, vertical);
            this.dashV = this.getGlobaleFacingVector3(this.facingDirection);
            this.originVM = this.currentSpeed;
            Quaternion quaternion = Quaternion.Euler(0f, this.facingDirection, 0f);
            base.GetComponent<Rigidbody>().rotation = quaternion;
            this.targetRotation = quaternion;
            if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
            {
                UnityEngine.Object.Instantiate(Resources.Load("FX/boost_smoke"), base.transform.position, base.transform.rotation);
            }
            else
            {
                PhotonNetwork.Instantiate("FX/boost_smoke", base.transform.position, base.transform.rotation, 0);
            }
            this.dashTime = 0.5f;
            this.crossFade("dash", 0.1f);
            base.GetComponent<Animation>()["dash"].time = 0.1f;
            this.state = HERO_STATE.AirDodge;
            this.falseAttack();
            base.GetComponent<Rigidbody>().AddForce((Vector3)(this.dashV * 40f), ForceMode.VelocityChange);
        }
    }

    public void die(Vector3 v, bool isBite)
    {
        if (this.invincible <= 0f)
        {
            if (this.titanForm && (this.eren_titan != null))
            {
                this.eren_titan.GetComponent<TITAN_EREN>().lifeTime = 0.1f;
            }
            if (this.bulletLeft != null)
            {
                this.bulletLeft.GetComponent<Bullet>().removeMe();
            }
            if (this.bulletRight != null)
            {
                this.bulletRight.GetComponent<Bullet>().removeMe();
            }
            this.meatDie.Play();
            if (((IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE) || base.photonView.isMine) && !this.useGun)
            {
                this.leftbladetrail.Deactivate();
                this.rightbladetrail.Deactivate();
                this.leftbladetrail2.Deactivate();
                this.rightbladetrail2.Deactivate();
            }
            this.breakApart2(v, isBite);
            this.currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().gameOver = true;
            GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>().gameLose2();
            this.falseAttack();
            this.hasDied = true;
            Transform transform = base.transform.Find("audio_die");
            transform.parent = null;
            transform.GetComponent<AudioSource>().Play();
            if (PlayerPrefs.HasKey("EnableSS") && (PlayerPrefs.GetInt("EnableSS") == 1))
            {
                GameObject.Find("MainCamera").GetComponent<IN_GAME_MAIN_CAMERA>().startSnapShot2(base.transform.position, 0, null, 0.02f);
            }
            UnityEngine.Object.Destroy(base.gameObject);
        }
    }

    public void die2(Transform tf)
    {
        if (this.invincible <= 0f)
        {
            if (this.titanForm && (this.eren_titan != null))
            {
                this.eren_titan.GetComponent<TITAN_EREN>().lifeTime = 0.1f;
            }
            if (this.bulletLeft != null)
            {
                this.bulletLeft.GetComponent<Bullet>().removeMe();
            }
            if (this.bulletRight != null)
            {
                this.bulletRight.GetComponent<Bullet>().removeMe();
            }
            Transform transform = base.transform.Find("audio_die");
            transform.parent = null;
            transform.GetComponent<AudioSource>().Play();
            this.meatDie.Play();
            this.currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().setMainObject(null, true, false);
            this.currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().gameOver = true;
            GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>().gameLose2();
            this.falseAttack();
            this.hasDied = true;
            GameObject obj2 = (GameObject)UnityEngine.Object.Instantiate(Resources.Load("hitMeat2"));
            obj2.transform.position = base.transform.position;
            UnityEngine.Object.Destroy(base.gameObject);
        }
    }

    private void dodge2(bool offTheWall = false)
    {
        if (((!FengGameManagerMKII.inputRC.isInputHorse(InputCodeRC.horseMount) || (this.myHorse == null)) || this.isMounted) || (Vector3.Distance(this.myHorse.transform.position, base.transform.position) >= 15f))
        {
            this.state = HERO_STATE.GroundDodge;
            if (!offTheWall)
            {
                float num;
                float num2;
                if (this.inputManager.isInput[InputCode.up])
                {
                    num = 1f;
                }
                else if (this.inputManager.isInput[InputCode.down])
                {
                    num = -1f;
                }
                else
                {
                    num = 0f;
                }
                if (this.inputManager.isInput[InputCode.left])
                {
                    num2 = -1f;
                }
                else if (this.inputManager.isInput[InputCode.right])
                {
                    num2 = 1f;
                }
                else
                {
                    num2 = 0f;
                }
                float num3 = this.getGlobalFacingDirection(num2, num);
                if ((num2 != 0f) || (num != 0f))
                {
                    this.facingDirection = num3 + 180f;
                    this.targetRotation = Quaternion.Euler(0f, this.facingDirection, 0f);
                }
                this.crossFade("dodge", 0.1f);
            }
            else
            {
                this.playAnimation("dodge");
                this.playAnimationAt("dodge", 0.2f);
            }
            this.sparks.enableEmission = false;
        }
    }

    private void erenTransform()
    {
        this.skillCDDuration = this.skillCDLast;
        if (this.bulletLeft != null)
        {
            this.bulletLeft.GetComponent<Bullet>().removeMe();
        }
        if (this.bulletRight != null)
        {
            this.bulletRight.GetComponent<Bullet>().removeMe();
        }
        if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
        {
            this.eren_titan = (GameObject)UnityEngine.Object.Instantiate(Resources.Load("TITAN_EREN"), base.transform.position, base.transform.rotation);
        }
        else
        {
            this.eren_titan = PhotonNetwork.Instantiate("TITAN_EREN", base.transform.position, base.transform.rotation, 0);
        }
        this.eren_titan.GetComponent<TITAN_EREN>().realBody = base.gameObject;
        GameObject.Find("MainCamera").GetComponent<IN_GAME_MAIN_CAMERA>().flashBlind();
        GameObject.Find("MainCamera").GetComponent<IN_GAME_MAIN_CAMERA>().setMainObject(this.eren_titan, true, false);
        this.eren_titan.GetComponent<TITAN_EREN>().born();
        this.eren_titan.GetComponent<Rigidbody>().velocity = base.GetComponent<Rigidbody>().velocity;
        base.GetComponent<Rigidbody>().velocity = Vector3.zero;
        base.transform.position = this.eren_titan.transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/neck").position;
        this.titanForm = true;
        if (IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.SINGLE)
        {
            object[] parameters = new object[] { this.eren_titan.GetPhotonView().viewID };
            base.photonView.RPC("whoIsMyErenTitan", PhotonTargets.Others, parameters);
        }
        if ((this.smoke_3dmg.enableEmission && (IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.SINGLE)) && base.photonView.isMine)
        {
            object[] objArray2 = new object[] { false };
            base.photonView.RPC("net3DMGSMOKE", PhotonTargets.Others, objArray2);
        }
        this.smoke_3dmg.enableEmission = false;
    }

    private void escapeFromGrab()
    {
    }

    public void falseAttack()
    {
        this.attackMove = false;
        if (this.useGun)
        {
            if (!this.attackReleased)
            {
                this.continueAnimation();
                this.attackReleased = true;
            }
        }
        else
        {
            if ((IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE) || base.photonView.isMine)
            {
                this.checkBoxLeft.GetComponent<TriggerColliderWeapon>().active_me = false;
                this.checkBoxRight.GetComponent<TriggerColliderWeapon>().active_me = false;
                this.checkBoxLeft.GetComponent<TriggerColliderWeapon>().clearHits();
                this.checkBoxRight.GetComponent<TriggerColliderWeapon>().clearHits();
                //this.leftbladetrail.StopSmoothly(0.2f);
                //this.rightbladetrail.StopSmoothly(0.2f);
                //this.leftbladetrail2.StopSmoothly(0.2f);
                //this.rightbladetrail2.StopSmoothly(0.2f);
            }
            this.attackLoop = 0;
            if (!this.attackReleased)
            {
                this.continueAnimation();
                this.attackReleased = true;
            }
        }
    }

    public void fillGas()
    {
        this.currentGas = this.totalGas;
    }

    private GameObject findNearestTitan()
    {
        GameObject[] objArray = GameObject.FindGameObjectsWithTag("titan");
        GameObject obj2 = null;
        float positiveInfinity = float.PositiveInfinity;
        Vector3 position = base.transform.position;
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

    private void FixedUpdate()
    {
        if ((!this.titanForm && !this.isCannon) && (!IN_GAME_MAIN_CAMERA.isPausing || (IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.SINGLE)))
        {
            this.currentSpeed = this.baseRigidBody.velocity.magnitude;
            if ((IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE) || base.photonView.isMine)
            {
                if (!((this.baseAnimation.IsPlaying("attack3_2") || this.baseAnimation.IsPlaying("attack5")) || this.baseAnimation.IsPlaying("special_petra")))
                {
                    this.baseRigidBody.rotation = Quaternion.Lerp(base.gameObject.transform.rotation, this.targetRotation, Time.deltaTime * 6f);
                }
                if (this.state == HERO_STATE.Grab)
                {
                    this.baseRigidBody.AddForce(-this.baseRigidBody.velocity, ForceMode.VelocityChange);
                }
                else
                {
                    if (this.IsGrounded())
                    {
                        if (!this.grounded)
                        {
                            this.justGrounded = true;
                        }
                        this.grounded = true;
                    }
                    else
                    {
                        this.grounded = false;
                    }
                    if (this.hookSomeOne)
                    {
                        if (this.hookTarget != null)
                        {
                            Vector3 vector2 = this.hookTarget.transform.position - this.baseTransform.position;
                            float magnitude = vector2.magnitude;
                            if (magnitude > 2f)
                            {
                                this.baseRigidBody.AddForce((Vector3)(((vector2.normalized * Mathf.Pow(magnitude, 0.15f)) * 30f) - (this.baseRigidBody.velocity * 0.95f)), ForceMode.VelocityChange);
                            }
                        }
                        else
                        {
                            this.hookSomeOne = false;
                        }
                    }
                    else if (this.hookBySomeOne && (this.badGuy != null))
                    {
                        if (this.badGuy != null)
                        {
                            Vector3 vector3 = this.badGuy.transform.position - this.baseTransform.position;
                            float f = vector3.magnitude;
                            if (f > 5f)
                            {
                                this.baseRigidBody.AddForce((Vector3)((vector3.normalized * Mathf.Pow(f, 0.15f)) * 0.2f), ForceMode.Impulse);
                            }
                        }
                        else
                        {
                            this.hookBySomeOne = false;
                        }
                    }
                    float x = 0f;
                    float z = 0f;
                    if (!IN_GAME_MAIN_CAMERA.isTyping)
                    {
                        if (this.inputManager.isInput[InputCode.up])
                        {
                            z = 1f;
                        }
                        else if (this.inputManager.isInput[InputCode.down])
                        {
                            z = -1f;
                        }
                        else
                        {
                            z = 0f;
                        }
                        if (this.inputManager.isInput[InputCode.left])
                        {
                            x = -1f;
                        }
                        else if (this.inputManager.isInput[InputCode.right])
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
                    this.isLeftHandHooked = false;
                    this.isRightHandHooked = false;
                    if (this.isLaunchLeft)
                    {
                        if ((this.bulletLeft != null) && this.bulletLeft.GetComponent<Bullet>().isHooked())
                        {
                            this.isLeftHandHooked = true;
                            Vector3 to = this.bulletLeft.transform.position - this.baseTransform.position;
                            to.Normalize();
                            to = (Vector3)(to * 10f);
                            if (!this.isLaunchRight)
                            {
                                to = (Vector3)(to * 2f);
                            }
                            if ((Vector3.Angle(this.baseRigidBody.velocity, to) > 90f) && this.inputManager.isInput[InputCode.jump])
                            {
                                flag3 = true;
                                flag2 = true;
                            }
                            if (!flag3)
                            {
                                this.baseRigidBody.AddForce(to);
                                if (Vector3.Angle(this.baseRigidBody.velocity, to) > 90f)
                                {
                                    this.baseRigidBody.AddForce((Vector3)(-this.baseRigidBody.velocity * 2f), ForceMode.Acceleration);
                                }
                            }
                        }
                        this.launchElapsedTimeL += Time.deltaTime;
                        if (this.QHold && (this.currentGas > 0f))
                        {
                            this.useGas(this.useGasSpeed * Time.deltaTime);
                        }
                        else if (this.launchElapsedTimeL > 0.3f)
                        {
                            this.isLaunchLeft = false;
                            if (this.bulletLeft != null)
                            {
                                this.bulletLeft.GetComponent<Bullet>().disable();
                                this.releaseIfIHookSb();
                                this.bulletLeft = null;
                                flag3 = false;
                            }
                        }
                    }
                    if (this.isLaunchRight)
                    {
                        if ((this.bulletRight != null) && this.bulletRight.GetComponent<Bullet>().isHooked())
                        {
                            this.isRightHandHooked = true;
                            Vector3 vector5 = this.bulletRight.transform.position - this.baseTransform.position;
                            vector5.Normalize();
                            vector5 = (Vector3)(vector5 * 10f);
                            if (!this.isLaunchLeft)
                            {
                                vector5 = (Vector3)(vector5 * 2f);
                            }
                            if ((Vector3.Angle(this.baseRigidBody.velocity, vector5) > 90f) && this.inputManager.isInput[InputCode.jump])
                            {
                                flag4 = true;
                                flag2 = true;
                            }
                            if (!flag4)
                            {
                                this.baseRigidBody.AddForce(vector5);
                                if (Vector3.Angle(this.baseRigidBody.velocity, vector5) > 90f)
                                {
                                    this.baseRigidBody.AddForce((Vector3)(-this.baseRigidBody.velocity * 2f), ForceMode.Acceleration);
                                }
                            }
                        }
                        this.launchElapsedTimeR += Time.deltaTime;
                        if (this.EHold && (this.currentGas > 0f))
                        {
                            this.useGas(this.useGasSpeed * Time.deltaTime);
                        }
                        else if (this.launchElapsedTimeR > 0.3f)
                        {
                            this.isLaunchRight = false;
                            if (this.bulletRight != null)
                            {
                                this.bulletRight.GetComponent<Bullet>().disable();
                                this.releaseIfIHookSb();
                                this.bulletRight = null;
                                flag4 = false;
                            }
                        }
                    }
                    if (this.grounded)
                    {
                        Vector3 vector7;
                        Vector3 zero = Vector3.zero;
                        if (this.state == HERO_STATE.Attack)
                        {
                            if (this.attackAnimation == "attack5")
                            {
                                if ((this.baseAnimation[this.attackAnimation].normalizedTime > 0.4f) && (this.baseAnimation[this.attackAnimation].normalizedTime < 0.61f))
                                {
                                    this.baseRigidBody.AddForce((Vector3)(base.gameObject.transform.forward * 200f));
                                }
                            }
                            else if (this.attackAnimation == "special_petra")
                            {
                                if ((this.baseAnimation[this.attackAnimation].normalizedTime > 0.35f) && (this.baseAnimation[this.attackAnimation].normalizedTime < 0.48f))
                                {
                                    this.baseRigidBody.AddForce((Vector3)(base.gameObject.transform.forward * 200f));
                                }
                            }
                            else if (this.baseAnimation.IsPlaying("attack3_2"))
                            {
                                zero = Vector3.zero;
                            }
                            else if (this.baseAnimation.IsPlaying("attack1") || this.baseAnimation.IsPlaying("attack2"))
                            {
                                this.baseRigidBody.AddForce((Vector3)(base.gameObject.transform.forward * 200f));
                            }
                            if (this.baseAnimation.IsPlaying("attack3_2"))
                            {
                                zero = Vector3.zero;
                            }
                        }
                        if (this.justGrounded)
                        {
                            if ((this.state != HERO_STATE.Attack) || (((this.attackAnimation != "attack3_1") && (this.attackAnimation != "attack5")) && (this.attackAnimation != "special_petra")))
                            {
                                if ((((this.state != HERO_STATE.Attack) && (x == 0f)) && ((z == 0f) && (this.bulletLeft == null))) && ((this.bulletRight == null) && (this.state != HERO_STATE.FillGas)))
                                {
                                    this.state = HERO_STATE.Land;
                                    this.crossFade("dash_land", 0.01f);
                                }
                                else
                                {
                                    this.buttonAttackRelease = true;
                                    if (((this.state != HERO_STATE.Attack) && (((this.baseRigidBody.velocity.x * this.baseRigidBody.velocity.x) + (this.baseRigidBody.velocity.z * this.baseRigidBody.velocity.z)) > ((this.speed * this.speed) * 1.5f))) && (this.state != HERO_STATE.FillGas))
                                    {
                                        this.state = HERO_STATE.Slide;
                                        this.crossFade("slide", 0.05f);
                                        this.facingDirection = Mathf.Atan2(this.baseRigidBody.velocity.x, this.baseRigidBody.velocity.z) * 57.29578f;
                                        this.targetRotation = Quaternion.Euler(0f, this.facingDirection, 0f);
                                        this.sparks.enableEmission = true;
                                    }
                                }
                            }
                            this.justGrounded = false;
                            zero = this.baseRigidBody.velocity;
                        }
                        if (((this.state == HERO_STATE.Attack) && (this.attackAnimation == "attack3_1")) && (this.baseAnimation[this.attackAnimation].normalizedTime >= 1f))
                        {
                            this.playAnimation("attack3_2");
                            this.resetAnimationSpeed();
                            vector7 = Vector3.zero;
                            this.baseRigidBody.velocity = vector7;
                            zero = vector7;
                            this.currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().startShake(0.2f, 0.3f, 0.95f);
                        }
                        if (this.state == HERO_STATE.GroundDodge)
                        {
                            if ((this.baseAnimation["dodge"].normalizedTime >= 0.2f) && (this.baseAnimation["dodge"].normalizedTime < 0.8f))
                            {
                                zero = (Vector3)((-this.baseTransform.forward * 2.4f) * this.speed);
                            }
                            if (this.baseAnimation["dodge"].normalizedTime > 0.8f)
                            {
                                zero = (Vector3)(this.baseRigidBody.velocity * 0.9f);
                            }
                        }
                        else if (this.state == HERO_STATE.Idle)
                        {
                            Vector3 vector8 = new Vector3(x, 0f, z);
                            float resultAngle = this.getGlobalFacingDirection(x, z);
                            zero = this.getGlobaleFacingVector3(resultAngle);
                            float num6 = (vector8.magnitude <= 0.95f) ? ((vector8.magnitude >= 0.25f) ? vector8.magnitude : 0f) : 1f;
                            zero = (Vector3)(zero * num6);
                            zero = (Vector3)(zero * this.speed);
                            if ((this.buffTime > 0f) && (this.currentBuff == BUFF.SpeedUp))
                            {
                                zero = (Vector3)(zero * 4f);
                            }
                            if ((x != 0f) || (z != 0f))
                            {
                                if (((!this.baseAnimation.IsPlaying("run_1") && !this.baseAnimation.IsPlaying("jump")) && !this.baseAnimation.IsPlaying("run_sasha")) && (!this.baseAnimation.IsPlaying("horse_geton") || (this.baseAnimation["horse_geton"].normalizedTime >= 0.5f)))
                                {
                                    if ((this.buffTime > 0f) && (this.currentBuff == BUFF.SpeedUp))
                                    {
                                        this.crossFade("run_sasha", 0.1f);
                                    }
                                    else
                                    {
                                        this.crossFade("run_1", 0.1f);
                                    }
                                }
                            }
                            else
                            {
                                if (!(((this.baseAnimation.IsPlaying(this.standAnimation) || (this.state == HERO_STATE.Land)) || (this.baseAnimation.IsPlaying("jump") || this.baseAnimation.IsPlaying("horse_geton"))) || this.baseAnimation.IsPlaying("grabbed")))
                                {
                                    this.crossFade(this.standAnimation, 0.1f);
                                    zero = (Vector3)(zero * 0f);
                                }
                                resultAngle = -874f;
                            }
                            if (resultAngle != -874f)
                            {
                                this.facingDirection = resultAngle;
                                this.targetRotation = Quaternion.Euler(0f, this.facingDirection, 0f);
                            }
                        }
                        else if (this.state == HERO_STATE.Land)
                        {
                            zero = (Vector3)(this.baseRigidBody.velocity * 0.96f);
                        }
                        else if (this.state == HERO_STATE.Slide)
                        {
                            zero = (Vector3)(this.baseRigidBody.velocity * 0.99f);
                            if (this.currentSpeed < (this.speed * 1.2f))
                            {
                                this.idle();
                                this.sparks.enableEmission = false;
                            }
                        }
                        Vector3 velocity = this.baseRigidBody.velocity;
                        Vector3 force = zero - velocity;
                        force.x = Mathf.Clamp(force.x, -this.maxVelocityChange, this.maxVelocityChange);
                        force.z = Mathf.Clamp(force.z, -this.maxVelocityChange, this.maxVelocityChange);
                        force.y = 0f;
                        if (this.baseAnimation.IsPlaying("jump") && (this.baseAnimation["jump"].normalizedTime > 0.18f))
                        {
                            force.y += 8f;
                        }
                        if ((this.baseAnimation.IsPlaying("horse_geton") && (this.baseAnimation["horse_geton"].normalizedTime > 0.18f)) && (this.baseAnimation["horse_geton"].normalizedTime < 1f))
                        {
                            float num7 = 6f;
                            force = -this.baseRigidBody.velocity;
                            force.y = num7;
                            float num8 = Vector3.Distance(this.myHorse.transform.position, this.baseTransform.position);
                            float num9 = ((0.6f * this.gravity) * num8) / 12f;
                            vector7 = this.myHorse.transform.position - this.baseTransform.position;
                            force += (Vector3)(num9 * vector7.normalized);
                        }
                        if (!((this.state == HERO_STATE.Attack) && this.useGun))
                        {
                            this.baseRigidBody.AddForce(force, ForceMode.VelocityChange);
                            this.baseRigidBody.rotation = Quaternion.Lerp(base.gameObject.transform.rotation, Quaternion.Euler(0f, this.facingDirection, 0f), Time.deltaTime * 10f);
                        }
                    }
                    else
                    {
                        if (this.sparks.enableEmission)
                        {
                            this.sparks.enableEmission = false;
                        }
                        if (((this.myHorse != null) && (this.baseAnimation.IsPlaying("horse_geton") || this.baseAnimation.IsPlaying("air_fall"))) && ((this.baseRigidBody.velocity.y < 0f) && (Vector3.Distance(this.myHorse.transform.position + ((Vector3)(Vector3.up * 1.65f)), this.baseTransform.position) < 0.5f)))
                        {
                            this.baseTransform.position = this.myHorse.transform.position + ((Vector3)(Vector3.up * 1.65f));
                            this.baseTransform.rotation = this.myHorse.transform.rotation;
                            this.isMounted = true;
                            this.crossFade("horse_idle", 0.1f);
                            this.myHorse.GetComponent<Horse>().mounted();
                        }
                        if (!((((((this.state != HERO_STATE.Idle) || this.baseAnimation.IsPlaying("dash")) || (this.baseAnimation.IsPlaying("wallrun") || this.baseAnimation.IsPlaying("toRoof"))) || ((this.baseAnimation.IsPlaying("horse_geton") || this.baseAnimation.IsPlaying("horse_getoff")) || (this.baseAnimation.IsPlaying("air_release") || this.isMounted))) || ((this.baseAnimation.IsPlaying("air_hook_l_just") && (this.baseAnimation["air_hook_l_just"].normalizedTime < 1f)) || (this.baseAnimation.IsPlaying("air_hook_r_just") && (this.baseAnimation["air_hook_r_just"].normalizedTime < 1f)))) ? (this.baseAnimation["dash"].normalizedTime < 0.99f) : false))
                        {
                            if (((!this.isLeftHandHooked && !this.isRightHandHooked) && ((this.baseAnimation.IsPlaying("air_hook_l") || this.baseAnimation.IsPlaying("air_hook_r")) || this.baseAnimation.IsPlaying("air_hook"))) && (this.baseRigidBody.velocity.y > 20f))
                            {
                                this.baseAnimation.CrossFade("air_release");
                            }
                            else
                            {
                                bool flag5 = (Mathf.Abs(this.baseRigidBody.velocity.x) + Mathf.Abs(this.baseRigidBody.velocity.z)) > 25f;
                                bool flag6 = this.baseRigidBody.velocity.y < 0f;
                                if (!flag5)
                                {
                                    if (flag6)
                                    {
                                        if (!this.baseAnimation.IsPlaying("air_fall"))
                                        {
                                            this.crossFade("air_fall", 0.2f);
                                        }
                                    }
                                    else if (!this.baseAnimation.IsPlaying("air_rise"))
                                    {
                                        this.crossFade("air_rise", 0.2f);
                                    }
                                }
                                else if (!this.isLeftHandHooked && !this.isRightHandHooked)
                                {
                                    float current = -Mathf.Atan2(this.baseRigidBody.velocity.z, this.baseRigidBody.velocity.x) * 57.29578f;
                                    float num11 = -Mathf.DeltaAngle(current, this.baseTransform.rotation.eulerAngles.y - 90f);
                                    if (Mathf.Abs(num11) < 45f)
                                    {
                                        if (!this.baseAnimation.IsPlaying("air2"))
                                        {
                                            this.crossFade("air2", 0.2f);
                                        }
                                    }
                                    else if ((num11 < 135f) && (num11 > 0f))
                                    {
                                        if (!this.baseAnimation.IsPlaying("air2_right"))
                                        {
                                            this.crossFade("air2_right", 0.2f);
                                        }
                                    }
                                    else if ((num11 > -135f) && (num11 < 0f))
                                    {
                                        if (!this.baseAnimation.IsPlaying("air2_left"))
                                        {
                                            this.crossFade("air2_left", 0.2f);
                                        }
                                    }
                                    else if (!this.baseAnimation.IsPlaying("air2_backward"))
                                    {
                                        this.crossFade("air2_backward", 0.2f);
                                    }
                                }

                                else if (!this.isRightHandHooked)
                                {
                                    TryCrossFade(Equipment.Weapon.HookForwardLeft, 0.1f);
                                }
                                else if (!this.isLeftHandHooked)
                                {
                                    TryCrossFade(Equipment.Weapon.HookForwardRight, 0.1f);
                                }
                                else if (!this.baseAnimation.IsPlaying(Equipment.Weapon.HookForward))
                                {
                                    TryCrossFade(Equipment.Weapon.HookForward, 0.1f);
                                }
                            }
                        }
                        if (((this.state == HERO_STATE.Idle) && this.baseAnimation.IsPlaying("air_release")) && (this.baseAnimation["air_release"].normalizedTime >= 1f))
                        {
                            this.crossFade("air_rise", 0.2f);
                        }
                        if (this.baseAnimation.IsPlaying("horse_getoff") && (this.baseAnimation["horse_getoff"].normalizedTime >= 1f))
                        {
                            this.crossFade("air_rise", 0.2f);
                        }
                        if (this.baseAnimation.IsPlaying("toRoof"))
                        {
                            if (this.baseAnimation["toRoof"].normalizedTime < 0.22f)
                            {
                                this.baseRigidBody.velocity = Vector3.zero;
                                this.baseRigidBody.AddForce(new Vector3(0f, this.gravity * this.baseRigidBody.mass, 0f));
                            }
                            else
                            {
                                if (!this.wallJump)
                                {
                                    this.wallJump = true;
                                    this.baseRigidBody.AddForce((Vector3)(Vector3.up * 8f), ForceMode.Impulse);
                                }
                                this.baseRigidBody.AddForce((Vector3)(this.baseTransform.forward * 0.05f), ForceMode.Impulse);
                            }
                            if (this.baseAnimation["toRoof"].normalizedTime >= 1f)
                            {
                                this.playAnimation("air_rise");
                            }
                        }
                        else if (!(((((this.state != HERO_STATE.Idle) || !this.isPressDirectionTowardsHero(x, z)) || (this.inputManager.isInput[InputCode.jump] || this.inputManager.isInput[InputCode.leftRope])) || ((this.inputManager.isInput[InputCode.rightRope] || this.inputManager.isInput[InputCode.bothRope]) || (!this.IsFrontGrounded() || this.baseAnimation.IsPlaying("wallrun")))) || this.baseAnimation.IsPlaying("dodge")))
                        {
                            this.crossFade("wallrun", 0.1f);
                            this.wallRunTime = 0f;
                        }
                        else if (this.baseAnimation.IsPlaying("wallrun"))
                        {
                            this.baseRigidBody.AddForce(((Vector3)(Vector3.up * this.speed)) - this.baseRigidBody.velocity, ForceMode.VelocityChange);
                            this.wallRunTime += Time.deltaTime;
                            if ((this.wallRunTime > 1f) || ((z == 0f) && (x == 0f)))
                            {
                                this.baseRigidBody.AddForce((Vector3)((-this.baseTransform.forward * this.speed) * 0.75f), ForceMode.Impulse);
                                this.dodge2(true);
                            }
                            else if (!this.IsUpFrontGrounded())
                            {
                                this.wallJump = false;
                                this.crossFade("toRoof", 0.1f);
                            }
                            else if (!this.IsFrontGrounded())
                            {
                                this.crossFade("air_fall", 0.1f);
                            }
                        }
                        else if ((!this.baseAnimation.IsPlaying("attack5") && !this.baseAnimation.IsPlaying("special_petra")) && (!this.baseAnimation.IsPlaying("dash") && !this.baseAnimation.IsPlaying("jump")))
                        {
                            Vector3 vector11 = new Vector3(x, 0f, z);
                            float num12 = this.getGlobalFacingDirection(x, z);
                            Vector3 vector12 = this.getGlobaleFacingVector3(num12);
                            float num13 = (vector11.magnitude <= 0.95f) ? ((vector11.magnitude >= 0.25f) ? vector11.magnitude : 0f) : 1f;
                            vector12 = (Vector3)(vector12 * num13);
                            vector12 = (Vector3)(vector12 * ((((float)this.setup.myCostume.stat.ACL) / 10f) * 2f));
                            if ((x == 0f) && (z == 0f))
                            {
                                if (this.state == HERO_STATE.Attack)
                                {
                                    vector12 = (Vector3)(vector12 * 0f);
                                }
                                num12 = -874f;
                            }
                            if (num12 != -874f)
                            {
                                this.facingDirection = num12;
                                this.targetRotation = Quaternion.Euler(0f, this.facingDirection, 0f);
                            }
                            if (((!flag3 && !flag4) && (!this.isMounted && this.inputManager.isInput[InputCode.jump])) && (this.currentGas > 0f))
                            {
                                if ((x != 0f) || (z != 0f))
                                {
                                    this.baseRigidBody.AddForce(vector12, ForceMode.Acceleration);
                                }
                                else
                                {
                                    this.baseRigidBody.AddForce((Vector3)(this.baseTransform.forward * vector12.magnitude), ForceMode.Acceleration);
                                }
                                flag2 = true;
                            }
                        }
                        if ((this.baseAnimation.IsPlaying("air_fall") && (this.currentSpeed < 0.2f)) && this.IsFrontGrounded())
                        {
                            this.crossFade("onWall", 0.3f);
                        }
                    }
                    this.spinning = false;
                    if (flag3 && flag4)
                    {
                        float num14 = this.currentSpeed + 0.1f;
                        baseRigidBody.AddForce(-baseRigidBody.velocity * 0.5f, ForceMode.Acceleration);
                        Vector3 vector13 = ((Vector3)((this.bulletRight.transform.position + this.bulletLeft.transform.position) * 0.5f)) - this.baseTransform.position;
                        float num15 = 0f;
                        if (FengGameManagerMKII.inputRC.isInputHuman(InputCodeRC.reelin))
                        {
                            num15 = -1f;
                        }
                        else if (FengGameManagerMKII.inputRC.isInputHuman(InputCodeRC.reelout))
                        {
                            num15 = 1f;
                        }
                        else
                        {
                            num15 = Input.GetAxis("Mouse ScrollWheel") * 5555f;
                        }
                        num15 = Mathf.Clamp(num15, -0.8f, 0.8f);
                        float num16 = 1f + num15;
                        Vector3 vector14 = Vector3.RotateTowards(vector13, this.baseRigidBody.velocity, 1.53938f * num16, 1.53938f * num16);
                        vector14.Normalize();
                        this.spinning = true;
                        this.baseRigidBody.velocity = (Vector3)(vector14 * num14);
                    }
                    else if (flag3)
                    {
                        float num17 = this.currentSpeed + 0.1f;
                        baseRigidBody.AddForce(-baseRigidBody.velocity * 0.5f, ForceMode.Acceleration);
                        Vector3 vector15 = this.bulletLeft.transform.position - this.baseTransform.position;
                        float num18 = 0f;
                        if (FengGameManagerMKII.inputRC.isInputHuman(InputCodeRC.reelin))
                        {
                            num18 = -1f;
                        }
                        else if (FengGameManagerMKII.inputRC.isInputHuman(InputCodeRC.reelout))
                        {
                            num18 = 1f;
                        }
                        else
                        {
                            num18 = Input.GetAxis("Mouse ScrollWheel") * 5555f;
                        }
                        num18 = Mathf.Clamp(num18, -0.8f, 0.8f);
                        float num19 = 1f + num18;
                        Vector3 vector16 = Vector3.RotateTowards(vector15, this.baseRigidBody.velocity, 1.53938f * num19, 1.53938f * num19);
                        vector16.Normalize();
                        this.spinning = true;
                        this.baseRigidBody.velocity = (Vector3)(vector16 * num17);
                    }
                    else if (flag4)
                    {
                        float num20 = this.currentSpeed + 0.1f;
                        baseRigidBody.AddForce(-baseRigidBody.velocity * 0.5f, ForceMode.Acceleration);
                        Vector3 vector17 = this.bulletRight.transform.position - this.baseTransform.position;
                        float num21 = 0f;
                        if (FengGameManagerMKII.inputRC.isInputHuman(InputCodeRC.reelin))
                        {
                            num21 = -1f;
                        }
                        else if (FengGameManagerMKII.inputRC.isInputHuman(InputCodeRC.reelout))
                        {
                            num21 = 1f;
                        }
                        else
                        {
                            num21 = Input.GetAxis("Mouse ScrollWheel") * 5555f;
                        }
                        num21 = Mathf.Clamp(num21, -0.8f, 0.8f);
                        float num22 = 1f + num21;
                        Vector3 vector18 = Vector3.RotateTowards(vector17, this.baseRigidBody.velocity, 1.53938f * num22, 1.53938f * num22);
                        vector18.Normalize();
                        this.spinning = true;
                        this.baseRigidBody.velocity = (Vector3)(vector18 * num20);
                    }
                    if (((this.state == HERO_STATE.Attack) && ((this.attackAnimation == "attack5") || (this.attackAnimation == "special_petra"))) && ((this.baseAnimation[this.attackAnimation].normalizedTime > 0.4f) && !this.attackMove))
                    {
                        this.attackMove = true;
                        if (this.launchPointRight.magnitude > 0f)
                        {
                            Vector3 vector19 = this.launchPointRight - this.baseTransform.position;
                            vector19.Normalize();
                            vector19 = (Vector3)(vector19 * 13f);
                            this.baseRigidBody.AddForce(vector19, ForceMode.Impulse);
                        }
                        if ((this.attackAnimation == "special_petra") && (this.launchPointLeft.magnitude > 0f))
                        {
                            Vector3 vector20 = this.launchPointLeft - this.baseTransform.position;
                            vector20.Normalize();
                            vector20 = (Vector3)(vector20 * 13f);
                            this.baseRigidBody.AddForce(vector20, ForceMode.Impulse);
                            if (this.bulletRight != null)
                            {
                                this.bulletRight.GetComponent<Bullet>().disable();
                                this.releaseIfIHookSb();
                            }
                            if (this.bulletLeft != null)
                            {
                                this.bulletLeft.GetComponent<Bullet>().disable();
                                this.releaseIfIHookSb();
                            }
                        }
                        this.baseRigidBody.AddForce((Vector3)(Vector3.up * 2f), ForceMode.Impulse);
                    }
                    bool flag7 = false;
                    if ((this.bulletLeft != null) || (this.bulletRight != null))
                    {
                        if (((this.bulletLeft != null) && (this.bulletLeft.transform.position.y > base.gameObject.transform.position.y)) && (this.isLaunchLeft && this.bulletLeft.GetComponent<Bullet>().isHooked()))
                        {
                            flag7 = true;
                        }
                        if (((this.bulletRight != null) && (this.bulletRight.transform.position.y > base.gameObject.transform.position.y)) && (this.isLaunchRight && this.bulletRight.GetComponent<Bullet>().isHooked()))
                        {
                            flag7 = true;
                        }
                    }
                    if (flag7)
                    {
                        this.baseRigidBody.AddForce(new Vector3(0f, -10f * this.baseRigidBody.mass, 0f));
                    }
                    else
                    {
                        this.baseRigidBody.AddForce(new Vector3(0f, -this.gravity * this.baseRigidBody.mass, 0f));
                    }
                    if (this.currentSpeed > 10f)
                    {
                        this.currentCamera.GetComponent<Camera>().fieldOfView = Mathf.Lerp(this.currentCamera.GetComponent<Camera>().fieldOfView, Mathf.Min((float)100f, (float)(this.currentSpeed + 40f)), 0.1f);
                    }
                    else
                    {
                        this.currentCamera.GetComponent<Camera>().fieldOfView = Mathf.Lerp(this.currentCamera.GetComponent<Camera>().fieldOfView, 50f, 0.1f);
                    }
                    if (flag2)
                    {
                        this.useGas(this.useGasSpeed * Time.deltaTime);
                        if ((!this.smoke_3dmg.enableEmission && (IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.SINGLE)) && base.photonView.isMine)
                        {
                            object[] parameters = new object[] { true };
                            base.photonView.RPC("net3DMGSMOKE", PhotonTargets.Others, parameters);
                        }
                        this.smoke_3dmg.enableEmission = true;
                    }
                    else
                    {
                        if ((this.smoke_3dmg.enableEmission && (IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.SINGLE)) && base.photonView.isMine)
                        {
                            object[] objArray3 = new object[] { false };
                            base.photonView.RPC("net3DMGSMOKE", PhotonTargets.Others, objArray3);
                        }
                        this.smoke_3dmg.enableEmission = false;
                    }
                    if (this.currentSpeed > 80f)
                    {
                        //if (!this.speedFXPS.enableEmission)
                        //{
                        //    this.speedFXPS.enableEmission = true;
                        //}
                        //this.speedFXPS.startSpeed = this.currentSpeed;
                        //this.speedFX.transform.LookAt(this.baseTransform.position + this.baseRigidBody.velocity);
                    }
                    //else if (this.speedFXPS.enableEmission)
                    //{
                    //    this.speedFXPS.enableEmission = false;
                    //}
                }
            }
        }
    }

    public string getDebugInfo()
    {
        string str = "\n";
        str = "Left:" + this.isLeftHandHooked + " ";
        if (this.isLeftHandHooked && (this.bulletLeft != null))
        {
            Vector3 vector = this.bulletLeft.transform.position - base.transform.position;
            str = str + ((int)(Mathf.Atan2(vector.x, vector.z) * 57.29578f));
        }
        string str2 = str;
        object[] objArray1 = new object[] { str2, "\nRight:", this.isRightHandHooked, " " };
        str = string.Concat(objArray1);
        if (this.isRightHandHooked && (this.bulletRight != null))
        {
            Vector3 vector2 = this.bulletRight.transform.position - base.transform.position;
            str = str + ((int)(Mathf.Atan2(vector2.x, vector2.z) * 57.29578f));
        }
        str = (((str + "\nfacingDirection:" + ((int)this.facingDirection)) + "\nActual facingDirection:" + ((int)base.transform.rotation.eulerAngles.y)) + "\nState:" + this.state.ToString()) + "\n\n\n\n\n";
        if (this.state == HERO_STATE.Attack)
        {
            this.targetRotation = Quaternion.Euler(0f, this.facingDirection, 0f);
        }
        return str;
    }

    private Vector3 getGlobaleFacingVector3(float resultAngle)
    {
        float num = -resultAngle + 90f;
        float x = Mathf.Cos(num * 0.01745329f);
        return new Vector3(x, 0f, Mathf.Sin(num * 0.01745329f));
    }

    private Vector3 getGlobaleFacingVector3(float horizontal, float vertical)
    {
        float num = -this.getGlobalFacingDirection(horizontal, vertical) + 90f;
        float x = Mathf.Cos(num * 0.01745329f);
        return new Vector3(x, 0f, Mathf.Sin(num * 0.01745329f));
    }

    private float getGlobalFacingDirection(float horizontal, float vertical)
    {
        if ((vertical == 0f) && (horizontal == 0f))
        {
            return base.transform.rotation.eulerAngles.y;
        }
        float y = this.currentCamera.transform.rotation.eulerAngles.y;
        float num2 = Mathf.Atan2(vertical, horizontal) * 57.29578f;
        num2 = -num2 + 90f;
        return (y + num2);
    }

    private float getLeanAngle(Vector3 p, bool left)
    {
        if (!this.useGun && (this.state == HERO_STATE.Attack))
        {
            return 0f;
        }
        float num = p.y - base.transform.position.y;
        float num2 = Vector3.Distance(p, base.transform.position);
        float a = Mathf.Acos(num / num2) * 57.29578f;
        a *= 0.1f;
        a *= 1f + Mathf.Pow(base.GetComponent<Rigidbody>().velocity.magnitude, 0.2f);
        Vector3 vector3 = p - base.transform.position;
        float current = Mathf.Atan2(vector3.x, vector3.z) * 57.29578f;
        float target = Mathf.Atan2(base.GetComponent<Rigidbody>().velocity.x, base.GetComponent<Rigidbody>().velocity.z) * 57.29578f;
        float num6 = Mathf.DeltaAngle(current, target);
        a += Mathf.Abs((float)(num6 * 0.5f));
        if (this.state != HERO_STATE.Attack)
        {
            a = Mathf.Min(a, 80f);
        }
        if (num6 > 0f)
        {
            this.leanLeft = true;
        }
        else
        {
            this.leanLeft = false;
        }
        if (this.useGun)
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

    private void getOffHorse()
    {
        this.playAnimation("horse_getoff");
        base.GetComponent<Rigidbody>().AddForce((Vector3)(((Vector3.up * 10f) - (base.transform.forward * 2f)) - (base.transform.right * 1f)), ForceMode.VelocityChange);
        this.unmounted();
    }

    private void getOnHorse()
    {
        this.playAnimation("horse_geton");
        this.facingDirection = this.myHorse.transform.rotation.eulerAngles.y;
        this.targetRotation = Quaternion.Euler(0f, this.facingDirection, 0f);
    }

    public void getSupply()
    {
        if (((base.GetComponent<Animation>().IsPlaying(this.standAnimation) || base.GetComponent<Animation>().IsPlaying("run_1")) || base.GetComponent<Animation>().IsPlaying("run_sasha")) && (((this.currentBladeSta != this.totalBladeSta) || (this.currentBladeNum != this.totalBladeNum)) || (((this.currentGas != this.totalGas) || (this.leftBulletLeft != this.bulletMAX)) || (this.rightBulletLeft != this.bulletMAX))))
        {
            this.state = HERO_STATE.FillGas;
            this.crossFade("supply", 0.1f);
        }
    }

    public void grabbed(GameObject titan, bool leftHand)
    {
        if (this.isMounted)
        {
            this.unmounted();
        }
        this.state = HERO_STATE.Grab;
        base.GetComponent<CapsuleCollider>().isTrigger = true;
        this.falseAttack();
        this.titanWhoGrabMe = titan;
        if (this.titanForm && (this.eren_titan != null))
        {
            this.eren_titan.GetComponent<TITAN_EREN>().lifeTime = 0.1f;
        }
        if (!this.useGun && ((IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE) || base.photonView.isMine))
        {
            //this.leftbladetrail.Deactivate();
            //this.rightbladetrail.Deactivate();
            //this.leftbladetrail2.Deactivate();
            //this.rightbladetrail2.Deactivate();
        }
        this.smoke_3dmg.enableEmission = false;
        this.sparks.enableEmission = false;
    }

    public bool HasDied()
    {
        return (this.hasDied || this.isInvincible());
    }

    private void headMovement()
    {
        Transform transform = base.transform.Find("Amarture/Controller_Body/hip/spine/chest/neck/head");
        Transform transform2 = base.transform.Find("Amarture/Controller_Body/hip/spine/chest/neck");
        float x = Mathf.Sqrt(((this.gunTarget.x - base.transform.position.x) * (this.gunTarget.x - base.transform.position.x)) + ((this.gunTarget.z - base.transform.position.z) * (this.gunTarget.z - base.transform.position.z)));
        this.targetHeadRotation = transform.rotation;
        Vector3 vector5 = this.gunTarget - base.transform.position;
        float current = -Mathf.Atan2(vector5.z, vector5.x) * 57.29578f;
        float num3 = -Mathf.DeltaAngle(current, base.transform.rotation.eulerAngles.y - 90f);
        num3 = Mathf.Clamp(num3, -40f, 40f);
        float y = transform2.position.y - this.gunTarget.y;
        float num5 = Mathf.Atan2(y, x) * 57.29578f;
        num5 = Mathf.Clamp(num5, -40f, 30f);
        this.targetHeadRotation = Quaternion.Euler(transform.rotation.eulerAngles.x + num5, transform.rotation.eulerAngles.y + num3, transform.rotation.eulerAngles.z);
        this.oldHeadRotation = Quaternion.Lerp(this.oldHeadRotation, this.targetHeadRotation, Time.deltaTime * 60f);
        transform.rotation = this.oldHeadRotation;
    }

    public void hookedByHuman(int hooker, Vector3 hookPosition)
    {
        object[] parameters = new object[] { hooker, hookPosition };
        base.photonView.RPC("RPCHookedByHuman", base.photonView.owner, parameters);
    }

    [PunRPC]
    public void hookFail()
    {
        this.hookTarget = null;
        this.hookSomeOne = false;
    }

    public void hookToHuman(GameObject target, Vector3 hookPosition)
    {
        this.releaseIfIHookSb();
        this.hookTarget = target;
        this.hookSomeOne = true;
        if (target.GetComponent<Hero>() != null)
        {
            target.GetComponent<Hero>().hookedByHuman(base.photonView.viewID, hookPosition);
        }
        this.launchForce = hookPosition - base.transform.position;
        float num = Mathf.Pow(this.launchForce.magnitude, 0.1f);
        if (this.grounded)
        {
            base.GetComponent<Rigidbody>().AddForce((Vector3)(Vector3.up * Mathf.Min((float)(this.launchForce.magnitude * 0.2f), (float)10f)), ForceMode.Impulse);
        }
        base.GetComponent<Rigidbody>().AddForce((Vector3)((this.launchForce * num) * 0.1f), ForceMode.Impulse);
    }

    private void idle()
    {
        if (this.state == HERO_STATE.Attack)
        {
            this.falseAttack();
        }
        this.state = HERO_STATE.Idle;
        this.crossFade(this.standAnimation, 0.1f);
    }

    private bool IsFrontGrounded()
    {
        LayerMask mask = ((int)1) << LayerMask.NameToLayer("Ground");
        LayerMask mask2 = ((int)1) << LayerMask.NameToLayer("EnemyBox");
        LayerMask mask3 = mask2 | mask;
        return Physics.Raycast(base.gameObject.transform.position + ((Vector3)(base.gameObject.transform.up * 1f)), base.gameObject.transform.forward, (float)1f, mask3.value);
    }

    public bool IsGrounded()
    {
        LayerMask mask = ((int)1) << LayerMask.NameToLayer("Ground");
        LayerMask mask2 = ((int)1) << LayerMask.NameToLayer("EnemyBox");
        LayerMask mask3 = mask2 | mask;
        return Physics.Raycast(base.gameObject.transform.position + ((Vector3)(Vector3.up * 0.1f)), -Vector3.up, (float)0.3f, mask3.value);
    }

    public bool isInvincible()
    {
        return (this.invincible > 0f);
    }

    private bool isPressDirectionTowardsHero(float h, float v)
    {
        if ((h == 0f) && (v == 0f))
        {
            return false;
        }
        return (Mathf.Abs(Mathf.DeltaAngle(this.getGlobalFacingDirection(h, v), base.transform.rotation.eulerAngles.y)) < 45f);
    }

    private bool IsUpFrontGrounded()
    {
        LayerMask mask = ((int)1) << LayerMask.NameToLayer("Ground");
        LayerMask mask2 = ((int)1) << LayerMask.NameToLayer("EnemyBox");
        LayerMask mask3 = mask2 | mask;
        return Physics.Raycast(base.gameObject.transform.position + ((Vector3)(base.gameObject.transform.up * 3f)), base.gameObject.transform.forward, (float)1.2f, mask3.value);
    }

    [PunRPC]
    private void killObject()
    {
        UnityEngine.Object.Destroy(base.gameObject);
    }

    public void lateUpdate()
    {
        if ((IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.SINGLE) && (this.myNetWorkName != null))
        {
            if (this.titanForm && (this.eren_titan != null))
            {
                this.myNetWorkName.transform.localPosition = (Vector3)((Vector3.up * Screen.height) * 2f);
            }
            Vector3 start = new Vector3(base.transform.position.x, base.transform.position.y + 2f, base.transform.position.z);
            GameObject obj2 = GameObject.Find("MainCamera");
            LayerMask mask = ((int)1) << LayerMask.NameToLayer("Ground");
            LayerMask mask2 = ((int)1) << LayerMask.NameToLayer("EnemyBox");
            LayerMask mask3 = mask2 | mask;
            if ((Vector3.Angle(obj2.transform.forward, start - obj2.transform.position) <= 90f) && !Physics.Linecast(start, obj2.transform.position, (int)mask3))
            {
                Vector2 vector5 = GameObject.Find("MainCamera").GetComponent<Camera>().WorldToScreenPoint(start);
                this.myNetWorkName.transform.localPosition = new Vector3((float)((int)(vector5.x - (Screen.width * 0.5f))), (float)((int)(vector5.y - (Screen.height * 0.5f))), 0f);
            }
            else
            {
                this.myNetWorkName.transform.localPosition = (Vector3)((Vector3.up * Screen.height) * 2f);
            }
        }
        if (!this.titanForm)
        {
            if ((IN_GAME_MAIN_CAMERA.cameraTilt == 1) && ((IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE) || base.photonView.isMine))
            {
                Quaternion quaternion3;
                Vector3 zero = Vector3.zero;
                Vector3 position = Vector3.zero;
                if ((this.isLaunchLeft && (this.bulletLeft != null)) && this.bulletLeft.GetComponent<Bullet>().isHooked())
                {
                    zero = this.bulletLeft.transform.position;
                }
                if ((this.isLaunchRight && (this.bulletRight != null)) && this.bulletRight.GetComponent<Bullet>().isHooked())
                {
                    position = this.bulletRight.transform.position;
                }
                Vector3 vector8 = Vector3.zero;
                if ((zero.magnitude != 0f) && (position.magnitude == 0f))
                {
                    vector8 = zero;
                }
                else if ((zero.magnitude == 0f) && (position.magnitude != 0f))
                {
                    vector8 = position;
                }
                else if ((zero.magnitude != 0f) && (position.magnitude != 0f))
                {
                    vector8 = (Vector3)((zero + position) * 0.5f);
                }
                Vector3 from = Vector3.Project(vector8 - base.transform.position, GameObject.Find("MainCamera").transform.up);
                Vector3 vector10 = Vector3.Project(vector8 - base.transform.position, GameObject.Find("MainCamera").transform.right);
                if (vector8.magnitude > 0f)
                {
                    Vector3 to = from + vector10;
                    float num = Vector3.Angle(vector8 - base.transform.position, base.GetComponent<Rigidbody>().velocity) * 0.005f;
                    Vector3 vector14 = GameObject.Find("MainCamera").transform.right + vector10.normalized;
                    quaternion3 = Quaternion.Euler(GameObject.Find("MainCamera").transform.rotation.eulerAngles.x, GameObject.Find("MainCamera").transform.rotation.eulerAngles.y, (vector14.magnitude >= 1f) ? (-Vector3.Angle(from, to) * num) : (Vector3.Angle(from, to) * num));
                }
                else
                {
                    quaternion3 = Quaternion.Euler(GameObject.Find("MainCamera").transform.rotation.eulerAngles.x, GameObject.Find("MainCamera").transform.rotation.eulerAngles.y, 0f);
                }
                GameObject.Find("MainCamera").transform.rotation = Quaternion.Lerp(GameObject.Find("MainCamera").transform.rotation, quaternion3, Time.deltaTime * 2f);
            }
            if ((this.state == HERO_STATE.Grab) && (this.titanWhoGrabMe != null))
            {
                if (this.titanWhoGrabMe.GetComponent<TITAN>() != null)
                {
                    base.transform.position = this.titanWhoGrabMe.GetComponent<TITAN>().grabTF.transform.position;
                    base.transform.rotation = this.titanWhoGrabMe.GetComponent<TITAN>().grabTF.transform.rotation;
                }
                else if (this.titanWhoGrabMe.GetComponent<FEMALE_TITAN>() != null)
                {
                    base.transform.position = this.titanWhoGrabMe.GetComponent<FEMALE_TITAN>().grabTF.transform.position;
                    base.transform.rotation = this.titanWhoGrabMe.GetComponent<FEMALE_TITAN>().grabTF.transform.rotation;
                }
            }
            if (this.useGun)
            {
                if (!this.leftArmAim && !this.rightArmAim)
                {
                    if (!this.grounded)
                    {
                        this.handL.localRotation = Quaternion.Euler(90f, 0f, 0f);
                        this.handR.localRotation = Quaternion.Euler(-90f, 0f, 0f);
                    }
                }
                else
                {
                    Vector3 vector17 = this.gunTarget - base.transform.position;
                    float current = -Mathf.Atan2(vector17.z, vector17.x) * 57.29578f;
                    float num3 = -Mathf.DeltaAngle(current, base.transform.rotation.eulerAngles.y - 90f);
                    this.headMovement();
                    if ((!this.isLeftHandHooked && this.leftArmAim) && ((num3 < 40f) && (num3 > -90f)))
                    {
                        this.leftArmAimTo(this.gunTarget);
                    }
                    if ((!this.isRightHandHooked && this.rightArmAim) && ((num3 > -40f) && (num3 < 90f)))
                    {
                        this.rightArmAimTo(this.gunTarget);
                    }
                }
                if (this.isLeftHandHooked && (this.bulletLeft != null))
                {
                    this.leftArmAimTo(this.bulletLeft.transform.position);
                }
                if (this.isRightHandHooked && (this.bulletRight != null))
                {
                    this.rightArmAimTo(this.bulletRight.transform.position);
                }
            }
            this.setHookedPplDirection();
            this.bodyLean();
            if ((!base.GetComponent<Animation>().IsPlaying("attack3_2") && !base.GetComponent<Animation>().IsPlaying("attack5")) && !base.GetComponent<Animation>().IsPlaying("special_petra"))
            {
                base.GetComponent<Rigidbody>().rotation = Quaternion.Lerp(base.gameObject.transform.rotation, this.targetRotation, Time.deltaTime * 6f);
            }
        }
    }

    public void lateUpdate2()
    {
        if ((IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.SINGLE) && (this.myNetWorkName != null))
        {
            if (this.titanForm && (this.eren_titan != null))
            {
                this.myNetWorkName.transform.localPosition = (Vector3)((Vector3.up * Screen.height) * 2f);
            }
            Vector3 start = new Vector3(this.baseTransform.position.x, this.baseTransform.position.y + 2f, this.baseTransform.position.z);
            GameObject maincamera = this.maincamera;
            LayerMask mask = ((int)1) << LayerMask.NameToLayer("Ground");
            LayerMask mask2 = ((int)1) << LayerMask.NameToLayer("EnemyBox");
            LayerMask mask3 = mask2 | mask;
            if ((Vector3.Angle(maincamera.transform.forward, start - maincamera.transform.position) > 90f) || Physics.Linecast(start, maincamera.transform.position, (int)mask3))
            {
                this.myNetWorkName.transform.localPosition = (Vector3)((Vector3.up * Screen.height) * 2f);
            }
            else
            {
                Vector2 vector2 = this.maincamera.GetComponent<Camera>().WorldToScreenPoint(start);
                this.myNetWorkName.transform.localPosition = new Vector3((float)((int)(vector2.x - (Screen.width * 0.5f))), (float)((int)(vector2.y - (Screen.height * 0.5f))), 0f);
            }
        }
        if (!this.titanForm && !this.isCannon)
        {
            if ((IN_GAME_MAIN_CAMERA.cameraTilt == 1) && ((IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE) || base.photonView.isMine))
            {
                Quaternion quaternion2;
                Vector3 zero = Vector3.zero;
                Vector3 position = Vector3.zero;
                if ((this.isLaunchLeft && (this.bulletLeft != null)) && this.bulletLeft.GetComponent<Bullet>().isHooked())
                {
                    zero = this.bulletLeft.transform.position;
                }
                if ((this.isLaunchRight && (this.bulletRight != null)) && this.bulletRight.GetComponent<Bullet>().isHooked())
                {
                    position = this.bulletRight.transform.position;
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
                    vector5 = (Vector3)((zero + position) * 0.5f);
                }
                Vector3 from = Vector3.Project(vector5 - this.baseTransform.position, this.maincamera.transform.up);
                Vector3 vector7 = Vector3.Project(vector5 - this.baseTransform.position, this.maincamera.transform.right);
                if (vector5.magnitude > 0f)
                {
                    Vector3 to = from + vector7;
                    float num = Vector3.Angle(vector5 - this.baseTransform.position, this.baseRigidBody.velocity) * 0.005f;
                    Vector3 vector9 = this.maincamera.transform.right + vector7.normalized;
                    quaternion2 = Quaternion.Euler(this.maincamera.transform.rotation.eulerAngles.x, this.maincamera.transform.rotation.eulerAngles.y, (vector9.magnitude >= 1f) ? (-Vector3.Angle(from, to) * num) : (Vector3.Angle(from, to) * num));
                }
                else
                {
                    quaternion2 = Quaternion.Euler(this.maincamera.transform.rotation.eulerAngles.x, this.maincamera.transform.rotation.eulerAngles.y, 0f);
                }
                this.maincamera.transform.rotation = Quaternion.Lerp(this.maincamera.transform.rotation, quaternion2, Time.deltaTime * 2f);
            }
            if ((this.state == HERO_STATE.Grab) && (this.titanWhoGrabMe != null))
            {
                if (this.titanWhoGrabMe.GetComponent<TITAN>() != null)
                {
                    this.baseTransform.position = this.titanWhoGrabMe.GetComponent<TITAN>().grabTF.transform.position;
                    this.baseTransform.rotation = this.titanWhoGrabMe.GetComponent<TITAN>().grabTF.transform.rotation;
                }
                else if (this.titanWhoGrabMe.GetComponent<FEMALE_TITAN>() != null)
                {
                    this.baseTransform.position = this.titanWhoGrabMe.GetComponent<FEMALE_TITAN>().grabTF.transform.position;
                    this.baseTransform.rotation = this.titanWhoGrabMe.GetComponent<FEMALE_TITAN>().grabTF.transform.rotation;
                }
            }
            if (this.useGun)
            {
                if (this.leftArmAim || this.rightArmAim)
                {
                    Vector3 vector10 = this.gunTarget - this.baseTransform.position;
                    float current = -Mathf.Atan2(vector10.z, vector10.x) * 57.29578f;
                    float num3 = -Mathf.DeltaAngle(current, this.baseTransform.rotation.eulerAngles.y - 90f);
                    this.headMovement();
                    if ((!this.isLeftHandHooked && this.leftArmAim) && ((num3 < 40f) && (num3 > -90f)))
                    {
                        this.leftArmAimTo(this.gunTarget);
                    }
                    if ((!this.isRightHandHooked && this.rightArmAim) && ((num3 > -40f) && (num3 < 90f)))
                    {
                        this.rightArmAimTo(this.gunTarget);
                    }
                }
                else if (!this.grounded)
                {
                    this.handL.localRotation = Quaternion.Euler(90f, 0f, 0f);
                    this.handR.localRotation = Quaternion.Euler(-90f, 0f, 0f);
                }
                if (this.isLeftHandHooked && (this.bulletLeft != null))
                {
                    this.leftArmAimTo(this.bulletLeft.transform.position);
                }
                if (this.isRightHandHooked && (this.bulletRight != null))
                {
                    this.rightArmAimTo(this.bulletRight.transform.position);
                }
            }
            this.setHookedPplDirection();
            this.bodyLean();
        }
    }

    public void launch(Vector3 des, bool left = true, bool leviMode = false)
    {
        if (this.isMounted)
        {
            this.unmounted();
        }
        if (this.state != HERO_STATE.Attack)
        {
            this.idle();
        }
        Vector3 vector = des - base.transform.position;
        if (left)
        {
            this.launchPointLeft = des;
        }
        else
        {
            this.launchPointRight = des;
        }
        vector.Normalize();
        vector = (Vector3)(vector * 20f);
        if (((this.bulletLeft != null) && (this.bulletRight != null)) && (this.bulletLeft.GetComponent<Bullet>().isHooked() && this.bulletRight.GetComponent<Bullet>().isHooked()))
        {
            vector = (Vector3)(vector * 0.8f);
        }
        if (!base.GetComponent<Animation>().IsPlaying("attack5") && !base.GetComponent<Animation>().IsPlaying("special_petra"))
        {
            leviMode = false;
        }
        else
        {
            leviMode = true;
        }
        if (!leviMode)
        {
            this.falseAttack();
            this.idle();
            if (this.useGun)
            {
                this.crossFade("AHSS_hook_forward_both", 0.1f);
            }
            else if (left && !this.isRightHandHooked)
            {
                this.crossFade("air_hook_l_just", 0.1f);
            }
            else if (!left && !this.isLeftHandHooked)
            {
                this.crossFade("air_hook_r_just", 0.1f);
            }
            else
            {
                this.crossFade("dash", 0.1f);
                base.GetComponent<Animation>()["dash"].time = 0f;
            }
        }
        if (left)
        {
            this.isLaunchLeft = true;
        }
        if (!left)
        {
            this.isLaunchRight = true;
        }
        this.launchForce = vector;
        if (!leviMode)
        {
            if (vector.y < 30f)
            {
                this.launchForce += (Vector3)(Vector3.up * (30f - vector.y));
            }
            if (des.y >= base.transform.position.y)
            {
                this.launchForce += (Vector3)((Vector3.up * (des.y - base.transform.position.y)) * 10f);
            }
            base.GetComponent<Rigidbody>().AddForce(this.launchForce);
        }
        this.facingDirection = Mathf.Atan2(this.launchForce.x, this.launchForce.z) * 57.29578f;
        Quaternion quaternion = Quaternion.Euler(0f, this.facingDirection, 0f);
        base.gameObject.transform.rotation = quaternion;
        base.GetComponent<Rigidbody>().rotation = quaternion;
        this.targetRotation = quaternion;
        if (left)
        {
            this.launchElapsedTimeL = 0f;
        }
        else
        {
            this.launchElapsedTimeR = 0f;
        }
        if (leviMode)
        {
            this.launchElapsedTimeR = -100f;
        }
        if (base.GetComponent<Animation>().IsPlaying("special_petra"))
        {
            this.launchElapsedTimeR = -100f;
            this.launchElapsedTimeL = -100f;
            if (this.bulletRight != null)
            {
                this.bulletRight.GetComponent<Bullet>().disable();
                this.releaseIfIHookSb();
            }
            if (this.bulletLeft != null)
            {
                this.bulletLeft.GetComponent<Bullet>().disable();
                this.releaseIfIHookSb();
            }
        }
        this.sparks.enableEmission = false;
    }

    private void launchLeftRope(RaycastHit hit, bool single, int mode = 0)
    {
        if (this.currentGas != 0f)
        {
            this.useGas(0f);
            if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
            {
                this.bulletLeft = (GameObject)UnityEngine.Object.Instantiate(Resources.Load("hook"));
            }
            else if (base.photonView.isMine)
            {
                this.bulletLeft = PhotonNetwork.Instantiate("hook", base.transform.position, base.transform.rotation, 0);
            }
            GameObject obj2 = !this.useGun ? this.hookRefL1 : this.hookRefL2;
            string str = !this.useGun ? "hookRefL1" : "hookRefL2";
            this.bulletLeft.transform.position = obj2.transform.position;
            Bullet component = this.bulletLeft.GetComponent<Bullet>();
            float num = !single ? ((hit.distance <= 50f) ? (hit.distance * 0.05f) : (hit.distance * 0.3f)) : 0f;
            Vector3 vector = (hit.point - ((Vector3)(base.transform.right * num))) - this.bulletLeft.transform.position;
            vector.Normalize();
            if (mode == 1)
            {
                component.launch((Vector3)(vector * 3f), base.GetComponent<Rigidbody>().velocity, str, true, base.gameObject, true);
            }
            else
            {
                component.launch((Vector3)(vector * 3f), base.GetComponent<Rigidbody>().velocity, str, true, base.gameObject, false);
            }
            this.launchPointLeft = Vector3.zero;
        }
    }

    private void launchRightRope(RaycastHit hit, bool single, int mode = 0)
    {
        if (this.currentGas != 0f)
        {
            this.useGas(0f);
            if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
            {
                this.bulletRight = (GameObject)UnityEngine.Object.Instantiate(Resources.Load("hook"));
            }
            else if (base.photonView.isMine)
            {
                this.bulletRight = PhotonNetwork.Instantiate("hook", base.transform.position, base.transform.rotation, 0);
            }
            GameObject obj2 = !this.useGun ? this.hookRefR1 : this.hookRefR2;
            string str = !this.useGun ? "hookRefR1" : "hookRefR2";
            this.bulletRight.transform.position = obj2.transform.position;
            Bullet component = this.bulletRight.GetComponent<Bullet>();
            float num = !single ? ((hit.distance <= 50f) ? (hit.distance * 0.05f) : (hit.distance * 0.3f)) : 0f;
            Vector3 vector = (hit.point + ((Vector3)(base.transform.right * num))) - this.bulletRight.transform.position;
            vector.Normalize();
            if (mode == 1)
            {
                component.launch((Vector3)(vector * 5f), base.GetComponent<Rigidbody>().velocity, str, false, base.gameObject, true);
            }
            else
            {
                component.launch((Vector3)(vector * 3f), base.GetComponent<Rigidbody>().velocity, str, false, base.gameObject, false);
            }
            this.launchPointRight = Vector3.zero;
        }
    }

    private void leftArmAimTo(Vector3 target)
    {
        float y = target.x - this.upperarmL.transform.position.x;
        float num2 = target.y - this.upperarmL.transform.position.y;
        float x = target.z - this.upperarmL.transform.position.z;
        float num4 = Mathf.Sqrt((y * y) + (x * x));
        this.handL.localRotation = Quaternion.Euler(90f, 0f, 0f);
        this.forearmL.localRotation = Quaternion.Euler(-90f, 0f, 0f);
        this.upperarmL.rotation = Quaternion.Euler(0f, 90f + (Mathf.Atan2(y, x) * 57.29578f), -Mathf.Atan2(num2, num4) * 57.29578f);
    }

    public void loadskin()
    {
        if ((IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE) || base.photonView.isMine)
        {
            if (((int)FengGameManagerMKII.settings[0x5d]) == 1)
            {
                foreach (Renderer renderer in base.GetComponentsInChildren<Renderer>())
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
                if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
                {
                    base.StartCoroutine(this.loadskinE(-1, url));
                }
                else
                {
                    int viewID = -1;
                    if (this.myHorse != null)
                    {
                        viewID = this.myHorse.GetPhotonView().viewID;
                    }
                    base.photonView.RPC("loadskinRPC", PhotonTargets.AllBuffered, new object[] { viewID, url });
                }
            }
        }
    }

    public IEnumerator loadskinE(int horse, string url)
    {
        while (!this.hasspawn)
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
        if (FengGameManagerMKII.Gamemode.Horse)
        {
            iteratorVariable4 = true;
        }
        bool iteratorVariable5 = false;
        if ((IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE) || this.photonView.isMine)
        {
            iteratorVariable5 = true;
        }
        if (this.setup.part_hair_1 != null)
        {
            Renderer renderer = this.setup.part_hair_1.GetComponent<Renderer>();
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
                        if (this.setup.myCostume.hairInfo.id >= 0)
                        {
                            renderer.material = CharacterMaterials.materials[this.setup.myCostume.hairInfo.texture];
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
        if (this.setup.part_cape != null)
        {
            Renderer iteratorVariable9 = this.setup.part_cape.GetComponent<Renderer>();
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
        if (this.setup.part_chest_3 != null)
        {
            Renderer iteratorVariable12 = this.setup.part_chest_3.GetComponent<Renderer>();
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
        foreach (Renderer iteratorVariable15 in this.GetComponentsInChildren<Renderer>())
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
                            if (this.setup.myCostume.hairInfo.id >= 0)
                            {
                                iteratorVariable15.material = CharacterMaterials.materials[this.setup.myCostume.hairInfo.texture];
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
                    this.leftbladetrail.MyMaterial.mainTexture = iteratorVariable43;
                    this.rightbladetrail.MyMaterial.mainTexture = iteratorVariable43;
                    FengGameManagerMKII.linkHash[0].Add(iteratorVariable2[12], this.leftbladetrail.MyMaterial);
                    this.leftbladetrail.MyMaterial = (Material)FengGameManagerMKII.linkHash[0][iteratorVariable2[12]];
                    this.rightbladetrail.MyMaterial = (Material)FengGameManagerMKII.linkHash[0][iteratorVariable2[12]];
                    this.leftbladetrail2.MyMaterial = this.leftbladetrail.MyMaterial;
                    this.rightbladetrail2.MyMaterial = this.leftbladetrail.MyMaterial;
                }
                else
                {
                    this.leftbladetrail2.MyMaterial = (Material)FengGameManagerMKII.linkHash[0][iteratorVariable2[12]];
                    this.rightbladetrail2.MyMaterial = (Material)FengGameManagerMKII.linkHash[0][iteratorVariable2[12]];
                    this.leftbladetrail.MyMaterial = (Material)FengGameManagerMKII.linkHash[0][iteratorVariable2[12]];
                    this.rightbladetrail.MyMaterial = (Material)FengGameManagerMKII.linkHash[0][iteratorVariable2[12]];
                }
            }
            else
            {
                this.leftbladetrail2.MyMaterial = (Material)FengGameManagerMKII.linkHash[0][iteratorVariable2[12]];
                this.rightbladetrail2.MyMaterial = (Material)FengGameManagerMKII.linkHash[0][iteratorVariable2[12]];
                this.leftbladetrail.MyMaterial = (Material)FengGameManagerMKII.linkHash[0][iteratorVariable2[12]];
                this.rightbladetrail.MyMaterial = (Material)FengGameManagerMKII.linkHash[0][iteratorVariable2[12]];
            }
        }
        if (iteratorVariable1)
        {
            FengGameManagerMKII.instance.unloadAssets();
        }
    }

    [PunRPC]
    public void loadskinRPC(int horse, string url)
    {
        if (((int)FengGameManagerMKII.settings[0]) == 1)
        {
            base.StartCoroutine(this.loadskinE(horse, url));
        }
    }

    public void markDie()
    {
        this.hasDied = true;
        this.state = HERO_STATE.Die;
    }

    [PunRPC]
    public void moveToRPC(float posX, float posY, float posZ, PhotonMessageInfo info)
    {
        if (info.sender.isMasterClient)
        {
            base.transform.position = new Vector3(posX, posY, posZ);
        }
    }

    [PunRPC]
    private void net3DMGSMOKE(bool ifON)
    {
        if (this.smoke_3dmg != null)
        {
            this.smoke_3dmg.enableEmission = ifON;
        }
    }

    [PunRPC]
    private void netContinueAnimation()
    {
        IEnumerator enumerator = base.GetComponent<Animation>().GetEnumerator();
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
        this.playAnimation(this.currentPlayingClipName());
    }

    [PunRPC]
    private void netCrossFade(string aniName, float time)
    {
        this.currentAnimation = aniName;
        if (base.GetComponent<Animation>() != null)
        {
            base.GetComponent<Animation>().CrossFade(aniName, time);
        }
    }

    [PunRPC]
    public void netDie(Vector3 v, bool isBite, int viewID = -1, string titanName = "", bool killByTitan = true, PhotonMessageInfo info = new PhotonMessageInfo())
    {
        if ((base.photonView.isMine && (FengGameManagerMKII.Gamemode.GamemodeType != GamemodeType.TitanRush)))
        {
            if (FengGameManagerMKII.ignoreList.Contains(info.sender.ID))
            {
                base.photonView.RPC("backToHumanRPC", PhotonTargets.Others, new object[0]);
                return;
            }
            if (!info.sender.isLocal && !info.sender.isMasterClient)
            {
                if ((info.sender.CustomProperties[PhotonPlayerProperty.name] == null) || (info.sender.CustomProperties[PhotonPlayerProperty.isTitan] == null))
                {
                    FengGameManagerMKII.instance.chatRoom.addLINE("<color=#FFCC00>Unusual Kill from ID " + info.sender.ID.ToString() + "</color>");
                }
                else if (viewID < 0)
                {
                    if (titanName == "")
                    {
                        FengGameManagerMKII.instance.chatRoom.addLINE("<color=#FFCC00>Unusual Kill from ID " + info.sender.ID.ToString() + " (possibly valid).</color>");
                    }
                    else
                    {
                        FengGameManagerMKII.instance.chatRoom.addLINE("<color=#FFCC00>Unusual Kill from ID " + info.sender.ID.ToString() + "</color>");
                    }
                }
                else if (PhotonView.Find(viewID) == null)
                {
                    FengGameManagerMKII.instance.chatRoom.addLINE("<color=#FFCC00>Unusual Kill from ID " + info.sender.ID.ToString() + "</color>");
                }
                else if (PhotonView.Find(viewID).owner.ID != info.sender.ID)
                {
                    FengGameManagerMKII.instance.chatRoom.addLINE("<color=#FFCC00>Unusual Kill from ID " + info.sender.ID.ToString() + "</color>");
                }
            }
        }
        if (PhotonNetwork.isMasterClient)
        {
            this.onDeathEvent(viewID, killByTitan);
            int iD = base.photonView.owner.ID;
            if (FengGameManagerMKII.heroHash.ContainsKey(iD))
            {
                FengGameManagerMKII.heroHash.Remove(iD);
            }
        }
        if (base.photonView.isMine)
        {
            Vector3 vector = (Vector3)(Vector3.up * 5000f);
            if (this.myBomb != null)
            {
                this.myBomb.destroyMe();
            }
            if (this.myCannon != null)
            {
                PhotonNetwork.Destroy(this.myCannon);
            }
            if (this.titanForm && (this.eren_titan != null))
            {
                this.eren_titan.GetComponent<TITAN_EREN>().lifeTime = 0.1f;
            }
            if (this.skillCD != null)
            {
                this.skillCD.transform.localPosition = vector;
            }
        }
        if (this.bulletLeft != null)
        {
            this.bulletLeft.GetComponent<Bullet>().removeMe();
        }
        if (this.bulletRight != null)
        {
            this.bulletRight.GetComponent<Bullet>().removeMe();
        }
        this.meatDie.Play();
        if (!(this.useGun || ((IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.SINGLE) && !base.photonView.isMine)))
        {
            //TODO: Re-enable these again
            //this.leftbladetrail.Deactivate();
            //this.rightbladetrail.Deactivate();
            //this.leftbladetrail2.Deactivate();
            //this.rightbladetrail2.Deactivate();
        }
        this.falseAttack();
        this.breakApart2(v, isBite);
        if (base.photonView.isMine)
        {
            this.currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().setSpectorMode(false);
            this.currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().gameOver = true;
            FengGameManagerMKII.instance.myRespawnTime = 0f;
        }
        this.hasDied = true;
        Transform transform = base.transform.Find("audio_die");
        if (transform != null)
        {
            transform.parent = null;
            transform.GetComponent<AudioSource>().Play();
        }
        base.gameObject.GetComponent<SmoothSyncMovement>().disabled = true;
        if (base.photonView.isMine)
        {
            PhotonNetwork.RemoveRPCs(base.photonView);
            ExitGames.Client.Photon.Hashtable propertiesToSet = new ExitGames.Client.Photon.Hashtable();
            propertiesToSet.Add(PhotonPlayerProperty.dead, true);
            PhotonNetwork.player.SetCustomProperties(propertiesToSet);
            propertiesToSet = new ExitGames.Client.Photon.Hashtable();
            propertiesToSet.Add(PhotonPlayerProperty.deaths, RCextensions.returnIntFromObject(PhotonNetwork.player.CustomProperties[PhotonPlayerProperty.deaths]) + 1);
            PhotonNetwork.player.SetCustomProperties(propertiesToSet);
            object[] parameters = new object[] { !(titanName == string.Empty) ? 1 : 0 };
            FengGameManagerMKII.instance.photonView.RPC("someOneIsDead", PhotonTargets.MasterClient, parameters);
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
        if (base.photonView.isMine)
        {
            PhotonNetwork.Destroy(base.photonView);
        }
    }

    [PunRPC]
    private void netDie2(int viewID = -1, string titanName = "", PhotonMessageInfo info = new PhotonMessageInfo())
    {
        GameObject obj2;
        if ((base.photonView.isMine) && (FengGameManagerMKII.Gamemode.GamemodeType != GamemodeType.TitanRush))
        {
            if (FengGameManagerMKII.ignoreList.Contains(info.sender.ID))
            {
                base.photonView.RPC("backToHumanRPC", PhotonTargets.Others, new object[0]);
                return;
            }
            if (!info.sender.isLocal && !info.sender.isMasterClient)
            {
                if ((info.sender.CustomProperties[PhotonPlayerProperty.name] == null) || (info.sender.CustomProperties[PhotonPlayerProperty.isTitan] == null))
                {
                    FengGameManagerMKII.instance.chatRoom.addLINE("<color=#FFCC00>Unusual Kill from ID " + info.sender.ID.ToString() + "</color>");
                }
                else if (viewID < 0)
                {
                    if (titanName == "")
                    {
                        FengGameManagerMKII.instance.chatRoom.addLINE("<color=#FFCC00>Unusual Kill from ID " + info.sender.ID.ToString() + " (possibly valid).</color>");
                    }
                    else if ((FengGameManagerMKII.Gamemode.PvPBomb) && (!FengGameManagerMKII.Gamemode.PvpCannons))
                    {
                        FengGameManagerMKII.instance.chatRoom.addLINE("<color=#FFCC00>Unusual Kill from ID " + info.sender.ID.ToString() + "</color>");
                    }
                }
                else if (PhotonView.Find(viewID) == null)
                {
                    FengGameManagerMKII.instance.chatRoom.addLINE("<color=#FFCC00>Unusual Kill from ID " + info.sender.ID.ToString() + "</color>");
                }
                else if (PhotonView.Find(viewID).owner.ID != info.sender.ID)
                {
                    FengGameManagerMKII.instance.chatRoom.addLINE("<color=#FFCC00>Unusual Kill from ID " + info.sender.ID.ToString() + "</color>");
                }
            }
        }
        if (base.photonView.isMine)
        {
            Vector3 vector = (Vector3)(Vector3.up * 5000f);
            if (this.myBomb != null)
            {
                this.myBomb.destroyMe();
            }
            if (this.myCannon != null)
            {
                PhotonNetwork.Destroy(this.myCannon);
            }
            PhotonNetwork.RemoveRPCs(base.photonView);
            if (this.titanForm && (this.eren_titan != null))
            {
                this.eren_titan.GetComponent<TITAN_EREN>().lifeTime = 0.1f;
            }
            if (this.skillCD != null)
            {
                this.skillCD.transform.localPosition = vector;
            }
        }
        this.meatDie.Play();
        if (this.bulletLeft != null)
        {
            this.bulletLeft.GetComponent<Bullet>().removeMe();
        }
        if (this.bulletRight != null)
        {
            this.bulletRight.GetComponent<Bullet>().removeMe();
        }
        Transform transform = base.transform.Find("audio_die");
        transform.parent = null;
        transform.GetComponent<AudioSource>().Play();
        if (base.photonView.isMine)
        {
            this.currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().setMainObject(null, true, false);
            this.currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().setSpectorMode(true);
            this.currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().gameOver = true;
            FengGameManagerMKII.instance.myRespawnTime = 0f;
        }
        this.falseAttack();
        this.hasDied = true;
        base.gameObject.GetComponent<SmoothSyncMovement>().disabled = true;
        if ((IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER) && base.photonView.isMine)
        {
            PhotonNetwork.RemoveRPCs(base.photonView);
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
            object[] parameters = new object[] { !(titanName == string.Empty) ? 1 : 0 };
            FengGameManagerMKII.instance.photonView.RPC("someOneIsDead", PhotonTargets.MasterClient, parameters);
        }
        if ((IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.SINGLE) && base.photonView.isMine)
        {
            obj2 = PhotonNetwork.Instantiate("hitMeat2", base.transform.position, Quaternion.Euler(270f, 0f, 0f), 0);
        }
        else
        {
            obj2 = (GameObject)UnityEngine.Object.Instantiate(Resources.Load("hitMeat2"));
        }
        obj2.transform.position = base.transform.position;
        if (base.photonView.isMine)
        {
            PhotonNetwork.Destroy(base.photonView);
        }
        if (PhotonNetwork.isMasterClient)
        {
            this.onDeathEvent(viewID, true);
            int iD = base.photonView.owner.ID;
            if (FengGameManagerMKII.heroHash.ContainsKey(iD))
            {
                FengGameManagerMKII.heroHash.Remove(iD);
            }
        }
    }

    public void netDieLocal(Vector3 v, bool isBite, int viewID = -1, string titanName = "", bool killByTitan = true)
    {
        if (base.photonView.isMine)
        {
            Vector3 vector = (Vector3)(Vector3.up * 5000f);
            if (this.titanForm && (this.eren_titan != null))
            {
                this.eren_titan.GetComponent<TITAN_EREN>().lifeTime = 0.1f;
            }
            if (this.myBomb != null)
            {
                this.myBomb.destroyMe();
            }
            if (this.myCannon != null)
            {
                PhotonNetwork.Destroy(this.myCannon);
            }
            if (this.skillCD != null)
            {
                this.skillCD.transform.localPosition = vector;
            }
        }
        if (this.bulletLeft != null)
        {
            this.bulletLeft.GetComponent<Bullet>().removeMe();
        }
        if (this.bulletRight != null)
        {
            this.bulletRight.GetComponent<Bullet>().removeMe();
        }
        this.meatDie.Play();
        if (!(this.useGun || ((IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.SINGLE) && !base.photonView.isMine)))
        {
            this.leftbladetrail.Deactivate();
            this.rightbladetrail.Deactivate();
            this.leftbladetrail2.Deactivate();
            this.rightbladetrail2.Deactivate();
        }
        this.falseAttack();
        this.breakApart2(v, isBite);
        if (base.photonView.isMine)
        {
            this.currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().setSpectorMode(false);
            this.currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().gameOver = true;
            FengGameManagerMKII.instance.myRespawnTime = 0f;
        }
        this.hasDied = true;
        Transform transform = base.transform.Find("audio_die");
        transform.parent = null;
        transform.GetComponent<AudioSource>().Play();
        base.gameObject.GetComponent<SmoothSyncMovement>().disabled = true;
        if (base.photonView.isMine)
        {
            PhotonNetwork.RemoveRPCs(base.photonView);
            ExitGames.Client.Photon.Hashtable propertiesToSet = new ExitGames.Client.Photon.Hashtable();
            propertiesToSet.Add(PhotonPlayerProperty.dead, true);
            PhotonNetwork.player.SetCustomProperties(propertiesToSet);
            propertiesToSet = new ExitGames.Client.Photon.Hashtable();
            propertiesToSet.Add(PhotonPlayerProperty.deaths, RCextensions.returnIntFromObject(PhotonNetwork.player.CustomProperties[PhotonPlayerProperty.deaths]) + 1);
            PhotonNetwork.player.SetCustomProperties(propertiesToSet);
            object[] parameters = new object[] { !(titanName == string.Empty) ? 1 : 0 };
            FengGameManagerMKII.instance.photonView.RPC("someOneIsDead", PhotonTargets.MasterClient, parameters);
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
        if (base.photonView.isMine)
        {
            PhotonNetwork.Destroy(base.photonView);
        }
        if (PhotonNetwork.isMasterClient)
        {
            this.onDeathEvent(viewID, killByTitan);
            int iD = base.photonView.owner.ID;
            if (FengGameManagerMKII.heroHash.ContainsKey(iD))
            {
                FengGameManagerMKII.heroHash.Remove(iD);
            }
        }
    }

    [PunRPC]
    private void netGrabbed(int id, bool leftHand)
    {
        this.titanWhoGrabMeID = id;
        this.grabbed(PhotonView.Find(id).gameObject, leftHand);
    }

    [PunRPC]
    private void netlaughAttack()
    {
        foreach (GameObject obj2 in GameObject.FindGameObjectsWithTag("titan"))
        {
            if (((Vector3.Distance(obj2.transform.position, base.transform.position) < 50f) && (Vector3.Angle(obj2.transform.forward, base.transform.position - obj2.transform.position) < 90f)) && (obj2.GetComponent<TITAN>() != null))
            {
                obj2.GetComponent<TITAN>().beLaughAttacked();
            }
        }
    }

    [PunRPC]
    private void netPauseAnimation()
    {
        IEnumerator enumerator = base.GetComponent<Animation>().GetEnumerator();
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
    private void netPlayAnimation(string aniName)
    {
        this.currentAnimation = aniName;
        if (base.GetComponent<Animation>() != null)
        {
            base.GetComponent<Animation>().Play(aniName);
        }
    }

    [PunRPC]
    private void netPlayAnimationAt(string aniName, float normalizedTime)
    {
        this.currentAnimation = aniName;
        if (base.GetComponent<Animation>() != null)
        {
            base.GetComponent<Animation>().Play(aniName);
            base.GetComponent<Animation>()[aniName].normalizedTime = normalizedTime;
        }
    }

    [PunRPC]
    private void netSetIsGrabbedFalse()
    {
        this.state = HERO_STATE.Idle;
    }

    [PunRPC]
    private void netTauntAttack(float tauntTime, float distance = 100f)
    {
        foreach (GameObject obj2 in GameObject.FindGameObjectsWithTag("titan"))
        {
            if ((Vector3.Distance(obj2.transform.position, base.transform.position) < distance) && (obj2.GetComponent<TITAN>() != null))
            {
                obj2.GetComponent<TITAN>().beTauntedBy(base.gameObject, tauntTime);
            }
        }
    }

    [PunRPC]
    private void netUngrabbed()
    {
        this.ungrabbed();
        this.netPlayAnimation(this.standAnimation);
        this.falseAttack();
    }

    public void onDeathEvent(int viewID, bool isTitan)
    {
        RCEvent event2;
        string[] strArray;
        if (isTitan)
        {
            if (FengGameManagerMKII.RCEvents.ContainsKey("OnPlayerDieByTitan"))
            {
                event2 = (RCEvent)FengGameManagerMKII.RCEvents["OnPlayerDieByTitan"];
                strArray = (string[])FengGameManagerMKII.RCVariableNames["OnPlayerDieByTitan"];
                if (FengGameManagerMKII.playerVariables.ContainsKey(strArray[0]))
                {
                    FengGameManagerMKII.playerVariables[strArray[0]] = base.photonView.owner;
                }
                else
                {
                    FengGameManagerMKII.playerVariables.Add(strArray[0], base.photonView.owner);
                }
                if (FengGameManagerMKII.titanVariables.ContainsKey(strArray[1]))
                {
                    FengGameManagerMKII.titanVariables[strArray[1]] = PhotonView.Find(viewID).gameObject.GetComponent<TITAN>();
                }
                else
                {
                    FengGameManagerMKII.titanVariables.Add(strArray[1], PhotonView.Find(viewID).gameObject.GetComponent<TITAN>());
                }
                event2.checkEvent();
            }
        }
        else if (FengGameManagerMKII.RCEvents.ContainsKey("OnPlayerDieByPlayer"))
        {
            event2 = (RCEvent)FengGameManagerMKII.RCEvents["OnPlayerDieByPlayer"];
            strArray = (string[])FengGameManagerMKII.RCVariableNames["OnPlayerDieByPlayer"];
            if (FengGameManagerMKII.playerVariables.ContainsKey(strArray[0]))
            {
                FengGameManagerMKII.playerVariables[strArray[0]] = base.photonView.owner;
            }
            else
            {
                FengGameManagerMKII.playerVariables.Add(strArray[0], base.photonView.owner);
            }
            if (FengGameManagerMKII.playerVariables.ContainsKey(strArray[1]))
            {
                FengGameManagerMKII.playerVariables[strArray[1]] = PhotonView.Find(viewID).owner;
            }
            else
            {
                FengGameManagerMKII.playerVariables.Add(strArray[1], PhotonView.Find(viewID).owner);
            }
            event2.checkEvent();
        }
    }

    private void OnDestroy()
    {
        if (this.myNetWorkName != null)
        {
            UnityEngine.Object.Destroy(this.myNetWorkName);
        }
        if (this.gunDummy != null)
        {
            UnityEngine.Object.Destroy(this.gunDummy);
        }
        if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER)
        {
            this.releaseIfIHookSb();
        }
        if (GameObject.Find("MultiplayerManager") != null)
        {
            GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>().removeHero(this);
        }
        //if ((IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER) && base.photonView.isMine)
        //{
        //    Vector3 vector = (Vector3) (Vector3.up * 5000f);
        //    this.cross1.transform.localPosition = vector;
        //    this.cross2.transform.localPosition = vector;
        //    this.crossL1.transform.localPosition = vector;
        //    this.crossL2.transform.localPosition = vector;
        //    this.crossR1.transform.localPosition = vector;
        //    this.crossR2.transform.localPosition = vector;
        //    this.LabelDistance.transform.localPosition = vector;
        //}
        if (this.setup.part_cape != null)
        {
            ClothFactory.DisposeObject(this.setup.part_cape);
        }
        if (this.setup.part_hair_1 != null)
        {
            ClothFactory.DisposeObject(this.setup.part_hair_1);
        }
        if (this.setup.part_hair_2 != null)
        {
            ClothFactory.DisposeObject(this.setup.part_hair_2);
        }
    }

    public void pauseAnimation()
    {
        IEnumerator enumerator = base.GetComponent<Animation>().GetEnumerator();
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
        if ((IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.SINGLE) && base.photonView.isMine)
        {
            base.photonView.RPC("netPauseAnimation", PhotonTargets.Others, new object[0]);
        }
    }

    public void playAnimation(string aniName)
    {
        this.currentAnimation = aniName;
        base.GetComponent<Animation>().Play(aniName);
        if (PhotonNetwork.connected && base.photonView.isMine)
        {
            object[] parameters = new object[] { aniName };
            base.photonView.RPC("netPlayAnimation", PhotonTargets.Others, parameters);
        }
    }

    private void playAnimationAt(string aniName, float normalizedTime)
    {
        this.currentAnimation = aniName;
        base.GetComponent<Animation>().Play(aniName);
        base.GetComponent<Animation>()[aniName].normalizedTime = normalizedTime;
        if (PhotonNetwork.connected && base.photonView.isMine)
        {
            object[] parameters = new object[] { aniName, normalizedTime };
            base.photonView.RPC("netPlayAnimationAt", PhotonTargets.Others, parameters);
        }
    }

    private void releaseIfIHookSb()
    {
        if (this.hookSomeOne && (this.hookTarget != null))
        {
            this.hookTarget.GetPhotonView().RPC("badGuyReleaseMe", this.hookTarget.GetPhotonView().owner, new object[0]);
            this.hookTarget = null;
            this.hookSomeOne = false;
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
        IEnumerator enumerator = base.GetComponent<Animation>().GetEnumerator();
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
        this.customAnimationSpeed();
    }

    [PunRPC]
    public void ReturnFromCannon(PhotonMessageInfo info)
    {
        if (info.sender == base.photonView.owner)
        {
            this.isCannon = false;
            base.gameObject.GetComponent<SmoothSyncMovement>().disabled = false;
        }
    }

    private void rightArmAimTo(Vector3 target)
    {
        float y = target.x - this.upperarmR.transform.position.x;
        float num2 = target.y - this.upperarmR.transform.position.y;
        float x = target.z - this.upperarmR.transform.position.z;
        float num4 = Mathf.Sqrt((y * y) + (x * x));
        this.handR.localRotation = Quaternion.Euler(-90f, 0f, 0f);
        this.forearmR.localRotation = Quaternion.Euler(90f, 0f, 0f);
        this.upperarmR.rotation = Quaternion.Euler(180f, 90f + (Mathf.Atan2(y, x) * 57.29578f), Mathf.Atan2(num2, num4) * 57.29578f);
    }

    [PunRPC]
    private void RPCHookedByHuman(int hooker, Vector3 hookPosition)
    {
        this.hookBySomeOne = true;
        this.badGuy = PhotonView.Find(hooker).gameObject;
        if (Vector3.Distance(hookPosition, base.transform.position) < 15f)
        {
            this.launchForce = PhotonView.Find(hooker).gameObject.transform.position - base.transform.position;
            base.GetComponent<Rigidbody>().AddForce((Vector3)(-base.GetComponent<Rigidbody>().velocity * 0.9f), ForceMode.VelocityChange);
            float num = Mathf.Pow(this.launchForce.magnitude, 0.1f);
            if (this.grounded)
            {
                base.GetComponent<Rigidbody>().AddForce((Vector3)(Vector3.up * Mathf.Min((float)(this.launchForce.magnitude * 0.2f), (float)10f)), ForceMode.Impulse);
            }
            base.GetComponent<Rigidbody>().AddForce((Vector3)((this.launchForce * num) * 0.1f), ForceMode.Impulse);
            if (this.state != HERO_STATE.Grab)
            {
                this.dashTime = 1f;
                this.crossFade("dash", 0.05f);
                base.GetComponent<Animation>()["dash"].time = 0.1f;
                this.state = HERO_STATE.AirDodge;
                this.falseAttack();
                this.facingDirection = Mathf.Atan2(this.launchForce.x, this.launchForce.z) * 57.29578f;
                Quaternion quaternion = Quaternion.Euler(0f, this.facingDirection, 0f);
                base.gameObject.transform.rotation = quaternion;
                base.GetComponent<Rigidbody>().rotation = quaternion;
                this.targetRotation = quaternion;
            }
        }
        else
        {
            this.hookBySomeOne = false;
            this.badGuy = null;
            PhotonView.Find(hooker).RPC("hookFail", PhotonView.Find(hooker).owner, new object[0]);
        }
    }

    private void salute()
    {
        this.state = HERO_STATE.Salute;
        this.crossFade("salute", 0.1f);
    }

    private void setHookedPplDirection()
    {
        this.almostSingleHook = false;
        if (this.isRightHandHooked && this.isLeftHandHooked)
        {
            if ((this.bulletLeft != null) && (this.bulletRight != null))
            {
                Vector3 normal = this.bulletLeft.transform.position - this.bulletRight.transform.position;
                if (normal.sqrMagnitude < 4f)
                {
                    Vector3 vector2 = ((Vector3)((this.bulletLeft.transform.position + this.bulletRight.transform.position) * 0.5f)) - base.transform.position;
                    this.facingDirection = Mathf.Atan2(vector2.x, vector2.z) * 57.29578f;
                    if (this.useGun && (this.state != HERO_STATE.Attack))
                    {
                        float current = -Mathf.Atan2(base.GetComponent<Rigidbody>().velocity.z, base.GetComponent<Rigidbody>().velocity.x) * 57.29578f;
                        float target = -Mathf.Atan2(vector2.z, vector2.x) * 57.29578f;
                        float num3 = -Mathf.DeltaAngle(current, target);
                        this.facingDirection += num3;
                    }
                    this.almostSingleHook = true;
                }
                else
                {
                    Vector3 to = base.transform.position - this.bulletLeft.transform.position;
                    Vector3 vector6 = base.transform.position - this.bulletRight.transform.position;
                    Vector3 vector7 = (Vector3)((this.bulletLeft.transform.position + this.bulletRight.transform.position) * 0.5f);
                    Vector3 from = base.transform.position - vector7;
                    if ((Vector3.Angle(from, to) < 30f) && (Vector3.Angle(from, vector6) < 30f))
                    {
                        this.almostSingleHook = true;
                        Vector3 vector9 = vector7 - base.transform.position;
                        this.facingDirection = Mathf.Atan2(vector9.x, vector9.z) * 57.29578f;
                    }
                    else
                    {
                        this.almostSingleHook = false;
                        Vector3 forward = base.transform.forward;
                        Vector3.OrthoNormalize(ref normal, ref forward);
                        this.facingDirection = Mathf.Atan2(forward.x, forward.z) * 57.29578f;
                        float num4 = Mathf.Atan2(to.x, to.z) * 57.29578f;
                        if (Mathf.DeltaAngle(num4, this.facingDirection) > 0f)
                        {
                            this.facingDirection += 180f;
                        }
                    }
                }
            }
        }
        else
        {
            this.almostSingleHook = true;
            Vector3 zero = Vector3.zero;
            if (this.isRightHandHooked && (this.bulletRight != null))
            {
                zero = this.bulletRight.transform.position - base.transform.position;
            }
            else
            {
                if (!this.isLeftHandHooked || (this.bulletLeft == null))
                {
                    return;
                }
                zero = this.bulletLeft.transform.position - base.transform.position;
            }
            this.facingDirection = Mathf.Atan2(zero.x, zero.z) * 57.29578f;
            if (this.state != HERO_STATE.Attack)
            {
                float num6 = -Mathf.Atan2(base.GetComponent<Rigidbody>().velocity.z, base.GetComponent<Rigidbody>().velocity.x) * 57.29578f;
                float num7 = -Mathf.Atan2(zero.z, zero.x) * 57.29578f;
                float num8 = -Mathf.DeltaAngle(num6, num7);
                if (this.useGun)
                {
                    this.facingDirection += num8;
                }
                else
                {
                    float num9 = 0f;
                    if ((this.isLeftHandHooked && (num8 < 0f)) || (this.isRightHandHooked && (num8 > 0f)))
                    {
                        num9 = -0.1f;
                    }
                    else
                    {
                        num9 = 0.1f;
                    }
                    this.facingDirection += num8 * num9;
                }
            }
        }
    }

    [PunRPC]
    public void SetMyCannon(int viewID, PhotonMessageInfo info)
    {
        if (info.sender == base.photonView.owner)
        {
            PhotonView view = PhotonView.Find(viewID);
            if (view != null)
            {
                this.myCannon = view.gameObject;
                if (this.myCannon != null)
                {
                    this.myCannonBase = this.myCannon.transform;
                    this.myCannonPlayer = this.myCannonBase.Find("PlayerPoint");
                    this.isCannon = true;
                }
            }
        }
    }

    [PunRPC]
    public void SetMyPhotonCamera(float offset, PhotonMessageInfo info)
    {
        if (base.photonView.owner == info.sender)
        {
            this.CameraMultiplier = offset;
            base.GetComponent<SmoothSyncMovement>().PhotonCamera = true;
            this.isPhotonCamera = true;
        }
    }

    [PunRPC]
    private void setMyTeam(int val)
    {
        this.myTeam = val;
        this.checkBoxLeft.GetComponent<TriggerColliderWeapon>().myTeam = val;
        this.checkBoxRight.GetComponent<TriggerColliderWeapon>().myTeam = val;
        if ((IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER) && PhotonNetwork.isMasterClient)
        {
            object[] objArray;
            //TODO: Sync these upon gamemode syncSettings
            if (FengGameManagerMKII.Gamemode.Pvp == PvpMode.AhssVsBlades)
            {
                int num = 0;
                if (base.photonView.owner.CustomProperties[PhotonPlayerProperty.RCteam] != null)
                {
                    num = RCextensions.returnIntFromObject(base.photonView.owner.CustomProperties[PhotonPlayerProperty.RCteam]);
                }
                if (val != num)
                {
                    objArray = new object[] { num };
                    base.photonView.RPC("setMyTeam", PhotonTargets.AllBuffered, objArray);
                }
            }
            else if (FengGameManagerMKII.Gamemode.Pvp == PvpMode.FreeForAll && (val != base.photonView.owner.ID))
            {
                objArray = new object[] { base.photonView.owner.ID };
                base.photonView.RPC("setMyTeam", PhotonTargets.AllBuffered, objArray);
            }
        }
    }

    public void setSkillHUDPosition()
    {
        this.skillCD = GameObject.Find("skill_cd_" + this.skillId);
        if (this.skillCD != null)
        {
            this.skillCD.transform.localPosition = GameObject.Find("skill_cd_bottom").transform.localPosition;
        }
        if (this.useGun)
        {
            this.skillCD.transform.localPosition = (Vector3)(Vector3.up * 5000f);
        }
    }

    public void setSkillHUDPosition2()
    {
        return;
        this.skillCD = GameObject.Find("skill_cd_" + this.skillIDHUD);
        if (this.skillCD != null)
        {
            this.skillCD.transform.localPosition = GameObject.Find("skill_cd_bottom").transform.localPosition;
        }
        if (this.useGun && (FengGameManagerMKII.Gamemode.PvPBomb))
        {
            this.skillCD.transform.localPosition = (Vector3)(Vector3.up * 5000f);
        }
    }

    public void setStat2()
    {
        this.skillCDLast = 1.5f;
        this.skillId = this.setup.myCostume.stat.skillId;
        if (this.skillId == "levi")
        {
            this.skillCDLast = 3.5f;
        }
        this.customAnimationSpeed();
        if (this.skillId == "armin")
        {
            this.skillCDLast = 5f;
        }
        if (this.skillId == "marco")
        {
            this.skillCDLast = 10f;
        }
        if (this.skillId == "jean")
        {
            this.skillCDLast = 0.001f;
        }
        if (this.skillId == "eren")
        {
            this.skillCDLast = 120f;
            if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER)
            {
                if ((FengGameManagerMKII.Gamemode.IsPlayerTitanEnabled || !FengGameManagerMKII.Gamemode.PlayerTitanShifters))
                {
                    this.skillId = "petra";
                    this.skillCDLast = 1f;
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
                        this.skillId = "petra";
                        this.skillCDLast = 1f;
                    }
                }
            }
        }
        if (this.skillId == "sasha")
        {
            this.skillCDLast = 20f;
        }
        if (this.skillId == "petra")
        {
            this.skillCDLast = 3.5f;
        }
        this.bombInit();
        this.speed = ((float)this.setup.myCostume.stat.SPD) / 10f;
        this.totalGas = this.currentGas = this.setup.myCostume.stat.GAS;
        this.totalBladeSta = this.currentBladeSta = this.setup.myCostume.stat.BLA;
        this.baseRigidBody.mass = 0.5f - ((this.setup.myCostume.stat.ACL - 100) * 0.001f);
        //GameObject.Find("skill_cd_bottom").transform.localPosition = new Vector3(0f, (-Screen.height * 0.5f) + 5f, 0f);
        //this.skillCD = GameObject.Find("skill_cd_" + this.skillIDHUD);
        //this.skillCD.transform.localPosition = GameObject.Find("skill_cd_bottom").transform.localPosition;
        //GameObject.Find("GasUI").transform.localPosition = GameObject.Find("skill_cd_bottom").transform.localPosition;
        if ((IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE) || base.photonView.isMine)
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
            this.standAnimation = "AHSS_stand_gun";
            this.useGun = true;
            this.gunDummy = new GameObject();
            this.gunDummy.name = "gunDummy";
            this.gunDummy.transform.position = this.baseTransform.position;
            this.gunDummy.transform.rotation = this.baseTransform.rotation;
            this.myGroup = GROUP.A;
            this.setTeam2(2);
            if ((IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE) || base.photonView.isMine)
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
                //if (this.skillId != "bomb")
                //{
                //    this.skillCD.transform.localPosition = (Vector3) (Vector3.up * 5000f);
                //}
            }
        }
        else if (this.setup.myCostume.sex == SEX.FEMALE)
        {
            this.standAnimation = "stand";
            this.setTeam2(1);
        }
        else
        {
            this.standAnimation = "stand_levi";
            this.setTeam2(1);
        }
    }

    public void setTeam(int team)
    {
        this.setMyTeam(team);
        if ((IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER) && base.photonView.isMine)
        {
            object[] parameters = new object[] { team };
            base.photonView.RPC("setMyTeam", PhotonTargets.OthersBuffered, parameters);
            ExitGames.Client.Photon.Hashtable propertiesToSet = new ExitGames.Client.Photon.Hashtable();
            propertiesToSet.Add(PhotonPlayerProperty.team, team);
            PhotonNetwork.player.SetCustomProperties(propertiesToSet);
        }
    }

    public void setTeam2(int team)
    {
        if ((IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER) && base.photonView.isMine)
        {
            object[] parameters = new object[] { team };
            base.photonView.RPC("setMyTeam", PhotonTargets.AllBuffered, parameters);
            ExitGames.Client.Photon.Hashtable propertiesToSet = new ExitGames.Client.Photon.Hashtable();
            propertiesToSet.Add(PhotonPlayerProperty.team, team);
            PhotonNetwork.player.SetCustomProperties(propertiesToSet);
        }
        else
        {
            this.setMyTeam(team);
        }
    }

    public void shootFlare(int type)
    {
        bool flag = false;
        if ((type == 1) && (this.flare1CD == 0f))
        {
            this.flare1CD = this.flareTotalCD;
            flag = true;
        }
        if ((type == 2) && (this.flare2CD == 0f))
        {
            this.flare2CD = this.flareTotalCD;
            flag = true;
        }
        if ((type == 3) && (this.flare3CD == 0f))
        {
            this.flare3CD = this.flareTotalCD;
            flag = true;
        }
        if (flag)
        {
            if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
            {
                GameObject obj2 = (GameObject)UnityEngine.Object.Instantiate(Resources.Load("FX/flareBullet" + type), base.transform.position, base.transform.rotation);
                obj2.GetComponent<FlareMovement>().dontShowHint();
                UnityEngine.Object.Destroy(obj2, 25f);
            }
            else
            {
                PhotonNetwork.Instantiate("FX/flareBullet" + type, base.transform.position, base.transform.rotation, 0).GetComponent<FlareMovement>().dontShowHint();
            }
        }
    }

    private void showAimUI2()
    {
        Vector3 vector;
        if (Cursor.visible)
        {
            GameObject obj2 = this.cross1;
            GameObject obj3 = this.cross2;
            GameObject obj4 = this.crossL1;
            GameObject obj5 = this.crossL2;
            GameObject obj6 = this.crossR1;
            GameObject obj7 = this.crossR2;
            var labelDistance = this.LabelDistance;
            vector = (Vector3)(Vector3.up * 10000f);
            obj7.transform.localPosition = vector;
            obj6.transform.localPosition = vector;
            obj5.transform.localPosition = vector;
            obj4.transform.localPosition = vector;
            labelDistance.gameObject.transform.localPosition = vector;
            obj3.transform.localPosition = vector;
            obj2.transform.localPosition = vector;
        }
        else
        {
            RaycastHit hit;
            this.checkTitan();
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            LayerMask mask = ((int)1) << LayerMask.NameToLayer("Ground");
            LayerMask mask2 = ((int)1) << LayerMask.NameToLayer("EnemyBox");
            LayerMask mask3 = mask2 | mask;
            if (Physics.Raycast(ray, out hit, 1E+07f, mask3.value))
            {
                RaycastHit hit2;
                GameObject obj9 = this.cross1;
                GameObject obj10 = this.cross2;
                obj9.transform.localPosition = Input.mousePosition;
                Transform transform = obj9.transform;
                transform.localPosition -= new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 0f);
                obj10.transform.localPosition = obj9.transform.localPosition;
                vector = hit.point - this.baseTransform.position;
                float magnitude = vector.magnitude;
                string str = (magnitude <= 1000f) ? ((int)magnitude).ToString() : "???";
                if (((int)FengGameManagerMKII.settings[0xbd]) == 1)
                {
                    str = str + "\n" + this.currentSpeed.ToString("F1") + " u/s";
                }
                else if (((int)FengGameManagerMKII.settings[0xbd]) == 2)
                {
                    str = str + "\n" + ((this.currentSpeed / 100f)).ToString("F1") + "K";
                }
                LabelDistance.text = str;
                if (magnitude > 120f)
                {
                    Transform transform11 = obj9.transform;
                    transform11.localPosition += (Vector3)(Vector3.up * 10000f);
                    LabelDistance.gameObject.transform.localPosition = obj10.transform.localPosition;
                }
                else
                {
                    Transform transform12 = obj10.transform;
                    transform12.localPosition += (Vector3)(Vector3.up * 10000f);
                    LabelDistance.gameObject.transform.localPosition = obj9.transform.localPosition;
                }
                Transform transform13 = LabelDistance.gameObject.transform;
                transform13.localPosition -= new Vector3(0f, 15f, 0f);
                Vector3 vector2 = new Vector3(0f, 0.4f, 0f);
                vector2 -= (Vector3)(this.baseTransform.right * 0.3f);
                Vector3 vector3 = new Vector3(0f, 0.4f, 0f);
                vector3 += (Vector3)(this.baseTransform.right * 0.3f);
                float num4 = (hit.distance <= 50f) ? (hit.distance * 0.05f) : (hit.distance * 0.3f);
                Vector3 vector4 = (hit.point - ((Vector3)(this.baseTransform.right * num4))) - (this.baseTransform.position + vector2);
                Vector3 vector5 = (hit.point + ((Vector3)(this.baseTransform.right * num4))) - (this.baseTransform.position + vector3);
                vector4.Normalize();
                vector5.Normalize();
                vector4 = (Vector3)(vector4 * 1000000f);
                vector5 = (Vector3)(vector5 * 1000000f);
                if (Physics.Linecast(this.baseTransform.position + vector2, (this.baseTransform.position + vector2) + vector4, out hit2, mask3.value))
                {
                    GameObject obj12 = this.crossL1;
                    obj12.transform.localPosition = this.currentCamera.WorldToScreenPoint(hit2.point);
                    Transform transform14 = obj12.transform;
                    transform14.localPosition -= new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 0f);
                    obj12.transform.localRotation = Quaternion.Euler(0f, 0f, (Mathf.Atan2(obj12.transform.localPosition.y - (Input.mousePosition.y - (Screen.height * 0.5f)), obj12.transform.localPosition.x - (Input.mousePosition.x - (Screen.width * 0.5f))) * 57.29578f) + 180f);
                    GameObject obj13 = this.crossL2;
                    obj13.transform.localPosition = obj12.transform.localPosition;
                    obj13.transform.localRotation = obj12.transform.localRotation;
                    if (hit2.distance > 120f)
                    {
                        Transform transform15 = obj12.transform;
                        transform15.localPosition += (Vector3)(Vector3.up * 10000f);
                    }
                    else
                    {
                        Transform transform16 = obj13.transform;
                        transform16.localPosition += (Vector3)(Vector3.up * 10000f);
                    }
                }
                if (Physics.Linecast(this.baseTransform.position + vector3, (this.baseTransform.position + vector3) + vector5, out hit2, mask3.value))
                {
                    GameObject obj14 = this.crossR1;
                    obj14.transform.localPosition = this.currentCamera.WorldToScreenPoint(hit2.point);
                    Transform transform17 = obj14.transform;
                    transform17.localPosition -= new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 0f);
                    obj14.transform.localRotation = Quaternion.Euler(0f, 0f, Mathf.Atan2(obj14.transform.localPosition.y - (Input.mousePosition.y - (Screen.height * 0.5f)), obj14.transform.localPosition.x - (Input.mousePosition.x - (Screen.width * 0.5f))) * 57.29578f);
                    GameObject obj15 = this.crossR2;
                    obj15.transform.localPosition = obj14.transform.localPosition;
                    obj15.transform.localRotation = obj14.transform.localRotation;
                    if (hit2.distance > 120f)
                    {
                        Transform transform18 = obj14.transform;
                        transform18.localPosition += (Vector3)(Vector3.up * 10000f);
                    }
                    else
                    {
                        Transform transform19 = obj15.transform;
                        transform19.localPosition += (Vector3)(Vector3.up * 10000f);
                    }
                }
            }
        }
    }

    private void showFlareCD()
    {
        if (GameObject.Find("UIflare1") != null)
        {
            //GameObject.Find("UIflare1").GetComponent<UISprite>().fillAmount = (this.flareTotalCD - this.flare1CD) / this.flareTotalCD;
            //GameObject.Find("UIflare2").GetComponent<UISprite>().fillAmount = (this.flareTotalCD - this.flare2CD) / this.flareTotalCD;
            //GameObject.Find("UIflare3").GetComponent<UISprite>().fillAmount = (this.flareTotalCD - this.flare3CD) / this.flareTotalCD;
        }
    }

    private void showFlareCD2()
    {
        if (this.cachedSprites["UIflare1"] != null)
        {
            this.cachedSprites["UIflare1"].fillAmount = (this.flareTotalCD - this.flare1CD) / this.flareTotalCD;
            this.cachedSprites["UIflare2"].fillAmount = (this.flareTotalCD - this.flare2CD) / this.flareTotalCD;
            this.cachedSprites["UIflare3"].fillAmount = (this.flareTotalCD - this.flare3CD) / this.flareTotalCD;
        }
    }

    private void showGas2()
    {
        float num = this.currentGas / this.totalGas;
        float num2 = this.currentBladeSta / this.totalBladeSta;
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
        //if (!this.useGun)
        //{
        //    this.cachedSprites["bladeCL"].fillAmount = this.currentBladeSta / this.totalBladeSta;
        //    this.cachedSprites["bladeCR"].fillAmount = this.currentBladeSta / this.totalBladeSta;
        //    if (num <= 0f)
        //    {
        //        this.cachedSprites["gasL"].color = Color.red;
        //        this.cachedSprites["gasR"].color = Color.red;
        //    }
        //    else if (num < 0.3f)
        //    {
        //        this.cachedSprites["gasL"].color = Color.yellow;
        //        this.cachedSprites["gasR"].color = Color.yellow;
        //    }
        //    else
        //    {
        //        this.cachedSprites["gasL"].color = Color.white;
        //        this.cachedSprites["gasR"].color = Color.white;
        //    }
        //    if (num2 <= 0f)
        //    {
        //        this.cachedSprites["bladel1"].color = Color.red;
        //        this.cachedSprites["blader1"].color = Color.red;
        //    }
        //    else if (num2 < 0.3f)
        //    {
        //        this.cachedSprites["bladel1"].color = Color.yellow;
        //        this.cachedSprites["blader1"].color = Color.yellow;
        //    }
        //    else
        //    {
        //        this.cachedSprites["bladel1"].color = Color.white;
        //        this.cachedSprites["blader1"].color = Color.white;
        //    }
        //    if (this.currentBladeNum <= 4)
        //    {
        //        this.cachedSprites["bladel5"].enabled = false;
        //        this.cachedSprites["blader5"].enabled = false;
        //    }
        //    else
        //    {
        //        this.cachedSprites["bladel5"].enabled = true;
        //        this.cachedSprites["blader5"].enabled = true;
        //    }
        //    if (this.currentBladeNum <= 3)
        //    {
        //        this.cachedSprites["bladel4"].enabled = false;
        //        this.cachedSprites["blader4"].enabled = false;
        //    }
        //    else
        //    {
        //        this.cachedSprites["bladel4"].enabled = true;
        //        this.cachedSprites["blader4"].enabled = true;
        //    }
        //    if (this.currentBladeNum <= 2)
        //    {
        //        this.cachedSprites["bladel3"].enabled = false;
        //        this.cachedSprites["blader3"].enabled = false;
        //    }
        //    else
        //    {
        //        this.cachedSprites["bladel3"].enabled = true;
        //        this.cachedSprites["blader3"].enabled = true;
        //    }
        //    if (this.currentBladeNum <= 1)
        //    {
        //        this.cachedSprites["bladel2"].enabled = false;
        //        this.cachedSprites["blader2"].enabled = false;
        //    }
        //    else
        //    {
        //        this.cachedSprites["bladel2"].enabled = true;
        //        this.cachedSprites["blader2"].enabled = true;
        //    }
        //    if (this.currentBladeNum <= 0)
        //    {
        //        this.cachedSprites["bladel1"].enabled = false;
        //        this.cachedSprites["blader1"].enabled = false;
        //    }
        //    else
        //    {
        //        this.cachedSprites["bladel1"].enabled = true;
        //        this.cachedSprites["blader1"].enabled = true;
        //    }
        //}
        //else
        //{
        //    if (this.leftGunHasBullet)
        //    {
        //        this.cachedSprites["bulletL"].enabled = true;
        //    }
        //    else
        //    {
        //        this.cachedSprites["bulletL"].enabled = false;
        //    }
        //    if (this.rightGunHasBullet)
        //    {
        //        this.cachedSprites["bulletR"].enabled = true;
        //    }
        //    else
        //    {
        //        this.cachedSprites["bulletR"].enabled = false;
        //    }
        //}
    }

    [PunRPC]
    private void showHitDamage()
    {
        GameObject target = GameObject.Find("LabelScore");
        if (target != null)
        {
            this.speed = Mathf.Max(10f, this.speed);
            //target.GetComponent<UILabel>().text = this.speed.ToString();
            target.transform.localScale = Vector3.zero;
            this.speed = (int)(this.speed * 0.1f);
            this.speed = Mathf.Clamp(this.speed, 40f, 150f);
            iTween.Stop(target);
            object[] args = new object[] { "x", this.speed, "y", this.speed, "z", this.speed, "easetype", iTween.EaseType.easeOutElastic, "time", 1f };
            iTween.ScaleTo(target, iTween.Hash(args));
            object[] objArray2 = new object[] { "x", 0, "y", 0, "z", 0, "easetype", iTween.EaseType.easeInBounce, "time", 0.5f, "delay", 2f };
            iTween.ScaleTo(target, iTween.Hash(objArray2));
        }
    }

    private void showSkillCD()
    {
        if (this.skillCD != null)
        {
            //this.skillCD.GetComponent<UISprite>().fillAmount = (this.skillCDLast - this.skillCDDuration) / this.skillCDLast;
        }
    }

    [PunRPC]
    public void SpawnCannonRPC(string settings, PhotonMessageInfo info)
    {
        if ((info.sender.isMasterClient && base.photonView.isMine) && (this.myCannon == null))
        {
            if ((this.myHorse != null) && this.isMounted)
            {
                this.getOffHorse();
            }
            this.idle();
            if (this.bulletLeft != null)
            {
                this.bulletLeft.GetComponent<Bullet>().removeMe();
            }
            if (this.bulletRight != null)
            {
                this.bulletRight.GetComponent<Bullet>().removeMe();
            }
            if ((this.smoke_3dmg.enableEmission && (IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.SINGLE)) && base.photonView.isMine)
            {
                object[] parameters = new object[] { false };
                base.photonView.RPC("net3DMGSMOKE", PhotonTargets.Others, parameters);
            }
            this.smoke_3dmg.enableEmission = false;
            base.GetComponent<Rigidbody>().velocity = Vector3.zero;
            string[] strArray = settings.Split(new char[] { ',' });
            if (strArray.Length > 15)
            {
                this.myCannon = PhotonNetwork.Instantiate("RCAsset/" + strArray[1], new Vector3(Convert.ToSingle(strArray[12]), Convert.ToSingle(strArray[13]), Convert.ToSingle(strArray[14])), new Quaternion(Convert.ToSingle(strArray[15]), Convert.ToSingle(strArray[0x10]), Convert.ToSingle(strArray[0x11]), Convert.ToSingle(strArray[0x12])), 0);
            }
            else
            {
                this.myCannon = PhotonNetwork.Instantiate("RCAsset/" + strArray[1], new Vector3(Convert.ToSingle(strArray[2]), Convert.ToSingle(strArray[3]), Convert.ToSingle(strArray[4])), new Quaternion(Convert.ToSingle(strArray[5]), Convert.ToSingle(strArray[6]), Convert.ToSingle(strArray[7]), Convert.ToSingle(strArray[8])), 0);
            }
            this.myCannonBase = this.myCannon.transform;
            this.myCannonPlayer = this.myCannon.transform.Find("PlayerPoint");
            this.isCannon = true;
            this.myCannon.GetComponent<Cannon>().myHero = this;
            this.myCannonRegion = null;
            Camera.main.GetComponent<IN_GAME_MAIN_CAMERA>().setMainObject(this.myCannon.transform.Find("Barrel").Find("FiringPoint").gameObject, true, false);
            Camera.main.fieldOfView = 55f;
            base.photonView.RPC("SetMyCannon", PhotonTargets.OthersBuffered, new object[] { this.myCannon.GetPhotonView().viewID });
            this.skillCDLastCannon = this.skillCDLast;
            this.skillCDLast = 3.5f;
            this.skillCDDuration = 3.5f;
        }
    }

    public void SetHorse()
    {
        if (IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.MULTIPLAYER || !photonView.isMine) return;
        if (FengGameManagerMKII.Gamemode.Horse && myHorse == null)
        {
            this.myHorse = PhotonNetwork.Instantiate("horse", this.baseTransform.position + ((Vector3)(Vector3.up * 5f)), this.baseTransform.rotation, 0);
            this.myHorse.GetComponent<Horse>().myHero = base.gameObject;
            this.myHorse.GetComponent<TITAN_CONTROLLER>().isHorse = true;
        }

        if (!FengGameManagerMKII.Gamemode.Horse && myHorse != null)
        {
            PhotonNetwork.Destroy(myHorse);
        }
    }

    private void Start()
    {
        FengGameManagerMKII.instance.addHero(this);
        gameObject.AddComponent<PlayerInteractable>();
        SetHorse();
        this.sparks = this.baseTransform.Find("slideSparks").GetComponent<ParticleSystem>();
        this.smoke_3dmg = this.baseTransform.Find("3dmg_smoke").GetComponent<ParticleSystem>();
        this.baseTransform.localScale = new Vector3(this.myScale, this.myScale, this.myScale);
        this.facingDirection = this.baseTransform.rotation.eulerAngles.y;
        this.targetRotation = Quaternion.Euler(0f, this.facingDirection, 0f);
        this.smoke_3dmg.enableEmission = false;
        this.sparks.enableEmission = false;
        //HACK
        //this.speedFXPS = this.speedFX1.GetComponent<ParticleSystem>();
        //this.speedFXPS.enableEmission = false;
        if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER)
        {
            if (PhotonNetwork.isMasterClient)
            {
                int iD = base.photonView.owner.ID;
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
            //this.myNetWorkName = (GameObject) UnityEngine.Object.Instantiate(Resources.Load("UI/LabelNameOverHead"));
            //this.myNetWorkName.name = "LabelNameOverHead";
            ////HACK
            ////this.myNetWorkName.transform.parent = obj2.GetComponent<UIReferArray>().panels[0].transform;
            //this.myNetWorkName.transform.localScale = new Vector3(14f, 14f, 14f);
            //this.myNetWorkName.GetComponent<UILabel>().text = string.Empty;
            if (base.photonView.isMine)
            {
                if (Minimap.instance != null)
                {
                    Minimap.instance.TrackGameObjectOnMinimap(base.gameObject, Color.green, false, true, Minimap.IconStyle.CIRCLE);
                }
                base.GetComponent<SmoothSyncMovement>().PhotonCamera = true;
                base.photonView.RPC("SetMyPhotonCamera", PhotonTargets.OthersBuffered, new object[] { PlayerPrefs.GetFloat("cameraDistance") + 0.3f });
            }
            else
            {
                bool flag2 = false;
                //HACK
                //if (base.photonView.owner.CustomProperties[PhotonPlayerProperty.RCteam] != null)
                //{
                //    switch (RCextensions.returnIntFromObject(base.photonView.owner.CustomProperties[PhotonPlayerProperty.RCteam]))
                //    {
                //        case 1:
                //            flag2 = true;
                //            if (Minimap.instance != null)
                //            {
                //                Minimap.instance.TrackGameObjectOnMinimap(base.gameObject, Color.cyan, false, true, Minimap.IconStyle.CIRCLE);
                //            }
                //            break;

                //        case 2:
                //            flag2 = true;
                //            if (Minimap.instance != null)
                //            {
                //                Minimap.instance.TrackGameObjectOnMinimap(base.gameObject, Color.magenta, false, true, Minimap.IconStyle.CIRCLE);
                //            }
                //            break;
                //    }
                //}
                //if (RCextensions.returnIntFromObject(base.photonView.owner.CustomProperties[PhotonPlayerProperty.team]) == 2)
                //{
                //    this.myNetWorkName.GetComponent<UILabel>().text = "[FF0000]AHSS\n[FFFFFF]";
                //    if (!flag2 && (Minimap.instance != null))
                //    {
                //        Minimap.instance.TrackGameObjectOnMinimap(base.gameObject, Color.red, false, true, Minimap.IconStyle.CIRCLE);
                //    }
                //}
                //else if (!flag2 && (Minimap.instance != null))
                //{
                //    Minimap.instance.TrackGameObjectOnMinimap(base.gameObject, Color.blue, false, true, Minimap.IconStyle.CIRCLE);
                //}
            }
            //string str = RCextensions.returnStringFromObject(base.photonView.owner.CustomProperties[PhotonPlayerProperty.guildName]);
            //if (str != string.Empty)
            //{
            //    UILabel component = this.myNetWorkName.GetComponent<UILabel>();
            //    string text = component.text;
            //    string[] strArray2 = new string[] { text, "[FFFF00]", str, "\n[FFFFFF]", RCextensions.returnStringFromObject(base.photonView.owner.CustomProperties[PhotonPlayerProperty.name]) };
            //    component.text = string.Concat(strArray2);
            //}
            //else
            //{
            //    UILabel label2 = this.myNetWorkName.GetComponent<UILabel>();
            //    label2.text = label2.text + RCextensions.returnStringFromObject(base.photonView.owner.CustomProperties[PhotonPlayerProperty.name]);
            //}
        }
        else if (Minimap.instance != null)
        {
            Minimap.instance.TrackGameObjectOnMinimap(base.gameObject, Color.green, false, true, Minimap.IconStyle.CIRCLE);
        }
        if ((IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.SINGLE) && !base.photonView.isMine)
        {
            base.gameObject.layer = LayerMask.NameToLayer("NetworkObject");
            if (IN_GAME_MAIN_CAMERA.dayLight == DayLight.Night)
            {
                GameObject obj3 = (GameObject)UnityEngine.Object.Instantiate(Resources.Load("flashlight"));
                obj3.transform.parent = this.baseTransform;
                obj3.transform.position = this.baseTransform.position + Vector3.up;
                obj3.transform.rotation = Quaternion.Euler(353f, 0f, 0f);
            }
            this.setup.init();
            this.setup.myCostume = new HeroCostume();
            this.setup.myCostume = CostumeConeveter.PhotonDataToHeroCostume2(base.photonView.owner);
            this.setup.setCharacterComponent();
            UnityEngine.Object.Destroy(this.checkBoxLeft);
            UnityEngine.Object.Destroy(this.checkBoxRight);
            UnityEngine.Object.Destroy(this.leftbladetrail);
            UnityEngine.Object.Destroy(this.rightbladetrail);
            UnityEngine.Object.Destroy(this.leftbladetrail2);
            UnityEngine.Object.Destroy(this.rightbladetrail2);
            this.hasspawn = true;
        }
        else
        {
            this.currentCamera = GameObject.Find("MainCamera").GetComponent<Camera>();
            this.inputManager = GameObject.Find("InputManagerController").GetComponent<FengCustomInputs>();
            this.loadskin();
            this.hasspawn = true;
            base.StartCoroutine(this.reloadSky());
        }
        this.bombImmune = false;
        if (FengGameManagerMKII.Gamemode.PvPBomb)
        {
            this.bombImmune = true;
            base.StartCoroutine(this.stopImmunity());
        }
    }

    public IEnumerator stopImmunity()
    {
        yield return new WaitForSeconds(5f);
        this.bombImmune = false;
    }

    private void suicide()
    {
    }

    private void suicide2()
    {
        if (IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.SINGLE)
        {
            this.netDieLocal((Vector3)(base.GetComponent<Rigidbody>().velocity * 50f), false, -1, string.Empty, true);
            FengGameManagerMKII.instance.needChooseSide = true;
            FengGameManagerMKII.instance.justSuicide = true;
        }
    }

    public void ungrabbed()
    {
        this.facingDirection = 0f;
        this.targetRotation = Quaternion.Euler(0f, 0f, 0f);
        base.transform.parent = null;
        base.GetComponent<CapsuleCollider>().isTrigger = false;
        this.state = HERO_STATE.Idle;
    }

    private void unmounted()
    {
        this.myHorse.GetComponent<Horse>().unmounted();
        this.isMounted = false;
    }

    public void update2()
    {
        if (!IN_GAME_MAIN_CAMERA.isPausing)
        {
            if (this.invincible > 0f)
            {
                this.invincible -= Time.deltaTime;
            }
            if (!this.hasDied)
            {
                if (this.titanForm && (this.eren_titan != null))
                {
                    this.baseTransform.position = this.eren_titan.transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/neck").position;
                    base.gameObject.GetComponent<SmoothSyncMovement>().disabled = true;
                }
                else if (this.isCannon && (this.myCannon != null))
                {
                    this.updateCannon();
                    base.gameObject.GetComponent<SmoothSyncMovement>().disabled = true;
                }
                if ((IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE) || base.photonView.isMine)
                {
                    if (this.myCannonRegion != null)
                    {
                        FengGameManagerMKII.instance.ShowHUDInfoCenter("Press 'Cannon Mount' key to use Cannon.");
                        if (FengGameManagerMKII.inputRC.isInputCannonDown(InputCodeRC.cannonMount))
                        {
                            this.myCannonRegion.photonView.RPC("RequestControlRPC", PhotonTargets.MasterClient, new object[] { base.photonView.viewID });
                        }
                    }
                    if ((this.state == HERO_STATE.Grab) && !this.useGun)
                    {
                        if (this.skillId == "jean")
                        {
                            if (((this.state != HERO_STATE.Attack) && (this.inputManager.isInputDown[InputCode.attack0] || this.inputManager.isInputDown[InputCode.attack1])) && ((this.escapeTimes > 0) && !this.baseAnimation.IsPlaying("grabbed_jean")))
                            {
                                this.playAnimation("grabbed_jean");
                                this.baseAnimation["grabbed_jean"].time = 0f;
                                this.escapeTimes--;
                            }
                            if ((this.baseAnimation.IsPlaying("grabbed_jean") && (this.baseAnimation["grabbed_jean"].normalizedTime > 0.64f)) && (this.titanWhoGrabMe.GetComponent<TITAN>() != null))
                            {
                                this.ungrabbed();
                                this.baseRigidBody.velocity = (Vector3)(Vector3.up * 30f);
                                if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
                                {
                                    this.titanWhoGrabMe.GetComponent<TITAN>().grabbedTargetEscape();
                                }
                                else
                                {
                                    base.photonView.RPC("netSetIsGrabbedFalse", PhotonTargets.All, new object[0]);
                                    if (PhotonNetwork.isMasterClient)
                                    {
                                        this.titanWhoGrabMe.GetComponent<TITAN>().grabbedTargetEscape();
                                    }
                                    else
                                    {
                                        PhotonView.Find(this.titanWhoGrabMeID).RPC("grabbedTargetEscape", PhotonTargets.MasterClient, new object[0]);
                                    }
                                }
                            }
                        }
                        else if (this.skillId == "eren")
                        {
                            this.showSkillCD();
                            if ((IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.SINGLE) || ((IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE) && !IN_GAME_MAIN_CAMERA.isPausing))
                            {
                                this.calcSkillCD();
                                this.calcFlareCD();
                            }
                            if (this.inputManager.isInputDown[InputCode.attack1])
                            {
                                bool flag2 = false;
                                if ((this.skillCDDuration > 0f) || flag2)
                                {
                                    flag2 = true;
                                }
                                else
                                {
                                    this.skillCDDuration = this.skillCDLast;
                                    if ((this.skillId == "eren") && (this.titanWhoGrabMe.GetComponent<TITAN>() != null))
                                    {
                                        this.ungrabbed();
                                        if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
                                        {
                                            this.titanWhoGrabMe.GetComponent<TITAN>().grabbedTargetEscape();
                                        }
                                        else
                                        {
                                            base.photonView.RPC("netSetIsGrabbedFalse", PhotonTargets.All, new object[0]);
                                            if (PhotonNetwork.isMasterClient)
                                            {
                                                this.titanWhoGrabMe.GetComponent<TITAN>().grabbedTargetEscape();
                                            }
                                            else
                                            {
                                                PhotonView.Find(this.titanWhoGrabMeID).photonView.RPC("grabbedTargetEscape", PhotonTargets.MasterClient, new object[0]);
                                            }
                                        }
                                        this.erenTransform();
                                    }
                                }
                            }
                        }
                    }
                    else if (!this.titanForm && !this.isCannon)
                    {
                        System.Boolean ReflectorVariable2;
                        System.Boolean ReflectorVariable1;
                        System.Boolean ReflectorVariable0;
                        this.bufferUpdate();
                        this.updateExt();
                        if (!this.grounded && (this.state != HERO_STATE.AirDodge))
                        {
                            if (((int)FengGameManagerMKII.settings[0xb5]) == 1)
                            {
                                this.checkDashRebind();
                            }
                            else
                            {
                                this.checkDashDoubleTap();
                            }
                            if (this.dashD)
                            {
                                this.dashD = false;
                                this.dash(0f, -1f);
                                return;
                            }
                            if (this.dashU)
                            {
                                this.dashU = false;
                                this.dash(0f, 1f);
                                return;
                            }
                            if (this.dashL)
                            {
                                this.dashL = false;
                                this.dash(-1f, 0f);
                                return;
                            }
                            if (this.dashR)
                            {
                                this.dashR = false;
                                this.dash(1f, 0f);
                                return;
                            }
                        }
                        if (this.grounded && ((this.state == HERO_STATE.Idle) || (this.state == HERO_STATE.Slide)))
                        {
                            if (!((!this.inputManager.isInputDown[InputCode.jump] || this.baseAnimation.IsPlaying("jump")) || this.baseAnimation.IsPlaying("horse_geton")))
                            {
                                this.idle();
                                this.crossFade("jump", 0.1f);
                                this.sparks.enableEmission = false;
                            }
                            if (!((!FengGameManagerMKII.inputRC.isInputHorseDown(InputCodeRC.horseMount) || this.baseAnimation.IsPlaying("jump")) || this.baseAnimation.IsPlaying("horse_geton")) && (((this.myHorse != null) && !this.isMounted) && (Vector3.Distance(this.myHorse.transform.position, base.transform.position) < 15f)))
                            {
                                this.getOnHorse();
                            }
                            if (!((!this.inputManager.isInputDown[InputCode.dodge] || this.baseAnimation.IsPlaying("jump")) || this.baseAnimation.IsPlaying("horse_geton")))
                            {
                                this.dodge2(false);
                                return;
                            }
                        }
                        if (this.state == HERO_STATE.Idle)
                        {
                            if (this.inputManager.isInputDown[InputCode.flare1])
                            {
                                this.shootFlare(1);
                            }
                            if (this.inputManager.isInputDown[InputCode.flare2])
                            {
                                this.shootFlare(2);
                            }
                            if (this.inputManager.isInputDown[InputCode.flare3])
                            {
                                this.shootFlare(3);
                            }
                            if (this.inputManager.isInputDown[InputCode.restart])
                            {
                                this.suicide2();
                            }
                            if (((this.myHorse != null) && this.isMounted) && FengGameManagerMKII.inputRC.isInputHorseDown(InputCodeRC.horseMount))
                            {
                                this.getOffHorse();
                            }
                            if (((base.GetComponent<Animation>().IsPlaying(this.standAnimation) || !this.grounded) && this.inputManager.isInputDown[InputCode.reload]) && ((!this.useGun || (FengGameManagerMKII.Gamemode.AhssAirReload)) || this.grounded))
                            {
                                this.changeBlade();
                                return;
                            }
                            if (this.baseAnimation.IsPlaying(this.standAnimation) && this.inputManager.isInputDown[InputCode.salute])
                            {
                                this.salute();
                                return;
                            }
                            if ((!this.isMounted && (this.inputManager.isInputDown[InputCode.attack0] || this.inputManager.isInputDown[InputCode.attack1])) && !this.useGun)
                            {
                                bool flag3 = false;
                                if (this.inputManager.isInputDown[InputCode.attack1])
                                {
                                    if ((this.skillCDDuration > 0f) || flag3)
                                    {
                                        flag3 = true;
                                    }
                                    else
                                    {
                                        this.skillCDDuration = this.skillCDLast;
                                        if (this.skillId == "eren")
                                        {
                                            this.erenTransform();
                                            return;
                                        }
                                        if (this.skillId == "marco")
                                        {
                                            if (this.IsGrounded())
                                            {
                                                this.attackAnimation = (UnityEngine.Random.Range(0, 2) != 0) ? "special_marco_1" : "special_marco_0";
                                                this.playAnimation(this.attackAnimation);
                                            }
                                            else
                                            {
                                                flag3 = true;
                                                this.skillCDDuration = 0f;
                                            }
                                        }
                                        else if (this.skillId == "armin")
                                        {
                                            if (this.IsGrounded())
                                            {
                                                this.attackAnimation = "special_armin";
                                                this.playAnimation("special_armin");
                                            }
                                            else
                                            {
                                                flag3 = true;
                                                this.skillCDDuration = 0f;
                                            }
                                        }
                                        else if (this.skillId == "sasha")
                                        {
                                            if (this.IsGrounded())
                                            {
                                                this.attackAnimation = "special_sasha";
                                                this.playAnimation("special_sasha");
                                                this.currentBuff = BUFF.SpeedUp;
                                                this.buffTime = 10f;
                                            }
                                            else
                                            {
                                                flag3 = true;
                                                this.skillCDDuration = 0f;
                                            }
                                        }
                                        else if (this.skillId == "mikasa")
                                        {
                                            this.attackAnimation = "attack3_1";
                                            this.playAnimation("attack3_1");
                                            this.baseRigidBody.velocity = (Vector3)(Vector3.up * 10f);
                                        }
                                        else if (this.skillId == "levi")
                                        {
                                            RaycastHit hit;
                                            this.attackAnimation = "attack5";
                                            this.playAnimation("attack5");
                                            this.baseRigidBody.velocity += (Vector3)(Vector3.up * 5f);
                                            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                                            LayerMask mask = ((int)1) << LayerMask.NameToLayer("Ground");
                                            LayerMask mask2 = ((int)1) << LayerMask.NameToLayer("EnemyBox");
                                            LayerMask mask3 = mask2 | mask;
                                            if (Physics.Raycast(ray, out hit, 1E+07f, mask3.value))
                                            {
                                                if (this.bulletRight != null)
                                                {
                                                    this.bulletRight.GetComponent<Bullet>().disable();
                                                    this.releaseIfIHookSb();
                                                }
                                                this.dashDirection = hit.point - this.baseTransform.position;
                                                this.launchRightRope(hit, true, 1);
                                                this.rope.Play();
                                            }
                                            this.facingDirection = Mathf.Atan2(this.dashDirection.x, this.dashDirection.z) * 57.29578f;
                                            this.targetRotation = Quaternion.Euler(0f, this.facingDirection, 0f);
                                            this.attackLoop = 3;
                                        }
                                        else if (this.skillId == "petra")
                                        {
                                            RaycastHit hit2;
                                            this.attackAnimation = "special_petra";
                                            this.playAnimation("special_petra");
                                            this.baseRigidBody.velocity += (Vector3)(Vector3.up * 5f);
                                            Ray ray2 = Camera.main.ScreenPointToRay(Input.mousePosition);
                                            LayerMask mask4 = ((int)1) << LayerMask.NameToLayer("Ground");
                                            LayerMask mask5 = ((int)1) << LayerMask.NameToLayer("EnemyBox");
                                            LayerMask mask6 = mask5 | mask4;
                                            if (Physics.Raycast(ray2, out hit2, 1E+07f, mask6.value))
                                            {
                                                if (this.bulletRight != null)
                                                {
                                                    this.bulletRight.GetComponent<Bullet>().disable();
                                                    this.releaseIfIHookSb();
                                                }
                                                if (this.bulletLeft != null)
                                                {
                                                    this.bulletLeft.GetComponent<Bullet>().disable();
                                                    this.releaseIfIHookSb();
                                                }
                                                this.dashDirection = hit2.point - this.baseTransform.position;
                                                this.launchLeftRope(hit2, true, 0);
                                                this.launchRightRope(hit2, true, 0);
                                                this.rope.Play();
                                            }
                                            this.facingDirection = Mathf.Atan2(this.dashDirection.x, this.dashDirection.z) * 57.29578f;
                                            this.targetRotation = Quaternion.Euler(0f, this.facingDirection, 0f);
                                            this.attackLoop = 3;
                                        }
                                        else
                                        {
                                            if (this.needLean)
                                            {
                                                if (this.leanLeft)
                                                {
                                                    this.attackAnimation = (UnityEngine.Random.Range(0, 100) >= 50) ? "attack1_hook_l1" : "attack1_hook_l2";
                                                }
                                                else
                                                {
                                                    this.attackAnimation = (UnityEngine.Random.Range(0, 100) >= 50) ? "attack1_hook_r1" : "attack1_hook_r2";
                                                }
                                            }
                                            else
                                            {
                                                this.attackAnimation = "attack1";
                                            }
                                            this.playAnimation(this.attackAnimation);
                                        }
                                    }
                                }
                                else if (this.inputManager.isInputDown[InputCode.attack0])
                                {
                                    if (this.needLean)
                                    {
                                        if (this.inputManager.isInput[InputCode.left])
                                        {
                                            this.attackAnimation = (UnityEngine.Random.Range(0, 100) >= 50) ? "attack1_hook_l1" : "attack1_hook_l2";
                                        }
                                        else if (this.inputManager.isInput[InputCode.right])
                                        {
                                            this.attackAnimation = (UnityEngine.Random.Range(0, 100) >= 50) ? "attack1_hook_r1" : "attack1_hook_r2";
                                        }
                                        else if (this.leanLeft)
                                        {
                                            this.attackAnimation = (UnityEngine.Random.Range(0, 100) >= 50) ? "attack1_hook_l1" : "attack1_hook_l2";
                                        }
                                        else
                                        {
                                            this.attackAnimation = (UnityEngine.Random.Range(0, 100) >= 50) ? "attack1_hook_r1" : "attack1_hook_r2";
                                        }
                                    }
                                    else if (this.inputManager.isInput[InputCode.left])
                                    {
                                        this.attackAnimation = "attack2";
                                    }
                                    else if (this.inputManager.isInput[InputCode.right])
                                    {
                                        this.attackAnimation = "attack1";
                                    }
                                    else if (this.lastHook != null)
                                    {
                                        if (this.lastHook.Find("Amarture/Core/Controller_Body/hip/spine/chest/neck") != null)
                                        {
                                            this.attackAccordingToTarget(this.lastHook.Find("Amarture/Core/Controller_Body/hip/spine/chest/neck"));
                                        }
                                        else
                                        {
                                            flag3 = true;
                                        }
                                    }
                                    else if ((this.bulletLeft != null) && (this.bulletLeft.transform.parent != null))
                                    {
                                        Transform a = this.bulletLeft.transform.parent.transform.root.Find("Amarture/Core/Controller_Body/hip/spine/chest/neck");
                                        if (a != null)
                                        {
                                            this.attackAccordingToTarget(a);
                                        }
                                        else
                                        {
                                            this.attackAccordingToMouse();
                                        }
                                    }
                                    else if ((this.bulletRight != null) && (this.bulletRight.transform.parent != null))
                                    {
                                        Transform transform2 = this.bulletRight.transform.parent.transform.root.Find("Amarture/Core/Controller_Body/hip/spine/chest/neck");
                                        if (transform2 != null)
                                        {
                                            this.attackAccordingToTarget(transform2);
                                        }
                                        else
                                        {
                                            this.attackAccordingToMouse();
                                        }
                                    }
                                    else
                                    {
                                        GameObject obj2 = this.findNearestTitan();
                                        if (obj2 != null)
                                        {
                                            Transform transform3 = obj2.transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/neck");
                                            if (transform3 != null)
                                            {
                                                this.attackAccordingToTarget(transform3);
                                            }
                                            else
                                            {
                                                this.attackAccordingToMouse();
                                            }
                                        }
                                        else
                                        {
                                            this.attackAccordingToMouse();
                                        }
                                    }
                                }
                                if (!flag3)
                                {
                                    this.checkBoxLeft.GetComponent<TriggerColliderWeapon>().clearHits();
                                    this.checkBoxRight.GetComponent<TriggerColliderWeapon>().clearHits();
                                    if (this.grounded)
                                    {
                                        this.baseRigidBody.AddForce((Vector3)(base.gameObject.transform.forward * 200f));
                                    }
                                    this.playAnimation(this.attackAnimation);
                                    this.baseAnimation[this.attackAnimation].time = 0f;
                                    this.buttonAttackRelease = false;
                                    this.state = HERO_STATE.Attack;
                                    if ((this.grounded || (this.attackAnimation == "attack3_1")) || ((this.attackAnimation == "attack5") || (this.attackAnimation == "special_petra")))
                                    {
                                        this.attackReleased = true;
                                        this.buttonAttackRelease = true;
                                    }
                                    else
                                    {
                                        this.attackReleased = false;
                                    }
                                    this.sparks.enableEmission = false;
                                }
                            }
                            if (this.useGun)
                            {
                                if (this.inputManager.isInput[InputCode.attack1])
                                {
                                    this.leftArmAim = true;
                                    this.rightArmAim = true;
                                }
                                else if (this.inputManager.isInput[InputCode.attack0])
                                {
                                    if (this.leftGunHasBullet)
                                    {
                                        this.leftArmAim = true;
                                        this.rightArmAim = false;
                                    }
                                    else
                                    {
                                        this.leftArmAim = false;
                                        if (this.rightGunHasBullet)
                                        {
                                            this.rightArmAim = true;
                                        }
                                        else
                                        {
                                            this.rightArmAim = false;
                                        }
                                    }
                                }
                                else
                                {
                                    this.leftArmAim = false;
                                    this.rightArmAim = false;
                                }
                                if (this.leftArmAim || this.rightArmAim)
                                {
                                    RaycastHit hit3;
                                    Ray ray3 = Camera.main.ScreenPointToRay(Input.mousePosition);
                                    LayerMask mask7 = ((int)1) << LayerMask.NameToLayer("Ground");
                                    LayerMask mask8 = ((int)1) << LayerMask.NameToLayer("EnemyBox");
                                    LayerMask mask9 = mask8 | mask7;
                                    if (Physics.Raycast(ray3, out hit3, 1E+07f, mask9.value))
                                    {
                                        this.gunTarget = hit3.point;
                                    }
                                }
                                bool flag4 = false;
                                bool flag5 = false;
                                bool flag6 = false;
                                if (this.inputManager.isInputUp[InputCode.attack1] && (this.skillId != "bomb"))
                                {
                                    if (this.leftGunHasBullet && this.rightGunHasBullet)
                                    {
                                        if (this.grounded)
                                        {
                                            this.attackAnimation = "AHSS_shoot_both";
                                        }
                                        else
                                        {
                                            this.attackAnimation = "AHSS_shoot_both_air";
                                        }
                                        flag4 = true;
                                    }
                                    else if (!(this.leftGunHasBullet || this.rightGunHasBullet))
                                    {
                                        flag5 = true;
                                    }
                                    else
                                    {
                                        flag6 = true;
                                    }
                                }
                                if (flag6 || this.inputManager.isInputUp[InputCode.attack0])
                                {
                                    if (this.grounded)
                                    {
                                        if (this.leftGunHasBullet && this.rightGunHasBullet)
                                        {
                                            if (this.isLeftHandHooked)
                                            {
                                                this.attackAnimation = "AHSS_shoot_r";
                                            }
                                            else
                                            {
                                                this.attackAnimation = "AHSS_shoot_l";
                                            }
                                        }
                                        else if (this.leftGunHasBullet)
                                        {
                                            this.attackAnimation = "AHSS_shoot_l";
                                        }
                                        else if (this.rightGunHasBullet)
                                        {
                                            this.attackAnimation = "AHSS_shoot_r";
                                        }
                                    }
                                    else if (this.leftGunHasBullet && this.rightGunHasBullet)
                                    {
                                        if (this.isLeftHandHooked)
                                        {
                                            this.attackAnimation = "AHSS_shoot_r_air";
                                        }
                                        else
                                        {
                                            this.attackAnimation = "AHSS_shoot_l_air";
                                        }
                                    }
                                    else if (this.leftGunHasBullet)
                                    {
                                        this.attackAnimation = "AHSS_shoot_l_air";
                                    }
                                    else if (this.rightGunHasBullet)
                                    {
                                        this.attackAnimation = "AHSS_shoot_r_air";
                                    }
                                    if (this.leftGunHasBullet || this.rightGunHasBullet)
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
                                    this.state = HERO_STATE.Attack;
                                    this.crossFade(this.attackAnimation, 0.05f);
                                    this.gunDummy.transform.position = this.baseTransform.position;
                                    this.gunDummy.transform.rotation = this.baseTransform.rotation;
                                    this.gunDummy.transform.LookAt(this.gunTarget);
                                    this.attackReleased = false;
                                    this.facingDirection = this.gunDummy.transform.rotation.eulerAngles.y;
                                    this.targetRotation = Quaternion.Euler(0f, this.facingDirection, 0f);
                                }
                                else if (flag5 && (this.grounded || (FengGameManagerMKII.Gamemode.AhssAirReload)))
                                {
                                    this.changeBlade();
                                }
                            }
                        }
                        else if (this.state == HERO_STATE.Attack)
                        {
                            if (!this.useGun)
                            {
                                if (!this.inputManager.isInput[InputCode.attack0])
                                {
                                    this.buttonAttackRelease = true;
                                }
                                if (!this.attackReleased)
                                {
                                    if (this.buttonAttackRelease)
                                    {
                                        this.continueAnimation();
                                        this.attackReleased = true;
                                    }
                                    else if (this.baseAnimation[this.attackAnimation].normalizedTime >= 0.32f)
                                    {
                                        this.pauseAnimation();
                                    }
                                }
                                if ((this.attackAnimation == "attack3_1") && (this.currentBladeSta > 0f))
                                {
                                    if (this.baseAnimation[this.attackAnimation].normalizedTime >= 0.8f)
                                    {
                                        if (!this.checkBoxLeft.GetComponent<TriggerColliderWeapon>().active_me)
                                        {
                                            this.checkBoxLeft.GetComponent<TriggerColliderWeapon>().active_me = true;
                                            if (((int)FengGameManagerMKII.settings[0x5c]) == 0)
                                            {
                                                this.leftbladetrail2.Activate();
                                                this.rightbladetrail2.Activate();
                                                this.leftbladetrail.Activate();
                                                this.rightbladetrail.Activate();
                                            }
                                            this.baseRigidBody.velocity = (Vector3)(-Vector3.up * 30f);
                                        }
                                        if (!this.checkBoxRight.GetComponent<TriggerColliderWeapon>().active_me)
                                        {
                                            this.checkBoxRight.GetComponent<TriggerColliderWeapon>().active_me = true;
                                            this.slash.Play();
                                        }
                                    }
                                    else if (this.checkBoxLeft.GetComponent<TriggerColliderWeapon>().active_me)
                                    {
                                        this.checkBoxLeft.GetComponent<TriggerColliderWeapon>().active_me = false;
                                        this.checkBoxRight.GetComponent<TriggerColliderWeapon>().active_me = false;
                                        this.checkBoxLeft.GetComponent<TriggerColliderWeapon>().clearHits();
                                        this.checkBoxRight.GetComponent<TriggerColliderWeapon>().clearHits();
                                        this.leftbladetrail.StopSmoothly(0.1f);
                                        this.rightbladetrail.StopSmoothly(0.1f);
                                        this.leftbladetrail2.StopSmoothly(0.1f);
                                        this.rightbladetrail2.StopSmoothly(0.1f);
                                    }
                                }
                                else
                                {
                                    float num;
                                    float num2;
                                    if (this.currentBladeSta == 0f)
                                    {
                                        num = -1f;
                                        num2 = -1f;
                                    }
                                    else if (this.attackAnimation == "attack5")
                                    {
                                        num2 = 0.35f;
                                        num = 0.5f;
                                    }
                                    else if (this.attackAnimation == "special_petra")
                                    {
                                        num2 = 0.35f;
                                        num = 0.48f;
                                    }
                                    else if (this.attackAnimation == "special_armin")
                                    {
                                        num2 = 0.25f;
                                        num = 0.35f;
                                    }
                                    else if (this.attackAnimation == "attack4")
                                    {
                                        num2 = 0.6f;
                                        num = 0.9f;
                                    }
                                    else if (this.attackAnimation == "special_sasha")
                                    {
                                        num = -1f;
                                        num2 = -1f;
                                    }
                                    else
                                    {
                                        num2 = 0.5f;
                                        num = 0.85f;
                                    }
                                    if ((this.baseAnimation[this.attackAnimation].normalizedTime > num2) && (this.baseAnimation[this.attackAnimation].normalizedTime < num))
                                    {
                                        if (!this.checkBoxLeft.GetComponent<TriggerColliderWeapon>().active_me)
                                        {
                                            this.checkBoxLeft.GetComponent<TriggerColliderWeapon>().active_me = true;
                                            this.slash.Play();
                                            if (((int)FengGameManagerMKII.settings[0x5c]) == 0)
                                            {
                                                //this.leftbladetrail2.Activate();
                                                //this.rightbladetrail2.Activate();
                                                //this.leftbladetrail.Activate();
                                                //this.rightbladetrail.Activate();
                                            }
                                        }
                                        if (!this.checkBoxRight.GetComponent<TriggerColliderWeapon>().active_me)
                                        {
                                            this.checkBoxRight.GetComponent<TriggerColliderWeapon>().active_me = true;
                                        }
                                    }
                                    else if (this.checkBoxLeft.GetComponent<TriggerColliderWeapon>().active_me)
                                    {
                                        this.checkBoxLeft.GetComponent<TriggerColliderWeapon>().active_me = false;
                                        this.checkBoxRight.GetComponent<TriggerColliderWeapon>().active_me = false;
                                        this.checkBoxLeft.GetComponent<TriggerColliderWeapon>().clearHits();
                                        this.checkBoxRight.GetComponent<TriggerColliderWeapon>().clearHits();
                                        //this.leftbladetrail2.StopSmoothly(0.1f);
                                        //this.rightbladetrail2.StopSmoothly(0.1f);
                                        //this.leftbladetrail.StopSmoothly(0.1f);
                                        //this.rightbladetrail.StopSmoothly(0.1f);
                                    }
                                    if ((this.attackLoop > 0) && (this.baseAnimation[this.attackAnimation].normalizedTime > num))
                                    {
                                        this.attackLoop--;
                                        this.playAnimationAt(this.attackAnimation, num2);
                                    }
                                }
                                if (this.baseAnimation[this.attackAnimation].normalizedTime >= 1f)
                                {
                                    if ((this.attackAnimation == "special_marco_0") || (this.attackAnimation == "special_marco_1"))
                                    {
                                        if (IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.SINGLE)
                                        {
                                            if (!PhotonNetwork.isMasterClient)
                                            {
                                                object[] parameters = new object[] { 5f, 100f };
                                                base.photonView.RPC("netTauntAttack", PhotonTargets.MasterClient, parameters);
                                            }
                                            else
                                            {
                                                this.netTauntAttack(5f, 100f);
                                            }
                                        }
                                        else
                                        {
                                            this.netTauntAttack(5f, 100f);
                                        }
                                        this.falseAttack();
                                        this.idle();
                                    }
                                    else if (this.attackAnimation == "special_armin")
                                    {
                                        if (IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.SINGLE)
                                        {
                                            if (!PhotonNetwork.isMasterClient)
                                            {
                                                base.photonView.RPC("netlaughAttack", PhotonTargets.MasterClient, new object[0]);
                                            }
                                            else
                                            {
                                                this.netlaughAttack();
                                            }
                                        }
                                        else
                                        {
                                            foreach (GameObject obj3 in GameObject.FindGameObjectsWithTag("titan"))
                                            {
                                                if (((Vector3.Distance(obj3.transform.position, this.baseTransform.position) < 50f) && (Vector3.Angle(obj3.transform.forward, this.baseTransform.position - obj3.transform.position) < 90f)) && (obj3.GetComponent<TITAN>() != null))
                                                {
                                                    obj3.GetComponent<TITAN>().beLaughAttacked();
                                                }
                                            }
                                        }
                                        this.falseAttack();
                                        this.idle();
                                    }
                                    else if (this.attackAnimation == "attack3_1")
                                    {
                                        this.baseRigidBody.velocity -= (Vector3)((Vector3.up * Time.deltaTime) * 30f);
                                    }
                                    else
                                    {
                                        this.falseAttack();
                                        this.idle();
                                    }
                                }
                                if (this.baseAnimation.IsPlaying("attack3_2") && (this.baseAnimation["attack3_2"].normalizedTime >= 1f))
                                {
                                    this.falseAttack();
                                    this.idle();
                                }
                            }
                            else
                            {
                                this.checkBoxLeft.GetComponent<TriggerColliderWeapon>().active_me = false;
                                this.checkBoxRight.GetComponent<TriggerColliderWeapon>().active_me = false;
                                this.baseTransform.rotation = Quaternion.Lerp(this.baseTransform.rotation, this.gunDummy.transform.rotation, Time.deltaTime * 30f);
                                if (!this.attackReleased && (this.baseAnimation[this.attackAnimation].normalizedTime > 0.167f))
                                {
                                    GameObject obj4;
                                    this.attackReleased = true;
                                    bool flag7 = false;
                                    if ((this.attackAnimation == "AHSS_shoot_both") || (this.attackAnimation == "AHSS_shoot_both_air"))
                                    {
                                        //Should use AHSSShotgunCollider instead of TriggerColliderWeapon.  
                                        //Apply that change when abstracting weapons from this class.
                                        //Note, when doing the abstraction, the relationship between the weapon collider and the abstracted weapon class should be carefully considered.
                                        this.checkBoxLeft.GetComponent<TriggerColliderWeapon>().active_me = true;
                                        this.checkBoxRight.GetComponent<TriggerColliderWeapon>().active_me = true;
                                        flag7 = true;
                                        this.leftGunHasBullet = false;
                                        this.rightGunHasBullet = false;
                                        this.baseRigidBody.AddForce((Vector3)(-this.baseTransform.forward * 1000f), ForceMode.Acceleration);
                                    }
                                    else
                                    {
                                        if ((this.attackAnimation == "AHSS_shoot_l") || (this.attackAnimation == "AHSS_shoot_l_air"))
                                        {
                                            this.checkBoxLeft.GetComponent<TriggerColliderWeapon>().active_me = true;
                                            this.leftGunHasBullet = false;
                                        }
                                        else
                                        {
                                            this.checkBoxRight.GetComponent<TriggerColliderWeapon>().active_me = true;
                                            this.rightGunHasBullet = false;
                                        }
                                        this.baseRigidBody.AddForce((Vector3)(-this.baseTransform.forward * 600f), ForceMode.Acceleration);
                                    }
                                    this.baseRigidBody.AddForce((Vector3)(Vector3.up * 200f), ForceMode.Acceleration);
                                    string prefabName = "FX/shotGun";
                                    if (flag7)
                                    {
                                        prefabName = "FX/shotGun 1";
                                    }
                                    if ((IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER) && base.photonView.isMine)
                                    {
                                        obj4 = PhotonNetwork.Instantiate(prefabName, (Vector3)((this.baseTransform.position + (this.baseTransform.up * 0.8f)) - (this.baseTransform.right * 0.1f)), this.baseTransform.rotation, 0);
                                        if (obj4.GetComponent<EnemyfxIDcontainer>() != null)
                                        {
                                            obj4.GetComponent<EnemyfxIDcontainer>().myOwnerViewID = base.photonView.viewID;
                                        }
                                    }
                                    else
                                    {
                                        obj4 = (GameObject)UnityEngine.Object.Instantiate(Resources.Load(prefabName), (Vector3)((this.baseTransform.position + (this.baseTransform.up * 0.8f)) - (this.baseTransform.right * 0.1f)), this.baseTransform.rotation);
                                    }
                                }
                                if (this.baseAnimation[this.attackAnimation].normalizedTime >= 1f)
                                {
                                    this.falseAttack();
                                    this.idle();
                                    this.checkBoxLeft.GetComponent<TriggerColliderWeapon>().active_me = false;
                                    this.checkBoxRight.GetComponent<TriggerColliderWeapon>().active_me = false;
                                }
                                if (!this.baseAnimation.IsPlaying(this.attackAnimation))
                                {
                                    this.falseAttack();
                                    this.idle();
                                    this.checkBoxLeft.GetComponent<TriggerColliderWeapon>().active_me = false;
                                    this.checkBoxRight.GetComponent<TriggerColliderWeapon>().active_me = false;
                                }
                            }
                        }
                        else if (this.state == HERO_STATE.ChangeBlade)
                        {
                            Equipment.Weapon.Reload();
                            if (this.baseAnimation[this.reloadAnimation].normalizedTime >= 1f)
                            {
                                this.idle();
                            }
                        }
                        else if (this.state == HERO_STATE.Salute)
                        {
                            if (this.baseAnimation["salute"].normalizedTime >= 1f)
                            {
                                this.idle();
                            }
                        }
                        else if (this.state == HERO_STATE.GroundDodge)
                        {
                            if (this.baseAnimation.IsPlaying("dodge"))
                            {
                                if (!(this.grounded || (this.baseAnimation["dodge"].normalizedTime <= 0.6f)))
                                {
                                    this.idle();
                                }
                                if (this.baseAnimation["dodge"].normalizedTime >= 1f)
                                {
                                    this.idle();
                                }
                            }
                        }
                        else if (this.state == HERO_STATE.Land)
                        {
                            if (this.baseAnimation.IsPlaying("dash_land") && (this.baseAnimation["dash_land"].normalizedTime >= 1f))
                            {
                                this.idle();
                            }
                        }
                        else if (this.state == HERO_STATE.FillGas)
                        {
                            if (this.baseAnimation.IsPlaying("supply") && (this.baseAnimation["supply"].normalizedTime >= 1f))
                            {
                                this.currentBladeSta = this.totalBladeSta;
                                this.currentBladeNum = this.totalBladeNum;
                                Equipment.Weapon.AmountLeft = Equipment.Weapon.AmountRight = totalBladeNum;
                                this.currentGas = this.totalGas;
                                if (!this.useGun)
                                {
                                    this.setup.part_blade_l.SetActive(true);
                                    this.setup.part_blade_r.SetActive(true);
                                }
                                else
                                {
                                    this.leftBulletLeft = this.rightBulletLeft = this.bulletMAX;
                                    this.rightGunHasBullet = true;
                                    this.leftGunHasBullet = true;
                                    this.setup.part_blade_l.SetActive(true);
                                    this.setup.part_blade_r.SetActive(true);
                                    this.updateRightMagUI();
                                    this.updateLeftMagUI();
                                }
                                this.idle();
                            }
                        }
                        else if (this.state == HERO_STATE.Slide)
                        {
                            if (!this.grounded)
                            {
                                this.idle();
                            }
                        }
                        else if (this.state == HERO_STATE.AirDodge)
                        {
                            if (this.dashTime > 0f)
                            {
                                this.dashTime -= Time.deltaTime;
                                if (this.currentSpeed > this.originVM)
                                {
                                    this.baseRigidBody.AddForce((Vector3)((-this.baseRigidBody.velocity * Time.deltaTime) * 1.7f), ForceMode.VelocityChange);
                                }
                            }
                            else
                            {
                                this.dashTime = 0f;
                                this.idle();
                            }
                        }
                        if (this.inputManager.isInput[InputCode.leftRope])
                        {
                            ReflectorVariable0 = true;
                        }
                        else
                        {
                            ReflectorVariable0 = false;
                        }
                        if (!(ReflectorVariable0 ? (((this.baseAnimation.IsPlaying("attack3_1") || this.baseAnimation.IsPlaying("attack5")) || (this.baseAnimation.IsPlaying("special_petra") || (this.state == HERO_STATE.Grab))) ? (this.state != HERO_STATE.Idle) : false) : true))
                        {
                            if (this.bulletLeft != null)
                            {
                                this.QHold = true;
                            }
                            else
                            {
                                RaycastHit hit4;
                                Ray ray4 = Camera.main.ScreenPointToRay(Input.mousePosition);
                                LayerMask mask10 = ((int)1) << LayerMask.NameToLayer("Ground");
                                LayerMask mask11 = ((int)1) << LayerMask.NameToLayer("EnemyBox");
                                LayerMask mask12 = mask11 | mask10;
                                if (Physics.Raycast(ray4, out hit4, 10000f, mask12.value))
                                {
                                    this.launchLeftRope(hit4, true, 0);
                                    this.rope.Play();
                                }
                            }
                        }
                        else
                        {
                            this.QHold = false;
                        }
                        if (this.inputManager.isInput[InputCode.rightRope])
                        {
                            ReflectorVariable1 = true;
                        }
                        else
                        {
                            ReflectorVariable1 = false;
                        }
                        if (!(ReflectorVariable1 ? (((this.baseAnimation.IsPlaying("attack3_1") || this.baseAnimation.IsPlaying("attack5")) || (this.baseAnimation.IsPlaying("special_petra") || (this.state == HERO_STATE.Grab))) ? (this.state != HERO_STATE.Idle) : false) : true))
                        {
                            if (this.bulletRight != null)
                            {
                                this.EHold = true;
                            }
                            else
                            {
                                RaycastHit hit5;
                                Ray ray5 = Camera.main.ScreenPointToRay(Input.mousePosition);
                                LayerMask mask13 = ((int)1) << LayerMask.NameToLayer("Ground");
                                LayerMask mask14 = ((int)1) << LayerMask.NameToLayer("EnemyBox");
                                LayerMask mask15 = mask14 | mask13;
                                if (Physics.Raycast(ray5, out hit5, 10000f, mask15.value))
                                {
                                    this.launchRightRope(hit5, true, 0);
                                    this.rope.Play();
                                }
                            }
                        }
                        else
                        {
                            this.EHold = false;
                        }
                        if (this.inputManager.isInput[InputCode.bothRope])
                        {
                            ReflectorVariable2 = true;
                        }
                        else
                        {
                            ReflectorVariable2 = false;
                        }
                        if (!(ReflectorVariable2 ? (((this.baseAnimation.IsPlaying("attack3_1") || this.baseAnimation.IsPlaying("attack5")) || (this.baseAnimation.IsPlaying("special_petra") || (this.state == HERO_STATE.Grab))) ? (this.state != HERO_STATE.Idle) : false) : true))
                        {
                            this.QHold = true;
                            this.EHold = true;
                            if ((this.bulletLeft == null) && (this.bulletRight == null))
                            {
                                RaycastHit hit6;
                                Ray ray6 = Camera.main.ScreenPointToRay(Input.mousePosition);
                                LayerMask mask16 = ((int)1) << LayerMask.NameToLayer("Ground");
                                LayerMask mask17 = ((int)1) << LayerMask.NameToLayer("EnemyBox");
                                LayerMask mask18 = mask17 | mask16;
                                if (Physics.Raycast(ray6, out hit6, 1000000f, mask18.value))
                                {
                                    this.launchLeftRope(hit6, false, 0);
                                    this.launchRightRope(hit6, false, 0);
                                    this.rope.Play();
                                }
                            }
                        }
                        if ((IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.SINGLE) || ((IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE) && !IN_GAME_MAIN_CAMERA.isPausing))
                        {
                            this.calcSkillCD();
                            this.calcFlareCD();
                        }
                        //if (!this.useGun)
                        //{
                        //    if (this.leftbladetrail.gameObject.GetActive())
                        //    {
                        //        this.leftbladetrail.update();
                        //        this.rightbladetrail.update();
                        //    }
                        //    if (this.leftbladetrail2.gameObject.GetActive())
                        //    {
                        //        this.leftbladetrail2.update();
                        //        this.rightbladetrail2.update();
                        //    }
                        //    if (this.leftbladetrail.gameObject.GetActive())
                        //    {
                        //        this.leftbladetrail.lateUpdate();
                        //        this.rightbladetrail.lateUpdate();
                        //    }
                        //    if (this.leftbladetrail2.gameObject.GetActive())
                        //    {
                        //        this.leftbladetrail2.lateUpdate();
                        //        this.rightbladetrail2.lateUpdate();
                        //    }
                        //}
                        if (!IN_GAME_MAIN_CAMERA.isPausing)
                        {
                            //this.showSkillCD();
                            //this.showFlareCD2();
                            this.showGas2();
                            this.showAimUI2();
                        }
                    }
                    else if (this.isCannon && !IN_GAME_MAIN_CAMERA.isPausing)
                    {
                        //this.showAimUI2();
                        //this.calcSkillCD();
                        //this.showSkillCD();
                    }
                }
            }
        }
    }

    public void updateCannon()
    {
        this.baseTransform.position = this.myCannonPlayer.position;
        this.baseTransform.rotation = this.myCannonBase.rotation;
    }

    public void updateExt()
    {
        if (this.skillId == "bomb")
        {
            if (this.inputManager.isInputDown[InputCode.attack1] && (this.skillCDDuration <= 0f))
            {
                if (!((this.myBomb == null) || this.myBomb.disabled))
                {
                    this.myBomb.Explode(this.bombRadius);
                }
                this.detonate = false;
                this.skillCDDuration = this.bombCD;
                RaycastHit hitInfo = new RaycastHit();
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                LayerMask mask = ((int)1) << LayerMask.NameToLayer("Ground");
                LayerMask mask2 = ((int)1) << LayerMask.NameToLayer("EnemyBox");
                LayerMask mask3 = mask2 | mask;
                this.currentV = this.baseTransform.position;
                this.targetV = this.currentV + ((Vector3)(Vector3.forward * 200f));
                if (Physics.Raycast(ray, out hitInfo, 1000000f, mask3.value))
                {
                    this.targetV = hitInfo.point;
                }
                Vector3 vector = Vector3.Normalize(this.targetV - this.currentV);
                GameObject obj2 = PhotonNetwork.Instantiate("RCAsset/BombMain", this.currentV + ((Vector3)(vector * 4f)), new Quaternion(0f, 0f, 0f, 1f), 0);
                obj2.GetComponent<Rigidbody>().velocity = (Vector3)(vector * this.bombSpeed);
                this.myBomb = obj2.GetComponent<Bomb>();
                this.bombTime = 0f;
            }
            else if ((this.myBomb != null) && !this.myBomb.disabled)
            {
                this.bombTime += Time.deltaTime;
                bool flag2 = false;
                if (this.inputManager.isInputUp[InputCode.attack1])
                {
                    this.detonate = true;
                }
                else if (this.inputManager.isInputDown[InputCode.attack1] && this.detonate)
                {
                    this.detonate = false;
                    flag2 = true;
                }
                if (this.bombTime >= this.bombTimeMax)
                {
                    flag2 = true;
                }
                if (flag2)
                {
                    this.myBomb.Explode(this.bombRadius);
                    this.detonate = false;
                }
            }
        }
    }

    private void updateLeftMagUI()
    {
        return;
        for (int i = 1; i <= this.bulletMAX; i++)
        {
            //GameObject.Find("bulletL" + i).GetComponent<UISprite>().enabled = false;
        }
        for (int j = 1; j <= this.leftBulletLeft; j++)
        {
            //GameObject.Find("bulletL" + j).GetComponent<UISprite>().enabled = true;
        }
    }

    private void updateRightMagUI()
    {
        return;
        for (int i = 1; i <= this.bulletMAX; i++)
        {
            //GameObject.Find("bulletR" + i).GetComponent<UISprite>().enabled = false;
        }
        for (int j = 1; j <= this.rightBulletLeft; j++)
        {
            //GameObject.Find("bulletR" + j).GetComponent<UISprite>().enabled = true;
        }
    }

    [Obsolete("Using a weapon should be moved within Weapon class...")]
    public void useBlade(int amount = 0)
    {
        if (amount == 0)
        {
            amount = 1;
        }
        amount *= 2;
        if (this.currentBladeSta > 0f)
        {
            this.currentBladeSta -= amount;
            if (this.currentBladeSta <= 0f)
            {
                if ((IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE) || base.photonView.isMine)
                {
                    //this.leftbladetrail.Deactivate();
                    //this.rightbladetrail.Deactivate();
                    //this.leftbladetrail2.Deactivate();
                    //this.rightbladetrail2.Deactivate();
                    this.checkBoxLeft.GetComponent<TriggerColliderWeapon>().active_me = false;
                    this.checkBoxRight.GetComponent<TriggerColliderWeapon>().active_me = false;
                }
                this.currentBladeSta = 0f;
                //this.throwBlades();
            }
        }
    }

    private void useGas(float amount = 0)
    {
        if (amount == 0f)
        {
            amount = this.useGasSpeed;
        }
        if (this.currentGas > 0f)
        {
            this.currentGas -= amount;
            if (this.currentGas < 0f)
            {
                this.currentGas = 0f;
            }
        }
    }

    [PunRPC]
    private void whoIsMyErenTitan(int id)
    {
        this.eren_titan = PhotonView.Find(id).gameObject;
        this.titanForm = true;
    }

    public bool isGrabbed
    {
        get
        {
            return (this.state == HERO_STATE.Grab);
        }
    }

    private HERO_STATE state
    {
        get
        {
            return this._state;
        }
        set
        {
            if ((this._state == HERO_STATE.AirDodge) || (this._state == HERO_STATE.GroundDodge))
            {
                this.dashTime = 0f;
            }
            this._state = value;
        }
    }
}