using System;
using System.Collections.Generic;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Elements
{
    public class UiDropdown : UiElement
    {
        public Dropdown Dropdown;

        public Text DisplayValue;
        public Text LabelText;
        public Type Type;

        public int Value
        {
            get { return Dropdown.value; }
            set { Dropdown.value = value; }
        }

        public string Label
        {
            get { return LabelText.text; }
            set { LabelText.text = value; }
        }

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

        private void Awake()
        {
            Dropdown.options = new List<Dropdown.OptionData>();
        }

    }
}
