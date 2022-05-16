using UnityEngine;
using MonoBehaviour = Photon.MonoBehaviour;

namespace Assets.Scripts.Settings.New
{
    /// <summary>
    /// A singleton class, containing all settings
    /// </summary>
    public class Setting : MonoBehaviour
    {
        protected static Setting Self;


        public static DebugSettings Debug { get; private set; }
        //TODO: Game Settings (GameMode ect), Graphic Settings, UI Settings (UI customization)
        
        public DebugSettings DefaultDebug;

#if UNITY_EDITOR
        [Header("Debugging")]
        [SerializeField] private DebugSettings CurrentDebugSettings;
#endif

        private void Awake()
        {
            if (Self != null) return;
            Self = this;

            if (DefaultDebug == null)
            {
                UnityEngine.Debug.LogError("No Debug Settings are set!");
                Quit();
            }
            Debug = Instantiate(DefaultDebug);

#if UNITY_EDITOR
            CurrentDebugSettings = Debug;
#endif

            static void Quit()
            {
#if UNITY_EDITOR

                UnityEditor.EditorApplication.isPlaying = false;
#else
                Application.Quit();
#endif
            }
        }
    }
}
