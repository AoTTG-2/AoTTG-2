using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.StateMachine
{
    public abstract class StateMachine
    {
        public IState CurrentState { get; protected set; }
        public IState PreviousState { get; protected set; }

        public void ChangeState(IState newState)
        {
            CurrentState?.Exit();
            PreviousState = CurrentState;
            CurrentState = newState;
            newState.Enter();
        }

        public void HandleInput()
        {
            CurrentState?.HandleInput();
        }
        public void Update()
        {
            CurrentState?.Update();
        }
        public void PhysicsUpdate()
        {
            CurrentState?.PhysicsUpdate();
        }

        public void OnAnimationEnterEvent()
        {
            CurrentState?.OnAnimatonEnterEvent();
        }
        public void OnAnimationExitEvent()
        {
            CurrentState?.OnAnimationExitEvent();
        }
        public void OnAnimationTransitionEvent()
        {
            CurrentState?.OnAnimationTransitionEvent();
        }
    }
}