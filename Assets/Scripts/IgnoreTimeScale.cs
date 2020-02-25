using System;
using UnityEngine;

[AddComponentMenu("NGUI/Internal/Ignore TimeScale Behaviour")]
public class IgnoreTimeScale : MonoBehaviour
{
    private float mActual;
    private float mRt;
    private float mTimeDelta;
    private float mTimeStart;
    private bool mTimeStarted;

    protected virtual void OnEnable()
    {
        this.mTimeStarted = true;
        this.mTimeDelta = 0f;
        this.mTimeStart = Time.realtimeSinceStartup;
    }

    protected float UpdateRealTimeDelta()
    {
        this.mRt = Time.realtimeSinceStartup;
        if (this.mTimeStarted)
        {
            float b = this.mRt - this.mTimeStart;
            this.mActual += Mathf.Max(0f, b);
            this.mTimeDelta = 0.001f * Mathf.Round(this.mActual * 1000f);
            this.mActual -= this.mTimeDelta;
            if (this.mTimeDelta > 1f)
            {
                this.mTimeDelta = 1f;
            }
            this.mTimeStart = this.mRt;
        }
        else
        {
            this.mTimeStarted = true;
            this.mTimeStart = this.mRt;
            this.mTimeDelta = 0f;
        }
        return this.mTimeDelta;
    }

    public float realTime
    {
        get
        {
            return this.mRt;
        }
    }

    public float realTimeDelta
    {
        get
        {
            return this.mTimeDelta;
        }
    }
}

