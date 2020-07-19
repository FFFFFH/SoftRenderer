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

        public static bool CompareFloat(float a, float b, float precision = 0.0001f)
        {
            return Math.Abs(a - b) < precision;
        }

    }
}
