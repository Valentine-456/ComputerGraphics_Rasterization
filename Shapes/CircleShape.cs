using ComputerGraphics_Rasterization.RenderLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace ComputerGraphics_Rasterization.Shapes
{
    internal class CircleShape : IMovableShape
    {
        public uint ZIndex { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int Radius { get; set; }
        public Color Color { get; set; }

        public CircleShape(int centerX, int centerY, int radius, Color color, uint zIndex = 0) 
        {
            X = centerX;
            Y = centerY;
            Radius = radius;
            Color = color;
            ZIndex = zIndex;
        }

        public void Draw(CanvasRenderer renderer)
        {
            MidpointCircleRenderer.DrawCircle(renderer, X, Y, Radius, Color);
        }

        public int? FindClosestHandle(int x, int y)
        {
            return 0;
        }

        public bool IsTargeted(int x, int y)
        {
            double dist = Math.Sqrt((x - X) * (x - X) + (y - Y) * (y - Y));
            return Math.Abs(dist - Radius) <= 5;
        }

        public void MoveEntireFigure(int dx, int dy)
        {
            X += dx;
            Y += dy;
        }

        public void MoveHandle(int handleId, int newX, int newY)
        {
            X = newX;
            Y = newY;
        }

        public override string ToString()
        {
            return $"Circle Z-Index: {ZIndex}";
        }
    }
}
