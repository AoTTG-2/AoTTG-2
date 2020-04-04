using Assets.Scripts.Gamemode;
using Assets.Scripts.UI.Elements;
using System;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Assets.Scripts.UI.InGame
{
    public class GameSettingsMenu : MonoBehaviour
    {
        public GameObject Content;

        public UiCheckbox Checkbox;
        public UiInput Input;
        public GameObject List;

        private GamemodeBase Gamemode;

        public void OnEnable()
        {
            Gamemode = new KillTitansGamemode();
            SetSettings();
        }

        private void SetSettings()
        {
            var props = Gamemode.GetType().GetProperties().Where(
                prop => Attribute.IsDefined(prop, typeof(UiElementAttribute))).ToList();
            foreach (var prop in props)
            {
                CreateUiElement(prop);
            }
        }

        private void CreateUiElement(PropertyInfo property)
        {
            GameObject uiObject = null;
            if (property.PropertyType == typeof(bool))
            {
                uiObject = Instantiate(Checkbox.gameObject);
                var checkbox = uiObject.GetComponent<UiCheckbox>();
                var attribute = (UiElementAttribute)
                    Attribute.GetCustomAttribute(property, typeof(UiElementAttribute), true);
                checkbox.Label = attribute.Label;
                checkbox.Value = (bool) property.GetValue(Gamemode);
                checkbox.gameObject.name = property.Name;
                checkbox.Initialize();
            } else if (property.PropertyType == typeof(string) || (property.PropertyType == typeof(int)))
            {
                uiObject = Instantiate(Input.gameObject);
                var input = uiObject.GetComponent<UiInput>();
                var attribute = (UiElementAttribute)
                    Attribute.GetCustomAttribute(property, typeof(UiElementAttribute), true);
                input.Label = attribute.Label;
                input.Value = property.GetValue(Gamemode);
                input.gameObject.name = property.Name;
                input.Initialize();
            }



            if (uiObject != null)
            {
                uiObject.transform.parent = Content.transform;
                uiObject.SetActive(true);
            }
        }

        private GamemodeBase GetGamemodeFromSettings()
        {
            var gamemode = new KillTitansGamemode();
            foreach (Transform child in Content.transform)
            {
                var element = child.gameObject.GetComponent<UiElement>();
                if (element == null) continue;

                object value = null;
                if (element is UiCheckbox)
                {
                    value = ((UiCheckbox) element).Value;
                } else if (element is UiInput)
                {
                    value = ((UiInput) element).Value;
                }

                if (value == null) continue;
                var propertyInfo = gamemode.GetType().GetProperty(element.gameObject.name);
                if (propertyInfo == null)
                {
                    Debug.LogWarning($"Property could not be found: {element.gameObject.name}");
                    continue;
                }
                propertyInfo.SetValue(gamemode, Convert.ChangeType(value, propertyInfo.PropertyType), null);
            }
            return gamemode;
        }

        public void SyncSettings()
        {
            var gamemode = GetGamemodeFromSettings();
            if (!PhotonNetwork.isMasterClient) return;
            var json = JsonUtility.ToJson(gamemode);
            FengGameManagerMKII.instance.photonView.RPC("SyncSettings", PhotonTargets.All, json, gamemode.GamemodeType);
        }
    }
}