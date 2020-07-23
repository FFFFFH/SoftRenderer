using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace SoftRenderer
{
    public class Debug
    {
        public static bool isTestMod = true;
        public static GraphicsDevice graphics;
        private static Font defaultFont;
        private static int posY = 200;
        private static List<string> LogContentList = new List<string>();
        private static List<Brush> LogBrushList = new List<Brush>();
        private static Dictionary<string,bool> ContentDict = new Dictionary<string, bool>();

        public static void Init(GraphicsDevice _graphics)
        {
            graphics = _graphics;
            defaultFont = new Font(FontFamily.GenericMonospace, 15);
        }
        
        public static void ShowLogs()
        {
            for (int i = 0; i < LogContentList.Count; i++)
            {
                graphics.DrawString(LogContentList[i],
                    defaultFont, LogBrushList[i], 0, posY + 20 * i);
            }
        }

        public static void Log(string content, Brush brush = null)
        {
            if (brush == null)
                brush = Brushes.Black;
            LogContentList.Add(content);
            LogBrushList.Add(brush);
        }

        public static void ClearLogs()
        {
            LogContentList.Clear();
            LogBrushList.Clear();
        }
    }
}
