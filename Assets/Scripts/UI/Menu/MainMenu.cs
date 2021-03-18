using Assets.Scripts.Services;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
#else
using UnityEngine;
#endif

namespace Assets.Scripts.UI.Menu
{
    public class MainMenu : UiNavigationElement
    {
        public void Singleplayer()
        {
            Navigate(typeof(Singleplayer));
        }

        public void Multiplayer()
        {
            Service.Photon.Connect();
            Navigate(typeof(Lobby));
        }

        public void Credits()
        {
            Navigate(typeof(Credits));
        }

        public void MapEditor()
        {
            //TODO : Fix this later.
            SceneManager.LoadScene(10);
            //fix this later too
            transform.root.gameObject.SetActive(false);
        }

        public void Quit()
        {

#if UNITY_EDITOR

            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}
