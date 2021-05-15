using Assets.Scripts.Services.Interface;
using Assets.Scripts.Settings;
using Photon;
using System;
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

        #region MonoBehavior
        private void Awake()
        {
            currentServerConfig = photonServerConfiguration.FirstOrDefault();
            if (photonServerConfiguration == null)
                photonServerConfiguration = new List<PhotonServerConfig>();
        }
        #endregion

        #region PunBehavior
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