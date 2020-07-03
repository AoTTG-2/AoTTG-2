using System;
using Zenject;

namespace Cannon
{
    internal sealed class MovingCannonState : ICannonState, IInitializable, IDisposable
    {
        private readonly Interactable startMovingInteractable;
        private readonly CannonStateManager stateManager;
        private readonly Interactable stopMovingInteractable;

        public MovingCannonState(
            CannonStateManager stateManager,
            [Inject(Id = "StartMovingInteractable")]
            Interactable startMovingInteractable,
            [Inject(Id = "StopMovingInteractable")]
            Interactable stopMovingInteractable)
        {
            this.stateManager = stateManager;
            this.startMovingInteractable = startMovingInteractable;
            this.stopMovingInteractable = stopMovingInteractable;
        }

        void IDisposable.Dispose()
        {
            startMovingInteractable.Interacted.RemoveListener(OnStartMoving);
            stopMovingInteractable.Interacted.RemoveListener(OnStopMoving);
        }

        void ICannonState.Enter()
        {
            SetMovingAvailability();
        }

        void ICannonState.Exit()
        {
            SetStoppedAvailability();
        }

        void IInitializable.Initialize()
        {
            startMovingInteractable.Interacted.AddListener(OnStartMoving);
            stopMovingInteractable.Interacted.AddListener(OnStopMoving);

            SetStoppedAvailability();
        }

        void ICannonState.Update()
        {
        }

        private void OnStartMoving(Hero _)
        {
            stateManager.Transition<MovingCannonState>();
        }

        private void OnStopMoving(Hero _)
        {
            stateManager.Transition<UnmannedCannonState>();
        }

        private void SetMovingAvailability()
        {
            startMovingInteractable.Available.Value = false;
            stopMovingInteractable.Available.Value = true;
        }

        private void SetStoppedAvailability()
        {
            startMovingInteractable.Available.Value = true;
            stopMovingInteractable.Available.Value = false;
        }
    }
}