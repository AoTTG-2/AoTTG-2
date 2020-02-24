using System;
using UnityEngine;

public class ColorAffector : Affector
{
    protected Color[] ColorArr;
    protected float ElapsedTime;
    protected float GradualLen;
    protected bool IsNodeLife;
    protected COLOR_GRADUAL_TYPE Type;

    public ColorAffector(Color[] colorArr, float gradualLen, COLOR_GRADUAL_TYPE type, EffectNode node) : base(node)
    {
        this.ColorArr = colorArr;
        this.Type = type;
        this.GradualLen = gradualLen;
        if (this.GradualLen < 0f)
        {
            this.IsNodeLife = true;
        }
    }

    public override void Reset()
    {
        this.ElapsedTime = 0f;
    }

    public override void Update()
    {
        this.ElapsedTime += Time.deltaTime;
        if (this.IsNodeLife)
        {
            this.GradualLen = base.Node.GetLifeTime();
        }
        if (this.GradualLen > 0f)
        {
            if (this.ElapsedTime > this.GradualLen)
            {
                if (this.Type != COLOR_GRADUAL_TYPE.CLAMP)
                {
                    if (this.Type == COLOR_GRADUAL_TYPE.LOOP)
                    {
                        this.ElapsedTime = 0f;
                    }
                    else
                    {
                        Color[] array = new Color[this.ColorArr.Length];
                        this.ColorArr.CopyTo(array, 0);
                        for (int i = 0; i < (array.Length / 2); i++)
                        {
                            this.ColorArr[(array.Length - i) - 1] = array[i];
                            this.ColorArr[i] = array[(array.Length - i) - 1];
                        }
                        this.ElapsedTime = 0f;
                    }
                }
            }
            else
            {
                int index = (int) ((this.ColorArr.Length - 1) * (this.ElapsedTime / this.GradualLen));
                if (index == (this.ColorArr.Length - 1))
                {
                    index--;
                }
                int num3 = index + 1;
                float num4 = this.GradualLen / ((float) (this.ColorArr.Length - 1));
                float t = (this.ElapsedTime - (num4 * index)) / num4;
                base.Node.Color = Color.Lerp(this.ColorArr[index], this.ColorArr[num3], t);
            }
        }
    }
}

