using Assets.Scripts.Gamemode;
using Assets.Scripts.UI.Elements;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.UI.InGame
{
    public class GameSettingMenu : MonoBehaviour
    {
        public GameSettingPage GameSettingPage;
        public ServerSettingsPage ServerSettingsPage;
        private List<GameSettingPage> Pages = new List<GameSettingPage>();
        private GamemodeBase Gamemode;

        private void OnEnable()
        {
            var page = Instantiate(GameSettingPage, gameObject.transform);
            page.Data = Gamemode = FengGameManagerMKII.Gamemode;
            page.Initialize();
            Pages.Add(page);
            Pages[0].gameObject.SetActive(true);
            ServerSettingsPage.gameObject.SetActive(false);
        }

        private void OnDisable()
        {
            foreach (var page in Pages)
            {
                Destroy(page.gameObject);
            }
            Pages.Clear();
        }

        public void ViewGameSettingPage()
        {
            Pages[0].gameObject.SetActive(true);
            ServerSettingsPage.gameObject.SetActive(false);
        }

        public void ViewServerSettingsPage()
        {
            Pages[0].gameObject.SetActive(false);
            ServerSettingsPage.gameObject.SetActive(true);
        }

        private GamemodeBase GetGamemodeFromSettings()
        {
            foreach (var page in Pages)
            {
                foreach (var element in page.GetComponentsInChildren<UiElement>())
                {
                    if (element == null) continue;

                    object value = null;
                    if (element is UiCheckbox)
                    {
                        value = ((UiCheckbox)element).Value;
                    }
                    else if (element is UiInput)
                    {
                        value = ((UiInput)element).InputField.text;
                    }
                    else if (element is UiDropdown)
                    {
                        value = ((UiDropdown)element).Dropdown.value;
                    }

                    if (value == null) continue;
                    var propertyInfo = Gamemode.GetType().GetProperty(element.gameObject.name);
                    if (propertyInfo == null)
                    {
                        Debug.LogWarning($"Property could not be found: {element.gameObject.name}");
                        continue;
                    }

                    if (propertyInfo.PropertyType.IsEnum)
                    {
                        propertyInfo.SetValue(Gamemode, value);
                    }
                    else
                    {
                        propertyInfo.SetValue(Gamemode, Convert.ChangeType(value, propertyInfo.PropertyType), null);
                    }
                }
                //foreach (Transform child in page.transform)
                //{
                //    var element = child.gameObject.GetComponent<UiElement>();
                //    if (element == null) continue;

                //    object value = null;
                //    if (element is UiCheckbox)
                //    {
                //        value = ((UiCheckbox)element).Value;
                //    }
                //    else if (element is UiInput)
                //    {
                //        value = ((UiInput)element).Value;
                //    } else if (element is UiDropdown)
                //    {
                //        value = ((UiDropdown) element).Dropdown.value;
                //    }

                //    if (value == null) continue;
                //    var propertyInfo = Gamemode.GetType().GetProperty(element.gameObject.name);
                //    if (propertyInfo == null)
                //    {
                //        Debug.LogWarning($"Property could not be found: {element.gameObject.name}");
                //        continue;
                //    }
                //    propertyInfo.SetValue(Gamemode, Convert.ChangeType(value, propertyInfo.PropertyType), null);
                //}
            }
            return Gamemode;
        }

        public void SyncSettings()
        {
            if (!PhotonNetwork.isMasterClient) return;
            var gamemode = GetGamemodeFromSettings();
            var json = JsonConvert.SerializeObject(gamemode);
            FengGameManagerMKII.instance.photonView.RPC("SyncSettings", PhotonTargets.All, json, gamemode.GamemodeType);
        }
    }
}