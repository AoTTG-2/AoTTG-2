using Assets.Scripts.UI.InGame.Rebinds;
using UnityEngine;

namespace Assets.Scripts.UI.InGame
{
    public class InGameMenu : MonoBehaviour
    {
        public GameSettingMenu GameSettingsMenu;
        public GameObject GraphicsView;
        public RebindsMenu RebindsMenu;

        // Used by Button.
        public void Quit()
        {
            PhotonNetwork.Disconnect();
            Destroy(GameObject.Find("Canvas"));
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
            RebindsMenu.gameObject.SetActive(true);
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
            RebindsMenu.gameObject.SetActive(false);
        }
    }
}
