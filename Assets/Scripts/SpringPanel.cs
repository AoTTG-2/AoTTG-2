using System;
using System.Runtime.CompilerServices;
using UnityEngine;

[AddComponentMenu("NGUI/Internal/Spring Panel"), RequireComponent(typeof(UIPanel))]
public class SpringPanel : IgnoreTimeScale
{
    private UIDraggablePanel mDrag;
    private UIPanel mPanel;
    private float mThreshold;
    private Transform mTrans;
    public OnFinished onFinished;
    public float strength = 10f;
    public Vector3 target = Vector3.zero;

    public static SpringPanel Begin(GameObject go, Vector3 pos, float strength)
    {
        SpringPanel component = go.GetComponent<SpringPanel>();
        if (component == null)
        {
            component = go.AddComponent<SpringPanel>();
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
        this.mPanel = base.GetComponent<UIPanel>();
        this.mDrag = base.GetComponent<UIDraggablePanel>();
        this.mTrans = base.transform;
    }

    private void Update()
    {
        float deltaTime = base.UpdateRealTimeDelta();
        if (this.mThreshold == 0f)
        {
            Vector3 vector = this.target - this.mTrans.localPosition;
            this.mThreshold = vector.magnitude * 0.005f;
        }
        bool flag = false;
        Vector3 localPosition = this.mTrans.localPosition;
        Vector3 target = NGUIMath.SpringLerp(this.mTrans.localPosition, this.target, this.strength, deltaTime);
        if (this.mThreshold >= Vector3.Magnitude(target - this.target))
        {
            target = this.target;
            base.enabled = false;
            flag = true;
        }
        this.mTrans.localPosition = target;
        Vector3 vector4 = target - localPosition;
        Vector4 clipRange = this.mPanel.clipRange;
        clipRange.x -= vector4.x;
        clipRange.y -= vector4.y;
        this.mPanel.clipRange = clipRange;
        if (this.mDrag != null)
        {
            this.mDrag.UpdateScrollbars(false);
        }
        if (flag && (this.onFinished != null))
        {
            this.onFinished();
        }
    }

    public delegate void OnFinished();
}

