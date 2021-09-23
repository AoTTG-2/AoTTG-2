using Assets.Scripts.Characters.Titan;

namespace Assets.Scripts.Characters.Humans.Skills
{
    public enum HeroSkill
    {
        /// <summary>
        /// Titan will play the laugh animation and thus temporarily disabling them
        /// </summary>
        Armin,
        /// <summary>
        /// Makes all nearby titans target the player
        /// </summary>
        Marco,
        /// <summary>
        /// Used to free themselves from the hand of a titan when grabbed
        /// </summary>
        Jean,
        /// <summary>
        /// Executes an attack with 3 horizontal spins, similar to Levis iconic spin attack
        /// </summary>
        Levi,
        /// <summary>
        /// Executes an attack with 3 vertical spins
        /// </summary>
        Petra,
        /// <summary>
        /// Drops all momentum and will do a vertical attack affected by gravity
        /// </summary>
        Mikasa,
        /// <summary>
        /// Transforms the player into <see cref="ErenTitan"/>
        /// </summary>
        Eren,
        Annie,
        Reiner,
        Bertholdt,
        /// <summary>
        /// Fires a bomb, only used in the Bomb Pvp mode
        /// </summary>
        BombPvp,
        Sasha,
    }
}
