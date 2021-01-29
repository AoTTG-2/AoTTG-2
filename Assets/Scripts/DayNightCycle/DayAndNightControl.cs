using Assets.Scripts.Services;
using Assets.Scripts.Settings;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using UnityEngine.Animations;

namespace Assets.Scripts.DayNightCycle
{
    public class DayAndNightControl : MonoBehaviour
    {
        // public GameObject Camera;
        public GameObject moon;
        public float sunTilt = -15f;
        [SerializeField] private TimecycleProfile timecycle = null;
        [SerializeField] private float sunRotationOffset = 0f;
        [Tooltip("The amount of frames to wait before doing the next lighting update")]
        [SerializeField] private int lightingUpdateInterval = 10;
        public Material skyBoxPROCEDURAL;

        [Range(0f, 24f)] public float currentTime;
        public float currentTime01 => currentTime / 24;
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
            lightIntensity = directionalLight.intensity; //what's the current intensity of the light
            // AFAIK procedural skybox needs this to work
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
           

            UpdateLight(); // Initial lighting update. Without this, the lighting will look as if it's lagging when the scene just loaded
            // Set up rotation constraint for the moon camera.
            RotationConstraint constraint = MoonCamera.gameObject.GetComponent<RotationConstraint>();
            if (constraint == null)
            {
                constraint = MoonCamera.gameObject.AddComponent<RotationConstraint>();
            }

            ConstraintSource sauce = new ConstraintSource();
            sauce.sourceTransform = Camera.main.transform;
            sauce.weight = 1f;
            constraint.AddSource(sauce);

            // constraint.rotationAtRest = constraint.rotationOffset = Vector3.zero; // Reset rotationAtRest and rotationOffset
            constraint.constraintActive = constraint.locked = true; // Enable the constraint and lock it
        }


       /* public void OnPhotonPlayerConnected(PhotonPlayer newPlayer)
        {
            
            //syncs settings after waiting a few seconds( initial lag on player join desynchs their time, hence waiting 5 seconds)
            StartCoroutine(ExecuteAfterTime(5));
        }
        IEnumerator ExecuteAfterTime(float time)
        {
            yield return new WaitForSeconds(time);
            Service.Settings.SyncSettings();
        }*/

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
            try
            {

                if (MainCamera == null)
                {
                    MainCamera = GameObject.Find("MainCamera").GetComponent<Camera>();
                }
                
               
            }
            catch (NullReferenceException)
            {
                //put here because non-mcs need a few frames before the camera is available, not using a try catch results in 
                //nullrefs
            }

            //The below syncs the field of view of the moon camera and the main camera, and removes unwanted issues with moon rendering
            //(main camera's field of view changes alot, and if the moon camera's doesnt, it distorts the moon's rendering)
            if (MainCamera != null)
            {
                MoonCamera.fieldOfView = MainCamera.fieldOfView;
                // MoonCamera.transform.rotation = MainCamera.transform.rotation;
            }

            if (!pause)
            {

                UpdateLight();
                currentTime += (Time.deltaTime / DayLength) * 24;
                if (currentTime01 >= 1)
                {
                    currentTime = 0;//once we hit "midnight"; any time after that sunrise will begin.
                    currentDay++; //make the day counter go up
                }
                if (frames == lightingUpdateInterval) { frames = 0; }
            }
            else
            {
                if (frames == lightingUpdateInterval)
                {
                    frames = 0;
                    UpdateLight();
                }
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
        
        void UpdateLight()
        {
            Quaternion tilt = Quaternion.AngleAxis(sunTilt, Vector3.forward);
            Quaternion rot = Quaternion.AngleAxis((currentTime01 * 360) - 90, Vector3.right);

            directionalLight.transform.rotation = tilt * rot; // Yes axial tilt
            directionalLight.transform.Rotate(Vector3.up, sunRotationOffset - 90, Space.World);
            moon.transform.forward = -directionalLight.transform.forward;

            if (timecycle)
            {
                // Sun & moon's color and brightness
                if (timecycle.overrideSunlight)
                {
                    directionalLight.color = timecycle.sunlightColor.Evaluate(currentTime01);
                    directionalLight.intensity = timecycle.sunlightColor.Evaluate(currentTime01).a * timecycle.maxSunlightIntensity;
                }
                if (timecycle.overrideMoonlight)
                {
                    Light moonLight = moon.GetComponent<Light>();
                    moonLight.color = timecycle.moonlightColor.Evaluate(currentTime01);
                    moonLight.intensity = timecycle.moonlightColor.Evaluate(currentTime01).a * timecycle.maxMoonlightIntensity;
                }

                // Environment lighting
                if (timecycle.overrideEnvironmentLighting)
                {
                    switch (timecycle.lightingOverrideMode)
                    {
                        case TimecycleProfile.AmbientLightingOverrideMode.Gradient:
                            RenderSettings.ambientSkyColor = timecycle.skyColor.Evaluate(currentTime01);
                            RenderSettings.ambientEquatorColor = timecycle.equatorColor.Evaluate(currentTime01);
                            RenderSettings.ambientGroundColor = timecycle.groundColor.Evaluate(currentTime01);
                            break;
                        case TimecycleProfile.AmbientLightingOverrideMode.Color:
                            RenderSettings.ambientLight = timecycle.lightingColor.Evaluate(currentTime01);
                            break;
                    }
                }

                // Fog
                if (timecycle.overrideFog)
                {
                    RenderSettings.fogColor = timecycle.fogColor.Evaluate(currentTime01);
                    RenderSettings.fogDensity = timecycle.fogColor.Evaluate(currentTime01).a * timecycle.maxFogDensity;
                }
            }
        }


        public TimeOfDay GetTimeOfDay()
        {
            if (currentTime01 >= 0f && currentTime01 <= 0.2f)
            {
                return TimeOfDay.Midnight;
            }
            else if (currentTime01 <= 0.5f && currentTime01 > 0.2f)
            {
                return TimeOfDay.Morning;
            }
            else if (currentTime01 >= 0.5f && currentTime01 < 0.75f)
            {
                return TimeOfDay.Afternoon;
            }
            else if (currentTime01 >= 0.75f && currentTime01 <= 1f)
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
