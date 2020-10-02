using System;
using UnityEngine;

namespace Assets.Scripts
{
    public delegate void OnPlayerKilled(int id);

    public delegate void OnRestart();

    [Obsolete("Use Service Events instead")]
    public class EventManager : MonoBehaviour
    {
        public static OnPlayerKilled OnPlayerKilled;
        public static OnRestart OnRestart;

        private void EventManager_OnPlayerKilled(int id)
        {
            FengGameManagerMKII.Gamemode.OnPlayerKilled(id);
        }

        private void EventManager_OnRestart()
        {
            FengGameManagerMKII.Gamemode.OnRestart();
        }
        
        private void Start()
        {
            OnPlayerKilled += EventManager_OnPlayerKilled;
            OnRestart += EventManager_OnRestart;
        }
    }
}