using Assets.Scripts.Services.Interface;
using Assets.Scripts.Settings;
using Photon;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Services
{
    public class PhotonService : PunBehaviour, IPhotonService
    {
        public VersionManager VersionManager;
        [SerializeField] private List<PhotonServerConfig> photonServerConfiguration;
        private PhotonServerConfig currentServerConfig;

        private static string IpAddress { get; set; }
        private bool isRegionChanging;
        private bool isJoinedLobby;
        private bool isOffline;

        #region MonoBehavior
        private void Awake()
        {
            currentServerConfig = photonServerConfiguration.FirstOrDefault();
            if (photonServerConfiguration == null)
                photonServerConfiguration = new List<PhotonServerConfig>();
        }
        #endregion

        #region PunBehavior

        public override void OnConnectedToMaster()
        {
            isOffline = true;
        }

        public override void OnConnectedToPhoton()
        {
            isRegionChanging = false;
        }

        public override void OnDisconnectedFromPhoton()
        {
            if (isRegionChanging)
            {
                PhotonNetwork.ConnectToMaster(currentServerConfig.IpAddress, currentServerConfig.Port, "", VersionManager.Version);
                isRegionChanging = false;
            }
        }

        public override void OnJoinedLobby()
        {
            base.OnJoinedLobby();
            isJoinedLobby = true;
        }

        public override void OnCreatedRoom()
        {
            isStatelesslyConnected = true;
        }

        #endregion

        public bool IsChangingRegion() => isRegionChanging;
        public List<PhotonServerConfig> GetAllServers() => photonServerConfiguration;

        public PhotonServerConfig GetCurrentConfig()
        {
            return currentServerConfig;
        }

        public PhotonServerConfig GetConfigByName(string configName)
        {
            return photonServerConfiguration.FirstOrDefault(x =>
                string.Equals(configName, x.Name, StringComparison.InvariantCultureIgnoreCase));
        }

        public void Connect()
        {
            if (Service.Authentication.AccessToken != null)
            {
                PhotonNetwork.AuthValues = new AuthenticationValues { AuthType = CustomAuthenticationType.Custom };
                PhotonNetwork.AuthValues.AddAuthParameter("token", Service.Authentication.AccessToken);
            }
            PhotonNetwork.ConnectToMaster(currentServerConfig.IpAddress, currentServerConfig.Port, "", VersionManager.Version);
        }

        public void StatelessConnect(bool offline, string roomName, string levelName, string gamemodeName)
        {
            if (offline)
            {
                PhotonNetwork.offlineMode = true;
                StartCoroutine(CreateOfflineRoom(levelName, gamemodeName));
            }
            else
            {
                StartCoroutine(CreateOnlineRoom(roomName, levelName, gamemodeName));
            }
        }

        private IEnumerator CreateOfflineRoom(string levelName, string gamemodeName)
        {
            PhotonNetwork.offlineMode = true; //TODO: Await the OnConnectedToMaster
            while (!isOffline)
            {
                yield return new WaitForSeconds(0.5f);
            }
            var roomOptions = new RoomOptions
            {
                IsVisible = true,
                IsOpen = true,
                MaxPlayers = 10,
                CustomRoomProperties = new ExitGames.Client.Photon.Hashtable
                {
                    { "name", "Singleplayer" },
                    { "level", levelName},
                    { "gamemode", gamemodeName }
                },
                CustomRoomPropertiesForLobby = new[] { "name", "level", "gamemode" }
            };

            PhotonNetwork.CreateRoom(Guid.NewGuid().ToString(), roomOptions, TypedLobby.Default);
        }
        
        private IEnumerator CreateOnlineRoom(string roomName, string levelName, string gamemodeName)
        {
            float startTime = Time.time;
            Service.Photon.Connect();

            while (!isJoinedLobby)
            {
                yield return new WaitForSeconds(0.2f);
                if (Time.time - startTime > 3)
                {
                    break;
                }
            }

            var roomOptions = new RoomOptions
            {
                IsVisible = true,
                IsOpen = true,
                MaxPlayers = 10,
                CustomRoomProperties = new ExitGames.Client.Photon.Hashtable
                {
                    { "name", roomName },
                    { "level", levelName},
                    { "gamemode", gamemodeName }
                },
                CustomRoomPropertiesForLobby = new[] { "name", "level", "gamemode" }
            };

            if (isJoinedLobby)
                PhotonNetwork.CreateRoom(Guid.NewGuid().ToString(), roomOptions, TypedLobby.Default);
        }

        //public void StatelessLocalCreate(string levelName, string gamemodeName)
        //{
        //    StartCoroutine(SetOffline(levelName, gamemodeName));
        //}

        //private IEnumerator SetOffline(string levelName, string gamemodeName)
        //{
        //    PhotonNetwork.offlineMode = true; //TODO: Await the OnConnectedToMaster
        //    while (!isOffline)
        //    {
        //        yield return new WaitForSeconds(0.5f);
        //    }
        //    JoinRoutine("123456", levelName, gamemodeName);
        //}

        //private void JoinRoutine(string roomID, string levelName, string gamemodeName)
        //{
        //    var roomOptions = new RoomOptions
        //    {
        //        IsVisible = true,
        //        IsOpen = true,
        //        MaxPlayers = 10,
        //        CustomRoomProperties = new ExitGames.Client.Photon.Hashtable
        //        {
        //            { "name", "Singleplayer" },
        //            { "level", levelName},
        //            { "gamemode", gamemodeName }
        //        },
        //        CustomRoomPropertiesForLobby = new[] { "name", "level", "gamemode" }
        //    };

        //    PhotonNetwork.CreateRoom(roomID, roomOptions, TypedLobby.Default);
        //}

        private bool isStatelesslyConnected = false;
        public bool IsStatelesslyConnected() => isStatelesslyConnected;

        public void ChangePhotonServer(PhotonServerConfig server)
        {
            currentServerConfig = server;

            isRegionChanging = true;
            if (PhotonNetwork.connected)
            {
                PhotonNetwork.Disconnect();
            }
            else
            {
                Connect();
            }
        }
    }
}