using Assets.Scripts.UI.Input;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.InGame.Rebinds
{
    public class RebindsMenu : MonoBehaviour
    {
        public GameObject TabViewContent;
        public Button TabViewButton;
        public GameObject RebindsViewContent;
        public RebindElement RebindElementPrefab;

        private Type CurrentRebinds;
        
        private void Awake()
        {
            var inputEnums = new List<Type>
            {
                typeof(InputCannon),
                typeof(InputHuman),
                typeof(InputHorse),
                typeof(InputTitan),
                typeof(InputUi)
            };

            foreach (var inputEnum in inputEnums)
            {
                var button = Instantiate(TabViewButton);
                var text = inputEnum.Name.Replace("Input", string.Empty);
                button.name = $"{text}Button";
                button.GetComponentInChildren<Text>().text = text;
                button.onClick.AddListener(delegate { ShowRebinds(inputEnum); });
                button.transform.SetParent(TabViewContent.transform);
            }
        }

        public void Default()
        {
            InputManager.SetDefaultRebinds(CurrentRebinds);
            ShowRebinds(CurrentRebinds);
        }

        public void Load()
        {
            InputManager.LoadRebinds(CurrentRebinds);
            ShowRebinds(CurrentRebinds);
        }

        public void Save()
        {
            if (CurrentRebinds == typeof(InputCannon))
            {
                SaveRebinds<InputCannon>();
            }
            else if (CurrentRebinds == typeof(InputHorse))
            {
                SaveRebinds<InputHorse>();
            }
            else if (CurrentRebinds == typeof(InputHuman))
            {
                SaveRebinds<InputHuman>();
            }
            else if (CurrentRebinds == typeof(InputTitan))
            {
                SaveRebinds<InputTitan>();
            }
            else if (CurrentRebinds == typeof(InputUi))
            {
                SaveRebinds<InputUi>();
            }
        }

        private void ShowRebinds(Type inputEnum)
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

            CurrentRebinds = inputEnum;
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
            var rebindDictionary = new Dictionary<T, KeyCode>();
            foreach (var rebindKey in rebindKeys)
            {
                rebindDictionary.Add((T) Enum.Parse(CurrentRebinds, rebindKey.Label.text), rebindKey.Key);
            }
            InputManager.SaveRebinds(rebindDictionary);
        }
    }
}
