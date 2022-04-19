using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Scripts.Characters.Humans.StateMachines.Movement
{
    public class HeroHookedState : HeroMovementState
    {
        private bool gasHeld;
        private float launchElapsedTimeL;
        private float launchElapsedTimeR;
        bool canUseGas = false;
        bool canReelOffLeftHook = false;
        bool canReelOffRightHook = false;
        public HeroHookedState(HeroMovementStateMachine heroMovementStateMachine) : base(heroMovementStateMachine)
        {
        }
        #region IState Methods
        protected override void AddInputActionsCallbacks()
        {
            base.AddInputActionsCallbacks();
            stateMachine.Hero.HumanInput.HumanActions.Jump.started += OnJumpStarted;
        }
        protected override void RemoveInputActionsCallbacks()
        {
            base.RemoveInputActionsCallbacks();
            stateMachine.Hero.HumanInput.HumanActions.Jump.started -= OnJumpStarted;
        }
        public override void PhysicsUpdate()
        {
            PullTowardsHook();
        }
        #endregion
        #region Main Methods
        private void LeftHookPull()
        {
            if (stateMachine.ReusableData.LeftHook != null && stateMachine.ReusableData.LeftHook.isHooked())
            {
                Vector3 directionToHook = stateMachine.ReusableData.LeftHook.transform.position - stateMachine.Hero.transform.position;
                directionToHook.Normalize();
                directionToHook *= 10;
                if (stateMachine.ReusableData.RightHook == null || !stateMachine.ReusableData.RightHook.isHooked())
                {
                    directionToHook *= 2;
                }
                if (Vector3.Angle(stateMachine.Hero.Rigidbody.velocity, directionToHook) > 90 && gasHeld)
                {
                    canUseGas = true;
                    canReelOffLeftHook = true;
                }
                if (!canReelOffLeftHook)
                {
                    stateMachine.Hero.Rigidbody.AddForce(directionToHook);
                    if (Vector3.Angle(stateMachine.Hero.Rigidbody.velocity, directionToHook) > 90f)
                    {
                        stateMachine.Hero.Rigidbody.AddForce((-stateMachine.Hero.Rigidbody.velocity * 2f), ForceMode.Acceleration);
                    }
                }
            }
            launchElapsedTimeL += Time.deltaTime;
            if (stateMachine.ReusableData.LeftHookHeld && stateMachine.ReusableData.CurrentGas > 0)
            {
                UseGas(stateMachine.ReusableData.UseGasSpeed * Time.deltaTime);
            }
            //TODO: Add the Hook Someone Logic
            else if (launchElapsedTimeL > stateMachine.ReusableData.TimeUntilHookDespawn)
            {
                if (stateMachine.ReusableData.LeftHook != null)
                {
                    stateMachine.ReusableData.LeftHook.disable();
                    stateMachine.ReusableData.LeftHook = null;
                    canReelOffLeftHook = false;
                }
            }
        }
        private void RightHookPull()
        {
            if (stateMachine.ReusableData.RightHook != null && stateMachine.ReusableData.RightHook.isHooked())
            {
                Vector3 directionToHook = stateMachine.ReusableData.RightHook.transform.position - stateMachine.Hero.transform.position;
                directionToHook.Normalize();
                directionToHook *= 10;
                if (stateMachine.ReusableData.LeftHook == null || !stateMachine.ReusableData.LeftHook.isHooked())
                {
                    directionToHook *= 2;
                }
                if (Vector3.Angle(stateMachine.Hero.Rigidbody.velocity, directionToHook) > 90 && gasHeld)
                {
                    canUseGas = true;
                    canReelOffRightHook = true;
                }
                if (!canReelOffRightHook)
                {
                    stateMachine.Hero.Rigidbody.AddForce(directionToHook);
                    if (Vector3.Angle(stateMachine.Hero.Rigidbody.velocity, directionToHook) > 90f)
                    {
                        stateMachine.Hero.Rigidbody.AddForce((-stateMachine.Hero.Rigidbody.velocity * 2f), ForceMode.Acceleration);
                    }
                }
            }
            launchElapsedTimeR += Time.deltaTime;
            if (stateMachine.ReusableData.RightHookHeld && stateMachine.ReusableData.CurrentGas > 0)
            {
                UseGas(stateMachine.ReusableData.UseGasSpeed * Time.deltaTime);
            }
            //TODO: Add the Hook Someone Logic
            else if (launchElapsedTimeR > stateMachine.ReusableData.TimeUntilHookDespawn)
            {
                if (stateMachine.ReusableData.RightHook != null)
                {
                    stateMachine.ReusableData.RightHook.disable();
                    stateMachine.ReusableData.RightHook = null;
                    canReelOffLeftHook = false;
                }
            }
        }
        private void PullTowardsHook()
        {
            LeftHookPull();
            RightHookPull();
        }
        #endregion
        #region IState Methods
        private void OnJumpStarted(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                gasHeld = true;
            }
            if(context.canceled)
            {
                gasHeld = false;
            }
        }
        #endregion
    }
}
