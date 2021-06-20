using UnityEngine;

namespace Assets.Scripts.CustomMaps
{
    [CreateAssetMenu(fileName = "Map Material", menuName = "Custom Map/Material")]
    public class MapMaterial : ScriptableObject
    {
        /// <summary>
        /// The name of the MapMaterial that is used within the Custom Map File. Changing this can cause compatibility issues, so a new migration tool needs to be created.
        /// </summary>
        public string Name;
        /// <summary>
        /// Only used for MapMaterials which did exist in AoTTG.
        /// </summary>
        public string LegacyName;
        public string Description;
        public Material Material;
    }
}