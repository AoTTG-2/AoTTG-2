using System;
using UnityEngine;

[AddComponentMenu("NGUI/Tween/Transform")]
public class TweenTransform : UITweener
{
    public Transform from;
    private Vector3 mPos;
    private Quaternion mRot;
    private Vector3 mScale;
    private Transform mTrans;
    public bool parentWhenFinished;
    public Transform to;

    public static TweenTransform Begin(GameObject go, float duration, Transform to)
    {
        return Begin(go, duration, null, to);
    }

    public static TweenTransform Begin(GameObject go, float duration, Transform from, Transform to)
    {
        TweenTransform transform = UITweener.Begin<TweenTransform>(go, duration);
        transform.from = from;
        transform.to = to;
        if (duration <= 0f)
        {
            transform.Sample(1f, true);
            transform.enabled = false;
        }
        return transform;
    }

    protected override void OnUpdate(float factor, bool isFinished)
    {
        if (this.to != null)
        {
            if (this.mTrans == null)
            {
                this.mTrans = base.transform;
                this.mPos = this.mTrans.position;
                this.mRot = this.mTrans.rotation;
                this.mScale = this.mTrans.localScale;
            }
            if (this.from != null)
            {
                this.mTrans.position = (Vector3) ((this.from.position * (1f - factor)) + (this.to.position * factor));
                this.mTrans.localScale = (Vector3) ((this.from.localScale * (1f - factor)) + (this.to.localScale * factor));
                this.mTrans.rotation = Quaternion.Slerp(this.from.rotation, this.to.rotation, factor);
            }
            else
            {
                this.mTrans.position = (Vector3) ((this.mPos * (1f - factor)) + (this.to.position * factor));
                this.mTrans.localScale = (Vector3) ((this.mScale * (1f - factor)) + (this.to.localScale * factor));
                this.mTrans.rotation = Quaternion.Slerp(this.mRot, this.to.rotation, factor);
            }
            if (this.parentWhenFinished && isFinished)
            {
                this.mTrans.parent = this.to;
            }
        }
    }
}

