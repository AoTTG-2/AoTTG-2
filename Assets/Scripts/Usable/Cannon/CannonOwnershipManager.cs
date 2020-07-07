namespace Cannon
{
    internal sealed class CannonOwnershipManager : ICannonOwnershipManager
    {
        private readonly PhotonView photonView;

        public CannonOwnershipManager(
            PhotonView photonView)
        {
            this.photonView = photonView;
        }

        public bool AllowOwnershipTransfer { get; set; }
        
        public event OwnershipChangedHandler LocalOwnershipTaken;
        public event OwnershipChangedHandler RemoteOwnershipTaken;
        public event OwnershipChangedHandler WorldOwnershipTaken;

        void ICannonOwnershipManager.RequestOwnership()
        {
            photonView.RequestOwnership();
        }

        void ICannonOwnershipManager.RelinquishOwnership()
        {
            photonView.TransferOwnership(0);
        }

        public void OnOwnershipTransferred(PhotonPlayer newOwner, PhotonPlayer player)
        {
            if (ReferenceEquals(newOwner, player))
                LocalOwnershipTaken?.Invoke();
            else if (ReferenceEquals(newOwner, null))
                WorldOwnershipTaken?.Invoke();
            else
                RemoteOwnershipTaken?.Invoke();
        }

        public void OnOwnerShipRequest(PhotonPlayer requestingPlayer)
        {
            if (photonView.isMine && AllowOwnershipTransfer)
                photonView.TransferOwnership(requestingPlayer);
        }
    }
}