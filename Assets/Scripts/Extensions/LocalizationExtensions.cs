using UnityEngine.Localization.Tables;

namespace Assets.Scripts.Extensions
{
    public static class LocalizationExtensions
    {

        /// <summary>
        /// A more convenient and shorter way to get the localized strings
        /// </summary>
        /// <param name="table"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetLocalizedString(this StringTable table, string key)
        {
            return table.GetEntry(key).GetLocalizedString();
        }

        /// <summary>
        /// A more convenient and shorter way to get the localized strings
        /// </summary>
        /// <param name="table"></param>
        /// <param name="key"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static string GetLocalizedString(this StringTable table, string key, params object[] args)
        {
            return table.GetEntry(key).GetLocalizedString(args);
        }
    }
}
