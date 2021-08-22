using System;
using UnityEngine;

namespace Assets.Scripts.Settings.New.Types
{
    [Serializable]
    public class RangeSetting<T> where T : struct, 
        IComparable,
        IComparable<T>,
        IConvertible,
        IEquatable<T>,
        IFormattable, ISetting
    {
        [SerializeField] private T minValue;
        [SerializeField] private T maxValue;
        [SerializeField] protected bool hasValue;

        public T MinValue
        {
            get => minValue;
            set
            {
                if (MinValueLimit.HasValue && minValue.CompareTo(MinValueLimit.Value) < 0) return;
                minValue = value;
                OnMinValueChanged?.Invoke(value);
            }
        }
        public T MaxValue
        {
            get => maxValue;
            set
            {
                if (MaxValueLimit.HasValue && maxValue.CompareTo(MaxValueLimit.Value) > 0) return;
                maxValue = value;
                OnMaxValueChanged?.Invoke(value);
            }
        }

        public T? MinValueLimit { get; private set; }
        public T? MaxValueLimit { get; private set; }

        /// <summary>
        /// Event is thrown if the <see cref="MinValue"/> has changed
        /// </summary>
        public event Action<T> OnMinValueChanged;
        /// <summary>
        /// Event is thrown if the <see cref="MaxValue"/> has changed
        /// </summary>
        public event Action<T> OnMaxValueChanged;

        public void Setup(T minValueLimit, T maxValueLimit)
        {
            MinValueLimit = minValueLimit;
            MaxValueLimit = maxValueLimit;
        }
    }
}
