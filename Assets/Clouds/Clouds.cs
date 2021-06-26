using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Clouds : MonoBehaviour
{
    [Header("Material Settings")]
    public float CloudTilling = 1;
    public float CloudsSpeed = 1;
    [Range(0, 360)]
    public float WindDirection = 86;
    [Space]
    [Range(0, 1)]
    public float CloudLumping = 0.169f;
    [Range(0, 1)]
    public float Overcasting = 0.073f;
    [Range(0, 3)]
    public float CloudLumpingHardness = 0.33f;
    public float CloudLumpsSize = 1;
    public float LumpVariationSpeed = 10;
    [Space]
    [Range(0, 1)]
    public float BigDetailsWeight = 0.324f;
    [Range(0, 1)]
    public float SmallPerturbationsWeight = 0.108f;
    [Range(0, 1)]
    public float CloudSoftness = 0.49f / 3;
    public float CloudRoundness = 1.2f;
    [Space]
    [Range(0, 10)]
    public float CloudDensity = 2.27f;
    [Range(0, 100)]
    public float LightScatteringDiffusion = 8.3f;
    [Range(0, 1)]
    public float LightScatteringBrightness = 0.61f;

    [Header("fog Settings")]
    [Range(0, 1)]
    public float fogDistance = 0.018f;
    [Range(0, 3)]
    public float fogFallofSoftness = 0.63f;


    [Header("Volume settings")]
    public int CloudVolumeSize = 50;
    private float Height;
    public float CloudVolumeHeight = 2;
    public bool AutoCalculateVolumeHeight;

    [Header("Object Settings")]
    public Mesh QuadMesh;
    public int layer;
    private Matrix4x4 matrix;
    private Vector3 StartPos;
    public Material HighQuality;
    public Material MediumQuality;
    public Material LowQuality;
    private Material[] CloudMats;
    [Range(0, 2)]
    public int CurrentQuality = 1;
    private Camera cam;
    private Material CurrentMat;
    public bool ForceUpdateClouds;

    // Start is called before the first frame update
    void Start()
    {
        prepCalculations();
    }

    // Update is called once per frame
    void Update()
    {
        RenderClouds();
        if (ForceUpdateClouds)
        {
            prepCalculations();
            RenderClouds();
            ForceUpdateClouds = false;
        }
    }
    private void OnValidate()
    {
        prepCalculations();
    }
    private void prepCalculations()
    {
        CloudMats = new Material[3];
        CloudMats[0] = HighQuality;
        CloudMats[1] = MediumQuality;
        CloudMats[2] = LowQuality;
        Height = transform.position.y;
        if (AutoCalculateVolumeHeight)
        {
            CloudVolumeHeight = Height / CloudVolumeSize / 2f;
        }
        cam = Camera.main;
        StartPos = transform.position + (Vector3.up * (CloudVolumeHeight * CloudVolumeSize / 2f));
        CurrentMat = CloudMats[CurrentQuality];
        CurrentMat.SetFloat("_midYValue", transform.position.y);
        CurrentMat.SetFloat("_cloudHeight", transform.position.y);
        CalculateMaterialSettings();
        RenderClouds();
    }
    private void RenderClouds()
    {
        
        for (int i = 0; i < CloudVolumeSize; i++)
        {
            matrix = Matrix4x4.TRS(StartPos - (Vector3.up * CloudVolumeHeight * i), transform.rotation, transform.lossyScale);
            Graphics.DrawMesh(QuadMesh, matrix, CurrentMat, layer, cam, 0, null, true, false, false);
        }
    }

    private void CalculateMaterialSettings()
    {
        CurrentMat.SetFloat("_WindDirection", (WindDirection / 360) * 6.28f);
        CurrentMat.SetFloat("_MainShapeSpeed", 3 * CloudsSpeed);
        CurrentMat.SetFloat("_DetailShapeSpeed", 2 * CloudsSpeed);
        
        CurrentMat.SetFloat("_MainShapeTilling", 5467 * CloudTilling);
        CurrentMat.SetFloat("_DetailShapeTilling", 1000 * CloudTilling);
        CurrentMat.SetFloat("_WeatherSize", CloudLumping);
        CurrentMat.SetFloat("_WeatherSoftness", CloudLumpingHardness);
        CurrentMat.SetFloat("_Weathertilling", 29471 * CloudLumpsSize);
        CurrentMat.SetFloat("_Weathervariationspeed", LumpVariationSpeed);
        CurrentMat.SetFloat("_Detailsweight", BigDetailsWeight);
        CurrentMat.SetFloat("_Perturbationsweight", SmallPerturbationsWeight);
        CurrentMat.SetFloat("_CloudSoftness", CloudSoftness * 3);
        CurrentMat.SetFloat("_CloudRoundness", CloudRoundness);
        CurrentMat.SetFloat("_CloudDensity", CloudDensity);
        CurrentMat.SetFloat("_CloudSize", Overcasting);
        
        
        if(CurrentQuality < 2)
        {
            if(CurrentQuality == 1)
            {
                CurrentMat.SetFloat("_PerturbationsTilling", 167.7f * CloudTilling);//=================
                CurrentMat.SetFloat("_PerturbationSpeed", 1.5f * CloudsSpeed);//=================
            }
            CurrentMat.SetFloat("_SSSDiffusion", LightScatteringDiffusion);
            CurrentMat.SetFloat("_FogSoftness", fogFallofSoftness);
            CurrentMat.SetFloat("_FogDistance", fogDistance);
            CurrentMat.SetFloat("_SSSbrightness", LightScatteringBrightness);
        }
    }
}
