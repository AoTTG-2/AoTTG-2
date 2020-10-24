using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.InGame
{
    public class TimeSwitcher : MonoBehaviour
    {

        public Text Label;
        public Slider Slider;
      
        DayAndNightControl DayNightCycle;
       /* public void Start()
        {
            DayNightCycle = GameObject.Find("DayNightCycle").GetComponent<DayAndNightControl>();
        }*/
        public void UpdateTime()
        {
            DayNightCycle = GameObject.Find("DayNightCycle").GetComponent<DayAndNightControl>();

            int sValue = (int)Slider.value;
            Debug.Log(sValue);

            DayNightCycle.timeMultiplier = sValue;


        }

        public struct TimeData
        {
            public int Slider;

            public TimeData(TimeSwitcher toCopy)
            {
                this.Slider = (int)toCopy.Slider.value;
                //Debug.Log(this.Slider);
            }
        }
    }
}
