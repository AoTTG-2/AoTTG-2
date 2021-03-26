using Assets.Scripts.Services;
using Assets.Scripts.Settings;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.DayNightCycle
{
    public class ToggleStaticSkybox : MonoBehaviour
    {
        public Toggle ToggleStatic;
        public GameObject DayNightController;
        public Text Label;
        DayAndNightControl dayNightCycle;

        void Start()
        {
            ToggleStatic.onValueChanged.AddListener(delegate
            {
                ToggleValueChanged(ToggleStatic);
            });

        }

        void ToggleValueChanged(Toggle change)
        {
            if (PhotonNetwork.isMasterClient)
            {
                dayNightCycle.Static = !dayNightCycle.Static;
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
            ToggleStatic.isOn = false;
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }
}

