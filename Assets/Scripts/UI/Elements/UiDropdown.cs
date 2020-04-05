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
                Dropdown.value = 0;
                DisplayValue.text = Dropdown.options[0].text;
                LabelText.text = Label;

            }
        }
    }
}
