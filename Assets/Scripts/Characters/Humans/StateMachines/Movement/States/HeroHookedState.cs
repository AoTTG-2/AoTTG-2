using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Scripts.Characters.Humans.StateMachines.Movement
{
    public class HeroHookedState : HeroMovementState
    {
        private float launchElapsedTimeL = 0;
        private float launchElapsedTimeR = 0;
        private bool canUseGas = false;
        private bool canReelOffLeftHook = false;
        private bool canReelOffRightHook = false;
        private float reelInputDirection;
        public HeroHookedState(HeroMovementStateMachine heroMovementStateMachine) : base(heroMovementStateMachine)
        {
        }
        #region IState Methods
        protected override void AddInputActionsCallbacks()
        {
            base.AddInputActionsCallbacks();
            stateMachine.Hero.HumanInput.HumanActions.ReelMnK.started += OnReelPressed;
            stateMachine.Hero.HumanInput.HumanActions.ReelMnK.canceled += OnReelPressed;
        }

        protected override void RemoveInputActionsCallbacks()
        {
            base.RemoveInputActionsCallbacks();
            stateMachine.Hero.HumanInput.HumanActions.ReelMnK.started -= OnReelPressed;
            stateMachine.Hero.HumanInput.HumanActions.ReelMnK.canceled -= OnReelPressed;
        }
        public override void PhysicsUpdate()
        {
            PullTowardsHook();
        }
        public override void Update()
        {
            if (stateMachine.ReusableData.IsHooked && stateMachine.ReusableData.JumpHeld && stateMachine.ReusableData.CurrentGas > 0)
            {
                UseGas(stateMachine.ReusableData.UseGasSpeed * Time.deltaTime);
                stateMachine.Hero.EmitSmoke(true);
            }
            else if (stateMachine.ReusableData.IsHooked && (!stateMachine.ReusableData.JumpHeld || stateMachine.ReusableData.CurrentGas <= 0))
            {
                stateMachine.Hero.EmitSmoke(false);
            }
            if (!stateMachine.ReusableData.IsGrounded && !stateMachine.ReusableData.IsHooked)
            {
                if (stateMachine.ReusableData.MovementInput == Vector2.zero)
                {
                    stateMachine.ChangeState(stateMachine.AirborneState);
                }
                else
                {
                    stateMachine.ChangeState(stateMachine.AirborneMovingState);
                }
                return;
            }
            else if (stateMachine.ReusableData.IsGrounded && !stateMachine.ReusableData.IsHooked)
            {
                stateMachine.ChangeState(stateMachine.IdlingState);
                return;
            }
        }
        public override void Exit()
        {
            base.Exit();
            stateMachine.Hero.EmitSmoke(false);
            canReelOffLeftHook = false;
            canReelOffRightHook = false;
            canUseGas = false;
        }
        #endregion
        #region Main Methods
        private void InitialForce()
        {
            Vector3 movementInput = stateMachine.ReusableData.MovementInput;
            float facingDirection = GetGlobalFacingDirection(movementInput.x, movementInput.z);
            Vector3 facingVector = GetGlobalFacingVector3(facingDirection);
            float movementMagnituteCheck = (movementInput.magnitude <= 0.95f) ? ((movementInput.magnitude >= 0.25f) ? movementInput.magnitude : 0f) : 1f;
            facingVector *= movementMagnituteCheck;
            facingVector *= ((stateMachine.ReusableData.Acceleration / 10f) * 2f);

            if (movementInput == Vector3.zero)
            {
                facingDirection = -874f;
            }

            if (facingDirection != -874f)
            {
                targetRotation = Quaternion.Euler(0f, facingDirection, 0f);
            }

            if (!canReelOffLeftHook && !canReelOffRightHook && stateMachine.ReusableData.CurrentGas > 0 && stateMachine.ReusableData.JumpHeld)
            {
                if (movementInput != Vector3.zero)
                {
                    stateMachine.Hero.Rigidbody.AddForce(facingVector, ForceMode.Acceleration);
                }
                else
                {
                    stateMachine.Hero.Rigidbody.AddForce(stateMachine.Hero.transform.forward * facingVector.magnitude, ForceMode.Acceleration);
                }
                canUseGas = true;
            }
        }
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
                if (Vector3.Angle(stateMachine.Hero.Rigidbody.velocity, directionToHook) > 90 && stateMachine.ReusableData.JumpHeld)
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
                    canReelOffLeftHook = false;
                    stateMachine.ReusableData.LeftHook.disable();
                    stateMachine.ReusableData.LeftHook = null;
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
                if (Vector3.Angle(stateMachine.Hero.Rigidbody.velocity, directionToHook) > 90 && stateMachine.ReusableData.JumpHeld)
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
                    canReelOffRightHook = false;
                    stateMachine.ReusableData.RightHook.disable();
                    stateMachine.ReusableData.RightHook = null;
                }
            }
        }
        private void Reel()
        {
            float speed = stateMachine.Hero.Rigidbody.velocity.magnitude + 0.1f;
            if (canReelOffLeftHook && canReelOffRightHook)
            {
                Vector3 reelDirection = ((stateMachine.ReusableData.RightHook.transform.position +
                    stateMachine.ReusableData.LeftHook.transform.position) * 0.5f) -
                    stateMachine.Hero.transform.position;
                reelInputDirection++;

                Vector3 rotationVector = Vector3.RotateTowards(reelDirection,
                    stateMachine.Hero.Rigidbody.velocity, 1.53938f * reelInputDirection, 1.53938f * reelInputDirection);
                rotationVector.Normalize();
                
                stateMachine.Hero.spinning = true;
                stateMachine.Hero.Rigidbody.velocity = rotationVector * speed;
            }
            else if (canReelOffLeftHook)
            {
                Vector3 reelDirection = stateMachine.ReusableData.LeftHook.transform.position - stateMachine.Hero.transform.position;
                reelInputDirection++;
                Vector3 rotationVector = Vector3.RotateTowards(reelDirection,
                    stateMachine.Hero.Rigidbody.velocity, 1.53938f * reelInputDirection, 1.53938f * reelInputDirection);
                rotationVector.Normalize();

                stateMachine.Hero.spinning = true;
                stateMachine.Hero.Rigidbody.velocity = rotationVector * speed;
            }
            else if (canReelOffRightHook)
            {
                Vector3 reelDirection = stateMachine.ReusableData.RightHook.transform.position - stateMachine.Hero.transform.position;
                reelInputDirection++;
                Vector3 rotationVector = Vector3.RotateTowards(reelDirection,
                    stateMachine.Hero.Rigidbody.velocity, 1.53938f * reelInputDirection, 1.53938f * reelInputDirection);
                rotationVector.Normalize();

                stateMachine.Hero.spinning = true;
                stateMachine.Hero.Rigidbody.velocity = rotationVector * speed;
            }
        }
        private void PullTowardsHook()
        {
            InitialForce();
            LeftHookPull();
            RightHookPull();
            Reel();
        }
        #endregion
        #region Input Actions
        private void OnReelPressed(InputAction.CallbackContext context)
        {
            reelInputDirection = Mathf.Clamp(context.ReadValue<float>(), -0.8f, 0.8f);
        }
        #endregion
    }
}
