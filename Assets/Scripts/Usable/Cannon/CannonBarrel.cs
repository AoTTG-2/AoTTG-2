using System;
using UnityEngine;
using Zenject;

namespace Cannon
{
    internal sealed class CannonBarrel
    {
        private readonly Transform barrel;

        private readonly CannonFacade cannon;
        private readonly CannonBall.Factory cannonBallFactory;
        private readonly Transform firePoint;
        private readonly Settings settings;
        private float lastFireTime;

        public CannonBarrel(
            Settings settings,
            CannonFacade cannon,
            CannonBall.Factory cannonBallFactory,
            Transform barrel,
            Transform firePoint)
        {
            this.settings = settings;
            this.cannon = cannon;
            this.cannonBallFactory = cannonBallFactory;
            this.barrel = barrel;
            this.firePoint = firePoint;
        }

        private bool CooldownActive =>
            Time.time < lastFireTime + settings.Cooldown;

        public void Rotate(float degrees)
        {
            barrel.Rotate(0f, 0f, degrees * Time.deltaTime);
        }

        public void TryFire()
        {
            if (!CooldownActive)
                Fire();
        }

        //[Inject]
        //private void Construct(CannonFacade cannon)
        //{
        //    this.cannon = cannon;
        //}

        private void Fire()
        {
            var velocity = firePoint.forward * settings.Force;
            cannonBallFactory.Create(cannon, velocity, firePoint.position, firePoint.rotation, 0);
            lastFireTime = Time.time;
        }

        [Serializable]
        public class Settings
        {
            public float Cooldown;
            public float Force;
        }
    }
}