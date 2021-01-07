
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

        DayAndNightControl DayNightCycle;
        public GameObject DefaultLightSet;
        // Start is called before the first frame update
        void Start()
        {
            DefaultLightSet = GameObject.Find("LightSet");
            //These defaults are stored so that when the system is toggled off, all colour settings are set back to the scene defaults
            DefaultSkyColor = RenderSettings.ambientSkyColor;
            DefaultEquatorColor = RenderSettings.ambientEquatorColor;
            DefaultHorizonColor = RenderSettings.ambientGroundColor;
            skyBoxReset = GameObject.Find("MainCamera").GetComponent<Skybox>().material;
            ToggleDayNight.isOn = false;

            DayNightCycle = GameObject.Find("Day and Night Controller").GetComponent<DayAndNightControl>();


        }

        // Update is called once per frame
        void Update()
        {
            

            if (ToggleDayNight.isOn)
            {
                DayNightCycle.pause = !DayNightCycle.pause;
               

            }
            else
            {
                DayNightCycle.pause = !DayNightCycle.pause;
            }

        }


    }
}

