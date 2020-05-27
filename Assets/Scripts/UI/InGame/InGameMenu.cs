using UnityEngine;

namespace Assets.Scripts.UI.InGame
{
    public class InGameMenu : MonoBehaviour
    {
        public GameSettingMenu GameSettingsMenu;
        public GameObject GraphicsView;
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

        private void OnEnable()
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        private void OnDisable()
        {
            GameSettingsMenu.gameObject.SetActive(false);
            GraphicsView.gameObject.SetActive(false);
            if (FindObjectOfType<GraphicsController>().label.text != "")
            {
                FindObjectOfType<GraphicsController>().label.text = "";
            }
            Cursor.visible = false;
            if (IN_GAME_MAIN_CAMERA.cameraMode == CAMERA_TYPE.TPS)
            {
                Cursor.lockState = CursorLockMode.Locked;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Confined;
            }
        }

        public void Quit()
        {
            PhotonNetwork.Disconnect();
			Destroy(GameObject.Find("Canvas"));
        }
    }
}