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

    public void SetFPSLimit()
    {
        FPSLimit.interactable = true;
        var text = FPSLimit.text;
        int i;
        Int32.TryParse(text.ToString(), out i);
        Application.targetFrameRate = i;
        if (FPSLimit.text == "")
        {
            DefaultFPS();
        }
    }

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
