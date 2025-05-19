using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows;

namespace ComputerGraphics_Rasterization.RenderLogic
{
    internal class SmithScanLineFiller
    {
        public static void Fill(CanvasRenderer renderer, Point seed, Color border, Color fill)
        {
            Stack<Point> stack = new Stack<Point>();
            stack.Push(seed);

            while (stack.Count > 0)
            {
                Point p = stack.Pop();
                int y = (int)p.Y;
                int x = (int)p.X;

                while (x >= 0 && !IsBorder(renderer, x, y, border) && !IsSameColor(renderer, x, y, fill)) x--;
                x++;

                bool spanAbove = false, spanBelow = false;

                while (x < renderer.Width && !IsBorder(renderer, x, y, border) && !IsSameColor(renderer, x, y, fill))
                {
                    renderer.SetPixel(x, y, fill);

                    if (y > 0 && !IsBorder(renderer, x, y - 1, border) && !IsSameColor(renderer, x, y - 1, fill))
                    {
                        if (!spanAbove) stack.Push(new Point(x, y - 1));
                        spanAbove = true;
                    }
                    else spanAbove = false;

                    if (y < renderer.Height - 1 && !IsBorder(renderer, x, y + 1, border) && !IsSameColor(renderer, x, y + 1, fill))
                    {
                        if (!spanBelow) stack.Push(new Point(x, y + 1));
                        spanBelow = true;
                    }
                    else spanBelow = false;

                    x++;
                }
            }
        }

        private static bool IsBorder(CanvasRenderer renderer, int x, int y, Color border) =>
            renderer.GetPixel(x, y) == border;

        private static bool IsSameColor(CanvasRenderer renderer, int x, int y, Color target) =>
            renderer.GetPixel(x, y) == target;
    }
}
