using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.CustomMaps
{
    /// <summary>
    /// A ScriptableObject which contains references to all MapObjects, MapTextures, MapMaterials and MapComponents available for the Custom Maps functionality
    /// </summary>
    [CreateAssetMenu(fileName = "Custom Map Configuration", menuName = "Custom Map/Configuration")]
    public class CustomMapConfiguration : ScriptableObject
    {
        /// <summary>
        /// A list of all supported MapObjects
        /// </summary>
        public List<MapObject> MapObjects;
        /// <summary>
        /// A list of all supported MapTextures
        /// </summary>
        public List<MapTexture> MapTextures;
        /// <summary>
        /// A list of all supported MapMaterials
        /// </summary>
        public List<MapMaterial> MapMaterials;
        /// <summary>
        /// A list of all supported MapComponents
        /// </summary>
        public List<MapComponent> MapComponents;
    }
}
