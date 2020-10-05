using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.InGame
{
    public class TimeSwitcher : MonoBehaviour
    {
        public DayAndNightControl DayAndNightControl;

        public Text Label;
        public Slider Slider;

        public void Awake()
        {
            var dayNightObject = GameObject.Find("DayNightSystem");
            DayAndNightControl = dayNightObject.GetComponent<DayAndNightControl>();
        }

        public void UpdateTime()
        {
            int sValue = (int)Slider.value;
<<<<<<< HEAD
            DayAndNightControl.timeMultiplier = sValue;
=======
           // DayAndNightControl.timeMultiplier = sValue;

>>>>>>> 9727c57904a93c2692494582a681a2a6cf0a16f9
        }

        public struct TimeData
        {
            public int Slider;

            public TimeData(TimeSwitcher toCopy)
            {
                this.Slider = (int)toCopy.Slider.value;
            }
        }
    }
}
