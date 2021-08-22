using Assets.Scripts.Services;
using Assets.Scripts.Settings;
using Assets.Scripts.Settings.New;
using Assets.Scripts.UI.Input;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using MonoBehaviour = UnityEngine.MonoBehaviour;

namespace Assets.Scripts
{
    /// <summary>
    /// Convenience component to enable a list of <see cref="StartupObjects"/> which should be disabled by default
    /// </summary>
    public class Startup : MonoBehaviour
    {
        public static bool HasLoaded;

        [Header("Dependencies")] 
        public InputManager InputManager;
        public Setting Settings;
        public Service Services;
        public FengGameManagerMKII GameManager;

        [Tooltip("Assure that the StartupObjects are all disabled in the Unity Editor!")]
        public List<GameObject> StartupObjects;
        
        [Header("Multiplayer Config (not yet implemented)")]
        public bool IsOffline;
        public string RoomName;
        public PhotonServerConfig Region;

        private void Awake()
        {
            if (HasLoaded)
            {
                Destroy(gameObject);
                return;
            }
            HasLoaded = true;

            var currentScene = SceneManager.GetActiveScene();
            if (currentScene.buildIndex == 0)
            {
                foreach (var startupObject in StartupObjects)
                {
                    startupObject.transform.parent = null;
                    startupObject.SetActive(true);
                    DontDestroyOnLoad(startupObject);
                }
            }
            else
            {
                StartCoroutine(TryLoadStatelessScene());
            }
        }

        private void Setup(GameObject data)
        {
            data.transform.parent = null;
            data.SetActive(true);
            DontDestroyOnLoad(data);
        }

        private IEnumerator TryLoadStatelessScene()
        {
            if (!IsOffline)
            {
                Debug.LogError("Multiplayer is not yet supported!");
                yield break;
            }

            Setup(InputManager.gameObject); // Doesn't have any dependencies itself
            Setup(Settings.gameObject);
            Setup(Services.gameObject); // Relies on InputManager
            Setup(GameManager.gameObject);

            // Joins a lobby & creates a room
            Service.Photon.StatelessLocalCreate();
            while (!Service.Photon.IsStatelesslyConnected())
            {
                yield return new WaitForSeconds(0.2f);
            }

            //
            Debug.Log("Startup successful");
        }
    }
}
