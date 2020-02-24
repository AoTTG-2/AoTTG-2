using System;
using UnityEngine;

[AddComponentMenu("NGUI/UI/Image Button"), ExecuteInEditMode]
public class UIImageButton : MonoBehaviour
{
    public string disabledSprite;
    public string hoverSprite;
    public string normalSprite;
    public string pressedSprite;
    public UISprite target;

    private void Awake()
    {
        if (this.target == null)
        {
            this.target = base.GetComponentInChildren<UISprite>();
        }
    }

    private void OnEnable()
    {
        this.UpdateImage();
    }

    private void OnHover(bool isOver)
    {
        if (this.isEnabled && (this.target != null))
        {
            this.target.spriteName = !isOver ? this.normalSprite : this.hoverSprite;
            this.target.MakePixelPerfect();
        }
    }

    private void OnPress(bool pressed)
    {
        if (pressed)
        {
            this.target.spriteName = this.pressedSprite;
            this.target.MakePixelPerfect();
        }
        else
        {
            this.UpdateImage();
        }
    }

    private void UpdateImage()
    {
        if (this.target != null)
        {
            if (this.isEnabled)
            {
                this.target.spriteName = !UICamera.IsHighlighted(base.gameObject) ? this.normalSprite : this.hoverSprite;
            }
            else
            {
                this.target.spriteName = this.disabledSprite;
            }
            this.target.MakePixelPerfect();
        }
    }

    public bool isEnabled
    {
        get
        {
            Collider collider = base.GetComponent<Collider>();
            return ((collider != null) && collider.enabled);
        }
        set
        {
            Collider collider = base.GetComponent<Collider>();
            if ((collider != null) && (collider.enabled != value))
            {
                collider.enabled = value;
                this.UpdateImage();
            }
        }
    }
}

