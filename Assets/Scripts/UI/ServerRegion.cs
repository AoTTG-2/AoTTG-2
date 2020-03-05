using System;
using System.Diagnostics;

namespace Assets.Scripts.UI
{
    public class ServerRegion : UiElement
    {
        public void JoinRegion(string region)
        {
            var cloudRegion = (CloudRegionCode) Enum.Parse(typeof(CloudRegionCode), region.ToLower());
            if (PhotonNetwork.ConnectToRegion(cloudRegion, "2020"))
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
