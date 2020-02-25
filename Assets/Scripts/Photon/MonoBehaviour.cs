namespace Photon
{
    using UnityEngine;

    public class MonoBehaviour : UnityEngine.MonoBehaviour
    {
        public new PhotonView networkView
        {
            get
            {
                Debug.LogWarning("Why are you still using networkView? should be PhotonView?");
                return PhotonView.Get(this);
            }
        }

        public PhotonView photonView
        {
            get
            {
                return PhotonView.Get(this);
            }
        }
    }
}

