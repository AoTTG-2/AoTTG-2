using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.CustomMaps
{
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

        ////TODO: Materials
        //public List<Material> MapMaterials;
    }
}
