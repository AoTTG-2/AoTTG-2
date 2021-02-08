using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Extensions
{
    public static class GenericExtensions
    {
        public static T GetValue<T>(this object[] array, int index)
        {
            return array.GetValue<object, T>(index);
        }
        public static T GetValue<A, T>(this A[] array, int index)
        {
            object o = array[index];

            return (T) o;
        }
    }
}