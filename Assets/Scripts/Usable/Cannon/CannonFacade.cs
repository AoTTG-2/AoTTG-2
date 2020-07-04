using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Cannon
{
    [RequireComponent(typeof(PhotonView))]
    public sealed class CannonFacade : Photon.MonoBehaviour
    {
        private CannonRequestManager requestManager;

        private CannonStateManager stateManager;

        public Hero Hero { get; private set; }

        /// <summary>
        /// Called on all clients by <paramref name="hero"/>.
        /// </summary>
        public void Mount(Hero hero)
        {
            // TODO: Set up the cannon mounting properly on all clients.
            Hero = hero;
        }

        /// <summary>
        /// Called locally by <see cref="Interactable.Interacted"/>.
        /// </summary>
        public void RequestMount(Hero hero)
        {
            var viewID = hero.photonView.viewID;
            photonView.RPC(nameof(RequestMountRPC), PhotonTargets.MasterClient, viewID);
        }

        [Inject]
        private void Construct(
            [InjectOptional]
            CannonRequestManager requestManager,
            CannonStateManager stateManager)
        {
            this.requestManager = requestManager;
            this.stateManager = stateManager;
        }

        /// <summary>
        /// Called locally by MasterClient.
        /// </summary>
        [PunRPC]
        private void OnRequestAcceptedRPC(int ownerID, PhotonMessageInfo info)
        {
            Debug.Assert(info.sender.IsMasterClient, $"Only MasterClient may call {nameof(OnRequestAcceptedRPC)}.");

            var owner = PhotonView.Find(ownerID).GetComponent<Hero>();
            owner.OnMountingCannon();
        }

        /// <summary>
        /// Called on MasterClient.
        /// </summary>
        [PunRPC]
        private void RequestMountRPC(int viewID, PhotonMessageInfo info)
        {
            Debug.Log(nameof(RequestMountRPC));

            Debug.Assert(PhotonNetwork.isMasterClient, $"{nameof(RequestMountRPC)} can only be called on MasterClient.");
            Debug.Assert(photonView.isMine, $"MasterClient should own the {nameof(CannonFacade)} when {nameof(RequestMountRPC)} is called.");

            var requestingHero = PhotonView.Find(viewID).gameObject.GetComponent<Hero>();
            Debug.Assert(requestingHero.photonView.owner == info.sender, "Hero owner and RPC sender must match up.");

            var requestAccepted = requestManager.TryRequest(info.sender.ID, photonView.viewID);
            Debug.Assert(requestAccepted, "Cannon request was denied.");

            if (requestAccepted)
            {
                photonView.TransferOwnership(info.sender.ID);
                photonView.RPC(nameof(OnRequestAcceptedRPC), info.sender, requestingHero.photonView.viewID);
            }
        }

        private void Reset()
        {
            // Yes, this is null when the component is first added.
            if (photonView.ObservedComponents == null)
                photonView.ObservedComponents = new List<Component>();

            photonView.ObservedComponents.Remove(this);
            photonView.ObservedComponents.Add(this);

            photonView.ownershipTransfer = OwnershipOption.Fixed;
        }

        private void Start()
        {
            stateManager.Transition<UnmannedCannonState>();
        }
    }
}