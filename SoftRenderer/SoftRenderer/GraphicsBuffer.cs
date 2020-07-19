using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftRenderer
{
    //双缓冲
    public class GraphicsBuffer
    {
        public Bitmap Current { get; private set; }
        public Bitmap BackGround { get; private set; }
        public GraphicsDevice CurrentGraphicsDevice { get; private set; }
        public GraphicsDevice BackGroundGraphicsDevice { get; private set; }


        public GraphicsBuffer(int width, int height)
        {
            Current = new Bitmap(width, height);
            BackGround = new Bitmap(width, height);
            CurrentGraphicsDevice = new GraphicsDevice(Current);
            BackGroundGraphicsDevice = new GraphicsDevice(BackGround);
        }

        //切换画布
        public void SwapBuffers()
        {
            var t = Current;
            Current = BackGround;
            BackGround = t;
        }

    }
}
