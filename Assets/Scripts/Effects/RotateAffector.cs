using System;
using UnityEngine;

public class RotateAffector : Affector
{
    protected float Delta;
    protected AnimationCurve RotateCurve;
    protected RSTYPE Type;

    public RotateAffector(float delta, EffectNode node) : base(node)
    {
        this.Type = RSTYPE.SIMPLE;
        this.Delta = delta;
    }

    public RotateAffector(AnimationCurve curve, EffectNode node) : base(node)
    {
        this.Type = RSTYPE.CURVE;
        this.RotateCurve = curve;
    }

    public override void Update()
    {
        float elapsedTime = base.Node.GetElapsedTime();
        if (this.Type == RSTYPE.CURVE)
        {
            base.Node.RotateAngle = (int) this.RotateCurve.Evaluate(elapsedTime);
        }
        else if (this.Type == RSTYPE.SIMPLE)
        {
            float num2 = base.Node.RotateAngle + (this.Delta * Time.deltaTime);
            base.Node.RotateAngle = num2;
        }
    }
}

