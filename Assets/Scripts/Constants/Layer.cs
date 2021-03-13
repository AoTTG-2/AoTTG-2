using UnityEngine;

namespace Assets.Scripts.Constants
{
    public static class Layer
    {
        /// <summary>
        /// Extension Method to retrieve the layer names from <see cref="Layers"/>
        /// </summary>
        public static string ToName(this Layers layer)
        {
            return LayerMask.LayerToName((int) layer);
        }

        /// <summary>
        /// Convert Layers Enum to LayerMask
        /// </summary>
        /// <param name="layer"></param>
        /// <returns></returns>
        public static LayerMask ToLayer(this Layers layer)
        {
            return 1 << (int)layer;
        }
    }
}

