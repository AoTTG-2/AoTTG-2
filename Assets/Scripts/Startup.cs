using System.Collections.Generic;
using UnityEngine;
using MonoBehaviour = UnityEngine.MonoBehaviour;

namespace Assets.Scripts
{
    public class Startup : MonoBehaviour
    {
        public static bool HasLoaded;
        public List<GameObject> StartupObjects;

        private void Awake()
        {
            if (HasLoaded)
            {
                Destroy(gameObject);
                return;
            }
            HasLoaded = true;
            foreach (var startupObject in StartupObjects)
            {
                startupObject.transform.parent = null;
                startupObject.SetActive(true);
                DontDestroyOnLoad(startupObject);
            }
        }
    }
}
