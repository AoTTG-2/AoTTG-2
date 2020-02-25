using System;
using UnityEngine;

[Serializable]
public class BMSymbol
{
    private int mAdvance;
    private int mHeight;
    private bool mIsValid;
    private int mLength;
    private int mOffsetX;
    private int mOffsetY;
    private UIAtlas.Sprite mSprite;
    private Rect mUV;
    private int mWidth;
    public string sequence;
    public string spriteName;

    public void MarkAsDirty()
    {
        this.mIsValid = false;
    }

    public bool Validate(UIAtlas atlas)
    {
        if (atlas == null)
        {
            return false;
        }
        if (!this.mIsValid)
        {
            if (string.IsNullOrEmpty(this.spriteName))
            {
                return false;
            }
            this.mSprite = (atlas == null) ? null : atlas.GetSprite(this.spriteName);
            if (this.mSprite != null)
            {
                Texture texture = atlas.texture;
                if (texture == null)
                {
                    this.mSprite = null;
                }
                else
                {
                    Rect outer = this.mSprite.outer;
                    this.mUV = outer;
                    if (atlas.coordinates == UIAtlas.Coordinates.Pixels)
                    {
                        this.mUV = NGUIMath.ConvertToTexCoords(this.mUV, texture.width, texture.height);
                    }
                    else
                    {
                        outer = NGUIMath.ConvertToPixels(outer, texture.width, texture.height, true);
                    }
                    this.mOffsetX = Mathf.RoundToInt(this.mSprite.paddingLeft * outer.width);
                    this.mOffsetY = Mathf.RoundToInt(this.mSprite.paddingTop * outer.width);
                    this.mWidth = Mathf.RoundToInt(outer.width);
                    this.mHeight = Mathf.RoundToInt(outer.height);
                    this.mAdvance = Mathf.RoundToInt(outer.width + ((this.mSprite.paddingRight + this.mSprite.paddingLeft) * outer.width));
                    this.mIsValid = true;
                }
            }
        }
        return (this.mSprite != null);
    }

    public int advance
    {
        get
        {
            return this.mAdvance;
        }
    }

    public int height
    {
        get
        {
            return this.mHeight;
        }
    }

    public int length
    {
        get
        {
            if (this.mLength == 0)
            {
                this.mLength = this.sequence.Length;
            }
            return this.mLength;
        }
    }

    public int offsetX
    {
        get
        {
            return this.mOffsetX;
        }
    }

    public int offsetY
    {
        get
        {
            return this.mOffsetY;
        }
    }

    public Rect uvRect
    {
        get
        {
            return this.mUV;
        }
    }

    public int width
    {
        get
        {
            return this.mWidth;
        }
    }
}

