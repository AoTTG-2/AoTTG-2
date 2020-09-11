using Assets.Scripts.Characters;
using Assets.Scripts.Services.Interface;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Services
{
    public class FactionService : IFactionService
    {
        private IPlayerService PlayerService => Service.Player;
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
            return factions.Where(x => x.Allies.Any(ally => ally != faction)).ToList();
        }

        private HashSet<Entity> GetAllHostile(Entity entity)
        {
            if (entity.Faction == null)
            {
                return EntityService.GetAllExcept(entity);
            }

            var hostileFactions = GetHostileFactions(entity.Faction);
            var hostileEntities = EntityService
                .GetAllExcept(entity).ToList().Where(x => hostileFactions.Any(faction => faction == x.Faction));
            return new HashSet<Entity>(hostileEntities);
        }

        private List<Entity> GetAllFriendly()
        {
            return new List<Entity>();
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
            return true;
        }

        public int CountHostile(Entity entity)
        {
            return GetAllHostile(entity).Count;
        }

        public int CountFriendly()
        {
            return GetAllFriendly().Count;
        }
    }
}
