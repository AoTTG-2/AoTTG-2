﻿using Assets.Scripts.Characters.Titan.Attacks;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Characters.Titan
{
    public class PlayerTitan : MindlessTitan
    {
        public Camera currentCamera;
        public float currentDirection;
        public FengCustomInputs inputManager;
        public bool sit;
        public float targetDirection;

        public bool IsCovering;
        public bool Ai;
        private float Rotation { get; set; }
        private float SpeedModifier { get; set; }

        protected override void Awake()
        {
            base.Awake();
            this.inputManager = GameObject.Find("InputManagerController").GetComponent<FengCustomInputs>();
            this.currentCamera = GameObject.Find("MainCamera").GetComponent<Camera>();
            /*if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
            {
                base.enabled = false;
            }*/
        }

        protected override void FixedUpdate()
        {
            if (Ai)
            {
                base.FixedUpdate();
                return;
            }
            if (!photonView.isMine) return;
            Rigidbody.AddForce(new Vector3(0f, -120f * Rigidbody.mass, 0f));
            if (targetDirection == -874f || CurrentAttack != null) return;
            Vector3 vector12 = transform.forward * Speed * SpeedModifier;
            Vector3 vector14 = vector12 - Rigidbody.velocity;
            vector14.x = Mathf.Clamp(vector14.x, -10f, 10f);
            vector14.z = Mathf.Clamp(vector14.z, -10f, 10f);
            vector14.y = 0f;
            Rigidbody.AddForce(vector14, ForceMode.VelocityChange);
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0f, targetDirection, 0f), (Speed * 0.15f) * Time.deltaTime);

        }

        public override void Initialize(TitanConfiguration configuration)
        {
            base.Initialize(configuration);
        }

        private Attack CanAttack(int key)
        {
            if (key == InputCodeRC.titanSlam)
            {
                return Attacks.FirstOrDefault(x => x is BodySlamAttack);
            }

            if (key == InputCodeRC.titanAntiAE)
            {
                var attack = Attacks.SingleOrDefault(x => x is SlapAttack) as SlapAttack;
                if (attack == null) return null;

                return attack.CanAttack(this, Rotation < 0f)
                    ? attack
                    : null;
            }

            if (key == InputCodeRC.titanGrabNape)
            {
                var attack = Attacks.SingleOrDefault(x => x is GrabAttack) as GrabAttack;
                if (attack == null) return null;
                return attack.CanAttackNape(this, Rotation < 0f)
                    ? attack
                    : null;
            }

            if (key == InputCodeRC.titanGrabFront)
            {
                var attack = Attacks.SingleOrDefault(x => x is GrabAttack) as GrabAttack;
                if (attack == null) return null;
                return attack.CanAttackGroundFront(this, Rotation < 0f)
                    ? attack
                    : null;
            }

            if (key == InputCodeRC.titanGrabBack)
            {
                var attack = Attacks.SingleOrDefault(x => x is GrabAttack) as GrabAttack;
                if (attack == null) return null;
                return attack.CanAttackGroundBack(this, Rotation < 0f)
                    ? attack
                    : null;
            }

            if (key == InputCodeRC.titanBite)
            {
                var attack = Attacks.SingleOrDefault(x => x is BiteAttack) as BiteAttack;
                if (attack == null) return null;
                return attack.CanAttack(this, Rotation)
                    ? attack
                    : null;
            }

            return null;
        }

        private bool Attack(int keycode)
        {
            if (FengGameManagerMKII.inputRC.isInputTitan(keycode))
            {
                CurrentAttack = CanAttack(keycode);
                if (CurrentAttack == null) return false;
                if (!CurrentAttack.CanAttack(this))
                {
                    CurrentAttack = null;
                }
                return true;
            }

            return false;
        }

        private bool IsAttacking()
        {
            if (CurrentAttack != null)
            {
                CurrentAttack.Execute(this);
                if (CurrentAttack.IsFinished)
                {
                    CurrentAttack.IsFinished = false;
                    CurrentAttack = null;
                    return false;
                }
                return true;
            }

            if (Attack(InputCodeRC.titanSlam)) return true;
            if (Attack(InputCodeRC.titanAntiAE)) return true;
            if (Attack(InputCodeRC.titanGrabNape)) return true;
            if (Attack(InputCodeRC.titanGrabBack)) return true;
            if (Attack(InputCodeRC.titanGrabFront)) return true;
            if (Attack(InputCodeRC.titanBite)) return true;

            //if (FengGameManagerMKII.inputRC.isInputTitan(InputCodeRC.titanPunch))
            //{
            //    this.isAttackDown = true;
            //}
            //if (FengGameManagerMKII.inputRC.isInputTitan(InputCodeRC.titanJump))
            //{
            //    this.isJumpDown = true;
            //}
            //if (this.inputManager.GetComponent<FengCustomInputs>().isInputDown[InputCode.restart])
            //{
            //    this.isSuicide = true;
            //}
            return false;
        }

        protected override void OnTitanDeath()
        {
            base.OnTitanDeath();
            if (!photonView.isMine) return;
            this.currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().setMainObject(null, true, false);
            this.currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().setSpectorMode(true);
            this.currentCamera.GetComponent<IN_GAME_MAIN_CAMERA>().gameOver = true;
            ExitGames.Client.Photon.Hashtable propertiesToSet = new ExitGames.Client.Photon.Hashtable();
            propertiesToSet.Add(PhotonPlayerProperty.dead, true);
            PhotonNetwork.player.SetCustomProperties(propertiesToSet);
            propertiesToSet = new ExitGames.Client.Photon.Hashtable();
            propertiesToSet.Add(PhotonPlayerProperty.deaths, ((int)PhotonNetwork.player.CustomProperties[PhotonPlayerProperty.deaths]) + 1);
            PhotonNetwork.player.SetCustomProperties(propertiesToSet);
        }

        protected override void Update()
        {
            if (!photonView.isMine) return;

            if (!IsAlive)
            {
                Dead();
                return;
            }

            if (Input.GetKeyDown(KeyCode.F))
            {
                Ai = !Ai;
            }

            if (Ai)
            {
                base.Update();
                return;
            }

            RefreshStamina();
            if (Stamina <= 0f)
            {
                CrossFade(AnimationRecovery, 0.0f);
                return;
            }

            if (TitanState == MindlessTitanState.Disabled)
            {
                var disabledBodyParts = TitanBody.GetDisabledBodyParts();
                if (disabledBodyParts.Any(x => x == BodyPart.LegLeft)
                    || disabledBodyParts.Any(x => x == BodyPart.LegRight))
                {
                    CurrentAnimation = "attack_abnormal_jump";
                    CrossFade(CurrentAnimation, 0.1f);
                    return;
                }
                ChangeState(MindlessTitanState.Wandering);
            }

            if (IsCovering && Animation.IsPlaying(AnimationCover) && Animation[AnimationCover].normalizedTime < 1f)
            {
                return;
            }

            if (IsCovering && !Animation.IsPlaying(AnimationCover))
            {
                IsCovering = false;
                Stamina -= 25;
            }

            if (IsAttacking()) return;

            if (!IsCovering && FengGameManagerMKII.inputRC.isInputTitan(InputCodeRC.titanCover))
            {
                CrossFade(AnimationCover, 0.0f);
                SpeedModifier = 0f;
                IsCovering = true;
                return;
            }

            int num;
            int num2;
            float y;
            float num4;
            float num5;

            if (FengGameManagerMKII.inputRC.isInputTitan(InputCodeRC.titanForward))
            {
                num = 1;
            }
            else if (FengGameManagerMKII.inputRC.isInputTitan(InputCodeRC.titanBack))
            {
                num = -1;
            }
            else
            {
                num = 0;
            }
            if (FengGameManagerMKII.inputRC.isInputTitan(InputCodeRC.titanLeft))
            {
                num2 = -1;
            }
            else if (FengGameManagerMKII.inputRC.isInputTitan(InputCodeRC.titanRight))
            {
                num2 = 1;
            }
            else
            {
                num2 = 0;
            }
            if ((num2 != 0) || (num != 0))
            {
                y = this.currentCamera.transform.rotation.eulerAngles.y;
                num4 = Mathf.Atan2((float)num, (float)num2) * 57.29578f;
                num4 = -num4 + 90f;
                num5 = y + num4;
                this.targetDirection = num5;
                if (FengGameManagerMKII.inputRC.isInputLevel(InputCodeRC.titanWalk))
                {
                    SpeedModifier = 0.2f;
                    CrossFade(AnimationWalk, 0.0f);
                }
                else
                {
                    SpeedModifier = 1f;
                    CrossFade(AnimationRun, 0.0f);
                }
            }
            else
            {
                this.targetDirection = -874f;
                CrossFade(AnimationIdle, 0.0f);
            }
            if (this.targetDirection != -874f)
            {
                this.currentDirection = this.targetDirection;
            }
            Rotation = this.currentCamera.transform.rotation.eulerAngles.y - this.currentDirection;
            if (Rotation >= 180f)
            {
                Rotation -= 360f;
            }
        }
    }
}
