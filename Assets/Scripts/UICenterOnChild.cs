using System;
using UnityEngine;

[AddComponentMenu("NGUI/Interaction/Center On Child")]
public class UICenterOnChild : MonoBehaviour
{
    private GameObject mCenteredObject;
    private UIDraggablePanel mDrag;
    public SpringPanel.OnFinished onFinished;
    public float springStrength = 8f;

    private void OnDragFinished()
    {
        if (base.enabled)
        {
            this.Recenter();
        }
    }

    private void OnEnable()
    {
        this.Recenter();
    }

    public void Recenter()
    {
        if (this.mDrag == null)
        {
            this.mDrag = NGUITools.FindInParents<UIDraggablePanel>(base.gameObject);
            if (this.mDrag == null)
            {
                Debug.LogWarning(string.Concat(new object[] { base.GetType(), " requires ", typeof(UIDraggablePanel), " on a parent object in order to work" }), this);
                base.enabled = false;
                return;
            }
            this.mDrag.onDragFinished = new UIDraggablePanel.OnDragFinished(this.OnDragFinished);
            if (this.mDrag.horizontalScrollBar != null)
            {
                this.mDrag.horizontalScrollBar.onDragFinished = new UIScrollBar.OnDragFinished(this.OnDragFinished);
            }
            if (this.mDrag.verticalScrollBar != null)
            {
                this.mDrag.verticalScrollBar.onDragFinished = new UIScrollBar.OnDragFinished(this.OnDragFinished);
            }
        }
        if (this.mDrag.panel != null)
        {
            Vector4 clipRange = this.mDrag.panel.clipRange;
            Transform cachedTransform = this.mDrag.panel.cachedTransform;
            Vector3 localPosition = cachedTransform.localPosition;
            localPosition.x += clipRange.x;
            localPosition.y += clipRange.y;
            localPosition = cachedTransform.parent.TransformPoint(localPosition);
            Vector3 vector3 = localPosition - ((Vector3) (this.mDrag.currentMomentum * (this.mDrag.momentumAmount * 0.1f)));
            this.mDrag.currentMomentum = Vector3.zero;
            float maxValue = float.MaxValue;
            Transform transform2 = null;
            Transform transform = base.transform;
            int index = 0;
            int childCount = transform.childCount;
            while (index < childCount)
            {
                Transform child = transform.GetChild(index);
                float num4 = Vector3.SqrMagnitude(child.position - vector3);
                if (num4 < maxValue)
                {
                    maxValue = num4;
                    transform2 = child;
                }
                index++;
            }
            if (transform2 != null)
            {
                this.mCenteredObject = transform2.gameObject;
                Vector3 vector4 = cachedTransform.InverseTransformPoint(transform2.position);
                Vector3 vector5 = cachedTransform.InverseTransformPoint(localPosition);
                Vector3 vector6 = vector4 - vector5;
                if (this.mDrag.scale.x == 0f)
                {
                    vector6.x = 0f;
                }
                if (this.mDrag.scale.y == 0f)
                {
                    vector6.y = 0f;
                }
                if (this.mDrag.scale.z == 0f)
                {
                    vector6.z = 0f;
                }
                SpringPanel.Begin(this.mDrag.gameObject, cachedTransform.localPosition - vector6, this.springStrength).onFinished = this.onFinished;
            }
            else
            {
                this.mCenteredObject = null;
            }
        }
    }

    public GameObject centeredObject
    {
        get
        {
            return this.mCenteredObject;
        }
    }
}

