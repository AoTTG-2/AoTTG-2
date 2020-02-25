using System;
using UnityEngine;

[AddComponentMenu("NGUI/Interaction/Forward Events")]
public class UIForwardEvents : MonoBehaviour
{
    public bool onClick;
    public bool onDoubleClick;
    public bool onDrag;
    public bool onDrop;
    public bool onHover;
    public bool onInput;
    public bool onPress;
    public bool onScroll;
    public bool onSelect;
    public bool onSubmit;
    public GameObject target;

    private void OnClick()
    {
        if (this.onClick && (this.target != null))
        {
            this.target.SendMessage("OnClick", SendMessageOptions.DontRequireReceiver);
        }
    }

    private void OnDoubleClick()
    {
        if (this.onDoubleClick && (this.target != null))
        {
            this.target.SendMessage("OnDoubleClick", SendMessageOptions.DontRequireReceiver);
        }
    }

    private void OnDrag(Vector2 delta)
    {
        if (this.onDrag && (this.target != null))
        {
            this.target.SendMessage("OnDrag", delta, SendMessageOptions.DontRequireReceiver);
        }
    }

    private void OnDrop(GameObject go)
    {
        if (this.onDrop && (this.target != null))
        {
            this.target.SendMessage("OnDrop", go, SendMessageOptions.DontRequireReceiver);
        }
    }

    private void OnHover(bool isOver)
    {
        if (this.onHover && (this.target != null))
        {
            this.target.SendMessage("OnHover", isOver, SendMessageOptions.DontRequireReceiver);
        }
    }

    private void OnInput(string text)
    {
        if (this.onInput && (this.target != null))
        {
            this.target.SendMessage("OnInput", text, SendMessageOptions.DontRequireReceiver);
        }
    }

    private void OnPress(bool pressed)
    {
        if (this.onPress && (this.target != null))
        {
            this.target.SendMessage("OnPress", pressed, SendMessageOptions.DontRequireReceiver);
        }
    }

    private void OnScroll(float delta)
    {
        if (this.onScroll && (this.target != null))
        {
            this.target.SendMessage("OnScroll", delta, SendMessageOptions.DontRequireReceiver);
        }
    }

    private void OnSelect(bool selected)
    {
        if (this.onSelect && (this.target != null))
        {
            this.target.SendMessage("OnSelect", selected, SendMessageOptions.DontRequireReceiver);
        }
    }

    private void OnSubmit()
    {
        if (this.onSubmit && (this.target != null))
        {
            this.target.SendMessage("OnSubmit", SendMessageOptions.DontRequireReceiver);
        }
    }
}

