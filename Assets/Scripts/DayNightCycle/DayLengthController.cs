using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.Services;
using Assets.Scripts.Settings;
namespace Assets.Scripts.DayNightCycle
{
    public class DayLengthController : MonoBehaviour
    {
        public Text Label;
        public InputField DayLengthInput;
        DayAndNightControl dayNightCycle;

        void Start()
        {
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

        private void SubmitDayLength(string daylength)
        {
                    if (float.TryParse(daylength, out var dayLength))
                    {
                        if (dayLength < 60)
                        {
                            dayLength = 60;
                        }
                        dayNightCycle.DayLength = dayLength;
                        GameSettings.Time.dayLength = dayLength;
                        Service.Settings.SyncSettings();
                    }      
        }

        //grabbing the local scene's DayAndNightControl script
        void OnEnable()
        {
            dayNightCycle = GameObject.Find("Day and Night Controller").GetComponent<DayAndNightControl>();
        }
    }
}
