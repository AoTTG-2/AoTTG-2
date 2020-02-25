using System;
using UnityEngine;

[AddComponentMenu("NGUI/Examples/Shader Quality"), ExecuteInEditMode]
public class ShaderQuality : MonoBehaviour
{
    private int mCurrent = 600;

    private void Update()
    {
        int num = (QualitySettings.GetQualityLevel() + 1) * 100;
        if (this.mCurrent != num)
        {
            this.mCurrent = num;
            Shader.globalMaximumLOD = this.mCurrent;
        }
    }
}

