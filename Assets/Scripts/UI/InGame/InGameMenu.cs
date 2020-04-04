using System;
using System.ComponentModel;
using System.Linq;
using Assets.Scripts.Gamemode;
using UnityEngine;

namespace Assets.Scripts.UI.InGame
{
    public class InGameMenu : MonoBehaviour
    {
        public GameSettingsMenu GameSettingsMenu;
        public void ShowGameSettingsMenu()
        {
            GameSettingsMenu.gameObject.SetActive(true);

        }

        private void SetGameSettingsMenu()
        {
        }

        public void Quit()
        {
            PhotonNetwork.Disconnect();
        }
    }
}