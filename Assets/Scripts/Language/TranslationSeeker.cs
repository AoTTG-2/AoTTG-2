using System;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.Language
{
    class TranslationSeeker
    {
        public const string Folder_path_add = "\\Language";

        List<string> possibleTranslations;

        public Action OnDone;

        public Action OnError;

        public TranslationSeeker()
        {
            possibleTranslations = new List<string>();
            Task.Run(() => SeekTranslations());
        }

        private void SeekTranslations()
        {
            try
            {
                possibleTranslations.Clear();

                var possible_translation_file = Directory.GetFiles(Application.streamingAssetsPath+Folder_path_add);

                foreach (var file_found in possible_translation_file)
                {
                    if (file_found.EndsWith(".po"))
                        possibleTranslations.Add(file_found);
                }
                this.OnDone.Invoke();
            }
            catch
            {
                this.OnError.Invoke();
            }
        }

        public void LoadAvailableTranslations(out List<string> translations)
        {
            if (this.possibleTranslations == null)
                translations = new List<string>() { "english" };
            else
                translations = this.possibleTranslations;
        }

    }
}
