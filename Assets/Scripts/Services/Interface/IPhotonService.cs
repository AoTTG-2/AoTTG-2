using Assets.Scripts.Settings;
using System.Collections.Generic;

namespace Assets.Scripts.Services.Interface
{
    public interface IPhotonService
    {
        /// <summary>
        /// Returns the current Photon Server Configuration
        /// </summary>
        PhotonServerConfig GetCurrentConfig();

        /// <summary>
        /// Returns a photon server configuration based on the <paramref name="configName"/>
        /// </summary>
        /// <param name="configName">An existing <see cref="PhotonServerConfig.Name"/></param>
        PhotonServerConfig GetConfigByName(string configName);

        /// <summary>
        /// Changes the current Photon Server to <paramref name="server"/>
        /// </summary>
        /// <param name="server">The new Photon Server configuration</param>
        void ChangePhotonServer(PhotonServerConfig server);

        /// <summary>
        /// Returns a list of all current registered Photon Servers
        /// </summary>
        List<PhotonServerConfig> GetAllServers();

        /// <summary>
        /// Returns a boolean, indicating if the <see cref="IPhotonService"/> is changing a region.
        /// Classes which implement <see cref="Photon.PunBehaviour.OnDisconnectedFromPhoton"/> should use this.
        /// </summary>
        /// <returns></returns>
        bool IsChangingRegion();

        /// <summary>
        /// Connects to the Photon Server based on the current PhotonServerConfiguration <see cref="GetCurrentConfig"/>
        /// </summary>
        void Connect();
    }
}