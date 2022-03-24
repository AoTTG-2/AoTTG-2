using Assets.Scripts.Characters.Humans;
using Assets.Scripts.Characters.Humans.Data.States;
using Assets.Scripts.Characters.Humans.StateMachines.Movement.States.Grounded;
using Assets.Scripts.Characters.Humans.StateMachines.Movement.States.Grounded.Moving;
using Assets.Scripts.StateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroMovementStateMachine : StateMachine
{
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
