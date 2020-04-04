﻿using Assets.Scripts.UI.InGame.HUD;
using UnityEngine;

namespace Assets.Scripts.UI.InGame
{
    public class InGameUi : MonoBehaviour
    {
        public HUD.HUD HUD;
        public InGameMenu Menu;
        public SpawnMenu SpawnMenu;

        void OnEnable()
        {
            HUD.gameObject.SetActive(true);
            SpawnMenu.gameObject.SetActive(true);
            Menu.gameObject.SetActive(false);
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Menu.gameObject.SetActive(!Menu.isActiveAndEnabled);
            }
        }
    }
}