
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
        DayAndNightControl DayNightCycle;
       void Start()
        {
            
            ToggleDayNight = GameObject.Find("ToggleDayNightCycle").GetComponent<Toggle>();
            DayNightCycle = GameObject.Find("Day and Night Controller").GetComponent<DayAndNightControl>();
        }

 
        void Update()
        {
            if (ToggleDayNight.isOn)
            {
                var se = new InputField.SubmitEvent();
                se.AddListener(SubmitTime);
                TimeInput.onEndEdit = se;
                DayNightCycle.currentTime = TimeSlider.value * 24;
            }
            
        }
        
        private void SubmitTime(string arg0)
        {
            string time = arg0;
            double seconds = System.TimeSpan.Parse(time).TotalSeconds;
            TimeSlider.value= (float) (seconds / 86400);
            DayNightCycle.currentTime = (float) (seconds/86400);
              
        }
        
    }
   

}

