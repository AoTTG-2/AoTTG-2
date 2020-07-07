using System;
using System.Collections.Generic;
using Zenject;

namespace Cannon
{
    internal sealed class RemoteControlledCannonState : CannonState, IInitializable, IDisposable
    {
        [Inject]
        private readonly List<CannonState> cannonStates;

        [Inject]
        private readonly List<Interactable> interactables;

        private readonly ICannonOwnershipManager ownershipManager;

        public RemoteControlledCannonState(
            CannonStateManager stateManager,
            ICannonOwnershipManager ownershipManager)
            : base(stateManager)
        {
            this.ownershipManager = ownershipManager;
        }

        void IDisposable.Dispose()
        {
            ownershipManager.RemoteOwnershipTaken -= OnRemoteOwnershipTaken;
        }

        void IInitializable.Initialize()
        {
            ownershipManager.RemoteOwnershipTaken += OnRemoteOwnershipTaken;
        }

        public override void Enter()
        {
            SetAllInteractablesUnavailable();
        }

        private void SetAllInteractablesUnavailable()
        {
            foreach (var interactable in interactables)
                interactable.Available.Value = false;
        }

        public override void Exit()
        {
            SetCannonsDefaultAvailability();
        }

        private void SetCannonsDefaultAvailability()
        {
            foreach (var interactable in cannonStates)
                interactable.SetAvailability(false);
        }

        private void OnRemoteOwnershipTaken()
        {
            StateManager.Transition<RemoteControlledCannonState>();
        }
    }
}