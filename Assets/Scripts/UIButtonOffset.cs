using System;
using UnityEngine;

[AddComponentMenu("NGUI/Interaction/Button Offset")]
public class UIButtonOffset : MonoBehaviour
{
    public float duration = 0.2f;
    public Vector3 hover = Vector3.zero;
    private bool mHighlighted;
    private Vector3 mPos;
    private bool mStarted;
    public Vector3 pressed = new Vector3(2f, -2f);
    public Transform tweenTarget;

    private void OnDisable()
    {
        if (this.mStarted && (this.tweenTarget != null))
        {
            TweenPosition component = this.tweenTarget.GetComponent<TweenPosition>();
            if (component != null)
            {
                component.position = this.mPos;
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
            TweenPosition.Begin(this.tweenTarget.gameObject, this.duration, !isOver ? this.mPos : (this.mPos + this.hover)).method = UITweener.Method.EaseInOut;
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
            TweenPosition.Begin(this.tweenTarget.gameObject, this.duration, !isPressed ? (!UICamera.IsHighlighted(base.gameObject) ? this.mPos : (this.mPos + this.hover)) : (this.mPos + this.pressed)).method = UITweener.Method.EaseInOut;
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
            this.mPos = this.tweenTarget.localPosition;
        }
    }
}

