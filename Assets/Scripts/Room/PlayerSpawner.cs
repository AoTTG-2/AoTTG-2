using UnityEngine;

namespace Assets.Scripts.Room
{
    public class PlayerSpawner : MonoBehaviour
    {
        public PlayerSpawnType Type = PlayerSpawnType.None;

        private void Awake()
        {
            FengGameManagerMKII.instance.PlayerSpawners.Add(this);
        }

        private void OnDestroy()
        {
            FengGameManagerMKII.instance.PlayerSpawners.Remove(this);
        }
    }
}
