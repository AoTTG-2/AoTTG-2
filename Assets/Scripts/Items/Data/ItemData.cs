using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Items.Data
{
    public abstract class ItemData : ScriptableObject
    {
        public string Name;
        public UnityEngine.Sprite Icon;
    }
}
