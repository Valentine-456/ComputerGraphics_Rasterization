using ComputerGraphics_Rasterization.RenderLogic;
using ComputerGraphics_Rasterization.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace ComputerGraphics_Rasterization.Shapes
{
    internal class LineShape : IMovableShape
    {
        public uint ZIndex { get; set; }

        public int X0 { get; set; }
        public int Y0 { get; set; }
        public int X1 { get; set; }
        public int Y1 { get; set; }
        public Color Color { get; set; }
        public int Thickness { get; set; }


        public LineShape(int x0, int y0, int x1, int y1, Color color, int thickness = 1, uint zIndex = 0) 
        {
            X0 = x0;
            Y0 = y0;
            X1 = x1;
            Y1 = y1;
            Color = color;
            Thickness = thickness;
            ZIndex = zIndex;
        }

        public void Draw(CanvasRenderer renderer)
        {
            if (SettingsService.IsAntialiasingEnabled)
                GouptaSproullsRenderer.DrawLine(renderer, X0, Y0, X1, Y1, Color, Thickness);
            else
                DDARenderer.DrawLine(renderer, X0, Y0, X1, Y1, Color, Thickness);
        }

        public bool IsTargeted(int x, int y)
        {
            double dx = X1 - X0;
            double dy = Y1 - Y0;
            double length = Math.Sqrt(dx * dx + dy * dy);

            if (length == 0) return false;

            double distance = Math.Abs(dy * x - dx * y + X1 * Y0 - Y1 * X0) / length;
            return distance <= Thickness / 2 + 2;

        }

        public override string ToString()
        {
            return $"Line Z-Index: {ZIndex}";
        }

        public void MoveEntireFigure(int dx, int dy)
        {
            X0 += dx;
            Y0 += dy;
            X1 += dx;
            Y1 += dy;
        }

        public int? FindClosestHandle(int x, int y)
        {
            double distStart = Math.Sqrt((x - X0) * (x - X0) + (y - Y0) * (y - Y0));
            double distEnd = Math.Sqrt((x - X1) * (x - X1) + (y - Y1) * (y - Y1));
            if ((distStart > 10) && (distEnd > 10))
                return null;
            return distStart < distEnd ? 0 : 1;
        }

        public void MoveHandle(int handleId, int newX, int newY)
        {
            if (handleId == 0)
            {
                X0 = newX;
                Y0 = newY;
            }
            else
            {
                X1 = newX;
                Y1 = newY;
            }
        }
    }
}
