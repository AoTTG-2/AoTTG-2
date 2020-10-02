using Assets.Scripts.Characters;
using Assets.Scripts.Services.Events;
using System.Collections.Generic;

namespace Assets.Scripts.Services.Interface
{
    public interface IEntityService : IService
    {
        /// <summary>
        /// Event is executed whenever a new entity is registered. This occurs after the entity has been initialized
        /// </summary>
        event OnRegister<Entity> OnRegister; 
        event OnUnRegister<Entity> OnUnRegister;

        void Register(Entity entity);
        void UnRegister(Entity entity);
        int Count();
        int Count<T>() where T : Entity;
        HashSet<Entity> GetAll();
        HashSet<T> GetAll<T>() where T : Entity;
        HashSet<Entity> GetAllExcept(Entity entity);
    }
}
