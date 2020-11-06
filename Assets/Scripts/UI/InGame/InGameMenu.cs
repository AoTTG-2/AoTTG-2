using Assets.Scripts.UI.InGame.Controls;
using UnityEngine;

namespace Assets.Scripts.UI.InGame
{
    public class InGameMenu : MonoBehaviour
    {
        /// <summary>
        /// This is the menu that's currently being shown to the player.
        /// </summary>
        public GameObject currentPage;

        /// <summary>
        /// This is the last menu shown to the player.
        /// </summary>
        public GameObject previousPage;

        // Used by Button.
        public void Quit()
        {
            PhotonNetwork.Disconnect();
        }

        /// <summary>
        /// Displays the selected <paramref name="menuObject"/> UI element.
        /// </summary>
        /// <param name="menuObject"></param>
        public void OpenMenu(GameObject menuObject)
        {
            if (previousPage != null)
                previousPage.SetActive(false);

            menuObject.SetActive(true);

            previousPage = currentPage;
            currentPage = menuObject;
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
            currentPage.SetActive(false);
        }
    }
}
