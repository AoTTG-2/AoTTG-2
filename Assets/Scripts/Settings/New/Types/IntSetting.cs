using Newtonsoft.Json;
using System;
using UnityEngine;

namespace Assets.Scripts.Settings.New.Types
{
    [Serializable]
    public class IntSetting
    {
        [SerializeField]
        private int value;
        [JsonIgnore]
        public int Value
        {
            get => (int) value;
            set
            {
                this.value = value;
                OnValueChanged?.Invoke(value);
            }
        }

        /// <summary>
        /// Event is thrown if the <see cref="Value"/> has changed
        /// </summary>
        public event Action<int> OnValueChanged;
    }
}
