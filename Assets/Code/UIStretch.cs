using System;
using UnityEngine;

[ExecuteInEditMode, AddComponentMenu("NGUI/UI/Stretch")]
public class UIStretch : MonoBehaviour
{
    public Vector2 initialSize = Vector2.one;
    private Animation mAnim;
    private Rect mRect;
    private UIRoot mRoot;
    private Transform mTrans;
    public UIPanel panelContainer;
    public Vector2 relativeSize = Vector2.one;
    public Style style;
    public Camera uiCamera;
    public UIWidget widgetContainer;

    private void Awake()
    {
        this.mAnim = base.GetComponent<Animation>();
        this.mRect = new Rect();
        this.mTrans = base.transform;
    }

    private void Start()
    {
        if (this.uiCamera == null)
        {
            this.uiCamera = NGUITools.FindCameraForLayer(base.gameObject.layer);
        }
        this.mRoot = NGUITools.FindInParents<UIRoot>(base.gameObject);
    }

    private void Update()
    {
        if (((this.mAnim == null) || !this.mAnim.isPlaying) && (this.style != Style.None))
        {
            float pixelSizeAdjustment = 1f;
            if (this.panelContainer != null)
            {
                if (this.panelContainer.clipping == UIDrawCall.Clipping.None)
                {
                    this.mRect.xMin = -Screen.width * 0.5f;
                    this.mRect.yMin = -Screen.height * 0.5f;
                    this.mRect.xMax = -this.mRect.xMin;
                    this.mRect.yMax = -this.mRect.yMin;
                }
                else
                {
                    Vector4 clipRange = this.panelContainer.clipRange;
                    this.mRect.x = clipRange.x - (clipRange.z * 0.5f);
                    this.mRect.y = clipRange.y - (clipRange.w * 0.5f);
                    this.mRect.width = clipRange.z;
                    this.mRect.height = clipRange.w;
                }
            }
            else if (this.widgetContainer != null)
            {
                Transform cachedTransform = this.widgetContainer.cachedTransform;
                Vector3 vector2 = cachedTransform.localScale;
                Vector3 localPosition = cachedTransform.localPosition;
                Vector3 relativeSize = (Vector3) this.widgetContainer.relativeSize;
                Vector3 pivotOffset = (Vector3) this.widgetContainer.pivotOffset;
                pivotOffset.y--;
                pivotOffset.x *= this.widgetContainer.relativeSize.x * vector2.x;
                pivotOffset.y *= this.widgetContainer.relativeSize.y * vector2.y;
                this.mRect.x = localPosition.x + pivotOffset.x;
                this.mRect.y = localPosition.y + pivotOffset.y;
                this.mRect.width = relativeSize.x * vector2.x;
                this.mRect.height = relativeSize.y * vector2.y;
            }
            else
            {
                if (this.uiCamera == null)
                {
                    return;
                }
                this.mRect = this.uiCamera.pixelRect;
                if (this.mRoot != null)
                {
                    pixelSizeAdjustment = this.mRoot.pixelSizeAdjustment;
                }
            }
            float width = this.mRect.width;
            float height = this.mRect.height;
            if ((pixelSizeAdjustment != 1f) && (height > 1f))
            {
                float num4 = ((float) this.mRoot.activeHeight) / height;
                width *= num4;
                height *= num4;
            }
            Vector3 localScale = this.mTrans.localScale;
            if (this.style == Style.BasedOnHeight)
            {
                localScale.x = this.relativeSize.x * height;
                localScale.y = this.relativeSize.y * height;
            }
            else if (this.style == Style.FillKeepingRatio)
            {
                float num5 = width / height;
                float num6 = this.initialSize.x / this.initialSize.y;
                if (num6 < num5)
                {
                    float num7 = width / this.initialSize.x;
                    localScale.x = width;
                    localScale.y = this.initialSize.y * num7;
                }
                else
                {
                    float num8 = height / this.initialSize.y;
                    localScale.x = this.initialSize.x * num8;
                    localScale.y = height;
                }
            }
            else if (this.style == Style.FitInternalKeepingRatio)
            {
                float num9 = width / height;
                float num10 = this.initialSize.x / this.initialSize.y;
                if (num10 > num9)
                {
                    float num11 = width / this.initialSize.x;
                    localScale.x = width;
                    localScale.y = this.initialSize.y * num11;
                }
                else
                {
                    float num12 = height / this.initialSize.y;
                    localScale.x = this.initialSize.x * num12;
                    localScale.y = height;
                }
            }
            else
            {
                if ((this.style == Style.Both) || (this.style == Style.Horizontal))
                {
                    localScale.x = this.relativeSize.x * width;
                }
                if ((this.style == Style.Both) || (this.style == Style.Vertical))
                {
                    localScale.y = this.relativeSize.y * height;
                }
            }
            if (this.mTrans.localScale != localScale)
            {
                this.mTrans.localScale = localScale;
            }
        }
    }

    public enum Style
    {
        None,
        Horizontal,
        Vertical,
        Both,
        BasedOnHeight,
        FillKeepingRatio,
        FitInternalKeepingRatio
    }
}

