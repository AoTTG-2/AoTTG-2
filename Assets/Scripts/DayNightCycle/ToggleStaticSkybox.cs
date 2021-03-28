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
        public Text Label;
        DayAndNightControl dayNightCycle;

        void Start()
        {
            ToggleStatic.onValueChanged.AddListener(delegate
            {
                ToggleValueChanged(ToggleStatic);
            });
            SceneManager.sceneLoaded += OnSceneLoaded;
            ToggleStatic.isOn = PlayerPrefs.GetInt("StaticSkybox") == 1 ? true : false;
        }

        void ToggleValueChanged(Toggle change)
        {
            if (PhotonNetwork.isMasterClient)
            {
                dayNightCycle.StaticSkybox = ToggleStatic.isOn;
                Debug.Log("static change");
            }

        }
        void OnEnable()
        {
            dayNightCycle = GameObject.Find("Day and Night Controller").GetComponent<DayAndNightControl>();
        }

        //on scene change unsubscribe from scene listener event
        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            dayNightCycle = GameObject.Find("Day and Night Controller").GetComponent<DayAndNightControl>();
            dayNightCycle.StaticSkybox = ToggleStatic.isOn;
            PlayerPrefs.SetInt("StaticSkybox", ToggleStatic.isOn ? 1 : 0);
            PlayerPrefs.Save();
        }

        void OnApplicationQuit()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }
}

