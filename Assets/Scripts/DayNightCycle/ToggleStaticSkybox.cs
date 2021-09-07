using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Scripts.DayNightCycle
{
    /// <summary>
    /// UI class to enable / disable the static skybox functionality
    /// </summary>
    public class ToggleStaticSkybox : MonoBehaviour
    {
        public Toggle ToggleStatic;
        public Text Label;
        DayAndNightControl dayNightCycle;

        private void Start()
        {
            ToggleStatic.onValueChanged.AddListener(delegate
            {
                ToggleValueChanged(ToggleStatic);
            });
            SceneManager.sceneLoaded += OnSceneLoaded;
            ToggleStatic.isOn = PlayerPrefs.GetInt(dayNightCycle.StaticSkyboxPlayerPref) == 1;
        }

        private void ToggleValueChanged(Toggle change)
        {
            if (PhotonNetwork.isMasterClient)
            {
                dayNightCycle.StaticSkybox = change.isOn;
            }

        }
        private void OnEnable()
        {
            dayNightCycle = GameObject.Find("Day and Night Controller").GetComponent<DayAndNightControl>();
        }

        //on scene change unsubscribe from scene listener event
        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            dayNightCycle = GameObject.Find("Day and Night Controller").GetComponent<DayAndNightControl>();
            dayNightCycle.StaticSkybox = ToggleStatic.isOn;
            PlayerPrefs.SetInt(dayNightCycle.StaticSkyboxPlayerPref, ToggleStatic.isOn ? 1 : 0);
            PlayerPrefs.Save();
        }

        private void OnApplicationQuit()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }
}

