using UnityEngine;

namespace Assets.Scripts.UI.Radial
{
    [CreateAssetMenu(fileName = "Radial Item", menuName = "UI/Radial Item", order = 1)]
    public class RadialItem : ScriptableObject
    {
        public string Name;
        public UnityEngine.Sprite Icon;
        public Radial NextRing;
    }
}
