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

                if (ToggleDayNight.isOn)
                {

                    dayNightCycle.pause = false;
                    GameSettings.Time.pause = false;
                    Service.Settings.SyncSettings();
                    Debug.Log("toggle synching");
                }
                else
                {
                    dayNightCycle.pause = true;
                    GameSettings.Time.pause = true;
                    Service.Settings.SyncSettings();
                    Debug.Log("toggle synching");
                }
            }
        }
            // Update is called once per frame
            void Update()
        {
            

        }
        void OnEnable()
        {
            dayNightCycle = GameObject.Find("Day and Night Controller").GetComponent<DayAndNightControl>();
        }

        

    }
}

