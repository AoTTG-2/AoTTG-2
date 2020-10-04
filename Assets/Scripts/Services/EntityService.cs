using Assets.Scripts.Characters;
using Assets.Scripts.Events;
using Assets.Scripts.Services.Interface;
using System.Collections.Generic;
using System.Linq;

namespace Assets.Scripts.Services
{
    public class EntityService : IEntityService
    {
        private readonly HashSet<Entity> entities = new HashSet<Entity>();

        public event OnRegister<Entity> OnRegister; 
        public event OnUnRegister<Entity> OnUnRegister;

        public void Register(Entity entity)
        {
            if (entities.Add(entity))
            {
                OnRegister?.Invoke(entity);
            }
        }

        public void UnRegister(Entity entity)
        {
            if (entities.Remove(entity))
            {
                OnUnRegister?.Invoke(entity);
            }
        }

        public HashSet<Entity> GetAll()
        {
            return entities;
        }

        public HashSet<T> GetAll<T>() where T : Entity
        {
            return new HashSet<T>(entities.OfType<T>());
        }

        public HashSet<Entity> GetAllExcept(Entity entity)
        {
            var allEntity = entities.ToList();
            allEntity.Remove(entity);
            return new HashSet<Entity>(allEntity);
        }

        public int Count()
        {
            return entities.Count;
        }

        public int Count<T>() where T : Entity
        {
            return entities.Count(x => x.GetType() == typeof(T));
        }

        public void OnRestart()
        {
            entities.Clear();
        }
    }
}
