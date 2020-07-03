using StateManager;

namespace Cannon
{
    internal class CannonState : State<CannonState>
    {
        public CannonState(StateManager<CannonState> stateManager)
            : base(stateManager)
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