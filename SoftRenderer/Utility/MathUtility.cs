using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftRenderer
{
    public class MathUtility
    {
        public const float Rad2Angle = 57.296f;
        public const float Angle2Rad = 0.01745f;

        public static bool IsEqual(float a, float b, float precision = 0.001f)
        {
            return Math.Abs(a - b) < precision;
        }

        public static int Clamp(int value, int min, int max)
        {
            if(value > max)
            {
                return max;
            }
            if (value < min)
            {
                return min;
            }
            return value;
        }

        public static float Lerp(float a, float b, float t)
        {
            if (t <= 0) return a;
            if (t >= 1) return b;
            return b * t + (1 - t) * a;
        }

        public static Vector Lerp(Vector a, Vector b, float t)
        {
            Vector vec = Vector.Zero;
            vec.x = MathUtility.Lerp(a.x, b.x, t);
            vec.y = MathUtility.Lerp(a.y, b.y, t);
            vec.z = MathUtility.Lerp(a.z, b.z, t);
            return vec;
        }

    }
}
