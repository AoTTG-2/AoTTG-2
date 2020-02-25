using System;
using System.Runtime.CompilerServices;
using UnityEngine;

[AddComponentMenu("NGUI/Interaction/Draggable Panel"), ExecuteInEditMode, RequireComponent(typeof(UIPanel))]
public class UIDraggablePanel : IgnoreTimeScale
{
    public bool disableDragIfFits;
    public DragEffect dragEffect = DragEffect.MomentumAndSpring;
    public UIScrollBar horizontalScrollBar;
    public bool iOSDragEmulation = true;
    private Bounds mBounds;
    private bool mCalculatedBounds;
    private int mDragID = -10;
    private bool mDragStarted;
    private Vector2 mDragStartOffset = Vector2.zero;
    private bool mIgnoreCallbacks;
    private Vector3 mLastPos;
    private Vector3 mMomentum = Vector3.zero;
    public float momentumAmount = 35f;
    private UIPanel mPanel;
    private Plane mPlane;
    private bool mPressed;
    private float mScroll;
    private bool mShouldMove;
    private Transform mTrans;
    public OnDragFinished onDragFinished;
    public Vector2 relativePositionOnReset = Vector2.zero;
    public bool repositionClipping;
    public bool restrictWithinPanel = true;
    public Vector3 scale = Vector3.one;
    public float scrollWheelFactor;
    public ShowCondition showScrollBars = ShowCondition.OnlyIfNeeded;
    public bool smoothDragStart = true;
    public UIScrollBar verticalScrollBar;

    private void Awake()
    {
        this.mTrans = base.transform;
        this.mPanel = base.GetComponent<UIPanel>();
        this.mPanel.onChange = (UIPanel.OnChangeDelegate) Delegate.Combine(this.mPanel.onChange, new UIPanel.OnChangeDelegate(this.OnPanelChange));
    }

    public void DisableSpring()
    {
        SpringPanel component = base.GetComponent<SpringPanel>();
        if (component != null)
        {
            component.enabled = false;
        }
    }

    public void Drag()
    {
        if ((base.enabled && NGUITools.GetActive(base.gameObject)) && this.mShouldMove)
        {
            if (this.mDragID == -10)
            {
                this.mDragID = UICamera.currentTouchID;
            }
            UICamera.currentTouch.clickNotification = UICamera.ClickNotification.BasedOnDelta;
            if (this.smoothDragStart && !this.mDragStarted)
            {
                this.mDragStarted = true;
                this.mDragStartOffset = UICamera.currentTouch.totalDelta;
            }
            Ray ray = !this.smoothDragStart ? UICamera.currentCamera.ScreenPointToRay((Vector3) UICamera.currentTouch.pos) : UICamera.currentCamera.ScreenPointToRay((Vector3) (UICamera.currentTouch.pos - this.mDragStartOffset));
            float enter = 0f;
            if (this.mPlane.Raycast(ray, out enter))
            {
                Vector3 point = ray.GetPoint(enter);
                Vector3 direction = point - this.mLastPos;
                this.mLastPos = point;
                if ((direction.x != 0f) || (direction.y != 0f))
                {
                    direction = this.mTrans.InverseTransformDirection(direction);
                    direction.Scale(this.scale);
                    direction = this.mTrans.TransformDirection(direction);
                }
                this.mMomentum = Vector3.Lerp(this.mMomentum, this.mMomentum + ((Vector3) (direction * (0.01f * this.momentumAmount))), 0.67f);
                if (!this.iOSDragEmulation)
                {
                    this.MoveAbsolute(direction);
                }
                else if (this.mPanel.CalculateConstrainOffset(this.bounds.min, this.bounds.max).magnitude > 0.001f)
                {
                    this.MoveAbsolute((Vector3) (direction * 0.5f));
                    this.mMomentum = (Vector3) (this.mMomentum * 0.5f);
                }
                else
                {
                    this.MoveAbsolute(direction);
                }
                if ((this.restrictWithinPanel && (this.mPanel.clipping != UIDrawCall.Clipping.None)) && (this.dragEffect != DragEffect.MomentumAndSpring))
                {
                    this.RestrictWithinBounds(true);
                }
            }
        }
    }

    private void LateUpdate()
    {
        if (this.repositionClipping)
        {
            this.repositionClipping = false;
            this.mCalculatedBounds = false;
            this.SetDragAmount(this.relativePositionOnReset.x, this.relativePositionOnReset.y, true);
        }
        if (Application.isPlaying)
        {
            float deltaTime = base.UpdateRealTimeDelta();
            if (this.showScrollBars != ShowCondition.Always)
            {
                bool shouldMoveVertically = false;
                bool shouldMoveHorizontally = false;
                if (((this.showScrollBars != ShowCondition.WhenDragging) || (this.mDragID != -10)) || (this.mMomentum.magnitude > 0.01f))
                {
                    shouldMoveVertically = this.shouldMoveVertically;
                    shouldMoveHorizontally = this.shouldMoveHorizontally;
                }
                if (this.verticalScrollBar != null)
                {
                    float num2 = this.verticalScrollBar.alpha + (!shouldMoveVertically ? (-deltaTime * 3f) : (deltaTime * 6f));
                    num2 = Mathf.Clamp01(num2);
                    if (this.verticalScrollBar.alpha != num2)
                    {
                        this.verticalScrollBar.alpha = num2;
                    }
                }
                if (this.horizontalScrollBar != null)
                {
                    float num3 = this.horizontalScrollBar.alpha + (!shouldMoveHorizontally ? (-deltaTime * 3f) : (deltaTime * 6f));
                    num3 = Mathf.Clamp01(num3);
                    if (this.horizontalScrollBar.alpha != num3)
                    {
                        this.horizontalScrollBar.alpha = num3;
                    }
                }
            }
            if (this.mShouldMove && !this.mPressed)
            {
                this.mMomentum -= (Vector3) (this.scale * (this.mScroll * 0.05f));
                if (this.mMomentum.magnitude > 0.0001f)
                {
                    this.mScroll = NGUIMath.SpringLerp(this.mScroll, 0f, 20f, deltaTime);
                    Vector3 absolute = NGUIMath.SpringDampen(ref this.mMomentum, 9f, deltaTime);
                    this.MoveAbsolute(absolute);
                    if (this.restrictWithinPanel && (this.mPanel.clipping != UIDrawCall.Clipping.None))
                    {
                        this.RestrictWithinBounds(false);
                    }
                    if ((this.mMomentum.magnitude < 0.0001f) && (this.onDragFinished != null))
                    {
                        this.onDragFinished();
                    }
                    return;
                }
                this.mScroll = 0f;
                this.mMomentum = Vector3.zero;
            }
            else
            {
                this.mScroll = 0f;
            }
            NGUIMath.SpringDampen(ref this.mMomentum, 9f, deltaTime);
        }
    }

    public void MoveAbsolute(Vector3 absolute)
    {
        Vector3 vector = this.mTrans.InverseTransformPoint(absolute);
        Vector3 vector2 = this.mTrans.InverseTransformPoint(Vector3.zero);
        this.MoveRelative(vector - vector2);
    }

    public void MoveRelative(Vector3 relative)
    {
        this.mTrans.localPosition += relative;
        Vector4 clipRange = this.mPanel.clipRange;
        clipRange.x -= relative.x;
        clipRange.y -= relative.y;
        this.mPanel.clipRange = clipRange;
        this.UpdateScrollbars(false);
    }

    private void OnDestroy()
    {
        if (this.mPanel != null)
        {
            this.mPanel.onChange = (UIPanel.OnChangeDelegate) Delegate.Remove(this.mPanel.onChange, new UIPanel.OnChangeDelegate(this.OnPanelChange));
        }
    }

    private void OnHorizontalBar(UIScrollBar sb)
    {
        if (!this.mIgnoreCallbacks)
        {
            float x = (this.horizontalScrollBar == null) ? 0f : this.horizontalScrollBar.scrollValue;
            float y = (this.verticalScrollBar == null) ? 0f : this.verticalScrollBar.scrollValue;
            this.SetDragAmount(x, y, false);
        }
    }

    private void OnPanelChange()
    {
        this.UpdateScrollbars(true);
    }

    private void OnVerticalBar(UIScrollBar sb)
    {
        if (!this.mIgnoreCallbacks)
        {
            float x = (this.horizontalScrollBar == null) ? 0f : this.horizontalScrollBar.scrollValue;
            float y = (this.verticalScrollBar == null) ? 0f : this.verticalScrollBar.scrollValue;
            this.SetDragAmount(x, y, false);
        }
    }

    public void Press(bool pressed)
    {
        if (this.smoothDragStart && pressed)
        {
            this.mDragStarted = false;
            this.mDragStartOffset = Vector2.zero;
        }
        if (base.enabled && NGUITools.GetActive(base.gameObject))
        {
            if (!pressed && (this.mDragID == UICamera.currentTouchID))
            {
                this.mDragID = -10;
            }
            this.mCalculatedBounds = false;
            this.mShouldMove = this.shouldMove;
            if (this.mShouldMove)
            {
                this.mPressed = pressed;
                if (pressed)
                {
                    this.mMomentum = Vector3.zero;
                    this.mScroll = 0f;
                    this.DisableSpring();
                    this.mLastPos = UICamera.lastHit.point;
                    this.mPlane = new Plane((Vector3) (this.mTrans.rotation * Vector3.back), this.mLastPos);
                }
                else
                {
                    if ((this.restrictWithinPanel && (this.mPanel.clipping != UIDrawCall.Clipping.None)) && (this.dragEffect == DragEffect.MomentumAndSpring))
                    {
                        this.RestrictWithinBounds(false);
                    }
                    if (this.onDragFinished != null)
                    {
                        this.onDragFinished();
                    }
                }
            }
        }
    }

    public void ResetPosition()
    {
        this.mCalculatedBounds = false;
        this.SetDragAmount(this.relativePositionOnReset.x, this.relativePositionOnReset.y, false);
        this.SetDragAmount(this.relativePositionOnReset.x, this.relativePositionOnReset.y, true);
    }

    public bool RestrictWithinBounds(bool instant)
    {
        Vector3 relative = this.mPanel.CalculateConstrainOffset(this.bounds.min, this.bounds.max);
        if (relative.magnitude <= 0.001f)
        {
            return false;
        }
        if (!instant && (this.dragEffect == DragEffect.MomentumAndSpring))
        {
            SpringPanel.Begin(this.mPanel.gameObject, this.mTrans.localPosition + relative, 13f);
        }
        else
        {
            this.MoveRelative(relative);
            this.mMomentum = Vector3.zero;
            this.mScroll = 0f;
        }
        return true;
    }

    public void Scroll(float delta)
    {
        if ((base.enabled && NGUITools.GetActive(base.gameObject)) && (this.scrollWheelFactor != 0f))
        {
            this.DisableSpring();
            this.mShouldMove = this.shouldMove;
            if (Mathf.Sign(this.mScroll) != Mathf.Sign(delta))
            {
                this.mScroll = 0f;
            }
            this.mScroll += delta * this.scrollWheelFactor;
        }
    }

    public void SetDragAmount(float x, float y, bool updateScrollbars)
    {
        this.DisableSpring();
        Bounds bounds = this.bounds;
        if ((bounds.min.x != bounds.max.x) && (bounds.min.y != bounds.max.y))
        {
            Vector4 clipRange = this.mPanel.clipRange;
            float num = clipRange.z * 0.5f;
            float num2 = clipRange.w * 0.5f;
            float from = bounds.min.x + num;
            float to = bounds.max.x - num;
            float num5 = bounds.min.y + num2;
            float num6 = bounds.max.y - num2;
            if (this.mPanel.clipping == UIDrawCall.Clipping.SoftClip)
            {
                from -= this.mPanel.clipSoftness.x;
                to += this.mPanel.clipSoftness.x;
                num5 -= this.mPanel.clipSoftness.y;
                num6 += this.mPanel.clipSoftness.y;
            }
            float num7 = Mathf.Lerp(from, to, x);
            float num8 = Mathf.Lerp(num6, num5, y);
            if (!updateScrollbars)
            {
                Vector3 localPosition = this.mTrans.localPosition;
                if (this.scale.x != 0f)
                {
                    localPosition.x += clipRange.x - num7;
                }
                if (this.scale.y != 0f)
                {
                    localPosition.y += clipRange.y - num8;
                }
                this.mTrans.localPosition = localPosition;
            }
            clipRange.x = num7;
            clipRange.y = num8;
            this.mPanel.clipRange = clipRange;
            if (updateScrollbars)
            {
                this.UpdateScrollbars(false);
            }
        }
    }

    private void Start()
    {
        this.UpdateScrollbars(true);
        if (this.horizontalScrollBar != null)
        {
            this.horizontalScrollBar.onChange = (UIScrollBar.OnScrollBarChange) Delegate.Combine(this.horizontalScrollBar.onChange, new UIScrollBar.OnScrollBarChange(this.OnHorizontalBar));
            this.horizontalScrollBar.alpha = ((this.showScrollBars == ShowCondition.Always) || this.shouldMoveHorizontally) ? 1f : 0f;
        }
        if (this.verticalScrollBar != null)
        {
            this.verticalScrollBar.onChange = (UIScrollBar.OnScrollBarChange) Delegate.Combine(this.verticalScrollBar.onChange, new UIScrollBar.OnScrollBarChange(this.OnVerticalBar));
            this.verticalScrollBar.alpha = ((this.showScrollBars == ShowCondition.Always) || this.shouldMoveVertically) ? 1f : 0f;
        }
    }

    public void UpdateScrollbars(bool recalculateBounds)
    {
        if (this.mPanel != null)
        {
            if ((this.horizontalScrollBar == null) && (this.verticalScrollBar == null))
            {
                if (recalculateBounds)
                {
                    this.mCalculatedBounds = false;
                }
            }
            else
            {
                if (recalculateBounds)
                {
                    this.mCalculatedBounds = false;
                    this.mShouldMove = this.shouldMove;
                }
                Bounds bounds = this.bounds;
                Vector2 min = bounds.min;
                Vector2 max = bounds.max;
                if (this.mPanel.clipping == UIDrawCall.Clipping.SoftClip)
                {
                    Vector2 clipSoftness = this.mPanel.clipSoftness;
                    min -= clipSoftness;
                    max += clipSoftness;
                }
                if ((this.horizontalScrollBar != null) && (max.x > min.x))
                {
                    Vector4 clipRange = this.mPanel.clipRange;
                    float num = clipRange.z * 0.5f;
                    float num2 = (clipRange.x - num) - bounds.min.x;
                    float num3 = (bounds.max.x - num) - clipRange.x;
                    float num4 = max.x - min.x;
                    num2 = Mathf.Clamp01(num2 / num4);
                    num3 = Mathf.Clamp01(num3 / num4);
                    float num5 = num2 + num3;
                    this.mIgnoreCallbacks = true;
                    this.horizontalScrollBar.barSize = 1f - num5;
                    this.horizontalScrollBar.scrollValue = (num5 <= 0.001f) ? 0f : (num2 / num5);
                    this.mIgnoreCallbacks = false;
                }
                if ((this.verticalScrollBar != null) && (max.y > min.y))
                {
                    Vector4 vector7 = this.mPanel.clipRange;
                    float num6 = vector7.w * 0.5f;
                    float num7 = (vector7.y - num6) - min.y;
                    float num8 = (max.y - num6) - vector7.y;
                    float num9 = max.y - min.y;
                    num7 = Mathf.Clamp01(num7 / num9);
                    num8 = Mathf.Clamp01(num8 / num9);
                    float num10 = num7 + num8;
                    this.mIgnoreCallbacks = true;
                    this.verticalScrollBar.barSize = 1f - num10;
                    this.verticalScrollBar.scrollValue = (num10 <= 0.001f) ? 0f : (1f - (num7 / num10));
                    this.mIgnoreCallbacks = false;
                }
            }
        }
    }

    public Bounds bounds
    {
        get
        {
            if (!this.mCalculatedBounds)
            {
                this.mCalculatedBounds = true;
                this.mBounds = NGUIMath.CalculateRelativeWidgetBounds(this.mTrans, this.mTrans);
            }
            return this.mBounds;
        }
    }

    public Vector3 currentMomentum
    {
        get
        {
            return this.mMomentum;
        }
        set
        {
            this.mMomentum = value;
            this.mShouldMove = true;
        }
    }

    public UIPanel panel
    {
        get
        {
            return this.mPanel;
        }
    }

    private bool shouldMove
    {
        get
        {
            if (!this.disableDragIfFits)
            {
                return true;
            }
            if (this.mPanel == null)
            {
                this.mPanel = base.GetComponent<UIPanel>();
            }
            Vector4 clipRange = this.mPanel.clipRange;
            Bounds bounds = this.bounds;
            float num = (clipRange.z != 0f) ? (clipRange.z * 0.5f) : ((float) Screen.width);
            float num2 = (clipRange.w != 0f) ? (clipRange.w * 0.5f) : ((float) Screen.height);
            if (!Mathf.Approximately(this.scale.x, 0f))
            {
                if (bounds.min.x < (clipRange.x - num))
                {
                    return true;
                }
                if (bounds.max.x > (clipRange.x + num))
                {
                    return true;
                }
            }
            if (!Mathf.Approximately(this.scale.y, 0f))
            {
                if (bounds.min.y < (clipRange.y - num2))
                {
                    return true;
                }
                if (bounds.max.y > (clipRange.y + num2))
                {
                    return true;
                }
            }
            return false;
        }
    }

    public bool shouldMoveHorizontally
    {
        get
        {
            float x = this.bounds.size.x;
            if (this.mPanel.clipping == UIDrawCall.Clipping.SoftClip)
            {
                x += this.mPanel.clipSoftness.x * 2f;
            }
            return (x > this.mPanel.clipRange.z);
        }
    }

    public bool shouldMoveVertically
    {
        get
        {
            float y = this.bounds.size.y;
            if (this.mPanel.clipping == UIDrawCall.Clipping.SoftClip)
            {
                y += this.mPanel.clipSoftness.y * 2f;
            }
            return (y > this.mPanel.clipRange.w);
        }
    }

    public enum DragEffect
    {
        None,
        Momentum,
        MomentumAndSpring
    }

    public delegate void OnDragFinished();

    public enum ShowCondition
    {
        Always,
        OnlyIfNeeded,
        WhenDragging
    }
}

