using Assets.Scripts.Characters.Humans;
using System;

namespace Assets.Scripts.Events.Args
{
    public class HeroKillEvent : EventArgs
    {
        public HeroKillEvent(Hero victim, Hero killer)
        {
            Victim = victim;
            Killer = killer;
        }

        /// <summary>
        /// Gets the killed <see cref="Hero"/>.
        /// </summary>
        public Hero Victim { get; }

        /// <summary>
        /// Gets the attacking <see cref="Hero"/>.
        /// </summary>
        public Hero Killer { get; }
    }
}