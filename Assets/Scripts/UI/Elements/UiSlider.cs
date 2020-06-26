using UnityEngine.UI;

namespace Assets.Scripts.UI.Elements
{
    public class UiSlider : UiElement
    {
        public Slider Slider;
        public Text LabelText;

        public string Label;
        public float Value;
        private const float MinValue = 0.0f;
        private const float MaxValue = 100f;

        public void Initialize()
        {
            LabelText.text = Label;
            Slider.value = Value;
            Slider.minValue = MinValue;
            Slider.maxValue = MaxValue;
        }
    }
}
