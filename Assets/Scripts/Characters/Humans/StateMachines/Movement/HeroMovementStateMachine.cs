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
    public HeroDashinState DashingState { get; }
    public HeroRunningState RunningState { get; }
    /*public PlayerSprintingState SprintingState { get; }
    public PlayerDashingState DashingState { get; }*/

    public HeroMovementStateMachine(Hero hero)
    {
        Hero = hero;
        ReusableData = new HeroStateReusableData();
        IdlingState = new HeroIdlingState(this);
        DashingState = new HeroDashinState(this);
        RunningState = new HeroRunningState(this);
        /*SprintingState = new PlayerSprintingState(this);
        DashingState = new PlayerDashingState(this);*/
    }
}
