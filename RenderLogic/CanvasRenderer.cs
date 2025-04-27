using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ComputerGraphics_Rasterization.RenderLogic
{
    internal class CanvasRenderer
    {
        private WriteableBitmap writeableBitmap;
        private IntPtr pBackBuffer;
        private int stride;
        private bool isLocked = false;

        public CanvasRenderer(WriteableBitmap bitmap)
        {
            this.writeableBitmap = bitmap;
        }

        public void BeginDraw()
        {
            if (!isLocked)
            {
                writeableBitmap.Lock();
                pBackBuffer = writeableBitmap.BackBuffer;
                stride = writeableBitmap.BackBufferStride;
                isLocked = true;
            }
        }

        public void EndDraw()
        {
            if (isLocked)
            {
                writeableBitmap.AddDirtyRect(new Int32Rect(0, 0, writeableBitmap.PixelWidth, writeableBitmap.PixelHeight));
                writeableBitmap.Unlock();
                isLocked = false;
            }
        }

        public void SetPixel(int x, int y, Color color)
        {
            if (!isLocked)
                throw new InvalidOperationException("BeginDraw must be called before setting pixels.");

            if (x < 0 || x >= writeableBitmap.PixelWidth || y < 0 || y >= writeableBitmap.PixelHeight)
                return;

            unsafe
            {
                byte* pixel = (byte*)pBackBuffer + y * stride + x * 4;
                pixel[0] = color.B;
                pixel[1] = color.G;
                pixel[2] = color.R;
                pixel[3] = 255;
            }
        }

        public void ClearCanvas()
        {
            BeginDraw();
            unsafe
            {
                for (int y = 0; y < writeableBitmap.PixelHeight; y++)
                {
                    byte* row = (byte*)pBackBuffer + y * stride;
                    for (int x = 0; x < writeableBitmap.PixelWidth; x++)
                    {
                        row[x * 4 + 0] = 255;
                        row[x * 4 + 1] = 255;
                        row[x * 4 + 2] = 255;
                        row[x * 4 + 3] = 255;
                    }
                }
            }
            EndDraw();
        }
    }
}
