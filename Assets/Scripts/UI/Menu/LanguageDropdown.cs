using Assets.Scripts.Services;
using System.Linq;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Settings;

namespace Assets.Scripts.UI.Menu
{
    /// <summary>
    /// UI Dropdown class for Language selection
    /// </summary>
    public class LanguageDropdown : MonoBehaviour
    {
        public TMP_Dropdown Dropdown;
        private const string LanguagePlayerPref = "LANGUAGE";

        private async void Start()
        {
            await LocalizationSettings.InitializationOperation.Task;
            var languages = LocalizationSettings.AvailableLocales.Locales;
            Dropdown.options.Clear();
            Dropdown.AddOptions(languages.Select(x => x.Identifier.Code).ToList());

            var language = PlayerPrefs.GetString(LanguagePlayerPref);
            if (language != null)
            {
                LocalizationSettings.SelectedLocale =
                    LocalizationSettings.AvailableLocales.Locales.SingleOrDefault(x => x.Identifier.Code == language);
                Dropdown.value = languages.Select(x => x.Identifier.Code).ToList().IndexOf(language);
            }
            await Service.Localization.ReloadLocalization();
        }

        public void OnValueChanged(int value)
        {
            LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[value];
            PlayerPrefs.SetString(LanguagePlayerPref, LocalizationSettings.SelectedLocale.Identifier.Code);
            Task.Run(() => Service.Localization.ReloadLocalization());
        }
    }
}
