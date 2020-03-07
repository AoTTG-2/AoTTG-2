using System;

public class Affector
{
    protected EffectNode Node;

    public Affector(EffectNode node)
    {
        this.Node = node;
    }

    public virtual void Reset()
    {
    }

    public virtual void Update()
    {
    }
}

