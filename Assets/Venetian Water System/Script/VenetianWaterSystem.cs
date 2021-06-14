using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class VenetianWaterSystem : MonoBehaviour
{
    //Basic Settings
    [Header("Basic Settings")]
    [Space]
    [SerializeField] [Range(0, 1)] public float smoothness = 0.98f;
    //Wave Settings =================================================================================================================================================================
    [Space]
    [Header("Wave Settings")]
    [Space]
    [Range(0, 12)]
    [Tooltip("Automatically calculates wave relations using a single value based on baufort scale, for more precise wave control, go into Advanced/PerWaveSettings")]
    [SerializeField] public float WaveIntensity = 5.49f;
    [HideInInspector] public float SmallWaveIntensity;//automatically calculated
    [HideInInspector] public float MediumWaveIntensity;//automatically calculated
    [HideInInspector] public float LargeWaveIntensity;//automatically calculated
    [SerializeField] public float WaveSpeed = 1;
    [HideInInspector] public float SmallWavespeed;//automatically calculated
    [HideInInspector] public float MediumWavespeed;//automatically calculated
    [HideInInspector] public float LargeWavespeed;//automatically calculated
    [Range(0,1)]
    [Tooltip("Changes the flow of water, low values means the water is flowing, high values mean the water is static, you can change flow direction using the flow direction field")]
    [SerializeField] public float WaterFlow = 1;
    [Range(0, 360)]
    [SerializeField] public float FlowDirection = 0;
    private float SaturatedDir;
    [SerializeField] public float WaveSize = 2;
    private float LargeSize;
    private float MediumSize;
    private float SmallSize;

    //Transparency settings =================================================================================================================================================================
    [Space]
    [Header("TransparencySettings")]
    [Space]
    [Tooltip("Note that this is used to calculate the color absorbed by the water, so it will look different depending on the light color")]
    [SerializeField] public Color WaterColor = new Color(0, 1/255, 2/255, 1);
    [SerializeField] public Color LifeColor = new Color(28/255, 1, 45/255, 1);
    private Color Absorbed;
    private Color Life;
    [Range(0,1)]
    [SerializeField] public float WaterDensity = 0.076f;
    [Range(0,1)]
    [SerializeField] public float LifeDensity = 0.382f;
    [Range(0, 1)]
    [SerializeField] public float WaterTransparancy = 1;
    [Range(0, 1)]
    [SerializeField] public float EdgeBlending = 0.286f;

    //Foam settings ====================================================================================================================================================================================
    [Space]
    [Header("Foam Settings")]
    [Space]
    [Range(0, 1)]
    [SerializeField] public float EdgeFoamDistance = 0.136f;
    [Range(0, 1)]
    [SerializeField] public float WaveFoam = 0;
    [Range(0, 1)]
    [SerializeField] public float FoamSoftness = 0.143f;
    [SerializeField] public float FoamTilling = 10;
    [SerializeField] public Color FoamColor = new Color(212/255, 243/255, 245/255, 1);

    //Underwater Scattering =======================================================================================================================================================
    [Range(0, 1)]
    [SerializeField] public float UnderWaterScatteringSize = 0.5f;
    [Range(0, 1)]
    [SerializeField] public float ScatteringBrightness = 0.14f;
    private float ScatteringPower;
    private float ScatteringScale;
    private float ScratteringBrightness;


    



    [HideInInspector] public Material Water;
    void Start()
    {
        CheckVars();
        CalculateWaves();
        SetMatVars();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Auto Scattering Calculations
    private void CalculateScattering()
    {
        ScatteringPower = (20.52f * 2) * UnderWaterScatteringSize;
        ScatteringScale = (3 * 2) * UnderWaterScatteringSize;
    }
    //Auto Color Calculations
    private void CalculateColors()
    {
        Absorbed = new Color(1 - WaterColor.r, 1 - WaterColor.g, 1 - WaterColor.b, 1);
        Life = new Color(1 - LifeColor.r, 1 - LifeColor.g, 1 - LifeColor.b, 1);
    }
    //Auto wave calculations
    private void CalculateWaves()
    {
        SmallWaveIntensity = (Mathf.Log(WaveIntensity + 0.126f)/8) + 0.26f;
        MediumWaveIntensity = WaveIntensity / 8;
        LargeWaveIntensity = Mathf.Pow(WaveIntensity/8, 1.2f);
        SmallWavespeed = (0.15f * WaveSpeed);
        MediumWavespeed = (1 * WaveSpeed);
        LargeWavespeed = (2 * WaveSpeed);
        SaturatedDir = FlowDirection / 360 * 6.24f;
        SmallSize = 4.63f * WaveSize;
        MediumSize = 22.32f * WaveSize;
        LargeSize = 171 * WaveSize;
        
    }

    private void CheckVars()
    {
        Water = GetComponent<Renderer>().sharedMaterial;
    }

    private void SetMatVars()
    {
        //waves
        Water.SetFloat("_SmallWavesHeight", SmallWaveIntensity);
        Water.SetFloat("_MediumWavesHeight", MediumWaveIntensity);
        Water.SetFloat("_LargeWavesHeight", LargeWaveIntensity);
        Water.SetFloat("_SmallWaveSpeed", SmallWavespeed);
        Water.SetFloat("_MediumWaveSpeed", MediumWavespeed);
        Water.SetFloat("_LargeWaveSpeed", LargeWavespeed);
        Water.SetFloat("_SmallWavesFlow", WaterFlow);
        Water.SetFloat("_MediumWavesFlow", WaterFlow);
        Water.SetFloat("_LargeWavesFlow", WaterFlow);
        Water.SetFloat("_LargeWaveDirection", SaturatedDir);
        Water.SetFloat("_MediumWaveDirection", SaturatedDir);
        Water.SetFloat("_SmallWaveDirection", SaturatedDir);
        Water.SetFloat("_largeWavesTilling", LargeSize);
        Water.SetFloat("_SmallWavesTilling", SmallSize);
        Water.SetFloat("_MediumWavesTilling", MediumSize);
        //Transparency
        Water.SetColor("_WaterAbsorbedColor", Absorbed);
        Water.SetColor("_LifeColor", Life);
        Water.SetFloat("_WaterDensity", WaterDensity);
        Water.SetFloat("_LifeDensity", LifeDensity);
        Water.SetFloat("_WaterDepth", WaterTransparancy * 0.795f);
        Water.SetFloat("_LifeDepth", WaterTransparancy * 0.2f);
        Water.SetFloat("_EdgeBlending", EdgeBlending);
        //foam
        Water.SetFloat("_FoamDistance", EdgeFoamDistance * 10);
        Water.SetFloat("_WaveFoam", WaveFoam);
        Water.SetFloat("_FoamTilling", FoamTilling);
        Water.SetFloat("_FoamSoftness", FoamSoftness);
        Water.SetColor("_FoamColor", FoamColor);
        //Scattering
        Water.SetFloat("_Scatteringpower", ScatteringPower);
        Water.SetFloat("_ScatteringScale", ScatteringScale);
        Water.SetFloat("_Scatteringbrightness", ScatteringBrightness);
        //Smoothness
        Water.SetFloat("_WaterSmoothness", smoothness);

    }

    private void OnValidate()
    {
        CheckVars();
        CalculateWaves();
        CalculateColors();
        CalculateScattering();
        SetMatVars();
    }

}
