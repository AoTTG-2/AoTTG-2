using System;
using UnityEngine;

public class PopListCamera : MonoBehaviour
{
    private void Awake()
    {
        if (PlayerPrefs.HasKey("cameraType"))
        {
            base.GetComponent<UIPopupList>().selection = PlayerPrefs.GetString("cameraType");
        }
    }

    private void OnSelectionChange()
    {
        if (base.GetComponent<UIPopupList>().selection == "ORIGINAL")
        {
            IN_GAME_MAIN_CAMERA.cameraMode = CAMERA_TYPE.ORIGINAL;
        }
        if (base.GetComponent<UIPopupList>().selection == "WOW")
        {
            IN_GAME_MAIN_CAMERA.cameraMode = CAMERA_TYPE.WOW;
        }
        if (base.GetComponent<UIPopupList>().selection == "TPS")
        {
            IN_GAME_MAIN_CAMERA.cameraMode = CAMERA_TYPE.TPS;
        }
        PlayerPrefs.SetString("cameraType", base.GetComponent<UIPopupList>().selection);
    }
}

