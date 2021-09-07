using Assets.Scripts.Services;
using Assets.Scripts.Settings;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Scripts.DayNightCycle
{
    /// <summary>
    /// The DayLengthController class handles changing the DayLength variable of the DayAndNightControl.cs script via
    /// the DayLengthInput UI component
    /// UI component
    /// </summary>
    public class DayLengthController : MonoBehaviour
    {
        public Text Label;
        public InputField DayLengthInput;
        DayAndNightControl dayNightCycle;

        void Start()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        /// <summary>
        /// SubmitDayLength is the method used to change the DayLength in DayAndNightControl.cs via the DayLengthInput Input Field UI component
        /// </summary>
        private void SubmitDayLength(string daylength)
        {
            if (float.TryParse(daylength, out var dayLength) && PhotonNetwork.isMasterClient)
            {
                if (dayLength < 60)
                {
                    dayLength = 60;
                }
                dayNightCycle.DayLength = dayLength;
                GameSettings.Time.DayLength = dayLength;
                Service.Settings.SyncSettings();
            }
        }

        //grabbing the local scene's DayAndNightControl script, and adding a scene changed listener
        void OnEnable()
        {
            dayNightCycle = GameObject.Find("Day and Night Controller").GetComponent<DayAndNightControl>();
            if (PhotonNetwork.isMasterClient)
            {
                var se = new InputField.SubmitEvent();
                se.AddListener(SubmitDayLength);
                DayLengthInput.onEndEdit = se;
            }
            else
            {
                var se = new InputField.SubmitEvent();
                se.RemoveListener(SubmitDayLength);
            }
        }

        //When Scene changes, erase input field
        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            DayLengthInput.text = "";

        }

        void OnApplicationQuit()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }
}
