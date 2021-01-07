using UnityEngine;

[CreateAssetMenu(fileName = "New Timecycle Profile", menuName = "Timecycle Profile")]
public class TimecycleProfile : ScriptableObject
{
    [Header("Sun and moon settings")]
    public bool overrideSunlight = false;
    public float maxSunlightIntensity = 1.5f;
    public Gradient sunlightColor;
    [Space]
    public bool overrideMoonlight = false;
    public float maxMoonlightIntensity = 0.3f;
    public Gradient moonlightColor;
    
    [Header("Environment lighting settings")]
    public bool overrideEnvironmentLighting = false;
    public AmbientLightingOverrideMode lightingOverrideMode = AmbientLightingOverrideMode.Gradient;
    [Space]
    public Gradient lightingColor;
    [Space]
    public Gradient skyColor;
    public Gradient equatorColor;
    public Gradient groundColor;

    [Header("Fog settings")]
    public bool overrideFog = false;
    public float maxFogDensity = 0.01f;
    public Gradient fogColor;

    public enum AmbientLightingOverrideMode
    {
        Gradient,
        Color
    }
}
