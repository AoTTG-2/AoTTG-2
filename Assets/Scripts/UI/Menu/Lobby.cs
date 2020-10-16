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
        [SerializeField]
        private VersionManager versionManager;

        public GameObject ScrollViewContent;
        public GameObject Row;

        private RoomRow selectedRoom;

        public RoomRow SelectedRoom
        {
            get
            {
                return selectedRoom;
            }
            set
            {
                selectedRoom?.PasswordPanel.SetActive(false);
                selectedRoom = value;
            }
        }

        private static string IpAddress { get; set; }

        public static void SetPhotonServerIp(bool isLocal)
        {
            IpAddress = isLocal 
                    ? "127.0.0.1"
                    : "51.210.5.100";
        }

        private int Region { get; set; }

        public void CreateRoom()
        {
            Navigate(typeof(CreateRoom));
        }

        protected override void OnEnable()
        {
            base.OnEnable();


            if (Service.Authentication.AccessToken != null)
            {
                PhotonNetwork.AuthValues = new AuthenticationValues { AuthType = CustomAuthenticationType.Custom };
                PhotonNetwork.AuthValues.AddAuthParameter("token", Service.Authentication.AccessToken);
            }
            else
            {
                // PhotonServer complains about no UserId being set, temp fix
                PhotonNetwork.AuthValues = new AuthenticationValues(Guid.NewGuid().ToString());
            }

            PhotonNetwork.ConnectToMaster(IpAddress, 5055, "", versionManager.Version);
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
            PhotonNetwork.ConnectToMaster(IpAddress, 5055, "", versionManager.Version);
            //PhotonNetwork.ConnectToRegion((CloudRegionCode) Region, "2021");
        }

        public void OnPhotonJoinRoomFailed(object[] codeAndMsg)
        {
            if (SelectedRoom == null) return;
            SelectedRoom.PasswordInputField.text = string.Empty;
            SelectedRoom.PasswordInputField.placeholder.GetComponent<Text>().text = codeAndMsg[1]?.ToString();
        }

        public void OnConnectedToPhoton()
        {
            CancelInvoke("RefreshLobby");
            InvokeRepeating("RefreshLobby", 1f, 5f);
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
                roomRow.DisplayName = $"{roomInfo.GetName()} | {roomInfo.GetLevel()} | {roomInfo.GetGamemode()} | {roomInfo.PlayerCount}/{roomInfo.MaxPlayers}";
                roomRow.Lobby = this;
                roomRow.IsPasswordRequired = roomInfo.IsPasswordRequired();
                roomRow.IsAccountRequired = roomInfo.IsAccountRequired();
            }
        }
    }
}