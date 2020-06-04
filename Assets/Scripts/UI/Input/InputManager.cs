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

        private const string InputHorsePlayerPrefs = "InputHorse";

        private void Awake()
        {
            LoadRebinds();
        }

        private static void LoadRebinds()
        {
            var horseRebinds = PlayerPrefs.GetString(InputHorsePlayerPrefs);
            if (string.IsNullOrEmpty(horseRebinds))
                SetDefaultHorseKeybindings();

            inputHorse = JsonConvert.DeserializeObject<Dictionary<InputHorse, KeyCode>>(horseRebinds);
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

            PlayerPrefs.SetString(InputHorsePlayerPrefs, JsonConvert.SerializeObject(inputHorse));
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

        public static bool KeyDown(InputHorse key)
        {
            if (InGameUi.IsMenuOpen()) return false;
            return UnityEngine.Input.GetKeyDown(inputHorse[key]);
        }
    }
}
