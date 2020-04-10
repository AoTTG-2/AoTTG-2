using Assets.Scripts.UI;
using Assets.Scripts.UI.Menu;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Menu
{
    public class RoomRow : MonoBehaviour
    {
        public string Room;
        public string DisplayName;
        public bool IsJoinable = true;

        public Lobby Lobby;

        // Use this for initialization
        void Start()
        {
            GetComponentInChildren<Text>().text = DisplayName;
        }

        public void JoinLobby()
        {
            if (!IsJoinable) return;
            PhotonNetwork.JoinRoom(Room);
            SceneManager.sceneLoaded += SceneManager_sceneLoaded;
        }

        private void SceneManager_sceneLoaded(Scene arg0, LoadSceneMode arg1)
        {
            Lobby.Canvas.ShowInGameUi();
        }
    }
}
