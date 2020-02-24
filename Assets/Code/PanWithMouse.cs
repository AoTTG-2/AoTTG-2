using System;
using UnityEngine;

[AddComponentMenu("NGUI/Examples/Pan With Mouse")]
public class PanWithMouse : IgnoreTimeScale
{
    public Vector2 degrees = new Vector2(5f, 3f);
    private Vector2 mRot = Vector2.zero;
    private Quaternion mStart;
    private Transform mTrans;
    public float range = 1f;

    private void Start()
    {
        this.mTrans = base.transform;
        this.mStart = this.mTrans.localRotation;
    }

    private void Update()
    {
        float num = base.UpdateRealTimeDelta();
        Vector3 mousePosition = Input.mousePosition;
        float num2 = Screen.width * 0.5f;
        float num3 = Screen.height * 0.5f;
        if (this.range < 0.1f)
        {
            this.range = 0.1f;
        }
        float x = Mathf.Clamp((float) (((mousePosition.x - num2) / num2) / this.range), (float) -1f, (float) 1f);
        float y = Mathf.Clamp((float) (((mousePosition.y - num3) / num3) / this.range), (float) -1f, (float) 1f);
        this.mRot = Vector2.Lerp(this.mRot, new Vector2(x, y), num * 5f);
        this.mTrans.localRotation = this.mStart * Quaternion.Euler(-this.mRot.y * this.degrees.y, this.mRot.x * this.degrees.x, 0f);
    }
}

