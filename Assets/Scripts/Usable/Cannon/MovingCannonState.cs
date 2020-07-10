using System;
using Assets.Scripts.UI.Input;
using UnityEngine;
using Zenject;

namespace Cannon
{
    internal sealed class MovingCannonState : CannonState, IInitializable, IDisposable
    {
        private readonly CannonBase @base;
        private readonly CannonInput input;
        private readonly Transform movePoint;
        private readonly ICannonOwnershipManager ownershipManager;
        private readonly Interactable startMovingInteractable;
        private readonly Interactable stopMovingInteractable;
        private readonly GroundCannonWheels wheels;
        private Hero mountedHero;

        public MovingCannonState(
            CannonStateManager stateManager,
            ICannonOwnershipManager ownershipManager,
            CannonInput input,
            CannonBase @base,
            GroundCannonWheels wheels,
            Interactable startMovingInteractable,
            Interactable stopMovingInteractable,
            Transform movePoint)
            : base(stateManager)
        {
            this.ownershipManager = ownershipManager;
            this.input = input;
            this.@base = @base;
            this.wheels = wheels;
            this.startMovingInteractable = startMovingInteractable;
            this.stopMovingInteractable = stopMovingInteractable;
            this.movePoint = movePoint;
        }

        void IDisposable.Dispose()
        {
            startMovingInteractable.Interacted.RemoveListener(OnStartMoving);
            stopMovingInteractable.Interacted.RemoveListener(OnStopMoving);
        }

        void IInitializable.Initialize()
        {
            startMovingInteractable.Interacted.AddListener(OnStartMoving);
            stopMovingInteractable.Interacted.AddListener(OnStopMoving);

            SetAvailability(false);
        }

        private void OnLocalOwnershipTaken()
        {
            StateManager.Transition<MovingCannonState>();
            ownershipManager.LocalOwnershipTaken -= OnLocalOwnershipTaken;
        }

        public override void Enter()
        {
            SetAvailability(true);
            
            mountedHero.OnMountingCannon();
            mountedHero.HeroDied += OnHeroDied;
        }

        private void OnHeroDied(Hero hero)
        {
            ownershipManager.RelinquishOwnership();
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

        public override void Update()
        {
            if (!mountedHero) return;

            var inputAxes = input.GetAxes();
            var slow = InputManager.Key(InputCannon.Slow);
            @base.Rotate(inputAxes.x, slow);
            wheels.Move(inputAxes.y, slow);
            
            mountedHero.transform.SetPositionAndRotation(
                movePoint.position,
                movePoint.rotation);
        }

        private void OnStartMoving(Hero hero)
        {
            mountedHero = hero;
            ownershipManager.LocalOwnershipTaken += OnLocalOwnershipTaken;
            ownershipManager.RequestOwnership();
        }

        private void OnStopMoving(Hero _)
        {
            ownershipManager.RelinquishOwnership();
        }

        public override void SetAvailability(bool isActive)
        {
            startMovingInteractable.Available.Value = !isActive;
            stopMovingInteractable.Available.Value = isActive;
        }
    }
}