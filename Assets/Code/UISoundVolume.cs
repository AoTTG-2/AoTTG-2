using System;
using UnityEngine;

[AddComponentMenu("NGUI/Interaction/Sound Volume"), RequireComponent(typeof(UISlider))]
public class UISoundVolume : MonoBehaviour
{
    private UISlider mSlider;

    private void Awake()
    {
        this.mSlider = base.GetComponent<UISlider>();
        this.mSlider.sliderValue = NGUITools.soundVolume;
        this.mSlider.eventReceiver = base.gameObject;
    }

    private void OnSliderChange(float val)
    {
        NGUITools.soundVolume = val;
    }
}

