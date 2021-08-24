using Assets.Scripts.Gamemode.Options;
using Newtonsoft.Json;
using System;
using System.Reflection;

namespace Assets.Scripts.Settings.New.Types
{
    [Serializable]
    [JsonConverter(typeof(EnumSettingConverter))]
    public class EnumSetting<T> : AbstractSetting<T> where T : Enum
    {
        public static bool operator ==(EnumSetting<T> left, T right)
        {
            return left != null && Convert.ToInt32(left.Value) == Convert.ToInt32(right);
        }

        public static bool operator !=(EnumSetting<T> left, T right)
        {
            return left != null && Convert.ToInt32(left.Value) != Convert.ToInt32(right);
        }
    }

    public class EnumSettingConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var valueType = value.GetType();
            var hasValue = valueType.GetField("hasValue", BindingFlags.Instance | BindingFlags.NonPublic)?.GetValue(value);
            var enumValue = valueType.GetField("value", BindingFlags.Instance | BindingFlags.NonPublic)?.GetValue(value);

            if (hasValue is bool boolValue && boolValue && enumValue is Enum @enum)
            {
                serializer.Serialize(writer, @enum.ToString());
            }
            else
            {
                serializer.Serialize(writer, null);
            }
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (!(reader.Value is string valueString)) return null;
            //if (objectType != typeof(EnumSetting<>)) throw new ArgumentException("Type is not supported!", nameof(objectType));
            var enumType = objectType.GetTypeInfo().GenericTypeArguments[0];
            BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.NonPublic;
            var result = Enum.Parse(enumType, valueString);

            var enumSetting = typeof(EnumSetting<>);
            var typedEnumSetting = enumSetting.MakeGenericType(enumType);
            var setting = Activator.CreateInstance(typedEnumSetting);

            var valueProp = typedEnumSetting.GetField("value", bindingFlags);
            var hasValueProp = typedEnumSetting.GetField("hasValue", bindingFlags);

            valueProp.SetValue(setting, result);
            hasValueProp.SetValue(setting, true);
            return setting;
        }

        public override bool CanConvert(Type objectType)
        {
            var result = objectType == typeof(EnumSetting<>);
            return result;
        }
    }
}
