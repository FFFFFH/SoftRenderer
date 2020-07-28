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

        public static Color Mul(Color col, float value)
        {
            return ColorUtility.ToColor((int)(col.R * value), (int)(col.G * value), (int)(col.B * value));
        }

        public static Color Div(Color col, float value)
        {
            return ColorUtility.ToColor((int)(col.R / value), (int)(col.G / value), (int)(col.B / value));
        }

        public static Color Minus(Color left, Color right)
        {
            return ColorUtility.ToColor(left.R - right.R, left.G - right.G, left.B - right.B);
        }

        public static Color Modulate(Color left, Color right)
        {
            return ColorUtility.ToColor(left.R * right.R, left.G * right.G, left.B * right.B);
        }

        public static Color Lerp(Color c1, Color c2, float t)
        {
            int r = (int)MathUtility.Lerp(c1.R, c2.R, t);
            int g = (int)MathUtility.Lerp(c1.G, c2.G, t); 
            int b = (int)MathUtility.Lerp(c1.B, c2.B, t); 
            return ColorUtility.ToColor(r, g, b);
        }

        //距离法确定三角形中间某点颜色
        public static Color Lerp(Vertex v1, Vertex v2, Vertex v3, Vertex point)
        {
            float w1 = 1 / Math.Max(0.01f,(v1.pos - point.pos).Length);
            float w2 = 1 / Math.Max(0.01f, (v2.pos - point.pos).Length);
            float w3 = 1 / Math.Max(0.01f, (v3.pos - point.pos).Length);

            Color col = ColorUtility.Add(ColorUtility.Mul(v1.color, w1), ColorUtility.Mul(v2.color, w2));
            col = ColorUtility.Add(col, ColorUtility.Mul(v3.color, w3));
            col = ColorUtility.Div(col, (w1 + w2 + w3));
            return col;
        }


    }
}
