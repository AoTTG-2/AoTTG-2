using Assets.Scripts.Services;
using Assets.Scripts.Settings;
using System;

namespace Assets.Scripts.UI.Elements
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class UiElementAttribute : Attribute
    {
        [Obsolete("Use HEADER instead")]
        public string Label { get; private set; }

        public string Header { get; private set; }
        public string Description { get; private set; }
        public LocalizationEnum Localization { get; set; }
        
        public UiElementAttribute(string label, string description, LocalizationEnum localization = LocalizationEnum.Setting)
        {
            Header = label;
            Description = description;
            Localization = localization;
        }

    }
}
