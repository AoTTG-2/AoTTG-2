using Assets.Scripts.Services.Interface;

namespace Assets.Scripts.Legacy.CustomMap
{
    /// <summary>
    /// Used by the <see cref="ICustomMapService"/> to determine the type of a RC Custom Map object
    /// </summary>
    public enum RcObjectType
    {
        Custom,
        Base,
        BaseSpecial,
        Misc,
        Racing,
        Spawnpoint,
        Spawn
    }
}
