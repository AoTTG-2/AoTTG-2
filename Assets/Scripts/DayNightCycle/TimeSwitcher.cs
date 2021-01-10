
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.DayNightCycle
{
    public class TimeSwitcher : MonoBehaviour
    {

        public Text Label;
        public Slider TimeSlider;
        public InputField TimeInput;
        public Toggle ToggleDayNight;
        DayAndNightControl dayNightCycle;
       void Start()
        {
            
            ToggleDayNight = GameObject.Find("ToggleDayNightCycle").GetComponent<Toggle>();
            dayNightCycle = GameObject.Find("Day and Night Controller").GetComponent<DayAndNightControl>();
        }

 
        void Update()
        {   
            if (PhotonNetwork.isMasterClient)
            {
                var se = new InputField.SubmitEvent();
                se.AddListener(SubmitTime);
                TimeInput.onEndEdit = se;
                dayNightCycle.currentTime = TimeSlider.value * 24;
            }
        }
        
        private void SubmitTime(string arg0)
        {
            string time = arg0;
            double seconds = System.TimeSpan.Parse(time).TotalSeconds;
            TimeSlider.value= (float) (seconds / 86400);
            dayNightCycle.currentTime = (float) (seconds/86400);
              
        }

        //grabbing the local scene's DayAndNightControl script
        void OnEnable()
        {
            dayNightCycle = GameObject.Find("Day and Night Controller").GetComponent<DayAndNightControl>();
        }

  
    }
   

}

