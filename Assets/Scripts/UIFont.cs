using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

[AddComponentMenu("NGUI/UI/Font"), ExecuteInEditMode]
public class UIFont : MonoBehaviour
{
    [HideInInspector, SerializeField]
    private UIAtlas mAtlas;
    private static CharacterInfo mChar;
    private List<Color> mColors = new List<Color>();
    [HideInInspector, SerializeField]
    private Font mDynamicFont;
    [HideInInspector, SerializeField]
    private float mDynamicFontOffset;
    [SerializeField, HideInInspector]
    private int mDynamicFontSize = 0x10;
    [SerializeField, HideInInspector]
    private FontStyle mDynamicFontStyle;
    [SerializeField, HideInInspector]
    private BMFont mFont = new BMFont();
    [SerializeField, HideInInspector]
    private Material mMat;
    [SerializeField, HideInInspector]
    private float mPixelSize = 1f;
    private int mPMA = -1;
    [HideInInspector, SerializeField]
    private UIFont mReplacement;
    [HideInInspector, SerializeField]
    private int mSpacingX;
    [SerializeField, HideInInspector]
    private int mSpacingY;
    private UIAtlas.Sprite mSprite;
    private bool mSpriteSet;
    [SerializeField, HideInInspector]
    private List<BMSymbol> mSymbols = new List<BMSymbol>();
    [HideInInspector, SerializeField]
    private Rect mUVRect = new Rect(0f, 0f, 1f, 1f);

    public void AddSymbol(string sequence, string spriteName)
    {
        this.GetSymbol(sequence, true).spriteName = spriteName;
        this.MarkAsDirty();
    }

    private void Align(BetterList<Vector3> verts, int indexOffset, Alignment alignment, int x, int lineWidth)
    {
        if (alignment != Alignment.Left)
        {
            int size = this.size;
            if (size > 0)
            {
                float num2 = 0f;
                if (alignment == Alignment.Right)
                {
                    num2 = Mathf.RoundToInt((float)(lineWidth - x));
                    if (num2 < 0f)
                    {
                        num2 = 0f;
                    }
                    num2 /= (float)this.size;
                }
                else
                {
                    num2 = Mathf.RoundToInt((lineWidth - x) * 0.5f);
                    if (num2 < 0f)
                    {
                        num2 = 0f;
                    }
                    num2 /= (float)this.size;
                    if ((lineWidth & 1) == 1)
                    {
                        num2 += 0.5f / ((float)size);
                    }
                }
                for (int i = indexOffset; i < verts.size; i++)
                {
                    Vector3 vector = verts.buffer[i];
                    vector.x += num2;
                    verts.buffer[i] = vector;
                }
            }
        }
    }

    public Vector2 CalculatePrintedSize(string text, bool encoding, SymbolStyle symbolStyle)
    {
        if (this.mReplacement != null)
        {
            return this.mReplacement.CalculatePrintedSize(text, encoding, symbolStyle);
        }
        Vector2 zero = Vector2.zero;
        bool isDynamic = this.isDynamic;
        if (isDynamic || (((this.mFont != null) && this.mFont.isValid) && !string.IsNullOrEmpty(text)))
        {
            if (encoding)
            {
                text = NGUITools.StripSymbols(text);
            }
            if (isDynamic)
            {
                this.mDynamicFont.textureRebuildCallback = new Font.FontTextureRebuildCallback(this.OnFontChanged);
                this.mDynamicFont.RequestCharactersInTexture(text, this.mDynamicFontSize, this.mDynamicFontStyle);
                this.mDynamicFont.textureRebuildCallback = null;
            }
            int length = text.Length;
            int num2 = 0;
            int num3 = 0;
            int num4 = 0;
            int previousChar = 0;
            int size = this.size;
            int num7 = size + this.mSpacingY;
            bool flag2 = (encoding && (symbolStyle != SymbolStyle.None)) && this.hasSymbols;
            for (int i = 0; i < length; i++)
            {
                char index = text[i];
                if (index == '\n')
                {
                    if (num3 > num2)
                    {
                        num2 = num3;
                    }
                    num3 = 0;
                    num4 += num7;
                    previousChar = 0;
                }
                else if (index < ' ')
                {
                    previousChar = 0;
                }
                else if (!isDynamic)
                {
                    BMSymbol symbol = !flag2 ? null : this.MatchSymbol(text, i, length);
                    if (symbol == null)
                    {
                        BMGlyph glyph = this.mFont.GetGlyph(index);
                        if (glyph != null)
                        {
                            num3 += this.mSpacingX + ((previousChar == 0) ? glyph.advance : (glyph.advance + glyph.GetKerning(previousChar)));
                            previousChar = index;
                        }
                    }
                    else
                    {
                        num3 += this.mSpacingX + symbol.width;
                        i += symbol.length - 1;
                        previousChar = 0;
                    }
                }
                else if (this.mDynamicFont.GetCharacterInfo(index, out mChar, this.mDynamicFontSize, this.mDynamicFontStyle))
                {
                    num3 += this.mSpacingX + ((int)mChar.width);
                }
            }
            float num9 = (size <= 0) ? 1f : (1f / ((float)size));
            zero.x = num9 * ((num3 <= num2) ? ((float)num2) : ((float)num3));
            zero.y = num9 * (num4 + num7);
        }
        return zero;
    }

    public static bool CheckIfRelated(UIFont a, UIFont b)
    {
        if ((a == null) || (b == null))
        {
            return false;
        }
        return (((a.isDynamic && b.isDynamic) && (a.dynamicFont.fontNames[0] == b.dynamicFont.fontNames[0])) || (((a == b) || a.References(b)) || b.References(a)));
    }

    private static void EndLine(ref StringBuilder s)
    {
        int num = s.Length - 1;
        if ((num > 0) && (s[num] == ' '))
        {
            s[num] = '\n';
        }
        else
        {
            s.Append('\n');
        }
    }

    public string GetEndOfLineThatFits(string text, float maxWidth, bool encoding, SymbolStyle symbolStyle)
    {
        if (this.mReplacement != null)
        {
            return this.mReplacement.GetEndOfLineThatFits(text, maxWidth, encoding, symbolStyle);
        }
        int num = Mathf.RoundToInt(maxWidth * this.size);
        if (num < 1)
        {
            return text;
        }
        int length = text.Length;
        int num3 = num;
        BMGlyph glyph = null;
        int offset = length;
        bool flag = (encoding && (symbolStyle != SymbolStyle.None)) && this.hasSymbols;
        bool isDynamic = this.isDynamic;
        if (isDynamic)
        {
            this.mDynamicFont.textureRebuildCallback = new Font.FontTextureRebuildCallback(this.OnFontChanged);
            this.mDynamicFont.RequestCharactersInTexture(text, this.mDynamicFontSize, this.mDynamicFontStyle);
            this.mDynamicFont.textureRebuildCallback = null;
        }
        while ((offset > 0) && (num3 > 0))
        {
            char index = text[--offset];
            BMSymbol symbol = !flag ? null : this.MatchSymbol(text, offset, length);
            int mSpacingX = this.mSpacingX;
            if (!isDynamic)
            {
                if (symbol != null)
                {
                    mSpacingX += symbol.advance;
                    goto Label_017F;
                }
                BMGlyph glyph2 = this.mFont.GetGlyph(index);
                if (glyph2 != null)
                {
                    mSpacingX += glyph2.advance + ((glyph != null) ? glyph.GetKerning(index) : 0);
                    glyph = glyph2;
                    goto Label_017F;
                }
                glyph = null;
                continue;
            }
            if (this.mDynamicFont.GetCharacterInfo(index, out mChar, this.mDynamicFontSize, this.mDynamicFontStyle))
            {
                mSpacingX += (int)mChar.width;
            }
        Label_017F:
            num3 -= mSpacingX;
        }
        if (num3 < 0)
        {
            offset++;
        }
        return text.Substring(offset, length - offset);
    }

    private BMSymbol GetSymbol(string sequence, bool createIfMissing)
    {
        int num = 0;
        int count = this.mSymbols.Count;
        while (num < count)
        {
            BMSymbol symbol = this.mSymbols[num];
            if (symbol.sequence == sequence)
            {
                return symbol;
            }
            num++;
        }
        if (createIfMissing)
        {
            BMSymbol item = new BMSymbol
            {
                sequence = sequence
            };
            this.mSymbols.Add(item);
            return item;
        }
        return null;
    }

    public void MarkAsDirty()
    {
        if (this.mReplacement != null)
        {
            this.mReplacement.MarkAsDirty();
        }
        this.RecalculateDynamicOffset();
        this.mSprite = null;
        UILabel[] labelArray = NGUITools.FindActive<UILabel>();
        int index = 0;
        int length = labelArray.Length;
        while (index < length)
        {
            UILabel label = labelArray[index];
            if ((label.enabled && NGUITools.GetActive(label.gameObject)) && CheckIfRelated(this, label.font))
            {
                UIFont font = label.font;
                label.font = null;
                label.font = font;
            }
            index++;
        }
        int num3 = 0;
        int count = this.mSymbols.Count;
        while (num3 < count)
        {
            this.symbols[num3].MarkAsDirty();
            num3++;
        }
    }

    private BMSymbol MatchSymbol(string text, int offset, int textLength)
    {
        int count = this.mSymbols.Count;
        if (count != 0)
        {
            textLength -= offset;
            for (int i = 0; i < count; i++)
            {
                BMSymbol symbol = this.mSymbols[i];
                int length = symbol.length;
                if ((length != 0) && (textLength >= length))
                {
                    bool flag = true;
                    for (int j = 0; j < length; j++)
                    {
                        if (text[offset + j] != symbol.sequence[j])
                        {
                            flag = false;
                            break;
                        }
                    }
                    if (flag && symbol.Validate(this.atlas))
                    {
                        return symbol;
                    }
                }
            }
        }
        return null;
    }

    private void OnFontChanged()
    {
        this.MarkAsDirty();
    }

    public void Print(string text, Color32 color, BetterList<Vector3> verts, BetterList<Vector2> uvs, BetterList<Color32> cols, bool encoding, SymbolStyle symbolStyle, Alignment alignment, int lineWidth, bool premultiply)
    {
        if (this.mReplacement != null)
        {
            this.mReplacement.Print(text, color, verts, uvs, cols, encoding, symbolStyle, alignment, lineWidth, premultiply);
        }
        else if (text != null)
        {
            if (!this.isValid)
            {
                Debug.LogError("Attempting to print using an invalid font!");
            }
            else
            {
                bool isDynamic = this.isDynamic;
                if (isDynamic)
                {
                    this.mDynamicFont.textureRebuildCallback = new Font.FontTextureRebuildCallback(this.OnFontChanged);
                    this.mDynamicFont.RequestCharactersInTexture(text, this.mDynamicFontSize, this.mDynamicFontStyle);
                    this.mDynamicFont.textureRebuildCallback = null;
                }
                this.mColors.Clear();
                this.mColors.Add((Color)color);
                int size = this.size;
                Vector2 vector = (size <= 0) ? Vector2.one : new Vector2(1f / ((float)size), 1f / ((float)size));
                int indexOffset = verts.size;
                int num3 = 0;
                int x = 0;
                int num5 = 0;
                int previousChar = 0;
                int num7 = size + this.mSpacingY;
                Vector3 zero = Vector3.zero;
                Vector3 vector3 = Vector3.zero;
                Vector2 vector4 = Vector2.zero;
                Vector2 vector5 = Vector2.zero;
                float num8 = this.uvRect.width / ((float)this.mFont.texWidth);
                float num9 = this.mUVRect.height / ((float)this.mFont.texHeight);
                int length = text.Length;
                bool flag2 = ((encoding && (symbolStyle != SymbolStyle.None)) && this.hasSymbols) && (this.sprite != null);
                for (int i = 0; i < length; i++)
                {
                    char index = text[i];
                    if (index == '\n')
                    {
                        if (x > num3)
                        {
                            num3 = x;
                        }
                        if (alignment != Alignment.Left)
                        {
                            this.Align(verts, indexOffset, alignment, x, lineWidth);
                            indexOffset = verts.size;
                        }
                        x = 0;
                        num5 += num7;
                        previousChar = 0;
                        continue;
                    }
                    if (index < ' ')
                    {
                        previousChar = 0;
                        continue;
                    }
                    if (encoding && (index == '['))
                    {
                        int num12 = NGUITools.ParseSymbol(text, i, this.mColors, premultiply);
                        if (num12 > 0)
                        {
                            color = this.mColors[this.mColors.Count - 1];
                            i += num12 - 1;
                            continue;
                        }
                    }
                    if (!isDynamic)
                    {
                        BMSymbol symbol = !flag2 ? null : this.MatchSymbol(text, i, length);
                        if (symbol == null)
                        {
                            BMGlyph glyph = this.mFont.GetGlyph(index);
                            if (glyph == null)
                            {
                                continue;
                            }
                            if (previousChar != 0)
                            {
                                x += glyph.GetKerning(previousChar);
                            }
                            if (index == ' ')
                            {
                                x += this.mSpacingX + glyph.advance;
                                previousChar = index;
                                continue;
                            }
                            zero.x = vector.x * (x + glyph.offsetX);
                            zero.y = -vector.y * (num5 + glyph.offsetY);
                            vector3.x = zero.x + (vector.x * glyph.width);
                            vector3.y = zero.y - (vector.y * glyph.height);
                            vector4.x = this.mUVRect.xMin + (num8 * glyph.x);
                            vector4.y = this.mUVRect.yMax - (num9 * glyph.y);
                            vector5.x = vector4.x + (num8 * glyph.width);
                            vector5.y = vector4.y - (num9 * glyph.height);
                            x += this.mSpacingX + glyph.advance;
                            previousChar = index;
                            if ((glyph.channel == 0) || (glyph.channel == 15))
                            {
                                for (int j = 0; j < 4; j++)
                                {
                                    cols.Add(color);
                                }
                            }
                            else
                            {
                                Color item = (Color)color;
                                item = (Color)(item * 0.49f);
                                switch (glyph.channel)
                                {
                                    case 1:
                                        item.b += 0.51f;
                                        break;

                                    case 2:
                                        item.g += 0.51f;
                                        break;

                                    case 4:
                                        item.r += 0.51f;
                                        break;

                                    case 8:
                                        item.a += 0.51f;
                                        break;
                                }
                                for (int k = 0; k < 4; k++)
                                {
                                    cols.Add(item);
                                }
                            }
                        }
                        else
                        {
                            zero.x = vector.x * (x + symbol.offsetX);
                            zero.y = -vector.y * (num5 + symbol.offsetY);
                            vector3.x = zero.x + (vector.x * symbol.width);
                            vector3.y = zero.y - (vector.y * symbol.height);
                            Rect uvRect = symbol.uvRect;
                            vector4.x = uvRect.xMin;
                            vector4.y = uvRect.yMax;
                            vector5.x = uvRect.xMax;
                            vector5.y = uvRect.yMin;
                            x += this.mSpacingX + symbol.advance;
                            i += symbol.length - 1;
                            previousChar = 0;
                            if (symbolStyle == SymbolStyle.Colored)
                            {
                                for (int m = 0; m < 4; m++)
                                {
                                    cols.Add(color);
                                }
                            }
                            else
                            {
                                Color32 white = Color.white;
                                white.a = color.a;
                                for (int n = 0; n < 4; n++)
                                {
                                    cols.Add(white);
                                }
                            }
                        }
                        verts.Add(new Vector3(vector3.x, zero.y));
                        verts.Add(new Vector3(vector3.x, vector3.y));
                        verts.Add(new Vector3(zero.x, vector3.y));
                        verts.Add(new Vector3(zero.x, zero.y));
                        uvs.Add(new Vector2(vector5.x, vector4.y));
                        uvs.Add(new Vector2(vector5.x, vector5.y));
                        uvs.Add(new Vector2(vector4.x, vector5.y));
                        uvs.Add(new Vector2(vector4.x, vector4.y));
                        continue;
                    }
                    if (this.mDynamicFont.GetCharacterInfo(index, out mChar, this.mDynamicFontSize, this.mDynamicFontStyle))
                    {
                        zero.x = vector.x * (x + mChar.vert.xMin);
                        zero.y = -vector.y * ((num5 - mChar.vert.yMax) + this.mDynamicFontOffset);
                        vector3.x = zero.x + (vector.x * mChar.vert.width);
                        vector3.y = zero.y - (vector.y * mChar.vert.height);
                        vector4.x = mChar.uv.xMin;
                        vector4.y = mChar.uv.yMin;
                        vector5.x = mChar.uv.xMax;
                        vector5.y = mChar.uv.yMax;
                        x += this.mSpacingX + ((int)mChar.width);
                        for (int num18 = 0; num18 < 4; num18++)
                        {
                            cols.Add(color);
                        }
                        if (mChar.flipped)
                        {
                            uvs.Add(new Vector2(vector4.x, vector5.y));
                            uvs.Add(new Vector2(vector4.x, vector4.y));
                            uvs.Add(new Vector2(vector5.x, vector4.y));
                            uvs.Add(new Vector2(vector5.x, vector5.y));
                        }
                        else
                        {
                            uvs.Add(new Vector2(vector5.x, vector4.y));
                            uvs.Add(new Vector2(vector4.x, vector4.y));
                            uvs.Add(new Vector2(vector4.x, vector5.y));
                            uvs.Add(new Vector2(vector5.x, vector5.y));
                        }
                        verts.Add(new Vector3(vector3.x, zero.y));
                        verts.Add(new Vector3(zero.x, zero.y));
                        verts.Add(new Vector3(zero.x, vector3.y));
                        verts.Add(new Vector3(vector3.x, vector3.y));
                    }
                }
                if ((alignment != Alignment.Left) && (indexOffset < verts.size))
                {
                    this.Align(verts, indexOffset, alignment, x, lineWidth);
                    indexOffset = verts.size;
                }
            }
        }
    }

    public bool RecalculateDynamicOffset()
    {
        if (this.mDynamicFont != null)
        {
            CharacterInfo info;
            this.mDynamicFont.RequestCharactersInTexture("j", this.mDynamicFontSize, this.mDynamicFontStyle);
            this.mDynamicFont.GetCharacterInfo('j', out info, this.mDynamicFontSize, this.mDynamicFontStyle);
            float objB = this.mDynamicFontSize + info.vert.yMax;
            if (!object.Equals(this.mDynamicFontOffset, objB))
            {
                this.mDynamicFontOffset = objB;
                return true;
            }
        }
        return false;
    }

    private bool References(UIFont font)
    {
        if (font == null)
        {
            return false;
        }
        return ((font == this) || ((this.mReplacement != null) && this.mReplacement.References(font)));
    }

    public void RemoveSymbol(string sequence)
    {
        BMSymbol item = this.GetSymbol(sequence, false);
        if (item != null)
        {
            this.symbols.Remove(item);
        }
        this.MarkAsDirty();
    }

    public void RenameSymbol(string before, string after)
    {
        BMSymbol symbol = this.GetSymbol(before, false);
        if (symbol != null)
        {
            symbol.sequence = after;
        }
        this.MarkAsDirty();
    }

    private void Trim()
    {
        Texture texture = this.mAtlas.texture;
        if ((texture != null) && (this.mSprite != null))
        {
            Rect rect = NGUIMath.ConvertToPixels(this.mUVRect, this.texture.width, this.texture.height, true);
            Rect rect2 = (this.mAtlas.coordinates != UIAtlas.Coordinates.TexCoords) ? this.mSprite.outer : NGUIMath.ConvertToPixels(this.mSprite.outer, texture.width, texture.height, true);
            int xMin = Mathf.RoundToInt(rect2.xMin - rect.xMin);
            int yMin = Mathf.RoundToInt(rect2.yMin - rect.yMin);
            int xMax = Mathf.RoundToInt(rect2.xMax - rect.xMin);
            int yMax = Mathf.RoundToInt(rect2.yMax - rect.yMin);
            this.mFont.Trim(xMin, yMin, xMax, yMax);
        }
    }

    public bool UsesSprite(string s)
    {
        if (!string.IsNullOrEmpty(s))
        {
            if (s.Equals(this.spriteName))
            {
                return true;
            }
            int num = 0;
            int count = this.symbols.Count;
            while (num < count)
            {
                BMSymbol symbol = this.symbols[num];
                if (s.Equals(symbol.spriteName))
                {
                    return true;
                }
                num++;
            }
        }
        return false;
    }

    public string WrapText(string text, float maxWidth, int maxLineCount)
    {
        return this.WrapText(text, maxWidth, maxLineCount, false, SymbolStyle.None);
    }

    public string WrapText(string text, float maxWidth, int maxLineCount, bool encoding)
    {
        return this.WrapText(text, maxWidth, maxLineCount, encoding, SymbolStyle.None);
    }

    public string WrapText(string text, float maxWidth, int maxLineCount, bool encoding, SymbolStyle symbolStyle)
    {
        if (this.mReplacement != null)
        {
            return this.mReplacement.WrapText(text, maxWidth, maxLineCount, encoding, symbolStyle);
        }
        int num = Mathf.RoundToInt(maxWidth * this.size);
        if (num < 1)
        {
            return text;
        }
        StringBuilder s = new StringBuilder();
        int length = text.Length;
        int num3 = num;
        int previousChar = 0;
        int startIndex = 0;
        int offset = 0;
        bool flag = true;
        bool flag2 = maxLineCount != 1;
        int num7 = 1;
        bool flag3 = (encoding && (symbolStyle != SymbolStyle.None)) && this.hasSymbols;
        bool isDynamic = this.isDynamic;
        if (isDynamic)
        {
            this.mDynamicFont.textureRebuildCallback = new Font.FontTextureRebuildCallback(this.OnFontChanged);
            this.mDynamicFont.RequestCharactersInTexture(text, this.mDynamicFontSize, this.mDynamicFontStyle);
            this.mDynamicFont.textureRebuildCallback = null;
        }
        while (offset < length)
        {
            char ch = text[offset];
            if (ch == '\n')
            {
                if (!flag2 || (num7 == maxLineCount))
                {
                    break;
                }
                num3 = num;
                if (startIndex < offset)
                {
                    s.Append(text.Substring(startIndex, (offset - startIndex) + 1));
                }
                else
                {
                    s.Append(ch);
                }
                flag = true;
                num7++;
                startIndex = offset + 1;
                previousChar = 0;
                goto Label_03E7;
            }
            if (((ch == ' ') && (previousChar != 0x20)) && (startIndex < offset))
            {
                s.Append(text.Substring(startIndex, (offset - startIndex) + 1));
                flag = false;
                startIndex = offset + 1;
                previousChar = ch;
            }
            if ((encoding && (ch == '[')) && ((offset + 2) < length))
            {
                if ((text[offset + 1] == '-') && (text[offset + 2] == ']'))
                {
                    offset += 2;
                    goto Label_03E7;
                }
                if ((((offset + 7) < length) && (text[offset + 7] == ']')) && (NGUITools.EncodeColor(NGUITools.ParseColor(text, offset + 1)) == text.Substring(offset + 1, 6).ToUpper()))
                {
                    offset += 7;
                    goto Label_03E7;
                }
            }
            BMSymbol symbol = !flag3 ? null : this.MatchSymbol(text, offset, length);
            int mSpacingX = this.mSpacingX;
            if (!isDynamic)
            {
                if (symbol != null)
                {
                    mSpacingX += symbol.advance;
                }
                else
                {
                    BMGlyph glyph = (symbol != null) ? null : this.mFont.GetGlyph(ch);
                    if (glyph == null)
                    {
                        goto Label_03E7;
                    }
                    mSpacingX += (previousChar == 0) ? glyph.advance : (glyph.advance + glyph.GetKerning(previousChar));
                }
            }
            else if (this.mDynamicFont.GetCharacterInfo(ch, out mChar, this.mDynamicFontSize, this.mDynamicFontStyle))
            {
                mSpacingX += Mathf.RoundToInt(mChar.width);
            }
            num3 -= mSpacingX;
            if (num3 < 0)
            {
                if ((flag || !flag2) || (num7 == maxLineCount))
                {
                    s.Append(text.Substring(startIndex, Mathf.Max(0, offset - startIndex)));
                    if (!flag2 || (num7 == maxLineCount))
                    {
                        startIndex = offset;
                        break;
                    }
                    EndLine(ref s);
                    flag = true;
                    num7++;
                    if (ch == ' ')
                    {
                        startIndex = offset + 1;
                        num3 = num;
                    }
                    else
                    {
                        startIndex = offset;
                        num3 = num - mSpacingX;
                    }
                    previousChar = 0;
                    goto Label_03C8;
                }
                while ((startIndex < length) && (text[startIndex] == ' '))
                {
                    startIndex++;
                }
                flag = true;
                num3 = num;
                offset = startIndex - 1;
                previousChar = 0;
                if (!flag2 || (num7 == maxLineCount))
                {
                    break;
                }
                num7++;
                EndLine(ref s);
                goto Label_03E7;
            }
            previousChar = ch;
        Label_03C8:
            if (!isDynamic && (symbol != null))
            {
                offset += symbol.length - 1;
                previousChar = 0;
            }
        Label_03E7:
            offset++;
        }
        if (startIndex < offset)
        {
            s.Append(text.Substring(startIndex, offset - startIndex));
        }
        return s.ToString();
    }

    public UIAtlas atlas
    {
        get
        {
            return ((this.mReplacement == null) ? this.mAtlas : this.mReplacement.atlas);
        }
        set
        {
            if (this.mReplacement != null)
            {
                this.mReplacement.atlas = value;
            }
            else if (this.mAtlas != value)
            {
                if (value == null)
                {
                    if (this.mAtlas != null)
                    {
                        this.mMat = this.mAtlas.spriteMaterial;
                    }
                    if (this.sprite != null)
                    {
                        this.mUVRect = this.uvRect;
                    }
                }
                this.mPMA = -1;
                this.mAtlas = value;
                this.MarkAsDirty();
            }
        }
    }

    public BMFont bmFont
    {
        get
        {
            return ((this.mReplacement == null) ? this.mFont : this.mReplacement.bmFont);
        }
    }

    public Font dynamicFont
    {
        get
        {
            return ((this.mReplacement == null) ? this.mDynamicFont : this.mReplacement.dynamicFont);
        }
        set
        {
            if (this.mReplacement != null)
            {
                this.mReplacement.dynamicFont = value;
            }
            else if (this.mDynamicFont != value)
            {
                if (this.mDynamicFont != null)
                {
                    this.material = null;
                }
                this.mDynamicFont = value;
                this.MarkAsDirty();
            }
        }
    }

    public int dynamicFontSize
    {
        get
        {
            return ((this.mReplacement == null) ? this.mDynamicFontSize : this.mReplacement.dynamicFontSize);
        }
        set
        {
            if (this.mReplacement != null)
            {
                this.mReplacement.dynamicFontSize = value;
            }
            else
            {
                value = Mathf.Clamp(value, 4, 0x80);
                if (this.mDynamicFontSize != value)
                {
                    this.mDynamicFontSize = value;
                    this.MarkAsDirty();
                }
            }
        }
    }

    public FontStyle dynamicFontStyle
    {
        get
        {
            return ((this.mReplacement == null) ? this.mDynamicFontStyle : this.mReplacement.dynamicFontStyle);
        }
        set
        {
            if (this.mReplacement != null)
            {
                this.mReplacement.dynamicFontStyle = value;
            }
            else if (this.mDynamicFontStyle != value)
            {
                this.mDynamicFontStyle = value;
                this.MarkAsDirty();
            }
        }
    }

    private Texture dynamicTexture
    {
        get
        {
            if (this.mReplacement != null)
            {
                return this.mReplacement.dynamicTexture;
            }
            if (this.isDynamic)
            {
                return this.mDynamicFont.material.mainTexture;
            }
            return null;
        }
    }

    public bool hasSymbols
    {
        get
        {
            return ((this.mReplacement == null) ? (this.mSymbols.Count != 0) : this.mReplacement.hasSymbols);
        }
    }

    public int horizontalSpacing
    {
        get
        {
            return ((this.mReplacement == null) ? this.mSpacingX : this.mReplacement.horizontalSpacing);
        }
        set
        {
            if (this.mReplacement != null)
            {
                this.mReplacement.horizontalSpacing = value;
            }
            else if (this.mSpacingX != value)
            {
                this.mSpacingX = value;
                this.MarkAsDirty();
            }
        }
    }

    public bool isDynamic
    {
        get
        {
            return (this.mDynamicFont != null);
        }
    }

    public bool isValid
    {
        get
        {
            return ((this.mDynamicFont != null) || this.mFont.isValid);
        }
    }

    public Material material
    {
        get
        {
            if (this.mReplacement != null)
            {
                return this.mReplacement.material;
            }
            if (this.mAtlas != null)
            {
                return this.mAtlas.spriteMaterial;
            }
            if (this.mMat != null)
            {
                if ((this.mDynamicFont != null) && (this.mMat != this.mDynamicFont.material))
                {
                    this.mMat.mainTexture = this.mDynamicFont.material.mainTexture;
                }
                return this.mMat;
            }
            if (this.mDynamicFont != null)
            {
                return this.mDynamicFont.material;
            }
            return null;
        }
        set
        {
            if (this.mReplacement != null)
            {
                this.mReplacement.material = value;
            }
            else if (this.mMat != value)
            {
                this.mPMA = -1;
                this.mMat = value;
                this.MarkAsDirty();
            }
        }
    }

    public float pixelSize
    {
        get
        {
            if (this.mReplacement != null)
            {
                return this.mReplacement.pixelSize;
            }
            if (this.mAtlas != null)
            {
                return this.mAtlas.pixelSize;
            }
            return this.mPixelSize;
        }
        set
        {
            if (this.mReplacement != null)
            {
                this.mReplacement.pixelSize = value;
            }
            else if (this.mAtlas != null)
            {
                this.mAtlas.pixelSize = value;
            }
            else
            {
                float num = Mathf.Clamp(value, 0.25f, 4f);
                if (this.mPixelSize != num)
                {
                    this.mPixelSize = num;
                    this.MarkAsDirty();
                }
            }
        }
    }

    public bool premultipliedAlpha
    {
        get
        {
            if (this.mReplacement != null)
            {
                return this.mReplacement.premultipliedAlpha;
            }
            if (this.mAtlas != null)
            {
                return this.mAtlas.premultipliedAlpha;
            }
            if (this.mPMA == -1)
            {
                Material material = this.material;
                this.mPMA = (((material == null) || (material.shader == null)) || !material.shader.name.Contains("Premultiplied")) ? 0 : 1;
            }
            return (this.mPMA == 1);
        }
    }

    public UIFont replacement
    {
        get
        {
            return this.mReplacement;
        }
        set
        {
            UIFont font = value;
            if (font == this)
            {
                font = null;
            }
            if (this.mReplacement != font)
            {
                if ((font != null) && (font.replacement == this))
                {
                    font.replacement = null;
                }
                if (this.mReplacement != null)
                {
                    this.MarkAsDirty();
                }
                this.mReplacement = font;
                this.MarkAsDirty();
            }
        }
    }

    public int size
    {
        get
        {
            return ((this.mReplacement == null) ? (!this.isDynamic ? this.mFont.charSize : this.mDynamicFontSize) : this.mReplacement.size);
        }
    }

    public UIAtlas.Sprite sprite
    {
        get
        {
            if (this.mReplacement != null)
            {
                return this.mReplacement.sprite;
            }
            if (!this.mSpriteSet)
            {
                this.mSprite = null;
            }
            if (this.mSprite == null)
            {
                if ((this.mAtlas != null) && !string.IsNullOrEmpty(this.mFont.spriteName))
                {
                    this.mSprite = this.mAtlas.GetSprite(this.mFont.spriteName);
                    if (this.mSprite == null)
                    {
                        this.mSprite = this.mAtlas.GetSprite(base.name);
                    }
                    this.mSpriteSet = true;
                    if (this.mSprite == null)
                    {
                        this.mFont.spriteName = null;
                    }
                }
                int num = 0;
                int count = this.mSymbols.Count;
                while (num < count)
                {
                    this.symbols[num].MarkAsDirty();
                    num++;
                }
            }
            return this.mSprite;
        }
    }

    public string spriteName
    {
        get
        {
            return ((this.mReplacement == null) ? this.mFont.spriteName : this.mReplacement.spriteName);
        }
        set
        {
            if (this.mReplacement != null)
            {
                this.mReplacement.spriteName = value;
            }
            else if (this.mFont.spriteName != value)
            {
                this.mFont.spriteName = value;
                this.MarkAsDirty();
            }
        }
    }

    public List<BMSymbol> symbols
    {
        get
        {
            return ((this.mReplacement == null) ? this.mSymbols : this.mReplacement.symbols);
        }
    }

    public int texHeight
    {
        get
        {
            return ((this.mReplacement == null) ? ((this.mFont == null) ? 1 : this.mFont.texHeight) : this.mReplacement.texHeight);
        }
    }

    public Texture2D texture
    {
        get
        {
            if (this.mReplacement != null)
            {
                return this.mReplacement.texture;
            }
            Material material = this.material;
            return ((material == null) ? null : (material.mainTexture as Texture2D));
        }
    }

    public int texWidth
    {
        get
        {
            return ((this.mReplacement == null) ? ((this.mFont == null) ? 1 : this.mFont.texWidth) : this.mReplacement.texWidth);
        }
    }

    public Rect uvRect
    {
        get
        {
            if (this.mReplacement != null)
            {
                return this.mReplacement.uvRect;
            }
            if (((this.mAtlas != null) && (this.mSprite == null)) && (this.sprite != null))
            {
                Texture texture = this.mAtlas.texture;
                if (texture != null)
                {
                    this.mUVRect = this.mSprite.outer;
                    if (this.mAtlas.coordinates == UIAtlas.Coordinates.Pixels)
                    {
                        this.mUVRect = NGUIMath.ConvertToTexCoords(this.mUVRect, texture.width, texture.height);
                    }
                    if (this.mSprite.hasPadding)
                    {
                        Rect mUVRect = this.mUVRect;
                        this.mUVRect.xMin = mUVRect.xMin - (this.mSprite.paddingLeft * mUVRect.width);
                        this.mUVRect.yMin = mUVRect.yMin - (this.mSprite.paddingBottom * mUVRect.height);
                        this.mUVRect.xMax = mUVRect.xMax + (this.mSprite.paddingRight * mUVRect.width);
                        this.mUVRect.yMax = mUVRect.yMax + (this.mSprite.paddingTop * mUVRect.height);
                    }
                    if (this.mSprite.hasPadding)
                    {
                        this.Trim();
                    }
                }
            }
            return this.mUVRect;
        }
        set
        {
            if (this.mReplacement != null)
            {
                this.mReplacement.uvRect = value;
            }
            else if ((this.sprite == null) && (this.mUVRect != value))
            {
                this.mUVRect = value;
                this.MarkAsDirty();
            }
        }
    }

    public int verticalSpacing
    {
        get
        {
            return ((this.mReplacement == null) ? this.mSpacingY : this.mReplacement.verticalSpacing);
        }
        set
        {
            if (this.mReplacement != null)
            {
                this.mReplacement.verticalSpacing = value;
            }
            else if (this.mSpacingY != value)
            {
                this.mSpacingY = value;
                this.MarkAsDirty();
            }
        }
    }

    public enum Alignment
    {
        Left,
        Center,
        Right
    }

    public enum SymbolStyle
    {
        None,
        Uncolored,
        Colored
    }
}

