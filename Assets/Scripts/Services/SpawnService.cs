using Assets.Scripts.Characters;
using Assets.Scripts.Characters.Titan;
using Assets.Scripts.Characters.Titan.Configuration;
using Assets.Scripts.Gamemode;
using Assets.Scripts.Room;
using Assets.Scripts.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Characters.Humans;
using Assets.Scripts.Characters.Humans.Customization;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Scripts.Services
{
    public class SpawnService : MonoBehaviour, ISpawnService
    {
        private IEntityService EntityService => Service.Entity;

        private readonly List<Spawner> spawners = new List<Spawner>();
        private static GamemodeBase Gamemode => FengGameManagerMKII.Gamemode;
        
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
            if (typedSpawners.Count == 0) return null;
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

        public (Vector3 position, Quaternion rotation) GetRandomSpawnPosition()
        {
            var position = new Vector3(Random.Range(-550, 550f), 0f, Random.Range(-550f, 500f));
            if (position.x >= 0f && position.x <= 50f) position.x = 50f;
            if (position.x <= 0f && position.x >= -50f) position.x = -50f;
            if (position.z >= 0f && position.z <= 50f) position.z = 50f;
            if (position.z <= 0f && position.z >= -50f) position.z = -50f;

            var rotation = new Vector3(0f, Random.Range(0, 360f), 0f);
            return (position, Quaternion.Euler(rotation));
        }

        public void OnRestart()
        {
        }

        public T Spawn<T>() where T : Entity
        {
            var type = typeof(T);
            if (type == typeof(PlayerTitan))
            {
                return Spawn<T>(new TitanConfiguration());
            }

            throw new ArgumentException($"{type} is not implemented");
        }

        public T Spawn<T>(EntityConfiguration configuration) where T : Entity
        {
            var type = typeof(T);
            if (type == typeof(PlayerTitan))
            {
                return SpawnPlayerTitan() as T;
            }

            if (type == typeof(MindlessTitan))
            {
                return SpawnTitan("MindlessTitan", configuration as TitanConfiguration) as T;
            }

            throw new ArgumentException($"{type} is not implemented");
        }

        public T Spawn<T>(Vector3 position, Quaternion rotation, EntityConfiguration configuration) where T : Entity
        {
            var type = typeof(T);
            if (type == typeof(MindlessTitan))
            {
                return SpawnTitan("MindlessTitan", position, rotation, configuration as TitanConfiguration) as T;
            }

            if (type == typeof(FemaleTitan))
                return SpawnTitan("FemaleTitan", position, rotation, null) as T;

            if (type == typeof(ColossalTitan))
                return SpawnTitan("ColossalTitan", position, rotation, null) as T;

            if (type == typeof(ErenTitan))
                return SpawnTitan("ErenTitan", position, rotation, null) as T;

            throw new ArgumentException($"{type} is not implemented");
        }

        public T Spawn<T>(Vector3 position, Quaternion rotation, CharacterPreset preset) where T : Human
        {
            var type = typeof(T);
            if (type == typeof(Hero))
            {
                return SpawnHero("Hero", position, rotation, preset) as T;
            }

            throw new ArgumentException($"{type} is not implemented");
        }

        private CharacterPreset LastUsedPreset { get; set; }
        private Hero SpawnHero(string prefab, Vector3 position, Quaternion rotation, CharacterPreset preset)
        {
            preset ??= LastUsedPreset;
            var human = PhotonNetwork.Instantiate(prefab, position, rotation, 0).GetComponent<Hero>();
            human.Initialize(preset);
            LastUsedPreset = preset;
            return human;
        }

        private TitanBase SpawnTitan(string prefab, TitanConfiguration configuration)
        {
            var spawn = GetRandom<TitanSpawner>();
            return SpawnTitan(prefab, spawn.transform.position, spawn.transform.rotation, configuration);
        }

        private TitanBase SpawnTitan(string prefab, Vector3 position, Quaternion rotation, TitanConfiguration configuration)
        {
            var titan = PhotonNetwork.Instantiate(prefab, position, rotation, 0).GetComponent<TitanBase>();
            titan.Initialize(configuration);
            return titan;
        }

        private PlayerTitan SpawnPlayerTitan()
        {
            var id = "TITAN";
            var tag = "titanRespawn";
            var location = Gamemode.GetPlayerSpawnLocation(tag);
            Vector3 position = location.transform.position;
            var titanSpawners = GetAll<TitanSpawner>().Where(x => x.Type == TitanSpawnerType.None).ToArray();
            if (titanSpawners.Length > 0) // RC Custom Map Spawns
            {
                position = titanSpawners[UnityEngine.Random.Range(0, titanSpawners.Length)].gameObject.transform.position;
            }
            PlayerTitan playerTitan = PhotonNetwork.Instantiate("PlayerTitan", position, new Quaternion(), 0).GetComponent<PlayerTitan>();
            playerTitan.Initialize(Gamemode.GetPlayerTitanConfiguration());
            Service.Player.Self = playerTitan;
            ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
            hashtable.Add("dead", false);
            ExitGames.Client.Photon.Hashtable propertiesToSet = hashtable;
            PhotonNetwork.player.SetCustomProperties(propertiesToSet);
            hashtable = new ExitGames.Client.Photon.Hashtable();
            hashtable.Add(PhotonPlayerProperty.isTitan, 2);
            propertiesToSet = hashtable;
            PhotonNetwork.player.SetCustomProperties(propertiesToSet);
            return playerTitan;
        }



        private void LateUpdate()
        {

        }
    }
}
