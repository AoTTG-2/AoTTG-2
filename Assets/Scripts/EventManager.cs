using UnityEngine;

namespace Assets.Scripts
{

    public delegate void OnTitanSpawned(TITAN titan);
    public delegate void OnRestart();
    public delegate void OnUpdate(float interval);
    public delegate void OnGameWon();
    public delegate void OnGameLost();

    public class EventManager : MonoBehaviour
    {
        private static FengGameManagerMKII _gameManager = FengGameManagerMKII.instance;
        public static OnTitanSpawned OnTitanSpawned;
        public static OnRestart OnRestart;
        public static OnUpdate OnUpdate;
        public static OnGameLost OnGameLost;
        public static OnGameWon OnGameWon;

        void Start()
        {
            OnTitanSpawned += EventManager_OnTitanSpawned;
            OnRestart += EventManager_OnRestart;
            OnUpdate += EventManager_OnUpdate;
            OnGameWon += EventManager_OnGameWon;
            OnGameLost += EventManager_OnGameLost;
        }

        private void EventManager_OnTitanSpawned(TITAN titan)
        {
            FengGameManagerMKII.Gamemode.OnTitanSpawned(titan);
        }

        private void EventManager_OnRestart()
        {
            FengGameManagerMKII.Gamemode.OnRestart();
        }

        private void EventManager_OnUpdate(float interval)
        {
            FengGameManagerMKII.Gamemode.OnUpdate(interval);
        }

        private void EventManager_OnGameWon()
        {
            FengGameManagerMKII.Gamemode.OnGameWon();
        }

        private void EventManager_OnGameLost()
        {
            FengGameManagerMKII.Gamemode.OnGameLost();
        }


    }
}
