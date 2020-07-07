using UnityEngine;
using Zenject;

namespace Cannon
{
    [RequireComponent(typeof(PhotonView))]
    public sealed class CannonFacade : Photon.MonoBehaviour
    {
        private CannonStateManager stateManager;
        private CannonOwnershipManager ownershipManager;

        private void OnOwnershipRequest(object[] viewAndPlayer)
        {
            var view = viewAndPlayer[0] as PhotonView;
            var requestingPlayer = viewAndPlayer[1] as PhotonPlayer;

            if (photonView != view)
                return;
            
            ownershipManager.OnOwnerShipRequest(requestingPlayer);
        }

        private void OnOwnershipTransfered(object[] viewAndPlayers)
        {
            var view = viewAndPlayers[0] as PhotonView;
            var newOwner = viewAndPlayers[1] as PhotonPlayer;

            if (photonView != view)
                return;

            ownershipManager.OnOwnershipTransferred(newOwner, PhotonNetwork.player);
        }

        [Inject]
        private void Construct(
            CannonStateManager stateManager,
            CannonOwnershipManager ownershipManager)
        {
            this.stateManager = stateManager;
            this.ownershipManager = ownershipManager;
        }

        private void Reset()
        {
            photonView.ownershipTransfer = OwnershipOption.Request;
        }

        private void Start()
        {
            stateManager.Transition<UnmannedCannonState>();
        }
    }
}