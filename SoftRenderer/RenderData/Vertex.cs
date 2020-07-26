using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftRenderer
{
    public class Vertex
    {
        private Vector3 m_Pos;

        public Vector3 pos
        {
            get
            {
                return m_Pos.Copy();
            }
            set
            {
                m_Pos = value;
            }
        }

        public Color color;

        public float u;

        public float v;

        public float onePerZ;

        public Vertex() { }

        public Vertex(Vector3 _pos, Color _color)
        {
            m_Pos = _pos;
            color = _color;
        }

        public void Copy(Vertex _vertex)
        {
            pos = _vertex.pos;
            color = _vertex.color;
            onePerZ = _vertex.onePerZ;
            u = _vertex.u;
            v = _vertex.v;
        }
    }
}
