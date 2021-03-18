using System.Collections.Generic;

namespace MapEditor
{
    public static class HashSetTools
    {
        //Converts the hash set into an array and returns it
        public static T[] ToArray<T>(this HashSet<T> hashSet)
        {
            T[] setArray = new T[hashSet.Count];
            int arrayIndex = 0;

            foreach (T element in hashSet)
                setArray[arrayIndex++] = element;

            return setArray;
        }

        //Excludes the second set from the first and stores the result in the given array
        public static T[] ExcludeToArray<T>(this HashSet<T> sourceSet, HashSet<T> excludeSet)
        {
            T[] resultArray = new T[sourceSet.Count - excludeSet.Count];
            int arrayIndex = 0;

            //Loop through the source set and add the elements not in the exclude set to the resultant array
            foreach (T element in sourceSet)
                if (!excludeSet.Contains(element))
                    resultArray[arrayIndex++] = element;

            return resultArray;
        }
    }
}