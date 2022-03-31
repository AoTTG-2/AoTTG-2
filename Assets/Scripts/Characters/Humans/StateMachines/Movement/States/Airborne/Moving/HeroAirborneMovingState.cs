using Assets.Scripts.Characters.Humans.StateMachines.Airborne;
using Assets.Scripts.Characters.Humans.StateMachines;

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

        }
        #endregion
    }
}
