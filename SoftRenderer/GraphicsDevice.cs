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
            DrawLine(new Point((int)p0.x,(int)p0.y), new Point((int)p1.x, (int)p1.y), color);
        }

        //绘制水平扫描线
        public void DrawScanLine(int x1, int x2, int y, Color color)
        {
            //TODO:替换为逐像素
            DrawLine(new Point(x1, y), new Point(x2, y), color);
        }

        public void DrawPoint(int x, int y, Color color)
        {
            SetPixel(x, y, color);
        }

        public void DrawString(string str, Font font, Brush brush, float x, float y)
        {
            canvasGraphics.DrawString(str, font, brush, x, y);
        }

        //线框
        public void DrawTriangleLine(Vector3 pa, Vector3 pb, Vector3 pc, Color color)
        {
            DrawLine(pa, pb, color);
            DrawLine(pa, pc, color);
            DrawLine(pc, pb, color);
        }

        //三角面
        public void DrawTriangleFill(Vector3 pa, Vector3 pb, Vector3 pc, Color color)
        {
            

        }

        //扫描线填充平底三角面
        public void FillBottomFlatTriangle(Vector3 v1, Vector3 v2, Vector3 v3,Color color = default(Color))
        {
            float invSlope1 = (v2.x - v1.x) / (v2.y - v1.y);
            float invSlope2 = (v3.x - v1.x) / (v3.y - v1.y);

            float curX1 = v1.x;
            float curX2 = v1.x;
            
            //屏幕原点在左上，所以是Y++不是--
            for (int scanLineY = (int)v1.y; scanLineY <= v2.y; scanLineY++)
            {
                DrawScanLine((int) curX1, (int) curX2, scanLineY, color);
                curX1 += invSlope1;
                curX2 += invSlope2;
            }
        }

        //扫描线填充平顶三角面
        public void FillTopFlatTriangle(Vector3 v1, Vector3 v2, Vector3 v3, Color color = default(Color))
        {
            float invSlope1 = (v3.x - v1.x) / (v3.y - v1.y);
            float invSlope2 = (v3.x - v2.x) / (v3.y - v2.y);

            float curX1 = v3.x;
            float curX2 = v3.x;

            //屏幕原点在左上，所以是Y++不是--
            for (int scanLineY = (int)v3.y; scanLineY > v1.y; scanLineY--)
            {
                DrawScanLine((int)curX1, (int)curX2, scanLineY, color);
                curX1 -= invSlope1;
                curX2 -= invSlope2;
            }
        }

        public List<Vector3> SortPoint(Vector3 v1, Vector3 v2, Vector3 v3, bool sortByX = false)
        {
            List<Vector3> result = new List<Vector3>(){v1,v2,v3};
            result.Sort((a, b) =>
            {
                float value1 = sortByX ? a.x : a.y;
                float value2 = sortByX ? b.x : b.y;
                if (value1 > value2)
                {
                    return 1;
                }
                else if(MathUtility.IsEqual(value1, value2))
                {
                    return 0;
                }
                return -1;
            });
            return result;
        }

        // -1 : 同行 ， 0 ：不等  1 ：同列
        public int CheckInSameLineState(Vector3 v1, Vector3 v2, Vector3 v3)
        {
            if (MathUtility.IsEqual(v1.y - v2.y - v3.y, 0))
            {
                return 1;
            }
            else if (MathUtility.IsEqual(v1.x - v2.x - v3.x, 0))
            {
                return -1;
            }
            return 0;
        }

        //将三角面切割成平顶平底两个三角面绘制
        public void FillTriangle(Vector3 v1, Vector3 v2, Vector3 v3, Color color = default(Color))
        {
            int lineState = CheckInSameLineState(v1, v2, v3);
            if (lineState != 0)
            {
                var vertPoint = SortPoint(v1, v2, v3, lineState == -1);
                DrawLine(vertPoint[0], vertPoint[2], color);
                return;
            }
            var verts = SortPoint(v1, v2, v3);
            if (MathUtility.IsEqual(verts[1].y,verts[2].y))
            {
                FillBottomFlatTriangle(verts[0],verts[1],verts[2], color);
            }
            else if (verts[0].y == verts[1].y)
            {
                FillTopFlatTriangle(verts[0], verts[1], verts[2], color);
            }
            else
            {
                Vector3 v4 = Vector3.Zero;
                v4.y = verts[1].y;
                v4.x = (int) (verts[0].x + (((float) verts[1].y - verts[0].y) / ((float) verts[2].y - verts[0].y)) *
                              (verts[2].x - verts[0].x));

                FillBottomFlatTriangle(verts[0], verts[1], v4, color);
                FillTopFlatTriangle(verts[1], v4, verts[2], color);

                DrawTriangleLine(v1, v2, v3, Color.Black);

            }
        }

        private void SetPixel(int x, int y, Color color)
        {
            canvas.SetPixel(x, y, color);
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
