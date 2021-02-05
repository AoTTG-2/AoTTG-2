using System;
using UnityEngine;
using UnityEngine.UI;

public class FPSLimiter : MonoBehaviour
{


    [SerializeField] public InputField fpsLimiter;

    public InputField FPSLimit
    {
        get { return fpsLimiter; }
        set { fpsLimiter = value; }
    }


    private void Start()
    {
        if (FPSLimit.text == "")
        {
            Application.targetFrameRate = Screen.currentResolution.refreshRate;
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
        if (FPSLimit.contentType.Equals(InputField.ContentType.IntegerNumber))
        {
            int i;
            Int32.TryParse(text.ToString(), out i);
            Application.targetFrameRate = i;

        }

    }

    [Serializable]
    public struct FPSData
    {
        public string field;

        public FPSData(FPSLimiter toCopy)
        {
            this.field = toCopy.FPSLimit.text;
        }
    }


}
