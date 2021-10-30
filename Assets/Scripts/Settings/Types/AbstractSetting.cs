﻿using Newtonsoft.Json;
using System;
using UnityEngine;

namespace Assets.Scripts.Settings.Types
{
    public abstract class AbstractSetting<T> : ISetting
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
                OnValueChanged?.Invoke(value);
            }
        }

        public bool HasValue => hasValue;

        /// <summary>
        /// Event is thrown if the <see cref="Value"/> has changed
        /// </summary>
        public event Action<T> OnValueChanged;
    }
}