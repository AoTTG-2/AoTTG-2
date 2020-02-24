using System;
using UnityEngine;

[ExecuteInEditMode, AddComponentMenu("NGUI/Internal/Draw Call")]
public class UIDrawCall : MonoBehaviour
{
    private Material mClippedMat;
    private Clipping mClipping;
    private Vector4 mClipRange;
    private Vector2 mClipSoft;
    private Material mDepthMat;
    private bool mDepthPass;
    private bool mEven = true;
    private MeshFilter mFilter;
    private int[] mIndices;
    private Mesh mMesh0;
    private Mesh mMesh1;
    private MeshRenderer mRen;
    private bool mReset = true;
    private Material mSharedMat;
    private Transform mTrans;

    private Mesh GetMesh(ref bool rebuildIndices, int vertexCount)
    {
        this.mEven = !this.mEven;
        if (this.mEven)
        {
            if (this.mMesh0 == null)
            {
                this.mMesh0 = new Mesh();
                this.mMesh0.hideFlags = HideFlags.DontSave;
                this.mMesh0.name = "Mesh0 for " + this.mSharedMat.name;
                this.mMesh0.MarkDynamic();
                rebuildIndices = true;
            }
            else if (rebuildIndices || (this.mMesh0.vertexCount != vertexCount))
            {
                rebuildIndices = true;
                this.mMesh0.Clear();
            }
            return this.mMesh0;
        }
        if (this.mMesh1 == null)
        {
            this.mMesh1 = new Mesh();
            this.mMesh1.hideFlags = HideFlags.DontSave;
            this.mMesh1.name = "Mesh1 for " + this.mSharedMat.name;
            this.mMesh1.MarkDynamic();
            rebuildIndices = true;
        }
        else if (rebuildIndices || (this.mMesh1.vertexCount != vertexCount))
        {
            rebuildIndices = true;
            this.mMesh1.Clear();
        }
        return this.mMesh1;
    }

    private void OnDestroy()
    {
        NGUITools.DestroyImmediate(this.mMesh0);
        NGUITools.DestroyImmediate(this.mMesh1);
        NGUITools.DestroyImmediate(this.mClippedMat);
        NGUITools.DestroyImmediate(this.mDepthMat);
    }

    private void OnWillRenderObject()
    {
        if (this.mReset)
        {
            this.mReset = false;
            this.UpdateMaterials();
        }
        if (this.mClippedMat != null)
        {
            this.mClippedMat.mainTextureOffset = new Vector2(-this.mClipRange.x / this.mClipRange.z, -this.mClipRange.y / this.mClipRange.w);
            this.mClippedMat.mainTextureScale = new Vector2(1f / this.mClipRange.z, 1f / this.mClipRange.w);
            Vector2 vector = new Vector2(1000f, 1000f);
            if (this.mClipSoft.x > 0f)
            {
                vector.x = this.mClipRange.z / this.mClipSoft.x;
            }
            if (this.mClipSoft.y > 0f)
            {
                vector.y = this.mClipRange.w / this.mClipSoft.y;
            }
            this.mClippedMat.SetVector("_ClipSharpness", vector);
        }
    }

    public void Set(BetterList<Vector3> verts, BetterList<Vector3> norms, BetterList<Vector4> tans, BetterList<Vector2> uvs, BetterList<Color32> cols)
    {
        int size = verts.size;
        if (((size > 0) && (size == uvs.size)) && ((size == cols.size) && ((size % 4) == 0)))
        {
            if (this.mFilter == null)
            {
                this.mFilter = base.gameObject.GetComponent<MeshFilter>();
            }
            if (this.mFilter == null)
            {
                this.mFilter = base.gameObject.AddComponent<MeshFilter>();
            }
            if (this.mRen == null)
            {
                this.mRen = base.gameObject.GetComponent<MeshRenderer>();
            }
            if (this.mRen == null)
            {
                this.mRen = base.gameObject.AddComponent<MeshRenderer>();
                this.UpdateMaterials();
            }
            else if ((this.mClippedMat != null) && (this.mClippedMat.mainTexture != this.mSharedMat.mainTexture))
            {
                this.UpdateMaterials();
            }
            if (verts.size < 0xfde8)
            {
                bool flag;
                int num2 = (size >> 1) * 3;
                if (flag = (this.mIndices == null) || (this.mIndices.Length != num2))
                {
                    this.mIndices = new int[num2];
                    int num3 = 0;
                    for (int i = 0; i < size; i += 4)
                    {
                        this.mIndices[num3++] = i;
                        this.mIndices[num3++] = i + 1;
                        this.mIndices[num3++] = i + 2;
                        this.mIndices[num3++] = i + 2;
                        this.mIndices[num3++] = i + 3;
                        this.mIndices[num3++] = i;
                    }
                }
                Mesh mesh = this.GetMesh(ref flag, verts.size);
                mesh.vertices = verts.ToArray();
                if (norms != null)
                {
                    mesh.normals = norms.ToArray();
                }
                if (tans != null)
                {
                    mesh.tangents = tans.ToArray();
                }
                mesh.uv = uvs.ToArray();
                mesh.colors32 = cols.ToArray();
                if (flag)
                {
                    mesh.triangles = this.mIndices;
                }
                mesh.RecalculateBounds();
                this.mFilter.mesh = mesh;
            }
            else
            {
                if (this.mFilter.mesh != null)
                {
                    this.mFilter.mesh.Clear();
                }
                Debug.LogError("Too many vertices on one panel: " + verts.size);
            }
        }
        else
        {
            if (this.mFilter.mesh != null)
            {
                this.mFilter.mesh.Clear();
            }
            Debug.LogError("UIWidgets must fill the buffer with 4 vertices per quad. Found " + size);
        }
    }

    private void UpdateMaterials()
    {
        if (this.mClipping != Clipping.None)
        {
            Shader shader = null;
            if (this.mClipping != Clipping.None)
            {
                string str = this.mSharedMat.shader.name.Replace(" (AlphaClip)", string.Empty).Replace(" (SoftClip)", string.Empty);
                if ((this.mClipping != Clipping.HardClip) && (this.mClipping != Clipping.AlphaClip))
                {
                    if (this.mClipping == Clipping.SoftClip)
                    {
                        shader = Shader.Find(str + " (SoftClip)");
                    }
                }
                else
                {
                    shader = Shader.Find(str + " (AlphaClip)");
                }
                if (shader == null)
                {
                    this.mClipping = Clipping.None;
                }
            }
            if (shader != null)
            {
                if (this.mClippedMat == null)
                {
                    this.mClippedMat = new Material(this.mSharedMat);
                    this.mClippedMat.hideFlags = HideFlags.DontSave;
                }
                this.mClippedMat.shader = shader;
                this.mClippedMat.CopyPropertiesFromMaterial(this.mSharedMat);
            }
            else if (this.mClippedMat != null)
            {
                NGUITools.Destroy(this.mClippedMat);
                this.mClippedMat = null;
            }
        }
        else if (this.mClippedMat != null)
        {
            NGUITools.Destroy(this.mClippedMat);
            this.mClippedMat = null;
        }
        if (this.mDepthPass)
        {
            if (this.mDepthMat == null)
            {
                Shader shader2 = Shader.Find("Unlit/Depth Cutout");
                this.mDepthMat = new Material(shader2);
                this.mDepthMat.hideFlags = HideFlags.DontSave;
            }
            this.mDepthMat.mainTexture = this.mSharedMat.mainTexture;
        }
        else if (this.mDepthMat != null)
        {
            NGUITools.Destroy(this.mDepthMat);
            this.mDepthMat = null;
        }
        Material material = (this.mClippedMat == null) ? this.mSharedMat : this.mClippedMat;
        if (this.mDepthMat != null)
        {
            if (((this.mRen.sharedMaterials == null) || (this.mRen.sharedMaterials.Length != 2)) || (this.mRen.sharedMaterials[1] != material))
            {
                this.mRen.sharedMaterials = new Material[] { this.mDepthMat, material };
            }
        }
        else if (this.mRen.sharedMaterial != material)
        {
            this.mRen.sharedMaterials = new Material[] { material };
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

    public Clipping clipping
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
                this.mReset = true;
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
            this.mClipRange = value;
        }
    }

    public Vector2 clipSoftness
    {
        get
        {
            return this.mClipSoft;
        }
        set
        {
            this.mClipSoft = value;
        }
    }

    public bool depthPass
    {
        get
        {
            return this.mDepthPass;
        }
        set
        {
            if (this.mDepthPass != value)
            {
                this.mDepthPass = value;
                this.mReset = true;
            }
        }
    }

    public bool isClipped
    {
        get
        {
            return (this.mClippedMat != null);
        }
    }

    public Material material
    {
        get
        {
            return this.mSharedMat;
        }
        set
        {
            this.mSharedMat = value;
        }
    }

    public int triangles
    {
        get
        {
            Mesh mesh = !this.mEven ? this.mMesh1 : this.mMesh0;
            return ((mesh == null) ? 0 : (mesh.vertexCount >> 1));
        }
    }

    public enum Clipping
    {
        None,
        HardClip,
        AlphaClip,
        SoftClip
    }
}

