using System;
using UnityEngine;

[AddComponentMenu("NGUI/Tween/Alpha")]
public class TweenAlpha : UITweener
{
    public float from = 1f;
    private UIPanel mPanel;
    public Transform mTrans;
    private UIWidget mWidget;
    public float to = 1f;

    private void Awake()
    {
        this.mPanel = base.GetComponent<UIPanel>();
        if (this.mPanel == null)
        {
            this.mWidget = base.GetComponentInChildren<UIWidget>();
        }
    }

    public static TweenAlpha Begin(GameObject go, float duration, float alpha)
    {
        TweenAlpha alpha2 = UITweener.Begin<TweenAlpha>(go, duration);
        alpha2.from = alpha2.alpha;
        alpha2.to = alpha;
        if (duration <= 0f)
        {
            alpha2.Sample(1f, true);
            alpha2.enabled = false;
        }
        return alpha2;
    }

    protected override void OnUpdate(float factor, bool isFinished)
    {
        this.alpha = Mathf.Lerp(this.from, this.to, factor);
    }

    public float alpha
    {
        get
        {
            if (this.mWidget != null)
            {
                return this.mWidget.alpha;
            }
            if (this.mPanel != null)
            {
                return this.mPanel.alpha;
            }
            return 0f;
        }
        set
        {
            if (this.mWidget != null)
            {
                this.mWidget.alpha = value;
            }
            else if (this.mPanel != null)
            {
                this.mPanel.alpha = value;
            }
        }
    }
}

