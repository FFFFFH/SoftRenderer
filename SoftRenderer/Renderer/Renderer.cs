using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftRenderer
{
    public class Renderer : Component
    {
        internal GraphicsDevice graphics
        {
            get { return Application.GraphicsDevice; }
        }

        public virtual void Render()
        {
        }

        public Renderer()
        {
            Application.AddRenderer(this); 
        }
    }
}
