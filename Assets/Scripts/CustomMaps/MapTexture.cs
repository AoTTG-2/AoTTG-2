using UnityEngine;

namespace Assets.Scripts.CustomMaps
{
    [CreateAssetMenu(fileName = "Map Texture", menuName = "Custom Map/Texture")]
    public class MapTexture : ScriptableObject
    {
        /// <summary>
        /// The name of the MapTexture that is used within the Custom Map File. Changing this can cause compatibility issues, so a new migration tool needs to be created.
        /// </summary>
        public string Name;
        /// <summary>
        /// Only used for MapTextures which did exist in AoTTG.
        /// </summary>
        public string LegacyName;
        public string Description;
        public Material Material;
    }
}
