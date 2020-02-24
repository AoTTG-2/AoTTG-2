using System;
using UnityEngine;

[AddComponentMenu("NGUI/UI/Tooltip")]
public class UITooltip : MonoBehaviour
{
    public float appearSpeed = 10f;
    public UISprite background;
    private float mCurrent;
    private static UITooltip mInstance;
    private Vector3 mPos;
    private Vector3 mSize;
    private float mTarget;
    private Transform mTrans;
    private UIWidget[] mWidgets;
    public bool scalingTransitions = true;
    public UILabel text;
    public Camera uiCamera;

    private void Awake()
    {
        mInstance = this;
    }

    private void OnDestroy()
    {
        mInstance = null;
    }

    private void SetAlpha(float val)
    {
        int index = 0;
        int length = this.mWidgets.Length;
        while (index < length)
        {
            UIWidget widget = this.mWidgets[index];
            Color color = widget.color;
            color.a = val;
            widget.color = color;
            index++;
        }
    }

    private void SetText(string tooltipText)
    {
        if ((this.text != null) && !string.IsNullOrEmpty(tooltipText))
        {
            this.mTarget = 1f;
            if (this.text != null)
            {
                this.text.text = tooltipText;
            }
            this.mPos = Input.mousePosition;
            if (this.background != null)
            {
                Transform transform = this.background.transform;
                Transform transform2 = this.text.transform;
                Vector3 localPosition = transform2.localPosition;
                Vector3 localScale = transform2.localScale;
                this.mSize = (Vector3) this.text.relativeSize;
                this.mSize.x *= localScale.x;
                this.mSize.y *= localScale.y;
                this.mSize.x += (this.background.border.x + this.background.border.z) + ((localPosition.x - this.background.border.x) * 2f);
                this.mSize.y += (this.background.border.y + this.background.border.w) + ((-localPosition.y - this.background.border.y) * 2f);
                this.mSize.z = 1f;
                transform.localScale = this.mSize;
            }
            if (this.uiCamera != null)
            {
                this.mPos.x = Mathf.Clamp01(this.mPos.x / ((float) Screen.width));
                this.mPos.y = Mathf.Clamp01(this.mPos.y / ((float) Screen.height));
                float num = this.uiCamera.orthographicSize / this.mTrans.parent.lossyScale.y;
                float num2 = (Screen.height * 0.5f) / num;
                Vector2 vector10 = new Vector2((num2 * this.mSize.x) / ((float) Screen.width), (num2 * this.mSize.y) / ((float) Screen.height));
                this.mPos.x = Mathf.Min(this.mPos.x, 1f - vector10.x);
                this.mPos.y = Mathf.Max(this.mPos.y, vector10.y);
                this.mTrans.position = this.uiCamera.ViewportToWorldPoint(this.mPos);
                this.mPos = this.mTrans.localPosition;
                this.mPos.x = Mathf.Round(this.mPos.x);
                this.mPos.y = Mathf.Round(this.mPos.y);
                this.mTrans.localPosition = this.mPos;
            }
            else
            {
                if ((this.mPos.x + this.mSize.x) > Screen.width)
                {
                    this.mPos.x = Screen.width - this.mSize.x;
                }
                if ((this.mPos.y - this.mSize.y) < 0f)
                {
                    this.mPos.y = this.mSize.y;
                }
                this.mPos.x -= Screen.width * 0.5f;
                this.mPos.y -= Screen.height * 0.5f;
            }
        }
        else
        {
            this.mTarget = 0f;
        }
    }

    public static void ShowText(string tooltipText)
    {
        if (mInstance != null)
        {
            mInstance.SetText(tooltipText);
        }
    }

    private void Start()
    {
        this.mTrans = base.transform;
        this.mWidgets = base.GetComponentsInChildren<UIWidget>();
        this.mPos = this.mTrans.localPosition;
        this.mSize = this.mTrans.localScale;
        if (this.uiCamera == null)
        {
            this.uiCamera = NGUITools.FindCameraForLayer(base.gameObject.layer);
        }
        this.SetAlpha(0f);
    }

    private void Update()
    {
        if (this.mCurrent != this.mTarget)
        {
            this.mCurrent = Mathf.Lerp(this.mCurrent, this.mTarget, Time.deltaTime * this.appearSpeed);
            if (Mathf.Abs((float) (this.mCurrent - this.mTarget)) < 0.001f)
            {
                this.mCurrent = this.mTarget;
            }
            this.SetAlpha(this.mCurrent * this.mCurrent);
            if (this.scalingTransitions)
            {
                Vector3 vector = (Vector3) (this.mSize * 0.25f);
                vector.y = -vector.y;
                Vector3 vector2 = (Vector3) (Vector3.one * (1.5f - (this.mCurrent * 0.5f)));
                Vector3 vector3 = Vector3.Lerp(this.mPos - vector, this.mPos, this.mCurrent);
                this.mTrans.localPosition = vector3;
                this.mTrans.localScale = vector2;
            }
        }
    }
}

