namespace Assets.Scripts.Characters.Titan
{
    /// <summary>
    /// ENUM for possible stats of a titan
    /// </summary>
    public enum TitanState
    {
        /// <summary>
        /// A titan isn't doing anything, and awaiting a new state
        /// </summary>
        Idle,
        /// <summary>
        /// The titan has been killed, and will despawn shortly
        /// </summary>
        Dead,
        /// <summary>
        /// A titan is wandering around the map
        /// </summary>
        Wandering,
        /// <summary>
        /// A titan is turning around
        /// </summary>
        Turning,
        /// <summary>
        /// A titan is chasing a target
        /// </summary>
        Chase,
        /// <summary>
        /// A titan is attacking its target
        /// </summary>
        Attacking,
        /// <summary>
        /// A titan has run out of stamina, and will go in a recovering state to quickly regenerate its stamina
        /// </summary>
        Recovering,
        /// <summary>
        /// A titan will eat a human
        /// </summary>
        Eat,
        /// <summary>
        /// A titan has its functional limbs disabled, and cannot do anything until its limbs are regenerated
        /// </summary>
        Disabled
    }
}
