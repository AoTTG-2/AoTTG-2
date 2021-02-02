using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.Services;
using Assets.Scripts.Settings;
using UnityEngine.SceneManagement;
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

        //grabbing the local scene's DayAndNightControl script, and adding a scene changed listener
        void OnEnable()
        {
            dayNightCycle = GameObject.Find("Day and Night Controller").GetComponent<DayAndNightControl>();
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        //When Scene changes, erase input field
        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            DayLengthInput.text = "";
        }

    }
}
