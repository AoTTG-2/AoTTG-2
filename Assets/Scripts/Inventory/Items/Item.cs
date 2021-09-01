using Assets.Scripts.Characters.Humans;
using Assets.Scripts.Inventory.Items.Data;
using UnityEngine;

namespace Assets.Scripts.Inventory.Items
{
    /// <summary>
    /// A base class for ingame items.
    /// </summary>
    public abstract class Item: MonoBehaviour
    {
        /// <summary>
        /// A reference to the original ScriptableObject
        /// </summary>
        public ItemData Data { get; }

        /// <summary>
        /// Use the Item
        /// </summary>
        public abstract void Use(Hero hero);

        protected Item(ItemData data)
        {
            Data = data;
        }
    }
}
