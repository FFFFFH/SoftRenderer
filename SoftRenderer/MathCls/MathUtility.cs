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

    }
}
