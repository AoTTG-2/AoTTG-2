using Assets.Scripts.Services;
using Assets.Scripts.Services.Interface;
using UnityEngine;

namespace Assets.Scripts.Room
{
    public class PlayerSpawner : MonoBehaviour
    {
        private readonly IRespawnService _respawnService = Service.Respawn;

        public PlayerSpawnType Type = PlayerSpawnType.None;

        private void Awake()
        {
            _respawnService.Add(this);
        }

        private void OnDestroy()
        {
            _respawnService.Remove(this);
        }
    }
}
