
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.InGame
{
    public class TimeSwitcher : MonoBehaviour
    {

        public Text Label;
        public Slider TimeSlider;
        public InputField TimeInput;
        DayAndNightControl DayNightCycle;
       void Start()
        {
            
            DayNightCycle = GameObject.Find("Day and Night Controller(Clone)").GetComponent<DayAndNightControl>();
            
            
        }

        void Update()
        {
            
            var se = new InputField.SubmitEvent();
            se.AddListener(SubmitTime);
            TimeInput.onEndEdit = se;
            
            DayNightCycle = GameObject.Find("Day and Night Controller(Clone)").GetComponent<DayAndNightControl>();
            DayNightCycle.currentTime = TimeSlider.value;
            
        }
        
        private void SubmitTime(string arg0)
        {

            string time = arg0;
            double seconds = System.TimeSpan.Parse(time).TotalSeconds;
            TimeSlider.value= (float) (seconds / 86400);
            DayNightCycle.currentTime = (float) (seconds/86400);
            Debug.Log((float) (seconds / 86400));
        }
        
    }
   

}

