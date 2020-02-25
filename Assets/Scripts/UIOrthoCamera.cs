using System;
using UnityEngine;

[ExecuteInEditMode, RequireComponent(typeof(Camera)), AddComponentMenu("NGUI/UI/Orthographic Camera")]
public class UIOrthoCamera : MonoBehaviour
{
    private Camera mCam;
    private Transform mTrans;

    private void Start()
    {
        this.mCam = base.GetComponent<Camera>();
        this.mTrans = base.transform;
        this.mCam.orthographic = true;
    }

    private void Update()
    {
        float num = this.mCam.rect.yMin * Screen.height;
        float num2 = this.mCam.rect.yMax * Screen.height;
        float b = ((num2 - num) * 0.5f) * this.mTrans.lossyScale.y;
        if (!Mathf.Approximately(this.mCam.orthographicSize, b))
        {
            this.mCam.orthographicSize = b;
        }
    }
}

