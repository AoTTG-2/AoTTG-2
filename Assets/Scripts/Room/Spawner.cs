using Assets.Scripts.Services;
using Assets.Scripts.Services.Interface;
using UnityEngine;

namespace Assets.Scripts.Room
{
    /// <summary>
    /// Abstract spawner class which will automatically register itself to the <see cref="ISpawnService"/>
    /// </summary>
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
