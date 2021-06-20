using Assets.Scripts.UI.Menu;
using UnityEngine;

namespace Assets.Scripts.UI
{
    public class MenuUi : MonoBehaviour
    {
        public MainMenu MainMenu;
        public Singleplayer Singleplayer;
        public MapConverter MapConverter;
        public Lobby Lobby;
        public CreateRoom CreateRoom;
        public Credits Credits;

        public void ShowMainMenu()
        {
            MainMenu.gameObject.SetActive(true);
            Singleplayer.gameObject.SetActive(false);
            MapConverter.gameObject.SetActive(false);
            Lobby.gameObject.SetActive(false);
            CreateRoom.gameObject.SetActive(false);
            Credits.gameObject.SetActive(false);
        }
    }
}
