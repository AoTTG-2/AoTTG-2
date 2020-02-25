using System;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("NGUI/Examples/UI Item Storage")]
public class UIItemStorage : MonoBehaviour
{
    public UIWidget background;
    public int maxColumns = 4;
    public int maxItemCount = 8;
    public int maxRows = 4;
    private List<InvGameItem> mItems = new List<InvGameItem>();
    public int padding = 10;
    public int spacing = 0x80;
    public GameObject template;

    public InvGameItem GetItem(int slot)
    {
        return ((slot >= this.items.Count) ? null : this.mItems[slot]);
    }

    public InvGameItem Replace(int slot, InvGameItem item)
    {
        if (slot < this.maxItemCount)
        {
            InvGameItem item2 = this.items[slot];
            this.mItems[slot] = item;
            return item2;
        }
        return item;
    }

    private void Start()
    {
        if (this.template != null)
        {
            int num = 0;
            Bounds bounds = new Bounds();
            for (int i = 0; i < this.maxRows; i++)
            {
                for (int j = 0; j < this.maxColumns; j++)
                {
                    GameObject obj2 = NGUITools.AddChild(base.gameObject, this.template);
                    obj2.transform.localPosition = new Vector3(this.padding + ((j + 0.5f) * this.spacing), -this.padding - ((i + 0.5f) * this.spacing), 0f);
                    UIStorageSlot component = obj2.GetComponent<UIStorageSlot>();
                    if (component != null)
                    {
                        component.storage = this;
                        component.slot = num;
                    }
                    bounds.Encapsulate(new Vector3((this.padding * 2f) + ((j + 1) * this.spacing), (-this.padding * 2f) - ((i + 1) * this.spacing), 0f));
                    if (++num >= this.maxItemCount)
                    {
                        if (this.background != null)
                        {
                            this.background.transform.localScale = bounds.size;
                        }
                        return;
                    }
                }
            }
            if (this.background != null)
            {
                this.background.transform.localScale = bounds.size;
            }
        }
    }

    public List<InvGameItem> items
    {
        get
        {
            while (this.mItems.Count < this.maxItemCount)
            {
                this.mItems.Add(null);
            }
            return this.mItems;
        }
    }
}

