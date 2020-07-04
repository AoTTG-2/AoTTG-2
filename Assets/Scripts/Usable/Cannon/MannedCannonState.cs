using Assets.Scripts.UI.Input;
using System;
using UnityEngine;
using Zenject;

namespace Cannon
{
    internal sealed class MannedCannonState : CannonState, IInitializable, IDisposable
    {
        private readonly CannonBase @base;
        private readonly CannonBarrel barrel;
        private readonly Interactable mountInteractable;
        private readonly Settings settings;
        private readonly Interactable unmountInteractable;
        private readonly Transform firePoint;
        private readonly Transform playerPoint;
        private Hero mountedHero;

        public MannedCannonState(
            Settings settings,
            CannonStateManager stateManager,
            CannonBase @base,
            CannonBarrel barrel,
            Interactable mountInteractable,
            Interactable unmountInteractable,
            Transform firePoint,
            Transform playerPoint)
            : base(stateManager)
        {
            this.settings = settings;
            this.@base = @base;
            this.barrel = barrel;
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

        public override void Enter()
        {
            SetMountedAvailability();

            Camera.main.GetComponent<IN_GAME_MAIN_CAMERA>().setMainObject(firePoint.gameObject, true, false);
            Camera.main.fieldOfView = 55f;
            mountedHero.OnMountingCannon();
        }

        public override void Exit()
        {
            SetUnmountedAvailability();

            if (mountedHero)
            {
                mountedHero.isCannon = false;
                Camera.main.GetComponent<IN_GAME_MAIN_CAMERA>().setMainObject(mountedHero.gameObject, true, false);
                mountedHero.baseRigidBody.velocity = Vector3.zero;
                mountedHero.photonView.RPC(nameof(mountedHero.ReturnFromCannon), PhotonTargets.Others);
                mountedHero.skillCDLast = mountedHero.skillCDLastCannon;
                mountedHero.skillCDDuration = mountedHero.skillCDLast;
            }
        }

        void IInitializable.Initialize()
        {
            mountInteractable.Interacted.AddListener(OnMount);
            unmountInteractable.Interacted.AddListener(OnUnmount);

            SetUnmountedAvailability();
        }

        public override void Update()
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

            mountedHero.transform.SetPositionAndRotation(
                playerPoint.position,
                playerPoint.rotation);
        }

        private void OnMount(Hero hero)
        {
            mountedHero = hero;
            StateManager.Transition<MannedCannonState>();
        }

        private void OnUnmount(Hero hero)
        {
            Debug.Assert(mountedHero == hero, "Mounted Hero and unmounting Hero should match.");
            StateManager.Transition<UnmannedCannonState>();
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