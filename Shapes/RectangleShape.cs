using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace ComputerGraphics_Rasterization.Shapes
{
    internal class RectangleShape : PolygonShape
    {
        private Point Corner1;
        private Point Corner2;
        public RectangleShape(Point corner1, Color color, int thickness = 1, uint zIndex = 0)
            : base(color, thickness, zIndex)
        {
            Corner1 = corner1;
            Corner2 = corner1;
            UpdateVertices();
            Close();
        }

        public void UpdateCorner2(Point corner2)
        {
            Corner2 = corner2;
            UpdateVertices();
        }

        private void UpdateVertices()
        {
            Vertices.Clear();

            double x1 = Math.Min(Corner1.X, Corner2.X);
            double y1 = Math.Min(Corner1.Y, Corner2.Y);
            double x2 = Math.Max(Corner1.X, Corner2.X);
            double y2 = Math.Max(Corner1.Y, Corner2.Y);

            Vertices.Add(new Point(x1, y1));
            Vertices.Add(new Point(x2, y1));
            Vertices.Add(new Point(x2, y2));
            Vertices.Add(new Point(x1, y2));
        }

        public override void MoveHandle(int handleId, int newX, int newY)
        {
            if (handleId < 0 || handleId >= 4)
                return;

            switch (handleId)
            {
                case 0:
                    Corner1 = new Point(newX, newY);
                    break;
                case 1:
                    Corner1 = new Point(Corner1.X, newY);
                    Corner2 = new Point(newX, Corner2.Y);
                    break;
                case 2:
                    Corner2 = new Point(newX, newY);
                    break;
                case 3:
                    Corner1 = new Point(newX, Corner1.Y);
                    Corner2 = new Point(Corner2.X, newY);
                    break;
            }

            UpdateVertices();
        }

        public override string ToString()
        {
            return $"Rectangle {Corner1} to {Corner2}, Z-Index: {ZIndex}";
        }
    }
}
