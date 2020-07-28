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
        private Vector m_Pos;

        public Vector pos
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

        public Vertex(Vector _pos, Color _color, float _u = 0, float _v = 0)
        {
            m_Pos = _pos;
            color = _color;
            u = _u;
            v = _v;
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
