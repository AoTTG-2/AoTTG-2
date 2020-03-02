using System;
using UnityEngine;

[RequireComponent(typeof(Collider)), AddComponentMenu("NGUI/Interaction/Button Keys")]
public class UIButtonKeys : MonoBehaviour
{
    public UIButtonKeys selectOnClick;
    public UIButtonKeys selectOnDown;
    public UIButtonKeys selectOnLeft;
    public UIButtonKeys selectOnRight;
    public UIButtonKeys selectOnUp;
    public bool startsSelected;

    private void OnClick()
    {
        if (base.enabled && (this.selectOnClick != null))
        {
            UICamera.selectedObject = this.selectOnClick.gameObject;
        }
    }

    private void OnKey(KeyCode key)
    {
        if (base.enabled && NGUITools.GetActive(base.gameObject))
        {
            switch (key)
            {
                case KeyCode.UpArrow:
                    if (this.selectOnUp != null)
                    {
                        UICamera.selectedObject = this.selectOnUp.gameObject;
                    }
                    break;

                case KeyCode.DownArrow:
                    if (this.selectOnDown != null)
                    {
                        UICamera.selectedObject = this.selectOnDown.gameObject;
                    }
                    break;

                case KeyCode.RightArrow:
                    if (this.selectOnRight != null)
                    {
                        UICamera.selectedObject = this.selectOnRight.gameObject;
                    }
                    break;

                case KeyCode.LeftArrow:
                    if (this.selectOnLeft != null)
                    {
                        UICamera.selectedObject = this.selectOnLeft.gameObject;
                    }
                    break;

                case KeyCode.Tab:
                    if (!Input.GetKey(KeyCode.LeftShift) && !Input.GetKey(KeyCode.RightShift))
                    {
                        if (this.selectOnRight != null)
                        {
                            UICamera.selectedObject = this.selectOnRight.gameObject;
                        }
                        else if (this.selectOnDown != null)
                        {
                            UICamera.selectedObject = this.selectOnDown.gameObject;
                        }
                        else if (this.selectOnUp != null)
                        {
                            UICamera.selectedObject = this.selectOnUp.gameObject;
                        }
                        else if (this.selectOnLeft != null)
                        {
                            UICamera.selectedObject = this.selectOnLeft.gameObject;
                        }
                    }
                    else if (this.selectOnLeft != null)
                    {
                        UICamera.selectedObject = this.selectOnLeft.gameObject;
                    }
                    else if (this.selectOnUp != null)
                    {
                        UICamera.selectedObject = this.selectOnUp.gameObject;
                    }
                    else if (this.selectOnDown != null)
                    {
                        UICamera.selectedObject = this.selectOnDown.gameObject;
                    }
                    else if (this.selectOnRight != null)
                    {
                        UICamera.selectedObject = this.selectOnRight.gameObject;
                    }
                    break;
            }
        }
    }

    private void Start()
    {
        if (this.startsSelected && ((UICamera.selectedObject == null) || !NGUITools.GetActive(UICamera.selectedObject)))
        {
            UICamera.selectedObject = base.gameObject;
        }
    }
}

