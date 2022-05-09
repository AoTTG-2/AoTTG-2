using Assets.Scripts.Room;
using Assets.Scripts.Services;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Menu
{
    /// <summary>
    /// UI class for the "Lobby" which contains a list of all available rooms.
    /// </summary>
    public class Lobby : UiNavigationElement
    {
        public GameObject ScrollViewContent;
        public GameObject Row;


        private RoomRow selectedRoom;

        /// <summary>
        /// Returns the room that is currently selected
        /// </summary>
        public RoomRow SelectedRoom
        {
            get { return selectedRoom; }
            set
            {
                selectedRoom?.PasswordPanel.SetActive(false);
                selectedRoom = value;
            }
        }

        #region UI Action
        public void CreateRoom()
        {
            Navigate(typeof(CreateRoom));
        }

        public override void Back()
        {
            base.Back();
            PhotonNetwork.Disconnect();
        }
        #endregion

        #region MonoBehavior



        protected override void OnEnable()
        {
            base.OnEnable();
            Service.Photon.Connect();
        }
        #endregion

        #region PunBehavior
        public override void OnConnectedToPhoton()
        {
            CancelInvoke(nameof(RefreshLobby));
            CancelInvoke(nameof(TryJoin));
            InvokeRepeating(nameof(RefreshLobby), 1f, 5f);
        }

        public override void OnFailedToConnectToPhoton(DisconnectCause cause)
        {
            ClearLobby();
            CancelInvoke(nameof(RefreshLobby));
            CancelInvoke(nameof(TryJoin));
            var row = Instantiate(Row, ScrollViewContent.transform);
            var noRoom = row.GetComponent<RoomRow>();
            noRoom.DisplayName = $"An Error Occured: {cause}";
            noRoom.IsJoinable = false;
            Destroy(SelectedRoom?.gameObject);
            SelectedRoom = null;

            Invoke(nameof(TryJoin), 5f);
        }

        public override void OnPhotonJoinRoomFailed(object[] codeAndMsg)
        {
            if (SelectedRoom == null) return;
            SelectedRoom.PasswordInputField.text = string.Empty;
            SelectedRoom.PasswordInputField.placeholder.GetComponent<Text>().text = codeAndMsg[1]?.ToString();
        }
        #endregion

        public void OnServerChanged(Button change)
        {
            var photonConfig = Service.Photon.GetConfigByName(change.GetComponentInChildren<Text>().text) ;
            Service.Photon.ChangePhotonServer(photonConfig);
            RefreshLobby();
        }

        private void ClearLobby()
        {
            foreach (Transform child in ScrollViewContent.transform)
            {
                if (child == SelectedRoom?.transform) continue;
                Destroy(child.gameObject);
            }
        }

        private void RefreshLobby()
        {
            ClearLobby();

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

        private void TryJoin()
        {
            Service.Photon.Connect();
        }
    }
}