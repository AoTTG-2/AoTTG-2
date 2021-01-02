using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeOfDay : MonoBehaviour
{
    [Range(0, 24)] public float time;
    public float sunTilt;
    public float sunRotationOffset;
    [Space]
    public TimecycleProfile timecycle;
    public Light moonLight;

    // Start is called before the first frame update
    void Start()
    {
        if (timecycle)
        {
            if (timecycle.overrideEnvironmentLighting == true)
            {
                switch (timecycle.lightingOverrideType)
                {
                    case LightingOverrideType.Gradient:
                        RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Trilight;
                        break;
                    case LightingOverrideType.Color:
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

    // Update is called once per frame
    void Update()
    {
        // Update sun rotation
        // This part was hell for me to implement especially with the sun tilt
        Quaternion tilt = Quaternion.AngleAxis(sunTilt, Vector3.forward);
        Quaternion rot = Quaternion.AngleAxis(((time / 24) * 360) - 90, Vector3.right);

        RenderSettings.sun.transform.rotation = tilt * rot;
        RenderSettings.sun.transform.Rotate(Vector3.up, sunRotationOffset - 90, Space.World);

        // Update moon rotation
        moonLight.transform.forward = -RenderSettings.sun.transform.forward;

        if (timecycle)
        {
            // Sun & moon's color and brightness
            if (timecycle.overrideSunlight)
            {
                RenderSettings.sun.color = timecycle.sunlightColor.Evaluate(time / 24);
                RenderSettings.sun.intensity = timecycle.sunlightColor.Evaluate(time / 24).a * timecycle.maxSunlightIntensity;
            }
            if (timecycle.overrideMoonlight)
            {
                moonLight.color = timecycle.moonlightColor.Evaluate(time / 24);
                moonLight.intensity = timecycle.moonlightColor.Evaluate(time / 24).a * timecycle.maxMoonlightIntensity;
            }

            // Environment lighting
            if (timecycle.overrideEnvironmentLighting == true)
            {
                switch (timecycle.lightingOverrideType)
                {
                    case LightingOverrideType.Gradient:
                        RenderSettings.ambientSkyColor = timecycle.skyColor.Evaluate(time / 24);
                        RenderSettings.ambientEquatorColor = timecycle.equatorColor.Evaluate(time / 24);
                        RenderSettings.ambientGroundColor = timecycle.groundColor.Evaluate(time / 24);
                        break;
                    case LightingOverrideType.Color:
                        RenderSettings.ambientLight = timecycle.lightingColor.Evaluate(time / 24);
                        break;
                }
            }

            // Fog
            if (timecycle.overrideFog)
            {
                RenderSettings.fogColor = timecycle.fogColor.Evaluate(time / 24);
                RenderSettings.fogDensity = timecycle.fogColor.Evaluate(time / 24).a * timecycle.maxFogDensity;
            }
        }
    }

    // This function is called when the script is loaded or a value is changed in the inspector (Called in the editor only)
    private void OnValidate()
    {
        Start();
        Update();
    }

    public enum LightingOverrideType
    {
        Gradient,
        Color
    }
}
