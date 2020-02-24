using System;
using UnityEngine;

[AddComponentMenu("NGUI/Tween/Rotation")]
public class TweenRotation : UITweener
{
    public Vector3 from;
    private Transform mTrans;
    public Vector3 to;

    public static TweenRotation Begin(GameObject go, float duration, Quaternion rot)
    {
        TweenRotation rotation = UITweener.Begin<TweenRotation>(go, duration);
        rotation.from = rotation.rotation.eulerAngles;
        rotation.to = rot.eulerAngles;
        if (duration <= 0f)
        {
            rotation.Sample(1f, true);
            rotation.enabled = false;
        }
        return rotation;
    }

    protected override void OnUpdate(float factor, bool isFinished)
    {
        this.cachedTransform.localRotation = Quaternion.Slerp(Quaternion.Euler(this.from), Quaternion.Euler(this.to), factor);
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

    public Quaternion rotation
    {
        get
        {
            return this.cachedTransform.localRotation;
        }
        set
        {
            this.cachedTransform.localRotation = value;
        }
    }
}

