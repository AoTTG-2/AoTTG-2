using System;
using UnityEngine;
using UnityEngine.UI;

public class FPSLimiter : MonoBehaviour
{
    public InputField fpsLimiter;
    public int limit;

    public InputField FPSLimit { get; set; }

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
        FPSLimit = fpsLimiter;
        FPSLimit.interactable = true;
        var text = FPSLimit.text;
        text = text.Replace("-", string.Empty);
        int.TryParse(text, out limit);
        Application.targetFrameRate = limit;
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
