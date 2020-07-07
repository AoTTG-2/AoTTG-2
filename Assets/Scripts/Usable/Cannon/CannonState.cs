using StateManager;

namespace Cannon
{
    internal class CannonState : State<CannonState>
    {
        public CannonState(CannonStateManager stateManager)
            : base(stateManager)
        {
        }

        public virtual void SetAvailability(bool isActive)
        {
        }

        public virtual void Enter()
        {
        }

        public virtual void Exit()
        {
        }

        public virtual void Update()
        {
        }
    }
}