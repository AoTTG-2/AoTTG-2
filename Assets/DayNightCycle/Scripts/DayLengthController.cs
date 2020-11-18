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
            
            
        }

        void Update()
        {
            DayNightCycle = GameObject.Find("Day and Night Controller(Clone)").GetComponent<DayAndNightControl>();
            DayLengthInput = gameObject.GetComponent<InputField>();
            var se = new InputField.SubmitEvent();
            se.AddListener(SubmitDayLength);
            DayLengthInput.onEndEdit = se;

        }

        private void SubmitDayLength(string arg0)
        {
            dayLength=float.Parse(arg0);
            DayNightCycle.DayLength = (float) dayLength;
            Debug.Log(DayNightCycle.DayLength);
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
