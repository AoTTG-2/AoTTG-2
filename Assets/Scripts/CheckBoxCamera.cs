using System;
using UnityEngine;

public class CheckBoxCamera : MonoBehaviour
{
    public new CAMERA_TYPE camera;

    private void OnSelectionChange(bool yes)
    {
        if (yes)
        {
            IN_GAME_MAIN_CAMERA.cameraMode = this.camera;
            PlayerPrefs.SetString("cameraType", this.camera.ToString().ToUpper());
        }
    }

    private void Start()
    {
        if (PlayerPrefs.HasKey("cameraType"))
        {
            if (this.camera.ToString().ToUpper() == PlayerPrefs.GetString("cameraType").ToUpper())
            {
                base.GetComponent<UICheckbox>().isChecked = true;
            }
            else
            {
                base.GetComponent<UICheckbox>().isChecked = false;
            }
        }
    }
}

