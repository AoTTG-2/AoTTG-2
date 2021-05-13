using Assets.Scripts.Services;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Settings;

namespace Assets.Scripts.UI.Menu
{
    public class LanguageDropdown : MonoBehaviour
    {
        public TMP_Dropdown Dropdown;

        private async void Start()
        {
            await LocalizationSettings.InitializationOperation.Task;
            var languages = LocalizationSettings.AvailableLocales.Locales;
            Dropdown.AddOptions(languages.Select(x => x.Identifier.CultureInfo.Name).ToList());
        }

        public void OnValueChanged(int value)
        {
            LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[value];
            Service.Localization.ReloadLocalization();
        }
    }
}
