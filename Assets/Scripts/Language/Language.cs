#define loadLanguageSettingFromPlayerPref

using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Language;

public static class Language
{
    private static bool _isLoadingTranslation;

    private static Dictionary<string, string> _translation;

    private static List<string> _availableTranslations;

    private static string _currentLanguage;

    private static void Initialize()
    {
        var seekTranslation = new TranslationSeeker();
        seekTranslation.LoadAvailableTranslations(out _availableTranslations);

        _translation = new Dictionary<string, string>();
        _currentLanguage = string.Empty;
        _isLoadingTranslation = true;
    }

    public static void SetLangauge(string newLanguage = "")
    {
        if (_availableTranslations == null)
            Initialize();
        
        if (string.IsNullOrEmpty(newLanguage))

#if loadLanguageSettingFromPlayerPref
            newLanguage = PlayerPrefs.GetString("Language", "english");
        if (!_availableTranslations.Contains(newLanguage))
        {
            Debug.LogError("the setted language \"" + newLanguage + "\" is missing, has been removed or is corrupted");
            newLanguage = "english";
        }
#else
            newLanguage = "english";
            //idk, load the setting from idk where :'D
#endif

        _isLoadingTranslation = true;
        var translationLoader = new TranslationLoader(newLanguage);
        translationLoader.OnDone += (newTranslation) => {
            _translation = newTranslation;
            _isLoadingTranslation = false;
        };
    }

    public static string translated(this string key)
    {
        if (_translation?.ContainsKey(key) ?? false)
            return _translation[key];
        else if (_isLoadingTranslation)
            return key;
        else
        {
            if (_availableTranslations == null)
            {
                Debug.LogWarning("translation not initialized");
                Language.SetLangauge();
            }
            else
                Debug.LogWarning("translation not available for ");
            return key;
        }
    }

}
