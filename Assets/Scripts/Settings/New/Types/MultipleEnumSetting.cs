using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Assets.Scripts.Settings.New.Types
{
    [Serializable]
    [JsonConverter(typeof(MultipleEnumSettingConverter<>))]
    public class MultipleEnumSetting<T> : AbstractSetting<List<T>> where T : Enum
    {
        
    }

    public class MultipleEnumSettingConverter<T> : JsonConverter<MultipleEnumSetting<T>> where T : Enum
    {
        public override void WriteJson(JsonWriter writer, MultipleEnumSetting<T> value, JsonSerializer serializer)
        {
            if (value.HasValue)
            {
                serializer.Serialize(writer, value.Value);
            }
            else
            {
                serializer.Serialize(writer, null);
            }
        }

        public override MultipleEnumSetting<T> ReadJson(JsonReader reader, Type objectType, MultipleEnumSetting<T> existingValue, bool hasExistingValue,
            JsonSerializer serializer)
        {
            //TODO: List
            //if (reader.Value is bool value)
            //{
            //    return new BoolSetting
            //    {
            //        Value = value
            //    };
            //}
            return null;
        }
    }
}
