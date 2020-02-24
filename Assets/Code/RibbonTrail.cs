using System;
using UnityEngine;

public class RibbonTrail
{
    public const int CHAIN_EMPTY = 0x1869f;
    protected UnityEngine.Color Color = UnityEngine.Color.white;
    protected float ElapsedTime;
    public int ElemCount;
    public Element[] ElementArray;
    protected float ElemLength;
    protected float Fps;
    public int Head;
    protected Vector3 HeadPosition;
    protected bool IndexDirty;
    protected Vector2 LowerLeftUV;
    public int MaxElements;
    public float SquaredElemLength;
    protected int StretchType;
    public int Tail;
    protected float TrailLength;
    protected float UnitWidth;
    protected Vector2 UVDimensions;
    protected VertexPool.VertexSegment Vertexsegment;

    public RibbonTrail(VertexPool.VertexSegment segment, float width, int maxelemnt, float len, Vector3 pos, int stretchType, float maxFps)
    {
        if (maxelemnt <= 2)
        {
            Debug.LogError("ribbon trail's maxelement should > 2!");
        }
        this.MaxElements = maxelemnt;
        this.Vertexsegment = segment;
        this.ElementArray = new Element[this.MaxElements];
        this.Tail = 0x1869f;
        this.Head = 0x1869f;
        this.SetTrailLen(len);
        this.UnitWidth = width;
        this.HeadPosition = pos;
        this.StretchType = stretchType;
        Element dtls = new Element(this.HeadPosition, this.UnitWidth);
        this.IndexDirty = false;
        this.Fps = 1f / maxFps;
        this.AddElememt(dtls);
        Element element2 = new Element(this.HeadPosition, this.UnitWidth);
        this.AddElememt(element2);
    }

    public void AddElememt(Element dtls)
    {
        if (this.Head == 0x1869f)
        {
            this.Tail = this.MaxElements - 1;
            this.Head = this.Tail;
            this.IndexDirty = true;
            this.ElemCount++;
        }
        else
        {
            if (this.Head == 0)
            {
                this.Head = this.MaxElements - 1;
            }
            else
            {
                this.Head--;
            }
            if (this.Head == this.Tail)
            {
                if (this.Tail == 0)
                {
                    this.Tail = this.MaxElements - 1;
                }
                else
                {
                    this.Tail--;
                }
            }
            else
            {
                this.ElemCount++;
            }
        }
        this.ElementArray[this.Head] = dtls;
        this.IndexDirty = true;
    }

    public void Reset()
    {
        this.ResetElementsPos();
    }

    public void ResetElementsPos()
    {
        if ((this.Head != 0x1869f) && (this.Head != this.Tail))
        {
            int head = this.Head;
            while (true)
            {
                int index = head;
                if (index == this.MaxElements)
                {
                    index = 0;
                }
                this.ElementArray[index].Position = this.HeadPosition;
                if (index == this.Tail)
                {
                    return;
                }
                head = index + 1;
            }
        }
    }

    public void SetColor(UnityEngine.Color color)
    {
        this.Color = color;
    }

    public void SetHeadPosition(Vector3 pos)
    {
        this.HeadPosition = pos;
    }

    public void SetTrailLen(float len)
    {
        this.TrailLength = len;
        this.ElemLength = this.TrailLength / ((float) (this.MaxElements - 1));
        this.SquaredElemLength = this.ElemLength * this.ElemLength;
    }

    public void SetUVCoord(Vector2 lowerleft, Vector2 dimensions)
    {
        this.LowerLeftUV = lowerleft;
        this.UVDimensions = dimensions;
    }

    public void Smooth()
    {
        if (this.ElemCount > 3)
        {
            Element element = this.ElementArray[this.Head];
            int index = this.Head + 1;
            if (index == this.MaxElements)
            {
                index = 0;
            }
            int num2 = index + 1;
            if (num2 == this.MaxElements)
            {
                num2 = 0;
            }
            Element element2 = this.ElementArray[index];
            Element element3 = this.ElementArray[num2];
            Vector3 from = element.Position - element2.Position;
            Vector3 to = element2.Position - element3.Position;
            float num3 = Vector3.Angle(from, to);
            if (num3 > 60f)
            {
                Vector3 vector3 = (Vector3) ((element.Position + element3.Position) / 2f);
                Vector3 vector4 = vector3 - element2.Position;
                Vector3 zero = Vector3.zero;
                float smoothTime = 0.1f / (num3 / 60f);
                element2.Position = Vector3.SmoothDamp(element2.Position, element2.Position + ((Vector3) (vector4.normalized * element2.Width)), ref zero, smoothTime);
            }
        }
    }

    public void Update()
    {
        this.ElapsedTime += Time.deltaTime;
        if (this.ElapsedTime >= this.Fps)
        {
            this.ElapsedTime -= this.Fps;
            bool flag = false;
            while (!flag)
            {
                Element element = this.ElementArray[this.Head];
                int index = this.Head + 1;
                if (index == this.MaxElements)
                {
                    index = 0;
                }
                Element element2 = this.ElementArray[index];
                Vector3 headPosition = this.HeadPosition;
                Vector3 vector2 = headPosition - element2.Position;
                if (vector2.sqrMagnitude >= this.SquaredElemLength)
                {
                    Vector3 vector3 = (Vector3) (vector2 * (this.ElemLength / vector2.magnitude));
                    element.Position = element2.Position + vector3;
                    Element dtls = new Element(headPosition, this.UnitWidth);
                    this.AddElememt(dtls);
                    vector2 = headPosition - element.Position;
                    if (vector2.sqrMagnitude <= this.SquaredElemLength)
                    {
                        flag = true;
                    }
                }
                else
                {
                    element.Position = headPosition;
                    flag = true;
                }
                if (((this.Tail + 1) % this.MaxElements) == this.Head)
                {
                    int num3;
                    Element element4 = this.ElementArray[this.Tail];
                    if (this.Tail == 0)
                    {
                        num3 = this.MaxElements - 1;
                    }
                    else
                    {
                        num3 = this.Tail - 1;
                    }
                    Element element5 = this.ElementArray[num3];
                    Vector3 vector4 = element4.Position - element5.Position;
                    float magnitude = vector4.magnitude;
                    if (magnitude > 1E-06)
                    {
                        float num5 = this.ElemLength - vector2.magnitude;
                        vector4 = (Vector3) (vector4 * (num5 / magnitude));
                        element4.Position = element5.Position + vector4;
                    }
                }
            }
            Vector3 position = Camera.main.transform.position;
            this.UpdateVertices(position);
            this.UpdateIndices();
        }
    }

    public void UpdateIndices()
    {
        if (this.IndexDirty)
        {
            VertexPool pool = this.Vertexsegment.Pool;
            if ((this.Head != 0x1869f) && (this.Head != this.Tail))
            {
                int head = this.Head;
                int num2 = 0;
                while (true)
                {
                    int num3 = head + 1;
                    if (num3 == this.MaxElements)
                    {
                        num3 = 0;
                    }
                    if ((num3 * 2) >= 0x10000)
                    {
                        Debug.LogError("Too many elements!");
                    }
                    int num4 = this.Vertexsegment.VertStart + (num3 * 2);
                    int num5 = this.Vertexsegment.VertStart + (head * 2);
                    int index = this.Vertexsegment.IndexStart + (num2 * 6);
                    pool.Indices[index] = num5;
                    pool.Indices[index + 1] = num5 + 1;
                    pool.Indices[index + 2] = num4;
                    pool.Indices[index + 3] = num5 + 1;
                    pool.Indices[index + 4] = num4 + 1;
                    pool.Indices[index + 5] = num4;
                    if (num3 == this.Tail)
                    {
                        pool.IndiceChanged = true;
                        break;
                    }
                    head = num3;
                    num2++;
                }
            }
            this.IndexDirty = false;
        }
    }

    public void UpdateVertices(Vector3 eyePos)
    {
        float num = 0f;
        float num2 = 0f;
        float num3 = this.ElemLength * (this.MaxElements - 2);
        if ((this.Head != 0x1869f) && (this.Head != this.Tail))
        {
            int head = this.Head;
            int index = this.Head;
            while (true)
            {
                Vector3 vector;
                if (index == this.MaxElements)
                {
                    index = 0;
                }
                Element element = this.ElementArray[index];
                if ((index * 2) >= 0x10000)
                {
                    Debug.LogError("Too many elements!");
                }
                int num6 = this.Vertexsegment.VertStart + (index * 2);
                int num7 = index + 1;
                if (num7 == this.MaxElements)
                {
                    num7 = 0;
                }
                if (index == this.Head)
                {
                    vector = this.ElementArray[num7].Position - element.Position;
                }
                else if (index == this.Tail)
                {
                    vector = element.Position - this.ElementArray[head].Position;
                }
                else
                {
                    vector = this.ElementArray[num7].Position - this.ElementArray[head].Position;
                }
                Vector3 rhs = eyePos - element.Position;
                Vector3 vector3 = Vector3.Cross(vector, rhs);
                vector3.Normalize();
                vector3 = (Vector3) (vector3 * (element.Width * 0.5f));
                Vector3 vector4 = element.Position - vector3;
                Vector3 vector5 = element.Position + vector3;
                VertexPool pool = this.Vertexsegment.Pool;
                if (this.StretchType == 0)
                {
                    num = (num2 / num3) * Mathf.Abs(this.UVDimensions.y);
                }
                else
                {
                    num = (num2 / num3) * Mathf.Abs(this.UVDimensions.x);
                }
                Vector2 zero = Vector2.zero;
                pool.Vertices[num6] = vector4;
                pool.Colors[num6] = this.Color;
                if (this.StretchType == 0)
                {
                    zero.x = this.LowerLeftUV.x + this.UVDimensions.x;
                    zero.y = this.LowerLeftUV.y - num;
                }
                else
                {
                    zero.x = this.LowerLeftUV.x + num;
                    zero.y = this.LowerLeftUV.y;
                }
                pool.UVs[num6] = zero;
                pool.Vertices[num6 + 1] = vector5;
                pool.Colors[num6 + 1] = this.Color;
                if (this.StretchType == 0)
                {
                    zero.x = this.LowerLeftUV.x;
                    zero.y = this.LowerLeftUV.y - num;
                }
                else
                {
                    zero.x = this.LowerLeftUV.x + num;
                    zero.y = this.LowerLeftUV.y - Mathf.Abs(this.UVDimensions.y);
                }
                pool.UVs[num6 + 1] = zero;
                if (index == this.Tail)
                {
                    this.Vertexsegment.Pool.UVChanged = true;
                    this.Vertexsegment.Pool.VertChanged = true;
                    this.Vertexsegment.Pool.ColorChanged = true;
                    return;
                }
                head = index;
                Vector3 vector7 = this.ElementArray[num7].Position - element.Position;
                num2 += vector7.magnitude;
                index++;
            }
        }
    }

    public class Element
    {
        public Vector3 Position;
        public float Width;

        public Element(Vector3 position, float width)
        {
            this.Position = position;
            this.Width = width;
        }
    }
}

