using Assets.Scripts.Room;
using System;
using UnityEngine;

namespace Assets.Scripts.UI.Menu
{
    public class Lobby : UiNavigationElement
    {
        public GameObject ScrollViewContent;
        public GameObject Row;

        private int Region { get; set; }

        public void CreateRoom()
        {
            Navigate(typeof(CreateRoom));
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            // PhotonServer complains about no UserId being set, temp fix
            PhotonNetwork.AuthValues = new AuthenticationValues(Guid.NewGuid().ToString());
            PhotonNetwork.ConnectToMaster("145.239.88.211", 5055, "", FengGameManagerMKII.Version);

            //PhotonNetwork.ConnectToRegion((CloudRegionCode)Region, "2021");
        }

        public void OnRegionChanged(int region)
        {
            Region = region;
            PhotonNetwork.Disconnect();
        }

        public void OnDisconnectedFromPhoton()
        {
            // PhotonServer complains about no UserId being set, temp fix
            PhotonNetwork.AuthValues = new AuthenticationValues(Guid.NewGuid().ToString());
            PhotonNetwork.ConnectToMaster("145.239.88.211", 5055, "", FengGameManagerMKII.Version);
            //PhotonNetwork.ConnectToRegion((CloudRegionCode) Region, "2021");
        }

        public void OnConnectedToPhoton()
        {
            CancelInvoke("RefreshLobby");
            InvokeRepeating("RefreshLobby", 2f, 5f);
        }

        private void RefreshLobby()
        {
            foreach (Transform child in ScrollViewContent.transform)
            {
                Destroy(child.gameObject);
            }

            var rooms = PhotonNetwork.GetRoomList();

            if (rooms.Length == 0)
            {
                var row = Instantiate(Row, ScrollViewContent.transform);
                var room = row.GetComponent<RoomRow>();
                room.DisplayName = "No Lobbies available...";
                room.IsJoinable = false;
                return;
            }

            foreach (var roomInfo in rooms)
            {
                var row = Instantiate(Row, ScrollViewContent.transform);
                var roomRow = row.GetComponent<RoomRow>();
                roomRow.Room = roomInfo.Name;
                roomRow.DisplayName = $"{roomInfo.GetName()} | {roomInfo.GetLevel()} | {roomInfo.GetGamemode()} | {roomInfo.PlayerCount}/{roomInfo.MaxPlayers}";
                roomRow.Lobby = this;
            }
        }
    }
}