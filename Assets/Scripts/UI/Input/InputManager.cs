using Assets.Scripts.UI.InGame;
using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.UI.Input
{
    public class InputManager : MonoBehaviour
    {
        private static Dictionary<InputHuman, KeyCode> inputHuman = new Dictionary<InputHuman, KeyCode>();
        private static Dictionary<InputHorse, KeyCode> inputHorse = new Dictionary<InputHorse, KeyCode>();
        private static Dictionary<InputTitan, KeyCode> inputTitan = new Dictionary<InputTitan, KeyCode>();

        private const string HorsePlayerPrefs = "InputHorse";
        private const string TitanPlayerPrefs = "InputTitan";

        private void Awake()
        {
            LoadRebinds();
        }

        private static void LoadRebinds()
        {
            var horseRebinds = PlayerPrefs.GetString(HorsePlayerPrefs);
            if (string.IsNullOrEmpty(horseRebinds))
                SetDefaultHorseKeybindings();

            inputHorse = JsonConvert.DeserializeObject<Dictionary<InputHorse, KeyCode>>(horseRebinds);

            SetDefaultTitanKeybindings();
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

            PlayerPrefs.SetString(TitanPlayerPrefs, JsonConvert.SerializeObject(inputHorse));
        }

        public static bool KeyPressed(InputHorse key)
        {
            if (InGameUi.IsMenuOpen()) return false;
            return UnityEngine.Input.GetKey(inputHorse[key]);
        }

        public static bool KeyPressed(InputHuman key)
        {
            if (InGameUi.IsMenuOpen()) return false;
            return UnityEngine.Input.GetKey(inputHuman[key]);
        }

        public static bool KeyPressed(InputTitan key)
        {
            if (InGameUi.IsMenuOpen()) return false;
            return UnityEngine.Input.GetKey(inputTitan[key]);
        }

        public static bool KeyDown(InputHorse key)
        {
            if (InGameUi.IsMenuOpen()) return false;
            return UnityEngine.Input.GetKeyDown(inputHorse[key]);
        }

        public static bool KeyDown(InputTitan key)
        {
            if (InGameUi.IsMenuOpen()) return false;
            return UnityEngine.Input.GetKeyDown(inputTitan[key]);
        }
    }
}
