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
                if (stateMachine.ReusableData.MovementInput == Vector2.zero) stateMachine.ChangeState(stateMachine.IdlingState);
                else stateMachine.ChangeState(stateMachine.RunningState);
                return;
            }
            if (stateMachine.ReusableData.MovementInput == Vector2.zero) return;
            OnMove();
        }
        protected override void AddInputActionsCallbacks()
        {
            base.AddInputActionsCallbacks();
            bindOnEnter = InputManager.Settings.GasBurstDoubleTap;
            if (bindOnEnter)
            {
                stateMachine.Hero.HumanInput.HumanActions.GasBurstDoubleTap.performed += OnGasBurstStarted;
            }
            else
            {
                stateMachine.Hero.HumanInput.HumanActions.Dodge.started += OnGasBurstStarted;
            }
        }
        protected override void RemoveInputActionsCallbacks()
        {
            base.RemoveInputActionsCallbacks();
            if (bindOnEnter)
            {
                stateMachine.Hero.HumanInput.HumanActions.GasBurstDoubleTap.performed -= OnGasBurstStarted;
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
        #region Main Methods
        private void OnMove()
        {
            stateMachine.ChangeState(stateMachine.AirborneMovingState);
        }
        #endregion
    }
}