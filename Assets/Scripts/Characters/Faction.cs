using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Characters
{
    /// <summary>
    /// Each entity has a faction, which is used to determine which other entities are friendly or hostile. Not being in a faction means that they are hostile to all.
    /// </summary>
    public class Faction
    {
        public string Name { get; set; }
        public string Prefix { get; set; }
        public Color Color { get; set; }
        public List<Faction> Allies { get; set; } = new List<Faction>();
    }
}
