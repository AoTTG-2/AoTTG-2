using Assets.Scripts.Characters.Humans.Data.States.Grounded;
using Assets.Scripts.StateMachine;
using UnityEngine;

namespace Assets.Scripts.Characters.Humans.StateMachines.Movement.States
{
    /// <summary>
    /// Movement State Class.
    /// Defines everything that is needed and can be done while moving.
    /// </summary>
    public class HeroMovementState : IState
    {
        protected HeroMovementStateMachine stateMachine;
        protected HeroGroundedData movementData;
        protected float currentSpeed;

        protected float facingDirection;
        protected Quaternion targetRotation;
        protected float maxVelocityChange = 10f;
        protected PhotonView photonView;

        public HeroMovementState(HeroMovementStateMachine heroMovementStateMachine)
        {
            stateMachine = heroMovementStateMachine;
            movementData = stateMachine.Hero.Data.GroundedData;
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
            if (stateMachine.Hero.Animation.IsPlaying(stateMachine.ReusableData.currentAnimation))
            {
                return;
            }
            else
            {
                CrossFade(stateMachine.ReusableData.currentAnimation);
            }
        }
        public virtual void UpdateAnimation(string newAnimation)
        {
            stateMachine.ReusableData.currentAnimation = newAnimation;
            CrossFade(newAnimation);
        }
        #endregion
        #region Main Methods
        private void ReadMovementInput()
        {
            stateMachine.ReusableData.MovementInput = stateMachine.Hero.HumanInput.HumanActions.Move.ReadValue<Vector2>();
        }
        #endregion
        #region Old Hero.cs Methods
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
        protected Vector3 GetGlobaleFacingVector3(float resultAngle)
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
        protected Vector3 GetHeroHorizontalVelocity()
        {
            Vector3 playerHorizontalVelocity = stateMachine.Hero.Rigidbody.velocity;
            playerHorizontalVelocity.y = 0f;
            return playerHorizontalVelocity;
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
        }
        protected virtual void RemoveInputActionsCallbacks()
        {
        }
        protected void SnapToFaceDirection()
        {
            stateMachine.Hero.Rigidbody.rotation = Quaternion.Euler(0f, facingDirection, 0f);
            targetRotation = Quaternion.Euler(0f, facingDirection, 0f);
        }
        #endregion
    }
}
