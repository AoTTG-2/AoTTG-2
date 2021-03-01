using UnityEngine;

public static class Layer
{
    /// <summary>
    /// Extension Method to retrieve the layer names from <see cref="Layers"/>
    /// </summary>
    public static string ToName(this Layers layer)
    {
        return LayerMask.LayerToName((int) layer);
    }
}

