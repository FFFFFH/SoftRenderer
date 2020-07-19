using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SoftRenderer
{
    class Application
    {
        public Form form; //窗口
        private GraphicsBuffer buffer; //双缓冲
        private const int width = 1024;
        private const int height = 512;
        private Font defaultFont;
        private const int targetFPS = 30;
        private readonly TimeSpan maxElapsedTime = TimeSpan.FromMilliseconds(1000.0 / targetFPS);
        private Debug debug;

        public Application()
        {
            Init();
        }

        private void Init()
        {
            form = new Form()
            {
                Size = new Size(1024, 512),
                StartPosition = FormStartPosition.CenterScreen
            };
            buffer = new GraphicsBuffer(width, height);
            defaultFont = new Font(FontFamily.GenericMonospace, 15);
        }

        public void Run()
        {
            form.Show();
            var stopwatch = new Stopwatch();
            var deltaTime = TimeSpan.FromMilliseconds(1000.0 / 60);

            Debug.Init(buffer.BackGroundGraphicsDevice);

            while (!form.IsDisposed)
            {
                stopwatch.Start();
                ClearBackground();
                DrawFPS(deltaTime);
                RenderMesh();

                Debug.ClearLogs();
                Debug.DoTest();
                Debug.ShowLogs();

                buffer.SwapBuffers();
                Present();
                System.Windows.Forms.Application.DoEvents();
                if (stopwatch.Elapsed < maxElapsedTime)
                {
                    Thread.Sleep(maxElapsedTime - stopwatch.Elapsed);
                }
                else
                {
                    deltaTime = maxElapsedTime;
                }
                stopwatch.Stop();
                deltaTime = stopwatch.Elapsed;
                stopwatch.Reset();
            }
        }

        public void DrawFPS(TimeSpan sp)
        {
            var graphic = buffer.BackGroundGraphicsDevice;
            graphic.DrawString($"FPS : {1000.0 / sp.Milliseconds}",
                defaultFont, Brushes.Black,0,0);
        }

        public void RenderMesh()
        {
            MeshRenderer meshRenderer = CreateMeshRenderer();
            meshRenderer.Render(buffer.BackGroundGraphicsDevice);

            //var graphic = buffer.BackGroundGraphicsDevice;
            //for (int i = 0; i < mesh.Vertices.Length; i+=3)
            //{
            //    graphic.DrawTriangle(mesh.Vertices[0], mesh.Vertices[1], mesh.Vertices[2],Color.Pink);
            //}
        }

        public MeshRenderer CreateMeshRenderer()
        {
            Mesh mesh = CreateMesh();
            Transform trans = new Transform();
            trans.position = new Vector3(0, 0, -1);
            trans.rotationAngle = new Vector3(0, 0, 0);
            MeshRenderer meshRenderer = new MeshRenderer(trans,mesh);
            return meshRenderer;
        }

        private Mesh CreateMesh()
        {
            Vector3[] vects = new Vector3[8];
            vects[0] = new Vector3(-1, -1, -1);
            vects[1] = new Vector3(1, -1, -1);
            vects[2] = new Vector3(1, 1, -1);
            vects[3] = new Vector3(-1, 1, -1);
            vects[4] = new Vector3(1, -1, 1);
            vects[5] = new Vector3(1, 1, 1);
            vects[6] = new Vector3(-1, -1, 1);
            vects[7] = new Vector3(-1, 1, 1);

            Surface[] surfaces = new Surface[8];
            surfaces[0].A = 0;
            surfaces[0].B = 1;
            surfaces[0].C = 2;

            surfaces[1].A = 0;
            surfaces[1].B = 2;
            surfaces[1].C = 3;

            surfaces[2].A = 1;
            surfaces[2].B = 4;
            surfaces[2].C = 5;

            surfaces[3].A = 1;
            surfaces[3].B = 5;
            surfaces[3].C = 2;

            surfaces[4].A = 4;
            surfaces[4].B = 6;
            surfaces[4].C = 7;

            surfaces[5].A = 4;
            surfaces[5].B = 7;
            surfaces[5].C = 5;

            surfaces[6].A = 6;
            surfaces[6].B = 0;
            surfaces[6].C = 3;

            surfaces[7].A = 6;
            surfaces[7].B = 3;
            surfaces[7].C = 7;

            Mesh mesh = new Mesh(vects, surfaces);
            return mesh;
        }

        public void ClearBackground()
        {
            var graphic = buffer.BackGroundGraphicsDevice;
            graphic.Clear(Color.AntiqueWhite);
        }

        private void Present()
        {
            using (var graphic = form.CreateGraphics())
            {
                graphic.DrawImage(buffer.Current, Point.Empty);
            }
        }
    }
}
