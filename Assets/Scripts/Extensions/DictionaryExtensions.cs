using System.Collections.Generic;

namespace Assets.Scripts.Extensions
{
    public static class DictionaryExtensions
    {
        /// <summary>
        /// Return the value of the dictionary with key, if the value isn't present it return a new instance of
        /// the type of the object requested
        /// </summary>
        /// <typeparam name="T">object returned type</typeparam>
        /// <typeparam name="T1">IDictionary key type</typeparam>
        /// <typeparam name="T2">IDictionary value type</typeparam>
        /// <param name="h">Idictionary from which trying to get the value</param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static T SafeGet<T, T1, T2>(this IDictionary<T1, T2> h, T1 key) where T : new()
        {
            if (!h.ContainsKey(key))
                return new T();
            object o = h[key];
            return (o is T ? (T) o : new T());
        }

        /// <summary>
        /// Return the value of the dictionary with key, if the value isn't present it return the object 
        /// passed as default value
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <param name="h"></param>
        /// <param name="key"></param>
        /// <param name="defaultValue">object returned in case the key is not present or the value object can't
        /// be casted into the type T</param>
        /// <returns></returns>
        public static T SafeGet<T, T1, T2>(this IDictionary<T1, T2> h, T1 key, T defaultValue)
        {
            if (!h.ContainsKey(key))
                return defaultValue;
            object o = h[key];
            return (o is T ? (T) o : defaultValue);
        }
    }
}
