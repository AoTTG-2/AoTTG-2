using CustomLogic;
using UnityEngine;

namespace Assets.Scripts
{

    public delegate void OnTitanSpawned(TITAN titan);
    public delegate void OnTitanKilled(string titanName);
    public delegate void OnUpdate(float interval);
    public delegate void OnRoundStart();
    public delegate void OnRestart();
    public delegate void OnGameWon();
    public delegate void OnGameLost();
    public delegate void OnPlayerKilled(int id);
    public delegate void OnChatInput(string input);

    public class EventManager : MonoBehaviour
    {
        public static OnTitanSpawned OnTitanSpawned;
        public static OnRoundStart OnRoundStart;
        public static OnRestart OnRestart;
        public static OnUpdate OnUpdate;
        public static OnGameLost OnGameLost;
        public static OnGameWon OnGameWon;
        public static OnTitanKilled OnTitanKilled;
        public static OnPlayerKilled OnPlayerKilled;
        public static OnChatInput OnChatInput;

        void Start()
        {
            OnTitanSpawned += EventManager_OnTitanSpawned;
            OnTitanKilled += EventManager_OnTitanKilled;
            OnRoundStart += EventManager_OnRoundStart;
            OnUpdate += EventManager_OnUpdate;
            OnGameWon += EventManager_OnGameWon;
            OnGameLost += EventManager_OnGameLost;
            OnPlayerKilled += EventManager_OnPlayerKilled;
            OnChatInput += EventManager_OnChatInput;
            OnRestart += EventManager_OnRestart;
        }

        private void EventManager_OnChatInput(string input)
        {
            Events.OnChatInput?.Invoke(input);
        }

        private void EventManager_OnPlayerKilled(int id)
        {
            FengGameManagerMKII.Gamemode.OnPlayerKilled(id);
        }

        private void EventManager_OnTitanSpawned(TITAN titan)
        {
            FengGameManagerMKII.Gamemode.OnTitanSpawned(titan);
        }

        private void EventManager_OnRoundStart()
        {
            Events.OnRoundStart?.Invoke();
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
