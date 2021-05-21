using UnityEngine;

namespace Assets.Scripts.UI.InGame.Controls
{
    public class ControlSettings
    {
        public float CameraDistance { get; set; } = 1.0f;
        public float MouseSensitivity { get; set; } = 0.5f;
        public bool CameraTilt { get; set; }
        public bool MouseInvert { get; set; }
        public bool GasBurstDoubleTap { get; set; }
        public bool AutoTranslate { get { return AutoTranslate; } set { AutoTranslate = value;
                Debug.Log($"Auto Translate is {value}"); } }
    }
}
