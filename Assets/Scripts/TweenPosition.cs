using System;
using UnityEngine;

[AddComponentMenu("NGUI/Tween/Position")]
public class TweenPosition : UITweener
{
    public Vector3 from;
    private Transform mTrans;
    public Vector3 to;

    public static TweenPosition Begin(GameObject go, float duration, Vector3 pos)
    {
        TweenPosition position = UITweener.Begin<TweenPosition>(go, duration);
        position.from = position.position;
        position.to = pos;
        if (duration <= 0f)
        {
            position.Sample(1f, true);
            position.enabled = false;
        }
        return position;
    }

    protected override void OnUpdate(float factor, bool isFinished)
    {
        this.cachedTransform.localPosition = (Vector3) ((this.from * (1f - factor)) + (this.to * factor));
    }

    public Transform cachedTransform
    {
        get
        {
            if (this.mTrans == null)
            {
                this.mTrans = base.transform;
            }
            return this.mTrans;
        }
    }

    public Vector3 position
    {
        get
        {
            return this.cachedTransform.localPosition;
        }
        set
        {
            this.cachedTransform.localPosition = value;
        }
    }
}

