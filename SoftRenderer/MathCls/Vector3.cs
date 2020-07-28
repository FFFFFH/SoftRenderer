using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CompareFloat = SoftRenderer.MathUtility;

namespace SoftRenderer
{

    public struct Vector
    {
        public Vector(float x, float y, float z, float w = 0)
        {
            Values = new float[4];
            Values[0] = x;
            Values[1] = y;
            Values[2] = z;
            Values[3] = w;
        }

        public Vector(float value = 0)
        {
            Values = new float[4];
            Values[0] = value;
            Values[1] = value;
            Values[2] = value;
            Values[3] = value;
        }

        public float Length =>
            (float)System.Math.Sqrt(Values[0] * Values[0] + Values[1] * Values[1] + Values[2] * Values[2])
            ;

        public static Vector One => new Vector(1);

        public static Vector UnitX => new Vector(1, 0, 0);

        public static Vector UnitY => new Vector(0, 1, 0);

        public static Vector UnitZ => new Vector(0, 0, 1);

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
        
        public float w
        {
            get { return Values[3]; }
            set { Values[3] = value; }
        }

        public static Vector Zero => new Vector(0);
        private float[] Values { get; }

        public static implicit operator Point(Vector v)
        {
            return new Point((int)v.x, (int)v.y);
        }

        public static implicit operator PointF(Vector v)
        {
            return new PointF(v.x, v.y);
        }

        public static Vector operator -(Vector a, Vector b)
        {
            return new Vector(0)
            {
                Values = {
                    [0] = a.Values[0] - b.Values[0],
                    [1] = a.Values[1] - b.Values[1],
                    [2] = a.Values[2] - b.Values[2]
                }
            };
        }

        public static Vector operator *(Vector a, float factor)
        {
            return new Vector(0)
            {
                Values = {
                    [0] = a.Values[0]*factor,
                    [1] = a.Values[1]*factor,
                    [2] = a.Values[2]*factor
                }
            };
        }

        public static Vector operator /(Vector a, float factor)
        {
            return new Vector(0)
            {
                Values = {
                    [0] = a.Values[0]/factor,
                    [1] = a.Values[1]/factor,
                    [2] = a.Values[2]/factor
                }
            };
        }

        public static bool operator ==(Vector a, Vector b)
        {
            return MathUtility.IsEqual(a.x,b.x)
                && MathUtility.IsEqual(a.y, b.y)
                && MathUtility.IsEqual(a.z, b.z)
                ;
        }

        public static bool operator !=(Vector a, Vector b)
        {
            return !MathUtility.IsEqual(a.x, b.x)
              || !MathUtility.IsEqual(a.y, b.y)
              || !MathUtility.IsEqual(a.z, b.z)
              ;
        }


        public static Vector operator +(Vector a, Vector b)
        {
            return new Vector(0)
            {
                Values = {
                    [0] = a.Values[0] + b.Values[0],
                    [1] = a.Values[1] + b.Values[1],
                    [2] = a.Values[2] + b.Values[2]
                }
            };
        }

        public Vector Cross(Vector v)
        {
            return new Vector(y * v.z - z * v.y, z * v.x - x * v.z, x * v.y - y * v.x);
        }

        public float Dot(Vector v)
        {
            return x * v.x + y * v.y + z * v.z;
        }

        public Vector Interpolate(Vector v, float factor)
        {
            return this + (v - this) * factor;
        }
        public Vector Normalize()
        {
            var length = Length;
            var factor = 0f;
            if (length > 0)
            {
                factor = 1.0f / length;
            }
            return new Vector(x * factor, y * factor, z * factor);
        }

        public static Vector Cross(Vector lhs, Vector rhs)
        {
            float x = lhs.y * rhs.z - lhs.z * rhs.y;
            float y = lhs.z * rhs.x - lhs.x * rhs.z;
            float z = lhs.x * rhs.y - lhs.y * rhs.x;
            return new Vector(x, y, z);
        }

        public static float Dot(Vector v1, Vector v2)
        {
            return v1.x * v2.x + v1.y * v2.y + v1.z * v2.z;
        }

        public Vector Copy()
        {
            return new Vector(x,y,z);
        }

        public Vector ApplyTransfer(Matrix matrix)
        {
            return matrix.ApplyTransfer(this); 
        }

        public override string ToString()
        {
            return $"x : {x}, y : {y}, z : {z} w : {w}";
        }

    }
}
