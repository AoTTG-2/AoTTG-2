using UnityEngine;
using Assets.Scripts.UI.Radial;
using UnityEngine.UI;

namespace Assets.Scripts.Inventory
{
    /// <summary>
    /// A scriptable object which holds the data for items which can be used within the inventory.
    /// </summary>
    [System.Serializable]
    [CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Items")]
    public class InventoryItem : ScriptableObject
    {
        public string itemName;
        public string itemDesc;
        public Image itemImage;
        public RadialMenu itemMenu;
        public Items.Item thisItem;
        [HideInInspector] public RadialElement thisElement;
    }

}
