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

        public void Initialize()
        {
            LabelText.text = Label;
            ValueText.text = Value.ToString();
            InputField.placeholder.GetComponent<Text>().text = Value.ToString();
            InputField.text = Value.ToString();
            var isNumeric = Value is sbyte
                            || Value is byte
                            || Value is short
                            || Value is ushort
                            || Value is int
                            || Value is uint
                            || Value is long
                            || Value is ulong
                            || Value is float
                            || Value is double
                            || Value is decimal;
            if (isNumeric)
            {
                InputField.characterValidation = InputField.CharacterValidation.Integer;
            }
        }
    }
}