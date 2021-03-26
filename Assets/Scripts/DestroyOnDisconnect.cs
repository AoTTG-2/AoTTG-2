using Photon;

namespace Assets.Scripts.Utility
{
    public class DestroyOnDisconnect : PunBehaviour
    {
        public override void OnDisconnectedFromPhoton()
        {
            Destroy(this.gameObject);
        }
    }
}
