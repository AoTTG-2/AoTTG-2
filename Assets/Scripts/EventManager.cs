using UnityEngine;

namespace Assets.Scripts
{

    public delegate void OnTitanSpawned(TITAN titan);

    public class EventManager : MonoBehaviour
    {
        private static FengGameManagerMKII _gameManager = FengGameManagerMKII.instance;
        public static OnTitanSpawned OnTitanSpawned;

        void Start()
        {
            OnTitanSpawned += EventManager_OnTitanSpawned;
        }

        private void EventManager_OnTitanSpawned(TITAN titan)
        {
            FengGameManagerMKII.Gamemode.OnTitanSpawned(titan);
        }

        
    }
}
