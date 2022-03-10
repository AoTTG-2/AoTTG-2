using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.StateMachine
{
    public abstract class StateMachine
    {
        protected IState currentState;

        public void ChangeState(IState newState)
        {
            currentState?.Exit();
            currentState = newState;
            newState.Enter();
        }

        public void HandleInput()
        {
            currentState?.HandleInput();
        }
        public void Update()
        {
            currentState?.Update();
        }
        public void PhysicsUpdate()
        {
            currentState?.PhysicsUpdate();
        }

        public void OnAnimationEnterEvent()
        {
            currentState?.OnAnimatonEnterEvent();
        }
        public void OnAnimationExitEvent()
        {
            currentState?.OnAnimationExitEvent();
        }
        public void OnAnimationTransitionEvent()
        {
            currentState?.OnAnimationTransitionEvent();
        }
    }
}