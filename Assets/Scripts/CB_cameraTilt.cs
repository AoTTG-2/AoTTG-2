using System;
using UnityEngine;

public class CB_cameraTilt : MonoBehaviour
{
    private bool init;

    private void OnActivate(bool result)
    {
        if (!this.init)
        {
            this.init = true;
            if (PlayerPrefs.HasKey("cameraTilt"))
            {
                base.gameObject.GetComponent<UICheckbox>().isChecked = PlayerPrefs.GetInt("cameraTilt") == 1;
            }
            else
            {
                PlayerPrefs.SetInt("cameraTilt", 1);
            }
        }
        else
        {
            PlayerPrefs.SetInt("cameraTilt", !result ? 0 : 1);
        }
        IN_GAME_MAIN_CAMERA.cameraTilt = PlayerPrefs.GetInt("cameraTilt");
    }
}

