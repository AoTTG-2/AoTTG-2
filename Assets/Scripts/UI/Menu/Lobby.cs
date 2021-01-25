using Assets.Scripts.Room;
using Assets.Scripts.Services;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Menu
{
    public class Lobby : UiNavigationElement
    {
        public GameObject ScrollViewContent;
        public GameObject Row;

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

        private int Region { get; set; }

        public void CreateRoom()
        {
            Navigate(typeof(CreateRoom));
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            Service.Photon.Initialize();
        }

        public void OnRegionChanged(int region)
        {
            Region = region;
            Service.Photon.ChangeRegionDisconnect();
        }

        public void OnDisconnectedFromPhoton()
        {
            //Check if this is required to be in the service.
            // PhotonServer complains about no UserId being set, temp fix
            //PhotonNetwork.ConnectToRegion((CloudRegionCode) Region, "2021");

            Service.Photon.OnDisconnectFromPhoton();
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