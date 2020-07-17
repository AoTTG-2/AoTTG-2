using UnityEngine;

namespace Assets.Scripts.Room
{
    public class PlayerSpawner : MonoBehaviour
    {
        public PlayerSpawnType Type = PlayerSpawnType.None;

        private void Awake()
        {
            Debug.Log("Added");
            FengGameManagerMKII.instance.PlayerSpawners.Add(this);
        }

        private void OnDestroy()
        {
            Debug.Log("Removed");
            FengGameManagerMKII.instance.PlayerSpawners.Remove(this);
        }
    }
}
