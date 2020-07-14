using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Legacy.CustomMap
{

    [CreateAssetMenu(fileName = "RC Legacy Prefabs", menuName = "Legacy/ScriptableObjects/RCLegacyPrefab", order = 1)]
    public class RCLegacyPrefab : ScriptableObject
    {
        public List<GameObject> RCPrefabs;

        public GameObject Get(string name)
        {
            var obj = RCPrefabs.SingleOrDefault(x => x.name == name);
            return obj;
        }
    }
}
