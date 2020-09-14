using Assets.Scripts.Room;
using Assets.Scripts.Services.Interface;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Services
{
    public class RespawnService : IRespawnService
    {
        private readonly List<PlayerSpawner> playerSpawners = new List<PlayerSpawner>();

        public void Add(PlayerSpawner playerSpawner)
        {
            playerSpawners.Add(playerSpawner);
        }

        public void Remove(PlayerSpawner playerSpawner)
        {
            playerSpawners.Remove(playerSpawner);
        }

        public List<PlayerSpawner> GetAll()
        {
            return playerSpawners;
        }

        public List<PlayerSpawner> GetByType(PlayerSpawnType type)
        {
            return playerSpawners.Where(x => x.Type == type).ToList();
        }

        public PlayerSpawner GetRandom()
        {
            return playerSpawners[Random.Range(0, playerSpawners.Count)];
        }
    }
}
