using UnityEngine;

namespace Assets.Scripts.CustomMaps
{
    /// <summary>
    /// A MapTexture is the equivalent of to Unity's Texture2D, but then used for Custom Maps
    /// </summary>
    [CreateAssetMenu(fileName = "Map Texture", menuName = "Custom Map/Texture")]
    public class MapTexture : ScriptableObject
    {
        /// <summary>
        /// The name of the MapTexture that is used within the Custom Map File. Changing this can cause compatibility issues, so a new migration tool needs to be created
        /// </summary>
        public string Name;
        /// <summary>
        /// Only used for MapTextures which did exist in AoTTG.
        /// </summary>
        public string LegacyName;
        public Vector2 LegacyTiling = new Vector2(1f, 1f);
        public string Description;
        public Texture2D Texture;
    }
}
