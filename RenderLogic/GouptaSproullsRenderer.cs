using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace ComputerGraphics_Rasterization.RenderLogic
{
    internal class GouptaSproullsRenderer
    {
        internal static void DrawLine(CanvasRenderer renderer, int x0, int y0, int x1, int y1, Color color, float thickness)
        {
            // Determine if the line is steep (more vertical than horizontal)
            bool steep = Math.Abs(y1 - y0) > Math.Abs(x1 - x0);

            // If the line is steep, swap x and y coordinates
            if (steep)
            {
                Swap(ref x0, ref y0);
                Swap(ref x1, ref y1);
            }

            // Ensure the line is drawn from left to right
            if (x0 > x1)
            {
                Swap(ref x0, ref x1);
                Swap(ref y0, ref y1);
            }

            // Calculate delta values
            int dx = x1 - x0;
            int dy = y1 - y0;

            // Bresenham's algorithm variables
            int dE = 2 * dy;
            int dNE = 2 * (dy - dx);
            int d = 2 * dy - dx;
            int two_v_dx = 0; // numerator, v=0 for the first pixel

            // Precomputed constants for distance calculation
            float invDenom = 1.0f / (2.0f * (float)Math.Sqrt(dx * dx + dy * dy));
            float two_dx_invDenom = 2.0f * dx * invDenom;

            int x = x0, y = y0;

            // Plot the first pixel and its perpendicular neighbors
            IntensifyPixel(renderer, x, y, steep, color, thickness, 0);

            // Plot pixels perpendicular to the start point
            for (int i = 1; ; ++i)
            {
                if (!IntensifyPixel(renderer, x, y + i, steep, color, thickness, i * two_dx_invDenom))
                    break;
            }

            for (int i = 1; ; ++i)
            {
                if (!IntensifyPixel(renderer, x, y - i, steep, color, thickness, i * two_dx_invDenom))
                    break;
            }

            // Draw the line
            while (x < x1)
            {
                ++x;

                // Update Bresenham's algorithm
                if (d < 0) // move to E
                {
                    two_v_dx = d + dx;
                    d += dE;
                }
                else // move to NE
                {
                    two_v_dx = d - dx;
                    d += dNE;
                    ++y;
                }

                // Plot the current point
                IntensifyPixel(renderer, x, y, steep, color, thickness, two_v_dx * invDenom);

                // Plot pixels perpendicular to the current point above
                for (int i = 1; ; ++i)
                {
                    if (!IntensifyPixel(renderer, x, y + i, steep, color, thickness,
                                      i * two_dx_invDenom - two_v_dx * invDenom))
                        break;
                }

                // Plot pixels perpendicular to the current point below
                for (int i = 1; ; ++i)
                {
                    if (!IntensifyPixel(renderer, x, y - i, steep, color, thickness,
                                      i * two_dx_invDenom + two_v_dx * invDenom))
                        break;
                }
            }
        }

        private static bool IntensifyPixel(CanvasRenderer renderer, int x, int y, bool steep,
                                        Color color, float thickness, float distance)
        {
            float r = 0.5f; // Pixel radius - standard for most implementations
            float cov = Coverage(thickness, distance, r);

            // Return false if coverage is zero or negative
            if (cov <= 0)
                return false;

            // Create a color with the appropriate intensity
            Color blended = Lerp(Colors.White, color, cov);

            // Plot the pixel in the correct orientation
            if (steep)
                renderer.SetPixel(y, x, blended);
            else
                renderer.SetPixel(x, y, blended);

            return true;
        }

        private static float Coverage(float thickness, float distance, float pixelRadius)
        {
            // Calculate the perpendicular distance from the ideal line
            float dist = Math.Abs(distance);
            float w = thickness / 2.0f;  // Half width of the line

            // If we're outside the line's width plus the pixel radius, no coverage
            if (dist >= w + pixelRadius)
                return 0.0f;

            // If we're fully within the line's width minus the pixel radius, full coverage
            if (dist <= w - pixelRadius)
                return 1.0f;

            // We're in the transition zone - calculate partial coverage
            float r = pixelRadius;
            float d = dist - w;  // Distance from edge of line

            // If we're beyond the pixel radius, no coverage
            if (Math.Abs(d) > r)
                return 0.0f;

            // Calculate coverage using the circular pixel model
            return (float)((1.0 / Math.PI) * Math.Acos(d / r) - (d / (Math.PI * r * r)) * Math.Sqrt(r * r - d * d));
        }

        private static Color Lerp(Color a, Color b, float t)
        {
            // Linear interpolation between two colors
            return Color.FromArgb(
                255,
                (byte)(a.R + (b.R - a.R) * t),
                (byte)(a.G + (b.G - a.G) * t),
                (byte)(a.B + (b.B - a.B) * t)
            );
        }

        private static void Swap(ref int a, ref int b)
        {
            int temp = a;
            a = b;
            b = temp;
        }
    }
}