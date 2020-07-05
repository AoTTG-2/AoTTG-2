using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using Zenject;

namespace Cannon
{
    [RequireComponent(typeof(PhotonView))]
    public sealed class CannonFacade : Photon.MonoBehaviour
    {
        private CannonRequestManager requestManager;

        private CannonStateManager stateManager;

        private void OnOwnershipRequest(object[] viewAndPlayer)
        {
            var view = viewAndPlayer[0] as PhotonView;
            var requestingPlayer = viewAndPlayer[1] as PhotonPlayer;

            if (photonView.isMine)
                photonView.TransferOwnership(requestingPlayer);
        }

        private void OnOwnershipTransfered(object[] viewAndPlayers)
        {
            var view = viewAndPlayers[0] as PhotonView;
            var newOwner = viewAndPlayers[1] as PhotonPlayer;
            var oldOwner = viewAndPlayers[2] as PhotonPlayer;
        }

        public void RequestMount(Hero hero)
        {
            photonView.RequestOwnership();
        }

        public void RequestUnmount(Hero hero)
        {
            Debug.Log(nameof(RequestUnmount));
            Assert.IsTrue(photonView.isMine);
            photonView.TransferOwnership(0);
        }
        
        [Inject]
        private void Construct(
            [InjectOptional]
            CannonRequestManager requestManager,
            CannonStateManager stateManager)
        {
            this.requestManager = requestManager;
            this.stateManager = stateManager;

            photonView.RequestOwnership();
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