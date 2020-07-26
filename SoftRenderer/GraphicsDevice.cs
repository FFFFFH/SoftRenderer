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

        public static GraphicsDevice main;

        public GraphicsDevice(Bitmap bitmap)
        {
            main = this;
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

        public void DrawScaneLine(int x1, int x2, int y, Color c1, Color c2)
        {
            if (x2 == x1)
            {
                SetPixel(x1, y, c1);
                return;
            }
            int left = x1 < x2 ? x1 : x2;
            Color leftColor = x1 < x2 ? c1 : c2;
            int right = x2 > x1 ? x2 : x1;
            Color rightColor = x2 > x1 ? c2 : c1;
            float iv = 1/(float)(right - left);
            for (int i = left; i <= right; i++)
            {
                float t = (i - x1) * iv;
                Color curC = ColorUtility.Lerp(leftColor, rightColor, t);
                SetPixel(i, y, curC);
            }
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

        //v1为最高点，v2为左下，v3为右下
        public void FillBottomFlatTriangle(Vertex v1, Vertex v2, Vertex v3)
        {
            Vertex leftBottomVert = v2.pos.x < v3.pos.x ? v2 : v3;
            Vertex rightBottomVert = v2.pos.x > v3.pos.x ? v2 : v3;

            Vector3 p1 = v1.pos;
            Vector3 p2 = leftBottomVert.pos.x < rightBottomVert.pos.x ? leftBottomVert.pos : rightBottomVert.pos;
            Vector3 p3 = leftBottomVert.pos.x > rightBottomVert.pos.x ? leftBottomVert.pos : rightBottomVert.pos;
            float invSlope1 = (p2.x - p1.x) / (p2.y - p1.y);
            float invSlope2 = (p3.x - p1.x) / (p3.y - p1.y);
            float onePerLength = 1/(p2.y - p1.y);
            float curX1 = p1.x;
            float curX2 = p1.x;

            Color top = v1.color;
            Color leftBottom = leftBottomVert.color;
            Color rightBottom = rightBottomVert.color;

            //屏幕原点在左上，所以是Y++不是--
            for (int scanLineY = (int)p1.y; scanLineY <= p2.y; scanLineY++)
            {
                Color rightColor = ColorUtility.Lerp(rightBottom, top, (p2.y - scanLineY) * onePerLength);
                Color leftColor = ColorUtility.Lerp(leftBottom, top, (p2.y - scanLineY) * onePerLength);
                DrawScaneLine((int)curX1, (int)curX2, scanLineY, leftColor, rightColor);
                curX1 += invSlope1;
                curX2 += invSlope2;
            }
        }

        // v1为左上，v2为右上，v3为最低点
        public void FillTopFlatTriangle(Vertex v1, Vertex v2, Vertex v3)
        {
            Vertex leftTopVert = v1.pos.x < v2.pos.x ? v1 : v2;
            Vertex rightTopVert = v1.pos.x > v2.pos.x ? v1 : v2;

            Vector3 p1 = v1.pos.x < v2.pos.x ? v1.pos : v2.pos;
            Vector3 p2 = v1.pos.x > v2.pos.x ? v1.pos : v2.pos;
            Vector3 p3 = v3.pos;
            float invSlope1 = (p3.x - p1.x) / (p3.y - p1.y);
            float invSlope2 = (p3.x - p2.x) / (p3.y - p2.y);
            float onePerLength = 1 / (p3.y - p1.y);
            float curX1 = p3.x;
            float curX2 = p3.x;

            Color bottom = v3.color;
            Color leftTop = leftTopVert.color;
            Color rightTop = rightTopVert.color;

            for (int scanLineY = (int)p3.y; scanLineY >= p2.y; scanLineY--)
            {
                Color rightColor = ColorUtility.Lerp(rightTop, bottom, (scanLineY - p1.y) * onePerLength);
                Color leftColor = ColorUtility.Lerp(leftTop, bottom, (scanLineY - p1.y) * onePerLength);
                DrawScaneLine((int)curX1, (int)curX2, scanLineY, leftColor, rightColor);
                curX1 -= invSlope1;
                curX2 -= invSlope2;
            }
        }

        public void FillTriangle(Vertex v1, Vertex v2, Vertex v3)
        {
            var verts = SortVertex(v1, v2, v3);
            if (MathUtility.IsEqual(verts[1].pos.y, verts[2].pos.y))
            {
                FillBottomFlatTriangle(verts[0], verts[1], verts[2]);
            }
            else if (verts[0].pos.y == verts[1].pos.y)
            {
                FillTopFlatTriangle(verts[0], verts[1], verts[2]);
            }
            else //将三角面切割成平顶平底两个三角面绘制
            {
                Vertex v4 = new Vertex();
                float y = verts[1].pos.y;
                float x = (int)(verts[0].pos.x + (((float)verts[1].pos.y - verts[0].pos.y) / ((float)verts[2].pos.y - verts[0].pos.y)) *
                              (verts[2].pos.x - verts[0].pos.x));
                Color color = ColorUtility.Lerp(verts[0].color, verts[2].color, (verts[1].pos.y - verts[0].pos.y) / (verts[2].pos.y - verts[0].pos.y));
                v4.pos = new Vector3(x, y, verts[1].pos.z);
                v4.color = color;
                FillBottomFlatTriangle(verts[0], verts[1], v4);
                FillTopFlatTriangle(verts[1], v4, verts[2]);
                //DrawLine(v1.pos, v4.pos, Color.Yellow);
                //DrawTriangleLine(verts[0].pos, verts[1].pos, v4.pos, Color.Gold);
                //DrawTriangleLine(verts[1].pos, v4.pos, verts[2].pos, Color.YellowGreen);
                DrawTriangleLine(v1.pos, v2.pos, v3.pos, Color.Black);
            }
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

        public List<Vertex> SortVertex(Vertex v1, Vertex v2, Vertex v3, bool sortByX = false)
        {
            List<Vertex> result = new List<Vertex>() { v1, v2, v3 };
            result.Sort((a, b) =>
            {
                float value1 = sortByX ? a.pos.x : a.pos.y;
                float value2 = sortByX ? b.pos.x : b.pos.y;
                if (value1 > value2)
                {
                    return 1;
                }
                else if (MathUtility.IsEqual(value1, value2))
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
            else //将三角面切割成平顶平底两个三角面绘制
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
