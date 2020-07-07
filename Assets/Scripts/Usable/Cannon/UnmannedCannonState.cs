using System;
using Zenject;

namespace Cannon
{
    internal sealed class UnmannedCannonState : CannonState, IInitializable, IDisposable
    {
        private readonly ICannonOwnershipManager ownershipManager;

        public UnmannedCannonState(
            CannonStateManager stateManager,
            ICannonOwnershipManager ownershipManager)
            : base(stateManager)
        {
            this.ownershipManager = ownershipManager;
        }

        void IDisposable.Dispose()
        {
            ownershipManager.WorldOwnershipTaken -= OnWorldOwnershipTaken;
        }

        void IInitializable.Initialize()
        {
            ownershipManager.WorldOwnershipTaken += OnWorldOwnershipTaken;
        }

        public override void Enter()
        {
            ownershipManager.AllowOwnershipTransfer = true;
        }

        public override void Exit()
        {
            ownershipManager.AllowOwnershipTransfer = false;
        }

        public override void Update()
        {
        }

        private void OnWorldOwnershipTaken()
        {
            StateManager.Transition<UnmannedCannonState>();
        }
    }
}