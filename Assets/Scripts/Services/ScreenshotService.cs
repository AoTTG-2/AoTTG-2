using Assets.Scripts.UI.Input;
using System;
using System.IO;
using UnityEngine;

namespace Assets.Scripts.Services
{
    /// <summary>
    /// A simple service which manages the Screenshot functionality
    /// </summary>
    public class ScreenshotService : MonoBehaviour
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
                FengGameManagerMKII.instance.chatRoom.OutputSystemMessage("Screenshot Captured.");    //Refactor, when ChatService implemented.
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
    }
}