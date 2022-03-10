using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Assets.Scripts.Characters.Humans.Data.States.Grounded
{
    [Serializable]
    public class HeroDashData
    {
        [field: SerializeField] public float SpeedModifier { get; private set; } = 40f;
        [field: SerializeField] public float DashCooldown { get; private set; } = 0.5f;
    }
}
