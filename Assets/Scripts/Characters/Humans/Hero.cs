using Assets.Scripts.Characters.Humans.Customization;
using Assets.Scripts.Characters.Humans.Skills;
using Assets.Scripts.Characters.Titan;
using Assets.Scripts.Gamemode.Options;
using Assets.Scripts.Serialization;
using Assets.Scripts.Services;
using Assets.Scripts.Settings;
using Assets.Scripts.UI.InGame.HUD;
using Assets.Scripts.UI.Input;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Characters.Humans
{
    public class Hero : Human
    {
        public CharacterPrefabs Prefabs;
        public Equipment.Equipment Equipment { get; set; }
        public EquipmentType EquipmentType;

        public Skill Skill { get; set; }

        public HumanState State { get; protected set; } = HumanState.Idle;

        private const float HookRaycastDistance = 1000f;

        #region Properties
        public HERO_STATE _state { get; set; }
        private HERO_STATE state
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

        private bool almostSingleHook { get; set; }
        public string attackAnimation { get; set; }
        public int attackLoop { get; set; }
        private bool attackReleased { get; set; }
        private GameObject badGuy { get; set; }
        public bool bigLean;
        public float bombCD;
        public bool bombImmune;
        public float bombRadius;
        public float bombSpeed;
        public float bombTime;
        public float bombTimeMax;
        private float buffTime { get; set; }
        public GameObject bulletLeft;
        private int bulletMAX { get; set; } = 7;
        public GameObject bulletRight;
        private bool buttonAttackRelease { get; set; }
        public Dictionary<string, Image> cachedSprites;
        public float CameraMultiplier;
        public GameObject checkBoxLeft;
        public GameObject checkBoxRight;
        public GameObject cross1;
        public GameObject cross2;
        public GameObject crossL1;
        public GameObject crossL2;
        public GameObject crossR1;
        public GameObject crossR2;
        public string CurrentAnimation;
        public float currentBladeSta = 100f;
        private BUFF currentBuff { get; set; }
        public Camera currentCamera;
        private float currentGas { get; set; } = 100f;
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
        private GameObject eren_titan { get; set; }
        public float facingDirection { get; set; }
        private Transform forearmL { get; set; }
        private Transform forearmR { get; set; }
        private float gravity { get; set; } = 20f;
        public bool grounded;
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
        public Text LabelDistance;
        public Transform lastHook;
        private float launchElapsedTimeL { get; set; }
        private float launchElapsedTimeR { get; set; }
        private Vector3 launchForce { get; set; }
        public Vector3 launchPointLeft { get; private set; }
        public Vector3 launchPointRight { get; private set; }
        private bool leanLeft { get; set; }
        private bool leftArmAim { get; set; }
        /*
    public XWeaponTrail leftbladetrail;
    public XWeaponTrail leftbladetrail2;
    */
        [Obsolete]
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
        /*
    public XWeaponTrail rightbladetrail;
    public XWeaponTrail rightbladetrail2;
    */
        [Obsolete]
        public int rightBulletLeft = 7;
        public bool rightGunHasBullet = true;
        public AudioSource rope;
        private float rTapTime { get; set; } = -1f;
        private GameObject skillCD { get; set; }
        public float skillCDDuration;
        public float skillCDLast;
        public float skillCDLastCannon;
        private string skillId { get; set; }
        public string skillIDHUD;
        public AudioSource slash;
        public AudioSource slashHit;
        private ParticleSystem smoke_3dmg { get; set; }
        private ParticleSystem sparks { get; set; }
        public float speed = 10f;
        public GameObject speedFX;
        public GameObject speedFX1;
        public bool spinning;
        private string standAnimation { get; set; } = "stand";
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

        public bool IsGrabbed => state == HERO_STATE.Grab;
        public bool IsInvincible => (invincible > 0f);

        #endregion


        public GameObject InGameUI;
        public TextMesh PlayerName;

        // Hero 2.0
        public Animation Animation { get; protected set; }
        public Rigidbody Rigidbody { get; protected set; }

        protected override void Awake()
        {
            base.Awake();
            Animation = GetComponent<Animation>();
            Rigidbody = GetComponent<Rigidbody>();

            InGameUI = GameObject.Find("InGameUi");
            Cache();
            Rigidbody.freezeRotation = true;
            Rigidbody.useGravity = false;
            handL = transform.Find("Amarture/Controller_Body/hip/spine/chest/shoulder_L/upper_arm_L/forearm_L/hand_L");
            handR = transform.Find("Amarture/Controller_Body/hip/spine/chest/shoulder_R/upper_arm_R/forearm_R/hand_R");
            forearmL = transform.Find("Amarture/Controller_Body/hip/spine/chest/shoulder_L/upper_arm_L/forearm_L");
            forearmR = transform.Find("Amarture/Controller_Body/hip/spine/chest/shoulder_R/upper_arm_R/forearm_R");
            upperarmL = transform.Find("Amarture/Controller_Body/hip/spine/chest/shoulder_L/upper_arm_L");
            upperarmR = transform.Find("Amarture/Controller_Body/hip/spine/chest/shoulder_R/upper_arm_R");
            Equipment = gameObject.AddComponent<Equipment.Equipment>();
            Faction = Service.Faction.GetHumanity();
            Service.Entity.Register(this);

            Animation["attack5"].speed = 1.85f;
            Animation["changeBlade"].speed = 1.2f;
            Animation["air_release"].speed = 0.6f;
            Animation["changeBlade_air"].speed = 0.8f;
            Animation["AHSS_gun_reload_both"].speed = 0.38f;
            Animation["AHSS_gun_reload_both_air"].speed = 0.5f;
            Animation["AHSS_gun_reload_l"].speed = 0.4f;
            Animation["AHSS_gun_reload_l_air"].speed = 0.5f;
            Animation["AHSS_gun_reload_r"].speed = 0.4f;
            Animation["AHSS_gun_reload_r_air"].speed = 0.5f;
        }

        private void Start()
        {
            gameObject.AddComponent<PlayerInteractable>();
            SetHorse();
            sparks = transform.Find("slideSparks").GetComponent<ParticleSystem>();
            smoke_3dmg = transform.Find("3dmg_smoke").GetComponent<ParticleSystem>();
            transform.localScale = new Vector3(myScale, myScale, myScale);
            facingDirection = transform.rotation.eulerAngles.y;
            targetRotation = Quaternion.Euler(0f, facingDirection, 0f);
            smoke_3dmg.enableEmission = false;
            sparks.enableEmission = false;
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
            if (photonView.isMine)
            {
                GetComponent<SmoothSyncMovement>().PhotonCamera = true;
                photonView.RPC(nameof(SetMyPhotonCamera), PhotonTargets.OthersBuffered,
                    new object[] { PlayerPrefs.GetFloat("cameraDistance") + 0.3f });
            }
            else
            {
                bool flag2 = false;
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
                    GameObject obj3 = (GameObject) UnityEngine.Object.Instantiate(Resources.Load("flashlight"));
                    obj3.transform.parent = transform;
                    obj3.transform.position = transform.position + Vector3.up;
                    obj3.transform.rotation = Quaternion.Euler(353f, 0f, 0f);
                }
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
                //loadskin();
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
                standAnimation = "AHSS_stand_gun";
                useGun = true;
                gunDummy = new GameObject();
                gunDummy.name = "gunDummy";
                gunDummy.transform.position = transform.position;
                gunDummy.transform.rotation = transform.rotation;
            }

            if (photonView.isMine)
            {
                //TODO: If this is a default preset, find a more efficient way
                var config = JsonConvert.SerializeObject(preset, Formatting.Indented, new ColorJsonConverter());
                photonView.RPC(nameof(InitializeRpc), PhotonTargets.OthersBuffered, config);
            }

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
                Initialize(JsonConvert.DeserializeObject<CharacterPreset>(characterPreset, new ColorJsonConverter()));
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
            Debug.Log($"Calling SetSpeed: {animationName}");
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
            Animation["attack5"].speed = 1.85f;
            Animation["changeBlade"].speed = 1.2f;
            Animation["air_release"].speed = 0.6f;
            Animation["changeBlade_air"].speed = 0.8f;
            Animation["AHSS_gun_reload_both"].speed = 0.38f;
            Animation["AHSS_gun_reload_both_air"].speed = 0.5f;
            Animation["AHSS_gun_reload_l"].speed = 0.4f;
            Animation["AHSS_gun_reload_l_air"].speed = 0.5f;
            Animation["AHSS_gun_reload_r"].speed = 0.4f;
            Animation["AHSS_gun_reload_r_air"].speed = 0.5f;
        }

        [PunRPC]
        private void NetCrossFade(string aniName, float time)
        {
            CurrentAnimation = aniName;
            if (Animation != null)
            {
                Animation.CrossFade(aniName, time);
            }
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

        private void ApplyForceToBody(GameObject GO, Vector3 v)
        {
            GO.GetComponent<Rigidbody>().AddForce(v);
            GO.GetComponent<Rigidbody>().AddTorque(UnityEngine.Random.Range((float) -10f, (float) 10f), UnityEngine.Random.Range((float) -10f, (float) 10f), UnityEngine.Random.Range((float) -10f, (float) 10f));
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

        public void BackToHuman()
        {
            gameObject.GetComponent<SmoothSyncMovement>().disabled = false;
            Rigidbody.velocity = Vector3.zero;
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
                if ((!useGun && (state == HERO_STATE.Attack)) && ((attackAnimation != "attack3_1") && (attackAnimation != "attack3_2")))
                {
                    float y = Rigidbody.velocity.y;
                    float x = Rigidbody.velocity.x;
                    float num4 = Rigidbody.velocity.z;
                    float num5 = Mathf.Sqrt((x * x) + (num4 * num4));
                    float num6 = Mathf.Atan2(y, num5) * Mathf.Rad2Deg;
                    targetRotation = Quaternion.Euler(-num6 * (1f - (Vector3.Angle(Rigidbody.velocity, transform.forward) / 90f)), facingDirection, 0f);
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
                        if (!useGun && (state != HERO_STATE.Attack))
                        {
                            a = currentSpeed * 0.1f;
                            a = Mathf.Min(a, 20f);
                        }
                        targetRotation = Quaternion.Euler(-a, facingDirection, z);
                    }
                    else if (state != HERO_STATE.Attack)
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
                int num = (int) FengGameManagerMKII.settings[250];
                int num2 = (int) FengGameManagerMKII.settings[0xfb];
                int num3 = (int) FengGameManagerMKII.settings[0xfc];
                int num4 = (int) FengGameManagerMKII.settings[0xfd];
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
                propertiesToSet.Add(PhotonPlayerProperty.RCBombR, (float) FengGameManagerMKII.settings[0xf6]);
                propertiesToSet.Add(PhotonPlayerProperty.RCBombG, (float) FengGameManagerMKII.settings[0xf7]);
                propertiesToSet.Add(PhotonPlayerProperty.RCBombB, (float) FengGameManagerMKII.settings[0xf8]);
                propertiesToSet.Add(PhotonPlayerProperty.RCBombA, (float) FengGameManagerMKII.settings[0xf9]);
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

        private void BreakApart2(Vector3 v, bool isBite)
        {
            throw new NotImplementedException("Character death is not implemented yet");
            //GameObject obj6;
            //GameObject obj7;
            //GameObject obj8;
            //GameObject obj9;
            //GameObject obj10;
            //GameObject obj2 = (GameObject)UnityEngine.Object.Instantiate(Resources.Load("Character_parts/AOTTG_HERO_body"), transform.position, transform.rotation);
            //obj2.gameObject.GetComponent<HERO_SETUP>().myCostume = setup.myCostume;
            //obj2.GetComponent<HERO_SETUP>().isDeadBody = true;
            //obj2.GetComponent<HERO_DEAD_BODY_SETUP>().init(currentAnimation, Animation[currentAnimation].normalizedTime, BODY_PARTS.ARM_R);
            //if (!isBite)
            //{
            //    GameObject gO = (GameObject)UnityEngine.Object.Instantiate(Resources.Load("Character_parts/AOTTG_HERO_body"), transform.position, transform.rotation);
            //    GameObject obj4 = (GameObject)UnityEngine.Object.Instantiate(Resources.Load("Character_parts/AOTTG_HERO_body"), transform.position, transform.rotation);
            //    GameObject obj5 = (GameObject)UnityEngine.Object.Instantiate(Resources.Load("Character_parts/AOTTG_HERO_body"), transform.position, transform.rotation);
            //    gO.gameObject.GetComponent<HERO_SETUP>().myCostume = setup.myCostume;
            //    obj4.gameObject.GetComponent<HERO_SETUP>().myCostume = setup.myCostume;
            //    obj5.gameObject.GetComponent<HERO_SETUP>().myCostume = setup.myCostume;
            //    gO.GetComponent<HERO_SETUP>().isDeadBody = true;
            //    obj4.GetComponent<HERO_SETUP>().isDeadBody = true;
            //    obj5.GetComponent<HERO_SETUP>().isDeadBody = true;
            //    gO.GetComponent<HERO_DEAD_BODY_SETUP>().init(currentAnimation, Animation[currentAnimation].normalizedTime, BODY_PARTS.UPPER);
            //    obj4.GetComponent<HERO_DEAD_BODY_SETUP>().init(currentAnimation, Animation[currentAnimation].normalizedTime, BODY_PARTS.LOWER);
            //    obj5.GetComponent<HERO_DEAD_BODY_SETUP>().init(currentAnimation, Animation[currentAnimation].normalizedTime, BODY_PARTS.ARM_L);
            //    applyForceToBody(gO, v);
            //    applyForceToBody(obj4, v);
            //    applyForceToBody(obj5, v);
            //    if (photonView.isMine)
            //    {
            //        currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().setMainObject(gO, false, false);
            //    }
            //}
            //else if (photonView.isMine)
            //{
            //    currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().setMainObject(obj2, false, false);
            //}
            //applyForceToBody(obj2, v);
            //Transform transform = transform.Find("Amarture/Controller_Body/hip/spine/chest/shoulder_L/upper_arm_L/forearm_L/hand_L").transform;
            //Transform transform2 = transform.Find("Amarture/Controller_Body/hip/spine/chest/shoulder_R/upper_arm_R/forearm_R/hand_R").transform;
            //if (useGun)
            //{
            //    obj6 = (GameObject)UnityEngine.Object.Instantiate(Resources.Load("Character_parts/character_gun_l"), transform.position, transform.rotation);
            //    obj7 = (GameObject)UnityEngine.Object.Instantiate(Resources.Load("Character_parts/character_gun_r"), transform2.position, transform2.rotation);
            //    obj8 = (GameObject)UnityEngine.Object.Instantiate(Resources.Load("Character_parts/character_3dmg_2"), transform.position, transform.rotation);
            //    obj9 = (GameObject)UnityEngine.Object.Instantiate(Resources.Load("Character_parts/character_gun_mag_l"), transform.position, transform.rotation);
            //    obj10 = (GameObject)UnityEngine.Object.Instantiate(Resources.Load("Character_parts/character_gun_mag_r"), transform.position, transform.rotation);
            //}
            //else
            //{
            //    obj6 = (GameObject)UnityEngine.Object.Instantiate(Resources.Load("Character_parts/character_blade_l"), transform.position, transform.rotation);
            //    obj7 = (GameObject)UnityEngine.Object.Instantiate(Resources.Load("Character_parts/character_blade_r"), transform2.position, transform2.rotation);
            //    obj8 = (GameObject)UnityEngine.Object.Instantiate(Resources.Load("Character_parts/character_3dmg"), transform.position, transform.rotation);
            //    obj9 = (GameObject)UnityEngine.Object.Instantiate(Resources.Load("Character_parts/character_3dmg_gas_l"), transform.position, transform.rotation);
            //    obj10 = (GameObject)UnityEngine.Object.Instantiate(Resources.Load("Character_parts/character_3dmg_gas_r"), transform.position, transform.rotation);
            //}
            //obj6.GetComponent<Renderer>().material = CharacterMaterials.materials[setup.myCostume._3dmg_texture];
            //obj7.GetComponent<Renderer>().material = CharacterMaterials.materials[setup.myCostume._3dmg_texture];
            //obj8.GetComponent<Renderer>().material = CharacterMaterials.materials[setup.myCostume._3dmg_texture];
            //obj9.GetComponent<Renderer>().material = CharacterMaterials.materials[setup.myCostume._3dmg_texture];
            //obj10.GetComponent<Renderer>().material = CharacterMaterials.materials[setup.myCostume._3dmg_texture];
            //applyForceToBody(obj6, v);
            //applyForceToBody(obj7, v);
            //applyForceToBody(obj8, v);
            //applyForceToBody(obj9, v);
            //applyForceToBody(obj10, v);
        }

        private void BufferUpdate()
        {
            if (buffTime > 0f)
            {
                buffTime -= Time.deltaTime;
                if (buffTime <= 0f)
                {
                    buffTime = 0f;
                    if ((currentBuff == BUFF.SpeedUp) && Animation.IsPlaying("run_sasha"))
                    {
                        CrossFade("run_1", 0.1f);
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
                cross1 = GameObject.Find("cross1");
                cross2 = GameObject.Find("cross2");
                crossL1 = GameObject.Find("crossL1");
                crossL2 = GameObject.Find("crossL2");
                crossR1 = GameObject.Find("crossR1");
                crossR2 = GameObject.Find("crossR2");
                LabelDistance = GameObject.Find("Distance").GetComponent<Text>();
                cachedSprites = new Dictionary<string, Image>();
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

        private void ChangeBlade()
        {
            if ((!useGun || grounded) || GameSettings.PvP.AhssAirReload.Value)
            {
                state = HERO_STATE.ChangeBlade;
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
            LayerMask mask = ((int) 1) << LayerMask.NameToLayer("PlayerAttackBox");
            LayerMask mask2 = ((int) 1) << LayerMask.NameToLayer("Ground");
            LayerMask mask3 = ((int) 1) << LayerMask.NameToLayer("EnemyBox");
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
            return;
            if (collision.gameObject.tag == "titan") return;
            var force = collision.impulse.magnitude / Time.fixedDeltaTime;
            if (GameSettings.Gamemode.ImpactForce > 0 && force >= GameSettings.Gamemode.ImpactForce)
            {
                Die(new Vector3(), false);
            }
        }


        private void Dash(float horizontal, float vertical)
        {
            if (((dashTime <= 0f) && (currentGas > 0f)) && !isMounted)
            {
                UseGas(totalGas * 0.04f);
                facingDirection = GetGlobalFacingDirection(horizontal, vertical);
                dashV = GetGlobaleFacingVector3(facingDirection);
                originVM = currentSpeed;
                Quaternion quaternion = Quaternion.Euler(0f, facingDirection, 0f);
                Rigidbody.rotation = quaternion;
                targetRotation = quaternion;
                PhotonNetwork.Instantiate("FX/boost_smoke", transform.position, transform.rotation, 0);
                dashTime = 0.5f;
                CrossFade("dash", 0.1f);
                Animation["dash"].time = 0.1f;
                state = HERO_STATE.AirDodge;
                FalseAttack();
                Rigidbody.AddForce((dashV * 40f), ForceMode.VelocityChange);
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
                meatDie.Play();
                if ((photonView.isMine) && !useGun)
                {
                    /*
                leftbladetrail.Deactivate();
                rightbladetrail.Deactivate();
                leftbladetrail2.Deactivate();
                rightbladetrail2.Deactivate();
                */
                }
                BreakApart2(v, isBite);
                currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().gameOver = true;
                FalseAttack();
                hasDied = true;
                Transform transform = transform.Find("audio_die");
                transform.parent = null;
                transform.GetComponent<AudioSource>().Play();

                var propertiesToSet = new ExitGames.Client.Photon.Hashtable();
                propertiesToSet.Add(PhotonPlayerProperty.deaths, (int) PhotonNetwork.player.CustomProperties[PhotonPlayerProperty.deaths] + 1);
                photonView.owner.SetCustomProperties(propertiesToSet);

                if (PlayerPrefs.HasKey("EnableSS") && (PlayerPrefs.GetInt("EnableSS") == 1))
                {
                    GameObject.Find("MainCamera").GetComponent<IN_GAME_MAIN_CAMERA>().StartSnapShot2(transform.position, 0, null, 0.02f);
                }
                UnityEngine.Object.Destroy(gameObject);
            }
        }

        public void Die2(Transform tf)
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
                Transform transform = transform.Find("audio_die");
                transform.parent = null;
                transform.GetComponent<AudioSource>().Play();
                meatDie.Play();
                currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().SetMainObject(null, true, false);
                currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().gameOver = true;
                FalseAttack();
                hasDied = true;
                GameObject obj2 = (GameObject) UnityEngine.Object.Instantiate(Resources.Load("hitMeat2"));
                obj2.transform.position = transform.position;
                UnityEngine.Object.Destroy(gameObject);
            }
        }

        private void Dodge2(bool offTheWall = false)
        {
            if (((!InputManager.Key(InputHorse.Mount) || !myHorse) || isMounted) || (Vector3.Distance(myHorse.transform.position, transform.position) >= 15f))
            {
                state = HERO_STATE.GroundDodge;
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
                    PlayAnimationAt("dodge", 0.2f);
                }
                sparks.enableEmission = false;
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
            eren_titan.GetComponent<Rigidbody>().velocity = Rigidbody.velocity;
            Rigidbody.velocity = Vector3.zero;
            transform.position = eren_titan.transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/neck").position;
            titanForm = true;
            object[] parameters = new object[] { eren_titan.GetPhotonView().viewID };
            photonView.RPC(nameof(WhoIsMyErenTitan), PhotonTargets.Others, parameters);
            if ((smoke_3dmg.enableEmission && photonView.isMine))
            {
                object[] objArray2 = new object[] { false };
                photonView.RPC(nameof(Net3DMGSMOKE), PhotonTargets.Others, objArray2);
            }
            smoke_3dmg.enableEmission = false;
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

        //Hotfix for Issue 97.
        private void AddRightForce()
        {
            //Whereas this may not be completely accurate to AoTTG, it is very close. Further balancing required in the future.
            Rigidbody.AddForce(Rigidbody.velocity * 0.00f, ForceMode.Acceleration);
        }

        private void FixedUpdate()
        {
            if (!photonView.isMine) return;
            if ((!titanForm && !isCannon) && (!IN_GAME_MAIN_CAMERA.isPausing))
            {
                currentSpeed = Rigidbody.velocity.magnitude;
                if (!((Animation.IsPlaying("attack3_2") || Animation.IsPlaying("attack5")) || Animation.IsPlaying("special_petra")))
                {
                    Rigidbody.rotation = Quaternion.Lerp(gameObject.transform.rotation, targetRotation, Time.deltaTime * 6f);
                }
                if (state == HERO_STATE.Grab)
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
                            Vector3 to = bulletLeft.transform.position - transform.position;
                            to.Normalize();
                            to = (to * 10f);
                            if (!isLaunchRight)
                            {
                                to = (to * 2f);
                            }
                            if ((Vector3.Angle(Rigidbody.velocity, to) > 90f) && InputManager.Key(InputHuman.Jump))
                            {
                                flag3 = true;
                                flag2 = true;
                            }
                            if (!flag3)
                            {
                                Rigidbody.AddForce(to);
                                if (Vector3.Angle(Rigidbody.velocity, to) > 90f)
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
                        if ((bulletRight != null) && bulletRight.GetComponent<Bullet>().isHooked())
                        {
                            isRightHandHooked = true;
                            Vector3 vector5 = bulletRight.transform.position - transform.position;
                            vector5.Normalize();
                            vector5 = (vector5 * 10f);
                            if (!isLaunchLeft)
                            {
                                vector5 = (vector5 * 2f);
                            }
                            if ((Vector3.Angle(Rigidbody.velocity, vector5) > 90f) && InputManager.Key(InputHuman.Jump))
                            {
                                flag4 = true;
                                flag2 = true;
                            }
                            if (!flag4)
                            {
                                Rigidbody.AddForce(vector5);
                                if (Vector3.Angle(Rigidbody.velocity, vector5) > 90f)
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
                        Vector3 zero = Vector3.zero;
                        if (state == HERO_STATE.Attack)
                        {
                            if (attackAnimation == "attack5")
                            {
                                if ((Animation[attackAnimation].normalizedTime > 0.4f) && (Animation[attackAnimation].normalizedTime < 0.61f))
                                {
                                    Rigidbody.AddForce((gameObject.transform.forward * 200f));
                                }
                            }
                            else if (Animation.IsPlaying("attack3_2"))
                            {
                                zero = Vector3.zero;
                            }
                            else if (Animation.IsPlaying("attack1") || Animation.IsPlaying("attack2"))
                            {
                                Rigidbody.AddForce((gameObject.transform.forward * 200f));
                            }
                            if (Animation.IsPlaying("attack3_2"))
                            {
                                zero = Vector3.zero;
                            }
                        }
                        if (justGrounded)
                        {
                            //TODO: attackAnimation conditions appear to be useless
                            if ((state != HERO_STATE.Attack) || (((attackAnimation != "attack3_1") && (attackAnimation != "attack5")) && (attackAnimation != "special_petra")))
                            {
                                if ((((state != HERO_STATE.Attack) && (x == 0f)) && ((z == 0f) && (bulletLeft == null))) && ((bulletRight == null) && (state != HERO_STATE.FillGas)))
                                {
                                    state = HERO_STATE.Land;
                                    CrossFade("dash_land", 0.01f);
                                }
                                else
                                {
                                    buttonAttackRelease = true;
                                    if (((state != HERO_STATE.Attack) && (((Rigidbody.velocity.x * Rigidbody.velocity.x) + (Rigidbody.velocity.z * Rigidbody.velocity.z)) > ((speed * speed) * 1.5f))) && (state != HERO_STATE.FillGas))
                                    {
                                        state = HERO_STATE.Slide;
                                        CrossFade("slide", 0.05f);
                                        facingDirection = Mathf.Atan2(Rigidbody.velocity.x, Rigidbody.velocity.z) * Mathf.Rad2Deg;
                                        targetRotation = Quaternion.Euler(0f, facingDirection, 0f);
                                        sparks.enableEmission = true;
                                    }
                                }
                            }
                            justGrounded = false;
                            zero = Rigidbody.velocity;
                        }
                        if (state == HERO_STATE.GroundDodge)
                        {
                            if ((Animation["dodge"].normalizedTime >= 0.2f) && (Animation["dodge"].normalizedTime < 0.8f))
                            {
                                zero = ((-transform.forward * 2.4f) * speed);
                            }
                            if (Animation["dodge"].normalizedTime > 0.8f)
                            {
                                zero = (Rigidbody.velocity * 0.9f);
                            }
                        }
                        else if (state == HERO_STATE.Idle)
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
                                if (((!Animation.IsPlaying("run_1") && !Animation.IsPlaying("jump")) && !Animation.IsPlaying("run_sasha")) && (!Animation.IsPlaying("horse_geton") || (Animation["horse_geton"].normalizedTime >= 0.5f)))
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
                                if (!(((Animation.IsPlaying(standAnimation) || (state == HERO_STATE.Land)) || (Animation.IsPlaying("jump") || Animation.IsPlaying("horse_geton"))) || Animation.IsPlaying("grabbed")))
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
                        else if (state == HERO_STATE.Land)
                        {
                            zero = (Rigidbody.velocity * 0.96f);
                        }
                        else if (state == HERO_STATE.Slide)
                        {
                            zero = (Rigidbody.velocity * 0.99f);
                            if (currentSpeed < (speed * 1.2f))
                            {
                                Idle();
                                sparks.enableEmission = false;
                            }
                        }
                        Vector3 velocity = Rigidbody.velocity;
                        Vector3 force = zero - velocity;
                        force.x = Mathf.Clamp(force.x, -maxVelocityChange, maxVelocityChange);
                        force.z = Mathf.Clamp(force.z, -maxVelocityChange, maxVelocityChange);
                        force.y = 0f;
                        if (Animation.IsPlaying("jump") && (Animation["jump"].normalizedTime > 0.18f))
                        {
                            force.y += 8f;
                        }
                        if ((Animation.IsPlaying("horse_geton") && (Animation["horse_geton"].normalizedTime > 0.18f)) && (Animation["horse_geton"].normalizedTime < 1f))
                        {
                            float num7 = 6f;
                            force = -Rigidbody.velocity;
                            force.y = num7;
                            float num8 = Vector3.Distance(myHorse.transform.position, transform.position);
                            float num9 = ((0.6f * gravity) * num8) / 12f;
                            vector7 = myHorse.transform.position - transform.position;
                            force += (num9 * vector7.normalized);
                        }
                        if (!(state == HERO_STATE.Attack && useGun))
                        {
                            Rigidbody.AddForce(force, ForceMode.VelocityChange);
                            Rigidbody.rotation = Quaternion.Lerp(gameObject.transform.rotation, Quaternion.Euler(0f, facingDirection, 0f), Time.deltaTime * 10f);
                        }
                    }
                    else
                    {
                        if (sparks.enableEmission)
                        {
                            sparks.enableEmission = false;
                        }
                        if ((myHorse && (Animation.IsPlaying("horse_geton") || Animation.IsPlaying("air_fall"))) && ((Rigidbody.velocity.y < 0f) && (Vector3.Distance(myHorse.transform.position + Vector3.up * 1.65f, transform.position) < 0.5f)))
                        {
                            transform.position = myHorse.transform.position + Vector3.up * 1.65f;
                            transform.rotation = myHorse.transform.rotation;
                            isMounted = true;
                            CrossFade("horse_idle", 0.1f);
                            myHorse.Mount();
                        }
                        if (!((((((state != HERO_STATE.Idle) || Animation.IsPlaying("dash")) || (Animation.IsPlaying("wallrun") || Animation.IsPlaying("toRoof"))) || ((Animation.IsPlaying("horse_geton") || Animation.IsPlaying("horse_getoff")) || (Animation.IsPlaying("air_release") || isMounted))) || ((Animation.IsPlaying("air_hook_l_just") && (Animation["air_hook_l_just"].normalizedTime < 1f)) || (Animation.IsPlaying("air_hook_r_just") && (Animation["air_hook_r_just"].normalizedTime < 1f)))) ? (Animation["dash"].normalizedTime < 0.99f) : false))
                        {
                            if (((!isLeftHandHooked && !isRightHandHooked) && ((Animation.IsPlaying("air_hook_l") || Animation.IsPlaying("air_hook_r")) || Animation.IsPlaying("air_hook"))) && (Rigidbody.velocity.y > 20f))
                            {
                                Animation.CrossFade("air_release");
                            }
                            else
                            {
                                bool flag5 = (Mathf.Abs(Rigidbody.velocity.x) + Mathf.Abs(Rigidbody.velocity.z)) > 25f;
                                bool flag6 = Rigidbody.velocity.y < 0f;
                                if (!flag5)
                                {
                                    if (flag6)
                                    {
                                        if (!Animation.IsPlaying("air_fall"))
                                        {
                                            CrossFade("air_fall", 0.2f);
                                        }
                                    }
                                    else if (!Animation.IsPlaying("air_rise"))
                                    {
                                        CrossFade("air_rise", 0.2f);
                                    }
                                }
                                else if (!isLeftHandHooked && !isRightHandHooked)
                                {
                                    float current = -Mathf.Atan2(Rigidbody.velocity.z, Rigidbody.velocity.x) * Mathf.Rad2Deg;
                                    float num11 = -Mathf.DeltaAngle(current, transform.rotation.eulerAngles.y - 90f);
                                    if (Mathf.Abs(num11) < 45f)
                                    {
                                        if (!Animation.IsPlaying("air2"))
                                        {
                                            CrossFade("air2", 0.2f);
                                        }
                                    }
                                    else if ((num11 < 135f) && (num11 > 0f))
                                    {
                                        if (!Animation.IsPlaying("air2_right"))
                                        {
                                            CrossFade("air2_right", 0.2f);
                                        }
                                    }
                                    else if ((num11 > -135f) && (num11 < 0f))
                                    {
                                        if (!Animation.IsPlaying("air2_left"))
                                        {
                                            CrossFade("air2_left", 0.2f);
                                        }
                                    }
                                    else if (!Animation.IsPlaying("air2_backward"))
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
                                else if (!Animation.IsPlaying(Equipment.Weapon.HookForward))
                                {
                                    TryCrossFade(Equipment.Weapon.HookForward, 0.1f);
                                }
                            }
                        }
                        if (((state == HERO_STATE.Idle) && Animation.IsPlaying("air_release")) && (Animation["air_release"].normalizedTime >= 1f))
                        {
                            CrossFade("air_rise", 0.2f);
                        }
                        if (Animation.IsPlaying("horse_getoff") && (Animation["horse_getoff"].normalizedTime >= 1f))
                        {
                            CrossFade("air_rise", 0.2f);
                        }
                        if (Animation.IsPlaying("toRoof"))
                        {
                            if (Animation["toRoof"].normalizedTime < 0.22f)
                            {
                                Rigidbody.velocity = Vector3.zero;
                                Rigidbody.AddForce(new Vector3(0f, gravity * Rigidbody.mass, 0f));
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
                            if (Animation["toRoof"].normalizedTime >= 1f)
                            {
                                PlayAnimation("air_rise");
                            }
                        }
                        else if (!(((((state != HERO_STATE.Idle) || !IsPressDirectionTowardsHero(x, z)) ||
                                     (InputManager.Key(InputHuman.Jump) ||
                                      InputManager.Key(InputHuman.HookLeft))) ||
                                    ((InputManager.Key(InputHuman.HookRight) ||
                                      InputManager.Key(InputHuman.HookBoth)) ||
                                     (!IsFrontGrounded() || Animation.IsPlaying("wallrun")))) ||
                                   Animation.IsPlaying("dodge")))
                        {
                            CrossFade("wallrun", 0.1f);
                            wallRunTime = 0f;
                        }
                        else if (Animation.IsPlaying("wallrun"))
                        {
                            Rigidbody.AddForce(((Vector3.up * speed)) - Rigidbody.velocity, ForceMode.VelocityChange);
                            wallRunTime += Time.deltaTime;
                            if ((wallRunTime > 1f) || ((z == 0f) && (x == 0f)))
                            {
                                Rigidbody.AddForce(((-transform.forward * speed) * 0.75f), ForceMode.Impulse);
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
                        // If we are using these skills, then we cannot use gas force
                        else if ((!Animation.IsPlaying("attack5") && !Animation.IsPlaying("special_petra")) && (!Animation.IsPlaying("dash") && !Animation.IsPlaying("jump")))
                        {
                            Vector3 vector11 = new Vector3(x, 0f, z);
                            float num12 = GetGlobalFacingDirection(x, z);
                            Vector3 vector12 = GetGlobaleFacingVector3(num12);
                            float num13 = (vector11.magnitude <= 0.95f) ? ((vector11.magnitude >= 0.25f) ? vector11.magnitude : 0f) : 1f;
                            vector12 = (vector12 * num13);
                            //TODO: ACL
                            vector12 = (vector12 * ((/*(float)setup.myCostume.stat.ACL) */ 125f / 10f) * 2f));
                            if ((x == 0f) && (z == 0f))
                            {
                                if (state == HERO_STATE.Attack)
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
                                    Rigidbody.AddForce(vector12, ForceMode.Acceleration);
                                }
                                else
                                {
                                    Rigidbody.AddForce((transform.forward * vector12.magnitude), ForceMode.Acceleration);
                                }
                                flag2 = true;
                            }
                        }
                        if ((Animation.IsPlaying("air_fall") && (currentSpeed < 0.2f)) && IsFrontGrounded())
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
                            num15 = Input.GetAxis("Mouse ScrollWheel") * 5555f;
                        }
                        num15 = Mathf.Clamp(num15, -0.8f, 0.8f);
                        float num16 = 1f + num15;
                        Vector3 vector14 = Vector3.RotateTowards(vector13, Rigidbody.velocity, 1.53938f * num16, 1.53938f * num16);
                        vector14.Normalize();
                        spinning = true;
                        Rigidbody.velocity = (vector14 * num14);
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
                            num18 = Input.GetAxis("Mouse ScrollWheel") * 5555f;
                        }
                        num18 = Mathf.Clamp(num18, -0.8f, 0.8f);
                        float num19 = 1f + num18;
                        Vector3 vector16 = Vector3.RotateTowards(vector15, Rigidbody.velocity, 1.53938f * num19, 1.53938f * num19);
                        vector16.Normalize();
                        spinning = true;
                        Rigidbody.velocity = (vector16 * num17);
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
                            num21 = Input.GetAxis("Mouse ScrollWheel") * 5555f;
                        }
                        num21 = Mathf.Clamp(num21, -0.8f, 0.8f);
                        float num22 = 1f + num21;
                        Vector3 vector18 = Vector3.RotateTowards(vector17, Rigidbody.velocity, 1.53938f * num22, 1.53938f * num22);
                        vector18.Normalize();
                        spinning = true;
                        Rigidbody.velocity = (vector18 * num20);
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
                        Rigidbody.AddForce(new Vector3(0f, -10f * Rigidbody.mass, 0f));
                    }
                    else
                    {
                        Rigidbody.AddForce(new Vector3(0f, -gravity * Rigidbody.mass, 0f));
                    }
                    if (currentSpeed > 10f)
                    {
                        currentCamera.GetComponent<Camera>().fieldOfView = Mathf.Lerp(currentCamera.GetComponent<Camera>().fieldOfView, Mathf.Min((float) 100f, (float) (currentSpeed + 40f)), 0.1f);
                    }
                    else
                    {
                        currentCamera.GetComponent<Camera>().fieldOfView = Mathf.Lerp(currentCamera.GetComponent<Camera>().fieldOfView, 50f, 0.1f);
                    }
                    if (flag2)
                    {
                        UseGas(useGasSpeed * Time.deltaTime);
                        if (!smoke_3dmg.enableEmission && photonView.isMine)
                        {
                            object[] parameters = new object[] { true };
                            photonView.RPC(nameof(Net3DMGSMOKE), PhotonTargets.Others, parameters);
                        }
                        smoke_3dmg.enableEmission = true;
                    }
                    else
                    {
                        if (smoke_3dmg.enableEmission && photonView.isMine)
                        {
                            object[] objArray3 = new object[] { false };
                            photonView.RPC(nameof(Net3DMGSMOKE), PhotonTargets.Others, objArray3);
                        }
                        smoke_3dmg.enableEmission = false;
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

        private Vector3 GetGlobaleFacingVector3(float resultAngle)
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

        private float GetLeanAngle(Vector3 p, bool left)
        {
            if (!useGun && (state == HERO_STATE.Attack))
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
            if (state != HERO_STATE.Attack)
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
            PlayAnimation("horse_getoff");
            Rigidbody.AddForce((((Vector3.up * 10f) - (transform.forward * 2f)) - (transform.right * 1f)), ForceMode.VelocityChange);
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
            if ((Animation.IsPlaying(standAnimation)
                 || Animation.IsPlaying("run_1")
                 || Animation.IsPlaying("run_sasha"))
                && (currentBladeSta != totalBladeSta || currentGas != totalGas || Equipment.Weapon.CanReload))
            {
                state = HERO_STATE.FillGas;
                CrossFade("supply", 0.1f);
            }
        }

        public void Grabbed(GameObject titan, bool leftHand)
        {
            if (isMounted)
            {
                Unmounted();
            }
            state = HERO_STATE.Grab;
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
            smoke_3dmg.enableEmission = false;
            sparks.enableEmission = false;
        }

        public bool HasDied()
        {
            return (hasDied || IsInvincible);
        }

        private void HeadMovement()
        {
            Transform transform = transform.Find("Amarture/Controller_Body/hip/spine/chest/neck/head");
            Transform transform2 = transform.Find("Amarture/Controller_Body/hip/spine/chest/neck");
            float x = Mathf.Sqrt(((gunTarget.x - transform.position.x) * (gunTarget.x - transform.position.x)) + ((gunTarget.z - transform.position.z) * (gunTarget.z - transform.position.z)));
            targetHeadRotation = transform.rotation;
            Vector3 vector5 = gunTarget - transform.position;
            float current = -Mathf.Atan2(vector5.z, vector5.x) * Mathf.Rad2Deg;
            float num3 = -Mathf.DeltaAngle(current, transform.rotation.eulerAngles.y - 90f);
            num3 = Mathf.Clamp(num3, -40f, 40f);
            float y = transform2.position.y - gunTarget.y;
            float num5 = Mathf.Atan2(y, x) * Mathf.Rad2Deg;
            num5 = Mathf.Clamp(num5, -40f, 30f);
            targetHeadRotation = Quaternion.Euler(transform.rotation.eulerAngles.x + num5, transform.rotation.eulerAngles.y + num3, transform.rotation.eulerAngles.z);
            oldHeadRotation = Quaternion.Lerp(oldHeadRotation, targetHeadRotation, Time.deltaTime * 60f);
            transform.rotation = oldHeadRotation;
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
            if (state == HERO_STATE.Attack)
            {
                FalseAttack();
            }
            state = HERO_STATE.Idle;
            CrossFade(standAnimation, 0.1f);
        }

        private bool IsFrontGrounded()
        {
            LayerMask mask = ((int) 1) << LayerMask.NameToLayer("Ground");
            LayerMask mask2 = ((int) 1) << LayerMask.NameToLayer("EnemyBox");
            LayerMask mask3 = mask2 | mask;
            return Physics.Raycast(gameObject.transform.position + ((gameObject.transform.up * 1f)), gameObject.transform.forward, (float) 1f, mask3.value);
        }

        public bool IsGrounded()
        {
            LayerMask mask = ((int) 1) << LayerMask.NameToLayer("Ground");
            LayerMask mask2 = ((int) 1) << LayerMask.NameToLayer("EnemyBox");
            LayerMask mask3 = mask2 | mask;
            return Physics.Raycast(gameObject.transform.position + ((Vector3.up * 0.1f)), -Vector3.up, (float) 0.3f, mask3.value);
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
            LayerMask mask =    1 << LayerMask.NameToLayer("Ground");
            LayerMask mask2 =   1 << LayerMask.NameToLayer("EnemyBox");
            LayerMask mask3 = mask2 | mask;
            return Physics.Raycast(gameObject.transform.position + ((gameObject.transform.up * 3f)), gameObject.transform.forward, (float) 1.2f, mask3.value);
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
                Vector3 start = new Vector3(transform.position.x, transform.position.y + 2f, transform.position.z);
                
                LayerMask mask = ((int) 1) << LayerMask.NameToLayer("Ground");
                LayerMask mask2 = ((int) 1) << LayerMask.NameToLayer("EnemyBox");
                LayerMask mask3 = mask2 | mask;
                if ((Vector3.Angle(maincamera.transform.forward, start - maincamera.transform.position) > 90f) || Physics.Linecast(start, maincamera.transform.position, (int) mask3))
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
                if ((state == HERO_STATE.Grab) && (titanWhoGrabMe != null))
                {
                    if (titanWhoGrabMe.GetComponent<MindlessTitan>() != null)
                    {
                        transform.position = titanWhoGrabMe.GetComponent<MindlessTitan>().grabTF.transform.position;
                        transform.rotation = titanWhoGrabMe.GetComponent<MindlessTitan>().grabTF.transform.rotation;
                    }
                    else if (titanWhoGrabMe.GetComponent<FemaleTitan>() != null)
                    {
                        transform.position = titanWhoGrabMe.GetComponent<FemaleTitan>().grabTF.transform.position;
                        transform.rotation = titanWhoGrabMe.GetComponent<FemaleTitan>().grabTF.transform.rotation;
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
            if (state != HERO_STATE.Attack)
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
            if (!Animation.IsPlaying("attack5") && !Animation.IsPlaying("special_petra"))
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
                    Animation["dash"].time = 0f;
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
            if (Animation.IsPlaying("special_petra"))
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

        public void LaunchLeftRope(float distance, Vector3 point, bool single, int mode = 0)
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
                Vector3 vector = (point - ((transform.right * num))) - bulletLeft.transform.position;
                vector.Normalize();
                if (mode == 1)
                {
                    component.launch((vector * 3f), Rigidbody.velocity, str, true, gameObject, true);
                }
                else
                {
                    component.launch((vector * 3f), Rigidbody.velocity, str, true, gameObject, false);
                }
                launchPointLeft = Vector3.zero;
            }
        }

        public void LaunchRightRope(float distance, Vector3 point, bool single, int mode = 0)
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
                    component.launch((vector * 5f), Rigidbody.velocity, str, false, gameObject, true);
                }
                else
                {
                    component.launch((vector * 3f), Rigidbody.velocity, str, false, gameObject, false);
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
            state = HERO_STATE.Die;
        }

        [PunRPC]
        private void Net3DMGSMOKE(bool ifON)
        {
            if (smoke_3dmg != null)
            {
                smoke_3dmg.enableEmission = ifON;
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
            meatDie.Play();
            if (!(useGun || (!photonView.isMine)))
            {
                //TODO: Re-enable these again
                //leftbladetrail.Deactivate();
                //rightbladetrail.Deactivate();
                //leftbladetrail2.Deactivate();
                //rightbladetrail2.Deactivate();
            }
            FalseAttack();
            BreakApart2(v, isBite);
            if (photonView.isMine)
            {
                currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().SetSpectorMode(false);
                currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().gameOver = true;
                FengGameManagerMKII.instance.myRespawnTime = 0f;
            }
            hasDied = true;
            Transform transform = transform.Find("audio_die");
            if (transform != null)
            {
                transform.parent = null;
                transform.GetComponent<AudioSource>().Play();
            }
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
            meatDie.Play();
            if (bulletLeft != null)
            {
                bulletLeft.GetComponent<Bullet>().removeMe();
            }
            if (bulletRight != null)
            {
                bulletRight.GetComponent<Bullet>().removeMe();
            }
            Transform transform = transform.Find("audio_die");
            transform.parent = null;
            transform.GetComponent<AudioSource>().Play();
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
                propertiesToSet.Add(PhotonPlayerProperty.deaths, ((int) PhotonNetwork.player.CustomProperties[PhotonPlayerProperty.deaths]) + 1);
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
                obj2 = PhotonNetwork.Instantiate("hitMeat2", transform.position, Quaternion.Euler(270f, 0f, 0f), 0);
            }
            else
            {
                obj2 = (GameObject) UnityEngine.Object.Instantiate(Resources.Load("hitMeat2"));
            }
            obj2.transform.position = transform.position;
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
            meatDie.Play();
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
            BreakApart2(v, isBite);
            if (photonView.isMine)
            {
                currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().SetSpectorMode(false);
                currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().gameOver = true;
                FengGameManagerMKII.instance.myRespawnTime = 0f;
            }
            hasDied = true;
            Transform transform = transform.Find("audio_die");
            transform.parent = null;
            transform.GetComponent<AudioSource>().Play();
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
        public void NetGrabbed(int id, bool leftHand)
        {
            titanWhoGrabMeID = id;
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
        private void NetPauseAnimation()
        {
            IEnumerator enumerator = Animation.GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    AnimationState current = (AnimationState) enumerator.Current;
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
        private void NetSetIsGrabbedFalse()
        {
            state = HERO_STATE.Idle;
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
        public void NetUngrabbed()
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
                UnityEngine.Object.Destroy(myNetWorkName);
            }
            if (gunDummy != null)
            {
                Destroy(gunDummy);
            }
            ReleaseIfIHookSb();

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
                Rigidbody.AddForce((-Rigidbody.velocity * 0.9f), ForceMode.VelocityChange);
                float num = Mathf.Pow(launchForce.magnitude, 0.1f);
                if (grounded)
                {
                    Rigidbody.AddForce((Vector3.up * Mathf.Min((float) (launchForce.magnitude * 0.2f), (float) 10f)), ForceMode.Impulse);
                }
                Rigidbody.AddForce(((launchForce * num) * 0.1f), ForceMode.Impulse);
                if (state != HERO_STATE.Grab)
                {
                    dashTime = 1f;
                    CrossFade("dash", 0.05f);
                    Animation["dash"].time = 0.1f;
                    state = HERO_STATE.AirDodge;
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
            state = HERO_STATE.Salute;
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
                        if (useGun && (state != HERO_STATE.Attack))
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
                if (state != HERO_STATE.Attack)
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
                        myCannonPlayer = myCannonFind("PlayerPoint");
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

        public void SetStat2()
        {
            skillCDLast = 1.5f;
            //skillId = setup.myCostume.stat.skillId;
            skillId = "levi";
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
            //speed = ((float)setup.myCostume.stat.SPD) / 10f;
            //totalGas = currentGas = setup.myCostume.stat.GAS;
            //totalBladeSta = currentBladeSta = setup.myCostume.stat.BLA;
            //baseRigidBody.mass = 0.5f - ((setup.myCostume.stat.ACL - 100) * 0.001f);
            speed = 100 / 10f;
            totalGas = currentGas = 125;
            totalBladeSta = currentBladeSta = 100;
            Rigidbody.mass = 0.5f - (125 - 100 * 0.001f);
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
                gunDummy.transform.position = transform.position;
                gunDummy.transform.rotation = transform.rotation;
                SetTeam2(2);
            }
            else if (/*setup.myCostume.sex == SEX.FEMALE*/false)
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
            var flare = Service.Inventory.GetItems<Assets.Scripts.Items.Flare>()[type - 1];
            flare.Use(this);
        }

        private void ShowAimUI2()
        {
            Vector3 vector;
            if (MenuManager.IsAnyMenuOpen)
            {
                GameObject cross1 =  this.cross1;
                GameObject cross2 =  this.cross2;
                GameObject crossL1 = this.crossL1;
                GameObject crossL2 = this.crossL2;
                GameObject crossR1 = this.crossR1;
                GameObject crossR2 = this.crossR2;
                var labelDistance = LabelDistance;
                vector = (Vector3.up * 10000f);
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
                CheckTitan();
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                LayerMask mask = ((int) 1) << LayerMask.NameToLayer("Ground");
                LayerMask mask2 = ((int) 1) << LayerMask.NameToLayer("EnemyBox");
                LayerMask mask3 = mask2 | mask;
                var distance = "???";
                var magnitude = HookRaycastDistance;
                var hitDistance = HookRaycastDistance;
                var hitPoint = ray.GetPoint(hitDistance);

                var mousePos = Input.mousePosition;
                cross1.transform.position = mousePos;
                cross2.transform.localPosition = cross1.transform.localPosition;

                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, 1000f, mask3.value))
                {
                    magnitude = (hit.point - transform.position).magnitude;
                    distance = ((int) magnitude).ToString();
                    hitDistance = hit.distance;
                    hitPoint = hit.point;
                }

                if (magnitude > 120f)
                {
                    cross1.transform.localPosition += (Vector3.up * 10000f);
                    LabelDistance.gameObject.transform.localPosition = cross2.transform.localPosition;
                }
                else
                {
                    cross2.transform.localPosition += (Vector3.up * 10000f);
                    LabelDistance.gameObject.transform.localPosition = cross1.transform.localPosition;
                }
                LabelDistance.gameObject.transform.localPosition -= new Vector3(0f, 15f, 0f);

                if (((int) FengGameManagerMKII.settings[0xbd]) == 1)
                {
                    distance += "\n" + currentSpeed.ToString("F1") + " u/s";
                }
                else if (((int) FengGameManagerMKII.settings[0xbd]) == 2)
                {
                    distance += "\n" + ((currentSpeed / 100f)).ToString("F1") + "K";
                }
                LabelDistance.text = distance;

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
                if (Physics.Linecast(transform.position + vector2, (transform.position + vector2) + vector4, out hit2, mask3.value))
                {
                    hitPoint = hit2.point;
                    hitDistance = hit2.distance;
                }

                crossL1.transform.position = currentCamera.WorldToScreenPoint(hitPoint);
                crossL1.transform.localRotation = Quaternion.Euler(0f, 0f, (Mathf.Atan2(crossL1.transform.position.y - mousePos.y, crossL1.transform.position.x - mousePos.x) * Mathf.Rad2Deg) + 180f);
                crossL2.transform.localPosition = crossL1.transform.localPosition;
                crossL2.transform.localRotation = crossL1.transform.localRotation;
                if (hitDistance > 120f)
                    crossL1.transform.localPosition += (Vector3.up * 10000f);
                else
                    crossL2.transform.localPosition += (Vector3.up * 10000f);

                hitPoint = (transform.position + vector3) + vector5;
                hitDistance = HookRaycastDistance;
                if (Physics.Linecast(transform.position + vector3, (transform.position + vector3) + vector5, out hit2, mask3.value))
                {
                    hitPoint = hit2.point;
                    hitDistance = hit2.distance;
                }

                crossR1.transform.position = currentCamera.WorldToScreenPoint(hitPoint);
                crossR1.transform.localRotation = Quaternion.Euler(0f, 0f, Mathf.Atan2(crossR1.transform.position.y - mousePos.y, crossR1.transform.position.x - mousePos.x) * Mathf.Rad2Deg);
                crossR2.transform.localPosition = crossR1.transform.localPosition;
                crossR2.transform.localRotation = crossR1.transform.localRotation;
                if (hitDistance > 120f)
                    crossR1.transform.localPosition += Vector3.up * 10000f;
                else
                    crossR2.transform.localPosition += Vector3.up * 10000f;
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
                speed = (int) (speed * 0.1f);
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

                if ((smoke_3dmg.enableEmission) && photonView.isMine)
                {
                    object[] parameters = new object[] { false };
                    photonView.RPC(nameof(Net3DMGSMOKE), PhotonTargets.Others, parameters);
                }
                smoke_3dmg.enableEmission = false;
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

        private void Suicide2()
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
            state = HERO_STATE.Idle;
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
                transform.position = eren_titan.transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/neck").position;
                gameObject.GetComponent<SmoothSyncMovement>().disabled = true;
            }
            else if (isCannon && (myCannon != null))
            {
                UpdateCannon();
                gameObject.GetComponent<SmoothSyncMovement>().disabled = true;
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

            if (Skill != null)
            {
                if (Skill.IsActive)
                {
                    Skill.OnUpdate();
                }
                else if (InputManager.KeyDown(InputHuman.AttackSpecial))
                {
                    if (!Skill.Use() && _state == HERO_STATE.Idle)
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

            if ((state == HERO_STATE.Grab) && !useGun)
            {
                if (skillId == "eren")
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
                bool isBothHooksPressed;
                System.Boolean isRightHookPressed;
                System.Boolean isLeftHookPressed;
                BufferUpdate();
                UpdateExt();
                if (!grounded && (state != HERO_STATE.AirDodge))
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
                if (grounded && ((state == HERO_STATE.Idle) || (state == HERO_STATE.Slide)))
                {
                    if (!((!InputManager.KeyDown(InputHuman.Jump) || Animation.IsPlaying("jump")) || Animation.IsPlaying("horse_geton")))
                    {
                        Idle();
                        CrossFade("jump", 0.1f);
                        sparks.enableEmission = false;
                    }
                    if (!((!InputManager.KeyDown(InputHorse.Mount) || Animation.IsPlaying("jump")) || Animation.IsPlaying("horse_geton")) && (((myHorse != null) && !isMounted) && (Vector3.Distance(myHorse.transform.position, transform.position) < 15f)))
                    {
                        GetOnHorse();
                    }
                    if (!((!InputManager.KeyDown(InputHuman.Dodge) || Animation.IsPlaying("jump")) || Animation.IsPlaying("horse_geton")))
                    {
                        Dodge2(false);
                        return;
                    }
                }
                if (state == HERO_STATE.Idle)
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
                            Suicide2();
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
                                Rigidbody.AddForce((gameObject.transform.forward * 200f));
                            }
                            PlayAnimation(attackAnimation);
                            Animation[attackAnimation].time = 0f;
                            buttonAttackRelease = false;
                            state = HERO_STATE.Attack;
                            if ((grounded || (attackAnimation == "attack3_1")) || ((attackAnimation == "attack5") || (attackAnimation == "special_petra")))
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
                            Ray ray3 = Camera.main.ScreenPointToRay(Input.mousePosition);
                            LayerMask mask7 = ((int) 1) << LayerMask.NameToLayer("Ground");
                            LayerMask mask8 = ((int) 1) << LayerMask.NameToLayer("EnemyBox");
                            LayerMask mask9 = mask8 | mask7;
                            if (Physics.Raycast(ray3, out hit3, 1E+07f, mask9.value))
                            {
                                gunTarget = hit3.point;
                            }
                        }
                        bool flag4 = false;
                        bool flag5 = false;
                        bool flag6 = false;
                        //TODO: AHSS skill dual shot
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
                            state = HERO_STATE.Attack;
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
                else if (state == HERO_STATE.Attack)
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
                                Debug.Log("Trying to freeze");
                                SetAnimationSpeed(attackAnimation, 0f);
                            }
                        }
                        if ((attackAnimation == "attack3_1") && (currentBladeSta > 0f))
                        {
                            if (Animation[attackAnimation].normalizedTime >= 0.8f)
                            {
                                if (!checkBoxLeft.GetComponent<TriggerColliderWeapon>().IsActive)
                                {
                                    checkBoxLeft.GetComponent<TriggerColliderWeapon>().IsActive = true;
                                    if (((int) FengGameManagerMKII.settings[0x5c]) == 0)
                                    {
                                        /*
                                                leftbladetrail2.Activate();
                                                rightbladetrail2.Activate();
                                                leftbladetrail.Activate();
                                                rightbladetrail.Activate();
                                                */
                                    }
                                    Rigidbody.velocity = (-Vector3.up * 30f);
                                }
                                if (!checkBoxRight.GetComponent<TriggerColliderWeapon>().IsActive)
                                {
                                    checkBoxRight.GetComponent<TriggerColliderWeapon>().IsActive = true;
                                    slash.Play();
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
                            if ((Animation[attackAnimation].normalizedTime > num2) && (Animation[attackAnimation].normalizedTime < num))
                            {
                                if (!checkBoxLeft.GetComponent<TriggerColliderWeapon>().IsActive)
                                {
                                    checkBoxLeft.GetComponent<TriggerColliderWeapon>().IsActive = true;
                                    slash.Play();
                                    if (((int) FengGameManagerMKII.settings[0x5c]) == 0)
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
                            if ((attackLoop > 0) && (Animation[attackAnimation].normalizedTime > num))
                            {
                                attackLoop--;
                                PlayAnimationAt(attackAnimation, num2);
                            }
                        }
                        if (Animation[attackAnimation].normalizedTime >= 1f)
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
                                    photonView.RPC(nameof(NetlaughAttack), PhotonTargets.MasterClient, new object[0]);
                                }
                                else
                                {
                                    NetlaughAttack();
                                }
                                FalseAttack();
                                Idle();
                            }
                            else if (attackAnimation == "attack3_1")
                            {
                                Rigidbody.velocity -= ((Vector3.up * Time.deltaTime) * 30f);
                            }
                            else
                            {
                                FalseAttack();
                                Idle();
                            }
                        }
                        if (Animation.IsPlaying("attack3_2") && (Animation["attack3_2"].normalizedTime >= 1f))
                        {
                            FalseAttack();
                            Idle();
                        }
                    }
                    else
                    {
                        checkBoxLeft.GetComponent<TriggerColliderWeapon>().IsActive = false;
                        checkBoxRight.GetComponent<TriggerColliderWeapon>().IsActive = false;
                        transform.rotation = Quaternion.Lerp(transform.rotation, gunDummy.transform.rotation, Time.deltaTime * 30f);
                        if (!attackReleased && (Animation[attackAnimation].normalizedTime > 0.167f))
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
                                Rigidbody.AddForce((-transform.forward * 1000f), ForceMode.Acceleration);
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
                                obj4 = (GameObject) UnityEngine.Object.Instantiate(Resources.Load(prefabName), ((transform.position + (transform.up * 0.8f)) - (transform.right * 0.1f)), transform.rotation);
                            }
                        }
                        if (Animation[attackAnimation].normalizedTime >= 1f)
                        {
                            FalseAttack();
                            Idle();
                            checkBoxLeft.GetComponent<TriggerColliderWeapon>().IsActive = false;
                            checkBoxRight.GetComponent<TriggerColliderWeapon>().IsActive = false;
                        }
                        if (!Animation.IsPlaying(attackAnimation))
                        {
                            FalseAttack();
                            Idle();
                            checkBoxLeft.GetComponent<TriggerColliderWeapon>().IsActive = false;
                            checkBoxRight.GetComponent<TriggerColliderWeapon>().IsActive = false;
                        }
                    }
                }
                else if (state == HERO_STATE.ChangeBlade)
                {
                    Equipment.Weapon.Reload();
                    if (Animation[reloadAnimation].normalizedTime >= 1f)
                    {
                        Idle();
                    }
                }
                else if (state == HERO_STATE.Salute)
                {
                    if (Animation["salute"].normalizedTime >= 1f)
                    {
                        Idle();
                    }
                }
                else if (state == HERO_STATE.GroundDodge)
                {
                    if (Animation.IsPlaying("dodge"))
                    {
                        if (!(grounded || (Animation["dodge"].normalizedTime <= 0.6f)))
                        {
                            Idle();
                        }
                        if (Animation["dodge"].normalizedTime >= 1f)
                        {
                            Idle();
                        }
                    }
                }
                else if (state == HERO_STATE.Land)
                {
                    if (Animation.IsPlaying("dash_land") && (Animation["dash_land"].normalizedTime >= 1f))
                    {
                        Idle();
                    }
                }
                else if (state == HERO_STATE.FillGas)
                {
                    if (Animation.IsPlaying("supply") && Animation["supply"].normalizedTime >= 1f)
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
                else if (state == HERO_STATE.Slide)
                {
                    if (!grounded)
                    {
                        Idle();
                    }
                }
                else if (state == HERO_STATE.AirDodge)
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
                        Idle();
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
                // (Using Attack3_1 OR Attack5 OR Petra OR Grabbed) AND NOT IDLE
                // 

                if (!(isLeftHookPressed ? (((Animation.IsPlaying("attack3_1") || Animation.IsPlaying("attack5")) || (Animation.IsPlaying("special_petra") || (state == HERO_STATE.Grab))) ? (state != HERO_STATE.Idle) : false) : true))

                {
                    if (bulletLeft != null)
                    {
                        QHold = true;
                    }
                    else
                    {
                        RaycastHit hit4;
                        Ray ray4 = Camera.main.ScreenPointToRay(Input.mousePosition);
                        LayerMask mask10 = ((int) 1) << LayerMask.NameToLayer("Ground");
                        LayerMask mask11 = ((int) 1) << LayerMask.NameToLayer("EnemyBox");
                        LayerMask mask12 = mask11 | mask10;
                        if (Physics.Raycast(ray4, out hit4, HookRaycastDistance, mask12.value))
                        {
                            LaunchLeftRope(hit4.distance, hit4.point, true);
                        }
                        else
                        {
                            LaunchLeftRope(HookRaycastDistance, ray4.GetPoint(HookRaycastDistance), true);
                        }
                        rope.Play();
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
                if (!(isRightHookPressed ? (((Animation.IsPlaying("attack3_1") || Animation.IsPlaying("attack5")) || (Animation.IsPlaying("special_petra") || (state == HERO_STATE.Grab))) ? (state != HERO_STATE.Idle) : false) : true))
                {
                    if (bulletRight != null)
                    {
                        EHold = true;
                    }
                    else
                    {
                        RaycastHit hit5;
                        Ray ray5 = Camera.main.ScreenPointToRay(Input.mousePosition);
                        LayerMask mask13 = ((int) 1) << LayerMask.NameToLayer("Ground");
                        LayerMask mask14 = ((int) 1) << LayerMask.NameToLayer("EnemyBox");
                        LayerMask mask15 = mask14 | mask13;
                        if (Physics.Raycast(ray5, out hit5, HookRaycastDistance, mask15.value))
                        {
                            LaunchRightRope(hit5.distance, hit5.point, true);
                        }
                        else
                        {
                            LaunchRightRope(HookRaycastDistance, ray5.GetPoint(HookRaycastDistance), true);
                        }
                        rope.Play();
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
                if (!(isBothHooksPressed ? (((Animation.IsPlaying("attack3_1") || Animation.IsPlaying("attack5")) || (Animation.IsPlaying("special_petra") || (state == HERO_STATE.Grab))) ? (state != HERO_STATE.Idle) : false) : true))
                {
                    QHold = true;
                    EHold = true;
                    if ((bulletLeft == null) && (bulletRight == null))
                    {
                        RaycastHit hit6;
                        Ray ray6 = Camera.main.ScreenPointToRay(Input.mousePosition);
                        LayerMask mask16 = ((int) 1) << LayerMask.NameToLayer("Ground");
                        LayerMask mask17 = ((int) 1) << LayerMask.NameToLayer("EnemyBox");
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
                        rope.Play();
                    }
                }
                if (!IN_GAME_MAIN_CAMERA.isPausing)
                {
                    CalcSkillCD();
                }
                if (!IN_GAME_MAIN_CAMERA.isPausing)
                {
                    //showSkillCD();
                    //showFlareCD2();
                    ShowGas2();
                    ShowAimUI2();
                }
            }
        }

        public void UpdateCannon()
        {
            transform.position = myCannonPlayer.position;
            transform.rotation = myCannonrotation;
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
                    LayerMask mask = ((int) 1) << LayerMask.NameToLayer("Ground");
                    LayerMask mask2 = ((int) 1) << LayerMask.NameToLayer("EnemyBox");
                    LayerMask mask3 = mask2 | mask;
                    currentV = transform.position;
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
    }
}