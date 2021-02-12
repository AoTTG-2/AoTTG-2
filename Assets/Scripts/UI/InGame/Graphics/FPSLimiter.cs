using System;
using UnityEngine;
using UnityEngine.UI;

public class FPSLimiter : MonoBehaviour
{
    public InputField fpsLimiter;
    public int limit; ///public so it can be easily modified and accessed by other classes

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
        text = text.Replace("-", string.Empty); ///prevents negative values from breaking fps limit
        int.TryParse(text.ToString(), out limit);
        Application.targetFrameRate = limit;
        if (FPSLimit.text == "")
        {
            DefaultFPS();
        }
    }

    public void DefaultFPS()
    {
        Application.targetFrameRate = Screen.currentResolution.refreshRate; ///VSync
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
