#define loadFromAssets

using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.Language
{
    class TranslationSeeker
    {
        List<string> possibleTranslations;

        public TranslationSeeker()
        {
            possibleTranslations = new List<string>();

#if UNITY_EDITOR
            string[] translations = AssetDatabase.FindAssets("l:translation", new string[] { "Languages" });
            foreach (var s in translations)
                Debug.Log(s);

            createTranslationListFile();
#elif loadFromAssets
            TextAsset[] availableTranslations = Resources.LoadAll<TextAsset>("Languages/");

#else
            //load from folder
#endif
        }

#if UNITY_EDITOR
        private void createTranslationListFile()
        {


        }
#endif

        public void LoadAvailableTranslations(out List<string> translations)
        {
            if (this.possibleTranslations == null)
                translations = new List<string>() { "english" };
            else
                translations = this.possibleTranslations;
        }
    }
}
