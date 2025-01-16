using Assets.Scripts.Characters;

namespace Assets.Scripts.Events
{
    public delegate void OnRegister<in T>(T entity) where T : Entity;
    public delegate void OnUnRegister<in T>(T entity) where T : Entity;

    public delegate void OnPlayerSpawn<in T>(T entity) where T : Entity;
    public delegate void OnPlayerDespawn<in T>(T entity) where T : Entity;

    public delegate void OnTitanKilled<in T1, in T2>(T1 killer, T2 victim, int damage = 0) where T1 : Entity where T2 : Entity;
}
