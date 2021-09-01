using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Inventory.Items;

namespace Assets.Scripts.Inventory
{
    /// <summary>
    /// Represents a hero's inventory. Contains a list of <see cref="InventoryItem"/>s
    /// </summary>
    [System.Serializable]
    [CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory/Player Inventory")]
    public class PlayerInventory : ScriptableObject
    {
        public List<InventoryItem> myInventory = new List<InventoryItem>();
    }

}