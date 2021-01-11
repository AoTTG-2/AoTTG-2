using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.DayNightCycle
{
    public class DayLengthController : MonoBehaviour
    {
        public Text Label;
        public InputField DayLengthInput;
        public Toggle ToggleDayNight;
        DayAndNightControl dayNightCycle = null;

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
                se.AddListener(SubmitDayLength);
                DayLengthInput.onEndEdit = se;
            }
        }

        private void SubmitDayLength(string arg0)
        {
            float dayLength =float.Parse(arg0);
            if (dayLength<60)
            {
                dayLength = 60;
            }
            dayNightCycle.DayLength = dayLength;
           
           
        }

        //grabbing the local scene's DayAndNightControl script
        void OnEnable()
        {
            dayNightCycle = GameObject.Find("Day and Night Controller").GetComponent<DayAndNightControl>();
        }
    }
}
