using System;
using UnityEngine;

namespace Cannon
{
    internal sealed class GroundCannonWheels
    {
        private readonly Transform leftWheel;
        private readonly Transform rightWheel;
        private readonly Settings settings;

        public GroundCannonWheels(
            Settings settings,
            Transform leftWheel,
            Transform rightWheel)
        {
            this.settings = settings;
            this.leftWheel = leftWheel;
            this.rightWheel = rightWheel;
        }

        public void Turn(float degrees)
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