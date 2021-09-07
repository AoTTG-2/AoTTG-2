using Assets.Scripts.Services;
using Assets.Scripts.Settings;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Scripts.DayNightCycle
{
    /// <summary>
    /// The TimeSwitcher class handles changing the time of DayAndNightControl.cs via the TimeSLider and  TimeInput Inputfield
    /// UI components
    /// </summary>
    public class TimeSwitcher : MonoBehaviour, IEndDragHandler, IDragHandler
    {
        public Text Label;
        public Slider TimeSlider;
        public InputField TimeInput;
        DayAndNightControl dayNightCycle;


        public void OnDrag(PointerEventData eventData)
        {
            if (PhotonNetwork.isMasterClient)
            {
                dayNightCycle.CurrentTime = TimeSlider.value * 24;
                GameSettings.Time.CurrentTime = dayNightCycle.CurrentTime;
            }
        }

        /// <summary>
        /// Seettings are only synched OnEndDrag of TimeSlider to minimize network usage
        /// </summary>
        public void OnEndDrag(PointerEventData eventData)
        {
            if (PhotonNetwork.isMasterClient && PhotonNetwork.connected)
            {
                dayNightCycle.CurrentTime = TimeSlider.value * 24;
                GameSettings.Time.CurrentTime = dayNightCycle.CurrentTime;
                Service.Settings.SyncSettings();
            }
        }
        void Start()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        void Update()
        {
            TimeSlider.value = dayNightCycle.CurrentTimeScale;
        }

        /// <summary>
        /// SubmitTime is the method used to change the CurrentTime of DayAndNightControl.cs via the TimeInput Input Field UI component
        /// </summary>
        private void SubmitTime(string time)
        {

            //only inputs with ":" and no negatives allowed, "-12:00" is an easter egg
            if (PhotonNetwork.isMasterClient && TimeSpan.TryParse(time, out var timeSpan) && time.Contains(":") && !time.Contains("-") || time.Contains("-12:00"))
            {
                double seconds = timeSpan.TotalSeconds;
                TimeSlider.value = (float) (seconds / 86400);
                dayNightCycle.CurrentTime = (float) (24 * seconds / 86400);
                GameSettings.Time.CurrentTime = dayNightCycle.CurrentTime;
                Service.Settings.SyncSettings();
            }
        }




        //grabbing the local scene's DayAndNightControl script, and adding a scene changed listener
        void OnEnable()
        {
            dayNightCycle = GameObject.Find("Day and Night Controller").GetComponent<DayAndNightControl>();
            TimeSlider.value = dayNightCycle.CurrentTimeScale;

            if (PhotonNetwork.isMasterClient)
            {
                var se = new InputField.SubmitEvent();
                se.AddListener(SubmitTime);
                TimeInput.onEndEdit = se;
            }
            else
            {
                var se = new InputField.SubmitEvent();
                se.RemoveListener(SubmitTime);
            }
        }

        //When Scene changes, erase input field
        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {

            TimeInput.text = "";
        }
        void OnApplicationQuit()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

    }


}

