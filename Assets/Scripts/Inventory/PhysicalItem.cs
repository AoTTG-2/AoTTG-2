using UnityEngine;

namespace Assets.Scripts.Inventory
{
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
