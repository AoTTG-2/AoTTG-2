using Newtonsoft.Json;
using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Assets.Scripts.Settings.New.Types
{
    [Serializable]
    [JsonConverter(typeof(FloatSettingConverter))]
    public class FloatSetting : ISetting
    {
        [SerializeField] protected float value;
        [SerializeField] protected bool hasValue;

        public float? MinValue { get; private set; }
        public float? MaxValue { get; private set; }
        public float? Default { get; private set; }

        [JsonIgnore]
        public virtual float Value
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
        public event Action<float> OnValueChanged;

        public void Setup(float? minValue, float? maxValue, float? @default)
        {
            MinValue = minValue;
            MaxValue = maxValue;
            Default = @default;
        }
    }

    public class FloatSettingConverter : JsonConverter<FloatSetting>
    {
        public override void WriteJson(JsonWriter writer, FloatSetting value, JsonSerializer serializer)
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

        public override FloatSetting ReadJson(JsonReader reader, Type objectType, FloatSetting existingValue, bool hasExistingValue,
            JsonSerializer serializer)
        {
            if (reader.Value is float value)
            {
                return new FloatSetting
                {
                    Value = value
                };
            }

            if (reader.Value is decimal longValue)
            {
                return new FloatSetting
                {
                    Value = (float) longValue
                };
            }
            return null;
        }
    }
}
