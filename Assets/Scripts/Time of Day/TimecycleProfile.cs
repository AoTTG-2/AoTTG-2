using UnityEngine;

namespace Assets.Scripts.DayNightCycle
{
    /// <summary>
    /// ScriptableObject containing settings for the time cycle. Used within <see cref="DayAndNightControl"/>
    /// </summary>
    [CreateAssetMenu(fileName = "New Timecycle Profile", menuName = "Timecycle Profile")]
    public class TimecycleProfile : ScriptableObject
    {
        [Header("Sun and moon settings")]
        public bool overrideSunlight = false;
        public float maxSunlightIntensity = 1.5f;
        public Gradient sunlightColor = new Gradient();
        [Space]
        public bool overrideMoonlight = false;
        public float maxMoonlightIntensity = 0.3f;
        public Gradient moonlightColor = new Gradient();

        [Header("Environment lighting settings")]
        public bool overrideEnvironmentLighting = false;
        public AmbientLightingOverrideMode lightingOverrideMode = AmbientLightingOverrideMode.Gradient;
        [Space]
        public Gradient lightingColor = new Gradient();
        [Space]
        public Gradient skyColor = new Gradient();
        public Gradient equatorColor = new Gradient();
        public Gradient groundColor = new Gradient();
        [Space]
        public AnimationCurve atmosphereThickness = new AnimationCurve();

        [Header("Fog settings")]
        public bool overrideFog = false;
        public float maxFogDensity = 0.01f;
        public Gradient fogColor = new Gradient();

        public enum AmbientLightingOverrideMode
        {
            Gradient,
            Color
        }
    }
}
