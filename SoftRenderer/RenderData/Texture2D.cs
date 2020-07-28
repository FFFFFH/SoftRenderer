using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftRenderer
{
    public class Texture2D
    {
        private Bitmap bitmap;
        public float offsetX;
        public float offsetY;

        public float tillingX;
        public float tillingY;

        public int width { get => bitmap.Width; }
        public int height { get => bitmap.Height; }

        //TODO: 支持缩放和偏移
        public Color ReadTexture(int uIndex, int vIndex)
        {
            //顶点在左上
            int u = MathUtility.Clamp(uIndex, 0, bitmap.Width - 1);
            int v = MathUtility.Clamp(vIndex, 0, bitmap.Height - 1);
            v = bitmap.Height - 1 - v;
            //u = bitmap.Width - 1 - u;
            return bitmap.GetPixel(u, v);
        }

        private Texture2D(Bitmap _bitmap)
        {
            bitmap = _bitmap;
        }

        public static Texture2D LoadTexture(string path, int width = 256, int height = 256)
        {
            try
            {
                Image img = Image.FromFile(path);
                return new Texture2D(new Bitmap(img, width, height));
            }
            catch (Exception ex)
            {
                return new Texture2D(new Bitmap(width, height));
            }
        }
    }
}
