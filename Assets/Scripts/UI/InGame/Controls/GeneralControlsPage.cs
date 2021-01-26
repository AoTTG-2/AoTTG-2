using System;
using Assets.Scripts.UI.Elements;
using Assets.Scripts.UI.Input;
using UnityEngine;

namespace Assets.Scripts.UI.InGame.Controls
{
    public class GeneralControlsPage : UiContainer
    {
        public UiInput CameraDistance;
        public UiInput MouseSensitivity;
        public UiCheckbox CameraTilt;
        public UiCheckbox MouseInvert;
        public UiCheckbox GasBurstDoubleTap;

        private void Awake()
        {
            AddChild(CameraDistance);
            AddChild(MouseSensitivity);
            AddChild(CameraTilt);
            AddChild(MouseInvert);
            AddChild(GasBurstDoubleTap);
            LoadSettings();
        }

        private void LoadSettings()
        {
            CameraDistance.Value = InputManager.Settings.CameraDistance;
            MouseSensitivity.Value = InputManager.Settings.MouseSensitivity;
            CameraTilt.Value = InputManager.Settings.CameraTilt;
            MouseInvert.Value = InputManager.Settings.MouseInvert;
            GasBurstDoubleTap.Value = InputManager.Settings.GasBurstDoubleTap;

            CameraDistance.Initialize();
            MouseSensitivity.Initialize();
            CameraTilt.Initialize();
            MouseInvert.Initialize();
            GasBurstDoubleTap.Initialize();
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
