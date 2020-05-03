using UnityEngine;

namespace Assets.Scripts.UI.InGame
{
    public class InGameMenu : MonoBehaviour
    {
        public GameSettingMenu GameSettingsMenu;
        public GraphicSettingMenu GraphicSettingsMenu;
        public void ShowGameSettingsMenu()
        {
            GameSettingsMenu.gameObject.SetActive(true);
        }

        private void SetGameSettingsMenu()
        {
        }

        public void ShowGraphicSettingsMenu()
        {
            GraphicSettingsMenu.gameObject.SetActive(true);
        }

        private void OnDisable()
        {
            GameSettingsMenu.gameObject.SetActive(false);
            GraphicSettingsMenu.gameObject.SetActive(false);
        }

        public void Quit()
        {
            PhotonNetwork.Disconnect();
        }
    }
}