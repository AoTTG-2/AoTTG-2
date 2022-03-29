using Assets.Scripts.Characters.Humans.Constants;
using UnityEngine;

namespace Assets.Scripts.Characters.Humans.StateMachines.Movement.States.Grounded
{
    /// <summary>
    /// Dodging State Class.
    /// Defines everything that is needed and can be done while the hero is dodging.
    /// Requires its animation to have a reference to the OnAnimationExitEvent funciton in Hero.cs.
    /// </summary>
    public class HeroDodgingState : HeroGroundedState
    {
        public HeroDodgingState(HeroMovementStateMachine heroMovementStateMachine) : base(heroMovementStateMachine)
        {
        }
        #region IState Methods
        public override void Enter()
        {
            base.Enter();
            Dodge();
        }
        //Need to override base so that hero can't trigger any buttons while dodging
        protected override void AddInputActionsCallbacks()
        {
        }
        protected override void RemoveInputActionsCallbacks()
        {
        }
        public override void OnAnimationExitEvent()
        {
            if (stateMachine.ReusableData.MovementInput != Vector2.zero)
            {
                stateMachine.ChangeState(stateMachine.RunningState);
                return;
            }
            stateMachine.ChangeState(stateMachine.IdlingState);
        }
        #endregion
        #region Main Methods
        private void Dodge()
        {
            UpdateAnimation();

            Vector3 movementVector = GetMovementInputDirection();
            if (movementVector != Vector3.zero)
            {
                stateMachine.Hero.transform.rotation *= Quaternion.Euler(0, 180f, 0);
            }

            Vector3 zero = (-stateMachine.Hero.transform.forward * 2.4f) * movementData.BaseSpeed;
            Vector3 currentVelocity = GetPlayerHorizontalVelocity();
            Vector3 force = zero - currentVelocity;
            force.x = Mathf.Clamp(force.x, -maxVelocityChange * 2, maxVelocityChange * 2); //quick fix for roll distance. needs adjusting
            force.z = Mathf.Clamp(force.z, -maxVelocityChange * 2, maxVelocityChange * 2); //quick fix for roll distance. needs adjusting
            force.y = 0f;

            ResetVelocity();
            stateMachine.Hero.Rigidbody.AddForce(force, ForceMode.VelocityChange);
            stateMachine.Hero.Rigidbody.rotation = Quaternion.Lerp(stateMachine.Hero.gameObject.transform.rotation, Quaternion.Euler(0f, facingDirection, 0f), Time.deltaTime * 10f);         
        }
        private void UpdateAnimation()
        {
            CrossFade(HeroAnim.DODGE);
        }
        #endregion
    }
}