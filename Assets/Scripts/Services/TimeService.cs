using Assets.Scripts.Services.Interface;
using Photon;
using System;

namespace Assets.Scripts.Services
{
    public class TimeService : PunBehaviour, ITimeService
    {
        private DateTime CreationTime { get; set; }
        private DateTime RoundTime { get; set; }

        public float GetRoomTime()
        {
            return (float) DateTime.UtcNow.Subtract(CreationTime).TotalSeconds;
        }

        public float GetRoundTime()
        {
            return (float) DateTime.UtcNow.Subtract(RoundTime).TotalSeconds;
        }

        public override void OnCreatedRoom()
        {
            CreationTime = DateTime.UtcNow;
        }

        private void OnLevelWasLoaded()
        {
            RoundTime = DateTime.UtcNow;
        }
    }
}
