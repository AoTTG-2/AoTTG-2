using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class skyModule : MonoBehaviour
{
    //Public: feel free to change this however you like 
    [HorizontalLine]
    [Header("Settings")]
    [SerializeField] private Camera MainCamera;
    [SerializeField] private bool enableReflections;
    [SerializeField] private Material CurrentSkyMat;
    [HorizontalLine]
    [Header("Time")]
    [Range(0, 24)]
    [SerializeField] public float TimeOfDay;
    [SerializeField] public float TimeSpeed;
    [HorizontalLine]
    [Header("Colors")]
    public Gradient AmbiantColor;
    public Gradient SunColor;
    public Gradient FogColor;
    public AnimationCurve SunIntensity;
    public AnimationCurve AtmosphereThickness;
    [HorizontalLine]
    [Header("Clouds")]
    public bool isRandom;
    [Range(0, 1)]
    public float AltoStratus;

    //Private: these are "system" variables and object, please restrain from modifying these as it could possibly fuck up the rest of the script
    private Light sun;
    private Light moon;
    private GameObject Clouds;
    private Material AltoStratMat;
    private ReflectionProbe Reflections;

    private void Start()
    {
        //enableObjects
        RenderSettings.skybox = CurrentSkyMat;
        Reflections.transform.position = new Vector3(0, 0, 0);
        if (enableReflections)
        {
            Reflections.gameObject.SetActive(true);
            Reflections.transform.parent = MainCamera.transform;
            Reflections.transform.localPosition = new Vector3(0, 0, 0);
        }
        else
        {
            Reflections.transform.parent = this.transform;
            Reflections.transform.position = new Vector3(0, 0, 0);
            Reflections.gameObject.SetActive(false);
        }
    }
    private void Update()
    {
        if (Application.isPlaying)
        {
            TimeOfDay += Time.deltaTime * (TimeSpeed/ 864);
            TimeOfDay %= 24;
            UpdateLighting(TimeOfDay / 24f);
            UpdateClouds();
            Clouds.transform.position = new Vector3(MainCamera.transform.position.x, Clouds.transform.position.y, MainCamera.transform.position.z);
        }
        else
        {
            UpdateLighting(TimeOfDay / 24f);
            UpdateClouds();
        }
    }
    //use this method whenever you need to update the current lighting
    private void UpdateLighting(float timePercent)
    {
        RenderSettings.ambientLight = AmbiantColor.Evaluate(timePercent);
        RenderSettings.fogColor = FogColor.Evaluate(timePercent);
        CurrentSkyMat.SetFloat("_AtmosphereThickness", AtmosphereThickness.Evaluate(timePercent));
        if (sun != null && moon != null)
        {
            sun.color = SunColor.Evaluate(timePercent);
            sun.transform.localRotation = Quaternion.Euler(new Vector3((timePercent * 360f) - 90f, 170f, 0));
            sun.intensity = SunIntensity.Evaluate(timePercent);
            moon.intensity = 0.1f * (1 - SunIntensity.Evaluate(timePercent));
        }
    }

    //cloud updating
    private void UpdateClouds()
    {
        AltoStratMat.SetFloat("_Speed", TimeSpeed);
        if (!isRandom)
        {
            AltoStratMat.SetFloat("_CloudCutoff", 1-AltoStratus);
        }
    }
    //just checking for the lights
    private void OnValidate()
    {
        if(sun != null)
            return;
        if(moon != null)
            return;
        if (Clouds != null)
            return;
        if (AltoStratMat != null)
            return;
        AltoStratMat = GameObject.Find("AltoStratus").GetComponent<Renderer>().sharedMaterial;
        sun = GameObject.Find("Sun").GetComponent<Light>();
        moon = GameObject.Find("Moon").GetComponent<Light>();
        Clouds = GameObject.Find("Clouds");
        Reflections = GameObject.Find("VenetianRefProb").GetComponent<ReflectionProbe>();
        if (enableReflections)
        {
            Reflections.gameObject.SetActive(true);
            Reflections.transform.parent = MainCamera.transform;
            Reflections.transform.localPosition = new Vector3(0, 0, 0);
        }
        else
        {
            Reflections.transform.parent = this.transform;
            Reflections.transform.position = new Vector3(0, 0, 0);
            Reflections.gameObject.SetActive(false);
        }
        RenderSettings.skybox = CurrentSkyMat;

    }


    //methods 
    public void EnableReflections(bool isEnabled)
    {
        enableReflections = isEnabled;
        if (isEnabled)
        {
            Reflections.gameObject.SetActive(true);
            Reflections.transform.parent = MainCamera.transform;
            Reflections.transform.position = new Vector3(0, 0, 0);
        }
        else
        {
            Reflections.transform.parent = this.transform;
            Reflections.transform.position = new Vector3(0, 0, 0);
            Reflections.gameObject.SetActive(false);
        }
    }
}
