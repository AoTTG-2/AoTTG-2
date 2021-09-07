using Assets.Scripts.Services.Interface;
using Photon;
using System;
using UnityEngine;
using Assets.Scripts.DayNightCycle;
namespace Assets.Scripts.Services
{
    public class TimeService : PunBehaviour, ITimeService
    {
        private DateTime CreationTime { get; set; }
        private DateTime RoundTime { get; set; }
        DayAndNightControl dayNightCycle;
        public float GetRoomTime()
        {
            return (float) DateTime.UtcNow.Subtract(CreationTime).TotalSeconds;
        }

        public float GetRoundTime()
        {
            return (float) DateTime.UtcNow.Subtract(RoundTime).TotalSeconds;
        }

        public int GetRoundDisplayTime()
        {
            return (int) Mathf.Floor(GetRoundTime());
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
