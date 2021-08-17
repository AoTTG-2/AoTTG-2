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

    }

    public class EnumSettingConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        {
            if (value is EnumSetting<TeamMode> {HasValue: true} team)
            {
                serializer.Serialize(writer, team.Value.ToString());
            }
            else
            {
                serializer.Serialize(writer, null);
            }
        }

        public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
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



            //if (objectType == typeof(EnumSetting<TeamMode>) && Enum.TryParse(valueString, out TeamMode value))
            //{
            //    return new EnumSetting<TeamMode>
            //    {
            //        Value = value
            //    };
            //}

            return setting;
        }

        public override bool CanConvert(Type objectType)
        {
            var result = objectType == typeof(EnumSetting<>);
            return result;
        }
    }
}
