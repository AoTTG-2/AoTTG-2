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

        public async void ReloadLocalization()
        {
            Common = await commonTable.GetTable().Task;
            Gamemode.Shared = await sharedGamemodeTable.GetTable().Task;
            Gamemode.Catch = await catchGamemodeTable.GetTable().Task;
            Gamemode.Racing = await racingGamemodeTable.GetTable().Task;
            Gamemode.Rush = await rushGamemodeTable.GetTable().Task;
            Gamemode.Wave = await waveGamemodeTable.GetTable().Task;
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
