using System;
using System.Runtime.CompilerServices;
using UnityEngine;

[ExecuteInEditMode, AddComponentMenu("NGUI/UI/Panel")]
public class UIPanel : MonoBehaviour
{
    public bool cullWhileDragging;
    public bool depthPass;
    public bool generateNormals;
    [SerializeField, HideInInspector]
    private float mAlpha = 1f;
    private Camera mCam;
    private BetterList<Material> mChanged = new BetterList<Material>();
    private UIPanel[] mChildPanels;
    [SerializeField, HideInInspector]
    private UIDrawCall.Clipping mClipping;
    [HideInInspector, SerializeField]
    private Vector4 mClipRange = Vector4.zero;
    [HideInInspector, SerializeField]
    private Vector2 mClipSoftness = new Vector2(40f, 40f);
    private BetterList<Color32> mCols = new BetterList<Color32>();
    private float mCullTime;
    [HideInInspector, SerializeField]
    private DebugInfo mDebugInfo = DebugInfo.Gizmos;
    private bool mDepthChanged;
    private BetterList<UIDrawCall> mDrawCalls = new BetterList<UIDrawCall>();
    private GameObject mGo;
    private int mLayer = -1;
    private float mMatrixTime;
    private Vector2 mMax = Vector2.zero;
    private Vector2 mMin = Vector2.zero;
    private BetterList<Vector3> mNorms = new BetterList<Vector3>();
    private BetterList<Vector4> mTans = new BetterList<Vector4>();
    private static float[] mTemp = new float[4];
    private Transform mTrans;
    private float mUpdateTime;
    private BetterList<Vector2> mUvs = new BetterList<Vector2>();
    private BetterList<Vector3> mVerts = new BetterList<Vector3>();
    private BetterList<UIWidget> mWidgets = new BetterList<UIWidget>();
    public OnChangeDelegate onChange;
    public bool showInPanelTool = true;
    public bool widgetsAreStatic;
    [HideInInspector]
    public Matrix4x4 worldToLocal = Matrix4x4.identity;

    public void AddWidget(UIWidget w)
    {
        if ((w != null) && !this.mWidgets.Contains(w))
        {
            this.mWidgets.Add(w);
            if (!this.mChanged.Contains(w.material))
            {
                this.mChanged.Add(w.material);
            }
            this.mDepthChanged = true;
        }
    }

    private void Awake()
    {
        this.mGo = base.gameObject;
        this.mTrans = base.transform;
    }

    public Vector3 CalculateConstrainOffset(Vector2 min, Vector2 max)
    {
        float num = this.clipRange.z * 0.5f;
        float num2 = this.clipRange.w * 0.5f;
        Vector2 minRect = new Vector2(min.x, min.y);
        Vector2 maxRect = new Vector2(max.x, max.y);
        Vector2 minArea = new Vector2(this.clipRange.x - num, this.clipRange.y - num2);
        Vector2 maxArea = new Vector2(this.clipRange.x + num, this.clipRange.y + num2);
        if (this.clipping == UIDrawCall.Clipping.SoftClip)
        {
            minArea.x += this.clipSoftness.x;
            minArea.y += this.clipSoftness.y;
            maxArea.x -= this.clipSoftness.x;
            maxArea.y -= this.clipSoftness.y;
        }
        return (Vector3) NGUIMath.ConstrainRect(minRect, maxRect, minArea, maxArea);
    }

    public bool ConstrainTargetToBounds(Transform target, bool immediate)
    {
        Bounds targetBounds = NGUIMath.CalculateRelativeWidgetBounds(this.cachedTransform, target);
        return this.ConstrainTargetToBounds(target, ref targetBounds, immediate);
    }

    public bool ConstrainTargetToBounds(Transform target, ref Bounds targetBounds, bool immediate)
    {
        Vector3 vector = this.CalculateConstrainOffset(targetBounds.min, targetBounds.max);
        if (vector.magnitude <= 0f)
        {
            return false;
        }
        if (immediate)
        {
            target.localPosition += vector;
            targetBounds.center += vector;
            SpringPosition component = target.GetComponent<SpringPosition>();
            if (component != null)
            {
                component.enabled = false;
            }
        }
        else
        {
            SpringPosition position2 = SpringPosition.Begin(target.gameObject, target.localPosition + vector, 13f);
            position2.ignoreTimeScale = true;
            position2.worldSpace = false;
        }
        return true;
    }

    private void Fill(Material mat)
    {
        int index = 0;
        while (index < this.mWidgets.size)
        {
            UIWidget widget = this.mWidgets.buffer[index];
            if (widget == null)
            {
                this.mWidgets.RemoveAt(index);
            }
            else
            {
                if ((widget.material == mat) && widget.isVisible)
                {
                    if (widget.panel == this)
                    {
                        if (this.generateNormals)
                        {
                            widget.WriteToBuffers(this.mVerts, this.mUvs, this.mCols, this.mNorms, this.mTans);
                        }
                        else
                        {
                            widget.WriteToBuffers(this.mVerts, this.mUvs, this.mCols, null, null);
                        }
                    }
                    else
                    {
                        this.mWidgets.RemoveAt(index);
                        continue;
                    }
                }
                index++;
            }
        }
        if (this.mVerts.size > 0)
        {
            UIDrawCall drawCall = this.GetDrawCall(mat, true);
            drawCall.depthPass = this.depthPass && (this.mClipping == UIDrawCall.Clipping.None);
            drawCall.Set(this.mVerts, !this.generateNormals ? null : this.mNorms, !this.generateNormals ? null : this.mTans, this.mUvs, this.mCols);
        }
        else
        {
            UIDrawCall item = this.GetDrawCall(mat, false);
            if (item != null)
            {
                this.mDrawCalls.Remove(item);
                NGUITools.DestroyImmediate(item.gameObject);
            }
        }
        this.mVerts.Clear();
        this.mNorms.Clear();
        this.mTans.Clear();
        this.mUvs.Clear();
        this.mCols.Clear();
    }

    public static UIPanel Find(Transform trans)
    {
        return Find(trans, true);
    }

    public static UIPanel Find(Transform trans, bool createIfMissing)
    {
        Transform transform = trans;
        UIPanel component = null;
        while (component == null)
        {
            if (trans == null)
            {
                break;
            }
            component = trans.GetComponent<UIPanel>();
            if ((component != null) || (trans.parent == null))
            {
                break;
            }
            trans = trans.parent;
        }
        if ((createIfMissing && (component == null)) && (trans != transform))
        {
            component = trans.gameObject.AddComponent<UIPanel>();
            SetChildLayer(component.cachedTransform, component.cachedGameObject.layer);
        }
        return component;
    }

    private UIDrawCall GetDrawCall(Material mat, bool createIfMissing)
    {
        int index = 0;
        int size = this.drawCalls.size;
        while (index < size)
        {
            UIDrawCall call = this.drawCalls.buffer[index];
            if (call.material == mat)
            {
                return call;
            }
            index++;
        }
        UIDrawCall item = null;
        if (createIfMissing)
        {
            GameObject target = new GameObject("_UIDrawCall [" + mat.name + "]");
            UnityEngine.Object.DontDestroyOnLoad(target);
            target.layer = this.cachedGameObject.layer;
            item = target.AddComponent<UIDrawCall>();
            item.material = mat;
            this.mDrawCalls.Add(item);
        }
        return item;
    }

    public bool IsVisible(UIWidget w)
    {
        if (this.mAlpha < 0.001f)
        {
            return false;
        }
        if ((!w.enabled || !NGUITools.GetActive(w.cachedGameObject)) || (w.alpha < 0.001f))
        {
            return false;
        }
        if (this.mClipping == UIDrawCall.Clipping.None)
        {
            return true;
        }
        Vector2 relativeSize = w.relativeSize;
        Vector2 vector2 = Vector2.Scale(w.pivotOffset, relativeSize);
        Vector2 vector3 = vector2;
        vector2.x += relativeSize.x;
        vector2.y -= relativeSize.y;
        Transform cachedTransform = w.cachedTransform;
        Vector3 a = cachedTransform.TransformPoint((Vector3) vector2);
        Vector3 b = cachedTransform.TransformPoint((Vector3) new Vector2(vector2.x, vector3.y));
        Vector3 c = cachedTransform.TransformPoint((Vector3) new Vector2(vector3.x, vector2.y));
        Vector3 d = cachedTransform.TransformPoint((Vector3) vector3);
        return this.IsVisible(a, b, c, d);
    }

    public bool IsVisible(Vector3 worldPos)
    {
        if (this.mAlpha < 0.001f)
        {
            return false;
        }
        if (this.mClipping != UIDrawCall.Clipping.None)
        {
            this.UpdateTransformMatrix();
            Vector3 vector = this.worldToLocal.MultiplyPoint3x4(worldPos);
            if (vector.x < this.mMin.x)
            {
                return false;
            }
            if (vector.y < this.mMin.y)
            {
                return false;
            }
            if (vector.x > this.mMax.x)
            {
                return false;
            }
            if (vector.y > this.mMax.y)
            {
                return false;
            }
        }
        return true;
    }

    private bool IsVisible(Vector3 a, Vector3 b, Vector3 c, Vector3 d)
    {
        this.UpdateTransformMatrix();
        a = this.worldToLocal.MultiplyPoint3x4(a);
        b = this.worldToLocal.MultiplyPoint3x4(b);
        c = this.worldToLocal.MultiplyPoint3x4(c);
        d = this.worldToLocal.MultiplyPoint3x4(d);
        mTemp[0] = a.x;
        mTemp[1] = b.x;
        mTemp[2] = c.x;
        mTemp[3] = d.x;
        float num = Mathf.Min(mTemp);
        float num2 = Mathf.Max(mTemp);
        mTemp[0] = a.y;
        mTemp[1] = b.y;
        mTemp[2] = c.y;
        mTemp[3] = d.y;
        float num3 = Mathf.Min(mTemp);
        float num4 = Mathf.Max(mTemp);
        if (num2 < this.mMin.x)
        {
            return false;
        }
        if (num4 < this.mMin.y)
        {
            return false;
        }
        if (num > this.mMax.x)
        {
            return false;
        }
        if (num3 > this.mMax.y)
        {
            return false;
        }
        return true;
    }

    private void LateUpdate()
    {
        this.mUpdateTime = Time.realtimeSinceStartup;
        this.UpdateTransformMatrix();
        if (this.mLayer != this.cachedGameObject.layer)
        {
            this.mLayer = this.mGo.layer;
            UICamera camera = UICamera.FindCameraForLayer(this.mLayer);
            this.mCam = (camera == null) ? NGUITools.FindCameraForLayer(this.mLayer) : camera.cachedCamera;
            SetChildLayer(this.cachedTransform, this.mLayer);
            int num = 0;
            int num2 = this.drawCalls.size;
            while (num < num2)
            {
                this.mDrawCalls.buffer[num].gameObject.layer = this.mLayer;
                num++;
            }
        }
        bool forceVisible = !this.cullWhileDragging ? ((this.clipping == UIDrawCall.Clipping.None) || (this.mCullTime > this.mUpdateTime)) : false;
        int num3 = 0;
        int size = this.mWidgets.size;
        while (num3 < size)
        {
            UIWidget widget = this.mWidgets[num3];
            if (widget.UpdateGeometry(this, forceVisible) && !this.mChanged.Contains(widget.material))
            {
                this.mChanged.Add(widget.material);
            }
            num3++;
        }
        if ((this.mChanged.size != 0) && (this.onChange != null))
        {
            this.onChange();
        }
        if (this.mDepthChanged)
        {
            this.mDepthChanged = false;
            this.mWidgets.Sort(new Comparison<UIWidget>(UIWidget.CompareFunc));
        }
        int index = 0;
        int num6 = this.mChanged.size;
        while (index < num6)
        {
            this.Fill(this.mChanged.buffer[index]);
            index++;
        }
        this.UpdateDrawcalls();
        this.mChanged.Clear();
    }

    public void MarkMaterialAsChanged(Material mat, bool sort)
    {
        if (mat != null)
        {
            if (sort)
            {
                this.mDepthChanged = true;
            }
            if (!this.mChanged.Contains(mat))
            {
                this.mChanged.Add(mat);
            }
        }
    }

    private void OnDisable()
    {
        int size = this.mDrawCalls.size;
        while (size > 0)
        {
            UIDrawCall call = this.mDrawCalls.buffer[--size];
            if (call != null)
            {
                NGUITools.DestroyImmediate(call.gameObject);
            }
        }
        this.mDrawCalls.Clear();
        this.mChanged.Clear();
    }

    private void OnEnable()
    {
        int index = 0;
        while (index < this.mWidgets.size)
        {
            UIWidget widget = this.mWidgets.buffer[index];
            if (widget != null)
            {
                this.MarkMaterialAsChanged(widget.material, true);
                index++;
            }
            else
            {
                this.mWidgets.RemoveAt(index);
            }
        }
    }

    public void Refresh()
    {
        UIWidget[] componentsInChildren = base.GetComponentsInChildren<UIWidget>();
        int index = 0;
        int length = componentsInChildren.Length;
        while (index < length)
        {
            componentsInChildren[index].Update();
            index++;
        }
        this.LateUpdate();
    }

    public void RemoveWidget(UIWidget w)
    {
        if (((w != null) && (w != null)) && (this.mWidgets.Remove(w) && (w.material != null)))
        {
            this.mChanged.Add(w.material);
        }
    }

    public void SetAlphaRecursive(float val, bool rebuildList)
    {
        if (rebuildList || (this.mChildPanels == null))
        {
            this.mChildPanels = base.GetComponentsInChildren<UIPanel>(true);
        }
        int index = 0;
        int length = this.mChildPanels.Length;
        while (index < length)
        {
            this.mChildPanels[index].alpha = val;
            index++;
        }
    }

    private static void SetChildLayer(Transform t, int layer)
    {
        for (int i = 0; i < t.childCount; i++)
        {
            Transform child = t.GetChild(i);
            if (child.GetComponent<UIPanel>() == null)
            {
                if (child.GetComponent<UIWidget>() != null)
                {
                    child.gameObject.layer = layer;
                }
                SetChildLayer(child, layer);
            }
        }
    }

    private void Start()
    {
        this.mLayer = this.mGo.layer;
        UICamera camera = UICamera.FindCameraForLayer(this.mLayer);
        this.mCam = (camera == null) ? NGUITools.FindCameraForLayer(this.mLayer) : camera.cachedCamera;
    }

    public void UpdateDrawcalls()
    {
        Vector4 zero = Vector4.zero;
        if (this.mClipping != UIDrawCall.Clipping.None)
        {
            zero = new Vector4(this.mClipRange.x, this.mClipRange.y, this.mClipRange.z * 0.5f, this.mClipRange.w * 0.5f);
        }
        if (zero.z == 0f)
        {
            zero.z = Screen.width * 0.5f;
        }
        if (zero.w == 0f)
        {
            zero.w = Screen.height * 0.5f;
        }
        switch (Application.platform)
        {
            case RuntimePlatform.WindowsPlayer:
            case RuntimePlatform.WindowsWebPlayer:
            case RuntimePlatform.WindowsEditor:
                zero.x -= 0.5f;
                zero.y += 0.5f;
                break;
        }
        Transform cachedTransform = this.cachedTransform;
        int index = 0;
        int size = this.mDrawCalls.size;
        while (index < size)
        {
            UIDrawCall call = this.mDrawCalls.buffer[index];
            call.clipping = this.mClipping;
            call.clipRange = zero;
            call.clipSoftness = this.mClipSoftness;
            call.depthPass = this.depthPass && (this.mClipping == UIDrawCall.Clipping.None);
            Transform transform = call.transform;
            transform.position = cachedTransform.position;
            transform.rotation = cachedTransform.rotation;
            transform.localScale = cachedTransform.lossyScale;
            index++;
        }
    }

    private void UpdateTransformMatrix()
    {
        if ((this.mUpdateTime == 0f) || (this.mMatrixTime != this.mUpdateTime))
        {
            this.mMatrixTime = this.mUpdateTime;
            this.worldToLocal = this.cachedTransform.worldToLocalMatrix;
            if (this.mClipping != UIDrawCall.Clipping.None)
            {
                Vector2 vector = new Vector2(this.mClipRange.z, this.mClipRange.w);
                if (vector.x == 0f)
                {
                    vector.x = (this.mCam != null) ? this.mCam.pixelWidth : ((float) Screen.width);
                }
                if (vector.y == 0f)
                {
                    vector.y = (this.mCam != null) ? this.mCam.pixelHeight : ((float) Screen.height);
                }
                vector = (Vector2) (vector * 0.5f);
                this.mMin.x = this.mClipRange.x - vector.x;
                this.mMin.y = this.mClipRange.y - vector.y;
                this.mMax.x = this.mClipRange.x + vector.x;
                this.mMax.y = this.mClipRange.y + vector.y;
            }
        }
    }

    public float alpha
    {
        get
        {
            return this.mAlpha;
        }
        set
        {
            float num = Mathf.Clamp01(value);
            if (this.mAlpha != num)
            {
                this.mAlpha = num;
                for (int i = 0; i < this.mDrawCalls.size; i++)
                {
                    UIDrawCall call = this.mDrawCalls[i];
                    this.MarkMaterialAsChanged(call.material, false);
                }
                for (int j = 0; j < this.mWidgets.size; j++)
                {
                    this.mWidgets[j].MarkAsChangedLite();
                }
            }
        }
    }

    public GameObject cachedGameObject
    {
        get
        {
            if (this.mGo == null)
            {
                this.mGo = base.gameObject;
            }
            return this.mGo;
        }
    }

    public Transform cachedTransform
    {
        get
        {
            if (this.mTrans == null)
            {
                this.mTrans = base.transform;
            }
            return this.mTrans;
        }
    }

    public UIDrawCall.Clipping clipping
    {
        get
        {
            return this.mClipping;
        }
        set
        {
            if (this.mClipping != value)
            {
                this.mClipping = value;
                this.mMatrixTime = 0f;
                this.UpdateDrawcalls();
            }
        }
    }

    public Vector4 clipRange
    {
        get
        {
            return this.mClipRange;
        }
        set
        {
            if (this.mClipRange != value)
            {
                this.mCullTime = (this.mCullTime != 0f) ? (Time.realtimeSinceStartup + 0.15f) : 0.001f;
                this.mClipRange = value;
                this.mMatrixTime = 0f;
                this.UpdateDrawcalls();
            }
        }
    }

    public Vector2 clipSoftness
    {
        get
        {
            return this.mClipSoftness;
        }
        set
        {
            if (this.mClipSoftness != value)
            {
                this.mClipSoftness = value;
                this.UpdateDrawcalls();
            }
        }
    }

    public DebugInfo debugInfo
    {
        get
        {
            return this.mDebugInfo;
        }
        set
        {
            if (this.mDebugInfo != value)
            {
                this.mDebugInfo = value;
                BetterList<UIDrawCall> drawCalls = this.drawCalls;
                HideFlags flags = (this.mDebugInfo != DebugInfo.Geometry) ? HideFlags.HideAndDontSave : (HideFlags.NotEditable | HideFlags.DontSave);
                int num = 0;
                int size = drawCalls.size;
                while (num < size)
                {
                    UIDrawCall call = drawCalls[num];
                    GameObject gameObject = call.gameObject;
                    NGUITools.SetActiveSelf(gameObject, false);
                    gameObject.hideFlags = flags;
                    NGUITools.SetActiveSelf(gameObject, true);
                    num++;
                }
            }
        }
    }

    public BetterList<UIDrawCall> drawCalls
    {
        get
        {
            int size = this.mDrawCalls.size;
            while (size > 0)
            {
                UIDrawCall call = this.mDrawCalls[--size];
                if (call == null)
                {
                    this.mDrawCalls.RemoveAt(size);
                }
            }
            return this.mDrawCalls;
        }
    }

    public BetterList<UIWidget> widgets
    {
        get
        {
            return this.mWidgets;
        }
    }

    public enum DebugInfo
    {
        None,
        Gizmos,
        Geometry
    }

    public delegate void OnChangeDelegate();
}

