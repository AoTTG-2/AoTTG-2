#define loadFromAssets

using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Threading.Tasks;

namespace Assets.Scripts.Language
{
    class TranslationLoader
    {
        public TranslationLoader(string translation_path)
        {
#if loadFromAssets
            TextAsset translationFile = Resources.Load<TextAsset>("Languages\\" + translation_path);
            if(translationFile==null)
            {
                Debug.Log("Language file not found");
                this.Onfail.Invoke();
            }
            else
            {
                Task.Run(()=>ReadFile(translationFile));
            }
#else
#endif
        }

        private void ReadFile(TextAsset translation_file)
        {
            Dictionary<string, string> newTranslation = new Dictionary<string, string>();

            StringReader reader = new StringReader(translation_file.text);

            string key = null;
            string val = null;
            
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                if (line.StartsWith("msgid \""))
                {
                    key = line.Substring(7, line.Length - 8).ToUpper();
                }
                else if (line.StartsWith("msgstr \""))
                {
                    val = line.Substring(8, line.Length - 9);
                }
                else
                {
                    if (key != null && val != null)
                    {
                        newTranslation.Add(key, val);
                        key = val = null;
                    }
                }
            }

            reader.Close();

            OnDone.Invoke(newTranslation);
        }

        public Action<Dictionary<string,string>> OnDone;

        public Action Onfail;

    }
}
