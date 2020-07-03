using Zenject;

namespace Cannon
{
    internal sealed class UnmannedCannonState : CannonState, IInitializable
    {
        public UnmannedCannonState(CannonStateManager stateManager)
            : base(stateManager)
        {
        }

        public override void Enter()
        {
        }

        public override void Exit()
        {
        }

        void IInitializable.Initialize()
        {
            // NonLazy didn't work, so I'm using this.
        }

        public override void Update()
        {
        }
    }
}