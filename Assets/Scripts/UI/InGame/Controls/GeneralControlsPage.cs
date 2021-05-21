using System;
using Assets.Scripts.UI.Elements;
using Assets.Scripts.UI.Input;

namespace Assets.Scripts.UI.InGame.Controls
{
    public class GeneralControlsPage : UiContainer
    {
        public UiInput CameraDistance;
        public UiInput MouseSensitivity;
        public UiCheckbox CameraTilt;
        public UiCheckbox MouseInvert;
        public UiCheckbox GasBurstDoubleTap;
        public UiCheckbox AutoTranslate;

        private void Awake()
        {
            AddChild(CameraDistance);
            AddChild(MouseSensitivity);
            AddChild(CameraTilt);
            AddChild(MouseInvert);
            AddChild(GasBurstDoubleTap);
            AddChild(AutoTranslate);
            LoadSettings();
        }

        private void LoadSettings()
        {
            CameraDistance.Value = InputManager.Settings.CameraDistance;
            MouseSensitivity.Value = InputManager.Settings.MouseSensitivity;
            CameraTilt.Value = InputManager.Settings.CameraTilt;
            MouseInvert.Value = InputManager.Settings.MouseInvert;
            GasBurstDoubleTap.Value = InputManager.Settings.GasBurstDoubleTap;
            AutoTranslate.Value = InputManager.Settings.AutoTranslate;

            CameraDistance.Initialize();
            MouseSensitivity.Initialize();
            CameraTilt.Initialize();
            MouseInvert.Initialize();
            GasBurstDoubleTap.Initialize();
            AutoTranslate.Initialize();
        }

        public void Save()
        {
            InputManager.Settings = new ControlSettings
            {
                CameraDistance = Convert.ToSingle(CameraDistance.Value),
                MouseSensitivity = Convert.ToSingle(MouseSensitivity.Value),
                CameraTilt = CameraTilt.Value,
                MouseInvert = MouseInvert.Value,
                GasBurstDoubleTap = GasBurstDoubleTap.Value
            };
            InputManager.SaveOtherPlayerPrefs();
        }

        public void Load()
        {
            LoadSettings();
        }

        public void Default()
        {
            InputManager.Settings = new ControlSettings();
            LoadSettings();
            InputManager.SaveOtherPlayerPrefs();
        }
    }
}
