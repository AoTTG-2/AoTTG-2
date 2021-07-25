using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using UnityEngine;

namespace Assets.Scripts.Serialization
{
    /// <summary>
    /// Converts a Color to a <see cref="SerializableColor"/>
    /// </summary>
    public class ColorJsonConverter : JsonConverter<Color>
    {
        public override void WriteJson(JsonWriter writer, Color value, JsonSerializer serializer)
        {
            var color = new SerializableColor(value);
            var json = JsonConvert.SerializeObject(color);
            writer.WriteValue(json);
        }

        public override Color ReadJson(JsonReader reader, Type objectType, Color existingValue, bool hasExistingValue,
            JsonSerializer serializer)
        {
            try
            {
                if (reader.TokenType == JsonToken.Null)
                    return new Color();

                var color = JObject.Parse(reader.Value.ToString()).ToObject<SerializableColor>();
                return color.ToColor();
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                return Color.clear;
            }

        }
    }
}
