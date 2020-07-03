using UnityEngine;
using Zenject;

namespace Cannon
{
    internal sealed class CannonBase
    {
        private Transform @base;

        public CannonBase(
            [Inject(Id = "Base")]
            Transform @base)
        {
            this.@base = @base;
        }

        public void Rotate(float degrees)
        {
            @base.Rotate(0f, degrees * Time.deltaTime, 0f);
        }
    }
}