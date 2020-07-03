using StateManager;
using Zenject;

namespace Cannon
{
    internal sealed class CannonStateManager : StateManager<CannonState>, ITickable
    {
        void ITickable.Tick()
        {
            CurrentState.Update();
        }

        protected override void Transition(CannonState newState)
        {
            CurrentState?.Exit();
            base.Transition(newState);
            CurrentState?.Enter();
        }
    }
}