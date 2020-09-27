using Assets.Scripts.Characters.Titan.Attacks;
using Assets.Scripts.Characters.Titan.Behavior;
using Assets.Scripts.Characters.Titan.Body;
using Assets.Scripts.Characters.Titan.Configuration;
using Assets.Scripts.Settings;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Scripts.Characters.Titan
{
    public class MindlessTitan : TitanBase
    {
        public TitanState PreviousState;
        public TitanState NextState;
        public MindlessTitanType MindlessType;

        private float DamageTimer { get; set; }
        public new MindlessTitanBody Body { get; protected set; }

        private float turnDeg;
        private float desDeg;
        private int nextUpdate = 1;
        private float attackCooldown;
        private float staminaLimit;

        private float FocusTimer { get; set; }
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

        private Hero GrabTarget { get; set; }
        public float RotationModifier { get; private set; }

        public Attack<MindlessTitan>[] Attacks { get; private set; }
        public Attack<MindlessTitan> CurrentAttack { get; set; } = new BodySlamAttack();
        private Collider[] Colliders { get; set; }
        private FengGameManagerMKII GameManager { get; set; }

        private bool asClientLookTarget;
        private Quaternion oldHeadRotation;
        private Quaternion targetHeadRotation;
        private Vector3 headscale;

        protected override void Awake()
        {
            base.Awake();
            GameManager = FengGameManagerMKII.instance;
            Body = GetComponent<MindlessTitanBody>();
            this.oldHeadRotation = Body.Head.rotation;
            this.grabTF = new GameObject();
            this.grabTF.name = "titansTmpGrabTF";
            grabTF.transform.SetParent(transform);
            Colliders = GetComponentsInChildren<Collider>().Where(x => x.name != "AABB" && x.name != "Detection")
                .ToArray();
            CheckColliders();

            GameObject obj2 = new GameObject
            {
                name = "PlayerCollisionDetection"
            };
            CapsuleCollider collider2 = obj2.AddComponent<CapsuleCollider>();
            CapsuleCollider component = Body.AABB.GetComponent<CapsuleCollider>();
            collider2.center = component.center;
            collider2.radius = Math.Abs(Body.Head.position.y - transform.position.y);
            collider2.height = component.height * 1.2f;
            collider2.material = component.material;
            collider2.isTrigger = true;
            collider2.name = "PlayerCollisionDetection";
            obj2.AddComponent<TitanTrigger>();
            obj2.layer = 0x10;
            obj2.transform.parent = Body.AABB;
            obj2.transform.localPosition = new Vector3(0f, 0f, 0f);
        }
        
        public override void Initialize(TitanConfiguration configuration)
        {
            Health = configuration.Health;
            MaxHealth = Health;
            Speed = configuration.Speed;
            SpeedRun = configuration.RunSpeed;
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

            foreach (var attack in Attacks)
            {
                attack.Initialize(this);
            }

            MindlessType = configuration.Type;
            name = MindlessType.ToString();

            Body.Initialize(configuration.LimbHealth, configuration.LimbRegeneration);

            transform.localScale = new Vector3(Size, Size, Size);
            var scale = Mathf.Min(Mathf.Pow(2f / Size, 0.35f), 1.25f);
            headscale = new Vector3(scale, scale, scale);
            Body.Head.localScale = headscale;
            LoadSkin();
            SetAnimationSpeed();

            AttackDistance = Vector3.Distance(base.transform.position, Body.AttackFrontGround.position) * 1.65f;

            if (Health > 0)
            {
                if (MindlessType == MindlessTitanType.Crawler)
                {
                    HealthLabel.transform.localPosition = new Vector3(0f, 10f + (1f / Size), 0f);
                }
            }

            if (photonView.isMine)
            {
                configuration.Behaviors = new List<TitanBehavior>();
                var config = JsonConvert.SerializeObject(configuration);
                photonView.RPC("InitializeRpc", PhotonTargets.OthersBuffered, config);

                if (Health > 0)
                {
                    photonView.RPC(nameof(UpdateHealthLabelRpc), PhotonTargets.All, Health, MaxHealth);
                }
            }
        }

        private void SetAnimationSpeed()
        {
            //Animation[AnimationWalk].speed = Mathf.Clamp(Speed / 8f, 0.5f, 1.5f);
            //if (AnimationRun != null)
            //    Animation[AnimationRun].speed = Mathf.Clamp(Speed / 16f, 0.5f, 1.5f);
        }

        [PunRPC]
        public void InitializeRpc(string titanConfiguration, PhotonMessageInfo info)
        {
            if (info.sender.ID == photonView.ownerId)
            {
                Initialize(JsonConvert.DeserializeObject<TitanConfiguration>(titanConfiguration));
            }
        }



        private void LoadSkin()
        {
            var eye = false;
            if (!((base.photonView.isMine) ? (((int)FengGameManagerMKII.settings[1]) != 1) : true))
            {
                int index = (int)UnityEngine.Random.Range((float)86f, (float)90f);
                int num2 = index - 60;
                if (((int)FengGameManagerMKII.settings[0x20]) == 1)
                {
                    num2 = UnityEngine.Random.Range(0x1a, 30);
                }
                string body = (string)FengGameManagerMKII.settings[index];
                string eyes = (string)FengGameManagerMKII.settings[num2];
                var skin = index;
                if ((eyes.EndsWith(".jpg") || eyes.EndsWith(".png")) || eyes.EndsWith(".jpeg"))
                {
                    eye = true;
                }
                base.GetComponent<TITAN_SETUP>().setVar(skin, eye);
            }
            GetComponent<TITAN_SETUP>().setHair2();
        }

        [PunRPC]
        private void setIfLookTarget(bool bo)
        {
            this.asClientLookTarget = bo;
        }

        public GameObject grabTF { get; set; }

        [PunRPC]
        public void Grab(bool isLeftHand)
        {
            var hand = isLeftHand ? Body.HandLeft : Body.HandRight;
            this.grabTF.transform.parent = hand;
            this.grabTF.transform.position = hand.GetComponent<SphereCollider>().transform.position;
            this.grabTF.transform.rotation = hand.GetComponent<SphereCollider>().transform.rotation;
            Transform transform1 = this.grabTF.transform;
            transform1.localPosition -= Vector3.right * hand.GetComponent<SphereCollider>().radius * 0.3f;
            Transform transform2 = this.grabTF.transform;
            if (isLeftHand)
            {
                transform2.localPosition -= Vector3.up * hand.GetComponent<SphereCollider>().radius * 0.51f;
            }
            else
            {
                transform2.localPosition += Vector3.up * hand.GetComponent<SphereCollider>().radius * 0.51f;
            }

            Transform transform3 = this.grabTF.transform;
            transform3.localPosition -= Vector3.forward * hand.GetComponent<SphereCollider>().radius * 0.3f;

            float zRotation = isLeftHand ? 180f : 0f;
            this.grabTF.transform.localRotation = Quaternion.Euler(this.grabTF.transform.localRotation.eulerAngles.x, this.grabTF.transform.localRotation.eulerAngles.y + 180f, this.grabTF.transform.localRotation.eulerAngles.z + zRotation);
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
                if (base.photonView.isMine)
                {
                    if (!grabTarget.HasDied())
                    {
                        grabTarget.markDie();
                        object[] objArray2 = new object[] { -1, base.name };
                        grabTarget.photonView.RPC("netDie2", PhotonTargets.All, objArray2);
                    }
                }
            }
        }

        private void HeadMovement()
        {
            if (State != TitanState.Dead)
            {
                if (base.photonView.isMine)
                {
                    targetHeadRotation = Body.Head.rotation;
                    bool flag2 = false;
                    if (State == TitanState.Chase && TargetDistance < 100f && Target != null)
                    {
                        var vector = Target.transform.position - transform.position;
                        var angle = -Mathf.Atan2(vector.z, vector.x) * 57.29578f;
                        float num = -Mathf.DeltaAngle(angle, base.transform.rotation.eulerAngles.y - 90f);
                        num = Mathf.Clamp(num, -40f, 40f);
                        float y = (Body.Neck.position.y + (Size * 2f)) - Target.transform.position.y;
                        float num3 = Mathf.Atan2(y, TargetDistance) * 57.29578f;
                        num3 = Mathf.Clamp(num3, -40f, 30f);
                        targetHeadRotation = Quaternion.Euler(Body.Head.rotation.eulerAngles.x + num3,
                            Body.Head.rotation.eulerAngles.y + num, Body.Head.rotation.eulerAngles.z);
                        if (!this.asClientLookTarget)
                        {
                            this.asClientLookTarget = true;
                            object[] parameters = new object[] {true};
                            base.photonView.RPC("setIfLookTarget", PhotonTargets.Others, parameters);
                        }

                        flag2 = true;
                    }

                    if (!(flag2 || !this.asClientLookTarget))
                    {
                        this.asClientLookTarget = false;
                        object[] objArray3 = new object[] {false};
                        base.photonView.RPC("setIfLookTarget", PhotonTargets.Others, objArray3);
                    }

                    if (State == TitanState.Attacking)
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
                        TargetDistance = Mathf.Sqrt(
                            ((Target.transform.position.x - transform.position.x) *
                             (Target.transform.position.x - transform.position.x)) +
                            ((Target.transform.position.z - transform.position.z) *
                             (Target.transform.position.z - transform.position.z)));
                    }
                    else
                    {
                        TargetDistance = float.MaxValue;
                    }

                    this.targetHeadRotation = Body.Head.rotation;
                    if ((this.asClientLookTarget && hasTarget) && (TargetDistance < 100f))
                    {
                        var vector2 = Target.transform.position - transform.position;
                        var angle = -Mathf.Atan2(vector2.z, vector2.x) * 57.29578f;
                        float num4 = -Mathf.DeltaAngle(angle, transform.rotation.eulerAngles.y - 90f);
                        num4 = Mathf.Clamp(num4, -40f, 40f);
                        float num5 = (Body.Neck.position.y + (Size * 2f)) - Target.transform.position.y;
                        float num6 = Mathf.Atan2(num5, TargetDistance) * 57.29578f;
                        num6 = Mathf.Clamp(num6, -40f, 30f);
                        this.targetHeadRotation = Quaternion.Euler(Body.Head.rotation.eulerAngles.x + num6,
                            Body.Head.rotation.eulerAngles.y + num4, Body.Head.rotation.eulerAngles.z);
                    }

                    this.oldHeadRotation = Quaternion.Slerp(this.oldHeadRotation, this.targetHeadRotation,
                        Time.deltaTime * 10f);
                }

                Body.Head.rotation = this.oldHeadRotation;
            }
            if (!base.GetComponent<Animation>().IsPlaying("die_headOff"))
            {
                Body.Head.localScale = this.headscale;
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
            Body.DamageBodyPart(bodyPart, damage);
            if (Body.GetDisabledBodyParts().Any(x => x == BodyPart.LegLeft)
                && Body.GetDisabledBodyParts().Any(x => x == BodyPart.LegRight))
            {
                ChangeState(TitanState.Disabled);
            }
        }

        [PunRPC]
        public void OnEyeHitRpc(int viewId, int damage)
        {
            if (MindlessType == MindlessTitanType.Crawler) return;
            if (!photonView.isMine) return;
            if (!IsAlive) return;
            Body.AddBodyPart(BodyPart.Eyes, Animation[AnimationEyes].length * Animation[AnimationEyes].speed);
            if (Body.GetDisabledBodyParts().Any(x => x == BodyPart.Eyes))
            {
                ChangeState(TitanState.Disabled);
            }
        }

        [PunRPC]
        public void OnNapeHitRpc(int viewId, int damage)
        {
            if (!IsAlive) return;
            var view = PhotonView.Find(viewId);
            if (view == null || !IsAlive && Time.time - DamageTimer > 0.2f) return;
            if (damage < GameSettings.Titan.MinimumDamage.Value) return;
            if (damage > GameSettings.Titan.MaximumDamage.Value)
            {
                damage = GameSettings.Titan.MaximumDamage.Value;
            }

            DamageTimer = Time.time;
            Health -= damage;

            if (MaxHealth > 0)
            {
                photonView.RPC(nameof(UpdateHealthLabelRpc), PhotonTargets.All, Health, MaxHealth);
            }

            if (Health <= 0)
            {
                Health = 0;
            }
            else
            {
                return;
            }

            OnTitanDeath();
            ChangeState(TitanState.Dead);
            FengGameManagerMKII.instance.titanGetKill(view.owner, damage, name);
        }

        public void OnTargetGrabbed(GameObject target, bool isLeftHand)
        {
            ChangeState(TitanState.Eat);
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
            base.OnDeath();
            ReleaseGrabbedTarget();
            if (GameSettings.Titan.Mindless.ExplodeMode.Value > 0)
                Invoke(nameof(Explode), 1f);
        }

        private void ReleaseGrabbedTarget()
        {
            if (GrabTarget != null)
            {
                GrabTarget.photonView.RPC("netUngrabbed", PhotonTargets.All);
            }
        }

        private void Explode()
        {
            var height = Size * 10f;
            if (MindlessType == MindlessTitanType.Crawler)
            {
                height = 0f;
            }
            Vector3 position = transform.position + Vector3.up * height;
            PhotonNetwork.Instantiate("FX/Thunder", position, Quaternion.Euler(270f, 0f, 0f), 0);
            PhotonNetwork.Instantiate("FX/boom1", position, Quaternion.Euler(270f, 0f, 0f), 0);
            foreach (Hero player in EntityService.GetAll<Hero>())
            {
                if (Vector3.Distance(player.transform.position, position) < GameSettings.Titan.Mindless.ExplodeMode.Value)
                {
                    player.markDie();
                    player.photonView.RPC("netDie2", PhotonTargets.All,  -1, "Server ");
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
                PhotonNetwork.Instantiate("FX/FXtitanDie1", Body.Hip.position, Quaternion.Euler(-90f, 0f, 0f), 0).transform.localScale = transform.localScale;
            }
            if (deathTime > 5f)
            {
                if (base.photonView.isMine)
                {
                    PhotonNetwork.Instantiate("FX/FXtitanDie", Body.Hip.position, Quaternion.Euler(-90f, 0f, 0f), 0).transform.localScale = transform.localScale;
                    PhotonNetwork.Destroy(gameObject);
                }
            }
        }

        public bool HasTarget()
        {
            return Target != null;
        }

        public void OnTargetDetected(Entity target)
        {
            Target = target;
            TargetDistance = float.MaxValue;
            ChangeState(TitanState.Chase);
            FocusTimer = 0f;
            this.oldHeadRotation = Body.Head.rotation;
        }

        public void ChangeState(TitanState state)
        {
            if (!IsAlive) return;
            if (state == State) return;

            if ((State == TitanState.Attacking)
                && state != TitanState.Dead
                && PreviousState != TitanState.Idle)
            {
                PreviousState = State;
                NextState = state;
                State = TitanState.Idle;
                IdleTimer = Idle;
                CrossFade("idle_2", 0.2f);
                return;
            }

            if (State == TitanState.Idle) {}

            PreviousState = State == TitanState.Idle 
                ? TitanState.Chase
                : State;
            State = state;
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
            ChangeState(TitanState.Turning);
            CurrentAnimation = degrees > 0f ? AnimationTurnLeft : AnimationTurnRight;
            CrossFade(CurrentAnimation, 0.0f);
            this.turnDeg = degrees;
            this.desDeg = base.gameObject.transform.rotation.eulerAngles.y + this.turnDeg;
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
        
        private void CheckColliders()
        {
            if (!IsHooked && !IsLooked && !IsColliding)
            {
                foreach (Collider collider in Colliders)
                {
                    if (collider != null)
                    {
                        collider.enabled = false;
                    }
                }
            }
            else if (IsHooked || IsLooked || IsColliding)
            {
                foreach (Collider collider in Colliders)
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
            foreach (Entity hero in EntityService.GetAll<Entity>())
            {
                if (FactionService.IsFriendly(this, hero)) continue;
                var distance = Vector3.Distance(hero.gameObject.transform.position, position);
                if (distance < targetDistance)
                {
                    Target = hero;
                    targetDistance = distance;
                }
            }
        }
        
        private void Pathfinding()
        {
            Vector3 forwardDirection = Body.Hip.transform.TransformDirection(new Vector3(-0.3f, 0, 1f));
            RaycastHit objectHit;
            var mask = ~LayerMask.NameToLayer("Ground");
            if (Physics.Raycast(Body.Hip.transform.position, forwardDirection, out objectHit, 50, mask))
            {
                Vector3 leftDirection = Body.Hip.transform.TransformDirection(new Vector3(-0.3f, -1f, 1f));
                Vector3 rightDirection = Body.Hip.transform.TransformDirection(new Vector3(-0.3f, 1f, 1f));
                RaycastHit leftHit;
                RaycastHit rightHit;
                Physics.Raycast(Body.Hip.transform.position, leftDirection, out leftHit, 250 , mask);
                Physics.Raycast(Body.Hip.transform.position, rightDirection, out rightHit, 250, mask);

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

        protected override void Update()
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

            if (Stamina < 0 && State != TitanState.Recovering && State != TitanState.Disabled)
            {
                ChangeState(TitanState.Recovering);
            }

            if (Behaviors != null && Behaviors.Any(x => x.OnUpdate()))
            {
                return;
            }


            switch (State)
            {
                case TitanState.Disabled:
                    OnDisabled();
                    break;
                case TitanState.Idle:
                    OnIdle();
                    break;
                case TitanState.Dead:
                    break;
                case TitanState.Wandering:
                    OnWandering();
                    break;
                case TitanState.Turning:
                    OnTurning();
                    break;
                case TitanState.Chase:
                    OnChasing();
                    break;
                case TitanState.Attacking:
                    OnAttacking();
                    break;
                case TitanState.Recovering:
                    OnRecovering();
                    break;
                case TitanState.Eat:
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

            if (State == TitanState.Wandering || State == TitanState.Chase && MindlessType != MindlessTitanType.Crawler)
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
            if (Target == null && State == TitanState.Attacking)
            {
                ChangeState(TitanState.Wandering);
            }

            HeadMovement();
        }


        protected override void OnAttacking()
        {
            if (CurrentAttack.IsFinished)
            {
                CurrentAttack.IsFinished = false;
                Stamina -= CurrentAttack.Stamina;
                attackCooldown = 0.25f;
                ChangeState(TitanState.Chase);
                return;
            }
            CurrentAttack.Execute();
        }

        protected override void OnChasing()
        {
            if (Target == null || ViewDistance < TargetDistance)
            {
                ChangeState(TitanState.Wandering);
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

            var availableAttacks = Attacks.Where(x => x.CanAttack()).ToArray();
            if (availableAttacks.Length > 0)
            {
                CurrentAttack = availableAttacks[Random.Range(0, availableAttacks.Length)];
                ChangeState(TitanState.Attacking);
            }
            else
            {
                Vector3 vector18 = Target.transform.position - transform.position;
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
            var disabledBodyParts = Body.GetDisabledBodyParts();
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
                ChangeState(TitanState.Chase);
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
                this.KillGrabbedTarget(GrabTarget);
            }

            if (Animation[CurrentAnimation].normalizedTime > 1f)
            {
                ChangeState(PreviousState);
            }
        }

        public float IdleTimer;
        protected void OnIdle()
        {
            IdleTimer -= Time.deltaTime;
            if (IdleTimer <= 0)
            {
                ChangeState(NextState);
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
                ChangeState(TitanState.Chase);
            }
        }

        protected void OnTurning()
        {
            CrossFade(CurrentAnimation, 0.2f);
            gameObject.transform.rotation = Quaternion.Lerp(gameObject.transform.rotation, Quaternion.Euler(0f, this.desDeg, 0f), (Time.deltaTime * Mathf.Abs(this.turnDeg)) * 0.015f);
            if (Animation[CurrentAnimation].normalizedTime > 1f)
            {
                ChangeState(PreviousState);
            }
        }

        protected override void OnWandering()
        {
            CurrentAnimation = AnimationWalk;
            if (!Animation.IsPlaying(CurrentAnimation))
            {
                CrossFade(CurrentAnimation, 0.5f);
            }
        }

        #endregion

        protected override void FixedUpdate()
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

            if (State == TitanState.Wandering)
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

            if (State == TitanState.Chase)
            {
                if (Target == null) return;
                var speed = CanRun()
                    ? SpeedRun
                    : Speed;

                var vector12 = transform.forward * speed;
                var vector14 = vector12 - Rigidbody.velocity;
                vector14.x = Mathf.Clamp(vector14.x, -10f, 10f);
                vector14.z = Mathf.Clamp(vector14.z, -10f, 10f);
                vector14.y = 0f;
                Rigidbody.AddForce(vector14, ForceMode.VelocityChange);

                var vector17 = Target.transform.position - transform.position;
                var current = -Mathf.Atan2(vector17.z, vector17.x) * 57.29578f + RotationModifier;
                float num4 = -Mathf.DeltaAngle(current, transform.rotation.eulerAngles.y - 90f);
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0f, transform.rotation.eulerAngles.y + num4, 0f), ((Speed * 0.5f) * Time.fixedDeltaTime) / Size);
            }
        }
    }
}
