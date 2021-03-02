using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.UI.Input
{
    public class InputEventDictionary<TKey, TValue> : Dictionary<TKey, TValue> where TKey : Enum
    {
        public Dictionary<TKey, TValue> Items = new Dictionary<TKey, TValue>();
        public InputEventDictionary() : base() { }
        InputEventDictionary(int capacity) : base(capacity) { }

        public event EventHandler ValueChanged;
    }
}