using UnityEngine;

[AddComponentMenu("NGUI/Examples/UI Equipment Slot")]
public class UIEquipmentSlot : UIItemSlot
{
    public InvEquipment equipment;
    public InvBaseItem.Slot slot;

    protected override InvGameItem Replace(InvGameItem item)
    {
        return ((this.equipment == null) ? item : this.equipment.Replace(this.slot, item));
    }

    protected override InvGameItem observedItem
    {
        get
        {
            return ((this.equipment == null) ? null : this.equipment.GetItem(this.slot));
        }
    }
}

