using Assets.Scripts.Services;
using Assets.Scripts.Settings.New;
using Assets.Scripts.Settings.New.Types;
using Assets.Scripts.UI.Elements;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.Localization.Tables;
using Attribute = System.Attribute;

namespace Assets.Scripts.UI.InGame.DebugWindow
{
    public class DebugToolsPanel : TabPanel
    {
        public UiToggleButton ToggleButtonPrefab;

        public GameObject DynamicContent;

        public StringTable StringTable;

        private void Awake()
        {
            var settings = Setting.Debug;
            var fields = settings.GetType().GetFields()
                .Where(x => Attribute.IsDefined(x, typeof(UiElementAttribute)));
                //.OrderBy(x => ((UiElementAttribute) x.GetCustomAttributes(typeof(UiElementAttribute), true)[0]).Category);

            foreach (var field in fields)
            {
                var attribute = (UiElementAttribute) Attribute.GetCustomAttribute(field, typeof(UiElementAttribute), true);
                CreateUiElement(field, settings, attribute);
            }
        }
        
        private void CreateUiElement(FieldInfo field, BaseSettings setting, UiElementAttribute attribute)
        {
            GameObject uiObject = null;
            var stringTable = Localization.GetStringTable(attribute.Localization);
            if (field.FieldType == typeof(BoolSetting))
            {
                uiObject = Instantiate(ToggleButtonPrefab.gameObject);
                var button = uiObject.GetComponent<UiToggleButton>();
                var boolSetting = (BoolSetting) field.GetValue(setting);
                button.Value = boolSetting.Value;
                button.Button.onClick.AddListener(() =>
                {
                    button.Value = !button.Value;
                    boolSetting.Value = button.Value;
                });
                button.Localize(stringTable, attribute.Header, attribute.Description);
            }

            if (uiObject != null)
            {
                uiObject.transform.SetParent(DynamicContent.transform);
                uiObject.transform.localScale = new Vector3(1, 1, 1);
                uiObject.SetActive(true);
            }
        }
    }
}
