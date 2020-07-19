using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftRenderer
{
    public class GraphicsDevice : IDisposable
    {
        private readonly Bitmap canvas;

        private readonly Graphics canvasGraphics;

        private int Height => canvas.Height;

        private int Width => canvas.Width;

        public GraphicsDevice(Bitmap bitmap)
        {
            canvas = bitmap;
            canvasGraphics = Graphics.FromImage(canvas);
        }

        public void Clear(Color color)
        {
            canvasGraphics.Clear(color);
        }

        //TODO:替换成自己的画线算法
        public void DrawLine(Point p0, Point p1, Color color)
        {
            canvasGraphics.DrawLine(new Pen(color), p0, p1);
        }

        public void DrawLine(Vector3 p0, Vector3 p1, Color color)
        {
            DrawLine(new Point((int)p0.X, (int)p0.Y), new Point((int)p1.X, (int)p1.Y), color);
        }

        public void DrawPoint(int x, int y, Color color)
        {
            SetPixel(x, y, color);
        }

        public void DrawString(string str, Font font, Brush brush, float x, float y)
        {
            canvasGraphics.DrawString(str, font, brush, x, y);
        }

        public void DrawTriangle(Vector3 pa, Vector3 pb, Vector3 pc, Color color)
        {
            DrawLine(pa, pb, color);
            DrawLine(pa, pc, color);
            DrawLine(pc, pb, color);
        }

        private void SetPixel(int x, int y, Color color)
        {
            canvas.SetPixel(x, y, color);
        }

        private void DrawMesh(MeshRenderer meshRenderer)
        {
            Matrix worldMatrix = meshRenderer.transform.translateMatrix;

        }


        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
