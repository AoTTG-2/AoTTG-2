using Assets.Scripts.Services;
using Assets.Scripts.Settings;
using Assets.Scripts.Settings.Game.Gamemodes;
using Assets.Scripts.UI;
using Assets.Scripts.UI.Input;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

        public UiHandler UIHandler;
        public UIInputHandler UIInputHandler;
        public IN_GAME_MAIN_CAMERA MainCamera;


        [Tooltip("Assure that the StartupObjects are all disabled in the Unity Editor!")]
        public List<GameObject> StartupObjects;
        
        [Header("Multiplayer Config (not yet implemented)")]
        public bool IsOffline;
        public string RoomName;
        public PhotonServerConfig Region;

        public GamemodeSetting SelectedGamemode;

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
            Setup(InputManager.gameObject); // Doesn't have any dependencies itself
            Setup(Settings.gameObject);
            Setup(Services.gameObject); // Relies on InputManager
            Setup(GameManager.gameObject);
            //MainCamera.gameObject.SetActive(true);
            //Setup(MainCamera.gameObject);
            Setup(UIHandler.gameObject);
            Setup(UIInputHandler.gameObject);

            var level = Settings.DefaultLevels.FirstOrDefault(x => x.SceneName == SceneManager.GetActiveScene().name);
            if (level == null)
            {
                Debug.LogError($"Startup: Level with SceneName {SceneManager.GetActiveScene().name} does not exist in Settings.Levels");
                yield break;
            }
            
            // Joins a lobby & creates a room
            Service.Photon.StatelessConnect(IsOffline, RoomName, level.Name, SelectedGamemode.Name);
            while (!Service.Photon.IsStatelesslyConnected())
            {
                yield return new WaitForSeconds(0.2f);
            }

            Service.Level.InvokeLevelLoaded(SceneManager.GetActiveScene().buildIndex, level);

            //
            Debug.Log("Startup successful");
        }
    }
}
