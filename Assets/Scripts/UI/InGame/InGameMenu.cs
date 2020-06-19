using UnityEngine;

namespace Assets.Scripts.UI.InGame
{
    public class InGameMenu : MonoBehaviour
    {
        public GameSettingMenu GameSettingsMenu;
        public GameObject GraphicsView;

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

        private void OnDisable()
        {
            GameSettingsMenu.gameObject.SetActive(false);
            GraphicsView.gameObject.SetActive(false);

            MenuManager.RegisterClosed();
        }

        private void OnEnable()
        {
            MenuManager.RegisterOpened();
        }

        private void SetGameSettingsMenu()
        {
        }
    }
}
