using Assets.Scripts.Characters;
using Assets.Scripts.Characters.Titan;
using Assets.Scripts.Room;
using Assets.Scripts.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Scripts.Services
{
    public class SpawnService : MonoBehaviour, ISpawnService
    {
        private readonly List<Spawner> spawners = new List<Spawner>();

        public void Add(Spawner spawner)
        {
            spawners.Add(spawner);
        }

        public void Remove(Spawner spawner)
        {
            spawners.Remove(spawner);
        }

        public List<T> GetAll<T>() where T : Spawner
        {
            return spawners.OfType<T>().ToList();
        }

        public T GetRandom<T>() where T : Spawner
        {
            var typedSpawners = GetAll<T>();
            return typedSpawners[Random.Range(0, typedSpawners.Count)];
        }
        
        public List<HumanSpawner> GetByType(PlayerSpawnType type)
        {
            return GetAll<HumanSpawner>().Where(x => x.Type == type).ToList();
        }

        public List<TitanSpawner> GetByType(TitanSpawnerType type)
        {
            return GetAll<TitanSpawner>().Where(x => x.Type == type).ToList();
        }

        public T Spawn<T>() where T : Entity
        {
            var type = typeof(T);
            if (type == typeof(MindlessTitan))
            {
                return (T) MindlessTitan.Spawn();
            }

            if (type == typeof(ErenTitan))
            {
                return (T) ErenTitan.Spawn();
            }

            if (type == typeof(MindlessTitan))
            {
                return (T) MindlessTitan.Spawn();
            }

            if (type == typeof(MindlessTitan))
            {
                return (T) MindlessTitan.Spawn();
            }

            if (type == typeof(MindlessTitan))
            {
                return (T) MindlessTitan.Spawn();
            }

            throw new NotImplementedException();
        }

        public T Spawn<T>(Vector3 position) where T : Entity
        {
            throw new System.NotImplementedException();
        }

        public void OnRestart()
        {
        }

        private void LateUpdate()
        {

        }
    }
}
