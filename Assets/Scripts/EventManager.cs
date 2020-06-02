using Assets.Scripts.Characters.Titan;
using System;
using UnityEngine;

namespace Assets.Scripts
{
    public delegate void OnGameLoading();

    public delegate void OnGameLost();

    public delegate void OnGameWon();

    public delegate void OnMenuToggled(bool isMenuActive);

    public delegate void OnPlayerKilled(int id);

    public delegate void OnRestart();

    public delegate void OnTitanKilled(string titanName);

    public delegate void OnTitanSpawned(MindlessTitan titan);

    public delegate void OnUpdate(float interval);

    public class EventManager : MonoBehaviour
    {
        public static OnGameLoading OnGameLoading;
        public static OnGameLost OnGameLost;
        public static OnGameWon OnGameWon;
        public static OnMenuToggled OnMenuToggled;
        public static OnPlayerKilled OnPlayerKilled;
        public static OnRestart OnRestart;
        public static OnTitanKilled OnTitanKilled;
        public static OnTitanSpawned OnTitanSpawned;
        public static OnUpdate OnUpdate;
        private static FengGameManagerMKII _gameManager = FengGameManagerMKII.instance;

        private void EventManager_OnGameLost()
        {
            FengGameManagerMKII.Gamemode.OnGameLost();
        }

        private void EventManager_OnGameWon()
        {
            FengGameManagerMKII.Gamemode.OnGameWon();
        }

        private void EventManager_OnMenuToggled(bool isMenuActive)
        {
            Debug.Log($"Menu: {isMenuActive}");
        }

        private void EventManager_OnPlayerKilled(int id)
        {
            FengGameManagerMKII.Gamemode.OnPlayerKilled(id);
        }

        private void EventManager_OnRestart()
        {
            FengGameManagerMKII.Gamemode.OnRestart();
        }

        private void EventManager_OnTitanKilled(string titanName)
        {
            FengGameManagerMKII.Gamemode.OnTitanKilled(titanName);
        }

        private void EventManager_OnTitanSpawned(MindlessTitan titan)
        {
            FengGameManagerMKII.Gamemode.OnTitanSpawned(titan);
        }

        private void EventManager_OnUpdate(float interval)
        {
            FengGameManagerMKII.Gamemode.OnUpdate(interval);
        }

        private void Start()
        {
            OnMenuToggled += EventManager_OnMenuToggled;
            OnGameLost += EventManager_OnGameLost;
            OnGameWon += EventManager_OnGameWon;
            OnPlayerKilled += EventManager_OnPlayerKilled;
            OnRestart += EventManager_OnRestart;
            OnTitanKilled += EventManager_OnTitanKilled;
            OnTitanSpawned += EventManager_OnTitanSpawned;
            OnUpdate += EventManager_OnUpdate;
        }
    }
}