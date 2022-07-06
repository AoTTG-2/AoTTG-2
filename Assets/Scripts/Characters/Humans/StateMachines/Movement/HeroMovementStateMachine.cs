using Assets.Scripts.Characters.Humans.Airborne.Moving;
using Assets.Scripts.Characters.Humans.Data.States;
using Assets.Scripts.Characters.Humans.StateMachines.Airborne;
using Assets.Scripts.Characters.Humans.StateMachines.Movement;
using Assets.Scripts.Characters.Humans.StateMachines.Movement.States.Grounded;
using Assets.Scripts.Characters.Humans.StateMachines.Movement.States.Grounded.Moving;
using UnityEngine;

namespace Assets.Scripts.Characters.Humans.StateMachines
{
    /// <summary>
    /// Movement State Machine.
    /// This class holds the data for all possible states the player can be which relate to movement.
    /// </summary>
    public class HeroMovementStateMachine : StateMachine.StateMachine
    {
        public Hero Hero { get; }
        public HeroStateReusableData ReusableData { get; }
        public HeroIdlingState IdlingState { get; }
        public HeroDodgingState DodgingState { get; }
        public HeroRunningState RunningState { get; }
        public HeroGroundedState GroundedState { get; }
        public HeroAirborneState AirborneState { get; }
        public HeroAirborneDashingState DashingState { get; }
        public HeroAirborneMovingState AirborneMovingState { get; }
        public HeroHookedState HookedState { get; }
        public HeroMovementStateMachine(Hero hero)
        {
            Hero = hero;
            ReusableData = new HeroStateReusableData();
            IdlingState = new HeroIdlingState(this);
            DodgingState = new HeroDodgingState(this);
            RunningState = new HeroRunningState(this);
            GroundedState = new HeroGroundedState(this);
            AirborneState = new HeroAirborneState(this);
            DashingState = new HeroAirborneDashingState(this);
            AirborneMovingState = new HeroAirborneMovingState(this);
            HookedState = new HeroHookedState(this);
        }
    }
}