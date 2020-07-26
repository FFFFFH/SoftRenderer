using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftRenderer
{
    public class Mesh
    {
        public Surface[] Surfaces { get; private set; }
        public Vertex[] Vertices { get; private set; }

        public Mesh(Vertex[] vertices, Surface[] surfaces)
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
