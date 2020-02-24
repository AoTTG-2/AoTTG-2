using System;
using UnityEngine;

public class LinearForceAffector : Affector
{
    protected Vector3 Force;

    public LinearForceAffector(Vector3 force, EffectNode node) : base(node)
    {
        this.Force = force;
    }

    public override void Update()
    {
        base.Node.Velocity += (Vector3) (this.Force * Time.deltaTime);
    }
}

