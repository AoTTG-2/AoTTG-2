using UnityEngine;

namespace Assets.Scripts.Graphics
{
    public static class FramerateController
    {
        public static void SetFramerateLimit(string limit)
        {
            var parsed = int.TryParse(limit, out int frameRate);

            if (parsed)
            {
                SetFramerateLimit(frameRate);
            }
        }

        public static void SetFramerateLimit(int limit)
        {
            Application.targetFrameRate = limit;
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
