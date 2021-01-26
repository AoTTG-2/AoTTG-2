using Assets.Scripts.Room;
using Assets.Scripts.Services;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Menu
{
    public class Lobby : UiNavigationElement
    {
        public GameObject ScrollViewContent;
        public GameObject Row;
        public Dropdown ServerDropdown;

        private RoomRow selectedRoom;

        public RoomRow SelectedRoom
        {
            get { return selectedRoom; }
            set
            {
                selectedRoom?.PasswordPanel.SetActive(false);
                selectedRoom = value;
            }
        }

        public void CreateRoom()
        {
            Navigate(typeof(CreateRoom));
        }

        private void Awake()
        {
            ServerDropdown.onValueChanged.AddListener(delegate
            {
                OnServerChanged(ServerDropdown);
            });
        }

        private void OnDestroy()
        {
            ServerDropdown.onValueChanged.RemoveAllListeners();
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            Service.Photon.Connect();
            ServerDropdown.options.Clear();

            var servers = Service.Photon.GetAllServers();
            foreach (var server in servers)
            {
                ServerDropdown.options.Add(new Dropdown.OptionData(server.Name));
            }
        }

        public override void Back()
        {
            base.Back();
            PhotonNetwork.Disconnect();
        }

        public void OnServerChanged(Dropdown change)
        {
            var photonConfig = Service.Photon.GetConfigByName(change.options[change.value]?.text);
            Service.Photon.ChangePhotonServer(photonConfig);
        }
        
        public void OnPhotonJoinRoomFailed(object[] codeAndMsg)
        {
            if (SelectedRoom == null) return;
            SelectedRoom.PasswordInputField.text = string.Empty;
            SelectedRoom.PasswordInputField.placeholder.GetComponent<Text>().text = codeAndMsg[1]?.ToString();
        }

        public void OnConnectedToPhoton()
        {
            CancelInvoke(nameof(RefreshLobby));
            InvokeRepeating(nameof(RefreshLobby), 1f, 5f);
        }

        private void RefreshLobby()
        {
            foreach (Transform child in ScrollViewContent.transform)
            {
                if (child == SelectedRoom?.transform) continue;
                Destroy(child.gameObject);
            }

            var rooms = PhotonNetwork.GetRoomList().ToList();

            if (rooms.Count == 0)
            {
                var row = Instantiate(Row, ScrollViewContent.transform);
                var noRoom = row.GetComponent<RoomRow>();
                noRoom.DisplayName = "No Lobbies available...";
                noRoom.IsJoinable = false;
                Destroy(SelectedRoom?.gameObject);
                SelectedRoom = null;
                return;
            }

            var room = rooms.SingleOrDefault(x => x.Name == SelectedRoom?.Room);
            if (selectedRoom == null)
            {
                Destroy(SelectedRoom?.gameObject);
                SelectedRoom = null;
            }
            else
            {
                rooms.Remove(room);
            }

            foreach (var roomInfo in rooms)
            {
                var row = Instantiate(Row, ScrollViewContent.transform);
                var roomRow = row.GetComponent<RoomRow>();
                roomRow.Room = roomInfo.Name;
                roomRow.DisplayName =
                    $"{roomInfo.GetName()} | {roomInfo.GetLevel()} | {roomInfo.GetGamemode()} | {roomInfo.PlayerCount}/{roomInfo.MaxPlayers}";
                roomRow.Lobby = this;
                roomRow.IsPasswordRequired = roomInfo.IsPasswordRequired();
                roomRow.IsAccountRequired = roomInfo.IsAccountRequired();
            }
        }
    }
}