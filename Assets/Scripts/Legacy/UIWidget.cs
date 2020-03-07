using System;
using System.Runtime.CompilerServices;
using UnityEngine;

[Obsolete]
public abstract class UIWidget : MonoBehaviour
{
    [CompilerGenerated]
    private static Comparison<UIWidget> f__amScache14;
    protected bool mChanged = true;
    [SerializeField, HideInInspector]
    private Color mColor = Color.white;
    [SerializeField, HideInInspector]
    private int mDepth;
    public Vector3 mDiffPos;
    public Quaternion mDiffRot;
    public Vector3 mDiffScale;
    private bool mForceVisible;
    protected GameObject mGo;
    private float mLastAlpha;
    private Matrix4x4 mLocalToPanel;
    [HideInInspector, SerializeField]
    protected Material mMat;
    private Vector3 mOldV0;
    private Vector3 mOldV1;
    protected object mPanel; //HACK
    [HideInInspector, SerializeField]
    private Pivot mPivot = Pivot.Center;
    protected bool mPlayMode = true;
    [HideInInspector, SerializeField]
    protected Texture mTex;
    protected Transform mTrans;
    private bool mVisibleByPanel = true;

    protected UIWidget()
    {
    }

    protected virtual void Awake()
    {
        this.mGo = base.gameObject;
        this.mPlayMode = Application.isPlaying;
    }

    public void CheckLayer()
    {
    }

    //TODO: Investigate this
    public void CreatePanel()
    {
        //if (((this.mPanel == null) && base.enabled) && (NGUITools.GetActive(base.gameObject) && (this.material != null)))
        //{
        //    this.mPanel = UIPanel.Find(this.cachedTransform);
        //    if (this.mPanel != null)
        //    {
        //        this.CheckLayer();
        //        this.mPanel.AddWidget(this);
        //        this.mChanged = true;
        //    }
        //}
    }

    public virtual void MakePixelPerfect()
    {
        Vector3 localScale = this.cachedTransform.localScale;
        int num = Mathf.RoundToInt(localScale.x);
        int num2 = Mathf.RoundToInt(localScale.y);
        localScale.x = num;
        localScale.y = num2;
        localScale.z = 1f;
        Vector3 localPosition = this.cachedTransform.localPosition;
        localPosition.z = Mathf.RoundToInt(localPosition.z);
        if (((num % 2) == 1) && (((this.pivot == Pivot.Top) || (this.pivot == Pivot.Center)) || (this.pivot == Pivot.Bottom)))
        {
            localPosition.x = Mathf.Floor(localPosition.x) + 0.5f;
        }
        else
        {
            localPosition.x = Mathf.Round(localPosition.x);
        }
        if (((num2 % 2) == 1) && (((this.pivot == Pivot.Left) || (this.pivot == Pivot.Center)) || (this.pivot == Pivot.Right)))
        {
            localPosition.y = Mathf.Ceil(localPosition.y) - 0.5f;
        }
        else
        {
            localPosition.y = Mathf.Round(localPosition.y);
        }
        this.cachedTransform.localPosition = localPosition;
        this.cachedTransform.localScale = localScale;
    }

    public virtual void MarkAsChanged()
    {
        this.mChanged = true;
        if ((((this.mPanel != null) && base.enabled) && (NGUITools.GetActive(base.gameObject) && !Application.isPlaying)) && (this.material != null))
        {
            this.CheckLayer();
        }
    }

    private void OnDisable()
    {
        if (!this.keepMaterial)
        {
            this.material = null;
        }
        this.mPanel = null;
    }

    protected virtual void OnEnable()
    {
        this.mChanged = true;
        if (!this.keepMaterial)
        {
            this.mMat = null;
            this.mTex = null;
        }
        this.mPanel = null;
    }

    public virtual void OnFill(BetterList<Vector3> verts, BetterList<Vector2> uvs, BetterList<Color32> cols)
    {
    }

    protected virtual void OnStart()
    {
    }

    private void Start()
    {
        this.OnStart();
        this.CreatePanel();
    }

    public virtual void Update()
    {
        if (this.mPanel == null)
        {
            this.CreatePanel();
        }
    }

    public float alpha
    {
        get
        {
            return this.mColor.a;
        }
        set
        {
            Color mColor = this.mColor;
            mColor.a = value;
            this.color = mColor;
        }
    }

    public virtual Vector4 border
    {
        get
        {
            return Vector4.zero;
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

    public Color color
    {
        get
        {
            return this.mColor;
        }
        set
        {
            if (!this.mColor.Equals(value))
            {
                this.mColor = value;
                this.mChanged = true;
            }
        }
    }

    public virtual bool keepMaterial
    {
        get
        {
            return false;
        }
    }

    public virtual Texture mainTexture
    {
        get
        {
            Material material = this.material;
            if (material != null)
            {
                if (material.mainTexture != null)
                {
                    this.mTex = material.mainTexture;
                }
                else if (this.mTex != null)
                {
                    //if (this.mPanel != null)
                    //{
                    //    this.mPanel.RemoveWidget(this);
                    //}
                    this.mPanel = null;
                    this.mMat.mainTexture = this.mTex;
                    if (base.enabled)
                    {
                        this.CreatePanel();
                    }
                }
            }
            return this.mTex;
        }
        set
        {
            Material material = this.material;
            if ((material == null) || (material.mainTexture != value))
            {
                //if (this.mPanel != null)
                //{
                //    this.mPanel.RemoveWidget(this);
                //}
                this.mPanel = null;
                this.mTex = value;
                material = this.material;
                if (material != null)
                {
                    material.mainTexture = value;
                    if (base.enabled)
                    {
                        this.CreatePanel();
                    }
                }
            }
        }
    }

    public virtual Material material
    {
        get
        {
            return this.mMat;
        }
        set
        {
            if (this.mMat != value)
            {
                //if ((this.mMat != null) && (this.mPanel != null))
                //{
                //    this.mPanel.RemoveWidget(this);
                //}
                this.mPanel = null;
                this.mMat = value;
                this.mTex = null;
                if (this.mMat != null)
                {
                    this.CreatePanel();
                }
            }
        }
    }

    public Pivot pivot
    {
        get
        {
            return this.mPivot;
        }
        set
        {
            if (this.mPivot != value)
            {
                Vector3 vector = NGUIMath.CalculateWidgetCorners(this)[0];
                this.mPivot = value;
                this.mChanged = true;
                Vector3 vector2 = NGUIMath.CalculateWidgetCorners(this)[0];
                Transform cachedTransform = this.cachedTransform;
                Vector3 position = cachedTransform.position;
                float z = cachedTransform.localPosition.z;
                position.x += vector.x - vector2.x;
                position.y += vector.y - vector2.y;
                this.cachedTransform.position = position;
                position = this.cachedTransform.localPosition;
                position.x = Mathf.Round(position.x);
                position.y = Mathf.Round(position.y);
                position.z = z;
                this.cachedTransform.localPosition = position;
            }
        }
    }

    public Vector2 pivotOffset
    {
        get
        {
            Vector2 zero = Vector2.zero;
            Vector4 relativePadding = this.relativePadding;
            Pivot pivot = this.pivot;
            switch (pivot)
            {
                case Pivot.Top:
                case Pivot.Center:
                case Pivot.Bottom:
                    zero.x = ((relativePadding.x - relativePadding.z) - 1f) * 0.5f;
                    break;

                default:
                    if (((pivot != Pivot.TopRight) && (pivot != Pivot.Right)) && (pivot != Pivot.BottomRight))
                    {
                        zero.x = relativePadding.x;
                    }
                    else
                    {
                        zero.x = -1f - relativePadding.z;
                    }
                    break;
            }
            switch (pivot)
            {
                case Pivot.Left:
                case Pivot.Center:
                case Pivot.Right:
                    zero.y = ((relativePadding.w - relativePadding.y) + 1f) * 0.5f;
                    return zero;
            }
            if (((pivot != Pivot.BottomLeft) && (pivot != Pivot.Bottom)) && (pivot != Pivot.BottomRight))
            {
                zero.y = -relativePadding.y;
                return zero;
            }
            zero.y = 1f + relativePadding.w;
            return zero;
        }
    }

    public virtual Vector4 relativePadding
    {
        get
        {
            return Vector4.zero;
        }
    }

    public virtual Vector2 relativeSize
    {
        get
        {
            return Vector2.one;
        }
    }

    public enum Pivot
    {
        TopLeft,
        Top,
        TopRight,
        Left,
        Center,
        Right,
        BottomLeft,
        Bottom,
        BottomRight
    }
}

