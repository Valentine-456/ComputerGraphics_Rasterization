using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace ComputerGraphics_Rasterization.RenderLogic
{
    internal class MidpointCircleRenderer
    {
        public static void DrawCircle(CanvasRenderer renderer, int centerX, int centerY, int radius, Color color)
        {
            int dE = 3;
            int dSE = 5 - 2 * radius;
            int d = 1 - radius;
            int x = 0;
            int y = radius;

            PlotCirclePoints(renderer, centerX, centerY, x, y, color);

            while (y > x)
            {
                if (d < 0)
                {
                    d += dE;
                    dE += 2;
                    dSE += 2;
                }
                else
                {
                    d += dSE;
                    dE += 2;
                    dSE += 4;
                    y--;
                }
                x++;

                PlotCirclePoints(renderer, centerX, centerY, x, y, color);
            }
        }

        private static void PlotCirclePoints(CanvasRenderer renderer, int centerX, int centerY, int x, int y, Color color)
        {
            renderer.SetPixel(centerX + x, centerY + y, color);
            renderer.SetPixel(centerX - x, centerY + y, color);
            renderer.SetPixel(centerX + x, centerY - y, color);
            renderer.SetPixel(centerX - x, centerY - y, color);
            renderer.SetPixel(centerX + y, centerY + x, color);
            renderer.SetPixel(centerX - y, centerY + x, color);
            renderer.SetPixel(centerX + y, centerY - x, color);
            renderer.SetPixel(centerX - y, centerY - x, color);
        }
    }

}
