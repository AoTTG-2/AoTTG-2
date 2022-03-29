using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Scripts.Characters.Humans.StateMachines
{
    public class HeroAirborneState : HeroMovementState
    {
        public HeroAirborneState(HeroMovementStateMachine heroMovementStateMachine) : base(heroMovementStateMachine)
        {
        }
        #region IState Methods
        protected override void AddInputActionsCallbacks()
        {
            stateMachine.Hero.HumanInput.HumanActions.Dodge.started += OnDodgeStarted;
        }
        protected override void RemoveInputActionsCallbacks()
        {

        }
        #endregion
        #region Input Actions
        private void OnDodgeStarted(InputAction.CallbackContext obj)
        {
            //Transition to air dodge state
            throw new NotImplementedException();
        }
        #endregion

    }
}