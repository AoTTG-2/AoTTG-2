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

        public void Quit()
        {
            PhotonNetwork.Disconnect();
        }
    }
}