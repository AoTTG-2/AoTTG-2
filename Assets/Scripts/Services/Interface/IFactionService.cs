using Assets.Scripts.Characters;
using System.Collections.Generic;

namespace Assets.Scripts.Services.Interface
{
    public interface IFactionService : IService
    {
        void Add(Faction faction);
        void Remove(Faction faction);
        Faction GetHumanity();
        Faction GetTitanity();
        List<Faction> GetAll();

        bool IsHostile(Entity self, Entity target);
        bool IsFriendly(Entity self, Entity target);
        int CountHostile(Entity entity);
        int CountFriendly();
    }
}
