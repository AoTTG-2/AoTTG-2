using Assets.Scripts.UI.Elements;
using Assets.Scripts.UI.Input;
using System;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.UI.InGame.Controls
{
    public class RebindsPage : UiContainer
    {
        public GameObject RebindsViewContent;
        public RebindElement RebindElementPrefab;

        private Type currentRebindType = typeof(InputHuman);

        public void Default()
        {
            InputManager.SetDefaultRebinds(currentRebindType);
            ShowRebinds(currentRebindType);
        }

        public void Load()
        {
            InputManager.LoadRebinds(currentRebindType);
            ShowRebinds(currentRebindType);
        }

        public void Save()
        {
            if (currentRebindType == typeof(InputCannon))
            {
                SaveRebinds<InputCannon>();
            }
            else if (currentRebindType == typeof(InputHorse))
            {
                SaveRebinds<InputHorse>();
            }
            else if (currentRebindType == typeof(InputHuman))
            {
                SaveRebinds<InputHuman>();
            }
            else if (currentRebindType == typeof(InputTitan))
            {
                SaveRebinds<InputTitan>();
            }
            else if (currentRebindType == typeof(InputUi))
            {
                SaveRebinds<InputUi>();
            }
        }

        public void ShowRebinds(Type inputEnum)
        {
            foreach (Transform child in RebindsViewContent.transform)
            {
                GameObject.Destroy(child.gameObject);
            }

            if (inputEnum == typeof(InputCannon))
            {
                CreateRebindElement<InputCannon>();
            }
            else if (inputEnum == typeof(InputHuman))
            {
                CreateRebindElement<InputHuman>();
            }
            else if (inputEnum == typeof(InputHorse))
            {
                CreateRebindElement<InputHorse>();
            }
            else if (inputEnum == typeof(InputTitan))
            {
                CreateRebindElement<InputTitan>();
            }
            else if (inputEnum == typeof(InputUi))
            {
                CreateRebindElement<InputUi>();
            }

            currentRebindType = inputEnum;
        }

        private void CreateRebindElement<T>()
        {
            foreach (T input in Enum.GetValues(typeof(T)))
            {
                var key = InputManager.GetKey(input);
                var rebindElement = Instantiate(RebindElementPrefab);
                rebindElement.transform.SetParent(RebindsViewContent.transform);
                rebindElement.Label.text = input.ToString();
                rebindElement.SetInputKeycode(key);
            }
        }

        private void SaveRebinds<T>()
        {
            var rebindKeys = RebindsViewContent.GetComponentsInChildren<RebindElement>();
            var keys = rebindKeys.Select(x => x.Key).ToArray();
            InputManager.SaveRebinds<T>(keys);
        }
    }
}
