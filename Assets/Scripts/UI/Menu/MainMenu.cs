using Assets.Scripts.Services;
using UnityEngine;
#if UNITY_EDITOR
#else
#endif

namespace Assets.Scripts.UI.Menu
{
    /// <summary>
    /// UI Container class which is used to navigate to all the Main menu UI elements
    /// </summary>
    public class MainMenu : UiNavigationElement
    {
        [SerializeField]
        private float onButtonHoverScaleIncrease,
            onButtonHoverScaleTime;
        [SerializeField]
        private Vector3 buttonDefaultScale;

        public void OnButtonTriggerEnter(GameObject button)
        {
            LeanTween.scale(button, buttonDefaultScale * onButtonHoverScaleIncrease,
                onButtonHoverScaleTime);
        }

        public void OnButtonTriggerExit(GameObject button)
        {
            LeanTween.scale(button, buttonDefaultScale,
                onButtonHoverScaleTime);
        }

        public void Singleplayer()
        {
            Navigate(typeof(Singleplayer));
        }

        public void Multiplayer()
        {
            Navigate(typeof(ServerSelector));
        }

        public void MultiplayerLobby()
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
            //TODO: Switch to MapEditor scene
            Navigate(typeof(MapConverter));
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
