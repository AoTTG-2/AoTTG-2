using UnityEngine;
using Assets.Scripts.UI.Radial;

namespace Assets.Scripts.Inventory
{

    [System.Serializable]
    [CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Items")]
    public class InventoryItem : ScriptableObject
    {

        public string itemName;
        public string itemDesc;
        public UnityEngine.Sprite itemImage;
        public RadialMenu itemMenu;
        public Items.Item thisItem;
        public RadialElement thisElement;

    }

}
