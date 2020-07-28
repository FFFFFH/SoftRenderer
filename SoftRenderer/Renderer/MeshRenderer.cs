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
            Vertex[] tempVerts = new Vertex[] { new Vertex(), new Vertex(), new Vertex() };
            int length = mesh.Surfaces.Length;
            for (int i = 0; i < length; i++)
            {
                var surface = mesh.Surfaces[i];
                //不把变换后的结果保存顶点中，否则下次渲染顶点位置就不对了
                tempVerts[0].Copy(mesh.Vertices[surface.A]);
                tempVerts[1].Copy(mesh.Vertices[surface.B]);
                tempVerts[2].Copy(mesh.Vertices[surface.C]);

                Vector v1 = tempVerts[0].pos;
                Vector v2 = tempVerts[1].pos;
                Vector v3 = tempVerts[2].pos;

                //Obj -> World
                Vector wp1 = transform.ApplyObj2World(v1);
                Vector wp2 = transform.ApplyObj2World(v2);
                Vector wp3 = transform.ApplyObj2World(v3);

                //World -> view
                Vector vp1 = transform.ApplyWorldToView(wp1);
                Vector vp2 = transform.ApplyWorldToView(wp2);
                Vector vp3 = transform.ApplyWorldToView(wp3);

                //背面消隐
                if (TransformUtility.IsBackCulling(vp1, vp2, vp3))
                {
                    continue;
                }

                //view -> 齐次裁剪空间
                Vector cp1 = transform.ApplyViewToClip(vp1);
                Vector cp2 = transform.ApplyViewToClip(vp2);
                Vector cp3 = transform.ApplyViewToClip(vp3);

                // CVV裁剪
                //if(...)

                //变换到齐次裁剪空间后，w值保存view空间下的z信息 
                //保存 1/z 用于后面求u'、v'
              
                tempVerts[0].onePerZ = 1 / cp1.w;
                tempVerts[1].onePerZ = 1 / cp2.w;
                tempVerts[2].onePerZ = 1 / cp3.w;

                tempVerts[0].u *= tempVerts[0].onePerZ;
                tempVerts[0].v *= tempVerts[0].onePerZ;
                tempVerts[1].u *= tempVerts[1].onePerZ;
                tempVerts[1].v *= tempVerts[1].onePerZ;
                tempVerts[2].u *= tempVerts[2].onePerZ;
                tempVerts[2].v *= tempVerts[2].onePerZ;

                //透视除法 齐次裁剪空间 -> NDC空间
                cp1 /= cp1.w;
                cp2 /= cp2.w;
                cp3 /= cp3.w;

                //屏幕映射 NDC空间下 -> 屏幕空间
                Vector sp1 = Screen.ComputeToScreenPos(cp1);
                Vector sp2 = Screen.ComputeToScreenPos(cp2);
                Vector sp3 = Screen.ComputeToScreenPos(cp3);

                tempVerts[0].pos = sp1;
                tempVerts[1].pos = sp2;
                tempVerts[2].pos = sp3;
                switch (Application.RenderType)
                {
                    case RenderType.Wireframe:
                        graphics.DrawTriangleLine(tempVerts[0].pos, tempVerts[1].pos, tempVerts[2].pos, Color.Red);
                        break;
                    case RenderType.Fill:
                        graphics.FillTriangle(tempVerts[0].pos, tempVerts[1].pos, tempVerts[2].pos, Color.Red);
                        break;
                    case RenderType.VertexColor:
                        graphics.FillTriangle(tempVerts[0], tempVerts[1], tempVerts[2]);
                        break;
                    case RenderType.Texture:
                        graphics.FillTriangle(tempVerts[0], tempVerts[1], tempVerts[2]);
                        break;
                }
            }
        }

    }
}
