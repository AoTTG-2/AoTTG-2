using System;
using UnityEngine;

public class Sprite
{
    public UnityEngine.Color Color;
    protected bool ColorChanged = false;
    protected float ElapsedTime;
    protected float Fps;
    protected Matrix4x4 LastMat;
    private Matrix4x4 LocalMat;
    protected Vector2 LowerLeftUV;
    public Camera MainCamera;
    public STransform MyTransform;
    private ORIPOINT OriPoint;
    protected Vector3 RotateAxis;
    private Quaternion Rotation;
    private Vector3 ScaleVector;
    private STYPE Type;
    protected bool UVChanged = false;
    protected Vector2 UVDimensions;
    private int UVStretch;
    public Vector3 v1 = Vector3.zero;
    public Vector3 v2 = Vector3.zero;
    public Vector3 v3 = Vector3.zero;
    public Vector3 v4 = Vector3.zero;
    protected VertexPool.VertexSegment Vertexsegment;
    private Matrix4x4 WorldMat;

    public Sprite(VertexPool.VertexSegment segment, float width, float height, STYPE type, ORIPOINT oripoint, Camera cam, int uvStretch, float maxFps)
    {
        this.MyTransform.position = Vector3.zero;
        this.MyTransform.rotation = Quaternion.identity;
        this.LocalMat = this.WorldMat = Matrix4x4.identity;
        this.Vertexsegment = segment;
        this.UVStretch = uvStretch;
        this.LastMat = Matrix4x4.identity;
        this.ElapsedTime = 0f;
        this.Fps = 1f / maxFps;
        this.OriPoint = oripoint;
        this.RotateAxis = Vector3.zero;
        this.SetSizeXZ(width, height);
        this.RotateAxis.y = 1f;
        this.Type = type;
        this.MainCamera = cam;
        this.ResetSegment();
    }

    public void Init(UnityEngine.Color color, Vector2 lowerLeftUV, Vector2 uvDimensions)
    {
        this.SetUVCoord(lowerLeftUV, uvDimensions);
        this.SetColor(color);
        this.SetRotation(Quaternion.identity);
        this.SetScale(1f, 1f);
        this.SetRotation((float) 0f);
    }

    public void Reset()
    {
        this.MyTransform.Reset();
        this.SetColor(UnityEngine.Color.white);
        this.SetUVCoord(Vector2.zero, Vector2.zero);
        this.ScaleVector = Vector3.one;
        this.Rotation = Quaternion.identity;
        VertexPool pool = this.Vertexsegment.Pool;
        int vertStart = this.Vertexsegment.VertStart;
        pool.Vertices[vertStart] = Vector3.zero;
        pool.Vertices[vertStart + 1] = Vector3.zero;
        pool.Vertices[vertStart + 2] = Vector3.zero;
        pool.Vertices[vertStart + 3] = Vector3.zero;
    }

    public void ResetSegment()
    {
        VertexPool pool = this.Vertexsegment.Pool;
        int indexStart = this.Vertexsegment.IndexStart;
        int vertStart = this.Vertexsegment.VertStart;
        pool.Indices[indexStart] = vertStart;
        pool.Indices[indexStart + 1] = vertStart + 3;
        pool.Indices[indexStart + 2] = vertStart + 1;
        pool.Indices[indexStart + 3] = vertStart + 3;
        pool.Indices[indexStart + 4] = vertStart + 2;
        pool.Indices[indexStart + 5] = vertStart + 1;
        pool.Vertices[vertStart] = Vector3.zero;
        pool.Vertices[vertStart + 1] = Vector3.zero;
        pool.Vertices[vertStart + 2] = Vector3.zero;
        pool.Vertices[vertStart + 3] = Vector3.zero;
        pool.Colors[vertStart] = UnityEngine.Color.white;
        pool.Colors[vertStart + 1] = UnityEngine.Color.white;
        pool.Colors[vertStart + 2] = UnityEngine.Color.white;
        pool.Colors[vertStart + 3] = UnityEngine.Color.white;
        pool.UVs[vertStart] = Vector2.zero;
        pool.UVs[vertStart + 1] = Vector2.zero;
        pool.UVs[vertStart + 2] = Vector2.zero;
        pool.UVs[vertStart + 3] = Vector2.zero;
        pool.VertChanged = true;
        pool.ColorChanged = true;
        pool.IndiceChanged = true;
        pool.UVChanged = true;
    }

    public void SetColor(UnityEngine.Color c)
    {
        this.Color = c;
        this.ColorChanged = true;
    }

    public void SetPosition(Vector3 pos)
    {
        this.MyTransform.position = pos;
    }

    public void SetRotation(float angle)
    {
        this.Rotation = Quaternion.AngleAxis(angle, this.RotateAxis);
    }

    public void SetRotation(Quaternion q)
    {
        this.MyTransform.rotation = q;
    }

    public void SetRotationFaceTo(Vector3 dir)
    {
        this.MyTransform.rotation = Quaternion.FromToRotation(Vector3.up, dir);
    }

    public void SetRotationTo(Vector3 dir)
    {
        if (dir != Vector3.zero)
        {
            Quaternion identity = Quaternion.identity;
            Vector3 toDirection = dir;
            toDirection.y = 0f;
            if (toDirection == Vector3.zero)
            {
                toDirection = Vector3.up;
            }
            if (this.OriPoint == ORIPOINT.CENTER)
            {
                Quaternion quaternion2 = Quaternion.FromToRotation(new Vector3(0f, 0f, 1f), toDirection);
                identity = Quaternion.FromToRotation(toDirection, dir) * quaternion2;
            }
            else if (this.OriPoint == ORIPOINT.LEFT_UP)
            {
                Quaternion quaternion4 = Quaternion.FromToRotation(this.LocalMat.MultiplyPoint3x4(this.v3), toDirection);
                identity = Quaternion.FromToRotation(toDirection, dir) * quaternion4;
            }
            else if (this.OriPoint == ORIPOINT.LEFT_BOTTOM)
            {
                Quaternion quaternion6 = Quaternion.FromToRotation(this.LocalMat.MultiplyPoint3x4(this.v4), toDirection);
                identity = Quaternion.FromToRotation(toDirection, dir) * quaternion6;
            }
            else if (this.OriPoint == ORIPOINT.RIGHT_BOTTOM)
            {
                Quaternion quaternion8 = Quaternion.FromToRotation(this.LocalMat.MultiplyPoint3x4(this.v1), toDirection);
                identity = Quaternion.FromToRotation(toDirection, dir) * quaternion8;
            }
            else if (this.OriPoint == ORIPOINT.RIGHT_UP)
            {
                Quaternion quaternion10 = Quaternion.FromToRotation(this.LocalMat.MultiplyPoint3x4(this.v2), toDirection);
                identity = Quaternion.FromToRotation(toDirection, dir) * quaternion10;
            }
            else if (this.OriPoint == ORIPOINT.BOTTOM_CENTER)
            {
                Quaternion quaternion12 = Quaternion.FromToRotation(new Vector3(0f, 0f, 1f), toDirection);
                identity = Quaternion.FromToRotation(toDirection, dir) * quaternion12;
            }
            else if (this.OriPoint == ORIPOINT.TOP_CENTER)
            {
                Quaternion quaternion14 = Quaternion.FromToRotation(new Vector3(0f, 0f, -1f), toDirection);
                identity = Quaternion.FromToRotation(toDirection, dir) * quaternion14;
            }
            else if (this.OriPoint == ORIPOINT.RIGHT_CENTER)
            {
                Quaternion quaternion16 = Quaternion.FromToRotation(new Vector3(-1f, 0f, 0f), toDirection);
                identity = Quaternion.FromToRotation(toDirection, dir) * quaternion16;
            }
            else if (this.OriPoint == ORIPOINT.LEFT_CENTER)
            {
                Quaternion quaternion18 = Quaternion.FromToRotation(new Vector3(1f, 0f, 0f), toDirection);
                identity = Quaternion.FromToRotation(toDirection, dir) * quaternion18;
            }
            this.MyTransform.rotation = identity;
        }
    }

    public void SetScale(float width, float height)
    {
        this.ScaleVector.x = width;
        this.ScaleVector.z = height;
    }

    public void SetSizeXZ(float width, float height)
    {
        this.v1 = new Vector3(-width / 2f, 0f, height / 2f);
        this.v2 = new Vector3(-width / 2f, 0f, -height / 2f);
        this.v3 = new Vector3(width / 2f, 0f, -height / 2f);
        this.v4 = new Vector3(width / 2f, 0f, height / 2f);
        Vector3 zero = Vector3.zero;
        if (this.OriPoint == ORIPOINT.LEFT_UP)
        {
            zero = this.v3;
        }
        else if (this.OriPoint == ORIPOINT.LEFT_BOTTOM)
        {
            zero = this.v4;
        }
        else if (this.OriPoint == ORIPOINT.RIGHT_BOTTOM)
        {
            zero = this.v1;
        }
        else if (this.OriPoint == ORIPOINT.RIGHT_UP)
        {
            zero = this.v2;
        }
        else if (this.OriPoint == ORIPOINT.BOTTOM_CENTER)
        {
            zero = new Vector3(0f, 0f, height / 2f);
        }
        else if (this.OriPoint == ORIPOINT.TOP_CENTER)
        {
            zero = new Vector3(0f, 0f, -height / 2f);
        }
        else if (this.OriPoint == ORIPOINT.LEFT_CENTER)
        {
            zero = new Vector3(width / 2f, 0f, 0f);
        }
        else if (this.OriPoint == ORIPOINT.RIGHT_CENTER)
        {
            zero = new Vector3(-width / 2f, 0f, 0f);
        }
        this.v1 += zero;
        this.v2 += zero;
        this.v3 += zero;
        this.v4 += zero;
    }

    public void SetUVCoord(Vector2 lowerleft, Vector2 dimensions)
    {
        this.LowerLeftUV = lowerleft;
        this.UVDimensions = dimensions;
        this.UVChanged = true;
    }

    public void Transform()
    {
        this.LocalMat.SetTRS(Vector3.zero, this.Rotation, this.ScaleVector);
        if (this.Type == STYPE.BILLBOARD)
        {
            UnityEngine.Transform transform = this.MainCamera.transform;
            this.MyTransform.LookAt(this.MyTransform.position + (transform.rotation * Vector3.up), (Vector3) (transform.rotation * Vector3.back));
        }
        this.WorldMat.SetTRS(this.MyTransform.position, this.MyTransform.rotation, Vector3.one);
        Matrix4x4 matrixx = this.WorldMat * this.LocalMat;
        VertexPool pool = this.Vertexsegment.Pool;
        int vertStart = this.Vertexsegment.VertStart;
        Vector3 vector = matrixx.MultiplyPoint3x4(this.v1);
        Vector3 vector2 = matrixx.MultiplyPoint3x4(this.v2);
        Vector3 vector3 = matrixx.MultiplyPoint3x4(this.v3);
        Vector3 vector4 = matrixx.MultiplyPoint3x4(this.v4);
        if (this.Type == STYPE.BILLBOARD_SELF)
        {
            Vector3 zero = Vector3.zero;
            Vector3 vector6 = Vector3.zero;
            float magnitude = 0f;
            if (this.UVStretch == 0)
            {
                zero = (Vector3) ((vector + vector4) / 2f);
                vector6 = (Vector3) ((vector2 + vector3) / 2f);
                Vector3 vector7 = vector4 - vector;
                magnitude = vector7.magnitude;
            }
            else
            {
                zero = (Vector3) ((vector + vector2) / 2f);
                vector6 = (Vector3) ((vector4 + vector3) / 2f);
                Vector3 vector8 = vector2 - vector;
                magnitude = vector8.magnitude;
            }
            Vector3 lhs = zero - vector6;
            Vector3 rhs = this.MainCamera.transform.position - zero;
            Vector3 vector11 = Vector3.Cross(lhs, rhs);
            vector11.Normalize();
            vector11 = (Vector3) (vector11 * (magnitude * 0.5f));
            Vector3 vector12 = this.MainCamera.transform.position - vector6;
            Vector3 vector13 = Vector3.Cross(lhs, vector12);
            vector13.Normalize();
            vector13 = (Vector3) (vector13 * (magnitude * 0.5f));
            if (this.UVStretch == 0)
            {
                vector = zero - vector11;
                vector4 = zero + vector11;
                vector2 = vector6 - vector13;
                vector3 = vector6 + vector13;
            }
            else
            {
                vector = zero - vector11;
                vector2 = zero + vector11;
                vector4 = vector6 - vector13;
                vector3 = vector6 + vector13;
            }
        }
        pool.Vertices[vertStart] = vector;
        pool.Vertices[vertStart + 1] = vector2;
        pool.Vertices[vertStart + 2] = vector3;
        pool.Vertices[vertStart + 3] = vector4;
    }

    public void Update(bool force)
    {
        this.ElapsedTime += Time.deltaTime;
        if ((this.ElapsedTime > this.Fps) || force)
        {
            this.Transform();
            if (this.UVChanged)
            {
                this.UpdateUV();
            }
            if (this.ColorChanged)
            {
                this.UpdateColor();
            }
            this.ColorChanged = false;
            this.UVChanged = false;
            if (!force)
            {
                this.ElapsedTime -= this.Fps;
            }
        }
    }

    public void UpdateColor()
    {
        VertexPool pool = this.Vertexsegment.Pool;
        int vertStart = this.Vertexsegment.VertStart;
        pool.Colors[vertStart] = this.Color;
        pool.Colors[vertStart + 1] = this.Color;
        pool.Colors[vertStart + 2] = this.Color;
        pool.Colors[vertStart + 3] = this.Color;
        this.Vertexsegment.Pool.ColorChanged = true;
    }

    public void UpdateUV()
    {
        VertexPool pool = this.Vertexsegment.Pool;
        int vertStart = this.Vertexsegment.VertStart;
        if (this.UVDimensions.y > 0f)
        {
            pool.UVs[vertStart] = this.LowerLeftUV + ((Vector2) (Vector2.up * this.UVDimensions.y));
            pool.UVs[vertStart + 1] = this.LowerLeftUV;
            pool.UVs[vertStart + 2] = this.LowerLeftUV + ((Vector2) (Vector2.right * this.UVDimensions.x));
            pool.UVs[vertStart + 3] = this.LowerLeftUV + this.UVDimensions;
        }
        else
        {
            pool.UVs[vertStart] = this.LowerLeftUV;
            pool.UVs[vertStart + 1] = this.LowerLeftUV + ((Vector2) (Vector2.up * this.UVDimensions.y));
            pool.UVs[vertStart + 2] = this.LowerLeftUV + this.UVDimensions;
            pool.UVs[vertStart + 3] = this.LowerLeftUV + ((Vector2) (Vector2.right * this.UVDimensions.x));
        }
        this.Vertexsegment.Pool.UVChanged = true;
    }
}

