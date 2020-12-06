public enum RespawnMode
{
    /// <summary>
    /// Players will never respawn.
    /// </summary>
    Never,

    /// <summary>
    /// Players will continue to respawn forever.
    /// <para>Note: Disables "Loose" screen for players.</para>
    /// </summary>
    Endless,
    
    /// <summary>
    /// Players will respawn whenever the round ends.
    /// </summary>
    NewRound
}

