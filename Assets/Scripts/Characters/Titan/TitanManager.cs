using System;
using System.Collections.Generic;
using System.Linq;

namespace Assets.Scripts.Characters.Titan
{
    public static class TitanManager
    {
        public static List<TitanBase> Titans { get; set; } = new List<TitanBase>();

        public static void Add(TitanBase titan)
        {
            Titans.Add(titan);
        }
        
        public static int Count<T>() where T : TitanBase
        {
            return Titans.Count(x => x.GetType() == typeof(T));
        }

        public static int Count<T>(Func<TitanBase, bool> func) where T : TitanBase
        {
            return Titans.Count(x => x.GetType() == typeof(T) && func.Invoke(x));
        }

        public static void Remove(TitanBase titan)
        {
            Titans.Remove(titan);
        }
    }
}
