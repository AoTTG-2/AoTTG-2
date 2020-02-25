using System;
using UnityEngine;

[AddComponentMenu("NGUI/Tween/Color")]
public class TweenColor : UITweener
{
    public Color from = Color.white;
    private Light mLight;
    private Material mMat;
    public Transform mTrans;
    private UIWidget mWidget;
    public Color to = Color.white;

    private void Awake()
    {
        this.mWidget = base.GetComponentInChildren<UIWidget>();
        Renderer renderer = base.GetComponent<Renderer>();
        if (renderer != null)
        {
            this.mMat = renderer.material;
        }
        this.mLight = base.GetComponent<Light>();
    }

    public static TweenColor Begin(GameObject go, float duration, Color color)
    {
        TweenColor color2 = UITweener.Begin<TweenColor>(go, duration);
        color2.from = color2.color;
        color2.to = color;
        if (duration <= 0f)
        {
            color2.Sample(1f, true);
            color2.enabled = false;
        }
        return color2;
    }

    protected override void OnUpdate(float factor, bool isFinished)
    {
        this.color = Color.Lerp(this.from, this.to, factor);
    }

    public Color color
    {
        get
        {
            if (this.mWidget != null)
            {
                return this.mWidget.color;
            }
            if (this.mLight != null)
            {
                return this.mLight.color;
            }
            if (this.mMat != null)
            {
                return this.mMat.color;
            }
            return Color.black;
        }
        set
        {
            if (this.mWidget != null)
            {
                this.mWidget.color = value;
            }
            if (this.mMat != null)
            {
                this.mMat.color = value;
            }
            if (this.mLight != null)
            {
                this.mLight.color = value;
                this.mLight.enabled = ((value.r + value.g) + value.b) > 0.01f;
            }
        }
    }
}

