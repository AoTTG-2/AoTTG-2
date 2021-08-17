using Newtonsoft.Json;
using System;

namespace Assets.Scripts.Settings.New.Types
{
    [Serializable]
    [JsonConverter(typeof(BoolSettingConverter))]
    public class BoolSetting : AbstractSetting<bool>
    {
        #region Overrides
        protected bool Equals(BoolSetting other)
        {
            return value == other.value;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((BoolSetting) obj);
        }

        public override int GetHashCode()
        {
            return value != null ? value.GetHashCode() : 0;
        }

        public override string ToString()
        {
            return value.ToString();
        }

        public static bool operator ==(BoolSetting left, bool right)
        {
            return left?.Value == right;
        }

        public static bool operator !=(BoolSetting left, bool right)
        {
            return left?.Value != right;
        }
        #endregion
    }

    public class BoolSettingConverter : JsonConverter<BoolSetting>
    {
        public override void WriteJson(JsonWriter writer, BoolSetting value, JsonSerializer serializer)
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

        public override BoolSetting ReadJson(JsonReader reader, Type objectType, BoolSetting existingValue, bool hasExistingValue,
            JsonSerializer serializer)
        {
            if (reader.Value is bool value)
            {
                return new BoolSetting
                {
                    Value = value
                };
            }
            return null;
        }
    }
}
