using System;
using UnityEngine;

[AddComponentMenu("NGUI/Examples/UI Storage Slot")]
public class UIStorageSlot : UIItemSlot
{
    public int slot;
    public UIItemStorage storage;

    protected override InvGameItem Replace(InvGameItem item)
    {
        return ((this.storage == null) ? item : this.storage.Replace(this.slot, item));
    }

    protected override InvGameItem observedItem
    {
        get
        {
            return ((this.storage == null) ? null : this.storage.GetItem(this.slot));
        }
    }
}

