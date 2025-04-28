using ComputerGraphics_Rasterization.RenderLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace ComputerGraphics_Rasterization.Shapes
{
    internal class RoundedRectangleShape : IMovableShape
    {
        public uint ZIndex { get; set; }
        public int X0 { get; set; }
        public int Y0 { get; set; }
        public int X1 { get; set; }
        public int Y1 { get; set; }
        public int Radius { get; set; }
        public Color Color { get; set; }

        private int clickCount = 0;
        private Color selectedColor;

        public bool IsComplete => clickCount >= 3;

        public RoundedRectangleShape(int x0, int y0, int x1, int y1, int radius, Color color, uint zIndex = 0)
        {
            X0 = x0;
            Y0 = y0;
            X1 = x1;
            Y1 = y1;
            Radius = radius;
            Color = color;
            ZIndex = zIndex;
        }

        public RoundedRectangleShape(Color selectedColor)
        {
            this.selectedColor = selectedColor;
        }

        public void AddClick(int x, int y)
        {
            if (clickCount == 0)
            {
                X0 = x;
                Y0 = y;
            }
            else if (clickCount == 1)
            {
                Radius = (int)Math.Sqrt((x - X0) * (x - X0) + (y - Y0) * (y - Y0));
            }
            else if (clickCount == 2)
            {
                X1 = x;
                Y1 = y;
            }

            clickCount++;
        }

        public void Draw(CanvasRenderer renderer)
        {
            if (clickCount < 3)
                return;

            double dx = X1 - X0;
            double dy = Y1 - Y0;
            double length = Math.Sqrt(dx * dx + dy * dy);

            double perpX = -dy / length;
            double perpY = dx / length;

            int offsetX = (int)(Radius * perpX);
            int offsetY = (int)(Radius * perpY);

            DDARenderer.DrawLine(renderer, X0 + offsetX, Y0 + offsetY, X1 + offsetX, Y1 + offsetY, Color, 1);
            DDARenderer.DrawLine(renderer, X0 - offsetX, Y0 - offsetY, X1 - offsetX, Y1 - offsetY, Color, 1);

            var cutVec = (X1 - X0, Y1 - Y0);
            MidpointCircleRenderer.DrawCircle(renderer, X0, Y0, Radius, Color, cutVec, true);
            MidpointCircleRenderer.DrawCircle(renderer, X1, Y1, Radius, Color, cutVec, false);

        }


        public bool IsTargeted(int x, int y)
        {
            double dx = X1 - X0;
            double dy = Y1 - Y0;
            double length = Math.Sqrt(dx * dx + dy * dy);

            if (length == 0) return false;

            double distance = Math.Abs(dy * x - dx * y + X1 * Y0 - Y1 * X0) / length;
            return distance <= Radius + 5;
        }


        public override string ToString()
        {
            return $"Rounded Rectangle Z-Index: {ZIndex}";
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
            double distToFirstCenter = Math.Sqrt((x - X0) * (x - X0) + (y - Y0) * (y - Y0));
            double distToSecondCenter = Math.Sqrt((x - X1) * (x - X1) + (y - Y1) * (y - Y1));

            if (distToFirstCenter <= 10) return 0;
            if (distToSecondCenter <= 10) return 1;

            return null;
        }

        public void MoveHandle(int handleId, int newX, int newY)
        {
            if (handleId == 0)
            {
                X0 = newX;
                Y0 = newY;
            }
            else if (handleId == 1)
            {
                X1 = newX;
                Y1 = newY;
            }
        }
    }
}
