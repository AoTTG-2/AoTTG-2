using System;
using UnityEngine;

[AddComponentMenu("NGUI/UI/Texture"), ExecuteInEditMode]
public class UITexture : UIWidget
{
    private bool mCreatingMat;
    private Material mDynamicMat;
    private int mPMA = -1;
    [SerializeField, HideInInspector]
    private Rect mRect = new Rect(0f, 0f, 1f, 1f);
    [HideInInspector, SerializeField]
    private Shader mShader;
    [HideInInspector, SerializeField]
    private Texture mTexture;

    public override void MakePixelPerfect()
    {
        Texture mainTexture = this.mainTexture;
        if (mainTexture != null)
        {
            Vector3 localScale = base.cachedTransform.localScale;
            localScale.x = mainTexture.width * this.uvRect.width;
            localScale.y = mainTexture.height * this.uvRect.height;
            localScale.z = 1f;
            base.cachedTransform.localScale = localScale;
        }
        base.MakePixelPerfect();
    }

    private void OnDestroy()
    {
        NGUITools.Destroy(this.mDynamicMat);
    }

    public override void OnFill(BetterList<Vector3> verts, BetterList<Vector2> uvs, BetterList<Color32> cols)
    {
        Color c = base.color;
        c.a *= base.mPanel.alpha;
        Color32 item = !this.premultipliedAlpha ? c : NGUITools.ApplyPMA(c);
        verts.Add(new Vector3(1f, 0f, 0f));
        verts.Add(new Vector3(1f, -1f, 0f));
        verts.Add(new Vector3(0f, -1f, 0f));
        verts.Add(new Vector3(0f, 0f, 0f));
        uvs.Add(new Vector2(this.mRect.xMax, this.mRect.yMax));
        uvs.Add(new Vector2(this.mRect.xMax, this.mRect.yMin));
        uvs.Add(new Vector2(this.mRect.xMin, this.mRect.yMin));
        uvs.Add(new Vector2(this.mRect.xMin, this.mRect.yMax));
        cols.Add(item);
        cols.Add(item);
        cols.Add(item);
        cols.Add(item);
    }

    public bool hasDynamicMaterial
    {
        get
        {
            return (this.mDynamicMat != null);
        }
    }

    public override bool keepMaterial
    {
        get
        {
            return true;
        }
    }

    public override Texture mainTexture
    {
        get
        {
            return ((this.mTexture == null) ? base.mainTexture : this.mTexture);
        }
        set
        {
            if ((base.mPanel != null) && (base.mMat != null))
            {
                base.mPanel.RemoveWidget(this);
            }
            if (base.mMat == null)
            {
                this.mDynamicMat = new Material(this.shader);
                this.mDynamicMat.hideFlags = HideFlags.DontSave;
                base.mMat = this.mDynamicMat;
            }
            base.mPanel = null;
            base.mTex = value;
            this.mTexture = value;
            base.mMat.mainTexture = value;
            if (base.enabled)
            {
                base.CreatePanel();
            }
        }
    }

    public override Material material
    {
        get
        {
            if (!this.mCreatingMat && (base.mMat == null))
            {
                this.mCreatingMat = true;
                if (this.mainTexture != null)
                {
                    if (this.mShader == null)
                    {
                        this.mShader = Shader.Find("Unlit/Texture");
                    }
                    this.mDynamicMat = new Material(this.mShader);
                    this.mDynamicMat.hideFlags = HideFlags.DontSave;
                    this.mDynamicMat.mainTexture = this.mainTexture;
                    base.material = this.mDynamicMat;
                    this.mPMA = 0;
                }
                this.mCreatingMat = false;
            }
            return base.mMat;
        }
        set
        {
            if ((this.mDynamicMat != value) && (this.mDynamicMat != null))
            {
                NGUITools.Destroy(this.mDynamicMat);
                this.mDynamicMat = null;
            }
            base.material = value;
            this.mPMA = -1;
        }
    }

    public bool premultipliedAlpha
    {
        get
        {
            if (this.mPMA == -1)
            {
                Material material = this.material;
                this.mPMA = (((material == null) || (material.shader == null)) || !material.shader.name.Contains("Premultiplied")) ? 0 : 1;
            }
            return (this.mPMA == 1);
        }
    }

    public Shader shader
    {
        get
        {
            if (this.mShader == null)
            {
                Material material = this.material;
                if (material != null)
                {
                    this.mShader = material.shader;
                }
                if (this.mShader == null)
                {
                    this.mShader = Shader.Find("Unlit/Texture");
                }
            }
            return this.mShader;
        }
        set
        {
            if (this.mShader != value)
            {
                this.mShader = value;
                Material material = this.material;
                if (material != null)
                {
                    material.shader = value;
                }
                this.mPMA = -1;
            }
        }
    }

    public Rect uvRect
    {
        get
        {
            return this.mRect;
        }
        set
        {
            if (this.mRect != value)
            {
                this.mRect = value;
                this.MarkAsChanged();
            }
        }
    }
}

