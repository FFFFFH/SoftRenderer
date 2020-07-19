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
                Size = new Size(Screen.width, Screen.height),
                StartPosition = FormStartPosition.CenterScreen
            };
            buffer = new GraphicsBuffer(Screen.width, Screen.height);
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
            Mesh mesh = CreateMesh1();
            Transform trans = new Transform();
            trans.position = new Vector3(0, 0, 0);
            trans.rotationAngle = new Vector3(0, 0, 0);
            trans.localScale = new Vector3(2, 2, 2); 
            MeshRenderer meshRenderer = new MeshRenderer(trans, mesh);
            return meshRenderer;
        }

        private Mesh CreateMesh1()
        {
            Vector3[] vects = new Vector3[6];
            vects[0] = new Vector3(-0.5f, -0.5f, 0);
            vects[1] = new Vector3(0.5f, -0.5f, 0);
            vects[2] = new Vector3(0.5f, 0.5f, 0);
            vects[3] = new Vector3(-0.5f, 0.5f, 0);

            vects[4] = new Vector3(0.5f, 0, 0.5f);
            vects[5] = new Vector3(0.5f, 1, 0.5f);


            Surface[] surfaces = new Surface[2];
            surfaces[0].A = 0;
            surfaces[0].B = 1;
            surfaces[0].C = 2;

            surfaces[1].A = 0;
            surfaces[1].B = 2;
            surfaces[1].C = 3;

            //surfaces[2].A = 1;
            //surfaces[2].B = 4;
            //surfaces[2].C = 5;

            //surfaces[3].A = 1;
            //surfaces[3].B = 5;
            //surfaces[3].C = 2;

            return new Mesh(vects, surfaces);
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

            Surface[] surfaces = new Surface[12];
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

            surfaces[8].A = 3;
            surfaces[8].B = 2;
            surfaces[8].C = 5;
            surfaces[9].A = 3;
            surfaces[9].B = 5;
            surfaces[9].C = 7;

            surfaces[10].A = 1;
            surfaces[10].B = 0;
            surfaces[10].C = 4;
            surfaces[11].A = 0;
            surfaces[11].B = 6;
            surfaces[11].C = 4;


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
