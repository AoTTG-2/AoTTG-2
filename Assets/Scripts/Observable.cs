using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Observable<T>
{
    [SerializeField]
    private T value;

    public Observable(T value = default(T))
    {
        this.value = value;
    }

    public delegate void ValueChangedHandler(T newValue);

    public event ValueChangedHandler ValueChanged;

    public T Value
    {
        get
        {
            return value;
        }

        set
        {
            var valueChanged = !EqualityComparer<T>.Default.Equals(this.value, value);
            this.value = value;
            if (valueChanged)
                ValueChanged?.Invoke(this.value);
        }
    }

    public static implicit operator T(Observable<T> observable) =>
        observable.value;
}