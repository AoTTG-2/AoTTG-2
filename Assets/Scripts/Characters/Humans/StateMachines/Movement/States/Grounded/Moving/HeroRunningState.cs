using Assets.Scripts.Characters.Humans.Constants;
using UnityEngine;

namespace Assets.Scripts.Characters.Humans.StateMachines.Movement.States.Grounded.Moving
{
    /// <summary>
    /// Running State Class.
    /// Defines everything that is needed and can be done while running.
    /// </summary>
    public class HeroRunningState : HeroMovingState
    {
        public HeroRunningState(HeroMovementStateMachine heroMovementStateMachine) : base(heroMovementStateMachine)
        {
        }
        #region IState Methods
        public override void Enter()
        {
            base.Enter();
            stateMachine.ReusableData.MovementSpeedModifier = movementData.BaseSpeed;
        }
        public override void Update()
        {
            base.Update();
            Move();
        }
        #endregion
        #region Main Methods
        private void Move()
        {
            if (stateMachine.ReusableData.MovementInput == Vector2.zero) return;

            Vector3 movementVector = GetMovementInputDirection();
            float resultAngle = GetGlobalFacingDirection(movementVector.x, movementVector.z);
            Vector3 zero = GetGlobalFacingVector3(resultAngle);
            float movementMagnitudeChecker = (movementVector.magnitude <= 0.95f) ? ((movementVector.magnitude >= 0.25f) ? movementVector.magnitude : 0f) : 1f;
            zero *= movementMagnitudeChecker;
            zero *= movementData.BaseSpeed;
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
            stateMachine.Hero.Rigidbody.rotation = Quaternion.Lerp(stateMachine.Hero.gameObject.transform.rotation, Quaternion.Euler(0f, facingDirection, 0f), Time.deltaTime * 10f);
            UpdateAnimation(HeroAnim.RUN_1);
        }

        #endregion
    }
}
