using System;
using System.Runtime.CompilerServices;
using UnityEngine;

[AddComponentMenu("NGUI/Tween/Spring Position")]
public class SpringPosition : IgnoreTimeScale
{
    public string callWhenFinished;
    public GameObject eventReceiver;
    public bool ignoreTimeScale;
    private float mThreshold;
    private Transform mTrans;
    public OnFinished onFinished;
    public float strength = 10f;
    public Vector3 target = Vector3.zero;
    public bool worldSpace;

    public static SpringPosition Begin(GameObject go, Vector3 pos, float strength)
    {
        SpringPosition component = go.GetComponent<SpringPosition>();
        if (component == null)
        {
            component = go.AddComponent<SpringPosition>();
        }
        component.target = pos;
        component.strength = strength;
        component.onFinished = null;
        if (!component.enabled)
        {
            component.mThreshold = 0f;
            component.enabled = true;
        }
        return component;
    }

    private void Start()
    {
        this.mTrans = base.transform;
    }

    private void Update()
    {
        float deltaTime = !this.ignoreTimeScale ? Time.deltaTime : base.UpdateRealTimeDelta();
        if (this.worldSpace)
        {
            if (this.mThreshold == 0f)
            {
                Vector3 vector = this.target - this.mTrans.position;
                this.mThreshold = vector.magnitude * 0.001f;
            }
            this.mTrans.position = NGUIMath.SpringLerp(this.mTrans.position, this.target, this.strength, deltaTime);
            Vector3 vector2 = this.target - this.mTrans.position;
            if (this.mThreshold >= vector2.magnitude)
            {
                this.mTrans.position = this.target;
                if (this.onFinished != null)
                {
                    this.onFinished(this);
                }
                if ((this.eventReceiver != null) && !string.IsNullOrEmpty(this.callWhenFinished))
                {
                    this.eventReceiver.SendMessage(this.callWhenFinished, this, SendMessageOptions.DontRequireReceiver);
                }
                base.enabled = false;
            }
        }
        else
        {
            if (this.mThreshold == 0f)
            {
                Vector3 vector3 = this.target - this.mTrans.localPosition;
                this.mThreshold = vector3.magnitude * 0.001f;
            }
            this.mTrans.localPosition = NGUIMath.SpringLerp(this.mTrans.localPosition, this.target, this.strength, deltaTime);
            Vector3 vector4 = this.target - this.mTrans.localPosition;
            if (this.mThreshold >= vector4.magnitude)
            {
                this.mTrans.localPosition = this.target;
                if (this.onFinished != null)
                {
                    this.onFinished(this);
                }
                if ((this.eventReceiver != null) && !string.IsNullOrEmpty(this.callWhenFinished))
                {
                    this.eventReceiver.SendMessage(this.callWhenFinished, this, SendMessageOptions.DontRequireReceiver);
                }
                base.enabled = false;
            }
        }
    }

    public delegate void OnFinished(SpringPosition spring);
}

