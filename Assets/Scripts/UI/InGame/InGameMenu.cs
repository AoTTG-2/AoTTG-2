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

        private void OnEnable()
        {
            CursorManagement.CameraMode = CursorManagement.Mode.Menu;
        }
        private void OnDisable()
        {
            CursorManagement.CameraMode = CursorManagement.PreferredCameraMode;
            GameSettingsMenu.gameObject.SetActive(false);
            GraphicSettingsMenu.gameObject.SetActive(false);
        }

        public void Quit()
        {
            PhotonNetwork.Disconnect();
			Destroy(GameObject.Find("Canvas"));
        }
    }
}