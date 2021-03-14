using UnityEngine;

namespace Assets.Scripts.UI.Radial
{
    [CreateAssetMenu(fileName = "Radial", menuName = "UI/Radial", order = 1)]
    public class Radial : ScriptableObject
    {
        public RadialItem[] Items;
    }
}
