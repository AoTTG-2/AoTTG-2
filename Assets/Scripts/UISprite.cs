using System;
using UnityEngine;

[ExecuteInEditMode, AddComponentMenu("NGUI/UI/Sprite")]
public class UISprite : UIWidget
{
    [SerializeField, HideInInspector]
    private UIAtlas mAtlas;
    [SerializeField, HideInInspector]
    private float mFillAmount = 1f;
    [HideInInspector, SerializeField]
    private bool mFillCenter = true;
    [SerializeField, HideInInspector]
    private FillDirection mFillDirection = FillDirection.Radial360;
    protected Rect mInner;
    protected Rect mInnerUV;
    [SerializeField, HideInInspector]
    private bool mInvert;
    protected Rect mOuter;
    protected Rect mOuterUV;
    protected Vector3 mScale = Vector3.one;
    protected UIAtlas.Sprite mSprite;
    [HideInInspector, SerializeField]
    private string mSpriteName;
    private bool mSpriteSet;
    [HideInInspector, SerializeField]
    private Type mType;

    protected bool AdjustRadial(Vector2[] xy, Vector2[] uv, float fill, bool invert)
    {
        if (fill < 0.001f)
        {
            return false;
        }
        if (invert || (fill <= 0.999f))
        {
            float f = Mathf.Clamp01(fill);
            if (!invert)
            {
                f = 1f - f;
            }
            f *= 1.570796f;
            float t = Mathf.Sin(f);
            float num3 = Mathf.Cos(f);
            if (t > num3)
            {
                num3 *= 1f / t;
                t = 1f;
                if (!invert)
                {
                    xy[0].y = Mathf.Lerp(xy[2].y, xy[0].y, num3);
                    xy[3].y = xy[0].y;
                    uv[0].y = Mathf.Lerp(uv[2].y, uv[0].y, num3);
                    uv[3].y = uv[0].y;
                }
            }
            else if (num3 > t)
            {
                t *= 1f / num3;
                num3 = 1f;
                if (invert)
                {
                    xy[0].x = Mathf.Lerp(xy[2].x, xy[0].x, t);
                    xy[1].x = xy[0].x;
                    uv[0].x = Mathf.Lerp(uv[2].x, uv[0].x, t);
                    uv[1].x = uv[0].x;
                }
            }
            else
            {
                t = 1f;
                num3 = 1f;
            }
            if (invert)
            {
                xy[1].y = Mathf.Lerp(xy[2].y, xy[0].y, num3);
                uv[1].y = Mathf.Lerp(uv[2].y, uv[0].y, num3);
            }
            else
            {
                xy[3].x = Mathf.Lerp(xy[2].x, xy[0].x, t);
                uv[3].x = Mathf.Lerp(uv[2].x, uv[0].x, t);
            }
        }
        return true;
    }

    protected void FilledFill(BetterList<Vector3> verts, BetterList<Vector2> uvs, BetterList<Color32> cols)
    {
        float x = 0f;
        float y = 0f;
        float num3 = 1f;
        float num4 = -1f;
        float xMin = this.mOuterUV.xMin;
        float yMin = this.mOuterUV.yMin;
        float xMax = this.mOuterUV.xMax;
        float yMax = this.mOuterUV.yMax;
        if ((this.mFillDirection == FillDirection.Horizontal) || (this.mFillDirection == FillDirection.Vertical))
        {
            float num9 = (xMax - xMin) * this.mFillAmount;
            float num10 = (yMax - yMin) * this.mFillAmount;
            if (this.fillDirection == FillDirection.Horizontal)
            {
                if (this.mInvert)
                {
                    x = 1f - this.mFillAmount;
                    xMin = xMax - num9;
                }
                else
                {
                    num3 *= this.mFillAmount;
                    xMax = xMin + num9;
                }
            }
            else if (this.fillDirection == FillDirection.Vertical)
            {
                if (this.mInvert)
                {
                    num4 *= this.mFillAmount;
                    yMin = yMax - num10;
                }
                else
                {
                    y = -(1f - this.mFillAmount);
                    yMax = yMin + num10;
                }
            }
        }
        Vector2[] xy = new Vector2[4];
        Vector2[] uv = new Vector2[4];
        xy[0] = new Vector2(num3, y);
        xy[1] = new Vector2(num3, num4);
        xy[2] = new Vector2(x, num4);
        xy[3] = new Vector2(x, y);
        uv[0] = new Vector2(xMax, yMax);
        uv[1] = new Vector2(xMax, yMin);
        uv[2] = new Vector2(xMin, yMin);
        uv[3] = new Vector2(xMin, yMax);
        Color c = base.color;
        c.a *= base.mPanel.alpha;
        Color32 item = !this.atlas.premultipliedAlpha ? c : NGUITools.ApplyPMA(c);
        if (this.fillDirection == FillDirection.Radial90)
        {
            if (!this.AdjustRadial(xy, uv, this.mFillAmount, this.mInvert))
            {
                return;
            }
        }
        else
        {
            if (this.fillDirection == FillDirection.Radial180)
            {
                Vector2[] v = new Vector2[4];
                Vector2[] vectorArray4 = new Vector2[4];
                for (int j = 0; j < 2; j++)
                {
                    float num12;
                    float num13;
                    v[0] = new Vector2(0f, 0f);
                    v[1] = new Vector2(0f, 1f);
                    v[2] = new Vector2(1f, 1f);
                    v[3] = new Vector2(1f, 0f);
                    vectorArray4[0] = new Vector2(0f, 0f);
                    vectorArray4[1] = new Vector2(0f, 1f);
                    vectorArray4[2] = new Vector2(1f, 1f);
                    vectorArray4[3] = new Vector2(1f, 0f);
                    if (this.mInvert)
                    {
                        if (j > 0)
                        {
                            this.Rotate(v, j);
                            this.Rotate(vectorArray4, j);
                        }
                    }
                    else if (j < 1)
                    {
                        this.Rotate(v, 1 - j);
                        this.Rotate(vectorArray4, 1 - j);
                    }
                    if (j == 1)
                    {
                        num12 = !this.mInvert ? 1f : 0.5f;
                        num13 = !this.mInvert ? 0.5f : 1f;
                    }
                    else
                    {
                        num12 = !this.mInvert ? 0.5f : 1f;
                        num13 = !this.mInvert ? 1f : 0.5f;
                    }
                    v[1].y = Mathf.Lerp(num12, num13, v[1].y);
                    v[2].y = Mathf.Lerp(num12, num13, v[2].y);
                    vectorArray4[1].y = Mathf.Lerp(num12, num13, vectorArray4[1].y);
                    vectorArray4[2].y = Mathf.Lerp(num12, num13, vectorArray4[2].y);
                    float fill = (this.mFillAmount * 2f) - j;
                    bool flag = (j % 2) == 1;
                    if (this.AdjustRadial(v, vectorArray4, fill, !flag))
                    {
                        if (this.mInvert)
                        {
                            flag = !flag;
                        }
                        if (flag)
                        {
                            for (int k = 0; k < 4; k++)
                            {
                                num12 = Mathf.Lerp(xy[0].x, xy[2].x, v[k].x);
                                num13 = Mathf.Lerp(xy[0].y, xy[2].y, v[k].y);
                                float num16 = Mathf.Lerp(uv[0].x, uv[2].x, vectorArray4[k].x);
                                float num17 = Mathf.Lerp(uv[0].y, uv[2].y, vectorArray4[k].y);
                                verts.Add(new Vector3(num12, num13, 0f));
                                uvs.Add(new Vector2(num16, num17));
                                cols.Add(item);
                            }
                        }
                        else
                        {
                            for (int m = 3; m > -1; m--)
                            {
                                num12 = Mathf.Lerp(xy[0].x, xy[2].x, v[m].x);
                                num13 = Mathf.Lerp(xy[0].y, xy[2].y, v[m].y);
                                float num19 = Mathf.Lerp(uv[0].x, uv[2].x, vectorArray4[m].x);
                                float num20 = Mathf.Lerp(uv[0].y, uv[2].y, vectorArray4[m].y);
                                verts.Add(new Vector3(num12, num13, 0f));
                                uvs.Add(new Vector2(num19, num20));
                                cols.Add(item);
                            }
                        }
                    }
                }
                return;
            }
            if (this.fillDirection == FillDirection.Radial360)
            {
                float[] numArray = new float[] { 0.5f, 1f, 0f, 0.5f, 0.5f, 1f, 0.5f, 1f, 0f, 0.5f, 0.5f, 1f, 0f, 0.5f, 0f, 0.5f };
                Vector2[] vectorArray5 = new Vector2[4];
                Vector2[] vectorArray6 = new Vector2[4];
                for (int n = 0; n < 4; n++)
                {
                    vectorArray5[0] = new Vector2(0f, 0f);
                    vectorArray5[1] = new Vector2(0f, 1f);
                    vectorArray5[2] = new Vector2(1f, 1f);
                    vectorArray5[3] = new Vector2(1f, 0f);
                    vectorArray6[0] = new Vector2(0f, 0f);
                    vectorArray6[1] = new Vector2(0f, 1f);
                    vectorArray6[2] = new Vector2(1f, 1f);
                    vectorArray6[3] = new Vector2(1f, 0f);
                    if (this.mInvert)
                    {
                        if (n > 0)
                        {
                            this.Rotate(vectorArray5, n);
                            this.Rotate(vectorArray6, n);
                        }
                    }
                    else if (n < 3)
                    {
                        this.Rotate(vectorArray5, 3 - n);
                        this.Rotate(vectorArray6, 3 - n);
                    }
                    for (int num22 = 0; num22 < 4; num22++)
                    {
                        int index = !this.mInvert ? (n * 4) : ((3 - n) * 4);
                        float from = numArray[index];
                        float to = numArray[index + 1];
                        float num26 = numArray[index + 2];
                        float num27 = numArray[index + 3];
                        vectorArray5[num22].x = Mathf.Lerp(from, to, vectorArray5[num22].x);
                        vectorArray5[num22].y = Mathf.Lerp(num26, num27, vectorArray5[num22].y);
                        vectorArray6[num22].x = Mathf.Lerp(from, to, vectorArray6[num22].x);
                        vectorArray6[num22].y = Mathf.Lerp(num26, num27, vectorArray6[num22].y);
                    }
                    float num28 = (this.mFillAmount * 4f) - n;
                    bool flag2 = (n % 2) == 1;
                    if (this.AdjustRadial(vectorArray5, vectorArray6, num28, !flag2))
                    {
                        if (this.mInvert)
                        {
                            flag2 = !flag2;
                        }
                        if (flag2)
                        {
                            for (int num29 = 0; num29 < 4; num29++)
                            {
                                float num30 = Mathf.Lerp(xy[0].x, xy[2].x, vectorArray5[num29].x);
                                float num31 = Mathf.Lerp(xy[0].y, xy[2].y, vectorArray5[num29].y);
                                float num32 = Mathf.Lerp(uv[0].x, uv[2].x, vectorArray6[num29].x);
                                float num33 = Mathf.Lerp(uv[0].y, uv[2].y, vectorArray6[num29].y);
                                verts.Add(new Vector3(num30, num31, 0f));
                                uvs.Add(new Vector2(num32, num33));
                                cols.Add(item);
                            }
                        }
                        else
                        {
                            for (int num34 = 3; num34 > -1; num34--)
                            {
                                float num35 = Mathf.Lerp(xy[0].x, xy[2].x, vectorArray5[num34].x);
                                float num36 = Mathf.Lerp(xy[0].y, xy[2].y, vectorArray5[num34].y);
                                float num37 = Mathf.Lerp(uv[0].x, uv[2].x, vectorArray6[num34].x);
                                float num38 = Mathf.Lerp(uv[0].y, uv[2].y, vectorArray6[num34].y);
                                verts.Add(new Vector3(num35, num36, 0f));
                                uvs.Add(new Vector2(num37, num38));
                                cols.Add(item);
                            }
                        }
                    }
                }
                return;
            }
        }
        for (int i = 0; i < 4; i++)
        {
            verts.Add(xy[i]);
            uvs.Add(uv[i]);
            cols.Add(item);
        }
    }

    public UIAtlas.Sprite GetAtlasSprite()
    {
        if (!this.mSpriteSet)
        {
            this.mSprite = null;
        }
        if ((this.mSprite == null) && (this.mAtlas != null))
        {
            if (!string.IsNullOrEmpty(this.mSpriteName))
            {
                UIAtlas.Sprite sp = this.mAtlas.GetSprite(this.mSpriteName);
                if (sp == null)
                {
                    return null;
                }
                this.SetAtlasSprite(sp);
            }
            if ((this.mSprite == null) && (this.mAtlas.spriteList.Count > 0))
            {
                UIAtlas.Sprite sprite2 = this.mAtlas.spriteList[0];
                if (sprite2 == null)
                {
                    return null;
                }
                this.SetAtlasSprite(sprite2);
                if (this.mSprite == null)
                {
                    Debug.LogError(this.mAtlas.name + " seems to have a null sprite!");
                    return null;
                }
                this.mSpriteName = this.mSprite.name;
            }
            if (this.mSprite != null)
            {
                this.material = this.mAtlas.spriteMaterial;
                this.UpdateUVs(true);
            }
        }
        return this.mSprite;
    }

    public override void MakePixelPerfect()
    {
        if (this.isValid)
        {
            this.UpdateUVs(false);
            switch (this.type)
            {
                case Type.Sliced:
                    {
                        Vector3 localPosition = base.cachedTransform.localPosition;
                        localPosition.x = Mathf.RoundToInt(localPosition.x);
                        localPosition.y = Mathf.RoundToInt(localPosition.y);
                        localPosition.z = Mathf.RoundToInt(localPosition.z);
                        base.cachedTransform.localPosition = localPosition;
                        Vector3 localScale = base.cachedTransform.localScale;
                        localScale.x = Mathf.RoundToInt(localScale.x * 0.5f) << 1;
                        localScale.y = Mathf.RoundToInt(localScale.y * 0.5f) << 1;
                        localScale.z = 1f;
                        base.cachedTransform.localScale = localScale;
                        break;
                    }
                case Type.Tiled:
                    {
                        Vector3 vector3 = base.cachedTransform.localPosition;
                        vector3.x = Mathf.RoundToInt(vector3.x);
                        vector3.y = Mathf.RoundToInt(vector3.y);
                        vector3.z = Mathf.RoundToInt(vector3.z);
                        base.cachedTransform.localPosition = vector3;
                        Vector3 vector4 = base.cachedTransform.localScale;
                        vector4.x = Mathf.RoundToInt(vector4.x);
                        vector4.y = Mathf.RoundToInt(vector4.y);
                        vector4.z = 1f;
                        base.cachedTransform.localScale = vector4;
                        break;
                    }
                default:
                    {
                        Texture mainTexture = this.mainTexture;
                        Vector3 vector5 = base.cachedTransform.localScale;
                        if (mainTexture != null)
                        {
                            Rect rect = NGUIMath.ConvertToPixels(this.outerUV, mainTexture.width, mainTexture.height, true);
                            float pixelSize = this.atlas.pixelSize;
                            vector5.x = Mathf.RoundToInt(rect.width * pixelSize) * Mathf.Sign(vector5.x);
                            vector5.y = Mathf.RoundToInt(rect.height * pixelSize) * Mathf.Sign(vector5.y);
                            vector5.z = 1f;
                            base.cachedTransform.localScale = vector5;
                        }
                        int num2 = Mathf.RoundToInt(Mathf.Abs(vector5.x) * ((1f + this.mSprite.paddingLeft) + this.mSprite.paddingRight));
                        int num3 = Mathf.RoundToInt(Mathf.Abs(vector5.y) * ((1f + this.mSprite.paddingTop) + this.mSprite.paddingBottom));
                        Vector3 vector6 = base.cachedTransform.localPosition;
                        vector6.x = Mathf.CeilToInt(vector6.x * 4f) >> 2;
                        vector6.y = Mathf.CeilToInt(vector6.y * 4f) >> 2;
                        vector6.z = Mathf.RoundToInt(vector6.z);
                        if (((num2 % 2) == 1) && (((base.pivot == UIWidget.Pivot.Top) || (base.pivot == UIWidget.Pivot.Center)) || (base.pivot == UIWidget.Pivot.Bottom)))
                        {
                            vector6.x += 0.5f;
                        }
                        if (((num3 % 2) == 1) && (((base.pivot == UIWidget.Pivot.Left) || (base.pivot == UIWidget.Pivot.Center)) || (base.pivot == UIWidget.Pivot.Right)))
                        {
                            vector6.y += 0.5f;
                        }
                        base.cachedTransform.localPosition = vector6;
                        break;
                    }
            }
        }
    }

    public override void OnFill(BetterList<Vector3> verts, BetterList<Vector2> uvs, BetterList<Color32> cols)
    {
        switch (this.type)
        {
            case Type.Simple:
                this.SimpleFill(verts, uvs, cols);
                break;

            case Type.Sliced:
                this.SlicedFill(verts, uvs, cols);
                break;

            case Type.Tiled:
                this.TiledFill(verts, uvs, cols);
                break;

            case Type.Filled:
                this.FilledFill(verts, uvs, cols);
                break;
        }
    }

    protected override void OnStart()
    {
        if (this.mAtlas != null)
        {
            this.UpdateUVs(true);
        }
    }

    protected void Rotate(Vector2[] v, int offset)
    {
        for (int i = 0; i < offset; i++)
        {
            Vector2 vector = new Vector2(v[3].x, v[3].y);
            v[3].x = v[2].y;
            v[3].y = v[2].x;
            v[2].x = v[1].y;
            v[2].y = v[1].x;
            v[1].x = v[0].y;
            v[1].y = v[0].x;
            v[0].x = vector.y;
            v[0].y = vector.x;
        }
    }

    protected void SetAtlasSprite(UIAtlas.Sprite sp)
    {
        base.mChanged = true;
        this.mSpriteSet = true;
        if (sp != null)
        {
            this.mSprite = sp;
            this.mSpriteName = this.mSprite.name;
        }
        else
        {
            this.mSpriteName = (this.mSprite == null) ? string.Empty : this.mSprite.name;
            this.mSprite = sp;
        }
    }

    protected void SimpleFill(BetterList<Vector3> verts, BetterList<Vector2> uvs, BetterList<Color32> cols)
    {
        Vector2 item = new Vector2(this.mOuterUV.xMin, this.mOuterUV.yMin);
        Vector2 vector2 = new Vector2(this.mOuterUV.xMax, this.mOuterUV.yMax);
        verts.Add(new Vector3(1f, 0f, 0f));
        verts.Add(new Vector3(1f, -1f, 0f));
        verts.Add(new Vector3(0f, -1f, 0f));
        verts.Add(new Vector3(0f, 0f, 0f));
        uvs.Add(vector2);
        uvs.Add(new Vector2(vector2.x, item.y));
        uvs.Add(item);
        uvs.Add(new Vector2(item.x, vector2.y));
        Color c = base.color;
        c.a *= base.mPanel.alpha;
        Color32 color2 = !this.atlas.premultipliedAlpha ? c : NGUITools.ApplyPMA(c);
        cols.Add(color2);
        cols.Add(color2);
        cols.Add(color2);
        cols.Add(color2);
    }

    protected void SlicedFill(BetterList<Vector3> verts, BetterList<Vector2> uvs, BetterList<Color32> cols)
    {
        if (this.mOuterUV == this.mInnerUV)
        {
            this.SimpleFill(verts, uvs, cols);
        }
        else
        {
            Vector2[] vectorArray = new Vector2[4];
            Vector2[] vectorArray2 = new Vector2[4];
            Texture mainTexture = this.mainTexture;
            vectorArray[0] = Vector2.zero;
            vectorArray[1] = Vector2.zero;
            vectorArray[2] = new Vector2(1f, -1f);
            vectorArray[3] = new Vector2(1f, -1f);
            if (mainTexture == null)
            {
                for (int j = 0; j < 4; j++)
                {
                    vectorArray2[j] = Vector2.zero;
                }
            }
            else
            {
                float pixelSize = this.atlas.pixelSize;
                float num2 = (this.mInnerUV.xMin - this.mOuterUV.xMin) * pixelSize;
                float num3 = (this.mOuterUV.xMax - this.mInnerUV.xMax) * pixelSize;
                float num4 = (this.mInnerUV.yMax - this.mOuterUV.yMax) * pixelSize;
                float num5 = (this.mOuterUV.yMin - this.mInnerUV.yMin) * pixelSize;
                Vector3 localScale = base.cachedTransform.localScale;
                localScale.x = Mathf.Max(0f, localScale.x);
                localScale.y = Mathf.Max(0f, localScale.y);
                Vector2 vector2 = new Vector2(localScale.x / ((float)mainTexture.width), localScale.y / ((float)mainTexture.height));
                Vector2 vector3 = new Vector2(num2 / vector2.x, num4 / vector2.y);
                Vector2 vector4 = new Vector2(num3 / vector2.x, num5 / vector2.y);
                UIWidget.Pivot pivot = base.pivot;
                switch (pivot)
                {
                    case UIWidget.Pivot.Right:
                    case UIWidget.Pivot.TopRight:
                    case UIWidget.Pivot.BottomRight:
                        vectorArray[0].x = Mathf.Min((float)0f, (float)(1f - (vector4.x + vector3.x)));
                        vectorArray[1].x = vectorArray[0].x + vector3.x;
                        vectorArray[2].x = vectorArray[0].x + Mathf.Max(vector3.x, 1f - vector4.x);
                        vectorArray[3].x = vectorArray[0].x + Mathf.Max((float)(vector3.x + vector4.x), (float)1f);
                        break;

                    default:
                        vectorArray[1].x = vector3.x;
                        vectorArray[2].x = Mathf.Max(vector3.x, 1f - vector4.x);
                        vectorArray[3].x = Mathf.Max((float)(vector3.x + vector4.x), (float)1f);
                        break;
                }
                switch (pivot)
                {
                    case UIWidget.Pivot.Bottom:
                    case UIWidget.Pivot.BottomLeft:
                    case UIWidget.Pivot.BottomRight:
                        vectorArray[0].y = Mathf.Max((float)0f, (float)(-1f - (vector4.y + vector3.y)));
                        vectorArray[1].y = vectorArray[0].y + vector3.y;
                        vectorArray[2].y = vectorArray[0].y + Mathf.Min(vector3.y, -1f - vector4.y);
                        vectorArray[3].y = vectorArray[0].y + Mathf.Min((float)(vector3.y + vector4.y), (float)-1f);
                        break;

                    default:
                        vectorArray[1].y = vector3.y;
                        vectorArray[2].y = Mathf.Min(vector3.y, -1f - vector4.y);
                        vectorArray[3].y = Mathf.Min((float)(vector3.y + vector4.y), (float)-1f);
                        break;
                }
                vectorArray2[0] = new Vector2(this.mOuterUV.xMin, this.mOuterUV.yMax);
                vectorArray2[1] = new Vector2(this.mInnerUV.xMin, this.mInnerUV.yMax);
                vectorArray2[2] = new Vector2(this.mInnerUV.xMax, this.mInnerUV.yMin);
                vectorArray2[3] = new Vector2(this.mOuterUV.xMax, this.mOuterUV.yMin);
            }
            Color c = base.color;
            c.a *= base.mPanel.alpha;
            Color32 item = !this.atlas.premultipliedAlpha ? c : NGUITools.ApplyPMA(c);
            for (int i = 0; i < 3; i++)
            {
                int index = i + 1;
                for (int k = 0; k < 3; k++)
                {
                    if ((this.mFillCenter || (i != 1)) || (k != 1))
                    {
                        int num10 = k + 1;
                        verts.Add(new Vector3(vectorArray[index].x, vectorArray[k].y, 0f));
                        verts.Add(new Vector3(vectorArray[index].x, vectorArray[num10].y, 0f));
                        verts.Add(new Vector3(vectorArray[i].x, vectorArray[num10].y, 0f));
                        verts.Add(new Vector3(vectorArray[i].x, vectorArray[k].y, 0f));
                        uvs.Add(new Vector2(vectorArray2[index].x, vectorArray2[k].y));
                        uvs.Add(new Vector2(vectorArray2[index].x, vectorArray2[num10].y));
                        uvs.Add(new Vector2(vectorArray2[i].x, vectorArray2[num10].y));
                        uvs.Add(new Vector2(vectorArray2[i].x, vectorArray2[k].y));
                        cols.Add(item);
                        cols.Add(item);
                        cols.Add(item);
                        cols.Add(item);
                    }
                }
            }
        }
    }

    protected void TiledFill(BetterList<Vector3> verts, BetterList<Vector2> uvs, BetterList<Color32> cols)
    {
        Texture mainTexture = this.material.mainTexture;
        if (mainTexture != null)
        {
            Rect mInner = this.mInner;
            if (this.atlas.coordinates == UIAtlas.Coordinates.TexCoords)
            {
                mInner = NGUIMath.ConvertToPixels(mInner, mainTexture.width, mainTexture.height, true);
            }
            Vector2 localScale = base.cachedTransform.localScale;
            float pixelSize = this.atlas.pixelSize;
            float num2 = Mathf.Abs((float)(mInner.width / localScale.x)) * pixelSize;
            float num3 = Mathf.Abs((float)(mInner.height / localScale.y)) * pixelSize;
            if ((num2 < 0.01f) || (num3 < 0.01f))
            {
                Debug.LogWarning("The tiled sprite (" + NGUITools.GetHierarchy(base.gameObject) + ") is too small.\nConsider using a bigger one.");
                num2 = 0.01f;
                num3 = 0.01f;
            }
            Vector2 vector2 = new Vector2(mInner.xMin / ((float)mainTexture.width), mInner.yMin / ((float)mainTexture.height));
            Vector2 vector3 = new Vector2(mInner.xMax / ((float)mainTexture.width), mInner.yMax / ((float)mainTexture.height));
            Vector2 vector4 = vector3;
            Color c = base.color;
            c.a *= base.mPanel.alpha;
            Color32 item = !this.atlas.premultipliedAlpha ? c : NGUITools.ApplyPMA(c);
            for (float i = 0f; i < 1f; i += num3)
            {
                float x = 0f;
                vector4.x = vector3.x;
                float num6 = i + num3;
                if (num6 > 1f)
                {
                    vector4.y = vector2.y + (((vector3.y - vector2.y) * (1f - i)) / (num6 - i));
                    num6 = 1f;
                }
                while (x < 1f)
                {
                    float num7 = x + num2;
                    if (num7 > 1f)
                    {
                        vector4.x = vector2.x + (((vector3.x - vector2.x) * (1f - x)) / (num7 - x));
                        num7 = 1f;
                    }
                    verts.Add(new Vector3(num7, -i, 0f));
                    verts.Add(new Vector3(num7, -num6, 0f));
                    verts.Add(new Vector3(x, -num6, 0f));
                    verts.Add(new Vector3(x, -i, 0f));
                    uvs.Add(new Vector2(vector4.x, 1f - vector2.y));
                    uvs.Add(new Vector2(vector4.x, 1f - vector4.y));
                    uvs.Add(new Vector2(vector2.x, 1f - vector4.y));
                    uvs.Add(new Vector2(vector2.x, 1f - vector2.y));
                    cols.Add(item);
                    cols.Add(item);
                    cols.Add(item);
                    cols.Add(item);
                    x += num2;
                }
            }
        }
    }

    public override void Update()
    {
        base.Update();
        if (base.mChanged || !this.mSpriteSet)
        {
            this.mSpriteSet = true;
            this.mSprite = null;
            base.mChanged = true;
            this.UpdateUVs(true);
        }
        else
        {
            this.UpdateUVs(false);
        }
    }

    public virtual void UpdateUVs(bool force)
    {
        if (((this.type == Type.Sliced) || (this.type == Type.Tiled)) && (base.cachedTransform.localScale != this.mScale))
        {
            this.mScale = base.cachedTransform.localScale;
            base.mChanged = true;
        }
        if (this.isValid && force)
        {
            Texture mainTexture = this.mainTexture;
            if (mainTexture != null)
            {
                this.mInner = this.mSprite.inner;
                this.mOuter = this.mSprite.outer;
                this.mInnerUV = this.mInner;
                this.mOuterUV = this.mOuter;
                if (this.atlas.coordinates == UIAtlas.Coordinates.Pixels)
                {
                    this.mOuterUV = NGUIMath.ConvertToTexCoords(this.mOuterUV, mainTexture.width, mainTexture.height);
                    this.mInnerUV = NGUIMath.ConvertToTexCoords(this.mInnerUV, mainTexture.width, mainTexture.height);
                }
            }
        }
    }

    public UIAtlas atlas
    {
        get
        {
            return this.mAtlas;
        }
        set
        {
            if (this.mAtlas != value)
            {
                this.mAtlas = value;
                this.mSpriteSet = false;
                this.mSprite = null;
                this.material = (this.mAtlas == null) ? null : this.mAtlas.spriteMaterial;
                if ((string.IsNullOrEmpty(this.mSpriteName) && (this.mAtlas != null)) && (this.mAtlas.spriteList.Count > 0))
                {
                    this.SetAtlasSprite(this.mAtlas.spriteList[0]);
                    this.mSpriteName = this.mSprite.name;
                }
                if (!string.IsNullOrEmpty(this.mSpriteName))
                {
                    string mSpriteName = this.mSpriteName;
                    this.mSpriteName = string.Empty;
                    this.spriteName = mSpriteName;
                    base.mChanged = true;
                    this.UpdateUVs(true);
                }
            }
        }
    }

    public override Vector4 border
    {
        get
        {
            if (this.type != Type.Sliced)
            {
                return base.border;
            }
            UIAtlas.Sprite atlasSprite = this.GetAtlasSprite();
            if (atlasSprite == null)
            {
                return Vector2.zero;
            }
            Rect outer = atlasSprite.outer;
            Rect inner = atlasSprite.inner;
            Texture mainTexture = this.mainTexture;
            if ((this.atlas.coordinates == UIAtlas.Coordinates.TexCoords) && (mainTexture != null))
            {
                outer = NGUIMath.ConvertToPixels(outer, mainTexture.width, mainTexture.height, true);
                inner = NGUIMath.ConvertToPixels(inner, mainTexture.width, mainTexture.height, true);
            }
            return (Vector4)(new Vector4(inner.xMin - outer.xMin, inner.yMin - outer.yMin, outer.xMax - inner.xMax, outer.yMax - inner.yMax) * this.atlas.pixelSize);
        }
    }

    public float fillAmount
    {
        get
        {
            return this.mFillAmount;
        }
        set
        {
            float num = Mathf.Clamp01(value);
            if (this.mFillAmount != num)
            {
                this.mFillAmount = num;
                base.mChanged = true;
            }
        }
    }

    public bool fillCenter
    {
        get
        {
            return this.mFillCenter;
        }
        set
        {
            if (this.mFillCenter != value)
            {
                this.mFillCenter = value;
                this.MarkAsChanged();
            }
        }
    }

    public FillDirection fillDirection
    {
        get
        {
            return this.mFillDirection;
        }
        set
        {
            if (this.mFillDirection != value)
            {
                this.mFillDirection = value;
                base.mChanged = true;
            }
        }
    }

    public Rect innerUV
    {
        get
        {
            this.UpdateUVs(false);
            return this.mInnerUV;
        }
    }

    public bool invert
    {
        get
        {
            return this.mInvert;
        }
        set
        {
            if (this.mInvert != value)
            {
                this.mInvert = value;
                base.mChanged = true;
            }
        }
    }

    public bool isValid
    {
        get
        {
            return (this.GetAtlasSprite() != null);
        }
    }

    public override Material material
    {
        get
        {
            Material material = base.material;
            if (material == null)
            {
                material = (this.mAtlas == null) ? null : this.mAtlas.spriteMaterial;
                this.mSprite = null;
                this.material = material;
                if (material != null)
                {
                    this.UpdateUVs(true);
                }
            }
            return material;
        }
    }

    public Rect outerUV
    {
        get
        {
            this.UpdateUVs(false);
            return this.mOuterUV;
        }
    }

    public override bool pixelPerfectAfterResize
    {
        get
        {
            return (this.type == Type.Sliced);
        }
    }

    public override Vector4 relativePadding
    {
        get
        {
            if (this.isValid && (this.type == Type.Simple))
            {
                return new Vector4(this.mSprite.paddingLeft, this.mSprite.paddingTop, this.mSprite.paddingRight, this.mSprite.paddingBottom);
            }
            return base.relativePadding;
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
            if (string.IsNullOrEmpty(value))
            {
                if (!string.IsNullOrEmpty(this.mSpriteName))
                {
                    this.mSpriteName = string.Empty;
                    this.mSprite = null;
                    base.mChanged = true;
                    this.mSpriteSet = false;
                }
            }
            else if (this.mSpriteName != value)
            {
                this.mSpriteName = value;
                this.mSprite = null;
                base.mChanged = true;
                this.mSpriteSet = false;
                if (this.isValid)
                {
                    this.UpdateUVs(true);
                }
            }
        }
    }

    public virtual Type type
    {
        get
        {
            return this.mType;
        }
        set
        {
            if (this.mType != value)
            {
                this.mType = value;
                this.MarkAsChanged();
            }
        }
    }

    public enum FillDirection
    {
        Horizontal,
        Vertical,
        Radial90,
        Radial180,
        Radial360
    }

    public enum Type
    {
        Simple,
        Sliced,
        Tiled,
        Filled
    }
}

