using System;
using UnityEngine;

[AddComponentMenu("NGUI/Interaction/Button Sound")]
public class UIButtonSound : MonoBehaviour
{
    public AudioClip audioClip;
    public float pitch = 1f;
    public Trigger trigger;
    public float volume = 1f;

    private void OnClick()
    {
        if (base.enabled && (this.trigger == Trigger.OnClick))
        {
            NGUITools.PlaySound(this.audioClip, this.volume, this.pitch);
        }
    }

    private void OnHover(bool isOver)
    {
        if (base.enabled && ((isOver && (this.trigger == Trigger.OnMouseOver)) || (!isOver && (this.trigger == Trigger.OnMouseOut))))
        {
            NGUITools.PlaySound(this.audioClip, this.volume, this.pitch);
        }
    }

    private void OnPress(bool isPressed)
    {
        if (base.enabled && ((isPressed && (this.trigger == Trigger.OnPress)) || (!isPressed && (this.trigger == Trigger.OnRelease))))
        {
            NGUITools.PlaySound(this.audioClip, this.volume, this.pitch);
        }
    }

    public enum Trigger
    {
        OnClick,
        OnMouseOver,
        OnMouseOut,
        OnPress,
        OnRelease
    }
}

