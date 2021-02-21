using Assets.Scripts.Services;
using Assets.Scripts.Settings.Gamemodes;
using Assets.Scripts.UI.Elements;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.UI.InGame
{
    public class GameSettingMenu : UiMenu
    {
        public GameSettingPage GameSettingPage;
        public ServerSettingsPage ServerSettingsPage;
        private readonly List<GameSettingPage> pages = new List<GameSettingPage>();
        private GamemodeSettings gamemode;

        public void ViewGameSettingPage()
        {
            pages[0].Show();
            ServerSettingsPage.Hide();
        }

        public void ViewServerSettingsPage()
        {
            //TODO Rework this for the new GameSettings
            //Pages[0].gameObject.SetActive(false);
            ServerSettingsPage.Show();
        }

        public void SyncSettings()
        {
            if (!PhotonNetwork.isMasterClient) return;
            Service.Settings.SyncSettings();
        }

        private GamemodeSettings GetGamemodeFromSettings()
        {
            foreach (var page in pages)
            {
                foreach (var element in page.GetComponentsInChildren<UiElement>())
                {
                    if (element == null) continue;

                    object value = null;
                    switch (element)
                    {
                        case UiCheckbox checkbox:
                            value = checkbox.Value;
                            break;
                        case UiInput input:
                            value = input.InputField.text;
                            break;
                        case UiDropdown dropdown:
                            value = dropdown.Dropdown.value;
                            break;
                    }

                    if (value == null) continue;
                    var propertyInfo = gamemode.GetType().GetProperty(element.gameObject.name);
                    if (propertyInfo == null)
                    {
                        Debug.LogWarning($"Property could not be found: {element.gameObject.name}");
                        continue;
                    }

                    if (propertyInfo.PropertyType.IsEnum)
                    {
                        propertyInfo.SetValue(gamemode, value);
                    }
                    else
                    {
                        propertyInfo.SetValue(gamemode, Convert.ChangeType(value, propertyInfo.PropertyType), null);
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
            return gamemode;
        }
        private void Awake()
        {
            AddChild(GameSettingPage);
            AddChild(ServerSettingsPage);
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            //TODO: Rework this for the new GameSettings
            //var page = Instantiate(GameSettingPage, gameObject.transform);
            //page.Data = Gamemode = GameSettings.Gamemode;
            //page.Initialize();
            //Pages.Add(page);
            //Pages[0].gameObject.SetActive(true);
            ServerSettingsPage.gameObject.SetActive(true);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            foreach (var page in pages)
            {
                Destroy(page.gameObject);
            }
            pages.Clear();
        }

    }
}