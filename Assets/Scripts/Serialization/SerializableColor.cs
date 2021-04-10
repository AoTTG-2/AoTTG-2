using System;
using Newtonsoft.Json;
using UnityEngine;

namespace Assets.Scripts.Serialization
{
    [Serializable]
    public struct SerializableColor
    {
        [JsonProperty("r")]
        public float Red;
        [JsonProperty("g")]
        public float Green;
        [JsonProperty("b")]
        public float Blue;
        [JsonProperty("a")]
        public float Alpha;

        public SerializableColor(Color color)
        {
            Red = color.r;
            Green = color.g;
            Blue = color.b;
            Alpha = color.a;
        }


        public Color ToColor()
        {
            return new Color(Red, Green, Blue, Alpha);
        }
    }
}
