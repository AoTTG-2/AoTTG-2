using Assets.Scripts.Characters;
using Assets.Scripts.Characters.Humans;
using Assets.Scripts.Characters.Humans.Customization;
using Assets.Scripts.Room;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Services.Interface
{
    public interface ISpawnService : IService
    {
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
    }
}
