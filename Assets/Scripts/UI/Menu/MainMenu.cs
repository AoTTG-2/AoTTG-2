using UnityEngine; 

namespace Assets.Scripts.UI.Menu
{
    public class MainMenu : UiNavigationElement
    {
        private void OnEnable()
        {
            // TODO: Find out whether it really should be confined.
            //Cursor.visible = true;
            //Cursor.lockState = CursorLockMode.Confined; 
            CursorManagement.CameraMode = CursorManagement.Mode.Menu;
        } 
        public void Singleplayer()
        {
            Navigate(typeof(Singleplayer));
        }

        public void Multiplayer()
        {
            Navigate(typeof(Lobby));
        }

        public void Credits()
        {
            Navigate(typeof(Credits));
        }

        public void MapEditor()
        {
            //TODO: Switch to MapEditor scene
        }
    }
}
