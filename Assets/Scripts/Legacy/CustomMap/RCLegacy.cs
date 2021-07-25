using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Legacy.CustomMap
{
    /// <summary>
    /// A scriptable object which contains a list of all AoTTG RC 2015 prefabs
    /// </summary>
    [CreateAssetMenu(fileName = "RC Legacy Prefabs", menuName = "Legacy/ScriptableObjects/RCLegacyPrefab", order = 1)]
    public class RCLegacy : ScriptableObject
    {
        public List<GameObject> RCPrefabs;
        public List<Material> RcMaterials;

        public GameObject GetPrefab(string name)
        {
            try
            {
                return RCPrefabs.Single(x => string.Equals(x.name, name, StringComparison.InvariantCultureIgnoreCase));
            }
            catch
            {
                Debug.LogError("Could not find: " + name);
                return null;
            }
        }

        public Material GetMaterial(string name)
        {
            return RcMaterials.SingleOrDefault(x => string.Equals(x.name, name, StringComparison.InvariantCultureIgnoreCase))
                           ?? RcMaterials.Single(x => string.Equals(x.name, "empty", StringComparison.InvariantCultureIgnoreCase));
        }
    }
}
