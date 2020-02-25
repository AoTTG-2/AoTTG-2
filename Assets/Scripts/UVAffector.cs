using System;
using UnityEngine;

public class UVAffector : Affector
{
    protected float ElapsedTime;
    protected UVAnimation Frames;
    protected float UVTime;

    public UVAffector(UVAnimation frame, float time, EffectNode node) : base(node)
    {
        this.Frames = frame;
        this.UVTime = time;
    }

    public override void Reset()
    {
        this.ElapsedTime = 0f;
        this.Frames.curFrame = 0;
    }

    public override void Update()
    {
        float num;
        this.ElapsedTime += Time.deltaTime;
        if (this.UVTime <= 0f)
        {
            num = base.Node.GetLifeTime() / ((float) this.Frames.frames.Length);
        }
        else
        {
            num = this.UVTime / ((float) this.Frames.frames.Length);
        }
        if (this.ElapsedTime >= num)
        {
            Vector2 zero = Vector2.zero;
            Vector2 dm = Vector2.zero;
            this.Frames.GetNextFrame(ref zero, ref dm);
            base.Node.LowerLeftUV = zero;
            base.Node.UVDimensions = dm;
            this.ElapsedTime -= num;
        }
    }
}

