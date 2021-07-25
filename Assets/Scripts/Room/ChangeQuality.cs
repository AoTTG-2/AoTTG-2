using System;
using UnityEngine;

[Obsolete("Will be removed as per #604")]
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
        if (val < 0.167f)
        {
            QualitySettings.SetQualityLevel(0, true);
        }
        else if (val < 0.33f)
        {
            QualitySettings.SetQualityLevel(1, true);
        }
        else if (val < 0.5f)
        {
            QualitySettings.SetQualityLevel(2, true);
        }
        else if (val < 0.67f)
        {
            QualitySettings.SetQualityLevel(3, true);
        }
        else if (val < 0.83f)
        {
            QualitySettings.SetQualityLevel(4, true);
        }
        else if (val <= 1f)
        {
            QualitySettings.SetQualityLevel(5, true);
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

