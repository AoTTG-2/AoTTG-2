using Assets.Scripts.Characters.Humans.Data.States;
using Assets.Scripts.Characters.Humans.StateMachines.Movement.States.Grounded;
using Assets.Scripts.Characters.Humans.StateMachines.Movement.States.Grounded.Moving;

namespace Assets.Scripts.Characters.Humans.StateMachines
{
    /// <summary>
    /// Movement State Machine.
    /// This class holds the data for all possible states the player can be which relate to movement.
    /// </summary>
    public class HeroMovementStateMachine : StateMachine.StateMachine
    {
        //TO DO: Add Airborne, Dashing state
        public Hero Hero { get; }
        public HeroStateReusableData ReusableData { get; }
        public HeroIdlingState IdlingState { get; }
        public HeroDodgingState DodgingState { get; }
        public HeroRunningState RunningState { get; }
        public HeroMovementStateMachine(Hero hero)
        {
            Hero = hero;
            ReusableData = new HeroStateReusableData();
            IdlingState = new HeroIdlingState(this);
            DodgingState = new HeroDodgingState(this);
            RunningState = new HeroRunningState(this);
        }
    }
}