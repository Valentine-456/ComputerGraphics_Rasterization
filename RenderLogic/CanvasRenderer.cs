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

        public CanvasRenderer(WriteableBitmap bitmap)
        {
            this.writeableBitmap = bitmap;
        }

        public void SetPixel(int x, int y, Color color)
        {
            if (x < 0 || x >= writeableBitmap.PixelWidth || y < 0 || y >= writeableBitmap.PixelHeight)
                return; 

            writeableBitmap.Lock();
            unsafe
            {
                IntPtr pBackBuffer = writeableBitmap.BackBuffer;
                int stride = writeableBitmap.BackBufferStride;
                byte* pixel = (byte*)pBackBuffer + y * stride + x * 4;

                pixel[0] = color.B;
                pixel[1] = color.G;
                pixel[2] = color.R;
                pixel[3] = 255;
            }
            writeableBitmap.AddDirtyRect(new System.Windows.Int32Rect(x, y, 1, 1));
            writeableBitmap.Unlock();
        }

        public void ClearCanvas() 
        {
            writeableBitmap.Lock();
            unsafe
            {
                IntPtr pBackBuffer = writeableBitmap.BackBuffer;
                int stride = writeableBitmap.BackBufferStride;
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
            writeableBitmap.AddDirtyRect(new Int32Rect(0, 0, writeableBitmap.PixelWidth, writeableBitmap.PixelHeight));
            writeableBitmap.Unlock();
        }
    }
}
