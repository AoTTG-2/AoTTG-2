using UnityEngine.UI;

namespace Assets.Scripts.UI.Elements
{
    public class UiCheckbox : UiElement
    {
        public string Label = "Text";
        public bool Value;
        
        public void Initialize()
        {
            gameObject.GetComponentInChildren<Text>().text = Label;
            gameObject.GetComponent<Toggle>().isOn = Value;
        }

        public void OnValueChanged(bool value)
        {
            Value = value;
        }
    }
}