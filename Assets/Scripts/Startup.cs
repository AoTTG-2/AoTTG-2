using Assets.Scripts.Services;
using Assets.Scripts.Settings;
using Assets.Scripts.Settings.Game;
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
        public GameObject Gamemode;


        [Tooltip("Assure that the StartupObjects are all disabled in the Unity Editor!")]
        public List<GameObject> StartupObjects;

        [Header("Settings")] 
        public GamemodeSetting CurrentGamemode;
        public List<RuleSet> CurrentRuleSets;

        [Header("Multiplayer Config")]
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
            var sceneName = SceneManager.GetActiveScene().name;
            var level = Settings.DefaultLevels.FirstOrDefault(x => x.SceneName == sceneName);
            if (level == null)
            {
                Debug.LogError($"Startup: Level with SceneName {SceneManager.GetActiveScene().name} does not exist in Settings.Levels!");
                yield break;
            }

            // Adds CurrentGamemode to a Level without modifying the actual ScriptableObject
            var index = Settings.DefaultLevels.FindIndex(x => x.SceneName == sceneName);
            var levelCopy = Instantiate(level);
            levelCopy.Gamemodes.Add(CurrentGamemode);
            Settings.DefaultLevels[index] = levelCopy;

            Settings.SetStatelessSettings(CurrentGamemode, CurrentRuleSets);
            Setup(Gamemode);
            Setup(InputManager.gameObject); // Doesn't have any dependencies itself
            Setup(Settings.gameObject);
            Setup(Services.gameObject); // Relies on InputManager
            Setup(GameManager.gameObject);
            //MainCamera.gameObject.SetActive(true);
            //Setup(MainCamera.gameObject);
            Setup(UIHandler.gameObject);
            Setup(UIInputHandler.gameObject);

            

            // Joins a lobby & creates a room
            Service.Photon.StatelessConnect(IsOffline, RoomName, level.Name, CurrentGamemode.Name, Region);
            while (!Service.Photon.IsStatelesslyConnected())
            {
                yield return new WaitForSeconds(0.2f);
            }

            //Service.Level.InvokeLevelLoaded(SceneManager.GetActiveScene().buildIndex, level);

            //
            Debug.Log("Startup successful");
        }
    }
}
