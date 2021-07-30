using UnityEngine;

namespace Assets.Scripts.CustomMaps
{
    /// <summary>
    /// A MapMaterial is the equivalent of to Unity's Prefabs, but then used for Custom Maps
    /// </summary>
    [CreateAssetMenu(fileName = "Map Object", menuName = "Custom Map/Map Object")]
    public class MapObject : ScriptableObject
    {
        /// <summary>
        /// The name of the MapObject that is used within the Custom Map File. Changing this can cause compatibility issues, so a new migration tool needs to be created.
        /// </summary>
        public string Name;
        /// <summary>
        /// Only used for MapObjects which did exist in AoTTG.
        /// </summary>
        public string LegacyName;
        /// <summary>
        /// Used for MapObjects which did have a different material in AoTTG.
        /// </summary>
        public MapMaterial LegacyMaterial;
        public string Description;
        public GameObject Prefab;
    }
}
