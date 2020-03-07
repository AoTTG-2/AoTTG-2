using System;
using UnityEngine;

public class VertexPool
{
    public const int BlockSize = 0x24;
    public float BoundsScheduleTime = 1f;
    public bool ColorChanged;
    public Color[] Colors;
    public float ElapsedTime;
    protected bool FirstUpdate = true;
    protected int IndexTotal;
    protected int IndexUsed;
    public bool IndiceChanged;
    public int[] Indices;
    public UnityEngine.Material Material;
    public UnityEngine.Mesh Mesh;
    public bool UVChanged;
    public Vector2[] UVs;
    public bool VertChanged;
    protected bool VertCountChanged = false;
    protected int VertexTotal = 0;
    protected int VertexUsed = 0;
    public Vector3[] Vertices;

    public VertexPool(UnityEngine.Mesh mesh, UnityEngine.Material material)
    {
        this.Mesh = mesh;
        this.Material = material;
        this.InitArrays();
        this.Vertices = this.Mesh.vertices;
        this.Indices = this.Mesh.triangles;
        this.Colors = this.Mesh.colors;
        this.UVs = this.Mesh.uv;
        this.VertChanged = true;
        this.UVChanged = true;
        this.ColorChanged = true;
        this.IndiceChanged = true;
    }

    public RibbonTrail AddRibbonTrail(float width, int maxelemnt, float len, Vector3 pos, int stretchType, float maxFps)
    {
        return new RibbonTrail(this.GetVertices(maxelemnt * 2, (maxelemnt - 1) * 6), width, maxelemnt, len, pos, stretchType, maxFps);
    }

    public Sprite AddSprite(float width, float height, STYPE type, ORIPOINT ori, Camera cam, int uvStretch, float maxFps)
    {
        return new Sprite(this.GetVertices(4, 6), width, height, type, ori, cam, uvStretch, maxFps);
    }

    public void EnlargeArrays(int count, int icount)
    {
        Vector3[] vertices = this.Vertices;
        this.Vertices = new Vector3[this.Vertices.Length + count];
        vertices.CopyTo(this.Vertices, 0);
        Vector2[] uVs = this.UVs;
        this.UVs = new Vector2[this.UVs.Length + count];
        uVs.CopyTo(this.UVs, 0);
        Color[] colors = this.Colors;
        this.Colors = new Color[this.Colors.Length + count];
        colors.CopyTo(this.Colors, 0);
        int[] indices = this.Indices;
        this.Indices = new int[this.Indices.Length + icount];
        indices.CopyTo(this.Indices, 0);
        this.VertCountChanged = true;
        this.IndiceChanged = true;
        this.ColorChanged = true;
        this.UVChanged = true;
        this.VertChanged = true;
    }

    public UnityEngine.Material GetMaterial()
    {
        return this.Material;
    }

    public VertexSegment GetVertices(int vcount, int icount)
    {
        int count = 0;
        int num2 = 0;
        if ((this.VertexUsed + vcount) >= this.VertexTotal)
        {
            count = ((vcount / 0x24) + 1) * 0x24;
        }
        if ((this.IndexUsed + icount) >= this.IndexTotal)
        {
            num2 = ((icount / 0x24) + 1) * 0x24;
        }
        this.VertexUsed += vcount;
        this.IndexUsed += icount;
        if ((count != 0) || (num2 != 0))
        {
            this.EnlargeArrays(count, num2);
            this.VertexTotal += count;
            this.IndexTotal += num2;
        }
        return new VertexSegment(this.VertexUsed - vcount, vcount, this.IndexUsed - icount, icount, this);
    }

    protected void InitArrays()
    {
        this.Vertices = new Vector3[4];
        this.UVs = new Vector2[4];
        this.Colors = new Color[4];
        this.Indices = new int[6];
        this.VertexTotal = 4;
        this.IndexTotal = 6;
    }

    public void LateUpdate()
    {
        if (this.VertCountChanged)
        {
            this.Mesh.Clear();
        }
        this.Mesh.vertices = this.Vertices;
        if (this.UVChanged)
        {
            this.Mesh.uv = this.UVs;
        }
        if (this.ColorChanged)
        {
            this.Mesh.colors = this.Colors;
        }
        if (this.IndiceChanged)
        {
            this.Mesh.triangles = this.Indices;
        }
        this.ElapsedTime += Time.deltaTime;
        if ((this.ElapsedTime > this.BoundsScheduleTime) || this.FirstUpdate)
        {
            this.RecalculateBounds();
            this.ElapsedTime = 0f;
        }
        if (this.ElapsedTime > this.BoundsScheduleTime)
        {
            this.FirstUpdate = false;
        }
        this.VertCountChanged = false;
        this.IndiceChanged = false;
        this.ColorChanged = false;
        this.UVChanged = false;
        this.VertChanged = false;
    }

    public void RecalculateBounds()
    {
        this.Mesh.RecalculateBounds();
    }

    public class VertexSegment
    {
        public int IndexCount;
        public int IndexStart;
        public VertexPool Pool;
        public int VertCount;
        public int VertStart;

        public VertexSegment(int start, int count, int istart, int icount, VertexPool pool)
        {
            this.VertStart = start;
            this.VertCount = count;
            this.IndexCount = icount;
            this.IndexStart = istart;
            this.Pool = pool;
        }
    }
}

