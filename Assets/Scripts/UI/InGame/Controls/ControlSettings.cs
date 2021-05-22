namespace Assets.Scripts.UI.InGame.Controls
{
    public class ControlSettings
    {
        public float CameraDistance { get; set; } = 1.0f;
        public float MouseSensitivity { get; set; } = 0.5f;
        public bool CameraTilt { get; set; }
        public bool MouseInvert { get; set; }
        public bool GasBurstDoubleTap { get; set; }
        public bool Translate { get; set; }
        public bool AutoTranslate { get; set; }
    }
}
