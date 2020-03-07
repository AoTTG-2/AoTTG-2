using System;
using UnityEngine;

public class ScaleAffector : Affector
{
    protected float DeltaX;
    protected float DeltaY;
    protected AnimationCurve ScaleXCurve;
    protected AnimationCurve ScaleYCurve;
    protected RSTYPE Type;

    public ScaleAffector(float x, float y, EffectNode node) : base(node)
    {
        this.Type = RSTYPE.SIMPLE;
        this.DeltaX = x;
        this.DeltaY = y;
    }

    public ScaleAffector(AnimationCurve curveX, AnimationCurve curveY, EffectNode node) : base(node)
    {
        this.Type = RSTYPE.CURVE;
        this.ScaleXCurve = curveX;
        this.ScaleYCurve = curveY;
    }

    public override void Update()
    {
        float elapsedTime = base.Node.GetElapsedTime();
        if (this.Type == RSTYPE.CURVE)
        {
            if (this.ScaleXCurve != null)
            {
                base.Node.Scale.x = this.ScaleXCurve.Evaluate(elapsedTime);
            }
            if (this.ScaleYCurve != null)
            {
                base.Node.Scale.y = this.ScaleYCurve.Evaluate(elapsedTime);
            }
        }
        else if (this.Type == RSTYPE.SIMPLE)
        {
            float num2 = base.Node.Scale.x + (this.DeltaX * Time.deltaTime);
            float num3 = base.Node.Scale.y + (this.DeltaY * Time.deltaTime);
            if ((num2 * base.Node.Scale.x) > 0f)
            {
                base.Node.Scale.x = num2;
            }
            if ((num3 * base.Node.Scale.y) > 0f)
            {
                base.Node.Scale.y = num3;
            }
        }
    }
}

