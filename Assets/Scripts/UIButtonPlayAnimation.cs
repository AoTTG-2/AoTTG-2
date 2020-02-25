using AnimationOrTween;
using System;
using UnityEngine;

[AddComponentMenu("NGUI/Interaction/Button Play Animation")]
public class UIButtonPlayAnimation : MonoBehaviour
{
    public string callWhenFinished;
    public bool clearSelection;
    public string clipName;
    public DisableCondition disableWhenFinished;
    public GameObject eventReceiver;
    public EnableCondition ifDisabledOnPlay;
    private bool mHighlighted;
    private bool mStarted;
    public ActiveAnimation.OnFinished onFinished;
    public AnimationOrTween.Direction playDirection = AnimationOrTween.Direction.Forward;
    public bool resetOnPlay;
    public Animation target;
    public AnimationOrTween.Trigger trigger;

    private void OnActivate(bool isActive)
    {
        if (base.enabled && (((this.trigger == AnimationOrTween.Trigger.OnActivate) || ((this.trigger == AnimationOrTween.Trigger.OnActivateTrue) && isActive)) || ((this.trigger == AnimationOrTween.Trigger.OnActivateFalse) && !isActive)))
        {
            this.Play(isActive);
        }
    }

    private void OnClick()
    {
        if (base.enabled && (this.trigger == AnimationOrTween.Trigger.OnClick))
        {
            this.Play(true);
        }
    }

    private void OnDoubleClick()
    {
        if (base.enabled && (this.trigger == AnimationOrTween.Trigger.OnDoubleClick))
        {
            this.Play(true);
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
            if (((this.trigger == AnimationOrTween.Trigger.OnHover) || ((this.trigger == AnimationOrTween.Trigger.OnHoverTrue) && isOver)) || ((this.trigger == AnimationOrTween.Trigger.OnHoverFalse) && !isOver))
            {
                this.Play(isOver);
            }
            this.mHighlighted = isOver;
        }
    }

    private void OnPress(bool isPressed)
    {
        if (base.enabled && (((this.trigger == AnimationOrTween.Trigger.OnPress) || ((this.trigger == AnimationOrTween.Trigger.OnPressTrue) && isPressed)) || ((this.trigger == AnimationOrTween.Trigger.OnPressFalse) && !isPressed)))
        {
            this.Play(isPressed);
        }
    }

    private void OnSelect(bool isSelected)
    {
        if (base.enabled && (((this.trigger == AnimationOrTween.Trigger.OnSelect) || ((this.trigger == AnimationOrTween.Trigger.OnSelectTrue) && isSelected)) || ((this.trigger == AnimationOrTween.Trigger.OnSelectFalse) && !isSelected)))
        {
            this.Play(true);
        }
    }

    private void Play(bool forward)
    {
        if (this.target == null)
        {
            this.target = base.GetComponentInChildren<Animation>();
        }
        if (this.target != null)
        {
            if (this.clearSelection && (UICamera.selectedObject == base.gameObject))
            {
                UICamera.selectedObject = null;
            }
            int num = -(int)this.playDirection;
            AnimationOrTween.Direction playDirection = !forward ? ((AnimationOrTween.Direction) num) : this.playDirection;
            ActiveAnimation animation = ActiveAnimation.Play(this.target, this.clipName, playDirection, this.ifDisabledOnPlay, this.disableWhenFinished);
            if (animation != null)
            {
                if (this.resetOnPlay)
                {
                    animation.Reset();
                }
                animation.onFinished = this.onFinished;
                if ((this.eventReceiver != null) && !string.IsNullOrEmpty(this.callWhenFinished))
                {
                    animation.eventReceiver = this.eventReceiver;
                    animation.callWhenFinished = this.callWhenFinished;
                }
                else
                {
                    animation.eventReceiver = null;
                }
            }
        }
    }

    private void Start()
    {
        this.mStarted = true;
    }
}

