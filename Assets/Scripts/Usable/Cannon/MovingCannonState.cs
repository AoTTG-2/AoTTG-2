using System;
using Zenject;

namespace Cannon
{
    internal sealed class MovingCannonState : CannonState, IInitializable, IDisposable
    {
        private readonly Interactable startMovingInteractable;
        private readonly Interactable stopMovingInteractable;

        public MovingCannonState(
            CannonStateManager stateManager,
            Interactable startMovingInteractable,
            Interactable stopMovingInteractable)
            : base(stateManager)
        {
            this.startMovingInteractable = startMovingInteractable;
            this.stopMovingInteractable = stopMovingInteractable;
        }

        void IDisposable.Dispose()
        {
            startMovingInteractable.Interacted.RemoveListener(OnStartMoving);
            stopMovingInteractable.Interacted.RemoveListener(OnStopMoving);
        }

        public override void Enter()
        {
            SetMovingAvailability();
        }

        public override void Exit()
        {
            SetStoppedAvailability();
        }

        void IInitializable.Initialize()
        {
            startMovingInteractable.Interacted.AddListener(OnStartMoving);
            stopMovingInteractable.Interacted.AddListener(OnStopMoving);

            SetStoppedAvailability();
        }

        public override void Update()
        {
        }

        private void OnStartMoving(Hero _)
        {
            StateManager.Transition<MovingCannonState>();
        }

        private void OnStopMoving(Hero _)
        {
            StateManager.Transition<UnmannedCannonState>();
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