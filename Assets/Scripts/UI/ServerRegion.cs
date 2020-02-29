using System.Diagnostics;

namespace Assets.Scripts.UI
{
    public class ServerRegion : UiElement
    {
        public void JoinRegion(int region)
        {
            var cloudRegion = (CloudRegionCode) region;
            if (PhotonNetwork.ConnectToMaster(cloudRegion))
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
