using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Tables;

namespace Assets.Scripts.Services
{
    /// <summary>
    /// A class which can give convenient static access to localization tables
    /// </summary>
    public class Localization : MonoBehaviour
    {
        [SerializeField] private LocalizedStringTable commonTable;
        [SerializeField] private LocalizedStringTable settingTable;

        [Header("Gamemodes")] 
        [SerializeField] private LocalizedStringTable sharedGamemodeTable;
        [SerializeField] private LocalizedStringTable catchGamemodeTable;
        [SerializeField] private LocalizedStringTable racingGamemodeTable;
        [SerializeField] private LocalizedStringTable rushGamemodeTable;
        [SerializeField] private LocalizedStringTable waveGamemodeTable;

        public static StringTable Common { get; set; }
        public static StringTable Setting { get; set; }
        public static GamemodeLocalization Gamemode { get; set; } = new GamemodeLocalization();

        public static StringTable GetStringTable(LocalizationEnum localizationEnum)
        {
            return localizationEnum switch
            {
                LocalizationEnum.Common => Common,
                LocalizationEnum.Setting => Setting,
                LocalizationEnum.Shared => Gamemode.Shared,
                LocalizationEnum.Catch => Gamemode.Catch,
                LocalizationEnum.Racing => Gamemode.Racing,
                LocalizationEnum.Wave => Gamemode.Wave,
                _ => Common
            };
        }

        private async void Awake()
        {
            await ReloadLocalization();
        }

        public async Task ReloadLocalization()
        {
            Common = await commonTable.GetTable().Task;
            Setting = await settingTable.GetTable().Task;
            Gamemode.Shared = await sharedGamemodeTable.GetTable().Task;
            Gamemode.Catch = await catchGamemodeTable.GetTable().Task;
            Gamemode.Racing = await racingGamemodeTable.GetTable().Task;
            Gamemode.Rush = await rushGamemodeTable.GetTable().Task;
            Gamemode.Wave = await waveGamemodeTable.GetTable().Task;
        }
    }

    public enum LocalizationEnum
    {
        Common,
        Setting,
        /// <summary>
        /// Shared gamemode localizations
        /// </summary>
        Shared,
        Catch,
        Racing,
        Wave
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
