using Assets.Scripts.Room;
using Assets.Scripts.Settings.Game;
using Assets.Scripts.Settings.Game.Gamemodes;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using MonoBehaviour = Photon.MonoBehaviour;

namespace Assets.Scripts.Settings
{
    /// <summary>
    /// A singleton class, containing all settings
    /// </summary>
    public class Setting : MonoBehaviour
    {
        protected static Setting Self;


        public static DebugSettings Debug { get; private set; }
        public static GameSettings Game { get; private set; }
        /// <summary>
        /// A reference to the current active Gamemode
        /// </summary>
        public static GamemodeSetting Gamemode { get; private set; }
        public static List<Level> Levels { get; private set; }
        //TODO: Game Settings (GameMode ect), Graphic Settings, UI Settings (UI customization)

        public DebugSettings DefaultDebug;
        public GamemodeSetting DefaultGamemodeSetting;
        public GameSettings DefaultGameSetting;
        public List<Level> DefaultLevels;
        public List<RuleSet> RuleSets;

#if UNITY_EDITOR
        [Header("Debugging")]
        [SerializeField] private DebugSettings CurrentDebugSettings;
        [SerializeField] private GamemodeSetting CurrentGamemodeSettings;
        [SerializeField] private GameSettings CurrentGameSettings;
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
            Levels = DefaultLevels.Select(Instantiate).ToList();
            Game = DefaultGameSetting.Copy() as GameSettings;

            RuleSets ??= new List<RuleSet>();
            Gamemode = Game.Setup<WaveGamemodeSetting>(RuleSets, Levels[0].Gamemodes[0] as WaveGamemodeSetting);

#if UNITY_EDITOR
            CurrentDebugSettings = Debug;
            CurrentGamemodeSettings = Gamemode;
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
