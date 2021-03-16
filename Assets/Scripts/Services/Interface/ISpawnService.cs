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
        void Add(Spawner spawner);
        void Remove(Spawner spawner);
        List<T> GetAll<T>() where T : Spawner;
        T GetRandom<T>() where T : Spawner;
        List<HumanSpawner> GetByType(PlayerSpawnType type);
        List<TitanSpawner> GetByType(TitanSpawnerType type);

        T Spawn<T>() where T : Entity;
        T Spawn<T>(EntityConfiguration configuration) where T : Entity;
        T Spawn<T>(Vector3 position, Quaternion rotation, EntityConfiguration configuration) where T : Entity;
        T Spawn<T>(Vector3 position, Quaternion rotation, CharacterPreset preset) where T : Human;
    }
}
