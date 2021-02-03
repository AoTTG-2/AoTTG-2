using Assets.Scripts.Services;
using Assets.Scripts.Settings;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.Animations;

namespace Assets.Scripts.DayNightCycle
{
    public class DayAndNightControl : MonoBehaviour
    {
        public GameObject moon;
        public float sunTilt = -15f;
        [SerializeField] private TimecycleProfile timecycle = null;
        [SerializeField] private float sunRotationOffset = 0f;
        [Tooltip("The amount of frames to wait before doing the next lighting update")]
        [SerializeField] private int lightingUpdateInterval = 10;
        public Material skyBoxPROCEDURAL;

        [Range(0f, 24f)] public float currentTime;
        public float CurrentTime01 => currentTime / 24;
        public Slider TimeSlider  = null;
        public Camera MoonCamera = null;
        public Camera MainCamera = null;
        public int currentDay = 0;
        public Light directionalLight;
        public float DayLength = 300f; //default value is 300 seconds in one day
        public bool pause { get; set; }

        [HideInInspector]
        float lightIntensity; //static variable to see what the current light's insensity is in the inspector

        Camera targetCam;
        private int frames;
       
        // Use this for initialization
        void Start()
        {
            pause=true;
            RenderSettings.skybox = skyBoxPROCEDURAL;
            Service.Settings.OnTimeSettingsChanged += Settings_OnTimeSettingsChanged;
            MoonCamera = GetComponentInChildren<Camera>();

            foreach (Camera c in GameObject.FindObjectsOfType<Camera>())
            {
                if (c.isActiveAndEnabled)
                {
                    targetCam = c;
                }
            }
            lightIntensity = directionalLight.intensity; // What's the current intensity of the sunlight
            // Procedural skybox needs this to work
            RenderSettings.sun = directionalLight;

            if (timecycle)
            {
                if (timecycle.overrideEnvironmentLighting)
                {
                    switch (timecycle.lightingOverrideMode)
                    {
                        case TimecycleProfile.AmbientLightingOverrideMode.Gradient:
                            RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Trilight;
                            break;
                        case TimecycleProfile.AmbientLightingOverrideMode.Color:
                            RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Flat;
                            break;
                    }
                }
                else
                {
                    RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Skybox;
                }
            }
            
            UpdateLight(); // Initial lighting update.

            // Set up rotation constraint for the moon camera.
            RotationConstraint constraint = MoonCamera.gameObject.GetComponent<RotationConstraint>();

            ConstraintSource source = new ConstraintSource();
            source.sourceTransform = Camera.main.transform;
            source.weight = 1f;

            constraint.AddSource(source);
            constraint.constraintActive = constraint.locked = true; // Enable the constraint and lock it
        }

        public void OnPhotonPlayerConnected(PhotonPlayer newPlayer)
        {
            Service.Settings.SyncSettings();
        }
     
        private void Settings_OnTimeSettingsChanged(TimeSettings settings)
        {
            currentTime = (float) GameSettings.Time.currentTime; // 9
            DayLength = (float) GameSettings.Time.dayLength; // 300
            pause = (bool) GameSettings.Time.pause; // true

            if (!pause)
            {
                // 300s since MC updated this
                var diff = (float) (DateTime.UtcNow - settings.LastModified).TotalSeconds;

                // 5 += ( 300 / 300) * 24 => 29 % 24 => 5
                currentTime += (diff / DayLength) * 24;
            }
        }

        private void OnDestroy()
        {
            Service.Settings.OnTimeSettingsChanged -= Settings_OnTimeSettingsChanged;
        }

        // Update is called once per frame
        void Update()
        {
            //additional check to ensure MC cant set non-MC daylengths to less than 60
            if (DayLength < 60)
            {
                DayLength = 60;
            }
            // This is to prevent null reference errors because non-MCs needs a few frames before the camera will be available.
            if (MainCamera == null)
            {
                MainCamera = Camera.main;
            }
            else
            {
                MoonCamera.fieldOfView = MainCamera.fieldOfView;
            }

            if (!pause)
            {
                currentTime += (Time.deltaTime / DayLength) * 24;
                if (CurrentTime01 >= 1) // If CurrentTime >= 24 hours
                {
                    currentTime = 0; // Reset time back to midnight
                    currentDay++; // Increment the day counter
                }
            }

            if (frames == lightingUpdateInterval)
            {
                frames = 0;
                UpdateLight();
                UpdateMaterial();
            }
            frames++;
            //MC loads settings
            if (PhotonNetwork.isMasterClient)
            {
                GameSettings.Time.currentTime = currentTime;
                GameSettings.Time.dayLength = DayLength;
                GameSettings.Time.pause = pause;
            }
        }

        void UpdateMaterial()
        {
            skyBoxPROCEDURAL.SetVector("_Axis", directionalLight.transform.right);
            skyBoxPROCEDURAL.SetFloat("_Angle", -CurrentTime01 * 360f);
        }
        
        void UpdateLight()
        {
            Quaternion tilt = Quaternion.AngleAxis(sunTilt, Vector3.forward);
            Quaternion rot = Quaternion.AngleAxis((CurrentTime01 * 360) - 90, Vector3.right);

            directionalLight.transform.rotation = tilt * rot; // Yes axial tilt
            directionalLight.transform.Rotate(Vector3.up, sunRotationOffset - 90, Space.World);
            moon.transform.forward = -directionalLight.transform.forward;

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
                    Light moonLight = moon.GetComponent<Light>();
                    moonLight.color = timecycle.moonlightColor.Evaluate(CurrentTime01);
                    moonLight.intensity = timecycle.moonlightColor.Evaluate(CurrentTime01).a * timecycle.maxMoonlightIntensity;
                }

                // Environment lighting
                if (timecycle.overrideEnvironmentLighting)
                {
                    switch (timecycle.lightingOverrideMode)
                    {
                        case TimecycleProfile.AmbientLightingOverrideMode.Gradient:
                            RenderSettings.ambientSkyColor = timecycle.skyColor.Evaluate(CurrentTime01);
                            RenderSettings.ambientEquatorColor = timecycle.equatorColor.Evaluate(CurrentTime01);
                            RenderSettings.ambientGroundColor = timecycle.groundColor.Evaluate(CurrentTime01);
                            break;
                        case TimecycleProfile.AmbientLightingOverrideMode.Color:
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
        }

        void OnValidate()
        {
            if (RenderSettings.skybox != skyBoxPROCEDURAL)
            {
                RenderSettings.skybox = skyBoxPROCEDURAL;
            }
            if (RenderSettings.sun != directionalLight)
            {
                RenderSettings.sun = directionalLight;
            }
            UpdateLight();
        }

        public TimeOfDay GetTimeOfDay()
        {
            if (CurrentTime01 >= 0f && CurrentTime01 <= 0.2f)
            {
                return TimeOfDay.Midnight;
            }
            else if (CurrentTime01 <= 0.5f && CurrentTime01 > 0.2f)
            {
                return TimeOfDay.Morning;
            }
            else if (CurrentTime01 >= 0.5f && CurrentTime01 < 0.75f)
            {
                return TimeOfDay.Afternoon;
            }
            else if (CurrentTime01 >= 0.75f && CurrentTime01 <= 1f)
            {
                return TimeOfDay.Night;
            }
            return TimeOfDay.UNKNOWN; // If this return is reached, something probably went wrong
        }

        public enum TimeOfDay
        {
            Midnight,
            Morning,
            Afternoon,
            Night,
            UNKNOWN,
        }
    }
}
