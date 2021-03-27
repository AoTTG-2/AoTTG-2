using System;
using UnityEngine;

public class ChangeQuality : MonoBehaviour
{
    private bool init;
    public static bool isTiltShiftOn;

    public static void setCurrentQuality()
    {
        if (PlayerPrefs.HasKey("GameQuality"))
        {
            setQuality(PlayerPrefs.GetFloat("GameQuality"));
        }
    }

    private static void setQuality(float val)
    {
        if (val < 0.143f)
        {
            QualitySettings.SetQualityLevel(0, true);
        }
        else if (val < 0.286f)
        {
            QualitySettings.SetQualityLevel(1, true);
        }
        else if (val < 0.429f)
        {
            QualitySettings.SetQualityLevel(2, true);
        }
        else if (val < 0.572f)
        {
            QualitySettings.SetQualityLevel(3, true);
        }
        else if (val < 0.715f)
        {
            QualitySettings.SetQualityLevel(4, true);
        }
        else if (val < 0.858f)
        {
            QualitySettings.SetQualityLevel(5, true);
        }
        else if (val <= 1f)
        {
            QualitySettings.SetQualityLevel(6, true);
        }
        if (val < 0.9f)
        {
            turnOffTiltShift();
        }
        else
        {
            turnOnTiltShift();
        }
    }

    public static void turnOffTiltShift()
    {
        isTiltShiftOn = false;
        if (GameObject.Find("MainCamera") != null)
        {
            //TODO TiltShift
            //GameObject.Find("MainCamera").GetComponent<TiltShift>().enabled = false;
        }
    }

    public static void turnOnTiltShift()
    {
        isTiltShiftOn = true;
        if (GameObject.Find("MainCamera") != null)
        {
            //TODO TiltShift
            //GameObject.Find("MainCamera").GetComponent<TiltShift>().enabled = true;
        }
    }
}

