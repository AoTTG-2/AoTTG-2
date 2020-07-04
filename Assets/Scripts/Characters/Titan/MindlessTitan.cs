using Assets.Scripts.Characters.Titan.Attacks;
using Assets.Scripts.Characters.Titan.Behavior;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using MonoBehaviour = Photon.MonoBehaviour;
using Random = UnityEngine.Random;

namespace Assets.Scripts.Characters.Titan
{
    public class MindlessTitan : MonoBehaviour
    {
        public MindlessTitanState TitanState = MindlessTitanState.Wandering;
        public MindlessTitanState PreviousState;
        public MindlessTitanType Type;

        public bool IsAlive => TitanState != MindlessTitanState.Dead;
        private float DamageTimer { get; set; }
        public TitanBody TitanBody { get; private set; }
        public Animation Animation { get; private set; }
        public Rigidbody Rigidbody { get; private set; }
        private TitanBehavior[] Behaviors { get; set; }

        protected string CurrentAnimation { get; set; } = "idle_2";
        protected string AnimationTurnLeft { get; set; } = "turnaround2";
        protected string AnimationTurnRight { get; set; } = "turnaround1";
        public string AnimationWalk { get; private set; } = "run_walk";
        protected string AnimationRun { get; set; }
        protected string AnimationRecovery { get; set; } = "tired";
        protected string AnimationDeath { get; set; } = "die_back";
        protected string AnimationIdle { get; set; } = "idle_2";
        protected string AnimationCover { get; set; } = "idle_recovery";
        protected string AnimationEyes { get; set; } = "hit_eye";

        private float turnDeg;
        private float desDeg;
        private int nextUpdate = 1;
        private float attackCooldown;
        private float staminaLimit;

        public float AttackDistance { get; protected set; }
        public float Speed = 10f;
        private float RunSpeed { get; set; }
        public float TargetDistance = 1f;
        public float Size = 3f;
        public float Stamina = 10f;
        public float StaminaRecovery = 1f;
        public int Health;
        public int HealthRegeneration;
        public float ViewDistance;
        public float Focus = 10f;
        private float FocusTimer { get; set; }
        private int MaxHealth { get; set; }

        private bool isHooked;
        public bool IsHooked
        {
            get { return isHooked; }
            set
            {
                if (value == isHooked) return;
                isHooked = value;
                CheckColliders();
            }
        }

        private bool isLooked;
        public bool IsLooked
        {
            get { return isLooked; }
            set
            {
                if (value == isLooked) return;
                isLooked = value;
                CheckColliders();
            }
        }

        private bool isColliding;
        public bool IsColliding
        {
            get { return isColliding; }
            set
            {
                if (value == isColliding) return;
                isColliding = value;
                CheckColliders();
            }
        }

        public Hero Target { get; set; }
        private Hero GrabTarget { get; set; }
        public float RotationModifier { get; private set; }

        public Attack[] Attacks { get; private set; }
        public Attack CurrentAttack { get; set; }
        private Collider[] Colliders { get; set; }
        private GameObject HealthLabel { get; set; }
        private FengGameManagerMKII GameManager { get; set; }

        private bool asClientLookTarget;
        private Quaternion oldHeadRotation;
        private Quaternion targetHeadRotation;
        private Vector3 headscale;

        // TODO: Create components in the editor and modify them at runtime.
        protected virtual void Awake()
        {
            GameManager = FengGameManagerMKII.instance;
            GameManager.addTitan(this);
            TitanBody = GetComponent<TitanBody>();
            Animation = GetComponent<Animation>();
            Rigidbody = GetComponent<Rigidbody>();
            oldHeadRotation = TitanBody.Head.rotation;
            grabTF = new GameObject();
            grabTF.name = "titansTmpGrabTF";
            Colliders = GetComponentsInChildren<Collider>().Where(x => x.name != "AABB" && x.name != "Detection")
                .ToArray();
            CheckColliders();

            var obj2 = new GameObject
            {
                name = "PlayerCollisionDetection"
            };
            var collider2 = obj2.AddComponent<CapsuleCollider>();
            var component = TitanBody.AABB.GetComponent<CapsuleCollider>();
            collider2.center = component.center;
            collider2.radius = Math.Abs(TitanBody.Head.position.y - transform.position.y);
            collider2.height = component.height * 1.2f;
            collider2.material = component.material;
            collider2.isTrigger = true;
            collider2.name = "PlayerCollisionDetection";
            obj2.AddComponent<TitanTrigger>();
            obj2.layer = 0x10;
            obj2.transform.parent = TitanBody.AABB;
            obj2.transform.localPosition = new Vector3(0f, 0f, 0f);
        }
        
        public virtual void Initialize(TitanConfiguration configuration)
        {
            Health = configuration.Health;
            MaxHealth = Health;
            Speed = configuration.Speed;
            RunSpeed = configuration.RunSpeed;
            Attacks = configuration.Attacks.ToArray();
            Size = configuration.Size;
            ViewDistance = configuration.ViewDistance;
            AnimationWalk = configuration.AnimationWalk;
            AnimationRun = configuration.AnimationRun;
            AnimationDeath = configuration.AnimationDeath;
            AnimationRecovery = configuration.AnimationRecovery;
            AnimationTurnLeft = configuration.AnimationTurnLeft;
            AnimationTurnRight = configuration.AnimationTurnRight;
            Stamina = configuration.Stamina;
            StaminaRecovery = configuration.StaminaRegeneration;
            staminaLimit = Stamina;
            Focus = configuration.Focus;
            FocusTimer = 0f;
            Behaviors = configuration.Behaviors.ToArray();
            foreach (var behavior in Behaviors)
            {
                behavior.Initialize(this);
            }
            Type = configuration.Type;
            name = Type.ToString();

            TitanBody.Initialize(configuration.LimbHealth, configuration.LimbRegeneration);

            transform.localScale = new Vector3(Size, Size, Size);
            var scale = Mathf.Min(Mathf.Pow(2f / Size, 0.35f), 1.25f);
            headscale = new Vector3(scale, scale, scale);
            TitanBody.Head.localScale = headscale;
            LoadSkin();

            AttackDistance = Vector3.Distance(transform.position, TitanBody.AttackFrontGround.position) * 1.65f;

            if (Health > 0)
            {
                HealthLabel = (GameObject)Instantiate(Resources.Load("UI/LabelNameOverHead"));
                HealthLabel.name = "HealthLabel";
                HealthLabel.transform.parent = transform;
                HealthLabel.transform.localPosition = new Vector3(0f, 20f + (1f / Size), 0f);
                if (Type == MindlessTitanType.Crawler)
                {
                    HealthLabel.transform.localPosition = new Vector3(0f, 10f + (1f / Size), 0f);
                }
                var x = 1f;
                if (Size < 1f)
                {
                    x = 1f / Size;
                }

                x *= 0.08f;
                HealthLabel.transform.localScale = new Vector3(x, x, x);
            }

            if (photonView.isMine)
            {
                configuration.Behaviors = new List<TitanBehavior>();
                var config = JsonConvert.SerializeObject(configuration);
                photonView.RPC(nameof(InitializeRpc), PhotonTargets.OthersBuffered, config);

                if (Health > 0)
                    photonView.RPC(nameof(UpdateHealthLabelRpc), PhotonTargets.AllBuffered, Health, MaxHealth);
            }
        }

        [PunRPC]
        public void InitializeRpc(string titanConfiguration, PhotonMessageInfo info)
        {
            if (info.sender.ID == photonView.ownerId)
                Initialize(JsonConvert.DeserializeObject<TitanConfiguration>(titanConfiguration));
        }

        public bool IsDisabled(params BodyPart[] bodyParts)
        {
            if (bodyParts == null) return false;
            return bodyParts.All(bodyPart => TitanBody.GetDisabledBodyParts().Contains(bodyPart));
        }

        private void LoadSkin()
        {
            var eye = false;
            if (!(((IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE) || photonView.isMine) ? (((int)FengGameManagerMKII.settings[1]) != 1) : true))
            {
                var index = (int) Random.Range((float)86f, (float)90f);
                var num2 = index - 60;
                if (((int)FengGameManagerMKII.settings[0x20]) == 1)
                {
                    num2 = Random.Range(0x1a, 30);
                }
                var body = (string)FengGameManagerMKII.settings[index];
                var eyes = (string)FengGameManagerMKII.settings[num2];
                var skin = index;
                if ((eyes.EndsWith(".jpg") || eyes.EndsWith(".png")) || eyes.EndsWith(".jpeg"))
                {
                    eye = true;
                }
                GetComponent<TITAN_SETUP>().setVar(skin, eye);
            }
            GetComponent<TITAN_SETUP>().setHair2();
        }

        [PunRPC]
        private void setIfLookTarget(bool bo)
        {
            asClientLookTarget = bo;
        }

        public GameObject grabTF { get; set; }

        [PunRPC]
        public void Grab(bool isLeftHand)
        {
            var hand = isLeftHand ? TitanBody.HandLeft : TitanBody.HandRight;
            grabTF.transform.parent = hand;
            grabTF.transform.position = hand.GetComponent<SphereCollider>().transform.position;
            grabTF.transform.rotation = hand.GetComponent<SphereCollider>().transform.rotation;
            var transform1 = grabTF.transform;
            transform1.localPosition -= Vector3.right * hand.GetComponent<SphereCollider>().radius * 0.3f;
            var transform2 = grabTF.transform;
            if (isLeftHand)
            {
                transform2.localPosition -= Vector3.up * hand.GetComponent<SphereCollider>().radius * 0.51f;
            }
            else
            {
                transform2.localPosition += Vector3.up * hand.GetComponent<SphereCollider>().radius * 0.51f;
            }

            var transform3 = grabTF.transform;
            transform3.localPosition -= Vector3.forward * hand.GetComponent<SphereCollider>().radius * 0.3f;

            var zRotation = isLeftHand ? 180f : 0f;
            grabTF.transform.localRotation = Quaternion.Euler(grabTF.transform.localRotation.eulerAngles.x, grabTF.transform.localRotation.eulerAngles.y + 180f, grabTF.transform.localRotation.eulerAngles.z + zRotation);
        }

        [PunRPC]
        public void GrabEscapeRpc()
        {
            GrabTarget = null;
        }

        private void KillGrabbedTarget(Hero grabTarget)
        {
            if (grabTarget != null)
            {
                if ((IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER) && photonView.isMine)
                {
                    if (!grabTarget.HasDied())
                    {
                        grabTarget.markDie();
                        grabTarget.photonView.RPC(nameof(grabTarget.netDie2), PhotonTargets.All, -1, name);
                    }
                }
                else if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
                {
                    grabTarget.die2(null);
                }
            }
        }

        private void HeadMovement()
        {
            if (TitanState != MindlessTitanState.Dead)
            {

                if (IN_GAME_MAIN_CAMERA.gametype != GAMETYPE.SINGLE)
                {
                    if (photonView.isMine)
                    {
                        targetHeadRotation = TitanBody.Head.rotation;
                        var flag2 = false;
                        if (TitanState == MindlessTitanState.Chase && TargetDistance < 100f && Target != null)
                        {
                            var vector = Target.transform.position - transform.position;
                            var angle = -Mathf.Atan2(vector.z, vector.x) * 57.29578f;
                            var num = -Mathf.DeltaAngle(angle, transform.rotation.eulerAngles.y - 90f);
                            num = Mathf.Clamp(num, -40f, 40f);
                            var y = (TitanBody.Neck.position.y + (Size * 2f)) - Target.transform.position.y;
                            var num3 = Mathf.Atan2(y, TargetDistance) * 57.29578f;
                            num3 = Mathf.Clamp(num3, -40f, 30f);
                            targetHeadRotation = Quaternion.Euler(TitanBody.Head.rotation.eulerAngles.x + num3, TitanBody.Head.rotation.eulerAngles.y + num, TitanBody.Head.rotation.eulerAngles.z);
                            if (!asClientLookTarget)
                            {
                                asClientLookTarget = true;
                                photonView.RPC(nameof(setIfLookTarget), PhotonTargets.Others, true);
                            }
                            flag2 = true;
                        }
                        if (!(flag2 || !asClientLookTarget))
                        {
                            asClientLookTarget = false;
                            photonView.RPC(nameof(setIfLookTarget), PhotonTargets.Others, false);
                        }
                        if (TitanState == MindlessTitanState.Attacking)
                        {
                            oldHeadRotation = Quaternion.Lerp(oldHeadRotation, targetHeadRotation, Time.deltaTime * 20f);
                        }
                        else
                        {
                            oldHeadRotation = Quaternion.Lerp(oldHeadRotation, targetHeadRotation, Time.deltaTime * 10f);
                        }
                    }
                    else
                    {
                        var hasTarget = Target != null;
                        if (hasTarget)
                        {
                            TargetDistance = Mathf.Sqrt(((Target.transform.position.x - transform.position.x) * (Target.transform.position.x - transform.position.x)) + ((Target.transform.position.z - transform.position.z) * (Target.transform.position.z - transform.position.z)));
                        }
                        else
                        {
                            TargetDistance = float.MaxValue;
                        }
                        targetHeadRotation = TitanBody.Head.rotation;
                        if ((asClientLookTarget && hasTarget) && (TargetDistance < 100f))
                        {
                            var vector2 = Target.transform.position - transform.position;
                            var angle = -Mathf.Atan2(vector2.z, vector2.x) * 57.29578f;
                            var num4 = -Mathf.DeltaAngle(angle, transform.rotation.eulerAngles.y - 90f);
                            num4 = Mathf.Clamp(num4, -40f, 40f);
                            var num5 = (TitanBody.Neck.position.y + (Size * 2f)) - Target.transform.position.y;
                            var num6 = Mathf.Atan2(num5, TargetDistance) * 57.29578f;
                            num6 = Mathf.Clamp(num6, -40f, 30f);
                            targetHeadRotation = Quaternion.Euler(TitanBody.Head.rotation.eulerAngles.x + num6, TitanBody.Head.rotation.eulerAngles.y + num4, TitanBody.Head.rotation.eulerAngles.z);
                        }
                        oldHeadRotation = Quaternion.Slerp(oldHeadRotation, targetHeadRotation, Time.deltaTime * 10f);
                    }
                }
                TitanBody.Head.rotation = oldHeadRotation;
            }
            if (!GetComponent<Animation>().IsPlaying("die_headOff"))
            {
                TitanBody.Head.localScale = headscale;
            }
        }

        [PunRPC]
        protected void UpdateHealthLabelRpc(int currentHealth, int maxHealth)
        {
            if (currentHealth < 0)
            {
                if (HealthLabel != null)
                {
                    Destroy(HealthLabel);
                }
            }
            else
            {
                var color = "7FFF00";
                var num2 = ((float)currentHealth) / ((float)maxHealth);
                if ((num2 < 0.75f) && (num2 >= 0.5f))
                {
                    color = "f2b50f";
                }
                else if ((num2 < 0.5f) && (num2 >= 0.25f))
                {
                    color = "ff8100";
                }
                else if (num2 < 0.25f)
                {
                    color = "ff3333";
                }
                HealthLabel.GetComponent<TextMesh>().text = $"<color=#{color}>{currentHealth}</color>";
            }
        }

        public void OnAnkleHit(int viewId, int damage) { }

        private float bodyPartDamageTimer;
        [PunRPC]
        public void OnBodyPartHitRpc(BodyPart bodyPart, int damage)
        {
            if (!photonView.isMine) return;
            if (Time.time - bodyPartDamageTimer < 0.4f) return;
            bodyPartDamageTimer = Time.time;
            TitanBody.DamageBodyPart(bodyPart, damage);
            if (TitanBody.GetDisabledBodyParts().Any(x => x == BodyPart.LegLeft)
                && TitanBody.GetDisabledBodyParts().Any(x => x == BodyPart.LegRight))
            {
                ChangeState(MindlessTitanState.Disabled);
            }
        }

        [PunRPC]
        public void OnCannonHitRpc(int viewId, string bodyPart)
        {
            if (!IsAlive) return;
            var attacker = PhotonView.Find(viewId).gameObject.GetComponent<Hero>();
            if (attacker == null)
            {
                Debug.LogWarning("Attacker is null");
                return;
            }

            var isCrawler = Type == MindlessTitanType.Crawler;
            if (bodyPart == "head")
            {
                var damage = 0;
                FengGameManagerMKII.instance.titanGetKill(attacker.photonView.owner, damage, name);
                OnTitanDeath();
                photonView.RPC(
                    nameof(OnKilledByCannon),
                    PhotonTargets.All,
                    attacker.transform.position,
                    isCrawler);
            }
            else if (Random.Range(0f, 1f) < 0.5f)
            {
                OnCannonBodyHit("hit_eren_L", attacker, isCrawler);
            }
            else
            {
                OnCannonBodyHit("hit_eren_R", attacker, isCrawler);
            }
        }

        private void OnCannonBodyHit(string animation, Hero attacker, bool isCrawler)
        {
            Target = attacker;
            CurrentAnimation = animation;

            if (isCrawler) return;
            ChangeState(MindlessTitanState.Hit);
            ReleaseGrabbedTarget();
            transform.rotation = Quaternion.Euler(0f, Quaternion.LookRotation(attacker.transform.position - transform.position).eulerAngles.y, 0f);
        }

        [PunRPC]
        public void OnEyeHitRpc(int viewId, int damage)
        {
            if (Type == MindlessTitanType.Crawler) return;
            if (!photonView.isMine) return;
            if (!IsAlive) return;
            TitanBody.AddBodyPart(BodyPart.Eyes, Animation[AnimationEyes].length * Animation[AnimationEyes].speed);
            if (TitanBody.GetDisabledBodyParts().Any(x => x == BodyPart.Eyes))
            {
                ChangeState(MindlessTitanState.Disabled);
            }
        }

        [PunRPC]
        private void OnKilledByCannon(Vector3 attacker, bool isCrawler)
        {
            if (photonView.isMine)
            {
                if (isCrawler)
                {
                    KillByKnockBack(attacker);
                }
                else
                {
                    KillByHeadExploded(attacker);
                }
            }
        }

        private void KillByHeadExploded(Vector3 attacker)
        {
            GameObject obj2;
            AnimationDeath = "die_headOff";
            ChangeState(MindlessTitanState.Dead);
            //TODO: Issue 98 Sounds
            //this.playSound("snd_titan_head_blow");
            transform.rotation = Quaternion.Euler(0f, Quaternion.LookRotation(attacker - transform.position).eulerAngles.y, 0f);
            if (photonView.isMine)
            {
                obj2 = PhotonNetwork.Instantiate("bloodExplore", TitanBody.Head.position + ((Vector3)((Vector3.up * 1f) * Size)), Quaternion.Euler(270f, 0f, 0f), 0);
            }
            else
            {
                obj2 = (GameObject) Instantiate(Resources.Load("bloodExplore"), TitanBody.Head.position + ((Vector3)((Vector3.up * 1f) * Size)), Quaternion.Euler(270f, 0f, 0f));
            }
            obj2.transform.localScale = transform.localScale;
            if ((IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER) && photonView.isMine)
            {
                obj2 = PhotonNetwork.Instantiate("bloodsplatter", TitanBody.Head.position, Quaternion.Euler(270f + TitanBody.Neck.rotation.eulerAngles.x, TitanBody.Neck.rotation.eulerAngles.y, TitanBody.Neck.rotation.eulerAngles.z), 0);
            }
            else
            {
                obj2 = (GameObject) Instantiate(Resources.Load("bloodsplatter"), TitanBody.Head.position, Quaternion.Euler(270f + TitanBody.Neck.rotation.eulerAngles.x, TitanBody.Neck.rotation.eulerAngles.y, TitanBody.Neck.rotation.eulerAngles.z));
            }
            obj2.transform.localScale = transform.localScale;
            obj2.transform.parent = TitanBody.Neck;
            if ((IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER) && photonView.isMine)
            {
                obj2 = PhotonNetwork.Instantiate("FX/justSmoke", TitanBody.Neck.position, Quaternion.Euler(270f, 0f, 0f), 0);
            }
            else
            {
                obj2 = (GameObject) Instantiate(Resources.Load("FX/justSmoke"), TitanBody.Neck.position, Quaternion.Euler(270f, 0f, 0f));
            }

            obj2.transform.parent = TitanBody.Neck;
        }

        private void KillByKnockBack(Vector3 attacker)
        {
            transform.rotation = Quaternion.Euler(0f, Quaternion.LookRotation(attacker - transform.position).eulerAngles.y, 0f);
            ChangeState(MindlessTitanState.Dead);
            AnimationDeath = "die_blow";
        }

        [PunRPC]
        public void OnNapeHitRpc(int viewId, int damage)
        {
            if (!IsAlive) return;
            var view = PhotonView.Find(viewId);
            if (view == null || !IsAlive && Time.time - DamageTimer > 0.2f) return;
            if (damage < FengGameManagerMKII.Gamemode.Settings.DamageMode) return;

            DamageTimer = Time.time;
            Health -= damage;

            if (MaxHealth > 0)
                photonView.RPC(nameof(UpdateHealthLabelRpc), PhotonTargets.AllBuffered, Health, MaxHealth);

            if (Health > 0)
                return;

            Health = 0;

            OnTitanDeath();
            ChangeState(MindlessTitanState.Dead);
            FengGameManagerMKII.instance.titanGetKill(view.owner, damage, name);
        }

        public void OnTargetGrabbed(GameObject target, bool isLeftHand)
        {
            ChangeState(MindlessTitanState.Eat);
            GrabTarget = target.GetComponent<Hero>();
            if (isLeftHand)
            {
                CurrentAnimation = "eat_l";
            }
            else
            {
                CurrentAnimation = "eat_r";
            }
        }

        protected virtual void OnTitanDeath()
        {
            ReleaseGrabbedTarget();
            if (FengGameManagerMKII.Gamemode.Settings.TitanExplodeMode > 0)
                Invoke("Explode", 1f);
        }

        private void ReleaseGrabbedTarget()
        {
            GrabTarget?.photonView.RPC(nameof(GrabTarget.netUngrabbed), PhotonTargets.All);
        }

        private void Explode()
        {
            var height = Size * 10f;
            if (Type == MindlessTitanType.Crawler)
            {
                height = 0f;
            }
            var position = transform.position + Vector3.up * height;
            PhotonNetwork.Instantiate("FX/Thunder", position, Quaternion.Euler(270f, 0f, 0f), 0);
            PhotonNetwork.Instantiate("FX/boom1", position, Quaternion.Euler(270f, 0f, 0f), 0);
            foreach (Hero player in FengGameManagerMKII.instance.getPlayers())
            {
                if (Vector3.Distance(player.transform.position, position) < FengGameManagerMKII.Gamemode.Settings.TitanExplodeMode)
                {
                    player.markDie();
                    player.photonView.RPC(nameof(player.netDie2), PhotonTargets.All);
                }
            }
        }

        private bool HasDieSteam { get; set; }
        protected void Dead()
        {
            if (!Animation.IsPlaying(AnimationDeath))
            {
                CrossFade(AnimationDeath, 0.05f);
            }
            var deathTime = Animation[AnimationDeath].normalizedTime;
            if (deathTime > 2f && !HasDieSteam && photonView.isMine)
            {
                HasDieSteam = true;
                PhotonNetwork.Instantiate("FX/FXtitanDie1", TitanBody.Hip.position, Quaternion.Euler(-90f, 0f, 0f), 0).transform.localScale = transform.localScale;
            }
            if (deathTime > 5f)
            {
                if (photonView.isMine)
                {
                    PhotonNetwork.Instantiate("FX/FXtitanDie", TitanBody.Hip.position, Quaternion.Euler(-90f, 0f, 0f), 0).transform.localScale = transform.localScale;
                    PhotonNetwork.Destroy(gameObject);
                }
            }
        }

        public bool HasTarget()
        {
            return Target != null;
        }

        public void OnTargetDetected(GameObject target)
        {
            Target = target.GetComponent<Hero>();
            TargetDistance = float.MaxValue;
            ChangeState(MindlessTitanState.Chase);
            FocusTimer = 0f;
            oldHeadRotation = TitanBody.Head.rotation;
        }

        public void ChangeState(MindlessTitanState state)
        {
            if (!IsAlive) return;
            if (state == TitanState) return;
            PreviousState = TitanState;
            TitanState = state;
        }

        protected void RefreshStamina()
        {
            _staminaCooldown -= Time.deltaTime;
            if (Stamina >= staminaLimit) return;
            Stamina += StaminaRecovery * Time.deltaTime;
            if (Stamina > staminaLimit)
            {
                Stamina = staminaLimit;
            }
        }

        private void CalculateTargetDistance()
        {
            TargetDistance = Target == null
                ? float.MaxValue
                : Mathf.Sqrt((Target.transform.position.x - transform.position.x) * (Target.transform.position.x - transform.position.x) + ((Target.transform.position.z - transform.position.z) * (Target.transform.position.z - transform.position.z)));
        }

        private void Turn(float degrees)
        {
            ChangeState(MindlessTitanState.Turning);
            CurrentAnimation = degrees > 0f ? AnimationTurnLeft : AnimationTurnRight;
            CrossFade(CurrentAnimation, 0.0f);
            turnDeg = degrees;
            desDeg = gameObject.transform.rotation.eulerAngles.y + turnDeg;
        }

        private bool Between(float value, float min = -1f, float max = 1f)
        {
            return value > min && value < max;
        }

        public bool IsStuck()
        {
            var velocity = Rigidbody.velocity;
            return Between(velocity.z, -Speed / 4, Speed / 4) 
                   && Between(velocity.x, -Speed / 4, Speed / 4) 
                   && Animation[CurrentAnimation].normalizedTime > 2f;
        }
        
        public void CrossFade(string newAnimation, float fadeLength, PhotonMessageInfo info = new PhotonMessageInfo())
        {
            if (Animation.IsPlaying(newAnimation)) return;
            if (photonView.isMine)
            {
                CurrentAnimation = newAnimation;
                Animation.CrossFade(newAnimation, fadeLength);
                photonView.RPC(
                    nameof(CrossFadeRpc),
                    PhotonTargets.Others,
                    newAnimation,
                    fadeLength);
            }
        }

        [PunRPC]
        protected void CrossFadeRpc(string newAnimation, float fadeLength,
            PhotonMessageInfo info = new PhotonMessageInfo())
        {
            if (info.sender.ID == photonView.owner.ID)
            {
                CurrentAnimation = newAnimation;
                Animation.CrossFade(newAnimation, fadeLength);
            }
        }

        private void CheckColliders()
        {
            if (!IsHooked && !IsLooked && !IsColliding)
            {
                foreach (var collider in Colliders)
                {
                    if (collider != null)
                    {
                        collider.enabled = false;
                    }
                }
            }
            else if (IsHooked || IsLooked || IsColliding)
            {
                foreach (var collider in Colliders)
                {
                    if (collider != null)
                    {
                        collider.enabled = true;
                    }
                }
            }
        }

        private float _staminaCooldown;
        private bool CanRun()
        {
            if (AnimationRun == null) return false;
            if (_staminaCooldown > 0) return false;

            if (Stamina <= staminaLimit / 4)
            {
                _staminaCooldown = 10f;
                return false;
            }
            return true;
        }

        private void OnTargetRefresh()
        {
            FocusTimer = 0f;
            var targetDistance = float.PositiveInfinity;
            var position = transform.position;
            foreach (Hero hero in FengGameManagerMKII.instance.getPlayers())
            {
                var distance = Vector3.Distance(hero.gameObject.transform.position, position);
                if (distance < targetDistance)
                {
                    Target = hero;
                    targetDistance = distance;
                }
            }
        }

        private void OnDestroy()
        {
            FengGameManagerMKII.instance.removeTitan(this);
        }
        
        private void Pathfinding()
        {
            var forwardDirection = TitanBody.Hip.transform.TransformDirection(new Vector3(-0.3f, 0, 1f));
            RaycastHit objectHit;
            var mask = ~LayerMask.NameToLayer("Ground");
            if (Physics.Raycast(TitanBody.Hip.transform.position, forwardDirection, out objectHit, 50, mask))
            {
                var leftDirection = TitanBody.Hip.transform.TransformDirection(new Vector3(-0.3f, -1f, 1f));
                var rightDirection = TitanBody.Hip.transform.TransformDirection(new Vector3(-0.3f, 1f, 1f));
                RaycastHit leftHit;
                RaycastHit rightHit;
                Physics.Raycast(TitanBody.Hip.transform.position, leftDirection, out leftHit, 250 , mask);
                Physics.Raycast(TitanBody.Hip.transform.position, rightDirection, out rightHit, 250, mask);

                if (leftHit.distance < rightHit.distance)
                {
                    RotationModifier += Random.Range(10f, 30f);
                }
                else
                {
                    RotationModifier += Random.Range(-30f, -10f); ;
                }

                return;
            }

            RotationModifier = 0f;
        }



        #region OnUpdate

        protected virtual void Update()
        {
            if (!photonView.isMine) return;

            if (!IsAlive)
            {
                Dead();
                return;
            }

            RefreshStamina();
            CalculateTargetDistance();

            if (Time.time >= nextUpdate)
            {
                nextUpdate = Mathf.FloorToInt(Time.time) + 1;
                UpdateEverySecond(nextUpdate);
            }

            if (Stamina < 0 && TitanState != MindlessTitanState.Recovering && TitanState != MindlessTitanState.Disabled)
            {
                ChangeState(MindlessTitanState.Recovering);
            }

            if (Behaviors != null && Behaviors.Any(x => x.OnUpdate()))
            {
                return;
            }


            switch (TitanState)
            {
                case MindlessTitanState.Disabled:
                    OnDisabled();
                    break;
                case MindlessTitanState.Idle:
                    break;
                case MindlessTitanState.Dead:
                    break;
                case MindlessTitanState.Hit:
                    OnHit();
                    break;
                case MindlessTitanState.Wandering:
                    OnWandering();
                    break;
                case MindlessTitanState.Turning:
                    OnTurning();
                    break;
                case MindlessTitanState.Chase:
                    OnChasing();
                    break;
                case MindlessTitanState.Attacking:
                    OnAttacking();
                    break;
                case MindlessTitanState.Recovering:
                    OnRecovering();
                    break;
                case MindlessTitanState.Eat:
                    OnGrabbing();
                    break;
            }
        }

        private void UpdateEverySecond(int seconds)
        {
            if (Behaviors != null && Behaviors.Any(x => x.OnUpdateEverySecond(seconds)))
            {
                return;
            }

            if (TitanState == MindlessTitanState.Wandering || TitanState == MindlessTitanState.Chase && Type != MindlessTitanType.Crawler)
            {
                Pathfinding();
            }
            else
            {
                RotationModifier = 0f;
            }
        }

        private void LateUpdate()
        {
            if (Target == null && TitanState == MindlessTitanState.Attacking)
            {
                ChangeState(MindlessTitanState.Wandering);
            }

            HeadMovement();
        }


        protected void OnAttacking()
        {
            if (CurrentAttack.IsFinished)
            {
                CurrentAttack.IsFinished = false;
                Stamina -= CurrentAttack.Stamina;
                attackCooldown = 0.25f;
                ChangeState(MindlessTitanState.Chase);
                return;
            }
            CurrentAttack.Execute(this);
        }

        protected void OnChasing()
        {
            if (Target == null || ViewDistance < TargetDistance)
            {
                ChangeState(MindlessTitanState.Wandering);
                return;
            }

            if (CanRun())
            {
                CurrentAnimation = AnimationRun;
                Stamina -= Time.deltaTime * 2;
            }
            else
            {
                CurrentAnimation = AnimationWalk;
            }
            CrossFade(CurrentAnimation, 0.1f);

            FocusTimer += Time.deltaTime;
            if (FocusTimer > Focus)
            {
                OnTargetRefresh();
            }

            if (attackCooldown > 0)
            {
                attackCooldown -= Time.deltaTime;
                return;
            }

            var availableAttacks = Attacks.Where(x => x.CanAttack(this)).ToArray();
            if (availableAttacks.Length > 0)
            {
                CurrentAttack = availableAttacks[Random.Range(0, availableAttacks.Length)];
                ChangeState(MindlessTitanState.Attacking);
            }
            else
            {
                var vector18 = Target.transform.position - transform.position;
                var angle = -Mathf.Atan2(vector18.z, vector18.x) * 57.29578f;
                var between = -Mathf.DeltaAngle(angle, gameObject.transform.rotation.eulerAngles.y - 90f);
                if (Mathf.Abs(between) > 45f && Vector3.Distance(Target.transform.position, transform.position) < 50f * Size)
                {
                    Turn(between);
                }
            }
        }

        protected void OnDisabled()
        {
            var disabledBodyParts = TitanBody.GetDisabledBodyParts();
            if (disabledBodyParts.Any(x => x == BodyPart.Eyes))
            {
                CurrentAnimation = AnimationEyes;
                CrossFade(CurrentAnimation, 0.1f);
                return;
            }

            if (disabledBodyParts.Any(x => x == BodyPart.LegLeft)
                || disabledBodyParts.Any(x => x == BodyPart.LegRight))
            {
                CurrentAnimation = "attack_abnormal_jump";
                CrossFade(CurrentAnimation, 0.1f);
            }
            else
            {
                ChangeState(MindlessTitanState.Chase);
            }
        }

        protected void OnGrabbing()
        {
            if (!Animation.IsPlaying(CurrentAnimation))
            {
                CrossFade(CurrentAnimation, 0.1f);
                return;
            }

            if (Animation[CurrentAnimation].normalizedTime >= 0.48f && GrabTarget != null)
            {
                KillGrabbedTarget(GrabTarget);
            }

            if (Animation[CurrentAnimation].normalizedTime > 1f)
            {
                ChangeState(PreviousState);
            }
        }

        protected void OnHit()
        {
            CrossFade(CurrentAnimation, 0f);
            if (Animation[CurrentAnimation].normalizedTime > 1f)
            {
                ChangeState(MindlessTitanState.Chase);
            }
        }

        protected void OnRecovering()
        {
            Stamina += Time.deltaTime * StaminaRecovery * 3f;
            CurrentAnimation = AnimationRecovery;
            if (!Animation.IsPlaying(CurrentAnimation))
            {
                CrossFade(CurrentAnimation, 0.1f);
            }

            if (Stamina > staminaLimit * 0.75f)
            {
                ChangeState(MindlessTitanState.Chase);
            }
        }

        protected void OnTurning()
        {
            CrossFade(CurrentAnimation, 0.2f);
            gameObject.transform.rotation = Quaternion.Lerp(gameObject.transform.rotation, Quaternion.Euler(0f, desDeg, 0f), (Time.deltaTime * Mathf.Abs(turnDeg)) * 0.015f);
            if (Animation[CurrentAnimation].normalizedTime > 1f)
            {
                ChangeState(PreviousState);
            }
        }

        protected void OnWandering()
        {
            CurrentAnimation = AnimationWalk;
            if (!Animation.IsPlaying(CurrentAnimation))
            {
                CrossFade(CurrentAnimation, 0.5f);
            }
        }

        #endregion

        protected virtual void FixedUpdate()
        {
            if (!photonView.isMine) return;
            Rigidbody.AddForce(new Vector3(0f, -120f * Rigidbody.mass, 0f));
            if (Behaviors != null && Behaviors.Any(x => x.OnFixedUpdate()))
            {
                return;
            }

            //TODO: Determine to keep this behavior or not. Having this disabled makes titan jumpers a lot more deadly.
            //if (Animation.IsPlaying("attack_jumper_0"))
            //{
            //    Vector3 vector9 = (Vector3)(((transform.forward * Speed) * Size) * 0.5f);
            //    Vector3 vector10 = Rigidbody.velocity;
            //    if ((Animation["attack_jumper_0"].normalizedTime <= 0.28f) || (Animation["attack_jumper_0"].normalizedTime >= 0.8f))
            //    {
            //        vector9 = Vector3.zero;
            //    }
            //    Vector3 vector11 = vector9 - vector10;
            //    var maxVelocityChange = 10f;
            //    vector11.x = Mathf.Clamp(vector11.x, -maxVelocityChange, maxVelocityChange);
            //    vector11.z = Mathf.Clamp(vector11.z, -maxVelocityChange, maxVelocityChange);
            //    vector11.y = 0f;
            //    Rigidbody.AddForce(vector11, ForceMode.VelocityChange);
            //}

            if (TitanState == MindlessTitanState.Wandering)
            {
                if (IsStuck())
                {
                    Turn(Random.Range(-270, 270));
                    return;
                }

                var runModifier = 1f;
                var vector12 = transform.forward * Speed * runModifier;
                var vector14 = vector12 - Rigidbody.velocity;
                vector14.x = Mathf.Clamp(vector14.x, -10f, 10f);
                vector14.z = Mathf.Clamp(vector14.z, -10f, 10f);
                vector14.y = 0f;
                Rigidbody.AddForce(vector14, ForceMode.VelocityChange);
                transform.Rotate(0, RotationModifier * Time.fixedDeltaTime, 0);
            }

            if (TitanState == MindlessTitanState.Chase)
            {
                if (Target == null) return;
                var speed = CanRun()
                    ? RunSpeed
                    : Speed;

                var vector12 = transform.forward * speed;
                var vector14 = vector12 - Rigidbody.velocity;
                vector14.x = Mathf.Clamp(vector14.x, -10f, 10f);
                vector14.z = Mathf.Clamp(vector14.z, -10f, 10f);
                vector14.y = 0f;
                Rigidbody.AddForce(vector14, ForceMode.VelocityChange);

                var vector17 = Target.transform.position - transform.position;
                var current = -Mathf.Atan2(vector17.z, vector17.x) * 57.29578f + RotationModifier;
                var num4 = -Mathf.DeltaAngle(current, transform.rotation.eulerAngles.y - 90f);
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0f, transform.rotation.eulerAngles.y + num4, 0f), ((Speed * 0.5f) * Time.fixedDeltaTime) / Size);
            }
        }
    }
}
