using Assets.Scripts.Characters;
using Assets.Scripts.Events;
using System.Collections.Generic;

namespace Assets.Scripts.Services.Interface
{
    public interface IFactionService : IService
    {
        event OnFactionDefeated OnFactionDefeated;

        void Add(Faction faction);
        void Remove(Faction faction);
        Faction GetHumanity();
        Faction GetTitanity();
        List<Faction> GetAll();

        bool IsHostile(Entity self, Entity target);
        bool IsFriendly(Entity self, Entity target);
        int CountHostile(Entity entity);
        int CountFriendly(Entity entity);
        HashSet<Entity> GetAllHostile(Entity entity);
    }
}
