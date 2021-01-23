using System;
using UnityEngine;
using UnityEngine.UI;

public class FPSLimiter : MonoBehaviour
{
    public InputField fpsLimiter;

    public InputField FPSLimit
    {
        get { return fpsLimiter; }
        set { fpsLimiter = value; }
    }

    private void Start()
    {
        if (FPSLimit.text == "")
        {
            DefaultFPS();
        }
        else
        {
            SetFPSLimit();
        }
    }

    /*function to change the FPS limit*/
    public void SetFPSLimit()
    {
        FPSLimit.interactable = true;
        var text = FPSLimit.text;
        int limit;
        Int32.TryParse(text.ToString(), out limit);
        Application.targetFrameRate = limit;
        /*if the field is empty it will set it back to the default. This prevents the FPS going nuts upon clearing the limit*/
        if (FPSLimit.text == "")
        {
            DefaultFPS();
        }
    }

    /*function for setting the FPS to the refresh rate*/
    public void DefaultFPS()
    {
        Application.targetFrameRate = Screen.currentResolution.refreshRate;
    }

    public struct FPSData
    {
        public string field;

        public FPSData(FPSLimiter toCopy)
        {
            this.field = toCopy.FPSLimit.text;
        }
    }

}
