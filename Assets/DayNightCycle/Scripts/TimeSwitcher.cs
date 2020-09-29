﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.InGame
{
    public class TimeSwitcher : MonoBehaviour
    {
        public Text Label;
        public Slider Slider;

        public void UpdateTime()
        {
            int sValue = (int)Slider.value;
            DayAndNightControl.timeMultiplier = sValue;

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