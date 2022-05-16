using System;
using UnityEngine;

namespace Assets.Scripts.Settings.New.Types
{
    [Serializable]
    public class BoolSetting
    {
        [SerializeField]
        private bool value;
        public bool Value
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
        public event Action<bool> OnValueChanged;

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
}
