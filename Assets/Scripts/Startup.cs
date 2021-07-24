using System.Collections.Generic;
using UnityEngine;
using MonoBehaviour = UnityEngine.MonoBehaviour;

namespace Assets.Scripts
{
    /// <summary>
    /// Convenience component to enable a list of <see cref="StartupObjects"/> which should be disabled by default
    /// </summary>
    public class Startup : MonoBehaviour
    {
        public static bool HasLoaded;
        [Tooltip("Assure that the StartupObjects are all disabled in the Unity Editor!")]
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
