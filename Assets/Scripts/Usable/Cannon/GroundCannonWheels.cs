using System;
using UnityEngine;

namespace Cannon
{
    internal sealed class GroundCannonWheels
    {
        private readonly Transform @base;
        private readonly Transform leftWheel;
        private readonly Transform rightWheel;
        private readonly Settings settings;

        public GroundCannonWheels(
            Settings settings,
            Transform @base,
            Transform leftWheel,
            Transform rightWheel)
        {
            this.settings = settings;
            this.@base = @base;
            this.leftWheel = leftWheel;
            this.rightWheel = rightWheel;
        }

        public void Turn(float degrees)
        {
            var frameDegrees = degrees * Time.deltaTime * settings.WheelTurnRotationSpeed;
            leftWheel.Rotate(frameDegrees, 0f, 0f);
            rightWheel.Rotate(-frameDegrees, 0f, 0f);
        }

        public void Move(float input, bool slow)
        {
            var speed = slow ? settings.SlowSpeed : settings.NormalSpeed;
            var movement = input * Time.deltaTime * speed;
            @base.Translate(0f, 0f, movement);
            var frameDegrees = movement * settings.WheelMoveRotationSpeed;
            leftWheel.Rotate(frameDegrees, 0f, 0f);
            rightWheel.Rotate(frameDegrees, 0f, 0f);
        }

        [Serializable]
        public class Settings
        {
            public float NormalSpeed = 5f;
            public float SlowSpeed = 2f;
            public float WheelTurnRotationSpeed = 1f;
            public float WheelMoveRotationSpeed = 30f;
        }
    }
}