using System;
using UnityEngine;

namespace Cannon
{
    internal sealed class GroundCannonBase : CannonBase
    {
        private readonly Transform leftWheel;
        private readonly Transform rightWheel;
        private readonly Settings settings;

        public GroundCannonBase(
            CannonBase.Settings baseSettings,
            Transform @base,
            Settings settings,
            Transform leftWheel,
            Transform rightWheel)
            : base(baseSettings, @base)
        {
            this.settings = settings;
            this.leftWheel = leftWheel;
            this.rightWheel = rightWheel;
        }

        protected override void ApplyRotation(float degrees)
        {
            base.ApplyRotation(degrees);
            RotateWheels(degrees);
        }

        private void RotateWheels(float degrees)
        {
            var frameDegrees = degrees * Time.deltaTime * settings.WheelRotationSpeed;
            leftWheel.Rotate(frameDegrees, 0f, 0f);
            rightWheel.Rotate(-frameDegrees, 0f, 0f);
        }

        [Serializable]
        public class Settings
        {
            public float WheelRotationSpeed = 1f;
        }
    }
}