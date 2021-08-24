using Assets.Scripts.Room;
using Assets.Scripts.Settings.New.Game;
using Assets.Scripts.Settings.New.Game.Gamemodes;
using System.Collections.Generic;
using System.Linq;
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
        public static GameSettingsNew Game { get; private set; }
        public static GamemodeSetting GamemodeSetting { get; private set; }
        public static List<Level> Levels { get; private set; }
        //TODO: Game Settings (GameMode ect), Graphic Settings, UI Settings (UI customization)
        
        public DebugSettings DefaultDebug;
        public GamemodeSetting DefaultGamemodeSetting;
        public GameSettingsNew DefaultGameSetting;
        public List<Level> DefaultLevels;

#if UNITY_EDITOR
        [Header("Debugging")]
        [SerializeField] private DebugSettings CurrentDebugSettings;
        [SerializeField] private GamemodeSetting CurrentGamemodeSettings;
        [SerializeField] private GameSettingsNew CurrentGameSettings;
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
            GamemodeSetting = Instantiate(DefaultGamemodeSetting);
            Levels = DefaultLevels.Select(Instantiate).ToList();
            Game = DefaultGameSetting.Copy() as GameSettingsNew;

#if UNITY_EDITOR
            CurrentDebugSettings = Debug;
            CurrentGamemodeSettings = GamemodeSetting;
            CurrentGameSettings = Game;
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
