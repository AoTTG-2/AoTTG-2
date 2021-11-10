using UnityEngine;

namespace Assets.Scripts.Settings
{
    /// <summary>
    /// Contains configuration settings for a PhotonServer
    /// </summary>
    [CreateAssetMenu(fileName = "PhotonServerConfig", menuName = "ScriptableObjects/PhotonServerConfig", order = 1)]
    public class PhotonServerConfig : ScriptableObject
    {
        [Tooltip("The image that will be displayed when a button is overlayed")]
        public Texture2D DisplayImage;
        [Tooltip("The display name of this Photon Server")]
        public string Name;
        [Tooltip("This IPv4 / IPv6 address of this Photon Server")]
        public string IpAddress;
        [Tooltip("The port of this Photon Server. Default 5055")]
        public int Port = 5055;
    }
}
