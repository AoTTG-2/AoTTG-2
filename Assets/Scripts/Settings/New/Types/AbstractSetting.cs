using Newtonsoft.Json;
using System;
using UnityEngine;

namespace Assets.Scripts.Settings.New.Types
{
    public abstract class AbstractSetting<T>
    {
        [SerializeField] protected T value;
        [SerializeField] protected bool hasValue;

        [JsonIgnore]
        public virtual T Value
        {
            get => value;
            set
            {
                this.value = value;
                
            }
        }

        public bool HasValue => hasValue;

        /// <summary>
        /// Event is thrown if the <see cref="Value"/> has changed
        /// </summary>
        public event Action<T> OnValueChanged;
    }
}
