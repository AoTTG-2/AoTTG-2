using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Assets.Scripts.Characters.Humans.Data.States.Grounded
{
    [Serializable]
    public class HeroGroundedData
    {
        //10f stolen from Hero.cs
        [field: SerializeField] public float BaseSpeed { get; private set; } = 10f;
        [field: SerializeField] public HeroDashData DashData { get; private set; }
    }
}