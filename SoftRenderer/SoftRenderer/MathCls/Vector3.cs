using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CompareFloat = SoftRenderer.MathUtility;

namespace SoftRenderer
{

    public struct Vector3
    {
        public Vector3(float x, float y, float z)
        {
            Values = new float[3];
            Values[0] = x;
            Values[1] = y;
            Values[2] = z;
        }

        public Vector3(float value = 0)
        {
            Values = new float[3];
            Values[0] = value;
            Values[1] = value;
            Values[2] = value;
        }

        public float Length =>
            (float)System.Math.Sqrt(Values[0] * Values[0] + Values[1] * Values[1] + Values[2] * Values[2])
            ;

        public static Vector3 One => new Vector3(1);

        public static Vector3 UnitX => new Vector3(1, 0, 0);

        public static Vector3 UnitY => new Vector3(0, 1, 0);

        public static Vector3 UnitZ => new Vector3(0, 0, 1);

        public float x
        {
            get { return Values[0]; }
            set { Values[0] = value; }
        }

        public float y
        {
            get { return Values[1]; }
            set { Values[1] = value; }
        }

        public float z
        {
            get { return Values[2]; }
            set { Values[2] = value; }
        }

        public static Vector3 Zero => new Vector3(0);
        private float[] Values { get; }

        public static implicit operator Point(Vector3 v)
        {
            return new Point((int)v.x, (int)v.y);
        }

        public static implicit operator PointF(Vector3 v)
        {
            return new PointF(v.x, v.y);
        }

        public static Vector3 operator -(Vector3 a, Vector3 b)
        {
            return new Vector3(0)
            {
                Values = {
                    [0] = a.Values[0] - b.Values[0],
                    [1] = a.Values[1] - b.Values[1],
                    [2] = a.Values[2] - b.Values[2]
                }
            };
        }

        public static Vector3 operator *(Vector3 a, float factor)
        {
            return new Vector3(0)
            {
                Values = {
                    [0] = a.Values[0]*factor,
                    [1] = a.Values[1]*factor,
                    [2] = a.Values[2]*factor
                }
            };
        }

        public static Vector3 operator /(Vector3 a, float factor)
        {
            return new Vector3(0)
            {
                Values = {
                    [0] = a.Values[0]/factor,
                    [1] = a.Values[1]/factor,
                    [2] = a.Values[2]/factor
                }
            };
        }

        public static bool operator ==(Vector3 a, Vector3 b)
        {
            return MathUtility.IsEqual(a.x,b.x)
                && MathUtility.IsEqual(a.y, b.y)
                && MathUtility.IsEqual(a.z, b.z)
                ;
        }

        public static bool operator !=(Vector3 a, Vector3 b)
        {
            return !MathUtility.IsEqual(a.x, b.x)
              || !MathUtility.IsEqual(a.y, b.y)
              || !MathUtility.IsEqual(a.z, b.z)
              ;
        }


        public static Vector3 operator +(Vector3 a, Vector3 b)
        {
            return new Vector3(0)
            {
                Values = {
                    [0] = a.Values[0] + b.Values[0],
                    [1] = a.Values[1] + b.Values[1],
                    [2] = a.Values[2] + b.Values[2]
                }
            };
        }

        public Vector3 Cross(Vector3 v)
        {
            return new Vector3(y * v.z - z * v.y, z * v.x - x * v.z, x * v.y - y * v.x);
        }

        public float Dot(Vector3 v)
        {
            return x * v.x + y * v.y + z * v.z;
        }

        public Vector3 Interpolate(Vector3 v, float factor)
        {
            return this + (v - this) * factor;
        }
        public Vector3 Normalize()
        {
            var length = Length;
            var factor = 0f;
            if (length > 0)
            {
                factor = 1.0f / length;
            }
            return new Vector3(x * factor, y * factor, z * factor);
        }

        public Vector3 Copy()
        {
            return new Vector3(x,y,z);
        }

        public Vector3 ApplyTransfer(Matrix matrix, bool div = false)
        {
            return matrix.ApplyTransfer(this, div); 
        }

        public override string ToString()
        {
            return $"x : {x}, y : {y}, z : {z}";
        }

    }
}
