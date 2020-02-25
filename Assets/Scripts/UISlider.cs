using System;
using System.Runtime.CompilerServices;
using UnityEngine;

[AddComponentMenu("NGUI/Interaction/Slider")]
public class UISlider : IgnoreTimeScale
{
    public static UISlider current;
    public Direction direction;
    public GameObject eventReceiver;
    public Transform foreground;
    public string functionName = "OnSliderChange";
    private Vector2 mCenter = Vector3.zero;
    private BoxCollider mCol;
    private UISprite mFGFilled;
    private Transform mFGTrans;
    private UIWidget mFGWidget;
    private bool mInitDone;
    private Vector2 mSize = Vector2.zero;
    private Transform mTrans;
    public int numberOfSteps;
    public OnValueChange onValueChange;
    [SerializeField, HideInInspector]
    private float rawValue = 1f;
    public Transform thumb;

    private void Awake()
    {
        this.mTrans = base.transform;
        this.mCol = base.GetComponent<Collider>() as BoxCollider;
    }

    public void ForceUpdate()
    {
        this.Set(this.rawValue, true);
    }

    private void Init()
    {
        this.mInitDone = true;
        if (this.foreground != null)
        {
            this.mFGWidget = this.foreground.GetComponent<UIWidget>();
            this.mFGFilled = (this.mFGWidget == null) ? null : (this.mFGWidget as UISprite);
            this.mFGTrans = this.foreground.transform;
            if (this.mSize == Vector2.zero)
            {
                this.mSize = this.foreground.localScale;
            }
            if (this.mCenter == Vector2.zero)
            {
                this.mCenter = (Vector2) (this.foreground.localPosition + (this.foreground.localScale * 0.5f));
            }
        }
        else if (this.mCol != null)
        {
            if (this.mSize == Vector2.zero)
            {
                this.mSize = this.mCol.size;
            }
            if (this.mCenter == Vector2.zero)
            {
                this.mCenter = this.mCol.center;
            }
        }
        else
        {
            Debug.LogWarning("UISlider expected to find a foreground object or a box collider to work with", this);
        }
    }

    private void OnDrag(Vector2 delta)
    {
        this.UpdateDrag();
    }

    private void OnDragThumb(GameObject go, Vector2 delta)
    {
        this.UpdateDrag();
    }

    private void OnKey(KeyCode key)
    {
        float num = (this.numberOfSteps <= 1f) ? 0.125f : (1f / ((float) (this.numberOfSteps - 1)));
        if (this.direction == Direction.Horizontal)
        {
            if (key == KeyCode.LeftArrow)
            {
                this.Set(this.rawValue - num, false);
            }
            else if (key == KeyCode.RightArrow)
            {
                this.Set(this.rawValue + num, false);
            }
        }
        else if (key == KeyCode.DownArrow)
        {
            this.Set(this.rawValue - num, false);
        }
        else if (key == KeyCode.UpArrow)
        {
            this.Set(this.rawValue + num, false);
        }
    }

    private void OnPress(bool pressed)
    {
        if (pressed && (UICamera.currentTouchID != -100))
        {
            this.UpdateDrag();
        }
    }

    private void OnPressThumb(GameObject go, bool pressed)
    {
        if (pressed)
        {
            this.UpdateDrag();
        }
    }

    private void Set(float input, bool force)
    {
        if (!this.mInitDone)
        {
            this.Init();
        }
        float num = Mathf.Clamp01(input);
        if (num < 0.001f)
        {
            num = 0f;
        }
        float sliderValue = this.sliderValue;
        this.rawValue = num;
        float num3 = this.sliderValue;
        if (force || (sliderValue != num3))
        {
            Vector3 mSize = (Vector3) this.mSize;
            if (this.direction == Direction.Horizontal)
            {
                mSize.x *= num3;
            }
            else
            {
                mSize.y *= num3;
            }
            if ((this.mFGFilled != null) && (this.mFGFilled.type == UISprite.Type.Filled))
            {
                this.mFGFilled.fillAmount = num3;
            }
            else if (this.foreground != null)
            {
                this.mFGTrans.localScale = mSize;
                if (this.mFGWidget != null)
                {
                    if (num3 > 0.001f)
                    {
                        this.mFGWidget.enabled = true;
                        this.mFGWidget.MarkAsChanged();
                    }
                    else
                    {
                        this.mFGWidget.enabled = false;
                    }
                }
            }
            if (this.thumb != null)
            {
                Vector3 localPosition = this.thumb.localPosition;
                if ((this.mFGFilled != null) && (this.mFGFilled.type == UISprite.Type.Filled))
                {
                    if (this.mFGFilled.fillDirection == UISprite.FillDirection.Horizontal)
                    {
                        localPosition.x = !this.mFGFilled.invert ? mSize.x : (this.mSize.x - mSize.x);
                    }
                    else if (this.mFGFilled.fillDirection == UISprite.FillDirection.Vertical)
                    {
                        localPosition.y = !this.mFGFilled.invert ? mSize.y : (this.mSize.y - mSize.y);
                    }
                    else
                    {
                        Debug.LogWarning("Slider thumb is only supported with Horizontal or Vertical fill direction", this);
                    }
                }
                else if (this.direction == Direction.Horizontal)
                {
                    localPosition.x = mSize.x;
                }
                else
                {
                    localPosition.y = mSize.y;
                }
                this.thumb.localPosition = localPosition;
            }
            current = this;
            if (((this.eventReceiver != null) && !string.IsNullOrEmpty(this.functionName)) && Application.isPlaying)
            {
                this.eventReceiver.SendMessage(this.functionName, num3, SendMessageOptions.DontRequireReceiver);
            }
            if (this.onValueChange != null)
            {
                this.onValueChange(num3);
            }
            current = null;
        }
    }

    private void Start()
    {
        this.Init();
        if ((Application.isPlaying && (this.thumb != null)) && (this.thumb.GetComponent<Collider>() != null))
        {
            UIEventListener listener = UIEventListener.Get(this.thumb.gameObject);
            listener.onPress = (UIEventListener.BoolDelegate) Delegate.Combine(listener.onPress, new UIEventListener.BoolDelegate(this.OnPressThumb));
            listener.onDrag = (UIEventListener.VectorDelegate) Delegate.Combine(listener.onDrag, new UIEventListener.VectorDelegate(this.OnDragThumb));
        }
        this.Set(this.rawValue, true);
    }

    private void UpdateDrag()
    {
        if (((this.mCol != null) && (UICamera.currentCamera != null)) && (UICamera.currentTouch != null))
        {
            float num;
            UICamera.currentTouch.clickNotification = UICamera.ClickNotification.None;
            Ray ray = UICamera.currentCamera.ScreenPointToRay((Vector3) UICamera.currentTouch.pos);
            Plane plane = new Plane((Vector3) (this.mTrans.rotation * Vector3.back), this.mTrans.position);
            if (plane.Raycast(ray, out num))
            {
                Vector3 vector = this.mTrans.localPosition + ((Vector3) (this.mCenter - (this.mSize * 0.5f)));
                Vector3 vector2 = this.mTrans.localPosition - vector;
                Vector3 vector4 = this.mTrans.InverseTransformPoint(ray.GetPoint(num)) + vector2;
                this.Set((this.direction != Direction.Horizontal) ? (vector4.y / this.mSize.y) : (vector4.x / this.mSize.x), false);
            }
        }
    }

    public Vector2 fullSize
    {
        get
        {
            return this.mSize;
        }
        set
        {
            if (this.mSize != value)
            {
                this.mSize = value;
                this.ForceUpdate();
            }
        }
    }

    public float sliderValue
    {
        get
        {
            float rawValue = this.rawValue;
            if (this.numberOfSteps > 1)
            {
                rawValue = Mathf.Round(rawValue * (this.numberOfSteps - 1)) / ((float) (this.numberOfSteps - 1));
            }
            return rawValue;
        }
        set
        {
            this.Set(value, false);
        }
    }

    public enum Direction
    {
        Horizontal,
        Vertical
    }

    public delegate void OnValueChange(float val);
}

