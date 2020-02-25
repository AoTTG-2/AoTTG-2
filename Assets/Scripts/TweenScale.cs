using System;
using UnityEngine;

[AddComponentMenu("NGUI/Tween/Scale")]
public class TweenScale : UITweener
{
    public Vector3 from = Vector3.one;
    private UITable mTable;
    private Transform mTrans;
    public Vector3 to = Vector3.one;
    public bool updateTable;

    public static TweenScale Begin(GameObject go, float duration, Vector3 scale)
    {
        TweenScale scale2 = UITweener.Begin<TweenScale>(go, duration);
        scale2.from = scale2.scale;
        scale2.to = scale;
        if (duration <= 0f)
        {
            scale2.Sample(1f, true);
            scale2.enabled = false;
        }
        return scale2;
    }

    protected override void OnUpdate(float factor, bool isFinished)
    {
        this.cachedTransform.localScale = (Vector3) ((this.from * (1f - factor)) + (this.to * factor));
        if (this.updateTable)
        {
            if (this.mTable == null)
            {
                this.mTable = NGUITools.FindInParents<UITable>(base.gameObject);
                if (this.mTable == null)
                {
                    this.updateTable = false;
                    return;
                }
            }
            this.mTable.repositionNow = true;
        }
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

    public Vector3 scale
    {
        get
        {
            return this.cachedTransform.localScale;
        }
        set
        {
            this.cachedTransform.localScale = value;
        }
    }
}

