using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftRenderer
{
    public class MeshRenderer
    {
        public Transform transform;

        public Mesh mesh;

        public MeshRenderer(Transform _transform, Mesh _mesh)
        {
            transform = _transform;
            mesh = _mesh;
        }

        public void Render(GraphicsDevice graphics)
        {
            Camera camera = Camera.main;
            camera.transform.position = new Vector3(0, 0, -10);
            transform.world2View = Matrix.LookAtLH(camera.transform.position,
                new Vector3(0,0,0), new Vector3(0, 1, 0));
            transform.viewToClip = Matrix.PerspectiveFov(camera.fov,
                camera.aspect, camera.nearPlane, camera.farPlene);

            for (int i = 0; i < mesh.Surfaces.Length; i++)
            {
                var surface = mesh.Surfaces[i];
                Vector3 v1 = mesh.Vertices[surface.A];
                Vector3 v2 = mesh.Vertices[surface.B];
                Vector3 v3 = mesh.Vertices[surface.C];

                Vector3 p1 = transform.ApplyTransfer(v1);
                Vector3 p2 = transform.ApplyTransfer(v2);
                Vector3 p3 = transform.ApplyTransfer(v3);

                graphics.DrawTriangle(p1, p2, p3, Color.Red);
            }
        }

    }
}
