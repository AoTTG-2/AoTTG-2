using UnityEngine;

namespace Assets.Scripts.Graphics
{
    /// <summary>
    /// Used to set the framerate to a specific value or unlock the framerate.
    /// </summary>
    public static class FramerateController
    {
        public static void SetFramerateLimit(int limit)
        {
            Application.targetFrameRate = limit;
            Debug.Log($"Framerate set to {Application.targetFrameRate}");
        }

        public static void LockFramerateToRefreshRate()
        {
            SetFramerateLimit(Screen.currentResolution.refreshRate);
        }

        public static void UnlockFramerate()
        {
            SetFramerateLimit(0);
        }
    }
}
