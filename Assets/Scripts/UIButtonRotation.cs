using System;
using UnityEngine;

[AddComponentMenu("NGUI/Interaction/Button Rotation")]
public class UIButtonRotation : MonoBehaviour
{
    public float duration = 0.2f;
    public Vector3 hover = Vector3.zero;
    private bool mHighlighted;
    private Quaternion mRot;
    private bool mStarted;
    public Vector3 pressed = Vector3.zero;
    public Transform tweenTarget;

    private void OnDisable()
    {
        if (this.mStarted && (this.tweenTarget != null))
        {
            TweenRotation component = this.tweenTarget.GetComponent<TweenRotation>();
            if (component != null)
            {
                component.rotation = this.mRot;
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
            TweenRotation.Begin(this.tweenTarget.gameObject, this.duration, !isOver ? this.mRot : (this.mRot * Quaternion.Euler(this.hover))).method = UITweener.Method.EaseInOut;
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
            TweenRotation.Begin(this.tweenTarget.gameObject, this.duration, !isPressed ? (!UICamera.IsHighlighted(base.gameObject) ? this.mRot : (this.mRot * Quaternion.Euler(this.hover))) : (this.mRot * Quaternion.Euler(this.pressed))).method = UITweener.Method.EaseInOut;
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
            this.mRot = this.tweenTarget.localRotation;
        }
    }
}

