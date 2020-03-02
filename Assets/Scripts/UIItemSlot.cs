using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class UIItemSlot : MonoBehaviour
{
    public UIWidget background;
    public AudioClip errorSound;
    public AudioClip grabSound;
    public UISprite icon;
    public UILabel label;
    private static InvGameItem mDraggedItem;
    private InvGameItem mItem;
    private string mText = string.Empty;
    public AudioClip placeSound;

    protected UIItemSlot()
    {
    }

    private void OnClick()
    {
        if (mDraggedItem != null)
        {
            this.OnDrop(null);
        }
        else if (this.mItem != null)
        {
            mDraggedItem = this.Replace(null);
            if (mDraggedItem != null)
            {
                NGUITools.PlaySound(this.grabSound);
            }
            this.UpdateCursor();
        }
    }

    private void OnDrag(Vector2 delta)
    {
        if ((mDraggedItem == null) && (this.mItem != null))
        {
            UICamera.currentTouch.clickNotification = UICamera.ClickNotification.BasedOnDelta;
            mDraggedItem = this.Replace(null);
            NGUITools.PlaySound(this.grabSound);
            this.UpdateCursor();
        }
    }

    private void OnDrop(GameObject go)
    {
        InvGameItem item = this.Replace(mDraggedItem);
        if (mDraggedItem == item)
        {
            NGUITools.PlaySound(this.errorSound);
        }
        else if (item != null)
        {
            NGUITools.PlaySound(this.grabSound);
        }
        else
        {
            NGUITools.PlaySound(this.placeSound);
        }
        mDraggedItem = item;
        this.UpdateCursor();
    }

    private void OnTooltip(bool show)
    {
        InvGameItem item = !show ? null : this.mItem;
        if (item != null)
        {
            InvBaseItem baseItem = item.baseItem;
            if (baseItem != null)
            {
                string[] textArray1 = new string[] { "[", NGUITools.EncodeColor(item.color), "]", item.name, "[-]\n" };
                string str2 = string.Concat(textArray1);
                object[] objArray1 = new object[] { str2, "[AFAFAF]Level ", item.itemLevel, " ", baseItem.slot };
                string tooltipText = string.Concat(objArray1);
                List<InvStat> list = item.CalculateStats();
                int num = 0;
                int count = list.Count;
                while (num < count)
                {
                    InvStat stat = list[num];
                    if (stat.amount != 0)
                    {
                        if (stat.amount < 0)
                        {
                            tooltipText = tooltipText + "\n[FF0000]" + stat.amount;
                        }
                        else
                        {
                            tooltipText = tooltipText + "\n[00FF00]+" + stat.amount;
                        }
                        if (stat.modifier == InvStat.Modifier.Percent)
                        {
                            tooltipText = tooltipText + "%";
                        }
                        tooltipText = (tooltipText + " " + stat.id) + "[-]";
                    }
                    num++;
                }
                if (!string.IsNullOrEmpty(baseItem.description))
                {
                    tooltipText = tooltipText + "\n[FF9900]" + baseItem.description;
                }
                UITooltip.ShowText(tooltipText);
                return;
            }
        }
        UITooltip.ShowText(null);
    }

    protected abstract InvGameItem Replace(InvGameItem item);
    private void Update()
    {
        InvGameItem observedItem = this.observedItem;
        if (this.mItem != observedItem)
        {
            this.mItem = observedItem;
            InvBaseItem item2 = (observedItem == null) ? null : observedItem.baseItem;
            if (this.label != null)
            {
                string str = (observedItem == null) ? null : observedItem.name;
                if (string.IsNullOrEmpty(this.mText))
                {
                    this.mText = this.label.text;
                }
                this.label.text = (str == null) ? this.mText : str;
            }
            if (this.icon != null)
            {
                if ((item2 != null) && (item2.iconAtlas != null))
                {
                    this.icon.atlas = item2.iconAtlas;
                    this.icon.spriteName = item2.iconName;
                    this.icon.enabled = true;
                    this.icon.MakePixelPerfect();
                }
                else
                {
                    this.icon.enabled = false;
                }
            }
            if (this.background != null)
            {
                this.background.color = (observedItem == null) ? Color.white : observedItem.color;
            }
        }
    }

    private void UpdateCursor()
    {
        if ((mDraggedItem != null) && (mDraggedItem.baseItem != null))
        {
            UICursor.Set(mDraggedItem.baseItem.iconAtlas, mDraggedItem.baseItem.iconName);
        }
        else
        {
            UICursor.Clear();
        }
    }

    protected abstract InvGameItem observedItem { get; }
}

