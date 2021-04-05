using Assets.Scripts.Services;

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
            Service.Photon.UpdateConnectionType(false);
            Navigate(typeof(Lobby));
        }

        public void LAN()
        {
            Service.Photon.UpdateConnectionType(true);
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
