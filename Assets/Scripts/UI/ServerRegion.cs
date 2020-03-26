using UnityEngine;

namespace Assets.Scripts.UI
{
    public class ServerRegion : UiElement
    {
        public GameObject ScrollViewContent;
        public GameObject Row;
        private int Region { get; set; }

        public void OnEnable()
        {
            PhotonNetwork.ConnectToRegion((CloudRegionCode)Region, "2020");
        }

        public void OnRegionChanged(int region)
        {
            Region = region;
            PhotonNetwork.Disconnect();

        }

        public void OnDisconnectedFromPhoton()
        {
            PhotonNetwork.ConnectToRegion((CloudRegionCode) Region, "2020");
        }

        public void OnConnectedToPhoton()
        {
            CancelInvoke("RefreshLobby");
            InvokeRepeating("RefreshLobby", 2f, 5f);
        }

        void RefreshLobby()
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
                room.Room = "No Lobbies available...";
                room.IsJoinable = false;
                return;
            }

            foreach (var roomInfo in rooms)
            {
                var row = Instantiate(Row, ScrollViewContent.transform);
                row.GetComponent<RoomRow>().Room = roomInfo.Name;
            }
        }
    }
}
