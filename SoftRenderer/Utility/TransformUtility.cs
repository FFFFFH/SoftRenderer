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
        public static bool IsBackCulling(Vector3 p1, Vector3 p2, Vector3 p3)
        {
            if (Application.RenderType == RenderType.Wireframe) return false;
            var v1 = p2 - p1;
            var v2 = p3 - p2;
            var normal = Vector3.Cross(v1, v2);
            var viewToPoint = p1 - Vector3.Zero;//因为在观察空间中，所以原点就是摄像机的位置\
            return Vector3.Dot(viewToPoint, normal) < 0;
        }
    }
}
