using System;
using UnityEngine;

[ExecuteInEditMode, AddComponentMenu("NGUI/UI/Anchor")]
public class UIAnchor : MonoBehaviour
{
    public bool halfPixelOffset = true;
    private Animation mAnim;
    private bool mNeedsHalfPixelOffset;
    private Rect mRect = new Rect();
    private UIRoot mRoot;
    private Transform mTrans;
    public UIPanel panelContainer;
    public Vector2 relativeOffset = Vector2.zero;
    public bool runOnlyOnce;
    public Side side = Side.Center;
    public Camera uiCamera;
    public UIWidget widgetContainer;

    private void Awake()
    {
        this.mTrans = base.transform;
        this.mAnim = base.GetComponent<Animation>();
    }

    private void Start()
    {

    }

    private void Update()
    {
        if (((this.mAnim == null) || !this.mAnim.enabled) || !this.mAnim.isPlaying)
        {
            bool flag = false;
            if (this.panelContainer != null)
            {
                if (this.panelContainer.clipping == UIDrawCall.Clipping.None)
                {
                    float num = (this.mRoot == null) ? 0.5f : ((((float) this.mRoot.activeHeight) / ((float) Screen.height)) * 0.5f);
                    this.mRect.xMin = -Screen.width * num;
                    this.mRect.yMin = -Screen.height * num;
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
                Vector3 localScale = cachedTransform.localScale;
                Vector3 localPosition = cachedTransform.localPosition;
                Vector3 relativeSize = (Vector3) this.widgetContainer.relativeSize;
                Vector3 pivotOffset = (Vector3) this.widgetContainer.pivotOffset;
                pivotOffset.y--;
                pivotOffset.x *= this.widgetContainer.relativeSize.x * localScale.x;
                pivotOffset.y *= this.widgetContainer.relativeSize.y * localScale.y;
                this.mRect.x = localPosition.x + pivotOffset.x;
                this.mRect.y = localPosition.y + pivotOffset.y;
                this.mRect.width = relativeSize.x * localScale.x;
                this.mRect.height = relativeSize.y * localScale.y;
            }
            else
            {
                if (this.uiCamera == null)
                {
                    return;
                }
                flag = true;
                this.mRect = this.uiCamera.pixelRect;
            }
            float x = (this.mRect.xMin + this.mRect.xMax) * 0.5f;
            float y = (this.mRect.yMin + this.mRect.yMax) * 0.5f;
            Vector3 position = new Vector3(x, y, 0f);
            if (this.side != Side.Center)
            {
                if (((this.side != Side.Right) && (this.side != Side.TopRight)) && (this.side != Side.BottomRight))
                {
                    if (((this.side != Side.Top) && (this.side != Side.Center)) && (this.side != Side.Bottom))
                    {
                        position.x = this.mRect.xMin;
                    }
                    else
                    {
                        position.x = x;
                    }
                }
                else
                {
                    position.x = this.mRect.xMax;
                }
                if (((this.side != Side.Top) && (this.side != Side.TopRight)) && (this.side != Side.TopLeft))
                {
                    if (((this.side != Side.Left) && (this.side != Side.Center)) && (this.side != Side.Right))
                    {
                        position.y = this.mRect.yMin;
                    }
                    else
                    {
                        position.y = y;
                    }
                }
                else
                {
                    position.y = this.mRect.yMax;
                }
            }
            float width = this.mRect.width;
            float height = this.mRect.height;
            position.x += this.relativeOffset.x * width;
            position.y += this.relativeOffset.y * height;
            if (flag)
            {
                if (this.uiCamera.orthographic)
                {
                    position.x = Mathf.Round(position.x);
                    position.y = Mathf.Round(position.y);
                    if (this.halfPixelOffset && this.mNeedsHalfPixelOffset)
                    {
                        position.x -= 0.5f;
                        position.y += 0.5f;
                    }
                }
                position.z = this.uiCamera.WorldToScreenPoint(this.mTrans.position).z;
                position = this.uiCamera.ScreenToWorldPoint(position);
            }
            else
            {
                position.x = Mathf.Round(position.x);
                position.y = Mathf.Round(position.y);
                if (this.panelContainer != null)
                {
                    position = this.panelContainer.cachedTransform.TransformPoint(position);
                }
                else if (this.widgetContainer != null)
                {
                    Transform parent = this.widgetContainer.cachedTransform.parent;
                    if (parent != null)
                    {
                        position = parent.TransformPoint(position);
                    }
                }
                position.z = this.mTrans.position.z;
            }
            if (this.mTrans.position != position)
            {
                this.mTrans.position = position;
            }
            if (this.runOnlyOnce && Application.isPlaying)
            {
                UnityEngine.Object.Destroy(this);
            }
        }
    }

    public enum Side
    {
        BottomLeft,
        Left,
        TopLeft,
        Top,
        TopRight,
        Right,
        BottomRight,
        Bottom,
        Center
    }
}

