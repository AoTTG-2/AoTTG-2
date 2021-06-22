using System.Collections.Generic;
using System.Linq;

namespace Assets.Scripts.Extensions
{
    public static class StringExtensions
    {
        public static string GetCustomMapAttribute(this IEnumerable<string> data, string attribute)
        {
            return data.SingleOrDefault(x => x.StartsWith($"{attribute}:"))?.Split(':')[1];
        }

        /// <summary>
        /// Trims a string at the start & end, AND it will remove all whitespaces.
        /// </summary>
        /// <param name="string"></param>
        /// <returns></returns>
        public static string TrimAll(this string @string)
        {
            return @string.Trim()
                .Replace(" ", string.Empty)
                .Replace("\r", string.Empty)
                .Replace("\n", string.Empty);
        }
    }
}
