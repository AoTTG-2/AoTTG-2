using System;
using System.Collections.Generic;

namespace StateManager
{
    public abstract class StateManager<T> where T : State<T>
    {
        private readonly Dictionary<Type, T> stateByType;

        protected StateManager()
        {
            stateByType = new Dictionary<Type, T>();
        }

        protected T CurrentState { get; private set; }

        public void Transition<TState>() where TState : State<T>
        {
            var newState = stateByType[typeof(TState)];
            Transition(newState);
        }

        internal void Register(T state)
        {
            stateByType.Add(state.GetType(), state);
        }

        protected virtual void Transition(T newState)
        {
            CurrentState = newState;
        }
    }
}