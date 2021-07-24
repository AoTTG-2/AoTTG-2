namespace Xft
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using UnityEngine;

    //TODO: Remove this for #223
    /// <summary>
    /// Part of the "XTF" package that AoTTG used for WeaponTrails, in AoTTG2 we will use a different package, so eventually these classes will be deleted.
    /// </summary>
    public class Spline
    {
        public int Granularity = 20;
        private List<SplineControlPoint> mControlPoints = new List<SplineControlPoint>();
        private List<SplineControlPoint> mSegments = new List<SplineControlPoint>();

        public SplineControlPoint AddControlPoint(Vector3 pos, Vector3 up)
        {
            SplineControlPoint item = new SplineControlPoint();
            item.Init(this);
            item.Position = pos;
            item.Normal = up;
            this.mControlPoints.Add(item);
            item.ControlPointIndex = this.mControlPoints.Count - 1;
            return item;
        }

        public static Vector3 CatmulRom(Vector3 T0, Vector3 P0, Vector3 P1, Vector3 T1, float f)
        {
            double num = -0.5;
            double num2 = 1.5;
            double num3 = -1.5;
            double num4 = 0.5;
            double num5 = -2.5;
            double num6 = 2.0;
            double num7 = -0.5;
            double num8 = -0.5;
            double num9 = 0.5;
            double num10 = (((num * T0.x) + (num2 * P0.x)) + (num3 * P1.x)) + (num4 * T1.x);
            double num11 = ((T0.x + (num5 * P0.x)) + (num6 * P1.x)) + (num7 * T1.x);
            double num12 = (num8 * T0.x) + (num9 * P1.x);
            double x = P0.x;
            double num14 = (((num * T0.y) + (num2 * P0.y)) + (num3 * P1.y)) + (num4 * T1.y);
            double num15 = ((T0.y + (num5 * P0.y)) + (num6 * P1.y)) + (num7 * T1.y);
            double num16 = (num8 * T0.y) + (num9 * P1.y);
            double y = P0.y;
            double num18 = (((num * T0.z) + (num2 * P0.z)) + (num3 * P1.z)) + (num4 * T1.z);
            double num19 = ((T0.z + (num5 * P0.z)) + (num6 * P1.z)) + (num7 * T1.z);
            double num20 = (num8 * T0.z) + (num9 * P1.z);
            double z = P0.z;
            float num22 = (float) ((((((num10 * f) + num11) * f) + num12) * f) + x);
            float num23 = (float) ((((((num14 * f) + num15) * f) + num16) * f) + y);
            return new Vector3(num22, num23, (float) ((((((num18 * f) + num19) * f) + num20) * f) + z));
        }

        public void Clear()
        {
            this.mControlPoints.Clear();
        }

        public Vector3 InterpolateByLen(float tl)
        {
            float num;
            return this.LenToSegment(tl, out num).Interpolate(num);
        }

        public Vector3 InterpolateNormalByLen(float tl)
        {
            float num;
            return this.LenToSegment(tl, out num).InterpolateNormal(num);
        }

        public SplineControlPoint LenToSegment(float t, out float localF)
        {
            SplineControlPoint point = null;
            t = Mathf.Clamp01(t);
            float num = t * this.mSegments[this.mSegments.Count - 1].Dist;
            int num2 = 0;
            num2 = 0;
            while (num2 < this.mSegments.Count)
            {
                if (this.mSegments[num2].Dist >= num)
                {
                    point = this.mSegments[num2];
                    break;
                }
                num2++;
            }
            if (num2 == 0)
            {
                localF = 0f;
                return point;
            }
            float num3 = 0f;
            int num4 = point.SegmentIndex - 1;
            SplineControlPoint point2 = this.mSegments[num4];
            num3 = point.Dist - point2.Dist;
            localF = (num - point2.Dist) / num3;
            return point2;
        }

        public SplineControlPoint NextControlPoint(SplineControlPoint controlpoint)
        {
            if (this.mControlPoints.Count == 0)
            {
                return null;
            }
            int num = controlpoint.ControlPointIndex + 1;
            if (num >= this.mControlPoints.Count)
            {
                return null;
            }
            return this.mControlPoints[num];
        }

        public Vector3 NextNormal(SplineControlPoint controlpoint)
        {
            SplineControlPoint point = this.NextControlPoint(controlpoint);
            if (point != null)
            {
                return point.Normal;
            }
            return controlpoint.Normal;
        }

        public Vector3 NextPosition(SplineControlPoint controlpoint)
        {
            SplineControlPoint point = this.NextControlPoint(controlpoint);
            if (point != null)
            {
                return point.Position;
            }
            return controlpoint.Position;
        }

        public SplineControlPoint PreviousControlPoint(SplineControlPoint controlpoint)
        {
            if (this.mControlPoints.Count == 0)
            {
                return null;
            }
            int num = controlpoint.ControlPointIndex - 1;
            if (num < 0)
            {
                return null;
            }
            return this.mControlPoints[num];
        }

        public Vector3 PreviousNormal(SplineControlPoint controlpoint)
        {
            SplineControlPoint point = this.PreviousControlPoint(controlpoint);
            if (point != null)
            {
                return point.Normal;
            }
            return controlpoint.Normal;
        }

        public Vector3 PreviousPosition(SplineControlPoint controlpoint)
        {
            SplineControlPoint point = this.PreviousControlPoint(controlpoint);
            if (point != null)
            {
                return point.Position;
            }
            return controlpoint.Position;
        }

        private void RefreshDistance()
        {
            if (this.mSegments.Count >= 1)
            {
                this.mSegments[0].Dist = 0f;
                for (int i = 1; i < this.mSegments.Count; i++)
                {
                    Vector3 vector = this.mSegments[i].Position - this.mSegments[i - 1].Position;
                    float magnitude = vector.magnitude;
                    this.mSegments[i].Dist = this.mSegments[i - 1].Dist + magnitude;
                }
            }
        }

        public void RefreshSpline()
        {
            this.mSegments.Clear();
            for (int i = 0; i < this.mControlPoints.Count; i++)
            {
                if (this.mControlPoints[i].IsValid)
                {
                    this.mSegments.Add(this.mControlPoints[i]);
                    this.mControlPoints[i].SegmentIndex = this.mSegments.Count - 1;
                }
            }
            this.RefreshDistance();
        }

        public List<SplineControlPoint> ControlPoints
        {
            get
            {
                return this.mControlPoints;
            }
        }

        public SplineControlPoint this[int index]
        {
            get
            {
                if ((index > -1) && (index < this.mSegments.Count))
                {
                    return this.mSegments[index];
                }
                return null;
            }
        }

        public List<SplineControlPoint> Segments
        {
            get
            {
                return this.mSegments;
            }
        }
    }
}

