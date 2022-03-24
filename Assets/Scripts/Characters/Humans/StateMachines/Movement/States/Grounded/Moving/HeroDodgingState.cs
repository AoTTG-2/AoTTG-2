using Assets.Scripts.Characters.Humans.Constants;
using Assets.Scripts.Characters.Humans.Data.States.Grounded;
using UnityEngine;

namespace Assets.Scripts.Characters.Humans.StateMachines.Movement.States.Grounded
{
    // TODO: Dash Distance/Time is way too high, find the exact amount in Hero.cs
    public class HeroDodgingState : HeroGroundedState
    {
        private readonly HeroDashData dashData;
        public HeroDodgingState(HeroMovementStateMachine heroMovementStateMachine) : base(heroMovementStateMachine)
        {
            dashData = movementData.DashData;
        }
        #region IState Methods
        public override void Enter()
        {
            base.Enter();
            Dodge();
        }
        #endregion
        #region Main Methods
        private void Dodge()
        {
            UpdateAnimation();
            //Add Force in Movement Direction

            //Remove Force after 1 second

            //Transition to idle 
        }
        private void UpdateAnimation()
        {
            CrossFade(HeroAnim.DODGE);
        }
        #endregion
    }
}