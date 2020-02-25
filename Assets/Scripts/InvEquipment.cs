using System;
using UnityEngine;

[AddComponentMenu("NGUI/Examples/Equipment")]
public class InvEquipment : MonoBehaviour
{
    private InvAttachmentPoint[] mAttachments;
    private InvGameItem[] mItems;

    public InvGameItem Equip(InvGameItem item)
    {
        if (item != null)
        {
            InvBaseItem baseItem = item.baseItem;
            if (baseItem != null)
            {
                return this.Replace(baseItem.slot, item);
            }
            Debug.LogWarning("Can't resolve the item ID of " + item.baseItemID);
        }
        return item;
    }

    public InvGameItem GetItem(InvBaseItem.Slot slot)
    {
        if (slot != InvBaseItem.Slot.None)
        {
            int index = ((int) slot) - 1;
            if ((this.mItems != null) && (index < this.mItems.Length))
            {
                return this.mItems[index];
            }
        }
        return null;
    }

    public bool HasEquipped(InvBaseItem.Slot slot)
    {
        if (this.mItems != null)
        {
            int index = 0;
            int length = this.mItems.Length;
            while (index < length)
            {
                InvBaseItem baseItem = this.mItems[index].baseItem;
                if ((baseItem != null) && (baseItem.slot == slot))
                {
                    return true;
                }
                index++;
            }
        }
        return false;
    }

    public bool HasEquipped(InvGameItem item)
    {
        if (this.mItems != null)
        {
            int index = 0;
            int length = this.mItems.Length;
            while (index < length)
            {
                if (this.mItems[index] == item)
                {
                    return true;
                }
                index++;
            }
        }
        return false;
    }

    public InvGameItem Replace(InvBaseItem.Slot slot, InvGameItem item)
    {
        InvBaseItem item2 = (item == null) ? null : item.baseItem;
        if (slot != InvBaseItem.Slot.None)
        {
            if ((item2 != null) && (item2.slot != slot))
            {
                return item;
            }
            if (this.mItems == null)
            {
                this.mItems = new InvGameItem[8];
            }
            InvGameItem item3 = this.mItems[((int) slot) - 1];
            this.mItems[((int) slot) - 1] = item;
            if (this.mAttachments == null)
            {
                this.mAttachments = base.GetComponentsInChildren<InvAttachmentPoint>();
            }
            int index = 0;
            int length = this.mAttachments.Length;
            while (index < length)
            {
                InvAttachmentPoint point = this.mAttachments[index];
                if (point.slot == slot)
                {
                    GameObject obj2 = point.Attach((item2 == null) ? null : item2.attachment);
                    if ((item2 != null) && (obj2 != null))
                    {
                        Renderer renderer = obj2.GetComponent<Renderer>();
                        if (renderer != null)
                        {
                            renderer.material.color = item2.color;
                        }
                    }
                }
                index++;
            }
            return item3;
        }
        if (item != null)
        {
            Debug.LogWarning("Can't equip \"" + item.name + "\" because it doesn't specify an item slot");
        }
        return item;
    }

    public InvGameItem Unequip(InvBaseItem.Slot slot)
    {
        return this.Replace(slot, null);
    }

    public InvGameItem Unequip(InvGameItem item)
    {
        if (item != null)
        {
            InvBaseItem baseItem = item.baseItem;
            if (baseItem != null)
            {
                return this.Replace(baseItem.slot, null);
            }
        }
        return item;
    }

    public InvGameItem[] equippedItems
    {
        get
        {
            return this.mItems;
        }
    }
}

