using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[AddComponentMenu("NGUI/Interaction/Table"), ExecuteInEditMode]
public class UITable : MonoBehaviour
{
    public int columns;
    public Direction direction;
    public bool hideInactive = true;
    public bool keepWithinPanel;
    private List<Transform> mChildren = new List<Transform>();
    private UIDraggablePanel mDrag;
    private UIPanel mPanel;
    private bool mStarted;
    public OnReposition onReposition;
    public Vector2 padding = Vector2.zero;
    public bool repositionNow;
    public bool sorted;

    private void LateUpdate()
    {
        if (this.repositionNow)
        {
            this.repositionNow = false;
            this.Reposition();
        }
    }

    public void Reposition()
    {
        if (this.mStarted)
        {
            Transform target = base.transform;
            this.mChildren.Clear();
            List<Transform> children = this.children;
            if (children.Count > 0)
            {
                this.RepositionVariableSize(children);
            }
            if (this.mDrag != null)
            {
                this.mDrag.UpdateScrollbars(true);
                this.mDrag.RestrictWithinBounds(true);
            }
            else if (this.mPanel != null)
            {
                this.mPanel.ConstrainTargetToBounds(target, true);
            }
            if (this.onReposition != null)
            {
                this.onReposition();
            }
        }
        else
        {
            this.repositionNow = true;
        }
    }

    private void RepositionVariableSize(List<Transform> children)
    {
        float num = 0f;
        float num2 = 0f;
        int num3 = (this.columns <= 0) ? 1 : ((children.Count / this.columns) + 1);
        int num4 = (this.columns <= 0) ? children.Count : this.columns;
        Bounds[,] boundsArray = new Bounds[num3, num4];
        Bounds[] boundsArray2 = new Bounds[num4];
        Bounds[] boundsArray3 = new Bounds[num3];
        int index = 0;
        int num6 = 0;
        int num7 = 0;
        int count = children.Count;
        while (num7 < count)
        {
            Transform trans = children[num7];
            Bounds bounds = NGUIMath.CalculateRelativeWidgetBounds(trans);
            Vector3 localScale = trans.localScale;
            bounds.min = Vector3.Scale(bounds.min, localScale);
            bounds.max = Vector3.Scale(bounds.max, localScale);
            boundsArray[num6, index] = bounds;
            boundsArray2[index].Encapsulate(bounds);
            boundsArray3[num6].Encapsulate(bounds);
            if ((++index >= this.columns) && (this.columns > 0))
            {
                index = 0;
                num6++;
            }
            num7++;
        }
        index = 0;
        num6 = 0;
        int num9 = 0;
        int num10 = children.Count;
        while (num9 < num10)
        {
            Transform transform2 = children[num9];
            Bounds bounds2 = boundsArray[num6, index];
            Bounds bounds3 = boundsArray2[index];
            Bounds bounds4 = boundsArray3[num6];
            Vector3 localPosition = transform2.localPosition;
            localPosition.x = (num + bounds2.extents.x) - bounds2.center.x;
            localPosition.x += (bounds2.min.x - bounds3.min.x) + this.padding.x;
            if (this.direction == Direction.Down)
            {
                localPosition.y = (-num2 - bounds2.extents.y) - bounds2.center.y;
                localPosition.y += ((((bounds2.max.y - bounds2.min.y) - bounds4.max.y) + bounds4.min.y) * 0.5f) - this.padding.y;
            }
            else
            {
                localPosition.y = (num2 + bounds2.extents.y) - bounds2.center.y;
                localPosition.y += ((((bounds2.max.y - bounds2.min.y) - bounds4.max.y) + bounds4.min.y) * 0.5f) - this.padding.y;
            }
            num += (bounds3.max.x - bounds3.min.x) + (this.padding.x * 2f);
            transform2.localPosition = localPosition;
            if ((++index >= this.columns) && (this.columns > 0))
            {
                index = 0;
                num6++;
                num = 0f;
                num2 += bounds4.size.y + (this.padding.y * 2f);
            }
            num9++;
        }
    }

    public static int SortByName(Transform a, Transform b)
    {
        return string.Compare(a.name, b.name);
    }

    private void Start()
    {
        this.mStarted = true;
        if (this.keepWithinPanel)
        {
            this.mPanel = NGUITools.FindInParents<UIPanel>(base.gameObject);
            this.mDrag = NGUITools.FindInParents<UIDraggablePanel>(base.gameObject);
        }
        this.Reposition();
    }

    public List<Transform> children
    {
        get
        {
            if (this.mChildren.Count == 0)
            {
                Transform transform = base.transform;
                this.mChildren.Clear();
                for (int i = 0; i < transform.childCount; i++)
                {
                    Transform child = transform.GetChild(i);
                    if (((child != null) && (child.gameObject != null)) && (!this.hideInactive || NGUITools.GetActive(child.gameObject)))
                    {
                        this.mChildren.Add(child);
                    }
                }
                if (this.sorted)
                {
                    this.mChildren.Sort(new Comparison<Transform>(UITable.SortByName));
                }
            }
            return this.mChildren;
        }
    }

    public enum Direction
    {
        Down,
        Up
    }

    public delegate void OnReposition();
}

