using System;
using UnityEngine;

public class CheckBoxShowSSInGame : MonoBehaviour
{
    private bool init;

    private void OnActivate(bool yes)
    {
        if (this.init)
        {
            if (yes)
            {
                PlayerPrefs.SetInt("showSSInGame", 1);
            }
            else
            {
                PlayerPrefs.SetInt("showSSInGame", 0);
            }
        }
    }

    private void Start()
    {
        this.init = true;
        if (PlayerPrefs.HasKey("showSSInGame"))
        {
            if (PlayerPrefs.GetInt("showSSInGame") == 1)
            {
                base.GetComponent<UICheckbox>().isChecked = true;
            }
            else
            {
                base.GetComponent<UICheckbox>().isChecked = false;
            }
        }
        else
        {
            base.GetComponent<UICheckbox>().isChecked = true;
            PlayerPrefs.SetInt("showSSInGame", 1);
        }
    }
}

