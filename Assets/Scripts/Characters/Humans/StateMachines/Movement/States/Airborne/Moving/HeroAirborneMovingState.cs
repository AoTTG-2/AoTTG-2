using Assets.Scripts.Characters.Humans.Constants;
using Assets.Scripts.Characters.Humans.StateMachines;
using Assets.Scripts.Characters.Humans.StateMachines.Airborne;
using UnityEngine;

namespace Assets.Scripts.Characters.Humans.Airborne.Moving
{
    public class HeroAirborneMovingState : HeroAirborneState
    {
        public HeroAirborneMovingState(HeroMovementStateMachine heroMovementStateMachine) : base(heroMovementStateMachine)
        {
        }
        #region IState Methods
        public override void Enter()
        {
            base.Enter();
            if (stateMachine.PreviousState == stateMachine.HookedState
                && GetPlayerVerticalVelocity() > 20f
                && stateMachine.ReusableData.CurrentAnimation != HeroAnim.AIR_RELEASE)
            {
                UpdateAnimation(HeroAnim.AIR_RELEASE);
            }
        }
        public override void Update()
        {
            base.Update();
            Move();
        }
        #endregion
        #region Main Methods
        private void Move()
        {
            if (GetPlayerHorizontalVelocity().x + GetPlayerHorizontalVelocity().z < 25f)
            {
                if (Falling) stateMachine.ChangeState(stateMachine.AirborneState);
                else if(stateMachine.ReusableData.CurrentAnimation != HeroAnim.AIR_RELEASE)
                {
                    UpdateAnimation(HeroAnim.AIR_RISE);
                }
            }
            else
            {
                PlayAnimationForDirection();
            }
        }
        #endregion
    }
}
