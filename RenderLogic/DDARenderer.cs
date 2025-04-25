using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace ComputerGraphics_Rasterization.RenderLogic
{
    internal class DDARenderer
    {
        internal static void DrawLine(CanvasRenderer renderer, int x0, int y0, int x1, int y1, Color color, int thickness)
        {
            int dy = y1 - y0;
            int dx = x1 - x0;

            int steps = Math.Max(Math.Abs(dx), Math.Abs(dy));
            float mx = dx / (float)steps;
            float my = dy / (float)steps;
            float x = x0;
            float y = y0;
            for (int i = 0; i <= steps; i++)
            {
                DrawThickPixel(renderer, (int)Math.Round(x), (int)Math.Round(y), color, thickness);
                x += mx;
                y += my;
            }

        }

        private static void DrawThickPixel(object rasterizer, int x0, int y0, Color color, int thickness)
        {
            throw new NotImplementedException();
        }
    }
}
