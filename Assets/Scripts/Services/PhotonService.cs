using System;
using Assets.Scripts.Services.Interface;
using Photon;
using UnityEngine;

namespace Assets.Scripts.Services
{
    public class PhotonService : PunBehaviour, IPhotonService
    {
        internal static VersionManager versionManager;
        private static string IpAddress { get; set; }

        public void UpdateConnectionType(bool isLocal)
        {
            IpAddress = isLocal ? "127.0.0.1" : "51.210.5.100";
        }

        public void OnDisconnectFromPhoton()
        {
            // PhotonServer complains about no UserId being set, temp fix
            PhotonNetwork.AuthValues = new AuthenticationValues(Guid.NewGuid().ToString());
            PhotonNetwork.ConnectToMaster(IpAddress, 5055, "", versionManager.Version);
            //PhotonNetwork.ConnectToRegion((CloudRegionCode) Region, "2021");
        }

        public void ChangeRegionDisconnect()
        {
            PhotonNetwork.Disconnect();
        }

        public void Initialize()
        {
            if (Service.Authentication.AccessToken != null)
            {
                PhotonNetwork.AuthValues = new AuthenticationValues {AuthType = CustomAuthenticationType.Custom};
                PhotonNetwork.AuthValues.AddAuthParameter("token", Service.Authentication.AccessToken);
            }
            else
            {
                // PhotonServer complains about no UserId being set, temp fix
                PhotonNetwork.AuthValues = new AuthenticationValues(Guid.NewGuid().ToString());
            }

            PhotonNetwork.ConnectToMaster(IpAddress, 5055, "", versionManager.Version);
        }
    }
}