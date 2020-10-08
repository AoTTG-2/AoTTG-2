#define loadLanguageSettingFromPlayerPref

using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Language;

namespace Assets.Scripts
{
    public static class Internationalization
    {
        private static bool _isLoadingTranslation;

        private static Dictionary<string, string> _translation;

        private static List<string> _availableTranslationsFiles;

        /// <summary>
        /// return the collection of the available translation.
        /// </summary>
        /// <returns></returns>
        public static string[] Translations()
        {
            if (_availableTranslationsFiles == null)
            {
                Debug.Log("Translations not initialized, please use Internationalization.SetLanguage() to initialize and load the language system");
                return new string[] { "english" };
            }
            else
            {
                return (_availableTranslationsFiles.Select(s => s.Substring(0, s.IndexOf('.')))).ToArray();
            }
        }

        private static string _currentLanguage;

        public static Action OnTranslationSet;

        private static void Initialize()
        {
            _translation = new Dictionary<string, string>();
            _currentLanguage = string.Empty;
            _isLoadingTranslation = true;

            var seekTranslation = new TranslationSeeker();
            seekTranslation.OnDone += () =>
            {
                seekTranslation.LoadAvailableTranslations(out _availableTranslationsFiles);
                SetLanguage();
            };
        }

        private static void SetLanguage(string newLanguage = "")
        {
#if loadLanguageSettingFromPlayerPref
            if (string.IsNullOrEmpty(newLanguage))
                newLanguage = PlayerPrefs.GetString("Language", "english");

            if (_currentLanguage.ToLower() == newLanguage.ToLower())
                return;

            string file_to_load = string.Empty;

            foreach (var file in _availableTranslationsFiles)
            {
                if (file.ToLower().Contains(newLanguage.ToLower()))
                {
                    file_to_load = file;
                    break;
                }
            }

            if (string.IsNullOrEmpty(file_to_load))
            {
                Debug.LogError("the setted language \"" + newLanguage + "\" is missing, has been removed or is corrupted");
                file_to_load = "english";
            }
#else
            newLanguage = "english";
            //idk, load the setting from idk where :'D
#endif

            _isLoadingTranslation = true;
            var translationLoader = new TranslationLoader(file_to_load);
            translationLoader.OnDone += (newTranslation) =>
            {
                _currentLanguage = newLanguage;
                _translation = newTranslation;
                _isLoadingTranslation = false;
                OnTranslationSet.Invoke();
            };
            translationLoader.Onfail += () =>
            {
                Debug.Log("translationLoader Error, language not changed");
                _isLoadingTranslation = false;
            };
        }

        /// <summary>
        /// The function that has to be called whenever you are trying to change the language
        /// </summary>
        /// <param name="newLanguage"></param>
        public static void SetLangauge(string newLanguage = "")
        {
            if (_isLoadingTranslation)
            {
                Debug.Log("Is already seeking a translation, wait for the previous order to be done");
                return;
            }
            if (_availableTranslationsFiles == null)
                Initialize();
            else
                SetLanguage(newLanguage);
        }

        /// <summary>
        /// Return the text translated, you can use it by simply adding to any string .Translated()
        /// Example:
        ///     "my text".Translated();
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string Translated(this string key)
        {
            if (_translation?.ContainsKey(key) ?? false)
                return _translation[key];
            else if (_isLoadingTranslation || _currentLanguage == "english")
                return key;
            else
            {
                if (_availableTranslationsFiles == null)
                {
                    Debug.LogWarning("translation not initialized");
                    Internationalization.SetLangauge();
                }
                else
                    Debug.LogWarning("translation not available for ");
                return key;
            }
        }
    }
}