using Assets.Scripts.Characters;
using Assets.Scripts.Events;
using System.Collections.Generic;

namespace Assets.Scripts.Services.Interface
{
    public interface IEntityService : IService
    {
        /// <summary>
        /// Invoked whenever a new entity is registered. This occurs after the entity has been initialized
        /// </summary>
        event OnRegister<Entity> OnRegister; 
        /// <summary>
        /// Invoked whenever the entity has unregisterd itself. This either happens when the entity dies or when the gameobject of the entity is destroyed.
        /// </summary>
        event OnUnRegister<Entity> OnUnRegister;

        void Register(Entity entity);
        void UnRegister(Entity entity);
        /// <summary>
        /// Returns the count of all registered entities
        /// </summary>
        /// <returns></returns>
        int Count();
        /// <summary>
        /// Returns the total count of all registered entities of <typeparamref name="T"/>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        int Count<T>() where T : Entity;
        /// <summary>
        /// Returns all entities
        /// </summary>
        /// <returns></returns>
        HashSet<Entity> GetAll();
        /// <summary>
        /// Returns all entities of type <typeparamref name="T"/>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        HashSet<T> GetAll<T>() where T : Entity;
        /// <summary>
        /// Returns all entities except <paramref name="entity"/>. Generally used when an entity needs to retrieve data of all other entities except itself
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        HashSet<Entity> GetAllExcept(Entity entity);
    }
}
