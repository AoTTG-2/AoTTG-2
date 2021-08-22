using Newtonsoft.Json;
using System;
using UnityEngine;

namespace Assets.Scripts.Settings.New.Types
{
    [Serializable]
    [JsonConverter(typeof(StringSettingConverter))]
    public class StringSetting
    {
        [SerializeField] private string value;
        [SerializeField] private bool hasValue;

        public int? MaxLength { get; private set; }

        [JsonIgnore]
        public virtual string Value
        {
            get => value;
            set
            {
                if (value.Length > MaxLength) return; 
                this.value = value;
                hasValue = true;
                OnValueChanged?.Invoke(value);
            }
        }

        public bool HasValue => hasValue;

        /// <summary>
        /// Event is thrown if the <see cref="Value"/> has changed
        /// </summary>
        public event Action<string> OnValueChanged;

        public void Setup(int? maxLength)
        {
            MaxLength = maxLength;
        }

        #region Overrides
        protected bool Equals(StringSetting other)
        {
            return value == other.value;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((StringSetting) obj);
        }

        public override int GetHashCode()
        {
            return value != null ? value.GetHashCode() : 0;
        }

        public override string ToString()
        {
            return value;
        }

        public static bool operator ==(StringSetting stringSetting, string right)
        {
            return stringSetting?.Value == right;
        }

        public static bool operator !=(StringSetting left, string right)
        {
            return left?.Value != right;
        }
        #endregion
    }

    public class StringSettingConverter : JsonConverter<StringSetting>
    {
        public override void WriteJson(JsonWriter writer, StringSetting value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, value.HasValue ? value.Value : null);
        }

        public override StringSetting ReadJson(JsonReader reader, Type objectType, StringSetting existingValue, bool hasExistingValue,
            JsonSerializer serializer)
        {
            if (reader.Value is string value)
            {
                return new StringSetting
                {
                    Value = value
                };
            }
            return null;
        }
    }

}
