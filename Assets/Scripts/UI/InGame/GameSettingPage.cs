using Assets.Scripts.Settings;
using Assets.Scripts.UI.Elements;
using System;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.InGame
{
    public class GameSettingPage : UiContainer
    {
        public GameObject Content;

        public UiCheckbox Checkbox;
        public UiInput Input;
        public UiDropdown Dropdown;
        public GameObject Category;
        public GameObject EmptyGridItem;

        public object Data;
        private SettingCategory category = SettingCategory.None;
        private readonly int _columnCount = 3;

        public void Initialize()
        {
            SetSettings();
        }

        private void SetSettings()
        {
            var properties = Data.GetType().GetProperties()
                .Where(prop => Attribute.IsDefined(prop, typeof(UiElementAttribute)));
                                  //OrderBy(x => ((UiElementAttribute)x.GetCustomAttributes(typeof(UiElementAttribute), true)[0]).Category);

            foreach (var property in properties)
            {
                var attribute = (UiElementAttribute)Attribute.GetCustomAttribute(property, typeof(UiElementAttribute), true);
                CreateUiElement(property, attribute);
            }
        }

        private void CreateUiElement(PropertyInfo property, UiElementAttribute attribute)
        {
            //if (attribute.Category != category)
            //{
            //    CreateCategory(attribute.Category);
            //}

            GameObject uiObject = null;
            if (property.PropertyType == typeof(bool))
            {
                uiObject = Instantiate(Checkbox.gameObject);
                var checkbox = uiObject.GetComponent<UiCheckbox>();

                checkbox.Label = attribute.Label;
                checkbox.Value = (bool) property.GetValue(Data);
                checkbox.gameObject.name = property.Name;
                checkbox.Initialize();
                AddChild(checkbox);
            } else if (property.PropertyType == typeof(string) 
                    || property.PropertyType == typeof(int) 
                    || property.PropertyType == typeof(float))
            {
                uiObject = Instantiate(Input.gameObject);
                var input = uiObject.GetComponent<UiInput>();
                input.Label = attribute.Label;
                input.Value = property.GetValue(Data);
                input.gameObject.name = property.Name;
                input.Initialize();
                AddChild(input);
            } else if (property.PropertyType.IsEnum)
            {
                uiObject = Instantiate(Dropdown.gameObject);
                var input = uiObject.GetComponent<UiDropdown>();
                input.Label = attribute.Label;
                input.Value = (int) property.GetValue(Data);
                input.gameObject.name = property.Name;
                input.Initialize(property.PropertyType);
                AddChild(input);
            }

            if (uiObject != null)
            {
                uiObject.transform.SetParent(Content.transform);
                uiObject.transform.localScale = new Vector3(1 , 1 ,1);
                uiObject.SetActive(true);
            }
        }

        private void CreateCategory(SettingCategory settingCategory)
        {
            category = settingCategory;
            var childCount = Content.transform.childCount;
            var categoryUi = Instantiate(Category);
            categoryUi.GetComponentInChildren<Text>().text = category.ToString();

            if (childCount == 0)
            {
                CreateEmptyGridItem();
                categoryUi.transform.SetParent(Content.transform);
                CreateEmptyGridItem();
                return;
            }

            var currentColumn = childCount % _columnCount;
            CreateEmptyGridItem(4 - currentColumn);
            categoryUi.transform.SetParent(Content.transform);
            CreateEmptyGridItem();
        }

        private void CreateEmptyGridItem(int amount = 1)
        {
            for (var i = 0; i < amount; i++)
            {
                Instantiate(EmptyGridItem, Content.transform);
            }
        }
    }
}