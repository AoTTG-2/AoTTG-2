using Assets.Scripts.Characters.Humans.Data;
using Assets.Scripts.Characters.Humans.Data.States.Grounded;
using Assets.Scripts.Constants;
using Assets.Scripts.StateMachine;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Scripts.Characters.Humans.StateMachines
{
    /// <summary>
    /// Movement State Class.
    /// Defines everything that is needed and can be done while moving.
    /// </summary>
    public class HeroMovementState : IState
    {
        protected HeroMovementStateMachine stateMachine;
        protected HeroGroundedData movementData;
        protected HeroAirborneData airborneData;
        protected float currentSpeed;
        protected float facingDirection;
        protected Quaternion targetRotation;
        protected float maxVelocityChange = 10f;
        protected PhotonView photonView;
        private enum HookType
        {
            left,
            right,
            both,
            none
        }
        public HeroMovementState(HeroMovementStateMachine heroMovementStateMachine)
        {
            stateMachine = heroMovementStateMachine;
            movementData = stateMachine.Hero.Data.GroundedData;
            airborneData = stateMachine.Hero.Data.DashData;
            facingDirection = stateMachine.Hero.transform.rotation.eulerAngles.y;
            targetRotation = Quaternion.Euler(0f, facingDirection, 0f);
            photonView = stateMachine.Hero.photonView;
        }
        #region IState Methods
        public virtual void Enter()
        {
            Debug.Log("State: " + GetType().Name);
            AddInputActionsCallbacks();
        }

        public virtual void Exit()
        {
            RemoveInputActionsCallbacks();
        }

        public virtual void HandleInput()
        {
            ReadMovementInput();
        }

        public virtual void OnAnimationExitEvent()
        {
        }

        public virtual void OnAnimationTransitionEvent()
        {
        }

        public virtual void OnAnimatonEnterEvent()
        {
        }

        public virtual void PhysicsUpdate()
        {
        }

        public virtual void Update()
        {
            if (!stateMachine.ReusableData.IsGrounded)
            {
                stateMachine.ChangeState(stateMachine.AirborneState);
            }
            if (stateMachine.Hero.Animation.IsPlaying(stateMachine.ReusableData.CurrentAnimation))
            {
                return;
            }
            else
            {
                CrossFade(stateMachine.ReusableData.CurrentAnimation);
            }
        }

        public virtual void UpdateAnimation(string newAnimation)
        {
            stateMachine.ReusableData.CurrentAnimation = newAnimation;
            CrossFade(newAnimation);
        }
        #endregion
        #region Main Methods
        private void ReadMovementInput()
        {
            stateMachine.ReusableData.MovementInput = stateMachine.Hero.HumanInput.HumanActions.Move.ReadValue<Vector2>();
            CheckGrounded();
        }
        private void CheckGrounded()
        {
            stateMachine.ReusableData.IsGrounded = IsGrounded();
        }
        private bool IsGrounded()
        {
            LayerMask mask = Layers.Ground.ToLayer() | Layers.EnemyBox.ToLayer();
            RaycastHit hit; //DONT DELETE THE OUT HIT FROM RAYCAST. IT BREAKS UTGARD CASTLE AND OTHER CONCAVE MESH COLLIDERS
            bool didHit = Physics.Raycast(stateMachine.Hero.gameObject.transform.position + (Vector3.up * 0.1f), Vector3.down, out hit, 0.3f, mask.value);
            return didHit;
        }
        private void OnHookUsed(InputAction.CallbackContext context)
        {
            switch (context.action.name)
            {
                case "Hook Left":
                    LaunchHook(HookType.left);
                    break;
                case "Hook Right":
                    LaunchHook(HookType.right);
                    break;
                case "Hook Both":
                    LaunchHook(HookType.both);
                    break;
                default:
                    Debug.LogError("Unknown Input");
                    LaunchHook(HookType.none);
                    return;
            }
        }
        private void LaunchHook(HookType hookType)
        {
            RaycastHit rightHookRayCastHit;
            Ray rightHookRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            LayerMask groundAndEnemyLayers = Layers.Ground.ToLayer() | Layers.EnemyBox.ToLayer();

            var didHit = Physics.Raycast(rightHookRay,
                                         out rightHookRayCastHit,
                                         stateMachine.ReusableData.HookRayCastDistance,
                                         groundAndEnemyLayers.value);
            var (distance, point) = didHit
                ? (rightHookRayCastHit.distance, rightHookRayCastHit.point)
                : (stateMachine.ReusableData.HookRayCastDistance, rightHookRay.GetPoint(stateMachine.ReusableData.HookRayCastDistance));

            switch (hookType)
            {
                case HookType.left:
                    if (stateMachine.Hero.hookLeft == null)
                        LaunchLeftRope(distance, point, true);
                    break;
                case HookType.right:
                    if (stateMachine.Hero.hookRight == null)
                        LaunchRightRope(distance, point, true);
                    break;
                case HookType.both:
                    if (stateMachine.Hero.hookLeft == null)
                        LaunchLeftRope(distance, point, false);
                    if (stateMachine.Hero.hookRight == null)
                        LaunchRightRope(distance, point, false);
                    break;
                default:
                    break;
            }
            if (stateMachine.ReusableData.CurrentGas > 0) stateMachine.Hero.rope.Play();
            else stateMachine.Hero.ropeNoGas.Play();
        }
        public void LaunchRightRope(float distance, Vector3 point, bool single, bool leviMode = false)
        {
            var source = stateMachine.Hero.useGun ? Bullet.HookSource.GunRight : Bullet.HookSource.BeltRight;
            stateMachine.Hero.LaunchRope(source, distance, point, single, leviMode);
        }
        public void LaunchLeftRope(float distance, Vector3 point, bool single, bool leviMode = false)
        {
            var source = stateMachine.Hero.useGun ? Bullet.HookSource.GunLeft : Bullet.HookSource.BeltLeft;
            stateMachine.Hero.LaunchRope(source, distance, point, single, leviMode);
        }
        #endregion
        #region Old Hero.cs Methods
        protected void UseGas(float amount = 0)
        {
            if (amount == 0f)
            {
                amount = stateMachine.ReusableData.UseGasSpeed;
            }
            if (stateMachine.ReusableData.CurrentGas > 0f)
            {
                stateMachine.ReusableData.CurrentGas -= amount;
                if (stateMachine.ReusableData.CurrentGas < 0f)
                {
                    stateMachine.ReusableData.CurrentGas = 0f;
                }
            }
        }
        protected void CrossFade(string newAnimation, float fadeLength = 0.1f)
        {
            photonView = stateMachine.Hero.photonView;
            if (string.IsNullOrWhiteSpace(newAnimation)) return;
            if (stateMachine.Hero.Animation.IsPlaying(newAnimation)) return;
            if (!photonView.isMine) return;

            stateMachine.Hero.CurrentAnimation = newAnimation;
            stateMachine.Hero.Animation.CrossFade(newAnimation, fadeLength);
            photonView.RPC(nameof(CrossFadeRpc), PhotonTargets.Others, newAnimation, fadeLength);
        }
        [PunRPC]
        protected void CrossFadeRpc(string newAnimation, float fadeLength, PhotonMessageInfo info)
        {
            if (info.sender.ID == photonView.owner.ID)
            {
                stateMachine.Hero.CurrentAnimation = newAnimation;
                stateMachine.Hero.Animation.CrossFade(newAnimation, fadeLength);
            }
        }
        protected float GetGlobalFacingDirection(float horizontal, float vertical)
        {
            if ((vertical == 0f) && (horizontal == 0f))
            {
                return stateMachine.Hero.transform.rotation.eulerAngles.y;
            }
            float y = stateMachine.Hero.currentCamera.transform.rotation.eulerAngles.y;
            float num2 = Mathf.Atan2(vertical, horizontal) * Mathf.Rad2Deg;
            num2 = -num2 + 90f;
            return (y + num2);
        }
        protected Vector3 GetGlobalFacingVector3(float resultAngle)
        {
            float num = -resultAngle + 90f;
            float x = Mathf.Cos(num * Mathf.Deg2Rad);
            return new Vector3(x, 0f, Mathf.Sin(num * Mathf.Deg2Rad));
        }
        #endregion
        #region Resuable Methods
        protected Vector3 GetPlayerHorizontalVelocity()
        {
            Vector3 playerHorizontalVelocity = stateMachine.Hero.Rigidbody.velocity;
            playerHorizontalVelocity.y = 0f;
            return playerHorizontalVelocity;
        }
        protected Vector3 GetMovementInputDirection()
        {
            return new Vector3(stateMachine.ReusableData.MovementInput.x, 0f, stateMachine.ReusableData.MovementInput.y);
        }
        protected Vector3 GetHeroVerticalVelocity()
        {
            return new Vector3(0f, stateMachine.Hero.Rigidbody.velocity.y, 0f);
        }
        protected void ResetVelocity()
        {
            stateMachine.Hero.Rigidbody.velocity = Vector3.zero;
        }
        protected void RotateHeroToFaceDirection()
        {
            stateMachine.Hero.Rigidbody.rotation = Quaternion.Lerp(stateMachine.Hero.gameObject.transform.rotation, targetRotation, Time.deltaTime * 6f);
        }
        protected virtual void AddInputActionsCallbacks()
        {
            stateMachine.Hero.HumanInput.HumanActions.HookLeft.started += OnHookUsed;
            stateMachine.Hero.HumanInput.HumanActions.HookRight.started += OnHookUsed;
            stateMachine.Hero.HumanInput.HumanActions.HookBoth.started += OnHookUsed;
        }
        protected virtual void RemoveInputActionsCallbacks()
        {
            stateMachine.Hero.HumanInput.HumanActions.HookLeft.started -= OnHookUsed;
            stateMachine.Hero.HumanInput.HumanActions.HookRight.started -= OnHookUsed;
            stateMachine.Hero.HumanInput.HumanActions.HookBoth.started -= OnHookUsed;
        }
        protected void SnapToFaceDirection()
        {
            stateMachine.Hero.Rigidbody.rotation = Quaternion.Euler(0f, facingDirection, 0f);
            targetRotation = Quaternion.Euler(0f, facingDirection, 0f);
        }
        #endregion
    }
}
