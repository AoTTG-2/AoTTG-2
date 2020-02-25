using System;
using UnityEngine;

[AddComponentMenu("NGUI/Examples/Equip Items")]
public class EquipItems : MonoBehaviour
{
    public int[] itemIDs;

    private void Start()
    {
        if ((this.itemIDs != null) && (this.itemIDs.Length > 0))
        {
            InvEquipment component = base.GetComponent<InvEquipment>();
            if (component == null)
            {
                component = base.gameObject.AddComponent<InvEquipment>();
            }
            int max = 12;
            int index = 0;
            int length = this.itemIDs.Length;
            while (index < length)
            {
                int num4 = this.itemIDs[index];
                InvBaseItem bi = InvDatabase.FindByID(num4);
                if (bi != null)
                {
                    InvGameItem item = new InvGameItem(num4, bi) {
                        quality = (InvGameItem.Quality) UnityEngine.Random.Range(0, max),
                        itemLevel = NGUITools.RandomRange(bi.minItemLevel, bi.maxItemLevel)
                    };
                    component.Equip(item);
                }
                else
                {
                    Debug.LogWarning("Can't resolve the item ID of " + num4);
                }
                index++;
            }
        }
        UnityEngine.Object.Destroy(this);
    }
}

