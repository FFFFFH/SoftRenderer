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

        public Bitmap texture;

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

        public void DrawLine(Vector p0, Vector p1, Color color)
        {
            DrawLine(new Point((int)p0.x,(int)p0.y), new Point((int)p1.x, (int)p1.y), color);
        }

        //绘制纯色扫描线
        public void DrawScanLine(int x1, int x2, int y, Color color)
        {
            DrawLine(new Point(x1, y), new Point(x2, y), color);
        }

        //只对颜色进行插值
        public void DrawScanLine(int x1, int x2, int y, Color c1, Color c2)
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

        public void DrawScanLine(Vertex left, Vertex right)
        {
            Texture2D texture = Application.Texture;
            int length = (int)(right.pos.x - left.pos.x);
            float onePerLength = length == 0 ? 0 : 1f/length;
            int y = (int)left.pos.y;
            for (int i = (int)left.pos.x; i < right.pos.x; i++)
            {
                float t = (i - left.pos.x) * onePerLength;
                float onePerZ = MathUtility.Lerp(left.onePerZ, right.onePerZ, t);
                if (false) //TODO: 深度测试
                {
                    continue;
                }
                //x'、y'与1/z是线性关系，u/z、v/z 与 x’、y’也是线性关系
                //所以可以对 1/z关于 x’、y'插值得到 1/z’
                //然后对u/z、v/z 关于 x’、y’插值得到 u'/z' 和 v'/z'
                // u'/z' 和 v'/z'除以 1/z’得出正确的u’、v'
                float tz = 1 / onePerZ;
                float u = MathUtility.Lerp(left.u, right.u, t) * tz * (texture.width - 1);
                float v = MathUtility.Lerp(left.v, right.v, t) * tz * (texture.height - 1);
                int uIndex = (int)Math.Round(u, MidpointRounding.AwayFromZero);
                int vIndex = (int)Math.Round(v, MidpointRounding.AwayFromZero);

                Color color = Color.White;
                //if (uIndex > 128)
                //{
                //    color = Color.Yellow;
                //    if (vIndex > 128)
                //    {
                //        color = Color.YellowGreen;
                //    }
                //}
                //else if (vIndex > 128)
                //{
                //    color = Color.SlateBlue;
                //}

                color = texture.ReadTexture(uIndex, vIndex);
                DrawPoint(i, y, color);
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
        public void DrawTriangleLine(Vector pa, Vector pb, Vector pc, Color color)
        {
            DrawLine(pa, pb, color);
            DrawLine(pa, pc, color);
            DrawLine(pc, pb, color);
        }

        //v1为最高点
        public void FillBottomFlatTriangle(Vertex v1, Vertex v2, Vertex v3)
        {
            Vertex topVert = v1;
            Vertex leftBottomVert = v2.pos.x < v3.pos.x ? v2 : v3;
            Vertex rightBottomVert = v2.pos.x > v3.pos.x ? v2 : v3;

            Vector p1 = v1.pos;
            Vector p2 = leftBottomVert.pos;
            Vector p3 = rightBottomVert.pos;
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
                float t = (p2.y - scanLineY) * onePerLength;
                Vertex right = TransformUtility.LerpVertexInScreenSpace(rightBottomVert, topVert, t);
                Vertex left = TransformUtility.LerpVertexInScreenSpace(leftBottomVert, topVert, t);

                if (Application.RenderType == RenderType.VertexColor)
                {
                    DrawScanLine((int)curX1, (int)curX2, scanLineY, left.color, right.color);
                }
                else if(Application.RenderType == RenderType.Texture)
                {
                    DrawScanLine(left, right);
                }

                //避免绘制非常窄的三角形时线段出界
                if ((invSlope1 < 0 && curX1 + invSlope1 >= p2.x) || (invSlope1 > 0 && curX1 + invSlope1 <= p2.x))
                {
                    curX1 += invSlope1;
                }
                if ((invSlope2 < 0 && curX2 + invSlope2 >= p3.x) || (invSlope2 > 0 && curX2 + invSlope2 <= p3.x))
                {
                    curX2 += invSlope2;
                }
            }
        }

        // v1为左上，v2为右上，v3为最低点
        public void FillTopFlatTriangle(Vertex v1, Vertex v2, Vertex v3)
        {
            Vertex bottomVert = v3;
            Vertex leftTopVert = v1.pos.x < v2.pos.x ? v1 : v2;
            Vertex rightTopVert = v1.pos.x > v2.pos.x ? v1 : v2;

            Vector p1 = leftTopVert.pos;
            Vector p2 = rightTopVert.pos;
            Vector p3 = v3.pos;
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
                float t = (scanLineY - p1.y) * onePerLength;
                Vertex left = TransformUtility.LerpVertexInScreenSpace(leftTopVert, bottomVert, t);
                Vertex right = TransformUtility.LerpVertexInScreenSpace(rightTopVert, bottomVert, t);

                if (Application.RenderType == RenderType.VertexColor)
                {
                    DrawScanLine((int)curX1, (int)curX2, scanLineY, left.color, right.color);
                }
                else if (Application.RenderType == RenderType.Texture)
                {
                    DrawScanLine(left, right);
                }

                if ((invSlope1 > 0 && curX1 - invSlope1 >= p1.x) || (invSlope1 < 0 && curX1 - invSlope1 <= p1.x))
                {
                    curX1 -= invSlope1;
                }
                if ((invSlope2 < 0 && curX2 - invSlope2 <= p2.x) || (invSlope2 > 0 && curX2 - invSlope2 >= p2.x))
                {
                    curX2 -= invSlope2;
                }
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
                float t = (verts[1].pos.y - verts[0].pos.y) / (verts[2].pos.y - verts[0].pos.y);
                Vertex v4 = TransformUtility.LerpVertexInScreenSpace(verts[0], verts[2], t);
                v4.color = ColorUtility.Lerp(verts[0],verts[1],verts[2], v4);
                float y = verts[1].pos.y;
                float x = (int)(verts[0].pos.x + (((float)verts[1].pos.y - verts[0].pos.y) / ((float)verts[2].pos.y - verts[0].pos.y)) *
                              (verts[2].pos.x - verts[0].pos.x));
                v4.pos = new Vector(x, y, verts[1].pos.z);
                FillBottomFlatTriangle(verts[0], verts[1], v4);
                FillTopFlatTriangle(verts[1], v4, verts[2]);
                //测试用
                //DrawTriangleLine(v1.pos, v2.pos, v3.pos, Color.Black);
            }
        }

        //扫描线填充平底三角面
        public void FillBottomFlatTriangle(Vector v1, Vector v2, Vector v3,Color color = default(Color))
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
        public void FillTopFlatTriangle(Vector v1, Vector v2, Vector v3, Color color = default(Color))
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

        public List<Vector> SortPoint(Vector v1, Vector v2, Vector v3, bool sortByX = false)
        {
            List<Vector> result = new List<Vector>(){v1,v2,v3};
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
        public int CheckInSameLineState(Vector v1, Vector v2, Vector v3)
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

        public void FillTriangle(Vector v1, Vector v2, Vector v3, Color color = default(Color))
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
                Vector v4 = Vector.Zero;
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
