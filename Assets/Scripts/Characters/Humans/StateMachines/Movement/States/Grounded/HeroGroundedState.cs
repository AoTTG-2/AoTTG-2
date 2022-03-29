using UnityEngine.InputSystem;

namespace Assets.Scripts.Characters.Humans.StateMachines.Movement.States.Grounded
{
    /// <summary>
    /// Grounded State Class.
    /// Defines everything that is needed and can be done while on the ground.
    /// </summary>
    public class HeroGroundedState : HeroMovementState
    {
        public HeroGroundedState(HeroMovementStateMachine heroMovementStateMachine) : base(heroMovementStateMachine)
        {
        }
        #region Reusable Methods
        protected override void AddInputActionsCallbacks()
        {
            base.AddInputActionsCallbacks();
            stateMachine.Hero.HumanInput.HumanActions.Move.canceled += OnMovementCanceled;
            stateMachine.Hero.HumanInput.HumanActions.Dodge.started += OnDodgeStarted;
        }
        protected override void RemoveInputActionsCallbacks()
        {
            base.RemoveInputActionsCallbacks();
            stateMachine.Hero.HumanInput.HumanActions.Move.canceled -= OnMovementCanceled;
            stateMachine.Hero.HumanInput.HumanActions.Dodge.started -= OnDodgeStarted;
        }
        protected void OnMove()
        {
            stateMachine.ChangeState(stateMachine.RunningState);
        }
        #endregion
        #region Input Methods
        private void OnDodgeStarted(InputAction.CallbackContext context)
        {
            stateMachine.ChangeState(stateMachine.DodgingState);
        }

        private void OnMovementCanceled(InputAction.CallbackContext context)
        {
            if (stateMachine.CurrentState == stateMachine.DodgingState) return;     // This keeps the state from changing while dodging. As it was if you dodged while moving in a direction it would stop the dodge partway through if you let go of the movement key
            stateMachine.ChangeState(stateMachine.IdlingState);
        }
        #endregion
    }
}