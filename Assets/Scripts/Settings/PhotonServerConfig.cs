using UnityEngine;

namespace Assets.Scripts.Settings
{
    [CreateAssetMenu(fileName = "PhotonServerConfig", menuName = "ScriptableObjects/PhotonServerConfig", order = 1)]
    public class PhotonServerConfig : ScriptableObject
    {
        [Tooltip("This display name of this Photon Server")]
        public string Name;
        [Tooltip("This IPv4 / IPv6 address of this Photon Server")]
        public string IpAddress;
        [Tooltip("The port of this Photon Server. Default 5055")]
        public int Port = 5055;
    }
}
