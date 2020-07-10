using System;
using UnityEngine;
using Zenject;

namespace Cannon
{
    internal sealed class MovingCannonState : CannonState, IInitializable, IDisposable
    {
        private readonly ICannonOwnershipManager ownershipManager;
        private readonly Interactable startMovingInteractable;
        private readonly Interactable stopMovingInteractable;
        private readonly Transform movePoint;
        private Hero mountedHero;

        public MovingCannonState(
            CannonStateManager stateManager,
            ICannonOwnershipManager ownershipManager,
            Interactable startMovingInteractable,
            Interactable stopMovingInteractable,
            Transform movePoint)
            : base(stateManager)
        {
            this.ownershipManager = ownershipManager;
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
        }

        public override void Exit()
        {
            SetAvailability(false);

            mountedHero = null;
        }

        public override void Update()
        {
            if (!mountedHero) return;
            
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