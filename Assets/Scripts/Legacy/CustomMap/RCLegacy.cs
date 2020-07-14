using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Legacy.CustomMap
{

    [CreateAssetMenu(fileName = "RC Legacy Prefabs", menuName = "Legacy/ScriptableObjects/RCLegacyPrefab", order = 1)]
    public class RCLegacy : ScriptableObject
    {
        public List<GameObject> RCPrefabs;
        public List<Material> RcMaterials;

        public GameObject GetPrefab(string name)
        {
            try
            {
                return RCPrefabs.Single(x => x.name == name);
            }
            catch
            {
                Debug.LogError("Could not find: " + name);
                return null;
            }
        }

        public Material GetMaterial(string name)
        {
            try
            {
                return RcMaterials.Single(x => x.name == name);
            }
            catch
            {
                Debug.LogError("Could not find: " + name);
                return null;
            }
        }
    }
}
