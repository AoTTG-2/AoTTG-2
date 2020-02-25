using System;
using UnityEngine;

public class SliderCameraDist : MonoBehaviour
{
    private bool init;

    private void OnSliderChange(float value)
    {
        if (!this.init)
        {
            this.init = true;
            if (PlayerPrefs.HasKey("cameraDistance"))
            {
                float @float = PlayerPrefs.GetFloat("cameraDistance");
                base.gameObject.GetComponent<UISlider>().sliderValue = @float;
                value = @float;
            }
            else
            {
                PlayerPrefs.SetFloat("cameraDistance", base.gameObject.GetComponent<UISlider>().sliderValue);
            }
        }
        else
        {
            PlayerPrefs.SetFloat("cameraDistance", base.gameObject.GetComponent<UISlider>().sliderValue);
        }
        IN_GAME_MAIN_CAMERA.cameraDistance = 0.3f + value;
    }
}

