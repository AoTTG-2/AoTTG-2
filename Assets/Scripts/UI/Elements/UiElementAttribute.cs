using System;

namespace Assets.Scripts.UI.Elements
{

    [AttributeUsage(AttributeTargets.Property)]
    public class UiElementAttribute : Attribute
    {
        public string Label { get; private set; }
        public string Description { get; private set; }

        public UiElementAttribute(string label, string description)
        {
            Label = label;
            Description = description;
        }

    }
}
