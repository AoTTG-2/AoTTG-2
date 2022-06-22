using Assets.Scripts.Characters;
using Assets.Scripts.Characters.Humans;
using Assets.Scripts.Characters.Humans.Customization;
using Assets.Scripts.Room;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Events;

namespace Assets.Scripts.Services.Interface
{
    public interface ISpawnService : IService
    {
        /// <summary>
        /// Occurs when SpawnPlayer is called.
        /// </summary>
        event OnPlayerSpawn<Entity> OnPlayerSpawn;
        /// <summary>
        /// Occurs when a hero or player titan entity matching Service.Player.Self is killed.
        /// </summary>
        event OnPlayerDespawn<Entity> OnPlayerDespawn;
        /// <summary>
        /// Adds a new spawner
        /// </summary>
        /// <param name="spawner"></param>
        void Add(Spawner spawner);
        /// <summary>
        /// Removes an existing spawner
        /// </summary>
        /// <param name="spawner"></param>
        void Remove(Spawner spawner);
        /// <summary>
        /// Returns all spawners
        /// </summary>
        /// <typeparam name="T">The type of the spawner</typeparam>
        /// <returns></returns>
        List<T> GetAll<T>() where T : Spawner;
        /// <summary>
        /// Returns a random spawner
        /// </summary>
        /// <typeparam name="T">The type of the spawner</typeparam>
        /// <returns></returns>
        T GetRandom<T>() where T : Spawner;
        /// <summary>
        /// Returns all HumanSpawners of <paramref name="type"/>
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        List<HumanSpawner> GetByType(PlayerSpawnType type);
        /// <summary>
        /// Returns all HumanSpawners of <paramref name="type"/>
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        List<TitanSpawner> GetByType(TitanSpawnerType type);
        /// <summary>
        /// Used for Custom maps
        /// </summary>
        /// <returns></returns>
        (Vector3 position, Quaternion rotation) GetRandomSpawnPosition();

        /// <summary>
        /// Spawns an entity. Default values of <see cref="EntityConfiguration"/> will be used
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        T Spawn<T>() where T : Entity;
        /// <summary>
        /// Spawns an entity while using the <paramref name="configuration"/>. A random spawn location will be determined by the implementation.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="configuration"></param>
        /// <returns></returns>
        T Spawn<T>(EntityConfiguration configuration) where T : Entity;
        /// <summary>
        /// Spawns an entity with configuration and spawn coordinates.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="position"></param>
        /// <param name="rotation"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        T Spawn<T>(Vector3 position, Quaternion rotation, EntityConfiguration configuration) where T : Entity;
        /// <summary>
        /// Spawns a human with the preset and spawn coordinates
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="position"></param>
        /// <param name="rotation"></param>
        /// <param name="preset"></param>
        /// <returns></returns>
        T Spawn<T>(Vector3 position, Quaternion rotation, CharacterPreset preset) where T : Human;

        /// <summary>
        /// Handles everything needed to spawn a player. Null is a valid argument for both of the parameters.
        /// If a null Spawner is given then the last used spawner will be used if it exists. If not, a random human spawner will be selected.
        /// Similarly a null CharacterPreset will use the last used preset if it exists. If not, the first character preset in the CharacterPreset list will be selected.
        /// </summary>
        /// <param name="gameObject"></param>
        /// <param name="preset"></param>
        /// <returns></returns>
        Hero SpawnPlayer(GameObject gameObject, CharacterPreset preset);

        void InvokeOnPlayerDespawn(Entity entity);

        void RespawnRpc(PhotonMessageInfo info);
        IEnumerator WaitAndRespawn(float time);

        IEnumerator WaitAndRespawnAt(float time, Spawner spawner);

        void NOTSpawnPlayer(string id = "2");
    }
}
