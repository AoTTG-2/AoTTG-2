using Assets.Scripts.Gamemode.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Assets.Scripts.Settings.New.Types
{
    [Serializable]
    [JsonConverter(typeof(MultipleEnumSettingConverter))]
    public class MultipleEnumSetting<T> : AbstractSetting<T[]>
    {

    }

    public class MultipleEnumSettingConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var valueType = value.GetType();
            var hasValue = valueType.GetField("hasValue", BindingFlags.Instance | BindingFlags.NonPublic)?.GetValue(value);
            var enumValue = valueType.GetField("value", BindingFlags.Instance | BindingFlags.NonPublic)?.GetValue(value);

            if (hasValue is bool boolValue && boolValue)
            {
                serializer.Serialize(writer, JsonConvert.SerializeObject(enumValue));
            }
            else
            {
                serializer.Serialize(writer, null);
            }
        }

        public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
        {
            if (!(reader.Value is string enumValues)) return null;
            //if (objectType != typeof(EnumSetting<>)) throw new ArgumentException("Type is not supported!", nameof(objectType));
            var enumType = objectType.GetTypeInfo().GenericTypeArguments[0];
            var bindingFlags = BindingFlags.Instance | BindingFlags.NonPublic;

            var enumSetting = typeof(MultipleEnumSetting<>);
            var typedEnumSetting = enumSetting.MakeGenericType(enumType);
            var setting = Activator.CreateInstance(typedEnumSetting);

            var stringEnums = JsonConvert.DeserializeObject<string[]>(enumValues);
            //var result = stringEnums.ToList().ConvertAll(x => Enum.Parse(enumType, x));
            //var type = result.GetType();
            var data = Array.CreateInstance(enumType, stringEnums.Length);
            for (var i = 0; i < stringEnums.Length; i++)
            {
                data.SetValue(Enum.Parse(enumType, stringEnums[i]), i);
            }

            var result = stringEnums.Select(x => Enum.Parse(enumType, x)).ToList();

            var valueProp = typedEnumSetting.GetField("value", bindingFlags);
            var hasValueProp = typedEnumSetting.GetField("hasValue", bindingFlags);

            valueProp.SetValue(setting, data);
            hasValueProp.SetValue(setting, true);
            return setting;
        }

        public override bool CanConvert(Type objectType)
        {
            var result = objectType == typeof(MultipleEnumSetting<>);
            return result;
        }
    }
}
