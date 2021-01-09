using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Events.Args
{
    public class HeroHitEvent : EventArgs
    {
        public HeroHitEvent(Hero victim, Hero hitter)
        {
            Victim = victim;
            Hitter = hitter;
        }

        /// <summary>
        /// Gets the killed <see cref="Hero"/>.
        /// </summary>
        public Hero Victim { get; }

        /// <summary>
        /// Gets the attacking <see cref="Hero"/>.
        /// </summary>
        public Hero Hitter { get; }
    }
}
