using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Menu
{
    public class RoomRow : UiElement
    {
        public Image PasswordIcon;
        public Image AccountIcon;
        public GameObject PasswordPanel;
        public InputField PasswordInputField;

        public string Room;
        public string DisplayName;

        private bool isPasswordRequired;
        public bool IsPasswordRequired
        {
            get => isPasswordRequired;
            set
            {
                PasswordIcon.gameObject.SetActive(value);
                isPasswordRequired = value;
            }
        }

        private bool isAccountRequired;
        public bool IsAccountRequired
        {
            get => isAccountRequired;
            set
            {
                AccountIcon.gameObject.SetActive(value);
                isAccountRequired = value;
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
            if (IsPasswordRequired)
            {
                PasswordPanel.SetActive(true);
                return;
            }
            PhotonNetwork.JoinRoom(Room);
        }

        public void JoinPasswordRoom()
        {
            if (string.IsNullOrEmpty(PasswordInputField.text))
            {
                return;
            }

            PhotonNetwork.JoinRoom(Room, PasswordInputField.text);
        }

        public void CancelPasswordRoom()
        {
            PasswordPanel.gameObject.SetActive(false);
        }
    }
}
