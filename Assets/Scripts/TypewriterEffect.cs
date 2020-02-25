using System;
using UnityEngine;

[AddComponentMenu("NGUI/Examples/Typewriter Effect"), RequireComponent(typeof(UILabel))]
public class TypewriterEffect : MonoBehaviour
{
    public int charsPerSecond = 40;
    private UILabel mLabel;
    private float mNextChar;
    private int mOffset;
    private string mText;

    private void Update()
    {
        if (this.mLabel == null)
        {
            this.mLabel = base.GetComponent<UILabel>();
            this.mLabel.supportEncoding = false;
            this.mLabel.symbolStyle = UIFont.SymbolStyle.None;
            this.mText = this.mLabel.font.WrapText(this.mLabel.text, ((float) this.mLabel.lineWidth) / this.mLabel.cachedTransform.localScale.x, this.mLabel.maxLineCount, false, UIFont.SymbolStyle.None);
        }
        if (this.mOffset < this.mText.Length)
        {
            if (this.mNextChar <= Time.time)
            {
                this.charsPerSecond = Mathf.Max(1, this.charsPerSecond);
                float num = 1f / ((float) this.charsPerSecond);
                switch (this.mText[this.mOffset])
                {
                    case '.':
                    case '\n':
                    case '!':
                    case '?':
                        num *= 4f;
                        break;
                }
                this.mNextChar = Time.time + num;
                this.mLabel.text = this.mText.Substring(0, ++this.mOffset);
            }
        }
        else
        {
            UnityEngine.Object.Destroy(this);
        }
    }
}

