using System;
using System.Collections.Generic;

[Serializable]
public class BMGlyph
{
    public int advance;
    public int channel;
    public int height;
    public int index;
    public List<int> kerning;
    public int offsetX;
    public int offsetY;
    public int width;
    public int x;
    public int y;

    public int GetKerning(int previousChar)
    {
        if (this.kerning != null)
        {
            int num = 0;
            int count = this.kerning.Count;
            while (num < count)
            {
                if (this.kerning[num] == previousChar)
                {
                    return this.kerning[num + 1];
                }
                num += 2;
            }
        }
        return 0;
    }

    public void SetKerning(int previousChar, int amount)
    {
        if (this.kerning == null)
        {
            this.kerning = new List<int>();
        }
        for (int i = 0; i < this.kerning.Count; i += 2)
        {
            if (this.kerning[i] == previousChar)
            {
                this.kerning[i + 1] = amount;
                return;
            }
        }
        this.kerning.Add(previousChar);
        this.kerning.Add(amount);
    }

    public void Trim(int xMin, int yMin, int xMax, int yMax)
    {
        int num = this.x + this.width;
        int num2 = this.y + this.height;
        if (this.x < xMin)
        {
            int num3 = xMin - this.x;
            this.x += num3;
            this.width -= num3;
            this.offsetX += num3;
        }
        if (this.y < yMin)
        {
            int num4 = yMin - this.y;
            this.y += num4;
            this.height -= num4;
            this.offsetY += num4;
        }
        if (num > xMax)
        {
            this.width -= num - xMax;
        }
        if (num2 > yMax)
        {
            this.height -= num2 - yMax;
        }
    }
}

