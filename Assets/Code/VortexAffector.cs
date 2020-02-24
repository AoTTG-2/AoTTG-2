using System;
using UnityEngine;

public class VortexAffector : Affector
{
    protected Vector3 Direction;
    private float Magnitude;
    private bool UseCurve;
    private AnimationCurve VortexCurve;

    public VortexAffector(float mag, Vector3 dir, EffectNode node) : base(node)
    {
        this.Magnitude = mag;
        this.Direction = dir;
        this.UseCurve = false;
    }

    public VortexAffector(AnimationCurve vortexCurve, Vector3 dir, EffectNode node) : base(node)
    {
        this.VortexCurve = vortexCurve;
        this.Direction = dir;
        this.UseCurve = true;
    }

    public override void Update()
    {
        Vector3 rhs = base.Node.GetLocalPosition() - base.Node.Owner.EmitPoint;
        if (rhs.magnitude != 0f)
        {
            float magnitude;
            float num2 = Vector3.Dot(this.Direction, rhs);
            rhs -= (Vector3) (num2 * this.Direction);
            Vector3 zero = Vector3.zero;
            if (rhs == Vector3.zero)
            {
                zero = rhs;
            }
            else
            {
                zero = Vector3.Cross(this.Direction, rhs).normalized;
            }
            float elapsedTime = base.Node.GetElapsedTime();
            if (this.UseCurve)
            {
                magnitude = this.VortexCurve.Evaluate(elapsedTime);
            }
            else
            {
                magnitude = this.Magnitude;
            }
            zero = (Vector3) (zero * (magnitude * Time.deltaTime));
            base.Node.Position += zero;
        }
    }
}

