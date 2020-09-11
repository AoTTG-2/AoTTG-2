using System.Collections.Generic;
using Assets.Scripts.Characters;

namespace Assets.Scripts.Services.Interface
{
    public interface IEntityService : IService
    {
        void Register(Entity entity);
        void UnRegister(Entity entity);
        int Count();
        int Count<T>() where T : Entity;
        HashSet<Entity> GetAll();
        HashSet<Entity> GetAllExcept(Entity entity);
    }
}
