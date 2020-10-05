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

        public void UpdateTime()
        {
            int sValue = (int)Slider.value;
<<<<<<< HEAD
<
            DayAndNightControl.timeMultiplier = sValue;
=======
           // DayAndNightControl.timeMultiplier = sValue;
>>>>>>> a0297d53b7a2600022dfae0ef984de60e3f23d90

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
