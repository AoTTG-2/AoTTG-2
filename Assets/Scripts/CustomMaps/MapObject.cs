using UnityEngine;

namespace Assets.Scripts.CustomMaps
{
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
        public string Description;
        public GameObject Prefab;
    }
}
