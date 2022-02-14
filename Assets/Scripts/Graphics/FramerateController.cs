using UnityEngine;

namespace Assets.Scripts.Graphics
{
    public static class FramerateController
    {
        public static void SetFramerateLimit(string limit)
        {
            if (string.IsNullOrEmpty(limit))
            {
                UnlockFramerate();
            }
            else
            {
                int.TryParse(limit, out int frameRate);
                Application.targetFrameRate = frameRate;
            }
        }

        public static void LockFramerateToRefreshRate()
        {
            Application.targetFrameRate = Screen.currentResolution.refreshRate;
        }

        public static void UnlockFramerate()
        {
            Application.targetFrameRate = 0;
        }
    }
}
