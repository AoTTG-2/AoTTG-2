using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Characters
{
    public class Faction
    {
        public string Name { get; set; }
        public string Prefix { get; set; }
        public Color Color { get; set; }
        public List<Faction> Allies { get; set; } = new List<Faction>();
    }
}
