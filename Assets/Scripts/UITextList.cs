using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

[AddComponentMenu("NGUI/UI/Text List")]
public class UITextList : MonoBehaviour
{
    public int maxEntries = 50;
    public float maxHeight;
    public float maxWidth;
    protected List<Paragraph> mParagraphs = new List<Paragraph>();
    protected float mScroll;
    protected bool mSelected;
    protected char[] mSeparator = new char[] { '\n' };
    protected int mTotalLines;
    public Style style;
    public bool supportScrollWheel = true;
    public UILabel textLabel;

    public void Add(string text)
    {
        this.Add(text, true);
    }

    protected void Add(string text, bool updateVisible)
    {
        Paragraph item = null;
        if (this.mParagraphs.Count < this.maxEntries)
        {
            item = new Paragraph();
        }
        else
        {
            item = this.mParagraphs[0];
            this.mParagraphs.RemoveAt(0);
        }
        item.text = text;
        this.mParagraphs.Add(item);
        if ((this.textLabel != null) && (this.textLabel.font != null))
        {
            item.lines = this.textLabel.font.WrapText(item.text, this.maxWidth / this.textLabel.transform.localScale.y, this.textLabel.maxLineCount, this.textLabel.supportEncoding, this.textLabel.symbolStyle).Split(this.mSeparator);
            this.mTotalLines = 0;
            int num = 0;
            int count = this.mParagraphs.Count;
            while (num < count)
            {
                this.mTotalLines += this.mParagraphs[num].lines.Length;
                num++;
            }
        }
        if (updateVisible)
        {
            this.UpdateVisibleText();
        }
    }

    private void Awake()
    {
        if (this.textLabel == null)
        {
            this.textLabel = base.GetComponentInChildren<UILabel>();
        }
        if (this.textLabel != null)
        {
            this.textLabel.lineWidth = 0;
        }
        Collider collider = base.GetComponent<Collider>();
        if (collider != null)
        {
            if (this.maxHeight <= 0f)
            {
                this.maxHeight = collider.bounds.size.y / base.transform.lossyScale.y;
            }
            if (this.maxWidth <= 0f)
            {
                this.maxWidth = collider.bounds.size.x / base.transform.lossyScale.x;
            }
        }
    }

    public void Clear()
    {
        this.mParagraphs.Clear();
        this.UpdateVisibleText();
    }

    private void OnScroll(float val)
    {
        if (this.mSelected && this.supportScrollWheel)
        {
            val *= (this.style != Style.Chat) ? -10f : 10f;
            this.mScroll = Mathf.Max((float) 0f, (float) (this.mScroll + val));
            this.UpdateVisibleText();
        }
    }

    private void OnSelect(bool selected)
    {
        this.mSelected = selected;
    }

    protected void UpdateVisibleText()
    {
        if ((this.textLabel != null) && (this.textLabel.font != null))
        {
            int num = 0;
            int num2 = (this.maxHeight <= 0f) ? 0x186a0 : Mathf.FloorToInt(this.maxHeight / this.textLabel.cachedTransform.localScale.y);
            int num3 = Mathf.RoundToInt(this.mScroll);
            if ((num2 + num3) > this.mTotalLines)
            {
                num3 = Mathf.Max(0, this.mTotalLines - num2);
                this.mScroll = num3;
            }
            if (this.style == Style.Chat)
            {
                num3 = Mathf.Max(0, (this.mTotalLines - num2) - num3);
            }
            StringBuilder builder = new StringBuilder();
            int num4 = 0;
            int count = this.mParagraphs.Count;
            while (num4 < count)
            {
                Paragraph paragraph = this.mParagraphs[num4];
                int index = 0;
                int length = paragraph.lines.Length;
                while (index < length)
                {
                    string str = paragraph.lines[index];
                    if (num3 > 0)
                    {
                        num3--;
                    }
                    else
                    {
                        if (builder.Length > 0)
                        {
                            builder.Append("\n");
                        }
                        builder.Append(str);
                        num++;
                        if (num >= num2)
                        {
                            break;
                        }
                    }
                    index++;
                }
                if (num >= num2)
                {
                    break;
                }
                num4++;
            }
            this.textLabel.text = builder.ToString();
        }
    }

    protected class Paragraph
    {
        public string[] lines;
        public string text;
    }

    public enum Style
    {
        Text,
        Chat
    }
}

