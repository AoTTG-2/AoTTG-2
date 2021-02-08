using Assets.Scripts.Services;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.Settings;
using UnityEngine.EventSystems;
using System;
using UnityEngine.SceneManagement;
namespace Assets.Scripts.DayNightCycle
{
    public class TimeSwitcher : MonoBehaviour, IEndDragHandler, IDragHandler
    {
        public Text Label;
        public Slider TimeSlider;
        public InputField TimeInput;
        DayAndNightControl dayNightCycle;

       void Start()
       { 
            dayNightCycle = GameObject.Find("Day and Night Controller").GetComponent<DayAndNightControl>();
            if (PhotonNetwork.isMasterClient)
            {
                var se = new InputField.SubmitEvent();
                se.AddListener(SubmitTime);
                TimeInput.onEndEdit = se;
            }
        }


        public void OnDrag(PointerEventData eventData)
        {
            if (PhotonNetwork.isMasterClient)
            {
                dayNightCycle.currentTime = TimeSlider.value * 24;
                GameSettings.Time.currentTime = dayNightCycle.currentTime;
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (PhotonNetwork.isMasterClient && PhotonNetwork.connected)
            {
                dayNightCycle.currentTime = TimeSlider.value * 24;
                GameSettings.Time.currentTime = dayNightCycle.currentTime;
                Service.Settings.SyncSettings();
            }
        }

        void Update()
        {
            TimeSlider.value = dayNightCycle.CurrentTime01;
        }

        private void SubmitTime(string time)
        {
            //only inputs with ":" and no negatives allowed, "-12:00" is an easter egg
            if (TimeSpan.TryParse(time, out var timeSpan) && time.Contains(":") &&!time.Contains("-") || time.Contains("-12:00"))
            {
                double seconds = timeSpan.TotalSeconds;
                TimeSlider.value = (float)(seconds / 86400);
                dayNightCycle.currentTime = (float)(24 * seconds / 86400);
                GameSettings.Time.currentTime = dayNightCycle.currentTime;
                Service.Settings.SyncSettings();
            }
        }




        //grabbing the local scene's DayAndNightControl script, and adding a scene changed listener
        void OnEnable()
        {
            dayNightCycle = GameObject.Find("Day and Night Controller").GetComponent<DayAndNightControl>();
            TimeSlider.value = dayNightCycle.CurrentTime01;
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        //When Scene changes, erase input field
        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            TimeInput.text = "";
        }

}
   

}

