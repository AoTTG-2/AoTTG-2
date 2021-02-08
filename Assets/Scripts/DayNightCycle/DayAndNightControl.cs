using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Animations;
using UnityEditor;
using Assets.Scripts.Services;
using Assets.Scripts.Settings;

namespace Assets.Scripts.DayNightCycle
{
    public class DayAndNightControl : MonoBehaviour
    {
        public GameObject moon;
        public ReflectionProbe reflectionProbe;
        public float sunTilt = -15f;
        [SerializeField] private TimecycleProfile timecycle = null;
        [SerializeField] private float sunRotationOffset = 0f;
        [Tooltip("The amount of frames to wait before doing the next lighting update")]
        [SerializeField] private int lightingUpdateInterval = 10;
        public Material skyboxMaterial;

        [Range(0f, 24f)] public float currentTime;
        public float CurrentTime01 => currentTime / 24;
        public Slider TimeSlider  = null;
        public Camera MoonCamera = null;
        public Camera MainCamera = null;
        public int currentDay = 0;
        public Light directionalLight;
        public float DayLength = 300f; //default value is 300 seconds in one day
        public bool pause { get; set; }
        public float lightIntensity; //static variable to see what the current light's insensity is in the inspector

        private int frames;
       
        // Use this for initialization
        void Start()
        {
            pause=true;
            Service.Settings.OnTimeSettingsChanged += Settings_OnTimeSettingsChanged;
            MoonCamera = GetComponentInChildren<Camera>();
            lightIntensity = directionalLight.intensity; // What's the current intensity of the sunlight

            UpdateLightingSettings();
            UpdateLight(); // Initial lighting update.

            StartCoroutine(SetupCameraConstraintAndReflectionProbe());
            IEnumerator SetupCameraConstraintAndReflectionProbe()
            {
                // Set up rotation constraint for the moon camera.
                RotationConstraint constraint = MoonCamera.gameObject.GetComponent<RotationConstraint>();

                ConstraintSource source = new ConstraintSource();
                Debug.Log("Waiting for Main Camera...");
                yield return new WaitUntil(() => MainCamera != null);
                Debug.Log("The main camera has arriveth");
                source.sourceTransform = Camera.main.transform;
                source.weight = 1f;
                constraint.AddSource(source);

                constraint.rotationOffset = Vector3.zero; // Resets the offset, just to be safe
                constraint.constraintActive = constraint.locked = true; // Enable the constraint and lock it

                reflectionProbe.transform.SetParent(Camera.main.transform);
                reflectionProbe.transform.localPosition = Vector3.zero;
            }
        }

     
        private void Settings_OnTimeSettingsChanged(TimeSettings settings)
        {
            currentTime = (float) GameSettings.Time.currentTime; 
            //additional check to ensure MC cant set non-MC daylengths to less than 60
            if ((float)GameSettings.Time.dayLength < 60)
            {
                DayLength = (float)GameSettings.Time.dayLength;
            }
            else { DayLength = 60; }
            pause = (bool) GameSettings.Time.pause;

            if (!pause)
            {
                // 300s since MC updated this
                var diff = (float) (DateTime.UtcNow - settings.LastModified).TotalSeconds;
                // 5 += ( 300 / 300) * 24 => 29 % 24 => 5
                currentTime += (diff / DayLength) * 24;
                //If time passed will put the currentTime over 24, do maths to correct for this and give an accurate
                //time according to a 24h time range
                if (currentTime > 24)
                {
                    currentTime = currentTime - (24 * (int)Math.Floor(currentTime/24));
                }
            }
        }

        private void OnDestroy()
        {
            Service.Settings.OnTimeSettingsChanged -= Settings_OnTimeSettingsChanged;
        }

        // Update is called once per frame
        void Update()
        {
            
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
            skyboxMaterial.SetVector("_Axis", directionalLight.transform.right);
            skyboxMaterial.SetFloat("_Angle", -CurrentTime01 * 360f);
        }

        void UpdateLightingSettings()
        {
            RenderSettings.skybox = skyboxMaterial;
            RenderSettings.sun = directionalLight; // Procedural skybox needs this to work
            RenderSettings.fog = true;

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
            UpdateLightingSettings();
            UpdateLight();
            reflectionProbe.RenderProbe();
#if UNITY_EDITOR
            // Reflection Probes have limited range so we'll want it to follow the scene view's camera when previewing changes
            Vector3 sceneViewPosition = SceneView.lastActiveSceneView.camera.transform.position;
            // Having it at the exact location of the scene view would be annoying because of the Reflection Probe gizmos
            reflectionProbe.transform.position = new Vector3(sceneViewPosition.x, sceneViewPosition.y - 5f, sceneViewPosition.z);
#endif
        }
    }
}
