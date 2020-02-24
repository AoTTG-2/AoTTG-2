using System;
using UnityEngine;

[AddComponentMenu("NGUI/Examples/Drag and Drop Item")]
public class DragDropItem : MonoBehaviour
{
    private bool mIsDragging;
    private Transform mParent;
    private bool mSticky;
    private Transform mTrans;
    public GameObject prefab;

    private void Awake()
    {
        this.mTrans = base.transform;
    }

    private void Drop()
    {
        Collider collider = UICamera.lastHit.collider;
        DragDropContainer container = (collider == null) ? null : collider.gameObject.GetComponent<DragDropContainer>();
        if (container != null)
        {
            this.mTrans.parent = container.transform;
            Vector3 localPosition = this.mTrans.localPosition;
            localPosition.z = 0f;
            this.mTrans.localPosition = localPosition;
        }
        else
        {
            this.mTrans.parent = this.mParent;
        }
        this.UpdateTable();
        NGUITools.MarkParentAsChanged(base.gameObject);
    }

    private void OnDrag(Vector2 delta)
    {
        if (base.enabled && (UICamera.currentTouchID > -2))
        {
            if (!this.mIsDragging)
            {
                this.mIsDragging = true;
                this.mParent = this.mTrans.parent;
                this.mTrans.parent = DragDropRoot.root;
                Vector3 localPosition = this.mTrans.localPosition;
                localPosition.z = 0f;
                this.mTrans.localPosition = localPosition;
                NGUITools.MarkParentAsChanged(base.gameObject);
            }
            else
            {
                this.mTrans.localPosition += (Vector3)delta;
            }
        }
    }

    private void OnPress(bool isPressed)
    {
        if (base.enabled)
        {
            if (isPressed)
            {
                if (!UICamera.current.stickyPress)
                {
                    this.mSticky = true;
                    UICamera.current.stickyPress = true;
                }
            }
            else if (this.mSticky)
            {
                this.mSticky = false;
                UICamera.current.stickyPress = false;
            }
            this.mIsDragging = false;
            Collider collider = base.GetComponent<Collider>();
            if (collider != null)
            {
                collider.enabled = !isPressed;
            }
            if (!isPressed)
            {
                this.Drop();
            }
        }
    }

    private void UpdateTable()
    {
        UITable table = NGUITools.FindInParents<UITable>(base.gameObject);
        if (table != null)
        {
            table.repositionNow = true;
        }
    }
}

