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
            bool steep = Math.Abs(y1 - y0) > Math.Abs(x1 - x0);
            if (steep)
            {
                Swap(ref x0, ref y0);
                Swap(ref x1, ref y1);
            }

            if (x0 > x1)
            {
                Swap(ref x0, ref x1);
                Swap(ref y0, ref y1);
            }

            int dx = x1 - x0;
            int dy = y1 - y0;

            int sign = (dy < 0) ? -1 : 1;
            dy = Math.Abs(dy);

            int dE = 2 * dy;
            int dNE = 2 * (dy - dx);
            int d = 2 * dy - dx;
            int two_v_dx = 0;

            float invDenom = 1.0f / (2.0f * (float)Math.Sqrt(dx * dx + dy * dy));
            float two_dx_invDenom = 2.0f * dx * invDenom;

            int x = x0, y = y0;

            IntensifyPixel(renderer, x, y, steep, color, thickness, 0);
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


            while (x < x1)
            {
                ++x;
                if (d < 0)
                {
                    two_v_dx = d + dx;
                    d += dE;
                }
                else
                {
                    two_v_dx = d - dx;
                    d += dNE;
                    y += sign;
                }


                IntensifyPixel(renderer, x, y, steep, color, thickness, two_v_dx * invDenom);

                for (int i = 1; ; ++i)
                {
                    float distanceFactor = (sign > 0) ?
                        i * two_dx_invDenom - two_v_dx * invDenom :
                        i * two_dx_invDenom + two_v_dx * invDenom;

                    if (!IntensifyPixel(renderer, x, y + i, steep, color, thickness, distanceFactor))
                        break;
                }

                for (int i = 1; ; ++i)
                {
                    float distanceFactor = (sign > 0) ?
                        i * two_dx_invDenom + two_v_dx * invDenom :
                        i * two_dx_invDenom - two_v_dx * invDenom;

                    if (!IntensifyPixel(renderer, x, y - i, steep, color, thickness, distanceFactor))
                        break;
                }
            }
        }

        private static bool IntensifyPixel(CanvasRenderer renderer, int x, int y, bool steep, Color color, float thickness, float distance)
        {
            float r = 0.5f;
            float cov = Coverage(thickness, distance, r);

            if (cov <= 0)
                return false;

            Color blended = Lerp(Colors.White, color, cov);

            if (steep)
                renderer.SetPixel(y, x, blended);
            else
                renderer.SetPixel(x, y, blended);

            return true;
        }

        private static float Coverage(float thickness, float distance, float pixelRadius)
        {
            float dist = Math.Abs(distance);
            float w = thickness / 2.0f;

            if (dist >= w + pixelRadius)
                return 0.0f;

            if (dist <= w - pixelRadius)
                return 1.0f;

            float r = pixelRadius;
            float d = dist - w;

            return (float)((1.0 / Math.PI) * Math.Acos(d / r) - (d / (Math.PI * r * r)) * Math.Sqrt(r * r - d * d));
        }

        private static Color Lerp(Color a, Color b, float t)
        {
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