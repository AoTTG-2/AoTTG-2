using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.InGame
{
    public class TimeSwitcher : MonoBehaviour
    {

        public Text Label;
        public Slider TimeSlider;
        public int sValue;
        public GameObject TimeSliderGet;
        
        DayAndNightControl DayNightCycle;
       void Start()
        {
            DayNightCycle = GameObject.Find("Day and Night Controller(Clone)").GetComponent<DayAndNightControl>();
        }

        void Update()
        {
            Debug.Log(TimeSlider.value);
            DayNightCycle.currentTime = TimeSlider.value;
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
