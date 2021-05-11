using Assets.Scripts.Services;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

namespace Assets.Scripts.UI.Menu
{
    public class LanguageDropdown : MonoBehaviour
    {
        public TMP_Dropdown Dropdown;
        public List<Locale> Languages;

        private IEnumerator Start()
        {
            yield return LocalizationSettings.InitializationOperation;
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
