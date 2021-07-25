namespace Assets.Scripts.Characters.Titan
{
    //TODO: These should be defined as ScriptableObjects instead of being a hardcoded enum
    public enum MindlessTitanType
    {
        /// <summary>
        /// The normal titan. It has a basic set of attacks and slowly walks
        /// </summary>
        Normal,
        /// <summary>
        /// A more difficult version of the <see cref="Normal"/>. It has extra attacks, and will always run
        /// </summary>
        Abberant,
        /// <summary>
        /// Same as an <see cref="Abberant"/>, although it has the jump attack.
        /// </summary>
        Jumper,
        /// <summary>
        /// An advanced titan which can punch and throw rocks
        /// </summary>
        Punk,
        /// <summary>
        /// A crawler is a titan that crawlers on the ground. Its only attack is jumping, and asides from that it will always chase you. Touching its mouth will immediately kill you
        /// </summary>
        Crawler,
        /// <summary>
        /// A titan that follows its target for an extended duration
        /// </summary>
        Stalker,
        /// <summary>
        /// A titan which attacks by unleashing steam attacks. This is not implemented yet
        /// </summary>
        Burster,
        /// <summary>
        /// A titan which has randomized behavior and attacks. An abnormal, you don't know what you will encounter with these
        /// </summary>
        Abnormal
    }
}
