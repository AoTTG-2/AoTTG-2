using Assets.Scripts.UI.Input;
using System;
using UnityEngine;
using Zenject;

namespace Cannon
{
    internal sealed class MannedCannonState : CannonState, IInitializable, IDisposable
    {
        private readonly CannonBarrel barrel;
        private readonly CannonBase @base;
        private readonly Transform firePoint;
        private readonly CannonInput input;
        private readonly Interactable mountInteractable;
        private readonly ICannonOwnershipManager ownershipManager;
        private readonly Transform playerPoint;
        private readonly Interactable unmountInteractable;
        private Hero mountedHero;

        public MannedCannonState(
            CannonStateManager stateManager,
            ICannonOwnershipManager ownershipManager,
            CannonBase @base,
            CannonBarrel barrel,
            CannonInput input,
            Interactable mountInteractable,
            Interactable unmountInteractable,
            Transform firePoint,
            Transform playerPoint)
            : base(stateManager)
        {
            this.ownershipManager = ownershipManager;
            this.@base = @base;
            this.barrel = barrel;
            this.input = input;
            this.mountInteractable = mountInteractable;
            this.unmountInteractable = unmountInteractable;
            this.firePoint = firePoint;
            this.playerPoint = playerPoint;
        }

        void IDisposable.Dispose()
        {
            mountInteractable.Interacted.RemoveListener(OnMount);
            unmountInteractable.Interacted.RemoveListener(OnUnmount);
        }

        void IInitializable.Initialize()
        {
            mountInteractable.Interacted.AddListener(OnMount);
            unmountInteractable.Interacted.AddListener(OnUnmount);

            SetAvailability(false);
        }

        public override void Enter()
        {
            SetAvailability(true);

            mountedHero.OnMountingCannon(firePoint.gameObject);
            mountedHero.HeroDied += OnHeroDied;
        }

        public override void Exit()
        {
            SetAvailability(false);

            if (mountedHero)
            {
                mountedHero.OnUnmountingCannon();
                mountedHero.HeroDied -= OnHeroDied;
            }

            mountedHero = null;
        }

        public override void SetAvailability(bool isActive)
        {
            mountInteractable.Available.Value = !isActive;
            unmountInteractable.Available.Value = isActive;
        }

        public override void Update()
        {
            if (!mountedHero) return;

            var inputAxes = input.GetAxes();
            var slow = InputManager.Key(InputCannon.Slow);
            @base.Rotate(inputAxes.x, slow);
            barrel.Rotate(inputAxes.y, slow);

            if (InputManager.KeyDown(InputCannon.Shoot))
                barrel.TryFire(mountedHero.photonView.viewID);

            mountedHero.transform.SetPositionAndRotation(
                playerPoint.position,
                playerPoint.rotation);
        }

        private void OnMount(Hero hero)
        {
            mountedHero = hero;
            ownershipManager.LocalOwnershipTaken += OnLocalOwnershipTaken;
            ownershipManager.RequestOwnership();
        }

        private void OnUnmount(Hero _)
        {
            ownershipManager.RelinquishOwnership();
        }

        private void OnHeroDied(Hero _)
        {
            ownershipManager.RelinquishOwnership();
        }

        private void OnLocalOwnershipTaken()
        {
            StateManager.Transition<MannedCannonState>();
            ownershipManager.LocalOwnershipTaken -= OnLocalOwnershipTaken;
        }
    }
}