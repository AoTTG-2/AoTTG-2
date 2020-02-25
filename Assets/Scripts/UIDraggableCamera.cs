using System;
using UnityEngine;

[RequireComponent(typeof(Camera)), AddComponentMenu("NGUI/Interaction/Draggable Camera")]
public class UIDraggableCamera : IgnoreTimeScale
{
    public UIDragObject.DragEffect dragEffect = UIDragObject.DragEffect.MomentumAndSpring;
    private Bounds mBounds;
    private Camera mCam;
    private bool mDragStarted;
    private Vector2 mMomentum = Vector2.zero;
    public float momentumAmount = 35f;
    private bool mPressed;
    private UIRoot mRoot;
    private float mScroll;
    private Transform mTrans;
    public Transform rootForBounds;
    public Vector2 scale = Vector2.one;
    public float scrollWheelFactor;
    public bool smoothDragStart = true;

    private void Awake()
    {
        this.mCam = base.GetComponent<Camera>();
        this.mTrans = base.transform;
        if (this.rootForBounds == null)
        {
            Debug.LogError(NGUITools.GetHierarchy(base.gameObject) + " needs the 'Root For Bounds' parameter to be set", this);
            base.enabled = false;
        }
    }

    private Vector3 CalculateConstrainOffset()
    {
        if ((this.rootForBounds != null) && (this.rootForBounds.childCount != 0))
        {
            Vector3 position = new Vector3(this.mCam.rect.xMin * Screen.width, this.mCam.rect.yMin * Screen.height, 0f);
            Vector3 vector2 = new Vector3(this.mCam.rect.xMax * Screen.width, this.mCam.rect.yMax * Screen.height, 0f);
            position = this.mCam.ScreenToWorldPoint(position);
            vector2 = this.mCam.ScreenToWorldPoint(vector2);
            Vector2 minRect = new Vector2(this.mBounds.min.x, this.mBounds.min.y);
            Vector2 maxRect = new Vector2(this.mBounds.max.x, this.mBounds.max.y);
            return (Vector3) NGUIMath.ConstrainRect(minRect, maxRect, position, vector2);
        }
        return Vector3.zero;
    }

    public bool ConstrainToBounds(bool immediate)
    {
        if ((this.mTrans != null) && (this.rootForBounds != null))
        {
            Vector3 vector = this.CalculateConstrainOffset();
            if (vector.magnitude > 0f)
            {
                if (immediate)
                {
                    this.mTrans.position -= vector;
                }
                else
                {
                    SpringPosition position = SpringPosition.Begin(base.gameObject, this.mTrans.position - vector, 13f);
                    position.ignoreTimeScale = true;
                    position.worldSpace = true;
                }
                return true;
            }
        }
        return false;
    }

    public void Drag(Vector2 delta)
    {
        if (this.smoothDragStart && !this.mDragStarted)
        {
            this.mDragStarted = true;
        }
        else
        {
            UICamera.currentTouch.clickNotification = UICamera.ClickNotification.BasedOnDelta;
            if (this.mRoot != null)
            {
                delta = (Vector2) (delta * this.mRoot.pixelSizeAdjustment);
            }
            Vector2 vector = Vector2.Scale(delta, -this.scale);
            this.mTrans.localPosition += (Vector3)vector;
            this.mMomentum = Vector2.Lerp(this.mMomentum, this.mMomentum + ((Vector2) (vector * (0.01f * this.momentumAmount))), 0.67f);
            if ((this.dragEffect != UIDragObject.DragEffect.MomentumAndSpring) && this.ConstrainToBounds(true))
            {
                this.mMomentum = Vector2.zero;
                this.mScroll = 0f;
            }
        }
    }

    public void Press(bool isPressed)
    {
        if (isPressed)
        {
            this.mDragStarted = false;
        }
        if (this.rootForBounds != null)
        {
            this.mPressed = isPressed;
            if (isPressed)
            {
                this.mBounds = NGUIMath.CalculateAbsoluteWidgetBounds(this.rootForBounds);
                this.mMomentum = Vector2.zero;
                this.mScroll = 0f;
                SpringPosition component = base.GetComponent<SpringPosition>();
                if (component != null)
                {
                    component.enabled = false;
                }
            }
            else if (this.dragEffect == UIDragObject.DragEffect.MomentumAndSpring)
            {
                this.ConstrainToBounds(false);
            }
        }
    }

    public void Scroll(float delta)
    {
        if (base.enabled && NGUITools.GetActive(base.gameObject))
        {
            if (Mathf.Sign(this.mScroll) != Mathf.Sign(delta))
            {
                this.mScroll = 0f;
            }
            this.mScroll += delta * this.scrollWheelFactor;
        }
    }

    private void Start()
    {
        this.mRoot = NGUITools.FindInParents<UIRoot>(base.gameObject);
    }

    private void Update()
    {
        float deltaTime = base.UpdateRealTimeDelta();
        if (this.mPressed)
        {
            SpringPosition component = base.GetComponent<SpringPosition>();
            if (component != null)
            {
                component.enabled = false;
            }
            this.mScroll = 0f;
        }
        else
        {
            this.mMomentum += (Vector2) (this.scale * (this.mScroll * 20f));
            this.mScroll = NGUIMath.SpringLerp(this.mScroll, 0f, 20f, deltaTime);
            if (this.mMomentum.magnitude > 0.01f)
            {
                this.mTrans.localPosition += (Vector3)NGUIMath.SpringDampen(ref this.mMomentum, 9f, deltaTime);
                this.mBounds = NGUIMath.CalculateAbsoluteWidgetBounds(this.rootForBounds);
                if (!this.ConstrainToBounds(this.dragEffect == UIDragObject.DragEffect.None))
                {
                    SpringPosition position2 = base.GetComponent<SpringPosition>();
                    if (position2 != null)
                    {
                        position2.enabled = false;
                    }
                }
                return;
            }
            this.mScroll = 0f;
        }
        NGUIMath.SpringDampen(ref this.mMomentum, 9f, deltaTime);
    }

    public Vector2 currentMomentum
    {
        get
        {
            return this.mMomentum;
        }
        set
        {
            this.mMomentum = value;
        }
    }
}

