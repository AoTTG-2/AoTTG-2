namespace Cannon
{
    internal delegate void OwnershipChangedHandler();
    
    internal interface ICannonOwnershipManager
    {
        bool AllowOwnershipTransfer { get; set; }
        
        event OwnershipChangedHandler LocalOwnershipTaken;
        event OwnershipChangedHandler RemoteOwnershipTaken;
        event OwnershipChangedHandler WorldOwnershipTaken;

        void RequestOwnership();
        void RelinquishOwnership();
    }
}