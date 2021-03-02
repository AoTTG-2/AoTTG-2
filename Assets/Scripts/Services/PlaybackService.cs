using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Services.Interface;
using Assets.Scripts.UI.Input;

namespace Assets.Scripts.Services
{
    public class PlaybackService : MonoBehaviour, IPlaybackService
    {
        private void Awake()
        {
            InputManager.Human.RegisterDown(InputHuman.Attack, OnAttack);
            InputManager.Cannon.RegisterUp(InputCannon.Down, OnCannonDownStop);
            InputManager.Horse.RegisterHeld(InputHorse.Mount, OnHorseMountHeld);
        }

        private void OnCannonDownStop()
        {
            Debug.Log("Stopped pressing cannon down");
        }

        private void OnAttack()
        {
            Debug.Log("Pressed Attack");
        }

        private void OnHorseMountHeld()
        {
            Debug.Log("Holding Horse Mount key down");
        }

        private void OnDestroy()
        {
            InputManager.Human.DeregisterDown(InputHuman.Attack, OnAttack);
            InputManager.Cannon.DeregisterUp(InputCannon.Down, OnCannonDownStop);
            InputManager.Horse.DeregisterHeld(InputHorse.Mount, OnHorseMountHeld);
        }

        public void Test()
        {
        }

        public bool Save(string filePath)
        {

            return true;
        }

        public void OnRestart()
        {

        }
    }
}