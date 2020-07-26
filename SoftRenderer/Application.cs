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
    public enum RenderType
    {
        Wireframe,
        Fill,
        GradientColor,
    }

    class Application
    {
        public static GraphicsDevice GraphicsDevice;
        public static RenderType RenderType = RenderType.Fill;

        private Form form; //窗口
        private GraphicsBuffer buffer; //双缓冲
    
        private Font defaultFont;
        private const int targetFPS = 30;
        private readonly TimeSpan maxElapsedTime = TimeSpan.FromMilliseconds(1000.0 / targetFPS);
        private Debug debug;
        private static List<Renderer> Renderers = new List<Renderer>();


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
            SceneLogic.Instance.Init();
            while (!form.IsDisposed)
            {
                stopwatch.Start();
                ClearBackground();
                Debug.ClearLogs();
                Application.GraphicsDevice = buffer.BackGroundGraphicsDevice;

                SceneLogic.Instance.Update();

                Render(); 

                DrawFPS(deltaTime);

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

        public static void AddRenderer(Renderer renderer)
        {
            Renderers.Add(renderer);
        }

        private void Render()
        {
            //TODO:这里可以拓展为多个相机，根据每个相机CullingMask
            //筛出要渲染的Render进行渲染
            Camera camera = Camera.main;
            for (int i = 0; i < Renderers.Count; i++)
            {
                Renderer renderer = Renderers[i];
                if(!renderer.enable) continue;
                var renderTrans = renderer.transform;
                renderTrans.world2View = camera.GetWorldToView();
                renderTrans.viewToClip = camera.GetPerspectiveMatrix();
                renderer.Render();
            }
        }
    }
}
