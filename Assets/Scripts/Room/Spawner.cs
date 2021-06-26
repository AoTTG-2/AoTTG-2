using Assets.Scripts.Services;
using Assets.Scripts.Services.Interface;
using UnityEngine;

namespace Assets.Scripts.Room
{
    public abstract class Spawner : MonoBehaviour
    {
        protected ISpawnService SpawnService => Service.Spawn;

        protected virtual void Awake()
        {
            SpawnService?.Add(this);
        }

        protected virtual void OnDestroy()
        {
            SpawnService?.Remove(this);
        }
    }
}
