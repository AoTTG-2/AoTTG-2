using UnityEngine;
using System.Runtime.Serialization;

namespace MapEditor
{
	[System.Serializable]
	public class TransformData : System.IEquatable<TransformData>, ISerializable
	{
		#region Fields
		public static readonly TransformData Identity = new TransformData(Vector3.zero, Quaternion.identity, Vector3.one);

		// If the matrix needs rebuilt, this will be true.  Used to delay expensive
		// matrix construction until necessary (since t/r/s can change a lot before a
		// matrix is needed).
		private bool dirty = true;

		[SerializeField] private Vector3 position;
		[SerializeField] private Quaternion rotation;
		[SerializeField] private Vector3 scale;

		private Matrix4x4 matrix;
		#endregion

		#region Properties
		public Vector3 Position
		{
			get { return position; }
			set { dirty = true; position = value; }
		}

		public Quaternion Rotation
		{
			get { return rotation; }
			set { dirty = true; rotation = value; }
		}

		public Vector3 Scale
		{
			get { return scale; }
			set { dirty = true; scale = value; }
		}

		public Vector3 up { get { return Rotation * Vector3.up; } }
		public Vector3 forward { get { return Rotation * Vector3.forward; } }
		public Vector3 right { get { return Rotation * Vector3.right; } }
		#endregion

		#region Constructors
		public TransformData()
		{
			this.Position = Vector3.zero;
			this.Rotation = Quaternion.identity;
			this.Scale = Vector3.one;
			this.matrix = Matrix4x4.identity;
			this.dirty = false;
		}

		public TransformData(Vector3 position, Quaternion rotation, Vector3 scale)
		{
			this.Position 	= position;
			this.Rotation 	= rotation;
			this.Scale		= scale;

			this.matrix 	= Matrix4x4.TRS(position, rotation, scale);
			this.dirty 	= false;
		}

		public TransformData(Transform transform)
		{
			this.Position 	= transform.position;
			this.Rotation 	= transform.localRotation;
			this.Scale		= transform.localScale;

			this.matrix 	= Matrix4x4.TRS(Position, Rotation, Scale);
			this.dirty 	= false;
		}

		public TransformData(TransformData transform)
		{
			this.Position 	= transform.Position;
			this.Rotation 	= transform.Rotation;
			this.Scale		= transform.Scale;

			this.matrix 	= Matrix4x4.TRS(Position, Rotation, Scale);
			this.dirty 	= false;
		}

		public TransformData(SerializationInfo info, StreamingContext context)
		{
			this.position = (Vector3) info.GetValue("position", typeof(Vector3));
			this.rotation = (Quaternion) info.GetValue("rotation", typeof(Quaternion));
			this.scale = (Vector3) info.GetValue("scale", typeof(Vector3));
			this.dirty = true;
		}
        #endregion

        #region Transform Methods
        public void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue("position", (Vector3)position, typeof(Vector3));
			info.AddValue("rotation", (Quaternion)rotation, typeof(Quaternion));
			info.AddValue("scale", (Vector3)scale, typeof(Vector3));
		}

		public void SetTRS(Transform trs)
		{
			this.Position 	= trs.position;
			this.Rotation 	= trs.localRotation;
			this.Scale		= trs.localScale;
			this.dirty 		= true;
		}

		bool Approx(Vector3 lhs, Vector3 rhs)
		{
			return 	Mathf.Abs(lhs.x - rhs.x) < Mathf.Epsilon &&
					Mathf.Abs(lhs.y - rhs.y) < Mathf.Epsilon &&
					Mathf.Abs(lhs.z - rhs.z) < Mathf.Epsilon;
		}

		bool Approx(Quaternion lhs, Quaternion rhs)
		{
			return 	Mathf.Abs(lhs.x - rhs.x) < Mathf.Epsilon &&
					Mathf.Abs(lhs.y - rhs.y) < Mathf.Epsilon &&
					Mathf.Abs(lhs.z - rhs.z) < Mathf.Epsilon &&
					Mathf.Abs(lhs.w - rhs.w) < Mathf.Epsilon;
		}

		public bool Equals(TransformData rhs)
		{
			return 	Approx(this.Position, rhs.Position) &&
					Approx(this.Rotation, rhs.Rotation) &&
					Approx(this.Scale, rhs.Scale);
		}

		public override bool Equals(object rhs)
		{
			return rhs is TransformData && this.Equals( (TransformData) rhs );
		}

		public override int GetHashCode()
		{
			return Position.GetHashCode() ^ Rotation.GetHashCode() ^ Scale.GetHashCode();
		}

		public Matrix4x4 GetMatrix()
		{
			if( !dirty )
			{
				return matrix;
			}
			else
			{
				dirty = false;
				matrix = Matrix4x4.TRS(Position, Rotation, Scale);
				return matrix;
			}
		}

		public override string ToString()
		{
			return Position.ToString("F2") + "\n" + Rotation.ToString("F2") + "\n" + Scale.ToString("F2");
		}
		#endregion

		#region Operators
		public static TransformData operator - (TransformData lhs, TransformData rhs)
		{
			TransformData t = new TransformData();

			t.Position = lhs.Position - rhs.Position;
			t.Rotation = Quaternion.Inverse(rhs.Rotation) * lhs.Rotation;
			t.Scale = new Vector3(	lhs.Scale.x / rhs.Scale.x,
									lhs.Scale.y / rhs.Scale.y,
									lhs.Scale.z / rhs.Scale.z);

			return t;
		}

		public static TransformData operator + (TransformData lhs, TransformData rhs)
		{
			TransformData t = new TransformData();

			t.Position = lhs.Position + rhs.Position;
			t.Rotation = lhs.Rotation * rhs.Rotation;
			t.Scale = new Vector3(	lhs.Scale.x * rhs.Scale.x,
									lhs.Scale.y * rhs.Scale.y,
									lhs.Scale.z * rhs.Scale.z);

			return t;
		}

		public static TransformData operator + (Transform lhs, TransformData rhs)
		{
			TransformData t = new TransformData();

			t.Position = lhs.position + rhs.Position;
			t.Rotation = lhs.localRotation * rhs.Rotation;
			t.Scale = new Vector3(	lhs.localScale.x * rhs.Scale.x,
									lhs.localScale.y * rhs.Scale.y,
									lhs.localScale.z * rhs.Scale.z);

			return t;
		}

		public static bool operator == (TransformData lhs, TransformData rhs)
		{
			return object.ReferenceEquals(lhs, rhs) || lhs.Equals(rhs);
		}

		public static bool operator != (TransformData lhs, TransformData rhs)
		{
			return !(lhs == rhs);
		}
        #endregion
	}
}