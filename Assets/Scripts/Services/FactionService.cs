using Assets.Scripts.Characters;
using Assets.Scripts.Events;
using Assets.Scripts.Services.Interface;
using Photon;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Services
{
    public class FactionService : PunBehaviour, IFactionService
    {
        public event OnFactionDefeated OnFactionDefeated;

        private IEntityService EntityService => Service.Entity;

        private static readonly Faction Humanity = new Faction
        {
            Name = "Humanity",
            Prefix = "H",
            Color = new Color(0.5f, 0.5f, 0.5f)
        };

        private static readonly Faction Titanity = new Faction
        {
            Name = "Titanity",
            Prefix = "T",
            Color = new Color(1f, 1f, 1f)
        };

        private readonly List<Faction> factions = new List<Faction> { Humanity, Titanity };

        private void Awake()
        {
            EntityService.OnUnRegister += EntityService_OnUnRegister;
        }

        private void EntityService_OnUnRegister(Entity entity)
        {
            if (!PhotonNetwork.inRoom) return;
            if (GetAllMembers(entity).Count == 0)
                OnFactionDefeated?.Invoke(entity.Faction);
        }

        public void Add(Faction faction)
        {
            factions.Add(faction);
        }

        public List<Faction> GetAll()
        {
            return factions;
        }

        public Faction GetHumanity()
        {
            return Humanity;
        }

        public Faction GetTitanity()
        {
            return Titanity;
        }

        public void Remove(Faction faction)
        {
            factions.Remove(faction);
        }

        private List<Faction> GetHostileFactions(Faction faction)
        {
            List<Faction> hostileFactions = new List<Faction>();

            foreach (Faction index in factions)
            {
                if (index != faction && !faction.Allies.ToList().Contains(index))
                {
                    hostileFactions.Add(index);
                }
            }
            return hostileFactions;
        }

        /// <summary>
        /// Returns all allied factions excluding itself.
        /// </summary>
        /// <param name="faction"></param>
        /// <returns></returns>
        private List<Faction> GetFriendlyFactions(Faction faction)
        {
            List<Faction> friendlyFactions = new List<Faction>();

            foreach (Faction index in factions)
            {
                if (index != faction && faction.Allies.ToList().Contains(index))
                {
                    friendlyFactions.Add(index);
                }
            }
            return friendlyFactions;
        }

        /// <summary>
        /// Gets all entities in the same faction as the parameter entity.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        private HashSet<Entity> GetAllMembers(Entity entity)
        {
            if (entity?.Faction == null)
            {
                return EntityService.GetAllExcept(entity);
            }

            var factionMembers = EntityService.GetAllExcept(entity).Where(x => x.Faction == entity.Faction).ToList();
            return new HashSet<Entity>(factionMembers);
        }

        public HashSet<Entity> GetAllHostile(Entity entity)
        {
            if (entity?.Faction == null)
            {
                return EntityService.GetAllExcept(entity);
            }

            var hostileFactions = GetHostileFactions(entity.Faction);
            var hostileEntities = EntityService
                .GetAllExcept(entity).Where(x => hostileFactions.Any(faction => faction == x.Faction) || x.Faction == null).ToList();
            return new HashSet<Entity>(hostileEntities);
        }

        /// <summary>
        /// Gets all entities in the same faction (as entity) and in allied factions.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        private HashSet<Entity> GetAllFriendly(Entity entity)
        {
            if (entity?.Faction == null)
            {
                return new HashSet<Entity>();
            }

            var friendlyFactions = GetFriendlyFactions(entity.Faction);
            friendlyFactions.Add(entity.Faction);
            var friendlyEntities = EntityService
                .GetAllExcept(entity).Where(x => friendlyFactions.Any(faction => faction == x.Faction) || x.Faction == null).ToList();
            return new HashSet<Entity>(friendlyEntities);
        }

        public void OnRestart()
        {
        }

        public bool IsHostile(Entity self, Entity target)
        {
            return GetHostileFactions(self.Faction).Contains(target.Faction);
        }

        public bool IsFriendly(Entity self, Entity target)
        {
            return !IsHostile(self, target);
        }

        public int CountHostile(Entity entity)
        {
            return GetAllHostile(entity).Count;
        }

        public int CountFriendly(Entity entity)
        {
            return GetAllFriendly(entity).Count;
        }
        public int CountMembers(Entity entity)
        {
            //+1 is added because the entity that calls GetAllMembers is excluded from the count.
            return GetAllMembers(entity).Count + 1;
        }
        public int CountMembers(Faction faction)
        {
            if (faction == null) return 1;
            return EntityService.GetAll().Count(x => x.Faction == faction);
        }
    }
}
