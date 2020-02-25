using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class BMFont
{
    [HideInInspector, SerializeField]
    private int mBase;
    private Dictionary<int, BMGlyph> mDict = new Dictionary<int, BMGlyph>();
    [HideInInspector, SerializeField]
    private int mHeight;
    [SerializeField, HideInInspector]
    private List<BMGlyph> mSaved = new List<BMGlyph>();
    [HideInInspector, SerializeField]
    private int mSize;
    [HideInInspector, SerializeField]
    private string mSpriteName;
    [SerializeField, HideInInspector]
    private int mWidth;

    public void Clear()
    {
        this.mDict.Clear();
        this.mSaved.Clear();
    }

    public BMGlyph GetGlyph(int index)
    {
        return this.GetGlyph(index, false);
    }

    public BMGlyph GetGlyph(int index, bool createIfMissing)
    {
        BMGlyph glyph = null;
        if (this.mDict.Count == 0)
        {
            int num = 0;
            int count = this.mSaved.Count;
            while (num < count)
            {
                BMGlyph glyph2 = this.mSaved[num];
                this.mDict.Add(glyph2.index, glyph2);
                num++;
            }
        }
        if (!this.mDict.TryGetValue(index, out glyph) && createIfMissing)
        {
            glyph = new BMGlyph {
                index = index
            };
            this.mSaved.Add(glyph);
            this.mDict.Add(index, glyph);
        }
        return glyph;
    }

    public void Trim(int xMin, int yMin, int xMax, int yMax)
    {
        if (this.isValid)
        {
            int num = 0;
            int count = this.mSaved.Count;
            while (num < count)
            {
                BMGlyph glyph = this.mSaved[num];
                if (glyph != null)
                {
                    glyph.Trim(xMin, yMin, xMax, yMax);
                }
                num++;
            }
        }
    }

    public int baseOffset
    {
        get
        {
            return this.mBase;
        }
        set
        {
            this.mBase = value;
        }
    }

    public int charSize
    {
        get
        {
            return this.mSize;
        }
        set
        {
            this.mSize = value;
        }
    }

    public int glyphCount
    {
        get
        {
            return (!this.isValid ? 0 : this.mSaved.Count);
        }
    }

    public bool isValid
    {
        get
        {
            return (this.mSaved.Count > 0);
        }
    }

    public string spriteName
    {
        get
        {
            return this.mSpriteName;
        }
        set
        {
            this.mSpriteName = value;
        }
    }

    public int texHeight
    {
        get
        {
            return this.mHeight;
        }
        set
        {
            this.mHeight = value;
        }
    }

    public int texWidth
    {
        get
        {
            return this.mWidth;
        }
        set
        {
            this.mWidth = value;
        }
    }
}

