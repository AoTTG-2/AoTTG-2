using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace Assets.Scripts.Inventory
{
    [System.Serializable]
    [CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Items")]
    public class InventoryItem : ScriptableObject
    {

        public static UnityEvent<string> thisEvent;
        [Tooltip("MUST MATCH NAME OF PHYSICAL ITEM")]
        public string itemName;
        public string itemDesc;
        public Image itemImage;
        public GameObject item;

        public void Use()
        {

            Debug.Log($"Used the item: {itemName}");
            item.GetComponent<IFunctionality>().Use();

        }

    }

}
