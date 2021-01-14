using Assets.Scripts.Services;
using Assets.Scripts.Settings;
using UnityEngine;
using UnityEngine.UI;


namespace Assets.Scripts.DayNightCycle
{
    public class ToggleDayNightController : MonoBehaviour
    {
        public Color DefaultSkyColor;
        public Color DefaultEquatorColor;
        public Color DefaultHorizonColor;
        public GameObject DayNightControllerPrefab = null;
        public Toggle ToggleDayNight;
        public Button PauseDayNightButton;
        public GameObject DayNightController;
        public GameObject MainLight;
        public Text Label;
        public Material skyBoxReset;
        private string time;
        private double seconds;

        DayAndNightControl dayNightCycle;
        public GameObject DefaultLightSet;
        // Start is called before the first frame update
        void Start()
        {
            DefaultLightSet = GameObject.Find("LightSet");
            //These defaults are stored so that when the system is toggled off, all colour settings are set back to the scene defaults
            DefaultSkyColor = RenderSettings.ambientSkyColor;
            DefaultEquatorColor = RenderSettings.ambientEquatorColor;
            DefaultHorizonColor = RenderSettings.ambientGroundColor;
            ToggleDayNight.isOn = false;
            ToggleDayNight.onValueChanged.AddListener(delegate {
                ToggleValueChanged(ToggleDayNight);
            });

        }

        void ToggleValueChanged(Toggle change)
        {
            if (PhotonNetwork.isMasterClient)
            {
                dayNightCycle.pause = !change.isOn;
                GameSettings.Time.pause = !change.isOn;
                Service.Settings.SyncSettings();
            }

        }
        void OnEnable()
        {
            dayNightCycle = GameObject.Find("Day and Night Controller").GetComponent<DayAndNightControl>();
        }

        

    }
}

