using Assets.Scripts.Characters;

namespace Assets.Scripts.Services.Events
{
    public delegate void OnSpawn<T>(T entity) where T : Entity;
}
