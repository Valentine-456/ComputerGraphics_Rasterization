using ComputerGraphics_Rasterization.RenderLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace ComputerGraphics_Rasterization.Shapes
{
    internal class LineShape : IShape
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

    }
}
