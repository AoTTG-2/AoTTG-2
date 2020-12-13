using System;
using Assets.Scripts.UI.Input;
using System.IO;
using UnityEngine;

public class ScreenshotHandler : MonoBehaviour
{
    private const string ScreenDirectory = "Screenshots";

    private void Awake()
    {
        var temp = $"{Application.dataPath}/{ScreenDirectory}";
        if (!Directory.Exists(temp))
            Directory.CreateDirectory(temp);
    }

    private void LateUpdate()
    {
        if (InputManager.KeyDown(InputUi.Screenshot))
        {
            var filename = $"{GetImagePath()}/{DateTime.Now:HHmmss}.png";
            ScreenCapture.CaptureScreenshot(filename);
        }
    }

    private static string GetImagePath()
    {
        string current = DateTime.Now.ToShortDateString();
        var cultured = current.Replace("/", "-");
        var final = $"{Application.dataPath}/{ScreenDirectory}/{cultured}";
        if (!Directory.Exists(final))
            Directory.CreateDirectory(final);

        return final;
    }

    #region Works, but discarded due to being expensive

    // public int resWidth = 2550;
    // public int resHeight = 3300;
    // private Camera cam;
    // private void GenerateScreenShot()
    // {
    //     RenderTexture rt = new RenderTexture(resWidth, resHeight, 24);
    //     cam.targetTexture = rt;
    //     Texture2D screenShot = new Texture2D(resWidth, resHeight, TextureFormat.RGB24, false);
    //     cam.Render();
    //     RenderTexture.active = rt;
    //     screenShot.ReadPixels(new Rect(0, 0, resWidth, resHeight), 0, 0);
    //     cam.targetTexture = null;
    //     RenderTexture.active = null; // JC: added to avoid errors
    //     Destroy(rt);
    //     byte[] bytes = screenShot.EncodeToPNG();
    //     string filename = GetImagePath();
    //
    //     File.WriteAllBytes(filename, bytes);
    //     
    //     Debug.Log($"Took screenshot to: {filename}");
    // }

    #endregion
}