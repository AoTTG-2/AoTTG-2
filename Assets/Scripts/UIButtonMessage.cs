using System;
using UnityEngine;

[AddComponentMenu("NGUI/Interaction/Button Message")]
public class UIButtonMessage : MonoBehaviour
{
    public string functionName;
    public bool includeChildren;
    private bool mHighlighted;
    private bool mStarted;
    public GameObject target;
    public Trigger trigger;

    private void OnClick()
    {
        if (base.enabled && (this.trigger == Trigger.OnClick))
        {
            this.Send();
        }
    }

    private void OnDoubleClick()
    {
        if (base.enabled && (this.trigger == Trigger.OnDoubleClick))
        {
            this.Send();
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
            if ((isOver && (this.trigger == Trigger.OnMouseOver)) || (!isOver && (this.trigger == Trigger.OnMouseOut)))
            {
                this.Send();
            }
            this.mHighlighted = isOver;
        }
    }

    private void OnPress(bool isPressed)
    {
        if (base.enabled && ((isPressed && (this.trigger == Trigger.OnPress)) || (!isPressed && (this.trigger == Trigger.OnRelease))))
        {
            this.Send();
        }
    }

    private void Send()
    {
        if (!string.IsNullOrEmpty(this.functionName))
        {
            if (this.target == null)
            {
                this.target = base.gameObject;
            }
            if (this.includeChildren)
            {
                Transform[] componentsInChildren = this.target.GetComponentsInChildren<Transform>();
                int index = 0;
                int length = componentsInChildren.Length;
                while (index < length)
                {
                    Transform transform = componentsInChildren[index];
                    transform.gameObject.SendMessage(this.functionName, base.gameObject, SendMessageOptions.DontRequireReceiver);
                    index++;
                }
            }
            else
            {
                this.target.SendMessage(this.functionName, base.gameObject, SendMessageOptions.DontRequireReceiver);
            }
        }
    }

    private void Start()
    {
        this.mStarted = true;
    }

    public enum Trigger
    {
        OnClick,
        OnMouseOver,
        OnMouseOut,
        OnPress,
        OnRelease,
        OnDoubleClick
    }
}

