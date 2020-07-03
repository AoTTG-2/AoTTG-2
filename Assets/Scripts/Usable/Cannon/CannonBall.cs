using UnityEngine;
using Zenject;

namespace Cannon
{
    [RequireComponent(typeof(SphereCollider), typeof(Rigidbody), typeof(PhotonView))]
    internal sealed class CannonBall : Photon.MonoBehaviour
    {
        private CannonFacade owner;

        [Inject]
        private void Construct(CannonFacade owner, Vector3 velocity)
        {
            this.owner = owner;
            GetComponent<Rigidbody>().velocity = velocity;
        }

        private void OnDestroy()
        {
        }

        private void Start()
        {
        }

        public sealed class Factory : PlaceholderFactory<CannonFacade, Vector3, Vector3, Quaternion, byte, CannonBall>
        {
        }
    }

    
}