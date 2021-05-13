using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Tables;

namespace Assets.Scripts.Services
{
    public class Localization : MonoBehaviour
    {
        [SerializeField] private LocalizedStringTable commonTable;

        [Header("Gamemodes")] 
        [SerializeField] private LocalizedStringTable sharedGamemodeTable;
        [SerializeField] private LocalizedStringTable catchGamemodeTable;
        [SerializeField] private LocalizedStringTable racingGamemodeTable;
        [SerializeField] private LocalizedStringTable rushGamemodeTable;
        [SerializeField] private LocalizedStringTable waveGamemodeTable;

        public static StringTable Common { get; set; }
        public static GamemodeLocalization Gamemode { get; set; } = new GamemodeLocalization();

        private void Start()
        {
            commonTable.GetTable().Completed += (op) => { Common = op.Result; };
            sharedGamemodeTable.GetTable().Completed += (op) => { Gamemode.Shared = op.Result; };
            catchGamemodeTable.GetTable().Completed += (op) => { Gamemode.Catch = op.Result; };
            racingGamemodeTable.GetTable().Completed += (op) => { Gamemode.Racing = op.Result; };
            rushGamemodeTable.GetTable().Completed += (op) => { Gamemode.Rush = op.Result; };
            waveGamemodeTable.GetTable().Completed += (op) => { Gamemode.Wave = op.Result; };
        }

        public void ReloadLocalization()
        {
            Start();
        }
    }

    public class GamemodeLocalization
    {
        /// <summary>
        /// Translation keys which are shared between gamemodes
        /// </summary>
        public StringTable Shared { get; set; }
        /// <summary>
        /// Translations for the Catch Gamemode
        /// </summary>
        public StringTable Catch { get; set; }
        /// <summary>
        /// Translations for the Racing Gamemode
        /// </summary>
        public StringTable Racing { get; set; }
        /// <summary>
        /// Translations for the Racing Gamemode
        /// </summary>
        public StringTable Rush { get; set; }
        /// <summary>
        /// Translations for the Wave Gamemode
        /// </summary>
        public StringTable Wave { get; set; }
    }
}
