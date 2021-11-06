using System;

namespace Assets.Scripts.Extensions
{
    public static class ComparableExtensions
    {
        /// <summary>
        /// return if the value is between the min and max (min<val<max) doesn't include boundaries
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="val"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static bool Between<T>(this T val, T min, T max) where T : IComparable
        {
            return (val.CompareTo(max) + min.CompareTo(val)) < 0;
        }
    }
}
