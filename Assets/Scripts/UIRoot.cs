using System;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("NGUI/UI/Root"), ExecuteInEditMode]
public class UIRoot : MonoBehaviour
{
    [HideInInspector]
    public bool automatic;
    public int manualHeight = 720;
    public int maximumHeight = 0x600;
    public int minimumHeight = 320;
    private static List<UIRoot> mRoots = new List<UIRoot>();
    private Transform mTrans;
    public Scaling scalingStyle = Scaling.FixedSize;

    private void Awake()
    {
        this.mTrans = base.transform;
        mRoots.Add(this);
        if (this.automatic)
        {
            this.scalingStyle = Scaling.PixelPerfect;
            this.automatic = false;
        }
    }

    public static void Broadcast(string funcName)
    {
        int num = 0;
        int count = mRoots.Count;
        while (num < count)
        {
            UIRoot root = mRoots[num];
            if (root != null)
            {
                root.BroadcastMessage(funcName, SendMessageOptions.DontRequireReceiver);
            }
            num++;
        }
    }

    public static void Broadcast(string funcName, object param)
    {
        if (param == null)
        {
            Debug.LogError("SendMessage is bugged when you try to pass 'null' in the parameter field. It behaves as if no parameter was specified.");
        }
        else
        {
            int num = 0;
            int count = mRoots.Count;
            while (num < count)
            {
                UIRoot root = mRoots[num];
                if (root != null)
                {
                    root.BroadcastMessage(funcName, param, SendMessageOptions.DontRequireReceiver);
                }
                num++;
            }
        }
    }

    public float GetPixelSizeAdjustment(int height)
    {
        height = Mathf.Max(2, height);
        if (this.scalingStyle == Scaling.FixedSize)
        {
            return (((float) this.manualHeight) / ((float) height));
        }
        if (height < this.minimumHeight)
        {
            return (((float) this.minimumHeight) / ((float) height));
        }
        if (height > this.maximumHeight)
        {
            return (((float) this.maximumHeight) / ((float) height));
        }
        return 1f;
    }

    public static float GetPixelSizeAdjustment(GameObject go)
    {
        UIRoot root = NGUITools.FindInParents<UIRoot>(go);
        return ((root == null) ? 1f : root.pixelSizeAdjustment);
    }

    private void OnDestroy()
    {
        mRoots.Remove(this);
    }

    private void Start()
    {
        UIOrthoCamera componentInChildren = base.GetComponentInChildren<UIOrthoCamera>();
        if (componentInChildren != null)
        {
            Debug.LogWarning("UIRoot should not be active at the same time as UIOrthoCamera. Disabling UIOrthoCamera.", componentInChildren);
            Camera component = componentInChildren.gameObject.GetComponent<Camera>();
            componentInChildren.enabled = false;
            if (component != null)
            {
                component.orthographicSize = 1f;
            }
        }
    }

    private void Update()
    {
        if (this.mTrans != null)
        {
            float activeHeight = this.activeHeight;
            if (activeHeight > 0f)
            {
                float x = 2f / activeHeight;
                Vector3 localScale = this.mTrans.localScale;
                if (((Mathf.Abs((float) (localScale.x - x)) > float.Epsilon) || (Mathf.Abs((float) (localScale.y - x)) > float.Epsilon)) || (Mathf.Abs((float) (localScale.z - x)) > float.Epsilon))
                {
                    this.mTrans.localScale = new Vector3(x, x, x);
                }
            }
        }
    }

    public int activeHeight
    {
        get
        {
            int num = Mathf.Max(2, Screen.height);
            if (this.scalingStyle == Scaling.FixedSize)
            {
                return this.manualHeight;
            }
            if (num < this.minimumHeight)
            {
                return this.minimumHeight;
            }
            if (num > this.maximumHeight)
            {
                return this.maximumHeight;
            }
            return num;
        }
    }

    public static List<UIRoot> list
    {
        get
        {
            return mRoots;
        }
    }

    public float pixelSizeAdjustment
    {
        get
        {
            return this.GetPixelSizeAdjustment(Screen.height);
        }
    }

    public enum Scaling
    {
        PixelPerfect,
        FixedSize,
        FixedSizeOnMobiles
    }
}

