using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Characters.Humans.Data.States
{
    public class HeroStateReusableData
    {
        public float Acceleration { get; set; } = 150f;
        public string CurrentAnimation { get; set; }
        public Vector2 MovementInput { get; set; }
        public bool JumpHeld { get; set; }
        public bool IsGrounded { get; set; }
        public float UseGasSpeed { get; set; } = 0.2f;
        public float CurrentGas { get; set; } = 100f;
        public float MovementSpeedModifier { get; set; } = 1f;
        public float HookRayCastDistance { get; private set; } = 1000f;
        public Bullet LeftHook { get; set; }
        public Bullet RightHook { get; set; }
        public bool LeftHookHeld { get; set; }
        public bool RightHookHeld { get; set; }
        public bool IsHooked => LeftHook?.isHooked() ?? RightHook?.isHooked() ?? false;
        public bool BothHooked => LeftHook && RightHook;
        public float TimeUntilHookDespawn { get; private set; } = 0.3f;

        private Vector3 currentTargetRotation;
        private Vector3 timeToReachTargetRotation;
        private Vector3 dampedTargetRotationCurrentVelocity;
        private Vector3 dampedTargetRotationPassedTime;
        public ref Vector3 CurrentTargetRotation
        {
            get
            {
                return ref currentTargetRotation;
            }
        }
        public ref Vector3 TimeToReachTargetRotation
        {
            get
            {
                return ref timeToReachTargetRotation;
            }
        }
        public ref Vector3 DampedTargetRotationCurrentVelocity
        {
            get
            {
                return ref dampedTargetRotationCurrentVelocity;
            }
        }
        public ref Vector3 DampedTargetRotationPassedTime
        {
            get
            {
                return ref dampedTargetRotationPassedTime;
            }
        }
    }
}
