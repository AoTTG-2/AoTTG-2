using Assets.Scripts.UI.Input;
using System;
using UnityEngine;
using Zenject;

namespace Cannon
{
    internal sealed class MannedCannonState : ICannonState, IInitializable, IDisposable
    {
        private readonly CannonBase @base;
        private readonly CannonBarrel barrel;
        private readonly Interactable mountInteractable;
        private readonly Settings settings;
        private readonly CannonStateManager stateManager;
        private readonly Interactable unmountInteractable;
        private Hero mountedHero;

        public MannedCannonState(
            Settings settings,
            CannonStateManager stateManager,
            CannonBase @base,
            CannonBarrel barrel,
            [Inject(Id = "MountInteractable")]
            Interactable mountInteractable,
            [Inject(Id = "UnmountInteractable")]
            Interactable unmountInteractable)
        {
            this.settings = settings;
            this.stateManager = stateManager;
            this.@base = @base;
            this.barrel = barrel;
            this.mountInteractable = mountInteractable;
            this.unmountInteractable = unmountInteractable;
        }

        void IDisposable.Dispose()
        {
            mountInteractable.Interacted.RemoveListener(OnMount);
            unmountInteractable.Interacted.RemoveListener(OnUnmount);
        }

        void ICannonState.Enter()
        {
            Debug.Log($"Mounted hero: {mountedHero.name}");
            SetMountedAvailability();
        }

        void ICannonState.Exit()
        {
            SetUnmountedAvailability();
        }

        void IInitializable.Initialize()
        {
            mountInteractable.Interacted.AddListener(OnMount);
            unmountInteractable.Interacted.AddListener(OnUnmount);

            SetUnmountedAvailability();
        }

        void ICannonState.Update()
        {
            var left = InputManager.Key(InputCannon.Left) ? -1f : 0f;
            var right = InputManager.Key(InputCannon.Right) ? 1f : 0f;
            var up = InputManager.Key(InputCannon.Up) ? 1f : 0f;
            var down = InputManager.Key(InputCannon.Down) ? -1f : 0f;
            var speed = InputManager.Key(InputCannon.Slow) ? settings.SlowSpeed : settings.NormalSpeed;
            var x = (left + right) * speed;
            var y = (up + down) * speed;

            @base.Rotate(x);
            barrel.Rotate(y);

            if (InputManager.KeyDown(InputCannon.Shoot))
                barrel.TryFire();
        }

        private void OnMount(Hero hero)
        {
            mountedHero = hero;
            stateManager.Transition<MannedCannonState>();
        }

        private void OnUnmount(Hero hero)
        {
            Debug.Assert(mountedHero == hero, "Mounted Hero and unmounting Hero should match.");
            stateManager.Transition<UnmannedCannonState>();
        }

        private void SetMountedAvailability()
        {
            mountInteractable.Available.Value = false;
            unmountInteractable.Available.Value = true;
        }

        private void SetUnmountedAvailability()
        {
            mountInteractable.Available.Value = true;
            unmountInteractable.Available.Value = false;
        }

        [Serializable]
        public class Settings
        {
            public float NormalSpeed;
            public float SlowSpeed;
        }
    }
}