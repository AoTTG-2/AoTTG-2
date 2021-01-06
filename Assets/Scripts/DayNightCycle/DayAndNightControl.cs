
using Assets.Scripts.Services;
using Assets.Scripts.Settings;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.DayNightCycle
{
    public enum AmbientLightingOverrideMode
    {
        Gradient,
        Color
    }

    public class DayAndNightControl : MonoBehaviour
    {

        public GameObject Camera;
        public GameObject moonState;
        public GameObject moon;
        public float sunTilt = -15f;
        [SerializeField] private TimecycleProfile timecycle;
        [SerializeField] private float sunRotationOffset;
        public Material skyBoxDAWN;
        public Material skyBoxDAY;
        public Material skyBoxSUNSET;
        public Material skyBoxNIGHT;
        public float currentTime { get; set; }
        public float CurrentTime01 { get { return currentTime / 24; } set { currentTime = value * 24; } }
        public Slider TimeSlider;
        public Camera MoonCamera;

        public int currentDay = 0;
        public Light directionalLight;
        private float SecondsInAFullDay = 120f;//default value is 120 seconds in one day
        public bool pause { get; set; }
        public float DayLength
        {
            get { return SecondsInAFullDay; }

            set { SecondsInAFullDay = value; }
        }

        [HideInInspector]
        public float timeMultiplier = 1f; //how fast the day goes by regardless of the secondsInAFullDay var. lower values will make the days go by longer, while higher values make it go faster. This may be useful if you're siumulating seasons where daylight and night times are altered.
        public bool showUI;
        float lightIntensity; //static variable to see what the current light's insensity is in the inspector


        Camera targetCam;

        // Use this for initialization
        void Start()
        {
            
            Service.Settings.OnTimeSettingsChanged += Settings_OnTimeSettingsChanged;
            MoonCamera = GameObject.Find("MoonCamera").GetComponent<Camera>();
            TimeSlider = GameObject.Find("TimeSlider").GetComponent<Slider>();
            foreach (Camera c in GameObject.FindObjectsOfType<Camera>())
            {
                if (c.isActiveAndEnabled)
                {
                    targetCam = c;
                }
            }
            lightIntensity = directionalLight.intensity; //what's the current intensity of the light
            // AFAIK procedural skybox needs this to work
            RenderSettings.sun = directionalLight;


            //Check if default light prefab exists, and if so, disable it
            if (GameObject.Find("LightSet"))
            {
                GameObject.Find("LightSet").SetActive(false);
            }

            if (timecycle)
            {
                if (timecycle.overrideEnvironmentLighting == true)
                {
                    switch (timecycle.lightingOverrideMode)
                    {
                        case AmbientLightingOverrideMode.Gradient:
                            RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Trilight;
                            break;
                        case AmbientLightingOverrideMode.Color:
                            RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Flat;
                            break;
                    }
                }
                else
                {
                    RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Skybox;
                }
            }
        }

        private void Settings_OnTimeSettingsChanged(TimeSettings settings)
        {
            currentTime = (float) GameSettings.Time.currentTime;
            DayLength = (float) GameSettings.Time.dayLength;
            pause = (bool) GameSettings.Time.pause;
            
        }

        private void OnDestroy()
        {
            Service.Settings.OnTimeSettingsChanged -= Settings_OnTimeSettingsChanged;
        }
        // Update is called once per frame
        void Update()
        {
            if (GameObject.Find("LightSet"))
            {
                GameObject.Find("LightSet").SetActive(false);
            }
            //updates timeslider when out of menu
            if (PhotonNetwork.isMasterClient)
            {
                if (!GameObject.Find("Game Settings"))
                {
                    TimeSlider.value = CurrentTime01;
                }
                else
                {
                    //SyncTimeRPC(currentTime, DayLength,pause);
                }
            }
            //The below syncs the field of view of the moon camera and the main camera, and removes unwanted issues with moon rendering
            //(main camera's field of view changes alot, and if the moon camera's doesnt, it distorts the moon's rendering)
            MoonCamera.fieldOfView = GameObject.Find("MainCamera").GetComponent<Camera>().fieldOfView;
            if (!pause)
            {
                foreach (Camera c in GameObject.FindObjectsOfType<Camera>())
                {
                    if (c.isActiveAndEnabled)
                    {
                        targetCam = c;
                    }
                    UpdateLight();
                    currentTime += (Time.deltaTime / SecondsInAFullDay) * 24;
                    if (CurrentTime01 >= 1)
                    {
                        currentTime = 0;//once we hit "midnight"; any time after that sunrise will begin.
                        currentDay++; //make the day counter go up
                    }
                }
            }
            //MC loads settings
            if (PhotonNetwork.isMasterClient)
            {
                PhotonView photonView = PhotonView.Get(this);
                
                //photonView.RPC("SyncTimeRPC", PhotonTargets.All, currentTime, DayLength, pause);
                GameSettings.Time.currentTime = currentTime;
                GameSettings.Time.dayLength = DayLength;
                GameSettings.Time.pause = pause;
                Debug.Log("mc");
                Debug.Log(GameSettings.Time.currentTime);
            }

            //TODO : add a OnJoinedRoom method so that on join, the MC's current time is loaded
            //TODO : resynch time once mc leaves the game settings, because by default while they are
            //in time settings the system is paused(but not for non-mc) , alternatively make it so
            //that the system is not paused while MC is on timesettings menu

        }


        void UpdateLight()
        {
            Quaternion tilt = Quaternion.AngleAxis(sunTilt, Vector3.forward);
            Quaternion rot = Quaternion.AngleAxis((CurrentTime01 * 360) - 90, Vector3.right);

            directionalLight.transform.rotation = tilt * rot; // Yes axial tilt
            directionalLight.transform.Rotate(Vector3.up, sunRotationOffset - 90, Space.World);

            // -moonState.transform.forward = -RenderSettings.sun.transform.forward;
            moonState.transform.forward = directionalLight.transform.forward;

            if (timecycle)
            {
                // Sun & moon's color and brightness
                if (timecycle.overrideSunlight)
                {
                    directionalLight.color = timecycle.sunlightColor.Evaluate(CurrentTime01);
                    directionalLight.intensity = timecycle.sunlightColor.Evaluate(CurrentTime01).a * timecycle.maxSunlightIntensity;
                }
                if (timecycle.overrideMoonlight)
                {
                    Light moonLight = moonState.GetComponentInChildren<Light>();
                    moonLight.color = timecycle.moonlightColor.Evaluate(CurrentTime01);
                    moonLight.intensity = timecycle.moonlightColor.Evaluate(CurrentTime01).a * timecycle.maxMoonlightIntensity;
                }

                // Environment lighting
                if (timecycle.overrideEnvironmentLighting == true)
                {
                    switch (timecycle.lightingOverrideMode)
                    {
                        case AmbientLightingOverrideMode.Gradient:
                            RenderSettings.ambientSkyColor = timecycle.skyColor.Evaluate(CurrentTime01);
                            RenderSettings.ambientEquatorColor = timecycle.equatorColor.Evaluate(CurrentTime01);
                            RenderSettings.ambientGroundColor = timecycle.groundColor.Evaluate(CurrentTime01);
                            break;
                        case AmbientLightingOverrideMode.Color:
                            RenderSettings.ambientLight = timecycle.lightingColor.Evaluate(CurrentTime01);
                            break;
                    }
                }

                // Fog
                if (timecycle.overrideFog)
                {
                    RenderSettings.fogColor = timecycle.fogColor.Evaluate(CurrentTime01);
                    RenderSettings.fogDensity = timecycle.fogColor.Evaluate(CurrentTime01).a * timecycle.maxFogDensity;
                }
            }

            //change skybox to add mood
            if (CurrentTime01 < 0.2f)
            {
                RenderSettings.skybox = skyBoxNIGHT;
            }
            if (CurrentTime01 > 0.25f && CurrentTime01 < 0.40f)
            {
                RenderSettings.skybox = skyBoxDAWN;
            }
            if (CurrentTime01 > 0.40f && CurrentTime01 < 0.75f )
            {
                RenderSettings.skybox = skyBoxDAY;
            }

            if (CurrentTime01 > 0.75f  && CurrentTime01 < 0.99f )
            {
                if (CurrentTime01 > 0.875f )
                {
                    RenderSettings.skybox = skyBoxNIGHT;
                }
                else
                {
                    RenderSettings.skybox = skyBoxSUNSET;
                }
            }
        }

        public string TimeOfDay()
        {
            // TODO: Consider using switch-case
            string dayState = "";
            if (CurrentTime01 > 0f  && CurrentTime01 < 0.1f )
            {
                dayState = "Midnight";
            }
            if (CurrentTime01 < 0.5f  && CurrentTime01 > 0.1f )
            {
                dayState = "Morning";
            }
            if (CurrentTime01 > 0.5f  && CurrentTime01 < 0.6f)
            {
                dayState = "Mid Noon";
            }
            if (CurrentTime01 > 0.6f && CurrentTime01 < 0.8f)
            {
                dayState = "Evening";
            }
            if (CurrentTime01 > 0.8f && CurrentTime01 < 1f)
            {
                dayState = "Night";
            }
            return dayState;
        }
        
    }
}
