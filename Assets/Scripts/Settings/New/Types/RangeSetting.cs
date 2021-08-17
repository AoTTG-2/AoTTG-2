using System;
using UnityEngine;

namespace Assets.Scripts.Settings.New.Types
{
    [Serializable]
    public class RangeSetting<T> where T : struct, IConvertible
    {
        [SerializeField] private T minValue;
        [SerializeField] private T maxValue;

        public T MinValue
        {
            get => minValue;
            set
            {
                minValue = value;
                OnMinValueChanged?.Invoke(value);
            }
        }
        public T MaxValue
        {
            get => maxValue;
            set
            {
                maxValue = value;
                OnMaxValueChanged?.Invoke(value);
            }
        }

        /// <summary>
        /// Event is thrown if the <see cref="MinValue"/> has changed
        /// </summary>
        public event Action<T> OnMinValueChanged;
        /// <summary>
        /// Event is thrown if the <see cref="MaxValue"/> has changed
        /// </summary>
        public event Action<T> OnMaxValueChanged;
    }
}
