using Assets.Scripts.Services;
using Assets.Scripts.Settings;
using Assets.Scripts.Settings.Game;
using System;
using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Rendering.Universal;

namespace Assets.Scripts.DayNightCycle
{
    /// <summary>
    /// The DayAndNightControl class controls the time of day and lighting of AoTTG2. Gradients for ambient light can be found in TimeCycleProfile.cs
    /// </summary>
    public class DayAndNightControl : MonoBehaviour
    {
        public Light Sun;
        public Light Moon;
        public ReflectionProbe ReflectionProbe;
        public float SunTilt = -15f;
        [SerializeField] private TimecycleProfile timecycle = null;
        [SerializeField] private float sunRotationOffset = 0f;
        [Tooltip("The amount of frames to wait before doing the next lighting update")]
        [SerializeField] private int lightingUpdateInterval = 10;
        public Material ProceduralSkyboxMaterial;
        public Material StaticNightSkyboxMaterial;
        public Material StaticDaySkyboxMaterial;
        public Material StaticDawnSkyboxMaterial;
        public Material StaticDuskSkyboxMaterial;
        public bool StaticSkybox;
        [Range(0f, 24f)] public float CurrentTime;
        public float CurrentTimeScale => CurrentTime / 24;
        public Camera MoonCamera = null;
        public Camera MainCamera = null;
        public int CurrentDay = 0;
        public float DayLength;
        public bool Pause { get; set; }
        public float LightIntensity; //static variable to see what the main light's insensity is in the inspector
        public string StaticSkyboxPlayerPref = "StaticSkybox";
        private int frames;

        void Start()
        {
            Setting.Gamemode.Time.CurrentTime.OnValueChanged += CurrentTime_OnValueChanged;
            Setting.Gamemode.Time.DayLength.OnValueChanged += DayLength_OnValueChanged; ;
            Setting.Gamemode.Time.Pause.OnValueChanged += Pause_OnValueChanged;

            //loads static skybox if player has set so in graphics settings
            if (PlayerPrefs.HasKey(StaticSkyboxPlayerPref))
            {
                StaticSkybox = PlayerPrefs.GetInt(StaticSkyboxPlayerPref) == 1;
            }
            //Sets Scene variables to time settings
            //if (PhotonNetwork.isMasterClient)
            {
                CurrentTime = Setting.Gamemode.Time.CurrentTime.Value;
                DayLength = Setting.Gamemode.Time.DayLength.Value;
                Pause = Setting.Gamemode.Time.Pause.Value;
                Service.Settings.SyncSettings();
            }
            MoonCamera = GetComponentInChildren<Camera>();
            LightIntensity = Sun.intensity; // What's the current intensity of the sunlight

            UpdateLightingSettings();
            UpdateLight(); // Initial lighting update.

            StartCoroutine(SetupCameraConstraintAndReflectionProbe());
            IEnumerator SetupCameraConstraintAndReflectionProbe()
            {
                // Set up rotation constraint for the moon camera.
                RotationConstraint constraint = MoonCamera.gameObject.GetComponent<RotationConstraint>();
                ConstraintSource source = new ConstraintSource();
                yield return new WaitUntil(() => MainCamera != null);
                source.sourceTransform = Camera.main.transform;
                source.weight = 1f;
                constraint.AddSource(source);

                constraint.rotationOffset = Vector3.zero; // Resets the offset, just to be safe
                constraint.constraintActive = constraint.locked = true; // Enable the constraint and lock it

                ReflectionProbe.transform.SetParent(Camera.main.transform);
                ReflectionProbe.transform.localPosition = Vector3.zero;

                var moonCameraData = MoonCamera.GetUniversalAdditionalCameraData();
                moonCameraData.cameraStack.Add(MainCamera);
            }
        }

        private void Pause_OnValueChanged(bool obj)
        {
            Pause = obj;

            if (!Pause)
            {
                var diff = (float) (DateTime.UtcNow - Setting.Gamemode.Time.LastModified).TotalSeconds;
                CurrentTime += (diff / DayLength) * 24;
                //If time passed will put the CurrentTime over 24, do maths to correct for this and give an accurate
                //time according to a 24h time range
                if (CurrentTime > 24)
                {
                    CurrentTime -= (24 * (int) Math.Floor(CurrentTime / 24));
                }
            }
        }

        private void DayLength_OnValueChanged(float obj)
        {
            if (Setting.Gamemode.Time.DayLength > 60)
            {
                DayLength = Setting.Gamemode.Time.DayLength.Value;
            }
            else
            {
                DayLength = 60;
                Setting.Gamemode.Time.DayLength.Value = DayLength;
            }
        }

        private void CurrentTime_OnValueChanged(float obj)
        {
            CurrentTime = obj;
        }
        
        private void OnDestroy()
        {
            Setting.Gamemode.Time.CurrentTime.OnValueChanged -= CurrentTime_OnValueChanged;
            Setting.Gamemode.Time.DayLength.OnValueChanged -= DayLength_OnValueChanged; ;
            Setting.Gamemode.Time.Pause.OnValueChanged -= Pause_OnValueChanged;
        }

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
            // StaticSkybox Skybox
            UpdateSkybox();
            if (!Pause)
            {
                CurrentTime += (Time.deltaTime / DayLength) * 24;
                if (CurrentTimeScale >= 1) // If CurrentTime >= 24 hours
                {
                    CurrentTime = 0; // Reset time back to midnight
                    CurrentDay++; // Increment the day counter
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
            //TODO: Settings shouldn't be set every frame
            //if (PhotonNetwork.isMasterClient)
            //{
            //    Setting.Gamemode.Time.CurrentTime.Value = CurrentTime;
            //    Setting.Gamemode.Time.DayLength.Value = DayLength;
            //    Setting.Gamemode.Time.Pause.Value = Pause;
            //}
        }

        void UpdateMaterial()
        {
            ProceduralSkyboxMaterial.SetFloat("_AtmosphereThickness", timecycle.atmosphereThickness.Evaluate(CurrentTimeScale));
        }
        void UpdateSkybox()
        {
            if (StaticSkybox)
            {
                if (CurrentTime <= 5)
                    RenderSettings.skybox = StaticNightSkyboxMaterial;
                else if (CurrentTime > 5 && CurrentTime <= 8)
                    RenderSettings.skybox = StaticDawnSkyboxMaterial;
                else if (CurrentTime > 8 && CurrentTime <= 18)
                    RenderSettings.skybox = StaticDaySkyboxMaterial;
                else if (CurrentTime > 17 && CurrentTime <= 19)
                    RenderSettings.skybox = StaticDuskSkyboxMaterial;
                else if (CurrentTime > 19)
                    RenderSettings.skybox = StaticNightSkyboxMaterial;
            }
            else
            {
                RenderSettings.skybox = ProceduralSkyboxMaterial;
                ProceduralSkyboxMaterial.SetVector("_Axis", Sun.transform.right);
                ProceduralSkyboxMaterial.SetFloat("_Angle", -CurrentTimeScale * 360f);
            }

        }
        void UpdateLightingSettings()
        {
            RenderSettings.sun = Sun; // Procedural skybox needs this to work
            //RenderSettings.fog = true;

            if (!timecycle) return;
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

        void UpdateLight()
        {
            bool isNightTime = (CurrentTime <= 6 || CurrentTime >= 18);
            Sun.enabled = !isNightTime;
            Moon.enabled = isNightTime;

            Quaternion tilt = Quaternion.AngleAxis(SunTilt, Vector3.forward);
            Quaternion rot = Quaternion.AngleAxis((CurrentTimeScale * 360) - 90, Vector3.right);

            Sun.transform.rotation = tilt * rot; // Yes axial tilt
            Sun.transform.Rotate(Vector3.up, sunRotationOffset - 90, Space.World);
            Moon.transform.forward = -Sun.transform.forward;

            if (!timecycle) return;

            // Sun & moon's color and brightness
            if (timecycle.overrideSunlight && !isNightTime)
            {
                Sun.color = timecycle.sunlightColor.Evaluate(CurrentTimeScale);
                Sun.intensity = timecycle.sunlightColor.Evaluate(CurrentTimeScale).a * timecycle.maxSunlightIntensity;
            }
            if (timecycle.overrideMoonlight && isNightTime)
            {
                Moon.color = timecycle.moonlightColor.Evaluate(CurrentTimeScale);
                Moon.intensity = timecycle.moonlightColor.Evaluate(CurrentTimeScale).a * timecycle.maxMoonlightIntensity;
            }

            // Environment lighting
            if (timecycle.overrideEnvironmentLighting)
            {
                switch (timecycle.lightingOverrideMode)
                {
                    case TimecycleProfile.AmbientLightingOverrideMode.Gradient:
                        RenderSettings.ambientSkyColor = timecycle.skyColor.Evaluate(CurrentTimeScale);
                        RenderSettings.ambientEquatorColor = timecycle.equatorColor.Evaluate(CurrentTimeScale);
                        RenderSettings.ambientGroundColor = timecycle.groundColor.Evaluate(CurrentTimeScale);
                        break;
                    case TimecycleProfile.AmbientLightingOverrideMode.Color:
                        RenderSettings.ambientLight = timecycle.lightingColor.Evaluate(CurrentTimeScale);
                        break;
                }
            }

            // Fog
            if (timecycle.overrideFog)
            {
                RenderSettings.fogColor = timecycle.fogColor.Evaluate(CurrentTimeScale);
                RenderSettings.fogDensity = timecycle.fogColor.Evaluate(CurrentTimeScale).a * timecycle.maxFogDensity;
            }


        }

#if UNITY_EDITOR
        void OnValidate()
        {
            UpdateLightingSettings();
            UpdateLight();
            ProceduralSkyboxMaterial.SetFloat("_AtmosphereThickness", timecycle.atmosphereThickness.Evaluate(CurrentTime));
            ReflectionProbe.RenderProbe();
            // Reflection Probes have limited range so we'll want it to follow the scene view's camera when previewing changes
            Vector3 sceneViewPosition = SceneView.lastActiveSceneView != null ? SceneView.lastActiveSceneView.camera.transform.position : Vector3.zero;
            // Having it at the exact location of the scene view would be annoying because of the Reflection Probe gizmos
            ReflectionProbe.transform.position = new Vector3(sceneViewPosition.x, sceneViewPosition.y - 5f, sceneViewPosition.z);

        }
#endif
    }
}

