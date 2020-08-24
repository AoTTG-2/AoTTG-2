using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Characters
{
    public static class FactionManager
    {
        private static readonly Faction Humanity = new Faction();
        private static readonly Faction Titanity = new Faction();

        public static List<Faction> Factions = new List<Faction>
        {
            Humanity, Titanity
        };

        public static Faction GetHumanity() => Humanity;
        public static Faction GetTitanity() => Titanity;

        public static Faction SetHumanity(GameObject gameObject)
        {
            Humanity.Members.Add(gameObject);
            return Humanity;
        }

        public static Faction SetTitanity(GameObject gameObject)
        {
            Titanity.Members.Add(gameObject);
            return Titanity;
        }
    }
}
