using System.Collections.Generic;

namespace Assets.Scripts.Extensions
{
    public static class ListExtensions
    {
        public static T GetItemOrNull<T>(this IList<T> list, int index) where T : class
        {
            return list.Count > index && index >= 0 ? list[index] : null;
        }

        public static T GetItemOrFirst<T>(this IList<T> list, int index)
        {
            return list.Count > index && index >= 0 ? list[index] : list[0];
        }
    }
}
