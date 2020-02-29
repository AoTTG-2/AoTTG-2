using System;
using System.Diagnostics;

namespace Assets.Scripts.UI
{
    public class ServerRegion : UiElement
    {
        public void JoinRegion(string region)
        {
            var cloudRegion = (CloudRegionCode) Enum.Parse(typeof(CloudRegionCode), region.ToLower());
            if (PhotonNetwork.ConnectToRegion(cloudRegion, UIMainReferences.version))
            {
                // Succeeded    
                Navigate(typeof(ServerList));
            }
            else
            {
                // Rip
            }
        }
    }
}
