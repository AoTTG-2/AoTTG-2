using Assets.Scripts.Characters.Humans;
using Assets.Scripts.Characters.Titan;
using System;

namespace Assets.Scripts.Events.Args
{
    public class TitanDamagedEvent : EventArgs
    {
        public TitanDamagedEvent(TitanBase titanBase, Hero hero, int damage = 10)
        {
            Titan = titanBase;
            Hero = hero;
            Damage = damage;
        }

        /// <summary>
        /// Gets the attacked <see cref="TitanBase">Titan</see>.
        /// </summary>
        public TitanBase Titan { get; }

        /// <summary>
        /// Gets the attacking <see cref="Hero"/>.
        /// </summary>
        public Hero Hero { get; }

        /// <summary>
        /// Gets the amount of damage dealt to the <see cref="TitanBase">Titan</see>.
        /// </summary>
        public int Damage { get; }
    }
}
