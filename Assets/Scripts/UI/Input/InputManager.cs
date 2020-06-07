using Assets.Scripts.UI.InGame;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.UI.Input
{
    public class InputManager : MonoBehaviour
    {
        private static Dictionary<InputCannon, KeyCode> inputCannon = new Dictionary<InputCannon, KeyCode>();
        private static Dictionary<InputHorse, KeyCode> inputHorse = new Dictionary<InputHorse, KeyCode>();
        private static Dictionary<InputHuman, KeyCode> inputHuman = new Dictionary<InputHuman, KeyCode>();
        private static Dictionary<InputTitan, KeyCode> inputTitan = new Dictionary<InputTitan, KeyCode>();
        private static Dictionary<InputUi, KeyCode> inputUi = new Dictionary<InputUi, KeyCode>();

        private const string CannonPlayerPrefs = "InputCannon";
        private const string HorsePlayerPrefs = "InputHorse";
        private const string HumanPlayerPrefs = "InputHuman";
        private const string TitanPlayerPrefs = "InputTitan";
        private const string UiPlayerPrefs = "InputUi";

        private void Awake()
        {
            LoadRebinds();
        }

        #region Default Rebinds

        private static void SetDefaultCannonKeybindings()
        {
            inputCannon = new Dictionary<InputCannon, KeyCode>
            {
                [InputCannon.Up] = KeyCode.W,
                [InputCannon.Down] = KeyCode.S,
                [InputCannon.Left] = KeyCode.A,
                [InputCannon.Right] = KeyCode.D,
                [InputCannon.Shoot] = KeyCode.Q,
                [InputCannon.Slow] = KeyCode.LeftShift,
                [InputCannon.Mount] = KeyCode.G
            };

            PlayerPrefs.SetString(CannonPlayerPrefs, JsonConvert.SerializeObject(inputCannon));
        }

        private static void SetDefaultHorseKeybindings()
        {
            inputHorse = new Dictionary<InputHorse, KeyCode>
            {
                [InputHorse.Forward] = KeyCode.W,
                [InputHorse.Backward] = KeyCode.S,
                [InputHorse.Left] = KeyCode.A,
                [InputHorse.Right] = KeyCode.D,
                [InputHorse.Jump] = KeyCode.LeftShift,
                [InputHorse.Walk] = KeyCode.Space,
                [InputHorse.Mount] = KeyCode.LeftControl
            };

            PlayerPrefs.SetString(HorsePlayerPrefs, JsonConvert.SerializeObject(inputHorse));
        }

        private static void SetDefaultHumanKeybindings()
        {
            inputHuman = new Dictionary<InputHuman, KeyCode>
            {
                [InputHuman.Forward] = KeyCode.W,
                [InputHuman.Backward] = KeyCode.S,
                [InputHuman.Left] = KeyCode.A,
                [InputHuman.Right] = KeyCode.D,
                [InputHuman.Jump] = KeyCode.LeftShift,
                [InputHuman.Gas] = KeyCode.LeftShift,
                [InputHuman.Dodge] = KeyCode.LeftControl,
                [InputHuman.Salute] = KeyCode.N,
                [InputHuman.Reload] = KeyCode.R,
                [InputHuman.ReelIn] = KeyCode.Space,
                [InputHuman.ReelOut] = KeyCode.Mouse2,
                [InputHuman.GasBurst] = KeyCode.LeftAlt,
                [InputHuman.Attack] = KeyCode.Mouse0,
                [InputHuman.AttackSpecial] = KeyCode.Mouse1,
                [InputHuman.HookLeft] = KeyCode.Q,
                [InputHuman.HookRight] = KeyCode.E,
                [InputHuman.HookBoth] = KeyCode.None,
                [InputHuman.Item1] = KeyCode.Alpha1,
                [InputHuman.Item2] = KeyCode.Alpha2,
                [InputHuman.Item3] = KeyCode.Alpha3,
                [InputHuman.Focus] = KeyCode.F
            };

            PlayerPrefs.SetString(HumanPlayerPrefs, JsonConvert.SerializeObject(inputHuman));
        }

        private static void SetDefaultTitanKeybindings()
        {
            inputTitan = new Dictionary<InputTitan, KeyCode>
            {
                [InputTitan.Forward] = KeyCode.W,
                [InputTitan.Backward] = KeyCode.S,
                [InputTitan.Left] = KeyCode.A,
                [InputTitan.Right] = KeyCode.D,
                [InputTitan.AttackGrabFront] = KeyCode.Alpha1,
                [InputTitan.AttackBite] = KeyCode.Alpha2,
                [InputTitan.AttackGrabBack] = KeyCode.Alpha3,
                [InputTitan.Jump] = KeyCode.Space,
                [InputTitan.Walk] = KeyCode.LeftShift,
                [InputTitan.AttackPunch] = KeyCode.Q,
                [InputTitan.AttackBodySlam] = KeyCode.E,
                [InputTitan.AttackSlap] = KeyCode.Mouse0,
                [InputTitan.AttackGrabNape] = KeyCode.Mouse1,
                [InputTitan.Cover] = KeyCode.Z,
                [InputTitan.Blend] = KeyCode.F
            };

            PlayerPrefs.SetString(TitanPlayerPrefs, JsonConvert.SerializeObject(inputTitan));
        }

        private static void SetDefaultUiKeybindings()
        {
            inputUi = new Dictionary<InputUi, KeyCode>
            {
                [InputUi.Chat] = KeyCode.Return,
                [InputUi.ToggleCursor] = KeyCode.X,
                [InputUi.LiveCamera] = KeyCode.Y,
                [InputUi.Minimap] = KeyCode.M,
                [InputUi.Fullscreen] = KeyCode.Backspace,
                [InputUi.Camera] = KeyCode.C,
                [InputUi.Pause] = KeyCode.P,
                [InputUi.Restart] = KeyCode.T,
                [InputUi.InteractionWheel] = KeyCode.Tab
            };

            PlayerPrefs.SetString(UiPlayerPrefs, JsonConvert.SerializeObject(inputUi));
        }

        #endregion

        private static void LoadRebinds()
        {
            //var horseRebinds = PlayerPrefs.GetString(HorsePlayerPrefs);
            //if (string.IsNullOrEmpty(horseRebinds))
            //    SetDefaultHorseKeybindings();

            //inputHorse = JsonConvert.DeserializeObject<Dictionary<InputHorse, KeyCode>>(horseRebinds);

            SetDefaultCannonKeybindings();
            SetDefaultHorseKeybindings();
            SetDefaultHumanKeybindings();
            SetDefaultTitanKeybindings();
            SetDefaultUiKeybindings();
        }

        public static bool KeyPressed<T>(T inputEnum)
        {
            if (inputEnum is InputCannon)
            {
                if (InGameUi.IsMenuOpen()) return false;
                var input = (InputCannon) (object) inputEnum;
                return UnityEngine.Input.GetKey(inputCannon[input]);
            }

            if (inputEnum is InputHuman)
            {
                if (InGameUi.IsMenuOpen()) return false;
                var input = (InputHuman) (object) inputEnum;
                return UnityEngine.Input.GetKey(inputHuman[input]);
            }

            if (inputEnum is InputHorse)
            {
                if (InGameUi.IsMenuOpen()) return false;
                var input = (InputHorse) (object) inputEnum;
                return UnityEngine.Input.GetKey(inputHorse[input]);
            }

            if (inputEnum is InputTitan)
            {
                if (InGameUi.IsMenuOpen()) return false;
                var input = (InputTitan) (object) inputEnum;
                return UnityEngine.Input.GetKey(inputTitan[input]);
            }

            if (inputEnum is InputUi)
            {
                var input = (InputUi) (object) inputEnum;
                return UnityEngine.Input.GetKey(inputUi[input]);
            }

            throw new ArgumentException($"{inputEnum.GetType()} is not implemented in InputManager.KeyPressed");
        }

        public static bool KeyDown<T>(T inputEnum)
        {
            if (inputEnum is InputCannon)
            {
                if (InGameUi.IsMenuOpen()) return false;
                var input = (InputCannon) (object) inputEnum;
                return UnityEngine.Input.GetKeyDown(inputCannon[input]);
            }

            if (inputEnum is InputHuman)
            {
                if (InGameUi.IsMenuOpen()) return false;
                var input = (InputHuman) (object) inputEnum;
                return UnityEngine.Input.GetKeyDown(inputHuman[input]);
            }

            if (inputEnum is InputHorse)
            {
                if (InGameUi.IsMenuOpen()) return false;
                var input = (InputHorse) (object) inputEnum;
                return UnityEngine.Input.GetKeyDown(inputHorse[input]);
            }

            if (inputEnum is InputTitan)
            {
                if (InGameUi.IsMenuOpen()) return false;
                var input = (InputTitan) (object) inputEnum;
                return UnityEngine.Input.GetKeyDown(inputTitan[input]);
            }

            if (inputEnum is InputUi)
            {
                var input = (InputUi) (object) inputEnum;
                return UnityEngine.Input.GetKeyDown(inputUi[input]);
            }

            throw new ArgumentException($"{inputEnum.GetType()} is not implemented in InputManager.KeyDown");
        }

        public static bool KeyUp<T>(T inputEnum)
        {
            if (inputEnum is InputCannon)
            {
                if (InGameUi.IsMenuOpen()) return false;
                var input = (InputCannon) (object) inputEnum;
                return UnityEngine.Input.GetKeyUp(inputCannon[input]);
            }

            if (inputEnum is InputHuman)
            {
                if (InGameUi.IsMenuOpen()) return false;
                var input = (InputHuman) (object) inputEnum;
                return UnityEngine.Input.GetKeyUp(inputHuman[input]);
            }

            if (inputEnum is InputHorse)
            {
                if (InGameUi.IsMenuOpen()) return false;
                var input = (InputHorse) (object) inputEnum;
                return UnityEngine.Input.GetKeyUp(inputHorse[input]);
            }

            if (inputEnum is InputTitan)
            {
                if (InGameUi.IsMenuOpen()) return false;
                var input = (InputTitan) (object) inputEnum;
                return UnityEngine.Input.GetKeyUp(inputTitan[input]);
            }

            if (inputEnum is InputUi)
            {
                var input = (InputUi) (object) inputEnum;
                return UnityEngine.Input.GetKeyUp(inputUi[input]);
            }

            throw new ArgumentException($"{inputEnum.GetType()} is not implemented in InputManager.KeyUp");
        }

        public static KeyCode GetKey<T>(T inputEnum)
        {
            if (inputEnum is InputCannon)
            {
                var input = (InputCannon) (object) inputEnum;
                return inputCannon[input];
            }

            if (inputEnum is InputHorse)
            {
                var input = (InputHorse) (object) inputEnum;
                return inputHorse[input];
            }

            if (inputEnum is InputHuman)
            {
                var input = (InputHuman) (object) inputEnum;
                return inputHuman[input];
            }

            if (inputEnum is InputTitan)
            {
                var input = (InputTitan) (object) inputEnum;
                return inputTitan[input];
            }

            if (inputEnum is InputUi)
            {
                var input = (InputUi) (object) inputEnum;
                return inputUi[input];
            }

            throw new ArgumentException($"{inputEnum.GetType()} is not implemented in InputManager.GetKey");
        }
    }
}
