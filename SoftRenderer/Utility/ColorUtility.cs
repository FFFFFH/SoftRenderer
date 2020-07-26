using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftRenderer
{
    public static class ColorUtility
    {
        public static Color ToColor(int r, int g, int b)
        {
            return Color.FromArgb(MathUtility.Clamp(r,0,255), MathUtility.Clamp(g, 0, 255), MathUtility.Clamp(b, 0, 255));
        }

        public static Color Add(Color left,  Color right)
        {
            return ColorUtility.ToColor(left.R + right.R, left.G + right.G, left.B + right.B);
        }

        public static Color Minus(Color left, Color right)
        {
            return ColorUtility.ToColor(left.R - right.R, left.G - right.G, left.B - right.B);
        }

        public static Color Modulate(Color left, Color right)
        {
            return ColorUtility.ToColor(left.R * right.R, left.G * right.G, left.B * right.B);
        }

        public static Color Lerp(Color c1, Color c2, float value)
        {
            int r = (int)(c2.R * value + c1.R * (1 - value));
            int g = (int)(c2.G * value + c1.G * (1 - value));
            int b = (int)(c2.B * value + c1.B * (1 - value));
            return ColorUtility.ToColor(r, g, b);
        }

    }
}
