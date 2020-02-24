using System;
using UnityEngine;

public class LanguageChangeListener : MonoBehaviour
{
    private void OnSelectionChange()
    {
        Language.type = Language.GetLangIndex(base.GetComponent<UIPopupList>().selection);
        PlayerPrefs.SetInt("language", Language.type);
    }

    private void Start()
    {
        if (Language.type == -1)
        {
            if (PlayerPrefs.HasKey("language"))
            {
                Language.type = PlayerPrefs.GetInt("language");
            }
            else
            {
                PlayerPrefs.SetInt("language", 0);
                Language.type = 0;
            }
            Language.init();
            base.GetComponent<UIPopupList>().selection = Language.GetLang(Language.type);
        }
        else
        {
            base.GetComponent<UIPopupList>().selection = Language.GetLang(Language.type);
        }
    }
}

