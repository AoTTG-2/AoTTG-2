using Assets.Scripts.Characters.Humans.Data;
using Assets.Scripts.Characters.Humans.Data.States.Grounded;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Hero", menuName = "Custom/Character/Hero")]
public class HeroSO : ScriptableObject
{
    [field: SerializeField] public HeroGroundedData GroundedData { get; private set; }
    [field: SerializeField] public HeroAirborneData DashData { get; private set; }
}
