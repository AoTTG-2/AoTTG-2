using System;
using UnityEngine;

namespace Cannon
{
    internal class CannonBase
    {
        private readonly Transform @base;
        private readonly Settings settings;

        public CannonBase(
            Settings settings,
            Transform @base)
        {
            this.settings = settings;
            this.@base = @base;
        }

        public void Rotate(float degrees)
        {
            if (degrees == 0f) return;
            ApplyRotation(degrees);
            ClampRotation();
        }

        protected virtual void ApplyRotation(float degrees)
        {
            @base.Rotate(0f, degrees * Time.deltaTime, 0f);
        }

        private void ClampRotation()
        {
            var newRotation = @base.rotation.eulerAngles;
            newRotation.y = MathfExtras.ClampAngle(newRotation.y, settings.MinRotation, settings.MaxRotation);
            @base.rotation = Quaternion.Euler(newRotation);
        }

        [Serializable]
        public class Settings
        {
            public float MinRotation = 0f;
            public float MaxRotation = 360f;
        }
    }
}