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

        private void OnEnable()
        {
            MenuManager.RegisterOpened();
        }

        private void OnDisable()
        {
            MenuManager.RegisterClosed();
        }

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
                button.onClick.AddListener(delegate {ShowRebinds(inputEnum);});
                button.transform.SetParent(TabViewContent.transform);
            }
        }

        private void ShowRebinds(Type inputEnum)
        {
            if (inputEnum == typeof(InputCannon))
            {
                foreach (InputCannon input in Enum.GetValues(typeof(InputCannon)))
                {
                    CreateRebindElement(input.ToString(), InputManager.GetKey(input).ToString());
                }
                Debug.Log("CANNONS");
            }
        }

        private void CreateRebindElement(string name, string value)
        {
            var rebindElement = Instantiate(RebindElementPrefab);
            rebindElement.transform.SetParent(RebindsViewContent.transform);
            rebindElement.Label.text = name;
            rebindElement.InputKey.GetComponentInChildren<Text>().text = value;
        }

        public void Load()
        {

        }

        public void Default()
        {

        }

        public void Save()
        {

        }
    }
}
