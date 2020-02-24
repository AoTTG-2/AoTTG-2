using System;
using UnityEngine;

[ExecuteInEditMode, AddComponentMenu("NGUI/UI/Label")]
public class UILabel : UIWidget
{
    [SerializeField, HideInInspector]
    private Color mEffectColor = Color.black;
    [HideInInspector, SerializeField]
    private Vector2 mEffectDistance = Vector2.one;
    [HideInInspector, SerializeField]
    private Effect mEffectStyle;
    [SerializeField, HideInInspector]
    private bool mEncoding = true;
    [SerializeField, HideInInspector]
    private UIFont mFont;
    private int mLastCount;
    private Effect mLastEffect;
    private bool mLastEncoding = true;
    private bool mLastPass;
    private Vector3 mLastScale = Vector3.one;
    private bool mLastShow;
    private string mLastText = string.Empty;
    private int mLastWidth;
    [SerializeField, HideInInspector]
    private float mLineWidth;
    [HideInInspector, SerializeField]
    private int mMaxLineCount;
    [HideInInspector, SerializeField]
    private int mMaxLineWidth;
    [SerializeField, HideInInspector]
    private bool mMultiline = true;
    [SerializeField, HideInInspector]
    private bool mPassword;
    private bool mPremultiply;
    private string mProcessedText;
    private bool mShouldBeProcessed = true;
    [HideInInspector, SerializeField]
    private bool mShowLastChar;
    [SerializeField, HideInInspector]
    private bool mShrinkToFit;
    private Vector2 mSize = Vector2.zero;
    [HideInInspector, SerializeField]
    private UIFont.SymbolStyle mSymbols = UIFont.SymbolStyle.Uncolored;
    [HideInInspector, SerializeField]
    private string mText = string.Empty;

    private void ApplyShadow(BetterList<Vector3> verts, BetterList<Vector2> uvs, BetterList<Color32> cols, int start, int end, float x, float y)
    {
        Color mEffectColor = this.mEffectColor;
        mEffectColor.a *= base.alpha * base.mPanel.alpha;
        Color32 color2 = !this.font.premultipliedAlpha ? mEffectColor : NGUITools.ApplyPMA(mEffectColor);
        for (int i = start; i < end; i++)
        {
            verts.Add(verts.buffer[i]);
            uvs.Add(uvs.buffer[i]);
            cols.Add(cols.buffer[i]);
            Vector3 vector = verts.buffer[i];
            vector.x += x;
            vector.y += y;
            verts.buffer[i] = vector;
            cols.buffer[i] = color2;
        }
    }

    public override void MakePixelPerfect()
    {
        if (this.mFont != null)
        {
            float pixelSize = this.font.pixelSize;
            Vector3 localScale = base.cachedTransform.localScale;
            localScale.x = this.mFont.size * pixelSize;
            localScale.y = localScale.x;
            localScale.z = 1f;
            Vector3 localPosition = base.cachedTransform.localPosition;
            localPosition.x = Mathf.CeilToInt((localPosition.x / pixelSize) * 4f) >> 2;
            localPosition.y = Mathf.CeilToInt((localPosition.y / pixelSize) * 4f) >> 2;
            localPosition.z = Mathf.RoundToInt(localPosition.z);
            localPosition.x *= pixelSize;
            localPosition.y *= pixelSize;
            base.cachedTransform.localPosition = localPosition;
            base.cachedTransform.localScale = localScale;
            if (this.shrinkToFit)
            {
                this.ProcessText();
            }
        }
        else
        {
            base.MakePixelPerfect();
        }
    }

    public override void MarkAsChanged()
    {
        this.hasChanged = true;
        base.MarkAsChanged();
    }

    public override void OnFill(BetterList<Vector3> verts, BetterList<Vector2> uvs, BetterList<Color32> cols)
    {
        if (this.mFont != null)
        {
            UIWidget.Pivot pivot = base.pivot;
            int size = verts.size;
            Color c = base.color;
            c.a *= base.mPanel.alpha;
            if (this.font.premultipliedAlpha)
            {
                c = NGUITools.ApplyPMA(c);
            }
            switch (pivot)
            {
                case UIWidget.Pivot.Left:
                case UIWidget.Pivot.TopLeft:
                case UIWidget.Pivot.BottomLeft:
                    this.mFont.Print(this.processedText, c, verts, uvs, cols, this.mEncoding, this.mSymbols, UIFont.Alignment.Left, 0, this.mPremultiply);
                    break;

                default:
                    if (((pivot != UIWidget.Pivot.Right) && (pivot != UIWidget.Pivot.TopRight)) && (pivot != UIWidget.Pivot.BottomRight))
                    {
                        this.mFont.Print(this.processedText, c, verts, uvs, cols, this.mEncoding, this.mSymbols, UIFont.Alignment.Center, Mathf.RoundToInt(this.relativeSize.x * this.mFont.size), this.mPremultiply);
                    }
                    else
                    {
                        this.mFont.Print(this.processedText, c, verts, uvs, cols, this.mEncoding, this.mSymbols, UIFont.Alignment.Right, Mathf.RoundToInt(this.relativeSize.x * this.mFont.size), this.mPremultiply);
                    }
                    break;
            }
            if (this.effectStyle != Effect.None)
            {
                int end = verts.size;
                float num3 = 1f / ((float) this.mFont.size);
                float x = num3 * this.mEffectDistance.x;
                float y = num3 * this.mEffectDistance.y;
                this.ApplyShadow(verts, uvs, cols, size, end, x, -y);
                if (this.effectStyle == Effect.Outline)
                {
                    size = end;
                    end = verts.size;
                    this.ApplyShadow(verts, uvs, cols, size, end, -x, y);
                    size = end;
                    end = verts.size;
                    this.ApplyShadow(verts, uvs, cols, size, end, x, y);
                    size = end;
                    end = verts.size;
                    this.ApplyShadow(verts, uvs, cols, size, end, -x, -y);
                }
            }
        }
    }

    protected override void OnStart()
    {
        if (this.mLineWidth > 0f)
        {
            this.mMaxLineWidth = Mathf.RoundToInt(this.mLineWidth);
            this.mLineWidth = 0f;
        }
        if (!this.mMultiline)
        {
            this.mMaxLineCount = 1;
            this.mMultiline = true;
        }
        this.mPremultiply = ((this.font != null) && (this.font.material != null)) && this.font.material.shader.name.Contains("Premultiplied");
    }

    private void ProcessText()
    {
        base.mChanged = true;
        this.hasChanged = false;
        this.mLastText = this.mText;
        float b = Mathf.Abs(base.cachedTransform.localScale.x);
        float num2 = this.mFont.size * this.mMaxLineCount;
        if (b <= 0f)
        {
            this.mSize.x = 1f;
            b = this.mFont.size;
            base.cachedTransform.localScale = new Vector3(0.01f, 0.01f, 1f);
            this.mProcessedText = string.Empty;
            goto Label_037C;
        }
    Label_0057:
        if (this.mPassword)
        {
            this.mProcessedText = string.Empty;
            if (this.mShowLastChar)
            {
                int num3 = 0;
                int num4 = this.mText.Length - 1;
                while (num3 < num4)
                {
                    this.mProcessedText = this.mProcessedText + "*";
                    num3++;
                }
                if (this.mText.Length > 0)
                {
                    this.mProcessedText = this.mProcessedText + this.mText[this.mText.Length - 1];
                }
            }
            else
            {
                int num5 = 0;
                int length = this.mText.Length;
                while (num5 < length)
                {
                    this.mProcessedText = this.mProcessedText + "*";
                    num5++;
                }
            }
            this.mProcessedText = this.mFont.WrapText(this.mProcessedText, ((float)this.mMaxLineWidth) / b, this.mMaxLineCount, false, UIFont.SymbolStyle.None);
        }
        else if (this.mMaxLineWidth > 0)
        {
            this.mProcessedText = this.mFont.WrapText(this.mText, ((float)this.mMaxLineWidth) / b, !this.mShrinkToFit ? this.mMaxLineCount : 0, this.mEncoding, this.mSymbols);
        }
        else if (!this.mShrinkToFit && (this.mMaxLineCount > 0))
        {
            this.mProcessedText = this.mFont.WrapText(this.mText, 100000f, this.mMaxLineCount, this.mEncoding, this.mSymbols);
        }
        else
        {
            this.mProcessedText = this.mText;
        }
        this.mSize = string.IsNullOrEmpty(this.mProcessedText) ? Vector2.one : this.mFont.CalculatePrintedSize(this.mProcessedText, this.mEncoding, this.mSymbols);
        if (this.mShrinkToFit)
        {
            if ((this.mMaxLineCount > 0) && ((this.mSize.y * b) > num2))
            {
                b = Mathf.Round(b - 1f);
                if (b > 1f)
                {
                    goto Label_0057;
                }
            }
            if (this.mMaxLineWidth > 0)
            {
                float num7 = ((float)this.mMaxLineWidth) / b;
                float a = ((this.mSize.x * b) <= num7) ? b : ((num7 / this.mSize.x) * b);
                b = Mathf.Min(a, b);
            }
            b = Mathf.Round(b);
            base.cachedTransform.localScale = new Vector3(b, b, 1f);
        }
        this.mSize.x = Mathf.Max(this.mSize.x, (b <= 0f) ? 1f : (((float)this.lineWidth) / b));
    Label_037C:
        this.mSize.y = Mathf.Max(this.mSize.y, 1f);
    }

    public Color effectColor
    {
        get
        {
            return this.mEffectColor;
        }
        set
        {
            if (!this.mEffectColor.Equals(value))
            {
                this.mEffectColor = value;
                if (this.mEffectStyle != Effect.None)
                {
                    this.hasChanged = true;
                }
            }
        }
    }

    public Vector2 effectDistance
    {
        get
        {
            return this.mEffectDistance;
        }
        set
        {
            if (this.mEffectDistance != value)
            {
                this.mEffectDistance = value;
                this.hasChanged = true;
            }
        }
    }

    public Effect effectStyle
    {
        get
        {
            return this.mEffectStyle;
        }
        set
        {
            if (this.mEffectStyle != value)
            {
                this.mEffectStyle = value;
                this.hasChanged = true;
            }
        }
    }

    public UIFont font
    {
        get
        {
            return this.mFont;
        }
        set
        {
            if (this.mFont != value)
            {
                this.mFont = value;
                this.material = (this.mFont == null) ? null : this.mFont.material;
                base.mChanged = true;
                this.hasChanged = true;
                this.MarkAsChanged();
            }
        }
    }

    private bool hasChanged
    {
        get
        {
            return ((((this.mShouldBeProcessed || (this.mLastText != this.text)) || ((this.mLastWidth != this.mMaxLineWidth) || (this.mLastEncoding != this.mEncoding))) || (((this.mLastCount != this.mMaxLineCount) || (this.mLastPass != this.mPassword)) || (this.mLastShow != this.mShowLastChar))) || (this.mLastEffect != this.mEffectStyle));
        }
        set
        {
            if (value)
            {
                base.mChanged = true;
                this.mShouldBeProcessed = true;
            }
            else
            {
                this.mShouldBeProcessed = false;
                this.mLastText = this.text;
                this.mLastWidth = this.mMaxLineWidth;
                this.mLastEncoding = this.mEncoding;
                this.mLastCount = this.mMaxLineCount;
                this.mLastPass = this.mPassword;
                this.mLastShow = this.mShowLastChar;
                this.mLastEffect = this.mEffectStyle;
            }
        }
    }

    public int lineWidth
    {
        get
        {
            return this.mMaxLineWidth;
        }
        set
        {
            if (this.mMaxLineWidth != value)
            {
                this.mMaxLineWidth = value;
                this.hasChanged = true;
                if (this.shrinkToFit)
                {
                    this.MakePixelPerfect();
                }
            }
        }
    }

    public override Material material
    {
        get
        {
            Material material = base.material;
            if (material == null)
            {
                material = (this.mFont == null) ? null : this.mFont.material;
                this.material = material;
            }
            return material;
        }
    }

    public int maxLineCount
    {
        get
        {
            return this.mMaxLineCount;
        }
        set
        {
            if (this.mMaxLineCount != value)
            {
                this.mMaxLineCount = Mathf.Max(value, 0);
                this.hasChanged = true;
                if (value == 1)
                {
                    this.mPassword = false;
                }
            }
        }
    }

    public bool multiLine
    {
        get
        {
            return (this.mMaxLineCount != 1);
        }
        set
        {
            if ((this.mMaxLineCount != 1) != value)
            {
                this.mMaxLineCount = !value ? 1 : 0;
                this.hasChanged = true;
                if (value)
                {
                    this.mPassword = false;
                }
            }
        }
    }

    public bool password
    {
        get
        {
            return this.mPassword;
        }
        set
        {
            if (this.mPassword != value)
            {
                if (value)
                {
                    this.mMaxLineCount = 1;
                    this.mEncoding = false;
                }
                this.mPassword = value;
                this.hasChanged = true;
            }
        }
    }

    public string processedText
    {
        get
        {
            if (this.mLastScale != base.cachedTransform.localScale)
            {
                this.mLastScale = base.cachedTransform.localScale;
                this.mShouldBeProcessed = true;
            }
            if (this.hasChanged)
            {
                this.ProcessText();
            }
            return this.mProcessedText;
        }
    }

    public override Vector2 relativeSize
    {
        get
        {
            if (this.mFont == null)
            {
                return Vector3.one;
            }
            if (this.hasChanged)
            {
                this.ProcessText();
            }
            return this.mSize;
        }
    }

    public bool showLastPasswordChar
    {
        get
        {
            return this.mShowLastChar;
        }
        set
        {
            if (this.mShowLastChar != value)
            {
                this.mShowLastChar = value;
                this.hasChanged = true;
            }
        }
    }

    public bool shrinkToFit
    {
        get
        {
            return this.mShrinkToFit;
        }
        set
        {
            if (this.mShrinkToFit != value)
            {
                this.mShrinkToFit = value;
                this.hasChanged = true;
            }
        }
    }

    public bool supportEncoding
    {
        get
        {
            return this.mEncoding;
        }
        set
        {
            if (this.mEncoding != value)
            {
                this.mEncoding = value;
                this.hasChanged = true;
                if (value)
                {
                    this.mPassword = false;
                }
            }
        }
    }

    public UIFont.SymbolStyle symbolStyle
    {
        get
        {
            return this.mSymbols;
        }
        set
        {
            if (this.mSymbols != value)
            {
                this.mSymbols = value;
                this.hasChanged = true;
            }
        }
    }

    public string text
    {
        get
        {
            return this.mText;
        }
        set
        {
            if (string.IsNullOrEmpty(value))
            {
                if (!string.IsNullOrEmpty(this.mText))
                {
                    this.mText = string.Empty;
                }
                this.hasChanged = true;
            }
            else if (this.mText != value)
            {
                this.mText = value;
                this.hasChanged = true;
                if (this.shrinkToFit)
                {
                    this.MakePixelPerfect();
                }
            }
        }
    }

    public enum Effect
    {
        None,
        Shadow,
        Outline
    }
}

