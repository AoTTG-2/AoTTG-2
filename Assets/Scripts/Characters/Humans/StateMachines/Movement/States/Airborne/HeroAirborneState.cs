using Assets.Scripts.UI.Input;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Scripts.Characters.Humans.StateMachines.Airborne
{
    public class HeroAirborneState : HeroMovementState
    {
        private bool bindOnEnter;
        public HeroAirborneState(HeroMovementStateMachine heroMovementStateMachine) : base(heroMovementStateMachine)
        {
        }
        #region IState Methods
        public override void Update()
        {
            if (stateMachine.ReusableData.IsGrounded)
            {
                stateMachine.ChangeState(stateMachine.IdlingState);
            }
        }
        protected override void AddInputActionsCallbacks()
        {
            bindOnEnter = InputManager.Settings.GasBurstDoubleTap;
            if (bindOnEnter)
            {
                stateMachine.Hero.HumanInput.HumanActions.GasBurstDoubleTap.started += OnGasBurstStarted;
            }
            else
            {
                stateMachine.Hero.HumanInput.HumanActions.Dodge.started += OnGasBurstStarted;
            }
        }
        protected override void RemoveInputActionsCallbacks()
        {
            if (bindOnEnter)
            {
                stateMachine.Hero.HumanInput.HumanActions.GasBurstDoubleTap.started -= OnGasBurstStarted;
            }
            else
            {
                stateMachine.Hero.HumanInput.HumanActions.Dodge.started -= OnGasBurstStarted;
            }
        }
        #endregion
        #region Input Actions
        private void OnGasBurstStarted(InputAction.CallbackContext obj)
        {
            if (Time.time - airborneData.LastPerformedDash >= airborneData.DashCooldown)
            {
                airborneData.LastPerformedDash = Time.time;
                stateMachine.ChangeState(stateMachine.DashingState);
            }
        }
        #endregion

    }
}