using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftRenderer
{
    public class Mesh
    {
        public Vector3 Position { get; set; } = Vector3.Zero;
        public Vector3 Rotation { get; set; } = Vector3.Zero;
        public Surface[] Surfaces { get; private set; }
        public Vector3[] Vertices { get; private set; }

        public Mesh(Vector3[] vertices, Surface[] surfaces)
        {
            Vertices = vertices;
            Surfaces = surfaces;
        }

    }

    public struct Surface
    {
        public int A;
        public int B;
        public int C;
    }
}
