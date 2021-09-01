using UnityEngine;

namespace Assets.Scripts.Inventory
{
    /// <summary>
    /// Component attached to a physical representation of an inventory item.
    /// </summary>
    public class PhysicalItem : MonoBehaviour
    {
        public InventoryItem myInventoryItem;

        private void Start()
        {
            gameObject.tag = "item";
            gameObject.layer = 25; //25 is current item layer
        }

    }

}
