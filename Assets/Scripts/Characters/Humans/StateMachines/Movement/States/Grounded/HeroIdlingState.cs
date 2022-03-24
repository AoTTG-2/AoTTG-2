using Assets.Scripts.Characters.Humans.Constants;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Characters.Humans.StateMachines.Movement.States.Grounded
{
    public class HeroIdlingState : HeroGroundedState
    {
        public HeroIdlingState(HeroMovementStateMachine heroMovementStateMachine) : base(heroMovementStateMachine)
        {
        }
        #region IState Methods
        public override void Enter()
        {
            base.Enter();
            ResetVelocity();
            UpdateAnimation();
            stateMachine.ReusableData.MovementSpeedModifier = 0f;
        }

        public override void Update()
        {
            base.Update();

            if (stateMachine.ReusableData.MovementInput == Vector2.zero) return;
            OnMove();
        }
        #endregion
        #region Main Methods
        private void UpdateAnimation()
        {
            stateMachine.Hero.IdleAnimation();
        }
        #endregion
    }
}
