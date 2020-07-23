using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftRenderer
{
    public class MeshRenderer : Renderer
    {
        public Mesh mesh;

        public override void Render()
        {//mesh.Surfaces.Length
            if (mesh == null) return;
            for (int i = 0; i < mesh.Surfaces.Length; i++)
            {
                var surface = mesh.Surfaces[i];
                Vector3 v1 = mesh.Vertices[surface.A];
                Vector3 v2 = mesh.Vertices[surface.B];
                Vector3 v3 = mesh.Vertices[surface.C];
                Vector3 p1 = transform.ApplyTransfer(v1);
                Vector3 p2 = transform.ApplyTransfer(v2);
                Vector3 p3 = transform.ApplyTransfer(v3);
                Vector3 sp1 = TransformUtility.ComputeToScreenPos(p1);
                Vector3 sp2 = TransformUtility.ComputeToScreenPos(p2);
                Vector3 sp3 = TransformUtility.ComputeToScreenPos(p3);
                graphics.FillTriangle(sp1, sp2, sp3, Color.Red);
                //graphics.FillBottomFlatTriangle(sp3, sp2,sp1,Color.Red);
                //graphics.DrawTriangleLine(sp1, sp2, sp3, Color.Red);
            }
        }

    }
}
