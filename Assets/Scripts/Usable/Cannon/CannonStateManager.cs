using System;
using System.Collections.Generic;
using Zenject;

namespace Cannon
{
    internal sealed class CannonStateManager : ITickable
    {
        private ICannonState currentState;
        private Dictionary<Type, ICannonState> stateByType;

        void ITickable.Tick()
        {
            currentState.Update();
        }

        public void Transition<T>() where T : ICannonState
        {
            ICannonState newState = stateByType[typeof(T)];
            currentState?.Exit();
            (currentState = newState)?.Enter();
        }

        [Inject]
        private void Construct(List<ICannonState> states)
        {
            stateByType = new Dictionary<Type, ICannonState>();
            foreach (var state in states)
                stateByType.Add(state.GetType(), state);
        }
    }
}