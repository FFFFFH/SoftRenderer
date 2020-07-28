using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftRenderer
{
    public class TransformUtility
    {
        public static bool IsBackCulling(Vector p1, Vector p2, Vector p3)
        {
            if (Application.RenderType == RenderType.Wireframe) return false;
            var v1 = p2 - p1;
            var v2 = p3 - p2;
            var normal = Vector.Cross(v1, v2);
            var viewToPoint = p1 - Vector.Zero;//因为在观察空间中，所以原点就是摄像机的位置\
            return Vector.Dot(viewToPoint, normal) < 0;
        }

        public static Vertex LerpVertexInScreenSpace(Vertex v1, Vertex v2, float t)
        {
            Vertex vert = new Vertex();
            vert.pos = MathUtility.Lerp(v1.pos, v2.pos, t);
            vert.color = ColorUtility.Lerp(v1.color, v2.color, t);
            vert.u = MathUtility.Lerp(v1.u, v2.u, t);
            vert.v = MathUtility.Lerp(v1.v, v2.v, t);
            vert.onePerZ = MathUtility.Lerp(v1.onePerZ, v2.onePerZ, t);
            return vert;
        }

    }
}
