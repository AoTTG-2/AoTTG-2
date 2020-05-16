using UnityEngine;

namespace Assets.Scripts.UI.InGame
{
    public class InGameMenu : MonoBehaviour
    {
        public GameSettingMenu GameSettingsMenu;
        public GraphicsView GraphicsView;
        public void ShowGameSettingsMenu()
        {
            GameSettingsMenu.gameObject.SetActive(true);
        }

        private void SetGameSettingsMenu()
        {
        }

        public void ShowGraphicSettingsMenu()
        {
            GraphicsView.gameObject.SetActive(true);
        }

        private void OnDisable()
        {
            GameSettingsMenu.gameObject.SetActive(false);
            GraphicsView.gameObject.SetActive(false);
        }

        public void Quit()
        {
            PhotonNetwork.Disconnect();
        }
    }
}