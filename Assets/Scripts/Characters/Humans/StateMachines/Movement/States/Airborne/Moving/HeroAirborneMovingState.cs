using Assets.Scripts.Characters.Humans.StateMachines.Airborne;
using Assets.Scripts.Characters.Humans.StateMachines;

namespace Assets.Scripts.Characters.Humans.Airborne.Moving
{
    public class HeroAirborneMovingState : HeroAirborneState
    {
        public HeroAirborneMovingState(HeroMovementStateMachine heroMovementStateMachine) : base(heroMovementStateMachine)
        {
        }
    }
}
