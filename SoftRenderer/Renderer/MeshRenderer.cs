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
        {   
            if (mesh == null) return;
            Vertex tempV1 = new Vertex();
            Vertex tempV2 = new Vertex();
            Vertex tempV3 = new Vertex();
            for (int i = 0; i < mesh.Surfaces.Length; i++)
            {
                var surface = mesh.Surfaces[i];
                //不能把变换后的结果保存顶点中，否则下次渲染顶点位置就不对了
                Vector3 v1 = mesh.Vertices[surface.A].pos;
                Vector3 v2 = mesh.Vertices[surface.B].pos;
                Vector3 v3 = mesh.Vertices[surface.C].pos;
                
                Vector3 wp1 = transform.ApplyObj2World(v1);
                Vector3 wp2 = transform.ApplyObj2World(v2);
                Vector3 wp3 = transform.ApplyObj2World(v3);

                Vector3 vp1 = transform.ApplyWorldToView(wp1);
                Vector3 vp2 = transform.ApplyWorldToView(wp2);
                Vector3 vp3 = transform.ApplyWorldToView(wp3);

                if (TransformUtility.IsBackCulling(vp1, vp2, vp3)) //背面消隐
                {
                    continue;
                }

                Vector3 cp1 = transform.ApplyViewToClip(vp1);
                Vector3 cp2 = transform.ApplyViewToClip(vp2);
                Vector3 cp3 = transform.ApplyViewToClip(vp3);

                Vector3 sp1 = Screen.ComputeToScreenPos(cp1);
                Vector3 sp2 = Screen.ComputeToScreenPos(cp2);
                Vector3 sp3 = Screen.ComputeToScreenPos(cp3);
                switch (Application.RenderType)
                {
                    case RenderType.Wireframe:
                        graphics.DrawTriangleLine(sp1, sp2, sp3, Color.Red);
                        break;
                    case RenderType.Fill:
                        graphics.FillTriangle(sp1, sp2, sp3, Color.Red);
                        break;
                    case RenderType.GradientColor:
                        tempV1.Copy(mesh.Vertices[surface.A]);
                        tempV2.Copy(mesh.Vertices[surface.B]);
                        tempV3.Copy(mesh.Vertices[surface.C]);
                        tempV1.pos = sp1;
                        tempV2.pos = sp2;
                        tempV3.pos = sp3;
                        //graphics.FillBottomFlatTriangle(tempV3, tempV1, tempV2);

                        graphics.FillTriangle(tempV3, tempV1, tempV2);
                        break;
                }
                //graphics.FillBottomFlatTriangle(sp3, sp2,sp1,Color.Red);
            }
        }

    }
}
