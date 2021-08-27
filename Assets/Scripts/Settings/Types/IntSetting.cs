using Newtonsoft.Json;
using System;
using UnityEngine;

namespace Assets.Scripts.Settings.Types
{
    [Serializable]
    [JsonConverter(typeof(IntSettingConverter))]
    public class IntSetting : ISetting
    {
        [SerializeField] protected int value;
        [SerializeField] protected bool hasValue;

        public int? MinValue { get; private set; }
        public int? MaxValue { get; private set; }
        public int? Default { get; private set; }

        [JsonIgnore]
        public virtual int Value
        {
            get => value;
            set
            {
                if (value < MinValue || value > MaxValue)
                {
                    if (!Default.HasValue) return;
                    this.value = Default.Value;
                    hasValue = true;
                    return;
                }
                this.value = value;
                hasValue = true;
                OnValueChanged?.Invoke(value);
            }
        }

        public bool HasValue => hasValue;

        /// <summary>
        /// Event is thrown if the <see cref="Value"/> has changed
        /// </summary>
        public event Action<int> OnValueChanged;

        public void Setup(int? minValue, int? maxValue, int? @default)
        {
            MinValue = minValue;
            MaxValue = maxValue;
            Default = @default;
        }
    }

    public class IntSettingConverter : JsonConverter<IntSetting>
    {
        public override void WriteJson(JsonWriter writer, IntSetting value, JsonSerializer serializer)
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

        public override IntSetting ReadJson(JsonReader reader, Type objectType, IntSetting existingValue, bool hasExistingValue,
            JsonSerializer serializer)
        {
            if (reader.Value is int value)
            {
                return new IntSetting
                {
                    Value = value
                };
            }

            if (reader.Value is long longValue)
            {
                return new IntSetting
                {
                    Value = (int)longValue
                };
            }
            return null;
        }
    }
}
