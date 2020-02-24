using System;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("NGUI/Interaction/Grid"), ExecuteInEditMode]
public class UIGrid : MonoBehaviour
{
    public Arrangement arrangement;
    public float cellHeight = 200f;
    public float cellWidth = 200f;
    public bool hideInactive = true;
    public int maxPerLine;
    private bool mStarted;
    public bool repositionNow;
    public bool sorted;

    public void Reposition()
    {
        if (!this.mStarted)
        {
            this.repositionNow = true;
        }
        else
        {
            Transform transform = base.transform;
            int num = 0;
            int num2 = 0;
            if (this.sorted)
            {
                List<Transform> list = new List<Transform>();
                for (int i = 0; i < transform.childCount; i++)
                {
                    Transform child = transform.GetChild(i);
                    if ((child != null) && (!this.hideInactive || NGUITools.GetActive(child.gameObject)))
                    {
                        list.Add(child);
                    }
                }
                list.Sort(new Comparison<Transform>(UIGrid.SortByName));
                int num4 = 0;
                int count = list.Count;
                while (num4 < count)
                {
                    Transform transform3 = list[num4];
                    if (NGUITools.GetActive(transform3.gameObject) || !this.hideInactive)
                    {
                        float z = transform3.localPosition.z;
                        transform3.localPosition = (this.arrangement != Arrangement.Horizontal) ? new Vector3(this.cellWidth * num2, -this.cellHeight * num, z) : new Vector3(this.cellWidth * num, -this.cellHeight * num2, z);
                        if ((++num >= this.maxPerLine) && (this.maxPerLine > 0))
                        {
                            num = 0;
                            num2++;
                        }
                    }
                    num4++;
                }
            }
            else
            {
                for (int j = 0; j < transform.childCount; j++)
                {
                    Transform transform4 = transform.GetChild(j);
                    if (NGUITools.GetActive(transform4.gameObject) || !this.hideInactive)
                    {
                        float num8 = transform4.localPosition.z;
                        transform4.localPosition = (this.arrangement != Arrangement.Horizontal) ? new Vector3(this.cellWidth * num2, -this.cellHeight * num, num8) : new Vector3(this.cellWidth * num, -this.cellHeight * num2, num8);
                        if ((++num >= this.maxPerLine) && (this.maxPerLine > 0))
                        {
                            num = 0;
                            num2++;
                        }
                    }
                }
            }
            UIDraggablePanel panel = NGUITools.FindInParents<UIDraggablePanel>(base.gameObject);
            if (panel != null)
            {
                panel.UpdateScrollbars(true);
            }
        }
    }

    public static int SortByName(Transform a, Transform b)
    {
        return string.Compare(a.name, b.name);
    }

    private void Start()
    {
        this.mStarted = true;
        this.Reposition();
    }

    private void Update()
    {
        if (this.repositionNow)
        {
            this.repositionNow = false;
            this.Reposition();
        }
    }

    public enum Arrangement
    {
        Horizontal,
        Vertical
    }
}

