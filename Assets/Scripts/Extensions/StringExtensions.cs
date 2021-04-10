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
    }
}
