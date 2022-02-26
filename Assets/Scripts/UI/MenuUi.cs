using Assets.Scripts.Audio;
using Assets.Scripts.Events.Args;
using Assets.Scripts.Graphics;
using Assets.Scripts.Services;
using Assets.Scripts.UI.Menu;
using UnityEngine;

namespace Assets.Scripts.UI
{
    /// <summary>
    /// Container class which contains references to all Main Menu UI elements
    /// </summary>
    public class MenuUi : MonoBehaviour
    {
        public MainMenu MainMenu;
        public Singleplayer Singleplayer;
        public MapConverter MapConverter;
        public ServerSelector ServerSelector;
        public Lobby Lobby;
        public CreateRoom CreateRoom;
        public Credits Credits;

        public void ShowMainMenu()
        {
            Service.Music.SetMusicState(new MusicStateChangedEvent(MusicState.MainMenu));
            FramerateController.LockFramerateToRefreshRate();
            MainMenu.gameObject.SetActive(true);
            Singleplayer.gameObject.SetActive(false);
            MapConverter.gameObject.SetActive(false);
            Lobby.gameObject.SetActive(false);
            CreateRoom.gameObject.SetActive(false);
            Credits.gameObject.SetActive(false);
        }
    }
}
