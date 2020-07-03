using System.Collections.Generic;

namespace Cannon
{
    internal sealed class CannonRequestManager
    {
        private readonly Dictionary<int, int> cannonIDByRequesterID = new Dictionary<int, int>();

        public bool TryAcceptRequest(int requesterID)
        {
            if (!cannonIDByRequesterID.ContainsKey(requesterID))
                return false;

            return cannonIDByRequesterID.Remove(requesterID); ;
        }

        public bool TryRequest(int requesterID, int cannonID)
        {
            if (cannonIDByRequesterID.ContainsKey(requesterID))
                return false;

            cannonIDByRequesterID.Add(requesterID, cannonID);
            return true;
        }
    }
}