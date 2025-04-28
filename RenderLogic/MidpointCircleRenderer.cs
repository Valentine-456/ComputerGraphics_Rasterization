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
            DrawCircle(renderer, centerX, centerY, radius, color, null, null);
        }

        public static void DrawCircle(CanvasRenderer renderer, int centerX, int centerY, int radius, Color color,
                                      (int x, int y)? cutVector, bool? drawPositiveSide)
        {
            int dE = 3;
            int dSE = 5 - 2 * radius;
            int d = 1 - radius;
            int x = 0;
            int y = radius;

            PlotCirclePoints(renderer, centerX, centerY, x, y, color, cutVector, drawPositiveSide);

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

                PlotCirclePoints(renderer, centerX, centerY, x, y, color, cutVector, drawPositiveSide);
            }
        }

        private static void PlotCirclePoints(CanvasRenderer renderer, int centerX, int centerY, int x, int y, Color color,
                                             (int x, int y)? cutVector, bool? drawPositiveSide)
        {
            TrySetPixel(renderer, centerX + x, centerY + y, centerX, centerY, color, cutVector, drawPositiveSide);
            TrySetPixel(renderer, centerX - x, centerY + y, centerX, centerY, color, cutVector, drawPositiveSide);
            TrySetPixel(renderer, centerX + x, centerY - y, centerX, centerY, color, cutVector, drawPositiveSide);
            TrySetPixel(renderer, centerX - x, centerY - y, centerX, centerY, color, cutVector, drawPositiveSide);
            TrySetPixel(renderer, centerX + y, centerY + x, centerX, centerY, color, cutVector, drawPositiveSide);
            TrySetPixel(renderer, centerX - y, centerY + x, centerX, centerY, color, cutVector, drawPositiveSide);
            TrySetPixel(renderer, centerX + y, centerY - x, centerX, centerY, color, cutVector, drawPositiveSide);
            TrySetPixel(renderer, centerX - y, centerY - x, centerX, centerY, color, cutVector, drawPositiveSide);
        }

        private static void TrySetPixel(CanvasRenderer renderer, int px, int py, int cx, int cy, Color color,
                                        (int x, int y)? cutVector, bool? drawPositiveSide)
        {
            if (cutVector.HasValue && drawPositiveSide.HasValue)
            {
                int vecX = px - cx;
                int vecY = py - cy;

                int cutVecX = cutVector.Value.x;
                int cutVecY = cutVector.Value.y;

                int dot = vecX * cutVecX + vecY * cutVecY;

                if (drawPositiveSide.Value && dot > 0)
                    return;
                if (!drawPositiveSide.Value && dot < 0)
                    return;
            }

            renderer.SetPixel(px, py, color);
        }
    }

}
