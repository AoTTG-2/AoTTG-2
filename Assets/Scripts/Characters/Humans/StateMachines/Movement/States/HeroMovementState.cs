using Assets.Scripts.Characters.Humans.Data.States.Grounded;
using Assets.Scripts.StateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Characters.Humans.StateMachines.Movement.States
{
    public class HeroMovementState : IState
    {
        protected HeroMovementStateMachine stateMachine;
        protected HeroGroundedData movementData;
        protected float currentSpeed;
        //Variables Stolen From Hero.cs
        protected float facingDirection;
        protected Quaternion targetRotation;
        protected float maxVelocityChange = 10f;

        public HeroMovementState(HeroMovementStateMachine heroMovementStateMachine)
        {
            stateMachine = heroMovementStateMachine;
            movementData = stateMachine.Hero.Data.GroundedData;
            facingDirection = stateMachine.Hero.transform.rotation.eulerAngles.y;
            targetRotation = Quaternion.Euler(0f, facingDirection, 0f);
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
            Move();
        }
        #endregion
        #region Main Methods
        private void ReadMovementInput()
        {
            stateMachine.ReusableData.MovementInput = stateMachine.Hero.HumanInput.HumanActions.Move.ReadValue<Vector2>();
        }
        private void Move()
        {
            if (stateMachine.ReusableData.MovementInput == Vector2.zero) return;
            Vector3 movementVector = GetMovementInputDirection();
            float resultAngle = GetGlobalFacingDirection(movementVector.y, movementVector.x);
            Vector3 zero = GetGlobaleFacingVector3(resultAngle);
            float movementMagnitudeChecker = (movementVector.magnitude <= 0.95f) ? ((movementVector.magnitude >= 0.25f) ? movementVector.magnitude : 0f) : 1f;
            zero = (zero * movementMagnitudeChecker);
            zero = (zero * movementData.BaseSpeed);
            if (resultAngle != -874f)
            {
                facingDirection = resultAngle;
                targetRotation = Quaternion.Euler(0f, facingDirection, 0f);
            }

            Vector3 velocity = stateMachine.Hero.Rigidbody.velocity;
            Vector3 force = zero - velocity;

            force.x = Mathf.Clamp(force.x, -maxVelocityChange, maxVelocityChange);
            force.z = Mathf.Clamp(force.z, -maxVelocityChange, maxVelocityChange);
            force.y = 0f;

            stateMachine.Hero.Rigidbody.AddForce(force, ForceMode.VelocityChange);
            RotateHeroToFaceDirection();
        }
        #endregion
        #region Old Hero.cs Methods
        public float GetGlobalFacingDirection(float horizontal, float vertical)
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
        public Vector3 GetGlobaleFacingVector3(float resultAngle)
        {
            float num = -resultAngle + 90f;
            float x = Mathf.Cos(num * Mathf.Deg2Rad);
            return new Vector3(x, 0f, Mathf.Sin(num * Mathf.Deg2Rad));
        }
        #endregion
        #region Resuable Methods
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
        public void RotateHeroToFaceDirection()
        {
            stateMachine.Hero.Rigidbody.rotation = Quaternion.Lerp(stateMachine.Hero.gameObject.transform.rotation, Quaternion.Euler(0f, facingDirection, 0f), Time.deltaTime * 10f);
        }
        protected virtual void AddInputActionsCallbacks()
        {
        }
        protected virtual void RemoveInputActionsCallbacks()
        {
        }
        public void SnapToFaceDirection()
        {
            stateMachine.Hero.Rigidbody.rotation = Quaternion.Euler(0f, facingDirection, 0f);
            targetRotation = Quaternion.Euler(0f, facingDirection, 0f);
        }
        #endregion
    }
}
