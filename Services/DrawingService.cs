using ComputerGraphics_Rasterization.Shapes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows;

namespace ComputerGraphics_Rasterization.Services
{
    internal class DrawingService
    {
        private DrawingMode currentMode = DrawingMode.None;
        private IShape currentShape = null;
        private CanvasService canvasService;

        public DrawingService(CanvasService canvasService)
        {
            this.canvasService = canvasService;
        }

        public void SetDrawingMode(DrawingMode mode)
        {
            currentMode = mode;
            currentShape = null;
        }

        public DrawingMode GetDrawingMode()
        {
            return currentMode;
        }

        public void ClearCurrentShape()
        {
            currentShape = null;
        }

        public void HandleLeftClick(Point click, Color selectedColor, int thickness)
        {
            if (currentShape == null)
            {
                switch (currentMode)
                {
                    case DrawingMode.Line:
                        currentShape = new LineShape((int)click.X, (int)click.Y, (int)click.X, (int)click.Y, selectedColor, thickness);
                        break;
                    case DrawingMode.Circle:
                        currentShape = new CircleShape((int)click.X, (int)click.Y, 0, selectedColor);
                        break;
                }
            }
            else
            {
                switch (currentShape)
                {
                    case LineShape line:
                        line.X1 = (int)click.X;
                        line.Y1 = (int)click.Y;
                        break;
                    case CircleShape circle:
                        circle.Radius = (int)Math.Sqrt(Math.Pow(click.X - circle.X, 2) + Math.Pow(click.Y - circle.Y, 2));
                        break;
                }

                canvasService.AddShape(currentShape);
                currentShape = null;
            }
        }

        public IShape GetCurrentShape() => currentShape;
    }
}
