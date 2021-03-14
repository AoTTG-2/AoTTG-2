using Assets.Scripts.Characters.Humans;
using Assets.Scripts.Characters.Titan;
using System;

namespace Assets.Scripts.Events.Args
{
    public class TitanHitEvent : EventArgs
    {
        public TitanHitEvent(TitanBase titanBase, BodyPart bodyPart, Hero hero, bool rightHand)
        {
            Titan = titanBase;
            PartHit = bodyPart;
            Hero = hero;
            RightHand = rightHand;
        }

        /// <summary>
        /// Gets the attacked <see cref="TitanBase">Titan</see>.
        /// </summary>
        public TitanBase Titan { get; }

        /// <summary>
        /// Gets the <see cref="TitanBase">Titan's</see> <see cref="BodyPart"/> that was hit.
        /// </summary>
        public BodyPart PartHit { get; }

        /// <summary>
        /// Gets the attacking <see cref="Hero"/>.
        /// </summary>
        public Hero Hero { get; }

        /// <summary>
        /// Gets whether the hero is attacking using his right hand.
        /// </summary>
        public bool RightHand { get; }
    }
}
