using UnityEngine.Localization.Tables;
using Debug = UnityEngine.Debug;

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
            if (table == null)
            {
                Debug.LogWarning("StringTable is not defined.");
                return key;
            }
            var entry = table.GetEntry(key);
            return entry == null ? key : entry.GetLocalizedString();
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
            if (table == null) return key;
            var entry = table.GetEntry(key);
            return entry == null ? key : entry.GetLocalizedString(args);
        }
    }
}
