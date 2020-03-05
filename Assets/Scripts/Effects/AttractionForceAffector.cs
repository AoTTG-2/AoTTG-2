using System;
using UnityEngine;

public class AttractionForceAffector : Affector
{
    private AnimationCurve AttractionCurve;
    private float Magnitude;
    protected Vector3 Position;
    private bool UseCurve;

    public AttractionForceAffector(float magnitude, Vector3 pos, EffectNode node) : base(node)
    {
        this.Magnitude = magnitude;
        this.Position = pos;
        this.UseCurve = false;
    }

    public AttractionForceAffector(AnimationCurve curve, Vector3 pos, EffectNode node) : base(node)
    {
        this.AttractionCurve = curve;
        this.Position = pos;
        this.UseCurve = true;
    }

    public override void Update()
    {
        Vector3 vector;
        float magnitude;
        if (base.Node.SyncClient)
        {
            vector = this.Position - base.Node.GetLocalPosition();
        }
        else
        {
            vector = (base.Node.ClientTrans.position + this.Position) - base.Node.GetLocalPosition();
        }
        float elapsedTime = base.Node.GetElapsedTime();
        if (this.UseCurve)
        {
            magnitude = this.AttractionCurve.Evaluate(elapsedTime);
        }
        else
        {
            magnitude = this.Magnitude;
        }
        float num3 = magnitude;
        base.Node.Velocity += (Vector3) ((vector.normalized * num3) * Time.deltaTime);
    }
}

