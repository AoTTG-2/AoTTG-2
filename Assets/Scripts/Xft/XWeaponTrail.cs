/*
namespace Xft
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public class XWeaponTrail : MonoBehaviour
    {
        public float Fps = 60f;
        public int Granularity = 60;
        public int MaxFrame = 14;
        protected float mElapsedTime;
        protected float mFadeElapsedime;
        protected float mFadeT = 1f;
        protected float mFadeTime = 1f;
        protected Element mHeadElem = new Element();
        protected bool mInited;
        protected bool mIsFading;
        protected GameObject mMeshObj;
        protected List<Element> mSnapshotList = new List<Element>();
        protected Spline mSpline = new Spline();
        protected float mTrailWidth;
        protected VertexPool mVertexPool;
        protected VertexPool.VertexSegment mVertexSegment;
        public Color MyColor = Color.white;
        public Material MyMaterial;
        public Transform PointEnd;
        public Transform PointStart;
        public static string Version = "1.0.1";

        public void Activate()
        {
            this.MaxFrame = 14;
            this.Init();
            if (this.mMeshObj == null)
            {
                this.InitMeshObj();
            }
            else
            {
                base.gameObject.SetActive(true);
                if (this.mMeshObj != null)
                {
                    this.mMeshObj.SetActive(true);
                }
                this.mFadeT = 1f;
                this.mIsFading = false;
                this.mFadeTime = 1f;
                this.mFadeElapsedime = 0f;
                this.mElapsedTime = 0f;
                for (int i = 0; i < this.mSnapshotList.Count; i++)
                {
                    this.mSnapshotList[i].PointStart = this.PointStart.position;
                    this.mSnapshotList[i].PointEnd = this.PointEnd.position;
                    this.mSpline.ControlPoints[i].Position = this.mSnapshotList[i].Pos;
                    this.mSpline.ControlPoints[i].Normal = this.mSnapshotList[i].PointEnd - this.mSnapshotList[i].PointStart;
                }
                this.RefreshSpline();
                this.UpdateVertex();
            }
        }

        public void Deactivate()
        {
            base.gameObject.SetActive(false);
            if (this.mMeshObj != null)
            {
                this.mMeshObj.SetActive(false);
            }
        }

        public void Init()
        {
            if (!this.mInited)
            {
                Vector3 vector = this.PointStart.position - this.PointEnd.position;
                this.mTrailWidth = vector.magnitude;
                this.InitMeshObj();
                this.InitOriginalElements();
                this.InitSpline();
                this.mInited = true;
            }
        }

        private void InitMeshObj()
        {
            this.mMeshObj = new GameObject("_XWeaponTrailMesh: " + base.gameObject.name);
            this.mMeshObj.layer = base.gameObject.layer;
            this.mMeshObj.SetActive(true);
            MeshFilter filter = this.mMeshObj.AddComponent<MeshFilter>();
            MeshRenderer renderer = this.mMeshObj.AddComponent<MeshRenderer>();
            renderer.castShadows = false;
            renderer.receiveShadows = false;
            renderer.GetComponent<Renderer>().sharedMaterial = this.MyMaterial;
            filter.sharedMesh = new Mesh();
            this.mVertexPool = new VertexPool(filter.sharedMesh, this.MyMaterial);
            this.mVertexSegment = this.mVertexPool.GetVertices(this.Granularity * 3, (this.Granularity - 1) * 12);
            this.UpdateIndices();
        }

        private void InitOriginalElements()
        {
            this.mSnapshotList.Clear();
            this.mSnapshotList.Add(new Element(this.PointStart.position, this.PointEnd.position));
            this.mSnapshotList.Add(new Element(this.PointStart.position, this.PointEnd.position));
        }

        private void InitSpline()
        {
            this.mSpline.Granularity = this.Granularity;
            this.mSpline.Clear();
            for (int i = 0; i < this.MaxFrame; i++)
            {
                this.mSpline.AddControlPoint(this.CurHeadPos, this.PointStart.position - this.PointEnd.position);
            }
        }

        public void lateUpdate()
        {
            if (this.mInited)
            {
                this.mVertexPool.LateUpdate();
            }
        }

        private void OnDrawGizmos()
        {
            if ((this.PointEnd != null) && (this.PointStart != null))
            {
                Vector3 vector = this.PointStart.position - this.PointEnd.position;
                float magnitude = vector.magnitude;
                if (magnitude >= float.Epsilon)
                {
                    Gizmos.color = Color.red;
                    Gizmos.DrawSphere(this.PointStart.position, magnitude * 0.04f);
                    Gizmos.color = Color.blue;
                    Gizmos.DrawSphere(this.PointEnd.position, magnitude * 0.04f);
                }
            }
        }

        private void RecordCurElem()
        {
            Element item = new Element(this.PointStart.position, this.PointEnd.position);
            if (this.mSnapshotList.Count < this.MaxFrame)
            {
                this.mSnapshotList.Insert(1, item);
            }
            else
            {
                this.mSnapshotList.RemoveAt(this.mSnapshotList.Count - 1);
                this.mSnapshotList.Insert(1, item);
            }
        }

        private void RefreshSpline()
        {
            for (int i = 0; i < this.mSnapshotList.Count; i++)
            {
                this.mSpline.ControlPoints[i].Position = this.mSnapshotList[i].Pos;
                this.mSpline.ControlPoints[i].Normal = this.mSnapshotList[i].PointEnd - this.mSnapshotList[i].PointStart;
            }
            this.mSpline.RefreshSpline();
        }

        private void Start()
        {
            this.Init();
        }

        public void StopSmoothly(float fadeTime)
        {
            this.mIsFading = true;
            this.mFadeTime = fadeTime;
        }

        public void update()
        {
            if (this.mInited)
            {
                if (this.mMeshObj == null)
                {
                    this.InitMeshObj();
                }
                else
                {
                    this.UpdateHeadElem();
                    this.mElapsedTime += Time.deltaTime;
                    if (this.mElapsedTime >= this.UpdateInterval)
                    {
                        this.mElapsedTime -= this.UpdateInterval;
                        this.RecordCurElem();
                        this.RefreshSpline();
                        this.UpdateFade();
                        this.UpdateVertex();
                    }
                }
            }
        }

        private void UpdateFade()
        {
            if (this.mIsFading)
            {
                this.mFadeElapsedime += Time.deltaTime;
                float num = this.mFadeElapsedime / this.mFadeTime;
                this.mFadeT = 1f - num;
                if (this.mFadeT < 0f)
                {
                    this.Deactivate();
                }
            }
        }

        private void UpdateHeadElem()
        {
            this.mSnapshotList[0].PointStart = this.PointStart.position;
            this.mSnapshotList[0].PointEnd = this.PointEnd.position;
        }

        private void UpdateIndices()
        {
            VertexPool pool = this.mVertexSegment.Pool;
            for (int i = 0; i < (this.Granularity - 1); i++)
            {
                int num2 = this.mVertexSegment.VertStart + (i * 3);
                int num3 = this.mVertexSegment.VertStart + ((i + 1) * 3);
                int index = this.mVertexSegment.IndexStart + (i * 12);
                pool.Indices[index] = num3;
                pool.Indices[index + 1] = num3 + 1;
                pool.Indices[index + 2] = num2;
                pool.Indices[index + 3] = num3 + 1;
                pool.Indices[index + 4] = num2 + 1;
                pool.Indices[index + 5] = num2;
                pool.Indices[index + 6] = num3 + 1;
                pool.Indices[index + 7] = num3 + 2;
                pool.Indices[index + 8] = num2 + 1;
                pool.Indices[index + 9] = num3 + 2;
                pool.Indices[index + 10] = num2 + 2;
                pool.Indices[index + 11] = num2 + 1;
            }
            pool.IndiceChanged = true;
        }

        private void UpdateVertex()
        {
            VertexPool pool = this.mVertexSegment.Pool;
            for (int i = 0; i < this.Granularity; i++)
            {
                int index = this.mVertexSegment.VertStart + (i * 3);
                float num3 = ((float) i) / ((float) this.Granularity);
                float tl = num3 * this.mFadeT;
                Vector2 zero = Vector2.zero;
                Vector3 vector2 = this.mSpline.InterpolateByLen(tl);
                Vector3 vector3 = this.mSpline.InterpolateNormalByLen(tl);
                Vector3 vector4 = vector2 + ((Vector3) ((vector3.normalized * this.mTrailWidth) * 0.5f));
                Vector3 vector5 = vector2 - ((Vector3) ((vector3.normalized * this.mTrailWidth) * 0.5f));
                pool.Vertices[index] = vector4;
                pool.Colors[index] = this.MyColor;
                zero.x = 0f;
                zero.y = num3;
                pool.UVs[index] = zero;
                pool.Vertices[index + 1] = vector2;
                pool.Colors[index + 1] = this.MyColor;
                zero.x = 0.5f;
                zero.y = num3;
                pool.UVs[index + 1] = zero;
                pool.Vertices[index + 2] = vector5;
                pool.Colors[index + 2] = this.MyColor;
                zero.x = 1f;
                zero.y = num3;
                pool.UVs[index + 2] = zero;
            }
            this.mVertexSegment.Pool.UVChanged = true;
            this.mVertexSegment.Pool.VertChanged = true;
            this.mVertexSegment.Pool.ColorChanged = true;
        }

        public Vector3 CurHeadPos
        {
            get
            {
                return (Vector3) ((this.PointStart.position + this.PointEnd.position) / 2f);
            }
        }

        public float TrailWidth
        {
            get
            {
                return this.mTrailWidth;
            }
        }

        public float UpdateInterval
        {
            get
            {
                return (1f / this.Fps);
            }
        }

        public class Element
        {
            public Vector3 PointEnd;
            public Vector3 PointStart;

            public Element()
            {
            }

            public Element(Vector3 start, Vector3 end)
            {
                this.PointStart = start;
                this.PointEnd = end;
            }

            public Vector3 Pos
            {
                get
                {
                    return (Vector3) ((this.PointStart + this.PointEnd) / 2f);
                }
            }
        }
    }
}

*/