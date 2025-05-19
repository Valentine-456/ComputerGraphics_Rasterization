using ComputerGraphics_Rasterization.RenderLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows;
using ComputerGraphics_Rasterization.Services;

namespace ComputerGraphics_Rasterization.Shapes
{
    internal class PolygonShape : IMovableShape
    {
        public uint ZIndex { get; set; }
        public List<Point> Vertices { get; private set; } = new List<Point>();
        public Color Color { get; set; }
        public int Thickness { get; set; }
        public bool IsClosed { get; private set; } = false;

        public PolygonShape(Color color, int thickness = 1, uint zIndex = 0)
        {
            Color = color;
            Thickness = thickness;
            ZIndex = zIndex;
        }

        public void Close()
        {
            IsClosed = true;
        }

        public void AddVertex(int x, int y)
        {
            Vertices.Add(new Point(x, y));
        }

        public bool CloseToFirstVertex(int x, int y, int threshold = 10)
        {
            if (Vertices.Count == 0)
                return false;

            var first = Vertices[0];
            double dist = Math.Sqrt((x - first.X) * (x - first.X) + (y - first.Y) * (y - first.Y));
            return dist <= threshold;
        }

        public bool IsTargeted(int x, int y)
        {
            foreach (var vertex in Vertices)
            {
                double dist = Math.Sqrt((x - vertex.X) * (x - vertex.X) + (y - vertex.Y) * (y - vertex.Y));
                if (dist <= 5)
                    return true;
            }

            for (int i = 0; i < Vertices.Count; i++)
            {
                Point a = Vertices[i];
                Point b = Vertices[(i + 1) % Vertices.Count];
                if (DistanceFromLine(a, b, new Point(x, y)) <= 5)
                    return true;
            }

            return false;
        }

        public void Draw(CanvasRenderer renderer)
        {
            if (Vertices.Count < 2)
                return;

            for (int i = 0; i < Vertices.Count - 1; i++)
            {
                var p1 = Vertices[i];
                var p2 = Vertices[i + 1];
                if (SettingsService.IsAntialiasingEnabled)
                    GouptaSproullsRenderer.DrawLine(renderer, (int)p1.X, (int)p1.Y, (int)p2.X, (int)p2.Y, Color, Thickness);
                else
                    DDARenderer.DrawLine(renderer, (int)p1.X, (int)p1.Y, (int)p2.X, (int)p2.Y, Color, Thickness);
            }

            if (IsClosed)
            {
                if (IsFilled && FillColor.HasValue)
                {
                    Fill(renderer);
                }
                var first = Vertices[0];
                var last = Vertices[Vertices.Count - 1];
                if (SettingsService.IsAntialiasingEnabled)
                    GouptaSproullsRenderer.DrawLine(renderer, (int)last.X, (int)last.Y, (int)first.X, (int)first.Y, Color, Thickness);
                else
                    DDARenderer.DrawLine(renderer, (int)last.X, (int)last.Y, (int)first.X, (int)first.Y, Color, Thickness);
            }
        }


        public void MoveEntireFigure(int dx, int dy)
        {
            for (int i = 0; i < Vertices.Count; i++)
            {
                Vertices[i] = new Point(Vertices[i].X + dx, Vertices[i].Y + dy);
            }
        }

        public int? FindClosestHandle(int x, int y)
        {
            for (int i = 0; i < Vertices.Count; i++)
            {
                double dist = Math.Sqrt((x - Vertices[i].X) * (x - Vertices[i].X) + (y - Vertices[i].Y) * (y - Vertices[i].Y));
                if (dist <= 10)
                    return i;
            }
            return null;
        }

        public virtual void MoveHandle(int handleId, int newX, int newY)
        {
            if (handleId >= 0 && handleId < Vertices.Count)
            {
                Vertices[handleId] = new Point(newX, newY);
            }
        }

        private double DistanceFromLine(Point a, Point b, Point p)
        {
            double A = p.X - a.X;
            double B = p.Y - a.Y;
            double C = b.X - a.X;
            double D = b.Y - a.Y;

            double dot = A * C + B * D;
            double len_sq = C * C + D * D;
            double param = len_sq != 0 ? dot / len_sq : -1;

            double xx, yy;

            if (param < 0)
            {
                xx = a.X;
                yy = a.Y;
            }
            else if (param > 1)
            {
                xx = b.X;
                yy = b.Y;
            }
            else
            {
                xx = a.X + param * C;
                yy = a.Y + param * D;
            }

            double dx = p.X - xx;
            double dy = p.Y - yy;
            return Math.Sqrt(dx * dx + dy * dy);
        }

        public override string ToString()
        {
            return $"Polygon {Vertices.Count()} vertices, Z-Index: {ZIndex}";
        }

        public bool IsConvex()
        {
            if (Vertices.Count < 3) return false;

            bool? sign = null;
            int count = Vertices.Count;

            for (int i = 0; i < count; i++)
            {
                var a = Vertices[i];
                var b = Vertices[(i + 1) % count];
                var c = Vertices[(i + 2) % count];

                var cross = ((b.X - a.X) * (c.Y - a.Y)) - ((b.Y - a.Y) * (c.X - a.X));
                if (cross == 0) continue;

                if (sign == null)
                    sign = cross > 0;
                else if ((cross > 0) != sign)
                    return false;
            }

            return true;
        }

        public bool IsFilled { get; set; } = false;
        public Color? FillColor { get; set; } = Colors.White;
        public string FillImagePath { get; set; } = null;

        public void Fill(CanvasRenderer renderer)
        {
            if (!IsClosed || Vertices.Count < 3 || FillColor == null)
                return;

            List<int> sortedIndices = Vertices
                .Select((pt, idx) => new { pt, idx })
                .OrderBy(v => v.pt.Y)
                .Select(v => v.idx)
                .ToList();

            List<ActiveEdge> AET = new List<ActiveEdge>();

            int k = 0;
            int y = (int)Vertices[sortedIndices[0]].Y;
            int ymax = (int)Vertices[sortedIndices[sortedIndices.Count - 1]].Y;

            while (y <= ymax)
            {
                while (k < sortedIndices.Count && (int)Vertices[sortedIndices[k]].Y == y)
                {
                    int i = sortedIndices[k];
                    int count = Vertices.Count;
                    Point pi = Vertices[i];
                    Point prev = Vertices[(i - 1 + count) % count];
                    Point next = Vertices[(i + 1) % count];

                    if ((int)prev.Y > (int)pi.Y)
                        AET.Add(CreateEdge(pi, prev));
                    if ((int)next.Y > (int)pi.Y)
                        AET.Add(CreateEdge(pi, next));

                    k++;
                }

                AET.RemoveAll(e => e.YMax == y);

                AET.Sort((a, b) => a.X.CompareTo(b.X));

                for (int i = 0; i + 1 < AET.Count; i += 2)
                {
                    int xStart = (int)Math.Round(AET[i].X);
                    int xEnd = (int)Math.Round(AET[i + 1].X);
                    for (int x = xStart; x <= xEnd; x++)
                    {
                        renderer.SetPixel(x, y, FillColor.Value);
                    }
                }

                y++;
                foreach (var edge in AET)
                {
                    edge.X += edge.InvSlope;
                }
            }
        }


        private ActiveEdge CreateEdge(Point p1, Point p2)
        {
            if (p1.Y > p2.Y)
            {
                Point temp = p1;
                p1 = p2;
                p2 = temp;
            }

            double invSlope = (p2.Y - p1.Y) == 0 ? 0 : (p2.X - p1.X) / (p2.Y - p1.Y);
            return new ActiveEdge(p1.X, invSlope, (int)p2.Y);
        }


    }
}
