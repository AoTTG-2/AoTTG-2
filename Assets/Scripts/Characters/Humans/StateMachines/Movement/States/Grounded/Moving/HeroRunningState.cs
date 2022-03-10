using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Characters.Humans.StateMachines.Movement.States.Grounded.Moving
{
    public class HeroRunningState : HeroMovingState
    {
        public HeroRunningState(HeroMovementStateMachine heroMovementStateMachine) : base(heroMovementStateMachine)
        {
        }
        #region IState Methods
        public override void Enter()
        {
            base.Enter();
            stateMachine.ReusableData.MovementSpeedModifier = movementData.BaseSpeed;
        }
        #endregion
    }
}
