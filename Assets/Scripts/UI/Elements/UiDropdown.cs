using System;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Elements
{
    public class UiDropdown : UiElement
    {
        public Dropdown Dropdown;
        public Text DisplayValue;
        public Type Type;
        public Text LabelText;

        public int Value;
        public string Label;

        public void Initialize(Type type)
        {
            Type = type;
            if (type.IsEnum)
            {
                var enums = Enum.GetValues(type);
                foreach (var enumObject in enums)
                {
                    Dropdown.options.Add(new Dropdown.OptionData() { text = enumObject.ToString() });
                }
                Dropdown.value = Value;
                DisplayValue.text = Dropdown.options[Value].text;
                LabelText.text = Label;

            }
        }
    }
}
