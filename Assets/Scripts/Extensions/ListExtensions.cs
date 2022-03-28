using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Extensions
{
    public static class ListExtensions
    {
        /// <summary>
        /// Retrieves an item from a list, but if the index does not exist, it will return null instead of an IndexOutOfRangeException
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static T GetItemOrNull<T>(this IList<T> list, int index) where T : class
        {
            return list.Count > index && index >= 0 ? list[index] : null;
        }

        /// <summary>
        /// Retrieves an item from a list, but if the index does not exist, it will return the first item
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static T GetItemOrFirst<T>(this IList<T> list, int index)
        {
            return list.Count > index && index >= 0 ? list[index] : list[0];
        }

        /// <summary>
        /// Retrieves a <see cref="Random.Range(float, float)"/> from the <paramref name="list"/>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static T GetRandom<T>(this IList<T> list) where T : class
        {
            var length = list.Count;
            return length == 0 ? null : list[Random.Range(0, length)];
        }
    }
}
