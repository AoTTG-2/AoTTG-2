using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Menu
{
    public class RoomRow : MonoBehaviour
    {
        public Image PasswordIcon;
        public GameObject PasswordPanel;
        public InputField PasswordInputField;

        public string Room;
        public string DisplayName;

        private bool isSecure;
        public bool IsSecure
        {
            get
            {
                return isSecure;
            }
            set
            {
                PasswordIcon.gameObject.SetActive(value);
                isSecure = value;
            }
        }
        public bool IsJoinable = true;

        public Lobby Lobby;

        // Use this for initialization
        void Start()
        {
            GetComponentInChildren<Text>().text = DisplayName;
        }

        public void JoinRoom()
        {
            if (!IsJoinable) return;
            Lobby.SelectedRoom = this;
            if (IsSecure)
            {
                PasswordPanel.SetActive(true);
                return;
            }
            PhotonNetwork.JoinRoom(Room);
            SceneManager.sceneLoaded += SceneManager_sceneLoaded;
        }

        public void JoinPasswordRoom()
        {
            if (string.IsNullOrEmpty(PasswordInputField.text))
            {
                return;
            }

            PhotonNetwork.JoinRoom(Room, PasswordInputField.text);
            SceneManager.sceneLoaded += SceneManager_sceneLoaded;
        }

        public void CancelPasswordRoom()
        {
            PasswordPanel.gameObject.SetActive(false);
        }


        private void SceneManager_sceneLoaded(Scene arg0, LoadSceneMode arg1)
        {
            Lobby.Canvas.ShowInGameUi();
        }
    }
}
