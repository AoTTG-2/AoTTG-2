using Assets.Scripts.Settings;
using System;

namespace Assets.Scripts.UI.Elements
{

    [AttributeUsage(AttributeTargets.Property)]
    public class UiElementAttribute : Attribute
    {
        public string Label { get; private set; }
        public string Description { get; private set; }
        public SettingCategory Category { get; set; }

        public UiElementAttribute(string label, string description, SettingCategory category = SettingCategory.General)
        {
            Label = label;
            Description = description;
            Category = category;
        }

    }
}
