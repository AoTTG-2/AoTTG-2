using System;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Elements
{
    public class UiInput : UiElement
    {
        public Text LabelText;
        public Text ValueText;
        public InputField InputField;

        public string Label = "Text";
        public object Value;

        public float MinValue = 0.0f;
        public float MaxValue = 0.0f;

        public void Initialize()
        {
            LabelText.text = Label;
            ValueText.text = Value.ToString();
            InputField.placeholder.GetComponent<Text>().text = Value.ToString();
            InputField.text = Value.ToString();
            InputField.onValueChanged.AddListener(delegate { Value = InputField.text; });

            var isNumeric = Value is sbyte
                            || Value is byte
                            || Value is short
                            || Value is ushort
                            || Value is int
                            || Value is uint
                            || Value is long
                            || Value is ulong;
            if (isNumeric)
            {
                InputField.characterValidation = InputField.CharacterValidation.Integer;
                SetRangeValidation();
            }

            var isDecimal = Value is float
                            || Value is double
                            || Value is decimal;
            if (isDecimal)
            {
                InputField.characterValidation = InputField.CharacterValidation.Decimal;
                SetRangeValidation();
            }
        }

        private void SetRangeValidation()
        {
            if (MinValue != 0.0f || MaxValue != 0.0f)
            {
                InputField.onValueChanged.AddListener(delegate { ValueChangedRangeCheck(); });
            }
        }

        private void ValueChangedRangeCheck()
        {
            if (InputField.text.EndsWith(".")) return;
            if (string.IsNullOrEmpty(InputField.text))
            {
                Value = InputField.placeholder.GetComponent<Text>().text;
                return;
            }
            var number = Mathf.Clamp(Convert.ToSingle(InputField.text, CultureInfo.InvariantCulture), MinValue, MaxValue);
            Value = number;
            InputField.text = number.ToString(CultureInfo.InvariantCulture);
        }
    }
}