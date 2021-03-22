using Assets.Scripts.Services;
using Assets.Scripts.Settings;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.DayNightCycle
{
        /// <summary>
        /// The ToggleDayNightController class handles the play/pause control of the DayAndNightControl script via a ToggleDayNight
        /// UI component
        /// </summary>
    public class ToggleDayNightController : MonoBehaviour
    {
        public Toggle ToggleDayNight;
        public GameObject DayNightController;
        public GameObject MainLight;
        public Text Label;
        DayAndNightControl dayNightCycle;

        void Start()
        {
            ToggleDayNight.isOn = false;
            ToggleDayNight.onValueChanged.AddListener(delegate {
                ToggleValueChanged(ToggleDayNight);
            });

        }

        void ToggleValueChanged(Toggle change)
        {
            if (PhotonNetwork.isMasterClient)
            {
                dayNightCycle.Pause = !change.isOn;
                GameSettings.Time.Pause = !change.isOn;
                Service.Settings.SyncSettings();
            }

        }
        void OnEnable()
        {
            dayNightCycle = GameObject.Find("Day and Night Controller").GetComponent<DayAndNightControl>();
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        
        //on scene change, reset the toggle + unsubscribe from scene listener event
        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            ToggleDayNight.isOn = false;
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }
}

