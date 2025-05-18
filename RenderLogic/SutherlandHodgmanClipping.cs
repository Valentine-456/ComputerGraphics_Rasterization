using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ComputerGraphics_Rasterization.RenderLogic
{
    internal class SutherlandHodgmanClipping
    {
        public static List<Point> ClipPolygon(List<Point> subjectPolygon, List<Point> clipPolygon)
        {
            List<Point> outputList = new List<Point>(subjectPolygon);

            for (int i = 0; i < clipPolygon.Count; i++)
            {
                Point cp1 = clipPolygon[i];
                Point cp2 = clipPolygon[(i + 1) % clipPolygon.Count];

                List<Point> inputList = new List<Point>(outputList);
                outputList.Clear();

                if (inputList.Count == 0)
                    break;

                Point s = inputList[inputList.Count - 1];

                foreach (Point e in inputList)
                {
                    if (IsInside(e, cp1, cp2))
                    {
                        if (!IsInside(s, cp1, cp2))
                            outputList.Add(ComputeIntersection(s, e, cp1, cp2));
                        outputList.Add(e);
                    }
                    else if (IsInside(s, cp1, cp2))
                    {
                        outputList.Add(ComputeIntersection(s, e, cp1, cp2));
                    }

                    s = e;
                }
            }

            return outputList;
        }

        private static bool IsInside(Point p, Point edgeStart, Point edgeEnd)
        {
            return (edgeEnd.X - edgeStart.X) * (p.Y - edgeStart.Y) -
                   (edgeEnd.Y - edgeStart.Y) * (p.X - edgeStart.X) >= 0;
        }

        private static Point ComputeIntersection(Point s, Point e, Point cp1, Point cp2)
        {
            double dx1 = e.X - s.X;
            double dy1 = e.Y - s.Y;
            double dx2 = cp2.X - cp1.X;
            double dy2 = cp2.Y - cp1.Y;

            double denominator = dx1 * dy2 - dy1 * dx2;

            if (denominator == 0)
                return e;

            double t = ((cp1.X - s.X) * dy2 + (s.Y - cp1.Y) * dx2) / denominator;
            return new Point(s.X + t * dx1, s.Y + t * dy1);
        }

    }
}
