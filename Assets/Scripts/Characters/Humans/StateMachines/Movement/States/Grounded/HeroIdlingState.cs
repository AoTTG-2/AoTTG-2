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

            stateMachine.ReusableData.MovementSpeedModifier = 0f;
            ResetVelocity();
        }

        public override void Update()
        {
            base.Update();

            if (stateMachine.ReusableData.MovementInput == Vector2.zero) return;
            OnMove();
        }
        #endregion
    }
}
