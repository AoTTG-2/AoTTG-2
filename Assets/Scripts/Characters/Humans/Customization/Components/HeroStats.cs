using System;
using UnityEngine;

namespace Assets.Scripts.Characters.Humans.Customization.Components
{
    [Serializable]
    public struct HeroStats
    {
        public const int PointLimit = 400;
        
        /// <summary>
        /// Acceleration
        /// </summary>
        [SerializeField] public int Acceleration;
        /// <summary>
        /// Ground speed
        /// </summary>
        [SerializeField] public int Speed;        
        /// <summary>
        /// Gas resources
        /// </summary>
        [SerializeField] public int Gas;        
        /// <summary>
        /// Equipment modifier.
        /// </summary>
        [SerializeField] public int Equipment;

        public bool IsValid()
        {
            if (Acceleration + Speed + Gas + Equipment > PointLimit) return false;

            if (Acceleration > 125 || Acceleration < 75) return false;
            if (Speed > 125 || Speed < 75) return false;
            if (Gas > 125 || Gas < 75) return false;
            if (Equipment > 125 || Equipment < 75) return false;

            return true;
        }
    }
}
