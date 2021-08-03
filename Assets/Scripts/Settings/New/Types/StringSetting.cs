using System;
using UnityEngine;

namespace Assets.Scripts.Settings.New.Types
{
    [Serializable]
    public class StringSetting
    {
        [SerializeField]
        private string value;
        public string Value
        {
            get => value;
            set
            {
                this.value = value;
                OnValueChanged?.Invoke(value);
            }
        }

        /// <summary>
        /// Event is thrown if the <see cref="Value"/> has changed
        /// </summary>
        public event Action<string> OnValueChanged;

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
}
