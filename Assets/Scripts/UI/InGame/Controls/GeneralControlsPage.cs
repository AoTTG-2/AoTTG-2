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
        public UiCheckbox Translate;
        public UiCheckbox AutoTranslate;

        private void Awake()
        {
            AddChild(CameraDistance);
            AddChild(MouseSensitivity);
            AddChild(CameraTilt);
            AddChild(MouseInvert);
            AddChild(GasBurstDoubleTap);
            AddChild(Translate);
            LoadSettings();
        }

        private void LoadSettings()
        {
            CameraDistance.Value = InputManager.Settings.CameraDistance;
            MouseSensitivity.Value = InputManager.Settings.MouseSensitivity;
            CameraTilt.Value = InputManager.Settings.CameraTilt;
            MouseInvert.Value = InputManager.Settings.MouseInvert;
            GasBurstDoubleTap.Value = InputManager.Settings.GasBurstDoubleTap;
            Translate.Value = InputManager.Settings.Translate;
            AutoTranslate.Value = InputManager.Settings.AutoTranslate;
            AutoTranslate.gameObject.SetActive(Translate.Value);

            CameraDistance.Initialize();
            MouseSensitivity.Initialize();
            CameraTilt.Initialize();
            MouseInvert.Initialize();
            GasBurstDoubleTap.Initialize();
            Translate.Initialize();
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
                GasBurstDoubleTap = GasBurstDoubleTap.Value,
                Translate = Translate.Value,
                AutoTranslate = AutoTranslate.Value,

        };

            AutoTranslate.gameObject.SetActive(Translate.Value);
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
