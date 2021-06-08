using Assets.Scripts.Characters.Humans;
using Assets.Scripts.Inventory.Items.Data;

namespace Assets.Scripts.Inventory.Items
{
    public abstract class Item
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
