using Assets.Scripts.UI.InGame.Controls;
using UnityEngine;

namespace Assets.Scripts.UI.InGame
{
    public class InGameMenu : MonoBehaviour
    {
        public GameSettingMenu GameSettingsMenu;
        public GameObject GraphicsView;
        public ControlsMenu ControlsMenu;

        // Used by Button.
        public void Quit()
        {
            PhotonNetwork.Disconnect();
        }

        // Used by Button.
        public void ShowGameSettingsMenu()
        {
            GameSettingsMenu.gameObject.SetActive(true);
        }

        // Used by Button.
        public void ShowGraphicSettingsMenu()
        {
            GraphicsView.gameObject.SetActive(true);
        }

        // Used by Button.
        public void ShowRebindsMenu()
        {
            ControlsMenu.gameObject.SetActive(true);
        }

        private void OnEnable()
        {
            MenuManager.RegisterOpened();
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }

        private void OnDisable()
        {
            MenuManager.RegisterClosed();
            GameSettingsMenu.gameObject.SetActive(false);
            GraphicsView.gameObject.SetActive(false);
            ControlsMenu.gameObject.SetActive(false);
        }
    }
}
