using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Characters.Titan.Attacks;
using UnityEngine;
using MonoBehaviour = Photon.MonoBehaviour;
using Random = UnityEngine.Random;

namespace Assets.Scripts.Characters.Titan
{
    public class MindlessTitan : MonoBehaviour
    {
        public MindlessTitanState TitanState = MindlessTitanState.Wandering;
        public float Speed = 10f;

        private bool IsAlive => TitanState != MindlessTitanState.Dead;
        private float DamageTimer { get; set; }
        public TitanBody TitanBody { get; protected set; }
        public Animation Animation { get; protected set; }
        private Rigidbody Rigidbody { get; set; }

        private string CurrentAnimation { get; set; } = "idle_2";

        private string AnimationTurnLeft { get; set; } = "turnaround2";
        private string AnimationTurnRight { get; set; } = "turnaround1";
        private string AnimationWalk { get; set; } = "run_walk";

        private float turnDeg;
        private float desDeg;

        private int nextUpdate = 1;
        public float Size = 1f;

        public Hero Target { get; set; }
        private float RotationModifier { get; set; }

        private List<Attack> Attacks { get; set; }
        private Attack CurrentAttack { get; set; }
        private float attackDistance = 15f;


        void Awake()
        {
            TitanBody = GetComponent<TitanBody>();
            Animation = GetComponent<Animation>();
            Rigidbody = GetComponent<Rigidbody>();
            Attacks = new List<Attack>
            {
                new RockThrowAttack()
            };
        }

        public void OnNapeHit(int viewId, int damage)
        {
            return;
            var view = PhotonView.Find(viewId);
            if (view == null || !IsAlive && Time.time - DamageTimer > 0.2f) return;
            DamageTimer = Time.time;
            FengGameManagerMKII.instance.titanGetKill(view.owner, damage, "Titan Nape");
        }

        public void OnEyeHit(int viewId, int damage)
        {
            return;
            var view = PhotonView.Find(viewId);
            if (view == null || !IsAlive && Time.time - DamageTimer < 0.2f) return;
            DamageTimer = Time.time;
            FengGameManagerMKII.instance.titanGetKill(view.owner, damage, "Titan Eye");
        }

        public void OnAnkleHit(int viewId, int damage)
        {
            return;
            var view = PhotonView.Find(viewId);
            if (view == null || !IsAlive && Time.time - DamageTimer < 0.2f) return;
            DamageTimer = Time.time;
            FengGameManagerMKII.instance.titanGetKill(view.owner, damage, "Titan Ankle");
        }

        public bool HasTarget()
        {
            return Target != null;
        }

        public void OnTargetDetected(GameObject target)
        {
            Target = target.GetComponent<Hero>();
            TitanState = MindlessTitanState.Chase;
        }

        void Update()
        {
            if (Time.time >= nextUpdate)
            {
                nextUpdate = Mathf.FloorToInt(Time.time) + 1;
                UpdateEverySecond();
            }

            if (TitanState == MindlessTitanState.Wandering)
            {
                CurrentAnimation = AnimationWalk;
                if (!Animation.IsPlaying(CurrentAnimation))
                {
                    Animation.CrossFade(CurrentAnimation);
                }
                return;
            }
            
            if (TitanState == MindlessTitanState.Turning)
            {
                gameObject.transform.rotation = Quaternion.Lerp(gameObject.transform.rotation, Quaternion.Euler(0f, this.desDeg, 0f), (Time.deltaTime * Mathf.Abs(this.turnDeg)) * 0.015f);
                if (Animation[CurrentAnimation].normalizedTime > 1f)
                {
                    TitanState = MindlessTitanState.Wandering;
                }

                return;
            }

            if (TitanState == MindlessTitanState.Chase)
            {
                CurrentAnimation = AnimationWalk;
                if (!Animation.IsPlaying(CurrentAnimation))
                {
                    Animation.CrossFade(CurrentAnimation);
                }

                CurrentAttack = Attacks.SingleOrDefault(x => x.CanAttack(this));
                if (CurrentAttack != null)
                {
                    TitanState = MindlessTitanState.Attacking;
                }
                return;
            }

            if (TitanState == MindlessTitanState.Attacking)
            {
                if (CurrentAttack.IsFinished)
                {
                    TitanState = MindlessTitanState.Chase;
                    CurrentAttack.IsFinished = false;
                    return;
                }
                CurrentAttack.Execute(this);
            }
        }

        private void Turn(float degrees)
        {
            TitanState = MindlessTitanState.Turning;
            CurrentAnimation = degrees > 0f ? AnimationTurnLeft : AnimationTurnRight;
            Animation.Play(CurrentAnimation);
            this.turnDeg = degrees;
            this.desDeg = base.gameObject.transform.rotation.eulerAngles.y + this.turnDeg;
        }

        private bool Between(float value, float min = -1f, float max = 1f)
        {
            return value > min && value < max;
        }

        private bool IsStuck()
        {
            var velocity = Rigidbody.velocity;
            return Between(velocity.z, -Speed / 4, Speed / 4) 
                   && Between(velocity.x, -Speed / 4, Speed / 4) 
                   && Animation[CurrentAnimation].normalizedTime > 2f;
        }

        void UpdateEverySecond()
        {
            if (TitanState == MindlessTitanState.Wandering)
            {
                if (Random.Range(0, 100) > 80)
                {
                    gameObject.transform.Rotate(0, Random.Range(-15, 15), 0);
                }
            }

            if (TitanState == MindlessTitanState.Chase && nextUpdate % 4 == 0)
            {
                if (IsStuck())
                {
                    RotationModifier = Random.Range(0, 2) == 1
                        ? Time.fixedDeltaTime * 5000f
                        : Time.fixedDeltaTime * -5000f;
                }
                else
                {
                    RotationModifier = 0;
                }
            }
        }

        void FixedUpdate()
        {
            if (TitanState == MindlessTitanState.Wandering)
            {
                if (IsStuck())
                {
                    Turn(Random.Range(-270, 270));
                    return;
                }
                Vector3 vector12 = transform.forward * Speed;
                Vector3 vector14 = vector12 - Rigidbody.velocity;
                vector14.x = Mathf.Clamp(vector14.x, -10f, 10f);
                vector14.z = Mathf.Clamp(vector14.z, -10f, 10f);
                vector14.y = 0f;
                Rigidbody.AddForce(vector14, ForceMode.VelocityChange);
            }

            if (TitanState == MindlessTitanState.Chase)
            {
                Vector3 vector17 = Target.transform.position - transform.position;
                var current = -Mathf.Atan2(vector17.z, vector17.x) * 57.29578f + RotationModifier;
                float num4 = -Mathf.DeltaAngle(current, transform.rotation.eulerAngles.y - 90f);
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0f, transform.rotation.eulerAngles.y + num4, 0f), ((Speed * 0.5f) * Time.deltaTime) / Size);

                Vector3 vector12 = transform.forward * Speed;
                Vector3 vector14 = vector12 - Rigidbody.velocity;
                vector14.x = Mathf.Clamp(vector14.x, -10f, 10f);
                vector14.z = Mathf.Clamp(vector14.z, -10f, 10f);
                vector14.y = 0f;
                Rigidbody.AddForce(vector14, ForceMode.VelocityChange);
            }
        }
    }
}
