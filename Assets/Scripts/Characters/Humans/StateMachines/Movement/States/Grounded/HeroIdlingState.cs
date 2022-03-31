using Assets.Scripts.Characters.Humans.Constants;
using UnityEngine;

namespace Assets.Scripts.Characters.Humans.StateMachines.Movement.States.Grounded
{
    /// <summary>
    /// Idling State Class.
    /// Defines everything that is needed and can be done while idling.
    /// </summary>
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
            UpdateAnimation(HeroAnim.STAND);
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
        #endregion
    }
}
