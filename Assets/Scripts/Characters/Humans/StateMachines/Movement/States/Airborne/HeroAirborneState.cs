using Assets.Scripts.Characters.Humans.Constants;
using Assets.Scripts.UI.Input;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Scripts.Characters.Humans.StateMachines.Airborne
{
    public class HeroAirborneState : HeroMovementState
    {
        private bool bindOnEnter;
        protected bool Falling => GetPlayerVerticalVelocity() < 0f;
        protected bool InRiseThreshold => GetPlayerVerticalVelocity() < 25f;
        public HeroAirborneState(HeroMovementStateMachine heroMovementStateMachine) : base(heroMovementStateMachine)
        {
        }
        #region IState Methods
        public override void Enter()
        {
            base.Enter();
            if (stateMachine.PreviousState == stateMachine.HookedState && GetPlayerVerticalVelocity() > 20f)
            {
                UpdateAnimation(HeroAnim.AIR_RELEASE);
            }
        }
        public override void Exit()
        {
            base.Exit();
            stateMachine.Hero.EmitSmoke(false);
        }
        public override void Update()
        {
            if (stateMachine.ReusableData.IsGrounded)
            {
                if (stateMachine.ReusableData.MovementInput == Vector2.zero) stateMachine.ChangeState(stateMachine.IdlingState);
                else stateMachine.ChangeState(stateMachine.RunningState);
                return;
            }
            if (stateMachine.ReusableData.IsHooked)
            {
                stateMachine.ChangeState(stateMachine.HookedState);
                return;
            }
            if (Falling && stateMachine.ReusableData.CurrentAnimation != HeroAnim.AIR_FALL)
            {
                UpdateAnimation(HeroAnim.AIR_FALL, 0.2f);
            }
            else if(!InRiseThreshold)
            {
                PlayAnimationForDirection();
            }
            if (stateMachine.ReusableData.JumpHeld && stateMachine.ReusableData.CurrentGas > 0)
            {
                UseGas(stateMachine.ReusableData.UseGasSpeed * Time.deltaTime);
                stateMachine.Hero.EmitSmoke(true);
            }
            else if(!stateMachine.ReusableData.JumpHeld || stateMachine.ReusableData.CurrentGas <= 0)
            {
                stateMachine.Hero.EmitSmoke(false);
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
        private void OnGasBurstStarted(InputAction.CallbackContext context)
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
        protected void PlayAnimationForDirection()
        {
            if (InRiseThreshold) return;
            if (stateMachine.ReusableData.CurrentAnimation == HeroAnim.AIR_RELEASE)
            {
                return;
            }
            if (!stateMachine.ReusableData.BothHooked)
            {
                float directionHeading = -Mathf.Atan2(GetPlayerHorizontalVelocity().z, GetPlayerHorizontalVelocity().x) * Mathf.Rad2Deg;
                float angleHeading = -Mathf.DeltaAngle(directionHeading, stateMachine.Hero.transform.eulerAngles.y - 90);

                if (Mathf.Abs(angleHeading) < 45f && stateMachine.ReusableData.CurrentAnimation != HeroAnim.AIR2)
                {
                    UpdateAnimation(HeroAnim.AIR2, 0.2f);
                }
                else if (angleHeading < 135f && angleHeading > 0f)
                {
                    if (stateMachine.ReusableData.CurrentAnimation != HeroAnim.AIR2_RIGHT)
                    {
                        UpdateAnimation(HeroAnim.AIR2_RIGHT, 0.2f);
                    }
                    return;
                }
                else if (angleHeading > -135f && angleHeading < 0f)
                {
                    if (stateMachine.ReusableData.CurrentAnimation != HeroAnim.AIR2_LEFT)
                    {
                        UpdateAnimation(HeroAnim.AIR2_LEFT, 0.2f);
                    }
                    return;
                }
                else if (stateMachine.ReusableData.CurrentAnimation != HeroAnim.AIR2_BACKWARD)
                {
                    UpdateAnimation(HeroAnim.AIR2_BACKWARD, 0.2f);
                }
            }
            else if (!stateMachine.ReusableData.RightHookHooked
                &&
                stateMachine.ReusableData.CurrentAnimation != stateMachine.Hero.Equipment.Weapon.HookForwardLeft)
            {
                UpdateAnimation(stateMachine.Hero.Equipment.Weapon.HookForwardLeft);
            }
            else if (!stateMachine.ReusableData.LeftHookHooked
                &&
                stateMachine.ReusableData.CurrentAnimation != stateMachine.Hero.Equipment.Weapon.HookForwardRight)
            {
                UpdateAnimation(stateMachine.Hero.Equipment.Weapon.HookForwardRight);
            }
            else if (stateMachine.ReusableData.CurrentAnimation != stateMachine.Hero.Equipment.Weapon.HookForward)
            {
                UpdateAnimation(stateMachine.Hero.Equipment.Weapon.HookForward);
            }
        }
        #endregion
    }
}