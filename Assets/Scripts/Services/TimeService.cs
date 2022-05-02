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

        private DateTime PausedStartTime { get; set; }
        private TimeSpan TotalPausedTime { get; set; }

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

        public float GetUnPausedRoundTime()
        {
            return GetRoundTime() - GetTotalPausedTime();
        }

        public int GetUnPausedRoundDisplayTime()
        {
            return (int) Mathf.Floor(GetUnPausedRoundTime());
        }
        public override void OnCreatedRoom()
        {
            CreationTime = DateTime.UtcNow;
        }

        public float GetTotalPausedTime()
        {
            return (float) TotalPausedTime.TotalSeconds;
        }

        private void ResetTotalPausedTime()
        {
            PausedStartTime = DateTime.MinValue;
            TotalPausedTime = TimeSpan.Zero;
        }

        /// <summary>
        /// Occurs on OnPaused
        /// </summary>
        private void SetPausedStartTime(object sender, EventArgs e)
        {
            PausedStartTime = DateTime.UtcNow;
        }

        /// <summary>
        /// Occurs on OnUnPaused. Calculates the time paused and adds it to the total.
        /// </summary>
        private void SetTotalPausedTime(object sender, EventArgs e)
        {
            TotalPausedTime = TotalPausedTime.Add(DateTime.UtcNow.Subtract(PausedStartTime));
        }
        private void Awake()
        {
            //Subscribes to OnPaused and OnUnPaused events
            Service.Pause.OnPaused += SetPausedStartTime;
            Service.Pause.OnUnPaused += SetTotalPausedTime;
        }
        private void OnLevelWasLoaded()
        {
            RoundTime = DateTime.UtcNow;
            ResetTotalPausedTime();
        }
    }
}
