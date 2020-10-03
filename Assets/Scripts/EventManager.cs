using System;
using UnityEngine;

namespace Assets.Scripts
{
    public delegate void OnRestart();

    [Obsolete("Use Service Events instead")]
    public class EventManager : MonoBehaviour
    {
        public static OnRestart OnRestart;

        private void EventManager_OnRestart()
        {
            FengGameManagerMKII.Gamemode.OnRestart();
        }
        
        private void Start()
        {
            OnRestart += EventManager_OnRestart;
        }
    }
}