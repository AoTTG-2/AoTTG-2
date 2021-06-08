using UnityEngine;

namespace Assets.Scripts.Inventory.Items.Data
{
    public abstract class ItemData : ScriptableObject
    {
        public string Name;
        public UnityEngine.Sprite Icon;
    }
}
