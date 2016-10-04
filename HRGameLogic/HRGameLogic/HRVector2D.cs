using System;
using System.Collections.Generic;

namespace HRGameLogic
{
	public struct HRVector2D {

#region Fields
		public float 				x;
		public float 				y;
		public float 				magnitude { get; private set; }

		public HRVector2D 			normalized
		{
			get { 
				return new HRVector2D(x / magnitude, y / magnitude);
			}
		}
		public float 				sqrMagnitude { get; private set; }
#endregion

#region Static Variables
		public static HRVector2D down
		{
			get { return new HRVector2D(0f, -1f); }
		}

		public static HRVector2D up
		{
			get { return new HRVector2D(0f, 1f); }
		}

		public static HRVector2D one
		{
			get { return new HRVector2D(1f, 1f); }
		}

		public static HRVector2D left
		{
			get { return new HRVector2D(-1f, 0f); }
		}

		public static HRVector2D right
		{
			get { return new HRVector2D(1f, 0f); }
		}

		public static HRVector2D zero
		{
			get { return new HRVector2D(0f, 0f); }
		}
#endregion

#region Constructor
		public HRVector2D(float x, float y)
		{
			this.x = x;
			this.y = y;
			sqrMagnitude = x * x + y * y;
			magnitude = Convert.ToSingle((Double)Math.Sqrt(sqrMagnitude));
			update();
		}

		public HRVector2D(Dictionary<string, object> dict)
		{
			this.x = 0f;
			this.y = 0f;

			if(dict.ContainsKey("x"))
				this.x = System.Convert.ToSingle(dict["x"]);

			if(dict.ContainsKey("y"))
				this.y = System.Convert.ToSingle(dict["y"]);	

			sqrMagnitude = x * x + y * y;
			magnitude = Convert.ToSingle((Double)Math.Sqrt(sqrMagnitude));
			update();
		}
#endregion

#region Private Functions
		private void update()
		{
			sqrMagnitude = x * x + y * y;
			magnitude = Convert.ToSingle((Double)Math.Sqrt(sqrMagnitude));
		}
#endregion

#region Public API
		public void Normalize()
		{
			x = normalized.x;
			y = normalized.y; 
			update();
		}

		public void Set(float x, float y)
		{
			this.x = x;
			this.y = y;
			update();
		}

		public override string ToString()
		{
			return string.Format("HRVector2D x: {0}, y: {1}", x, y);
		}

		public override bool Equals(object obj)
		{
			if(obj is HRVector2D)
			{
				return (HRVector2D)obj == this;
			}
			return false;
		}

		public override int GetHashCode()
		{
			return x.GetHashCode() ^ y.GetHashCode();			
		}

		public Dictionary<string, object> ToDict()
		{
			Dictionary<string, object> result = new Dictionary<string, object>();
			result.Add("x", x);
			result.Add("y", y);

			return result;
		}
#endregion

#region Overload Operator
		public static HRVector2D operator + (HRVector2D left, HRVector2D right)
		{
			return new HRVector2D(left.x + right.x, left.y + right.y);		
		}

		public static HRVector2D operator - (HRVector2D left, HRVector2D right)
		{
			return new HRVector2D(left.x - right.x, left.y - right.y);
		}

		public static HRVector2D operator * (HRVector2D left, float right)
		{
			return new HRVector2D(left.x * right, left.y * right);
		}

		public static HRVector2D operator * (float left, HRVector2D right)
		{
			return new HRVector2D(left * right.x, left * right.y);
		}

		public static HRVector2D operator / (HRVector2D left, float right)
		{
			return new HRVector2D(left.x / right, left.y / right);
		}

		public static bool operator == (HRVector2D left, HRVector2D right)
		{
			return left.x == right.x && left.y == right.y;
		}

		public static bool operator != (HRVector2D left, HRVector2D right)
		{
			return left.x != right.x || left.y != right.y;
		}
#endregion

#region Public Static Functions
		public static float Distance(HRVector2D a, HRVector2D b)
		{
			return (a - b).magnitude;	
		}
#endregion
	}
}
