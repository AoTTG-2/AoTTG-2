namespace Xft
{
    using System;
    using UnityEngine;

    //TODO: Remove this for #223
    /// <summary>
    /// Part of the "XTF" package that AoTTG used for WeaponTrails, in AoTTG2 we will use a different package, so eventually these classes will be deleted.
    /// </summary>
    public class SplineControlPoint
    {
        public int ControlPointIndex = -1;
        public float Dist;
        protected Spline mSpline;
        public Vector3 Normal;
        public Vector3 Position;
        public int SegmentIndex = -1;

        private Vector3 GetNext2Normal()
        {
            SplineControlPoint nextControlPoint = this.NextControlPoint;
            if (nextControlPoint != null)
            {
                return nextControlPoint.NextNormal;
            }
            return this.Normal;
        }

        private Vector3 GetNext2Position()
        {
            SplineControlPoint nextControlPoint = this.NextControlPoint;
            if (nextControlPoint != null)
            {
                return nextControlPoint.NextPosition;
            }
            return this.NextPosition;
        }

        public void Init(Spline owner)
        {
            this.mSpline = owner;
            this.SegmentIndex = -1;
        }

        public Vector3 Interpolate(float localF)
        {
            localF = Mathf.Clamp01(localF);
            return Spline.CatmulRom(this.PreviousPosition, this.Position, this.NextPosition, this.GetNext2Position(), localF);
        }

        public Vector3 InterpolateNormal(float localF)
        {
            localF = Mathf.Clamp01(localF);
            return Spline.CatmulRom(this.PreviousNormal, this.Normal, this.NextNormal, this.GetNext2Normal(), localF);
        }

        public bool IsValid
        {
            get
            {
                return (this.NextControlPoint != null);
            }
        }

        public SplineControlPoint NextControlPoint
        {
            get
            {
                return this.mSpline.NextControlPoint(this);
            }
        }

        public Vector3 NextNormal
        {
            get
            {
                return this.mSpline.NextNormal(this);
            }
        }

        public Vector3 NextPosition
        {
            get
            {
                return this.mSpline.NextPosition(this);
            }
        }

        public SplineControlPoint PreviousControlPoint
        {
            get
            {
                return this.mSpline.PreviousControlPoint(this);
            }
        }

        public Vector3 PreviousNormal
        {
            get
            {
                return this.mSpline.PreviousNormal(this);
            }
        }

        public Vector3 PreviousPosition
        {
            get
            {
                return this.mSpline.PreviousPosition(this);
            }
        }
    }
}

