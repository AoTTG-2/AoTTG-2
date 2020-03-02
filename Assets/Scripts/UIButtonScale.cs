using System;
using UnityEngine;

[AddComponentMenu("NGUI/Interaction/Button Scale")]
public class UIButtonScale : MonoBehaviour
{
    public float duration = 0.2f;
    public Vector3 hover = new Vector3(1.1f, 1.1f, 1.1f);
    private bool mHighlighted;
    private Vector3 mScale;
    private bool mStarted;
    public Vector3 pressed = new Vector3(1.05f, 1.05f, 1.05f);
    public Transform tweenTarget;

    private void OnDisable()
    {
        if (this.mStarted && (this.tweenTarget != null))
        {
            TweenScale component = this.tweenTarget.GetComponent<TweenScale>();
            if (component != null)
            {
                component.scale = this.mScale;
                component.enabled = false;
            }
        }
    }

    private void OnEnable()
    {
        if (this.mStarted && this.mHighlighted)
        {
            this.OnHover(UICamera.IsHighlighted(base.gameObject));
        }
    }

    private void OnHover(bool isOver)
    {
        if (base.enabled)
        {
            if (!this.mStarted)
            {
                this.Start();
            }
            TweenScale.Begin(this.tweenTarget.gameObject, this.duration, !isOver ? this.mScale : Vector3.Scale(this.mScale, this.hover)).method = UITweener.Method.EaseInOut;
            this.mHighlighted = isOver;
        }
    }

    private void OnPress(bool isPressed)
    {
        if (base.enabled)
        {
            if (!this.mStarted)
            {
                this.Start();
            }
            TweenScale.Begin(this.tweenTarget.gameObject, this.duration, !isPressed ? (!UICamera.IsHighlighted(base.gameObject) ? this.mScale : Vector3.Scale(this.mScale, this.hover)) : Vector3.Scale(this.mScale, this.pressed)).method = UITweener.Method.EaseInOut;
        }
    }

    private void Start()
    {
        if (!this.mStarted)
        {
            this.mStarted = true;
            if (this.tweenTarget == null)
            {
                this.tweenTarget = base.transform;
            }
            this.mScale = this.tweenTarget.localScale;
        }
    }
}

