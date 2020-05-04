using UnityEngine;

namespace Assets.Scripts
{

    public delegate void OnTitanSpawned(TITAN titan);
    public delegate void OnTitanKilled(string titanName);
    public delegate void OnUpdate(float interval);
    public delegate void OnRestart();
    public delegate void OnGameWon();
    public delegate void OnGameLost();
    public delegate void OnPlayerKilled(int id);

    public class EventManager : MonoBehaviour
    {
        private static FengGameManagerMKII _gameManager = FengGameManagerMKII.instance;
        public static OnTitanSpawned OnTitanSpawned;
        public static OnRestart OnRestart;
        public static OnUpdate OnUpdate;
        public static OnGameLost OnGameLost;
        public static OnGameWon OnGameWon;
        public static OnTitanKilled OnTitanKilled;
        public static OnPlayerKilled OnPlayerKilled;

        void Start()
        {
            OnTitanSpawned += EventManager_OnTitanSpawned;
            OnTitanKilled += EventManager_OnTitanKilled;
            OnRestart += EventManager_OnRestart;
            OnUpdate += EventManager_OnUpdate;
            OnGameWon += EventManager_OnGameWon;
            OnGameLost += EventManager_OnGameLost;
            OnPlayerKilled += EventManager_OnPlayerKilled;
        }

        private void EventManager_OnPlayerKilled(int id)
        {
            FengGameManagerMKII.Gamemode.OnPlayerKilled(id);
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

        private void EventManager_OnTitanKilled(string titanName)
        {
            FengGameManagerMKII.Gamemode.OnTitanKilled(titanName);
        }
    }
}
