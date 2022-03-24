using UnityEngine.InputSystem;

namespace Assets.Scripts.Characters.Humans.StateMachines.Movement.States.Grounded
{
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
        private void OnDodgeStarted(InputAction.CallbackContext contenxt)
        {
            stateMachine.ChangeState(stateMachine.DodgingState);
        }

        private void OnMovementCanceled(InputAction.CallbackContext context)
        {
            stateMachine.ChangeState(stateMachine.IdlingState);
        }
        #endregion
    }
}