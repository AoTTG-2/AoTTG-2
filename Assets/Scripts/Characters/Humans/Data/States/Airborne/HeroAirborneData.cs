using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Characters.Humans.Data
{
    [Serializable]
    public class HeroAirborneData
    {
        [field: SerializeField] public float SpeedModifier { get; private set; } = 40f;
        [field: SerializeField] public float DashCooldown { get; private set; } = 1f;
        [field: SerializeField] public float LastPerformedDash { get; set; } = 0f;
    }
}
