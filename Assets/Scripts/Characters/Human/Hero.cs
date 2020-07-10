using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Characters.Titan;
using Assets.Scripts.Gamemode.Options;
using Assets.Scripts.UI.Input;
using UnityEngine;
using UnityEngine.UI;
using Xft;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using Random = UnityEngine.Random;

public class Hero : Human
{
    private Equipment Equipment { get; set; }
    public EquipmentType equipmentType;

    public List<HeroSkill> Skills;

    public HERO_STATE state;
    private bool almostSingleHook;
    private string attackAnimation;
    private int attackLoop;
    private bool attackMove;
    private bool attackReleased;
    public AudioSource audioAlly;
    public AudioSource audioHitwall;
    private GameObject badGuy;
    public Animation baseAnimation;
    public Rigidbody baseRigidBody;
    public Transform baseTransform;
    public bool bigLean;
    public float bombCd;
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
    private Dictionary<string, Image> CachedSprites;
    public float cameraMultiplier;
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
    private bool eHold;
    private GameObject erenTitan;
    private int escapeTimes = 1;
    private float facingDirection;
    private float flare1Cd;
    private float flare2Cd;
    private float flare3Cd;
    private float flareTotalCd = 30f;
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
    public Text labelDistance;
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
    public GROUP myGroup;
    private Horse myHorse;
    public GameObject myNetWorkName;
    public float myScale = 1f;
    public int myTeam = 1;
    public List<MindlessTitan> myTitans;
    private bool needLean;
    private Quaternion oldHeadRotation;
    private float originVm;
    private bool qHold;
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
    private GameObject skillCd;
    public float skillCdDuration;
    public float skillCdLast;
    public float skillCdLastCannon;
    private string skillId;
    public string skillIdhud;
    public AudioSource slash;
    public AudioSource slashHit;
    private ParticleSystem smoke3Dmg;
    private ParticleSystem sparks;
    public float speed = 10f;
    public GameObject speedFX;
    public GameObject speedFX1;
    private ParticleSystem speedFxps;
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

    public GameObject inGameUI;
    public TextMesh playerName;

    public delegate void HeroDiedHandler(Hero hero);
    public event HeroDiedHandler HeroDied;

    private void ApplyForceToBody(GameObject go, Vector3 v)
    {
        go.GetComponent<Rigidbody>().AddForce(v);
        go.GetComponent<Rigidbody>().AddTorque(Random.Range(-10f, 10f), Random.Range(-10f, 10f), Random.Range(-10f, 10f));
    }

    private void AttackAccordingToMouse()
    {
        if (Input.mousePosition.x < Screen.width * 0.5)
        {
            attackAnimation = "attack2";
        }
        else
        {
            attackAnimation = "attack1";
        }
    }

    private void AttackAccordingToTarget(Transform a)
    {
        var vector = a.position - transform.position;
        var current = -Mathf.Atan2(vector.z, vector.x) * 57.29578f;
        var f = -Mathf.DeltaAngle(current, transform.rotation.eulerAngles.y - 90f);
        if (Mathf.Abs(f) < 90f && vector.magnitude < 6f && a.position.y <= transform.position.y + 2f && a.position.y >= transform.position.y - 5f)
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

    private void Awake()
    {
        inGameUI = GameObject.Find("InGameUi");
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
    }

    public void BackToHuman()
    {
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        titanForm = false;
        Ungrabbed();
        FalseAttack();
        skillCdDuration = skillCdLast;
        GameObject.Find("MainCamera").GetComponent<IN_GAME_MAIN_CAMERA>().setMainObject(gameObject);
        if (IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.SINGLE)
        {
            photonView.RPC(nameof(backToHumanRPC), PhotonTargets.Others);
        }
    }

    [PunRPC]
    private void backToHumanRPC()
    {
        titanForm = false;
        erenTitan = null;
    }

    [PunRPC]
    public void badGuyReleaseMe()
    {
        hookBySomeOne = false;
        badGuy = null;
    }

    [PunRPC]
    public void blowAway(Vector3 force)
    {
        if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE || photonView.isMine)
        {
            GetComponent<Rigidbody>().AddForce(force, ForceMode.Impulse);
            transform.LookAt(transform.position);
        }
    }

    private void BodyLean()
    {
        if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE || photonView.isMine)
        {
            var z = 0f;
            needLean = false;
            if (!useGun && State == HERO_STATE.Attack && attackAnimation != "attack3_1" && attackAnimation != "attack3_2")
            {
                var y = GetComponent<Rigidbody>().velocity.y;
                var x = GetComponent<Rigidbody>().velocity.x;
                var num4 = GetComponent<Rigidbody>().velocity.z;
                var num5 = Mathf.Sqrt(x * x + num4 * num4);
                var num6 = Mathf.Atan2(y, num5) * 57.29578f;
                targetRotation = Quaternion.Euler(-num6 * (1f - Vector3.Angle(GetComponent<Rigidbody>().velocity, transform.forward) / 90f), facingDirection, 0f);
                if (isLeftHandHooked && bulletLeft != null || isRightHandHooked && bulletRight != null)
                {
                    transform.rotation = targetRotation;
                }
            }
            else
            {
                if (isLeftHandHooked && bulletLeft != null && isRightHandHooked && bulletRight != null)
                {
                    if (almostSingleHook)
                    {
                        needLean = true;
                        z = GETLeanAngle(bulletRight.transform.position, true);
                    }
                }
                else if (isLeftHandHooked && bulletLeft != null)
                {
                    needLean = true;
                    z = GETLeanAngle(bulletLeft.transform.position, true);
                }
                else if (isRightHandHooked && bulletRight != null)
                {
                    needLean = true;
                    z = GETLeanAngle(bulletRight.transform.position, false);
                }
                if (needLean)
                {
                    var a = 0f;
                    if (!useGun && State != HERO_STATE.Attack)
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

    private void BombInit()
    {
        skillIdhud = skillId;
        skillCdDuration = skillCdLast;
        if (FengGameManagerMKII.Gamemode.Settings.PvPBomb)
        {
            var num = (int)FengGameManagerMKII.settings[250];
            var num2 = (int)FengGameManagerMKII.settings[0xfb];
            var num3 = (int)FengGameManagerMKII.settings[0xfc];
            var num4 = (int)FengGameManagerMKII.settings[0xfd];
            if (num < 0 || num > 10)
            {
                num = 5;
                FengGameManagerMKII.settings[250] = 5;
            }
            if (num2 < 0 || num2 > 10)
            {
                num2 = 5;
                FengGameManagerMKII.settings[0xfb] = 5;
            }
            if (num3 < 0 || num3 > 10)
            {
                num3 = 5;
                FengGameManagerMKII.settings[0xfc] = 5;
            }
            if (num4 < 0 || num4 > 10)
            {
                num4 = 5;
                FengGameManagerMKII.settings[0xfd] = 5;
            }
            if (num + num2 + num3 + num4 > 20)
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
            bombTimeMax = (num2 * 60f + 200f) / (num3 * 60f + 200f);
            bombRadius = num * 4f + 20f;
            bombCd = num4 * -0.4f + 5f;
            bombSpeed = num3 * 60f + 200f;
            var propertiesToSet = new Hashtable();
            propertiesToSet.Add(PhotonPlayerProperty.RCBombR, (float)FengGameManagerMKII.settings[0xf6]);
            propertiesToSet.Add(PhotonPlayerProperty.RCBombG, (float)FengGameManagerMKII.settings[0xf7]);
            propertiesToSet.Add(PhotonPlayerProperty.RCBombB, (float)FengGameManagerMKII.settings[0xf8]);
            propertiesToSet.Add(PhotonPlayerProperty.RCBombA, (float)FengGameManagerMKII.settings[0xf9]);
            propertiesToSet.Add(PhotonPlayerProperty.RCBombRadius, bombRadius);
            PhotonNetwork.player.SetCustomProperties(propertiesToSet);
            skillId = "bomb";
            skillIdhud = "armin";
            skillCdLast = bombCd;
            skillCdDuration = 10f;
            if (FengGameManagerMKII.instance.roundTime > 10f)
            {
                skillCdDuration = 5f;
            }
        }
    }

    private void BreakApart2(Vector3 v, bool isBite)
    {
        GameObject obj6;
        GameObject obj7;
        GameObject obj8;
        GameObject obj9;
        GameObject obj10;
        var obj2 = (GameObject)Instantiate(Resources.Load("Character_parts/AOTTG_HERO_body"), this.transform.position, this.transform.rotation);
        obj2.gameObject.GetComponent<HERO_SETUP>().myCostume = setup.myCostume;
        obj2.GetComponent<HERO_SETUP>().isDeadBody = true;
        obj2.GetComponent<HERO_DEAD_BODY_SETUP>().init(currentAnimation, GetComponent<Animation>()[currentAnimation].normalizedTime, BODY_PARTS.ARM_R);
        if (!isBite)
        {
            var gO = (GameObject)Instantiate(Resources.Load("Character_parts/AOTTG_HERO_body"), this.transform.position, this.transform.rotation);
            var obj4 = (GameObject)Instantiate(Resources.Load("Character_parts/AOTTG_HERO_body"), this.transform.position, this.transform.rotation);
            var obj5 = (GameObject)Instantiate(Resources.Load("Character_parts/AOTTG_HERO_body"), this.transform.position, this.transform.rotation);
            gO.gameObject.GetComponent<HERO_SETUP>().myCostume = setup.myCostume;
            obj4.gameObject.GetComponent<HERO_SETUP>().myCostume = setup.myCostume;
            obj5.gameObject.GetComponent<HERO_SETUP>().myCostume = setup.myCostume;
            gO.GetComponent<HERO_SETUP>().isDeadBody = true;
            obj4.GetComponent<HERO_SETUP>().isDeadBody = true;
            obj5.GetComponent<HERO_SETUP>().isDeadBody = true;
            gO.GetComponent<HERO_DEAD_BODY_SETUP>().init(currentAnimation, GetComponent<Animation>()[currentAnimation].normalizedTime, BODY_PARTS.UPPER);
            obj4.GetComponent<HERO_DEAD_BODY_SETUP>().init(currentAnimation, GetComponent<Animation>()[currentAnimation].normalizedTime, BODY_PARTS.LOWER);
            obj5.GetComponent<HERO_DEAD_BODY_SETUP>().init(currentAnimation, GetComponent<Animation>()[currentAnimation].normalizedTime, BODY_PARTS.ARM_L);
            ApplyForceToBody(gO, v);
            ApplyForceToBody(obj4, v);
            ApplyForceToBody(obj5, v);
            if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE || photonView.isMine)
            {
                currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().setMainObject(gO, false);
            }
        }
        else if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE || photonView.isMine)
        {
            currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().setMainObject(obj2, false);
        }
        ApplyForceToBody(obj2, v);
        var transform = this.transform.Find("Amarture/Controller_Body/hip/spine/chest/shoulder_L/upper_arm_L/forearm_L/hand_L").transform;
        var transform2 = this.transform.Find("Amarture/Controller_Body/hip/spine/chest/shoulder_R/upper_arm_R/forearm_R/hand_R").transform;
        if (useGun)
        {
            obj6 = (GameObject)Instantiate(Resources.Load("Character_parts/character_gun_l"), transform.position, transform.rotation);
            obj7 = (GameObject)Instantiate(Resources.Load("Character_parts/character_gun_r"), transform2.position, transform2.rotation);
            obj8 = (GameObject)Instantiate(Resources.Load("Character_parts/character_3dmg_2"), this.transform.position, this.transform.rotation);
            obj9 = (GameObject)Instantiate(Resources.Load("Character_parts/character_gun_mag_l"), this.transform.position, this.transform.rotation);
            obj10 = (GameObject)Instantiate(Resources.Load("Character_parts/character_gun_mag_r"), this.transform.position, this.transform.rotation);
        }
        else
        {
            obj6 = (GameObject)Instantiate(Resources.Load("Character_parts/character_blade_l"), transform.position, transform.rotation);
            obj7 = (GameObject)Instantiate(Resources.Load("Character_parts/character_blade_r"), transform2.position, transform2.rotation);
            obj8 = (GameObject)Instantiate(Resources.Load("Character_parts/character_3dmg"), this.transform.position, this.transform.rotation);
            obj9 = (GameObject)Instantiate(Resources.Load("Character_parts/character_3dmg_gas_l"), this.transform.position, this.transform.rotation);
            obj10 = (GameObject)Instantiate(Resources.Load("Character_parts/character_3dmg_gas_r"), this.transform.position, this.transform.rotation);
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
                if (currentBuff == BUFF.SpeedUp && GetComponent<Animation>().IsPlaying("run_sasha"))
                {
                    CrossFade("run_1", 0.1f);
                }
                currentBuff = BUFF.NoBuff;
            }
        }
    }

    private void Cache()
    {
        baseTransform = transform;
        baseRigidBody = GetComponent<Rigidbody>();
        maincamera = GameObject.Find("MainCamera");
        if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE || photonView.isMine)
        {
            baseAnimation = GetComponent<Animation>();
            cross1 = GameObject.Find("cross1");
            cross2 = GameObject.Find("cross2");
            crossL1 = GameObject.Find("crossL1");
            crossL2 = GameObject.Find("crossL2");
            crossR1 = GameObject.Find("crossR1");
            crossR2 = GameObject.Find("crossR2");
            labelDistance = GameObject.Find("Distance").GetComponent<Text>();
            CachedSprites = new Dictionary<string, Image>();
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
            foreach (Image image in inGameUI.GetComponentsInChildren(typeof(Image), true))
            {
                if (image == null) continue;
                if (image.gameObject.name.Contains("Gas"))
                {
                    CachedSprites.Add(image.gameObject.name, image);
                }
            }
        }
    }

    private void CalcFlareCd()
    {
        if (flare1Cd > 0f)
        {
            flare1Cd -= Time.deltaTime;
            if (flare1Cd < 0f)
            {
                flare1Cd = 0f;
            }
        }
        if (flare2Cd > 0f)
        {
            flare2Cd -= Time.deltaTime;
            if (flare2Cd < 0f)
            {
                flare2Cd = 0f;
            }
        }
        if (flare3Cd > 0f)
        {
            flare3Cd -= Time.deltaTime;
            if (flare3Cd < 0f)
            {
                flare3Cd = 0f;
            }
        }
    }

    private void CalcSkillCd()
    {
        if (skillCdDuration > 0f)
        {
            skillCdDuration -= Time.deltaTime;
            if (skillCdDuration < 0f)
            {
                skillCdDuration = 0f;
            }
        }
    }

    private float CalculateJumpVerticalSpeed()
    {
        return Mathf.Sqrt(2f * jumpHeight * gravity);
    }

    private void ChangeBlade()
    {
        if (!useGun || grounded || FengGameManagerMKII.Gamemode.Settings.AhssAirReload)
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

    private void CheckTitan()
    {
        int count;
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        LayerMask mask = 1 << LayerMask.NameToLayer("PlayerAttackBox");
        LayerMask mask2 = 1 << LayerMask.NameToLayer("Ground");
        LayerMask mask3 = 1 << LayerMask.NameToLayer("EnemyBox");
        LayerMask mask4 = mask | mask2 | mask3;
        var hitArray = Physics.RaycastAll(ray, 180f, mask4.value);
        var list = new List<RaycastHit>();
        var list2 = new List<MindlessTitan>();
        for (count = 0; count < hitArray.Length; count++)
        {
            var item = hitArray[count];
            list.Add(item);
        }
        list.Sort((x, y) => x.distance.CompareTo(y.distance));
        var num2 = 180f;
        for (count = 0; count < list.Count; count++)
        {
            var hit2 = list[count];
            var gameObject = hit2.collider.gameObject;
            if (gameObject.layer == 0x10)
            {
                if (gameObject.name.Contains("PlayerCollisionDetection") && (hit2 = list[count]).distance < num2)
                {
                    num2 -= 60f;
                    if (num2 <= 60f)
                    {
                        count = list.Count;
                    }
                    var component = gameObject.transform.root.gameObject.GetComponent<MindlessTitan>();
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
            var titan2 = myTitans[count];
            if (!list2.Contains(titan2))
            {
                titan2.IsLooked = false;
            }
        }
        for (count = 0; count < list2.Count; count++)
        {
            var titan3 = list2[count];
            titan3.IsLooked = true;
        }
        myTitans = list2;
    }

    public void ClearPopup()
    {
        FengGameManagerMKII.instance.ShowHUDInfoCenter(string.Empty);
    }

    private void ContinueAnimation()
    {
        var enumerator = GetComponent<Animation>().GetEnumerator();
        try
        {
            while (enumerator.MoveNext())
            {
                var current = (AnimationState)enumerator.Current;
                if (current != null && current.speed == 1f)
                {
                    return;
                }
                current.speed = 1f;
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
        CustomAnimationSpeed();
        PlayAnimation(CurrentPlayingClipName());
        if (IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.SINGLE && photonView.isMine)
        {
            photonView.RPC(nameof(netContinueAnimation), PhotonTargets.Others);
        }
    }

    public void CrossFade(string aniName, float time)
    {
        currentAnimation = aniName;
        GetComponent<Animation>().CrossFade(aniName, time);
        if (PhotonNetwork.connected && photonView.isMine)
            photonView.RPC(nameof(netCrossFade), PhotonTargets.Others, aniName, time);
    }

    public void TryCrossFade(string animationName, float time)
    {
        if (!baseAnimation.IsPlaying(animationName))
        {
            CrossFade(animationName, time);
        }
    }

    private string CurrentPlayingClipName()
    {
        var enumerator = GetComponent<Animation>().GetEnumerator();
        try
        {
            while (enumerator.MoveNext())
            {
                var current = (AnimationState)enumerator.Current;
                if (current != null && GetComponent<Animation>().IsPlaying(current.name))
                {
                    return current.name;
                }
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
        return string.Empty;
    }

    private void CustomAnimationSpeed()
    {
        GetComponent<Animation>()["attack5"].speed = 1.85f;
        GetComponent<Animation>()["changeBlade"].speed = 1.2f;
        GetComponent<Animation>()["air_release"].speed = 0.6f;
        GetComponent<Animation>()["changeBlade_air"].speed = 0.8f;
        GetComponent<Animation>()["AHSS_gun_reload_both"].speed = 0.38f;
        GetComponent<Animation>()["AHSS_gun_reload_both_air"].speed = 0.5f;
        GetComponent<Animation>()["AHSS_gun_reload_l"].speed = 0.4f;
        GetComponent<Animation>()["AHSS_gun_reload_l_air"].speed = 0.5f;
        GetComponent<Animation>()["AHSS_gun_reload_r"].speed = 0.4f;
        GetComponent<Animation>()["AHSS_gun_reload_r_air"].speed = 0.5f;
    }

    private void Dash(float horizontal, float vertical)
    {
        if (dashTime <= 0f && currentGas > 0f && !isMounted)
        {
            UseGas(totalGas * 0.04f);
            facingDirection = GETGlobalFacingDirection(horizontal, vertical);
            dashV = GETGlobaleFacingVector3(facingDirection);
            originVm = currentSpeed;
            var quaternion = Quaternion.Euler(0f, facingDirection, 0f);
            GetComponent<Rigidbody>().rotation = quaternion;
            targetRotation = quaternion;
            if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
            {
                Instantiate(Resources.Load("FX/boost_smoke"), transform.position, transform.rotation);
            }
            else
            {
                PhotonNetwork.Instantiate("FX/boost_smoke", transform.position, transform.rotation, 0);
            }
            dashTime = 0.5f;
            CrossFade("dash", 0.1f);
            GetComponent<Animation>()["dash"].time = 0.1f;
            State = HERO_STATE.AirDodge;
            FalseAttack();
            GetComponent<Rigidbody>().AddForce(dashV * 40f, ForceMode.VelocityChange);
        }
    }

    public void Die(Vector3 v, bool isBite)
    {
        if (invincible <= 0f)
        {
            if (titanForm && erenTitan != null)
            {
                erenTitan.GetComponent<TITAN_EREN>().lifeTime = 0.1f;
            }
            if (bulletLeft != null)
            {
                bulletLeft.GetComponent<Bullet>().removeMe();
            }
            if (bulletRight != null)
            {
                bulletRight.GetComponent<Bullet>().removeMe();
            }
            meatDie.Play();
            if ((IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE || photonView.isMine) && !useGun)
            {
                leftbladetrail.Deactivate();
                rightbladetrail.Deactivate();
                leftbladetrail2.Deactivate();
                rightbladetrail2.Deactivate();
            }
            BreakApart2(v, isBite);
            currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().gameOver = true;
            GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>().gameLose2();
            FalseAttack();
            hasDied = true;
            var transform = this.transform.Find("audio_die");
            transform.parent = null;
            transform.GetComponent<AudioSource>().Play();
            if (PlayerPrefs.HasKey("EnableSS") && PlayerPrefs.GetInt("EnableSS") == 1)
            {
                GameObject.Find("MainCamera").GetComponent<IN_GAME_MAIN_CAMERA>().startSnapShot2(this.transform.position, 0, null, 0.02f);
            }
            Destroy(gameObject);
        }
    }

    public void Die2(Transform tf)
    {
        if (invincible <= 0f)
        {
            if (titanForm && erenTitan != null)
            {
                erenTitan.GetComponent<TITAN_EREN>().lifeTime = 0.1f;
            }
            if (bulletLeft != null)
            {
                bulletLeft.GetComponent<Bullet>().removeMe();
            }
            if (bulletRight != null)
            {
                bulletRight.GetComponent<Bullet>().removeMe();
            }
            var transform = this.transform.Find("audio_die");
            transform.parent = null;
            transform.GetComponent<AudioSource>().Play();
            meatDie.Play();
            currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().setMainObject(null);
            currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().gameOver = true;
            GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>().gameLose2();
            FalseAttack();
            hasDied = true;
            var obj2 = (GameObject)Instantiate(Resources.Load("hitMeat2"));
            obj2.transform.position = this.transform.position;
            Destroy(gameObject);
        }
    }

    private void Dodge2(bool offTheWall = false)
    {
        if (!InputManager.Key(InputHorse.Mount) || !myHorse || isMounted || Vector3.Distance(myHorse.transform.position, transform.position) >= 15f)
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
                var num3 = GETGlobalFacingDirection(num2, num);
                if (num2 != 0f || num != 0f)
                {
                    facingDirection = num3 + 180f;
                    targetRotation = Quaternion.Euler(0f, facingDirection, 0f);
                }
                CrossFade("dodge", 0.1f);
            }
            else
            {
                PlayAnimation("dodge");
                PlayAnimationAt("dodge", 0.2f);
            }
            sparks.enableEmission = false;
        }
    }

    private void ErenTransform()
    {
        skillCdDuration = skillCdLast;
        if (bulletLeft != null)
        {
            bulletLeft.GetComponent<Bullet>().removeMe();
        }
        if (bulletRight != null)
        {
            bulletRight.GetComponent<Bullet>().removeMe();
        }
        if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
        {
            erenTitan = (GameObject)Instantiate(Resources.Load("TITAN_EREN"), transform.position, transform.rotation);
        }
        else
        {
            erenTitan = PhotonNetwork.Instantiate("TITAN_EREN", transform.position, transform.rotation, 0);
        }
        erenTitan.GetComponent<TITAN_EREN>().realBody = gameObject;
        GameObject.Find("MainCamera").GetComponent<IN_GAME_MAIN_CAMERA>().flashBlind();
        GameObject.Find("MainCamera").GetComponent<IN_GAME_MAIN_CAMERA>().setMainObject(erenTitan);
        erenTitan.GetComponent<TITAN_EREN>().born();
        erenTitan.GetComponent<Rigidbody>().velocity = GetComponent<Rigidbody>().velocity;
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        transform.position = erenTitan.transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/neck").position;
        titanForm = true;
        if (IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.SINGLE)
            photonView.RPC(nameof(whoIsMyErenTitan), PhotonTargets.Others, erenTitan.GetPhotonView().viewID);

        if (smoke3Dmg.enableEmission && IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.SINGLE && photonView.isMine)
            photonView.RPC(nameof(net3DMGSMOKE), PhotonTargets.Others, false);

        smoke3Dmg.enableEmission = false;
    }

    private void EscapeFromGrab()
    {
    }

    private void FalseAttack()
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
            if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE || photonView.isMine)
            {
                checkBoxLeft.GetComponent<TriggerColliderWeapon>().active_me = false;
                checkBoxRight.GetComponent<TriggerColliderWeapon>().active_me = false;
                checkBoxLeft.GetComponent<TriggerColliderWeapon>().clearHits();
                checkBoxRight.GetComponent<TriggerColliderWeapon>().clearHits();
                //this.leftbladetrail.StopSmoothly(0.2f);
                //this.rightbladetrail.StopSmoothly(0.2f);
                //this.leftbladetrail2.StopSmoothly(0.2f);
                //this.rightbladetrail2.StopSmoothly(0.2f);
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
        var objArray = GameObject.FindGameObjectsWithTag("titan");
        GameObject obj2 = null;
        var positiveInfinity = float.PositiveInfinity;
        var position = transform.position;
        foreach (var obj3 in objArray)
        {
            var vector2 = obj3.transform.position - position;
            var sqrMagnitude = vector2.sqrMagnitude;
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
        if (!titanForm && !isCannon && (!IN_GAME_MAIN_CAMERA.isPausing || IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.SINGLE))
        {
            currentSpeed = baseRigidBody.velocity.magnitude;
            if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE || photonView.isMine)
            {
                if (!(baseAnimation.IsPlaying("attack3_2") || baseAnimation.IsPlaying("attack5") || baseAnimation.IsPlaying("special_petra")))
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
                            var vector2 = hookTarget.transform.position - baseTransform.position;
                            var magnitude = vector2.magnitude;
                            if (magnitude > 2f)
                            {
                                baseRigidBody.AddForce(vector2.normalized * Mathf.Pow(magnitude, 0.15f) * 30f - baseRigidBody.velocity * 0.95f, ForceMode.VelocityChange);
                            }
                        }
                        else
                        {
                            hookSomeOne = false;
                        }
                    }
                    else if (hookBySomeOne && badGuy != null)
                    {
                        if (badGuy != null)
                        {
                            var vector3 = badGuy.transform.position - baseTransform.position;
                            var f = vector3.magnitude;
                            if (f > 5f)
                            {
                                baseRigidBody.AddForce(vector3.normalized * Mathf.Pow(f, 0.15f) * 0.2f, ForceMode.Impulse);
                            }
                        }
                        else
                        {
                            hookBySomeOne = false;
                        }
                    }
                    var x = 0f;
                    var z = 0f;
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
                    var flag2 = false;
                    var flag3 = false;
                    var flag4 = false;
                    isLeftHandHooked = false;
                    isRightHandHooked = false;
                    if (isLaunchLeft)
                    {
                        if (bulletLeft != null && bulletLeft.GetComponent<Bullet>().isHooked())
                        {
                            isLeftHandHooked = true;
                            var to = bulletLeft.transform.position - baseTransform.position;
                            to.Normalize();
                            to = to * 10f;
                            if (!isLaunchRight)
                            {
                                to = to * 2f;
                            }
                            if (Vector3.Angle(baseRigidBody.velocity, to) > 90f && InputManager.Key(InputHuman.Jump))
                            {
                                flag3 = true;
                                flag2 = true;
                            }
                            if (!flag3)
                            {
                                baseRigidBody.AddForce(to);
                                if (Vector3.Angle(baseRigidBody.velocity, to) > 90f)
                                {
                                    baseRigidBody.AddForce(-baseRigidBody.velocity * 2f, ForceMode.Acceleration);
                                }
                            }
                        }
                        launchElapsedTimeL += Time.deltaTime;
                        if (qHold && currentGas > 0f)
                        {
                            UseGas(useGasSpeed * Time.deltaTime);
                        }
                        else if (launchElapsedTimeL > 0.3f)
                        {
                            isLaunchLeft = false;
                            if (bulletLeft != null)
                            {
                                bulletLeft.GetComponent<Bullet>().disable();
                                ReleaseIfIHookSb();
                                bulletLeft = null;
                                flag3 = false;
                            }
                        }
                    }
                    if (isLaunchRight)
                    {
                        if (bulletRight != null && bulletRight.GetComponent<Bullet>().isHooked())
                        {
                            isRightHandHooked = true;
                            var vector5 = bulletRight.transform.position - baseTransform.position;
                            vector5.Normalize();
                            vector5 = vector5 * 10f;
                            if (!isLaunchLeft)
                            {
                                vector5 = vector5 * 2f;
                            }
                            if (Vector3.Angle(baseRigidBody.velocity, vector5) > 90f && InputManager.Key(InputHuman.Jump))
                            {
                                flag4 = true;
                                flag2 = true;
                            }
                            if (!flag4)
                            {
                                baseRigidBody.AddForce(vector5);
                                if (Vector3.Angle(baseRigidBody.velocity, vector5) > 90f)
                                {
                                    baseRigidBody.AddForce(-baseRigidBody.velocity * 2f, ForceMode.Acceleration);
                                }
                            }
                        }
                        launchElapsedTimeR += Time.deltaTime;
                        if (eHold && currentGas > 0f)
                        {
                            UseGas(useGasSpeed * Time.deltaTime);
                        }
                        else if (launchElapsedTimeR > 0.3f)
                        {
                            isLaunchRight = false;
                            if (bulletRight != null)
                            {
                                bulletRight.GetComponent<Bullet>().disable();
                                ReleaseIfIHookSb();
                                bulletRight = null;
                                flag4 = false;
                            }
                        }
                    }
                    if (grounded)
                    {
                        Vector3 vector7;
                        var zero = Vector3.zero;
                        if (State == HERO_STATE.Attack)
                        {
                            if (attackAnimation == "attack5")
                            {
                                if (baseAnimation[attackAnimation].normalizedTime > 0.4f && baseAnimation[attackAnimation].normalizedTime < 0.61f)
                                {
                                    baseRigidBody.AddForce(gameObject.transform.forward * 200f);
                                }
                            }
                            else if (attackAnimation == "special_petra")
                            {
                                if (baseAnimation[attackAnimation].normalizedTime > 0.35f && baseAnimation[attackAnimation].normalizedTime < 0.48f)
                                {
                                    baseRigidBody.AddForce(gameObject.transform.forward * 200f);
                                }
                            }
                            else if (baseAnimation.IsPlaying("attack3_2"))
                            {
                                zero = Vector3.zero;
                            }
                            else if (baseAnimation.IsPlaying("attack1") || baseAnimation.IsPlaying("attack2"))
                            {
                                baseRigidBody.AddForce(gameObject.transform.forward * 200f);
                            }
                            if (baseAnimation.IsPlaying("attack3_2"))
                            {
                                zero = Vector3.zero;
                            }
                        }
                        if (justGrounded)
                        {
                            if (State != HERO_STATE.Attack || attackAnimation != "attack3_1" && attackAnimation != "attack5" && attackAnimation != "special_petra")
                            {
                                if (State != HERO_STATE.Attack && x == 0f && z == 0f && bulletLeft == null && bulletRight == null && State != HERO_STATE.FillGas)
                                {
                                    State = HERO_STATE.Land;
                                    CrossFade("dash_land", 0.01f);
                                }
                                else
                                {
                                    buttonAttackRelease = true;
                                    if (State != HERO_STATE.Attack && baseRigidBody.velocity.x * baseRigidBody.velocity.x + baseRigidBody.velocity.z * baseRigidBody.velocity.z > speed * speed * 1.5f && State != HERO_STATE.FillGas)
                                    {
                                        State = HERO_STATE.Slide;
                                        CrossFade("slide", 0.05f);
                                        facingDirection = Mathf.Atan2(baseRigidBody.velocity.x, baseRigidBody.velocity.z) * 57.29578f;
                                        targetRotation = Quaternion.Euler(0f, facingDirection, 0f);
                                        sparks.enableEmission = true;
                                    }
                                }
                            }
                            justGrounded = false;
                            zero = baseRigidBody.velocity;
                        }
                        if (State == HERO_STATE.Attack && attackAnimation == "attack3_1" && baseAnimation[attackAnimation].normalizedTime >= 1f)
                        {
                            PlayAnimation("attack3_2");
                            ResetAnimationSpeed();
                            vector7 = Vector3.zero;
                            baseRigidBody.velocity = vector7;
                            zero = vector7;
                            currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().startShake(0.2f, 0.3f);
                        }
                        if (State == HERO_STATE.GroundDodge)
                        {
                            if (baseAnimation["dodge"].normalizedTime >= 0.2f && baseAnimation["dodge"].normalizedTime < 0.8f)
                            {
                                zero = -baseTransform.forward * 2.4f * speed;
                            }
                            if (baseAnimation["dodge"].normalizedTime > 0.8f)
                            {
                                zero = baseRigidBody.velocity * 0.9f;
                            }
                        }
                        else if (State == HERO_STATE.Idle)
                        {
                            var vector8 = new Vector3(x, 0f, z);
                            var resultAngle = GETGlobalFacingDirection(x, z);
                            zero = GETGlobaleFacingVector3(resultAngle);
                            var num6 = vector8.magnitude <= 0.95f ? vector8.magnitude >= 0.25f ? vector8.magnitude : 0f : 1f;
                            zero = zero * num6;
                            zero = zero * speed;
                            if (buffTime > 0f && currentBuff == BUFF.SpeedUp)
                            {
                                zero = zero * 4f;
                            }
                            if (x != 0f || z != 0f)
                            {
                                if (!baseAnimation.IsPlaying("run_1") && !baseAnimation.IsPlaying("jump") && !baseAnimation.IsPlaying("run_sasha") && (!baseAnimation.IsPlaying("horse_geton") || baseAnimation["horse_geton"].normalizedTime >= 0.5f))
                                {
                                    if (buffTime > 0f && currentBuff == BUFF.SpeedUp)
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
                                if (!(baseAnimation.IsPlaying(standAnimation) || State == HERO_STATE.Land || baseAnimation.IsPlaying("jump") || baseAnimation.IsPlaying("horse_geton") || baseAnimation.IsPlaying("grabbed")))
                                {
                                    CrossFade(standAnimation, 0.1f);
                                    zero = zero * 0f;
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
                            zero = baseRigidBody.velocity * 0.96f;
                        }
                        else if (State == HERO_STATE.Slide)
                        {
                            zero = baseRigidBody.velocity * 0.99f;
                            if (currentSpeed < speed * 1.2f)
                            {
                                Idle();
                                sparks.enableEmission = false;
                            }
                        }
                        var velocity = baseRigidBody.velocity;
                        var force = zero - velocity;
                        force.x = Mathf.Clamp(force.x, -maxVelocityChange, maxVelocityChange);
                        force.z = Mathf.Clamp(force.z, -maxVelocityChange, maxVelocityChange);
                        force.y = 0f;
                        if (baseAnimation.IsPlaying("jump") && baseAnimation["jump"].normalizedTime > 0.18f)
                        {
                            force.y += 8f;
                        }
                        if (baseAnimation.IsPlaying("horse_geton") && baseAnimation["horse_geton"].normalizedTime > 0.18f && baseAnimation["horse_geton"].normalizedTime < 1f)
                        {
                            var num7 = 6f;
                            force = -baseRigidBody.velocity;
                            force.y = num7;
                            var num8 = Vector3.Distance(myHorse.transform.position, baseTransform.position);
                            var num9 = 0.6f * gravity * num8 / 12f;
                            vector7 = myHorse.transform.position - baseTransform.position;
                            force += num9 * vector7.normalized;
                        }
                        if (!(State == HERO_STATE.Attack && useGun))
                        {
                            baseRigidBody.AddForce(force, ForceMode.VelocityChange);
                            baseRigidBody.rotation = Quaternion.Lerp(gameObject.transform.rotation, Quaternion.Euler(0f, facingDirection, 0f), Time.deltaTime * 10f);
                        }
                    }
                    else
                    {
                        if (sparks.enableEmission)
                        {
                            sparks.enableEmission = false;
                        }
                        if (myHorse && (baseAnimation.IsPlaying("horse_geton") || baseAnimation.IsPlaying("air_fall")) && baseRigidBody.velocity.y < 0f && Vector3.Distance(myHorse.transform.position + Vector3.up * 1.65f, baseTransform.position) < 0.5f)
                        {
                            baseTransform.position = myHorse.transform.position + Vector3.up * 1.65f;
                            baseTransform.rotation = myHorse.transform.rotation;
                            isMounted = true;
                            CrossFade("horse_idle", 0.1f);
                            myHorse.Mount();
                        }
                        if (!(State != HERO_STATE.Idle || baseAnimation.IsPlaying("dash") || baseAnimation.IsPlaying("wallrun") || baseAnimation.IsPlaying("toRoof") || baseAnimation.IsPlaying("horse_geton") || baseAnimation.IsPlaying("horse_getoff") || baseAnimation.IsPlaying("air_release") || isMounted || baseAnimation.IsPlaying("air_hook_l_just") && baseAnimation["air_hook_l_just"].normalizedTime < 1f || baseAnimation.IsPlaying("air_hook_r_just") && baseAnimation["air_hook_r_just"].normalizedTime < 1f ? baseAnimation["dash"].normalizedTime < 0.99f : false))
                        {
                            if (!isLeftHandHooked && !isRightHandHooked && (baseAnimation.IsPlaying("air_hook_l") || baseAnimation.IsPlaying("air_hook_r") || baseAnimation.IsPlaying("air_hook")) && baseRigidBody.velocity.y > 20f)
                            {
                                baseAnimation.CrossFade("air_release");
                            }
                            else
                            {
                                var flag5 = Mathf.Abs(baseRigidBody.velocity.x) + Mathf.Abs(baseRigidBody.velocity.z) > 25f;
                                var flag6 = baseRigidBody.velocity.y < 0f;
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
                                    var current = -Mathf.Atan2(baseRigidBody.velocity.z, baseRigidBody.velocity.x) * 57.29578f;
                                    var num11 = -Mathf.DeltaAngle(current, baseTransform.rotation.eulerAngles.y - 90f);
                                    if (Mathf.Abs(num11) < 45f)
                                    {
                                        if (!baseAnimation.IsPlaying("air2"))
                                        {
                                            CrossFade("air2", 0.2f);
                                        }
                                    }
                                    else if (num11 < 135f && num11 > 0f)
                                    {
                                        if (!baseAnimation.IsPlaying("air2_right"))
                                        {
                                            CrossFade("air2_right", 0.2f);
                                        }
                                    }
                                    else if (num11 > -135f && num11 < 0f)
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
                        if (State == HERO_STATE.Idle && baseAnimation.IsPlaying("air_release") && baseAnimation["air_release"].normalizedTime >= 1f)
                        {
                            CrossFade("air_rise", 0.2f);
                        }
                        if (baseAnimation.IsPlaying("horse_getoff") && baseAnimation["horse_getoff"].normalizedTime >= 1f)
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
                                    baseRigidBody.AddForce(Vector3.up * 8f, ForceMode.Impulse);
                                }
                                baseRigidBody.AddForce(baseTransform.forward * 0.05f, ForceMode.Impulse);
                            }
                            if (baseAnimation["toRoof"].normalizedTime >= 1f)
                            {
                                PlayAnimation("air_rise");
                            }
                        }
                        else if (!(State != HERO_STATE.Idle || !IsPressDirectionTowardsHero(x, z) || InputManager.Key(InputHuman.Jump) || InputManager.Key(InputHuman.HookLeft) || InputManager.Key(InputHuman.HookRight) || InputManager.Key(InputHuman.HookBoth) || !IsFrontGrounded() || baseAnimation.IsPlaying("wallrun") || baseAnimation.IsPlaying("dodge")))
                        {
                            CrossFade("wallrun", 0.1f);
                            wallRunTime = 0f;
                        }
                        else if (baseAnimation.IsPlaying("wallrun"))
                        {
                            baseRigidBody.AddForce(Vector3.up * speed - baseRigidBody.velocity, ForceMode.VelocityChange);
                            wallRunTime += Time.deltaTime;
                            if (wallRunTime > 1f || z == 0f && x == 0f)
                            {
                                baseRigidBody.AddForce(-baseTransform.forward * speed * 0.75f, ForceMode.Impulse);
                                Dodge2(true);
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
                        else if (!baseAnimation.IsPlaying("attack5") && !baseAnimation.IsPlaying("special_petra") && !baseAnimation.IsPlaying("dash") && !baseAnimation.IsPlaying("jump"))
                        {
                            var vector11 = new Vector3(x, 0f, z);
                            var num12 = GETGlobalFacingDirection(x, z);
                            var vector12 = GETGlobaleFacingVector3(num12);
                            var num13 = vector11.magnitude <= 0.95f ? vector11.magnitude >= 0.25f ? vector11.magnitude : 0f : 1f;
                            vector12 = vector12 * num13;
                            vector12 = vector12 * (setup.myCostume.stat.ACL / 10f * 2f);
                            if (x == 0f && z == 0f)
                            {
                                if (State == HERO_STATE.Attack)
                                {
                                    vector12 = vector12 * 0f;
                                }
                                num12 = -874f;
                            }
                            if (num12 != -874f)
                            {
                                facingDirection = num12;
                                targetRotation = Quaternion.Euler(0f, facingDirection, 0f);
                            }
                            if (!flag3 && !flag4 && !isMounted && InputManager.Key(InputHuman.Jump) && currentGas > 0f)
                            {
                                if (x != 0f || z != 0f)
                                {
                                    baseRigidBody.AddForce(vector12, ForceMode.Acceleration);
                                }
                                else
                                {
                                    baseRigidBody.AddForce(baseTransform.forward * vector12.magnitude, ForceMode.Acceleration);
                                }
                                flag2 = true;
                            }
                        }
                        if (baseAnimation.IsPlaying("air_fall") && currentSpeed < 0.2f && IsFrontGrounded())
                        {
                            CrossFade("onWall", 0.3f);
                        }
                    }
                    spinning = false;
                    if (flag3 && flag4)
                    {
                        var num14 = currentSpeed + 0.1f;
                        AddRightForce();
                        var vector13 = (bulletRight.transform.position + bulletLeft.transform.position) * 0.5f - baseTransform.position;
                        var num15 = 0f;
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
                        var num16 = 1f + num15;
                        var vector14 = Vector3.RotateTowards(vector13, baseRigidBody.velocity, 1.53938f * num16, 1.53938f * num16);
                        vector14.Normalize();
                        spinning = true;
                        baseRigidBody.velocity = vector14 * num14;
                    }
                    else if (flag3)
                    {
                        var num17 = currentSpeed + 0.1f;
                        AddRightForce();
                        var vector15 = bulletLeft.transform.position - baseTransform.position;
                        var num18 = 0f;
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
                        var num19 = 1f + num18;
                        var vector16 = Vector3.RotateTowards(vector15, baseRigidBody.velocity, 1.53938f * num19, 1.53938f * num19);
                        vector16.Normalize();
                        spinning = true;
                        baseRigidBody.velocity = vector16 * num17;
                    }
                    else if (flag4)
                    {
                        var num20 = currentSpeed + 0.1f;
                        AddRightForce();
                        var vector17 = bulletRight.transform.position - baseTransform.position;
                        var num21 = 0f;
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
                        var num22 = 1f + num21;
                        var vector18 = Vector3.RotateTowards(vector17, baseRigidBody.velocity, 1.53938f * num22, 1.53938f * num22);
                        vector18.Normalize();
                        spinning = true;
                        baseRigidBody.velocity = vector18 * num20;
                    }
                    if (State == HERO_STATE.Attack && (attackAnimation == "attack5" || attackAnimation == "special_petra") && baseAnimation[attackAnimation].normalizedTime > 0.4f && !attackMove)
                    {
                        attackMove = true;
                        if (launchPointRight.magnitude > 0f)
                        {
                            var vector19 = launchPointRight - baseTransform.position;
                            vector19.Normalize();
                            vector19 = vector19 * 13f;
                            baseRigidBody.AddForce(vector19, ForceMode.Impulse);
                        }
                        if (attackAnimation == "special_petra" && launchPointLeft.magnitude > 0f)
                        {
                            var vector20 = launchPointLeft - baseTransform.position;
                            vector20.Normalize();
                            vector20 = vector20 * 13f;
                            baseRigidBody.AddForce(vector20, ForceMode.Impulse);
                            if (bulletRight != null)
                            {
                                bulletRight.GetComponent<Bullet>().disable();
                                ReleaseIfIHookSb();
                            }
                            if (bulletLeft != null)
                            {
                                bulletLeft.GetComponent<Bullet>().disable();
                                ReleaseIfIHookSb();
                            }
                        }
                        baseRigidBody.AddForce(Vector3.up * 2f, ForceMode.Impulse);
                    }
                    var flag7 = false;
                    if (bulletLeft != null || bulletRight != null)
                    {
                        if (bulletLeft != null && bulletLeft.transform.position.y > gameObject.transform.position.y && isLaunchLeft && bulletLeft.GetComponent<Bullet>().isHooked())
                        {
                            flag7 = true;
                        }
                        if (bulletRight != null && bulletRight.transform.position.y > gameObject.transform.position.y && isLaunchRight && bulletRight.GetComponent<Bullet>().isHooked())
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
                        currentCamera.GetComponent<Camera>().fieldOfView = Mathf.Lerp(currentCamera.GetComponent<Camera>().fieldOfView, Mathf.Min(100f, currentSpeed + 40f), 0.1f);
                    }
                    else
                    {
                        currentCamera.GetComponent<Camera>().fieldOfView = Mathf.Lerp(currentCamera.GetComponent<Camera>().fieldOfView, 50f, 0.1f);
                    }
                    if (flag2)
                    {
                        UseGas(useGasSpeed * Time.deltaTime);
                        if (!smoke3Dmg.enableEmission && IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.SINGLE && photonView.isMine)
                            photonView.RPC(nameof(net3DMGSMOKE), PhotonTargets.Others, true);

                        smoke3Dmg.enableEmission = true;
                    }
                    else
                    {
                        if (smoke3Dmg.enableEmission && IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.SINGLE && photonView.isMine)
                            photonView.RPC(nameof(net3DMGSMOKE), PhotonTargets.Others, false);

                        smoke3Dmg.enableEmission = false;
                    }
                    if (currentSpeed > 80f)
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

    public string GETDebugInfo()
    {
        var str = "\n";
        str = "Left:" + isLeftHandHooked + " ";
        if (isLeftHandHooked && bulletLeft != null)
        {
            var vector = bulletLeft.transform.position - transform.position;
            str = str + (int)(Mathf.Atan2(vector.x, vector.z) * 57.29578f);
        }
        var str2 = str;
        object[] objArray1 = { str2, "\nRight:", isRightHandHooked, " " };
        str = string.Concat(objArray1);
        if (isRightHandHooked && bulletRight != null)
        {
            var vector2 = bulletRight.transform.position - transform.position;
            str = str + (int)(Mathf.Atan2(vector2.x, vector2.z) * 57.29578f);
        }
        str = str + "\nfacingDirection:" + (int)facingDirection + "\nActual facingDirection:" + (int)transform.rotation.eulerAngles.y + "\nState:" + State + "\n\n\n\n\n";
        if (State == HERO_STATE.Attack)
        {
            targetRotation = Quaternion.Euler(0f, facingDirection, 0f);
        }
        return str;
    }

    private Vector3 GETGlobaleFacingVector3(float resultAngle)
    {
        var num = -resultAngle + 90f;
        var x = Mathf.Cos(num * 0.01745329f);
        return new Vector3(x, 0f, Mathf.Sin(num * 0.01745329f));
    }

    private Vector3 GETGlobaleFacingVector3(float horizontal, float vertical)
    {
        var num = -GETGlobalFacingDirection(horizontal, vertical) + 90f;
        var x = Mathf.Cos(num * 0.01745329f);
        return new Vector3(x, 0f, Mathf.Sin(num * 0.01745329f));
    }

    private float GETGlobalFacingDirection(float horizontal, float vertical)
    {
        if (vertical == 0f && horizontal == 0f)
        {
            return transform.rotation.eulerAngles.y;
        }
        var y = currentCamera.transform.rotation.eulerAngles.y;
        var num2 = Mathf.Atan2(vertical, horizontal) * 57.29578f;
        num2 = -num2 + 90f;
        return y + num2;
    }

    private float GETLeanAngle(Vector3 p, bool left)
    {
        if (!useGun && State == HERO_STATE.Attack)
        {
            return 0f;
        }
        var num = p.y - transform.position.y;
        var num2 = Vector3.Distance(p, transform.position);
        var a = Mathf.Acos(num / num2) * 57.29578f;
        a *= 0.1f;
        a *= 1f + Mathf.Pow(GetComponent<Rigidbody>().velocity.magnitude, 0.2f);
        var vector3 = p - transform.position;
        var current = Mathf.Atan2(vector3.x, vector3.z) * 57.29578f;
        var target = Mathf.Atan2(GetComponent<Rigidbody>().velocity.x, GetComponent<Rigidbody>().velocity.z) * 57.29578f;
        var num6 = Mathf.DeltaAngle(current, target);
        a += Mathf.Abs(num6 * 0.5f);
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
            return a * (num6 >= 0f ? 1 : (float)-1);
        }
        var num7 = 0f;
        if (left && num6 < 0f || !left && num6 > 0f)
        {
            num7 = 0.1f;
        }
        else
        {
            num7 = 0.5f;
        }
        return a * (num6 >= 0f ? num7 : -num7);
    }

    private void GETOffHorse()
    {
        PlayAnimation("horse_getoff");
        GetComponent<Rigidbody>().AddForce(Vector3.up * 10f - transform.forward * 2f - transform.right * 1f, ForceMode.VelocityChange);
        Unmounted();
    }

    private void getOnHorse()
    {
        PlayAnimation("horse_geton");
        facingDirection = myHorse.transform.rotation.eulerAngles.y;
        targetRotation = Quaternion.Euler(0f, facingDirection, 0f);
    }

    public void GETSupply()
    {
        if ((GetComponent<Animation>().IsPlaying(standAnimation) || GetComponent<Animation>().IsPlaying("run_1") || GetComponent<Animation>().IsPlaying("run_sasha")) && (currentBladeSta != totalBladeSta || currentBladeNum != totalBladeNum || currentGas != totalGas || leftBulletLeft != bulletMAX || rightBulletLeft != bulletMAX))
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
        if (titanForm && erenTitan != null)
        {
            erenTitan.GetComponent<TITAN_EREN>().lifeTime = 0.1f;
        }
        if (!useGun && (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE || photonView.isMine))
        {
            //this.leftbladetrail.Deactivate();
            //this.rightbladetrail.Deactivate();
            //this.leftbladetrail2.Deactivate();
            //this.rightbladetrail2.Deactivate();
        }
        smoke3Dmg.enableEmission = false;
        sparks.enableEmission = false;
    }

    public bool HasDied()
    {
        return hasDied || IsInvincible();
    }

    private void HeadMovement()
    {
        var transform = this.transform.Find("Amarture/Controller_Body/hip/spine/chest/neck/head");
        var transform2 = this.transform.Find("Amarture/Controller_Body/hip/spine/chest/neck");
        var x = Mathf.Sqrt((gunTarget.x - this.transform.position.x) * (gunTarget.x - this.transform.position.x) + (gunTarget.z - this.transform.position.z) * (gunTarget.z - this.transform.position.z));
        targetHeadRotation = transform.rotation;
        var vector5 = gunTarget - this.transform.position;
        var current = -Mathf.Atan2(vector5.z, vector5.x) * 57.29578f;
        var num3 = -Mathf.DeltaAngle(current, this.transform.rotation.eulerAngles.y - 90f);
        num3 = Mathf.Clamp(num3, -40f, 40f);
        var y = transform2.position.y - gunTarget.y;
        var num5 = Mathf.Atan2(y, x) * 57.29578f;
        num5 = Mathf.Clamp(num5, -40f, 30f);
        targetHeadRotation = Quaternion.Euler(transform.rotation.eulerAngles.x + num5, transform.rotation.eulerAngles.y + num3, transform.rotation.eulerAngles.z);
        oldHeadRotation = Quaternion.Lerp(oldHeadRotation, targetHeadRotation, Time.deltaTime * 60f);
        transform.rotation = oldHeadRotation;
    }

    private void HookedByHuman(int hooker, Vector3 hookPosition)
    {
        photonView.RPC(nameof(RPCHookedByHuman), photonView.owner, hooker, hookPosition);
    }

    [PunRPC]
    public void hookFail()
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
        var num = Mathf.Pow(launchForce.magnitude, 0.1f);
        if (grounded)
        {
            GetComponent<Rigidbody>().AddForce(Vector3.up * Mathf.Min(launchForce.magnitude * 0.2f, 10f), ForceMode.Impulse);
        }
        GetComponent<Rigidbody>().AddForce(launchForce * num * 0.1f, ForceMode.Impulse);
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
        LayerMask mask = 1 << LayerMask.NameToLayer("Ground");
        LayerMask mask2 = 1 << LayerMask.NameToLayer("EnemyBox");
        LayerMask mask3 = mask2 | mask;
        return Physics.Raycast(gameObject.transform.position + gameObject.transform.up * 1f, gameObject.transform.forward, 1f, mask3.value);
    }

    private bool IsGrounded()
    {
        LayerMask mask = 1 << LayerMask.NameToLayer("Ground");
        LayerMask mask2 = 1 << LayerMask.NameToLayer("EnemyBox");
        LayerMask mask3 = mask2 | mask;
        return Physics.Raycast(gameObject.transform.position + Vector3.up * 0.1f, -Vector3.up, 0.3f, mask3.value);
    }

    public bool IsInvincible()
    {
        return invincible > 0f;
    }

    private bool IsPressDirectionTowardsHero(float h, float v)
    {
        if (h == 0f && v == 0f)
        {
            return false;
        }
        return Mathf.Abs(Mathf.DeltaAngle(GETGlobalFacingDirection(h, v), transform.rotation.eulerAngles.y)) < 45f;
    }

    private bool IsUpFrontGrounded()
    {
        LayerMask mask = 1 << LayerMask.NameToLayer("Ground");
        LayerMask mask2 = 1 << LayerMask.NameToLayer("EnemyBox");
        LayerMask mask3 = mask2 | mask;
        return Physics.Raycast(gameObject.transform.position + gameObject.transform.up * 3f, gameObject.transform.forward, 1.2f, mask3.value);
    }

    [PunRPC]
    private void killObject()
    {
        Destroy(gameObject);
    }

    public void LateUpdate2()
    {
        if (IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.SINGLE && myNetWorkName != null)
        {
            if (titanForm && erenTitan != null)
            {
                myNetWorkName.transform.localPosition = Vector3.up * Screen.height * 2f;
            }
            var start = new Vector3(baseTransform.position.x, baseTransform.position.y + 2f, baseTransform.position.z);
            var maincamera = this.maincamera;
            LayerMask mask = 1 << LayerMask.NameToLayer("Ground");
            LayerMask mask2 = 1 << LayerMask.NameToLayer("EnemyBox");
            LayerMask mask3 = mask2 | mask;
            if (Vector3.Angle(maincamera.transform.forward, start - maincamera.transform.position) > 90f || Physics.Linecast(start, maincamera.transform.position, mask3))
            {
                myNetWorkName.transform.localPosition = Vector3.up * Screen.height * 2f;
            }
            else
            {
                Vector2 vector2 = this.maincamera.GetComponent<Camera>().WorldToScreenPoint(start);
                myNetWorkName.transform.localPosition = new Vector3((int)(vector2.x - Screen.width * 0.5f), (int)(vector2.y - Screen.height * 0.5f), 0f);
            }
        }
        if (!titanForm && !isCannon)
        {
            if (InputManager.Settings.CameraTilt && (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE || photonView.isMine))
            {
                Quaternion quaternion2;
                var zero = Vector3.zero;
                var position = Vector3.zero;
                if (isLaunchLeft && bulletLeft != null && bulletLeft.GetComponent<Bullet>().isHooked())
                {
                    zero = bulletLeft.transform.position;
                }
                if (isLaunchRight && bulletRight != null && bulletRight.GetComponent<Bullet>().isHooked())
                {
                    position = bulletRight.transform.position;
                }
                var vector5 = Vector3.zero;
                if (zero.magnitude != 0f && position.magnitude == 0f)
                {
                    vector5 = zero;
                }
                else if (zero.magnitude == 0f && position.magnitude != 0f)
                {
                    vector5 = position;
                }
                else if (zero.magnitude != 0f && position.magnitude != 0f)
                {
                    vector5 = (zero + position) * 0.5f;
                }
                var from = Vector3.Project(vector5 - baseTransform.position, maincamera.transform.up);
                var vector7 = Vector3.Project(vector5 - baseTransform.position, maincamera.transform.right);
                if (vector5.magnitude > 0f)
                {
                    var to = from + vector7;
                    var num = Vector3.Angle(vector5 - baseTransform.position, baseRigidBody.velocity) * 0.005f;
                    var vector9 = maincamera.transform.right + vector7.normalized;
                    quaternion2 = Quaternion.Euler(maincamera.transform.rotation.eulerAngles.x, maincamera.transform.rotation.eulerAngles.y, vector9.magnitude >= 1f ? -Vector3.Angle(@from, to) * num : Vector3.Angle(@from, to) * num);
                }
                else
                {
                    quaternion2 = Quaternion.Euler(maincamera.transform.rotation.eulerAngles.x, maincamera.transform.rotation.eulerAngles.y, 0f);
                }
                maincamera.transform.rotation = Quaternion.Lerp(maincamera.transform.rotation, quaternion2, Time.deltaTime * 2f);
            }
            if (State == HERO_STATE.Grab && titanWhoGrabMe != null)
            {
                if (titanWhoGrabMe.GetComponent<MindlessTitan>() != null)
                {
                    baseTransform.position = titanWhoGrabMe.GetComponent<MindlessTitan>().grabTF.transform.position;
                    baseTransform.rotation = titanWhoGrabMe.GetComponent<MindlessTitan>().grabTF.transform.rotation;
                }
                else if (titanWhoGrabMe.GetComponent<FEMALE_TITAN>() != null)
                {
                    baseTransform.position = titanWhoGrabMe.GetComponent<FEMALE_TITAN>().grabTF.transform.position;
                    baseTransform.rotation = titanWhoGrabMe.GetComponent<FEMALE_TITAN>().grabTF.transform.rotation;
                }
            }
            if (useGun)
            {
                if (leftArmAim || rightArmAim)
                {
                    var vector10 = gunTarget - baseTransform.position;
                    var current = -Mathf.Atan2(vector10.z, vector10.x) * 57.29578f;
                    var num3 = -Mathf.DeltaAngle(current, baseTransform.rotation.eulerAngles.y - 90f);
                    HeadMovement();
                    if (!isLeftHandHooked && leftArmAim && num3 < 40f && num3 > -90f)
                    {
                        LeftArmAimTo(gunTarget);
                    }
                    if (!isRightHandHooked && rightArmAim && num3 > -40f && num3 < 90f)
                    {
                        RightArmAimTo(gunTarget);
                    }
                }
                else if (!grounded)
                {
                    handL.localRotation = Quaternion.Euler(90f, 0f, 0f);
                    handR.localRotation = Quaternion.Euler(-90f, 0f, 0f);
                }
                if (isLeftHandHooked && bulletLeft != null)
                {
                    LeftArmAimTo(bulletLeft.transform.position);
                }
                if (isRightHandHooked && bulletRight != null)
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
        var vector = des - transform.position;
        if (left)
        {
            launchPointLeft = des;
        }
        else
        {
            launchPointRight = des;
        }
        vector.Normalize();
        vector = vector * 20f;
        if (bulletLeft != null && bulletRight != null && bulletLeft.GetComponent<Bullet>().isHooked() && bulletRight.GetComponent<Bullet>().isHooked())
        {
            vector = vector * 0.8f;
        }
        if (!GetComponent<Animation>().IsPlaying("attack5") && !GetComponent<Animation>().IsPlaying("special_petra"))
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
                GetComponent<Animation>()["dash"].time = 0f;
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
                launchForce += Vector3.up * (30f - vector.y);
            }
            if (des.y >= transform.position.y)
            {
                launchForce += Vector3.up * (des.y - transform.position.y) * 10f;
            }
            GetComponent<Rigidbody>().AddForce(launchForce);
        }
        facingDirection = Mathf.Atan2(launchForce.x, launchForce.z) * 57.29578f;
        var quaternion = Quaternion.Euler(0f, facingDirection, 0f);
        gameObject.transform.rotation = quaternion;
        GetComponent<Rigidbody>().rotation = quaternion;
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
        if (GetComponent<Animation>().IsPlaying("special_petra"))
        {
            launchElapsedTimeR = -100f;
            launchElapsedTimeL = -100f;
            if (bulletRight != null)
            {
                bulletRight.GetComponent<Bullet>().disable();
                ReleaseIfIHookSb();
            }
            if (bulletLeft != null)
            {
                bulletLeft.GetComponent<Bullet>().disable();
                ReleaseIfIHookSb();
            }
        }
        sparks.enableEmission = false;
    }

    private void LaunchLeftRope(RaycastHit hit, bool single, int mode = 0)
    {
        if (currentGas != 0f)
        {
            UseGas();
            if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
            {
                bulletLeft = (GameObject)Instantiate(Resources.Load("hook"));
            }
            else if (photonView.isMine)
            {
                bulletLeft = PhotonNetwork.Instantiate("hook", transform.position, transform.rotation, 0);
            }
            var obj2 = !useGun ? hookRefL1 : hookRefL2;
            var str = !useGun ? "hookRefL1" : "hookRefL2";
            bulletLeft.transform.position = obj2.transform.position;
            var component = bulletLeft.GetComponent<Bullet>();
            var num = !single ? hit.distance <= 50f ? hit.distance * 0.05f : hit.distance * 0.3f : 0f;
            var vector = hit.point - transform.right * num - bulletLeft.transform.position;
            vector.Normalize();
            if (mode == 1)
            {
                component.launch(vector * 3f, GetComponent<Rigidbody>().velocity, str, true, gameObject, true);
            }
            else
            {
                component.launch(vector * 3f, GetComponent<Rigidbody>().velocity, str, true, gameObject);
            }
            launchPointLeft = Vector3.zero;
        }
    }

    private void LaunchRightRope(RaycastHit hit, bool single, int mode = 0)
    {
        if (currentGas != 0f)
        {
            UseGas();
            if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
            {
                bulletRight = (GameObject)Instantiate(Resources.Load("hook"));
            }
            else if (photonView.isMine)
            {
                bulletRight = PhotonNetwork.Instantiate("hook", transform.position, transform.rotation, 0);
            }
            var obj2 = !useGun ? hookRefR1 : hookRefR2;
            var str = !useGun ? "hookRefR1" : "hookRefR2";
            bulletRight.transform.position = obj2.transform.position;
            var component = bulletRight.GetComponent<Bullet>();
            var num = !single ? hit.distance <= 50f ? hit.distance * 0.05f : hit.distance * 0.3f : 0f;
            var vector = hit.point + transform.right * num - bulletRight.transform.position;
            vector.Normalize();
            if (mode == 1)
            {
                component.launch(vector * 5f, GetComponent<Rigidbody>().velocity, str, false, gameObject, true);
            }
            else
            {
                component.launch(vector * 3f, GetComponent<Rigidbody>().velocity, str, false, gameObject);
            }
            launchPointRight = Vector3.zero;
        }
    }

    private void LeftArmAimTo(Vector3 target)
    {
        var y = target.x - upperarmL.transform.position.x;
        var num2 = target.y - upperarmL.transform.position.y;
        var x = target.z - upperarmL.transform.position.z;
        var num4 = Mathf.Sqrt(y * y + x * x);
        handL.localRotation = Quaternion.Euler(90f, 0f, 0f);
        forearmL.localRotation = Quaternion.Euler(-90f, 0f, 0f);
        upperarmL.rotation = Quaternion.Euler(0f, 90f + Mathf.Atan2(y, x) * 57.29578f, -Mathf.Atan2(num2, num4) * 57.29578f);
    }

    private void Loadskin()
    {
        if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE || photonView.isMine)
        {
            if ((int)FengGameManagerMKII.settings[0x5d] == 1)
            {
                foreach (var renderer in GetComponentsInChildren<Renderer>())
                {
                    if (renderer.name.Contains("speed"))
                    {
                        renderer.enabled = false;
                    }
                }
            }
            if ((int)FengGameManagerMKII.settings[0] == 1)
            {
                var index = 14;
                var num3 = 4;
                var num4 = 5;
                var num5 = 6;
                var num6 = 7;
                var num7 = 8;
                var num8 = 9;
                var num9 = 10;
                var num10 = 11;
                var num11 = 12;
                var num12 = 13;
                var num13 = 3;
                var num14 = 0x5e;
                if ((int)FengGameManagerMKII.settings[0x85] == 1)
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
                else if ((int)FengGameManagerMKII.settings[0x85] == 2)
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
                var str = (string)FengGameManagerMKII.settings[index];
                var str2 = (string)FengGameManagerMKII.settings[num3];
                var str3 = (string)FengGameManagerMKII.settings[num4];
                var str4 = (string)FengGameManagerMKII.settings[num5];
                var str5 = (string)FengGameManagerMKII.settings[num6];
                var str6 = (string)FengGameManagerMKII.settings[num7];
                var str7 = (string)FengGameManagerMKII.settings[num8];
                var str8 = (string)FengGameManagerMKII.settings[num9];
                var str9 = (string)FengGameManagerMKII.settings[num10];
                var str10 = (string)FengGameManagerMKII.settings[num11];
                var str11 = (string)FengGameManagerMKII.settings[num12];
                var str12 = (string)FengGameManagerMKII.settings[num13];
                var str13 = (string)FengGameManagerMKII.settings[num14];
                var url = str12 + "," + str2 + "," + str3 + "," + str4 + "," + str5 + "," + str6 + "," + str7 + "," + str8 + "," + str9 + "," + str10 + "," + str11 + "," + str + "," + str13;
                if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
                {
                    StartCoroutine(LoadskinE(-1, url));
                }
                else
                {
                    var viewID = myHorse?.viewID ?? -1;
                    photonView.RPC(nameof(loadskinRPC), PhotonTargets.AllBuffered, viewID, url);
                }
            }
        }
    }

    public IEnumerator LoadskinE(int horse, string url)
    {
        while (!hasspawn)
        {
            yield return null;
        }

        var iteratorVariable1 = false;
        var mipmap = (int)FengGameManagerMKII.settings[0x3f] != 1;
        var iteratorVariable2 = url.Split(',');
        var iteratorVariable3 = (int)FengGameManagerMKII.settings[15] == 0;
        var iteratorVariable4 = FengGameManagerMKII.Gamemode.Settings.Horse;
        var iteratorVariable5 = IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE || photonView.isMine;
        if (setup.part_hair_1 != null)
        {
            var renderer = setup.part_hair_1.GetComponent<Renderer>();
            if (iteratorVariable2[1].EndsWith(".jpg") || iteratorVariable2[1].EndsWith(".png") || iteratorVariable2[1].EndsWith(".jpeg"))
            {
                if (!FengGameManagerMKII.linkHash[0].ContainsKey(iteratorVariable2[1]))
                {
                    var link = new WWW(iteratorVariable2[1]);
                    yield return link;
                    var iteratorVariable8 = RCextensions.loadimage(link, mipmap, 0x30d40);
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
            var iteratorVariable9 = setup.part_cape.GetComponent<Renderer>();
            if (iteratorVariable2[7].EndsWith(".jpg") || iteratorVariable2[7].EndsWith(".png") || iteratorVariable2[7].EndsWith(".jpeg"))
            {
                if (!FengGameManagerMKII.linkHash[0].ContainsKey(iteratorVariable2[7]))
                {
                    var iteratorVariable10 = new WWW(iteratorVariable2[7]);
                    yield return iteratorVariable10;
                    var iteratorVariable11 = RCextensions.loadimage(iteratorVariable10, mipmap, 0x30d40);
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
            var iteratorVariable12 = setup.part_chest_3.GetComponent<Renderer>();
            if (iteratorVariable2[6].EndsWith(".jpg") || iteratorVariable2[6].EndsWith(".png") || iteratorVariable2[6].EndsWith(".jpeg"))
            {
                if (!FengGameManagerMKII.linkHash[1].ContainsKey(iteratorVariable2[6]))
                {
                    var iteratorVariable13 = new WWW(iteratorVariable2[6]);
                    yield return iteratorVariable13;
                    var iteratorVariable14 = RCextensions.loadimage(iteratorVariable13, mipmap, 0x7a120);
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
        foreach (var iteratorVariable15 in GetComponentsInChildren<Renderer>())
        {
            if (iteratorVariable15.name.Contains(FengGameManagerMKII.s[1]))
            {
                if (iteratorVariable2[1].EndsWith(".jpg") || iteratorVariable2[1].EndsWith(".png") || iteratorVariable2[1].EndsWith(".jpeg"))
                {
                    if (!FengGameManagerMKII.linkHash[0].ContainsKey(iteratorVariable2[1]))
                    {
                        var iteratorVariable16 = new WWW(iteratorVariable2[1]);
                        yield return iteratorVariable16;
                        var iteratorVariable17 = RCextensions.loadimage(iteratorVariable16, mipmap, 0x30d40);
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
                if (iteratorVariable2[2].EndsWith(".jpg") || iteratorVariable2[2].EndsWith(".png") || iteratorVariable2[2].EndsWith(".jpeg"))
                {
                    if (!FengGameManagerMKII.linkHash[0].ContainsKey(iteratorVariable2[2]))
                    {
                        var iteratorVariable18 = new WWW(iteratorVariable2[2]);
                        yield return iteratorVariable18;
                        var iteratorVariable19 = RCextensions.loadimage(iteratorVariable18, mipmap, 0x30d40);
                        iteratorVariable18.Dispose();
                        if (!FengGameManagerMKII.linkHash[0].ContainsKey(iteratorVariable2[2]))
                        {
                            iteratorVariable1 = true;
                            iteratorVariable15.material.mainTextureScale = iteratorVariable15.material.mainTextureScale * 8f;
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
                if (iteratorVariable2[3].EndsWith(".jpg") || iteratorVariable2[3].EndsWith(".png") || iteratorVariable2[3].EndsWith(".jpeg"))
                {
                    if (!FengGameManagerMKII.linkHash[0].ContainsKey(iteratorVariable2[3]))
                    {
                        var iteratorVariable20 = new WWW(iteratorVariable2[3]);
                        yield return iteratorVariable20;
                        var iteratorVariable21 = RCextensions.loadimage(iteratorVariable20, mipmap, 0x30d40);
                        iteratorVariable20.Dispose();
                        if (!FengGameManagerMKII.linkHash[0].ContainsKey(iteratorVariable2[3]))
                        {
                            iteratorVariable1 = true;
                            iteratorVariable15.material.mainTextureScale = iteratorVariable15.material.mainTextureScale * 8f;
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
                if (iteratorVariable2[4].EndsWith(".jpg") || iteratorVariable2[4].EndsWith(".png") || iteratorVariable2[4].EndsWith(".jpeg"))
                {
                    if (!FengGameManagerMKII.linkHash[0].ContainsKey(iteratorVariable2[4]))
                    {
                        var iteratorVariable22 = new WWW(iteratorVariable2[4]);
                        yield return iteratorVariable22;
                        var iteratorVariable23 = RCextensions.loadimage(iteratorVariable22, mipmap, 0x30d40);
                        iteratorVariable22.Dispose();
                        if (!FengGameManagerMKII.linkHash[0].ContainsKey(iteratorVariable2[4]))
                        {
                            iteratorVariable1 = true;
                            iteratorVariable15.material.mainTextureScale = iteratorVariable15.material.mainTextureScale * 8f;
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
            else if (iteratorVariable15.name.Contains(FengGameManagerMKII.s[5]) || iteratorVariable15.name.Contains(FengGameManagerMKII.s[6]) || iteratorVariable15.name.Contains(FengGameManagerMKII.s[10]))
            {
                if (iteratorVariable2[5].EndsWith(".jpg") || iteratorVariable2[5].EndsWith(".png") || iteratorVariable2[5].EndsWith(".jpeg"))
                {
                    if (!FengGameManagerMKII.linkHash[0].ContainsKey(iteratorVariable2[5]))
                    {
                        var iteratorVariable24 = new WWW(iteratorVariable2[5]);
                        yield return iteratorVariable24;
                        var iteratorVariable25 = RCextensions.loadimage(iteratorVariable24, mipmap, 0x30d40);
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
            else if (iteratorVariable15.name.Contains(FengGameManagerMKII.s[7]) || iteratorVariable15.name.Contains(FengGameManagerMKII.s[8]) || iteratorVariable15.name.Contains(FengGameManagerMKII.s[9]) || iteratorVariable15.name.Contains(FengGameManagerMKII.s[0x18]))
            {
                if (iteratorVariable2[6].EndsWith(".jpg") || iteratorVariable2[6].EndsWith(".png") || iteratorVariable2[6].EndsWith(".jpeg"))
                {
                    if (!FengGameManagerMKII.linkHash[1].ContainsKey(iteratorVariable2[6]))
                    {
                        var iteratorVariable26 = new WWW(iteratorVariable2[6]);
                        yield return iteratorVariable26;
                        var iteratorVariable27 = RCextensions.loadimage(iteratorVariable26, mipmap, 0x7a120);
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
                if (iteratorVariable2[7].EndsWith(".jpg") || iteratorVariable2[7].EndsWith(".png") || iteratorVariable2[7].EndsWith(".jpeg"))
                {
                    if (!FengGameManagerMKII.linkHash[0].ContainsKey(iteratorVariable2[7]))
                    {
                        var iteratorVariable28 = new WWW(iteratorVariable2[7]);
                        yield return iteratorVariable28;
                        var iteratorVariable29 = RCextensions.loadimage(iteratorVariable28, mipmap, 0x30d40);
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
            else if (iteratorVariable15.name.Contains(FengGameManagerMKII.s[15]) || (iteratorVariable15.name.Contains(FengGameManagerMKII.s[13]) || iteratorVariable15.name.Contains(FengGameManagerMKII.s[0x1a])) && !iteratorVariable15.name.Contains("_r"))
            {
                if (iteratorVariable2[8].EndsWith(".jpg") || iteratorVariable2[8].EndsWith(".png") || iteratorVariable2[8].EndsWith(".jpeg"))
                {
                    if (!FengGameManagerMKII.linkHash[1].ContainsKey(iteratorVariable2[8]))
                    {
                        var iteratorVariable30 = new WWW(iteratorVariable2[8]);
                        yield return iteratorVariable30;
                        var iteratorVariable31 = RCextensions.loadimage(iteratorVariable30, mipmap, 0x7a120);
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
            else if (iteratorVariable15.name.Contains(FengGameManagerMKII.s[0x11]) || iteratorVariable15.name.Contains(FengGameManagerMKII.s[0x10]) || iteratorVariable15.name.Contains(FengGameManagerMKII.s[0x1a]) && iteratorVariable15.name.Contains("_r"))
            {
                if (iteratorVariable2[9].EndsWith(".jpg") || iteratorVariable2[9].EndsWith(".png") || iteratorVariable2[9].EndsWith(".jpeg"))
                {
                    if (!FengGameManagerMKII.linkHash[1].ContainsKey(iteratorVariable2[9]))
                    {
                        var iteratorVariable32 = new WWW(iteratorVariable2[9]);
                        yield return iteratorVariable32;
                        var iteratorVariable33 = RCextensions.loadimage(iteratorVariable32, mipmap, 0x7a120);
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
            else if (iteratorVariable15.name == FengGameManagerMKII.s[0x12] && iteratorVariable3)
            {
                if (iteratorVariable2[10].EndsWith(".jpg") || iteratorVariable2[10].EndsWith(".png") || iteratorVariable2[10].EndsWith(".jpeg"))
                {
                    if (!FengGameManagerMKII.linkHash[0].ContainsKey(iteratorVariable2[10]))
                    {
                        var iteratorVariable34 = new WWW(iteratorVariable2[10]);
                        yield return iteratorVariable34;
                        var iteratorVariable35 = RCextensions.loadimage(iteratorVariable34, mipmap, 0x30d40);
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
                if (iteratorVariable2[11].EndsWith(".jpg") || iteratorVariable2[11].EndsWith(".png") || iteratorVariable2[11].EndsWith(".jpeg"))
                {
                    if (!FengGameManagerMKII.linkHash[0].ContainsKey(iteratorVariable2[11]))
                    {
                        var iteratorVariable36 = new WWW(iteratorVariable2[11]);
                        yield return iteratorVariable36;
                        var iteratorVariable37 = RCextensions.loadimage(iteratorVariable36, mipmap, 0x30d40);
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
        if (iteratorVariable4 && horse >= 0)
        {
            var gameObject = PhotonView.Find(horse).gameObject;
            if (gameObject != null)
            {
                foreach (var iteratorVariable39 in gameObject.GetComponentsInChildren<Renderer>())
                {
                    if (iteratorVariable39.name.Contains(FengGameManagerMKII.s[0x13]))
                    {
                        if (iteratorVariable2[0].EndsWith(".jpg") || iteratorVariable2[0].EndsWith(".png") || iteratorVariable2[0].EndsWith(".jpeg"))
                        {
                            if (!FengGameManagerMKII.linkHash[1].ContainsKey(iteratorVariable2[0]))
                            {
                                var iteratorVariable40 = new WWW(iteratorVariable2[0]);
                                yield return iteratorVariable40;
                                var iteratorVariable41 = RCextensions.loadimage(iteratorVariable40, mipmap, 0x7a120);
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
        if (iteratorVariable5 && (iteratorVariable2[12].EndsWith(".jpg") || iteratorVariable2[12].EndsWith(".png") || iteratorVariable2[12].EndsWith(".jpeg")))
        {
            if (!FengGameManagerMKII.linkHash[0].ContainsKey(iteratorVariable2[12]))
            {
                var iteratorVariable42 = new WWW(iteratorVariable2[12]);
                yield return iteratorVariable42;
                var iteratorVariable43 = RCextensions.loadimage(iteratorVariable42, mipmap, 0x30d40);
                iteratorVariable42.Dispose();
                if (!FengGameManagerMKII.linkHash[0].ContainsKey(iteratorVariable2[12]))
                {
                    iteratorVariable1 = true;
                    leftbladetrail.MyMaterial.mainTexture = iteratorVariable43;
                    rightbladetrail.MyMaterial.mainTexture = iteratorVariable43;
                    FengGameManagerMKII.linkHash[0].Add(iteratorVariable2[12], leftbladetrail.MyMaterial);
                    leftbladetrail.MyMaterial = (Material)FengGameManagerMKII.linkHash[0][iteratorVariable2[12]];
                    rightbladetrail.MyMaterial = (Material)FengGameManagerMKII.linkHash[0][iteratorVariable2[12]];
                    leftbladetrail2.MyMaterial = leftbladetrail.MyMaterial;
                    rightbladetrail2.MyMaterial = leftbladetrail.MyMaterial;
                }
                else
                {
                    leftbladetrail2.MyMaterial = (Material)FengGameManagerMKII.linkHash[0][iteratorVariable2[12]];
                    rightbladetrail2.MyMaterial = (Material)FengGameManagerMKII.linkHash[0][iteratorVariable2[12]];
                    leftbladetrail.MyMaterial = (Material)FengGameManagerMKII.linkHash[0][iteratorVariable2[12]];
                    rightbladetrail.MyMaterial = (Material)FengGameManagerMKII.linkHash[0][iteratorVariable2[12]];
                }
            }
            else
            {
                leftbladetrail2.MyMaterial = (Material)FengGameManagerMKII.linkHash[0][iteratorVariable2[12]];
                rightbladetrail2.MyMaterial = (Material)FengGameManagerMKII.linkHash[0][iteratorVariable2[12]];
                leftbladetrail.MyMaterial = (Material)FengGameManagerMKII.linkHash[0][iteratorVariable2[12]];
                rightbladetrail.MyMaterial = (Material)FengGameManagerMKII.linkHash[0][iteratorVariable2[12]];
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
        if ((int)FengGameManagerMKII.settings[0] == 1)
        {
            StartCoroutine(LoadskinE(horse, url));
        }
    }

    public void MarkDie()
    {
        hasDied = true;
        State = HERO_STATE.Die;
    }

    [PunRPC]
    public void moveToRPC(float posX, float posY, float posZ, PhotonMessageInfo info)
    {
        if (info.sender.isMasterClient)
        {
            transform.position = new Vector3(posX, posY, posZ);
        }
    }

    [PunRPC]
    private void net3DMGSMOKE(bool ifON)
    {
        if (smoke3Dmg != null)
        {
            smoke3Dmg.enableEmission = ifON;
        }
    }

    [PunRPC]
    private void netContinueAnimation()
    {
        var enumerator = GetComponent<Animation>().GetEnumerator();
        try
        {
            while (enumerator.MoveNext())
            {
                var current = (AnimationState)enumerator.Current;
                if (current != null && current.speed == 1f)
                {
                    return;
                }
                current.speed = 1f;
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
        PlayAnimation(CurrentPlayingClipName());
    }

    [PunRPC]
    private void netCrossFade(string aniName, float time)
    {
        currentAnimation = aniName;
        if (GetComponent<Animation>() != null)
        {
            GetComponent<Animation>().CrossFade(aniName, time);
        }
    }

    [PunRPC]
    public void netDie(Vector3 v, bool isBite, int viewID = -1, string titanName = "", bool killByTitan = true, PhotonMessageInfo info = new PhotonMessageInfo())
    {
        if (photonView.isMine && FengGameManagerMKII.Gamemode.Settings.GamemodeType != GamemodeType.TitanRush)
        {
            if (FengGameManagerMKII.ignoreList.Contains(info.sender.ID))
            {
                photonView.RPC(nameof(backToHumanRPC), PhotonTargets.Others);
                return;
            }
            if (!info.sender.isLocal && !info.sender.isMasterClient)
            {
                if (info.sender.CustomProperties[PhotonPlayerProperty.name] == null || info.sender.CustomProperties[PhotonPlayerProperty.isTitan] == null)
                {
                    FengGameManagerMKII.instance.chatRoom.AddMessage("<color=#FFCC00>Unusual Kill from ID " + info.sender.ID + "</color>");
                }
                else if (viewID < 0)
                {
                    if (titanName == "")
                    {
                        FengGameManagerMKII.instance.chatRoom.AddMessage("<color=#FFCC00>Unusual Kill from ID " + info.sender.ID + " (possibly valid).</color>");
                    }
                    else
                    {
                        FengGameManagerMKII.instance.chatRoom.AddMessage("<color=#FFCC00>Unusual Kill from ID " + info.sender.ID + "</color>");
                    }
                }
                else if (PhotonView.Find(viewID) == null)
                {
                    FengGameManagerMKII.instance.chatRoom.AddMessage("<color=#FFCC00>Unusual Kill from ID " + info.sender.ID + "</color>");
                }
                else if (PhotonView.Find(viewID).owner.ID != info.sender.ID)
                {
                    FengGameManagerMKII.instance.chatRoom.AddMessage("<color=#FFCC00>Unusual Kill from ID " + info.sender.ID + "</color>");
                }
            }
        }
        if (PhotonNetwork.isMasterClient)
        {
            var iD = photonView.owner.ID;
            if (FengGameManagerMKII.heroHash.ContainsKey(iD))
            {
                FengGameManagerMKII.heroHash.Remove(iD);
            }
        }
        if (photonView.isMine)
        {
            var vector = Vector3.up * 5000f;
            if (myBomb != null)
            {
                myBomb.destroyMe();
            }
            if (titanForm && erenTitan != null)
            {
                erenTitan.GetComponent<TITAN_EREN>().lifeTime = 0.1f;
            }
            if (skillCd != null)
            {
                skillCd.transform.localPosition = vector;
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
        meatDie.Play();
        if (!(useGun || IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.SINGLE && !photonView.isMine))
        {
            //TODO: Re-enable these again
            //this.leftbladetrail.Deactivate();
            //this.rightbladetrail.Deactivate();
            //this.leftbladetrail2.Deactivate();
            //this.rightbladetrail2.Deactivate();
        }
        FalseAttack();
        BreakApart2(v, isBite);
        if (photonView.isMine)
        {
            currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().setSpectorMode(false);
            currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().gameOver = true;
            FengGameManagerMKII.instance.myRespawnTime = 0f;
        }
        hasDied = true;
        var transform = this.transform.Find("audio_die");
        if (transform != null)
        {
            transform.parent = null;
            transform.GetComponent<AudioSource>().Play();
        }

        if (photonView.isMine)
        {
            PhotonNetwork.RemoveRPCs(photonView);
            var propertiesToSet = new Hashtable();
            propertiesToSet.Add(PhotonPlayerProperty.dead, true);
            PhotonNetwork.player.SetCustomProperties(propertiesToSet);
            propertiesToSet = new Hashtable();
            propertiesToSet.Add(PhotonPlayerProperty.deaths, RCextensions.returnIntFromObject(PhotonNetwork.player.CustomProperties[PhotonPlayerProperty.deaths]) + 1);
            PhotonNetwork.player.SetCustomProperties(propertiesToSet);
            var id = string.IsNullOrEmpty(titanName) ? 0 : 1;
            FengGameManagerMKII.instance.photonView.RPC(nameof(FengGameManagerMKII.instance.someOneIsDead), PhotonTargets.MasterClient, id);
            if (viewID != -1)
            {
                var view2 = PhotonView.Find(viewID);
                if (view2 != null)
                {
                    FengGameManagerMKII.instance.sendKillInfo(killByTitan, $"<color=#ffc000>[{info.sender.ID}]</color> " + RCextensions.returnStringFromObject(view2.owner.CustomProperties[PhotonPlayerProperty.name]), false, RCextensions.returnStringFromObject(PhotonNetwork.player.CustomProperties[PhotonPlayerProperty.name]));
                    propertiesToSet = new Hashtable();
                    propertiesToSet.Add(PhotonPlayerProperty.kills, RCextensions.returnIntFromObject(view2.owner.CustomProperties[PhotonPlayerProperty.kills]) + 1);
                    view2.owner.SetCustomProperties(propertiesToSet);
                }
            }
            else
            {
                FengGameManagerMKII.instance.sendKillInfo(!(titanName == string.Empty), $"<color=#ffc000>[{info.sender.ID}]</color> " + titanName, false, RCextensions.returnStringFromObject(PhotonNetwork.player.CustomProperties[PhotonPlayerProperty.name]));
            }
        }
        if (photonView.isMine)
        {
            PhotonNetwork.Destroy(photonView);
        }
        
        HeroDied?.Invoke(this);
    }

    [PunRPC]
    public void netDie2(int viewID = -1, string titanName = "", PhotonMessageInfo info = new PhotonMessageInfo())
    {
        GameObject obj2;
        if (photonView.isMine && FengGameManagerMKII.Gamemode.Settings.GamemodeType != GamemodeType.TitanRush)
        {
            if (FengGameManagerMKII.ignoreList.Contains(info.sender.ID))
            {
                photonView.RPC(nameof(backToHumanRPC), PhotonTargets.Others);
                return;
            }
            if (!info.sender.isLocal && !info.sender.isMasterClient)
            {
                if (info.sender.CustomProperties[PhotonPlayerProperty.name] == null || info.sender.CustomProperties[PhotonPlayerProperty.isTitan] == null)
                {
                    FengGameManagerMKII.instance.chatRoom.AddMessage("<color=#FFCC00>Unusual Kill from ID " + info.sender.ID + "</color>");
                }
                else if (viewID < 0)
                {
                    if (titanName == "")
                    {
                        FengGameManagerMKII.instance.chatRoom.AddMessage("<color=#FFCC00>Unusual Kill from ID " + info.sender.ID + " (possibly valid).</color>");
                    }
                    else if (FengGameManagerMKII.Gamemode.Settings.PvPBomb && !FengGameManagerMKII.Gamemode.Settings.PvpCannons)
                    {
                        FengGameManagerMKII.instance.chatRoom.AddMessage("<color=#FFCC00>Unusual Kill from ID " + info.sender.ID + "</color>");
                    }
                }
                else if (PhotonView.Find(viewID) == null)
                {
                    FengGameManagerMKII.instance.chatRoom.AddMessage("<color=#FFCC00>Unusual Kill from ID " + info.sender.ID + "</color>");
                }
                else if (PhotonView.Find(viewID).owner.ID != info.sender.ID)
                {
                    FengGameManagerMKII.instance.chatRoom.AddMessage("<color=#FFCC00>Unusual Kill from ID " + info.sender.ID + "</color>");
                }
            }
        }
        if (photonView.isMine)
        {
            var vector = Vector3.up * 5000f;
            if (myBomb != null)
            {
                myBomb.destroyMe();
            }
            PhotonNetwork.RemoveRPCs(photonView);
            if (titanForm && erenTitan != null)
            {
                erenTitan.GetComponent<TITAN_EREN>().lifeTime = 0.1f;
            }
            if (skillCd != null)
            {
                skillCd.transform.localPosition = vector;
            }
        }
        meatDie.Play();
        if (bulletLeft != null)
        {
            bulletLeft.GetComponent<Bullet>().removeMe();
        }
        if (bulletRight != null)
        {
            bulletRight.GetComponent<Bullet>().removeMe();
        }
        var transform = this.transform.Find("audio_die");
        transform.parent = null;
        transform.GetComponent<AudioSource>().Play();
        if (photonView.isMine)
        {
            currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().setMainObject(null);
            currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().setSpectorMode(true);
            currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().gameOver = true;
            FengGameManagerMKII.instance.myRespawnTime = 0f;
        }
        FalseAttack();
        hasDied = true;

        if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER && photonView.isMine)
        {
            PhotonNetwork.RemoveRPCs(photonView);
            var propertiesToSet = new Hashtable();
            propertiesToSet.Add(PhotonPlayerProperty.dead, true);
            PhotonNetwork.player.SetCustomProperties(propertiesToSet);
            propertiesToSet = new Hashtable();
            propertiesToSet.Add(PhotonPlayerProperty.deaths, (int)PhotonNetwork.player.CustomProperties[PhotonPlayerProperty.deaths] + 1);
            PhotonNetwork.player.SetCustomProperties(propertiesToSet);
            if (viewID != -1)
            {
                var view2 = PhotonView.Find(viewID);
                if (view2 != null)
                {
                    FengGameManagerMKII.instance.sendKillInfo(true, $"<color=#ffc000>[{info.sender.ID}]</color> " + RCextensions.returnStringFromObject(view2.owner.CustomProperties[PhotonPlayerProperty.name]), false, RCextensions.returnStringFromObject(PhotonNetwork.player.CustomProperties[PhotonPlayerProperty.name]));
                    propertiesToSet = new Hashtable();
                    propertiesToSet.Add(PhotonPlayerProperty.kills, RCextensions.returnIntFromObject(view2.owner.CustomProperties[PhotonPlayerProperty.kills]) + 1);
                    view2.owner.SetCustomProperties(propertiesToSet);
                }
            }
            else
            {
                FengGameManagerMKII.instance.sendKillInfo(true, $"<color=#ffc000>[{info.sender.ID}]</color> " + titanName, false, RCextensions.returnStringFromObject(PhotonNetwork.player.CustomProperties[PhotonPlayerProperty.name]));
            }
            var id = string.IsNullOrEmpty(titanName) ? 0 : 1;
            FengGameManagerMKII.instance.photonView.RPC(nameof(FengGameManagerMKII.instance.someOneIsDead), PhotonTargets.MasterClient, id);
        }
        if (IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.SINGLE && photonView.isMine)
        {
            obj2 = PhotonNetwork.Instantiate("hitMeat2", this.transform.position, Quaternion.Euler(270f, 0f, 0f), 0);
        }
        else
        {
            obj2 = (GameObject)Instantiate(Resources.Load("hitMeat2"));
        }
        obj2.transform.position = this.transform.position;
        if (photonView.isMine)
        {
            PhotonNetwork.Destroy(photonView);
        }
        if (PhotonNetwork.isMasterClient)
        {
            var iD = photonView.owner.ID;
            if (FengGameManagerMKII.heroHash.ContainsKey(iD))
            {
                FengGameManagerMKII.heroHash.Remove(iD);
            }
        }
        
        HeroDied?.Invoke(this);
    }

    public void NetDieLocal(Vector3 v, bool isBite, int viewID = -1, string titanName = "", bool killByTitan = true)
    {
        if (photonView.isMine)
        {
            var vector = Vector3.up * 5000f;
            if (titanForm && erenTitan != null)
            {
                erenTitan.GetComponent<TITAN_EREN>().lifeTime = 0.1f;
            }
            if (myBomb != null)
            {
                myBomb.destroyMe();
            }
            if (skillCd != null)
            {
                skillCd.transform.localPosition = vector;
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
        meatDie.Play();
        if (!(useGun || IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.SINGLE && !photonView.isMine))
        {
            leftbladetrail.Deactivate();
            rightbladetrail.Deactivate();
            leftbladetrail2.Deactivate();
            rightbladetrail2.Deactivate();
        }
        FalseAttack();
        BreakApart2(v, isBite);
        if (photonView.isMine)
        {
            currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().setSpectorMode(false);
            currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().gameOver = true;
            FengGameManagerMKII.instance.myRespawnTime = 0f;
        }
        hasDied = true;
        var transform = this.transform.Find("audio_die");
        transform.parent = null;
        transform.GetComponent<AudioSource>().Play();

        if (photonView.isMine)
        {
            PhotonNetwork.RemoveRPCs(photonView);
            var propertiesToSet = new Hashtable();
            propertiesToSet.Add(PhotonPlayerProperty.dead, true);
            PhotonNetwork.player.SetCustomProperties(propertiesToSet);
            propertiesToSet = new Hashtable();
            propertiesToSet.Add(PhotonPlayerProperty.deaths, RCextensions.returnIntFromObject(PhotonNetwork.player.CustomProperties[PhotonPlayerProperty.deaths]) + 1);
            PhotonNetwork.player.SetCustomProperties(propertiesToSet);
            var id = string.IsNullOrEmpty(titanName) ? 0 : 1;
            FengGameManagerMKII.instance.photonView.RPC(nameof(FengGameManagerMKII.instance.someOneIsDead), PhotonTargets.MasterClient, id);
            if (viewID != -1)
            {
                var view = PhotonView.Find(viewID);
                if (view != null)
                {
                    FengGameManagerMKII.instance.sendKillInfo(killByTitan, RCextensions.returnStringFromObject(view.owner.CustomProperties[PhotonPlayerProperty.name]), false, RCextensions.returnStringFromObject(PhotonNetwork.player.CustomProperties[PhotonPlayerProperty.name]));
                    propertiesToSet = new Hashtable();
                    propertiesToSet.Add(PhotonPlayerProperty.kills, RCextensions.returnIntFromObject(view.owner.CustomProperties[PhotonPlayerProperty.kills]) + 1);
                    view.owner.SetCustomProperties(propertiesToSet);
                }
            }
            else
            {
                FengGameManagerMKII.instance.sendKillInfo(!(titanName == string.Empty), titanName, false, RCextensions.returnStringFromObject(PhotonNetwork.player.CustomProperties[PhotonPlayerProperty.name]));
            }
        }
        if (photonView.isMine)
        {
            PhotonNetwork.Destroy(photonView);
        }
        if (PhotonNetwork.isMasterClient)
        {
            var iD = photonView.owner.ID;
            if (FengGameManagerMKII.heroHash.ContainsKey(iD))
            {
                FengGameManagerMKII.heroHash.Remove(iD);
            }
        }

        HeroDied?.Invoke(this);
    }

    [PunRPC]
    public void netGrabbed(int id, bool leftHand)
    {
        titanWhoGrabMeID = id;
        Grabbed(PhotonView.Find(id).gameObject, leftHand);
    }

    [PunRPC]
    private void netlaughAttack()
    {
        throw new NotImplementedException("Titan laugh attack is not implemented yet");
        //foreach (GameObject obj2 in GameObject.FindGameObjectsWithTag("titan"))
        //{
        //    if (((Vector3.Distance(obj2.transform.position, base.transform.position) < 50f) && (Vector3.Angle(obj2.transform.forward, base.transform.position - obj2.transform.position) < 90f)) && (obj2.GetComponent<TITAN>() != null))
        //    {
        //        obj2.GetComponent<TITAN>().beLaughAttacked();
        //    }
        //}
    }

    [PunRPC]
    public void netPauseAnimation()
    {
        var enumerator = GetComponent<Animation>().GetEnumerator();
        try
        {
            while (enumerator.MoveNext())
            {
                var current = (AnimationState)enumerator.Current;
                current.speed = 0f;
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
    }

    [PunRPC]
    public void netPlayAnimation(string aniName)
    {
        currentAnimation = aniName;
        if (GetComponent<Animation>() != null)
        {
            GetComponent<Animation>().Play(aniName);
        }
    }

    [PunRPC]
    public void netPlayAnimationAt(string aniName, float normalizedTime)
    {
        currentAnimation = aniName;
        if (GetComponent<Animation>() != null)
        {
            GetComponent<Animation>().Play(aniName);
            GetComponent<Animation>()[aniName].normalizedTime = normalizedTime;
        }
    }

    [PunRPC]
    public void netSetIsGrabbedFalse()
    {
        State = HERO_STATE.Idle;
    }

    [PunRPC]
    public void netTauntAttack(float tauntTime, float distance = 100f)
    {
        throw new NotImplementedException("Titan taunt behavior is not yet implemented");
        //foreach (GameObject obj2 in GameObject.FindGameObjectsWithTag("titan"))
        //{
        //    if ((Vector3.Distance(obj2.transform.position, base.transform.position) < distance) && (obj2.GetComponent<TITAN>() != null))
        //    {
        //        obj2.GetComponent<TITAN>().beTauntedBy(base.gameObject, tauntTime);
        //    }
        //}
    }

    [PunRPC]
    public void netUngrabbed()
    {
        Ungrabbed();
        netPlayAnimation(standAnimation);
        FalseAttack();
    }
    
    private void OnDestroy()
    {
        if (myNetWorkName != null)
        {
            Destroy(myNetWorkName);
        }
        if (gunDummy != null)
        {
            Destroy(gunDummy);
        }
        if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER)
        {
            ReleaseIfIHookSb();
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

    private void PauseAnimation()
    {
        var enumerator = GetComponent<Animation>().GetEnumerator();
        try
        {
            while (enumerator.MoveNext())
            {
                var current = (AnimationState)enumerator.Current;
                if (current != null)
                    current.speed = 0f;
            }
        }
        finally
        {
            (enumerator as IDisposable)?.Dispose();
        }
        if (IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.SINGLE && photonView.isMine)
        {
            photonView.RPC(nameof(netPauseAnimation), PhotonTargets.Others);
        }
    }

    private void PlayAnimation(string aniName)
    {
        currentAnimation = aniName;
        GetComponent<Animation>().Play(aniName);
        if (PhotonNetwork.connected && photonView.isMine)
            photonView.RPC(nameof(netPlayAnimation), PhotonTargets.Others, aniName);
    }

    private void PlayAnimationAt(string aniName, float normalizedTime)
    {
        currentAnimation = aniName;
        GetComponent<Animation>().Play(aniName);
        GetComponent<Animation>()[aniName].normalizedTime = normalizedTime;
        if (PhotonNetwork.connected && photonView.isMine)
            photonView.RPC(nameof(netPlayAnimationAt), PhotonTargets.Others, aniName, normalizedTime);
    }

    private void ReleaseIfIHookSb()
    {
        if (hookSomeOne && hookTarget != null)
        {
            hookTarget.GetPhotonView().RPC(nameof(badGuyReleaseMe), hookTarget.GetPhotonView().owner);
            hookTarget = null;
            hookSomeOne = false;
        }
    }

    private IEnumerator ReloadSky()
    {
        yield return new WaitForSeconds(0.5f);
        if (FengGameManagerMKII.skyMaterial != null && Camera.main.GetComponent<Skybox>().material != FengGameManagerMKII.skyMaterial)
        {
            Camera.main.GetComponent<Skybox>().material = FengGameManagerMKII.skyMaterial;
        }
    }

    private void ResetAnimationSpeed()
    {
        var enumerator = GetComponent<Animation>().GetEnumerator();
        try
        {
            while (enumerator.MoveNext())
            {
                var current = (AnimationState)enumerator.Current;
                if (current != null)
                    current.speed = 1f;
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
        CustomAnimationSpeed();
    }

    private void RightArmAimTo(Vector3 target)
    {
        var y = target.x - upperarmR.transform.position.x;
        var num2 = target.y - upperarmR.transform.position.y;
        var x = target.z - upperarmR.transform.position.z;
        var num4 = Mathf.Sqrt(y * y + x * x);
        handR.localRotation = Quaternion.Euler(-90f, 0f, 0f);
        forearmR.localRotation = Quaternion.Euler(90f, 0f, 0f);
        upperarmR.rotation = Quaternion.Euler(180f, 90f + Mathf.Atan2(y, x) * 57.29578f, Mathf.Atan2(num2, num4) * 57.29578f);
    }

    [PunRPC]
    private void RPCHookedByHuman(int hooker, Vector3 hookPosition)
    {
        hookBySomeOne = true;
        badGuy = PhotonView.Find(hooker).gameObject;
        if (Vector3.Distance(hookPosition, transform.position) < 15f)
        {
            launchForce = PhotonView.Find(hooker).gameObject.transform.position - transform.position;
            GetComponent<Rigidbody>().AddForce(-GetComponent<Rigidbody>().velocity * 0.9f, ForceMode.VelocityChange);
            var num = Mathf.Pow(launchForce.magnitude, 0.1f);
            if (grounded)
            {
                GetComponent<Rigidbody>().AddForce(Vector3.up * Mathf.Min(launchForce.magnitude * 0.2f, 10f), ForceMode.Impulse);
            }
            GetComponent<Rigidbody>().AddForce(launchForce * num * 0.1f, ForceMode.Impulse);
            if (State != HERO_STATE.Grab)
            {
                dashTime = 1f;
                CrossFade("dash", 0.05f);
                GetComponent<Animation>()["dash"].time = 0.1f;
                State = HERO_STATE.AirDodge;
                FalseAttack();
                facingDirection = Mathf.Atan2(launchForce.x, launchForce.z) * 57.29578f;
                var quaternion = Quaternion.Euler(0f, facingDirection, 0f);
                gameObject.transform.rotation = quaternion;
                GetComponent<Rigidbody>().rotation = quaternion;
                targetRotation = quaternion;
            }
        }
        else
        {
            hookBySomeOne = false;
            badGuy = null;
            PhotonView.Find(hooker).RPC("hookFail", PhotonView.Find(hooker).owner);
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
            if (bulletLeft != null && bulletRight != null)
            {
                var normal = bulletLeft.transform.position - bulletRight.transform.position;
                if (normal.sqrMagnitude < 4f)
                {
                    var vector2 = (bulletLeft.transform.position + bulletRight.transform.position) * 0.5f - transform.position;
                    facingDirection = Mathf.Atan2(vector2.x, vector2.z) * 57.29578f;
                    if (useGun && State != HERO_STATE.Attack)
                    {
                        var current = -Mathf.Atan2(GetComponent<Rigidbody>().velocity.z, GetComponent<Rigidbody>().velocity.x) * 57.29578f;
                        var target = -Mathf.Atan2(vector2.z, vector2.x) * 57.29578f;
                        var num3 = -Mathf.DeltaAngle(current, target);
                        facingDirection += num3;
                    }
                    almostSingleHook = true;
                }
                else
                {
                    var to = transform.position - bulletLeft.transform.position;
                    var vector6 = transform.position - bulletRight.transform.position;
                    var vector7 = (bulletLeft.transform.position + bulletRight.transform.position) * 0.5f;
                    var from = transform.position - vector7;
                    if (Vector3.Angle(@from, to) < 30f && Vector3.Angle(@from, vector6) < 30f)
                    {
                        almostSingleHook = true;
                        var vector9 = vector7 - transform.position;
                        facingDirection = Mathf.Atan2(vector9.x, vector9.z) * 57.29578f;
                    }
                    else
                    {
                        almostSingleHook = false;
                        var forward = transform.forward;
                        Vector3.OrthoNormalize(ref normal, ref forward);
                        facingDirection = Mathf.Atan2(forward.x, forward.z) * 57.29578f;
                        var num4 = Mathf.Atan2(to.x, to.z) * 57.29578f;
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
            var zero = Vector3.zero;
            if (isRightHandHooked && bulletRight != null)
            {
                zero = bulletRight.transform.position - transform.position;
            }
            else
            {
                if (!isLeftHandHooked || bulletLeft == null)
                {
                    return;
                }
                zero = bulletLeft.transform.position - transform.position;
            }
            facingDirection = Mathf.Atan2(zero.x, zero.z) * 57.29578f;
            if (State != HERO_STATE.Attack)
            {
                var num6 = -Mathf.Atan2(GetComponent<Rigidbody>().velocity.z, GetComponent<Rigidbody>().velocity.x) * 57.29578f;
                var num7 = -Mathf.Atan2(zero.z, zero.x) * 57.29578f;
                var num8 = -Mathf.DeltaAngle(num6, num7);
                if (useGun)
                {
                    facingDirection += num8;
                }
                else
                {
                    var num9 = 0f;
                    if (isLeftHandHooked && num8 < 0f || isRightHandHooked && num8 > 0f)
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
    public void SetMyPhotonCamera(float offset, PhotonMessageInfo info)
    {
        if (photonView.owner == info.sender)
        {
            cameraMultiplier = offset;
            isPhotonCamera = true;
        }
    }

    [PunRPC]
    private void setMyTeam(int val)
    {
        myTeam = val;
        checkBoxLeft.GetComponent<TriggerColliderWeapon>().myTeam = val;
        checkBoxRight.GetComponent<TriggerColliderWeapon>().myTeam = val;
        if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER && PhotonNetwork.isMasterClient)
        {
            object[] objArray;
            //TODO: Sync these upon gamemode syncSettings
            if (FengGameManagerMKII.Gamemode.Settings.Pvp == PvpMode.AhssVsBlades)
            {
                var num = 0;
                if (photonView.owner.CustomProperties[PhotonPlayerProperty.RCteam] != null)
                {
                    num = RCextensions.returnIntFromObject(photonView.owner.CustomProperties[PhotonPlayerProperty.RCteam]);
                }
                if (val != num)
                {
                    photonView.RPC(nameof(setMyTeam), PhotonTargets.AllBuffered, num);
                }
            }
            else if (FengGameManagerMKII.Gamemode.Settings.Pvp == PvpMode.FreeForAll && val != photonView.owner.ID)
            {
                photonView.RPC(nameof(setMyTeam), PhotonTargets.AllBuffered, photonView.owner.ID);
            }
        }
    }

    public void SetSkillHudPosition()
    {
        skillCd = GameObject.Find("skill_cd_" + skillId);
        if (skillCd != null)
        {
            skillCd.transform.localPosition = GameObject.Find("skill_cd_bottom").transform.localPosition;
        }
        if (useGun)
        {
            skillCd.transform.localPosition = Vector3.up * 5000f;
        }
    }

    public void SetSkillHudPosition2()
    {
        return;
        skillCd = GameObject.Find("skill_cd_" + skillIdhud);
        if (skillCd != null)
        {
            skillCd.transform.localPosition = GameObject.Find("skill_cd_bottom").transform.localPosition;
        }
        if (useGun && FengGameManagerMKII.Gamemode.Settings.PvPBomb)
        {
            skillCd.transform.localPosition = Vector3.up * 5000f;
        }
    }

    public void SetStat2()
    {
        skillCdLast = 1.5f;
        skillId = setup.myCostume.stat.skillId;
        if (skillId == "levi")
        {
            skillCdLast = 3.5f;
        }
        CustomAnimationSpeed();
        if (skillId == "armin")
        {
            skillCdLast = 5f;
        }
        if (skillId == "marco")
        {
            skillCdLast = 10f;
        }
        if (skillId == "jean")
        {
            skillCdLast = 0.001f;
        }
        if (skillId == "eren")
        {
            skillCdLast = 120f;
            if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER)
            {
                if (!FengGameManagerMKII.Gamemode.Settings.PlayerShifters)
                {
                    skillId = "petra";
                    skillCdLast = 1f;
                }
                else
                {
                    var num = 0;
                    foreach (var player in PhotonNetwork.playerList)
                    {
                        if (RCextensions.returnIntFromObject(player.CustomProperties[PhotonPlayerProperty.isTitan]) == 1 && RCextensions.returnStringFromObject(player.CustomProperties[PhotonPlayerProperty.character]).ToUpper() == "EREN")
                        {
                            num++;
                        }
                    }
                    if (num > 1)
                    {
                        skillId = "petra";
                        skillCdLast = 1f;
                    }
                }
            }
        }
        if (skillId == "sasha")
        {
            skillCdLast = 20f;
        }
        if (skillId == "petra")
        {
            skillCdLast = 3.5f;
        }
        BombInit();
        speed = setup.myCostume.stat.SPD / 10f;
        totalGas = currentGas = setup.myCostume.stat.GAS;
        totalBladeSta = currentBladeSta = setup.myCostume.stat.BLA;
        baseRigidBody.mass = 0.5f - (setup.myCostume.stat.ACL - 100) * 0.001f;
        //GameObject.Find("skill_cd_bottom").transform.localPosition = new Vector3(0f, (-Screen.height * 0.5f) + 5f, 0f);
        //this.skillCD = GameObject.Find("skill_cd_" + this.skillIDHUD);
        //this.skillCD.transform.localPosition = GameObject.Find("skill_cd_bottom").transform.localPosition;
        //GameObject.Find("GasUI").transform.localPosition = GameObject.Find("skill_cd_bottom").transform.localPosition;
        if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE || photonView.isMine)
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
        if (equipmentType == EquipmentType.Ahss)
        {
            standAnimation = "AHSS_stand_gun";
            useGun = true;
            gunDummy = new GameObject();
            gunDummy.name = "gunDummy";
            gunDummy.transform.position = baseTransform.position;
            gunDummy.transform.rotation = baseTransform.rotation;
            myGroup = GROUP.A;
            SetTeam2(2);
            if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE || photonView.isMine)
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
        setMyTeam(team);
        if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER && photonView.isMine)
        {
            photonView.RPC(nameof(setMyTeam), PhotonTargets.OthersBuffered, team);
            var propertiesToSet = new Hashtable();
            propertiesToSet.Add(PhotonPlayerProperty.team, team);
            PhotonNetwork.player.SetCustomProperties(propertiesToSet);
        }
    }

    private void SetTeam2(int team)
    {
        if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER && photonView.isMine)
        {
            photonView.RPC(nameof(setMyTeam), PhotonTargets.AllBuffered, team);
            var propertiesToSet = new Hashtable();
            propertiesToSet.Add(PhotonPlayerProperty.team, team);
            PhotonNetwork.player.SetCustomProperties(propertiesToSet);
        }
        else
        {
            setMyTeam(team);
        }
    }

    private void ShootFlare(int type)
    {
        var flag = false;
        if (type == 1 && flare1Cd == 0f)
        {
            flare1Cd = flareTotalCd;
            flag = true;
        }
        if (type == 2 && flare2Cd == 0f)
        {
            flare2Cd = flareTotalCd;
            flag = true;
        }
        if (type == 3 && flare3Cd == 0f)
        {
            flare3Cd = flareTotalCd;
            flag = true;
        }
        if (flag)
        {
            if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
            {
                var obj2 = (GameObject)Instantiate(Resources.Load("FX/flareBullet" + type), transform.position, transform.rotation);
                obj2.GetComponent<FlareMovement>().dontShowHint();
                Destroy(obj2, 25f);
            }
            else
            {
                PhotonNetwork.Instantiate("FX/flareBullet" + type, transform.position, transform.rotation, 0).GetComponent<FlareMovement>().dontShowHint();
            }
        }
    }

    private void ShowAimUI2()
    {
        Vector3 vector;
        if (MenuManager.IsMenuOpen)
        {
            var cross1 = this.cross1;
            var cross2 = this.cross2;
            var crossL1 = this.crossL1;
            var crossL2 = this.crossL2;
            var crossR1 = this.crossR1;
            var crossR2 = this.crossR2;
            var labelDistance = this.labelDistance;
            vector = Vector3.up * 10000f;
            crossR2.transform.localPosition = vector;
            crossR1.transform.localPosition = vector;
            crossL2.transform.localPosition = vector;
            crossL1.transform.localPosition = vector;
            labelDistance.gameObject.transform.localPosition = vector;
            cross2.transform.localPosition = vector;
            cross1.transform.localPosition = vector;
        }
        else
        {
            RaycastHit hit;
            CheckTitan();
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            LayerMask mask = 1 << LayerMask.NameToLayer("Ground");
            LayerMask mask2 = 1 << LayerMask.NameToLayer("EnemyBox");
            LayerMask mask3 = mask2 | mask;
            if (Physics.Raycast(ray, out hit, 1E+07f, mask3.value))
            {
                RaycastHit hit2;
                var cross1 = this.cross1;
                var cross2 = this.cross2;
                cross1.transform.localPosition = Input.mousePosition;
                var transform = cross1.transform;
                transform.localPosition -= new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 0f);
                cross2.transform.localPosition = cross1.transform.localPosition;
                vector = hit.point - baseTransform.position;
                var magnitude = vector.magnitude;
                var str = magnitude <= 1000f ? ((int)magnitude).ToString() : "???";
                if ((int)FengGameManagerMKII.settings[0xbd] == 1)
                {
                    str = str + "\n" + currentSpeed.ToString("F1") + " u/s";
                }
                else if ((int)FengGameManagerMKII.settings[0xbd] == 2)
                {
                    str = str + "\n" + (currentSpeed / 100f).ToString("F1") + "K";
                }
                labelDistance.text = str;
                if (magnitude > 120f)
                {
                    cross1.transform.localPosition += Vector3.up * 10000f;
                    labelDistance.gameObject.transform.localPosition = cross2.transform.localPosition;
                }
                else
                {
                    cross2.transform.localPosition += Vector3.up * 10000f;
                    labelDistance.gameObject.transform.localPosition = cross1.transform.localPosition;
                }
                var transform13 = labelDistance.gameObject.transform;
                transform13.localPosition -= new Vector3(0f, 15f, 0f);
                var vector2 = new Vector3(0f, 0.4f, 0f);
                vector2 -= baseTransform.right * 0.3f;
                var vector3 = new Vector3(0f, 0.4f, 0f);
                vector3 += baseTransform.right * 0.3f;
                var num4 = hit.distance <= 50f ? hit.distance * 0.05f : hit.distance * 0.3f;
                var vector4 = hit.point - baseTransform.right * num4 - (baseTransform.position + vector2);
                var vector5 = hit.point + baseTransform.right * num4 - (baseTransform.position + vector3);
                vector4.Normalize();
                vector5.Normalize();
                vector4 = vector4 * 1000000f;
                vector5 = vector5 * 1000000f;
                if (Physics.Linecast(baseTransform.position + vector2, baseTransform.position + vector2 + vector4, out hit2, mask3.value))
                {
                    crossL1.transform.localPosition = currentCamera.WorldToScreenPoint(hit2.point);
                    crossL1.transform.localPosition -= new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 0f);
                    crossL1.transform.localRotation = Quaternion.Euler(0f, 0f, Mathf.Atan2(crossL1.transform.localPosition.y - (Input.mousePosition.y - Screen.height * 0.5f), crossL1.transform.localPosition.x - (Input.mousePosition.x - Screen.width * 0.5f)) * 57.29578f + 180f);
                    crossL2.transform.localPosition = crossL1.transform.localPosition;
                    crossL2.transform.localRotation = crossL1.transform.localRotation;
                    if (hit2.distance > 120f)
                        crossL1.transform.localPosition += Vector3.up * 10000f;
                    else
                        crossL2.transform.localPosition += Vector3.up * 10000f;
                }
                if (Physics.Linecast(baseTransform.position + vector3, baseTransform.position + vector3 + vector5, out hit2, mask3.value))
                {
                    crossR1.transform.localPosition = currentCamera.WorldToScreenPoint(hit2.point);
                    crossR1.transform.localPosition -= new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 0f);
                    crossR1.transform.localRotation = Quaternion.Euler(0f, 0f, Mathf.Atan2(crossR1.transform.localPosition.y - (Input.mousePosition.y - Screen.height * 0.5f), crossR1.transform.localPosition.x - (Input.mousePosition.x - Screen.width * 0.5f)) * 57.29578f);
                    crossR2.transform.localPosition = crossR1.transform.localPosition;
                    crossR2.transform.localRotation = crossR1.transform.localRotation;
                    if (hit2.distance > 120f)
                        crossR1.transform.localPosition += Vector3.up * 10000f;
                    else
                        crossR2.transform.localPosition += Vector3.up * 10000f;
                }
            }
        }
    }

    private void ShowFlareCd()
    {
        if (GameObject.Find("UIflare1") != null)
        {
            //GameObject.Find("UIflare1").GetComponent<UISprite>().fillAmount = (this.flareTotalCD - this.flare1CD) / this.flareTotalCD;
            //GameObject.Find("UIflare2").GetComponent<UISprite>().fillAmount = (this.flareTotalCD - this.flare2CD) / this.flareTotalCD;
            //GameObject.Find("UIflare3").GetComponent<UISprite>().fillAmount = (this.flareTotalCD - this.flare3CD) / this.flareTotalCD;
        }
    }

    private void ShowFlareCd2()
    {
        if (CachedSprites["UIflare1"] != null)
        {
            CachedSprites["UIflare1"].fillAmount = (flareTotalCd - flare1Cd) / flareTotalCd;
            CachedSprites["UIflare2"].fillAmount = (flareTotalCd - flare2Cd) / flareTotalCd;
            CachedSprites["UIflare3"].fillAmount = (flareTotalCd - flare3Cd) / flareTotalCd;
        }
    }

    private void ShowGas2()
    {
        var num = currentGas / totalGas;
        var num2 = currentBladeSta / totalBladeSta;
        CachedSprites["GasLeft"].fillAmount = CachedSprites["GasRight"].fillAmount = currentGas / totalGas;
        if (num <= 0.25f)
        {
            CachedSprites["GasLeft"].color = CachedSprites["GasRight"].color = Color.red;
        }
        else if (num < 0.5f)
        {
            CachedSprites["GasLeft"].color = CachedSprites["GasRight"].color = Color.yellow;
        }
        else
        {
            CachedSprites["GasLeft"].color = CachedSprites["GasRight"].color = Color.white;
        }
        Equipment.Weapon.UpdateSupplyUi(inGameUI);
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
        var target = GameObject.Find("LabelScore");
        if (target != null)
        {
            speed = Mathf.Max(10f, speed);
            //target.GetComponent<UILabel>().text = this.speed.ToString();
            target.transform.localScale = Vector3.zero;
            speed = (int)(speed * 0.1f);
            speed = Mathf.Clamp(speed, 40f, 150f);
            iTween.Stop(target);
            object[] args = { "x", speed, "y", speed, "z", speed, "easetype", iTween.EaseType.easeOutElastic, "time", 1f };
            iTween.ScaleTo(target, iTween.Hash(args));
            object[] objArray2 = { "x", 0, "y", 0, "z", 0, "easetype", iTween.EaseType.easeInBounce, "time", 0.5f, "delay", 2f };
            iTween.ScaleTo(target, iTween.Hash(objArray2));
        }
    }

    private void ShowSkillCd()
    {
        if (skillCd != null)
        {
            //this.skillCD.GetComponent<UISprite>().fillAmount = (this.skillCDLast - this.skillCDDuration) / this.skillCDLast;
        }
    }

    /// <summary>
    /// Called locally.
    /// </summary>
    public void OnMountingCannon(GameObject cameraFocus = null)
    {
        Debug.Assert(photonView.isMine, $"{nameof(OnMountingCannon)} must be called on the local player.");
        Debug.Assert(!isCannon, "Can't mount cannon while using a cannon.");

        // TODO: Improve this.
        if (cameraFocus)
            Camera.main.GetComponent<IN_GAME_MAIN_CAMERA>().setMainObject(cameraFocus);
        Camera.main.fieldOfView = 55f;
        
        PrepareForCannon();
        photonView.RPC(nameof(MountCannonRPC), PhotonTargets.AllBuffered);
    }

    public void OnUnmountingCannon()
    {
        // TODO: Improve this.
        isCannon = false;
        Camera.main.GetComponent<IN_GAME_MAIN_CAMERA>().setMainObject(gameObject);
        baseRigidBody.velocity = Vector3.zero;
        photonView.RPC(nameof(ReturnFromCannon), PhotonTargets.Others);
        skillCdLast = skillCdLastCannon;
        skillCdDuration = skillCdLast;
    }

    /// <summary>
    /// Called on all clients.
    /// </summary>
    [PunRPC]
    private void MountCannonRPC(PhotonMessageInfo info)
    {
        Debug.Assert(info.sender == photonView.owner, $"{nameof(MountCannonRPC)} was called by non-owner.");

        isCannon = true;
    }
    
    [PunRPC]
    public void ReturnFromCannon(PhotonMessageInfo info)
    {
        Debug.Assert(info.sender == photonView.owner, $"{nameof(ReturnFromCannon)} was called by non-owner.");
        isCannon = false;
    }


    private void PrepareForCannon()
    {
        if (myHorse && isMounted)
            GETOffHorse();

        Idle();

        if (bulletLeft != null)
            bulletLeft.GetComponent<Bullet>().removeMe();

        if (bulletRight != null)
            bulletRight.GetComponent<Bullet>().removeMe();

        if (smoke3Dmg.enableEmission && IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.SINGLE && photonView.isMine)
            photonView.RPC(nameof(net3DMGSMOKE), PhotonTargets.Others, false);

        smoke3Dmg.enableEmission = false;

        GetComponent<Rigidbody>().velocity = Vector3.zero;
    }

    public void SetHorse()
    {
        if (IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.MULTIPLAYER || !photonView.isMine) return;
        if (FengGameManagerMKII.Gamemode.Settings.Horse && myHorse == null)
        {
            var position = baseTransform.position + Vector3.up * 5f;
            var rotation = baseTransform.rotation;
            myHorse = Horse.Create(this, position, rotation);
        }

        if (!FengGameManagerMKII.Gamemode.Settings.Horse && myHorse != null)
        {
            PhotonNetwork.Destroy(myHorse);
        }
    }

    private void Start()
    {
        FengGameManagerMKII.instance.addHero(this);
        if (photonView.isMine)
            gameObject.AddComponent<InteractionManager>();
        SetHorse();
        sparks = baseTransform.Find("slideSparks").GetComponent<ParticleSystem>();
        smoke3Dmg = baseTransform.Find("3dmg_smoke").GetComponent<ParticleSystem>();
        baseTransform.localScale = new Vector3(myScale, myScale, myScale);
        facingDirection = baseTransform.rotation.eulerAngles.y;
        targetRotation = Quaternion.Euler(0f, facingDirection, 0f);
        smoke3Dmg.enableEmission = false;
        sparks.enableEmission = false;
        //HACK
        //this.speedFXPS = this.speedFX1.GetComponent<ParticleSystem>();
        //this.speedFXPS.enableEmission = false;
        if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER)
        {
            if (PhotonNetwork.isMasterClient)
            {
                var iD = photonView.owner.ID;
                if (FengGameManagerMKII.heroHash.ContainsKey(iD))
                {
                    FengGameManagerMKII.heroHash[iD] = this;
                }
                else
                {
                    FengGameManagerMKII.heroHash.Add(iD, this);
                }
            }
            playerName.GetComponent<TextMesh>().text = FengGameManagerMKII.instance.name;
            ////HACK
            ////GameObject obj2 = GameObject.Find("UI_IN_GAME");
            //this.myNetWorkName = (GameObject) UnityEngine.Object.Instantiate(Resources.Load("UI/LabelNameOverHead"));
            //this.myNetWorkName.name = "LabelNameOverHead";
            ////HACK
            ////this.myNetWorkName.transform.parent = obj2.GetComponent<UIReferArray>().panels[0].transform;
            //this.myNetWorkName.transform.localScale = new Vector3(14f, 14f, 14f);
            //this.myNetWorkName.GetComponent<UILabel>().text = string.Empty;
            if (photonView.isMine)
            {
                photonView.RPC(nameof(SetMyPhotonCamera), PhotonTargets.OthersBuffered, PlayerPrefs.GetFloat("cameraDistance") + 0.3f);
            }
            else
            {
                var flag2 = false;
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
        if (IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.SINGLE && !photonView.isMine)
        {
            gameObject.layer = LayerMask.NameToLayer("NetworkObject");
            if (IN_GAME_MAIN_CAMERA.dayLight == DayLight.Night)
            {
                var obj3 = (GameObject)Instantiate(Resources.Load("flashlight"));
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
            Destroy(leftbladetrail);
            Destroy(rightbladetrail);
            Destroy(leftbladetrail2);
            Destroy(rightbladetrail2);
            hasspawn = true;
        }
        else
        {
            currentCamera = GameObject.Find("MainCamera").GetComponent<Camera>();
            Loadskin();
            hasspawn = true;
            StartCoroutine(ReloadSky());
        }
        bombImmune = false;
        if (FengGameManagerMKII.Gamemode.Settings.PvPBomb)
        {
            bombImmune = true;
            StartCoroutine(StopImmunity());
        }
    }

    public IEnumerator StopImmunity()
    {
        yield return new WaitForSeconds(5f);
        bombImmune = false;
    }

    private void Suicide()
    {
    }

    private void Suicide2()
    {
        if (IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.SINGLE)
        {
            NetDieLocal(GetComponent<Rigidbody>().velocity * 50f, false, -1, string.Empty);
            FengGameManagerMKII.instance.needChooseSide = true;
            FengGameManagerMKII.instance.justSuicide = true;
        }
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

    public void Update2()
    {
        if (!IN_GAME_MAIN_CAMERA.isPausing)
        {
            if (invincible > 0f)
            {
                invincible -= Time.deltaTime;
            }
            if (!hasDied)
            {
                if (titanForm && erenTitan != null)
                {
                    baseTransform.position = erenTitan.transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/neck").position;
                }

                if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE || photonView.isMine)
                {
                    if (State == HERO_STATE.Grab && !useGun)
                    {
                        if (skillId == "jean")
                        {
                            if (State != HERO_STATE.Attack &&
                                (InputManager.KeyDown(InputHuman.Attack) ||
                                 InputManager.KeyDown(InputHuman.AttackSpecial)) && escapeTimes > 0 && !baseAnimation.IsPlaying("grabbed_jean"))
                            {
                                PlayAnimation("grabbed_jean");
                                baseAnimation["grabbed_jean"].time = 0f;
                                escapeTimes--;
                            }
                            if (baseAnimation.IsPlaying("grabbed_jean") && baseAnimation["grabbed_jean"].normalizedTime > 0.64f && titanWhoGrabMe.GetComponent<MindlessTitan>() != null)
                            {
                                Ungrabbed();
                                baseRigidBody.velocity = Vector3.up * 30f;
                                var titan = titanWhoGrabMe.GetComponent<MindlessTitan>();
                                if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
                                {
                                    titan.GrabEscapeRpc();
                                }
                                else
                                {
                                    photonView.RPC(nameof(netSetIsGrabbedFalse), PhotonTargets.All);
                                    if (PhotonNetwork.isMasterClient)
                                        titan.GrabEscapeRpc();
                                    else
                                        titan.photonView.RPC(nameof(titan.GrabEscapeRpc), PhotonTargets.MasterClient);
                                }
                            }
                        }
                        else if (skillId == "eren")
                        {
                            ShowSkillCd();
                            if (IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.SINGLE || IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE && !IN_GAME_MAIN_CAMERA.isPausing)
                            {
                                CalcSkillCd();
                                CalcFlareCd();
                            }
                            if (InputManager.KeyDown(InputHuman.AttackSpecial))
                            {
                                var flag2 = false;
                                if (skillCdDuration > 0f || flag2)
                                {
                                    flag2 = true;
                                }
                                else
                                {
                                    skillCdDuration = skillCdLast;
                                    if (skillId == "eren" && titanWhoGrabMe.GetComponent<MindlessTitan>() != null)
                                    {
                                        var titan = titanWhoGrabMe.GetComponent<MindlessTitan>();
                                        Ungrabbed();
                                        if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
                                        {
                                            titan.GrabEscapeRpc();
                                        }
                                        else
                                        {
                                            photonView.RPC(nameof(netSetIsGrabbedFalse), PhotonTargets.All);
                                            if (PhotonNetwork.isMasterClient)
                                                titan.GrabEscapeRpc();
                                            else
                                                titan.photonView.RPC(nameof(titan.GrabEscapeRpc), PhotonTargets.MasterClient);
                                        }
                                        
                                        ErenTransform();
                                    }
                                }
                            }
                        }
                    }
                    else if (!titanForm && !isCannon)
                    {
                        Boolean reflectorVariable2;
                        Boolean reflectorVariable1;
                        Boolean reflectorVariable0;
                        BufferUpdate();
                        UpdateExt();
                        if (!grounded && State != HERO_STATE.AirDodge)
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
                        if (grounded && (State == HERO_STATE.Idle || State == HERO_STATE.Slide))
                        {
                            if (!(!InputManager.KeyDown(InputHuman.Jump) || baseAnimation.IsPlaying("jump") || baseAnimation.IsPlaying("horse_geton")))
                            {
                                Idle();
                                CrossFade("jump", 0.1f);
                                sparks.enableEmission = false;
                            }
                            if (!(!InputManager.KeyDown(InputHorse.Mount) || baseAnimation.IsPlaying("jump") || baseAnimation.IsPlaying("horse_geton")) && myHorse != null && !isMounted && Vector3.Distance(myHorse.transform.position, transform.position) < 15f)
                            {
                                getOnHorse();
                            }
                            if (!(!InputManager.KeyDown(InputHuman.Dodge) || baseAnimation.IsPlaying("jump") || baseAnimation.IsPlaying("horse_geton")))
                            {
                                Dodge2();
                                return;
                            }
                        }
                        if (State == HERO_STATE.Idle)
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
                            if (InputManager.KeyDown(InputUi.Restart))
                            {
                                Suicide2();
                            }
                            if (myHorse != null && isMounted && InputManager.KeyDown(InputHorse.Mount))
                            {
                                GETOffHorse();
                            }
                            if ((GetComponent<Animation>().IsPlaying(standAnimation) || !grounded) && InputManager.KeyDown(InputHuman.Reload) && (!useGun || FengGameManagerMKII.Gamemode.Settings.AhssAirReload || grounded))
                            {
                                ChangeBlade();
                                return;
                            }
                            if (baseAnimation.IsPlaying(standAnimation) && InputManager.KeyDown(InputHuman.Salute))
                            {
                                Salute();
                                return;
                            }
                            if (!isMounted && (InputManager.KeyDown(InputHuman.Attack) || InputManager.KeyDown(InputHuman.AttackSpecial)) && !useGun)
                            {
                                var flag3 = false;
                                if (InputManager.KeyDown(InputHuman.AttackSpecial))
                                {
                                    if (skillCdDuration > 0f || flag3)
                                    {
                                        flag3 = true;
                                    }
                                    else
                                    {
                                        skillCdDuration = skillCdLast;
                                        if (skillId == "eren")
                                        {
                                            ErenTransform();
                                            return;
                                        }
                                        if (skillId == "marco")
                                        {
                                            if (IsGrounded())
                                            {
                                                attackAnimation = Random.Range(0, 2) != 0 ? "special_marco_1" : "special_marco_0";
                                                PlayAnimation(attackAnimation);
                                            }
                                            else
                                            {
                                                flag3 = true;
                                                skillCdDuration = 0f;
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
                                                skillCdDuration = 0f;
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
                                                skillCdDuration = 0f;
                                            }
                                        }
                                        else if (skillId == "mikasa")
                                        {
                                            attackAnimation = "attack3_1";
                                            PlayAnimation("attack3_1");
                                            baseRigidBody.velocity = Vector3.up * 10f;
                                        }
                                        else if (skillId == "levi")
                                        {
                                            RaycastHit hit;
                                            attackAnimation = "attack5";
                                            PlayAnimation("attack5");
                                            baseRigidBody.velocity += Vector3.up * 5f;
                                            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                                            LayerMask mask = 1 << LayerMask.NameToLayer("Ground");
                                            LayerMask mask2 = 1 << LayerMask.NameToLayer("EnemyBox");
                                            LayerMask mask3 = mask2 | mask;
                                            if (Physics.Raycast(ray, out hit, 1E+07f, mask3.value))
                                            {
                                                if (bulletRight != null)
                                                {
                                                    bulletRight.GetComponent<Bullet>().disable();
                                                    ReleaseIfIHookSb();
                                                }
                                                dashDirection = hit.point - baseTransform.position;
                                                LaunchRightRope(hit, true, 1);
                                                rope.Play();
                                            }
                                            facingDirection = Mathf.Atan2(dashDirection.x, dashDirection.z) * 57.29578f;
                                            targetRotation = Quaternion.Euler(0f, facingDirection, 0f);
                                            attackLoop = 3;
                                        }
                                        else if (skillId == "petra")
                                        {
                                            RaycastHit hit2;
                                            attackAnimation = "special_petra";
                                            PlayAnimation("special_petra");
                                            baseRigidBody.velocity += Vector3.up * 5f;
                                            var ray2 = Camera.main.ScreenPointToRay(Input.mousePosition);
                                            LayerMask mask4 = 1 << LayerMask.NameToLayer("Ground");
                                            LayerMask mask5 = 1 << LayerMask.NameToLayer("EnemyBox");
                                            LayerMask mask6 = mask5 | mask4;
                                            if (Physics.Raycast(ray2, out hit2, 1E+07f, mask6.value))
                                            {
                                                if (bulletRight != null)
                                                {
                                                    bulletRight.GetComponent<Bullet>().disable();
                                                    ReleaseIfIHookSb();
                                                }
                                                if (bulletLeft != null)
                                                {
                                                    bulletLeft.GetComponent<Bullet>().disable();
                                                    ReleaseIfIHookSb();
                                                }
                                                dashDirection = hit2.point - baseTransform.position;
                                                LaunchLeftRope(hit2, true);
                                                LaunchRightRope(hit2, true);
                                                rope.Play();
                                            }
                                            facingDirection = Mathf.Atan2(dashDirection.x, dashDirection.z) * 57.29578f;
                                            targetRotation = Quaternion.Euler(0f, facingDirection, 0f);
                                            attackLoop = 3;
                                        }
                                        else
                                        {
                                            if (needLean)
                                            {
                                                if (leanLeft)
                                                {
                                                    attackAnimation = Random.Range(0, 100) >= 50 ? "attack1_hook_l1" : "attack1_hook_l2";
                                                }
                                                else
                                                {
                                                    attackAnimation = Random.Range(0, 100) >= 50 ? "attack1_hook_r1" : "attack1_hook_r2";
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
                                            attackAnimation = Random.Range(0, 100) >= 50 ? "attack1_hook_l1" : "attack1_hook_l2";
                                        }
                                        else if (InputManager.Key(InputHuman.Right))
                                        {
                                            attackAnimation = Random.Range(0, 100) >= 50 ? "attack1_hook_r1" : "attack1_hook_r2";
                                        }
                                        else if (leanLeft)
                                        {
                                            attackAnimation = Random.Range(0, 100) >= 50 ? "attack1_hook_l1" : "attack1_hook_l2";
                                        }
                                        else
                                        {
                                            attackAnimation = Random.Range(0, 100) >= 50 ? "attack1_hook_r1" : "attack1_hook_r2";
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
                                    else if (bulletLeft != null && bulletLeft.transform.parent != null)
                                    {
                                        var a = bulletLeft.transform.parent.transform.root.Find("Amarture/Core/Controller_Body/hip/spine/chest/neck");
                                        if (a != null)
                                        {
                                            AttackAccordingToTarget(a);
                                        }
                                        else
                                        {
                                            AttackAccordingToMouse();
                                        }
                                    }
                                    else if (bulletRight != null && bulletRight.transform.parent != null)
                                    {
                                        var transform2 = bulletRight.transform.parent.transform.root.Find("Amarture/Core/Controller_Body/hip/spine/chest/neck");
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
                                        var obj2 = FindNearestTitan();
                                        if (obj2 != null)
                                        {
                                            var transform3 = obj2.transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/neck");
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
                                    checkBoxLeft.GetComponent<TriggerColliderWeapon>().clearHits();
                                    checkBoxRight.GetComponent<TriggerColliderWeapon>().clearHits();
                                    if (grounded)
                                    {
                                        baseRigidBody.AddForce(gameObject.transform.forward * 200f);
                                    }
                                    PlayAnimation(attackAnimation);
                                    baseAnimation[attackAnimation].time = 0f;
                                    buttonAttackRelease = false;
                                    State = HERO_STATE.Attack;
                                    if (grounded || attackAnimation == "attack3_1" || attackAnimation == "attack5" || attackAnimation == "special_petra")
                                    {
                                        attackReleased = true;
                                        buttonAttackRelease = true;
                                    }
                                    else
                                    {
                                        attackReleased = false;
                                    }
                                    sparks.enableEmission = false;
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
                                    var ray3 = Camera.main.ScreenPointToRay(Input.mousePosition);
                                    LayerMask mask7 = 1 << LayerMask.NameToLayer("Ground");
                                    LayerMask mask8 = 1 << LayerMask.NameToLayer("EnemyBox");
                                    LayerMask mask9 = mask8 | mask7;
                                    if (Physics.Raycast(ray3, out hit3, 1E+07f, mask9.value))
                                    {
                                        gunTarget = hit3.point;
                                    }
                                }
                                var flag4 = false;
                                var flag5 = false;
                                var flag6 = false;
                                if (InputManager.KeyUp(InputHuman.AttackSpecial) && skillId != "bomb")
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
                                else if (flag5 && (grounded || FengGameManagerMKII.Gamemode.Settings.AhssAirReload))
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
                                if (attackAnimation == "attack3_1" && currentBladeSta > 0f)
                                {
                                    if (baseAnimation[attackAnimation].normalizedTime >= 0.8f)
                                    {
                                        if (!checkBoxLeft.GetComponent<TriggerColliderWeapon>().active_me)
                                        {
                                            checkBoxLeft.GetComponent<TriggerColliderWeapon>().active_me = true;
                                            if ((int)FengGameManagerMKII.settings[0x5c] == 0)
                                            {
                                                leftbladetrail2.Activate();
                                                rightbladetrail2.Activate();
                                                leftbladetrail.Activate();
                                                rightbladetrail.Activate();
                                            }
                                            baseRigidBody.velocity = -Vector3.up * 30f;
                                        }
                                        if (!checkBoxRight.GetComponent<TriggerColliderWeapon>().active_me)
                                        {
                                            checkBoxRight.GetComponent<TriggerColliderWeapon>().active_me = true;
                                            slash.Play();
                                        }
                                    }
                                    else if (checkBoxLeft.GetComponent<TriggerColliderWeapon>().active_me)
                                    {
                                        checkBoxLeft.GetComponent<TriggerColliderWeapon>().active_me = false;
                                        checkBoxRight.GetComponent<TriggerColliderWeapon>().active_me = false;
                                        checkBoxLeft.GetComponent<TriggerColliderWeapon>().clearHits();
                                        checkBoxRight.GetComponent<TriggerColliderWeapon>().clearHits();
                                        leftbladetrail.StopSmoothly(0.1f);
                                        rightbladetrail.StopSmoothly(0.1f);
                                        leftbladetrail2.StopSmoothly(0.1f);
                                        rightbladetrail2.StopSmoothly(0.1f);
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
                                    if (baseAnimation[attackAnimation].normalizedTime > num2 && baseAnimation[attackAnimation].normalizedTime < num)
                                    {
                                        if (!checkBoxLeft.GetComponent<TriggerColliderWeapon>().active_me)
                                        {
                                            checkBoxLeft.GetComponent<TriggerColliderWeapon>().active_me = true;
                                            slash.Play();
                                            if ((int)FengGameManagerMKII.settings[0x5c] == 0)
                                            {
                                                //this.leftbladetrail2.Activate();
                                                //this.rightbladetrail2.Activate();
                                                //this.leftbladetrail.Activate();
                                                //this.rightbladetrail.Activate();
                                            }
                                        }
                                        if (!checkBoxRight.GetComponent<TriggerColliderWeapon>().active_me)
                                        {
                                            checkBoxRight.GetComponent<TriggerColliderWeapon>().active_me = true;
                                        }
                                    }
                                    else if (checkBoxLeft.GetComponent<TriggerColliderWeapon>().active_me)
                                    {
                                        checkBoxLeft.GetComponent<TriggerColliderWeapon>().active_me = false;
                                        checkBoxRight.GetComponent<TriggerColliderWeapon>().active_me = false;
                                        checkBoxLeft.GetComponent<TriggerColliderWeapon>().clearHits();
                                        checkBoxRight.GetComponent<TriggerColliderWeapon>().clearHits();
                                        //this.leftbladetrail2.StopSmoothly(0.1f);
                                        //this.rightbladetrail2.StopSmoothly(0.1f);
                                        //this.leftbladetrail.StopSmoothly(0.1f);
                                        //this.rightbladetrail.StopSmoothly(0.1f);
                                    }
                                    if (attackLoop > 0 && baseAnimation[attackAnimation].normalizedTime > num)
                                    {
                                        attackLoop--;
                                        PlayAnimationAt(attackAnimation, num2);
                                    }
                                }
                                if (baseAnimation[attackAnimation].normalizedTime >= 1f)
                                {
                                    if (attackAnimation == "special_marco_0" || attackAnimation == "special_marco_1")
                                    {
                                        if (IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.SINGLE)
                                        {
                                            if (!PhotonNetwork.isMasterClient)
                                                photonView.RPC(nameof(netTauntAttack), PhotonTargets.MasterClient, 5f, 100f);
                                            else
                                                netTauntAttack(5f);
                                        }
                                        else
                                        {
                                            netTauntAttack(5f);
                                        }
                                        FalseAttack();
                                        Idle();
                                    }
                                    else if (attackAnimation == "special_armin")
                                    {
                                        if (IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.SINGLE)
                                        {
                                            if (!PhotonNetwork.isMasterClient)
                                            {
                                                photonView.RPC(nameof(netlaughAttack), PhotonTargets.MasterClient);
                                            }
                                            else
                                            {
                                                netlaughAttack();
                                            }
                                        }
                                        else
                                        {
                                            foreach (var obj3 in GameObject.FindGameObjectsWithTag("titan"))
                                            {
                                                //if (((Vector3.Distance(obj3.transform.position, this.baseTransform.position) < 50f) && (Vector3.Angle(obj3.transform.forward, this.baseTransform.position - obj3.transform.position) < 90f)) && (obj3.GetComponent<TITAN>() != null))
                                                //{
                                                //    obj3.GetComponent<TITAN>().beLaughAttacked();
                                                //}
                                            }
                                        }
                                        FalseAttack();
                                        Idle();
                                    }
                                    else if (attackAnimation == "attack3_1")
                                    {
                                        baseRigidBody.velocity -= Vector3.up * Time.deltaTime * 30f;
                                    }
                                    else
                                    {
                                        FalseAttack();
                                        Idle();
                                    }
                                }
                                if (baseAnimation.IsPlaying("attack3_2") && baseAnimation["attack3_2"].normalizedTime >= 1f)
                                {
                                    FalseAttack();
                                    Idle();
                                }
                            }
                            else
                            {
                                checkBoxLeft.GetComponent<TriggerColliderWeapon>().active_me = false;
                                checkBoxRight.GetComponent<TriggerColliderWeapon>().active_me = false;
                                baseTransform.rotation = Quaternion.Lerp(baseTransform.rotation, gunDummy.transform.rotation, Time.deltaTime * 30f);
                                if (!attackReleased && baseAnimation[attackAnimation].normalizedTime > 0.167f)
                                {
                                    GameObject obj4;
                                    attackReleased = true;
                                    var flag7 = false;
                                    if (attackAnimation == "AHSS_shoot_both" || attackAnimation == "AHSS_shoot_both_air")
                                    {
                                        //Should use AHSSShotgunCollider instead of TriggerColliderWeapon.  
                                        //Apply that change when abstracting weapons from this class.
                                        //Note, when doing the abstraction, the relationship between the weapon collider and the abstracted weapon class should be carefully considered.
                                        checkBoxLeft.GetComponent<TriggerColliderWeapon>().active_me = true;
                                        checkBoxRight.GetComponent<TriggerColliderWeapon>().active_me = true;
                                        flag7 = true;
                                        leftGunHasBullet = false;
                                        rightGunHasBullet = false;
                                        baseRigidBody.AddForce(-baseTransform.forward * 1000f, ForceMode.Acceleration);
                                    }
                                    else
                                    {
                                        if (attackAnimation == "AHSS_shoot_l" || attackAnimation == "AHSS_shoot_l_air")
                                        {
                                            checkBoxLeft.GetComponent<TriggerColliderWeapon>().active_me = true;
                                            leftGunHasBullet = false;
                                        }
                                        else
                                        {
                                            checkBoxRight.GetComponent<TriggerColliderWeapon>().active_me = true;
                                            rightGunHasBullet = false;
                                        }
                                        baseRigidBody.AddForce(-baseTransform.forward * 600f, ForceMode.Acceleration);
                                    }
                                    baseRigidBody.AddForce(Vector3.up * 200f, ForceMode.Acceleration);
                                    var prefabName = "FX/shotGun";
                                    if (flag7)
                                    {
                                        prefabName = "FX/shotGun 1";
                                    }
                                    if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER && photonView.isMine)
                                    {
                                        obj4 = PhotonNetwork.Instantiate(prefabName, baseTransform.position + baseTransform.up * 0.8f - baseTransform.right * 0.1f, baseTransform.rotation, 0);
                                        if (obj4.GetComponent<EnemyfxIDcontainer>() != null)
                                        {
                                            obj4.GetComponent<EnemyfxIDcontainer>().myOwnerViewID = photonView.viewID;
                                        }
                                    }
                                    else
                                    {
                                        obj4 = (GameObject)Instantiate(Resources.Load(prefabName), baseTransform.position + baseTransform.up * 0.8f - baseTransform.right * 0.1f, baseTransform.rotation);
                                    }
                                }
                                if (baseAnimation[attackAnimation].normalizedTime >= 1f)
                                {
                                    FalseAttack();
                                    Idle();
                                    checkBoxLeft.GetComponent<TriggerColliderWeapon>().active_me = false;
                                    checkBoxRight.GetComponent<TriggerColliderWeapon>().active_me = false;
                                }
                                if (!baseAnimation.IsPlaying(attackAnimation))
                                {
                                    FalseAttack();
                                    Idle();
                                    checkBoxLeft.GetComponent<TriggerColliderWeapon>().active_me = false;
                                    checkBoxRight.GetComponent<TriggerColliderWeapon>().active_me = false;
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
                                if (!(grounded || baseAnimation["dodge"].normalizedTime <= 0.6f))
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
                            if (baseAnimation.IsPlaying("dash_land") && baseAnimation["dash_land"].normalizedTime >= 1f)
                            {
                                Idle();
                            }
                        }
                        else if (State == HERO_STATE.FillGas)
                        {
                            if (baseAnimation.IsPlaying("supply") && baseAnimation["supply"].normalizedTime >= 1f)
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
                                if (currentSpeed > originVm)
                                {
                                    baseRigidBody.AddForce(-baseRigidBody.velocity * Time.deltaTime * 1.7f, ForceMode.VelocityChange);
                                }
                            }
                            else
                            {
                                dashTime = 0f;
                                Idle();
                            }
                        }
                        reflectorVariable0 = InputManager.Key(InputHuman.HookLeft);
                        if (!(!reflectorVariable0 || (baseAnimation.IsPlaying("attack3_1") || baseAnimation.IsPlaying("attack5") || baseAnimation.IsPlaying("special_petra") || State == HERO_STATE.Grab) && State != HERO_STATE.Idle))
                        {
                            if (bulletLeft != null)
                            {
                                qHold = true;
                            }
                            else
                            {
                                RaycastHit hit4;
                                var ray4 = Camera.main.ScreenPointToRay(Input.mousePosition);
                                LayerMask mask10 = 1 << LayerMask.NameToLayer("Ground");
                                LayerMask mask11 = 1 << LayerMask.NameToLayer("EnemyBox");
                                LayerMask mask12 = mask11 | mask10;
                                if (Physics.Raycast(ray4, out hit4, 10000f, mask12.value))
                                {
                                    LaunchLeftRope(hit4, true);
                                    rope.Play();
                                }
                            }
                        }
                        else
                        {
                            qHold = false;
                        }
                        if (InputManager.Key(InputHuman.HookRight))
                        {
                            reflectorVariable1 = true;
                        }
                        else
                        {
                            reflectorVariable1 = false;
                        }
                        if (!(reflectorVariable1 ? baseAnimation.IsPlaying("attack3_1") || baseAnimation.IsPlaying("attack5") || baseAnimation.IsPlaying("special_petra") || State == HERO_STATE.Grab ? State != HERO_STATE.Idle : false : true))
                        {
                            if (bulletRight != null)
                            {
                                eHold = true;
                            }
                            else
                            {
                                RaycastHit hit5;
                                var ray5 = Camera.main.ScreenPointToRay(Input.mousePosition);
                                LayerMask mask13 = 1 << LayerMask.NameToLayer("Ground");
                                LayerMask mask14 = 1 << LayerMask.NameToLayer("EnemyBox");
                                LayerMask mask15 = mask14 | mask13;
                                if (Physics.Raycast(ray5, out hit5, 10000f, mask15.value))
                                {
                                    LaunchRightRope(hit5, true);
                                    rope.Play();
                                }
                            }
                        }
                        else
                        {
                            eHold = false;
                        }
                        if (InputManager.Key(InputHuman.HookBoth))
                        {
                            reflectorVariable2 = true;
                        }
                        else
                        {
                            reflectorVariable2 = false;
                        }
                        if (!(reflectorVariable2 ? baseAnimation.IsPlaying("attack3_1") || baseAnimation.IsPlaying("attack5") || baseAnimation.IsPlaying("special_petra") || State == HERO_STATE.Grab ? State != HERO_STATE.Idle : false : true))
                        {
                            qHold = true;
                            eHold = true;
                            if (bulletLeft == null && bulletRight == null)
                            {
                                RaycastHit hit6;
                                var ray6 = Camera.main.ScreenPointToRay(Input.mousePosition);
                                LayerMask mask16 = 1 << LayerMask.NameToLayer("Ground");
                                LayerMask mask17 = 1 << LayerMask.NameToLayer("EnemyBox");
                                LayerMask mask18 = mask17 | mask16;
                                if (Physics.Raycast(ray6, out hit6, 1000000f, mask18.value))
                                {
                                    LaunchLeftRope(hit6, false);
                                    LaunchRightRope(hit6, false);
                                    rope.Play();
                                }
                            }
                        }
                        if (IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.SINGLE || IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE && !IN_GAME_MAIN_CAMERA.isPausing)
                        {
                            CalcSkillCd();
                            CalcFlareCd();
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
                            ShowGas2();
                            ShowAimUI2();
                        }
                    }
                    else if (isCannon && !IN_GAME_MAIN_CAMERA.isPausing)
                    {
                        ShowAimUI2();
                        CalcSkillCd();
                        ShowSkillCd();
                    }
                }
            }
        }
    }

    private void UpdateExt()
    {
        if (skillId == "bomb")
        {
            if (InputManager.KeyDown(InputHuman.AttackSpecial) && skillCdDuration <= 0f)
            {
                if (!(myBomb == null || myBomb.disabled))
                {
                    myBomb.Explode(bombRadius);
                }
                detonate = false;
                skillCdDuration = bombCd;
                var hitInfo = new RaycastHit();
                var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                LayerMask mask = 1 << LayerMask.NameToLayer("Ground");
                LayerMask mask2 = 1 << LayerMask.NameToLayer("EnemyBox");
                LayerMask mask3 = mask2 | mask;
                currentV = baseTransform.position;
                targetV = currentV + Vector3.forward * 200f;
                if (Physics.Raycast(ray, out hitInfo, 1000000f, mask3.value))
                {
                    targetV = hitInfo.point;
                }
                var vector = Vector3.Normalize(targetV - currentV);
                var obj2 = PhotonNetwork.Instantiate("RC Resources/RC Prefabs/BombMain", currentV + vector * 4f, new Quaternion(0f, 0f, 0f, 1f), 0);
                obj2.GetComponent<Rigidbody>().velocity = vector * bombSpeed;
                myBomb = obj2.GetComponent<Bomb>();
                bombTime = 0f;
            }
            else if (myBomb != null && !myBomb.disabled)
            {
                bombTime += Time.deltaTime;
                var flag2 = false;
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
        return;
        for (var i = 1; i <= bulletMAX; i++)
        {
            //GameObject.Find("bulletL" + i).GetComponent<UISprite>().enabled = false;
        }
        for (var j = 1; j <= leftBulletLeft; j++)
        {
            //GameObject.Find("bulletL" + j).GetComponent<UISprite>().enabled = true;
        }
    }

    private void UpdateRightMagUI()
    {
        return;
        for (var i = 1; i <= bulletMAX; i++)
        {
            //GameObject.Find("bulletR" + i).GetComponent<UISprite>().enabled = false;
        }
        for (var j = 1; j <= rightBulletLeft; j++)
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
    private void whoIsMyErenTitan(int id)
    {
        erenTitan = PhotonView.Find(id).gameObject;
        titanForm = true;
    }

    public bool IsGrabbed => State == HERO_STATE.Grab;

    private HERO_STATE State
    {
        get
        {
            return state;
        }
        set
        {
            if (state == HERO_STATE.AirDodge || state == HERO_STATE.GroundDodge)
                dashTime = 0f;
            state = value;
        }
    }
}