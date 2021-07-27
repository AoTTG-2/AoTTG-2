using Assets.Scripts.Characters;
using Assets.Scripts.Events;
using System.Collections.Generic;

namespace Assets.Scripts.Services.Interface
{
    public interface IFactionService : IService
    {
        /// <summary>
        /// Invoked when all members of a faction are dead
        /// </summary>
        event OnFactionDefeated OnFactionDefeated;

        /// <summary>
        /// Adds a new faction
        /// </summary>
        /// <param name="faction"></param>
        void Add(Faction faction);
        /// <summary>
        /// Removes a faction
        /// </summary>
        /// <param name="faction"></param>
        void Remove(Faction faction);
        /// <summary>
        /// Returns the "Humanity" faction
        /// </summary>
        /// <returns></returns>
        Faction GetHumanity();
        /// <summary>
        /// Returns the "Titanity" faction
        /// </summary>
        /// <returns></returns>
        Faction GetTitanity();
        /// <summary>
        /// Returns all factions
        /// </summary>
        /// <returns></returns>
        List<Faction> GetAll();

        /// <summary>
        /// Determines if the target entity is a member of a hostile faction
        /// </summary>
        /// <param name="self"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        bool IsHostile(Entity self, Entity target);
        /// <summary>
        /// Determines if the entity is a member of a friendly faction
        /// </summary>
        /// <param name="self"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        bool IsFriendly(Entity self, Entity target);
        /// <summary>
        /// Returns the total amount of hostile entities
        /// </summary>
        /// <param name="entity">The entity for which the hostile entities should be counted</param>
        /// <returns></returns>
        int CountHostile(Entity entity);
        /// <summary>
        /// Returns the total amount of friendly entities
        /// </summary>
        /// <param name="entity">The entity for which the friendly entities should be counted</param>
        /// <returns></returns>
        int CountFriendly(Entity entity);
        /// <summary>
        /// Returns all hostile entities
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        HashSet<Entity> GetAllHostile(Entity entity);
    }
}
