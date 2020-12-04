
using UnityEngine;
using UnityEngine.UI;


    public class ToggleDayNightController : MonoBehaviour
    {
        public Color DefaultSkyColor;
        public Color DefaultEquatorColor;
        public Color DefaultHorizonColor;

        public GameObject DayNightControllerPrefab = null;
        public Toggle ToggleDayNight;
        public Button ResetDayNightButton;
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
            Button btn = ResetDayNightButton.GetComponent<Button>();
            btn.onClick.AddListener(PauseDayNightSystem);
            DayNightCycle = GameObject.Find("Day and Night Controller(Clone)").GetComponent<DayAndNightControl>();


        }

        public void PauseDayNightSystem()
        {
            DayNightCycle = GameObject.Find("Day and Night Controller(Clone)").GetComponent<DayAndNightControl>();
            DayNightCycle.pause = !DayNightCycle.pause;
        }
        // Update is called once per frame
        void Update()
        {

            //when changing scene, this becomes null, so here we refnd and reassign it
            if (DefaultLightSet == null)
            {
                DefaultLightSet = GameObject.Find("LightSet");
            }
            if (ToggleDayNight.isOn)
            {
                DefaultLightSet.SetActive(false);
                if (!GameObject.Find("Day and Night Controller(Clone)"))
                {
                            float dynamicScale = (GameObject.Find("GroundTerrain").GetComponent<Collider>().bounds.size.x +
                            GameObject.Find("GroundTerrain").GetComponent<Collider>().bounds.size.z) / 3500;
                    //^^^^3500 is an experimentally determined value that allows the daynight cycle to fully set and rise just beyond the borders
                    //of the scene's terrain

                    DayNightController = Instantiate(DayNightControllerPrefab, GameObject.Find("GroundTerrain").GetComponent<Collider>().bounds.center, Quaternion.identity);
                    DayNightController.transform.localScale = new Vector3(dynamicScale, dynamicScale, dynamicScale);//scales the object to fit the scene
                }

            }
            else
            {

                DayNightController = GameObject.Find("Day and Night Controller(Clone)");
                Destroy(DayNightController);
                GameObject.Find("MainCamera").GetComponent<Skybox>().material = skyBoxReset;
                RenderSettings.ambientSkyColor = DefaultSkyColor;
                RenderSettings.ambientEquatorColor = DefaultEquatorColor;
                RenderSettings.ambientGroundColor = DefaultHorizonColor;
                DefaultLightSet.SetActive(true);

            }

        }


    }

