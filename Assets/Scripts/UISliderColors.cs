using System;
using UnityEngine;

[ExecuteInEditMode, AddComponentMenu("NGUI/Examples/Slider Colors"), RequireComponent(typeof(UISlider))]
public class UISliderColors : MonoBehaviour
{
    public Color[] colors = new Color[] { Color.red, Color.yellow, Color.green };
    private UISlider mSlider;
    public UISprite sprite;

    private void Start()
    {
        this.mSlider = base.GetComponent<UISlider>();
        this.Update();
    }

    private void Update()
    {
        if ((this.sprite != null) && (this.colors.Length != 0))
        {
            float f = this.mSlider.sliderValue * (this.colors.Length - 1);
            int index = Mathf.FloorToInt(f);
            Color color = this.colors[0];
            if (index >= 0)
            {
                if ((index + 1) < this.colors.Length)
                {
                    float t = f - index;
                    color = Color.Lerp(this.colors[index], this.colors[index + 1], t);
                }
                else if (index < this.colors.Length)
                {
                    color = this.colors[index];
                }
                else
                {
                    color = this.colors[this.colors.Length - 1];
                }
            }
            color.a = this.sprite.color.a;
            this.sprite.color = color;
        }
    }
}

