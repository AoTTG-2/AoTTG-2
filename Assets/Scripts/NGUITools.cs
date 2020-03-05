using System;
using System.Collections.Generic;
using UnityEngine;

public static class NGUITools
{
    private static void Activate(Transform t)
    {
        SetActiveSelf(t.gameObject, true);
        int index = 0;
        int childCount = t.childCount;
        while (index < childCount)
        {
            if (t.GetChild(index).gameObject.activeSelf)
            {
                return;
            }
            index++;
        }
        int num3 = 0;
        int num4 = t.childCount;
        while (num3 < num4)
        {
            Activate(t.GetChild(num3));
            num3++;
        }
    }

    public static Color ApplyPMA(Color c)
    {
        if (c.a != 1f)
        {
            c.r *= c.a;
            c.g *= c.a;
            c.b *= c.a;
        }
        return c;
    }

    private static void Deactivate(Transform t)
    {
        SetActiveSelf(t.gameObject, false);
    }

    public static void Destroy(UnityEngine.Object obj)
    {
        if (obj != null)
        {
            if (Application.isPlaying)
            {
                if (obj is GameObject)
                {
                    GameObject obj2 = obj as GameObject;
                    obj2.transform.parent = null;
                }
                UnityEngine.Object.Destroy(obj);
            }
            else
            {
                UnityEngine.Object.DestroyImmediate(obj);
            }
        }
    }

    public static T[] FindActive<T>() where T: Component
    {
        return (UnityEngine.Object.FindObjectsOfType(typeof(T)) as T[]);
    }

    public static bool GetActive(GameObject go)
    {
        return ((go != null) && go.activeInHierarchy);
    }

    public static string GetHierarchy(GameObject obj)
    {
        string name = obj.name;
        while (obj.transform.parent != null)
        {
            obj = obj.transform.parent.gameObject;
            name = obj.name + "/" + name;
        }
        return ("\"" + name + "\"");
    }

    public static void MakePixelPerfect(Transform t)
    {
        UIWidget component = t.GetComponent<UIWidget>();
        if (component != null)
        {
            component.MakePixelPerfect();
        }
        else
        {
            t.localPosition = Round(t.localPosition);
            t.localScale = Round(t.localScale);
            int index = 0;
            int childCount = t.childCount;
            while (index < childCount)
            {
                MakePixelPerfect(t.GetChild(index));
                index++;
            }
        }
    }

    public static Vector3 Round(Vector3 v)
    {
        v.x = Mathf.Round(v.x);
        v.y = Mathf.Round(v.y);
        v.z = Mathf.Round(v.z);
        return v;
    }

    public static void SetActive(GameObject go, bool state)
    {
        if (state)
        {
            Activate(go.transform);
        }
        else
        {
            Deactivate(go.transform);
        }
    }

    public static void SetActiveSelf(GameObject go, bool state)
    {
        go.SetActive(state);
    }

    public static void SetLayer(GameObject go, int layer)
    {
        go.layer = layer;
        Transform transform = go.transform;
        int index = 0;
        int childCount = transform.childCount;
        while (index < childCount)
        {
            SetLayer(transform.GetChild(index).gameObject, layer);
            index++;
        }
    }
}

