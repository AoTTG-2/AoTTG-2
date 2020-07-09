using System;
using UnityEngine;

namespace Cannon
{
    internal sealed class CannonBase
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
            @base.Rotate(0f, degrees * Time.deltaTime, 0f);
            ClampRotation();
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