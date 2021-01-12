using Assets.Scripts.Services;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.Settings;
using System.Collections;
namespace Assets.Scripts.DayNightCycle
{
    public class TimeSwitcher : MonoBehaviour
    {

        public Text Label;
        public Slider TimeSlider;
        public InputField TimeInput;
        public Toggle ToggleDayNight;
        private int x = 0;
        DayAndNightControl dayNightCycle;
       void Start()
        {
            
            ToggleDayNight = GameObject.Find("ToggleDayNightCycle").GetComponent<Toggle>();
            dayNightCycle = GameObject.Find("Day and Night Controller").GetComponent<DayAndNightControl>();
            TimeSlider.onValueChanged.AddListener (delegate {ValueChangeCheck ();});
        }

        public void ValueChangeCheck()
	    {
            if (PhotonNetwork.isMasterClient)
            {
                 dayNightCycle.currentTime = TimeSlider.value * 24;
                 GameSettings.Time.currentTime = dayNightCycle.currentTime;
		         Service.Settings.SyncSettings();
            }
	    }
        void Update()
        {   
            if (PhotonNetwork.isMasterClient)
            {
                var se = new InputField.SubmitEvent();
                se.AddListener(SubmitTime);
                TimeInput.onEndEdit = se;
            }
        }
        
        private void SubmitTime(string arg0)
        {
            string time = arg0;
            if (time.Contains(":"))
                {
                 try 
                    {
                    double seconds = System.TimeSpan.Parse(time).TotalSeconds;
                    TimeSlider.value= (float) (seconds / 86400);
                    dayNightCycle.currentTime = (float) (seconds/86400);
                    GameSettings.Time.currentTime = dayNightCycle.currentTime;
                    Service.Settings.SyncSettings();

                    }
                 catch 
                    {
                     
                    }
                }
            
              
        }

        //grabbing the local scene's DayAndNightControl script
        void OnEnable()
        {
            dayNightCycle = GameObject.Find("Day and Night Controller").GetComponent<DayAndNightControl>();
            TimeSlider.value = dayNightCycle.CurrentTime01;
            x=0;
        }

  
    }
   

}

