using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.InGame
{
    public class DayLengthController : MonoBehaviour
    {

        public Text Label;

        public InputField DayLengthInput;
        private string time;
        private double dayLength;


        DayAndNightControl DayNightCycle;
        void Start()
        {
            DayNightCycle = GameObject.Find("Day and Night Controller(Clone)").GetComponent<DayAndNightControl>();
            var input = gameObject.GetComponent<InputField>();
            var se = new InputField.SubmitEvent();
            se.AddListener(SubmitTime);
            input.onEndEdit = se;
            
        }

        void Update()
        {

            Debug.Log(DayNightCycle.SecondsInAFullDay);
           // DayNightCycle.currentTime = TimeSlider.value;
        }

        private void SubmitTime(string arg0)
        {
           // Debug.Log(arg0);
            
            dayLength=float.Parse(arg0);
            
            DayNightCycle.SecondsInAfullDay = (float) dayLength;
            Debug.Log(dayLength);
        }

        //here put functions that u wanna call from the change button at ServerSettingsPage.cs, I left
        //comments in ServerSettingsPage.cs that calls the below example function
        /* public void UpdateTime()
         {
             Debug.Log(TimeSlider.value);
             DayNightCycle.currentTime = TimeSlider.value;


         }*/

    }
}
