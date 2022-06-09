using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.StateMachine
{
    public interface IState
    {
        public void Enter();
        public void Exit();
        public void HandleInput();
        public void Update();
        public void PhysicsUpdate();
        public void OnAnimatonEnterEvent();
        public void OnAnimationExitEvent();
        public void OnAnimationTransitionEvent();
        public void UpdateAnimation(string newAnimation, float fadeTime = .1f);
    }
}