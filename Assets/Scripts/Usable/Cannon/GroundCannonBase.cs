using UnityEngine;

namespace Cannon
{
    internal sealed class GroundCannonBase : CannonBase
    {
        private readonly GroundCannonWheels wheels;

        public GroundCannonBase(
            Settings baseSettings,
            Transform @base,
            GroundCannonWheels wheels)
            : base(baseSettings, @base)
        {
            this.wheels = wheels;
        }

        protected override void ApplyRotation(float degrees)
        {
            base.ApplyRotation(degrees);
            wheels.Turn(degrees);
        }
    }
}