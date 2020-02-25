using System;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("NGUI/Examples/Equip Random Item")]
public class EquipRandomItem : MonoBehaviour
{
    public InvEquipment equipment;

    private void OnClick()
    {
        if (this.equipment != null)
        {
            List<InvBaseItem> items = InvDatabase.list[0].items;
            if (items.Count != 0)
            {
                int id = UnityEngine.Random.Range(0, items.Count);
                InvBaseItem bi = items[id];
                InvGameItem item = new InvGameItem(id, bi) {
                    quality = (InvGameItem.Quality) UnityEngine.Random.Range(0, 12),
                    itemLevel = NGUITools.RandomRange(bi.minItemLevel, bi.maxItemLevel)
                };
                this.equipment.Equip(item);
            }
        }
    }
}

